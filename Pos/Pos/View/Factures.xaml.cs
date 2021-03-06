﻿using dotMorten.Xamarin.Forms;
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
    public partial class Factures : ContentPage
    {
       public  List<Client> clients;
       public List<Facture> factures;

        public Factures()
        {
            InitializeComponent();
            clients = GetClients();
            factures = GetFacture();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            BindableLayout.SetItemsSource(CVFct, factures.Where(x => x.Commande_Client == ""));
        }

        public List<Client> GetClients()
        {
            using (SQLiteConnection con = new SQLiteConnection(App.dbPath))
            {
                con.CreateTable<Client>();
                return  con.Table<Client>().ToList();
            }
        }

        public List<Facture> GetFacture()
        {
            using (SQLiteConnection con = new SQLiteConnection(App.dbPath))
            {
              
                con.CreateTable<Facture>();
                return con.Table<Facture>().ToList();
            }
        }

        private void AutoSuggestBox_TextChanged(Object sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            // Only get results when it was a user typing, 
            // otherwise assume the value got filled in by TextMemberPath 
            // or the handler for SuggestionChosen.
            if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
            {
                //Set the ItemsSource to be your filtered dataset

                ((AutoSuggestBox)sender).ItemsSource = clients.Where(x => x.name.Contains(((AutoSuggestBox)sender).Text)).Select(x => x).ToList();
                ((AutoSuggestBox)sender).DisplayMemberPath = "clientName";

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
            ((AutoSuggestBox)sender).TextMemberPath = "clientName";
        }

        private void Edite_Invoked(object sender, EventArgs e)
        {
            var pr = ((SwipeItem)sender).BindingContext as Facture;

            if (pr.Commande_Client != "")
                Navigation.PushAsync(new DataList(pr));
            else
                Navigation.PushAsync(new MainPage(pr));
        }
        private void Delete_Invoked(object sender, EventArgs e)
        {

        }

        private async void TbAdd_Clicked(object sender, EventArgs e)
        {
            await  Navigation.PushAsync(new SelectClient());        
        }

        private void TbEdit_Clicked(object sender, EventArgs e)
        {

        }

        private void TbDetele_Clicked(object sender, EventArgs e)
        {
            BindableLayout.SetItemsSource(CVFct, factures.Where(x => x.Commande_Client == ""));
        }

        private void Entry_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (TxtSearch.Text.Length > 0)
                BtClearText.IsVisible = true;
            else
            {
            BtClearText.IsVisible = false;
            BindableLayout.SetItemsSource(CVFct, factures.Where(x => x.Commande_Client == ""));
            return;
            }
               

            var lst = factures
                .Where(x => x.name.Contains(TxtSearch.Text))
                .Select(x => x).ToList();
            
            BindableLayout.SetItemsSource(CVFct, lst);
        }

        private void BtClearText_Clicked(object sender, EventArgs e)
        {
            TxtSearch.Text = "";
        }
    }
}