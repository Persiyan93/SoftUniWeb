using System;
using System.ComponentModel.DataAnnotations;

namespace Models
{
    public class Company
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        
        public string VatNumber { get; set; }

        public int IdentificationNumber { get; set; }

        [Required]
        public int AddressId { get; set; }

        public Address Address { get; set; }

        public string ManagerName { get; set; }

        [Required]
        public bool IsForeignCompany { get; set; }
    }
}
