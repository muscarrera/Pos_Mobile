using Pos.Model;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Pos.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DataList : ContentPage
    {
        public List<Product> products;
        public int fctId;
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
        }

 
        private async void Go_Back(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }

    }
}