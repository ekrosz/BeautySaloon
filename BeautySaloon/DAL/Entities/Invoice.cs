using BeautySaloon.DAL.Entities.Contracts;
using BeautySaloon.DAL.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeautySaloon.DAL.Entities
{
    public class Invoice : IEntity, IAuditable
    {
        public Guid Id { get; set; }

        public InvoiceType InvoiceType { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime UpdatedOn { get; set; }

        public User? Employee { get; set; } = default!;

        public string? Comment { get; set; }

        public int Count { get; set; }

        public decimal? Cost { get; set; }

        public Material Material { get; set; } = default!;

        public Guid UserModifierId { get; set; }

        public void Update(
        InvoiceType invoiceType,
        int count,
        decimal cost,
        Material material,
        User? employee,
        string? comment)
        {
            InvoiceType = invoiceType;
            Count = count;
            Cost = cost;
            Material = material;
            Employee = employee;
            Comment = comment;
        }
    }
}
