using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Linq;
using System.IO;
using System.Reflection;
using System.Collections.Generic;
using System.Threading.Tasks;
using SQLite;
using System.Globalization;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace isida
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Tovari : ContentPage
    {


        public Tovari()
        {
            InitializeComponent();

            loadTovar();
        }

        protected override void OnAppearing()
        {
            MainSearchBar.Text = "";
            tovarList.SelectedItem = null;

            base.OnAppearing();
        }


        private async void loadTovar()
        {
            await App.Database.DropTable();

            await App.Database.CreateTable();

            string[,] arr =
            {
                { "Анаферон", "170"},
                {"Аспирин", "40"},
                { "Верошпирон", "85"},
                {"Гематоген","24" },
                { "Глево","43" },
                { "Диклофенак","230"},
                {"Ибупрофен","180" },
                { "Имудон", "300"},
                {"Мезим", "220" },
                {"Нимесил", "212" },
                {"Но-шпа", "145" },
                {"Пенталгин", "410" },
                {"Эналаприл", "268" }

            };

            for (int i = 1; i < arr.GetLength(0); i++)
            {
                Tovar tov = new Tovar();
                tov.addTovar(arr[i, 0], Convert.ToInt32(arr[i, 1]));
                await App.Database.SaveItemAsync(tov);
            }
            tovarList.ItemsSource = await App.Database.GetItemsAsync();
        }

        // обработка нажатия элемента в списке
        private async void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            Tovar selectedFriend = (Tovar)e.SelectedItem;
            Korzina friendPage = new Korzina();
            friendPage.BindingContext = selectedFriend;
            if (((ListView)sender).SelectedItem != null) await Navigation.PushAsync(friendPage);
        }

        public List<Tovar> x;
        public async Task InitAsync()
        {

            var p = await App.database.GetItemsAsync();

            x = p;
        }

        private async void SearchBar_TextChanged(object sender, TextChangedEventArgs e)
        {
            var items = await App.Database.GetItemsAsync();

            tovarList.BeginRefresh();

            if (string.IsNullOrWhiteSpace(e.NewTextValue))
                tovarList.ItemsSource = items;
            else
                tovarList.ItemsSource = items.Where(i => i.Name.ToLower().Contains(e.NewTextValue.ToLower()));


            tovarList.EndRefresh();
        }
    }    
}