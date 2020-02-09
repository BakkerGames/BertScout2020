using BertScout2020Data.Models;
using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BertScout2020.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FixMatchPage : ContentPage
    {
        private EventTeamMatch match;

        private readonly List<Team> Teams;

        public FixMatchPage(EventTeamMatch item)
        {
            InitializeComponent();
            match = item;
            Teams = App.Database.GetTeamsByEventAsync(App.currFRCEventKey).Result;
            int savePickerIndex = -1;
            for (int i = 0; i < Teams.Count; i++)
            {
                Team t = Teams[i];
                Picker_TeamNumber.Items.Add(t.TeamNumberDashName);
                if (t.TeamNumber == match.TeamNumber)
                {
                    savePickerIndex = i;
                }
            }
            Entry_ScouterName.Text = match.ScouterName;
            Picker_TeamNumber.SelectedIndex = savePickerIndex;
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
            dummyTeamNumber = Teams[Picker_TeamNumber.SelectedIndex].TeamNumber;
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
        }
    }
}