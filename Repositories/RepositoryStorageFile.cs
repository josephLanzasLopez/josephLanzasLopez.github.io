using Microsoft.Azure;
using Microsoft.WindowsAzure.Storage.File;
using Microsoft.WindowsAzure.Storage;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace LN_WEB.Repositories
{
    public class RepositoryStorageFile
    {

        CloudFileDirectory root;
        public RepositoryStorageFile()
        {
            String keys = CloudConfigurationManager.GetSetting("ConexionBlobs");
            CloudStorageAccount account = CloudStorageAccount.Parse(keys);
            CloudFileClient client = account.CreateCloudFileClient();
            CloudFileShare shared = client.GetShareReference("archivos");
            this.root = shared.GetRootDirectoryReference();
        }
        public void UploadFile(String filename, Stream stream)
        {
            CloudFile file = this.root.GetFileReference(filename);
            file.UploadFromStream(stream);
        }
        public void DeleteFile(String filename)
        {
            CloudFile file = this.root.GetFileReference(filename);
            file.DeleteIfExists();
        }
        public List<String> GetStorageFiles()
        {
            List<String> filenames = new List<String>();
            List<IListFileItem> storagefiles = this.root.ListFilesAndDirectories().ToList();
            foreach (IListFileItem file in storagefiles)
            {
                String uri = file.StorageUri.PrimaryUri.ToString();
                int last = uri.LastIndexOf('/') + 1;
                String filename = uri.Substring(last);
                filenames.Add(filename);
            }
            return filenames;
        }
    }
}