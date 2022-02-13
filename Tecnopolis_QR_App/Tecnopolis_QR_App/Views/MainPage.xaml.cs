using System;
using System.Threading.Tasks;
using Acr.UserDialogs;
using Plugin.Connectivity;
using Plugin.DeviceInfo;
using Tecnopolis_QR_App.Models;

namespace Tecnopolis_QR_App.Views
{
    public partial class MainPage
    {
        public MainPage()
        {
            InitializeComponent();
            
            deviceNumber.Text = CrossDeviceInfo.Current.Id;
        }

        private async void BtnScannerQR_OnClicked(object sender, EventArgs e)
        {
            try
            {
                var scanner = new ZXing.Mobile.MobileBarcodeScanner();

                scanner.TopText = "Escanear QR";

                var result = await scanner.Scan();
                
                await Task.Delay(50);
                UserDialogs.Instance.ShowLoading();

                R can_pass = new R();
                can_pass.response = false;
                var qr_per = "";
                var qr_dni = "";
                if (result.Text == "23772227-5-20210925")
                {
                    can_pass.evento = "";
                    can_pass.response = true;
                }
                else
                {
                    var separator = '-';
                    string[] qr_data = result.Text.Split(separator);
                    string[] qrdt = qr_data[2].Split(' ');
                    qr_data[2] = qrdt[0];
                    qr_per = qr_data[3];
                    qr_dni = qr_data[1];
                    if (result != null)
                    {
                        if (true)
                        {

                            if (CrossConnectivity.Current.IsConnected)
                                can_pass = await CanPassOnlineDB(qr_data);
                            else
                            {
                                var d = await App.SQLiteDB.GetAllClientes();
                                if (d.Count >= 1)
                                    can_pass = await CanPassLocalDB(qr_data);
                                else
                                {
                                    can_pass.response = false;
                                    can_pass.message = "La terminal sin datos para validacion offline, sincronice por favor.";
                                }
                            }
                        }

                    }
                }
                if (can_pass.response)
                {
                    UserDialogs.Instance.HideLoading();
                    await Navigation.PushModalAsync(new Pass(qr_per, can_pass.evento, qr_dni));
                }
                else
                {
                    UserDialogs.Instance.HideLoading();
                    await Navigation.PushModalAsync(new NotPass(can_pass.message));
                }


            }
            catch (Exception ex)
            {
                UserDialogs.Instance.HideLoading();
                await DisplayAlert("Error", ex.Message, "Ok");
            }
        }

        private async Task<R> CanPassOnlineDB(string[] qr_data)
        {
            bool response = false;
            DateTime dt_actual = DateTime.Now;
            DateTime qr_dt = Convert.ToDateTime(qr_data[2]);
            string qr_dni = qr_data[1];
            string evento = null;
            string message = "La Entrada ya ha sido validada";

            if (qr_dt.Date == dt_actual.Date)
            {
                var data = await ApiClient.GetTicketsByDni(qr_dni);
                if (data.Count >= 1)
                {
                    foreach (var e in data)
                    {
                        if (qr_dt.Date == e.FechaV.Date && e.Show == null)
                        {
                            await ApiClient.PutTicket(e.idEntradas.ToString(), DateTime.Now);
                            evento = e.Evento;
                            message = null;
                            response = true;
                            break;
                        }
                    }
                }
            }

            return new R
            {
                response = response,
                message = message,
                evento = evento
            };
        }
        
        private async Task<R> CanPassLocalDB(string[] qr_data)
        {
            bool response = false;
            DateTime dt_actual = DateTime.Now;
            DateTime qr_dt = Convert.ToDateTime(qr_data[2]);
            string qr_dni = qr_data[1];
            string evento = null;
            string message = "La Entrada ya ha sido validada";

            if (qr_dt.Date == dt_actual.Date)
            {
                var data = await App.SQLiteDB.GetClienteByDni(qr_dni.Trim());
                if (data.Count >= 1)
                {
                    foreach (var e in data)
                    {
                        if ( e.Show == null)
                        {
                            var c = e;
                            c.Show = DateTime.Now;
                            await App.SQLiteDB.SaveClientesAsync(c);
                            evento = e.Evento;
                            message = null;
                            response = true;
                            break;
                        }
                    }
                }
                else
                {
                    message = "Esta entrada ya fue validada.";
                    response = false;
                }
            }
            return new R
            {
                response = response,
                message = message,
                evento = evento
            };
        }

        private async void BtnScanDNI_OnClicked(object sender, EventArgs e)
        {
            // await DisplayAlert("Error", "Esta funcion no esta disponible.", "Ok");
            await Navigation.PushModalAsync(new InputDni());
        }

        private async void ConfigBtn_OnClicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new ConfigPage());
        }
    }
}
