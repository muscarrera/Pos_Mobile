using Java.Nio.Channels;
using Java.Sql;
using Newtonsoft.Json;
using Pos.Model;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
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
        Facture facture;

        public double Total
        {
            get {

                double t = 0;
                foreach (Product pr in products)
                {
                    t += double.Parse(pr.Total);
                }
                facture.Total = t;
                Facture.Edit(facture);
                return t; }
        }

        public DataList()
        {
            InitializeComponent();
        }

        public DataList(Facture fct)
        {
            InitializeComponent();
            fctId = fct.id;
            facture=fct;
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
                return con.Table<Product>().Where(x => x.fctid == this.fctId).Select(x => x).ToList();
            }
        }

        private List<Product> GetFacture()
        {
            using (SQLiteConnection con = new SQLiteConnection(App.dbPath))
            {
                con.CreateTable<Product>();
                return con.Table<Product>().Where(x => x.fctid == this.fctId).Select(x => x).ToList();
            }
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            products = new List<Product>(GetListOfProduct());
           // CVArt.ItemsSource = products;
            LbTotal.Text = string.Format("{0:F2}", Total);
            BindableLayout.SetItemsSource(CVPrd, products);

            lbId.Text = fctId.ToString();
            lbDate.Text = facture.date.ToString("dd/MM/yyyy e\\n HH:mm");
            lbName.Text = facture.name;
            
        }



        public async Task SaveCommandAsync()
        {
            HttpClient client;
            client = new HttpClient();

             facture.items= products;
                                                                                                                                                                                      
            string json = JsonConvert.SerializeObject(facture);
            StringContent content = new StringContent(json, Encoding.UTF8, "application/json");

            HttpResponseMessage response = null;
            if (facture.Commande_Client == "0" ) // is new
            {
                response = await client.PostAsync($"{App.uriAPI}/api/fct/gr/{facture.Commande_Client}", content);
            }
            
             if (response.IsSuccessStatusCode)
            {
                await DisplayAlert(@"\tTodoItem successfully saved.","","ok");
                facture.Commande_Client = response.Content.ToString();
                Facture.Edit(facture);
            }
            }



        private async void Go_Back(object sender, EventArgs e)
        {
            if (facture.modePayement=="")
            {


                return;
            }

            await SaveCommandAsync();
            await Navigation.PushAsync(new Factures());
        }

        private async void Edite_Invoked(object sender, EventArgs e)
        {
          
            var pr = ((SwipeItem)sender).BindingContext as Product;
             await Navigation.PushAsync(new DetailProduct(pr));
        }

        private async void Delete_Invoked(object sender, EventArgs e)
        {
            var pr = ((SwipeItem)sender).BindingContext as Product;

            string str = "Voulez vous suprimer : " + Environment.NewLine;
            str += pr.qte.ToString() + " (" + pr.unit + " )" + pr.name; 
            bool answer = await DisplayAlert("Supression?", str, "Oui", "Non");

            if (answer)
            {
                if (Product.Delete(pr))
                {
                products.Remove(pr);
                LbTotal.Text = string.Format("{0:F2}", Total);
                    List<Product> prd = new List<Product>(products);
                    BindableLayout.SetItemsSource(CVPrd, prd);
                }

            }
         }
         
        private void ChangeClient_Click(object sender, EventArgs e)
        {
            Navigation.PushAsync(new SelectClient(facture));
        }
         private void Button_Clicked(object sender, EventArgs e)
        {
            var md = new SelectModePayement(facture.cid);

            Navigation.PushModalAsync(md);
        }
    }
}