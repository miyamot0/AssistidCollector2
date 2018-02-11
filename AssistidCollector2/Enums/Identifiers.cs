//----------------------------------------------------------------------------------------------
// <copyright file="Identifiers.cs" 
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

namespace AssistidCollector2.Enums
{
    public static class Identifiers
    {
        public const int Start = 0;
        public const int Movies = 1;
        public const int SportGames = 2;
        public const int FoodShopping = 3;
        public const int DogWalking = 4;
        public const int ListenMusic = 5;
        public const int EnjoyExercise = 6;
        public const int GoForWalk = 7;
        public const int CreatedActivity = 8;
        public const int CoffeeShop = 9;

        /// <summary>
        /// Gets the description.
        /// </summary>
        /// <returns>The description.</returns>
        /// <param name="id">Identifier.</param>
        public static string GetDescription(int id)
        {
            switch(id)
            {
                case 0:
                    return "Social Inclusion App";

                case 1:
                    return "Take a trip to the movies";

                case 2:
                    return "Play a game or a sport";

                case 3:
                    return "Go shopping for food";

                case 4:
                    return "Take a dog for a walk";

                case 5:
                    return "Listen to music";

                case 6:
                    return "Exercise for fun";

                case 7:
                    return "Take a short walk";

                case 8:
                    return "Create your own activity";

                case 9:
                    return "Visit a coffee shop";

                default:
                    return "Error";
            }
        }
    }
}
