using Android.App;
using Android.Content;
using Android.Widget;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace DenOfArt.API
{
    public class APIRequestHelper
    {
        Context context;
        IMyAPI myAPI;

        public APIRequestHelper(Context context, IMyAPI myAPI)
        {
            this.context = context;
            this.myAPI = myAPI;
        }

        public async Task<string> RequestCheckUserExistAsync(string username)
        {
            //Create Parameter to POST request
            Dictionary<string, object> data = new Dictionary<string, object>();
            data.Add("username", username);

            string result = await myAPI.UserExist(data);
            
            return result;
            //Toast.MakeText(context, result, ToastLength.Short).Show();
        }

        public async Task RequestRegisterUserAsync(string username, string password, string email, string phoneno)
        {
            if (String.IsNullOrEmpty(username))
            {
                Toast.MakeText(context, "User name can not null or empty", ToastLength.Short).Show();
                return;
            }

            if (String.IsNullOrEmpty(password))
            {
                Toast.MakeText(context, "Password can not null or empty", ToastLength.Short).Show();
                return;
            }

            if (String.IsNullOrEmpty(email))
            {
                Toast.MakeText(context, "Email can not null or empty", ToastLength.Short).Show();
                return;
            }
            //Create Parameter to POST request
            Dictionary<string, object> data = new Dictionary<string, object>();
            data.Add("username", username);
            data.Add("password", password);
            data.Add("email", email);
            data.Add("phonenumber", phoneno);

            string result = await myAPI.RegisterUser(data);
            //oast.MakeText(context, result, ToastLength.Short).Show();
        }

        public async Task<string> RequestLoginUserAsync(string username, string password)
        {
            //Create Dialog
            //Create Parameter to POST request
            Dictionary<string, object> data = new Dictionary<string, object>();
            data.Add("username", username);
            data.Add("password", password);

            string result = await myAPI.LoginUser(data);
            return result;
        }

        public async Task<RootAppointmentObject> RequestAppointmentAsync(string username)
        {
            //Create Dialog
            //Create Parameter to POST request
            Dictionary<string, object> data = new Dictionary<string, object>();
            data.Add("username", username);

            RootAppointmentObject response = await myAPI.GetAppointment(data);

            return response;
        }
    }
}
