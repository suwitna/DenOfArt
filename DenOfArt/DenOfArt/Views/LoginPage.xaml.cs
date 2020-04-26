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

            btnLogin.IsEnabled = true;
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

            btnLogin.IsEnabled = false;
            //Cloud database
            //Register data to firebase also
            RegUserJson user = await apiRequestHelper.RequestLoginUserAsync(EntryUser.Text, EntryPassword.Text);
            if (user != null & user.UserName == EntryUser.Text)
            {
                var dbpath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "UserDatabase.db");
                var db = new SQLiteConnection(dbpath);
                var reguser = db.GetTableInfo("RegUserTable");
                if (reguser.Count == 0)
                {
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
                else
                {
                    var item = db.Table<RegUserTable>().Where(u => u.UserName.Equals(user.UserName)).FirstOrDefault();

                    if (item == null)
                    {
                        var data = new RegUserTable()
                        {
                            UserName = user.UserName,
                            Password = user.Password,
                            Email = user.Email,
                            PhoneNumber = user.PhoneNumber,
                        };
                        db.Insert(data);
                    }
                }
                RootProfileObject profileData = await apiRequestHelper.RequestProfileAsync(user.UserName);
                if (profileData != null && profileData.Data != null && profileData.Data.Count > 0)
                {
                    List<ProfileJson> Data = profileData.Data;
                    if (Data.Count > 0)
                    {
                        ProfileJson json = Data.First<ProfileJson>();

                        var userprofile = db.GetTableInfo("ProfileTable");
                        if (userprofile.Count == 0)
                        { 
                            db.CreateTable<ProfileTable>();
                            var profile = new ProfileTable()
                            {
                                UserName = json.UserName,
                                FirstName = json.FileName,
                                LastName = json.LastName,
                                Gender = (json.Gender == null ? "" : json.Gender),
                                DateOfBirth = (json.DateOfBirth == null ? "" : json.DateOfBirth),
                                Address1 = json.Address1 == null? "" : json.Address1,
                                Address2 = json.Address2 == null ? "" : json.Address2,
                                Address3 = json.Address3 == null ? "" : json.Address3,
                                Email = json.Email,
                                PhoneNumber = json.PhoneNumber,
  
                                CreateDate = DateTime.Now,
                                UpdateDate = DateTime.Now,
                            };
                            
                            db.Insert(profile);
                        }
                        else
                        {
                            var item = db.Table<ProfileTable>().Where(u => u.UserName.Equals(user.UserName)).FirstOrDefault();

                            if (item == null)
                            {
                                var profile = new ProfileTable()
                                {
                                    UserName = json.UserName,
                                    FirstName = json.FileName,
                                    LastName = json.LastName,
                                    Gender = (json.Gender == null ? "" : json.Gender),
                                    DateOfBirth = (json.DateOfBirth == null ? "" : json.DateOfBirth),
                                    Address1 = json.Address1 == null ? "" : json.Address1,
                                    Address2 = json.Address2 == null ? "" : json.Address2,
                                    Address3 = json.Address3 == null ? "" : json.Address3,
                                    Email = json.Email,
                                    PhoneNumber = json.PhoneNumber,

                                    CreateDate = DateTime.Now,
                                    UpdateDate = DateTime.Now,
                                };

                                db.Insert(profile);
                            }
                        }
                    }
                }
                
                ///////////////////

                Device.StartTimer(TimeSpan.FromSeconds(2), () => {
                    btnLogin.IsEnabled = true;

                    Toast.MakeText(context, "Login Successfull", ToastLength.Short).Show();
                    Application.Current.Properties.Add("USER_NAME", EntryUser.Text);
                    App.Current.MainPage = new MainPage();
                    return false;
                });
            }
            else
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    var result2 = await this.DisplayAlert(null, "ข้อผิดพลาด! ไม่พบข้อมูลผู้ใช้งาน\nกรุณาตรวจสอบชื่อผู้ใช้งานและรหัสผ่าน", null, "ตกลง");

                    if (!result2)
                    {
                        btnLogin.IsEnabled = true;
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