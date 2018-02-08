
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
                PageId = Identifiers.Pages.Movies,
                PageTitle = "Getting together for the movies",
                PageDescription = "This lesson focuses on learning how to interact in the community at the movies.",
                PageButton = "Select this option to practice at the movies.",
                PageImage = "placeholder.png"
            });

            taskModels.Add(new SocialInclusionTasks()
            {
                PageId = Identifiers.Pages.SportGames,
                PageTitle = "Learn a sport or a game",
                PageDescription = "This program is dedicated to learning how to play a sport or a game with others.",
                PageButton = "Select this option to practice playing games.",
                PageImage = "placeholder.png"
            });

            taskModels.Add(new SocialInclusionTasks()
            {
                PageId = Identifiers.Pages.FoodShopping,
                PageTitle = "Go shopping for food",
                PageDescription = "The steps in this section focus on the skills needed to go shopping in the community.",
                PageButton = "Select this option to practice shopping.",
                PageImage = "placeholder.png"
            });

            taskModels.Add(new SocialInclusionTasks()
            {
                PageId = Identifiers.Pages.DogWalking,
                PageTitle = "Take a dog for a walk",
                PageDescription = "These activities focus on take a dog for a walk while out in the community",
                PageButton = "Select this option to practice walking a dog.",
                PageImage = "placeholder.png"
            });

            taskModels.Add(new SocialInclusionTasks()
            {
                PageId = Identifiers.Pages.ListenMusic,
                PageTitle = "Play or listen to music",
                PageDescription = "This program is dedicated to playing or listening to music with others.",
                PageButton = "Select this option to listen to music.",
                PageImage = "placeholder.png"
            });

            taskModels.Add(new SocialInclusionTasks()
            {
                PageId = Identifiers.Pages.EnjoyExercise,
                PageTitle = "Enjoy some exercise",
                PageDescription = "This routine focuses on exercising with other in the community.",
                PageButton = "Select this option to practice exercising.",
                PageImage = "placeholder.png"
            });

            taskModels.Add(new SocialInclusionTasks()
            {
                PageId = Identifiers.Pages.GoForWalk,
                PageTitle = "Take a walk in the park",
                PageDescription = "These activities are dedicated to taking a trip in the park.",
                PageButton = "Select this option to take a walk.",
                PageImage = "placeholder.png"
            });


            taskModels.Add(new SocialInclusionTasks()
            {
                PageId = Identifiers.Pages.CreatedActivity,
                PageTitle = "Create your own activity",
                PageDescription = "This activity is one of your own choosing.",
                PageButton = "Select this option to practice your activity.",
                PageImage = "placeholder.png"
            });

            tapGestureRecognizer = new TapGestureRecognizer();
            tapGestureRecognizer.Tapped += TapGestureRecognizer_Tapped;

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

        void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {

        }

        void Handle_Clicked(object sender, System.EventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}
