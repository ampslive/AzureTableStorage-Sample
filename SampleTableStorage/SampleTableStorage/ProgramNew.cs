using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SampleTableStorage
{
    class ProgramNew
    {
        //url https://docs.microsoft.com/en-in/azure/visual-studio/vs-storage-aspnet5-getting-started-tables
        // https://docs.microsoft.com/en-us/azure/cosmos-db/table-storage-design-guide

        public static CloudStorageAccount storageAccount = new CloudStorageAccount(
        new Microsoft.WindowsAzure.Storage.Auth.StorageCredentials(
            "pocats", "LdDT9SnihG+U64KbsBJ31xIdBbZxtEQMuX7lOT+ixUidE3rURq4t6nfBj0q+aXPorOHskzVlz3fizsW6Gy6KiA=="), true);

        static void MainNew(string[] args)
        {
            CreateTableAsync(nameof(Blog)).Wait();

            //Blog blog = new Blog("1", "Test");
            //blog.Name = "My Second Blog";
            //blog.DateCreated = DateTime.Now;
            ////blog.Posts = new List<Post>() { new Post { Title = "Post 1" } };
            //blog.Description = "Description for Second Blog";
            //blog.Add(blog);

            //Comment com = new Comment("Comment", "Com2");
            //com.Content = "Sample Content";
            ////com.DateCreated = DateTime.Now;
            //com.Add(com);

            Blog com = new Blog(Guid.NewGuid().ToString(), "A new beginning");

            com.Get("");
        }

        public static async Task<CloudTable> CreateTableAsync(string tableName)
        {
            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();
            CloudTable table = tableClient.GetTableReference(tableName);

            // Create the CloudTable if it does not exist
            table.CreateIfNotExistsAsync().Wait();

            return table;
        }
    }

    
}
