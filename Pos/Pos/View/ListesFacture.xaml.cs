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

            Facture fct = new Facture() { 
            Clid =0,
            ClientName="Client",
            FctDate= DateTime.Now,
            Total=0,
            Avance=0
            };

            using (SQLiteConnection con = new SQLiteConnection(App.dbPath))
            {
                con.CreateTable<Facture>();
                int i = con.Insert(fct);

                if (i > 0)
                {
                    Facture ft = con.Table<Facture>().Last();
                    Detail = new NavigationPage(new MainPage(ft));
                    IsPresented = false;
                }
                                  
            }

          }

        private void GoToFactures(object sender, EventArgs e)
        {
            Detail = new NavigationPage(new Factures());
            IsPresented = false;
        }
    }
}