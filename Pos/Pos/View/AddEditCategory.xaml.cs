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
    public partial class AddEditCategory : ContentPage
    {
        bool isEdit = false;
        Category cat;
        public ImageSource ImgSource = "https://encrypted-tbn0.gstatic.com/images?q=tbn%3AANd9GcTowxV7hjlNpwEyzDD9UicBxyW8mPIznm__4G29TRWwhIMsRiFW&usqp=CAU";
        public AddEditCategory()
        {
            InitializeComponent();
            this.BindingContext = this;
        }

        public AddEditCategory(Category c)
        {
             InitializeComponent();
            isEdit = true;
            cat = c;
            TxtName.Text = cat.catName;
            this.BindingContext = this;
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
                cat.catName = TxtName.Text;
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
                catName = TxtName.Text
            };

            using (SQLiteConnection con = new SQLiteConnection(App.dbPath))
            {
                con.CreateTable<Category>();
                int i = con.Insert(cat);

                if (i > 0)
                    Navigation.PopAsync();
            }


        }

    }
}