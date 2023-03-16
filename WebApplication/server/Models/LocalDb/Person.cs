using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication.Models.LocalDb
{
  [Table("Person", Schema = "public")]
  public partial class Person
  {
    public DateTime BirthDate
    {
      get;
      set;
    }
    public Guid UserModifierId
    {
      get;
      set;
    }
    public DateTime UpdatedOn
    {
      get;
      set;
    }
    public string Name_FirstName
    {
      get;
      set;
    }
    public string Name_LastName
    {
      get;
      set;
    }
    public DateTime CreatedOn
    {
      get;
      set;
    }
    public string PhoneNumber
    {
      get;
      set;
    }
    public string Name_MiddleName
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

    public ICollection<Appointment> Appointments { get; set; }
    public ICollection<Order> Orders { get; set; }
    public string Email
    {
      get;
      set;
    }
    public bool IsDeleted
    {
      get;
      set;
    }
  }
}
