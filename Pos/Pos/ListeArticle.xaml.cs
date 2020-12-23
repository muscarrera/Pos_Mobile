using dotMorten.Xamarin.Forms;
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

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            articleList = await App.articleData.GetArticlesAsync();
            
            lsArt.ItemsSource = new List<Article>(articleList.Take(100));

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
                str += pr.name+ " (" + pr.@ref + " )" ;
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

                ((AutoSuggestBox)sender).ItemsSource = articleList.Where(x => x.name.Contains(((AutoSuggestBox)sender).Text)).Select(x => x).ToList();
                ((AutoSuggestBox)sender).DisplayMemberPath = "name";

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
            ((AutoSuggestBox)sender).TextMemberPath = "name";
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
                str += pr.name + " (" + pr.@ref + " )";
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

        private void TXT_TextChanged(object sender, TextChangedEventArgs e)
        {
            var ls = articleList
                .Where((x) =>  x.name.Contains(txt.Text) || x.@ref.Contains(txt.Text) || x.code.Contains(txt.Text))
                .Select(x => x).ToList();

            lsArt.ItemsSource = ls;
        }
    }
}
