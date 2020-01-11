﻿using Common.JSON;

namespace BertScout2020Data.Models
{
    public partial class EventTeamMatch
    {
        // ToJson and ToString routines

        public JObject ToJson()
        {
            JObject result = new JObject()
            {
                { "Id", Id },
                { "Uuid", Uuid },
                { "Changed", Changed },
                { "EventKey", EventKey },
                { "TeamNumber", TeamNumber },
                { "MatchNumber", MatchNumber },
                { "AutoStartPos", AutoStartPos },
                { "AutoLeaveInitLine", AutoLeaveInitLine },
                { "AutoLowCell", AutoLowCell },
                { "AutoHighCell", AutoHighCell },
                { "AutoInnerCell", AutoInnerCell },
                { "TeleLowCell", TeleLowCell },
                { "TeleHighCell", TeleHighCell },
                { "TeleInnerCell", TeleInnerCell },
                { "RotationControl", RotationControl },
                { "PositionControl", PositionControl },
                { "ClimbStatus", ClimbStatus },
                { "LevelSwitch", LevelSwitch },
                { "Fouls", Fouls },
                { "Broken", Broken },
                { "AllianceResult", AllianceResult },
                { "StageRankingPoint", StageRankingPoint },
                { "ClimbRankingPoint", ClimbRankingPoint },
                { "ScouterName", ScouterName },
                { "Comments", Comments },
            };
            return result;
        }

        public override string ToString()
        {
            return ToJson().ToString();
        }

        public static EventTeamMatch Parse(string value)
        {
            JObject item = JObject.Parse(value);
            EventTeamMatch result = new EventTeamMatch()
            {
                Id = (int?)item.GetValueOrNull("Id"),
                Uuid = (string)item.GetValue("Uuid"),
                Changed = (int)item.GetValue("Changed"),
                EventKey = (string)item.GetValue("EventKey"),
                TeamNumber = (int)item.GetValue("TeamNumber"),
                MatchNumber = (int)item.GetValue("MatchNumber"),
                AutoStartPos = (int)item.GetValue("AutoStartPos"),
                AutoLeaveInitLine = (int)item.GetValue("AutoLeaveInitLine"),
                AutoLowCell = (int)item.GetValue("AutoLowCell"),
                AutoHighCell = (int)item.GetValue("AutoHighCell"),
                AutoInnerCell = (int)item.GetValue("AutoInnerCell"),
                TeleLowCell = (int)item.GetValue("TeleLowCell"),
                TeleHighCell = (int)item.GetValue("TeleHighCell"),
                TeleInnerCell = (int)item.GetValue("TeleInnerCell"),
                RotationControl = (int)item.GetValue("RotationControl"),
                PositionControl = (int)item.GetValue("PositionControl"),
                ClimbStatus = (int)item.GetValue("ClimbStatus"),
                LevelSwitch = (int)item.GetValue("LevelSwitch"),
                Fouls = (int)item.GetValue("Fouls"),
                Broken = (int)item.GetValue("Broken"),
                AllianceResult = (int)item.GetValue("AllianceResult"),
                StageRankingPoint = (int)item.GetValue("StageRankingPoint"),
                ClimbRankingPoint = (int)item.GetValue("ClimbRankingPoint"),
                ScouterName = (string)item.GetValue("ScouterName"),
                Comments = (string)item.GetValue("Comments"),
            };
            return result;
        }
    }
}