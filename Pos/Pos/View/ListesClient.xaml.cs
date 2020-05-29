using Android.Views.Accessibility;
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
    public partial class ListesClient : ContentPage
    {
        public List<Client> clienteList;
        public bool EditMode;
        public Facture fct;
        public ListesClient()
        {
            InitializeComponent();
            fct = new Facture();
        }
        public ListesClient(bool isEdit,Facture ft)
        {
            InitializeComponent();
            EditMode = isEdit;
            fct = ft;
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
                if (EditMode)
                    NavigationPage.SetHasNavigationBar(this, false);
            }
        }

        #region AutoSuggestBox
        private void AutoSuggestBox_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            // Only get results when it was a user typing, 
            // otherwise assume the value got filled in by TextMemberPath 
            // or the handler for SuggestionChosen.
            if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
            {
                //Set the ItemsSource to be your filtered dataset
                //sender.ItemsSource = dataset;


            }
        }

        private void AutoSuggestBox_SuggestionChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args)
        {
            // Set sender.Text. You can use args.SelectedItem to build your text string.
        }

        private void AutoSuggestBox_QuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {
            if (args.ChosenSuggestion != null)
            {
                // User selected an item from the suggestion list, take an action on it here.
            }
            else
            {
                // User hit Enter from the search box. Use args.QueryText to determine what to do.
            }
        }
        #endregion

        private async void TbDetele_Clicked(object sender, EventArgs e)
        {
            try
            {
                Client pr = lsClient.SelectedItem as Client;

                string str = "Voulez vous suprimer : " + Environment.NewLine;
                str += pr.ClientName ;
                bool answer = await DisplayAlert("Supression?", str, "Oui", "Non");

                if (answer)
                {
                    if (Client.Delete(pr))
                    {
                        clienteList.Remove(pr);
                        lsClient.ItemsSource = new List<Client>(clienteList);
                    }
                }
             }
            catch (Exception) { }
        }

        private async void TbEdit_Clicked(object sender, EventArgs e)
        {
            try
            {
                Client cl = lsClient.SelectedItem as Client;
                if (cl != null)
                    await Navigation.PushAsync(new AddEditClient(cl));

            }
            catch (Exception) { }
        }
         
        private async void TbAdd_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AddEditClient());
        }

        private void TxtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (TxtSearch.Text.Length > 0)
                BtClearText.IsVisible = true;
            else
                BtClearText.IsVisible = false;



            var lst = clienteList
                .Where(x => x.ClientName.Contains(TxtSearch.Text))
                .Select(x => x).ToList();

            lsClient.ItemsSource = lst;
        }

        private void BtClearText_Clicked(object sender, EventArgs e)
        {
            TxtSearch.Text = "";
        }

        private async void lsClient_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (fct.Fid == 0 || EditMode ==false)
                return;

            var ct = lsClient.SelectedItem as Client;

            if (ct == null)
                return;

            fct.Clid = ct.Clid;
            fct.ClientName = ct.ClientName;

           Facture.Edit(fct);
           await Navigation.PopAsync();
        }
    }
}