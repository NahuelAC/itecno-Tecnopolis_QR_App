using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using ZXing.Net.Mobile.Forms;
using static Tecnopolis_QR_App.Pass;

namespace Tecnopolis_QR_App
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();

            xd();
        }

        private async void BtnScannerQR_OnClicked(object sender, EventArgs e)
        {
            try
            {
                var scanner = new ZXing.Mobile.MobileBarcodeScanner();
                //scanner.UseCustomOverlay = true;

                scanner.TopText = "Escanear QR";

                var result = await scanner.Scan();

                var separator = '-';
                string[] qr_data = result.Text.Split(separator);

                if (result != null)
                {
                    //await DisplayAlert("Informacion QR", $"Data 1: {qr_data[0]}\nData 2: {qr_data[1]}\nData 3: {qr_data[2]}", "Ok");
                    await Navigation.PushModalAsync(new Pass(qr_data[1]));
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message.ToString(), "Ok");
            }
        }

        private async void xd()
        {
            await Navigation.PushModalAsync(new NotPass());

        }

        private async void BtnInputDNI_OnClicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new InputDni());

        }
    }
}
