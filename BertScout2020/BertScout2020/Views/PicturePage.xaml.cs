using BertScout2020Data.Models;
using System;
using System.IO;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BertScout2020.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PicturePage : ContentPage
    {
        public PicturePage(Team item)
        {
            InitializeComponent();
            displayPictureFromDisk(item.TeamNumber);
        }

        private void displayPictureFromDisk(int teamNumber)
        {
            string picturePath = App.GetMyPicturesPath().Concat($"/{teamNumber.ToString("0000")}.jpg").ToString();
            if(File.Exists(picturePath))
            {
                Image_Display.BackgroundColor = new Color(0, 255, 0);
            }
            else
            {
                Image_Display.BackgroundColor = new Color(255, 0, 0);
            }
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
                //System.Drawing.Bitmap image = ImageSource.FromStream(() => { return photo.GetStream(); });
                //string newName = "{Binding} Teams";
                //string fileNameAndPath = "";
                //Rename(fileNameAndPath, newName);
                Image_Display.Source = ImageSource.FromStream(() => { return photo.GetStream(); });
            }
        }

        // copied from https://www.codeproject.com/Questions/442057/How-to-rename-the-image-file
        public void Rename(string FileNameAndPath, string NewName)
        {
            System.IO.FileInfo fi = new System.IO.FileInfo(FileNameAndPath);
            string NewFilePathName = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(FileNameAndPath), NewName);
            System.IO.FileInfo f2 = new System.IO.FileInfo(NewFilePathName);

            try
            {
                if (f2.Exists)
                {
                    f2.Attributes = System.IO.FileAttributes.Normal;
                    f2.Delete();
                }

                fi.CopyTo(NewFilePathName);
                fi.Delete();
            }
            catch
            {

            }
        }
    }
}