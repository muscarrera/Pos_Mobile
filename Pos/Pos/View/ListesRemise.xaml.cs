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
    public partial class ListesRemise : ContentPage
    {
        public List<ArticleRemise> remisesList;
        public ListesRemise()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            using (SQLiteConnection con = new SQLiteConnection(App.dbPath))
            {
                con.CreateTable<ArticleRemise>();
                var cls = con.Table<ArticleRemise>().ToList();
                lsClient.ItemsSource = cls;
                remisesList = new List<ArticleRemise>(cls);
               
            }
        }
        private void TxtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (TxtSearch.Text.Length > 0)
                BtClearText.IsVisible = true;
            else
                BtClearText.IsVisible = false;



            var lst = remisesList
                .Where(x => x.name.Contains(TxtSearch.Text))
                .Select(x => x).OrderByDescending(x => x.id).Take(15).ToList();

            lsClient.ItemsSource = lst;
        }
        private void BtClearText_Clicked(object sender, EventArgs e)
        {
            TxtSearch.Text = "";
        }

        private void lsClient_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {

        }
    }
}