using System.ComponentModel.DataAnnotations;

namespace AssecoTest.Models
{
    public class UserInformation
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Username { get; set; }

        [Required]
        public string Email { get; set; }



        public string Street { get; set; }
        public string Suite { get; set; }
        public string City { get; set; }
        public string Zipcode { get; set; }
        public string Lat { get; set; }
        public string Lng { get; set; }


        public string Phone { get; set; }
        public string Website { get; set; }

        [Display(Name= "Company Name")]
        public string CompanyName { get; set; }
        [Display(Name = "Company Catch Phrase")]
        public string CompanyCatchPhrase { get; set; }
        public string Bs { get; set; }


    }
}

