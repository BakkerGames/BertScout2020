using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BertScout2020.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PicturePage : ContentPage
    {
        public PicturePage()
        {
            InitializeComponent();
        }

        private void Button_TakePicture_Clicked(object sender, EventArgs e)
        {
            Button_TakePicture.BackgroundColor = App.SelectedButtonColor;
            //https://docs.microsoft.com/en-us/xamarin/android/app-fundamentals/permissions?tabs=windows
        }
    }
}