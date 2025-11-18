using NArchitecture.Core.Application.Dtos;
using Domain.ValueObjects;
using Shared.Enums;

namespace Application.Features.Payments.Queries.GetList;

public class GetListPaymentListItemDto : IDto
{
    public Guid Id { get; set; }
    public Guid BookingId { get; set; }
    public Money Amount { get; set; }
    public DateTime Date { get; set; }
    public string TransactionId { get; set; }
    public PaymentStatus Status { get; set; }
}