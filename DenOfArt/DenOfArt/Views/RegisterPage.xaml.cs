using Android.Content;
using Android.Widget;
using DenOfArt.API;
using DenOfArt.Tables;
using Refit;
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
        Context context;
        APIRequestHelper apiRequestHelper;
        IMyAPI myAPI;
        public RegisterPage()
        {
            NavigationPage.SetHasNavigationBar(this, true);
            InitializeComponent();

            var currentContext = Android.App.Application.Context;
            this.context = currentContext;

            myAPI = RestService.For<IMyAPI>(App._apiURL.ToString());
            apiRequestHelper = new APIRequestHelper(currentContext, myAPI);
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            if (EntryUserName.Text == null)
            {
                //await this.DisplayAlert(null, "กรุณาระบุชื่อผู้ใช้งาน", null, "ตกลง");
                EntryUserName.PlaceholderColor = Color.FromHex("#ffb3ba");
                EntryUserName.Placeholder = "กรุณาระบุชื่อผู้ใช้งาน";
                EntryUserName.Focus();
                return;
            }

            if (EntryUserPassword.Text == null)
            {
                //await this.DisplayAlert(null, "กรุณาระบุรหัสผ่าน", null, "ตกลง");
                EntryUserPassword.PlaceholderColor = Color.FromHex("#ffb3ba");
                EntryUserPassword.Placeholder = "กรุณาระบุรหัสผ่าน";
                EntryUserPassword.Focus();
                return;
            }

            if (EntryUserEmail.Text == null)
            {
                //await this.DisplayAlert(null, "กรุณาระบุรหัสผ่าน", null, "ตกลง");
                EntryUserEmail.PlaceholderColor = Color.FromHex("#ffb3ba");
                EntryUserEmail.Placeholder = "กรุณาระบุอีเมล์";
                EntryUserEmail.Focus();
                return;
            }

            if (EntryUserPhoneNumber.Text == null)
            {
                //await this.DisplayAlert(null, "กรุณาระบุรหัสผ่าน", null, "ตกลง");
                EntryUserPhoneNumber.PlaceholderColor = Color.FromHex("#ffb3ba");
                EntryUserPhoneNumber.Placeholder = "กรุณาระบุเบอร์โทรศัพท์";
                EntryUserPhoneNumber.Focus();
                return;
            }

            Device.BeginInvokeOnMainThread(async () => {
                //Register data to firebase also
                var isExist = await apiRequestHelper.RequestCheckUserExistAsync(EntryUserName.Text);
                if (isExist == "true")
                {
                    Toast.MakeText(this.context, "ชื่อผู้ใช้งานซ้ำ", ToastLength.Short).Show();
                    EntryUserName.PlaceholderColor = Color.FromHex("#ffb3ba");
                    EntryUserName.Placeholder = "กรุณาระบุชื่อผู้ใช้งานใหม่";
                    EntryUserName.Focus();
                    return;
                }

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

            
                //Register data to firebase also
                await apiRequestHelper.RequestRegisterUserAsync(EntryUserName.Text, EntryUserPassword.Text, EntryUserEmail.Text, EntryUserPhoneNumber.Text);
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