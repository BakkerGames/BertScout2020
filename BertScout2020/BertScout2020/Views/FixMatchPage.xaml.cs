using BertScout2020.Services;
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

        private readonly List<Team> Teams;
        private readonly SortedList<int, string> TeamNumName;

        public FixMatchPage(EventTeamMatch item)
        {
            InitializeComponent();
            match = item;
            Teams = App.Database.GetTeamsByEventAsync(App.currFRCEventKey).Result;
            TeamNumName = new SortedList<int, string>();
            foreach (Team t in Teams)
            {
                string teamPadded = $"    {t.TeamNumber}";
                teamPadded = teamPadded.Substring(teamPadded.Length - 4);
                TeamNumName.Add(t.TeamNumber, $"{teamPadded} - {t.Name}");
            }
            int savePickerIndex = -1;
            for (int i = 0; i < TeamNumName.Count; i++)
            {
                string value = TeamNumName.Values[i];
                Picker_TeamNumber.Items.Add(value);
                if (TeamNumName.Keys[i] == match.TeamNumber)
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
            dummyTeamNumber = int.Parse(Picker_TeamNumber.SelectedItem.ToString().Substring(0, 4));
            //try
            //{
            //    dummyTeamNumber = int.Parse(Entry_TeamNumber.Text);
            //    if (dummyTeamNumber <= 0 || dummyTeamNumber > 9999)
            //    {
            //        throw new SystemException();
            //    }
            //}
            //catch (Exception)
            //{
            //    Label_ErrorMessage.Text = "Invalid Team Number";
            //    return;
            //    //TODO if team not in event, add it (if doesn't exist?

            //}
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