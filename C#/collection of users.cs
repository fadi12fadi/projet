using System;
using System.Collections.Generic;

public class CollectionOfUsers
{
    private List<User> _users;
    private List<User> _bannedUsers;

    public CollectionOfUsers()
    {
        _users = new List<User>(); 
        _bannedUsers = new List<User>();
    }

    public void AddUser(User user)
    {
        _users.Add(user);
    }

    public void BanUser(User user)
    {
        _users.Remove(user);
        _bannedUsers.Add(user);
    }

    public bool RemoveUser(long id)
    {
        var userToRemove = _users.Find(u => u.ID == id);
        if (userToRemove != null)
        {
            _users.Remove(userToRemove);
            return true;
        }
        return false;
    }

    public void ShowUsers()
    {
        foreach (var user in _users)
        {
            Console.WriteLine($"User ID: {user.ID}, Username: {user.Username}");
        }
    }
}