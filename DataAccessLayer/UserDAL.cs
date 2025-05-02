using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// this two lines are essential to deal with DB
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Net.Http.Headers;
using System.Net;
using System.Windows.Input;


/* 
 * This Layer contains only the functions that deals with the DataBase NOTHING ELSE
 * such as :
 * Connecting to the database
 * Executing SQL queries (SELECT, INSERT, UPDATE, DELETE)
*/
namespace DataAccessLayer
{
    // Defining the class User of the DataAccessLayer
    public class UserDAL
    {

        public int id { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public string matricule { get; set; }
        public bool isAdmin { get; set; }
        public string lastName { get; set; }
        public string firstName { get; set; }
        public DateTime signInDate { get; set; }
        public string codeNiN { get; set; }
        public string status { get; set; }
        public string email { get; set; }

        public UserDAL()
        {
            this.id = 0;
            this.username = "";
            this.password = "";
            this.matricule = "";
            this.isAdmin = false;
            this.lastName = "";
            this.firstName = "";
            this.signInDate = DateTime.Now;
            this.codeNiN = "";
            this.status = "pending";
            this.email = "";
        }

        // get all theses with title

        public static TheseDAL GetThesesByTheseId(int id)
        {
            TheseDAL these = new TheseDAL();
            these.prof = new ProfessorDAL();
            these.author = new AuthorDAL();
            // query to get the these and its author and the prof that is supervising it by the id
            string query = @"SELECT 
                    t.*, 
                    p.nom AS prof_nom, p.prenom AS prof_prenom,
                    a.nom AS author_nom, a.prenom AS author_prenom, a.mail AS author_mail,
                    FROM Theses t
                    LEFT JOIN Profs p ON t.prof_id = p.prof_id
                    JOIN author_Thesis at ON at.auteur_id = a.auteur_id AND at.these_id = t.these_id
                    JOIN Auteurs a ON a.auteur_id = at.auteur_id
                    WHERE t.these_id = @id";
            using (SQLiteConnection connection = new SQLiteConnection(DataAccessSettings.connectionString))
            using (SQLiteCommand command = new SQLiteCommand(query, connection))
            {
                // Add the parameter
                command.Parameters.AddWithValue("@id", id);
                connection.Open();
                using (SQLiteDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        these.these_id = Convert.ToInt32(reader["these_id"]);
                        these.titre = (string)reader["titre"];
                        these.datePub = Convert.ToDateTime(reader["datePub"]);
                        these.status = (string)reader["status"];
                        // getting the pdf file from the db to the data folder to open it
                        string dataFolder = Path.Combine(
                        AppDomain.CurrentDomain.BaseDirectory,
                        "..", "..", "..",
                        "Data", "Documents", "theseAdmin");
                        these.pdfPath = Path.Combine(dataFolder, (string)reader["pdfPath"]);
                        these.pdfData = reader["pdfData"] as byte[];
                        // Assign values with null checking
                        these.sousTitre = reader["sousTitre"] as string ?? "";
                        these.domain = reader["domain"] as string ?? "";
                        these.faculty = reader["faculty"] as string ?? "";
                        these.description = reader["description"] as string ?? "";
                        these.university = reader["university"] as string ?? "";
                        these.dateSoutenance = reader["dateSoutenance"] as DateTime? ?? DateTime.MinValue;
                        string dataImageFolder = Path.Combine(
                        AppDomain.CurrentDomain.BaseDirectory,
                        "..", "..", "..",
                        "Data", "Images");
                        these.imagePath = Path.Combine(dataImageFolder, (string)reader["imagePath"]);
                        these.imageData = reader["imageData"] as byte[];

                        these.prof.nom = reader["prof_nom"] as string ?? "";
                        these.prof.prenom = reader["prof_prenom"] as string ?? "";

                        these.author.FirstName = reader["author_prenom"] as string ?? "";
                        these.author.LastName = reader["author_nom"] as string ?? "";
                        these.author.Email = reader["author_mail"] as string ?? "";

                    }
                    connection.Close();
                }
            }
            return these;
        }

