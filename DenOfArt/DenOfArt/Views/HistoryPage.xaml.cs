using DenOfArt.Model;
using DenOfArt.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DenOfArt.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HistoryPage : TabbedPage
    {
        public HistoryPage()
        {
            InitializeComponent();
            this.SelectedTabColor = Color.FromHex("#FFFFFF");
            this.UnselectedTabColor = Color.FromHex("#4C4C4C");
        }
        protected async override void OnAppearing()
        {

            base.OnAppearing();
            await GetHistoryData();
        }
        public async Task GetHistoryData()
        {
            List<AppointmentView> list = new List<AppointmentView>();
            AppointmentView view = new AppointmentView();
            view.HN = "หมายเลข HN: " + "25630023";
            view.AppointmentDate = DateTime.Now.ToString("dd/MM/yyyy");
            view.AppointmentTime = DateTime.Now.ToString("hh:mm tt");
            view.Subject = "เลื่อน " + view.AppointmentDate + " เวลา " + view.AppointmentTime;
            view.CustomerName = "คุณสมชาย ใจดี";
            view.Reason = "หมายเหตุ : ติดประชุน";
            view.Status = "รอการดำเนินการแล้ว";
            
            list.Add(view);

            view = new AppointmentView();
            view.HN = "หมายเลข HN: " + "25630023";
            view.AppointmentDate = DateTime.Now.ToString("dd/MM/yyyy");
            view.AppointmentTime = DateTime.Now.ToString("hh:mm tt");
            view.Subject = "เลื่อน " + view.AppointmentDate + " เวลา " + view.AppointmentTime;
            view.CustomerName = "คุณสมชาย ใจดี";
            view.Reason = "หมายเหตุ : ไม่สบาย";
            view.Status = "ดำเนินการแล้ว";

            list.Add(view);

            view = new AppointmentView();
            view.HN = "หมายเลข HN: " + "25630023";
            view.AppointmentDate = DateTime.Now.ToString("dd/MM/yyyy");
            view.AppointmentTime = DateTime.Now.ToString("hh:mm tt");
            view.Subject = "เลื่อน " + view.AppointmentDate + " เวลา " + view.AppointmentTime;
            view.CustomerName = "คุณสมชาย ใจดี";
            view.Reason = "หมายเหตุ : ไม่สบาย";
            view.Status = "ดำเนินการแล้ว";

            list.Add(view);

            view = new AppointmentView();
            view.HN = "หมายเลข HN: " + "25630023";
            view.AppointmentDate = DateTime.Now.ToString("dd/MM/yyyy");
            view.AppointmentTime = DateTime.Now.ToString("hh:mm tt");
            view.Subject = "เลื่อน " + view.AppointmentDate + " เวลา " + view.AppointmentTime;
            view.CustomerName = "คุณสมชาย ใจดี";
            view.Reason = "หมายเหตุ : ไม่สบาย";
            view.Status = "ดำเนินการแล้ว";

            list.Add(view);

            view = new AppointmentView();
            view.HN = "หมายเลข HN: " + "25630023";
            view.AppointmentDate = DateTime.Now.ToString("dd/MM/yyyy");
            view.AppointmentTime = DateTime.Now.ToString("hh:mm tt");
            view.Subject = "เลื่อน " + view.AppointmentDate + " เวลา " + view.AppointmentTime;
            view.CustomerName = "คุณสมชาย ใจดี";
            view.Reason = "หมายเหตุ : ไม่สบาย";
            view.Status = "ดำเนินการแล้ว";

            list.Add(view);

            view = new AppointmentView();
            view.HN = "หมายเลข HN: " + "25630023";
            view.AppointmentDate = DateTime.Now.ToString("dd/MM/yyyy");
            view.AppointmentTime = DateTime.Now.ToString("hh:mm tt");
            view.Subject = "เลื่อน " + view.AppointmentDate + " เวลา " + view.AppointmentTime;
            view.CustomerName = "คุณสมชาย ใจดี";
            view.Reason = "หมายเหตุ : ไม่สบาย";
            view.Status = "ดำเนินการแล้ว";

            list.Add(view);

            view = new AppointmentView();
            view.HN = "หมายเลข HN: " + "25630023";
            view.AppointmentDate = DateTime.Now.ToString("dd/MM/yyyy");
            view.AppointmentTime = DateTime.Now.ToString("hh:mm tt");
            view.Subject = "เลื่อน " + view.AppointmentDate + " เวลา " + view.AppointmentTime;
            view.CustomerName = "คุณสมชาย ใจดี";
            view.Reason = "หมายเหตุ : ไม่สบาย";
            view.Status = "ดำเนินการแล้ว";

            list.Add(view);
            listHistory.ItemsSource = list;
        }
    }
}