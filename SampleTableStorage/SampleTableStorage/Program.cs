using System;
using System.Collections.Generic;
using System.Text;

namespace SampleTableStorage
{
    class Program
    {
        static void Main(string[] args)
        {
            Blog blog = new Blog();
            blog.Name = "New Beginning " + DateTimeOffset.Now;
            //blog.DateCreated = DateTime.Now;

            //ATSRepository<Blog> obj = new Blog();
            //obj.Add(blog);
            //obj.Get("");

            ATSRepository<Services> obj = new Services();
            obj.Get("");
        }
    }

    
}
