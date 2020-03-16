using DenOfArt.Tables;
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
        public LoginPage()
        {
            NavigationPage.SetHasNavigationBar(this, false);
            InitializeComponent();

            Application.Current.Properties.Clear();
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
        }



    }
}