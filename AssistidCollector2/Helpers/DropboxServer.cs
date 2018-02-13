﻿using System;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using AssistidCollector2.Interfaces;
using Dropbox.Api.Files;
using Xamarin.Forms;

namespace AssistidCollector2.Helpers
{
    public static class DropboxServer
    {
        /// <summary>
        /// Create remote folder
        /// </summary>
        /// <returns></returns>
        public static async Task<bool> CreateDropboxFolder()
        {
            await Task.Delay(App.DropboxDeltaTimeout);

            Metadata meta = await App.DropboxClient.Files.GetMetadataAsync("/" + App.ApplicationId);

            CreateFolderResult response = null;

            if (!meta.IsFolder)
            {
                response = await App.DropboxClient.Files.CreateFolderV2Async("/" + App.ApplicationId);

                return response.Metadata.IsFolder;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// Download file to local
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        private static async Task DownloadFile(string filePath)
        {
            Debug.WriteLineIf(App.Debugging, Settings.AppName + " >>> Downloading " + filePath);

            using (var response = await App.DropboxClient.Files.DownloadAsync("/" + filePath))
            {
                var receivedData = await response.GetContentAsStringAsync();

                DependencyService.Get<InterfaceSaveLoad>().SaveFile(filePath, receivedData);
            }
        }

        /// <summary>
        /// Count files
        /// </summary>
        /// <returns></returns>
        public static async Task<ListFolderResult> CountIndividualFiles()
        {
            await Task.Delay(App.DropboxDeltaTimeout);

            try
            {
                ListFolderResult response = await App.DropboxClient.Files.ListFolderAsync("/" + App.ApplicationId);

                if (response == null || response.Entries.Count == 0)
                {
                    return null;
                }

                return response;

            }
            catch (Dropbox.Api.ApiException<GetMetadataError>)
            {
                return null;
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// Counts the feedback files.
        /// </summary>
        /// <returns>The feedback files.</returns>
        public static async Task<ListFolderResult> CountFeedbackFiles()
        {
            await Task.Delay(App.DropboxDeltaTimeout);

            try
            {
                ListFolderResult response = await App.DropboxClient.Files.ListFolderAsync("/img-" + App.ApplicationId);

                if (response == null || response.Entries.Count == 0)
                {
                    return null;
                }

                return response;

            }
            catch (Dropbox.Api.ApiException<GetMetadataError>)
            {
                return null;
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// Upload file
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="fileNumber"></param>
        public static async Task<string> UploadFile(System.IO.MemoryStream stream, int fileNumber)
        {
            await Task.Delay(App.DropboxDeltaTimeout);

            string filePath = "/" + App.ApplicationId + "/" + App.ApplicationId + "_" + fileNumber.ToString("d4") + ".csv";

            string result = await UploadFile(stream, filePath);

            return result;
        }

        /// <summary>
        /// Upload file task
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="filePath"></param>
        /// <returns></returns>
        private static async Task<string> UploadFile(System.IO.MemoryStream stream, string filePath)
        {
            FileMetadata uploaded = await App.DropboxClient.Files.UploadAsync(filePath, WriteMode.Overwrite.Instance, body: stream);

            return uploaded.Id;
        }

        /// <summary>
        /// Uploads the image.
        /// </summary>
        /// <returns>The image.</returns>
        /// <param name="stream">Stream.</param>
        /// <param name="fileNumber">File number.</param>
        public static async Task<string> UploadImage(System.IO.MemoryStream stream, int fileNumber)
        {
            await Task.Delay(App.DropboxDeltaTimeout);

            string filePath = "/img-" + App.ApplicationId + "/" + App.ApplicationId + "_" + fileNumber.ToString("d4") + ".png";

            string result = await UploadFile(stream, filePath);

            return result;            
        }

        /// <summary>
        /// Upload a message to cloud
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public static async Task<string> UploadMessage(string message)
        {
            string messagePath = String.Format("{0:u}", DateTime.Now) + ".txt";

            messagePath = messagePath.Replace(':', '-');
            messagePath = messagePath.Replace(' ', '_');

            messagePath = "/messages/" + App.ApplicationId + "_" + messagePath;

            FileMetadata uploadedMsg = await App.DropboxClient.Files.UploadAsync(messagePath, WriteMode.Overwrite.Instance,
                body: new System.IO.MemoryStream(Encoding.UTF8.GetBytes(message)));

            return uploadedMsg.Id;
        }
    }
}
