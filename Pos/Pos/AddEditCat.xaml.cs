using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Pos.Model;
using SQLite;

namespace Pos
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddEditCat : ContentPage
    {
        bool isEdit = false;
        Category cat;
        public AddEditCat()
        {
            InitializeComponent();
            btDelete.IsVisible = false;
                    }

        public AddEditCat(Category c)
        {
            InitializeComponent();
            isEdit = true;
            btAdd.Text = "Modifie";
            cat = c;
        }
        private void BtAdd_Clicked(object sender, EventArgs e)
        {
            if (!isEdit)
                AddNewCat();
            else
               EditCat();
        }

        private void BtDelete_Clicked(object sender, EventArgs e)
        {
            if (isEdit)
                DeleteCat();
        }

        private void DeleteCat()
        {
            using (SQLiteConnection con = new SQLiteConnection(App.dbPath))
            {
                con.CreateTable<Category>();
                int i = con.Delete(cat);

                if (i > 0)
                    Navigation.PopAsync();
            }
        }

        private void EditCat()
        {
            using (SQLiteConnection con = new SQLiteConnection(App.dbPath))
            {
                cat.catName = txtCatName.Text;
                con.CreateTable<Category>();
                int i = con.Update(cat);

                if (i > 0)
                    Navigation.PopAsync();
            }
        }

        private void AddNewCat()
        {
            Category cat = new Category()
            {
                catName = txtCatName.Text
            };

            using (SQLiteConnection con = new SQLiteConnection(App.dbPath))
            {
                con.CreateTable<Category>();
               int i= con.Insert(cat);

                if (i > 0)
                    Navigation.PopAsync();
            }


        }
    }

}