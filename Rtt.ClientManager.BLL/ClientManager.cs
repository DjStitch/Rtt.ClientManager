using System.Collections.Generic;
using Rtt.ClientManager.DAL;
using Rtt.ClientManager.DAL.Models;
using RTT.ClientManager.DAL.Models;

namespace Rtt.ClientManager.BLL
{
    public class ClientManager
    {
        private readonly ClientDbAccess _clientDbAccess;

        public ClientManager(string dbFilePath)
        {
            _clientDbAccess = new ClientDbAccess(dbFilePath);
        }

        public void CreateTablesIfNotExists()
        {
            _clientDbAccess.CreateTablesIfNotExists();
        }

        public void AddClient(Client client)
        {
            _clientDbAccess.AddClient(client);
        }

        public List<Client> GetAllClients()
        {
            return _clientDbAccess.GetAllClients();
        }

        public int UpsertClient(Client client)
        {
            return _clientDbAccess.UpsertClient(client);
        }

        public void UpdateClient(Client client)
        {
            _clientDbAccess.UpdateClient(client);
        }

        public void DeleteClient(int clientId)
        {
            _clientDbAccess.DeleteClient(clientId);
        }

        public Client GetClientById(int clientId)
        {
            return _clientDbAccess.GetClientById(clientId);
        }

        public void AddAddress(Address address)
        {
            _clientDbAccess.AddAddress(address);
        }

        public void AddContactInfo(ContactInfo contactInfo)
        {
            _clientDbAccess.AddContactInfo(contactInfo);
        }

        public void DeleteAddress(int addressId)
        {
            _clientDbAccess.DeleteAddress(addressId);
        }

        public void DeleteContactInfo(int contactId)
        {
            _clientDbAccess.DeleteContactInfo(contactId);
        }

        public void UpdateAddress(Address address)
        {
            _clientDbAccess.UpdateAddress(address);
        }

        public void UpdateContactInfo(ContactInfo contactInfo)
        {
            _clientDbAccess.UpdateContactInfo(contactInfo);
        }
    }
}
