using DenOfArt.Views;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DenOfArt
{
    public partial class App : Application
    {
        public static string _apiURL { get; set; }
        public App()
        {
            NavigationPage.SetHasNavigationBar(this, false);
            App._apiURL = "https://mysterious-brook-25806.herokuapp.com/";
            //App._apiURL = "http://192.168.1.41:3000";
            InitializeComponent();
            MainPage = new NavigationPage(new LoginPage());
            //MainPage = new NavigationPage(new LocationTrackerPage());
            //MainPage = new NavigationPage(new APITestPage());
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
