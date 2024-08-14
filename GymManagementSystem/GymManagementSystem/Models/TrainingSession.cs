using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementSystem.Models
{
    public class TrainingSession
    {
        public int SessionID { get; set; }
        public int CustomerID { get; set; }
        public int TrainerID { get; set; }
        public TimeSpan SessionTiming { get; set; } // TimeSpan
        public DateTime TrainingStartDate { get; set; }
        public DateTime TrainingEndDate { get; set; }
        public string SessionStatus { get; set; }
    }

}
