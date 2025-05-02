using System;

namespace DataAccessLayer.Exceptions
{
    // Base exception class for all DAL exceptions
    public class DataAccessException : Exception
    {
        public DataAccessException(string message) : base(message) { }
        public DataAccessException(string message, Exception innerException) : base(message, innerException) { }
    }

    // Database connection related exceptions
    public class DatabaseConnectionException : DataAccessException
    {
        public DatabaseConnectionException(string message) : base(message) { }
        public DatabaseConnectionException(string message, Exception innerException) : base(message, innerException) { }
    }

    // Constraint violation exceptions
    public class ConstraintViolationException : DataAccessException
    {
        public string ConstraintType { get; }
        public string FieldName { get; }

        public ConstraintViolationException(string message, string constraintType, string fieldName, Exception innerException)
            : base(message, innerException)
        {
            ConstraintType = constraintType;
            FieldName = fieldName;
        }
    }

    // Specific constraint violations
    public class UniqueConstraintViolationException : ConstraintViolationException
    {
        public UniqueConstraintViolationException(string fieldName, Exception innerException)
            : base($"A record with this {fieldName} already exists.", "UNIQUE", fieldName, innerException) { }
    }

    public class ForeignKeyConstraintViolationException : ConstraintViolationException
    {
        public ForeignKeyConstraintViolationException(string fieldName, Exception innerException)
            : base($"Referenced {fieldName} does not exist.", "FOREIGN KEY", fieldName, innerException) { }
    }

    public class NotNullConstraintViolationException : ConstraintViolationException
    {
        public NotNullConstraintViolationException(string fieldName, Exception innerException)
            : base($"{fieldName} cannot be null.", "NOT NULL", fieldName, innerException) { }
    }

    // Specific field violation exceptions for account creation
    public class DuplicateUsernameException : UniqueConstraintViolationException
    {
        public DuplicateUsernameException(Exception innerException)
            : base("username", innerException) { }
    }

    public class DuplicateEmailException : UniqueConstraintViolationException
    {
        public DuplicateEmailException(Exception innerException)
            : base("email", innerException) { }
    }

    public class DuplicateNINException : UniqueConstraintViolationException
    {
        public DuplicateNINException(Exception innerException)
            : base("code_NIN", innerException) { }
    }

    public class InvalidRoleException : ForeignKeyConstraintViolationException
    {
        public InvalidRoleException(Exception innerException)
            : base("role", innerException) { }
    }

    // Data format exceptions
    public class DataFormatException : DataAccessException
    {
        public DataFormatException(string message) : base(message) { }
        public DataFormatException(string message, Exception innerException) : base(message, innerException) { }
    }

    // Query execution exceptions
    public class QueryExecutionException : DataAccessException
    {
        public QueryExecutionException(string message) : base(message) { }
        public QueryExecutionException(string message, Exception innerException) : base(message, innerException) { }
    }
}