using System;
using Acr.UserDialogs;
using Plugin.Connectivity;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Tecnopolis_QR_App.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class InputDni : ContentPage
    {
        public InputDni()
        {
            InitializeComponent();
        }
        
        private async void CloseBtn_OnClicked(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }

        private async void EnterBtn_OnClicked(object sender, EventArgs e)
        {
            try
            {

                if (CrossConnectivity.Current.IsConnected)
                {
                    UserDialogs.Instance.ShowLoading();
                    var dni = EntryDni.Text;
                    var can_pass = false;
                    var data = await ApiClient.GetTicketsByDni(dni);
                    foreach (var item in data)
                    {
                        if (item.FechaV.Day == DateTime.Now.Day && item.Show == null)
                        {
                            await ApiClient.PutTicket(item.idEntradas.ToString(), DateTime.Now);
                            UserDialogs.Instance.HideLoading();
                            await Navigation.PushModalAsync(new Pass(item.Visitantes.ToString(), item.Evento, item.DNI));
                            can_pass = true;
                            break;
                        }
                    }

                    if (!can_pass)
                    {
                        UserDialogs.Instance.HideLoading();
                        await Navigation.PushModalAsync(new NotPass("La entrada ya a sido validada"));
                    }
                }
                else
                {
                    await DisplayAlert("Error", "Necesita estar conectado a internet para utilizar esta funcion", "Ok");
                }
            }
            catch (Exception exception)
            {
                await DisplayAlert("Error", exception.Message, "Ok");
                Console.WriteLine(exception);
            }
        }
    }
}