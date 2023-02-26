namespace BeautySaloon.Common;

public record Constants
{
    public record Roles
    {
        public const string Admin = "Admin";

        public const string Employee = "Employee";

        public const string AdminAndEmployee = Admin + "," + Employee;
    }
}
