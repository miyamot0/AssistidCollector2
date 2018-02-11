using System;

using Xamarin.Forms;
using AssistidCollector2.Enums;
using AssistidCollector2.Models;
using AssistidCollector1.Views;
using System.IO;
using System.Diagnostics;

namespace AssistidCollector2.Views
{
    public class CardViewStepTemplate : ContentView
    {
        public int PageId;
        public int imgDimension = 250;

        string itemDescription;

        public CardViewStepTemplate(SocialInclusionStep socialTask)
        {
            PageId = socialTask.PageType;

            itemDescription = socialTask.Description;

            Grid grid = new Grid
            {
                Padding = new Thickness(0, 1, 1, 1),
                RowSpacing = 1,
                ColumnSpacing = 1,
                BackgroundColor = Color.FromHex("E3E3E3"),
                VerticalOptions = LayoutOptions.FillAndExpand,
                RowDefinitions = {
                    new RowDefinition { Height = new GridLength (imgDimension, GridUnitType.Auto) },
                },
                ColumnDefinitions = {
                    new ColumnDefinition { Width = new GridLength (5, GridUnitType.Absolute) },
                    new ColumnDefinition { Width = new GridLength (imgDimension, GridUnitType.Absolute) },
                    new ColumnDefinition { Width = new GridLength (1, GridUnitType.Star) }
                }
            };

            grid.Children.Add(new CardStatusView(), 0, 1, 0, 1);
            grid.Children.Add(new Image()
            {
                Source = Xamarin.Forms.ImageSource.FromStream(() => new MemoryStream(Convert.FromBase64String(socialTask.ImgBytes))),
                HeightRequest = imgDimension,
                WidthRequest = imgDimension,
                Aspect = Aspect.AspectFill
            }, 1, 2, 0, 1);
            grid.Children.Add(new CardDetailsView(socialTask.Title, socialTask.Description, LayoutOptions.StartAndExpand), 2, 3, 0, 1);

            Padding = new Thickness(0, 0, 0, 20);

            Content = grid;

            TapGestureRecognizer tgr = new TapGestureRecognizer();
            tgr.Tapped += (s, e) =>
            {

                Debug.WriteLineIf(App.Debugging, "Description to output: " + itemDescription);
                //Frame tappedFrame = s as Frame;

                //SelectedRating = int.Parse(tappedFrame.AutomationId);

                //await ColorFramesAsync(RatingOptions[SelectedRating].Color);
            };

            GestureRecognizers.Add(tgr);

        }
    }
}

