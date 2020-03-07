using DenOfArt.Views;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DenOfArt
{
    public partial class App : Application
    {
        public App()
        {
            NavigationPage.SetHasNavigationBar(this, false);
            InitializeComponent();
            MainPage = new NavigationPage(new LoginPage());
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
