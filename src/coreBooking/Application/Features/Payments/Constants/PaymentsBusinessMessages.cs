namespace Application.Features.Payments.Constants;

public static class PaymentsBusinessMessages
{
    public const string SectionName = "Payments";

    public const string PaymentNotExists = "PaymentNotExists";
    public const string TransactionIdAlreadyExists = "TransactionIdAlreadyExists"; // YENÝ
    public const string BookingNotExists = "BookingNotExists"; // YENÝ
    public const string PaymentAlreadyCompleted = "PaymentAlreadyCompleted";
    public const string PaymentCannotBeDeleted = "PaymentCannotBeDeleted";
}