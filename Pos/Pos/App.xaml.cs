using Plugin.SharedTransitions;
using Pos.Data;
using Pos.View;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Pos
{
    public partial class App : Application
    {
       public static string dbPath = string.Empty;
        public static string imgPath = string.Empty;
       
        public static ArticleData articleData;
        public App()
        {
            InitializeComponent();

            //  MainPage = new SharedTransitionNavigationPage(new MainPage());
            MainPage = new SharedTransitionNavigationPage(new ListesFacture());
        }

        public App(string dbLocalPath)
        {
            InitializeComponent();

           // MainPage = new SharedTransitionNavigationPage(new MainPage());
            dbPath = dbLocalPath;
            articleData = new ArticleData(dbPath);
        }
        public App(string dbLocalPath,String imgLocalPath)
        {
            InitializeComponent();
            MainPage = new SharedTransitionNavigationPage(new ListesFacture());
            //  MainPage = new SharedTransitionNavigationPage(new MainPage());
            dbPath = dbLocalPath;
            imgPath = imgLocalPath;
            articleData = new ArticleData(dbPath);
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
