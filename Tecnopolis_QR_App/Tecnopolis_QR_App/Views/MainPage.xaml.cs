using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Acr.UserDialogs;
using Newtonsoft.Json;
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
                    //bool can_pass = CanPassOnlineDB(qr_data);
                    bool can_pass = await CanPassLocalDB(qr_data);

                    if (can_pass)
                    {
                        UserDialogs.Instance.HideLoading();
                        await Navigation.PushModalAsync(new Pass(qr_data[3]));
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
                var dni_already_register = await App.SQLiteDB.GetClienteByDniAsync(qr_dni);
                if (dni_already_register != null && dni_already_register.fechayhora.Day == qr_dt.Day)
                    response = false;
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

            return response;
        }

        private async void BtnInputDNI_OnClicked(object sender, EventArgs e)
        {

        }

        private async void BtnUploadData_OnClicked(object sender, EventArgs e)
        {
            UserDialogs.Instance.ShowLoading();
            try
            { 
                if (CrossConnectivity.Current.IsConnected)
                {
                    var datac = await App.SQLiteDB.GetAllClientes();

                    foreach (var cliente in datac)
                    {
                        var res = await ApiPostTicket(cliente);
                        await App.SQLiteDB.DeleteClienteAsync(cliente);
                    }
                    UserDialogs.Instance.HideLoading();
                    await DisplayAlert("", "La operacion se realizo con exito.", "Ok");
                }
                else
                {
                    UserDialogs.Instance.HideLoading();
                    await DisplayAlert
                    (
                        "Error",
                        "Error al conectar con el servidor. Asegurese de estar conectado a internet.", 
                        "Ok"
                    );
                }
            }
            catch (Exception exception)
            {
                UserDialogs.Instance.HideLoading();
                await DisplayAlert("Error", exception.Message, "Ok");
            }
        }
        
        public static async Task<string> ApiPostTicket(Clientes c)
        {
            HttpClient Client = new HttpClient();
            
            HttpResponseMessage res =
                await Client.PostAsync($"http://itecno.com.ar:3000/api/tecnopolis/tickets/localdatabasebackup/{c.espectaculo_id}/{c.dni}/{c.fechayhora.ToString("yyyy-M-dd hh:mm:ss")}/{c.personas}/{c.sala}/{CrossDeviceInfo.Current.Id}", null);
            string resBody = await res.Content.ReadAsStringAsync();

            return resBody;
        }
    }
}
