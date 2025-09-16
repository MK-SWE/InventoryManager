using Inventory.Application.Common.Interfaces;

namespace Inventory.Application.StockReservation.ValueObjects;

public class ReserveStockReference : IReservationResult
{
    public Guid ReserveId { get; }
    public bool IsSuccess { get; }
    public DateTime? ExpireDate { get; }
    public string? ErrorMessage { get; }
    
    public ReserveStockReference(Guid reserveId, bool isSuccess, DateTime? expireDate, string? errorMessage)
    {
        ReserveId = reserveId;
        IsSuccess = isSuccess;
        ExpireDate = expireDate;
        ErrorMessage = errorMessage;
    }
}