using Plugin.SharedTransitions;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Pos
{
    public partial class App : Application
    {
       public static string dbPath = string.Empty;
        public static string imgPath = string.Empty;

        public App()
        {
            InitializeComponent();

            MainPage = new SharedTransitionNavigationPage(new MainPage());
        }

        public App(string dbLocalPath)
        {
            InitializeComponent();

            MainPage = new SharedTransitionNavigationPage(new MainPage());
            dbPath = dbLocalPath;
        }
        public App(string dbLocalPath,String imgLocalPath)
        {
            InitializeComponent();

            MainPage = new SharedTransitionNavigationPage(new MainPage());
            dbPath = dbLocalPath;
            imgPath = imgLocalPath;
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
