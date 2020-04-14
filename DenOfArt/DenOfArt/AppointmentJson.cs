using System;
using System.Collections.Generic;
using System.Text;

namespace DenOfArt
{
    public class AppointmentJson
    {
        public string UserName { get; set; }
        public string HN { get; set; }
        public string CustomerName { get; set; }
        public string Subject { get; set; }
        public string AppointmentDate { get; set; }
        public string AppointmentTime { get; set; }
        public string IsApprove { get; set; }
        public string IsTreat { get; set; }
        public string TreatBy { get; set; }
        public string TreatDetail { get; set; }
        public string TreatDate { get; set; }
        public string TreatTime { get; set; }
        public string IsCancel { get; set; }
        public string CancelReason { get; set; }
        public string IsPostpone { get; set; }
        public string PostponeReason { get; set; }
        public string CreateBy { get; set; }
        public string CreateDate { get; set; }
        public string UpdateBy { get; set; }
        public string UpdateDate { get; set; }
    }
}
