using DenOfArt.Model;
using DenOfArt.Service;
using System;
using System.Collections.Generic;
using System.Text;

namespace DenOfArt.ViewModels
{
    public class ViewModelBase
    {
        public IList<Meeting> Meetings { get; set; }

        public ViewModelBase()
        {
            Meetings = DataService.Instance.Data;
        }
    }
}
