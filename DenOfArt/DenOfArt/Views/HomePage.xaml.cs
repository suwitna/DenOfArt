using DenOfArt.API;
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
    public partial class HomePage : ContentPage
    {
        APIRequestHelper apiRequestHelper;
        IMyAPI myAPI;
        public HomePage()
        {
            InitializeComponent();
            var currentContext = Android.App.Application.Context;

            myAPI = RestService.For<IMyAPI>(App._apiURL.ToString());
            apiRequestHelper = new APIRequestHelper(currentContext, myAPI);

            popupLoadingView.IsVisible = true;
            activityIndicator.IsRunning = true;

            GetHistoryData();

        }

        public async Task GetHistoryData()
        {
            var username = Application.Current.Properties["USER_NAME"] as string;

            if (username != null && username != "")
            {
                List<AppointmentView> listAppr = new List<AppointmentView>();
                RootAppointmentObject appointmentData = await apiRequestHelper.RequestAllAppointmentAsync(username);
                if (appointmentData != null)
                {
                    List<AppointmentJson> Data = appointmentData.Data;
                    if (Data != null)
                    {
                        foreach (var data in Data)
                        {
                            if (data.IsApprove == "Y" || data.IsCancel == "Y" || data.IsTreat == "Y")
                            {

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

                                if (data.IsApprove == "" && data.IsCancel == "" && data.IsPostpone == "" && data.IsTreat == "")
                                {
                                    view.ImgAcceptReject = "waiting";
                                }
                                else if (data.IsApprove == "Y")
                                {
                                    view.ImgAcceptReject = "accept";
                                }
                                else if (data.IsPostpone == "Y")
                                {
                                    view.CustomerName = "เลื่อนเป็นวันที่ " + data.PostponeDate + " " + data.PostponeTime;
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