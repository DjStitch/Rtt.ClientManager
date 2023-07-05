using Rtt.ClientManager.DAL;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Rtt.ClientManager.DAL.Models;

namespace RTT.ClientManager.DAL.Models;

[Table("Addresses")]
public class Address
{
    [Key] public int AddressId { get; set; }
    [Required] public AddressType AddressType { get; set; } = AddressType.Home;
    [MaxLength(50)] public string Address1 { get; set; } = string.Empty;
    [MaxLength(50)] public string Address2 { get; set; } = string.Empty;
    [MaxLength(50)] public string Address3 { get; set; } = string.Empty;
    [ForeignKey("Client")] public int ClientId { get; set; }
    public Client? Client { get; set; } = null;
}

public enum AddressType
{
    Home,
    Work,
    Postal
}