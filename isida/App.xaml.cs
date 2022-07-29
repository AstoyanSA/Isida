using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.IO;
using System.Reflection;
using System.Collections.Generic;
using System.Threading.Tasks;
using SQLite;
using System.Text;
using System.Globalization;
using System.Net;
using System.Net.Mail;

namespace isida
{
    [Table("Tovari")]
    public class Tovar
    {
        [PrimaryKey, AutoIncrement, Column("_id")]
        public int Id { get; set; }

        public string Name { get; set; }
        public int Cost { get; set; }

        public void addTovar(string name, int cost)
        {
            this.Name = name;
            this.Cost = cost;
        }
    }

    [Table("Korzina")]
    public class TovKorz
    {
        [PrimaryKey, AutoIncrement, Column("_id")]
        public int Id { get; set; }

        public string Name { get; set; }
        public int Cost { get; set; }
        public int Kol { get; set; }

        public int Summ { get; set; }
    }

    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            MainPage = new NavigationPage(new Tabbed());
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }



        public const string DATABASE_NAME = "tov.db";
        public static TovarAsyncRepository database;
        public static TovarAsyncRepository Database
        {
            get
            {
                if (database == null)
                {
                    // путь, по которому будет находиться база данных
                    string dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), DATABASE_NAME);
                    // если база данных не существует (еще не скопирована)
                    if (!File.Exists(dbPath))
                    {
                        // получаем текущую сборку
                        var assembly = IntrospectionExtensions.GetTypeInfo(typeof(App)).Assembly;
                        // берем из нее ресурс базы данных и создаем из него поток
                        using (Stream stream = assembly.GetManifestResourceStream($"isida.{DATABASE_NAME}"))
                        {
                            using (FileStream fs = new FileStream(dbPath, FileMode.OpenOrCreate))
                            {
                                stream.CopyTo(fs);  // копируем файл базы данных в нужное нам место
                                fs.Flush();
                            }
                        }
                    }
                    database = new TovarAsyncRepository(dbPath);
                }
                return database;
            }
        }


        public const string DATABASE_NAME2 = "korzina.db";
        public static TovKorzAsyncRepository database2;
        public static TovKorzAsyncRepository Database2
        {
            get
            {
                if (database2 == null)
                {
                    // путь, по которому будет находиться база данных
                    string dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), DATABASE_NAME2);
                    // если база данных не существует (еще не скопирована)
                    if (!File.Exists(dbPath))
                    {
                        // получаем текущую сборку
                        var assembly = IntrospectionExtensions.GetTypeInfo(typeof(App)).Assembly;
                        // берем из нее ресурс базы данных и создаем из него поток
                        using (Stream stream = assembly.GetManifestResourceStream($"isida.{DATABASE_NAME2}"))
                        {
                            using (FileStream fs = new FileStream(dbPath, FileMode.OpenOrCreate))
                            {
                                stream.CopyTo(fs);  // копируем файл базы данных в нужное нам место
                                fs.Flush();
                            }
                        }
                    }
                    database2 = new TovKorzAsyncRepository(dbPath);
                }
                return database2;
            }
        }
    }



    public class TovarAsyncRepository
    {
        SQLiteAsyncConnection database;

        public TovarAsyncRepository(string databasePath)
        {
            database = new SQLiteAsyncConnection(databasePath);
        }

        public async Task CreateTable()
        {
            await database.CreateTableAsync<Tovar>();
        }

        public async Task DropTable()
        {
            await database.DropTableAsync<Tovar>();
        }

        public async Task<List<Tovar>> GetItemsAsync()
        {
            return await database.Table<Tovar>().ToListAsync();
        }

        public async Task<Tovar> GetItemAsync(int id)
        {
            return await database.GetAsync<Tovar>(id);
        }
        public async Task<int> DeleteItemAsync(Tovar item)
        {
            return await database.DeleteAsync(item);
        }
        public async Task<int> SaveItemAsync(Tovar item)
        {
            if (item.Id != 0)
            {
                await database.UpdateAsync(item);
                return item.Id;
            }
            else
            {
                return await database.InsertAsync(item);
            }
        }
    }

    public class TovKorzAsyncRepository
    {
        SQLiteAsyncConnection database2;

        public TovKorzAsyncRepository(string databasePath)
        {
            database2 = new SQLiteAsyncConnection(databasePath);
        }

        public async Task CreateTable()
        {
            await database2.CreateTableAsync<TovKorz>();
        }
        public async Task<List<TovKorz>> GetItemsAsync()
        {
            return await database2.Table<TovKorz>().ToListAsync();

        }
        public async Task<TovKorz> GetItemAsync(int id)
        {
            return await database2.GetAsync<TovKorz>(id);
        }

        public async Task<int> GetSummAsync()
        {
            return await database2.ExecuteScalarAsync<int>("SELECT Sum(Summ) FROM [Korzina] ");
        }

        public async Task<int> DeleteItemAsync(TovKorz item)
        {
            return await database2.DeleteAsync(item);
        }
        public async Task<int> SaveItemAsync(TovKorz item)
        {
            if (item.Id != 0)
            {
                await database2.UpdateAsync(item);
                return item.Id;
            }
            else
            {
                return await database2.InsertAsync(item);
            }
        }
    }
}
