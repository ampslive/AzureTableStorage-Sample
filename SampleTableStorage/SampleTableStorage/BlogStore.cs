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
            CloudTable table = tableClient.GetTableReference(this.GetType().Name);

            // Point Query
            TableOperation retrieveServiceOperation = TableOperation.Retrieve<ServiceDetail>("NLAG-CT-20190224", "SERVICEDETAILS");
            var serviceDetails = table.ExecuteAsync(retrieveServiceOperation).Result;

            TableOperation retrieveRehearsalOperation = TableOperation.Retrieve<RehearsalDetail>("NLAG-CT-20190224", "REHEARSALDETAILS");
            var rehearsalDetails = table.ExecuteAsync(retrieveRehearsalOperation).Result;

            //Range Query on Row Key
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

            //Range Query on Partition Key
            var rangeQueryPKey = new TableQuery().Where(
                TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.GreaterThan, "NLAG-CT-20190210"));

            var pkeys = table.ExecuteQuerySegmentedAsync(rangeQueryPKey, null).Result;

            //Partition Scan
            TableQuery queryPScan = new TableQuery().Where(
            TableQuery.CombineFilters(
            TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, "NLAG-CT-20190217"),
            TableOperators.And,
            TableQuery.GenerateFilterCondition("AG1", QueryComparisons.Equal, "Manjrekar")));

            var ag1Manjrekar = table.ExecuteQuerySegmentedAsync(queryPScan, null).Result;

            //Table Scan
            TableQuery queryTScan = new TableQuery().Where(
            TableQuery.GenerateFilterCondition("AG1", QueryComparisons.Equal, "Manjrekar"));

            var ag1ManjrekarTScan = table.ExecuteQuerySegmentedAsync(queryTScan, null).Result;
        }

    }

    public class Organisation  
    {
        public string Name { get; set; }
        public string ShortName { get; set; }
        public string LogoUri { get; set; }
        public string WebsiteUri { get; set; }
    }

    public class Subscription
    {
        public string SubscriptionType { get; set; }
    }

    public class Department
    {
        public string Name { get; set; }
        public string ShortName { get; set; }
        public bool IsActive { get; set; }
    }

    public class OrganisationEntity : ATSRepository<OrganisationEntity>
    {
        public OrganisationEntity(string partitionKey)
        {
            this.PartitionKey = partitionKey;
        }
        public Organisation Details { get; set; }
        public string Subscription { get; set; }
        public List<Department> Departments { get; set; }
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
