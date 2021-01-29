using Java.Nio.Channels;
using Java.Sql;
using Newtonsoft.Json;
using Pos.Model;
using Rg.Plugins.Popup.Extensions;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Xamarin.Essentials;
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
        string[] Mode = { "Cache" };

        public double Total
        {
            get {

                double t = 0;
                foreach (Product pr in products)
                {
                    t += double.Parse(pr.Total);
                }
                facture.total = t;
                Facture.Edit(facture);
                return t; }
        }
        private string modePayemant;

        public string ModePayement
        {
            get { return modePayemant; }
            set { 
                modePayemant = value;
                btModePayement.Text = $"MP: {value}";
                 }
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
        protected async override void OnAppearing()
        {
            base.OnAppearing();
            products = new List<Product>(GetListOfProduct());
            // CVArt.ItemsSource = products;
            LbTotal.Text = string.Format("{0:F2}", Total);
            BindableLayout.SetItemsSource(CVPrd, products);

            lbId.Text = fctId.ToString();
            lbDate.Text = facture.date.ToString("dd/MM/yyyy e\\n HH:mm");
            lbName.Text = facture.name;
            modePayemant = facture.modePayement;


            if (facture.Commande_Client != "")
                plBlocTotal.IsVisible = false;
            else
                plBlocTotal.IsVisible = true;



            if (facture.cid == 0)
            {
                try
                {
                   if (Connectivity.NetworkAccess != NetworkAccess.Internet) {
                        plBlocTotal.IsVisible = false;
                        return; }

                    var cl = new Client();
                    cl = GetClient();

                    using (var _client = new HttpClient())
                    {
                        _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                        var uri = new Uri($"{App.uriAPI}/api/client");

                        string serializedObject = JsonConvert.SerializeObject(cl);
                        HttpContent contentPost = new StringContent(serializedObject, System.Text.Encoding.UTF8, "application/json");
                        contentPost.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                        HttpResponseMessage response = await _client.PostAsync(uri, contentPost);
                        if (response.IsSuccessStatusCode)
                        {
                            var data = await response.Content.ReadAsStringAsync();

                            cl.Clid = int.Parse(data);
                            Client.Edit(cl);
                        }

                    }
                }
                catch (Exception)
                {
                    plBlocTotal.IsVisible = false;
                    return;
                }
            }

            await GetModePayement_API();
        }

        private Client GetClient()
        {
            using (SQLiteConnection con = new SQLiteConnection(App.dbPath))
            {
                con.CreateTable<Client>();
                return con.Table<Client>().Where(x => x.name == facture.name).Select(x => x).FirstOrDefault();
            }
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

        public async Task GetModePayement_API()
        {
           if (Connectivity.NetworkAccess == NetworkAccess.Internet)
            {
                HttpClient client;
                client = new HttpClient();
                try
                {
                    string content = await client.GetStringAsync($"{App.uriAPI}/api/modecln/{facture.cid}/{facture.total}");
                    Mode = content.Split('|');
                }
                catch (Exception) { }
            }
        }
        public async Task ShowModal_modePayement()
        {
            //SelectModePayement mp = new SelectModePayement(facture.cid, Total);
            //string action = await DisplayActionSheet("ActionSheet: SavePhoto?", "Cancel", "Delete", "Photo Roll", "Email");

            string action = await DisplayActionSheet("Selectioner le Mode dePeiement ", "", "", Mode);


            facture.modePayement  = action;
            ModePayement = facture.modePayement;
        }

        public async Task SaveCommandAsync()
        {
            if (Connectivity.NetworkAccess != NetworkAccess.Internet) { return; }

            if (facture.modePayement =="")   { await  ShowModal_modePayement();}


             HttpClient client;
            client = new HttpClient();

             //facture.items= products;
                                                                                                                                                                                      
            string json = JsonConvert.SerializeObject(facture);
            StringContent content = new StringContent(json, Encoding.UTF8, "application/json");

            HttpResponseMessage response = null;
            if (facture.Commande_Client == "0" ) // is new
            {
                response = await client.PostAsync($"{App.uriAPI}/api/commande", content);
            }
            var result = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                await DisplayAlert(@"\successfully saved.", "", "ok");
                facture.Commande_Client = response.Content.ToString();
                Facture.Edit(facture);
            }
         }
        public async Task SaveProduct()
        {
            try
            {
                if (Connectivity.NetworkAccess != NetworkAccess.Internet) { return; }
                if (facture.modePayement == "") { await ShowModal_modePayement(); }

                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    var uri = new Uri($"{App.uriAPI}/api/commande");
                  
                    List<Item> itemsList = new List<Item>();
                    foreach (var pr in products) {
                        Item i = new Item();
                        i.arid = pr.arid;
                        i.cid = pr.cid;
                        i.name = pr.name;
                        i.@ref = pr.@ref;
                        i.price = pr.price;
                        i.bprice = pr.bprice;
                        i.remise = pr.remise;
                        i.qte = pr.qte;
                        i.commession = pr.commession;
                        i.tva = pr.tva;
                        i.poid = pr.poid;
                        i.depot = pr.depot;

                        itemsList.Add(i);
                    }

                    facture.items = itemsList;
                    
                    string serializedObject = JsonConvert.SerializeObject(facture);
                    HttpContent contentPost = new StringContent(serializedObject, System.Text.Encoding.UTF8, "application/json");
                    contentPost.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                    if (facture.Commande_Client == "")
                    {
                     HttpResponseMessage response = await client.PostAsync(uri, contentPost);
                    if (response.IsSuccessStatusCode) {
                    var data = await response.Content.ReadAsStringAsync();

                        facture.Commande_Client = data;
                        Facture.Edit(facture); 
                    }
                    }
               
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert(ex.Message, "Error", "Ok");
            }
        }

        private async void Save_Commande(object sender, EventArgs e)
        {
            btSave.IsEnabled = false;
            await SaveProduct();
            //await SaveCommandAsync();
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
        private async void Button_Clicked(object sender, EventArgs e)
        {
            await ShowModal_modePayement();
        }


    }
}