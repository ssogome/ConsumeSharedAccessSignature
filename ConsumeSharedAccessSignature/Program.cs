using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;


namespace ConsumeSharedAccessSignature
{
    class Program
    {
        static void Main(string[] args)
        {
            string containerSAS = "<your container SAS>";
            string blobSAS = "<your blob SAS>";
            string containerSASWithAccessPolicy = "<your container SAS with access policy>";
            string blobSASWithAccessPolicy = "<your blob SAS with access policy>";
        }

        static void UseContainerSAS(string sas)
        {
            //Try performing container operations with the SAS provided.

            //Return a reference to the container using the SAS URI.
            CloudBlobContainer container = new CloudBlobContainer(new Uri(sas));

            //Create a list to store blob URIs returned by a listing operation on the container.
            List<ICloudBlob> blobList = new List<ICloudBlob>();

            //Write operation: write a new blob to the container.
            try
            {
                CloudBlockBlob blob = container.GetBlockBlobReference("blobCreatedViaSAS.txt");
                string blobContent = "This blob was created with a shared access signature granting write permissions to the container. ";
                MemoryStream msWrite = new MemoryStream(Encoding.UTF8.GetBytes(blobContent));
                msWrite.Position = 0;
                using (msWrite)
                {
                    blob.UploadFromStream(msWrite);
                }
                Console.WriteLine("Write operation succeeded for SAS " + sas);
                Console.WriteLine();
            }
            catch (StorageException e)
            {
                Console.WriteLine("Write operation failed for SAS " + sas);
                Console.WriteLine("Additional error information: " + e.Message);
                Console.WriteLine();
            }

            //List operation: List the blobs in the container.
            try
            {
                foreach (ICloudBlob blob in container.ListBlobs())
                {
                    blobList.Add(blob);
                }
                Console.WriteLine("List operation succeeded for SAS " + sas);
                Console.WriteLine();
            }
            catch (StorageException e)
            {
                Console.WriteLine("List operation failed for SAS " + sas);
                Console.WriteLine("Additional error information: " + e.Message);
                Console.WriteLine();
            }

            //Read operation: Get a reference to one of the blobs in the container and read it.
            try
            {
                CloudBlockBlob blob = container.GetBlockBlobReference(blobList[0].Name);
                MemoryStream msRead = new MemoryStream();
                msRead.Position = 0;
                using (msRead)
                {
                    blob.DownloadToStream(msRead);
                    Console.WriteLine(msRead.Length);
                }
                Console.WriteLine("Read operation succeeded for SAS " + sas);
                Console.WriteLine();
            }
            catch (StorageException e)
            {
                Console.WriteLine("Read operation failed for SAS " + sas);
                Console.WriteLine("Additional error information: " + e.Message);
                Console.WriteLine();
            }
            Console.WriteLine();

            //Delete operation: Delete a blob in the container.
            try
            {
                CloudBlockBlob blob = container.GetBlockBlobReference(blobList[0].Name);
                blob.Delete();
                Console.WriteLine("Delete operation succeeded for SAS " + sas);
                Console.WriteLine();
            }
            catch (StorageException e)
            {
                Console.WriteLine("Delete operation failed for SAS " + sas);
                Console.WriteLine("Additional error information: " + e.Message);
                Console.WriteLine();
            }
        }

    }
}
