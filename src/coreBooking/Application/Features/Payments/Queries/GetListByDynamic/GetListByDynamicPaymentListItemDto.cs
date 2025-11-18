using Domain.ValueObjects;
using NArchitecture.Core.Application.Dtos;
using Shared.Enums;

namespace Application.Features.Payments.Queries.GetListByDynamic;

public class GetListByDynamicPaymentListItemDto : IDto
{
    public Guid Id { get; set; } // Genellikle Id de listelerde istenir
    public Guid BookingId { get; set; }

    // --- V2: Flat Output ---
    public decimal AmountValue { get; set; }
    public Currency AmountCurrency { get; set; }

    public DateTime Date { get; set; }
    public string TransactionId { get; set; }
    public PaymentStatus Status { get; set; }
}