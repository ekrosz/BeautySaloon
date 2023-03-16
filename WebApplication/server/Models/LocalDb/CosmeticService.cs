using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication.Models.LocalDb
{
  [Table("CosmeticService", Schema = "public")]
  public partial class CosmeticService
  {
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
    public Guid UserModifierId
    {
      get;
      set;
    }
    public string Description
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

    public int ExecuteTimeInMinutes
    {
      get;
      set;
    }
    public DateTime UpdatedOn
    {
      get;
      set;
    }
  }
}
