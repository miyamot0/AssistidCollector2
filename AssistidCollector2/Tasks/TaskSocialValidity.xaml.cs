using System;
using System.Diagnostics;
using System.IO;
using Acr.UserDialogs;
using Plugin.Media;
using Xamarin.Forms;
using AssistidCollector2.Interfaces;
using AssistidCollector2.Storage;

namespace AssistidCollector2.Tasks
{
    public partial class TaskSocialValidity : ContentPage
    {
        public string ImgBytes = "";
        public int AppRating = -1;
        public int AppRating2 = -1;
        public int AppRating3 = -1;
        public int TaskType = -1;

        public TaskSocialValidity(int taskType)
        {
            InitializeComponent();

            TaskType = taskType;

            Title = "Take a Picture to Remember";

            NavigationPage.SetHasNavigationBar(this, true);
            NavigationPage.SetHasBackButton(this, true);
            NavigationPage.SetBackButtonTitle(this, "Back");

            TapGestureRecognizer tgr = new TapGestureRecognizer();
            tgr.Tapped += Tgr_Tapped;

            socialValidityNegative.GestureRecognizers.Add(tgr);
            socialValidityMiddle.GestureRecognizers.Add(tgr);
            socialValidityPositive.GestureRecognizers.Add(tgr);

            socialValidityNegative2.GestureRecognizers.Add(tgr);
            socialValidityMiddle2.GestureRecognizers.Add(tgr);
            socialValidityPositive2.GestureRecognizers.Add(tgr);

            socialValidityNegative3.GestureRecognizers.Add(tgr);
            socialValidityMiddle3.GestureRecognizers.Add(tgr);
            socialValidityPositive3.GestureRecognizers.Add(tgr);
        }

        /// <summary>
        /// Gets the base64.
        /// </summary>
        /// <returns>The base64.</returns>
        public string GetBase64()
        {
            return ImgBytes;
        }

        /// <summary>
        /// Takes the picture async.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">E.</param>
        async void Take_PictureAsync(object sender, System.EventArgs e)
        {
            try
            {
                App.isTakingPictures = true;

                if (CrossMedia.Current.IsCameraAvailable && CrossMedia.Current.IsTakePhotoSupported)
                {
                    Plugin.Media.Abstractions.StoreCameraMediaOptions mediaOptions = new Plugin.Media.Abstractions.StoreCameraMediaOptions
                    {
                        Name = $"{DateTime.UtcNow}.jpg",
                        PhotoSize = Plugin.Media.Abstractions.PhotoSize.Medium,
                        CompressionQuality = 80,
                        SaveToAlbum = true,
                    };

                    using (Plugin.Media.Abstractions.MediaFile file = await CrossMedia.Current.TakePhotoAsync(mediaOptions))
                    {
                        if (file == null || file.Path == null || file.Path == "")
                        {
                            Debug.WriteLineIf(App.Debugging, "Failed to capture");
                        }
                        else
                        {
                            //Debug.WriteLineIf(App.Debugging, "Local: " + file.Path);

                            var imageStream = file.GetStream();
                            byte[] filebytearray = new byte[imageStream.Length];
                            imageStream.Read(filebytearray, 0, (int)imageStream.Length);

                            ImgBytes = Convert.ToBase64String(filebytearray);

                            socialValidityPhoto.Source = Xamarin.Forms.ImageSource.FromStream(() => new MemoryStream(Convert.FromBase64String(
                                ImgBytes)));
                        }
                    }

                    App.isTakingPictures = false;
                }
                else
                {
                    await UserDialogs.Instance.AlertAsync("Camera Error", "Please make sure you grant permissions to the camera");

                    App.isTakingPictures = false;
                }
            }
            catch (Exception e2)
            {
                App.isTakingPictures = false;

                Debug.WriteLineIf(App.Debugging, e2.ToString());
            }
        }

