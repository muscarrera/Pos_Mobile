using Newtonsoft.Json;
using Pos.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Pos.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Test : ContentPage
    {
        readonly HttpClient _httpClient = new HttpClient();
       
        public Test()
        {
            InitializeComponent();
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();


            //reuse this
            if (await isDbValueChanged(App.article_value_string))
            {
                   string str = $"{App.uriAPI}/api/art/{DateTime.Now.Ticks}/gr";
            //var response = await _httpClient.GetAsync(str);

            string json =await _httpClient.GetStringAsync(str);
            var ls= JsonConvert.DeserializeObject<Article>(json);
            }
         
         
            }

        public async Task<bool> isDbValueChanged(string str)
        {
            try
            {
                

                string _str = $"{App.uriAPI}/api/val/{str}/gr";
                string content = await _httpClient.GetStringAsync(_str); ;

                return content.ToLower().Contains("true");
            }
            catch (Exception) { return false; }
        }
    }
}