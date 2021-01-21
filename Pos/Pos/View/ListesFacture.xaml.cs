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
    public partial class ListesFacture : MasterDetailPage 
    {
      
        public ListesFacture()
        {
            InitializeComponent();
            Detail = new NavigationPage(new Factures());
            IsPresented = false;
        }

        private void GoToClients(object sender, EventArgs e)
        {
            Detail = new NavigationPage(new ListesClient());
            IsPresented = false;
        }

        private void GoToGroupes(object sender, EventArgs e)
        {
            Detail = new NavigationPage(new ListeCategorie());
            IsPresented = false;
        }

        private void GoToAddArticle(object sender, EventArgs e)
        {
            Detail = new NavigationPage(new AddEditArticles());
            IsPresented = false;
        }

        private void GoToArticles(object sender, EventArgs e)
        {
            Detail = new NavigationPage(new ListeArticle());
            IsPresented = false;
        }

        private void GoToAddFacture(object sender, EventArgs e)
        {
            Detail = new NavigationPage(new SelectClient());
            IsPresented = false;
        }

        private void GoToFactures(object sender, EventArgs e)
        {
            Detail = new NavigationPage(new Factures());
            IsPresented = false;
        }

        private void GoToParams(object sender, EventArgs e)
        {
            Detail = new NavigationPage(new Setting());
            IsPresented = false;
        }

        private async void LogOut(object sender, EventArgs e)
        {
            await Navigation.PopToRootAsync();
            System.Environment.Exit(0);
        }
    }
}