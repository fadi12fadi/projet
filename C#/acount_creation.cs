using System;
using System.Security.Cryptography;
using System.Text;

public class AccountCreation
{
    private readonly CollectionOfUsers _usersDatabase;

    public AccountCreation(CollectionOfUsers usersDatabase)
    {
        _usersDatabase = usersDatabase;
    }

    public bool AddUser(
        string firstName, 
        string lastName, 
        string username,
        string rawPassword,
        string matricule,
        long codeNIN,
        long id)
    {
        // 1. Check if username exists
        if (_usersDatabase.GetUserByUsername(username) != null)
        {
            Console.WriteLine("Username already exists!");
            return false;
        }

        // 2. Hash password
        string hashedPassword = HashPassword(rawPassword);

        // 3. Create user object
        var newUser = new UserAccount(
            firstName: firstName,
            lastName: lastName,
            username: username,
            password: hashedPassword,  // Store only the hash
            matricule: matricule,
            codeNIN: codeNIN,
            id: id
        );

        // 4. Add to database
        _usersDatabase.AddUser(newUser);
        Console.WriteLine($"User {username} created successfully!");
        return true;
    }

    private string HashPassword(string password)
    {
        using SHA256 sha256 = SHA256.Create();
        byte[] hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
        return BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
    }
}using System;
using System.Security.Cryptography;
using System.Text;

public class AccountCreation
{
    private readonly CollectionOfUsers _usersDatabase;

    public AccountCreation(CollectionOfUsers usersDatabase)
    {
        _usersDatabase = usersDatabase;
    }

    public bool AddUser(
        string firstName, 
        string lastName, 
        string username,
        string rawPassword,  // Raw password input
        string matricule,
        long codeNIN,
        long id)
    {
        // 1. Check if username exists
        if (_usersDatabase.GetUserByUsername(username) != null)
        {
            Console.WriteLine("Username already exists!");
            return false;
        }

        // 2. Hash password
        string hashedPassword = HashPassword(rawPassword);

        // 3. Create user object
        var newUser = new UserAccount(
            firstName: firstName,
            lastName: lastName,
            username: username,
            password: hashedPassword,  // Store only the hash
            matricule: matricule,
            codeNIN: codeNIN,
            id: id
        );

        // 4. Add to database
        _usersDatabase.AddUser(newUser);
        Console.WriteLine($"User {username} created successfully!");
        return true;
    }

    private string HashPassword(string password)
    {
        using SHA256 sha256 = SHA256.Create();
        byte[] hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
        return BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
    }
}