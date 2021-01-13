using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Pos.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Setting : ContentPage
    {
             public Setting()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            txtpwd.Text = Preferences.Get("UserPass", "a");
            txtuser.Text = Preferences.Get("UserName", "User Com");
            txturi.Text  = Preferences.Get("UriAPI", "http://10.0.2.2:8000");
            txtart.Text = Preferences.Get("article_value_string", "article_grosphone1");
            txtclt.Text = Preferences.Get("client_value_string", "client_grosphone1");
            txtctg.Text = Preferences.Get("category_value_string", "category_grosphone1");
            txtId.Text = Preferences.Get("UserId",1).ToString();


        }
        private void Button_Clicked(object sender, EventArgs e)
        {
            //if (txtactive.Text.Trim().StartsWith("/*/"))
            //    return;


           Preferences.Set("UserPass", txtpwd.Text);
           Preferences.Set("UserName", txtuser.Text);
           Preferences.Set("UriAPI", txturi.Text);
           Preferences.Set("article_value_string", txtart.Text);
           Preferences.Set("client_value_string", txtclt.Text);
           Preferences.Set("category_value_string", txtctg.Text);
            Preferences.Set("UserId", int.Parse(txtId.Text));
            int nm = DateTime.Now.Day * 105;
            nm += 105;
            string str = $"/*/{nm}/";

            try
            {
                if(txtactive.Text.Trim() == str)
                Preferences.Set("active_date", DateTime.Now.AddYears(1));

            }
            catch (Exception){}
          

        }
    }
}