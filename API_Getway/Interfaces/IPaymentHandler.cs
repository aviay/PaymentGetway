namespace API_Getway.Interfaces
{
    public interface IPaymentHandler
    {
        IPaymentPrivder ExecuteCreation(string paymentProvider);
    }
}
