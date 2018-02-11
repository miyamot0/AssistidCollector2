using System;
using System.Collections.Generic;

using Xamarin.Forms;
using AssistidCollector2.Models;
using AssistidCollector2.Enums;
using AssistidCollector2.Views;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Acr.UserDialogs;
using AssistidCollector2.Storage;
using System.Linq;
using Xamarin.Forms.Internals;

namespace AssistidCollector2.Tasks
{
    public partial class TaskCustomize : ContentPage
    {
        List<SocialInclusionStep> taskModels;
        CardViewStepTemplate cardCheckTemplate;
        DateTime startTime;

        private int PageType = Identifiers.CreatedActivity;

        public TaskCustomize()
        {
            InitializeComponent();

            Title = "Personalized Activity List";

            startTime = DateTime.Now;

            taskModels = new List<SocialInclusionStep>();

            NavigationPage.SetHasNavigationBar(this, true);
            NavigationPage.SetHasBackButton(this, true);
            NavigationPage.SetBackButtonTitle(this, "Back");

            PollForDataAsync(PageType);
        }

        /// <summary>
        /// Polls for data async.
        /// </summary>
        /// <param name="pageType">Page type.</param>
        async void PollForDataAsync(int pageType)
        {
            try
            {
                //var mStoredSteps = await App.Database.GetSteps(PageType);
                var mStoredSteps = await App.Database.GetStepsAsync();

                if (mStoredSteps != null)
                {
                    var mSpecificSteps = mStoredSteps.Where(model => model.TaskType == pageType).ToList();

                    if (mSpecificSteps == null)
                    {
                        return;
                    }

                    foreach (SocialStepModel model in mStoredSteps)
                    {
                        Debug.WriteLineIf(App.Debugging, "Save_StepAsync: StepTitle >>> " + model.Title);
                        Debug.WriteLineIf(App.Debugging, "Save_StepAsync: StepDescription >>> " + model.Description);
                        //Debug.WriteLineIf(App.Debugging, "Save_StepAsync: StepImgPath >>> " + model.ImgPath);

                        taskModels.Add(new SocialInclusionStep()
                        {
                            ID = model.ID,
                            PageType = model.TaskType,
                            Title = model.Title,
                            Description = model.Description,
                            ImgBytes = model.ImgBytes
                        });
                    }

                    foreach (SocialInclusionStep item in taskModels)
                    {
                        cardCheckTemplate = new CardViewStepTemplate(item);
                        customPageStackContent.Children.Add(cardCheckTemplate);
                    }                    
                }
            }
            catch (Exception e)
            {
                Debug.WriteLineIf(App.Debugging, "Exception: " + e.ToString());
            }
        }

        /// <summary>
        /// Handles the save clicked.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">E.</param>
        async void Handle_Save_ClickedAsync(object sender, System.EventArgs e)
        {
            if ((sender as Button) != null) { (sender as Button).IsEnabled = false; }

            /*

            string returnString = ViewTools.CommaSeparatedValue("Data,Value", "Intervention,Sleep Onset",
                sleepOnsetStackContent, taskModels,
                startTime, DateTime.Now.Subtract(startTime));

            int result = await App.Database.SaveItemAsync(new StorageModel()
            {
                CSV = returnString,
                Intervention = "Sleep Onset"
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

            

            */

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
            var mNewView = new TaskCreator();
            mNewView.Disappearing += (sender2, e2) =>
            {
                if (App.isTakingPictures == false)
                {
                    waitHandle.Set();
                }
            };

            await Navigation.PushModalAsync(mNewView);

            await Task.Run(() => waitHandle.WaitOne());

            Debug.WriteLineIf(App.Debugging, "Fired");

            AddNewContent();
        }

        /// <summary>
        /// Adds the new content.
        /// </summary>
        private async void AddNewContent()
        {
            if (App.temporaryStep.Title == "" ||
                App.temporaryStep.Description == "" ||
                App.temporaryStep.ImgBytes == "")
            {
                await UserDialogs.Instance.AlertAsync("Error adding new items", "Try again");
            }
            else
            {
                var task = new SocialInclusionStep()
                {
                    PageType = PageType,
                    Title = App.temporaryStep.Title,
                    Description = App.temporaryStep.Description,
                    ImgBytes = App.temporaryStep.ImgBytes
                };

                taskModels.Add(task);

                cardCheckTemplate = new CardViewStepTemplate(task);
                customPageStackContent.Children.Add(cardCheckTemplate);

                try
                {
                    var result = await App.Database.SaveItemAsync(new SocialStepModel()
                    {
                        TaskType = PageType,
                        Title = App.temporaryStep.Title,
                        Description = App.temporaryStep.Description,
                        ImgBytes = App.temporaryStep.ImgBytes
                    });

                    Debug.WriteLineIf(App.Debugging, "Result: " + result.ToString());
                }
                catch (Exception e)
                {
                    Debug.WriteLineIf(App.Debugging, "Exceptoin: " + e.ToString());
                    
                }

            }
        }

        async

        /// <summary>
        /// Handles the edit steps clicked.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">E.</param>
        void Handle_Edit_Steps_ClickedAsync(object sender, System.EventArgs e)
        {
            string[] stepsInList = taskModels.Select(m => m.Title).ToArray();

            if (stepsInList == null || stepsInList.Length == 0)
            {
                return;
            }

            string destroyString = "OK";

            CancellationTokenSource cancelSrc = new CancellationTokenSource();

            string result = await UserDialogs.Instance.ActionSheetAsync("Pick Item to Edit", "Close", destroyString, cancelSrc.Token, stepsInList);

            Debug.WriteLineIf(App.Debugging, result);

            if (result != destroyString)
            {
                bool promptDelete = await UserDialogs.Instance.ConfirmAsync("Delete step?", "Confirm", destroyString, "Cancel", cancelSrc.Token);

                int indexWithinList = stepsInList.IndexOf(result);

                Debug.WriteLineIf(App.Debugging, "indexWithinList: " + indexWithinList.ToString());

                if (indexWithinList != -1)
                {
                    var item = taskModels.Where(m => m.ID == taskModels.ElementAt(indexWithinList).ID).First();

                    taskModels.Clear();

                    customPageStackContent.Children.Clear();

                    await App.Database.DeleteStepAsync(item.ID);

                    PollForDataAsync(PageType);
                }
            }
        }
    }
}