        /// <summary>
        /// Handles the clicked async.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">E.</param>
        async void Handle_ClickedAsync(object sender, System.EventArgs e)
        {
            if (AppRating == -1 || AppRating2 == -1 || AppRating3 == -1)
            {
                await UserDialogs.Instance.AlertAsync("Please rate the app");

                DependencyService.Get<InterfaceTextToSpeech>().Speak("Pick one of the faces");
            }
            else if (ImgBytes.Length == 0)
            {
                await UserDialogs.Instance.AlertAsync("Please take a picture");

                DependencyService.Get<InterfaceTextToSpeech>().Speak("Take a picture to show how you feel");
            }
            else
            {
                int result = await App.Database.SaveItemAsync(new SocialValidityModel()
                {
                    Rating = AppRating,
                    Rating2 = AppRating2,
                    Rating3 = AppRating3,
                    Base64 = ImgBytes,
                    TaskNumber = TaskType
                });

                await Navigation.PopAsync();
            }
        }

        /// <summary>
        /// Tgrs the tapped.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">E.</param>
        void Tgr_Tapped(object sender, EventArgs e)
        {
            var img = sender as Image;

            if (img != null)
            {
                if (img.Id == socialValidityNegative.Id)
                {
                    ResetColors();

                    socialValidityNegative.BackgroundColor = Color.LightGreen;

                    AppRating = 1;
                }
                else if (img.Id == socialValidityMiddle.Id)
                {
                    ResetColors();

                    socialValidityMiddle.BackgroundColor = Color.LightGreen;

                    AppRating = 2;
                }
                else if (img.Id == socialValidityPositive.Id)
                {
                    ResetColors();

                    socialValidityPositive.BackgroundColor = Color.LightGreen;

                    AppRating = 3;
                }

                //

                else if (img.Id == socialValidityNegative2.Id)
                {
                    ResetColors2();

                    socialValidityNegative2.BackgroundColor = Color.LightGreen;

                    AppRating2 = 1;
                }
                else if (img.Id == socialValidityMiddle2.Id)
                {
                    ResetColors2();

                    socialValidityMiddle2.BackgroundColor = Color.LightGreen;

                    AppRating2 = 2;
                }
                else if (img.Id == socialValidityPositive2.Id)
                {
                    ResetColors2();

                    socialValidityPositive2.BackgroundColor = Color.LightGreen;

                    AppRating2 = 3;
                }

                //

                else if (img.Id == socialValidityNegative3.Id)
                {
                    ResetColors3();

                    socialValidityNegative3.BackgroundColor = Color.LightGreen;

                    AppRating3 = 1;
                }
                else if (img.Id == socialValidityMiddle3.Id)
                {
                    ResetColors3();

                    socialValidityMiddle3.BackgroundColor = Color.LightGreen;

                    AppRating3 = 2;
                }
                else if (img.Id == socialValidityPositive3.Id)
                {
                    ResetColors3();

                    socialValidityPositive3.BackgroundColor = Color.LightGreen;

                    AppRating3 = 3;
                }
            }
        }

        /// <summary>
        /// Resets the colors.
        /// </summary>
        void ResetColors()
        {
            socialValidityNegative.BackgroundColor = Color.Transparent;
            socialValidityMiddle.BackgroundColor = Color.Transparent;
            socialValidityPositive.BackgroundColor = Color.Transparent;
        }

        /// <summary>
        /// Resets the colors.
        /// </summary>
        void ResetColors2()
        {
            socialValidityNegative2.BackgroundColor = Color.Transparent;
            socialValidityMiddle2.BackgroundColor = Color.Transparent;
            socialValidityPositive2.BackgroundColor = Color.Transparent;
        }

        /// <summary>
        /// Resets the colors.
        /// </summary>
        void ResetColors3()
        {
            socialValidityNegative3.BackgroundColor = Color.Transparent;
            socialValidityMiddle3.BackgroundColor = Color.Transparent;
            socialValidityPositive3.BackgroundColor = Color.Transparent;
        }
    }
}
