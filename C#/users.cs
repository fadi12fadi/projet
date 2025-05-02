using System;
using System.Collections.Generic;


public class UserAccount : User
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Username { get; set; }
    public string Email { get; set; } 
    public string Password { get; private set; }
    public string Matricule { get; set; }
    public long CodeNIN { get; set; }
    public bool IsAdmin { get; private set; }
    public long ID { get; private set; }
    public List<These> Favorites { get; private set; } 
    public List<These> MyTheses { get; private set; }

    public UserAccount(
        string firstName, string lastName, string username, 
        string password, string matricule, long codeNIN, long id)
    {
        FirstName = firstName;
        LastName = lastName;
        Username = username;
        Password = password;
        Matricule = matricule;
        CodeNIN = codeNIN;
        IsAdmin = false;
        ID = id;
        Favorites = new List<These>();
        MyTheses = new List<These>();
    }

    // Methods
    public void AddTheseToCollection(These obj, CollectionOfTheses collection)
    {
        collection.RequestToAddThese(obj);
    }

    public void AddToMyTheses(These obj)
    {
        MyTheses.Add(obj);
    }

    public void AddToFavorites(These obj)
    {
        Favorites.Add(obj);
    }

    public void DisplayMyTheses()
    {
        foreach (var item in MyTheses)
        {
            User.DisplayThese(item);
        }
    }

    public void DisplayFavorites()
    {
        foreach (var item in Favorites)
        {
            User.DisplayThese(item);
        }
    }
}