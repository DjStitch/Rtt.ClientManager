using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using RTT.ClientManager.DAL.Models;

namespace Rtt.ClientManager.DAL.Models;

[Table("Clients")]
public class Client
{
    [Key] public int ClientId { get; set; } 

    [MaxLength(50)] [Required] public string FirstName { get; set; } = string.Empty;

    [Required] [MaxLength(50)] public string LastName { get; set; } = string.Empty;

    [Column("Age")] [Range(0, 120)] public int Age { get; set; }

    [Column("Gender")] public string Gender { get; set; } = string.Empty;

    public DateTime? DateOfBirth { get; set; }

    [MaxLength(50)] public string Nationality { get; set; } = string.Empty;

    [MaxLength(13)] public string IdentificationNumber { get; set; } = string.Empty;

    [MaxLength(50)] public string Occupation { get; set; } = string.Empty;

    public List<Address> Addresses { get; set; } = new();
    public List<ContactInfo> ContactInformation { get; set; } = new();
}