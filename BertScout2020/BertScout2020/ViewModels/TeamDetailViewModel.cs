using BertScout2020.Models;
using BertScout2020.Services;
using BertScout2020Data.Models;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace BertScout2020.ViewModels
{
    public class TeamDetailViewModel : BaseViewModel
    {
        public string savedEventKey;
        public Team savedTeam;

        public int TotalRP = 0;
        public int TotalScore = 0;
        public int MatchCount = 0;
        public int AverageScore = 0;
        public int TotalPowercells = 0;
        public int AveragePowercells = 0;

        public IDataStore<EventTeamMatch> DataStoreMatch;

        public ObservableCollection<MatchResult> MatchResults { get; set; }

        public TeamDetailViewModel(string eventKey, Team item)
        {
            savedEventKey = eventKey;
            savedTeam = item;
            Title = $"Team {App.currTeamNumber} Details";
            MatchResults = new ObservableCollection<MatchResult>();
            DataStoreMatch = new SqlDataStoreEventTeamMatches(eventKey, item.TeamNumber);
            ExecuteLoadEventTeamMatchesCommand();
        }

        public void ExecuteLoadEventTeamMatchesCommand()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                MatchResults.Clear();
                var matches = DataStoreMatch.GetItemsAsync(true).Result;
                foreach (var match in matches)
                {
                    MatchResult obj = new MatchResult();
                    obj.EventKey = match.EventKey;
                    obj.TeamNumber = match.TeamNumber;
                    obj.MatchNumber = match.MatchNumber;
                    int matchRP = CalculateMatchRP(match);
                    int matchScore = CalculateMatchResult(match);
                    int powercellCount = CalculatePowercellCount(match);
                    // show match results
                    obj.Text1 = $"Match {match.MatchNumber} -" +
                        $" Score: {matchScore} RP: {matchRP}" +
                        $" Powercell: {powercellCount}";
                    string broken = "";
                    if (match.Broken == 1)
                    {
                        broken= "Broken ";
                    }
                   
                    obj.Text2 = broken + match.Comments;
                    if (matchRP > 0 || matchScore > 0 || match.Broken > 0 || powercellCount > 0) 
                    {
                        TotalRP += matchRP;
                        TotalScore += matchScore;
                        TotalPowercells += powercellCount;
                        MatchCount++;
                        MatchResults.Add(obj);
                    }
                }
                AverageScore = TotalScore / MatchCount;
                AveragePowercells = TotalPowercells / MatchCount;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }

      

        private int CalculatePowercellCount(EventTeamMatch match)
        {
            int result = 0;
            result += match.AutoBottomCell;
            result += match.AutoInnerCell;
            result += match.AutoOuterCell;
            result += match.TeleOuterCell;
            result += match.TeleBottomCell;
            result += match.TeleInnerCell;
            return result;
        }

        private int CalculateMatchRP(EventTeamMatch match)
        {
            int rp = 0;
            rp += match.AllianceResult;
            rp += match.StageRankingPoint;
            rp += match.ClimbRankingPoint;
            return rp;
        }

        private int CalculateMatchResult(EventTeamMatch match)
        {
            int score = 0;
            //not scoring movement type
            //score += match.AutoStartPos;
            score += match.AutoLeaveInitLine * 5;
            score += match.AutoBottomCell * 2;
            score += match.AutoOuterCell * 4;

            score += match.AutoInnerCell * 6;
            score += match.TeleBottomCell * 1;
            score += match.TeleOuterCell * 2;
            score += match.TeleInnerCell * 3;
            //not scoring highest platform
            score += match.RotationControl * 10;
            score += match.PositionControl * 20;

            //score += match.ClimbStatus;
            switch (match.ClimbStatus)
            {
                case 1:
                    score += 5;
                    break;
                case 2:
                    score += 25;
                    break;
                case 3:
                    score += 25;
                    break;
            }
            //not scoring buddy climb
            score += match.LevelSwitch * 15;

            //score += match.Defense;
            //score += match.Cooperation;
            score -= match.Fouls * 3;
            //score -= match.Broken*20;

            return score;
        }
    }
}
