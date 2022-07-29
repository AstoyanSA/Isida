using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using SQLite;
using Xamarin.Essentials;

namespace isida
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TovKorzina : ContentPage
    {
        public TovKorzina()
        {
            InitializeComponent();

        }

        protected override async void OnAppearing()
        {          

            // создание таблицы, если ее нет
            await App.Database2.CreateTable();

            // привязка данных
            tovKorzList.ItemsSource = await App.Database2.GetItemsAsync();


            string userInfo = Preferences.Get("user", "Введите данные!");

            user_label.Text = userInfo;


            SummCalc();

            base.OnAppearing();
        }

        public void dostavkaCheck(object sender, EventArgs e)
        {
            if (dostavka.IsChecked == true)
            {
                user_adress.IsEnabled = true;
            }
            else
            {
                user_adress.IsEnabled = false;
            }
        }

        public void GetInfo(string user)
        {
            Preferences.Set("user", user);
        }

        public async void SummCalc()
        {
            
            Itog.Text = "Итого:     ";
            int s = await App.Database2.GetSummAsync();

            Itog.Text += s + " руб.";

        }

        private async void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            TovKorz selectedFriend = (TovKorz)e.SelectedItem;
            ChangeKorz friendPage = new ChangeKorz();
            friendPage.BindingContext = selectedFriend;
            await Navigation.PushAsync(friendPage);
        }

        public async void Zakaz(object sender, EventArgs e)
        {

            int s = await App.Database2.GetSummAsync();

            var current = Connectivity.NetworkAccess;

            if (current == NetworkAccess.Internet)
            {
                if (s > 0)
                {
                    if ((user_label.Text.Length < 10) || (user_label.Text == null) || (user_label.Text == "") || (user_label.Text == "Введите данные!"))
                    {
                        await DisplayAlert("Уведомление", "ВВЕДИТЕ ДАННЫЕ ПОЛЬЗОВАТЕЛЯ!", "OK");
                        (this.Parent as TabbedPage).CurrentPage = (Parent as TabbedPage).Children[0];
                    }
                    else
                    {
                        if (dostavka.IsChecked == false)
                        {
                            user_adress.Text = "Самовывоз";

                            SendMail();
                        }
                        else
                        {
                            if ((user_adress.Text == "Самовывоз") || (user_adress.Text == null))
                            {
                                await DisplayAlert("Уведомление", "Введите адрес!", "ОК");
                            }
                            else
                            {
                                SendMail();
                            }
                        }
                    }
                }
                else
                {
                    await DisplayAlert("Уведомление", "Корзина пуста!", "ОК");
                }
            }
            else
            {
                await DisplayAlert("Уведомление", "Проверьте подключение к интернету!", "ОК");
            }
            
        }

        public async void SendMail()
        {
            string arr ="";
            foreach (TovKorz t in tovKorzList.ItemsSource)
            {
                arr += "Наименование: " + t.Name + ". Цена: " + t.Cost + " руб. " + "Количество: " + t.Kol + " шт.\n";
            }
            string content_info = arr + Itog.Text + " " + "Доставка: " + user_adress.Text + ". " + "Данные: " + user_label.Text;

            SmtpClient client = new SmtpClient("smtp.mail.ru", 2525);
            client.EnableSsl = true;
            client.Credentials = new NetworkCredential("test@mail.ru", "test-key");

            string msgFrom = "test@mail.ru";

            string msgTo = "test@mail.ru";

            string msgSubject = "Новый заказ!";

            string msgBody = String.Format(content_info);

            MailMessage msg = new MailMessage(msgFrom, msgTo, msgSubject, msgBody);

            try
            {
                client.Send(msg);
                await DisplayAlert("Уведомление", "Заказ принят!", "ОК");
            }
            catch (Exception)
            {
                await DisplayAlert("Уведомление", "Ошибка!", "ОК");
            }
        }
    }
}