using System;
using System.Collections.Generic;
using System.Text;

namespace MyApp.Core.Models
{
    public class MyEntity : TenantEntity
    {
        //use a string? when this will not be a required field. 
        //null! stops a compiler warning. Since this is a DB required field we can suppress the warning here.
        public string Name { get; set; } = null!; 

        public string? Description { get; set; }
    }
}