        // get all theses with title
        public static List<TheseDAL> GetThesesByTitle(string title)
        {
            List<TheseDAL> theses = new List<TheseDAL>();
            // query to get the these and its author and the prof that is supervising it by the title


            string query = @"SELECT 
                    t.*, 
                    p.nom AS prof_nom, p.prenom AS prof_prenom,
                    a.nom AS author_nom, a.prenom AS author_prenom, a.mail AS author_mail
                    FROM Theses t
                    LEFT JOIN Profs p ON t.prof_id = p.prof_id
                    JOIN author_Thesis at ON at.auteur_id = a.auteur_id AND at.these_id = t.these_id
                    JOIN Auteurs a ON a.auteur_id = at.auteur_id
                    WHERE t.titre LIKE @title AND status = 'accepted'";
            // we used left join in the profs table because we can encouter a these without a prof in DB (prof_id = NULL)
            // Use SQLiteConnection and SQLiteCommand
            using (SQLiteConnection connection = new SQLiteConnection(DataAccessSettings.connectionString))
            using (SQLiteCommand command = new SQLiteCommand(query, connection))
            {
                // Add the parameter
                command.Parameters.AddWithValue("@title", '%' + title + '%');

                connection.Open();
                using (SQLiteDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        TheseDAL these = new TheseDAL();
                        these.prof = new ProfessorDAL();
                        these.author = new AuthorDAL();

                        these.these_id = Convert.ToInt32(reader["these_id"]);
                        these.titre = (string)reader["titre"];
                        these.datePub = Convert.ToDateTime(reader["datePub"]);
                        these.status = (string)reader["status"];

                        // getting the pdf file from the db to the data folder to open it
                        string dataFolder = Path.Combine(
                        AppDomain.CurrentDomain.BaseDirectory,
                        "..", "..", "..",
                        "Data", "Documents", "theseAdmin");
                        these.pdfPath = Path.Combine(dataFolder, (string)reader["pdfPath"]);

                        these.pdfData = reader["pdfData"] as byte[];

                        // Assign values with null checking
                        these.sousTitre = reader["sousTitre"] as string ?? "";
                        these.domain = reader["domain"] as string ?? "";
                        these.faculty = reader["faculty"] as string ?? "";
                        these.description = reader["description"] as string ?? "";
                        these.university = reader["university"] as string ?? "";
                        these.dateSoutenance = reader["dateSoutenance"] as DateTime? ?? DateTime.MinValue;

                        string dataImageFolder = Path.Combine(
                        AppDomain.CurrentDomain.BaseDirectory,
                        "..", "..", "..",
                        "Data", "Images");
                        these.imagePath = Path.Combine(dataImageFolder, (string)reader["imagePath"]);

                        these.imageData = reader["imageData"] as byte[];

                        these.prof.nom = reader["prof_nom"] as string ?? "";
                        these.prof.prenom = reader["prof_prenom"] as string ?? "";

                        these.author.FirstName = reader["author_prenom"] as string ?? "";
                        these.author.LastName = reader["author_nom"] as string ?? "";
                        these.author.Email = reader["author_mail"] as string ?? "";

                        theses.Add(these);
                    }
                }
            }
            return theses;
        }

        // get all theses that are written by the input author
        public static List<TheseDAL> GetThesesByAuthor(string authorName)
        {
            List<TheseDAL> theses = new List<TheseDAL>();

            // query to get the these and the prof that is supervising it by the author name
            string query = @"SELECT 
                    t.*, 
                    p.nom AS prof_nom, p.prenom AS prof_prenom,
                    a.nom AS author_nom, a.prenom AS author_prenom, a.mail AS author_mail
                    FROM Theses t         
                    LEFT JOIN Profs p ON t.prof_id = p.prof_id
                    JOIN author_thesis at ON at.auteur_id = a.auteur_id AND at.these_id = t.these_id
                    JOIN Auteurs a ON a.auteur_id = at.auteur_id
                    WHERE (a.nom LIKE @authorName OR a.prenom LIKE @authorName) AND status = 'accepted'";

            using (SQLiteConnection connection = new SQLiteConnection(DataAccessSettings.connectionString))
            using (SQLiteCommand command = new SQLiteCommand(query, connection))
            {
                command.Parameters.AddWithValue("@authorName", authorName);
                connection.Open();
                using (SQLiteDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        TheseDAL these = new TheseDAL();
                        these.prof = new ProfessorDAL();
                        these.author = new AuthorDAL();

                        these.these_id = Convert.ToInt32(reader["these_id"]);
                        these.titre = (string)reader["titre"];
                        these.datePub = Convert.ToDateTime(reader["datePub"]);
                        these.status = (string)reader["status"];
                        // getting the pdf file from the db to the data folder to open it
                        string dataFolder = Path.Combine(
                        AppDomain.CurrentDomain.BaseDirectory,
                        "..", "..", "..",
                        "Data", "Documents", "theseAdmin");
                        these.pdfPath = Path.Combine(dataFolder, (string)reader["pdfPath"]);

                        these.pdfData = reader["pdfData"] as byte[];

                        // assign their values and check if null
                        these.sousTitre = reader["sousTitre"] as string ?? "";
                        these.domain = reader["domain"] as string ?? "";
                        these.faculty = reader["faculty"] as string ?? "";
                        these.description = reader["description"] as string ?? "";
                        these.university = reader["university"] as string ?? "";
                        these.dateSoutenance = reader["dateSoutenance"] as DateTime? ?? DateTime.MinValue;
                        
                        string dataImageFolder = Path.Combine(
                        AppDomain.CurrentDomain.BaseDirectory,
                        "..", "..", "..",
                        "Data", "Images");
                        these.imagePath = Path.Combine(dataImageFolder, (string)reader["imagePath"]);

                        these.imageData = reader["imageData"] as byte[];

                        these.prof.nom = reader["prof_nom"] as string ?? "";
                        these.prof.prenom = reader["prof_prenom"] as string ?? "";
                        
                        

                        these.author.FirstName = reader["author_prenom"] as string ?? "";
                        these.author.LastName = reader["author_nom"] as string ?? "";
                        these.author.Email = reader["author_mail"] as string ?? "";

                        theses.Add(these);
                    }
                }
            }

