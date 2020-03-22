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
        byte[] tmpImageBytes;
        byte[] tmpImageBytesNew;
        string tmpFirstName;
        string tmpLastName;
        string tmpGender;
        DateTime tmpDateofBirth;
        string tmpPhone;
        string tmpEmail;
        string tmpAddr1;
        string tmpAddr2;
        string tmpAddr3;
        bool isCancelMode = false;

        public ProfilePage()
        {
            InitializeComponent();
            SetEditMode(false);
            popupLoadingView.IsVisible = true;
            activityIndicator.IsRunning = true;
            LoadProfile();
            Device.StartTimer(TimeSpan.FromSeconds(0.50), () => {
                popupLoadingView.IsVisible = false;
                activityIndicator.IsRunning = false;
                return true;
            });
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
        private void LoadProfile()
        {
            if (Application.Current.Properties.ContainsKey("USER_NAME"))
            {
                if (isCancelMode)
                {
                    EntryFirstName.Text = tmpFirstName;
                    EntryLastName.Text = tmpLastName;

                    if (tmpImageBytesNew != null)
                    {
                        Stream sm = BytesToStream(tmpImageBytes);
                        selectedImage.Source = ImageSource.FromStream(() => sm);
                    }

                    if (tmpGender != null)
                    {
                        SelectGender.SelectedItem = tmpGender;
                    }

                    if (tmpDateofBirth != null)
                    {
                        SelectDateOfBirth.Date = tmpDateofBirth;
                    }

                    if (tmpPhone != null)
                    {
                        EntryUserPhoneNumber.Text = tmpPhone;
                    }

                    if (tmpEmail != null)
                    {
                        EntryEmail.Text = tmpEmail;
                    }

                    if (tmpAddr1 != null)
                    {
                        EntryAddress1.Text = tmpAddr1;
                    }

                    if (tmpAddr2 != null)
                    {
                        EntryAddress2.Text = tmpAddr2;
                    }

                    if (tmpAddr3 != null)
                    {
                        EntryAddress3.Text = tmpAddr3;
                    }
                    tmpImageBytesNew = null;
                    isCancelMode = false;
                }
                else
                {
                    var username = Application.Current.Properties["USER_NAME"] as string;

                    var dbpath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "UserDatabase.db");
                    var db = new SQLiteConnection(dbpath);
                    var myquery = db.Table<RegUserTable>().Where(u => u.UserName.Equals(username)).FirstOrDefault();

                    if (myquery != null)
                    {

                    }

                    var profile = db.Table<ProfileTable>().Where(u => u.UserName.Equals(username)).FirstOrDefault();
                    if (myquery != null && profile != null)
                    {
                        EntryFirstName.Text = profile.FirstName;
                        EntryLastName.Text = profile.LastName;
                        tmpFirstName = profile.FirstName;
                        tmpLastName = profile.LastName;

                        if (profile.Content != null)
                        {
                            if (tmpImageBytes == null)
                            {
                                tmpImageBytes = profile.Content;
                                Stream sm = BytesToStream(tmpImageBytes);
                                selectedImage.Source = ImageSource.FromStream(() => sm);
                            }
                            else
                            {

                                var imageCompare = tmpImageBytes.SequenceEqual(profile.Content);
                                if (!imageCompare)
                                {
                                    tmpImageBytes = profile.Content;
                                    Stream sm = BytesToStream(tmpImageBytes);
                                    selectedImage.Source = ImageSource.FromStream(() => sm);
                                }
                            }


                        }

                        if (profile.Gender != null)
                        {
                            SelectGender.SelectedItem = profile.Gender;
                            tmpGender = profile.Gender;
                        }

                        if (profile.DateOfBirth != null)
                        {
                            DateTime date = DateTime.ParseExact(profile.DateOfBirth, "dd/MM/yyyy", null);
                            SelectDateOfBirth.Date = date;
                            tmpDateofBirth = date;
                        }

                        if (profile.PhoneNumber != null)
                        {
                            EntryUserPhoneNumber.Text = profile.PhoneNumber;
                            tmpPhone = profile.PhoneNumber;
                        }

                        if (profile.Email != null)
                        {
                            EntryEmail.Text = profile.Email;
                            tmpEmail = profile.Email;
                        }

                        if (profile.Address1 != null)
                        {
                            EntryAddress1.Text = profile.Address1;
                            tmpAddr1 = profile.Address1;
                        }

                        if (profile.Address2 != null)
                        {
                            EntryAddress2.Text = profile.Address2;
                            tmpAddr2 = profile.Address2;
                        }

                        if (profile.Address3 != null)
                        {
                            EntryAddress3.Text = profile.Address3;
                            tmpAddr3 = profile.Address3;
                        }
                    }
                }
            }
        }

        private async void SelectImage_Clicked(object sender, EventArgs e)
        {
            await CrossMedia.Current.Initialize();

            if (!CrossMedia.Current.IsPickPhotoSupported)
            {
                await DisplayAlert(null, "โปรแกรมไม่รองรับการ\n(E102)", "ตกลง", null);
                return;
            }

            var mediaOption = new PickMediaOptions()
            {
                PhotoSize = PhotoSize.Small
            };

            var selectedImageFile = await CrossMedia.Current.PickPhotoAsync(mediaOption);

            if (selectedImageFile == null)
            {
                //await DisplayAlert(null, "ไม่สามารถเปิดรูปได้\nโปรดลองใหม่อีกครั้งในภายหลัง\n(E103)", null, "ตกลง");
                return;
            }

            selectedImage.Source = ImageSource.FromStream(() => selectedImageFile.GetStream());
            GetImageBytes(selectedImageFile.GetStream());
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
                tmpImageBytesNew = memoryStream.ToArray();
            }
            return tmpImageBytesNew;
        }

        public Stream BytesToStream(byte[] bytes)
        {
            Stream stream = new MemoryStream(bytes);
            return stream;
        }

        private void SaveProfile(string username, string email)
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
                    DateOfBirth = SelectDateOfBirth.Date.ToString("dd/MM/yyyy"),
                    Address1 = EntryAddress1.Text,
                    Address2 = EntryAddress2.Text,
                    Address3 = EntryAddress3.Text,
                    Email = email,
                    PhoneNumber = EntryUserPhoneNumber.Text,
                    Content = tmpImageBytesNew,

                    CreateDate = DateTime.Now,
                    UpdateDate = DateTime.Now,
                };

                db.Insert(item);
                Device.BeginInvokeOnMainThread(async () =>
                {
                    var result = await this.DisplayAlert(null, "เพิ่มข้อมูลลูกค้าเสร็จแล้ว", null, "ตกลง");
                });
                return;
            }
            else
            {
                item.FirstName = EntryFirstName.Text;
                item.LastName = EntryLastName.Text;
                item.Gender = SelectGender.SelectedItem.ToString();
                item.DateOfBirth = SelectDateOfBirth.Date.ToString("dd/MM/yyyy");
                item.Address1 = EntryAddress1.Text;
                item.Address2 = EntryAddress2.Text;
                item.Address3 = EntryAddress3.Text;
                item.Email = email;
                item.PhoneNumber = EntryUserPhoneNumber.Text;

                if(tmpImageBytesNew != null)
                {
                    item.Content = tmpImageBytesNew;
                }
               
                item.UpdateDate = DateTime.Now;

                db.RunInTransaction(() =>
                {
                    db.Update(item);
                });

                //Refresh Profile Image
                var md = (MasterDetailPage)Application.Current.MainPage;
                var menu = (MainPageMaster)md.Master;
                menu.LoadProfile();

                return;
            }
        }

        private void ToolbarEdit_Clicked(object sender, EventArgs e)
        {
            AddSaveCancelNavItem();
            return;
        }

        private void SetEditMode(bool mode)
        {
            FrameImage.IsEnabled = mode;
            EntryFirstName.IsEnabled = mode;
            EntryLastName.IsEnabled = mode;
            SelectGender.IsEnabled = mode;
            SelectDateOfBirth.IsEnabled = mode;
            EntryUserPhoneNumber.IsEnabled = mode;
            EntryEmail.IsEnabled = false;
            EntryAddress1.IsEnabled = mode;
            EntryAddress2.IsEnabled = mode;
            EntryAddress3.IsEnabled = mode;
            return;
        }

        private void SaveProfile()
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
                    try
                    {
                        db.RunInTransaction(() =>
                        {
                            db.Update(item);
                        });

                        SaveProfile(username, item.Email);

                        Device.BeginInvokeOnMainThread(async () =>
                        {
                            popupSavingView.IsVisible = true;
                            Device.StartTimer(TimeSpan.FromSeconds(1), () => {
                                popupSavingView.IsVisible = false;
                                return true;
                            });
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
            AddEditNavItem();
            SetEditMode(false);
            return;
        }

        private void CancelProfile()
        {
            isCancelMode = true;
            AddEditNavItem();
            SetEditMode(false);
            return;
        }

        private void AddEditNavItem()
        {
            LoadProfile();
            ToolbarItems.Clear();
            ToolbarItems.Add(new ToolbarItem("แก้ไข", null, async () =>
            {
                AddSaveCancelNavItem();
            }));

            return;
        }

        private void AddSaveCancelNavItem()
        {
            SetEditMode(true);
            ToolbarItems.Remove(ToolbarItems.First<ToolbarItem>());
            ToolbarItems.Add(new ToolbarItem("บันทึก", null, async () =>
            {
                var result =  await DisplayAlert(null, "ต้องกการบันทึกการแก้ไข้ข้อมูล?", "บันทึก", "ยกเลิก");
                if(result)
                { 
                    SaveProfile();
                    return;
                }
                else
                {
                    return;
                }
            }));
            ToolbarItems.Add(new ToolbarItem("ยกเลิก", null, async () =>
            {
                CancelProfile();
                return;
            }));

            return;
        }
    }
}