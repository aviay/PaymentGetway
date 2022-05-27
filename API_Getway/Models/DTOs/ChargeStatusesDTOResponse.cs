namespace API_Getway.Models.DTOs
{
    public class ChargeStatusesDTOResponse
    {
        public List<DeclineReason> Reasons;
        public ChargeStatusesDTOResponse(IEnumerable<Charge> result)
        {
            Reasons = new List<DeclineReason>();
            if(result != null)
            {
                foreach (var item in result)
                {
                    Reasons.Add(new DeclineReason
                    {
                        Reason = item.Reason,
                        Count = item.Count
                    });
                }
            }
        }
    }

    public class DeclineReason
    {
        public string Reason { get; set; }
        public int Count{ get; set; }

    }
}
