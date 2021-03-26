using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class Invoice
    {
        [Key]
        public int InvoiceId { get; set; }

        public Company Company { get; set; }

        public int InvoiceNumber { get; set; }

        [Required]
        public string ContractNumber { get; set; }

        public decimal PriceWithoutVAT { get; set; }

        public int? VatRate { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/mm/yyyy}")]
        public DateTime DateOfIssue { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/mm/yyyy}")]
        public DateTime MaturityDay { get; set; }
    }
}
