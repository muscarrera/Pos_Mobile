using Android.Views;
using Android.Views.Animations;
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
                return con.Table<Product>().Where(x => x.Fid == this.fctId).Select(x => x).ToList();
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
            ProductList = new List<Product>( GetListOfProduct());
            LbCounter.Text = ProductList.Count.ToString();

            BindableLayout.SetItemsSource( StCat, CategoryList);
            BindableLayout.SetItemsSource(CVPrd, ProductList);
            CVArt.ItemsSource = ArticleList;
        }
        private void TapCategory_Top(object sender, EventArgs e)
        {
            DisplayAlert("eeeee","ttt","ok");
        }
        private void ListCounter_Tapped(object sender, EventArgs e)
        {
            string TextValue = "";

            //if (TxtSearch.Text != null)
            //{
            //     if (TxtSearch.Text.Contains(':'))
            //     {
            //       string[] str = TxtSearch.Text.Split(';');
            //          for (int i = 0; i < str.Length; i++)
            //          {
            //        if (!str[i].Contains(':'))
            //        {
            //        if (TextValue != "")
            //                    TextValue += ";";
            //        TextValue += str[i];
            //        }
            //        }
            //    }
            //}
         
            //if (TextValue != "")
            //    TextValue += ";";
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

            var item = ProductList.FirstOrDefault(i => i.Arid == current.Arid);

            if (item != null)
            {
                double qte = item.Qte + 1;
                foreach (Product pr in ProductList)
                {
                    if (pr.Arid == current.Arid)
                    {
                        pr.Qte = qte;
                        Product.Edit(pr);
                        break;
                    }
                }
            } else
            {
                Product pr = new Product();
                pr.article = current;
                pr.Fid = this.fctId;

                if (Product.AddNew(pr))
                    ProductList.Add(pr);
            }

            // BindableLayout.SetItemsSource(CVPrd, ProductList);
           
            var nn = new List<Product>(ProductList);
            
            BindableLayout.SetItemsSource(CVPrd, nn);
            LbCounter.Text = ProductList.Count.ToString();
            CVArt.SelectedItem = null; ;
            }
            catch (Exception ex) { }
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

        private void Button_Clicked(object sender, EventArgs e)
        {
          // click star


        }
        private void TxtSearch_TextChanged(object sender, TextChangedEventArgs e)
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
                    SearchedArticleList = SearchedArticleList
                        .Where(x => x.Cid == str[i] )
                        .Select(x => x).ToList();
                }else
                {
                        SearchedArticleList = SearchedArticleList
                        .Where(x => x.ArtName.Contains(str[i]))
                        .Select(x => x).ToList();
                }
            }
            CVArt.ItemsSource = SearchedArticleList;
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
            await Navigation.PushAsync(new DataList(fctId));
        }
    }
}
