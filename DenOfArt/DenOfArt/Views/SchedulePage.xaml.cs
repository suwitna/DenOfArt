using Syncfusion.SfSchedule.XForms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DenOfArt.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SchedulePage : ContentPage
    {
        public SchedulePage()
        {
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("MjIyOTkzQDMxMzcyZTM0MmUzMEFiRHhQTms2NTJySzBzZ1dhM2xhTml0RVJxTVBwZ0QrWHVMVjBZblNSMUk9");
            InitializeComponent();

            ScheduleAppointmentCollection appointmentCollection = new ScheduleAppointmentCollection();
            //Creating new event   
            ScheduleAppointment clientMeeting = new ScheduleAppointment();
            DateTime currentDate = DateTime.Now;
            DateTime startTime = new DateTime(currentDate.Year, currentDate.Month, currentDate.Day+3, 10, 0, 0);
            DateTime endTime = new DateTime(currentDate.Year, currentDate.Month, currentDate.Day+3, 11, 0, 0);
            clientMeeting.StartTime = startTime;
            clientMeeting.EndTime = endTime;
            clientMeeting.Color = Color.FromHex("#e1917f");
            clientMeeting.Subject = "ขูดหินปูน";
            appointmentCollection.Add(clientMeeting);

            clientMeeting = new ScheduleAppointment();
            currentDate = DateTime.Now;
            startTime = new DateTime(currentDate.Year, currentDate.Month, currentDate.Day + 3, 13, 0, 0);
            endTime = new DateTime(currentDate.Year, currentDate.Month, currentDate.Day + 3, 14, 0, 0);
            clientMeeting.StartTime = startTime;
            clientMeeting.EndTime = endTime;
            clientMeeting.Color = Color.FromHex("#20B2AA");
            clientMeeting.Subject = "รักษารากฟันครั้งที่ 1";
            appointmentCollection.Add(clientMeeting);

            clientMeeting = new ScheduleAppointment();
            currentDate = DateTime.Now;
            startTime = new DateTime(currentDate.Year, currentDate.Month, currentDate.Day + 10, 9, 0, 0);
            endTime = new DateTime(currentDate.Year, currentDate.Month, currentDate.Day + 10, 10, 0, 0);
            clientMeeting.StartTime = startTime;
            clientMeeting.EndTime = endTime;
            clientMeeting.Color = Color.FromHex("#20B2AA");
            clientMeeting.Subject = "รักษารากฟันครั้งที่ 2";
            appointmentCollection.Add(clientMeeting);
            schedule.DataSource = appointmentCollection;
        }
    }
}