using BertScout2020Data.Models;
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
            this.Title = "Add New Event";
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

        async private void doAddNewEvent()
        {
            string eventName = Entry_EventName.Text;
            string eventKey = Entry_EventKey.Text;
            string eventLocation = Entry_EventLocation.Text;
            string startDate = Start_DatePicker.Date.ToString("yyyy-MM-dd");
            string endDate = End_DatePicker.Date.ToString("yyyy-MM-dd");

            if (Start_DatePicker.Date > End_DatePicker.Date)
            {
                Label_ErrorMessage.Text = "Invalid end or start date";
                return;
            }

            if (string.IsNullOrEmpty(eventName) ||
                string.IsNullOrEmpty(eventKey) ||
                string.IsNullOrEmpty(eventLocation))
            {
                Label_ErrorMessage.Text = "Please fill out all fields.";
                return;
            }

            //add new event - does it already exist?
            //TODO: new events are not added to the database, so the program cannot compare event keys
            //TODO: also doesn't work for events in the database
            FRCEvent oldEvent = null;
            try
            {
                oldEvent = await App.database.GetEventAsync(eventKey);
            }
            catch (Exception)
            {
                //do nothing
            }

            if (oldEvent != null && oldEvent.Id != null)
            {
                Label_ErrorMessage.Text = $"Event {eventName} already exists.";
                return;
            }

            FRCEvent newEvent = new FRCEvent();
            newEvent.EventKey = eventKey;
            newEvent.Name = eventName;
            newEvent.Location = eventLocation;
            newEvent.StartDate = startDate;
            newEvent.EndDate = endDate;
            newEvent.Changed = 1;
            await App.database.SaveFRCEventAsync(newEvent);

            Entry_EventName.Text = "";
            Entry_EventKey.Text = "";
            Entry_EventLocation.Text = "";
            Label_ErrorMessage.Text = $"Added new event {Entry_EventName.Text}.";
            Label_ErrorMessage2.Text = $"Please exit to main page or add another event.";
        }
    }
}