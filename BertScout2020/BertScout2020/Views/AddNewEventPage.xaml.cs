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
            Entry_EventName.Text = "";
            _addNewEventBusy = false;
        }

        private void doAddNewEvent()
        {
            if (string.IsNullOrEmpty(Entry_EventName.Text))
            {
                return;
            }
            this.Title = $"Added new event {Entry_EventName.Text}";

            /*// add new event - does it already exist?
            foreach (Team existing in viewModel.Teams)
            {
                if (existing.TeamNumber == newTeamNumber)
                {
                    this.Title = $"Team {newTeamNumber} is already in this event";
                    return;
                }
            }*/

            //TODO: sort by date and paste into event list page

        }
    }
}