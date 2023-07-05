using System;
using System.Collections.Generic;
using System.Data.SQLite;
using Rtt.ClientManager.DAL.Models;
using RTT.ClientManager.DAL.Models;

namespace Rtt.ClientManager.DAL;

public class ClientDbAccess
{
    private readonly string _connectionString;

    public ClientDbAccess(string dbFilePath)
    {
        _connectionString = $"Data Source={dbFilePath};Version=3;";
    }

    public void CreateTablesIfNotExists()
    {
        using var connection = new SQLiteConnection(_connectionString);
        connection.Open();

        const string query = @"
        CREATE TABLE IF NOT EXISTS Clients (
            ClientId INTEGER PRIMARY KEY AUTOINCREMENT,
            FirstName TEXT NOT NULL,
            LastName TEXT NOT NULL,
            Age INTEGER CHECK (Age >= 0 AND Age <= 120),
            Gender TEXT DEFAULT '',
            DateOfBirth DATETIME,
            Nationality TEXT DEFAULT '',
            IdentificationNumber TEXT DEFAULT '',
            Occupation TEXT DEFAULT ''
        );

        CREATE TABLE IF NOT EXISTS Addresses (
            AddressId INTEGER PRIMARY KEY AUTOINCREMENT,
            AddressType INTEGER NOT NULL,
            Address1 TEXT DEFAULT '',
            Address2 TEXT DEFAULT '',
            Address3 TEXT DEFAULT '',
            ClientId INTEGER,
            FOREIGN KEY (ClientId) REFERENCES Clients (ClientId)
        );

        CREATE TABLE IF NOT EXISTS ContactInformation (
            ContactId INTEGER PRIMARY KEY AUTOINCREMENT,
            ContactType INTEGER NOT NULL,
            Number TEXT DEFAULT '',
            ClientId INTEGER,
            FOREIGN KEY (ClientId) REFERENCES Clients (ClientId)
        );";

        using var command = new SQLiteCommand(query, connection);
        command.ExecuteNonQuery();
    }

    public void AddClient(Client client)
    {
        using var connection = new SQLiteConnection(_connectionString);
        connection.Open();

        const string query = @"
                INSERT INTO Clients (FirstName, LastName, Age, Gender, DateOfBirth, Nationality, IdentificationNumber, Occupation)
                VALUES (@FirstName, @LastName, @Age, @Gender, @DateOfBirth, @Nationality, @IdentificationNumber, @Occupation);
            ";

        using var command = new SQLiteCommand(query, connection);
        command.Parameters.AddWithValue("@FirstName", client.FirstName);
        command.Parameters.AddWithValue("@LastName", client.LastName);
        command.Parameters.AddWithValue("@Age", client.Age);
        command.Parameters.AddWithValue("@Gender", client.Gender);
        command.Parameters.AddWithValue("@DateOfBirth", client.DateOfBirth);
        command.Parameters.AddWithValue("@Nationality", client.Nationality);
        command.Parameters.AddWithValue("@IdentificationNumber", client.IdentificationNumber);
        command.Parameters.AddWithValue("@Occupation", client.Occupation);

        command.ExecuteNonQuery();
    }

    public List<Client> GetAllClients()
    {
        var clients = new List<Client>();

        using var connection = new SQLiteConnection(_connectionString);
        connection.Open();

        const string query = "SELECT * FROM Clients;";

        using var command = new SQLiteCommand(query, connection);
        using var reader = command.ExecuteReader();

        while (reader.Read())
        {
            var client = new Client
            {
                ClientId = Convert.ToInt32(reader["ClientId"]),
                FirstName = reader["FirstName"].ToString(),
                LastName = reader["LastName"].ToString(),
                Age = Convert.ToInt32(reader["Age"]),
                Gender = reader["Gender"].ToString(),
                DateOfBirth = reader["DateOfBirth"] == DBNull.Value ? null : Convert.ToDateTime(reader["DateOfBirth"]),
                Nationality = reader["Nationality"].ToString(),
                IdentificationNumber = reader["IdentificationNumber"].ToString(),
                Occupation = reader["Occupation"].ToString()
            };

            clients.Add(client);
        }

        return clients;
    }


    public int UpsertClient(Client client)
    {
        var gotClient = GetClientById(client.ClientId);
        if (gotClient != null)
            UpdateClient(client);
        else
            AddClient(client);

        return client.ClientId;
    }

    public void UpdateClient(Client client)
    {
        using var connection = new SQLiteConnection(_connectionString);
        connection.Open();

        const string query = @"
                UPDATE Clients
                SET FirstName = @FirstName,
                    LastName = @LastName,
                    Age = @Age,
                    Gender = @Gender,
                    DateOfBirth = @DateOfBirth,
                    Nationality = @Nationality,
                    IdentificationNumber = @IdentificationNumber,
                    Occupation = @Occupation
                WHERE ClientId = @ClientId;
            ";

        using var command = new SQLiteCommand(query, connection);
        command.Parameters.AddWithValue("@FirstName", client.FirstName);
        command.Parameters.AddWithValue("@LastName", client.LastName);
        command.Parameters.AddWithValue("@Age", client.Age);
        command.Parameters.AddWithValue("@Gender", client.Gender);
        command.Parameters.AddWithValue("@DateOfBirth", client.DateOfBirth);
        command.Parameters.AddWithValue("@Nationality", client.Nationality);
        command.Parameters.AddWithValue("@IdentificationNumber", client.IdentificationNumber);
        command.Parameters.AddWithValue("@Occupation", client.Occupation);
        command.Parameters.AddWithValue("@ClientId", client.ClientId);

        command.ExecuteNonQuery();
    }

