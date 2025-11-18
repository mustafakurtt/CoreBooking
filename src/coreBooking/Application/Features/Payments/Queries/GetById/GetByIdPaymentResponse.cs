using NArchitecture.Core.Application.Responses;
using Domain.ValueObjects;
using Shared.Enums;

namespace Application.Features.Payments.Queries.GetById;

public class GetByIdPaymentResponse : IResponse
{
    public Guid Id { get; set; }
    public Guid BookingId { get; set; }

    // V2 GÜNCELLEMESÝ: Flat Output
    public decimal AmountValue { get; set; }
    public Currency AmountCurrency { get; set; }

    public DateTime Date { get; set; }
    public string TransactionId { get; set; }
    public PaymentStatus Status { get; set; }

    // EKSTRA: Ýliþkili Veriler (Ýsteðe baðlý ama faydalý)
    // public string CustomerName { get; set; } // Join ile getirilebilir
}