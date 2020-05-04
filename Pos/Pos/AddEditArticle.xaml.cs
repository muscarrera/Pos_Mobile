using Android.Graphics;
using Plugin.Media;
using Plugin.Media.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Pos
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddEditArticle : ContentPage
    {
        public AddEditArticle()
        {
            InitializeComponent();
        }

       async  void BtLoad_Clicked(object sender, EventArgs e)
        {
            await CrossMedia.Current.Initialize();
            if (!CrossMedia.Current.IsPickPhotoSupported)
            {
                await DisplayAlert("Not Supported", " Some text", "ok");
                return;
            }
            var mediaOption = new PickMediaOptions()
            {
                PhotoSize = PhotoSize.Medium
            };

            var selectedImage = await CrossMedia.Current.PickPhotoAsync(mediaOption);
            Img.Source = ImageSource.FromStream(() => selectedImage.GetStream());
        }

        async void BtPick_Clicked(object sender, EventArgs e)
        {
            await CrossMedia.Current.Initialize();
            if (!CrossMedia.Current.IsPickPhotoSupported)
            {
                await DisplayAlert("Not Supported", " Some text", "ok");
                return;
            }
            var file = await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions
            {
                Directory = "Sample",
                Name = "test.jpg"
            });

                       Img.Source = ImageSource.FromStream(() =>
            {
                var stream = file.GetStream();
                return stream;
            });


        }
    }

    //public enum MediaFileType
    //{
    //    Image,
    //    Video
    //}

    //public class MediaFile
    //{
    //    public string PreviewPath { get; set; }
    //    public string Path { get; set; }
    //    public MediaFileType Type { get; set; }
    //}

    //public interface IMultiMediaPickerService
    //{
    //    event EventHandler<MediaFile> OnMediaPicked;
    //    event EventHandler<IList<MediaFile>> OnMediaPickedCompleted;
    //    Task<IList<MediaFile>> PickPhotosAsync();
    //    Task<IList<MediaFile>> PickVideosAsync();
    //    void Clean();
    //}


    //**************************************************

}