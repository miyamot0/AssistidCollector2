using System;
using System.Linq;
using AssistidCollector2.Helpers;
using AssistidCollector2.Interfaces;
using AssistidCollector2.Pages;
using AssistidCollector2.Storage;
using AssistidCollector2.Tasks;
using Dropbox.Api;
using Xamarin.Forms;

namespace AssistidCollector2
{
    public partial class App : Application
    {
        public static string AccessToken
        {
            get
            {
                return Settings.AuthToken;
            }
            set
            {
                Settings.AuthToken = value;
            }
        }

        public static string ApplicationId
        {
            get
            {
                return Settings.AppId;
            }
            set
            {
                Settings.AppId = value;
            }
        }

        public static string ApplicationName
        {
            get
            {
                return Settings.AppName;
            }
            set
            {
                Settings.AppName = value;
            }
        }

        private static DropboxClient dropboxClient;
        public static DropboxClient DropboxClient
        {
            get
            {
                return dropboxClient;
            }
            private set
            {
                using (DropboxClient old = dropboxClient)
                {
                    dropboxClient = value;
                }

                DropboxClientChanged?.Invoke(dropboxClient, EventArgs.Empty);
            }
        }

        private static ApplicationDatabase database;
        public static ApplicationDatabase Database
        {
            get
            {
                if (database == null)
                {
                    database = new ApplicationDatabase(DependencyService.Get<InterfaceSaveLoad>().GetLocalFilePath("Database.db3"));
                }

                return database;
            }
        }

        public static event EventHandler DropboxClientChanged;

        private static Random random = new Random();
        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public static bool Debugging = true;
        public static bool UpdatingAttempts = false;

        public static int DropboxDeltaTimeout = 2000;

        public static bool RefreshServer = false;

        public App()
        {
            InitializeComponent();

            Database.Init();

            if (ApplicationId == string.Empty)
            {
                ApplicationId = String.Format("{0:X}", RandomString(12));
            }

            if (ApplicationName == string.Empty)
            {
                ApplicationName = "AssistidApp01";
            }

            if (Device.RuntimePlatform == Device.Android)
            {
                bool isAdministrator = DependencyService.Get<InterfaceAdministrator>().IsAdmin();
                DependencyService.Get<InterfaceAdministrator>().RequestAdmin(isAdministrator);
            }

            MainPage = new NavigationPage(new LoadingPage());
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }

        /// <summary>
        /// Re-init dropbox
        /// </summary>
        public static void ReloadDropbox()
        {
            dropboxClient = new DropboxClient(AccessToken, new DropboxClientConfig(ApplicationName));
        }

        public void ShowStartPage()
        {
            MainPage = new NavigationPage(new TaskPageStart());
        }
    }
}
