using Android.Content;
using Android.Widget;
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
    public partial class LoginPage : ContentPage
    {
        Context context;
        APIRequestHelper apiRequestHelper;
        IMyAPI myAPI;

        public LoginPage()
        {
            NavigationPage.SetHasNavigationBar(this, false);
            InitializeComponent();

            Application.Current.Properties.Clear();

            var currentContext = Android.App.Application.Context;
            this.context = currentContext;

            myAPI = RestService.For<IMyAPI>(App._apiURL.ToString());
            apiRequestHelper = new APIRequestHelper(currentContext, myAPI);
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

        async void Login_Clicked(object sender, EventArgs e)
        {
            if (EntryUser.Text == null)
            {
                //await this.DisplayAlert(null, "กรุณาระบุชื่อผู้ใช้งาน", null, "ตกลง");
                EntryUser.PlaceholderColor = Color.FromHex("#ffb3ba");
                EntryUser.Placeholder = "กรุณาระบุชื่อผู้ใช้งาน";
                EntryUser.Focus();
                return;
            }

            if (EntryPassword.Text == null)
            {
                //await this.DisplayAlert(null, "กรุณาระบุรหัสผ่าน", null, "ตกลง");
                EntryPassword.PlaceholderColor = Color.FromHex("#ffb3ba");
                EntryPassword.Placeholder = "กรุณาระบุรหัสผ่าน";
                EntryPassword.Focus();
                return;
            }

            //Cloud database
            //Register data to firebase also
            var result = await apiRequestHelper.RequestLoginUserAsync(EntryUser.Text, EntryPassword.Text);
            if (result == "true")
            {
                var dbpath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "UserDatabase.db");
                var db = new SQLiteConnection(dbpath);
                var reguser = db.GetTableInfo("RegUserTable");
                if (reguser.Count == 0)
                {
                    RegUserJson user = await apiRequestHelper.RequestGetUserDataAsync(EntryUser.Text);

                    db.CreateTable<RegUserTable>();

                    var item = new RegUserTable()
                    {
                        UserName = user.UserName,
                        Password = user.Password,
                        Email = user.Email,
                        PhoneNumber = user.PhoneNumber,
                    };

                    db.Insert(item);
                }
                Toast.MakeText(context, "Login Successfull", ToastLength.Short).Show();
                Application.Current.Properties.Add("USER_NAME", EntryUser.Text);
                App.Current.MainPage = new MainPage();
            }
            else
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    var result2 = await this.DisplayAlert(null, "ข้อผิดพลาด! ไม่พบข้อมูลผู้ใช้งาน\nกรุณาตรวจสอบชื่อผู้ใช้งานและรหัสผ่าน", null, "ตกลง");

                    if (!result2)
                    {
                        EntryUser.Focus();
                    }
                });
            }
            //End of Cloud database


            //Local database
            /*
            var dbpath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "UserDatabase.db");
            var db = new SQLiteConnection(dbpath);
            db.CreateTable<RegUserTable>();
            var myquery = db.Table<RegUserTable>().Where(u => u.UserName.Equals(EntryUser.Text) && u.Password.Equals(EntryPassword.Text)).FirstOrDefault();
            
            if (myquery != null)
            {
                Application.Current.Properties.Add("USER_NAME", myquery.UserName);
                App.Current.MainPage = new MainPage();
            }
            else
            {
                Device.BeginInvokeOnMainThread(async () => {
                    if(EntryUser.Text == null)
                    {
                        //await this.DisplayAlert(null, "กรุณาระบุชื่อผู้ใช้งาน", null, "ตกลง");
                        EntryUser.PlaceholderColor = Color.FromHex("#ffb3ba");
                        EntryUser.Placeholder = "กรุณาระบุชื่อผู้ใช้งาน";
                        EntryUser.Focus();
                        return;
                    }

                    if (EntryPassword.Text == null)
                    {
                        //await this.DisplayAlert(null, "กรุณาระบุรหัสผ่าน", null, "ตกลง");
                        EntryPassword.PlaceholderColor = Color.FromHex("#ffb3ba");
                        EntryPassword.Placeholder = "กรุณาระบุรหัสผ่าน";
                        EntryPassword.Focus();
                        return;
                    }

                    var result = await this.DisplayAlert(null, "ข้อผิดพลาด! ไม่พบข้อมูลผู้ใช้งาน\nกรุณาตรวจสอบชื่อผู้ใช้งานและรหัสผ่าน", null, "ตกลง");

                    if (!result)
                    {
                        EntryUser.Focus();
                    }
                });

            }
           */
            //End of Local database
        }
    }
}