using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// here we are communicating with the DB layer that we created, to use all the classes and functions that are implemented in the DB layer
using DataAccessLayer;


/*  
 * This Layer contains all the business logic of the application.  
 * It processes data, applies rules, and communicates with the Data Access Layer (DAL).  
 * It should NOT directly interact with the database or UI.  
 * Example: hashing the user password before saving it hashed to the database.  
 */

namespace BusinessLogicLayer
{
    public class UserBLL
    {
        public int id;
        public string username;
        public string password;
        public long matricule;
        public bool isAdmin;
        public string lastName;
        public string firstName;
        public DateTime signInDate;
        public int codeNiN;
        public string status;


        public UserBLL() { }

        // general constructor with all the attributes
        public UserBLL(int id, string username, string password, long matricule, bool isAdmin, string lastName, string firstName, DateTime signInDate, int codeNiN, string status)
        {
            this.id = id;
            this.username = username;
            this.password = password;
            this.matricule = matricule;
            this.isAdmin = isAdmin;
            this.lastName = lastName;
            this.firstName = firstName;
            this.signInDate = signInDate;
            this.codeNiN = codeNiN;
            this.status = status;
        }


        // here we are creating a constructor
        // that will take the user data from the DAL layer as a struct -UserDAL-
        // and store it in the BLL layer as an object -UserBLL-
        public UserBLL(DataAccessLayer.UserDAL user)
        {
            this.id = user.id;
            this.username = user.username;
            this.password = user.password;
            this.matricule = user.matricule;
            this.isAdmin = user.isAdmin;
            this.lastName = user.lastName;
            this.firstName = user.firstName;
            this.signInDate = user.signInDate;
            this.codeNiN = user.codeNiN;
            this.status = user.status;
        }

        public static UserBLL testFindByUserName(string username)
        {
            DataAccessLayer.UserDAL u = DataAccessLayer.AdminDAL.GetUser(username);

            UserBLL user = new UserBLL(u);

            return user;
            
        }
    }
}
