using Pos.Model;
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
    public partial class DetailProduct : ContentPage
    {
        public Product product;
        public DetailProduct()
        {
            InitializeComponent();
        }

        public DetailProduct(Product pr)
        {
            InitializeComponent();
            product = pr;
        }

       private void GoBack(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            Img.Source = product.ImagePath;
            lbName.Text = product.name;
            txtQte.Text = product.qte.ToString();
            txtPrice.Text = product.price.ToString();
            lbUnit.Text = product.unit;
        }

        private void Add_Qte(object sender, EventArgs e)
        {
            txtQte.Text = (double.Parse(txtQte.Text) + 1).ToString();
        }

        private void Min_Qte(object sender, EventArgs e)
        {
            double qte = double.Parse(txtQte.Text) - 1;
            if (qte < 0)
                qte = 0;
            txtQte.Text = qte.ToString();
        }

        private async void Go_Back(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }

        private async void Add_Item(object sender, EventArgs e)
        {
            product.qte = double.Parse(txtQte.Text);
            product.price = double.Parse(txtPrice.Text);

            Product.Edit(product);
            await Navigation.PopAsync();
        }
    }
}