
namespace CalifornianHealthLib.Models
{

    public partial class Patient
    {
        public int ID { get; set; }

        public string FName { get; set; }

        public string LName { get; set; }

        public string Address1 { get; set; }

        public string Address2 { get; set; }

        public string City { get; set; }

        public string Postcode { get; set; }
    }
}