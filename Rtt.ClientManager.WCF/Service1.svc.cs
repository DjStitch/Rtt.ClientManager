using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using Rtt.ClientManager.DAL.Models;
using Rtt.ClientManager.WCF.Rtt.ClientManager.BLL;
using RTT.ClientManager.DAL.Models;

namespace Rtt.ClientManager.WCF
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.

    public class Service1 : IService1
    {
        private readonly BLL.ClientManager _clientManager;

        public Service1(string dbFilePath)
        {
            _clientManager = new BLL.ClientManager(dbFilePath);
        }

        public void CreateTablesIfNotExists()
        {
            _clientManager.CreateTablesIfNotExists();
        }

        public void AddClient(Client client)
        {
            _clientManager.AddClient(client);
        }

        public List<Client> GetAllClients()
        {
            return _clientManager.GetAllClients();
        }

        public int UpsertClient(Client client)
        {
            return _clientManager.UpsertClient(client);
        }

        public void UpdateClient(Client client)
        {
            _clientManager.UpdateClient(client);
        }

        public void DeleteClient(int clientId)
        {
            _clientManager.DeleteClient(clientId);
        }

        public Client GetClientById(int clientId)
        {
            return _clientManager.GetClientById(clientId);
        }

        public void AddAddress(Address address)
        {
            _clientManager.AddAddress(address);
        }

        public void AddContactInfo(ContactInfo contactInfo)
        {
            _clientManager.AddContactInfo(contactInfo);
        }

        public void DeleteAddress(int addressId)
        {
            _clientManager.DeleteAddress(addressId);
        }

        public void DeleteContactInfo(int contactId)
        {
            _clientManager.DeleteContactInfo(contactId);
        }

        public void UpdateAddress(Address address)
        {
            _clientManager.UpdateAddress(address);
        }

        public void UpdateContactInfo(ContactInfo contactInfo)
        {
            _clientManager.UpdateContactInfo(contactInfo);
        }

        public CompositeType GetCompositeData()
        {
            // Create a sample CompositeType object
            var compositeData = new CompositeType
            {
                Id = 1,
                Name = "Sample Composite Data",
                Clients = _clientManager.GetAllClients()
            };

            return compositeData;
        }
    }

}
