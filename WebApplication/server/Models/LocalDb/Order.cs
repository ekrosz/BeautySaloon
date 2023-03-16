using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication.Models.LocalDb
{
  [Table("Order", Schema = "public")]
  public partial class Order
  {
    public DateTime UpdatedOn
    {
      get;
      set;
    }
    public Guid PersonId
    {
      get;
      set;
    }
    public Person Person { get; set; }
    public Guid UserModifierId
    {
      get;
      set;
    }
    public User User { get; set; }
    public int PaymentMethod
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

    public string Comment
    {
      get;
      set;
    }
    public decimal Cost
    {
      get;
      set;
    }
    public DateTime CreatedOn
    {
      get;
      set;
    }
  }
}
