using System;
using System.IO;
using Plugin.Connectivity;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Tecnopolis_QR_App;
using Tecnopolis_QR_App.Views;


namespace Tecnopolis_QR_App
{
    public partial class App : Application
    {
        static DatabaseLocal db;
        public App()
        {
            InitializeComponent();

            MainPage = new MainPage();

        }

        public static DatabaseLocal SQLiteDB
        {
            get
            {
                if (db==null)
                {
                    db = new DatabaseLocal(Path.Combine(
                        Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Clientes.db3"));
                }
                return db;
            }
        }
        

        public bool DoIHaveInternet()
        {
            return CrossConnectivity.Current.IsConnected;
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
