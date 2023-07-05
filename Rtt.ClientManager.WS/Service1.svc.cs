using System;
using System.Collections.Generic;
using Rtt.ClientManager.Shared;

namespace Rtt.ClientManager.WS
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
    public class Service1 : IService1
    {
        public Service1()
        {
            Client = new BLL.ClientManager();
        }

        private BLL.ClientManager Client { get; }


        public void CreateDatabase()
        {
            Client.CreateTablesIfNotExists();
        }

        public string GetData(int value)
        {
            return string.Format("You entered: {0}", value);
        }


        public CompositeType GetDataUsingDataContract(CompositeType composite)
        {
            if (composite == null) throw new ArgumentNullException("composite");
            if (composite.BoolValue) composite.StringValue += "Suffix";
            return composite;
        }

        public void DeleteClient(Client client)
        {
            Client.DeleteClient(client.ClientId);
        }

        public void UpdateClient(Client client)
        {
            Client.UpdateClient(client);
        }


        public List<Client> GetAllClients()
        {
            return Client.GetAllClients();
        }

        public Client CreateClient(Client createClient)
        {
            Client.AddClient(createClient);

            return createClient;
        }

        public Client Upsert(Client updClient)
        {
            Client.UpsertClient(updClient);
            return updClient;
        }
    }
}