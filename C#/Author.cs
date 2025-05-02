public class Author
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public long ID { get; set; }

    public Author(string firstName, string lastName, long id )
    {
        FirstName = firstName;
        LastName = lastName;
        ID = id;
    }
}