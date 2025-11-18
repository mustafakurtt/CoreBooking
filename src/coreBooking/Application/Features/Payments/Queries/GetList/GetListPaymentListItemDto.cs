using NArchitecture.Core.Application.Dtos;
using Domain.ValueObjects;
using Shared.Enums;

namespace Application.Features.Payments.Queries.GetList;

public class GetListPaymentListItemDto : IDto
{
    public Guid Id { get; set; }
    public Guid BookingId { get; set; }

    // --- V2: Flat Output ---
    public decimal AmountValue { get; set; }
    public Currency AmountCurrency { get; set; }

    public DateTime Date { get; set; }
    public string TransactionId { get; set; }
    public PaymentStatus Status { get; set; }
}