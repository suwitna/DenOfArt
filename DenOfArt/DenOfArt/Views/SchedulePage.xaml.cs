using DenOfArt.API;
using DenOfArt.ViewModels;
using Refit;
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
        APIRequestHelper apiRequestHelper;
        IMyAPI myAPI;
        public SchedulePage()
        {
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("MjIyOTkzQDMxMzcyZTM0MmUzMEFiRHhQTms2NTJySzBzZ1dhM2xhTml0RVJxTVBwZ0QrWHVMVjBZblNSMUk9");
            InitializeComponent();
            popupLoadingView.IsVisible = true;
            activityIndicator.IsRunning = true;

            var currentContext = Android.App.Application.Context;

            myAPI = RestService.For<IMyAPI>(App._apiURL.ToString());
            apiRequestHelper = new APIRequestHelper(currentContext, myAPI);

            AddAppointment.Clicked += AddAppointment_Clicked;
            CancelAppointment.Clicked += CancelAppointment_Clicked;

            LoadAppointment();
        }

        public async void LoadAppointment() 
        {

            var username = Application.Current.Properties["USER_NAME"] as string;

            if (username != null && username != "")
            {
                
                List<AppointmentView> listAppr = new List<AppointmentView>();
                List<AppointmentView> listHist = new List<AppointmentView>();
                RootAppointmentObject appointmentData = await apiRequestHelper.RequestAllAppointmentAsync(username);
                if (appointmentData != null)
                {
                    List<AppointmentJson> Data = appointmentData.Data;
                    if (Data != null)
                    {
                        ScheduleAppointmentCollection appointmentCollection = new ScheduleAppointmentCollection();
                        foreach (var data in Data)
                        {
                            if (data.IsCancel == "" && data.IsTreat == "")
                            {
                                //Creating new event   
                                ScheduleAppointment clientMeeting = new ScheduleAppointment();
                                DateTime startTime = DateTime.ParseExact(data.AppointmentDate+" "+ data.AppointmentTime, "dd/MM/yyyy hh:mm", null);
                                clientMeeting.StartTime = startTime;
                                clientMeeting.Color = Color.FromHex("#3DBCFF");
                                clientMeeting.Subject = data.Subject;
                                appointmentCollection.Add(clientMeeting);
                            }
                        }
                        schedule.DataSource = appointmentCollection;
                        schedule.SelectedDate = DateTime.Now;
                    }
                }

                Device.StartTimer(TimeSpan.FromSeconds(1), () => {
                    popupLoadingView.IsVisible = false;
                    activityIndicator.IsRunning = false;
                    return true;
                });
            }
            /*
            ScheduleAppointmentCollection appointmentCollection = new ScheduleAppointmentCollection();
            //Creating new event   
            ScheduleAppointment clientMeeting = new ScheduleAppointment();
            DateTime currentDate = DateTime.Now;
            DateTime startTime = new DateTime(currentDate.Year, currentDate.Month, currentDate.Day + 3, 10, 0, 0);
            DateTime endTime = new DateTime(currentDate.Year, currentDate.Month, currentDate.Day + 3, 11, 0, 0);
            clientMeeting.StartTime = startTime;
            clientMeeting.EndTime = endTime;
            clientMeeting.Color = Color.FromHex("#D09292");
            clientMeeting.Subject = "ขูดหินปูน";
            appointmentCollection.Add(clientMeeting);

            clientMeeting = new ScheduleAppointment();
            currentDate = DateTime.Now;
            startTime = new DateTime(currentDate.Year, currentDate.Month, currentDate.Day, 13, 0, 0);
            endTime = new DateTime(currentDate.Year, currentDate.Month, currentDate.Day, 18, 0, 0);
            clientMeeting.StartTime = startTime;
            clientMeeting.EndTime = endTime;
            clientMeeting.Color = Color.FromHex("#C094CC");
            clientMeeting.Subject = "รักษารากฟันครั้งที่ 1";
            appointmentCollection.Add(clientMeeting);

            clientMeeting = new ScheduleAppointment();
            currentDate = DateTime.Now;
            startTime = new DateTime(currentDate.Year, currentDate.Month + 1, currentDate.Day, 9, 0, 0);
            endTime = new DateTime(currentDate.Year, currentDate.Month + 1, currentDate.Day, 10, 0, 0);
            clientMeeting.StartTime = startTime;
            clientMeeting.EndTime = endTime;
            clientMeeting.Color = Color.FromHex("#D09292");
            clientMeeting.Subject = "รักษารากฟันครั้งที่ 2";
            appointmentCollection.Add(clientMeeting);

            schedule.DataSource = appointmentCollection;
            schedule.SelectedDate = currentDate;

            popupLoadingView.IsVisible = true;
            activityIndicator.IsRunning = true;

            Device.StartTimer(TimeSpan.FromSeconds(2), () => {
                popupLoadingView.IsVisible = false;
                activityIndicator.IsRunning = false;
                return true;
            });
            */
        }
        private async void AddAppointment_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AppointmentPage());
        }

        private async void CancelAppointment_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AppointmentCancelPage());
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