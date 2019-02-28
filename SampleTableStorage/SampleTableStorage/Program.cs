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

            var obj = new OrganisationEntity("NLAG");
            obj.Departments = new List<Department>();
            obj.Departments.Add(new Department { Name = "Youth Alive", ShortName = "YA", IsActive = true });
            obj.Details = new Organisation() { Name = "New Life Assembly", ShortName = "NLAG", WebsiteUri = "Website", LogoUri = "Logo" };
            ATSRepository<OrganisationEntity> ats = new OrganisationEntity("NLAG");
            ats.Add(obj);
           // obj.Get("");
        }
    }

    
}
