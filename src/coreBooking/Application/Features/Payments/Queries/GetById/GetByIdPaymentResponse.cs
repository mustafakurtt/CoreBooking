using NArchitecture.Core.Application.Responses;
using Domain.ValueObjects;
using Shared.Enums;

namespace Application.Features.Payments.Queries.GetById;

public class GetByIdPaymentResponse : IResponse
{
    public Guid Id { get; set; }
    public Guid BookingId { get; set; }
    public Money Amount { get; set; }
    public DateTime Date { get; set; }
    public string TransactionId { get; set; }
    public PaymentStatus Status { get; set; }
}