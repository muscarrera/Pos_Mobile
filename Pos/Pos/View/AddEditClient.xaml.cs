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
            txtName.Text = cl.ClientName;
            txtInfo.Text = cl.Info;
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
                client.ClientName = txtName.Text;
                client.Info = txtInfo.Text;
              
                con.CreateTable<Client>();
                int i = con.Update(client);

                if (i > 0)
                    Navigation.PopAsync();
            }
        }
        private void AddNewCat()
        {
            client.ClientName = txtName.Text;
            client.Info = txtInfo.Text;

            using (SQLiteConnection con = new SQLiteConnection(App.dbPath))
            {
                con.CreateTable<Client>();
                int i = con.Insert(client);

                if (i > 0)
                    Navigation.PopAsync();
            }
        }

    }
}