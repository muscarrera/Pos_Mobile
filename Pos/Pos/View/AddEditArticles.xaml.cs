﻿using Plugin.Media;
using Plugin.Media.Abstractions;
using Pos.Model;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Pos.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddEditArticles : ContentPage
    {

        bool isEdit = false;
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
            isEdit = true;
            article = ar;

            TxtName.Text=article.ArtName ; 
            TxtRef.Text=article.ArtRef  ;
            TxtUnit.Text = article.Unit;
            TxtPrix.Text = article.Price.ToString();
            CatPicker.SelectedItem = cats.Where(x => x.catName == article.Cid).Select(x => x).FirstOrDefault();
            Img.Source = ImageSource.FromFile(article.ImgName);
        }
        private void Button_Clicked(object sender, EventArgs e)
        {
            if (!isEdit)
               AddNewCat();
            else
              EditCat();
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
        private async void EditCat()
        {
            await System.Threading.Tasks.Task.Run(() =>
            {
                using (SQLiteConnection con = new SQLiteConnection(App.dbPath))
            {
                article.ArtName = TxtName.Text;
                article.ArtRef = TxtRef.Text;
                article.Unit = TxtUnit.Text;
                article.Price = double.Parse(TxtPrix.Text);
                article.Cid = (string)(CatPicker.SelectedItem as Category).catName;
                con.CreateTable<Article>();
                int i = con.Update(article);

                if (i > 0)
                    Navigation.PopAsync();
                }
            });
        }
        private async void AddNewCat()
        {
            article.ArtName = TxtName.Text;
            article.ArtRef = TxtRef.Text;
            article.Price = double.Parse(TxtPrix.Text);
            article.Unit = TxtUnit.Text;
            article.Cid = (string)(CatPicker.SelectedItem as Category).catName;

            await System.Threading.Tasks.Task.Run(() =>
            {
                using (SQLiteConnection con = new SQLiteConnection(App.dbPath))
                {
                    con.CreateTable<Article>();
                    int i = con.Insert(article);

                    if (i > 0)
                        Navigation.PopAsync();
                }
            });
         
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
           // string imgName = selectedImage.Path.Split('/').Last();
            article.ImgName = selectedImage.Path;

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
             article.ImgName = file.AlbumPath;
                   }

      }
}