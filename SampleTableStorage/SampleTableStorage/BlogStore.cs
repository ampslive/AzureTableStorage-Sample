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
}
