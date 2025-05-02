using System;
using System.Data.SQLite;
using DataAccessLayer.Exceptions;

namespace DataAccessLayer
{
    public class AccountCreationDAL
    {
        public static string CreateAccountAsPending(string firstName, string lastName, string codeNIN,
                                                  string username, string password, string matricule, string email)
        {
            using (SQLiteConnection connection = new SQLiteConnection(DataAccessSettings.connectionString))
            {
                string result = "Account created successfully";

                try
                {
                    connection.Open();

                    string query = "INSERT INTO Users (isAdmin, username, passwordHashed, matricule, signinDate, firstName, lastName, email, code_NIN, status) " +
                        "VALUES (0, @username, @passwordHashed, @matricule, @signInDate, @firstName, @lastName, @email, @codeNIN, 'pending')";

                    using (SQLiteCommand command = new SQLiteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@firstName", firstName);
                        command.Parameters.AddWithValue("@lastName", lastName);
                        command.Parameters.AddWithValue("@email", string.IsNullOrEmpty(email) ? (object)DBNull.Value : email);
                        command.Parameters.AddWithValue("@codeNIN", codeNIN);
                        command.Parameters.AddWithValue("@username", username);
                        command.Parameters.AddWithValue("@passwordHashed", password);
                        command.Parameters.AddWithValue("@matricule", string.IsNullOrEmpty(matricule) ? (object)DBNull.Value : matricule);
                        // to store date in format yyyy-mm-dd hh-mm-ss
                        var now = DateTime.Now;
                        var trimmed = new DateTime(now.Year, now.Month, now.Day, now.Hour, now.Minute, now.Second);
                        command.Parameters.AddWithValue("@signInDate", trimmed);


                        command.ExecuteNonQuery();
                    }
                }
                catch (SQLiteException ex)
                {
                    // Handle unique constraint violations
                    if (ex.ErrorCode == (int)SQLiteErrorCode.Constraint)
                    {
                        string errorMessage = ex.Message.ToLower();

                        if (errorMessage.Contains("unique") || errorMessage.Contains("constraint"))
                        {
                            // Determine which field caused the unique constraint violation
                            if (errorMessage.Contains("username"))
                            {
                                throw new DuplicateUsernameException(ex);
                            }
                            else if (errorMessage.Contains("email"))
                            {
                                throw new DuplicateEmailException(ex);
                            }
                            else if (errorMessage.Contains("code_nin"))
                            {
                                throw new DuplicateNINException(ex);
                            }
                            else
                            {
                                // Generic unique constraint violation
                                throw new UniqueConstraintViolationException("unknown field", ex);
                            }
                        }
                        else if (errorMessage.Contains("foreign key"))
                        {
                            throw new ForeignKeyConstraintViolationException("unknown reference", ex);
                        }
                        else if (errorMessage.Contains("not null"))
                        {
                            throw new NotNullConstraintViolationException("unknown field", ex);
                        }
                        else
                        {
                            // Other constraint violations
                            throw new ConstraintViolationException("A database constraint was violated",
                                                                 "unknown", "unknown", ex);
                        }
                    }
                    else if (ex.ErrorCode == (int)SQLiteErrorCode.IoErr ||
                             ex.ErrorCode == (int)SQLiteErrorCode.Corrupt ||
                             ex.ErrorCode == (int)SQLiteErrorCode.NotADb)
                    {
                        throw new DatabaseConnectionException("Database file error", ex);
                    }
                    else
                    {
                        // Handle other SQLite exceptions
                        throw new QueryExecutionException("Error executing database query", ex);
                    }
                }
                catch (Exception ex)
                {
                    // Handle other general exceptions
                    throw new DataAccessException("An unexpected error occurred while creating the account", ex);
                }
                finally
                {
                    connection.Close();
                }

                return result;
            }
        }
    }
}