using Application.Features.Payments.Constants;
using Application.Services.Repositories;
using NArchitecture.Core.Application.Rules;
using NArchitecture.Core.CrossCuttingConcerns.Exception.Types;
using NArchitecture.Core.Localization.Abstraction;
using Domain.Entities;
using Shared.Enums;

namespace Application.Features.Payments.Rules;

public class PaymentBusinessRules : BaseBusinessRules
{
    private readonly IPaymentRepository _paymentRepository;
    private readonly IBookingRepository _bookingRepository; // EKLENDÝ
    private readonly ILocalizationService _localizationService;

    public PaymentBusinessRules(
        IPaymentRepository paymentRepository,
        IBookingRepository bookingRepository, // EKLENDÝ
        ILocalizationService localizationService)
    {
        _paymentRepository = paymentRepository;
        _bookingRepository = bookingRepository; // EKLENDÝ
        _localizationService = localizationService;
    }

    private async Task throwBusinessException(string messageKey)
    {
        string message = await _localizationService.GetLocalizedAsync(messageKey, PaymentsBusinessMessages.SectionName);
        throw new BusinessException(message);
    }

    // --- MEVCUT KURALLAR ---
    public async Task PaymentShouldExistWhenSelected(Payment? payment)
    {
        if (payment == null)
            await throwBusinessException(PaymentsBusinessMessages.PaymentNotExists);
    }

    public async Task PaymentIdShouldExistWhenSelected(Guid id, CancellationToken cancellationToken)
    {
        Payment? payment = await _paymentRepository.GetAsync(
            predicate: p => p.Id == id,
            enableTracking: false,
            cancellationToken: cancellationToken
        );
        await PaymentShouldExistWhenSelected(payment);
    }

    public async Task TransactionIdShouldNotExists(string transactionId, CancellationToken cancellationToken)
    {
        bool doesExist = await _paymentRepository.AnyAsync(
            predicate: p => p.TransactionId == transactionId,
            cancellationToken: cancellationToken
        );

        if (doesExist)
            await throwBusinessException(PaymentsBusinessMessages.TransactionIdAlreadyExists);
    }

    public async Task BookingIdShouldExist(Guid bookingId, CancellationToken cancellationToken)
    {
        bool doesExist = await _bookingRepository.AnyAsync(
            predicate: b => b.Id == bookingId,
            cancellationToken: cancellationToken
        );

        if (!doesExist)
            await throwBusinessException(PaymentsBusinessMessages.BookingNotExists);
    }

    // YENÝ: Güncelleme sýrasýnda TransactionId benzersizliði (Kendi ID'si hariç)
    public async Task TransactionIdShouldNotExistsWhenUpdate(Guid id, string transactionId, CancellationToken cancellationToken)
    {
        bool doesExist = await _paymentRepository.AnyAsync(
            predicate: p => p.Id != id && p.TransactionId == transactionId,
            cancellationToken: cancellationToken
        );

        if (doesExist)
            await throwBusinessException(PaymentsBusinessMessages.TransactionIdAlreadyExists);
    }

    // YENÝ: Tamamlanmýþ ödeme güncellenemez (Opsiyonel Kural Örneði)
    public async Task PaymentShouldNotBeCompleted(Payment payment)
    {
        if (payment.Status == PaymentStatus.Completed)
            await throwBusinessException(PaymentsBusinessMessages.PaymentAlreadyCompleted);
    }

    public Task PaymentShouldNotBeCompletedOrRefunded(Payment payment)
    {
        // Eðer ödeme "Tamamlandý" veya "Ýade Edildi" ise silinemez!
        if (payment.Status == PaymentStatus.Completed || payment.Status == PaymentStatus.Refunded)
        {
            // Bu bir async iþlem gerektirmediði için Task.CompletedTask dönebiliriz 
            // veya exception fýrlattýðýmýz için direkt throw ederiz.
            return throwBusinessException(PaymentsBusinessMessages.PaymentCannotBeDeleted);
        }
        return Task.CompletedTask;
    }
}