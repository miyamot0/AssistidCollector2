//----------------------------------------------------------------------------------------------
// <copyright file="ApplicationDatabase.cs" 
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
using System.Linq;
using System.Threading.Tasks;
using SQLite;

namespace AssistidCollector2.Storage
{
    /// <summary>
    /// Application database
    /// </summary>
    public class ApplicationDatabase
    {
        readonly SQLiteAsyncConnection database;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="dbPath"></param>
        public ApplicationDatabase(string dbPath)
        {
            database = new SQLiteAsyncConnection(dbPath);
            //database.CreateTableAsync<ManifestModel>().Wait();
            database.CreateTableAsync<StorageModel>().Wait();
        }

        public void Init() { }

        /// <summary>
        /// Get Data from db
        /// </summary>
        /// <returns></returns>
        public Task<List<StorageModel>> GetDataAsync()
        {
            return database.Table<StorageModel>().ToListAsync();
        }

        /// <summary>
        /// Get largest ID
        /// </summary>
        /// <returns></returns>
        public int GetLargestID()
        {
            try
            {
                return GetDataAsync().Result.Aggregate((i1, i2) => i1.ID > i2.ID ? i1 : i2).ID;
            }
            catch
            {
                return 0;
            }
        }

        /// <summary>
        /// Save item
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public Task<int> SaveItemAsync(StorageModel item)
        {
            return database.InsertAsync(item);
        }
    }
}
