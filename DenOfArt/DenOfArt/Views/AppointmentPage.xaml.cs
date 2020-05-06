using Android.Content;
using Android.Widget;
using DenOfArt.API;
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
    public partial class AppointmentPage : ContentPage
    {
        Context context;
        APIRequestHelper apiRequestHelper;
        IMyAPI myAPI;
        public AppointmentPage()
        {
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("MjIyOTkzQDMxMzcyZTM0MmUzMEFiRHhQTms2NTJySzBzZ1dhM2xhTml0RVJxTVBwZ0QrWHVMVjBZblNSMUk9");
            InitializeComponent();
            var currentContext = Android.App.Application.Context;
            this.context = currentContext;
            myAPI = RestService.For<IMyAPI>(App._apiURL.ToString());
            apiRequestHelper = new APIRequestHelper(currentContext, myAPI);

            //ImageTabFrom.Tapped += ImageTabFrom_Tapped;
            ImageTabTo.Tapped += ImageTabTo_Tapped;
            SaveAppointment.Clicked += SaveAppointment_Clicked;

            var getDate = Application.Current.Properties["APP_DATE"] as string;
            var getTime = Application.Current.Properties["APP_TIME"] as string;
            var getSubject = Application.Current.Properties["APP_SUB"] as string;

            DateTime date = DateTime.ParseExact(getDate, "dd/MM/yyyy", null);
            TimeSpan time = TimeSpan.ParseExact(getTime, "hh\\:mm", null);

            AppointmentDateFrom.IsEnabled = false;
            AppointmentTimeFrom.IsEnabled = false;
            
            AppointmentDateFrom.Date = date;
            AppointmentTimeFrom.Time = time;
            lblSubject.Text = "เลื่อนนัด: " + getSubject;
        }

        private async void SaveAppointment_Clicked(object sender, EventArgs e)
        {
            var username = Application.Current.Properties["USER_NAME"] as string;
            if (username != null & username != "")
            {
                AppointmentJson json = new AppointmentJson();
                json.UserName = username;
                json.AppointmentDate = AppointmentDateFrom.Date.ToString("dd/MM/yyyy");
                json.AppointmentTime = AppointmentTimeFrom.Time.ToString(@"hh\:mm");

                Device.BeginInvokeOnMainThread(async () => {
                    string result = await apiRequestHelper.RequestCheckExistAppointmentAsync(json);
                    if (result == "true")
                    {
                        json.IsApprove = "";
                        json.IsPostpone = "Y";
                        json.PostponeDate = AppointmentDateTo.Date.ToString("dd/MM/yyyy");
                        json.PostponeTime = AppointmentTimeTo.Time.ToString(@"hh\:mm");
                        json.PostponeReason = Subject.Text.Trim();
                        json.Status = "รอการยืนยัน";

                        await apiRequestHelper.RequestUpdateAppointmentAsync(json);

                        Toast.MakeText(context, "บันทึกข้อมูลสำเร็จ", ToastLength.Short).Show();
                        List<Page> li = Navigation.NavigationStack.ToList();
                        Page last = li.ElementAt(li.Count - 1);
                        Navigation.RemovePage(last);
                    }
                    else{
                        Toast.MakeText(context, "ไม่พบข้อมูลนัดหมาย วัน" + json.AppointmentDate + " เวลา: " + json.AppointmentTime + " !", ToastLength.Short).Show();
                    }
                });
            }
        }

        private void ImageTabFrom_Tapped(object sender, EventArgs e)
        {
            AppointmentDateFrom.Focus();
        }

        private void ImageTabTo_Tapped(object sender, EventArgs e)
        {
            AppointmentDateTo.Focus();
        }
    }
}