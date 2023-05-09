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
        [Obsolete("For EF")]
        private Invoice()
        {
        }

        public Invoice(
        InvoiceType invoiceType,
        DateTime invoiceDate,
        Guid? employeeId,
        string? comment)
        {
            InvoiceType = invoiceType;
            EmployeeId = employeeId;
            InvoiceDate = invoiceDate;
            Comment = comment;
        }
        public Guid Id { get; set; }

        public InvoiceType InvoiceType { get; set; }

        public DateTime InvoiceDate { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime UpdatedOn { get; set; }

        public User? Employee { get; set; } = default!;

        public Guid? EmployeeId { get; set; }

        public string? Comment { get; set; }

        public List<InvoiceMaterial> InvoiceMaterials { get; set; } = new List<InvoiceMaterial>();

        public void AddMaterials(IEnumerable<InvoiceMaterial> materials)
        {
            InvoiceMaterials.Clear();
            InvoiceMaterials.AddRange(materials);
        }

        public Guid UserModifierId { get; set; }

        public void Update(
        InvoiceType invoiceType,
        DateTime invoiceDate,
        Guid? employeeId,
        string? comment)
        {
            InvoiceType = invoiceType;
            InvoiceDate = invoiceDate;
            EmployeeId = employeeId;
            Comment = comment;
        }
    }
}
