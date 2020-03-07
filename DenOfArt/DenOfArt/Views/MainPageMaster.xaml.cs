﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DenOfArt.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPageMaster : ContentPage
    {
        public ListView ListView;

        public MainPageMaster()
        {
            InitializeComponent();

            BindingContext = new MainPageMasterViewModel();
            ListView = MenuItemsListView;
        }

        class MainPageMasterViewModel : INotifyPropertyChanged
        {
            public ObservableCollection<MainPageMasterMenuItem> MenuItems { get; set; }

            public MainPageMasterViewModel()
            {
                MenuItems = new ObservableCollection<MainPageMasterMenuItem>(new[]
                {
                    new MainPageMasterMenuItem { Id = 0, Title = "หน้าหลัก", IconSource="home_b.png", TargetType = typeof(HomePage)},
                    new MainPageMasterMenuItem { Id = 1, Title = "โปรไฟล์", IconSource="account_b.png", TargetType = typeof(ProfilePage)},
                    new MainPageMasterMenuItem { Id = 2, Title = "ข้อมูลการรักษา", IconSource="list_b.png", TargetType = typeof(HomePage)},
                    new MainPageMasterMenuItem { Id = 3, Title = "ข้อมูลการนัดหมาย", IconSource="date_b.png", TargetType = typeof(HomePage)},
                    new MainPageMasterMenuItem { Id = 4, Title = "ตั้งค่า", IconSource="setting_b.png", TargetType = typeof(HomePage)},
                    new MainPageMasterMenuItem { Id = 5, Title = "ออกจากระบบ", IconSource="logout_b.png"},
                });
            }

            #region INotifyPropertyChanged Implementation
            public event PropertyChangedEventHandler PropertyChanged;
            void OnPropertyChanged([CallerMemberName] string propertyName = "")
            {
                if (PropertyChanged == null)
                    return;

                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
            #endregion
        }

    }
}