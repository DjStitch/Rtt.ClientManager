using Rtt.ClientManager.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using RTT.ClientManager.DAL.Models;

namespace Rtt.ClientManager.WCF
{

    namespace Rtt.ClientManager.BLL
    {
        [ServiceContract]
        public interface IService1
        {
            [OperationContract]
            void CreateTablesIfNotExists();

            [OperationContract]
            void AddClient(Client client);

            [OperationContract]
            List<Client> GetAllClients();

            [OperationContract]
            int UpsertClient(Client client);

            [OperationContract]
            void UpdateClient(Client client);

            [OperationContract]
            void DeleteClient(int clientId);

            [OperationContract]
            Client GetClientById(int clientId);

            [OperationContract]
            void AddAddress(Address address);

            [OperationContract]
            void AddContactInfo(ContactInfo contactInfo);

            [OperationContract]
            void DeleteAddress(int addressId);

            [OperationContract]
            void DeleteContactInfo(int contactId);

            [OperationContract]
            void UpdateAddress(Address address);

            [OperationContract]
            void UpdateContactInfo(ContactInfo contactInfo);
        }
    }


    [DataContract]
    public class CompositeType
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public List<Client> Clients { get; set; }
    }
}
