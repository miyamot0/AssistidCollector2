using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Acr.UserDialogs;
using AssistidCollector2.Enums;
using AssistidCollector2.Helpers;
using AssistidCollector2.Models;
using AssistidCollector2.Storage;
using Plugin.Connectivity;
using Xamarin.Forms;

namespace AssistidCollector2.Tasks
{
    public partial class TaskExercise : ContentPage
    {
        List<SocialInclusionStep> taskModels;
        DateTime startTime;

        private const int PageType = Identifiers.EnjoyExercise;

        public TaskExercise()
        {
            InitializeComponent();

            Title = "Enjoy Exercise";

            startTime = DateTime.Now;

            taskModels = new List<SocialInclusionStep>();

            NavigationPage.SetHasNavigationBar(this, true);
            NavigationPage.SetHasBackButton(this, true);
            NavigationPage.SetBackButtonTitle(this, "Back");

            PollForDataAsync();
        }

        /// <summary>
        /// Polls for data async.
        /// </summary>
        /// <param name="pageType">Page type.</param>
        void PollForDataAsync()
        {
            ViewTools.HandlePollDataAsync(taskModels, customPageStackContent, PageType);
        }

        /// <summary>
        /// Handles the save clicked.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">E.</param>
        async void Handle_Save_ClickedAsync(object sender, System.EventArgs e)
        {
            if ((sender as Button) != null) { (sender as Button).IsEnabled = false; }

            string returnString = ViewTools.CommaSeparatedValue("Data,Value", "InterventionCode," + PageType.ToString(),
                                                                customPageStackContent, startTime, DateTime.Now.Subtract(startTime));

            int result = await App.Database.SaveItemAsync(new StorageModel()
            {
                CSV = returnString,
                Intervention = PageType.ToString()
            });

            if (CrossConnectivity.Current.IsConnected)
            {
                CancellationTokenSource cancelSrc = new CancellationTokenSource();
                ProgressDialogConfig config = new ProgressDialogConfig()
                    .SetTitle("Uploading to Server")
                    .SetIsDeterministic(false)
                    .SetMaskType(MaskType.Black)
                    .SetCancel(onCancel: cancelSrc.Cancel);

                using (IProgressDialog progress = UserDialogs.Instance.Progress(config))
                {
                    await DropboxServer.UploadFile(new System.IO.MemoryStream(Encoding.UTF8.GetBytes(returnString)), App.Database.GetLargestID());

                    await Task.Delay(100);
                }
            }

            App.RefreshServer = true;

            await Navigation.PopAsync();
        }

        /// <summary>
        /// Handles the add steps clicked async.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">E.</param>
        async void Handle_Add_Steps_ClickedAsync(object sender, System.EventArgs e)
        {
            EventWaitHandle waitHandle = new EventWaitHandle(false, EventResetMode.AutoReset);

            TaskCreator mNewView = new TaskCreator();

            mNewView.Disappearing += (sender2, e2) =>
            {
                if (App.isTakingPictures == false)
                {
                    waitHandle.Set();
                }
            };

            await Navigation.PushModalAsync(mNewView);

            await Task.Run(() => waitHandle.WaitOne());

            //Debug.WriteLineIf(App.Debugging, "Fired");

            ViewTools.HandleStepAddedAsync(taskModels, customPageStackContent, PageType);
        }

        /// <summary>
        /// Handles the edit steps clicked.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">E.</param>
        void Handle_Edit_Steps_Clicked(object sender, System.EventArgs e)
        {
            ViewTools.HandleStepRemovalAsync(taskModels, customPageStackContent, PollForDataAsync);
        }
    }
}
