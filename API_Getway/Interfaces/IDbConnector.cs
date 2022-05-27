using API_Getway.Models;

namespace API_Getway.Interfaces
{
    public interface IDbConnector
    {
        void AddDeclineReasonToDb(string merchantId, string resultReason);
        IEnumerable<Charge> GetChargeStatusesFromDb(string merchantId);
    }
}
