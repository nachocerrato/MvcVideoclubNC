using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using MvcVideoclubNC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MvcVideoclubNC.Services
{
    public class ServiceStorageBlobs
    {
        private BlobServiceClient client;

        public ServiceStorageBlobs(string keys)
        {
            this.client = new BlobServiceClient(keys);
        }

        //Método para devolver todos los contenedores
        public async Task<List<string>> GetContainersAsync()
        {
            List<string> containers = new List<string>();
            await 
                foreach(var container in this.client.GetBlobContainersAsync())
            {
                containers.Add(container.Name);
            }
            return containers;
        }

        //Método para mostrar los blobs de un contenedor
        public async Task<List<Blob>> GetBlobsAsync(string containerName)
        {
            BlobContainerClient containerClient =
                this.client.GetBlobContainerClient(containerName);
            List<Blob> blobs = new List<Blob>();
            await
                foreach(BlobItem blob in containerClient.GetBlobsAsync())
            {
                BlobClient blobClient = containerClient.GetBlobClient(blob.Name);
                blobs.Add(
                    new Blob
                    {
                        Nombre = blob.Name,
                        Url = blobClient.Uri.AbsoluteUri
                    });
            }
            return blobs;
        }

        public string GetBlobUrl(string containerName, string imagen)
        {
            
            BlobContainerClient containerClient =
                this.client.GetBlobContainerClient(containerName);

            BlobClient blobClient = containerClient.GetBlobClient(imagen);

            return blobClient.Uri.AbsoluteUri;
        }
    }
}
