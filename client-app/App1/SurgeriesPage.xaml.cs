using App1.Models;
using MongoDB.Bson;
using Realms;
using Realms.Sync;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using AsyncTask = System.Threading.Tasks.Task;

namespace App1.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SurgeriesPage : ContentPage
    {
        public static Realms.Sync.App RealmApp;
        public Realm Realm;

        public Models.Environment CurrentEnvironment = Models.Environment.None;

        public static Dictionary<Models.Environment, string> Apps = new Dictionary<Models.Environment, string>()
        {
            { Models.Environment.Development, "dev_app-wzipx" },
            { Models.Environment.Staging, "stage_app-ecbqh" },
            { Models.Environment.LiveA, "livea_app-jmgtv" }
        };

        public ObservableCollection<Surgery> Items { get; set; }

        public SurgeriesPage()
        {
            InitializeComponent();

            Items = new ObservableCollection<Surgery>();
            MyListView.ItemsSource = Items;
        }


        protected override async void OnAppearing()
        {
            string envByUser = await DisplayActionSheet("Choose environment to link to:", "Cancel", null, 
                Models.Environment.Development.ToString(), 
                Models.Environment.Staging.ToString(),
                Models.Environment.LiveA.ToString());

            if(envByUser == null)
            { 
                return; 
            }

            var chosenEnvironment = (Models.Environment)Enum.Parse(typeof(Models.Environment), envByUser);
            if (chosenEnvironment != CurrentEnvironment)
            { 
                CurrentEnvironment = chosenEnvironment;

                var realmAppId = Apps[chosenEnvironment];

                RealmApp = Realms.Sync.App.Create(realmAppId);
            }

            var user = await RealmApp.LogInAsync(Credentials.EmailPassword("user12@example.com", "user12"));

            SyncConfiguration syncConfig = new SyncConfiguration("my-value-1", RealmApp.CurrentUser);
            try
            {
                Realm = await Realm.GetInstanceAsync(syncConfig);
                    
                MyTitle.Text = $"{Apps[chosenEnvironment]} ({envByUser})";

            }
            catch (Exception e)
            {
                await DisplayAlert("Error", e.Message, "ok");
            }
            
            await PopulateItemsList();
        }

        private async AsyncTask PopulateItemsList()
        {
            try
            {
                var surgeryList = Realm.All<Surgery>().ToList();
                
                var firstXBySurgeonName = surgeryList.OrderBy(s => s.Surgeon).Take(10);
                
                var observableList = new ObservableCollection<Surgery>(firstXBySurgeonName.ToList());

                Items = observableList;
                MyListView.ItemsSource = Items;

            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "ok");
            }
        }

        protected override async void OnDisappearing()
        {
            if (Realm != null)
            {
                Realm.Dispose(); 
            }
            if (RealmApp.CurrentUser != null) 
            {
                await RealmApp.CurrentUser.LogOutAsync();
            }
            RealmApp = null;
            MyListView.ItemsSource = null;
        }
    }
}