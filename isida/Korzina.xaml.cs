using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace isida
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Korzina : ContentPage
    {
        public Korzina()
        {
            InitializeComponent();
            ((NavigationPage)Application.Current.MainPage).BarBackgroundColor = Color.FromHex("#066839");
        }

        public ListView itemTov;

        private async void SaveTovar(object sender, EventArgs e)
        {
            var tovar = (Tovar)BindingContext;
            var Tname = tovar.Name;
            var Tcost = tovar.Cost;
            TovKorz tovkorz = new TovKorz();
            tovkorz.Name = Tname;
            tovkorz.Cost = Tcost;
            if (Convert.ToInt32(EntryKol.Text) < 1)
            {
                tovkorz.Kol = 1;
            }
            else
            {
                tovkorz.Kol = Convert.ToInt32(EntryKol.Text);
            }
            tovkorz.Summ = tovkorz.Kol * tovkorz.Cost;
            /*
            var itemsKorzina = await App.Database2.GetItemAsync();

            if (itemsKorzina.Name == Tname)
            {
                await DisplayAlert("Уведомление", "Товар уже в корзине!", "OK");
            }
            else
            {
                await App.Database2.SaveItemAsync(tovkorz);
                await this.Navigation.PopAsync();
            }

            itemTov.ItemsSource = (await App.Database2.GetItemsAsync()).Where(i => i.Name.ToLower().Contains(Tname.ToLower()));

            itemTov.;

            if (itemTov.ItemsSource == null)
            {
                await DisplayAlert("Уведомление", tovkorz.Name, "OK");
            }
            else
            {
                await App.Database2.SaveItemAsync(tovkorz);
                await this.Navigation.PopAsync();
            }
            */

            
            await App.Database2.SaveItemAsync(tovkorz);
            await this.Navigation.PopAsync();
        }
    }
}