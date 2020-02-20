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

        private async void Button_TakePicture_Clicked(object sender, EventArgs e)
        {
            Button_TakePicture.BackgroundColor = App.SelectedButtonColor;
            //https://docs.microsoft.com/en-us/xamarin/android/app-fundamentals/permissions?tabs=windows
            //https://xamarinhelp.com/use-camera-take-photo-xamarin-forms/
            //https://github.com/adamped/CameraXF/blob/master/CameraSample/CameraSample/MainPage.xaml.cs
            var photo = await Plugin.Media.CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions() { });

            if (photo != null)
            {
                Image_Display.Source = ImageSource.FromStream(() => { return photo.GetStream(); });
            }
        }
    }
}