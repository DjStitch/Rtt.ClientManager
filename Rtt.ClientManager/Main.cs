using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using Rtt.ClientManager.RttServiceReference;
using Rtt.ClientManager.Shared;

namespace Rtt.ClientManager
{
    public partial class Main : Form, IDisposable
    {
        private readonly Service1Client _client;

        public Main()
        {
            InitializeComponent();
            _client = new Service1Client();
        }

        private  void LoadClients()
        {
            // Show the progress bar
            try
            {
                progressBar.Visible = true;
                progressBar.Value = 0;
                lblStatus.Text = @"Loading Client Please wait";
                //Application.DoEvents();

                // Call the WCF service asynchronously to retrieve client data
                //var clients = await Task.Run(() =>  _client.GetAllClients());

                var clients = _client.GetAllClients();

                // Clear existing items and columns in the ListView
                listViewClients.Items.Clear();
                listViewClients.Columns.Clear();

                // Add column names to the ListView
                listViewClients.Columns.Add("First Name", 200);
                listViewClients.Columns.Add("Last Name");
                listViewClients.Columns.Add("Age");
                listViewClients.Columns.Add("Gender");
                listViewClients.Columns.Add("Date of Birth");
                listViewClients.Columns.Add("Nationality");
                listViewClients.Columns.Add("Identification Number");
                listViewClients.Columns.Add("Occupation");

                // Set column widths individually
                listViewClients.Columns[0].Width = 100; // First Name
                listViewClients.Columns[1].Width = 100; // Last Name
                listViewClients.Columns[2].Width = 50; // Age
                listViewClients.Columns[3].Width = 70; // Gender
                listViewClients.Columns[4].Width = 100; // Date of Birth
                listViewClients.Columns[5].Width = 100; // Nationality
                listViewClients.Columns[6].Width = 120; // Identification Number
                listViewClients.Columns[7].Width = 100; // Occupation
                // Populate the ListView control
                foreach (var client in clients)
                {
                    var item = new ListViewItem(client.FirstName);
                    item.SubItems.Add(client.LastName);
                    item.SubItems.Add(client.Age.ToString());
                    item.SubItems.Add(client.Gender);
                    item.SubItems.Add(client.DateOfBirth?.ToString("yyyy-MM-dd"));
                    item.SubItems.Add(client.Nationality);
                    item.SubItems.Add(client.IdentificationNumber);
                    item.SubItems.Add(client.Occupation);
                    item.Tag = client.ClientId;

                    listViewClients.Items.Add(item);
                }

                // Auto-resize columns to fit the data text size
                //listViewClients.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);

                // Select the first item in the list
                if (listViewClients.Items.Count > 0)
                {
                    listViewClients.Items[0].Selected = true;
                    listViewClients.Select();
                }

                // Hide the progress bar
                progressBar.Visible = false;
                lblStatus.Text = @"Ready";
                Application.DoEvents();
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while creating the client: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                progressBar.Visible = false;
                lblStatus.Text = @"Error, Please check if WS is running";
                
                Application.DoEvents();

            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Create an instance of the generated client proxy
            using (var client = new Service1Client())
            {
                _client.CreateDatabase();
            }
        }

        private void Main_Load(object sender, EventArgs e)
        {
            LoadClients();
        }

        private void bRefreash_Click(object sender, EventArgs e)
        {
            LoadClients();
        }

        private void listViewClients_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listViewClients.SelectedItems.Count <= 0) return;

            // Get the selected client
            var selectedItem = listViewClients.SelectedItems[0];
            var clientId = GetClientIdFromSelectedListViewItem(selectedItem);

            // Call the WCF service to retrieve the client's details
            var clientDetails = _client.GetClientDetails(clientId);

            if (clientDetails == null) return;

            // Set the Date of Birth using the MaskedTextBox
            if (clientDetails.DateOfBirth.HasValue)
            {
                var formattedDate = clientDetails.DateOfBirth.Value.ToString("MM/dd/yyyy");
                textBoxDateOfBirth.Text = formattedDate;
            }
            else
            {
                textBoxDateOfBirth.Clear();
            }

            // Display the client's details
            textBoxFirstName.Text = clientDetails.FirstName;
            textBoxLastName.Text = clientDetails.LastName;
            textBoxAge.Text = clientDetails.Age.ToString();
            textBoxGender.Text = clientDetails.Gender;
            textBoxDateOfBirth.Text = clientDetails.DateOfBirth?.ToString("yyyy-MM-dd");
            textBoxNationality.Text = clientDetails.Nationality;
            textBoxIdentificationNumber.Text = clientDetails.IdentificationNumber;
            textBoxOccupation.Text = clientDetails.Occupation;

            // Display the client's addresses
            listViewAddresses.Items.Clear();
            foreach (var address in clientDetails.Addresses)
            {
                var item = new ListViewItem(address.Address1);
                item.SubItems.Add(address.Address2);
                item.SubItems.Add(address.Address3);
                listViewAddresses.Items.Add(item);
            }

