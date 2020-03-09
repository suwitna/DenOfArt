using DenOfArt.Tables;
using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DenOfArt.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RegisterPage : ContentPage
    {
        public RegisterPage()
        {
            NavigationPage.SetHasNavigationBar(this, true);
            InitializeComponent();
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            var dbpath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "UserDatabase.db");
            var db = new SQLiteConnection(dbpath);
            db.CreateTable<RegUserTable>();

            var item = new RegUserTable()
            {
                UserName = EntryUserName.Text,
                Password = EntryUserPassword.Text,
                Email = EntryUserEmail.Text,
                PhoneNumber = EntryUserPhoneNumber.Text,
            };

            db.Insert(item);

            var profile = new ProfileTable()
            {
                UserName = EntryUserName.Text,
            };
            db.CreateTable<ProfileTable>();
            db.Insert(profile);

            Device.BeginInvokeOnMainThread(async () =>{
                var result = await this.DisplayAlert(null, "สมัครสมาชิกสำเร็จ!", null, "ตกลง");

                if (!result)
                {
                    await Navigation.PushAsync(new LoginPage());
                }
            }) ;
        }

        async void Cancel_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new LoginPage());
        }
    }
}