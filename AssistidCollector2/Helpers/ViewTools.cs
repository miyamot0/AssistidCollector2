
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
using AssistidCollector2.Storage;
using AssistidCollector2.Views;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace AssistidCollector2.Helpers
{
    public static class ViewTools
    {
        /// <summary>
        /// Commas the separated value.
        /// </summary>
        /// <returns>The separated value.</returns>
        /// <param name="header">Header.</param>
        /// <param name="intervention">Intervention.</param>
        /// <param name="rating">Rating.</param>
        /// <param name="layout">Layout.</param>
        /// <param name="startTime">Start time.</param>
        /// <param name="timeDifference">Time difference.</param>
        public static string CommaSeparatedValue(string header, string intervention, int rating,
                                                 StackLayout layout, DateTime startTime, TimeSpan timeDifference)
        {
            string returnString = header + Environment.NewLine;
            returnString += intervention + Environment.NewLine;
            returnString += "Rating," + rating.ToString() + Environment.NewLine;

            CardViewStepTemplate holder;

            foreach (var child in layout.Children)
            {
                holder = child as CardViewStepTemplate;

                if (holder != null)
                {
                    returnString += holder.itemDescription + "," + holder.WasActivated + Environment.NewLine;
                }
            }

            returnString += "Date," + startTime.Date.ToString() + Environment.NewLine;
            returnString += "Start," + startTime.TimeOfDay.ToString() + Environment.NewLine;
            returnString += "Seconds," + DateTime.Now.Subtract(startTime).TotalSeconds.ToString() + Environment.NewLine;

            return returnString;
        }

        /// <summary>
        /// Handles the poll data async.
        /// </summary>
        /// <param name="taskModels">Task models.</param>
        /// <param name="customPageStackContent">Custom page stack content.</param>
        /// <param name="PageType">Page type.</param>
        public static async void HandlePollDataAsync(List<SocialInclusionStep> taskModels, StackLayout customPageStackContent, int PageType)
        {
            try
            {
                var mStoredSteps = await App.Database.GetStepsAsync();

                //Debug.WriteLineIf(App.Debugging, "Save_StepAsync: PageType >>> " + PageType.ToString());

                if (mStoredSteps != null)
                {
                    var mSpecificSteps = mStoredSteps.Where(model => model.TaskType == PageType).ToList();

                    if (mSpecificSteps == null)
                    {
                        return;
                    }

                    foreach (SocialStepModel model in mSpecificSteps)
                    {
                        //Debug.WriteLineIf(App.Debugging, "Save_StepAsync: StepTitle >>> " + model.Title);
                        //Debug.WriteLineIf(App.Debugging, "Save_StepAsync: StepDescription >>> " + model.Description);
                        //Debug.WriteLineIf(App.Debugging, "Save_StepAsync: StepID >>> " + model.TaskType.ToString());

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
                        CardViewStepTemplate cardCheckTemplate = new CardViewStepTemplate(item);
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
        /// Handles the step added async.
        /// </summary>
        /// <param name="taskModels">Task models.</param>
        /// <param name="customPageStackContent">Custom page stack content.</param>
        /// <param name="PageType">Page type.</param>
        public static async void HandleStepAddedAsync(List<SocialInclusionStep> taskModels, StackLayout customPageStackContent, int PageType)
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

                CardViewStepTemplate cardCheckTemplate = new CardViewStepTemplate(task);
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

                    //Debug.WriteLineIf(App.Debugging, "Result: " + result.ToString());
                }
                catch (Exception e)
                {
                    Debug.WriteLineIf(App.Debugging, "Exceptoin: " + e.ToString());
                }
            }            
        }

        /// <summary>
        /// Handles the step removal async.
        /// </summary>
        /// <param name="taskModels">Task models.</param>
        /// <param name="customPageStackContent">Custom page stack content.</param>
        /// <param name="pollForDataAsync">Poll for data async.</param>
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
