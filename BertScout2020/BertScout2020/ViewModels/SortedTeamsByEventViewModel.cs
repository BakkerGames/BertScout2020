using BertScout2020.Models;
using BertScout2020.Services;
using BertScout2020Data.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Xamarin.Forms;

namespace BertScout2020.ViewModels
{
    public class SortedTeamsByEventViewModel : BaseViewModel
    {
        public IDataStore<Team> DataStoreTeam => new SqlDataStoreTeams(App.currFRCEventKey);

        public ObservableCollection<TeamResult> TeamResults { get; set; }
        public Command LoadEventTeamsCommand { get; set; }

        public SortedTeamsByEventViewModel()
        {
            Title = App.currFRCEventName;
            TeamResults = new ObservableCollection<TeamResult>();
            ExecuteLoadEventTeamsCommand();
        }

        public void ExecuteLoadEventTeamsCommand()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                TeamResults.Clear();
                var teams = DataStoreTeam.GetItemsAsync(true).Result;
                foreach (var team in teams)
                {
                    TeamDetailViewModel matchViewModel = new TeamDetailViewModel(App.currFRCEventKey, team);
                    TeamResult teamResult = new TeamResult();
                    teamResult.TeamNumber = team.TeamNumber;
                    teamResult.Name = team.Name;
                    teamResult.TotalRP = matchViewModel.TotalRP;
                    teamResult.TotalScore = matchViewModel.TotalScore;
                    teamResult.AverageScore = matchViewModel.AverageScore;
                    teamResult.TotalPowercells = matchViewModel.TotalPowercells;
                    teamResult.AveragePowercells = matchViewModel.AveragePowercells;
                    teamResult.TotalBroken = matchViewModel.TotalBroken;


                    TeamResults.Add(teamResult);
                }
            }
            catch (Exception ex)
            {
                Title = ex.Message;
            }
            finally
            {
                IsBusy = false;
            }
        }

        internal void SortByTeamNumber()
        {
            List<TeamResult> ordered = TeamResults.OrderBy(o => o.TeamNumber).ToList();
            TeamResults.Clear();
            foreach (TeamResult item in ordered)
            {
                TeamResults.Add(item);
            }
        }

        //FIXME - still sorts by avg. score
        public void SortByTotalScore()
        {
            List<TeamResult> ordered = TeamResults.OrderByDescending(o => o.TotalScore).ToList();
            TeamResults.Clear();
            foreach (TeamResult item in ordered)
            {
                TeamResults.Add(item);
            }
        }

        public void SortByAverageScore()
        {
            List<TeamResult> ordered = TeamResults.OrderByDescending(o => o.AverageScore).ToList();
            TeamResults.Clear();
            foreach (TeamResult item in ordered)
            {
                TeamResults.Add(item);
            }
        }

        public void SortByRankingPoints()
        {
            List<TeamResult> ordered = TeamResults.OrderByDescending(o => o.TotalRP).ToList();
            TeamResults.Clear();
            foreach (TeamResult item in ordered)
            {
                TeamResults.Add(item);
            }
        }

        public void SortByPowercellCount()
        {
            List<TeamResult> ordered = TeamResults.OrderByDescending(o => o.TotalPowercells).ToList();
            TeamResults.Clear();
            foreach (TeamResult item in ordered)
            {
                TeamResults.Add(item);
            }
        }

        internal void SortByAveragePowercells()
        {
            List<TeamResult> ordered = TeamResults.OrderByDescending(o => o.AveragePowercells).ToList();
            TeamResults.Clear();
            foreach (TeamResult item in ordered)
            {
                TeamResults.Add(item);
            }
        }

        //FIXME - still sorts by avg. pow. cells
        internal void SortByBroken()
        {
            List<TeamResult> ordered = TeamResults.OrderBy(o => o.TotalBroken).ToList();
            TeamResults.Clear();
            foreach (TeamResult item in ordered)
            {
                TeamResults.Add(item);
            }
        }


    }
}