    public void DeleteClient(int clientId)
    {
        using var connection = new SQLiteConnection(_connectionString);
        connection.Open();

        const string query = "DELETE FROM Clients WHERE ClientId = @ClientId;";

        using var command = new SQLiteCommand(query, connection);
        command.Parameters.AddWithValue("@ClientId", clientId);

        command.ExecuteNonQuery();
    }


    public Client GetClientById(int clientId)
    {
        using var connection = new SQLiteConnection(_connectionString);
        connection.Open();

        var query = "SELECT * FROM Clients WHERE ClientId = @ClientId;";

        using var command = new SQLiteCommand(query, connection);
        command.Parameters.AddWithValue("@ClientId", clientId);

        using var reader = command.ExecuteReader();

        if (!reader.Read()) return null; // Client not found
        var client = new Client
        {
            ClientId = Convert.ToInt32(reader["ClientId"]),
            FirstName = reader["FirstName"].ToString(),
            LastName = reader["LastName"].ToString(),
            Age = Convert.ToInt32(reader["Age"]),
            Gender = reader["Gender"].ToString(),
            DateOfBirth = reader["DateOfBirth"] == DBNull.Value ? null : Convert.ToDateTime(reader["DateOfBirth"]),
            Nationality = reader["Nationality"].ToString(),
            IdentificationNumber = reader["IdentificationNumber"].ToString(),
            Occupation = reader["Occupation"].ToString()
        };

        return client;

    }

    public void AddAddress(Address address)
    {
        using var connection = new SQLiteConnection(_connectionString);
        connection.Open();

        var query = @"
        INSERT INTO Addresses (AddressType, Address1, Address2, Address3, ClientId)
        VALUES (@AddressType, @Address1, @Address2, @Address3, @ClientId);
    ";

        using var command = new SQLiteCommand(query, connection);
        command.Parameters.AddWithValue("@AddressType", (int)address.AddressType);
        command.Parameters.AddWithValue("@Address1", address.Address1);
        command.Parameters.AddWithValue("@Address2", address.Address2);
        command.Parameters.AddWithValue("@Address3", address.Address3);
        command.Parameters.AddWithValue("@ClientId", address.ClientId);

        command.ExecuteNonQuery();
    }

    public void AddContactInfo(ContactInfo contactInfo)
    {
        using var connection = new SQLiteConnection(_connectionString);
        connection.Open();

        const string query = @"
        INSERT INTO ContactInformation (ContactType, Number, ClientId)
        VALUES (@ContactType, @Number, @ClientId);
    ";

        using var command = new SQLiteCommand(query, connection);
        command.Parameters.AddWithValue("@ContactType", (int)contactInfo.ContactType);
        command.Parameters.AddWithValue("@Number", contactInfo.Number);
        command.Parameters.AddWithValue("@ClientId", contactInfo.ClientId);

        command.ExecuteNonQuery();
    }

    public void DeleteAddress(int addressId)
    {
        using var connection = new SQLiteConnection(_connectionString);
        connection.Open();

        const string query = "DELETE FROM Addresses WHERE AddressId = @AddressId;";

        using var command = new SQLiteCommand(query, connection);
        command.Parameters.AddWithValue("@AddressId", addressId);

        command.ExecuteNonQuery();
    }

    public void DeleteContactInfo(int contactId)
    {
        using var connection = new SQLiteConnection(_connectionString);
        connection.Open();

        const string query = "DELETE FROM ContactInformation WHERE ContactId = @ContactId;";

        using var command = new SQLiteCommand(query, connection);
        command.Parameters.AddWithValue("@ContactId", contactId);

        command.ExecuteNonQuery();
    }

    public void UpdateAddress(Address address)
    {
        using var connection = new SQLiteConnection(_connectionString);
        connection.Open();

        const string query = @"
        UPDATE Addresses
        SET AddressType = @AddressType,
            Address1 = @Address1,
            Address2 = @Address2,
            Address3 = @Address3,
            ClientId = @ClientId
        WHERE AddressId = @AddressId;
    ";

        using var command = new SQLiteCommand(query, connection);
        command.Parameters.AddWithValue("@AddressType", (int)address.AddressType);
        command.Parameters.AddWithValue("@Address1", address.Address1);
        command.Parameters.AddWithValue("@Address2", address.Address2);
        command.Parameters.AddWithValue("@Address3", address.Address3);
        command.Parameters.AddWithValue("@ClientId", address.ClientId);
        command.Parameters.AddWithValue("@AddressId", address.AddressId);

        command.ExecuteNonQuery();
    }

    public void UpdateContactInfo(ContactInfo contactInfo)
    {
        using var connection = new SQLiteConnection(_connectionString);
        connection.Open();

        const string query = @"
        UPDATE ContactInformation
        SET ContactType = @ContactType,
            Number = @Number,
            ClientId = @ClientId
        WHERE ContactId = @ContactId;
    ";

        using var command = new SQLiteCommand(query, connection);
        command.Parameters.AddWithValue("@ContactType", (int)contactInfo.ContactType);
        command.Parameters.AddWithValue("@Number", contactInfo.Number);
        command.Parameters.AddWithValue("@ClientId", contactInfo.ClientId);
        command.Parameters.AddWithValue("@ContactId", contactInfo.ContactId);

        command.ExecuteNonQuery();
    }
}