using System;
using System.Collections.Generic;

namespace Gestion.Models
{
    public class Invoice
    {
        public int Id { get; set; }
        public DateTime Date { get; set; } = DateTime.Now;

        public int ClientId { get; set; }
        public Client Client { get; set; }

        public List<InvoiceDetail> Details { get; set; } = new List<InvoiceDetail>();
    }
}
