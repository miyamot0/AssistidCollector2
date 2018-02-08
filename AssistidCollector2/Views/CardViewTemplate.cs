//----------------------------------------------------------------------------------------------
// <copyright file="CardViewTemplate.cs" 
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
//
// =============================================================================================
//
// This class is derived from existing methods and styles from Adam Wolf at 
// https://github.com/awolf/Xamarin-Forms-InAnger. The base methods have been
// adjusted to improve the clarity, simplicity, and recognizability of
// information presented to users. The license for this work is indicated below:
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.
//
//----------------------------------------------------------------------------------------------

using System;
using AssistidCollector2.Enums;
using Xamarin.Forms;
using AssistidCollector2.Models;
using AssistidCollector1.Views;

namespace AssistidCollector2.Views
{
    public class CardViewTemplate : ContentView
    {
        private int imgDimension = 250;
        public Identifiers.Pages PageId;

        public CardViewTemplate(SocialInclusionTasks sleepTask)
        {
            PageId = sleepTask.PageId;

            Grid grid = new Grid
            {
                Padding = new Thickness(0, 1, 1, 1),
                RowSpacing = 1,
                ColumnSpacing = 1,
                BackgroundColor = Color.FromHex("E3E3E3"),
                VerticalOptions = LayoutOptions.FillAndExpand,
                RowDefinitions = {
                    new RowDefinition { Height = new GridLength (imgDimension, GridUnitType.Auto) },
                    new RowDefinition { Height = new GridLength (30, GridUnitType.Absolute) }
                },
                ColumnDefinitions = {
                    new ColumnDefinition { Width = new GridLength (4, GridUnitType.Absolute) },
                    new ColumnDefinition { Width = new GridLength (imgDimension, GridUnitType.Absolute) },
                    new ColumnDefinition { Width = new GridLength (1, GridUnitType.Star) },
                    new ColumnDefinition { Width = new GridLength (50, GridUnitType.Absolute) },
                    new ColumnDefinition { Width = new GridLength (50, GridUnitType.Absolute) }
                }
            };

            grid.Children.Add(new CardStatusView(), 0, 1, 0, 2);
            grid.Children.Add(new Image()
            {
                Source = sleepTask.PageImage,
                HeightRequest = imgDimension,
                WidthRequest = imgDimension,
                Aspect = Aspect.AspectFill
            }, 1, 2, 0, 1);
            grid.Children.Add(new CardDetailsView(sleepTask.PageTitle, sleepTask.PageDescription), 2, 5, 0, 1);
            grid.Children.Add(new CardButtonView(sleepTask.PageButton), 1, 5, 1, 2);

            Padding = new Thickness(0, 0, 0, 20);

            Content = grid;
        }
    }
}
