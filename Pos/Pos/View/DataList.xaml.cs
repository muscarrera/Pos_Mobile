using Pos.Model;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.PancakeView;
using Xamarin.Forms.Xaml;

namespace Pos.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DataList : ContentPage
    {
        public List<Product> products;
        public int fctId;

        public decimal Total
        {
            get {

                decimal t = 0;
                foreach (Product pr in products)
                {
                    t += decimal.Parse(pr.Total);
                }
                return t; }
        }

        public DataList()
        {
            InitializeComponent();
        }

        public DataList(int fid)
        {
            InitializeComponent();
            fctId = fid;
        }

      
        private void GoBack(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }

        private List<Product> GetListOfProduct()
        {
            using (SQLiteConnection con = new SQLiteConnection(App.dbPath))
            {
                con.CreateTable<Product>();
                return con.Table<Product>().Where(x => x.Fid == this.fctId).Select(x => x).ToList();
            }
        }

        private List<Product> GetFacture()
        {
            using (SQLiteConnection con = new SQLiteConnection(App.dbPath))
            {
                con.CreateTable<Product>();
                return con.Table<Product>().Where(x => x.Fid == this.fctId).Select(x => x).ToList();
            }
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            products = new List<Product>(GetListOfProduct());
            CVArt.ItemsSource = products;
            LbTotal.Text = string.Format("{0:F2}", Total);
        }

 
        private async void Go_Back(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }

        private async void Edite_Invoked(object sender, EventArgs e)
        {
          
             
            var pr = CVArt.SelectedItem  as Product;

            await Navigation.PushAsync(new DetailProduct(pr));
        }

        private void Delete_Invoked(object sender, EventArgs e)
        {

        }

        private void CollectionViewListSelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}