using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
namespace AzureBlobUpload
{
    class Program
    {
        public static object CloudConfigurationManager { get; private set; }

        static void Main(string[] args)
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(
       ConfigurationManager.AppSettings.Get("StorageConnectionString"));

            // Create the blob client.
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

            // Retrieve reference to a previously created container.
            CloudBlobContainer container = blobClient.GetContainerReference("myfiles");
            
            // Retrieve a reference to a container.
            CloudBlockBlob blockBlob = container.GetBlockBlobReference("Hello.txt");
            // Create the container if it doesn't already exist.
            container.CreateIfNotExists();
            // Create or overwrite the "myblob" blob with contents from a local file.
            using (var fileStream = System.IO.File.OpenRead(@"Hello.txt"))
            {
                blockBlob.UploadFromStream(fileStream);//fails if container does not exist
            }
            //
            //By default, the new container is private, meaning that you must specify your storage access key to download blobs from this container.If you want to make the files within the container available to everyone, you can set the container to be public using the following code:
            //container.SetPermissions(
            //new BlobContainerPermissions { PublicAccess = BlobContainerPublicAccessType.Blob });

            // Loop over items within the container and output the length and URI.
            foreach (IListBlobItem item in container.ListBlobs(null, false))
            {
                if (item.GetType() == typeof(CloudBlockBlob))
                {
                    CloudBlockBlob blob = (CloudBlockBlob)item;

                    Console.WriteLine("Block blob of length {0}: {1}", blob.Properties.Length, blob.Uri);

                }
                else if (item.GetType() == typeof(CloudPageBlob))
                {
                    CloudPageBlob pageBlob = (CloudPageBlob)item;

                    Console.WriteLine("Page blob of length {0}: {1}", pageBlob.Properties.Length, pageBlob.Uri);

                }
                else if (item.GetType() == typeof(CloudBlobDirectory))
                {
                    CloudBlobDirectory directory = (CloudBlobDirectory)item;

                    Console.WriteLine("Directory: {0}", directory.Uri);
                }
            }
        }
    }
}
