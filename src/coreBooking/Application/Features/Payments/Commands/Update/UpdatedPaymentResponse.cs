using NArchitecture.Core.Application.Responses;
using Domain.ValueObjects;
using Shared.Enums;

namespace Application.Features.Payments.Commands.Update;

public class UpdatedPaymentResponse : IResponse
{
    public Guid Id { get; set; }
    public Guid BookingId { get; set; }

    // Flat Output
    public decimal AmountValue { get; set; }
    public Currency AmountCurrency { get; set; }

    public DateTime Date { get; set; }
    public string TransactionId { get; set; }
    public PaymentStatus Status { get; set; }
}