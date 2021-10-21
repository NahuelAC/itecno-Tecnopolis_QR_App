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
    public partial class ModePage : ContentPage
    {
        public ModePage()
        {
            InitializeComponent();
            
            picker.Items.Add("Tecnopolis");
            picker.Items.Add("Centro Cultural Kirchner");
        }

        private async void CloseBtn_OnClicked(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }

        private async void CBtn_OnClicked(object sender, EventArgs e)
        {
            string mode = picker.SelectedItem.ToString();
            string password = entry.Text;

            if (password == "arqytechmodepassword")
            {
                
            }
        }
    }
}