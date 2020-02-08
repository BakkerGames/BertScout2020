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
    public partial class FixMatchPage : ContentPage
    {
        private EventTeamMatch match;
        public FixMatchPage(EventTeamMatch item)
        {
            InitializeComponent();
            match = item;
            Entry_ScouterName.Text = match.ScouterName;
            Entry_TeamNumber.Text = match.TeamNumber.ToString();
            Entry_MatchNumber.Text = match.MatchNumber.ToString();
        }

        private void ToolbarItem_Save_Clicked(object sender, EventArgs e)
        {
            int dummyTeamNumber = 0;
            int dummyMatchNumber = 0;
            string dummyScouterName = "";

            try
            {
                dummyScouterName = Entry_ScouterName.Text;
                if (string.IsNullOrEmpty(dummyScouterName))
                {
                    throw new SystemException();
                }
            }
            catch (Exception)
            {
                Label_ErrorMessage.Text = "Invalid Scouter Name";
                return;
            }
            try
            {
                dummyTeamNumber = int.Parse(Entry_TeamNumber.Text);
                if (dummyTeamNumber <= 0 || dummyTeamNumber > 9999)
                {
                    throw new SystemException();
                }
            }
            catch (Exception)
            {
                Label_ErrorMessage.Text = "Invalid Team Number";
                return;
                //TODO if team not in event, add it (if doesn't exist?
            }
            try
            {
                dummyMatchNumber = int.Parse(Entry_MatchNumber.Text);
                if (dummyMatchNumber <= 0 || dummyMatchNumber > 999)
                {
                    throw new SystemException();
                }
            }
            catch (Exception)
            {
                Label_ErrorMessage.Text = "Invalid Match Number";
                return;
            }
            match.TeamNumber = dummyTeamNumber;
            match.MatchNumber = dummyMatchNumber;
            match.ScouterName = dummyScouterName;
            App.database.SaveEventTeamMatchAsync(match);
            Label_ErrorMessage.Text = "Save Complete - Please exit to team selection page";
            //Navigation.PopAsync();

        }
    }
}