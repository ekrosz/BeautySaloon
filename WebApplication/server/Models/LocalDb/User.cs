using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication.Models.LocalDb
{
  [Table("User", Schema = "public")]
  public partial class User
  {
    public Guid? RefreshSecretKey
    {
      get;
      set;
    }
    public int Role
    {
      get;
      set;
    }
    public bool IsDeleted
    {
      get;
      set;
    }
    public string PhoneNumber
    {
      get;
      set;
    }
    public string Name_LastName
    {
      get;
      set;
    }
    public string Email
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
    public string Password
    {
      get;
      set;
    }
    public string Name_MiddleName
    {
      get;
      set;
    }
    public string Name_FirstName
    {
      get;
      set;
    }
    public string Login
    {
      get;
      set;
    }
  }
}
