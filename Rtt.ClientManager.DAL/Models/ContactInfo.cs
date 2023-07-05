using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Rtt.ClientManager.DAL;
using Rtt.ClientManager.DAL.Models;

namespace RTT.ClientManager.DAL.Models;

[Table("ContactInformation")]
public class ContactInfo
{
    [Key] public int ContactId { get; set; }
    public ContactType ContactType { get; set; } = ContactType.Cell;
    public string Number { get; set; } = string.Empty;
    [ForeignKey("Client")] public int ClientId { get; set; }
    public Client? Client { get; set; } = null;
}

public enum ContactType
{
    Cell,
    HomePhone,
    Email,
    Work
}