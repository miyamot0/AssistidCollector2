//----------------------------------------------------------------------------------------------
// <copyright file="ImplementationAdministrator.cs" 
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
using AssistidCollector2.Interfaces;
using Android.App.Admin;
using Android.App;
using Android.Content;
using AssistidCollector2.Droid.Base;
using AssistidCollector2.Droid.Implementations;

[assembly: Xamarin.Forms.Dependency(typeof(ImplementationAdministrator))]
namespace AssistidCollector2.Droid.Implementations
{
    public class ImplementationAdministrator : InterfaceAdministrator
    {
        /// <summary>
        /// Pull up settings
        /// </summary>
        public void AccessSettings()
        {
            Plugin.CurrentActivity.CrossCurrentActivity.Current.Activity.StartActivityForResult(new Intent(Android.Provider.Settings.ActionSettings), 0);
        }

        /// <summary>
        /// Query lock status
        /// </summary>
        /// <returns></returns>
        public bool IsAdmin()
        {
            DevicePolicyManager devicePolicyManager = (DevicePolicyManager)Application.Context.GetSystemService(Context.DevicePolicyService);
            ComponentName mDeviceAdminRcvr = new ComponentName(Application.Context, Java.Lang.Class.FromType(typeof(DeviceAdminReceiverClass)).Name);

            return devicePolicyManager.IsAdminActive(mDeviceAdminRcvr);
        }

        /// <summary>
        /// Request lock status
        /// </summary>
        /// <param name="status"></param>
        public void RequestAdmin(bool status)
        {
            DevicePolicyManager devicePolicyManager = (DevicePolicyManager)Application.Context.GetSystemService(Context.DevicePolicyService);
            ComponentName mDeviceAdminRcvr = new ComponentName(Application.Context, Java.Lang.Class.FromType(typeof(DeviceAdminReceiverClass)).Name);

            if (devicePolicyManager.IsAdminActive(mDeviceAdminRcvr))
            {
                devicePolicyManager.SetLockTaskPackages(mDeviceAdminRcvr, new String[] { Application.Context.PackageName });

                if (devicePolicyManager.IsLockTaskPermitted(Application.Context.PackageName))
                {
                    if (status)
                    {
                        Plugin.CurrentActivity.CrossCurrentActivity.Current.Activity.StartLockTask();
                    }
                    else
                    {
                        Plugin.CurrentActivity.CrossCurrentActivity.Current.Activity.StopLockTask();
                    }
                }
            }
        }
    }
}
