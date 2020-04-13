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
    }
}
