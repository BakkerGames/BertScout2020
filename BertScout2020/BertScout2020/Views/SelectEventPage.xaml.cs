using BertScout2020.ViewModels;
using BertScout2020Data.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BertScout2020.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SelectEventPage : ContentPage
    {
        SelectEventsViewModel viewModel;
        bool _preparing = false;

        public SelectEventPage()
        {
            InitializeComponent();

            BindingContext = viewModel = new SelectEventsViewModel();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            _preparing = true;

            if (viewModel.FRCEvents.Count == 0)
                viewModel.LoadFRCEventsCommand.Execute(null);

            foreach (FRCEvent item in viewModel.FRCEvents)
            {
                if (item.EventKey == App.currFRCEventKey)
                {
                    FRCEventsListView.SelectedItem = item;
                    break;
                }
            }

            _preparing = false;
        }

        void OnItemSelected(object sender, SelectedItemChangedEventArgs args)
        {
            FRCEvent item = (FRCEvent)args.SelectedItem;
            if (item == null)
                return;

            App.currFRCEventKey = item.EventKey;
            App.currFRCEventName = item.Name;
            App.highestMatchNumber = 0;

            this.Title = item.Name;

            if (_preparing)
            {
                return;
            }

            Navigation.PopAsync();
        }
    }
}