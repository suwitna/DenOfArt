using System;
using Syncfusion.SfCalendar.XForms;
using Xamarin.Forms;

namespace DenOfArt.Model
{
    class CalendarViewModel
    {
        public CalendarEventCollection CalendarInlineEvents { get; set; } = new CalendarEventCollection();
        public CalendarViewModel()
        {
            CalendarInlineEvent event1 = new CalendarInlineEvent();
            event1.StartTime = new DateTime(2017, 5, 1, 5, 0, 0);
            event1.EndTime = new DateTime(2017, 5, 1, 7, 0, 0);
            event1.Subject = "Go to Meeting";
            event1.Color = Color.Fuchsia;

            CalendarInlineEvent event2 = new CalendarInlineEvent();
            event2.StartTime = new DateTime(2017, 5, 1, 10, 0, 0);
            event2.EndTime = new DateTime(2017, 5, 1, 12, 0, 0);
            event2.Subject = "Planning";
            event2.Color = Color.Green;

            CalendarInlineEvents.Add(event1);
            CalendarInlineEvents.Add(event2);
        }
    }
}
