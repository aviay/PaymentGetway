using API_Getway.Interfaces;
using API_Getway.Models;

namespace API_Getway.DAL
{
    public class DbConnector : IDbConnector
    {
        private readonly PaymentContext _dbContext;

        public DbConnector(PaymentContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void AddDeclineReasonToDb(string merchantId, string resultReason)
        {
            var merchant = new Merchant { Id = merchantId };
            var dbMerchant = _dbContext.Merchants.FirstOrDefault(x => x.Id == merchantId );
            if(dbMerchant == null)
            {
                _dbContext.Merchants.Add(merchant);
            }

            var dbCharge = _dbContext.Charges.FirstOrDefault(x => x.MerchantId == merchantId && x.Reason == resultReason);
            if (dbCharge == null)
            {
                var charge = new Charge
                {
                    Id = Guid.NewGuid().ToString(),
                    MerchantId = merchant.Id,
                    Reason = resultReason,
                    Count = 1
                };
                _dbContext.Charges.Add(charge);
            }
            else
            {
                dbCharge.Count++;
            }

            _dbContext.SaveChanges();
        }

        public IEnumerable<Charge> GetChargeStatusesFromDb(string merchantId)
        {
            var dbCharges = _dbContext.Charges.Where(x => x.MerchantId == merchantId).ToList();
            return dbCharges;
        }
    }
}
