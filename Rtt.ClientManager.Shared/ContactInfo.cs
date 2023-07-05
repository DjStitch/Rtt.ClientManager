using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Rtt.ClientManager.Shared;

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