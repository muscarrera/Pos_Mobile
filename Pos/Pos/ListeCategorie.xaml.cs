using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Pos.Model;
using Pos.View;

namespace Pos
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ListeCategorie : ContentPage
    {
        public ListeCategorie()
        {
            InitializeComponent();
        }

        private async void TbAdd_Clicked(object sender, EventArgs e)
        {
            //Navigation.PushAsync(new AddEditCat());
            await Navigation.PushAsync(new AddEditCategory());
        }

        private async void TbEdit_Clicked(object sender, EventArgs e)
        {
            Category it = lsCat.SelectedItem as Category;
          await  Navigation.PushAsync(new AddEditCategory(it));
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            using (SQLiteConnection con = new SQLiteConnection(App.dbPath))
            {
                con.CreateTable<Category>();
                var cats= con.Table<Category>().ToList();
                lsCat.ItemsSource = cats;
            }
        }
    }
}