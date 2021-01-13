using Android.Views;
using Android.Views.Animations;
using Newtonsoft.Json;
using Pos.Helpers;
using Pos.Model;
using Pos.View;
using SQLite;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.PancakeView;

namespace Pos
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {

        public List<Category> CategoryList;
        public List<Article> ArticleList;
        public List<Article> SearchedArticleList;
        public List<Product> ProductList;
        public int fctId = 1;
        public Facture facture;


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
                return con.Table<Product>().Where(x => x.fctid == this.fctId).Select(x => x).ToList();
            }
        }

        public MainPage()
        {
            InitializeComponent();
            this.BindingContext = this;

        }

        public MainPage(Facture fct)
        {
            InitializeComponent();
            this.BindingContext = this;
            facture = fct;
            fctId = fct.id;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
       
                 CategoryList = new List<Category>(GetListOfCat());
                 ArticleList = await App.articleData.GetArticlesAsync();
                 ProductList = new List<Product>(GetListOfProduct());
                 LbCounter.Text = ProductList.Count.ToString();
                 try
                 {
                     BindableLayout.SetItemsSource(StCat, CategoryList);
                     BindableLayout.SetItemsSource(CVPrd, ProductList);
                     CVArt.ItemsSource = ArticleList.Take(50);
                 }
                 catch (Exception) { }

        }

        private void TapCategory_Top(object sender, EventArgs e)
        {
            DisplayAlert("category","","ok");
        }
        private void ListCounter_Tapped(object sender, EventArgs e)
        {
            string TextValue = "";
            TextValue += "Ct:";

            var view = sender as PancakeView;
            var parent = view.Parent as StackLayout;
            var st = view.Children[0] as Label;
            TextValue += st.Text + ";";
            TxtSearch.Text = TextValue;

            foreach (PancakeView element in parent.Children)
            {
                VisualStateManager.GoToState(element, "Diselected");
                var ch = element.Children[0] as Label;
                VisualStateManager.GoToState(ch, "Diselected");
            }
                VisualStateManager.GoToState(view, "Selected");
                VisualStateManager.GoToState(st, "Selected");
        }

        private void CollectionViewListSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                var current = e.CurrentSelection.FirstOrDefault() as Article;

            var item = ProductList.FirstOrDefault(i => i.arid == current.arid);

            if (item != null)
            {
                double qte = item.qte + 1;
                foreach (Product pr in ProductList)
                {
                    if (pr.arid == current.arid)
                    {
                        pr.qte = qte;
                        Product.Edit(pr);
                        break;
                    }
                }
            } else
            {
                Product pr = new Product();
                pr.article = current;
                pr.fctid = this.fctId;

                if (Product.AddNew(pr))
                    ProductList.Add(pr);
            }

            // BindableLayout.SetItemsSource(CVPrd, ProductList);
           
            var nn = new List<Product>(ProductList);
            
            BindableLayout.SetItemsSource(CVPrd, nn);
            LbCounter.Text = ProductList.Count.ToString();
            CVArt.SelectedItem = null; ;
            }
            catch (Exception) { }
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

        private void TxtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (TxtSearch.Text.Length > 0)
                    BtClearText.IsVisible = true;
                else
                    BtClearText.IsVisible = false;


                string[] str = TxtSearch.Text.Split(';');

                SearchedArticleList = ArticleList;
                for (int i = 0; i < str.Length; i++)
                {
                    if (str[i].Trim() == "")
                        continue;

                    if (str[i].Contains(':'))
                    {
                        str[i] = str[i].Split(':').Last();

                        int ctid = 0;
                        ctid = CategoryList.Where(x => x.name == str[i])
                          .Select(x => x.cid).First();

                        if (ctid > 0)
                        {
                            SearchedArticleList = SearchedArticleList
                           .Where(x => x.cid == ctid)
                           .Select(x => x).ToList();
                        }
                    }
                    else
                    {
                        SearchedArticleList = SearchedArticleList
                        .Where(x => x.name.ToUpper().Contains(str[i].ToUpper()))
                        .Select(x => x).ToList();
                    }
                }
                CVArt.ItemsSource = SearchedArticleList.Take(50);
            }
            catch (Exception) { }

        }
        private void BtClearText_Clicked(object sender, EventArgs e)
        {
            TxtSearch.Text = "";
        }

        private async void EditProduct_Buttom(object sender, EventArgs e)
        {
            var pr = (sender as PancakeView).BindingContext as Product;
            await Navigation.PushAsync(new DetailProduct(pr));
        }

        private async void GoTo_List(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new DataList(facture));
        }
    }
}
