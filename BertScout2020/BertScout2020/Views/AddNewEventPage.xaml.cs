using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BertScout2020.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddNewEventPage : ContentPage
    {
        public AddNewEventPage()
        {
            InitializeComponent();
        }

        private bool _addNewEventBusy = false;
        private void Button_AddNewEvent_Clicked(object sender, EventArgs e)
        {
            // prevent multiple clicks at once
            if (_addNewEventBusy)
            {
                return;
            }
            _addNewEventBusy = true;
            doAddNewEvent();
            _addNewEventBusy = false;
        }

        private void doAddNewEvent()
        {
            string eventName = Entry_EventName.Text;
            string eventKey = Entry_EventKey.Text;
            string eventLocation = Entry_EventLocation.Text;
            string startDate = Start_DatePicker.Date.ToString();
            string endDate = End_DatePicker.Date.ToString();

            if (string.IsNullOrEmpty(eventName) || 
                string.IsNullOrEmpty(eventKey) || 
                string.IsNullOrEmpty(eventLocation))
            {
                this.Title = "Please fill out all fields.";
                return;
            }

            //add new event - does it already exist?
            foreach (FRCEvent existing in viewModel.Events)
            {
                if (existing.EventKey == eventKey)
                {
                    this.Title = $"Event {eventName} already exists.";
                    return;
                }
            }

            //TODO: sort by date and paste into event list page


            Entry_EventName.Text = "";
            Entry_EventKey.Text = "";
            Entry_EventLocation.Text = "";
            this.Title = $"Added new event {Entry_EventName.Text}";
        }
    }
}