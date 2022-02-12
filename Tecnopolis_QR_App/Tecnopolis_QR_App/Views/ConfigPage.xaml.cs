﻿using System;
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

        private async void AdbBtn_OnClicked(object sender, EventArgs e)
        {
            UserDialogs.Instance.ShowLoading();
            try
            {
                if (CrossConnectivity.Current.IsConnected)
                {
                    var data = await ApiClient.GetTicketsByDateNow();

                    foreach (var entrada in data)
                    {
                        await App.SQLiteDB.SaveClientesAsync(new Clientes
                        {
                            espectaculo_id = entrada.idEventos.ToString(),
                            dni = entrada.DNI,
                            fechayhora = entrada.FechaV,
                            Show = entrada.Show,
                        });
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
            catch (Exception ex)
            {
                UserDialogs.Instance.HideLoading();
                // await DisplayAlert
                // (
                //     "Error",
                //     "Error al recibir los datos del servidor, intente mas tarde. Si el error persiste contacte con un desarrollador.", 
                //     "Ok"
                // );
                await DisplayAlert
                (
                    "Error",
                    ex.Message,
                    "Ok"
                );
            }
        }

        private async void UdlBtn_OnClicked(object sender, EventArgs e)
        {
            if (CrossConnectivity.Current.IsConnected)
            {
                UserDialogs.Instance.ShowLoading();
                try
                {
                    var data = await App.SQLiteDB.GetAllClientes();

                    foreach (var c in data)
                    {
                        var d = await ApiClient.GetTicketsByDni(c.dni);
                        foreach (var de in d)
                        {
                            if (de.FechaV.Date == c.fechayhora.Date)
                            {
                                await ApiClient.PutTicket(de.idEntradas.ToString(), Convert.ToDateTime(c.Show));
                                await App.SQLiteDB.DeleteClienteAsync(c);
                                break;
                            }
                        }
                    }
                    UserDialogs.Instance.HideLoading();
                    await DisplayAlert("", "La operacion se realizo con exito.", "Ok");
                }
                catch (Exception exception)
                {
                    UserDialogs.Instance.HideLoading();
                    await DisplayAlert("Error", exception.Message, "Ok");
                    Console.WriteLine(exception);
                }
            }
            else
            {
                await DisplayAlert("Error", "Necesita estar conectado a internet para utilizar esta funcion", "Ok");
            }
        }

        private async void CloseBtn_OnClicked(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }
    }
}