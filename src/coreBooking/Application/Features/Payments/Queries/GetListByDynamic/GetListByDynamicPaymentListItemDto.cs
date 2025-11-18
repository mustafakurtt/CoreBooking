using Domain.ValueObjects;
using NArchitecture.Core.Application.Dtos;
using Shared.Enums;

namespace Application.Features.Payments.Queries.GetListByDynamic;

public class GetListByDynamicPaymentListItemDto : IDto
{
    public Guid BookingId { get; set; }
    public Money Amount { get; set; }
    public DateTime Date { get; set; }
    public string TransactionId { get; set; }
    public PaymentStatus Status { get; set; }
}
