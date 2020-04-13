using Android.Content;
using DenOfArt.API;
using DenOfArt.Tables;
using Refit;
using SQLite;
using System;
using System.IO;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DenOfArt.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class APITestPage : ContentPage
    {
        APIRequestHelper apiRequestHelper;
        IMyAPI myAPI;

        public APITestPage()
        {
            InitializeComponent();
            btnLogin.Clicked += BtnLogin_Clicked;
            var currentContext = Android.App.Application.Context;

            myAPI = RestService.For<IMyAPI>(App._apiURL.ToString());
            apiRequestHelper = new APIRequestHelper(currentContext, myAPI);
        }

        private async void BtnLogin_Clicked(object sender, EventArgs e)
        {
            await apiRequestHelper.RequestLoginUserAsync(EntryUser.Text, EntryPassword.Text);
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

        async void ForgotPassword_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new PasswordPage());
        }
        async void Signup_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new RegisterPage());
        }
    }
}