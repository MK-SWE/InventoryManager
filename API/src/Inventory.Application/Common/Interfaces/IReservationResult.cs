namespace Inventory.Application.Common.Interfaces;

public interface IReservationResult
{
    bool IsSuccess { get; }
    string? ErrorMessage { get; }
}