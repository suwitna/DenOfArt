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
        byte[] ImageBytes;
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

                }

                var profile = db.Table<ProfileTable>().Where(u => u.UserName.Equals(username)).FirstOrDefault();
                if (profile != null)
                {
                    EntryFirstName.Text = profile.FirstName;
                    EntryLastName.Text = profile.LastName;

                    if (profile.Content != null)
                    {
                        ImageBytes = profile.Content;
                        Stream sm = BytesToStream(ImageBytes);
                        selectedImage.Source = ImageSource.FromStream(() => sm);
                    }
                    
                    if(profile.Gender != null)
                    {
                        SelectGender.SelectedItem = profile.Gender;
                    }
                    
                    if(profile.Age != null)
                    { 
                        EntryAge.Text = profile.Age;
                        StepperAge.Value = Convert.ToInt32(profile.Age);
                    }

                    if (profile.DateOfBirth != null)
                    {
                        DateTime date = DateTime.ParseExact(profile.DateOfBirth, "dd/MM/yyyy", null);
                        SelectDateOfBirth.Date = date;
                    }

                    if(profile.PhoneNumber != null)
                    {
                        EntryUserPhoneNumber.Text = profile.PhoneNumber;
                    }
                }
            }
        }

        private void Save_Clicked(object sender, EventArgs e)
        {
            if (Application.Current.Properties.ContainsKey("USER_NAME"))
            {
                var username = Application.Current.Properties["USER_NAME"] as string;
                var dbpath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "UserDatabase.db");
                var db = new SQLiteConnection(dbpath);
                var item = db.Table<RegUserTable>().Where(u => u.UserName.Equals(username)).FirstOrDefault();

                if (item != null)
                {
                    item.PhoneNumber = EntryUserPhoneNumber.Text;
                    try {
                        db.RunInTransaction(() =>
                        {
                            db.Update(item);
                        });

                        SaveProfile(username, item.Email);

                        Device.BeginInvokeOnMainThread(async () =>
                        {
                            await this.DisplayAlert(null, "แก้ไขข้อมูลเรียบร้อยแล้ว", null, "ตกลง");
                        });
                    }
                    catch (Exception sqlEx)
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
                await DisplayAlert(null, "ไม่สามารถเปิดรูปได้\nโปรดลองใหม่อีกครั้งในภายหลัง\n(E103)", null, "ตกลง");
                return;
            }

            selectedImage.Source = ImageSource.FromStream(() => selectedImageFile.GetStream());
            GetImageBytes(selectedImageFile.GetStream());
        }

        private void Cancel_Clicked(object sender, EventArgs e)
        {

        }

        private void Stepper_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            EntryAge.Text = e.NewValue.ToString();
        }


        public byte[] ReadFully(Stream input)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                input.CopyTo(ms);
                return ms.ToArray();
            }
        }

        private byte[] GetImageBytes(Stream stream)
        {
            using (var memoryStream = new System.IO.MemoryStream())
            {
                stream.CopyTo(memoryStream);
                ImageBytes = memoryStream.ToArray();
            }
            return ImageBytes;
        }

        public Stream BytesToStream(byte[] bytes)
        {
            Stream stream = new MemoryStream(bytes);
            return stream;
        }

        private async void SaveProfile(string username, string email)
        {
            var dbpath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "UserDatabase.db");
            var db = new SQLiteConnection(dbpath);
            db.CreateTable<ProfileTable>();

            var item = db.Table<ProfileTable>().Where(u => u.UserName.Equals(username)).FirstOrDefault();

            if (item == null)
            {
                item = new ProfileTable()
                {
                    UserName = username,
                    FirstName = EntryFirstName.Text,
                    LastName = EntryLastName.Text,
                    Gender = SelectGender.SelectedItem.ToString(),
                    Age = EntryAge.Text,
                    DateOfBirth = SelectDateOfBirth.ToString(),
                    Address1 = "",
                    Address2 = "",
                    Address3 = "",
                    Email = email,
                    PhoneNumber = EntryUserPhoneNumber.Text,
                    Content = ImageBytes,

                    CreateDate = DateTime.Now,
                    UpdateDate = DateTime.Now,
                };

                db.Insert(item);
                Device.BeginInvokeOnMainThread(async () =>
                {
                    var result = await this.DisplayAlert(null, "เพิ่มข้อมูลลูกค้าเสร็จแล้ว", null, "ตกลง");
                });
            }
            else
            {
                item.FirstName = EntryFirstName.Text;
                item.LastName = EntryLastName.Text;
                item.Gender = SelectGender.SelectedItem.ToString();
                item.Age = EntryAge.Text;
                item.DateOfBirth = SelectDateOfBirth.Date.ToString("dd/MM/yyyy");
                item.Address1 = "";
                item.Address2 = "";
                item.Address3 = "";
                item.Email = email;
                item.PhoneNumber = EntryUserPhoneNumber.Text;
                item.Content = ImageBytes;

                item.UpdateDate = DateTime.Now;

                db.RunInTransaction(() =>
                {
                    db.Update(item);
                });
            }
        }
    }
}