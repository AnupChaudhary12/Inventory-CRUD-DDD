namespace Inventory.Shared;

public static class AppMessages
{
    public static string SuccessMessage(string entityName) => $"{entityName} added successfully";
    public static string NullMessage(string objectName) => $"{objectName} is null or empty";
    public static string RetrieveSuccessMessage(string objectName) => $"{objectName} is retrieved successfully";

}
