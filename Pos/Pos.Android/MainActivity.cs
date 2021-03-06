﻿using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using System.IO;
using Plugin.Media;
using Xamarin.Forms;

namespace Pos.Droid
{
    [Activity(Label = "Pos", Icon = "@mipmap/ic_launcher", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected  override async void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);

            Rg.Plugins.Popup.Popup.Init(this);

            Forms.SetFlags("SwipeView_Experimental");
            await CrossMedia.Current.Initialize();
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);


            string dbPath = "PosGrosAFD.sqlite";
            string folderPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            string dbFullPath = Path.Combine(folderPath, dbPath);
            string imgFullPath = Path.Combine(folderPath, "AlMohassib/");

            LoadApplication(new App(dbFullPath, imgFullPath));
        }

        public override void OnBackPressed()
        {
            return;
            // base.OnBackPressed();
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);
           
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}