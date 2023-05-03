using BeautySaloon.DAL.Entities.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeautySaloon.DAL.Entities
{
    public class InvoiceMaterial : IEntity, IAuditable
    {
        [Obsolete("For EF")]
        private InvoiceMaterial()
        {
        }

        public InvoiceMaterial(
            Guid materialId, 
            int count, 
            decimal? cost)
        {
            MaterialId = materialId;
            Count = count;
            Cost = cost;
        }

        public Guid Id { get; set; }

        public int Count { get; set; }

        public decimal? Cost { get; set; }

        public Material Material { get; set; } = default!;

        public Guid MaterialId { get; set; }

        public Invoice Invoice { get; set; } = default!;

        public DateTime CreatedOn { get; set; }

        public DateTime UpdatedOn { get; set; }

        public Guid UserModifierId { get; set; }

    }
}
