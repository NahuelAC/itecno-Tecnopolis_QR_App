using System;
using Acr.UserDialogs;
using Plugin.Connectivity;
using Plugin.DeviceInfo;
using Tecnopolis_QR_App.Models;
using Xamarin.Forms.Xaml;

namespace Tecnopolis_QR_App.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ConfigPage
    {
        public ConfigPage()
        {
            InitializeComponent();
        }

        private async void SyncBtn_OnClicked(object sender, EventArgs e)
        {
            UserDialogs.Instance.ShowLoading();
            try
            {
                if (CrossConnectivity.Current.IsConnected)
                {
                    var data = await App.SQLiteDB.GetAllClientes();

                    foreach (var c in data)
                    {
                        if (c.Show != null)
                        {
                            var x = await ApiClient.GetTicketsByDni(c.dni);
                            foreach (var de in x)
                            {
                                if (de.FechaV.Date == c.fechayhora.Date && de.Show == null)
                                {
                                    await ApiClient.PutTicket(de.idEntradas.ToString(), Convert.ToDateTime(c.Show));
                                    //await App.SQLiteDB.DeleteClienteAsync(c);
                                    break;
                                }
                            }
                        }
                    }

                    await App.SQLiteDB.DeleteAllClientesAsync();

                    var data2 = await ApiClient.GetTicketsByDateNow();

                    foreach (var entrada in data2)
                    {
                        await App.SQLiteDB.SaveClientesAsync(new Clientes
                        {
                            espectaculo_id = entrada.idEventos.ToString(),
                            dni = entrada.DNI.Trim(),
                            fechayhora = entrada.FechaV,
                            Evento = entrada.Evento,
                            Show = entrada.Show,
                        });
                    }
                    UserDialogs.Instance.HideLoading();
                    var d = await App.SQLiteDB.GetAllClientes();
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
            catch (Exception ex)
            {
                UserDialogs.Instance.HideLoading();
                await DisplayAlert
                (
                    "Error",
                    "Error al recibir los datos del servidor, intente mas tarde. Si el error persiste contacte con un desarrollador.",
                    "Ok"
                );
                /*await DisplayAlert
                (
                    "Error",
                    ex.Message,
                    "Ok"
                );*/
            }
        }


        private async void CloseBtn_OnClicked(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }
    }
}