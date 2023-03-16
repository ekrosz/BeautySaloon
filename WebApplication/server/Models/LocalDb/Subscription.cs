using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication.Models.LocalDb
{
  [Table("Subscription", Schema = "public")]
  public partial class Subscription
  {
    public Guid UserModifierId
    {
      get;
      set;
    }
    public int? LifeTimeInDays
    {
      get;
      set;
    }
    [Key]
    public Guid Id
    {
      get;
      set;
    }

    public decimal Price
    {
      get;
      set;
    }
    public DateTime UpdatedOn
    {
      get;
      set;
    }
    public DateTime CreatedOn
    {
      get;
      set;
    }
    public string Name
    {
      get;
      set;
    }
  }
}
