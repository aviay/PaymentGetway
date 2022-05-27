namespace API_Getway.Interfaces
{
    public interface IPaymentHandlerFactory
    {
        IPaymentPrivder Create();
    }
}
