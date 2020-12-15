using Pos.Model;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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


        private void Button_Clicked(object sender, EventArgs e)
        {
            if (!isEdit)
                AddNewCat();
            else
                EditCat();
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
        private void AddNewCat()
        {
            client.name = txtName.Text;
            client.tel = txtTel.Text;
            client.adresse = txtAdresse.Text;
            client.ville = txtVille.Text;

            if (client.Clid == 0)
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
                client.plafond = 0;
                client.ModePayement = "Cache";
                client.isBlocked = false;
            }

            using (SQLiteConnection con = new SQLiteConnection(App.dbPath))
            {
                con.CreateTable<Client>();
                int i = con.Insert(client);

                if (i > 0) { 
                    //send it to api 




                    Navigation.PopAsync();
                }
            }
        }

    }
}