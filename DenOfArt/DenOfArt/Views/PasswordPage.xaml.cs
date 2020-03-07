using DenOfArt.Tables;
using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DenOfArt.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PasswordPage : ContentPage
    {
        public PasswordPage()
        {
            InitializeComponent();
        }

        async void Request_Clicked(object sender, EventArgs e)
        {
            var email = EntryEmail.Text;
            var dbpath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "UserDatabase.db");
            var db = new SQLiteConnection(dbpath);
            var myquery = db.Table<RegUserTable>().Where(u => u.Email.Equals(email)).FirstOrDefault();

            if (myquery != null)
            {
                var send = 0;
                var username = myquery.UserName;
                var password = myquery.Password;

                string subject;
                string body;
                List<string> recipients;

                
                subject = "Recovery your password";
                body = "Password:" + password;
                recipients = new List<string>();
                recipients.Add(email);

                await SendEmail(subject, body, recipients);

                if (send == 0)
                {
                    Device.BeginInvokeOnMainThread(async () =>
                    {
                        await this.DisplayAlert("Result", "System sent you information to your email successfull.", null, "OK");
                        await Navigation.PushAsync(new LoginPage());
                    });
                }
            }
            else 
            { 
                Device.BeginInvokeOnMainThread(async () => {
                    await this.DisplayAlert("Error", "The email you are looking was not found.", null, "OK");
                });
            }
        }

        public async Task SendEmail(string subject, string body, List<string> recipients)
        {
            try
            {
                var message = new EmailMessage
                {
                    Subject = subject,
                    Body = body,
                    To = recipients,
                };
                await Email.ComposeAsync(message);
            }
            catch (FeatureNotSupportedException fbsEx)
            {
                // Email is not supported on this device  
            }
            catch (Exception ex)
            {
                // Some other exception occurred  
            }
        }
    }
}