            return theses;
        }

        // get all these by keywords
        public static List<TheseDAL> GetThesesByKeywords(string[] keywordList)
        {

            List<string> likeConditions = new List<string>();
            Dictionary<string, string> parameters = new Dictionary<string, string>();

            int index = 0; // to create a unique parameter name for each keyword to use later in the query
            foreach (string kw in keywordList)
            {
                string paramName = "@kw" + index;
                likeConditions.Add($"LOWER(kw.KeyWord) LIKE {paramName}");
                parameters[paramName] = "%" + kw.ToLower() + "%";
                index++;
            }

            // Join all conditions with OR
            string whereClause = string.Join(" OR ", likeConditions);

            // Final dynamic query
            string query = $@"SELECT DISTINCT t.*,
                    p.nom AS prof_nom, p.prenom AS prof_prenom,
                    a.nom AS author_nom, a.prenom AS author_prenom, a.mail AS author_mail                 
                    FROM Theses t
                    JOIN KeyWord_These kwt ON kwt.these_id = t.these_id
                    JOIN KeyWords kw ON LOWER(kwt.keyWord) = LOWER(kw.keyWord)
                    AND ({whereClause})
                    LEFT JOIN Profs p ON t.prof_id = p.prof_id
                    JOIN author_Thesis at ON at.these_id = t.these_id
                    JOIN Auteurs a ON a.auteur_id = at.auteur_id
                    WHERE t.status = 'accepted'
                    GROUP BY t.these_id ";


            List<TheseDAL> theses = new List<TheseDAL>();


            using (SQLiteConnection connection = new SQLiteConnection(DataAccessSettings.connectionString))
            using (SQLiteCommand command = new SQLiteCommand(query, connection))
            {
                foreach (var param in parameters)
                {
                    command.Parameters.AddWithValue(param.Key, param.Value);
                }

                connection.Open();

                using (SQLiteDataReader reader = command.ExecuteReader())
                {

                    while (reader.Read())
                    {

                        TheseDAL these = new TheseDAL();
                        these.prof = new ProfessorDAL();
                        these.author = new AuthorDAL();

                        these.these_id = Convert.ToInt32(reader["these_id"]);
                        these.titre = (string)reader["titre"];
                        these.datePub = Convert.ToDateTime(reader["datePub"]);
                        these.status = (string)reader["status"];
                        // getting the pdf file from the db to the data folder to open it
                        string dataFolder = Path.Combine(
                        AppDomain.CurrentDomain.BaseDirectory,
                        "..", "..", "..",
                        "Data", "Documents", "theseAdmin");
                        these.pdfPath = Path.Combine(dataFolder, (string)reader["pdfPath"]);
                        these.pdfData = reader["pdfData"] as byte[];
                        // assign their values and check if null
                        these.sousTitre = reader["sousTitre"] as string ?? "";
                        these.domain = reader["domain"] as string ?? "";
                        these.faculty = reader["faculty"] as string ?? "";
                        these.description = reader["description"] as string ?? "";
                        these.university = reader["university"] as string ?? "";
                        these.dateSoutenance = reader["dateSoutenance"] as DateTime? ?? DateTime.MinValue;
                        string dataImageFolder = Path.Combine(
                        AppDomain.CurrentDomain.BaseDirectory,
                        "..", "..", "..",
                        "Data", "Images");
                        these.imagePath = Path.Combine(dataImageFolder, (string)reader["imagePath"]);
                        these.imageData = reader["imageData"] as byte[];

                        these.prof.nom = reader["prof_nom"] as string ?? "";
                        these.prof.prenom = reader["prof_prenom"] as string ?? "";

                        these.author.FirstName = reader["author_prenom"] as string ?? "";
                        these.author.LastName = reader["author_nom"] as string ?? "";
                        these.author.Email = reader["author_mail"] as string ?? "";

                        theses.Add(these);
                    }
                }
            }
            return theses;
        }

        // get all these by domaine
        public static List<TheseDAL> GetThesesByDomain(string domain)
        {
            List<TheseDAL> theses = new List<TheseDAL>();


            string query = $@"SELECT DISTINCT t.*,
                p.nom AS prof_nom, p.prenom AS prof_prenom,
                a.nom AS author_nom, a.prenom AS author_prenom, a.mail AS author_mail
                FROM Theses t
                LEFT JOIN Profs p ON t.prof_id = p.prof_id
                JOIN author_Thesis at ON at.these_id = t.these_id
                JOIN Auteurs a ON a.auteur_id = at.auteur_id
                WHERE LOWER(t.domain) LIKE LOWER('%{domain}%') AND t.status = 'accepted'
                GROUP BY t.these_id";
            using (SQLiteConnection connection = new SQLiteConnection(DataAccessSettings.connectionString))
            using (SQLiteCommand command = new SQLiteCommand(query, connection))
            {
                command.Parameters.AddWithValue("@domain", domain);
                connection.Open();

                using (SQLiteDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        TheseDAL these = new TheseDAL();
                        these.prof = new ProfessorDAL();
                        these.author = new AuthorDAL();

                        these.these_id = Convert.ToInt32(reader["these_id"]);
                        these.titre = (string)reader["titre"];
                        these.datePub = Convert.ToDateTime(reader["datePub"]);
                        these.status = (string)reader["status"];
                        these.domain = (string)reader["domain"];
                        // getting the pdf file from the db to the data folder to open it
                        string dataFolder = Path.Combine(
                        AppDomain.CurrentDomain.BaseDirectory,
                        "..", "..", "..",
                        "Data", "Documents", "theseAdmin");
                        these.pdfPath = Path.Combine(dataFolder, (string)reader["pdfPath"]);
                        these.pdfData = reader["pdfData"] as byte[];
                        // assign their values and check if null
                        these.sousTitre = reader["sousTitre"] as string ?? "";
                        these.faculty = reader["faculty"] as string ?? "";
                        these.description = reader["description"] as string ?? "";
                        these.university = reader["university"] as string ?? "";
                        these.dateSoutenance = reader["dateSoutenance"] as DateTime? ?? DateTime.MinValue;
                        string dataImageFolder = Path.Combine(
                        AppDomain.CurrentDomain.BaseDirectory,
                        "..", "..", "..",
                        "Data", "Images");
                        these.imagePath = Path.Combine(dataImageFolder, (string)reader["imagePath"]);
                        these.imageData = reader["imageData"] as byte[];

                        these.prof.nom = reader["prof_nom"] as string ?? "";
                        these.prof.prenom = reader["prof_prenom"] as string ?? "";

                        these.author.FirstName = reader["author_prenom"] as string ?? "";
                        these.author.LastName = reader["author_nom"] as string ?? "";
                        these.author.Email = reader["author_mail"] as string ?? "";

                        theses.Add(these);
                    }
                }
            }
            return theses;
        }

        public static List<TheseDAL> GetThesesByUni(string univ)
        {
            List<TheseDAL> theses = new List<TheseDAL>();
            string query = $@"SELECT DISTINCT t.*,
                p.nom AS prof_nom, p.prenom AS prof_prenom,
                a.nom AS author_nom, a.prenom AS author_prenom, a.mail AS author_mail
                FROM Theses t
                LEFT JOIN Profs p ON t.prof_id = p.prof_id
                JOIN author_Thesis at ON at.these_id = t.these_id
                JOIN Auteurs a ON a.auteur_id = at.auteur_id
                WHERE LOWER(t.university) LIKE LOWER('%{univ}%') AND t.status = 'accepted'
                GROUP BY t.these_id";
            using (SQLiteConnection connection = new SQLiteConnection(DataAccessSettings.connectionString))
            using (SQLiteCommand command = new SQLiteCommand(query, connection))
            {
                connection.Open();
                using (SQLiteDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        TheseDAL these = new TheseDAL();
                        these.prof = new ProfessorDAL();
                        these.author = new AuthorDAL();

                        these.these_id = Convert.ToInt32(reader["these_id"]);
                        these.titre = (string)reader["titre"];
                        these.datePub = Convert.ToDateTime(reader["datePub"]);
                        these.status = (string)reader["status"];
                        these.domain = (string)reader["domain"];
                        // getting the pdf file from the db to the data folder to open it
                        string dataFolder = Path.Combine(
                        AppDomain.CurrentDomain.BaseDirectory,
                        "..", "..", "..",
                        "Data", "Documents", "theseAdmin");
                        these.pdfPath = Path.Combine(dataFolder, (string)reader["pdfPath"]);
                        these.pdfData = reader["pdfData"] as byte[];
                        // assign their values and check if null
                        these.sousTitre = reader["sousTitre"] as string ?? "";
                        these.faculty = reader["faculty"] as string ?? "";
                        these.description = reader["description"] as string ?? "";
                        these.university = reader["university"] as string ?? "";
                        these.dateSoutenance = reader["dateSoutenance"] as DateTime? ?? DateTime.MinValue;
                        string dataImageFolder = Path.Combine(
                        AppDomain.CurrentDomain.BaseDirectory,
                        "..", "..", "..",
                        "Data", "Images");
                        these.imagePath = Path.Combine(dataImageFolder, (string)reader["imagePath"]);
                        these.imageData = reader["imageData"] as byte[];

                        these.prof.nom = reader["prof_nom"] as string ?? "";
                        these.prof.prenom = reader["prof_prenom"] as string ?? "";

                        these.author.FirstName = reader["author_prenom"] as string ?? "";
                        these.author.LastName = reader["author_nom"] as string ?? "";
                        these.author.Email = reader["author_mail"] as string ?? "";

                        theses.Add(these);
                    }
                }
            }
            return theses;
        }

        public static List<TheseDAL> GetThesesByFaculty(string fac)
        {
            List<TheseDAL> theses = new List<TheseDAL>();
            string query = $@"SELECT DISTINCT t.*,
                p.nom AS prof_nom, p.prenom AS prof_prenom,
                a.nom AS author_nom, a.prenom AS author_prenom, a.mail AS author_mail
                FROM Theses t
                LEFT JOIN Profs p ON t.prof_id = p.prof_id
                JOIN author_Thesis at ON at.these_id = t.these_id
                JOIN Auteurs a ON a.auteur_id = at.auteur_id
                WHERE LOWER(t.faculty) LIKE LOWER('%{fac}%') AND t.status = 'accepted'
                GROUP BY t.these_id";
            using (SQLiteConnection connection = new SQLiteConnection(DataAccessSettings.connectionString))
            using (SQLiteCommand command = new SQLiteCommand(query, connection))
            {
                connection.Open();
                using (SQLiteDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        TheseDAL these = new TheseDAL();
                        these.prof = new ProfessorDAL();
                        these.author = new AuthorDAL();

                        these.these_id = Convert.ToInt32(reader["these_id"]);
                        these.titre = (string)reader["titre"];
                        these.datePub = Convert.ToDateTime(reader["datePub"]);
                        these.status = (string)reader["status"];
                        these.domain = (string)reader["domain"];
                        // getting the pdf file from the db to the data folder to open it
                        string dataFolder = Path.Combine(
                        AppDomain.CurrentDomain.BaseDirectory,
                        "..", "..", "..",
                        "Data", "Documents", "theseAdmin");
                        these.pdfPath = Path.Combine(dataFolder, (string)reader["pdfPath"]);
                        these.pdfData = reader["pdfData"] as byte[];
                        // assign their values and check if null
                        these.sousTitre = reader["sousTitre"] as string ?? "";
                        these.faculty = reader["faculty"] as string ?? "";
                        these.description = reader["description"] as string ?? "";
                        these.university = reader["university"] as string ?? "";
                        these.dateSoutenance = reader["dateSoutenance"] as DateTime? ?? DateTime.MinValue;
                        string dataImageFolder = Path.Combine(
                        AppDomain.CurrentDomain.BaseDirectory,
                        "..", "..", "..",
                        "Data", "Images");
                        these.imagePath = Path.Combine(dataImageFolder, (string)reader["imagePath"]);
                        these.imageData = reader["imageData"] as byte[];

                        these.prof.nom = reader["prof_nom"] as string ?? "";
                        these.prof.prenom = reader["prof_prenom"] as string ?? "";

                        these.author.FirstName = reader["author_prenom"] as string ?? "";
                        these.author.LastName = reader["author_nom"] as string ?? "";
                        these.author.Email = reader["author_mail"] as string ?? "";

                        theses.Add(these);
                    }
                }
            }
            return theses;
        }

        public static List<TheseDAL> GetThesesByYearSoutenance(string year)
        {
            List<TheseDAL> theses = new List<TheseDAL>();
            string query = @"
                SELECT DISTINCT t.*,
                p.nom AS prof_nom, p.prenom AS prof_prenom,
                a.nom AS author_nom, a.prenom AS author_prenom, a.mail AS author_mail
                FROM Theses t
                LEFT JOIN Profs p ON t.prof_id = p.prof_id
                JOIN author_Thesis at ON at.these_id = t.these_id
                JOIN Auteurs a ON a.auteur_id = at.auteur_id
                WHERE strftime('%Y', t.dateSoutenance) = @year AND t.status = 'accepted'
                GROUP BY t.these_id";

            using (SQLiteConnection connection = new SQLiteConnection(DataAccessSettings.connectionString))
            using (SQLiteCommand command = new SQLiteCommand(query, connection))
            {
                connection.Open();
                command.Parameters.AddWithValue("@year", year);

                using (SQLiteDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        TheseDAL these = new TheseDAL();
                        these.prof = new ProfessorDAL();
                        these.author = new AuthorDAL();

                        these.these_id = Convert.ToInt32(reader["these_id"]);
                        these.titre = (string)reader["titre"];
                        these.datePub = Convert.ToDateTime(reader["datePub"]);
                        these.status = (string)reader["status"];
                        these.domain = (string)reader["domain"];
                        // getting the pdf file from the db to the data folder to open it
                        string dataFolder = Path.Combine(
                        AppDomain.CurrentDomain.BaseDirectory,
                        "..", "..", "..",
                        "Data", "Documents", "theseAdmin");
                        these.pdfPath = Path.Combine(dataFolder, (string)reader["pdfPath"]);
                        these.pdfData = reader["pdfData"] as byte[];

                        these.sousTitre = reader["sousTitre"] as string ?? "";
                        these.faculty = reader["faculty"] as string ?? "";
                        these.description = reader["description"] as string ?? "";
                        these.university = reader["university"] as string ?? "";
                        these.dateSoutenance = reader["dateSoutenance"] as DateTime? ?? DateTime.MinValue;
                        string dataImageFolder = Path.Combine(
                        AppDomain.CurrentDomain.BaseDirectory,
                        "..", "..", "..",
                        "Data", "Images");
                        these.imagePath = Path.Combine(dataImageFolder, (string)reader["imagePath"]);
                        these.imageData = reader["imageData"] as byte[];

                        these.prof.nom = reader["prof_nom"] as string ?? "";
                        these.prof.prenom = reader["prof_prenom"] as string ?? "";

                        these.author.FirstName = reader["author_prenom"] as string ?? "";
                        these.author.LastName = reader["author_nom"] as string ?? "";
                        these.author.Email = reader["author_mail"] as string ?? "";

                        theses.Add(these);
                    }
                }
            }
            return theses;
        }

        //private static bool IsThesisAlreadyFavorited(int userId, int thesisFavId)
        //{
        //    bool alreadyFavorited = false;

        //    using (SQLiteConnection connection = new SQLiteConnection(DataAccessSettings.connectionString))
        //    {
        //        connection.Open();

        //        string query = "SELECT COUNT(*) FROM user_favorites WHERE user_id = @UserId AND thesefav_id = @ThesisFavId";

        //        using (SQLiteCommand command = new SQLiteCommand(query, connection))
        //        {
        //            command.Parameters.AddWithValue("@UserId", userId);
        //            command.Parameters.AddWithValue("@ThesisFavId", thesisFavId);

        //            // ExecuteScalar returns the first column of the first row
        //            int count = Convert.ToInt32(command.ExecuteScalar());

        //            // If count is greater than 0, the thesis is already favorited
        //            alreadyFavorited = (count > 0);
        //        }
        //    }

        //    return alreadyFavorited;
        //}

        public static string setTheseAsFav(int currentUserID, int tID)
        {
            string query = @"
                INSERT INTO user_favorites (user_id, thesefav_id, added_date)
                VALUES (@userId, @ThesisFavId, CURRENT_TIMESTAMP)
                ON CONFLICT(user_id, thesefav_id)
                DO UPDATE SET added_date = CURRENT_TIMESTAMP;";

            using (SQLiteConnection connection = new SQLiteConnection(DataAccessSettings.connectionString))
            {
                connection.Open();
                using (SQLiteCommand cmd = new SQLiteCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@userId", currentUserID);
                    cmd.Parameters.AddWithValue("@ThesisFavId", tID);


                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected > 0)
                        return "Thesis added to favorites successfully!";
                    else
                        return "Failed to add thesis to favorites.";

                }
            }

        }

        public static string undoFav(int currentUserID, int tID)
        {
            string query = @"
                DELETE FROM user_favorites
                WHERE user_id = @userId AND thesefav_id = @ThesisFavId;";
            using (SQLiteConnection connection = new SQLiteConnection(DataAccessSettings.connectionString))
            {
                connection.Open();
                using (SQLiteCommand cmd = new SQLiteCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@userId", currentUserID);
                    cmd.Parameters.AddWithValue("@ThesisFavId", tID);

                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected > 0)
                        return "The thesis is deleted from your favorites List !";
                    else
                        return "an error ocurred when deleting the thesis from your favorites List !";
                }
            }
        }

        public static List<TheseDAL> getFavs(int uID)
        {
            // here we are calling the function in the DAL layer to get the thesis that are favorite
            // and we are returning the result of the function
            List<TheseDAL> theses = new List<TheseDAL>();

            string query = @"SELECT 
                    t.*,
                    p.nom AS prof_nom, p.prenom AS prof_prenom,
                    a.nom AS author_nom, a.prenom AS author_prenom, a.mail AS author_mail,
                    CASE WHEN uf.user_id IS NOT NULL THEN 1 ELSE 0 END AS is_favorite
                    FROM Theses t
                    LEFT JOIN Profs p ON t.prof_id = p.prof_id
                    LEFT JOIN user_favorites uf ON t.these_id = uf.thesefav_id AND uf.user_id = @userId
                    JOIN author_Thesis at ON t.these_id = at.these_id
                    JOIN Auteurs a ON at.auteur_id = a.auteur_id
                    WHERE uf.user_id IS NOT NULL
                    ORDER BY uf.added_date DESC";

            using (SQLiteConnection connection = new SQLiteConnection(DataAccessSettings.connectionString))
            using (SQLiteCommand command = new SQLiteCommand(query, connection))
            {
                command.Parameters.AddWithValue("@userId", uID);
                connection.Open();
                using (SQLiteDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        TheseDAL these = new TheseDAL();
                        these.prof = new ProfessorDAL();
                        these.author = new AuthorDAL();

                        these.these_id = Convert.ToInt32(reader["these_id"]);
                        these.titre = (string)reader["titre"];
                        these.datePub = Convert.ToDateTime(reader["datePub"]);
                        these.status = (string)reader["status"];
                        these.domain = (string)reader["domain"];
                        // getting the pdf file from the db to the data folder to open it
                        string dataFolder = Path.Combine(
                        AppDomain.CurrentDomain.BaseDirectory,
                        "..", "..", "..",
                        "Data", "Documents", "theseAdmin");
                        these.pdfPath = Path.Combine(dataFolder, (string)reader["pdfPath"]);
                        these.sousTitre = (string)reader["sousTitre"];
                        these.pdfData = reader["pdfData"] as byte[];
                        // assign their values and check if null
                        these.pdfData = reader["pdfData"] as byte[] ?? null;
                        these.faculty = reader["faculty"] as string ?? "";
                        these.description = reader["description"] as string ?? "";
                        these.university = reader["university"] as string ?? "";
                        these.dateSoutenance = reader["dateSoutenance"] as DateTime? ?? DateTime.MinValue;
                        string dataImageFolder = Path.Combine(
                        AppDomain.CurrentDomain.BaseDirectory,
                        "..", "..", "..",
                        "Data", "Images");
                        these.imagePath = Path.Combine(dataImageFolder, (string)reader["imagePath"]);
                        these.imageData = reader["imageData"] as byte[] ?? null;

                        these.prof.nom = reader["prof_nom"] as string ?? "";
                        these.prof.prenom = reader["prof_prenom"] as string ?? "";

                        these.author.FirstName = reader["author_prenom"] as string ?? "";
                        these.author.LastName = reader["author_nom"] as string ?? "";
                        these.author.Email = reader["author_mail"] as string ?? "";

                        theses.Add(these);

                    }
                }
            }
            return theses;
        }

        //private static bool IsThesisAlreadyHistory(int userId, int thesisHisId)
        //{
        //    bool isInHistory = false;

        //    using (SQLiteConnection connection = new SQLiteConnection(DataAccessSettings.connectionString))
        //    {
        //        connection.Open();

        //        string query = "SELECT COUNT(*) FROM user_SearchHistory WHERE user_id = @UserId AND theseSearch_id = @ThesisHisId";

        //        using (SQLiteCommand command = new SQLiteCommand(query, connection))
        //        {
        //            command.Parameters.AddWithValue("@UserId", userId);
        //            command.Parameters.AddWithValue("@ThesisHisId", thesisHisId);

        //            // ExecuteScalar returns the first column of the first row
        //            int count = Convert.ToInt32(command.ExecuteScalar());

        //            // If count is greater than 0, the thesis is already favorited
        //            isInHistory = (count > 0);
        //        }
        //    }

        //    return isInHistory;
        //}
        public static void putTheseInHist(int currentUserID, int tID)
        { 

            string query = @"
                    INSERT INTO user_SearchHistory (user_id, theseSearch_id, added_date)
                    VALUES (@userId, @ThesisHisId, CURRENT_TIMESTAMP)
                    ON CONFLICT(user_id, theseSearch_id)
                    DO UPDATE SET added_date = CURRENT_TIMESTAMP;";
            using (SQLiteConnection connection = new SQLiteConnection(DataAccessSettings.connectionString))
            {
                connection.Open();
                using (SQLiteCommand cmd = new SQLiteCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@userId", currentUserID);
                    cmd.Parameters.AddWithValue("@ThesisHisId", tID);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public static string deleteFromHistory(int currentUserID, int tID)
        {
            string query = @"
                    DELETE FROM user_SearchHistory
                    WHERE user_id = @userId AND theseSearch_id = @ThesisHisId;";
            using (SQLiteConnection connection = new SQLiteConnection(DataAccessSettings.connectionString))
            {
                connection.Open();
                using (SQLiteCommand cmd = new SQLiteCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@userId", currentUserID);
                    cmd.Parameters.AddWithValue("@ThesisHisId", tID);
                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected > 0)
                        return "The thesis is deleted from your History List !";
                    else
                        return "an error ocurred when deleting the thesis from your History List !";

                }
            }
        }

        public static List<TheseDAL> getHistorics(int currentUserID)
        {
            // here we are calling the function in the DAL layer to get the thesis that are in the history
            // and we are returning the result of the function
            List<TheseDAL> theses = new List<TheseDAL>();
            string query = @"SELECT 
                    t.*,
                    p.nom AS prof_nom, p.prenom AS prof_prenom,
                    a.nom AS author_nom, a.prenom AS author_prenom, a.mail AS author_mail,
                    CASE WHEN uh.user_id IS NOT NULL THEN 1 ELSE 0 END AS is_histo
                    FROM Theses t
                    LEFT JOIN Profs p ON t.prof_id = p.prof_id
                    LEFT JOIN user_SearchHistory uh ON t.these_id = uh.theseSearch_id AND uh.user_id = @userId
                    JOIN author_Thesis at ON t.these_id = at.these_id
                    JOIN Auteurs a ON at.auteur_id = a.auteur_id
                    WHERE uh.user_id IS NOT NULL
                    ORDER BY uh.added_date DESC";
            using (SQLiteConnection connection = new SQLiteConnection(DataAccessSettings.connectionString))
            using (SQLiteCommand command = new SQLiteCommand(query, connection))
            {
                command.Parameters.AddWithValue("@userId", currentUserID);
                connection.Open();
                using (SQLiteDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        TheseDAL these = new TheseDAL();
                        these.prof = new ProfessorDAL();
                        these.author = new AuthorDAL();
                        these.these_id = Convert.ToInt32(reader["these_id"]);
                        these.titre = (string)reader["titre"];
                        these.datePub = Convert.ToDateTime(reader["datePub"]);
                        these.status = (string)reader["status"];
                        these.domain = (string)reader["domain"];
                        // getting the pdf file from the db to the data folder to open it
                        string dataFolder = Path.Combine(
                        AppDomain.CurrentDomain.BaseDirectory,
                        "..", "..", "..",
                        "Data", "Documents", "theseAdmin");
                        these.pdfPath = Path.Combine(dataFolder, (string)reader["pdfPath"]);
                        these.pdfData = reader["pdfData"] as byte[];
                        // assign their values and check if null
                        these.sousTitre = reader["sousTitre"] as string ?? "";
                        these.faculty = reader["faculty"] as string ?? "";
                        these.description = reader["description"] as string ?? "";
                        these.university = reader["university"] as string ?? "";
                        these.dateSoutenance = reader["dateSoutenance"] as DateTime? ?? DateTime.MinValue;
                        string dataImageFolder = Path.Combine(
                        AppDomain.CurrentDomain.BaseDirectory,
                        "..", "..", "..",
                        "Data", "Images");
                        these.imagePath = Path.Combine(dataImageFolder, (string)reader["imagePath"]);


                        these.prof.nom = reader["prof_nom"] as string ?? "";
                        these.prof.prenom = reader["prof_prenom"] as string ?? "";

                        these.author.FirstName = reader["author_prenom"] as string ?? "";
                        these.author.LastName = reader["author_nom"] as string ?? "";
                        these.author.Email = reader["author_mail"] as string ?? "";

                        theses.Add(these);
                    }
                }
            }
            return theses;
        }


        public static void markUserAndTheseProposed(int userId, int theseId)
        {
            SQLiteConnection connection = new SQLiteConnection(DataAccessSettings.connectionString);
            connection.Open();

            // this code should be executed every time we open the connection to pay attention to the foreign key constraints
            using (var commandPragma = new SQLiteCommand("PRAGMA foreign_keys = ON", connection))
            {
                commandPragma.ExecuteNonQuery();
            }

            using (SQLiteCommand insertUserToUserTheseTableCommand = new SQLiteCommand("INSERT INTO user_Mythese (user_id, thesePropose_id) VALUES (@user_id, @theseId);", connection))
            {
                insertUserToUserTheseTableCommand.Parameters.AddWithValue("@user_id", userId);
                insertUserToUserTheseTableCommand.Parameters.AddWithValue("@theseId", theseId);

                // run the command:
                int rowsAffected = insertUserToUserTheseTableCommand.ExecuteNonQuery();
            }
            connection.Close();
        }
    }
}
