using Plugin.Media;
using Plugin.Media.Abstractions;
using Pos.Model;
using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using System.Drawing;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Pos.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddEditArticles : ContentPage
    {

      
        Article article;
        List<Category> cats;


        public AddEditArticles()
        {
            InitializeComponent();
            GetListOfCat();
            article = new Article();
        }
        public AddEditArticles(Article ar)
        {
            InitializeComponent();
            cats= new List<Category>(GetListOfCat());
            article = ar;

            TxtName.Text=article.name ; 
            TxtRef.Text=article.@ref  ;
            TxtUnit.Text = "u";
            TxtPrix.Text = article.sprice .ToString();
            CatPicker.SelectedItem = cats.Where(x => x.cid == article.cid).Select(x => x).FirstOrDefault();
            Img.Source = article.ImagePath ;
        }
        private async void Button_Clicked(object sender, EventArgs e)
        {
            article.name = TxtName.Text;
            article.@ref = TxtRef.Text;
            //article.Unit = TxtUnit.Text;
            article.sprice = double.Parse(TxtPrix.Text);
            article.cid = (int)(CatPicker.SelectedItem as Category).cid;

            await App.articleData.SaveArticleAsync(article);
            //  await Navigation.PopAsync();
            //Detail = new NavigationPage(new AddEditArticles());
            //IsPresented = false;
        }

        private List<Category> GetListOfCat()
        {
            using (SQLiteConnection con = new SQLiteConnection(App.dbPath))
            {
                con.CreateTable<Category>();

                List<Category> ls = con.Table<Category>().ToList();
                 CatPicker.ItemsSource = ls;
                 return ls;
            }
        }
        private async void LoadImage(object sender, EventArgs e)
        {
            await CrossMedia.Current.Initialize();
            if (!CrossMedia.Current.IsPickPhotoSupported)
            {
                await DisplayAlert("Not Supported", " Some text", "ok");
                return;
            }
            var mediaOption = new PickMediaOptions()
            {
                PhotoSize = PhotoSize.Small
            };

            var selectedImage = await CrossMedia.Current.PickPhotoAsync(mediaOption);
           Img.Source = ImageSource.FromStream(() => selectedImage.GetStream());
           string imgName = selectedImage.Path.Split('/').Last();
                        
            article.img = imgName;




            //article.img = GetImageBytes(selectedImage.GetStream()) ;

            Img.Source = article.ImagePath;

        }

        private byte[] GetImageBytes(Stream stream)
        {
            try
            {
            byte[] ImageBytes;
            using (var memoryStream = new System.IO.MemoryStream())
            {
                BinaryReader br = new BinaryReader(stream);
                ImageBytes = br.ReadBytes((int)stream.Length);

                var ms = new MemoryStream();
                ms.CopyTo(memoryStream);
                ImageBytes = memoryStream.ToArray();
            }
            return ImageBytes;
            }
            catch (Exception)
            {
                return null;
               
            }
          }


        private async void TakeImage(object sender, EventArgs e)
        {
            await CrossMedia.Current.Initialize();
            if (!CrossMedia.Current.IsPickPhotoSupported)
            {
                await DisplayAlert("Not Supported", " Some text", "ok");
                return;
            }
            string imgName = DateTime.Now.Ticks + ".jpg";
            var file = await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions
            {
                Directory = "AlMohassib",
                Name = imgName,
                SaveToAlbum = true,
              
            });


            Img.Source = ImageSource.FromStream(() =>
            {
                var stream = file.GetStream();
                return stream;
            });

            //article.img = GetImageBytes(file.GetStream());

            //Img.Source = article.ImagePath;

          }

      }
}