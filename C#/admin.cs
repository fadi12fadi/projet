using System;
using System.Collections.Generic;

public class Admin
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Username { get; set; }
    public string Email { get; set; }
    public string Password { get; private set; }
    public long ID { get; private set; }

    private CollectionOfUsers _userCollection;
    private CollectionOfTheses _thesisCollection;

    public Admin(
        string firstName, string lastName, string username,
        string password, long id,
        CollectionOfUsers userCollection,
        CollectionOfTheses thesisCollection
    )
    {
        FirstName = firstName;
        LastName = lastName;
        Username = username;
        Password = password;
        ID = id;
        _userCollection = userCollection;
        _thesisCollection = thesisCollection;
    }

    // --- User Management ---
    public void SetUserAdminStatus(long userId, bool isAdmin)
    {
        var user = _userCollection.GetUserById(userId);
        if (user != null && user is UserAccount userAccount)
        {
            userAccount.SetAdminStatus(isAdmin);
        }
    }

    public bool RemoveUser(long userId) => _userCollection.RemoveUser(userId);

    // --- Thesis Management ---
    public List<These> GetPendingThesesRequests() => 
        _thesisCollection.GetPendingThesesRequests();

    public bool ApprovePendingThese(long thesisId) => 
        _thesisCollection.ApproveThese(thesisId);

    public void AddTheseManually(These thesis)
    {
        _thesisCollection.AddApprovedThese(thesis);
    }

    public void RejectPendingThese(long thesisId)
    {
        var thesis = _thesisCollection.GetPendingThesesRequests()
            .FirstOrDefault(t => t.ID == thesisId);
        if (thesis != null)
        {
            _thesisCollection.RemovePendingThese(thesisId);
        }
    }
}