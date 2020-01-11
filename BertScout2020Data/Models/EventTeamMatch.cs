using SQLite;
using System;

// remember to increment the dbVersion in App.xaml.cs when changing this model

namespace BertScout2020Data.Models
{
    public partial class EventTeamMatch : IEquatable<EventTeamMatch>, IComparable<EventTeamMatch>
    {
        [PrimaryKey, AutoIncrement]
        public int? Id { get; set; }
        public string Uuid { get; set; }
        public int Changed { get; set; }
        [Indexed]
        public string EventKey { get; set; }
        [Indexed]
        public int TeamNumber { get; set; }
        [Indexed]
        public int MatchNumber { get; set; }
        public int AutoStartPos { get; set; }
        public int AutoLeaveInitLine { get; set; }
        public int AutoLowCell { get; set; }
        public int AutoHighCell { get; set; }
        public int AutoInnerCell { get; set; }
        public int TeleLowCell { get; set; }
        public int TeleHighCell { get; set; }
        public int TeleInnerCell { get; set; }
        public int RotationControl { get; set; }
        public int PositionControl { get; set; }
        public int ClimbStatus { get; set; }
        public int LevelSwitch { get; set; }
        public int Fouls { get; set; }
        public int Broken { get; set; }
        public int AllianceResult { get; set; }
        public int StageRankingPoint { get; set; }
        public int ClimbRankingPoint { get; set; }
        public string ScouterName { get; set; }
        public string Comments { get; set; }

        public int CompareTo(EventTeamMatch other)
        {
            if (other == null) return 1; // this is greater
            if (ReferenceEquals(this, other)) return 0; // same object
            if (Id == null)
            {
                if (other.Id == null) return 0; // equal
                return -1; // other is greater
            }
            if (other.Id == null) return 1; // this is greater
            return Id.Value.CompareTo(other.Id.Value);
        }

        public bool Equals(EventTeamMatch other)
        {
            if (other == null) return false;
            if (ReferenceEquals(this, other)) return true; // same object
            if (!Id.HasValue || !other.Id.HasValue) return false; // one or both is null
            return Id.Value.Equals(other.Id.Value);
        }
    }
}
