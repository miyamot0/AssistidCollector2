
//----------------------------------------------------------------------------------------------
// <copyright file="TaskPageStart.xaml.cs" 
// Copyright February 2, 2018 Shawn Gilroy
//
// This file is part of AssistidCollector2
//
// AssistidCollector2 is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, version 3.
//
// AssistidCollector2 is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with AssistidCollector2.  If not, see http://www.gnu.org/licenses/. 
// </copyright>
//
// <summary>
// The AssistidCollector2 is a tool to assist clinicans and researchers in the treatment of communication disorders.
// 
// Email: shawn(dot)gilroy(at)temple.edu
//
// </summary>
//----------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;

using Xamarin.Forms;
using AssistidCollector2.Models;
using AssistidCollector2.Enums;
using AssistidCollector2.Views;
using AssistidCollector2.Interfaces;
using System.Diagnostics;
using Plugin.Connectivity;
using System.Threading;
using Acr.UserDialogs;
using Dropbox.Api.Files;
using AssistidCollector2.Storage;
using AssistidCollector2.Helpers;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace AssistidCollector2.Tasks
{
    public partial class TaskPageStart : ContentPage
    {
        List<SocialInclusionTasks> taskModels;
        TapGestureRecognizer tapGestureRecognizer;
        CardViewTemplate cardViewTemplate;

        public TaskPageStart()
        {
            InitializeComponent();

            taskModels = new List<SocialInclusionTasks>();

            taskModels.Add(new SocialInclusionTasks()
            {
                PageId = Identifiers.FoodShopping,
                PageTitle = Identifiers.GetDescription(Identifiers.FoodShopping),
                PageDescription = "The steps in this section focus on the skills needed to go shopping in the community.",
                PageButton = "Select this option to practice shopping.",
                PageImage = "FoodShopping.png"
            });

            taskModels.Add(new SocialInclusionTasks()
            {
                PageId = Identifiers.SportGames,
                PageTitle = Identifiers.GetDescription(Identifiers.SportGames),
                PageDescription = "This program is dedicated to learning how to play a sport or a game with others.",
                PageButton = "Select this option to practice playing games.",
                PageImage = "Sports.png"
            });

            taskModels.Add(new SocialInclusionTasks()
            {
                PageId = Identifiers.DogWalking,
                PageTitle = Identifiers.GetDescription(Identifiers.DogWalking),
                PageDescription = "These activities focus on take a dog for a walk while out in the community",
                PageButton = "Select this option to practice walking a dog.",
                PageImage = "WalkDog.png"
            });

            taskModels.Add(new SocialInclusionTasks()
            {
                PageId = Identifiers.Movies,
                PageTitle = Identifiers.GetDescription(Identifiers.Movies),
                PageDescription = "This lesson focuses on learning how to interact in the community at the movies.",
                PageButton = "Select this option to practice at the movies.",
                PageImage = "Movies.png"
            });

            taskModels.Add(new SocialInclusionTasks()
            {
                PageId = Identifiers.ListenMusic,
                PageTitle = Identifiers.GetDescription(Identifiers.ListenMusic),
                PageDescription = "This program is dedicated to playing or listening to music with others.",
                PageButton = "Select this option to listen to music.",
                PageImage = "Music.png"
            });

            taskModels.Add(new SocialInclusionTasks()
            {
                PageId = Identifiers.EnjoyExercise,
                PageTitle = Identifiers.GetDescription(Identifiers.EnjoyExercise),
                PageDescription = "This routine focuses on exercising with other in the community.",
                PageButton = "Select this option to practice exercising.",
                PageImage = "Exercise.png"
            });

            taskModels.Add(new SocialInclusionTasks()
            {
                PageId = Identifiers.GoForWalk,
                PageTitle = Identifiers.GetDescription(Identifiers.GoForWalk),
                PageDescription = "These activities are dedicated to taking a trip in the park.",
                PageButton = "Select this option to take a walk.",
                PageImage = "WalkPark.png"
            });

            taskModels.Add(new SocialInclusionTasks()
            {
                PageId = Identifiers.CoffeeShop,
                PageTitle = Identifiers.GetDescription(Identifiers.CoffeeShop),
                PageDescription = "Take a visit to the coffee shop.",
                PageButton = "Select this option to have coffee.",
                PageImage = "CoffeeShop.png"
            });

            taskModels.Add(new SocialInclusionTasks()
            {
                PageId = Identifiers.CreatedActivity,
                PageTitle = Identifiers.GetDescription(Identifiers.CreatedActivity),
                PageDescription = "This activity is one of your own choosing.",
                PageButton = "Select this option to practice your activity.",
                PageImage = "CreateOwn.png"
            });

            tapGestureRecognizer = new TapGestureRecognizer();
            tapGestureRecognizer.Tapped += TapGestureRecognizer_TappedAsync;

            foreach (SocialInclusionTasks item in taskModels)
            {
                cardViewTemplate = new CardViewTemplate(item);
                cardViewTemplate.GestureRecognizers.Add(tapGestureRecognizer);

                startPageStackContent.Children.Add(cardViewTemplate);
            }

            // Remove this toolbar item if on iOS, is unnecessary
            if (Device.RuntimePlatform == Device.iOS)
            {
                ToolbarItems.Remove(settingsItem);
            }
        }

        /// <summary>
        /// Taps the gesture recognizer tapped async.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">E.</param>
        async void TapGestureRecognizer_TappedAsync(object sender, EventArgs e)
        {
            var getCardTapped = sender as CardViewTemplate;

            ContentPage view = null;

            if (getCardTapped != null)
            {
                switch (getCardTapped.PageId)
                {
                    case Identifiers.CreatedActivity:
                        view = new TaskCustomize();

                        break;

                    case Identifiers.DogWalking:
                        view = new TaskDogWalking();

                        break;

                    case Identifiers.EnjoyExercise:
                        view = new TaskExercise();

                        break;

                    case Identifiers.FoodShopping:
                        view = new TaskFoodShopping();

                        break;

                    case Identifiers.GoForWalk:
                        view = new TaskTakeWalk();

                        break;

                    case Identifiers.ListenMusic:
                        view = new TaskMusic();

                        break;

                    case Identifiers.Movies:
                        view = new TaskMovies();

                        break;

                    case Identifiers.CoffeeShop:
                        view = new TaskCoffee();

                        break;

                    case Identifiers.SportGames:
                        view = new TaskSports();

                        break;

                    default:
                        view = new ContentPage();

                        break;
                }

                App.RefreshServer = false;

                view.Disappearing += (sender2, e2) =>
                {
                    if (App.RefreshServer)
                    {
                        Handle_Sync_ClickedAsync(sender2, e2);
                    }

                    App.RefreshServer = false;
                };

                await Navigation.PushAsync(view, true);
            }
        }

        /// <summary>
        /// Handles the clicked.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">E.</param>
        async void Handle_Clicked(object sender, System.EventArgs e)
        {
            await Navigation.PushModalAsync(new TaskHelp());
        }

        /// <summary>
        /// Handles the settings clicked.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">E.</param>
        void Handle_Settings_Clicked(object sender, System.EventArgs e)
        {
            DependencyService.Get<InterfaceAdministrator>().AccessSettings();
        }

        /// <summary>
        /// Handles the sync clicked async.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">E.</param>
        async void Handle_Sync_ClickedAsync(object sender, System.EventArgs e)
        {
            Debug.WriteLineIf(App.Debugging, "Update_Clicked()");

            if (!CrossConnectivity.Current.IsConnected)
            {
                return;
            }

            CancellationTokenSource cancelSrc = new CancellationTokenSource();
            ProgressDialogConfig config = new ProgressDialogConfig()
                .SetTitle("Syncing with server")
                .SetIsDeterministic(false)
                .SetMaskType(MaskType.Black)
                .SetCancel(onCancel: cancelSrc.Cancel);

            using (IProgressDialog progress = UserDialogs.Instance.Progress(config))
            {
                try
                {
                    ListFolderResult serverFiles = await DropboxServer.CountIndividualFiles();
                    List<StorageModel> currentData = await App.Database.GetDataAsync();

                    if (serverFiles == null || currentData == null || cancelSrc.IsCancellationRequested)
                    {
                        // Nothing.. just move on
                    }
                    else if (currentData.Count == serverFiles.Entries.Count)
                    {
                        // Same.. no worries
                    }
                    else if (currentData.Count > serverFiles.Entries.Count)
                    {
                        List<int> localIds = currentData.Select(l => l.ID).ToList();

                        List<string> remoteIdsStr = serverFiles.Entries.Select(r => r.Name).ToList();
                        remoteIdsStr = remoteIdsStr.Select(r => r.Split('_')[1]).ToList();
                        remoteIdsStr = remoteIdsStr.Select(r => r.Split('.')[0]).ToList();

                        List<int> remoteIds = remoteIdsStr.Select(r => int.Parse(r)).ToList();

                        var missing = localIds.Except(remoteIds);

                        for (int count = 0; count < missing.Count() && !cancelSrc.IsCancellationRequested; count++)
                        {
                            if (cancelSrc.IsCancellationRequested)
                            {
                                continue;
                            }
                            else
                            {
                                await Task.Delay(App.DropboxDeltaTimeout);
                            }

                            progress.Title = "Uploading File " + (count + 1) + " of " + missing.Count().ToString();

                            var mStorageModel = currentData.Single(m => m.ID == missing.ElementAt(count));

                            await DropboxServer.UploadFile(new System.IO.MemoryStream(Encoding.UTF8.GetBytes(mStorageModel.CSV)), mStorageModel.ID);
                        }
                    }
                }
                catch (Exception exc)
                {
                    Debug.WriteLineIf(App.Debugging, exc.ToString());
                }

                try
                {
                    ListFolderResult serverFiles = await DropboxServer.CountFeedbackFiles();
                    List<SocialValidityModel> currentData = await App.Database.GetSocialValidity();

                    if (serverFiles == null || currentData == null || cancelSrc.IsCancellationRequested)
                    {
                        // Nothing.. just move on
                    }
                    else if (currentData.Count == serverFiles.Entries.Count)
                    {
                        // Same.. no worries
                    }
                    else if (currentData.Count > serverFiles.Entries.Count)
                    {
                        List<int> localIds = currentData.Select(l => l.ID).ToList();

                        List<string> remoteIdsStr = serverFiles.Entries.Select(r => r.Name).ToList();
                        remoteIdsStr = remoteIdsStr.Select(r => r.Split('_')[1]).ToList();
                        remoteIdsStr = remoteIdsStr.Select(r => r.Split('.')[0]).ToList();

                        List<int> remoteIds = remoteIdsStr.Select(r => int.Parse(r)).ToList();

                        var missing = localIds.Except(remoteIds);

                        for (int count = 0; count < missing.Count() && !cancelSrc.IsCancellationRequested; count++)
                        {
                            if (cancelSrc.IsCancellationRequested)
                            {
                                continue;
                            }
                            else
                            {
                                await Task.Delay(App.DropboxDeltaTimeout);
                            }

                            progress.Title = "Uploading File " + (count + 1) + " of " + missing.Count().ToString();

                            var mStorageModel = currentData.Single(m => m.ID == missing.ElementAt(count));

                            await DropboxServer.UploadImage(new MemoryStream(Convert.FromBase64String(mStorageModel.Base64)), mStorageModel.ID);
                        }
                    }
                }
                catch (Exception exc)
                {
                    Debug.WriteLineIf(App.Debugging, exc.ToString());
                }
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
