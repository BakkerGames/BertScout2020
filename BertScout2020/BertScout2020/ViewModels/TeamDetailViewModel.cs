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
        public int TotalHatches = 0;
        public int TotalCargo = 0;
        public int AverageHatches = 0;
        public int AverageCargo = 0;

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
                    int hatchCount = CalculateHatchCount(match);
                    int cargoCount = CalculateCargoCount(match);
                    // show match results
                    obj.Text1 = $"Match {match.MatchNumber} -" +
                        $" Score: {matchScore} RP: {matchRP}" +
                        $" Hatch: {hatchCount} Cargo: {cargoCount}";
                    string broken = "";
                    if (match.Broken == 1)
                    {
                        broken= "Broken: Some ";
                    }
                    else if (match.Broken == 2)
                    {
                        broken= "Broken: Lots ";
                    }
                    obj.Text2 = broken + match.Comments;
                    if (matchRP > 0 || matchScore > 0 || match.Broken > 0 || hatchCount > 0 || cargoCount > 0)
                    {
                        TotalRP += matchRP;
                        TotalScore += matchScore;
                        TotalHatches += hatchCount;
                        TotalCargo += cargoCount;
                        MatchCount++;
                        MatchResults.Add(obj);
                    }
                }
                AverageScore = TotalScore / MatchCount;
                AverageHatches = TotalHatches / MatchCount;
                AverageCargo = TotalCargo / MatchCount;
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

        private int CalculateCargoCount(EventTeamMatch match)
        {
            int result = 0;
            result += match.AutoHighCell;
            result += match.TeleLowCell;
            result += match.TeleInnerCell;
            return result;
        }

        private int CalculateHatchCount(EventTeamMatch match)
        {
            int result = 0;
            result += match.AutoLowCell;
            result += match.AutoInnerCell;
            result += match.TeleHighCell;
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
            score += match.AutoLeaveInitLine * 3;
            score += match.AutoLowCell * 2;
            score += match.AutoHighCell * 3;

            score += match.AutoInnerCell * 2;
            score += match.TeleLowCell * 3;
            score += match.TeleHighCell * 2;
            score += match.TeleInnerCell * 3;
            //not scoring highest platform
            //score += match.RotationControl;
            //score += match.PositionControl;

            //score += match.ClimbStatus;
            switch (match.ClimbStatus)
            {
                case 1:
                    score += 3;
                    break;
                case 2:
                    score += 6;
                    break;
                case 3:
                    score += 12;
                    break;
            }
            //not scoring buddy climb
            //score += match.LevelSwitch;

            //score += match.Defense;
            //score += match.Cooperation;
            score -= match.Fouls * 3;
            //score -= match.Broken*20;

            return score;
        }
    }
}
