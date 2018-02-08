//----------------------------------------------------------------------------------------------
// <copyright file="CardStatusView.cs" 
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
using Xamarin.Forms;

namespace AssistidCollector2.Views
{
    /// <summary>
    /// Card status view
    /// </summary>
    public class CardStatusView : ContentView
    {
        public CardStatusView()
        {
            Content = new BoxView
            {
                VerticalOptions = LayoutOptions.Fill,
                HorizontalOptions = LayoutOptions.Fill,
                BackgroundColor = GetStatusColor()
            };
        }

        /// <summary>
        /// Get color from identifier
        /// </summary>
        /// <param name="strategyType"></param>
        /// <returns></returns>
        public static Color GetStatusColor()
        {
            Color mColor;

            /*
            switch (strategyType)
            {
                case Identifiers.Strategies.Specific:
                    mColor = Color.FromHex("C5C5C5");
                    break;

                case Identifiers.Strategies.Relaxation:
                    mColor = Color.FromHex("00A2D3");
                    break;

                case Identifiers.Strategies.SleepHygiene:
                    mColor = Color.FromHex("E74C3C");
                    break;

                default:
                    mColor = Color.FromHex("E3E3E3");
                    break;
            }
            */

            mColor = Color.FromHex("C5C5C5");

            return mColor;
        }
    }
}
