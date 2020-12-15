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
    public partial class SelectModePayement : ContentPage
    {

       public  string SelectedMode = "Cache";
        int cid;
        public SelectModePayement(int _cid)
        {
            InitializeComponent();
            cid = _cid;

        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
           

        }

        public async Task<bool> GetModePayement_API(string str)
        {
            HttpClient client;
            client = new HttpClient();
            try
            {
                string content = await client.GetStringAsync($"{App.uriAPI}/api/mdp/{str}");
                if (content.ToLower().Contains("true"))
                {
                    return true;
                }
                else { return false; }
            }
            catch (Exception) { return false; }

        }

        private void TbSelectMode_Click(object sender, EventArgs e)
        {

        }
    }
}