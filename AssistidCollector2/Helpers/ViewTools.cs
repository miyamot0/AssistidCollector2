
//----------------------------------------------------------------------------------------------
// <copyright file="ViewTools.cs" 
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
using System.Diagnostics;
using System.Linq;
using System.Threading;
using Acr.UserDialogs;
using AssistidCollector2.Models;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace AssistidCollector2.Helpers
{
    public static class ViewTools
    {
        /*
        public static string CommaSeparatedValue(string header, string intervention, StackLayout layout, List<SleepTasks> taskModels,
            DateTime startTime, TimeSpan timeDifference)
        {
            string returnString = header + Environment.NewLine;
            returnString = intervention + Environment.NewLine;

            CardCheckTemplate holder;

            bool isChecked;
            string currentTitle;
            int counter = 0;

            foreach (var child in layout.Children)
            {
                holder = child as CardCheckTemplate;

                if (holder != null)
                {
                    isChecked = ViewTools.GetSwitchValue(holder.grid);
                    currentTitle = taskModels[counter].PageTitle;

                    returnString += currentTitle + ",";
                    returnString += (isChecked) ? "True" : "False";
                    returnString += Environment.NewLine;

                    counter++;
                }
            }

            returnString += "Date," + startTime.Date.ToString() + Environment.NewLine;
            returnString += "Start," + startTime.TimeOfDay.ToString() + Environment.NewLine;
            returnString += "Seconds," + timeDifference.TotalSeconds.ToString() + Environment.NewLine;

            return returnString;
        }
        */

        public static async void HandleStepRemovalAsync(List<SocialInclusionStep> taskModels, StackLayout customPageStackContent, Action pollForDataAsync)
        {
            string[] stepsInList = taskModels.Select(m => m.Title).ToArray();

            if (stepsInList == null || stepsInList.Length == 0)
            {
                return;
            }

            string destroyString = "OK";

            CancellationTokenSource cancelSrc = new CancellationTokenSource();

            string result = await UserDialogs.Instance.ActionSheetAsync("Pick Item to Edit", "Close", destroyString, cancelSrc.Token, stepsInList);

            //Debug.WriteLineIf(App.Debugging, result);

            if (result != destroyString)
            {
                bool promptDelete = await UserDialogs.Instance.ConfirmAsync("Delete step?", "Confirm", destroyString, "Cancel", cancelSrc.Token);

                int indexWithinList = stepsInList.IndexOf(result);

                //Debug.WriteLineIf(App.Debugging, "indexWithinList: " + indexWithinList.ToString());

                if (indexWithinList != -1)
                {
                    var item = taskModels.Where(m => m.ID == taskModels.ElementAt(indexWithinList).ID).First();

                    taskModels.Clear();

                    customPageStackContent.Children.Clear();

                    await App.Database.DeleteStepAsync(item.ID);

                    pollForDataAsync();
                }
            }
        }
    }
}
