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
        }

        private async void ListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var item = e.SelectedItem as MainPageMasterMenuItem;
            if (item == null)
                return;

            if (item.Id == 5)
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