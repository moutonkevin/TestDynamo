using System.Collections.Generic;

namespace Homedish.Models
{
    public class Address
    {
        public IEnumerable<string> Lines { get; set; }
        public string Postcode { get; set; }
        public string Suburb { get; set; }
    }
}
