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
            var myquery = db.Table<RegUserTable>().Where(u => u.UserName.Equals(EntryUser.Text) && u.Password.Equals(EntryPassword.Text)).FirstOrDefault();
            
            if (myquery != null)
            {
                Application.Current.Properties.Add("USER_NAME", myquery.UserName);
                App.Current.MainPage = new MainPage();
            }
            else
            {
                Device.BeginInvokeOnMainThread(async () => {
                    var result = await this.DisplayAlert("Error", "Failed User Name or Password", "Yes", "Cancel");

                    if (result)
                    {
                        await Navigation.PushAsync(new LoginPage());
                    }
                    else
                    {
                        await Navigation.PushAsync(new LoginPage());
                    }
                });
            }
        }



    }
}