using Pos.Model;
using Pos.View;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Pos
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ListeArticle : ContentPage
    {
        public ListeArticle()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            using (SQLiteConnection con = new SQLiteConnection(App.dbPath))
            {
                con.CreateTable<Article>();
                var arts = con.Table<Article>().ToList();
                lsArt.ItemsSource = arts;
            }
        }

        private async  void TbAdd_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AddEditArticles());
        }

        private async void TbEdit_Clicked(object sender, EventArgs e)
        {
            Article it = lsArt.SelectedItem as Article;
            await Navigation.PushAsync(new AddEditArticles(it));
        }
    }
}