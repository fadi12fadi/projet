using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class AuthorDAL
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public int ID { get; set; }
    public string Email { get; set; }

    // a constructor

    public AuthorDAL()
    {

    }

    public AuthorDAL(string firstName, string lastName, int id, string email)
    {
        FirstName = firstName;
        LastName = lastName;
        ID = id;
        Email = email;
    }
}