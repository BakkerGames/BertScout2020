using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CreateEventTeamXML
{
    public partial class FormMain : Form
    {
        public FormMain()
        {
            InitializeComponent();
        }

        private void buttonStart_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBoxFrom.Text) ||
                string.IsNullOrEmpty(textBoxTo.Text) ||
                string.IsNullOrEmpty(textBoxEvent.Text))
            {
                return;
            }
            if (!File.Exists(textBoxFrom.Text))
            {
                return;
            }
            string[] teams = File.ReadAllLines(textBoxFrom.Text);
            StringBuilder result = new StringBuilder();
            result.AppendLine("<?xml version=\"1.0\" encoding=\"utf-8\" ?>");
            result.AppendLine("<ArrayOfEventTeam xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">");
            foreach (string team in teams)
            {
                if (string.IsNullOrEmpty(team))
                {
                    continue;
                }
                result.AppendLine("  <EventTeam>");
                result.Append("    <Uuid>");
                result.Append(Guid.NewGuid());
                result.AppendLine("</Uuid>");
                result.Append("    <EventKey>");
                result.Append(textBoxEvent.Text);
                result.AppendLine("</EventKey>");
                result.Append("    <TeamNumber>");
                result.Append(team);
                result.AppendLine("</TeamNumber>");
                result.AppendLine("  </EventTeam>");
            }
            result.AppendLine("</ArrayOfEventTeam>");
            // write out results
            File.WriteAllText(textBoxTo.Text, result.ToString());
            textBoxResults.Text = result.ToString();
        }
    }
}
