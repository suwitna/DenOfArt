using Android.Content;
using Android.Widget;
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
        Context context;
        APIRequestHelper apiRequestHelper;
        IMyAPI myAPI;
        bool isAppointmentDate = false;
        public SchedulePage()
        {
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("MjIyOTkzQDMxMzcyZTM0MmUzMEFiRHhQTms2NTJySzBzZ1dhM2xhTml0RVJxTVBwZ0QrWHVMVjBZblNSMUk9");
            InitializeComponent();
            popupLoadingView.IsVisible = true;
            activityIndicator.IsRunning = true;

            var currentContext = Android.App.Application.Context;
            this.context = currentContext;
            myAPI = RestService.For<IMyAPI>(App._apiURL.ToString());
            apiRequestHelper = new APIRequestHelper(currentContext, myAPI);

            AddAppointment.Clicked += AddAppointment_Clicked;
            CancelAppointment.Clicked += CancelAppointment_Clicked;
            schedule.CellTapped += Schedule_CellTapped;
            LoadAppointment();
        }

        private void Schedule_CellTapped(object sender, CellTappedEventArgs e)
        {
            isAppointmentDate = false;
            List <Object> appointments = e.Appointments;
            if(appointments != null && appointments.Count > 0)
            {
                foreach(var appointment in appointments)
                {
                    var data = appointment as ScheduleAppointment;
                    DateTime start = data.StartTime;
                    string subject = data.Subject;

                    isAppointmentDate = true;
                    string appdate = start.ToString("dd/MM/yyyy");
                    string appTime = start.ToString("hh:mm");

                    if (Application.Current.Properties.ContainsKey("APP_DATE"))
                    {
                        Application.Current.Properties["APP_DATE"] = appdate;
                    }
                    else 
                    {
                        Application.Current.Properties.Add("APP_DATE", appdate);
                    }

                    if (Application.Current.Properties.ContainsKey("APP_TIME"))
                    {
                        Application.Current.Properties["APP_TIME"] = appTime;
                    }
                    else
                    {
                        Application.Current.Properties.Add("APP_TIME", appTime);
                    }

                    if (Application.Current.Properties.ContainsKey("APP_SUB"))
                    {
                        Application.Current.Properties["APP_SUB"] = subject;
                    }
                    else
                    {
                        Application.Current.Properties.Add("APP_SUB", subject);
                    }
                    return;
                }
            }
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
            if (!isAppointmentDate)
            {
                Toast.MakeText(context, "เลือกวันที่มีการนัดหมายก่อน!", ToastLength.Short).Show();
                return;
            }
            await Navigation.PushAsync(new AppointmentPage());
        }

        private async void CancelAppointment_Clicked(object sender, EventArgs e)
        {
            if (!isAppointmentDate)
            {
                Toast.MakeText(context, "เลือกวันที่มีการนัดหมายก่อน!", ToastLength.Short).Show();
                return;
            }
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