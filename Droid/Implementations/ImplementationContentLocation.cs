//----------------------------------------------------------------------------------------------
// <copyright file="ImplementationContentLocation.cs" 
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
// The AssistidCollector2 is a tool to assist clinicans and researchers in the treatment of various disorders.
// 
// Email: shawn(dot)gilroy(at)temple.edu
//
// </summary>
//----------------------------------------------------------------------------------------------

using AssistidCollector2.Droid.Implementations;
using AssistidCollector2.Interfaces;
using Xamarin.Forms;

[assembly: Dependency(typeof(ImplementationContentLocation))]
namespace AssistidCollector2.Droid.Implementations
{
    public class ImplementationContentLocation : InterfaceContentLocation
    {
        public string GetBaseLocation()
        {
            return "file:///android_asset/";
        }

        public string GetPersonalLocation()
        {
            return System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
        }
    }
}
