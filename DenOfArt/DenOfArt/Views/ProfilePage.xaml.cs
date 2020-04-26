using Android.Content;
using Android.Widget;
using DenOfArt.API;
using DenOfArt.Tables;
using Plugin.Media;
using Plugin.Media.Abstractions;
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

        Context context;
        APIRequestHelper apiRequestHelper;
        IMyAPI myAPI;
        public ProfilePage()
        {
            InitializeComponent();
            //Initial API
            var currentContext = Android.App.Application.Context;
            this.context = currentContext;
            myAPI = RestService.For<IMyAPI>(App._apiURL.ToString());
            apiRequestHelper = new APIRequestHelper(currentContext, myAPI);
            //End of initial

            SetEditMode(false);
            popupLoadingView.IsVisible = true;
            activityIndicator.IsRunning = true;
            LoadProfile();
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
        private async void LoadProfile()
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

                    if (tmpGender != null && SelectGender.SelectedItem != null)
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

                    RootProfileObject profileData = await apiRequestHelper.RequestProfileAsync(username);
                    if (profileData != null && profileData.Data != null && profileData.Data.Count > 0)
                    {
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

                            if (profile.Gender != null && profile.Gender.Trim() != "")
                            {
                                SelectGender.SelectedItem = profile.Gender;
                                tmpGender = profile.Gender;
                            }

                            if (profile.DateOfBirth != null && profile.DateOfBirth.Trim() != "")
                            {
                                DateTime date = DateTime.ParseExact(profile.DateOfBirth, "dd/MM/yyyy", null);
                                SelectDateOfBirth.Date = date;
                                tmpDateofBirth = date;
                            }

                            if (profile.PhoneNumber != null && profile.PhoneNumber.Trim() != "")
                            {
                                EntryUserPhoneNumber.Text = profile.PhoneNumber;
                                tmpPhone = profile.PhoneNumber;
                            }

                            if (profile.Email != null && profile.Email.Trim() != "")
                            {
                                EntryEmail.Text = profile.Email;
                                tmpEmail = profile.Email;
                            }

                            if (profile.Address1 != null && profile.Address1.Trim() != "")
                            {
                                EntryAddress1.Text = profile.Address1;
                                tmpAddr1 = profile.Address1;
                            }

                            if (profile.Address2 != null && profile.Address2.Trim() != "")
                            {
                                EntryAddress2.Text = profile.Address2;
                                tmpAddr2 = profile.Address2;
                            }

                            if (profile.Address3 != null && profile.Address3.Trim() != "")
                            {
                                EntryAddress3.Text = profile.Address3;
                                tmpAddr3 = profile.Address3;
                            }
                        }
                    }
                }
            }

            Device.StartTimer(TimeSpan.FromSeconds(1), () => {
                popupLoadingView.IsVisible = false;
                activityIndicator.IsRunning = false;
                return true;
            });
        }

        private async void SelectImage_Clicked(object sender, EventArgs e)
        {
            await CrossMedia.Current.Initialize();

            if (!CrossMedia.Current.IsPickPhotoSupported)
            {
                Toast.MakeText(context, "โปรแกรมไม่รองรับการ\n(E103)", ToastLength.Short).Show();
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

        //private async void SaveProfile(string username, string email)
        private async void SaveProfile(string username)
        {
            RootProfileObject profileData = await apiRequestHelper.RequestProfileAsync(username);
            if (profileData != null && profileData.Data != null && profileData.Data.Count > 0)
            {
                List<ProfileJson> Data = profileData.Data;
                if(Data.Count > 0)
                {
                    ProfileJson json = Data.First<ProfileJson>();

                    var dbpath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "UserDatabase.db");
                    var db = new SQLiteConnection(dbpath);
                    var userprofile = db.GetTableInfo("ProfileTable");

                    if (userprofile.Count == 0)
                    {
                        //TODO เอาข้อมูลจาก Cloud มาอัพเดท Local DB
                        var item = new ProfileTable()
                        {
                            UserName = username,
                            FirstName = EntryFirstName.Text,
                            LastName = EntryLastName.Text,
                            Gender = ((SelectGender == null || SelectGender.SelectedItem  == null)? "" : SelectGender.SelectedItem.ToString()),
                            DateOfBirth = ((SelectDateOfBirth == null || SelectDateOfBirth.Date == null)? "" : SelectDateOfBirth.Date.ToString("dd/MM/yyyy")),
                            Address1 = EntryAddress1.Text,
                            Address2 = EntryAddress2.Text,
                            Address3 = EntryAddress3.Text,
                            Email = json.Email,
                            PhoneNumber = json.PhoneNumber,
                            Content = tmpImageBytesNew,

                            CreateDate = DateTime.Now,
                            UpdateDate = DateTime.Now,
                        };
                        db.CreateTable<ProfileTable>();
                        db.Insert(item);

                        //json.UserName = username;
                        json.FirstName = EntryFirstName.Text;
                        json.LastName = EntryLastName.Text;
                        if (SelectGender != null && SelectGender.SelectedItem != null)
                        {
                            json.Gender = SelectGender.SelectedItem.ToString();
                        }
                        if (SelectDateOfBirth != null && SelectDateOfBirth.Date != null)
                        {
                            json.DateOfBirth = SelectDateOfBirth.Date.ToString("dd/MM/yyyy");
                        }
                        json.Address1 = EntryAddress1.Text;
                        json.Address2 = EntryAddress2.Text;
                        json.Address3 = EntryAddress3.Text;
                        //json.Email = email;
                        //json.PhoneNumber = EntryUserPhoneNumber.Text;
                        //json.CreateDate = DateTime.Now.ToString("dd/MM/yyyy hh:mm tt");
                        json.UpdateDate = DateTime.Now.ToString("dd/MM/yyyy hh:mm tt");

                        await apiRequestHelper.RequestUpdateProfileAsync(json);

                        Device.BeginInvokeOnMainThread(async () =>
                        {
                            Toast.MakeText(context, "บันทึกข้อมูลสำเร็จ", ToastLength.Short).Show();
                        });
                    }
                    else
                    {
                        var item = db.Table<ProfileTable>().Where(u => u.UserName.Equals(username)).FirstOrDefault();

                        if (item != null)
                        {
                            item.FirstName = EntryFirstName.Text;
                            item.LastName = EntryLastName.Text;
                            if (SelectGender != null && SelectGender.SelectedItem != null)
                            {
                                item.Gender = SelectGender.SelectedItem.ToString();
                            }
                            if (SelectDateOfBirth != null && SelectDateOfBirth.Date != null)
                            {
                                item.DateOfBirth = SelectDateOfBirth.Date.ToString("dd/MM/yyyy");
                            }
                            item.Address1 = EntryAddress1.Text;
                            item.Address2 = EntryAddress2.Text;
                            item.Address3 = EntryAddress3.Text;
                            //item.Email = email;
                            item.PhoneNumber = EntryUserPhoneNumber.Text;

                            if (tmpImageBytesNew != null)
                            {
                                item.Content = tmpImageBytesNew;
                            }
                            item.UpdateDate = DateTime.Now;

                            db.RunInTransaction(() =>
                            {
                                db.Update(item);
                            });

                            json.UserName = username;
                            json.ProfileId = item.ProfileId.ToString();
                            json.FirstName = EntryFirstName.Text;
                            json.LastName = EntryLastName.Text;

                            if (SelectGender != null && SelectGender.SelectedItem != null)
                            {
                                json.Gender = SelectGender.SelectedItem.ToString();
                            }
                            if (SelectDateOfBirth != null && SelectDateOfBirth.Date != null)
                            {
                                json.DateOfBirth = SelectDateOfBirth.Date.ToString("dd/MM/yyyy");
                            }
                            json.Address1 = EntryAddress1.Text;
                            json.Address2 = EntryAddress2.Text;
                            json.Address3 = EntryAddress3.Text;
                            //json.Email = email;
                            //json.PhoneNumber = EntryUserPhoneNumber.Text;
                            json.UpdateDate = DateTime.Now.ToString("dd/MM/yyyy hh:mm tt");




                            var result = await apiRequestHelper.RequestUpdateProfileAsync(json);
                            Console.WriteLine("SaveProfile result:", result);

                            Device.BeginInvokeOnMainThread(async () =>
                            {
                                Toast.MakeText(context, "บันทึกข้อมูลสำเร็จ", ToastLength.Short).Show();
                            });
                        }
                        else
                        {
                            //TODO เอาข้อมูลจาก Cloud มาอัพเดท Local DB
                            item = new ProfileTable()
                            {
                                UserName = username,
                                FirstName = EntryFirstName.Text,
                                LastName = EntryLastName.Text,
                                Gender = ((SelectGender == null || SelectGender.SelectedItem  == null) ? "" : SelectGender.SelectedItem.ToString()),
                                DateOfBirth = ((SelectDateOfBirth == null || SelectDateOfBirth.Date == null) ? "" : SelectDateOfBirth.Date.ToString("dd/MM/yyyy")),
                                Address1 = EntryAddress1.Text,
                                Address2 = EntryAddress2.Text,
                                Address3 = EntryAddress3.Text,
                                Email = json.Email,
                                PhoneNumber = json.PhoneNumber,
                                Content = tmpImageBytesNew,

                                CreateDate = DateTime.Now,
                                UpdateDate = DateTime.Now,
                            };
                            db.CreateTable<ProfileTable>();
                            db.Insert(item);

                            //json.UserName = username;
                            json.FirstName = EntryFirstName.Text;
                            json.LastName = EntryLastName.Text;
                            if (SelectGender != null && SelectGender.SelectedItem != null)
                            {
                                json.Gender = SelectGender.SelectedItem.ToString();
                            }
                            if (SelectDateOfBirth != null && SelectDateOfBirth.Date != null)
                            {
                                json.DateOfBirth = SelectDateOfBirth.Date.ToString("dd/MM/yyyy");
                            }
                            json.Address1 = EntryAddress1.Text;
                            json.Address2 = EntryAddress2.Text;
                            json.Address3 = EntryAddress3.Text;
                            //json.Email = email;
                            //json.PhoneNumber = EntryUserPhoneNumber.Text;
                            //json.CreateDate = DateTime.Now.ToString("dd/MM/yyyy hh:mm tt");
                            json.UpdateDate = DateTime.Now.ToString("dd/MM/yyyy hh:mm tt");

                            await apiRequestHelper.RequestUpdateProfileAsync(json);

                            Device.BeginInvokeOnMainThread(async () =>
                            {
                                Toast.MakeText(context, "บันทึกข้อมูลสำเร็จ", ToastLength.Short).Show();
                            });
                        }
                        

                        

                        //Refresh Profile Image
                        var md = (MasterDetailPage)Application.Current.MainPage;
                        var menu = (MainPageMaster)md.Master;
                        menu.LoadProfile();

                        return;
                    }
                }
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
                //var dbpath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "UserDatabase.db");
                //var db = new SQLiteConnection(dbpath);
                //var item = db.Table<RegUserTable>().Where(u => u.UserName.Equals(username)).FirstOrDefault();

                if (username != null)
                {
                    //item.PhoneNumber = EntryUserPhoneNumber.Text;
                    try
                    {
                        //db.RunInTransaction(() =>
                        //{
                        //    db.Update(item);
                        //});

                        //SaveProfile(username, item.Email);
                        SaveProfile(username);
                        Device.BeginInvokeOnMainThread(async () =>
                        {
                            //popupSavingView.IsVisible = true;
                            Device.StartTimer(TimeSpan.FromSeconds(1), () => {
                                //popupSavingView.IsVisible = false;
                                ToolbarItems.Clear();
                                ToolbarItems.Add(new ToolbarItem("แก้ไข", null, async () =>
                                {
                                    AddSaveCancelNavItem();
                                }));
                                SetEditMode(false);
                                return false;
                            });
                        });
                    }
                    catch (Exception sqlEx)
                    {
                        System.Diagnostics.Debug.WriteLine(sqlEx);
                        Device.BeginInvokeOnMainThread(async () =>
                        {
                            Toast.MakeText(context, "มีข้อผิดพลาดเกิดขึ้น\nโปรดลองใหม่อีกครั้งในภายหลัง\n(E100)", ToastLength.Short).Show();
                            AddEditNavItem();
                            SetEditMode(false);
                        });
                    }
                }
                else
                {
                    Device.BeginInvokeOnMainThread(async () =>
                    {
                        Toast.MakeText(context, "มีข้อผิดพลาดเกิดขึ้น\nโปรดลองใหม่อีกครั้งในภายหลัง\n(E101)", ToastLength.Short).Show();
                        AddEditNavItem();
                        SetEditMode(false);
                        await Navigation.PushAsync(new MainPage());
                    });
                }
            }
            else
            {
                Application.Current.Properties.Clear();
                Device.BeginInvokeOnMainThread(async () =>
                {
                    Toast.MakeText(context, "มีข้อผิดพลาดเกิดขึ้น\nโปรดลองใหม่อีกครั้งในภายหลัง\n(E102)", ToastLength.Short).Show();
                    AddEditNavItem();
                    SetEditMode(false);
                    await Navigation.PushAsync(new LoginPage());
                });
            }
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
                //var result =  await DisplayAlert(null, "ต้องกการบันทึกการแก้ไข้ข้อมูล?", "บันทึก", "ยกเลิก");
                //if(result)
                //{ 
                    SaveProfile();
                    return;
                //}
                //else
                //{
                //    return;
                //}
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