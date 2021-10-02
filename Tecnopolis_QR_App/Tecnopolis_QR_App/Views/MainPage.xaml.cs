using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tecnopolis_QR_App.Models;
using Xamarin.Forms;
using ZXing.Net.Mobile.Forms;


namespace Tecnopolis_QR_App
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
                string[] qrdt = qr_data[2].Split(' ');
                qr_data[2] = qrdt[0] + " " + qrdt[1];

                if (result != null)
                {
                    //bool can_pass = CanPassOnlineDB(qr_data);
                    bool can_pass = await CanPassLocalDB(qr_data);

                    if (can_pass)
                    {
                        await Navigation.PushModalAsync(new Pass(qr_data[3]));
                    }
                    else
                    {
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
                bool data = new DatabaseOnline().getInfo(qr_dni, qr_dt);
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

            if (qr_dt.Date == dt_actual.Date)
            {
                var dni_already_register = await App.SQLiteDB.GetClienteByDniAsync(qr_dni, qr_dt);
                if (dni_already_register != null && dni_already_register.fechayhora.Day == qr_dt.Day)
                    response = false;
                else
                {
                    Clientes clients = new Clientes
                    {
                        espectaculo_id = qr_data[0],
                        dni = qr_data[1],
                        fechayhora = Convert.ToDateTime(qr_data[2]),
                        personas = qr_data[3],
                        sala = qr_data[4]
                    };
                    await App.SQLiteDB.SaveClientesAsync(clients);

                    response = true;
                }
                
            }

            return response;
        }

        private async void BtnInputDNI_OnClicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new InputDni());

        }
    }
}
