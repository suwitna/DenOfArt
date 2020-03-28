using DenOfArt.Model;
using Firebase.Database;
using Firebase.Database.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DenOfArt
{
    class FirebaseHelper
    {
        FirebaseClient firebase = new FirebaseClient("https://maplocation-4b2a1.firebaseio.com/");
        public async Task<List<Person>> GetAllPersons()
        {
            return (await firebase
              .Child("Persons")
              .OnceAsync<Person>()).Select(item => new Person
              {
                  Name = item.Object.Name,
                  PersonId = item.Object.PersonId
              }).ToList();
        }

        public async Task<Person> GetPerson(int personId)
        {
            var allPersons = await GetAllPersons();
            await firebase
              .Child("Persons")
              .OnceAsync<Person>();
            return allPersons.Where(a => a.PersonId == personId).FirstOrDefault();
        }

        public async Task AddPerson(int personId, string name)
        {

            await firebase
              .Child("Persons")
              .PostAsync(new Person() { PersonId = personId, Name = name });
        }

        public async Task UpdatePerson(int personId, string name)
        {
            var toUpdatePerson = (await firebase
              .Child("Persons")
              .OnceAsync<Person>()).Where(a => a.Object.PersonId == personId).FirstOrDefault();

            await firebase
              .Child("Persons")
              .Child(toUpdatePerson.Key)
              .PutAsync(new Person() { PersonId = personId, Name = name });
        }

        public async Task DeletePerson(int personId)
        {
            var toDeletePerson = (await firebase
              .Child("Persons")
              .OnceAsync<Person>()).Where(a => a.Object.PersonId == personId).FirstOrDefault();
            await firebase.Child("Persons").Child(toDeletePerson.Key).DeleteAsync();

        }

        public async Task AddMapHistory(MapHistory history)
        {

            await firebase
              .Child("MapHistory")
              .PostAsync(history);
        }

        public async Task DeleteMapHistor()
        {
            await firebase.Child("MapHistory").DeleteAsync();

        }

        public async Task<List<MapHistory>> GetAllHistory()
        {
            return (await firebase
              .Child("GeoHistory")
              .OnceAsync<MapHistory>()).Select(item => new MapHistory
              {
                  LoginName = item.Object.LoginName,
                  Accuracy = item.Object.Accuracy,
                  GeoLatitude = item.Object.GeoLatitude,
                  GeoLongitude = item.Object.GeoLongitude,
                  PinType = item.Object.PinType,
                  PinLabel = item.Object.PinLabel,
                  PinAddress = item.Object.PinAddress,
                  SaveTime = item.Object.SaveTime
              }).ToList();
        }
    }
}
