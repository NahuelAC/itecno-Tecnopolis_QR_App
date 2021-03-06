using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Tecnopolis_QR_App
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NotPass : ContentPage
    {
        public NotPass(string message = null)
        {
            InitializeComponent();
            Message.Text = message;
        }
        private async void CloseBtn_OnClicked(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }
    }
}