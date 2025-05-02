using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public class UserConnectionDAL
    {
        // function to login if the username and password are correct
        public static UserDAL Login(string username, string password)
        {
            // ------------ connection to db settings 
            SQLiteConnection connection = new SQLiteConnection(DataAccessSettings.connectionString);

            // the query to get the user that matches the username and password from the database
            string query = "SELECT * FROM Users WHERE username = @username AND passwordHashed = @pswd";

            SQLiteCommand command = new SQLiteCommand(query, connection);

            command.Parameters.AddWithValue("@username", username);
            command.Parameters.AddWithValue("@pswd", password);
            // ------------ connection to db settings

            UserDAL user = new UserDAL();
            user.id = 0; // it will stay 0 if the user is not found

            try
            {
                connection.Open();

                // the reader will read the data from the database and execute the query
                SQLiteDataReader reader = command.ExecuteReader();

                // if the user is found the while loop will be executed
                // and the reader will read the data and store it in the user struct
                while (reader.Read())
                {
                    user.id = Convert.ToInt32(reader["user_id"]);
                    user.username = (string)reader["username"];
                    user.password = (string)reader["passwordHashed"];
                    user.matricule = ((object)reader["matricule"] == (object)DBNull.Value) ? "0" : (string)reader["matricule"];
                    user.email = ((object)reader["email"] == (object)DBNull.Value) ? "0" : (string)reader["email"];
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
                //Console.WriteLine(ex.Message);
            }
            finally
            {
                connection.Close();
            }


            return user; // returning the user struct to the BLL
        }

    }
}
