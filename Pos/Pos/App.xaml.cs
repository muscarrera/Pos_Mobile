using Plugin.SharedTransitions;
using Pos.Data;
using Pos.View;
using System;
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

               
        public static int userID = 12;
        public static string userName = "User Comm";

        public static ArticleData articleData;
        public static ClientData clientData;
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

            userID = Preferences.Get("UserId",12);
            userName = Preferences.Get("UserName", "User Com");
            uriAPI = Preferences.Get("UriAPI", "http://10.0.2.2:8000");
            article_value_string = Preferences.Get("article_value_string", "article_grosphone1");
            client_value_string = Preferences.Get("client_value_string", "client_grosphone1");
            category_value_string = Preferences.Get("category_value_string", "category_grosphone1");

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
