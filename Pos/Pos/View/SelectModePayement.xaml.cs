using Rg.Plugins.Popup.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Pos.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SelectModePayement : Rg.Plugins.Popup.Pages.PopupPage
    {

       public  string[]  Mode = { "Cache" };
        public string SelectedMode =  "Cache" ;
        public int cid = 0;
        public int total = 0;

        public SelectModePayement(int _cid, double _total)
        {
            InitializeComponent();
            cid = _cid;
            total = (int)Math.Round(_total);
        }

    

        // Invoked after an animation appearing
        protected async override void OnAppearingAnimationEnd()
        {
            base.OnAppearingAnimationEnd();
            LsMode.ItemsSource = Mode;
            await GetModePayement_API();
        }


        public async Task GetModePayement_API()
        {
            HttpClient client;
            client = new HttpClient();
            try
            {
                string content = await client.GetStringAsync($"{App.uriAPI}/api/modecln/{cid}/{total}");
                Mode = content.Split('|');

                LsMode.ItemsSource = Mode;
                ModeIndic.IsRunning = false;
                ModeIndic.IsVisible = false;
            }
            catch (Exception) {}

        }

      

        private async void LsMode_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            await Navigation.PopPopupAsync();
        }
    }
}