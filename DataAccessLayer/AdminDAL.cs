using DataAccessLayer.Exceptions;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public class AdminDAL : UserDAL
    {

        // --------------- Admin-User Related Functions ----------------
        public static UserDAL GetUser(string username)
        {
            // ------------ connection to db settings 
            SQLiteConnection connection = new SQLiteConnection(DataAccessSettings.connectionString);

            // the query to get the user that matches the username from the database
            string query = "SELECT * FROM Users WHERE username = @username";

            SQLiteCommand command = new SQLiteCommand(query, connection);

            command.Parameters.AddWithValue("@username", username);
            // ------------ connection to db settings

            UserDAL user = new UserDAL();
            user.id = 0; // it will stay 0 if the user is not found
            try
            {
                connection.Open();

                // this code should be executed every time we open the connection to pay attention to the foreign key constraints
                using (var commandPragma = new SQLiteCommand("PRAGMA foreign_keys = ON", connection))
                {
                    commandPragma.ExecuteNonQuery();
                }

                // the reader will read the data from the database and execute the query
                SQLiteDataReader reader = command.ExecuteReader();

                // if the user is found the while loop will be executed
                // and the reader will read the data and store it in the user struct
                while (reader.Read())
                {
                    user.id = Convert.ToInt32(reader["user_id"]);
                    user.username = (string)reader["username"];
                    user.password = (string)reader["passwordHashed"];
                    user.matricule = reader["matricule"] != DBNull.Value ? (string)reader["matricule"] : string.Empty;
                    user.email = reader["email"] != DBNull.Value ? (string)reader["email"] : string.Empty;
                    user.isAdmin = (bool)reader["isAdmin"];
                    user.lastName = (string)reader["lastName"];
                    user.firstName = (string)reader["firstName"];
                    user.signInDate = Convert.ToDateTime(reader["signInDate"]);
                    user.codeNiN = (string)reader["code_NIN"];
                    user.status = (string)reader["status"];
                }

                reader.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                connection.Close();
            }


            return user; // returning the user struct to the BLL
        }

        // overloaded method to get the user by id
        public static UserDAL GetUser(int id)
        {
            // ------------ connection to db settings 
            SQLiteConnection connection = new SQLiteConnection(DataAccessSettings.connectionString);

            // the query to get the user that matches the username from the database
            string query = "SELECT * FROM Users WHERE user_id = @id";

            SQLiteCommand command = new SQLiteCommand(query, connection);

            command.Parameters.AddWithValue("@user_id", id);
            // ------------ connection to db settings

            UserDAL user = new UserDAL();
            user.id = 0; // it will stay 0 if the user is not found
            try
            {
                connection.Open();

                // this code should be executed every time we open the connection to pay attention to the foreign key constraints
                using (var commandPragma = new SQLiteCommand("PRAGMA foreign_keys = ON", connection))
                {
                    commandPragma.ExecuteNonQuery();
                }

                // the reader will read the data from the database and execute the query
                SQLiteDataReader reader = command.ExecuteReader();

                // if the user is found the while loop will be executed
                // and the reader will read the data and store it in the user struct
                while (reader.Read())
                {
                    user.id = Convert.ToInt32(reader["user_id"]);
                    user.username = (string)reader["username"];
                    user.password = (string)reader["passwordHashed"];
                    user.matricule = (string)reader["matricule"];
                    user.isAdmin = (bool)reader["isAdmin"];
                    user.lastName = (string)reader["lastName"];
                    user.firstName = (string)reader["firstName"];
                    user.signInDate = Convert.ToDateTime(reader["signInDate"]);
                    user.codeNiN = (string)reader["code_NIN"];
                    user.status = (string)reader["status"];
                }

                reader.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                connection.Close();
            }


            return user; // returning the user struct to the BLL
        }

        // i can't make the getUser(NIN) overloaded because it will be simillar to the getUser(id) method
        public static UserDAL GetUserByNIN(string codeNin)
        {
            // ------------ connection to db settings 
            SQLiteConnection connection = new SQLiteConnection(DataAccessSettings.connectionString);

            // the query to get the user that matches the username from the database
            string query = "SELECT * FROM Users WHERE code_NIN = @codeNin";

            SQLiteCommand command = new SQLiteCommand(query, connection);

            command.Parameters.AddWithValue("@codeNin", codeNin);
            // ------------ connection to db settings

            UserDAL user = new UserDAL();
            user.id = 0; // it will stay 0 if the user is not found
            try
            {
                connection.Open();

                // this code should be executed every time we open the connection to pay attention to the foreign key constraints
                using (var commandPragma = new SQLiteCommand("PRAGMA foreign_keys = ON", connection))
                {
                    commandPragma.ExecuteNonQuery();
                }

                // the reader will read the data from the database and execute the query
                SQLiteDataReader reader = command.ExecuteReader();

                // if the user is found the while loop will be executed
                // and the reader will read the data and store it in the user struct
                while (reader.Read())
                {
                    user.id = Convert.ToInt32(reader["user_id"]);
                    user.username = (string)reader["username"];
                    user.password = (string)reader["passwordHashed"];
                    user.matricule = (string)reader["matricule"];
                    user.isAdmin = (bool)reader["isAdmin"];
                    user.lastName = (string)reader["lastName"];
                    user.firstName = (string)reader["firstName"];
                    user.signInDate = Convert.ToDateTime(reader["signInDate"]);
                    user.codeNiN = (string)reader["code_NIN"];
                    user.status = (string)reader["status"];
                }

                reader.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                connection.Close();
            }


            return user; // returning the user struct to the BLL
        }

        // function to search for a user by matricule and return it as a struct
        public static UserDAL GetUserByMatricule(string matricule)
        {
            // ------------ connection to db settings 
            SQLiteConnection connection = new SQLiteConnection(DataAccessSettings.connectionString);
            // the query to get the user that matches the matricule from the database
            string query = "SELECT * FROM Users WHERE matricule = @matricule";
            SQLiteCommand command = new SQLiteCommand(query, connection);
            command.Parameters.AddWithValue("@matricule", matricule);
            // ------------ connection to db settings

            UserDAL user = new UserDAL();
            user.id = 0; // it will stay 0 if the user is not found

            try
            {
                connection.Open();

                // this code should be executed every time we open the connection to pay attention to the foreign key constraints
                using (var commandPragma = new SQLiteCommand("PRAGMA foreign_keys = ON", connection))
                {
                    commandPragma.ExecuteNonQuery();
                }

                // the reader will read the data from the database and execute the query
                SQLiteDataReader reader = command.ExecuteReader();

                // if the user is found the while loop will be executed
                // and the reader will read the data and store it in the user struct
                while (reader.Read())
                {
                    user.id = Convert.ToInt32(reader["user_id"]);
                    user.username = (string)reader["username"];
                    user.password = (string)reader["passwordHashed"];
                    user.matricule = (string)reader["matricule"];
                    user.isAdmin = (bool)reader["isAdmin"];
                    user.lastName = (string)reader["lastName"];
                    user.firstName = (string)reader["firstName"];
                    user.signInDate = Convert.ToDateTime(reader["signInDate"]);
                    user.codeNiN = (string)reader["code_NIN"];
                    user.status = (string)reader["status"];
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                connection.Close();
            }
            return user; // returning the user struct to the BLL
        }

        // function to get a user by his first name and last name
        public static List<UserDAL> GetUsersByFullName(string firstName, string lastName)
        {
            // ------------ connection to db settings 
            SQLiteConnection connection = new SQLiteConnection(DataAccessSettings.connectionString);
            // the query to get the user that matches the first name and last name from the database
            string query = "SELECT * FROM Users WHERE firstName = @firstName AND lastName = @lastName";
            SQLiteCommand command = new SQLiteCommand(query, connection);
            command.Parameters.AddWithValue("@firstName", firstName);
            command.Parameters.AddWithValue("@lastName", lastName);
            // ------------ connection to db settings

            UserDAL user = new UserDAL();
            List<UserDAL> users = new List<UserDAL>();
            try
            {
                connection.Open();

                // this code should be executed every time we open the connection to pay attention to the foreign key constraints
                using (var commandPragma = new SQLiteCommand("PRAGMA foreign_keys = ON", connection))
                {
                    commandPragma.ExecuteNonQuery();
                }

                // the reader will read the data from the database and execute the query
                SQLiteDataReader reader = command.ExecuteReader();

                // if the user is found the while loop will be executed
                // and the reader will read the data and store it in the user struct
                while (reader.Read())
                {
                    user.id = Convert.ToInt32(reader["user_id"]);
                    user.username = (string)reader["username"];
                    user.password = (string)reader["passwordHashed"];
                    user.matricule = (string)reader["matricule"];
                    user.isAdmin = (bool)reader["isAdmin"];
                    user.lastName = (string)reader["lastName"];
                    user.firstName = (string)reader["firstName"];
                    user.signInDate = Convert.ToDateTime(reader["signInDate"]);
                    user.codeNiN = (string)reader["code_NIN"];
                    user.status = (string)reader["status"];

                    users.Add(user); // add users one by one to the Users List
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                connection.Close();
            }
            return users; // returning the users list to the BLL
        }

        // function to get a user by his first name
        public static List<UserDAL> GetUsersByFirstName(string firstName)
        {
            // ------------ connection to db settings 
            SQLiteConnection connection = new SQLiteConnection(DataAccessSettings.connectionString);
            // the query to get the user that matches the first name from the database
            string query = "SELECT * FROM Users WHERE firstName = @firstName";
            SQLiteCommand command = new SQLiteCommand(query, connection);
            command.Parameters.AddWithValue("@firstName", firstName);
            // ------------ connection to db settings

            UserDAL user = new UserDAL();
            List<UserDAL> users = new List<UserDAL>();
            try
            {
                connection.Open();

                // this code should be executed every time we open the connection to pay attention to the foreign key constraints
                using (var commandPragma = new SQLiteCommand("PRAGMA foreign_keys = ON", connection))
                {
                    commandPragma.ExecuteNonQuery();
                }

                // the reader will read the data from the database and execute the query
                SQLiteDataReader reader = command.ExecuteReader();
                // if the user is found the while loop will be executed
                // and the reader will read the data and store it in the user struct
                while (reader.Read())
                {
                    user.id = Convert.ToInt32(reader["user_id"]);
                    user.username = (string)reader["username"];
                    user.password = (string)reader["passwordHashed"];
                    user.matricule = (string)reader["matricule"];
                    user.isAdmin = (bool)reader["isAdmin"];
                    user.lastName = (string)reader["lastName"];
                    user.email = (string)reader["email"];
                    user.firstName = (string)reader["firstName"];
                    user.signInDate = Convert.ToDateTime(reader["signInDate"]);
                    user.codeNiN = (string)reader["code_NIN"];
                    user.status = (string)reader["status"];

                    users.Add(user); // add users one by one to the Users List
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                connection.Close();
            }
            return users; // returning the users list to the BLL
        }

        // function to get a user by his last name
        public static List<UserDAL> GetUsersByLastName(string lastName)
        {
            // ------------ connection to db settings 
            SQLiteConnection connection = new SQLiteConnection(DataAccessSettings.connectionString);
            // the query to get the user that matches the last name from the database
            string query = "SELECT * FROM Users WHERE lastName = @lastName";
            SQLiteCommand command = new SQLiteCommand(query, connection);
            command.Parameters.AddWithValue("@lastName", lastName);
            // ------------ connection to db settings

            UserDAL user = new UserDAL();
            List<UserDAL> users = new List<UserDAL>();

            try
            {
                connection.Open();

                // this code should be executed every time we open the connection to pay attention to the foreign key constraints
                using (var commandPragma = new SQLiteCommand("PRAGMA foreign_keys = ON", connection))
                {
                    commandPragma.ExecuteNonQuery();
                }

                // the reader will read the data from the database and execute the query

                SQLiteDataReader reader = command.ExecuteReader();

                // if the user is found the while loop will be executed
                // and the reader will read the data and store it in the user struct
                while (reader.Read())
                {
                    user.id = Convert.ToInt32(reader["user_id"]);
                    user.username = (string)reader["username"];
                    user.password = (string)reader["passwordHashed"];
                    user.matricule = (string)reader["matricule"];
                    user.isAdmin = (bool)reader["isAdmin"];
                    user.lastName = (string)reader["lastName"];
                    user.firstName = (string)reader["firstName"];
                    user.signInDate = Convert.ToDateTime(reader["signInDate"]);
                    user.codeNiN = (string)reader["code_NIN"];
                    user.status = (string)reader["status"];

                    users.Add(user); // add users one by one to the Users List
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                connection.Close();
            }
            return users;
        }

        // this method will return all the users in the database in a list of user struct
        public static List<UserDAL> GetAllUsers()
        {
            // hadi l method machi ay user yaccedilha lazem ghir les admins
            // tsma lazem tkoun condition (isAdmin = true) f BLL 9bal ma t3ayet lhadi l method


            // ------------ connection to db settings 
            SQLiteConnection connection = new SQLiteConnection(DataAccessSettings.connectionString);

            // the query to get the user that matches the username from the database
            string query = "SELECT * FROM Users";

            SQLiteCommand command = new SQLiteCommand(query, connection);
            // ------------ connection to db settings


            List<UserDAL> users = new List<UserDAL>();
            UserDAL user = new UserDAL();

            try
            {
                connection.Open();

                // this code should be executed every time we open the connection to pay attention to the foreign key constraints
                using (var commandPragma = new SQLiteCommand("PRAGMA foreign_keys = ON", connection))
                {
                    command.ExecuteNonQuery();
                }

                // the reader will read the data from the database and execute the query
                SQLiteDataReader reader = command.ExecuteReader();

                // if at least one user is found the while loop will be executed
                // reader will read the data and store it in the user struct and then in the users list
                while (reader.Read())
                {
                    user.id = Convert.ToInt32(reader["user_id"]);
                    user.username = (string)reader["username"];
                    user.password = (string)reader["passwordHashed"];
                    user.matricule = (string)reader["matricule"];
                    user.isAdmin = (bool)reader["isAdmin"];
                    user.lastName = (string)reader["lastName"];
                    user.firstName = (string)reader["firstName"];
                    user.signInDate = Convert.ToDateTime(reader["signInDate"]);
                    user.codeNiN = (string)reader["code_NIN"];
                    user.status = (string)reader["status"];


                    users.Add(user); // add users one by one to the Users List
                }

                reader.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                connection.Close();
            }


            return users; // returning the users list to the BLL
        }

        // this function will return all the users that shares the same status in the database
        public static List<UserDAL> GetAllUsers(string status)
        {
            SQLiteConnection connection = new SQLiteConnection(DataAccessSettings.connectionString);
            string query = "SELECT * FROM Users WHERE status = @status";
            SQLiteCommand command = new SQLiteCommand(query, connection);
            command.Parameters.AddWithValue("@status", status);

            List<UserDAL> users = new List<UserDAL>();

            try
            {
                connection.Open();

                using (var commandPragma = new SQLiteCommand("PRAGMA foreign_keys = ON", connection))
                {
                    commandPragma.ExecuteNonQuery();
                }

                SQLiteDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    UserDAL user = new UserDAL();

                    user.id = reader["user_id"] == DBNull.Value ? 0 : Convert.ToInt32(reader["user_id"]);
                    user.username = reader["username"] == DBNull.Value ? null : reader["username"].ToString();
                    user.password = reader["passwordHashed"] == DBNull.Value ? null : reader["passwordHashed"].ToString();
                    user.matricule = reader["matricule"] == DBNull.Value ? null : reader["matricule"].ToString();
                    user.isAdmin = reader["isAdmin"] == DBNull.Value ? false : Convert.ToBoolean(reader["isAdmin"]);
                    user.lastName = reader["lastName"] == DBNull.Value ? null : reader["lastName"].ToString();
                    user.firstName = reader["firstName"] == DBNull.Value ? null : reader["firstName"].ToString();
                    user.signInDate = reader["signInDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(reader["signInDate"]);
                    user.codeNiN = reader["code_NIN"] == DBNull.Value ? null : reader["code_NIN"].ToString();
                    user.status = reader["status"] == DBNull.Value ? null : reader["status"].ToString();
                    user.email = reader["email"] == DBNull.Value ? null : reader["email"].ToString();

                    users.Add(user);
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error fetching users: " + ex.Message);
            }
            finally
            {
                connection.Close();
            }

            return users;
        }


        // this function will return all the admins in the database
        public static List<AdminDAL> GetAllAdmins()
        {
            // ------------ connection to db settings 
            SQLiteConnection connection = new SQLiteConnection(DataAccessSettings.connectionString);
            // the query to get the admins from the database
            string query = "SELECT * FROM Users WHERE isAdmin = 1";
            SQLiteCommand command = new SQLiteCommand(query, connection);
            // ------------ connection to db settings

            List<AdminDAL> users = new List<AdminDAL>();
            AdminDAL user = new AdminDAL();
            try
            {
                connection.Open();

                // this code should be executed every time we open the connection to pay attention to the foreign key constraints
                using (var commandPragma = new SQLiteCommand("PRAGMA foreign_keys = ON", connection))
                {
                    commandPragma.ExecuteNonQuery();
                }

                // the reader will read the data from the database and execute the query
                SQLiteDataReader reader = command.ExecuteReader();

                // if at least one user is found the while loop will be executed
                // reader will read the data and store it in the user struct and then in the users list
                while (reader.Read())
                {
                    user.id = Convert.ToInt32(reader["user_id"]);
                    user.username = (string)reader["username"];
                    user.password = (string)reader["passwordHashed"];
                    user.matricule = (string)reader["matricule"];
                    user.isAdmin = (bool)reader["isAdmin"];
                    user.lastName = (string)reader["lastName"];
                    user.firstName = (string)reader["firstName"];
                    user.signInDate = Convert.ToDateTime(reader["signInDate"]);
                    user.codeNiN = (string)reader["code_NIN"];
                    user.status = (string)reader["status"];
                    users.Add(user); // add users one by one to the Users List
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                connection.Close();
            }
            return users;
        }

        // this function return true if the is set to admin seccussfully , false otherwise
        public static bool setUserAsAdmin(string username)
        {
            bool result = false;
            string query = "UPDATE Users SET isAdmin = 1 WHERE username = @username";

            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(DataAccessSettings.connectionString))
                {
                    connection.Open();

                    // Enable foreign key constraints
                    using (var commandPragma = new SQLiteCommand("PRAGMA foreign_keys = ON", connection))
                    {
                        commandPragma.ExecuteNonQuery();
                    }

                    // Execute the update query
                    using (SQLiteCommand command = new SQLiteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@username", username);
                        int rowsAffected = command.ExecuteNonQuery();
                        result = (rowsAffected > 0);
                    }
                } // Connection automatically closed and disposed here
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                // Consider logging the exception or throwing it up to be handled by caller
            }

            return result;
        }

        // function to delete the user with id from the database
        public static bool deleteUser(string username)
        {

            // ------------ connection to db settings 
            SQLiteConnection connection = new SQLiteConnection(DataAccessSettings.connectionString);

            // the query to delete the user with id sent to the method (user_id)
            string query = "DELETE FROM Users WHERE username = @username";

            SQLiteCommand command = new SQLiteCommand(query, connection);

            command.Parameters.AddWithValue("@username", username);
            // ------------ connection to db settings

            // result is output of the function
            bool result = false;
            try
            {
                connection.Open();

                // this code should be executed every time we open the connection to pay attention to the foreign key constraints
                using (var commandPragma = new SQLiteCommand("PRAGMA foreign_keys = ON", connection))
                {
                    commandPragma.ExecuteNonQuery();
                }

                // delete the User and return the number of rows in db affected
                int rowsAffected = command.ExecuteNonQuery();

                // if rows affected > 0 then the user is deleted successfully
                result = (rowsAffected > 0);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                connection.Close();
            }

            return result; // returning the result to the BLL

        }

        // function to accept a user when he create a new account
        public static string acceptUserAccount(string username)
        {
            SQLiteConnection connection = new SQLiteConnection(DataAccessSettings.connectionString);

            using (SQLiteCommand command = new SQLiteCommand("UPDATE Users SET status = 'accepted' WHERE username = @username", connection))
            {
                command.Parameters.AddWithValue("@username", username);
                try
                {
                    connection.Open();

                    // Enforce foreign key constraints
                    using (var commandPragma = new SQLiteCommand("PRAGMA foreign_keys = ON", connection))
                    {
                        commandPragma.ExecuteNonQuery();
                    }

                    int rowsAffected = command.ExecuteNonQuery();
                    return rowsAffected > 0 ? $"User Account with username : {username} is accepted successfully!" : $"Account with username : {username} not found or already accepted.";
                }
                catch (SQLiteException ex)
                {
                    switch (ex.ErrorCode)
                    {
                        case (int)SQLiteErrorCode.Constraint:
                        case (int)SQLiteErrorCode.Constraint_ForeignKey:
                            return "Related data is missing. Please verify account dependencies.";

                        case (int)SQLiteErrorCode.Constraint_Unique:
                            return "Username is already accepted or causes a uniqueness conflict.";

                        case (int)SQLiteErrorCode.Constraint_NotNull:
                            return "Missing required information. Please fill in all fields.";

                        default:
                            return "A database error occurred while accepting the account.";
                    }
                }
                catch (InvalidOperationException)
                {
                    return "Invalid operation on the database. Please contact the administrator.";
                }
                catch (Exception)
                {
                    return "An unexpected error occurred. Please try again later.";
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        public static string refuseUserAccount(string username)
        {
            SQLiteConnection connection = new SQLiteConnection(DataAccessSettings.connectionString);

            using (SQLiteCommand command = new SQLiteCommand("UPDATE Users SET status = 'rejected' WHERE username = @username", connection))
            {
                command.Parameters.AddWithValue("@username", username);
                try
                {
                    connection.Open();

                    // Enforce foreign key constraints
                    using (var commandPragma = new SQLiteCommand("PRAGMA foreign_keys = ON", connection))
                    {
                        commandPragma.ExecuteNonQuery();
                    }

                    int rowsAffected = command.ExecuteNonQuery();
                    return rowsAffected > 0 ? $"User Account with username : {username} is rejected successfully!" : $"Account with username : {username} not found or already rejected.";
                }
                catch (SQLiteException ex)
                {
                    switch (ex.ErrorCode)
                    {
                        case (int)SQLiteErrorCode.Constraint:
                        case (int)SQLiteErrorCode.Constraint_ForeignKey:
                            return "Related data is missing. Please verify account dependencies.";

                        case (int)SQLiteErrorCode.Constraint_Unique:
                            return "Username is already accepted or causes a uniqueness conflict.";

                        case (int)SQLiteErrorCode.Constraint_NotNull:
                            return "Missing required information. Please fill in all fields.";

                        default:
                            return "A database error occurred while accepting the account.";
                    }
                }
                catch (InvalidOperationException)
                {
                    return "Invalid operation on the database. Please contact the administrator.";
                }
                catch (Exception)
                {
                    return "An unexpected error occurred. Please try again later.";
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        // function to accept all users in pending status
        public static void acceptAllPendingUsers()
        {
            SQLiteConnection connection = new SQLiteConnection(DataAccessSettings.connectionString);
            using (SQLiteCommand command = new SQLiteCommand("UPDATE Users SET status = 'Accepted' WHERE status = 'Pending'", connection))
            {
                try
                {
                    connection.Open();

                    // this code should be executed every time we open the connection to pay attention to the foreign key constraints
                    using (var commandPragma = new SQLiteCommand("PRAGMA foreign_keys = ON", connection))
                    {
                        commandPragma.ExecuteNonQuery();
                    }

                    command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    // Handle Excp
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        // ---------------------------------------------------------------
        private static int addProf(ProfessorDAL pr)
        {
            if(pr == null)
                return 0;
            SQLiteConnection connection = new SQLiteConnection(DataAccessSettings.connectionString);
            connection.Open();

            // this code should be executed every time we open the connection to pay attention to the foreign key constraints
            using (var commandPragma = new SQLiteCommand("PRAGMA foreign_keys = ON", connection))
            {
                commandPragma.ExecuteNonQuery();
            }

            // Check if professor exists and get ID
            int profId;
            string checkProfQuery = "SELECT prof_id FROM Profs WHERE LOWER(nom) = LOWER(@nom) AND LOWER(prenom) = LOWER(@prenom)";
            using (SQLiteCommand checkCommand = new SQLiteCommand(checkProfQuery, connection))
            {
                checkCommand.Parameters.AddWithValue("@nom", pr.nom);
                checkCommand.Parameters.AddWithValue("@prenom", pr.prenom);

                var resultCheckProf = checkCommand.ExecuteScalar();

                if (resultCheckProf != null) // if the professor exists we keep his id to relate him to the these 
                    profId = Convert.ToInt32(resultCheckProf);
                else
                {
                    // Insert professor and get ID
                    string insertProfQuery = "INSERT INTO Profs (nom, prenom) VALUES (@nom, @prenom);" +
                        " SELECT last_insert_rowid();"; // this line is to get the prof_id of the inserted prof to establish reltion between the prof and the these

                    using (SQLiteCommand insertProfCommand = new SQLiteCommand(insertProfQuery, connection))
                    {
                        insertProfCommand.Parameters.AddWithValue("@nom", pr.nom);
                        insertProfCommand.Parameters.AddWithValue("@prenom", pr.prenom);
                        var insertProfResult = insertProfCommand.ExecuteScalar();

                        if (insertProfResult != null)
                            profId = Convert.ToInt32(insertProfResult);
                        else
                        {
                            // Handle error: failed to insert professor
                            profId = -1; // this tells the caller that the prof is not inserted correctly
                        }

                    }

                }
            }
            connection.Close();
            return profId;
        }
        private static void connectAuthorsWithTheseInDB(int authorId, int theseId)
        {
            SQLiteConnection connection = new SQLiteConnection(DataAccessSettings.connectionString);
            connection.Open();

            // this code should be executed every time we open the connection to pay attention to the foreign key constraints
            using (var commandPragma = new SQLiteCommand("PRAGMA foreign_keys = ON", connection))
            {
                commandPragma.ExecuteNonQuery();
            }

            using (SQLiteCommand insertAuthorToConjuctionTableCommand = new SQLiteCommand("INSERT INTO author_Thesis (auteur_id, these_id) VALUES (@authorId, @theseId);", connection))
            {
                insertAuthorToConjuctionTableCommand.Parameters.AddWithValue("@authorId", authorId);
                insertAuthorToConjuctionTableCommand.Parameters.AddWithValue("@theseId", theseId);

                // run the command:
                int insertAuthorToConjuctionTableResult = insertAuthorToConjuctionTableCommand.ExecuteNonQuery();

                if (insertAuthorToConjuctionTableResult <= 0)
                {
                    // Handle error: failed to insert author into conjunction table
                    authorId = -1; // this tells the caller that the author is not inserted correctly
                }
            }
        }
        private static void addAuthors(List<AuthorDAL> auth, int theseId)
        {
            SQLiteConnection connection = new SQLiteConnection(DataAccessSettings.connectionString);
            connection.Open();

            // this code should be executed every time we open the connection to pay attention to the foreign key constraints
            using (var commandPragma = new SQLiteCommand("PRAGMA foreign_keys = ON", connection))
            {
                commandPragma.ExecuteNonQuery();
            }

            List<int> AuthorsId = new List<int>();
            foreach (AuthorDAL author in auth)
            {
                int authorId;

                string checkAuthorQuery = "SELECT auteur_id FROM Auteurs WHERE LOWER(nom) = LOWER(@nom) AND LOWER(prenom) = LOWER(@prenom)";
                using (SQLiteCommand checkCommand = new SQLiteCommand(checkAuthorQuery, connection))
                {
                    checkCommand.Parameters.AddWithValue("@nom", author.LastName);
                    checkCommand.Parameters.AddWithValue("@prenom", author.FirstName);

                    var resultCheckAuthor = checkCommand.ExecuteScalar();

                    if (resultCheckAuthor != null) // if the auhtor exists we keep his id to relate him to the these 
                    {
                        authorId = Convert.ToInt32(resultCheckAuthor);

                        // insert authorid into the conjuction table to relate the these with the author
                        connectAuthorsWithTheseInDB(authorId, theseId);

                    }
                    else // if we get here that means that the author doesnt exist in DB previously
                    {
                        // Insert author and get ID
                        string insertAuthorQuery = "INSERT INTO Auteurs (nom, prenom, mail) VALUES (@nom, @prenom, @mail);" +
                            " SELECT last_insert_rowid();"; // this line is to get the auteur_id of the inserted author to establish reltion between the author and the these

                        using (SQLiteCommand insertAuthorCommand = new SQLiteCommand(insertAuthorQuery, connection))
                        {
                            insertAuthorCommand.Parameters.AddWithValue("@nom", author.LastName);
                            insertAuthorCommand.Parameters.AddWithValue("@prenom", author.FirstName);
                            insertAuthorCommand.Parameters.AddWithValue("@mail", author.Email);

                            var insertAuthorResult = insertAuthorCommand.ExecuteScalar();

                            if (insertAuthorResult != null)
                            {
                                authorId = Convert.ToInt32(insertAuthorResult);
                                AuthorsId.Add(authorId);

                            }
                                
                            else
                            {
                                // Handle error: failed to insert author
                                authorId = -1; // this tells the caller that the author is not inserted correctly
                            }

                        }
                        // after adding the authors we have to connect them to the these in the junction table
                        foreach (int id in AuthorsId)
                        {
                            connectAuthorsWithTheseInDB(id, theseId);
                        }
                    }

                    AuthorsId.Add(authorId); // we are going to use this when making relation between these and authors
                }
            }

            connection.Close();
        }

        private static void AddKeywordsToThesis(int thesisId, string[] keywords)
        {
            using (var connection = new SQLiteConnection(DataAccessSettings.connectionString))
            {
                connection.Open();

                foreach (var keyword in keywords)
                {
                    EnsureKeywordExists(connection, keyword);
                    EnsureKeywordThesisLink(connection, thesisId, keyword);
                }
            }

        }

        private static void EnsureKeywordExists(SQLiteConnection conn, string keyword)
        {
            using (var checkCmd = new SQLiteCommand("SELECT 1 FROM KeyWords WHERE keyWord = @keyword", conn))
            {
                checkCmd.Parameters.AddWithValue("@keyword", keyword);

                var exists = checkCmd.ExecuteScalar();
                if (exists != null) return;

                using (var insertCmd = new SQLiteCommand("INSERT INTO KeyWords (keyword) VALUES (@keyword)", conn))
                {
                    insertCmd.Parameters.AddWithValue("@keyword", keyword);
                    insertCmd.ExecuteNonQuery();
                }

            }

        }

        private static void EnsureKeywordThesisLink(SQLiteConnection conn, int thesisId, string keyword)
        {
            using (var checkCmd = new SQLiteCommand(@"
            SELECT 1 FROM KeyWord_These 
            WHERE these_id = @thesisId AND keyWord = @keyword", conn))
            {
                checkCmd.Parameters.AddWithValue("@thesisId", thesisId);
                checkCmd.Parameters.AddWithValue("@keyword", keyword);

                var exists = checkCmd.ExecuteScalar();
                if (exists != null) return;

                using (var insertCmd = new SQLiteCommand(@"
            INSERT INTO KeyWord_These (these_id, keyWord)
            VALUES (@thesisId, @keyword)", conn))
                {
                    insertCmd.Parameters.AddWithValue("@thesisId", thesisId);
                    insertCmd.Parameters.AddWithValue("@keyword", keyword);
                    insertCmd.ExecuteNonQuery();
                }

            }

        }


        public static string AddThese(TheseDAL t, ProfessorDAL pr, List<AuthorDAL> auth)
        {
            // ------------ connection to db settings 
            SQLiteConnection connection = new SQLiteConnection(DataAccessSettings.connectionString);

            try
            {
                // ADD PROF TO DB IF NOT EXISTED
                int profId = addProf(pr);
                if (profId == -1)
                    return "Error: Professor information could not be saved. Please check the professor details and try again.";

                // the query to add the these to the database
                string query = "INSERT INTO Theses (titre, sousTitre, faculty, domain, datePub, description, status, university, prof_id, dateSoutenance, imagePath, imageData, pdfPath, pdfData) " +
                    "VALUES (@titre, @sousTitre, @faculty, @domain, @datePub, @description, @status, @university, @prof_id, @dateSoutenance, @imagePath, @imageData, @pdfPath, @pdfData);" +
                    "SELECT last_insert_rowid();"; // this line is to get the these_id of the inserted these to establish relation between the these and the authors

                SQLiteCommand command = new SQLiteCommand(query, connection);
                command.Parameters.AddWithValue("@titre", t.titre);
                command.Parameters.AddWithValue("@sousTitre", t.sousTitre);
                command.Parameters.AddWithValue("@faculty", t.faculty);
                command.Parameters.AddWithValue("@datePub", t.datePub);
                command.Parameters.AddWithValue("@status", t.status);
                command.Parameters.AddWithValue("@imagePath", t.imagePath);
                command.Parameters.AddWithValue("@imageData", t.imageData);
                command.Parameters.AddWithValue("@description", t.description);
                command.Parameters.AddWithValue("@university", t.university);
                command.Parameters.AddWithValue("@prof_id", profId);
                command.Parameters.AddWithValue("@pdfPath", t.pdfPath);
                command.Parameters.AddWithValue("@pdfData", t.pdfData);
                command.Parameters.AddWithValue("@domain", t.domain);
                command.Parameters.AddWithValue("@dateSoutenance", t.dateSoutenance);

                connection.Open();
                // Enable foreign key constraints
                using (var commandPragma = new SQLiteCommand("PRAGMA foreign_keys = ON", connection))
                {
                    commandPragma.ExecuteNonQuery();
                }

                // add the These to the database and return the thesis ID
                var rowsAffected = command.ExecuteScalar();

                // AT THIS MOMENT THE THESE IS ADDED TO DB
                // CONNECT THE THESE TO ITS AUTHORS AND KEYWORDS IN DB:
                if (rowsAffected != null)
                {
                    int theseID = Convert.ToInt32(rowsAffected);

                    try
                    {
                        addAuthors(auth, theseID);
                    }
                    catch (Exception ex)
                    {
                        // If adding authors fails, we should still continue with keywords
                        // but log the error and include it in the return message
                        return $"Thesis added but there was an issue linking authors: {ex.Message}";
                    }

                    try
                    {
                        AddKeywordsToThesis(theseID, t.keywords);
                    }
                    catch (Exception ex)
                    {
                        return $"Thesis added but there was an issue with keywords: {ex.Message}";
                    }

                    return "Thesis added successfully!";
                }
                else
                {
                    return "Error: Failed to add thesis. No ID was returned from the database.";
                }
            }
            catch (SQLiteException ex) when (ex.Message.Contains("UNIQUE constraint failed"))
            {
                // Handle specific SQLite constraint violations
                if (ex.Message.Contains("titre"))
                    return "Error: A thesis with this title already exists in the database.";
                else
                    return new UniqueConstraintViolationException("thesis record", ex).Message;
            }
            catch (SQLiteException ex) when (ex.Message.Contains("FOREIGN KEY constraint failed"))
            {
                return new ForeignKeyConstraintViolationException("referenced record", ex).Message;
            }
            catch (SQLiteException ex) when (ex.Message.Contains("NOT NULL constraint failed"))
            {
                string fieldName = "unknown field";

                // Try to extract field name from error message
                if (ex.Message.Contains("titre"))
                    fieldName = "title";
                else if (ex.Message.Contains("sousTitre"))
                    fieldName = "subtitle";
                else if (ex.Message.Contains("faculty"))
                    fieldName = "faculty";
                else if (ex.Message.Contains("domain"))
                    fieldName = "domain";
                else if (ex.Message.Contains("university"))
                    fieldName = "university";

                return new NotNullConstraintViolationException(fieldName, ex).Message;
            }
            catch (SQLiteException ex)
            {
                return $"Database error: {ex.Message}";
            }
            catch (InvalidOperationException ex)
            {
                return new DatabaseConnectionException("Could not connect to the database. Please try again later.", ex).Message;
            }
            catch (Exception ex)
            {
                return $"An unexpected error occurred: {ex.Message}";
            }
            finally
            {
                if (connection.State == System.Data.ConnectionState.Open)
                    connection.Close();
            }
        }

        // overloaded method dedicated for the user to propose a new these
        public static string AddThese(int userID, TheseDAL t, ProfessorDAL pr, List<AuthorDAL> auth)
        {
            // ------------ connection to db settings 
            SQLiteConnection connection = new SQLiteConnection(DataAccessSettings.connectionString);

            try
            {
                // ADD PROF TO DB IF NOT EXISTED
                int profId = addProf(pr);
                if (profId == -1)
                    return "Error: Professor information could not be saved. Please check the professor details and try again.";

                // the query to add the these to the database
                string query = "INSERT INTO Theses (titre, sousTitre, faculty, domain, datePub, description, status, university, prof_id, dateSoutenance, imagePath, imageData, pdfPath, pdfData) " +
                    "VALUES (@titre, @sousTitre, @faculty, @domain, @datePub, @description, @status, @university, @prof_id, @dateSoutenance, @imagePath, @imageData, @pdfPath, @pdfData);" +
                    "SELECT last_insert_rowid();"; // this line is to get the these_id of the inserted these to establish relation between the these and the authors

                SQLiteCommand command = new SQLiteCommand(query, connection);
                command.Parameters.AddWithValue("@titre", t.titre);
                command.Parameters.AddWithValue("@sousTitre", t.sousTitre);
                command.Parameters.AddWithValue("@faculty", t.faculty);
                command.Parameters.AddWithValue("@datePub", t.datePub);
                command.Parameters.AddWithValue("@status", t.status);
                command.Parameters.AddWithValue("@imagePath", t.imagePath);
                command.Parameters.AddWithValue("@imageData", t.imageData);
                command.Parameters.AddWithValue("@description", t.description);
                command.Parameters.AddWithValue("@university", t.university);
                command.Parameters.AddWithValue("@prof_id", profId);
                command.Parameters.AddWithValue("@pdfPath", t.pdfPath);
                command.Parameters.AddWithValue("@pdfData", t.pdfData);
                command.Parameters.AddWithValue("@domain", t.domain);
                command.Parameters.AddWithValue("@dateSoutenance", t.dateSoutenance);

                connection.Open();
                // Enable foreign key constraints
                using (var commandPragma = new SQLiteCommand("PRAGMA foreign_keys = ON", connection))
                {
                    commandPragma.ExecuteNonQuery();
                }

                // add the These to the database and return the thesis ID
                var rowsAffected = command.ExecuteScalar();

                // AT THIS MOMENT THE THESE IS ADDED TO DB
                // CONNECT THE THESE TO ITS AUTHORS AND KEYWORDS IN DB:
                if (rowsAffected != null)
                {
                    int theseID = Convert.ToInt32(rowsAffected);

                    try
                    {
                        addAuthors(auth, theseID);
                    }
                    catch (Exception ex)
                    {
                        // If adding authors fails, we should still continue with keywords
                        // but log the error and include it in the return message
                        return $"Thesis added but there was an issue linking authors: {ex.Message}";
                    }

                    try
                    {
                        AddKeywordsToThesis(theseID, t.keywords);
                    }
                    catch (Exception ex)
                    {
                        return $"Thesis added but there was an issue with keywords: {ex.Message}";
                    }

                    if (t.status == "pending") // here we are checking if this these is proposed by a user to make a relation between user and these in db
                    {
                        try
                        {
                            // making a relation between the user who proposed the these and the these in db 
                            UserDAL.markUserAndTheseProposed(userID, theseID);
                        }
                        catch (Exception ex)
                        {
                            return $"Thesis added with an issue!"; // a message to the user can't be technichal
                        }
                    }


                    return "Thesis added successfully!";
                }
                else
                {
                    return "Error: Failed to add thesis. No ID was returned from the database.";
                }
            }
            catch (SQLiteException ex) when (ex.Message.Contains("UNIQUE constraint failed"))
            {
                // Handle specific SQLite constraint violations
                if (ex.Message.Contains("titre"))
                    return "Error: A thesis with this title already exists in the database.";
                else
                    return new UniqueConstraintViolationException("thesis record", ex).Message;
            }
            catch (SQLiteException ex) when (ex.Message.Contains("FOREIGN KEY constraint failed"))
            {
                return new ForeignKeyConstraintViolationException("referenced record", ex).Message;
            }
            catch (SQLiteException ex) when (ex.Message.Contains("NOT NULL constraint failed"))
            {
                string fieldName = "unknown field";

                // Try to extract field name from error message
                if (ex.Message.Contains("titre"))
                    fieldName = "title";
                else if (ex.Message.Contains("sousTitre"))
                    fieldName = "subtitle";
                else if (ex.Message.Contains("faculty"))
                    fieldName = "faculty";
                else if (ex.Message.Contains("domain"))
                    fieldName = "domain";
                else if (ex.Message.Contains("university"))
                    fieldName = "university";

                return new NotNullConstraintViolationException(fieldName, ex).Message;
            }
            catch (SQLiteException ex)
            {
                return $"Database error: {ex.Message}";
            }
            catch (InvalidOperationException ex)
            {
                return new DatabaseConnectionException("Could not connect to the database. Please try again later.", ex).Message;
            }
            catch (Exception ex)
            {
                return $"An unexpected error occurred: {ex.Message}";
            }
            finally
            {
                if (connection.State == System.Data.ConnectionState.Open)
                    connection.Close();
            }
        }
        
        // method to accept a pending these or Rejected (en attend) -make it approved- 
        public static void setTheseAsApproved(int these_id)
        {
            // ------------ connection to db settings 
            SQLiteConnection connection = new SQLiteConnection(DataAccessSettings.connectionString);
            // the query to set the these with id (these_id) as accepted
            string query = "UPDATE Theses SET status = 'accepted' WHERE these_id = @these_id";
            SQLiteCommand command = new SQLiteCommand(query, connection);
            command.Parameters.AddWithValue("@these_id", these_id);
            // ------------ connection to db settings
            try
            {
                connection.Open();

                // this code should be executed every time we open the connection to pay attention to the foreign key constraints
                using (var commandPragma = new SQLiteCommand("PRAGMA foreign_keys = ON", connection))
                {
                    commandPragma.ExecuteNonQuery();
                }

                // set the These As accepted
                int rowsAffected = command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                connection.Close();
            }
        }

        // this method is used to retrieve all the methods that are either (approved,pending,rejected)
        public static List<TheseDAL> GetAllTheses(string status)
        {
            // select all these that have the same status

            SQLiteConnection connection = new SQLiteConnection(DataAccessSettings.connectionString);
            string query = $@"SELECT t.*,
                p.nom AS prof_nom, p.prenom AS prof_prenom,
                a.nom AS author_nom, a.prenom AS author_prenom, a.mail AS author_mail
                FROM Theses t
                LEFT JOIN Profs p ON t.prof_id = p.prof_id
                JOIN author_Thesis at ON at.these_id = t.these_id
                JOIN Auteurs a ON a.auteur_id = at.auteur_id
                WHERE status = @status
                GROUP BY t.these_id";
            SQLiteCommand command = new SQLiteCommand(query, connection);
            command.Parameters.AddWithValue("@status", status);


            List<TheseDAL> theses = new List<TheseDAL>();
            try
            {
                connection.Open();

                // this code should be executed every time we open the connection to pay attention to the foreign key constraints
                using (var commandPragma = new SQLiteCommand("PRAGMA foreign_keys = ON", connection))
                {
                    commandPragma.ExecuteNonQuery();
                }

                SQLiteDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    // read the data from the database and store it in the these object
                    TheseDAL these = new TheseDAL();
                    these.prof = new ProfessorDAL();
                    these.author = new AuthorDAL();

                    these.these_id = Convert.ToInt32(reader["these_id"]);
                    these.titre = (string)reader["titre"];
                    these.sousTitre = (string)reader["sousTitre"];
                    these.faculty = (string)reader["faculty"];
                    these.domain = (string)reader["domain"];
                    these.datePub = Convert.ToDateTime(reader["datePub"]);
                    these.pdfData = reader["pdfData"] != DBNull.Value ? (byte[])reader["pdfData"] : new byte[0];
                    // getting the pdf file from the db to the data folder to open it
                    string dataFolder = Path.Combine(
                    AppDomain.CurrentDomain.BaseDirectory,
                    "..", "..", "..",
                    "Data", "Documents", "theseAdmin");
                    these.pdfPath = Path.Combine(dataFolder, (string)reader["pdfPath"]);
                    these.description = (string)reader["description"];
                    these.status = (string)reader["status"];
                    these.university = (string)reader["university"];
                    these.prof_id = reader["prof_id"] != DBNull.Value ? Convert.ToInt32(reader["prof_id"]) : 0; // Defaulting to 0 if null
                    these.imageData = reader["imageData"] != DBNull.Value ? (byte[])reader["imageData"] : new byte[0];
                    string dataImageFolder = Path.Combine(
                        AppDomain.CurrentDomain.BaseDirectory,
                        "..", "..", "..",
                        "Data", "Images");
                    these.imagePath = Path.Combine(dataImageFolder, (string)reader["imagePath"]);
                    these.dateSoutenance = Convert.ToDateTime(reader["dateSoutenance"]);

                    these.prof.nom = reader["prof_nom"] as string ?? "";
                    these.prof.prenom = reader["prof_prenom"] as string ?? "";

                    these.author.FirstName = reader["author_prenom"] as string ?? "";
                    these.author.LastName = reader["author_nom"] as string ?? "";
                    these.author.Email = reader["author_mail"] as string ?? "";

                    theses.Add(these);
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                connection.Close();
            }

            return theses;
        } 

        // ---------------------------------------------------------------

    }
}
