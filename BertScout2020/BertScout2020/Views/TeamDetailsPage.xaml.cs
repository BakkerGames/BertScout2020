using BertScout2020.Models;
using BertScout2020.ViewModels;
using BertScout2020Data.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BertScout2020.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TeamDetailsPage : ContentPage
    {
        TeamDetailViewModel viewModel;

        public TeamDetailsPage(string eventKey, Team item)
        {
            InitializeComponent();

            BindingContext = viewModel = new TeamDetailViewModel(eventKey, item);
        }

        protected override void OnAppearing()
        {
            TeamDetails_Number.Text = viewModel.savedTeam.TeamNumber.ToString(); //Show Team Number ex: 133
            if (viewModel.savedTeam.Name.Length > 28)
            {
            TeamDetails_Name.Text = viewModel.savedTeam.Name.Substring(0,28); // Show Team Name ex: B.E.R.T.
            }
            else
            {

            TeamDetails_Name.Text = viewModel.savedTeam.Name; // Show Team Name ex: B.E.R.T.
            }

            if(viewModel.savedTeam.Location.Length > 28)
            {
                TeamDetails_Location.Text = viewModel.savedTeam.Location.Substring(0,28); //Show Location ex: Standish, ME, USA
            }
            else
            {

            TeamDetails_Location.Text = viewModel.savedTeam.Location; //Show Location ex: Standish, ME, USA
            }
            TeamDetails_RP.Text = viewModel.TotalRP.ToString(); //Show total RP
            TeamDetails_AVGS.Text = viewModel.AverageScore.ToString(); //Show Average Score
        }

        private async void TeamMatchesListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            MatchResult item = (MatchResult)e.SelectedItem;
            if (item == null)
            {
                return;
            }
            // key = "EventKey|TeamNumber|MatchNumber"
            string key = $"{item.EventKey}|{item.TeamNumber}|{item.MatchNumber}";
            EventTeamMatch itemMatch = viewModel.DataStoreMatch.GetItemByKeyAsync(key).Result;
            await Navigation.PushAsync(new EditEventTeamMatchPage(itemMatch));
        }
    }
}