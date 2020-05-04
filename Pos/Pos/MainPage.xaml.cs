using Pos.Model;
using Pos.View;
using SQLite;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Pos
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {

        public List<Category> CategoryList;
        public List<Article> ArticleList;
        public List<Product> ProductList;

        private List<Article> GetListOfArt()
        {
            using (SQLiteConnection con = new SQLiteConnection(App.dbPath))
            {
                con.CreateTable<Article>();
                return con.Table<Article>().ToList();
            }
        }
        private List<Category> GetListOfCat()
        {
            using (SQLiteConnection con = new SQLiteConnection(App.dbPath))
            {
                con.CreateTable<Category>();
                return con.Table<Category>().ToList();
            }
         }
        private List<Product> GetListOfProduct()
        {
            using (SQLiteConnection con = new SQLiteConnection(App.dbPath))
            {
                con.CreateTable<Product>();
                return con.Table<Product>().ToList();
            }
        }

        public MainPage()
        {
            InitializeComponent();
            this.BindingContext = this;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            CategoryList = new List<Category>(GetListOfCat());
            ArticleList =  new List<Article>(GetListOfArt());
            ProductList = GetListOfProduct();
            LbCounter.Text = CategoryList.Count.ToString();

            BindableLayout.SetItemsSource( StCat, CategoryList);
            CVArt.ItemsSource = ArticleList;
        }
        private void TapCategory_Top(object sender, EventArgs e)
        {
            DisplayAlert("eeeee","ttt","ok");
        }

        private async void ListCounter_Tapped(object sender, EventArgs e)
        {
            var catP = new DataList();
            await Navigation.PushAsync(catP);
        }

        private void CollectionViewListSelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private async void btToCat_Clicked(object sender, EventArgs e)
        {
            var catP = new ListeCategorie();
            await Navigation.PushAsync(catP);
        }
        private async void btToart_Clicked(object sender, EventArgs e)
        {
            var catP = new ListeArticle();
            await Navigation.PushAsync(catP);
        }
    }
}
