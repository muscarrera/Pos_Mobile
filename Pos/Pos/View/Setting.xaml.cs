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
            txtpwd.Text = Preferences.Get(App.str_userPass, "a");
            txtuser.Text = Preferences.Get(App.str_userName , "User Com");
            txturi.Text  = Preferences.Get(App.str_uriAPI, "http://10.0.2.2:8000");
            txtart.Text = Preferences.Get(App.str_article_value_string, "article_grosphone1");
            txtclt.Text = Preferences.Get(App.str_client_value_string, "client_grosphone1");
            txtctg.Text = Preferences.Get(App.str_category_value_string, "category_grosphone1");
            txtId.Text = Preferences.Get(App.str_userID ,1).ToString();


        }
        private void Button_Clicked(object sender, EventArgs e)
        {
            //if (txtactive.Text.Trim().StartsWith("/*/"))
            //    return;


           Preferences.Set(App.str_userPass, txtpwd.Text);
           Preferences.Set(App.str_userName, txtuser.Text);
           Preferences.Set(App.str_uriAPI, txturi.Text);
           Preferences.Set(App.str_article_value_string, txtart.Text);
           Preferences.Set(App.str_client_value_string, txtclt.Text);
           Preferences.Set(App.str_category_value_string, txtctg.Text);
            Preferences.Set(App.str_userID , int.Parse(txtId.Text));
            int nm = DateTime.Now.Day * 105;
            nm += 105;
            string str = $"/*/{nm}/";

            try
            {
                if(txtactive.Text.Trim() == str)
                Preferences.Set(App.str_activeDate, DateTime.Now.AddYears(1));

            }
            catch (Exception){}
          

        }
    }
}