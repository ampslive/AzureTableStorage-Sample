using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace SampleTableStorage
{
    /// <summary>
    /// Azure Table Storage Repository
    /// </summary>
    public abstract class ATSRepository<T> : TableEntity, IRepository<T>
    {
        private CloudTableClient _tableClient;
        private CloudTable _cloudTable;
        private TableOperation _tableOperation;
        public ATSRepository()
        {
            _tableClient = ProgramNew.storageAccount.CreateCloudTableClient();
        }

        public virtual void Add(T obj)
        {
            //Table Exists?
            _cloudTable = _tableClient.GetTableReference(obj.GetType().Name);
            _cloudTable.CreateIfNotExistsAsync().Wait();

            //Obtain Partition Key and Row Key
            //SetPartitionRowKeys(obj);
            PropertyInfo[] props = obj.GetType().GetProperties();
            foreach(var p in props)
            {
                if(p.Name != nameof(RowKey) && p.Name != nameof(PartitionKey) && p.Name != nameof(ETag) && p.Name != nameof(Timestamp))
                {
                    obj.GetType().GetProperty("RowKey").SetValue(obj, p.Name.ToString(), null);

                    //TODO: Create Object with PPartition, Row and Properties 
                    string rowkey = obj.GetType().GetProperty("RowKey").GetValue(obj).ToString();
                    string partitionKey = obj.GetType().GetProperty("PartitionKey").GetValue(obj).ToString();


                    if (p.PropertyType.Name.Contains("String"))
                    {
                        var entity = new DynamicTableEntity(partitionKey, rowkey);
                        //entity.Properties = EntityPropertyConverter.Flatten(p.GetValue(obj), null);

                        entity.Properties.Add("Value", new EntityProperty(p.GetValue(obj).ToString()));
                        InsertIntoTable(entity);
                    }
                    else if (!p.PropertyType.Name.Contains("List"))
                    {
                        var entity = new DynamicTableEntity(partitionKey, rowkey);
                        entity.Properties = EntityPropertyConverter.Flatten(p.GetValue(obj), null);

                        InsertIntoTable(entity);
                    }
                    else
                    {
                        int i = 1;
                        foreach (var f in (IEnumerable)p.GetValue(obj))
                        {
                            var entity = new DynamicTableEntity(partitionKey.ToUpper(), rowkey + EntityPropertyConverter.DefaultPropertyNameDelimiter + i++);
                            entity.Properties = EntityPropertyConverter.Flatten(f, null);

                            InsertIntoTable(entity);
                        }
                    }

                    //_tableOperation = TableOperation.InsertOrMerge((ITableEntity)entity);
                    //_cloudTable.ExecuteAsync(_tableOperation).Wait();
                    
                }
            }

            //Insert or Upsert
            //_tableOperation = TableOperation.InsertOrMerge((ITableEntity)obj);
            //_cloudTable.ExecuteAsync(_tableOperation).Wait();
        }

        private void InsertIntoTable(DynamicTableEntity entity)
        {
            _tableOperation = TableOperation.InsertOrMerge((ITableEntity)entity);
            _cloudTable.ExecuteAsync(_tableOperation).Wait();
        }

        private static void SetPartitionRowKeys(T obj)
        {
            var partitionkey = obj.GetType().Name;
            var rowkey = obj.GetType().GetProperty("Name").GetValue(obj).ToString();

            PropertyInfo[] props = obj.GetType().GetProperties();
            List<object> lst = new List<object>();
            foreach (var p in props)
            {
                lst.AddRange(p.GetCustomAttributes());

                if (p.GetCustomAttributes(typeof(RowKey), false).Length > 0)
                {
                    p.SetValue(obj, partitionkey,null);
                    break;
                }
            }
            //if(lst.C .e(typeof(PartitionKey)))
            //{

            //}

            obj.GetType().GetProperty("RowKey").SetValue(obj, rowkey, null);
        }

        public virtual void Get(string id)
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
        }

        public virtual List<T> GetAll()
        {
            throw new NotImplementedException();
        }
    }

    interface IRepository<T>
    {
        void Get(string id);
        List<T> GetAll();
        void Add(T obj);
    }
}
