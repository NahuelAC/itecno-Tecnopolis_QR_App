using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tecnopolis_QR_App.Models;
using Xamarin.Forms;
using static Tecnopolis_QR_App.ApiClient;

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
            await Tecnopolis();
        }

        private async Task Tecnopolis()
        {
            try
            {
                var scanner = new ZXing.Mobile.MobileBarcodeScanner();

                scanner.TopText = "Escanear QR";

                var result = await scanner.Scan();

                var separator = '-';
                string[] qr_data = result.Text.Split(separator);
                var x = qr_data[2].Split(' ');
                qr_data[2] = x[0] + ' ' + x[1];

                if (result != null)
                {
                    char mode = 'l';
                    bool can_pass = false;
                    
                    if (new App().DoIHaveInternet())
                    {
                        can_pass = await TCanPassOnlineDB(qr_data);
                        mode = 'o';
                    }
                    else
                    {
                        can_pass = await TCanPassLocalDB(qr_data);
                        mode = 'l';
                    }

                    // bool can_pass = await CanPassLocalDB(qr_data);

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
            catch (Exception e)
            {
                if (new App().DoIHaveInternet())
                    await DisplayAlert("Error", "No se puede conectar a la base de datos. Si el problema persiste desconectese de internet para utilizar la base de datos local.", "Ok");
                else
                    await DisplayAlert("Error", e.Message.ToString(), "Ok");
            }
        }

        private async Task<bool> TCanPassOnlineDB(string[] qr_data)
        {
            bool response = false;
            DateTime dt_actual = DateTime.Now;
            DateTime qr_dt = Convert.ToDateTime(qr_data[2]);
            string qr_dni = qr_data[1];
        
            if (qr_dt.Date == dt_actual.Date && dt_actual.Hour <= (qr_dt.Hour + 2))
            {
                var data = await TApiGetTicketsByDni(qr_dni);
                
                foreach (var entrada in data)
                {
                    if (entrada.DNI == qr_dni && entrada.Fecha == qr_dt)
                    {
                        response = true;
                        break;
                    }
                }
            }
        
            return response;
        }
        
        private async Task<bool> TCanPassLocalDB(string[] qr_data)
        {
            bool response = false;
            DateTime dt_actual = DateTime.Now;
            DateTime qr_dt = Convert.ToDateTime(qr_data[2]);
            string qr_dni = qr_data[1];

            // if (qr_dt.Date == dt_actual.Date && dt_actual.Hour <= (qr_dt.Hour + 2))
            if (qr_dt.Date == dt_actual.Date)
            {
                var datae = await App.SQLiteDB.GetEntradaByDni(qr_dni);

                if ( !IsEmpty(datae) )
                    foreach (var e in datae)
                    {
                        if (e.DNI == qr_dni && e.Fecha == qr_dt)
                        {
                            response = true;
                            break;
                        }
                    }
                else
                    response = false;

                if (response)
                    response = true;
                else
                {
                    var datac = await App.SQLiteDB.GetClienteByDni(qr_dni);

                    if (!IsEmpty(datac))
                    {
                        foreach (var c in datac)
                        {
                            if (c.dni == qr_dni && c.fechayhora == qr_dt)
                            {
                                response = false;
                                break;
                            }
                        }
                    }
                    else
                    {
                        Clientes client = new Clientes 
                        {
                        espectaculo_id = qr_data[0],
                        dni = qr_data[1],
                        fechayhora = Convert.ToDateTime(qr_data[2]),
                        personas = qr_data[3],
                        sala = qr_data[4]
                        };
                        await App.SQLiteDB.SaveClientesAsync(client);

                        response = true;
                    }
                }
                
            }

            return response;
        }

        private async void BtnScanDNI_OnClicked(object sender, EventArgs e)
        {
            await DisplayAlert("Error", "Funcion no disponible", "Ok");

        }

        public static bool IsEmpty<T>(List<T> list)
        {
            if (list == null)
            {
                return true;
            }

            return !list.Any();
        }

        private async void ConfigBtn_OnClicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new ConfigPage());
        }
    }
}
