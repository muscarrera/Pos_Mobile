using dotMorten.Xamarin.Forms;
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
    public partial class SelectClient : ContentPage
    {
        public List<Client> clienteList;
        public Facture facture;
        public bool EditMode = false;
        public SelectClient()
        {
            InitializeComponent();
        }
        public SelectClient(Facture fct)
        {
            InitializeComponent();
            facture = fct;
            EditMode = true;
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            using (SQLiteConnection con = new SQLiteConnection(App.dbPath))
            {
                con.CreateTable<Client>();
                var cls = con.Table<Client>().ToList();
                lsClient.ItemsSource = cls;
                clienteList = new List<Client>(cls);
            }
        }
 
        
       private async Task AddNew()
        {
            var current = lsClient.SelectedItem as Client;

            Facture fct = new Facture()
            {

                cid = current.Clid,
                name = current.name,
                modePayement = "",
                date = DateTime.Now,
                total = 0,
                avance = 0,
                tva = 0,
                remise = 0,
                droitTimbre = 0,
                pj = 0,
                compteId = 0,
                commercialID = App.userID,
                writer = App.userName,
                driver = "",
                Bon_Commande = "0",
                Bon_Livraison = "0",
                Devis = "0",
                Commande_Client = "",
                isAdmin = "LANCER",
                isValid = false
            };

            using (SQLiteConnection con = new SQLiteConnection(App.dbPath))
            {
                con.CreateTable<Facture>();
                int i = con.Insert(fct);

                if (i > 0)
                {
                    Facture ft = con.Table<Facture>().Last();

                    var catP = new MainPage(ft);
                    await Navigation.PushAsync(catP);
                }

            }
        }

        private async Task EditClient()
        {
            var current = lsClient.SelectedItem as Client;

            facture.cid = current.Clid;
            facture.name = current.name;
           
            using (SQLiteConnection con = new SQLiteConnection(App.dbPath))
            {
                con.CreateTable<Facture>();
                int i = con.Update(facture);

                if (i > 0)
                {
                 await Navigation.PopAsync();
                }

            }
        }
        private void BtClearText_Clicked(object sender, EventArgs e)
        {
            TxtSearch.Text = "";
        }

        private void lsClient_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {

        }

        private void TxtSearch_TextChanged(object sender, TextChangedEventArgs e)
        { 
            BtClearText.IsVisible = false;
            if (TxtSearch.Text.Length > 0) { BtClearText.IsVisible = true; }

           List<Client> SearchedList;
           SearchedList = new List<Client>(clienteList) ;
           
           SearchedList = SearchedList
                    .Where(x => x.name.ToUpper().Contains(TxtSearch.Text.ToUpper()))
                    .Select(x => x).ToList();
                
         lsClient.ItemsSource = SearchedList;      
        }

        private async  void TbAdd_Clicked(object sender, EventArgs e)
        {
            if (EditMode)
            {
                await EditClient();
            }
            else
            {
                await AddNew();
            }
        }
    }
}