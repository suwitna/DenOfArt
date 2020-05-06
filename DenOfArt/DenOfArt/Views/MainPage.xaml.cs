using DenOfArt.MenuItems;
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
    public partial class MainPage : MasterDetailPage
    {
        public MainPage()
        {
            NavigationPage.SetHasNavigationBar(this, false);
            InitializeComponent();
            MasterPage.ListView.ItemSelected += ListView_ItemSelected;

            MasterPageItem item = new MasterPageItem();
            item.TargetType = typeof(SchedulePage);
            item.Title = "ข้อมูลการนัดหมาย";
            item.Icon = "date_b.png";

            var page = (Page)Activator.CreateInstance(item.TargetType);
            page.Title = item.Title;
            page.IconImageSource = item.Icon;

            Detail = new NavigationPage(page);
            IsPresented = false;

            MasterPage.ListView.SelectedItem = null;
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

        private async void ListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var item = e.SelectedItem as MainPageMasterMenuItem;
            if (item == null)
                return;

            if (item.Id == 4)
            {
                Device.BeginInvokeOnMainThread(async () => {
                    var result = await this.DisplayAlert(null, "ต้องการออกจากระบบ?", "ตกลง", "ยกเลิก");

                    if (result)
                    {
                        Application.Current.Properties.Clear();
                        var page = (Page)Activator.CreateInstance(typeof(LoginPage));

                        Detail = new NavigationPage(page);
                        IsPresented = false;

                        MasterPage.ListView.SelectedItem = null;
                    }
                    else
                    {
                        MasterPage.ListView.SelectedItem = null;
                    }
                });
            }
            else 
            { 
                var page = (Page)Activator.CreateInstance(item.TargetType);
                page.Title = item.Title;

                Detail = new NavigationPage(page);
                IsPresented = false;

                MasterPage.ListView.SelectedItem = null;
            }
        }
    }
}