            if (listViewAddresses.Items.Count == 0)
            {
                var item = new ListViewItem("No Address");
                item.SubItems.Add("xxxx");
                item.SubItems.Add("xxxxx");
                listViewAddresses.Items.Add(item);
            }

            // Display the client's contact information
            listViewContactInfo.Items.Clear();
            foreach (var contactInfo in clientDetails.ContactInformation)
            {
                var item = new ListViewItem(contactInfo.ContactType.ToString());
                item.SubItems.Add(contactInfo.Number);
                listViewContactInfo.Items.Add(item);
            }

            if (listViewContactInfo.Items.Count == 0)
            {
                var item = new ListViewItem(ContactType.Cell.ToString());
                item.SubItems.Add("No Number");
                listViewContactInfo.Items.Add(item);
            }
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {
        }

        private void label7_Click(object sender, EventArgs e)
        {
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (listViewClients.SelectedItems.Count <= 0) return;
            // Get the selected client
            var selectedItem = listViewClients.SelectedItems[0];
            var firstName = selectedItem.SubItems[0].Text;
            var lastName = selectedItem.SubItems[1].Text;
            var age = Convert.ToInt32(selectedItem.SubItems[2].Text);
            var gender = selectedItem.SubItems[3].Text;
            var dateOfBirth = Convert.ToDateTime(selectedItem.SubItems[4].Text);

            var nationality = selectedItem.SubItems[5].Text;
            var identificationNumber = selectedItem.SubItems[6].Text;
            var occupation = selectedItem.SubItems[7].Text;

            // Update the client in the database
            var clientId = GetClientIdFromSelectedListViewItem(selectedItem);
            var updClient = MapDetails(clientId);
            UpdateClientInDatabase(updClient);

            // Display a success message or perform any other necessary actions
            MessageBox.Show(@"Client updated successfully.");
            LoadClients();
        }

        private void UpdateClientInDatabase(Client client)
        {
            // Call the WCF service to update the client in the database
            _client.UpdateClient(client);
        }

        private int GetClientIdFromSelectedListViewItem(ListViewItem item)
        {
            // Retrieve the client ID from the Tag property of the ListViewItem
            if (item.Tag is int clientId) return clientId;

            return -1;
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (listViewClients.SelectedItems.Count <= 0) return;
            // Get the selected client
            var selectedItem = listViewClients.SelectedItems[0];
            var clientId = GetClientIdFromSelectedListViewItem(selectedItem);

            // Confirm the deletion with the user
            var result = MessageBox.Show(@"Are you sure you want to delete this client?", @"Confirmation",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result != DialogResult.Yes) return;
            // Delete the client from the database

            var updClient = MapDetails(clientId);
            DeleteClientFromDatabase(updClient);

            // Remove the client from the ListView
            listViewClients.Items.Remove(selectedItem);

            // Clear the details at the bottom
            ClearDetails();

            // Display a success message or perform any other necessary actions
            MessageBox.Show(@"Client deleted successfully.");
            LoadClients();
        }

        private Client MapDetails(int id)
        {
            var newClient = new Client
            {
                ClientId = id,
                FirstName = textBoxFirstName.Text,
                LastName = textBoxLastName.Text,
                Age = int.Parse(textBoxAge.Text),
                Gender = textBoxGender.Text,
                DateOfBirth = ParseDateTime(textBoxDateOfBirth.Text),
                Nationality = textBoxNationality.Text,
                IdentificationNumber = textBoxIdentificationNumber.Text,
                Occupation = textBoxOccupation.Text,
                Addresses = new List<Address>(),
                ContactInformation = new List<ContactInfo>()
            };

            return newClient;
        }

        private DateTime? ParseDateTime(string date)
        {
            if (DateTime.TryParse(date, out var parsedDate)) return parsedDate;
            return null;
        }

        private void DeleteClientFromDatabase(Client client)
        {
            // Call the WCF service to delete the client from the database
            _client.DeleteClient(client);
        }

        private void ClearDetails()
        {
            // Clear the details at the bottom
            textBoxFirstName.Text = string.Empty;
            textBoxLastName.Text = string.Empty;
            textBoxAge.Text = string.Empty;
            textBoxGender.Text = string.Empty;
            textBoxDateOfBirth.Text = string.Empty;
            textBoxNationality.Text = string.Empty;
            textBoxIdentificationNumber.Text = string.Empty;
            textBoxOccupation.Text = string.Empty;
        }

