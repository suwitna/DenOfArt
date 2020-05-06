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
    public partial class AppointmentCancelPage : ContentPage
    {
        Context context;
        APIRequestHelper apiRequestHelper;
        IMyAPI myAPI;
        public AppointmentCancelPage()
        {
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("MjIyOTkzQDMxMzcyZTM0MmUzMEFiRHhQTms2NTJySzBzZ1dhM2xhTml0RVJxTVBwZ0QrWHVMVjBZblNSMUk9");
            InitializeComponent();
            var currentContext = Android.App.Application.Context;
            this.context = currentContext;
            myAPI = RestService.For<IMyAPI>(App._apiURL.ToString());
            apiRequestHelper = new APIRequestHelper(currentContext, myAPI);

            //ImageTabFrom.Tapped += ImageTabFrom_Tapped;
            CancelAppointment.Clicked += CancelAppointment_Clicked;

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

        private void ImageTabFrom_Tapped(object sender, EventArgs e)
        {
            AppointmentDateFrom.Focus();
        }

        private async void CancelAppointment_Clicked(object sender, EventArgs e)
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
                        json.IsCancel = "Y";
                        json.CancelReason = Subject.Text.Trim();
                        json.Status = "รอการยืนยัน";

                        await apiRequestHelper.RequestUpdateAppointmentAsync(json);
                        Toast.MakeText(context, "บันทึกข้อมูลสำเร็จ", ToastLength.Short).Show();
                        List<Page> li = Navigation.NavigationStack.ToList();
                        Page last = li.ElementAt(li.Count - 1);
                        Navigation.RemovePage(last);
                    }
                    else
                    {
                        Toast.MakeText(context, "ไม่พบข้อมูลนัดหมาย วัน" + json.AppointmentDate + " เวลา: " + json.AppointmentTime + " !", ToastLength.Short).Show();
                    }
                });
            }
        }
    }
}