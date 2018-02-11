using System;

using Xamarin.Forms;
using AssistidCollector2.Enums;
using AssistidCollector2.Models;
using AssistidCollector1.Views;
using System.IO;

namespace AssistidCollector2.Views
{
    public class CardViewStepTemplate : ContentView
    {
        public int PageId;
        public int imgDimension = 250;

        public CardViewStepTemplate(SocialInclusionStep socialTask)
        {
            PageId = socialTask.PageType;

            Grid grid = new Grid
            {
                Padding = new Thickness(0, 1, 1, 1),
                RowSpacing = 1,
                ColumnSpacing = 1,
                BackgroundColor = Color.FromHex("E3E3E3"),
                VerticalOptions = LayoutOptions.FillAndExpand,
                RowDefinitions = {
                    new RowDefinition { Height = new GridLength (imgDimension, GridUnitType.Auto) },
                    //new RowDefinition { Height = new GridLength (2, GridUnitType.Absolute) }
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
                //Source = socialTask.ImgPath,
                HeightRequest = imgDimension,
                WidthRequest = imgDimension,
                Aspect = Aspect.AspectFill
            }, 1, 2, 0, 1);
            grid.Children.Add(new CardDetailsView(socialTask.Title, socialTask.Description, LayoutOptions.StartAndExpand), 2, 3, 0, 1);
            //grid.Children.Add(new CardButtonView(""), 1, 3, 1, 2);

            Padding = new Thickness(0, 0, 0, 20);

            Content = grid;

        }
    }
}

