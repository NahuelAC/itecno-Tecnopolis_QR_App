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

        //private bool CanPass(string[] qr_data)
        //{
        //    bool response = false;
        //    DateTime dt_actual = DateTime.Now;
        //    DateTime qr_dt = Convert.ToDateTime(qr_data[2]);
        //    string qr_dni = qr_data[1];

        //    if (qr_dt.Date == dt_actual.Date && dt_actual.Hour <= (qr_dt.Hour + 2))
        //    {
        //        bool data = new DatabaseOnline().getInfo(qr_dni, qr_dt);
        //        response = data;
        //    }

        //    return response;
        //}

        //private async void RegistBtn_OnClickedAsync(object sender, EventArgs e)
        //{
        //    bool can_pass = CanPass(qr_data);

        //    if (can_pass)
        //    {
        //        await Navigation.PushModalAsync(new Pass(qr_data[3]));
        //    }
        //    else
        //    {
        //        await Navigation.PushModalAsync(new NotPass());
        //    }
        //}
    }
}