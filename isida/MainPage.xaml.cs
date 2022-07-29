using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace isida
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            MainAlet();
        }

        public async void MainAlet()
        {
            await DisplayAlert("ВНИМАНИЕ!", "Для оформления заказа необходим доступ к сети интернет!", "ОК");
        }
    }
}
