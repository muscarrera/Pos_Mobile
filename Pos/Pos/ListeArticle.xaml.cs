﻿using dotMorten.Xamarin.Forms;
using Pos.Model;
using Pos.View;
using SQLite;
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
    public partial class ListeArticle : ContentPage
    {
        public List<Article> articleList;
        public ListeArticle()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            using (SQLiteConnection con = new SQLiteConnection(App.dbPath))
            {
                con.CreateTable<Article>();
                var arts = con.Table<Article>().ToList();
                lsArt.ItemsSource = arts;
                articleList = new List<Article>(arts);
            }
        }

        private async  void TbAdd_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AddEditArticles());
        }

        private async void TbEdit_Clicked(object sender, EventArgs e)
        {
            try
            {
             Article it = lsArt.SelectedItem as Article;
                if (it != null)
                    await Navigation.PushAsync(new AddEditArticles(it));

            }
            catch (Exception) {}

        }
        private async void Delete_Clicked(object sender, EventArgs e)
        {
            try
             {
              Article pr = lsArt.SelectedItem as Article;

                string str = "Voulez vous suprimer : " + Environment.NewLine;
                str += pr.ArtName+ " (" + pr.ArtRef + " )" ;
                bool answer = await DisplayAlert("Supression?", str, "Oui", "Non");

                if (answer)
                {
                    if (Article.Delete(pr))
                    {
                        articleList.Remove(pr);
                        lsArt.ItemsSource = new List<Article>(articleList);
                    }
                }

            }
            catch (Exception) { }
        }

        private void AutoSuggestBox_TextChanged(Object sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            // Only get results when it was a user typing, 
            // otherwise assume the value got filled in by TextMemberPath 
            // or the handler for SuggestionChosen.
            if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
            {
                //Set the ItemsSource to be your filtered dataset

                ((AutoSuggestBox)sender).ItemsSource = articleList.Where(x => x.ArtName.Contains(((AutoSuggestBox)sender).Text)).Select(x => x).ToList();
                ((AutoSuggestBox)sender).DisplayMemberPath = "ArtName";

            }
        }
        private void AutoSuggestBox_QuerySubmitted(object sender, AutoSuggestBoxQuerySubmittedEventArgs e)
        {
            if (e.ChosenSuggestion != null)
            {
                // User selected an item from the suggestion list, take an action on it here.
            }
            else
            {
                // User hit Enter from the search box. Use args.QueryText to determine what to do.
            }
        }

        private void AutoSuggestBox_SuggestionChosen(object sender, AutoSuggestBoxSuggestionChosenEventArgs e)
        {
            ((AutoSuggestBox)sender).TextMemberPath = "ArtName";
        }

        private async void Edite_Invoked(object sender, EventArgs e)
        {
          

            try
            {
                var it = (sender as SwipeItem).BindingContext as Article;
                if (it != null)
                    await Navigation.PushAsync(new AddEditArticles(it));

            }
            catch (Exception) { }
        }

        private async void Delete_Invoked(object sender, EventArgs e)
        {
            try
            {
                var pr = (sender as SwipeItem).BindingContext as Article;

                string str = "Voulez vous suprimer : " + Environment.NewLine;
                str += pr.ArtName + " (" + pr.ArtRef + " )";
                bool answer = await DisplayAlert("Supression?", str, "Oui", "Non");

                if (answer)
                {
                    if (Article.Delete(pr))
                    {
                        articleList.Remove(pr);
                        lsArt.ItemsSource = new List<Article>(articleList);
                    }
                }

            }
            catch (Exception) { }
        }

        private void CollectionViewListSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
        }

  
    }
}
