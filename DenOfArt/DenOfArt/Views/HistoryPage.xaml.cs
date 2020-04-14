using DenOfArt.API;
using DenOfArt.Model;
using DenOfArt.ViewModels;
using Refit;
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
    public partial class HistoryPage : TabbedPage
    {
        APIRequestHelper apiRequestHelper;
        IMyAPI myAPI;
        public HistoryPage()
        {
            InitializeComponent();
            var currentContext = Android.App.Application.Context;

            myAPI = RestService.For<IMyAPI>(App._apiURL.ToString());
            apiRequestHelper = new APIRequestHelper(currentContext, myAPI);

            this.SelectedTabColor = Color.FromHex("#FFFFFF");
            this.UnselectedTabColor = Color.FromHex("#4C4C4C");
        }
        protected async override void OnAppearing()
        {

            base.OnAppearing();
            await GetHistoryData();
        }
        public async Task GetHistoryData()
        {
            var username = Application.Current.Properties["USER_NAME"] as string;

            if (username != null && username != "")
            {
                List<AppointmentView> listAppr = new List<AppointmentView>();
                List<AppointmentView> listHist = new List<AppointmentView>();
                RootAppointmentObject appointmentData = await apiRequestHelper.RequestAppointmentAsync(username);
                if (appointmentData != null)
                {
                    List<AppointmentJson> Data = appointmentData.Data;
                    if (Data != null)
                    {
                        foreach (var data in Data)
                        {
                            if (data.IsApprove != "Y")
                            {
                                AppointmentView view = new AppointmentView();
                                view.HN = "หมายเลข HN: " + data.HN;
                                view.AppointmentDate = data.AppointmentDate;
                                view.AppointmentTime = data.AppointmentTime;
                                view.Subject = data.Subject;
                                view.CustomerName = data.CustomerName;

                                listAppr.Add(view);
                            }
                            else if (data.IsApprove == "Y" && data.IsTreat == "Y")
                            {
                                AppointmentView view = new AppointmentView();
                                view.HN = "หมายเลข HN: " + data.HN;
                                view.AppointmentDate = data.AppointmentDate;
                                view.AppointmentTime = data.AppointmentTime;
                                view.Subject = data.Subject;
                                view.CustomerName = data.CustomerName;
                                view.Reason = "หมายเหตุ : " + data.TreatDetail;
                                view.Status = "รักษาแล้ว";

                                listHist.Add(view);
                            }
                        }
                    }
                }
                /*
                view = new AppointmentView();
                view.HN = "หมายเลข HN: " + "25630023";
                view.AppointmentDate = DateTime.Now.ToString("dd/MM/yyyy");
                view.AppointmentTime = DateTime.Now.ToString("hh:mm tt");
                view.Subject = "เลื่อน " + view.AppointmentDate + " เวลา " + view.AppointmentTime;
                view.CustomerName = "คุณสมชาย ใจดี";
                view.Reason = "หมายเหตุ : ไม่สบาย";
                view.Status = "ดำเนินการแล้ว";

                list.Add(view);

                view = new AppointmentView();
                view.HN = "หมายเลข HN: " + "25630023";
                view.AppointmentDate = DateTime.Now.ToString("dd/MM/yyyy");
                view.AppointmentTime = DateTime.Now.ToString("hh:mm tt");
                view.Subject = "เลื่อน " + view.AppointmentDate + " เวลา " + view.AppointmentTime;
                view.CustomerName = "คุณสมชาย ใจดี";
                view.Reason = "หมายเหตุ : ไม่สบาย";
                view.Status = "ดำเนินการแล้ว";

                list.Add(view);

                view = new AppointmentView();
                view.HN = "หมายเลข HN: " + "25630023";
                view.AppointmentDate = DateTime.Now.ToString("dd/MM/yyyy");
                view.AppointmentTime = DateTime.Now.ToString("hh:mm tt");
                view.Subject = "เลื่อน " + view.AppointmentDate + " เวลา " + view.AppointmentTime;
                view.CustomerName = "คุณสมชาย ใจดี";
                view.Reason = "หมายเหตุ : ไม่สบาย";
                view.Status = "ดำเนินการแล้ว";

                list.Add(view);

                view = new AppointmentView();
                view.HN = "หมายเลข HN: " + "25630023";
                view.AppointmentDate = DateTime.Now.ToString("dd/MM/yyyy");
                view.AppointmentTime = DateTime.Now.ToString("hh:mm tt");
                view.Subject = "เลื่อน " + view.AppointmentDate + " เวลา " + view.AppointmentTime;
                view.CustomerName = "คุณสมชาย ใจดี";
                view.Reason = "หมายเหตุ : ไม่สบาย";
                view.Status = "ดำเนินการแล้ว";

                list.Add(view);

                view = new AppointmentView();
                view.HN = "หมายเลข HN: " + "25630023";
                view.AppointmentDate = DateTime.Now.ToString("dd/MM/yyyy");
                view.AppointmentTime = DateTime.Now.ToString("hh:mm tt");
                view.Subject = "เลื่อน " + view.AppointmentDate + " เวลา " + view.AppointmentTime;
                view.CustomerName = "คุณสมชาย ใจดี";
                view.Reason = "หมายเหตุ : ไม่สบาย";
                view.Status = "ดำเนินการแล้ว";

                list.Add(view);

                view = new AppointmentView();
                view.HN = "หมายเลข HN: " + "25630023";
                view.AppointmentDate = DateTime.Now.ToString("dd/MM/yyyy");
                view.AppointmentTime = DateTime.Now.ToString("hh:mm tt");
                view.Subject = "เลื่อน " + view.AppointmentDate + " เวลา " + view.AppointmentTime;
                view.CustomerName = "คุณสมชาย ใจดี";
                view.Reason = "หมายเหตุ : ไม่สบาย";
                view.Status = "ดำเนินการแล้ว";

                list.Add(view);
                */
                listHistory.ItemsSource = listHist;
                listApprove.ItemsSource = listAppr;
            }
        }
    }
}