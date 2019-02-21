using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Text;

namespace SampleTableStorage
{
    public class Blog : ATSRepository<Blog>
    {
        public int Id { get; set; }
        [PartitionKey]
        public string Name { get; set; }
        public string Description { get; set; }
        //public DateTime DateCreated { get; set; }
        public List<Post> Posts { get; set; }

        public Blog()
        {

        }

        public Blog(string id, string name)
        {
            //this.PartitionKey = id;
            //this.RowKey = name;
        }


    }

    public class RowKey : Attribute
    {
    }

    public class PartitionKey : Attribute
    {
    }

    public class Post //: TableEntity
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Permalink { get; set; }
        public string Content { get; set; }
        public string Author { get; set; }
        public DateTime DateCreated { get; set; }
        public List<Comment> Comments { get; set; }
    }

    public class Comment : ATSRepository<Comment>
    {
        //public int Id { get; set; }
        public string UserId { get; set; }
        public string Content { get; set; }
        //public DateTime DateCreated { get; set; }

        public Comment(string id, string name)
        {
            this.PartitionKey = id;
            this.RowKey = name;
        }
    }

    public class Services : ATSRepository<Services>
    {
        public ServiceDetail SERVICEDETAILS { get; set; }
        public TeamDetail TeamDetails { get; set; }

        public override void Get(string id)
        {
            CloudTableClient tableClient = ProgramNew.storageAccount.CreateCloudTableClient();
            CloudTable table = tableClient.GetTableReference(nameof(Services));
            // Create a retrieve operation that takes a customer entity.
            TableOperation retrieveServiceOperation = TableOperation.Retrieve<ServiceDetail>("NLAG-CT-20190224", "SERVICEDETAILS");
            TableOperation retrieveRehearsalOperation = TableOperation.Retrieve<RehearsalDetail>("NLAG-CT-20190224", "REHEARSALDETAILS");
            // Execute the retrieve operation.
            //TableResult retrievedResult = 
            var serviceDetails = table.ExecuteAsync(retrieveServiceOperation).Result;
            var rehearsalDetails = table.ExecuteAsync(retrieveRehearsalOperation).Result;

            TableQuery rangeQuery = new TableQuery().Where(
            TableQuery.CombineFilters(
            TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, "NLAG-CT-20190224"),
            TableOperators.And,
            TableQuery.GenerateFilterCondition("RowKey", QueryComparisons.GreaterThan, "")));

            var worshipLeaders = table.ExecuteQuerySegmentedAsync(rangeQuery, null).Result;
            foreach (var item in worshipLeaders.Results)
            {
                if (item.RowKey == "REHEARSALDETAILS")
                {
                    var props = item.Properties;

                    RehearsalDetail rehearsal = EntityPropertyConverter.ConvertBack<RehearsalDetail>(item.Properties, null);
                    
                }
            };
        }

    }



    public class WorshipLeader : TableEntity
    {
        public string Name { get; set; }
    }

    public class TeamDetail : TableEntity
    {
    }

    public class ServiceDetail : TableEntity
    {
        public string ServiceName { get; set; }
        public string ServiceNotes { get; set; }
    }

    public class RehearsalDetail : TableEntity
    {
        public string Date { get; set; }
        public string Location { get; set; }
    }
}
