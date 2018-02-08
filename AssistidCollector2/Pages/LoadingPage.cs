using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Acr.UserDialogs;
using AssistidCollector2.Helpers;
using AssistidCollector2.Storage;
using AssistidCollector2.Tasks;
using Dropbox.Api.Files;
using Plugin.Connectivity;
using Xamarin.Forms;

namespace AssistidCollector2.Pages
{
    public class LoadingPage : ContentPage
    {
        /// <summary>
        /// Loading page for manifest
        /// </summary>
        public LoadingPage()
        {
            Content = new StackLayout
            {
                BackgroundColor = Color.FromHex("31D3F0"),
                Children = {
                    /*
                    new Image()
                    {
                        Source = "splash.png",
                        Aspect = Aspect.AspectFill,
                        VerticalOptions = LayoutOptions.CenterAndExpand
                    }
                    */
                }
            };
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            CheckCredentials();
        }

        /// <summary>
        /// CheckCredentials()
        /// </summary>
        public async void CheckCredentials()
        {
            Debug.WriteLineIf(App.Debugging, "CheckCredentials()");

            if (App.AccessToken == null || App.AccessToken == "")
            {
                var userInput = await UserDialogs.Instance.PromptAsync("Please input API token", null, "OK", "Cancel", "Api Token");

                Debug.WriteLineIf(App.Debugging, userInput.Text);

                App.AccessToken = userInput.Text;

                App.ReloadDropbox();
            }
            else
            {
                App.ReloadDropbox();
            }

            LoadAssets();
        }

        /// <summary>
        /// Load Stuff
        /// </summary>
        public async void LoadAssets()
        {
            // Skip loading if no internet
            if (!CrossConnectivity.Current.IsConnected)
            {
                App.Current.MainPage = new NavigationPage(new TaskPageStart());
            }

            Debug.WriteLineIf(App.Debugging, "LoadAssets()");

            CancellationTokenSource cancelSrc = new CancellationTokenSource();
            ProgressDialogConfig config = new ProgressDialogConfig()
                .SetTitle("Contacting Server")
                .SetIsDeterministic(false)
                .SetMaskType(MaskType.Black)
                .SetCancel(onCancel: cancelSrc.Cancel);

            using (IProgressDialog progress = UserDialogs.Instance.Progress(config))
            {
                try
                {
                    if (App.UpdatingAttempts)
                    {
                        /* Stubbed
                        progress.Title = "Downloading manifest";

                        var mManifest = await App.Database.GetManifestAsync();

                        if (mManifest != null && mManifest.Count == 1)
                        {
                            App.MainManifest = JsonConvert.DeserializeObject<Manifest>(mManifest.First().JSON);
                        }
                        else
                        {
                            App.MainManifest = null;
                        }

                        progress.Title = "Parsing Manifest";

                        await DropboxServer.DownloadManifest(App.MainManifest);
                        */
                    }

                    progress.Title = "Polling local database";

                    List<StorageModel> currentItems = null;

                    try
                    {
                        currentItems = await App.Database.GetDataAsync();
                    }
                    catch (Exception e)
                    {
                        currentItems = null;

                        Debug.WriteLineIf(App.Debugging, e.ToString());
                    }

                    progress.Title = "Polling remote database";

                    ListFolderResult serverFiles = await DropboxServer.CountIndividualFiles();

                    int count = 0;

                    if (serverFiles == null || currentItems == null || cancelSrc.IsCancellationRequested)
                    {
                        // Nothing.. just move on
                    }
                    else if (currentItems.Count == serverFiles.Entries.Count)
                    {
                        // Same.. no worries
                    }
                    else if (currentItems.Count > serverFiles.Entries.Count)
                    {
                        List<int> localIds = currentItems.Select(l => l.ID).ToList();

                        List<string> remoteIdsStr = serverFiles.Entries.Select(r => r.Name).ToList();
                        remoteIdsStr = remoteIdsStr.Select(r => r.Split('_')[1]).ToList();
                        remoteIdsStr = remoteIdsStr.Select(r => r.Split('.')[0]).ToList();

                        List<int> remoteIds = remoteIdsStr.Select(r => int.Parse(r)).ToList();

                        var missing = localIds.Except(remoteIds);

                        foreach (int index in missing)
                        {
                            if (cancelSrc.IsCancellationRequested)
                            {
                                continue;
                            }

                            progress.Title = "Uploading File " + (count + 1) + " of " + missing.Count().ToString();

                            var mStorageModel = currentItems.Single(m => m.ID == index);

                            await DropboxServer.UploadFile(new System.IO.MemoryStream(Encoding.UTF8.GetBytes(mStorageModel.CSV)), mStorageModel.ID);

                            await Task.Delay(App.DropboxDeltaTimeout);

                            count++;
                        }
                    }
                }
                catch (Exception e)
                {
                    Debug.WriteLineIf(App.Debugging, e.ToString());
                }

                App.Current.MainPage = new NavigationPage(new TaskPageStart());
            }
        }

        /// <summary>
        /// Base methods
        /// </summary>
        /// <returns></returns>
        protected override bool OnBackButtonPressed()
        {
            base.OnBackButtonPressed();
            return true;
        }
    }
}

