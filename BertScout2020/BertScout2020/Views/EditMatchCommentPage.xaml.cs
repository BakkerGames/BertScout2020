using BertScout2020.ViewModels;
using BertScout2020Data.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BertScout2020.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EditMatchCommentPage : ContentPage
    {
        bool _loadingFlag = false;
        EditEventTeamMatchViewModel viewModel;

        public EditMatchCommentPage(EventTeamMatch item)
        {
            InitializeComponent();

            if (item.Changed % 2 == 0) // change from even to odd, odd = must upload
            {
                item.Changed++;
            }

            BindingContext = viewModel = new EditEventTeamMatchViewModel(item);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            _loadingFlag = true;
            Editor_MatchComment.Text = viewModel.item.Comments ?? "";
            ErrorMessage.Text = "";
            _loadingFlag = false;
            Editor_MatchComment.Focus();
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            SaveComments();
        }

        private void ToolbarItem_Save_Clicked(object sender, System.EventArgs e)
        {
            if (Editor_MatchComment.Text.ToLower() == App.DeleteMatchPassword.ToLower())
            {
                // delete the match record
                ErrorMessage.Text = "Deleting the match record...";
                if (viewModel.item.Id.HasValue)
                {
                    App.database.DeleteEventTeamMatchAsync(viewModel.item.Id.Value);
                    ErrorMessage.Text = "Match record has been deleted";
                }
                else
                {
                    ErrorMessage.Text = "Nothing to do, match record not saved yet";
                }
            }
            else
            {
                // don't delete the match
                SaveComments();
                if (string.IsNullOrEmpty(viewModel.item.ScouterName))
                {
                    viewModel.item.ScouterName = App.currScouterName;
                    Title = $"Team {App.currTeamNumber} - Match {App.currMatchNumber} - {viewModel.item.ScouterName}";

                }
            }
        }

        private void SaveComments()
        {
            try
            {
                if (viewModel.item.Comments != Editor_MatchComment.Text)
                {
                    viewModel.item.Comments = Editor_MatchComment.Text?.Trim();
                    App.database.SaveEventTeamMatchAsync(viewModel.item);
                }
                ErrorMessage.Text = "";
            }
            catch (System.Exception ex)
            {
                ErrorMessage.Text = ex.Message;
            }
        }

        private void Editor_MatchComment_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!_loadingFlag && string.IsNullOrEmpty(ErrorMessage.Text))
            {
                ErrorMessage.Text = "(Not Saved)";
            }
        }
    }
}
