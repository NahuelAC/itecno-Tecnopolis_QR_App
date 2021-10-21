using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Tecnopolis_QR_App.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ConfigPage : ContentPage
    {
        public ConfigPage()
        {
            InitializeComponent();
        }

        private async void CloseBtn_OnClicked(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }

        private void UdlBtn_OnClicked(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void AdbBtn_OnClicked(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private async void CmBtn_OnClicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new ModePage());
        }
    }
}