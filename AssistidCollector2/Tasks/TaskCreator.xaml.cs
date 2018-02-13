using System;
using System.Collections.Generic;

using Xamarin.Forms;
using System.Diagnostics;
using Plugin.Media;
using Acr.UserDialogs;
using System.IO;

namespace AssistidCollector2.Tasks
{
    public partial class TaskCreator : ContentPage
    {
        string _stepTitle = "";
        public string StepTitle
        {
            get
            {
                return _stepTitle;
            }
            set
            {
                _stepTitle = value;
            }
        }

        string _stepDescription = "";
        public string StepDescription
        {
            get
            {
                return _stepDescription;
            }
            set
            {
                _stepDescription = value;
            }
        }

        string _stepImgBytes = "";
        public string StepImgBytes
        {
            get
            {
                return _stepImgBytes;
            }
            set
            {
                _stepImgBytes = value;
            }
        }

        public TaskCreator()
        {
            InitializeComponent();

            NavigationPage.SetHasNavigationBar(this, true);
            NavigationPage.SetHasBackButton(this, true);
            NavigationPage.SetBackButtonTitle(this, "Back");
        }

        /// <summary>
        /// Takes the photo async.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">E.</param>
        async void Take_PhotoAsync(object sender, System.EventArgs e)
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
                        //SaveToAlbum = true,
                    };

                    using (Plugin.Media.Abstractions.MediaFile file = await CrossMedia.Current.TakePhotoAsync(mediaOptions))
                    {
                        if (file == null || file.Path == null || file.Path == "")
                        {
                            Debug.WriteLineIf(App.Debugging, "Failed to capture");
                        }
                        else
                        {
                            //Debug.WriteLineIf(App.Debugging, "Album: " + file.AlbumPath);
                            //Debug.WriteLineIf(App.Debugging, "Local: " + file.Path);

                            var imageStream = file.GetStream();
                            byte[] filebytearray = new byte[imageStream.Length];
                            imageStream.Read(filebytearray, 0, (int)imageStream.Length);

                            StepImgBytes = Convert.ToBase64String(filebytearray);

                            stepPhoto.Source = Xamarin.Forms.ImageSource.FromStream(() => new MemoryStream(Convert.FromBase64String(StepImgBytes)));

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
        /// Saves the step async.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">E.</param>
        async void Save_StepAsync(object sender, System.EventArgs e)
        {
            StepTitle = stepTitle.Text;
            StepDescription = stepDescription.Text;

            //Debug.WriteLineIf(App.Debugging, "Save_StepAsync: isTakingPics >>> " + App.isTakingPictures);
            //Debug.WriteLineIf(App.Debugging, "Save_StepAsync: StepTitle >>> " + StepTitle);
            //Debug.WriteLineIf(App.Debugging, "Save_StepAsync: StepDescription >>> " + StepDescription);

            if (StepTitle == "" || StepDescription == "" || StepImgBytes == "")
            {
                await UserDialogs.Instance.AlertAsync("Please complete all fields", "Incomplete Information");
            }
            else
            {
                App.isTakingPictures = false;

                App.temporaryStep = new Models.SocialInclusionStep()
                {
                    Title = StepTitle,
                    Description = StepDescription,
                    ImgBytes = StepImgBytes
                };

                await Navigation.PopModalAsync();
            }
        }
    }
}
