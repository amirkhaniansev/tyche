using System;

namespace Tyche.TycheDAL.Models
{
    public class BlockedIP
    {
        public int Id { get; set; }

        public string IPAddress { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public int ReasonId { get; set; }
    }
}