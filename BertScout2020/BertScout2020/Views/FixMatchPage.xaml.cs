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
            match.ScouterName = Entry_ScouterName.Text;
            try
            {
                match.TeamNumber = int.Parse(Entry_TeamNumber.Text);
            }
            catch (Exception)
            {
                Label_ErrorMessage.Text = "Invalid Team Number";
                return;
            }
            try
            {
                match.MatchNumber = int.Parse(Entry_MatchNumber.Text);
            }
            catch (Exception)
            {
                Label_ErrorMessage.Text = "Invalid Match Number";
                return;
            }
            App.database.SaveEventTeamMatchAsync(match);
            Label_ErrorMessage.Text = "Save Complete";
            Navigation.PopAsync();

        }
    }
}