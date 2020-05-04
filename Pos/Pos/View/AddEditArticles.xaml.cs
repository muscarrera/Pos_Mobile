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
    public partial class AddEditArticles : ContentPage
    {

        bool isEdit = false;
        Article article;

        public AddEditArticles()
        {
            InitializeComponent();
        }
        public AddEditArticles(Article ar)
        {
            InitializeComponent();
            isEdit = true;
            article = ar;
        }
        private void Button_Clicked(object sender, EventArgs e)
        {
            if (!isEdit)
                AddNewCat();
            else
                EditCat();
        }

        private void EditCat()
        {
            using (SQLiteConnection con = new SQLiteConnection(App.dbPath))
            {
                article.ArtName = TxtName.Text;
                article.ArtRef = TxtRef.Text;
                article.Price = double.Parse(TxtPrix.Text);

                con.CreateTable<Article>();
                int i = con.Update(article);

                if (i > 0)
                    Navigation.PopAsync();
            }
        }

        private void AddNewCat()
        {
            Article art = new Article()
            {
                ArtName = TxtName.Text,
                ArtRef = TxtRef.Text,
                Price = double.Parse(TxtPrix.Text)
            };

            using (SQLiteConnection con = new SQLiteConnection(App.dbPath))
            {
                con.CreateTable<Article>();
                int i = con.Insert(art);

                if (i > 0)
                    Navigation.PopAsync();
            }


        }

        private void Button_Clicked_1(object sender, EventArgs e)
        {

        }
    }
}