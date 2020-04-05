using System;
using System.Collections.Generic;
using System.Text;

namespace DenOfArt.ViewModels
{
    public class AppointmentView
    {
        public string Key { get; set; }
        public string UserName { get; set; }
        public string HN { get; set; }
        public string CustomerName { get; set; }
        public string Subject { get; set; }
        public string AppointmentDate { get; set; }
        public string AppointmentTime { get; set; }
        public bool IsApprove { get; set; }
        public bool IsTreat { get; set; }
        public string TreatBy { get; set; }
        public string TreatDate { get; set; }
        public string TreatTime { get; set; }
        public bool IsCancel { get; set; }
        public bool IsPostpone { get; set; }
        public string Reason { get; set; }
        public string Status { get; set; }
        public string CreateBy { get; set; }
        public string CreateDate { get; set; }
        public string UpdateBy { get; set; }
        public string UpdateDate { get; set; }
    }
}
