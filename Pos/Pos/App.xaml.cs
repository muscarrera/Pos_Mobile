using Plugin.SharedTransitions;
using Pos.Data;
using Pos.Model;
using Pos.View;
using System;
using System.Collections.Generic;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Pos
{
    public partial class App : Application
    {
       public static string dbPath = string.Empty;
       public static string imgPath = string.Empty;
       public static string uriAPI = "http://10.0.2.2:8000";
       public static string  lastLoadedDate = "0";

        public static string article_value_string = "article_grosphone1";
        public static string client_value_string = "client_grosphone1";
        public static string category_value_string = "category_grosphone1";
        public static string remise_value_string = "remise_article_grosphone1";


        public static string str_userID = "UserId";
        public static string str_userName = "UserName";
        public static string str_article_value_string = "article_value_string";
        public static string str_client_value_string = "client_value_string";
        public static string str_category_value_string = "category_value_string";
        public static string str_uriAPI = "UriAPI";
        public static string str_userPass = "UserPass";
        public static string str_activeDate = "active_date";

        public static int userID = 12;
        public static string userName = "User Comm";

        public static ArticleData articleData;
        public static ClientData clientData;
        public static List<ArticleRemise> listeRemise;
        public App()
        {
            InitializeComponent();

            //  MainPage = new SharedTransitionNavigationPage(new MainPage());
            MainPage = new SharedTransitionNavigationPage(new Login());
        }

        public App(string dbLocalPath,String imgLocalPath)
        {
            InitializeComponent();
           
            //  MainPage = new SharedTransitionNavigationPage(new MainPage());
            dbPath = dbLocalPath;
            imgPath = imgLocalPath;
            try
            {
                articleData = new ArticleData(dbPath);
                clientData = new ClientData(dbPath);
            }
            catch (Exception)
            {
            }

            //lastLoadedDate = Preferences.Get("lastLoadedDate", "11");

            userID = Preferences.Get(str_userID ,12);
            userName = Preferences.Get(str_userName , "User Com");
            uriAPI = Preferences.Get(str_uriAPI , "http://10.0.2.2:8000");
            article_value_string = Preferences.Get(str_article_value_string , "article_grosphone1");
            client_value_string = Preferences.Get(str_client_value_string , "client_grosphone1");
            category_value_string = Preferences.Get(str_category_value_string , "category_grosphone1");

            remise_value_string = "remise_" + article_value_string;

            MainPage = new SharedTransitionNavigationPage(new Login());

        }
        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
