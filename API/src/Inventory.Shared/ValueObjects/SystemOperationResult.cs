using Inventory.Shared.Enums;

namespace Inventory.Shared.ValueObjects;

public record SystemOperationResult
{
    public required string OperationStatus { get; init; } = nameof(SystemOperationStatus.NotStarted);
    public required string Result { get; init; }
    public string? ErrorMessage { get; init; } = string.Empty;
    public object? Payload { get; init; } = null;
    
    public SystemOperationResult Create(string result, SystemOperationStatus status, string? errorMessage) =>
        new()
        {
            OperationStatus = status.ToString(),
            Result = result,
            ErrorMessage = errorMessage
        };
};

public sealed record CreateEntityResult: SystemOperationResult
{
    public required int NewEntityId { get; init; }
}