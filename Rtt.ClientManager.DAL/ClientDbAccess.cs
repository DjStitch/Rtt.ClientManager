using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using Rtt.ClientManager.Shared;

namespace Rtt.ClientManager.DAL;

public class ClientDbAccess
{
    private string _connectionString;

    public void SetDbFile(string dbName = "RttDataBase.db")
    {
        const string path = @"C:\RTT";

        if (!Directory.Exists(path)) Directory.CreateDirectory(path);

        var filePath = Path.Combine(path, dbName);
        _connectionString = $"Data Source={filePath};Version=3;";
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

        SeedDb();
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

    private void SeedDb()
    {

        using var connection = new SQLiteConnection(_connectionString);
        connection.Open();

        const string query = "SELECT * FROM Clients;";

        using var command1 = new SQLiteCommand(query, connection);
        using var reader = command1.ExecuteReader();

        if (reader.HasRows != false) return;
        const string sql = @"INSERT INTO Clients (ClientId, FirstName, LastName, Age, Gender, DateOfBirth, Nationality, IdentificationNumber, Occupation)
                VALUES
                    (1, 'Siya', 'Kolisi', 30, 'Male', '1991-06-16', 'South African', 'SA001', 'Rugby Player'),
                    (2, 'Francois', 'Pienaar', 54, 'Male', '1967-01-02', 'South African', 'SA002', 'Rugby Player'),
                    (3, 'Bryan', 'Habana', 38, 'Male', '1983-06-12', 'South African', 'SA003', 'Rugby Player'),
                    (4, 'Joel', 'Stransky', 54, 'Male', '1967-07-16', 'South African', 'SA004', 'Rugby Player'),
                    (5, 'Faf', 'de Klerk', 29, 'Male', '1991-10-19', 'South African', 'SA005', 'Rugby Player'),
                    (6, 'Morné', 'Steyn', 37, 'Male', '1984-07-11', 'South African', 'SA006', 'Rugby Player'),
                    (7, 'Victor', 'Matfield', 44, 'Male', '1977-05-11', 'South African', 'SA007', 'Rugby Player'),
                    (8, 'Schalk', 'Burger', 38, 'Male', '1983-04-13', 'South African', 'SA008', 'Rugby Player'),
                    (9, 'Bakkies', 'Botha', 42, 'Male', '1979-09-22', 'South African', 'SA009', 'Rugby Player'),
                    (10, 'John', 'Smit', 44, 'Male', '1978-04-03', 'South African', 'SA010', 'Rugby Player'),
                    (11, 'Fourie', 'du Preez', 39, 'Male', '1982-03-24', 'South African', 'SA011', 'Rugby Player'),
                    (12, 'Jannie', 'du Plessis', 38, 'Male', '1982-05-16', 'South African', 'SA012', 'Rugby Player'),
                    (13, 'Tendai', 'Mtawarira', 36, 'Male', '1985-08-01', 'South African', 'SA013', 'Rugby Player'),
                    (14, 'Pieter-Steph', 'du Toit', 28, 'Male', '1992-08-20', 'South African', 'SA014', 'Rugby Player'),
                    (15, 'Herschel', 'Jantjies', 25, 'Male', '1995-04-22', 'South African', 'SA015', 'Rugby Player'),
                    (16, 'Handré', 'Pollard', 27, 'Male', '1994-03-11', 'South African', 'SA016', 'Rugby Player'),
                    (17, 'Aphiwe', 'Dyantyi', 26, 'Male', '1994-08-02', 'South African', 'SA017', 'Rugby Player'),
                    (18, 'Beast', 'Mtawarira', 36, 'Male', '1985-06-08', 'South African', 'SA018', 'Rugby Player'),
                    (19, 'Duane', 'Vermeulen', 35, 'Male', '1986-07-03', 'South African', 'SA019', 'Rugby Player'),
                    (20, 'Breyton', 'Paulse', 45, 'Male', '1976-07-28', 'South African', 'SA020', 'Rugby Player');
                ";

        using var command2 = new SQLiteCommand(sql, connection);
        command2.ExecuteNonQuery();
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