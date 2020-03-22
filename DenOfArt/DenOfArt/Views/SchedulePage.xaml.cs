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
            clientMeeting.Color = Color.FromHex("#D09292");
            clientMeeting.Subject = "ขูดหินปูน";
            appointmentCollection.Add(clientMeeting);

            clientMeeting = new ScheduleAppointment();
            currentDate = DateTime.Now;
            startTime = new DateTime(currentDate.Year, currentDate.Month, currentDate.Day + 3, 13, 0, 0);
            endTime = new DateTime(currentDate.Year, currentDate.Month, currentDate.Day + 3, 14, 0, 0);
            clientMeeting.StartTime = startTime;
            clientMeeting.EndTime = endTime;
            clientMeeting.Color = Color.FromHex("#C094CC");
            clientMeeting.Subject = "รักษารากฟันครั้งที่ 1";
            appointmentCollection.Add(clientMeeting);

            clientMeeting = new ScheduleAppointment();
            currentDate = DateTime.Now;
            startTime = new DateTime(currentDate.Year, currentDate.Month+1, currentDate.Day, 9, 0, 0);
            endTime = new DateTime(currentDate.Year, currentDate.Month+1, currentDate.Day, 10, 0, 0);
            clientMeeting.StartTime = startTime;
            clientMeeting.EndTime = endTime;
            clientMeeting.Color = Color.FromHex("#D09292");
            clientMeeting.Subject = "รักษารากฟันครั้งที่ 2";
            appointmentCollection.Add(clientMeeting);
            schedule.DataSource = appointmentCollection;

            popupLoadingView.IsVisible = true;
            activityIndicator.IsRunning = true;

            Device.StartTimer(TimeSpan.FromSeconds(2), () => {
                popupLoadingView.IsVisible = false;
                activityIndicator.IsRunning = false;
                return true;
            });
        }

        protected override bool OnBackButtonPressed()
        {
            Device.BeginInvokeOnMainThread(async () => {
                var result = await DisplayAlert("ออกจากแอพพลิเคชั่น", "ท่านกำลังออกจากระบบ โปรดยืนยัน?", "ตกลง", "ยกเลิก");
                if (result)
                {
                    // await this.Navigation.PopAsync(); // or anything else
                    if (Device.RuntimePlatform == Device.Android)
                    {
                        Android.OS.Process.KillProcess(Android.OS.Process.MyPid());
                    }
                }
            });

            return true;
        }
    }
}