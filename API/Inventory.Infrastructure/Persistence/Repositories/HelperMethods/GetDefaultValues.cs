namespace Inventory.Infrastructure.Persistence.Repositories.HelperMethods;

public static class GetDefaultValues
{
    public static object? GetValue(Type type)
    {
        return type.IsValueType ? Activator.CreateInstance(type) : null;
    }
}