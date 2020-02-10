using Common.JSON;

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
                { "AutoBottomCell", AutoBottomCell },
                { "AutoOuterCell", AutoOuterCell },
                { "AutoInnerCell", AutoInnerCell },
                { "TeleBottomCell", TeleBottomCell },
                { "TeleOuterCell", TeleOuterCell },
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
                { "DeviceName", DeviceName },
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
                Id = (int?)(item.GetValueOrNull("Id")),
                Uuid = (string)(item.GetValueOrNull("Uuid")),
                Changed = (int)(item.GetValueOrNull("Changed") ?? 0),
                EventKey = (string)(item.GetValueOrNull("EventKey") ?? ""),
                TeamNumber = (int)(item.GetValueOrNull("TeamNumber") ?? 0),
                MatchNumber = (int)(item.GetValueOrNull("MatchNumber") ?? 0),
                AutoStartPos = (int)(item.GetValueOrNull("AutoStartPos") ?? 0),
                AutoLeaveInitLine = (int)(item.GetValueOrNull("AutoLeaveInitLine") ?? 0),
                AutoBottomCell = (int)(item.GetValueOrNull("AutoBottomCell") ?? 0),
                AutoOuterCell = (int)(item.GetValueOrNull("AutoOuterCell") ?? 0),
                AutoInnerCell = (int)(item.GetValueOrNull("AutoInnerCell") ?? 0),
                TeleBottomCell = (int)(item.GetValueOrNull("TeleBottomCell") ?? 0),
                TeleOuterCell = (int)(item.GetValueOrNull("TeleOuterCell") ?? 0),
                TeleInnerCell = (int)(item.GetValueOrNull("TeleInnerCell") ?? 0),
                RotationControl = (int)(item.GetValueOrNull("RotationControl") ?? 0),
                PositionControl = (int)(item.GetValueOrNull("PositionControl") ?? 0),
                ClimbStatus = (int)(item.GetValueOrNull("ClimbStatus") ?? 0),
                LevelSwitch = (int)(item.GetValueOrNull("LevelSwitch") ?? 0),
                Fouls = (int)(item.GetValueOrNull("Fouls") ?? 0),
                Broken = (int)(item.GetValueOrNull("Broken") ?? 0),
                AllianceResult = (int)(item.GetValueOrNull("AllianceResult") ?? 0),
                StageRankingPoint = (int)(item.GetValueOrNull("StageRankingPoint") ?? 0),
                ClimbRankingPoint = (int)(item.GetValueOrNull("ClimbRankingPoint") ?? 0),
                ScouterName = (string)(item.GetValueOrNull("ScouterName") ?? ""),
                Comments = (string)(item.GetValueOrNull("Comments") ?? ""),
                DeviceName = (string)(item.GetValueOrNull("DeviceName") ?? "")
            };
            return result;
        }
    }
}