        private void btnCreate_Click(object sender, EventArgs e)
        {

            lblStatus.Text = @"Ready";

            try
            {
                if (ReferenceEquals(btnCreate.Tag, "1"))
                {
                    ClearDetails();
                    btnUpdate.Enabled = false;
                    bRefreash.Enabled = false;
                    btnDelete.Enabled = false;

                    btnCreate.Text = @"SAVE NOW";

                    MessageBox.Show(@"Please input the clients details in fields");
                    btnCreate.Tag = "2";
                    return;
                }

                // Prompt for address details


                // Get the input values for the new client
                var firstName = textBoxFirstName.Text;
                var lastName = textBoxLastName.Text;
                //var age = int.Parse(textBoxAge.Text);
                var gender = textBoxGender.Text;
                DateTime? dateOfBirth = null;
                if (!string.IsNullOrEmpty(textBoxDateOfBirth.Text)) dateOfBirth = DateTime.Parse(textBoxDateOfBirth.Text);
                var nationality = textBoxNationality.Text;
                var identificationNumber = textBoxIdentificationNumber.Text;
                var occupation = textBoxOccupation.Text;


                if (!int.TryParse(textBoxAge.Text, out var age) || age < 1 || age > 120)
                {
                    MessageBox.Show(@"Please enter a valid age between 1 and 120.");
                    textBoxAge.Focus();
                    return;
                }

                var address1 = PromptForInput("Enter Address 1:");
                if (string.IsNullOrWhiteSpace(address1))
                {
                    // User canceled the address input, abort client creation
                    btnCreate.Enabled = true;
                    bRefreash.Enabled = true;
                    btnDelete.Enabled = true;
                    btnUpdate.Text = "Update";
                    return;
                }

                // Prompt for contact details
                var contact = PromptForInput("Enter Contact:");
                if (string.IsNullOrWhiteSpace(contact))
                {
                    // User canceled the contact input, abort client creation
                    btnCreate.Enabled = true;
                    bRefreash.Enabled = true;
                    btnDelete.Enabled = true;
                    btnUpdate.Text = "Update";
                    return;
                }

                var newCleint = MapDetails(listViewClients.Items.Count + 1);

                if (address1.Length > 0)
                {
                    newCleint.Addresses.Add(new Address
                    {
                        AddressType= AddressType.Home,
                        Address1 = address1,
                        ClientId = newCleint.ClientId
                    });
                }

                if (contact.Length > 0)
                {
                    newCleint.ContactInformation.Add(new ContactInfo
                    {
                        ContactType = ContactType.Cell,
                        Number = contact,
                        ClientId = newCleint.ClientId
                    });
                }


        // Create the new client in the database
        var clientId = CreateClientInDatabase(newCleint);
                // Create a new ListViewItem for the new client




                var newItem = new ListViewItem(new[]
                {
                    firstName,
                    lastName,
                    age.ToString(),
                    gender,
                    dateOfBirth?.ToString("yyyy-MM-dd"),
                    nationality,
                    identificationNumber,
                    occupation
                });


                // Set the Tag property of the ListViewItem to store the client ID
                newItem.Tag = newCleint.ClientId;

                // Add the new ListViewItem to the ListView
                listViewClients.Items.Add(newItem);

                // Clear the input fields
                ClearDetails();

                btnCreate.Tag = "1";
                // Display a success message or perform any other necessary actions
                MessageBox.Show(@"Client created successfully.");
                btnUpdate.Enabled = true;
                bRefreash.Enabled = true;
                btnDelete.Enabled = true;
                btnCreate.Text = @"Create Client";
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while creating the client: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                progressBar.Visible = false;
                lblStatus.Text = @"Error";

                Application.DoEvents();

            }
        }

        private string PromptForInput(string prompt)
        {
            var inputBox = new TextBox();
            var promptLabel = new Label();
            var okButton = new Button();
            var cancelButton = new Button();

            // Set up the input box, prompt label, and buttons
            inputBox.Dock = DockStyle.Fill;
            promptLabel.Text = prompt;
            promptLabel.Dock = DockStyle.Top;
            okButton.Text = "OK";
            cancelButton.Text = "Cancel";
            okButton.DialogResult = DialogResult.OK;
            cancelButton.DialogResult = DialogResult.Cancel;

            // Create a panel to host the input box, prompt label, and buttons
            var panel = new TableLayoutPanel();
            panel.Dock = DockStyle.Fill;
            panel.RowCount = 2;
            panel.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            panel.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            panel.Controls.Add(inputBox, 0, 0);
            panel.Controls.Add(promptLabel, 0, 1);
            panel.Controls.Add(okButton, 0, 2);
            panel.Controls.Add(cancelButton, 1, 2);

            // Create a form to host the panel
            var form = new Form();
            form.Text = "Enter Input";
            form.FormBorderStyle = FormBorderStyle.FixedDialog;
            form.MinimizeBox = false;
            form.MaximizeBox = false;
            form.StartPosition = FormStartPosition.CenterParent;
            form.ClientSize = new Size(300, 120);
            form.Controls.Add(panel);

            // Show the input form as a dialog
            var result = form.ShowDialog();

            if (result == DialogResult.OK)
            {
                return inputBox.Text;
            }
            else
            {
                return null;
            }
        }

        private int CreateClientInDatabase(Client creClient)
        {
            var CClient = _client.CreateClient(creClient);

            return 0;
        }
    }
}