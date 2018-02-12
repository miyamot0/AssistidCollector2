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
        private static object collisionLock = new object();

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="dbPath"></param>
        public ApplicationDatabase(string dbPath)
        {
            database = new SQLiteAsyncConnection(dbPath);
            database.CreateTableAsync<SocialStepModel>().Wait();
            database.CreateTableAsync<StorageModel>().Wait();
            database.CreateTableAsync<SocialValidityModel>().Wait();
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
        /// Gets the steps async.
        /// </summary>
        /// <returns>The steps async.</returns>
        public Task<List<SocialStepModel>> GetStepsAsync()
        {
            return database.Table<SocialStepModel>().ToListAsync();
        }

        /// <summary>
        /// Gets the steps.
        /// </summary>
        /// <returns>The steps.</returns>
        /// <param name="taskType">Task type.</param>
        public Task<List<SocialStepModel>> GetSteps(int taskType)
        {
            lock (collisionLock)
            {

                return database.QueryAsync<SocialStepModel>("select * from SocialStepModel where TaskType = ?", taskType);
                //return database.Query<SocialStepModel>("select 'Price' as 'Money', 'Time' as 'Date' from Valuation where StockId = ?", stock.Id);

                /*
                var query = from steps in database.Table<SocialStepModel>()
                            where steps.PageType == taskType
                            select steps;

                return query.ToListAsync();
                */
            }
        }

        /// <summary>
        /// Gets the largest step IDA sync.
        /// </summary>
        /// <returns>The largest step IDA sync.</returns>
        /// <param name="taskType">Task type.</param>
        public async Task<int> GetLargestStepIDAsync(int taskType)
        {
            try
            {
                var result = await database.Table<SocialStepModel>().ToListAsync();
                var id = result.Aggregate((i1, i2) => i1.ID > i2.ID ? i1 : i2).ID;

                return id;
            }
            catch
            {
                return 0;
            }
        }

        /// <summary>
        /// Gets the social validity.
        /// </summary>
        /// <returns>The social validity.</returns>
        public Task<List<SocialValidityModel>> GetSocialValidity()
        {
            return database.Table<SocialValidityModel>().ToListAsync();
        }

        /// <summary>
        /// Gets the largest feedback identifier.
        /// </summary>
        /// <returns>The largest feedback identifier.</returns>
        public int GetLargestFeedbackID()
        {
            try
            {
                return GetSocialValidity().Result.Aggregate((i1, i2) => i1.ID > i2.ID ? i1 : i2).ID;
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

        /// <summary>
        /// Saves the item async.
        /// </summary>
        /// <returns>The item async.</returns>
        /// <param name="item">Item.</param>
        public Task<int> SaveItemAsync(SocialStepModel item)
        {
            return database.InsertAsync(item);
        }

        /// <summary>
        /// Saves the item async.
        /// </summary>
        /// <returns>The item async.</returns>
        /// <param name="item">Item.</param>
        public Task<int> SaveItemAsync(SocialValidityModel item)
        {
            return database.InsertAsync(item);
        }

        /// <summary>
        /// Deletes the step async.
        /// </summary>
        /// <returns>The step async.</returns>
        /// <param name="ID">Identifier.</param>
        public Task<int> DeleteStepAsync(int ID)
        {
            SocialStepModel item = GetStepsAsync().Result.Where(m => m.ID == ID).First();

            return database.DeleteAsync(item);
        }
    }
}
