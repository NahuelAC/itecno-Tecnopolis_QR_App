using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tecnopolis_QR_App.Models;
using Xamarin.Forms;

namespace Tecnopolis_QR_App.Views
{
    public partial class MainPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private async void BtnScannerQR_OnClicked(object sender, EventArgs e)
        {
            try
            {
                var scanner = new ZXing.Mobile.MobileBarcodeScanner();

                scanner.TopText = "Escanear QR";

                var result = await scanner.Scan();

                var separator = '-';
                string[] qr_data = result.Text.Split(separator);

                if (result != null)
                {
                    char mode = 'l';
                    // bool can_pass = false;
                    //
                    // if (new App().DoIHaveInternet())
                    // {
                    //     can_pass = CanPassOnlineDB(qr_data);
                    //     mode = 'o';
                    // }
                    // else
                    // {
                    //     can_pass = await CanPassLocalDB(qr_data);
                    //     mode = 'l';
                    // }

                    bool can_pass = await CanPassLocalDB(qr_data);

                    if (can_pass)
                    {
                        await Navigation.PushModalAsync(new Pass(qr_data[4]));
                    }
                    else
                    {
                        if (mode == 'o')
                            await DisplayAlert("Error", "No se puede conectar a la base de datos. Si el problema persiste desconectese de internet para utilizar la base de datos local.", "Ok");
                        await Navigation.PushModalAsync(new NotPass());
                    }
                }

            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message.ToString(), "Ok");
            }
        }

        private bool CanPassOnlineDB(string[] qr_data)
        {
            bool response = false;
            DateTime dt_actual = DateTime.Now;
            DateTime qr_dt = Convert.ToDateTime(qr_data[2]);
            string qr_dni = qr_data[1];

            if (qr_dt.Date == dt_actual.Date && dt_actual.Hour <= (qr_dt.Hour + 2))
            {
                bool data = new DatabaseOnline().GetClientByDni(qr_dni, qr_dt);
                response = data;
            }

            return response;
        }
        private async Task<bool> CanPassLocalDB(string[] qr_data)
        {
            bool response = false;
            DateTime dt_actual = DateTime.Now;
            DateTime qr_dt = Convert.ToDateTime(qr_data[2]);
            string qr_dni = qr_data[1];

            // if (qr_dt.Date == dt_actual.Date && dt_actual.Hour <= (qr_dt.Hour + 2))
            if (qr_dt.Date == dt_actual.Date)
            {
                var data = await App.SQLiteDB.GetClienteByDni(qr_dni);

                if ( !IsEmpty(data) )
                    foreach (var client in data)
                    {
                        if (client.dni == qr_dni && client.fechayhora == qr_dt)
                        {
                            response = true;
                            break;
                        }
                    }
                else
                    response = false;

                if (response)
                    response = false;
                else
                {
                    Clientes client = new Clientes
                    {
                        espectaculo_id = qr_data[0],
                        dni = qr_data[1],
                        fechayhora = Convert.ToDateTime(qr_data[2]),
                        personas = qr_data[4],
                        sala = qr_data[5]
                    };
                    await App.SQLiteDB.SaveClientesAsync(client);

                    response = true;
                }
                
            }

            return response;
        }

        private async void BtnInputDNI_OnClicked(object sender, EventArgs e)
        {
            // await Navigation.PushModalAsync(new InputDni());
            await DisplayAlert("Error", "Funcion no disponible", "Ok");

        }

        private async void ShowDisplayAlert(string message)
        {
            await DisplayAlert(" ", message, "Ok");
        }

        public static bool IsEmpty<T>(List<T> list)
        {
            if (list == null)
            {
                return true;
            }

            return !list.Any();
        }

        private async void UploadDataBtn_OnClicked(object sender, EventArgs e)
        {
            await DisplayAlert("Error", "Funcion no disponible", "Ok");
        }
    }
}
