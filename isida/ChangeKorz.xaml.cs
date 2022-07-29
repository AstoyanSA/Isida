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
    public partial class ChangeKorz : ContentPage
    {
        public ChangeKorz()
        {
            InitializeComponent();
            ((NavigationPage)Application.Current.MainPage).BarBackgroundColor = Color.FromHex("#066839");
        }

        private async void SaveFriend(object sender, EventArgs e)
        {
            var tovkorz = (TovKorz)BindingContext;
            if (!String.IsNullOrEmpty(tovkorz.Name))
            {
                await App.Database2.SaveItemAsync(tovkorz);
            }
            await this.Navigation.PopAsync();
        }
        private async void DeleteFriend(object sender, EventArgs e)
        {
            var tovkorz = (TovKorz)BindingContext;
            await App.Database2.DeleteItemAsync(tovkorz);
            await this.Navigation.PopAsync();
        }
        private async void Cancel(object sender, EventArgs e)
        {
            await this.Navigation.PopAsync();
        }
    }
}