using System;
using System.Collections.Generic;
using System.Text;

namespace DenOfArt.Model
{
    public class Appointment
    {
        public string Key { get; set; }
        public string UserName { get; set; }
        public string HN { get; set; }
        public string CustomerName { get; set; }
        public string Subject { get; set; }
        public DateTime AppointmentDate { get; set; }
        public DateTime AppointmentTime { get; set; }
        public bool IsApprove { get; set; }
        public bool IsTreat { get; set; }
        public string TreatBy { get; set; }
        public string TreatDetail { get; set; }
        public DateTime TreatDate { get; set; }
        public DateTime TreatTime { get; set; }
        public bool IsCancel { get; set; }
        public string CancelReason { get; set; }
        public bool IsPostpone { get; set; }
        public string PostponeReason { get; set; }
        public string CreateBy { get; set; }
        public DateTime CreateDate { get; set; }
        public string UpdateBy { get; set; }
        public DateTime UpdateDate { get; set; }
    }
}
