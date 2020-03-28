using Plugin.Geolocator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Xaml;
using Firebase.Database;
using Firebase.Database.Query;
using DenOfArt.Model;

namespace DenOfArt.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LocationTrackerPage : ContentPage
    {
        FirebaseHelper firebaseHelper = new FirebaseHelper();
        public LocationTrackerPage()
        {
            InitializeComponent();
            btnGetLocation.Clicked += BtnGetLocation_Clicked;
            //btnAdd.Clicked += BtnAdd_Clicked;
            //btnUpdate.Clicked += BtnUpdate_Clicked;
            Device.StartTimer(TimeSpan.FromSeconds(0.01), () =>
            {
                Task.Factory.StartNew(async () =>
                {
                    Device.BeginInvokeOnMainThread(async () =>
                    {
                        await firebaseHelper.DeleteMapHistor();
                        await RetreiveLocation();
                    });

                });
                return false;
            });

            Device.StartTimer(TimeSpan.FromSeconds(60), () =>
            {
                Task.Factory.StartNew(async () =>
                {
                    Device.BeginInvokeOnMainThread(async () =>
                    {
                        await RetreiveLocation();
                    });

                });
                return true;
            });
        }
        /*
        private async void BtnUpdate_Clicked(object sender, EventArgs e)
        {
            await firebaseHelper.UpdatePerson(1, "Mika");
            await DisplayAlert("Success", "Person Updated Successfully", "OK");
            var allPersons = await firebaseHelper.GetAllPersons();
            lstPersons.ItemsSource = allPersons;
        }

        private async void BtnAdd_Clicked(object sender, EventArgs e)
        {
            await firebaseHelper.AddPerson(1, "suwit");
            await DisplayAlert("Success", "Person Added Successfully", "OK");
            var allPersons = await firebaseHelper.GetAllPersons();
            lstPersons.ItemsSource = allPersons;
        }
        
        protected async override void OnAppearing()
        {

            base.OnAppearing();
            var allPersons = await firebaseHelper.GetAllPersons();
            lstPersons.ItemsSource = allPersons;
        }
        */

        protected async override void OnAppearing()
        {

            base.OnAppearing();
        }

        private async void BtnGetLocation_Clicked(object sender, EventArgs e)
        {
            await RetreiveLocation();
        }

        private async Task RetreiveLocation()
        {
            if (Application.Current.Properties.ContainsKey("USER_NAME"))
            {
                var username = Application.Current.Properties["USER_NAME"] as string;

                var locator = CrossGeolocator.Current;
                locator.DesiredAccuracy = 20;

                var position = await locator.GetPositionAsync(TimeSpan.FromTicks(10000));
                txtLat.Text = "Latitude : " + position.Latitude.ToString();
                txtLong.Text = "Longitude : " + position.Longitude.ToString();

                

                Position pos = new Position(position.Latitude, position.Longitude);
                DateTime datetime = DateTime.Now;
                CustomPin pin = new CustomPin
                {
                    Type = PinType.Place,
                    Position = pos,
                    Label = "ข้อมูลพิกัดล่าสุด("+ username + ")",
                    Address = "พิกัด: " + position.Latitude.ToString() + ", " + position.Longitude.ToString(),
                    Name = username,
                    Time = datetime.ToString(),
                    Url = "www.google.co.th"
                };
                MyMap.Pins.Clear();
                MyMap.MoveToRegion(MapSpan.FromCenterAndRadius(new Position(position.Latitude, position.Longitude)
                                  , Distance.FromMeters(500)));
                MyMap.Pins.Add(pin);

                MapHistory history = new MapHistory();
                history.LoginName = username;
                history.Accuracy = locator.DesiredAccuracy.ToString();
                history.GeoLatitude = position.Latitude.ToString();
                history.GeoLongitude = position.Longitude.ToString();
                history.PinType = PinType.Place.ToString();
                history.PinLabel = pin.Label.ToString();
                history.PinAddress = pin.Address.ToString();
                history.SaveTime = datetime;

                AddMapHistory(history);
            }
        }

        private async void AddMapHistory(MapHistory history)
        {
            await firebaseHelper.AddMapHistory(history);
            //await DisplayAlert("Success", "Person Added Successfully", "OK");
            Console.WriteLine("Map history added Successfully");
            // var allPersons = await firebaseHelper.GetAllPersons();
            // lstPersons.ItemsSource = allPersons;
        }
    }
}