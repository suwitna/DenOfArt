using DenOfArt.Tables;
using Plugin.Media;
using Plugin.Media.Abstractions;
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
    public partial class ProfilePage : ContentPage
    {
        MediaFile file;
        public ProfilePage()
        {
            InitializeComponent();
            LoadProfile();
        }

        private async void LoadProfile()
        {
            if (Application.Current.Properties.ContainsKey("USER_NAME"))
            {
                var username = Application.Current.Properties["USER_NAME"] as string;

                var dbpath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "UserDatabase.db");
                var db = new SQLiteConnection(dbpath);
                var myquery = db.Table<RegUserTable>().Where(u => u.UserName.Equals(username)).FirstOrDefault();

                if (myquery != null)
                {
                    EntryUserName.Text = myquery.UserName;
                    EntryUserPassword.Text = myquery.Password;
                    EntryUserEmail.Text = myquery.Email;
                    EntryUserPhoneNumber.Text = myquery.PhoneNumber;
                }
            }
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            if (Application.Current.Properties.ContainsKey("USER_NAME"))
            {
                var dbpath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "UserDatabase.db");
                var db = new SQLiteConnection(dbpath);
                var item = db.Table<RegUserTable>().Where(u => u.UserName.Equals(EntryUserName.Text) && u.Password.Equals(EntryUserPassword.Text)).FirstOrDefault();

                if (item != null)
                {
                    item.Email = EntryUserEmail.Text;
                    item.PhoneNumber = EntryUserPhoneNumber.Text;
                    try { 
                        db.RunInTransaction(() =>
                        {
                            db.Update(item);
                        });

                        Device.BeginInvokeOnMainThread(async () =>
                        {
                            await this.DisplayAlert(null, "แก้ไขข้อมูลเรียบร้อยแล้ว", null, "ตกลง");
                        });
                    }
                    catch(Exception sqlEx) 
                    {
                        System.Diagnostics.Debug.WriteLine(sqlEx);
                        Device.BeginInvokeOnMainThread(async () =>
                        {
                            await this.DisplayAlert(null, "มีข้อผิดพลาดเกิดขึ้น\nโปรดลองใหม่อีกครั้งในภายหลัง\n(E100)", null, "ตกลง");
                        });
                    }
                }
                else
                {
                    Device.BeginInvokeOnMainThread(async () =>
                    {
                        await this.DisplayAlert(null, "มีข้อผิดพลาดเกิดขึ้น\nโปรดลองใหม่อีกครั้งในภายหลัง\n(E101)", null, "ตกลง");
                        await Navigation.PushAsync(new MainPage());
                    });
                }
            }
            else
            {
                Application.Current.Properties.Clear();
                Device.BeginInvokeOnMainThread(async () =>
                {
                    await this.DisplayAlert(null, "มีข้อผิดพลาดเกิดขึ้น\nโปรดลองใหม่อีกครั้งในภายหลัง\n(E01)", null, "ตกลง");
                    await Navigation.PushAsync(new LoginPage());
                });
            }
        }

        private async void Button_Clicked_1(object sender, EventArgs e)
        {
            await CrossMedia.Current.Initialize();

            if (!CrossMedia.Current.IsPickPhotoSupported)
            {
                await DisplayAlert(null, "ไม่รองรับการ\n(E102)", "ตกลง", null);
                return;
            }

            var mediaOption = new PickMediaOptions()
            {
                PhotoSize = PhotoSize.Small
            };

            var selectedImageFile = await CrossMedia.Current.PickPhotoAsync(mediaOption);

            if (selectedImageFile == null)
            {
                await DisplayAlert(null, "ไม่สามารถเปิดรูปได้\nโปรดลองใหม่อีกครั้งในภายหลัง\n(E103)", "ตกลง", null);
                return;
            }

            selectedImage.Source = ImageSource.FromStream(()=> selectedImageFile.GetStream());
        }
    }
}