using DenOfArt.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace DenOfArt.Service
{
    public class DataService
    {
        public ObservableCollection<Meeting> Data { get; set; }

        private static DataService dataService;

        public static DataService Instance => dataService ?? (dataService = new DataService());

        private DataService()
        {
            Data = new ObservableCollection<Meeting>
            {
                /*
                new Meeting("General Meeting", "Online", new DateTime(2020, 3, 17, 10, 0, 0)),
                new Meeting("Release Retrospective", "Office", new DateTime(2020, 3, 17, 9, 0, 0)),
                new Meeting("Sprint Meeting", "Online", new DateTime(2020, 3, 17, 10, 0, 0)),
                new Meeting("Release Planning", "Online", new DateTime(2020, 3, 17, 16, 0, 0)),
                new Meeting("General Meeting", "Office", new DateTime(2020, 3, 17, 11, 0, 0)),
                new Meeting("Customer Meeting", "Online", new DateTime(2020, 3, 17, 12, 0, 0)),
                new Meeting("Sprint Meeting", "Office", new DateTime(2020, 3, 17, 15, 0, 0)),
                new Meeting("Sprint Meeting", "Online", new DateTime(2020, 3, 17, 13, 0, 0)),
                */
                new Meeting("รักษารากฟัน ครั้งที่ 1", "Office", new DateTime(2020, 3, 15, 10, 0, 0)),
                new Meeting("รักษารากฟัน ครั้งที่ 2", "Office", new DateTime(2020, 3, 25, 10, 0, 0)),
                new Meeting("นัดขูดหินปูน", "Online", new DateTime(2020, 4, 5, 18, 0, 0))
            };

        }
    }
}
