using BeautySaloon.DAL.Entities.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeautySaloon.DAL.Entities
{
    public class  Material : IEntity, IAuditable
    {
        [Obsolete("For EF")]
        private Material()
        {
        }
        public Material(
            string name,
            string? description)
        {
            Name = name;
            Description = description;
        }
        public Guid Id { get; set; }

        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public DateTime CreatedOn { get; set; }

        public DateTime UpdatedOn { get; set; }
        public Guid UserModifierId { get; set; }
        public void Update(
        string name,
        string? description)
        {
            Name = name;
            Description = description;
        }
    }
}
