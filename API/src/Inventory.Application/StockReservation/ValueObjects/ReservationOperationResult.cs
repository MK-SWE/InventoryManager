using Inventory.Application.Common.Interfaces;

namespace Inventory.Application.StockReservation.ValueObjects;

public class ReservationOperationResult : IReservationResult
{
    public bool IsSuccess { get; }
    public string? ErrorMessage { get; }
    
    public ReservationOperationResult(bool isSuccess, string? errorMessage= null)
    {
        IsSuccess = isSuccess;
        ErrorMessage = errorMessage;
    }
}