using Newtonsoft.Json;
using Pos.Model;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Pos.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddEditClient : ContentPage
    {
        bool isEdit = false;
        Client client;
        public AddEditClient()
        {
            InitializeComponent();
            client = new Client();
        }

        public AddEditClient(Client cl)
        {
            InitializeComponent();
            client = cl;
            txtName.Text = cl.name;
            txtTel .Text = cl.tel;
            txtAdresse.Text = cl.adresse;
            txtVille.Text = cl.ville;
            isEdit = true;
        }


        private async void Button_Clicked(object sender, EventArgs e)
        {
          await AddNewCat();
           
          }
         private void EditCat()
        {
            using (SQLiteConnection con = new SQLiteConnection(App.dbPath))
            {
                client.name = txtName.Text;
                client.tel = txtTel.Text;
                client.adresse = txtAdresse.Text;
                client.ville = txtVille.Text;


                con.CreateTable<Client>();
                int i = con.Update(client);

                if (i > 0)
                    Navigation.PopAsync();
            }
        }
        private async Task AddNewCat()
        {
            int i = 0;
            client.name = txtName.Text;
            client.tel = txtTel.Text;
            client.adresse = txtAdresse.Text;
            client.ville = txtVille.Text;

            using (SQLiteConnection con = new SQLiteConnection(App.dbPath))
            {
                con.CreateTable<Client>();

                if ( client.Clid <= 0)
            {
                client.@ref = $"clt_{App.userName}";
                client.groupe = "GM Gros Commercial";
                client.isCompany = false;
                client.ice = "ice";
                client.gsm = txtTel.Text;
                client.email = "Email";
                client.cp = "CP";
                client.info = txtVille.Text;
                client.responsable = txtName.Text;
                client.porte_Monie = 0;
                client.plafond = 1000;
                client.ModePayement = "Cache";
                client.isBlocked = false;

               if (!isEdit)
                        i= con.Insert(client);
               else
                        i = con.Update(client);
               }
                else { i = con.Update(client); }
             }       

             if (i > 0) {
                //send it to api 

                try { await SaveClientAsnc(); }
                catch (Exception) { }

                 await  Navigation.PopAsync();
                }
            
        }

        public async Task SaveClientAsnc()
        {
            try
            {
                if (client.Clid > 0) { return; }

                if (Connectivity.NetworkAccess != NetworkAccess.Internet) { return; }
                
                using (var _client = new HttpClient())
                {
                    _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    var uri = new Uri($"{App.uriAPI}/api/client");

                    string serializedObject = JsonConvert.SerializeObject(client);
                    HttpContent contentPost = new StringContent(serializedObject, System.Text.Encoding.UTF8, "application/json");
                    contentPost.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                     
                        HttpResponseMessage response = await _client.PostAsync(uri, contentPost);
                        if (response.IsSuccessStatusCode)
                        {
                            var data = await response.Content.ReadAsStringAsync();

                            client.Clid = int.Parse(data);
                            Client.Edit(client);
                        }
                    
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert(ex.Message, "Error", "Ok");
            }
        }
    }
}