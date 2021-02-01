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
        List<Category> catList;
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
            try
            {
                 Category it = lsCat.SelectedItem as Category;
                if (it != null)
                    await  Navigation.PushAsync(new AddEditCategory(it));
         
            }
            catch (Exception)     {            }
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            using (SQLiteConnection con = new SQLiteConnection(App.dbPath))
            { 
                con.CreateTable<Category>();
                var cats= con.Table<Category>().ToList();
                lsCat.ItemsSource = cats;
                catList = cats;
            }
        }

        private async void TbDetele_Clicked(object sender, EventArgs e)
        {
            try
            {
                Category pr = lsCat.SelectedItem as Category;

                string str = "Voulez vous suprimer : " + Environment.NewLine;
                str += pr.name;
                bool answer = await DisplayAlert("Supression?", str, "Oui", "Non");

                if (answer)
                {
                    if (Category.Delete(pr))
                    {
                        catList.Remove(pr);
                        lsCat.ItemsSource = new List<Category>(catList);
                    }
                }
            }
            catch (Exception) { }
        }
    }
}