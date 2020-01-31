using BertScout2020Data.Models;

namespace BertScout2020.ViewModels
{
    public class EditEventTeamMatchViewModel : BaseViewModel
    {
        public EventTeamMatch item;

        public EditEventTeamMatchViewModel(EventTeamMatch item)
        {
            this.item = item;
            Title = $"Team {App.currTeamNumber} - Match {App.currMatchNumber} - {this.item.ScouterName}";
        }
    }
}
