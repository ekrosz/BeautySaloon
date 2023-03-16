using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication.Models.LocalDb
{
  [Table("Appointment", Schema = "public")]
  public partial class Appointment
  {
    public string Comment
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

    public int DurationInMinutes
    {
      get;
      set;
    }
    public Guid UserModifierId
    {
      get;
      set;
    }
    public User User { get; set; }
    public Guid PersonId
    {
      get;
      set;
    }
    public Person Person { get; set; }
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
    public DateTime AppointmentDate
    {
      get;
      set;
    }
  }
}
