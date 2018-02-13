﻿using System;
using System.Collections.Generic;

using Xamarin.Forms;
using AssistidCollector2.Models;
using AssistidCollector2.Enums;
using System.Threading;
using System.Threading.Tasks;
using Acr.UserDialogs;
using AssistidCollector2.Storage;
using AssistidCollector2.Helpers;
using Plugin.Connectivity;
using System.Text;
using System.IO;

namespace AssistidCollector2.Tasks
{
    public partial class TaskCustomize : ContentPage
    {
        List<SocialInclusionStep> taskModels;
        DateTime startTime;

        private int PageType = Identifiers.CreatedActivity;

        public TaskCustomize()
        {
            InitializeComponent();

            Title = Identifiers.GetDescription(PageType);

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

            EventWaitHandle waitHandle = new EventWaitHandle(false, EventResetMode.AutoReset);

            TaskSocialValidity mNewView = new TaskSocialValidity(PageType);

            mNewView.Disappearing += (sender2, e2) =>
            {
                if (App.isTakingPictures == false)
                {
                    waitHandle.Set();
                }
            };

            await Navigation.PushAsync(mNewView);

            await Task.Run(() => waitHandle.WaitOne());

            (sender as Button).IsEnabled = true;

            if (mNewView.GetBase64().Length == 0)
            {
                return;
            }

            string returnString = ViewTools.CommaSeparatedValue("Data,Value",
                                                                "InterventionCode," + PageType.ToString(),
                                                                mNewView.AppRating,
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

                    await DropboxServer.UploadImage(new MemoryStream(Convert.FromBase64String(mNewView.GetBase64())), App.Database.GetLargestFeedbackID());

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

            await Navigation.PushAsync(mNewView);

            await Task.Run(() => waitHandle.WaitOne());

            if (App.temporaryStep.Description == "" || App.temporaryStep.Title == "")
            {
                return;
            }

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
