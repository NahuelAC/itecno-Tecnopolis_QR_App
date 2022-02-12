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

                var separator = '-';
                string[] qr_data = result.Text.Split(separator);
                string[] qrdt = qr_data[2].Split(' ');
                qr_data[2] = qrdt[0];

                if (result != null)
                {
                    bool can_pass = false;
                    
                    if (CrossConnectivity.Current.IsConnected)
                    if (false)
                        can_pass = await CanPassOnlineDB(qr_data);
                    else
                        can_pass = await CanPassLocalDB(qr_data);

                    if (can_pass)
                    {
                        UserDialogs.Instance.HideLoading();
                        await Navigation.PushModalAsync(new Pass(qr_data[4]));
                    }
                    else
                    {
                        UserDialogs.Instance.HideLoading();
                        await Navigation.PushModalAsync(new NotPass());
                    }
                }

            }
            catch (Exception ex)
            {
                UserDialogs.Instance.HideLoading();
                await DisplayAlert("Error", ex.Message, "Ok");
            }
        }

        private async Task<bool> CanPassOnlineDB(string[] qr_data)
        {
            bool response = false;
            DateTime dt_actual = DateTime.Now;
            DateTime qr_dt = Convert.ToDateTime(qr_data[2]);
            string qr_dni = qr_data[1];

            if (qr_dt.Date == dt_actual.Date)
            {
                var data = await ApiClient.GetTicketsByDni(qr_dni);
                if (data != null)
                {
                    foreach (var e in data)
                    {
                        if (qr_dt.Date == e.FechaV.Date && e.Show == null)
                        {
                            await ApiClient.PutTicket(e.idEntradas.ToString(), DateTime.Now);
                            response = true;
                            await DisplayAlert("", response.ToString(), "Ok");
                            break;
                        }
                    }
                }
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
                var data = await App.SQLiteDB.GetClienteByDni(qr_dni);
                if (data != null)
                {
                    await DisplayAlert("", "Hay data", "Ok");
                    var ishere = false;
                    Clientes c = new Clientes();
                    foreach (var e in data)
                    {
                        if (e.fechayhora.Day == qr_dt.Day && e.Show != null)
                        {
                            await DisplayAlert("", "Esta aqui", "Ok");
                            if (e.Show != null)
                            {
                                response = false;
                                ishere = true;
                                break;
                            }
                            else
                                c = e;
                            
                        }
                        else
                            ishere = false;
                    }
                    

                    if (!ishere)
                    {
                        c.Show = DateTime.Now;
                        await App.SQLiteDB.SaveClientesAsync(c);

                        response = true;
                    }
                }
                else
                {
                    await DisplayAlert("", "No hay data", "Ok");
                    await App.SQLiteDB.SaveClientesAsync(new Clientes
                    {
                        espectaculo_id = qr_data[0],
                        dni = qr_data[1],
                        fechayhora = Convert.ToDateTime(qr_data[2]),
                        Show = DateTime.Now,
                    });
                    

                    response = false;
                }
            }
            return response;
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
