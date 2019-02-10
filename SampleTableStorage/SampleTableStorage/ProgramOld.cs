using System;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System.Threading.Tasks;

namespace SampleTableStorage
{
    class ProgramOld
    {
        //url https://docs.microsoft.com/en-in/azure/visual-studio/vs-storage-aspnet5-getting-started-tables

        public static CloudStorageAccount storageAccount = new CloudStorageAccount(
        new Microsoft.WindowsAzure.Storage.Auth.StorageCredentials(
            "aztab", "zMXR8DQEYTWLspPXsvRLDadHXgDrmJKUatiVrXpchQ8ONOKDbC68iUh7s9tkRTl2Crn27j5T6BNCE62EpJdVPw=="), true);

        static void MainOld(string[] args)
        {
            //Connectionstring DefaultEndpointsProtocol=https;AccountName=aztab;AccountKey=zMXR8DQEYTWLspPXsvRLDadHXgDrmJKUatiVrXpchQ8ONOKDbC68iUh7s9tkRTl2Crn27j5T6BNCE62EpJdVPw==;EndpointSuffix=core.windows.net
             
            Console.WriteLine("Hello World!");

            CreatePeopleTableAsync().Wait();

            CustomerEntity customer1 = new CustomerEntity("Harp", "Walter");
            customer1.Email = "amit2@contoso.com";
            customer1.PhoneNumber = "425-555-0101";

            customer1.Add(customer1);

            ATSHelper h = new ATSHelper();
            h.Add(customer1);
            // Create the TableOperation that inserts the customer entity.
        }

        public static async Task<CloudTable> CreatePeopleTableAsync()
        {
            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();
            CloudTable peopleTable = tableClient.GetTableReference("peopletab");
            
            // Create the CloudTable if it does not exist
            peopleTable.CreateIfNotExistsAsync().Wait();

            return peopleTable;
        }
    }

    public class CustomerEntity : ABS, IRep<CustomerEntity> // : ATSHElper //: TableEntity
    {
        public CustomerEntity(string lastName, string firstName)
        {
            //this.PartitionKey = lastName;
            //this.RowKey = firstName;
        }

        public CustomerEntity() { }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }
        public override int Id { get ; set ; }

        public void Add(CustomerEntity obj)
        {
            //throw new NotImplementedException();
        }
    }

    public interface IRep<T>
    {
        void Add(T obj);
        void Get(string name);
    }

    public abstract class ABS : IRep<ABS>
    {
        public abstract int Id { get; set; }

        public virtual void Add(ABS obj)
        {
            //throw new NotImplementedException();
        }

        public virtual void Get(string name)
        {
            //throw new NotImplementedException();
        }
    }


    public class ATSHelper : TableEntity, IRep<ABS>//, IRep<CustomerEntity>
    {
        //public void Add(ATSHelper obj)
        //{
            

        //    this.PartitionKey = obj.GetType().GetProperty("Email").GetValue(obj,null).ToString();
        //    this.RowKey = "rowson";
            

        //    TableOperation insertOperation = TableOperation.InsertOrMerge(this);

        //    // Execute the insert operation.

        //    CloudTableClient tableClient = Program.storageAccount.CreateCloudTableClient();
        //    CloudTable peopleTable = tableClient.GetTableReference("peopletab");

        //    // Create the CloudTable if it does not exist
        //    peopleTable.CreateIfNotExistsAsync().Wait();
        //    peopleTable.ExecuteAsync(insertOperation).Wait();
        //}

        //public void Add(CustomerEntity obj)
        //{
        //    this.PartitionKey = obj.Email;
        //    this.RowKey = "rowson";


        //    TableOperation insertOperation = TableOperation.InsertOrMerge(this);

        //    // Execute the insert operation.

        //    CloudTableClient tableClient = Program.storageAccount.CreateCloudTableClient();
        //    CloudTable peopleTable = tableClient.GetTableReference("peopletab");

        //    // Create the CloudTable if it does not exist
        //    peopleTable.CreateIfNotExistsAsync().Wait();
        //    peopleTable.ExecuteAsync(insertOperation).Wait();
        //}

        public void Add(ABS obj)
        {
            if(obj.GetType().GetProperty("Email") != null)
            {
                string tableName = obj.GetType().Name;
                this.PartitionKey = obj.GetType().GetProperty("Email").GetValue(obj).ToString();
            }
        }

        public void Get(string name)
        {
            throw new NotImplementedException();
        }

    }
}
