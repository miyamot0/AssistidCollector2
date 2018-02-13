//----------------------------------------------------------------------------------------------
// <copyright file="InterfaceSaveLoad.cs" 
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


namespace AssistidCollector2.Interfaces
{
    public interface InterfaceSaveLoad
    {
        /// <summary>
        /// Build local path
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        string GetLocalFilePath(string filename);

        /// <summary>
        /// Save text file to personal folder
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="text"></param>
        void SaveFile(string filename, string text);

        /// <summary>
        /// Load text file from personal folder
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        string LoadFile(string filename);

        /// <summary>
        /// Check if file exists in personal folder
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        bool FileExists(string filename);

        /// <summary>
        /// Install (android only)
        /// </summary>
        /// <param name="filename"></param>
        void InstallLocationFile(string filename);
    }
}
