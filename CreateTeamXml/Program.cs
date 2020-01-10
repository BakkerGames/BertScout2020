using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CreateTeamXml
{
    class Program
    {
        private static string baseDir = "D:\\Projects\\BertScout2020\\BertScout2020XmlData\\EmbeddedResources";
        private static Dictionary<int, string> teamUuidDict = new Dictionary<int, string>();

        static void Main(string[] args)
        {
            string fileNameIn = "2020_Team_List.txt";
            string fileNameOldXml = "Teams.xml";
            string fileNameOut = "TeamsOut.xml";
            string[] teamsIn = File.ReadAllLines(Path.Combine(baseDir, fileNameIn));
            string[] teamsXmlOld = File.ReadAllLines(Path.Combine(baseDir, fileNameOldXml));
            string teamUuid = "";
            int teamNum;
            StringBuilder xmlOut = new StringBuilder();

            foreach (string line in teamsXmlOld)
            {
                string lineTrim = line.Trim();
                if (lineTrim.Contains("<Uuid>"))
                {
                    teamUuid = lineTrim.Substring(6, lineTrim.Length - 6 - 7);
                }
                else if (lineTrim.Contains("<TeamNumber>"))
                {
                    teamNum = int.Parse(lineTrim.Substring(12, lineTrim.Length - 12 - 13));
                    teamUuidDict.Add(teamNum, teamUuid);
                }
            }

            xmlOut.AppendLine( "<?xml version=\"1.0\" encoding=\"utf-8\"?>");
            xmlOut.AppendLine("<ArrayOfTeam xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">");

            foreach (string line in teamsIn)
            {
                string[] tokens = line.Split('\t');
                if (tokens[1] == "--")
                {
                    continue;
                }
                teamNum = int.Parse(tokens[0]);
                if (teamUuidDict.ContainsKey(teamNum))
                {
                    teamUuid = teamUuidDict[teamNum];
                }
                else
                {
                    teamUuid = Guid.NewGuid().ToString();
                }
                // do need to change & to &amp;
                string teamName = tokens[1];
                teamName = teamName.Replace("&","&amp;");
                xmlOut.AppendLine("  <Team>");
                xmlOut.AppendLine($"    <Uuid>{teamUuid}</Uuid>");
                xmlOut.AppendLine($"    <TeamNumber>{tokens[0]}</TeamNumber>");
                xmlOut.AppendLine($"    <Name>{teamName}</Name>");
                xmlOut.AppendLine($"    <Location>{tokens[2]}</Location>");
                xmlOut.AppendLine("  </Team>");
            }

            xmlOut.AppendLine("</ArrayOfTeam>");

            File.WriteAllText(Path.Combine(baseDir, fileNameOut), xmlOut.ToString());

            //Console.Write("Press enter to continue...");
            //Console.ReadLine();
        }
    }
}
