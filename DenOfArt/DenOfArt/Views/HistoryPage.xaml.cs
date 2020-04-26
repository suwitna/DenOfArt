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
            popupLoadingView.IsVisible = true;
            activityIndicator.IsRunning = true;
            await GetHistoryData();
        }
        public async Task GetHistoryData()
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
                        foreach (var data in Data)
                        {
                            if (data.IsApprove == "Y" && (data.IsCancel == "Y" || data.IsTreat == "Y"))
                            {
                                AppointmentView view = new AppointmentView();
                                view.HN = "หมายเลข HN: " + data.HN;
                                view.AppointmentDate = data.AppointmentDate;
                                view.AppointmentTime = data.AppointmentTime;
                                view.Subject = data.Subject;
                                view.CustomerName = data.CustomerName;
                                view.Reason = "หมายเหตุ : " + data.TreatDetail;
                                view.Status = data.Status;
                                view.ImgAcceptReject = "accept";

                                listHist.Add(view);
                            }
                            else
                            {
                                AppointmentView view = new AppointmentView();
                                view.HN = "หมายเลข HN: " + data.HN;
                                view.AppointmentDate = data.AppointmentDate;
                                view.AppointmentTime = data.AppointmentTime;
                                view.Subject = data.Subject;
                                view.CustomerName = data.CustomerName;
                                view.Status = data.Status;
                                view.ImgAcceptReject = "waiting";

                                if(data.IsApprove == "Y")
                                {
                                    view.ImgAcceptReject = "accept";
                                }
                                else if (data.IsPostpone == "Y")
                                {
                                    view.CustomerName = "เลื่อนเป็นวันที่ " + data.PostponeDate+ " "+ data.PostponeTime;
                                    view.Subject = data.PostponeReason;

                                    view.ImgAcceptReject = "waiting";
                                }
                                else if (data.IsCancel == "Y")
                                {
                                    view.CustomerName = "ยกเลิกนัด";

                                    view.Subject = data.CancelReason;

                                    view.ImgAcceptReject = "waiting";
                                }

                                listAppr.Add(view);

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

            Device.StartTimer(TimeSpan.FromSeconds(1), () => {
                popupLoadingView.IsVisible = false;
                activityIndicator.IsRunning = false;
                return true;
            });
        }
    }
}