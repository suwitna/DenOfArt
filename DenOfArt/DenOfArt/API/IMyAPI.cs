using Refit;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DenOfArt.API
{
    public interface IMyAPI
    {
        [Post("/DenOfArtLogin")]
        Task<string> LoginUser([Body(BodySerializationMethod.UrlEncoded)] Dictionary<string, object> data);
        
        [Post("/DenOfArtRegister")]
        Task<string> RegisterUser([Body(BodySerializationMethod.UrlEncoded)] Dictionary<string, object> data);

        [Post("/DenOfArtUserExist")]
        Task<string> UserExist([Body(BodySerializationMethod.UrlEncoded)] Dictionary<string, object> data);

        [Post("/DenOfArtGetUserData")]
        Task<RegUserJson> GetUserData([Body(BodySerializationMethod.UrlEncoded)] Dictionary<string, object> data);

        [Post("/DenOfArtGetAllAppointment")]
        Task<RootAppointmentObject> GetAllAppointment([Body(BodySerializationMethod.UrlEncoded)] Dictionary<string, object> data);

        [Post("/DenOfArtExistAppointment")]
        Task<string> ExistAppointment([Body(BodySerializationMethod.UrlEncoded)] Dictionary<string, object> data);

        [Post("/DenOfArtUpdateAppointment")]
        Task<string> UpdateAppointment([Body(BodySerializationMethod.UrlEncoded)] Dictionary<string, object> data);

        
        [Post("/DenOfArtGetProfile")]
        Task<RootProfileObject> GetProfile([Body(BodySerializationMethod.UrlEncoded)] Dictionary<string, object> data);
        
        [Post("/DenOfArtAddProfile")]
        Task<string> AddProfile([Body(BodySerializationMethod.UrlEncoded)] Dictionary<string, object> data);

        [Post("/DenOfArtUpdateProfile")]
        Task<string> UpdateProfile([Body(BodySerializationMethod.UrlEncoded)] Dictionary<string, object> data);
    }
}
