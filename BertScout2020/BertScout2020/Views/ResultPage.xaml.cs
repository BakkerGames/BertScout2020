// For dropdown menu-use picker, info at https://docs.microsoft.com/en-us/xamarin/xamarin-forms/user-interface/picker/

using BertScout2020.Models;
using BertScout2020.ViewModels;
using BertScout2020Data.Models;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BertScout2020.Views
{

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ResultPage : ContentPage

    {
        SortedTeamsByEventViewModel viewModel;

        public ResultPage()
        {
            InitializeComponent();
            BindingContext = viewModel = new SortedTeamsByEventViewModel();
            TeamNumber.BackgroundColor = App.SelectedButtonColor;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            //EventTeamsListView.SelectedItem = null;
        }

        private async void EventTeamsListView_ItemSelected(object sender, SelectedItemChangedEventArgs args)
        {
            TeamResult item = (TeamResult)args.SelectedItem;
            if (item == null)
            {
                return;
            }
            App.currTeamNumber = item.TeamNumber;
            App.currTeamName = item.Name;
            Team itemTeam = viewModel.DataStoreTeam.GetItemByKeyAsync(item.TeamNumber.ToString()).Result;
            await Navigation.PushAsync(new TeamDetailsPage(App.currFRCEventKey, itemTeam));
        }

        private void TeamNumber_Clicked(object sender, EventArgs e)
        {
            ClearAllSortButtons();
            TeamNumber.BackgroundColor = App.SelectedButtonColor;
            viewModel.SortByTeamNumber();
        }

        private void RP_Clicked(object sender, EventArgs e)
        {
            ClearAllSortButtons();
            RP.BackgroundColor = App.SelectedButtonColor;
            viewModel.SortByRankingPoints();
        }

        private void Score_Clicked(object sender, EventArgs e)
        {
            ClearAllSortButtons();
            Score.BackgroundColor = App.SelectedButtonColor;
            viewModel.SortByTotalScore();
        }

        private void AvgScore_Clicked(object sender, EventArgs e)
        {
            ClearAllSortButtons();
            AvgScore.BackgroundColor = App.SelectedButtonColor;
            viewModel.SortByAverageScore();
        }

        private void PowercellCount_Clicked(object sender, EventArgs e)
        {
            ClearAllSortButtons();
            PowercellCount.BackgroundColor = App.SelectedButtonColor;
            viewModel.SortByPowercellCount();
        }

        private void Broken_Clicked(object sender, EventArgs e)
        {
            ClearAllSortButtons();
            Broken.BackgroundColor = App.SelectedButtonColor;
            viewModel.SortByBroken();
        }



        private void AveragePowercells_Clicked(object sender, EventArgs e)
        {
            ClearAllSortButtons();
            AveragePowercells.BackgroundColor = App.SelectedButtonColor;
            viewModel.SortByAveragePowercells();
        }

      

        private void ClearAllSortButtons()
        {
            TeamNumber.BackgroundColor = App.UnselectedButtonColor;
            RP.BackgroundColor = App.UnselectedButtonColor;
            AvgScore.BackgroundColor = App.UnselectedButtonColor;
            PowercellCount.BackgroundColor = App.UnselectedButtonColor;
            AveragePowercells.BackgroundColor = App.UnselectedButtonColor;
        }
    }
}