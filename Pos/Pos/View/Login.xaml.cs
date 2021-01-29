using Newtonsoft.Json;
using Pos.Model;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Microsoft.IdentityModel.Tokens;
using System.Text.RegularExpressions;
using System.Threading;

namespace Pos.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Login : ContentPage
    {
        readonly HttpClient _httpClient = new HttpClient();
        string UserPass = "pass";
        bool isActive = false;

        private DateTime _dt;

        public DateTime activeDate
        {
            get { return _dt; }
            set { 
                _dt = value;

                int nm = (_dt - DateTime.Now).Days;

                if (nm > -30)
                    isActive = true;
             }
        }


        public bool HasArticle
        {
            get { return false; }
            set {
                ArtIndic.IsRunning = false;
                ArtIndic.IsVisible = false;
            if (value)
                    lbArticle.Text = "Listes des articles ... Ok";
            else
                    lbArticle.Text = "Listes des articles ... Error"; ;
            }
        }
       

        public Login()
        {
            InitializeComponent();
                        
                TxtUser.Text = Preferences.Get(App.userName, "user");
                UserPass = Preferences.Get(App.str_userPass, "pass");
                activeDate = Preferences.Get(App.str_activeDate, DateTime.Now);

            txtactive.IsVisible = !isActive;
        }


        protected override async void OnAppearing()
        {
            base.OnAppearing();

            //test WIFI

            if (TestConnection()==false)
                   return;

           try
            {
               HasArticle = await GetArticlesAPI();
               //   HasArticle = Task.Run(()=> GetArticlesAPI());

            }
            catch (Exception)
            {
                ArtIndic.IsRunning = false;
                ArtIndic.IsVisible = false;
            }

            try
            {
                await GetCatsAPI();
            }
            catch (Exception) { }

            try
            {
                await GetRemisesAPI();
            }
            catch (Exception) { }
        }

        //get Value
        public async Task<bool> isDbValueChanged(string str)
        {
            try
            {
               
                string _str = $"{App.uriAPI}/api/val/{str}/gr";
                string content = await _httpClient.GetStringAsync(_str); ;

                return content.ToLower().Contains("true");
            }
            catch (Exception) {
               // await DisplayAlert(ex.Message, "", "ok");
                return false; }
    }

        //get Article
        public async Task<bool> GetArticlesAPI()
        {
           
            List<Article> arts;
           
            
            if (await isDbValueChanged(App.article_value_string) == false)
            {
                plArt.IsVisible = false;
                return false;
            }
            try
            {
         
                string str = $"{App.uriAPI}/api/art/{DateTime.Now.Ticks}/gr";
               string content = await _httpClient.GetStringAsync(str);
               arts = JsonConvert.DeserializeObject<List<Article>>(content); 
                              
                if (await App.articleData.FillOverTableAsync(arts)>0)
                {
                    //update value 

                     str = $"{App.uriAPI}/api/val/{App.article_value_string}";
                     content = await _httpClient.GetStringAsync(str);
                   
                    return true;
                }
                return true;
            }
            catch (Exception) {
              //  await DisplayAlert(ex.Message, "", "ok");
                return false;}
           
        }
       //getCategories 
        public async Task<int> GetCatsAPI()
        {
           
            List<Category> arts = new List<Category>();
            
            if (await isDbValueChanged(App.category_value_string) == false)
            {
                return 0;
            }

            string content = await _httpClient.GetStringAsync($"{App.uriAPI}/api/cat");

            try
            {
            arts = JsonConvert.DeserializeObject<List<Category>>(content);
            }
            catch (Exception)
            {
              //  await DisplayAlert(ex.Message, "", "ok");
            }

            

            try
            {
                using (SQLiteConnection con = new SQLiteConnection(App.dbPath))
                {
                    con.CreateTable<Category>();
                    con.DropTable<Category>();
                    con.CreateTable<Category>();

                    int i = con.InsertAll(arts);

                    string str = $"{App.uriAPI}/api/val/{App.category_value_string}";
                    content = await _httpClient.GetStringAsync(str);

                    return i;
                }
            }
            catch (Exception) { return 0; }
        }

        //getCategories 
        public async Task<int> GetRemisesAPI()
        {

            List<ArticleRemise> arts = new List<ArticleRemise>();

            if (await isDbValueChanged(App.remise_value_string) == false)
            {
                using (SQLiteConnection con = new SQLiteConnection(App.dbPath))
                {
                    con.CreateTable<ArticleRemise>();
                    App.listeRemise = con.Table<ArticleRemise>().ToList();
                    App.listeRemise = App.listeRemise.OrderByDescending(x => x.qte).ToList();
                }


                return 0;  }

            try {

            string content = await _httpClient.GetStringAsync($"{App.uriAPI}/api/remise"); 
            arts = JsonConvert.DeserializeObject<List<ArticleRemise>>(content);
                       
                using (SQLiteConnection con = new SQLiteConnection(App.dbPath))
                {
                    con.CreateTable<ArticleRemise>();
                    con.DropTable<ArticleRemise>();
                    con.CreateTable<ArticleRemise>();

                    int i = con.InsertAll(arts);

                    string str = $"{App.uriAPI}/api/val/{App.remise_value_string }";
                    content = await _httpClient.GetStringAsync(str);

                    App.listeRemise = arts.OrderByDescending(x => x.qte).ToList() ;

                    return i;
                }
            }
            catch (Exception ) {
               // await DisplayAlert(ex.Message, "err", "ok");
                return 0; }
        }
        //get Clients
        public async Task<bool> GetClientsAPI()
        {
           
            List<Client> arts;
            
            try
            {
                if (isDbValueChanged(App.client_value_string).Result.Equals(false))
                {
                    return false;
                }

                string content = await _httpClient.GetStringAsync($"{App.uriAPI}/api/clt/{DateTime.Now.Ticks}/gr");
                arts = JsonConvert.DeserializeObject<List<Client>>(content);

                if (await App.clientData.FillOverTableAsync(arts) > 0)
                {
                    string str = $"{App.uriAPI}/api/val/{App.client_value_string}";
                    content = await _httpClient.GetStringAsync(str);
                }
                
                return true;
            }
            catch (Exception) {  return false;  }
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            if (ArtIndic.IsRunning) { return; }

            if (isActive ==false)
            {
                int nm = DateTime.Now.Day * 105;
                nm += 105;

                if (int.Parse(txtactive.Text.Trim()) == nm)
                {
                 Preferences.Set("active_date", DateTime.Now.AddYears(1));
                } else { return; }
           }


            if (TxtPass.Text == UserPass)
            {
                var catP = new ListesFacture();
                await Navigation.PushAsync(catP);
            }
        }

        private bool TestConnection()
        {
            try
            {
                if (Connectivity.NetworkAccess == NetworkAccess.Internet)
                {
                    plWifi.IsVisible = false;
                    plArt.IsVisible = true;
                    return true;
                }
                else
                {
                    ArtIndic.IsRunning = false;
                    plArt.IsVisible = false;
                    
                    lbWifi.Text = "Non connecté, aucune connexion n'est disponible";
                    plWifi.BackgroundColor = Color.Red;
                    //await  DisplayAlert("wifi error", "test", "ok");
                    return false;
                }
            }
            catch (Exception) { return false; }
        }
    }
}