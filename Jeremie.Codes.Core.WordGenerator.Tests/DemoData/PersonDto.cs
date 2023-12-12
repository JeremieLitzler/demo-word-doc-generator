using System.Data.SqlTypes;

namespace Jeremie.Codes.Core.WordGenerator.Tests.DemoData
{
    public class PersonDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string AddressLane { get; set; }
        public string AddressPostalCode { get; set; }
        public string AddressCity { get; set; }
    }
}
