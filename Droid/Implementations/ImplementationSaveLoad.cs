//----------------------------------------------------------------------------------------------
// <copyright file="ImplementationSaveLoad.cs" 
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

using System;
using System.IO;
using Android.Content;
using AssistidCollector2.Droid.Implementations;
using AssistidCollector2.Interfaces;
using Uri = Android.Net.Uri;
using Xamarin.Forms;

[assembly: Dependency(typeof(ImplementationSaveLoad))]
namespace AssistidCollector2.Droid.Implementations
{
    public class ImplementationSaveLoad : InterfaceSaveLoad
    {
        public string GetLocalFilePath(string filename)
        {
            string path = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            return Path.Combine(path, filename);
        }

        public bool FileExists(string filename)
        {
            return File.Exists(CreatePathToFile(filename));
        }

        public string LoadFile(string filename)
        {
            var path = CreatePathToFile(filename);

            using (StreamReader sr = File.OpenText(path))
            {
                return sr.ReadToEnd();
            }
        }

        public void SaveFile(string filename, string text)
        {
            var path = CreatePathToFile(filename);
            using (StreamWriter sw = File.CreateText(path))
            {
                sw.Write(text);
            }
        }

        string CreatePathToFile(string filename)
        {
            var docsPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            return Path.Combine(docsPath, filename);
        }

        [Obsolete]
        public void InstallLocationFile(string filename)
        {
            var path = CreatePathToFile(filename);

            Intent promptInstall = new Intent(Intent.ActionView);
            promptInstall.SetDataAndType(Uri.Parse(path), "application/vnd.android.package-archive");
            promptInstall.SetFlags(ActivityFlags.GrantReadUriPermission);
            promptInstall.SetFlags(ActivityFlags.NewTask);
            promptInstall.SetFlags(ActivityFlags.ClearTop);

            //var intent = new Intent(Intent.ActionView);
            //global::Android.Net.Uri pdfFile = global::Android.Net.Uri.FromFile(new Java.IO.File(path));
            //intent.SetDataAndType(pdfFile, "application/pdf");
            //intent.SetFlags(ActivityFlags.GrantReadUriPermission);
            //intent.SetFlags(ActivityFlags.NewTask);
            //intent.SetFlags(ActivityFlags.ClearWhenTaskReset);

            //Context context = Android.App.Application.Context;

            Forms.Context.StartActivity(promptInstall);
        }
    }
}
