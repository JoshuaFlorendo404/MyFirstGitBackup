using System;
using System.Windows.Forms;
using System.Threading.Tasks;
using System.Drawing;
using System.Linq;
using System.IO;
using System.Net.NetworkInformation;
using System.Xml.Linq;
using MySql.Data.MySqlClient;

namespace ContactDB_OOP
{
    public partial class frmContact : Form
    {
        private string connectionString = "Server=localhost;" +
                             "Port=3306;" +
                             "Database=contact_db;" +
                             "User ID=root;" +
                             "Password=;";

        private readonly ContactRepository _contactRepository;
        private readonly GenderRepository _genderRepository;
        private readonly CityRepository _cityRepository;
        private readonly PositionRepository _positionRepository;
        private readonly SimRepository _simRepository;
        private readonly CompanyRepository _companyRepository;

        public frmContact()
        {
            InitializeComponent();
            var dbConnection = new MyCon();

            _contactRepository = new ContactRepository(new MyCon());
            _genderRepository = new GenderRepository(new MyCon());
            _cityRepository = new CityRepository(new MyCon());
            _positionRepository = new PositionRepository(new MyCon());
            _simRepository = new SimRepository(new MyCon());
            _companyRepository = new CompanyRepository(new MyCon());
        }

        private void HideButton()
        {
            btnCANCEL.Enabled = false;
        }

        private async void frmContact_Load(object sender, EventArgs e)
        {
            HideButton();

            await LoadContactsAsync();
            await LoadGendersAsync();
            await LoadCityAsync();
            await LoadPositionsAsync();
            await LoadSimAsync();
            await LoadCompanyAsync();


        }
            /*---------------------------------------------------------METHODS----------------------------------------------------------------*/
        private async Task LoadContactsAsync()
        {
            try
            {
                // Show loading indicator
                Cursor = Cursors.WaitCursor;
                dgvContact.Enabled = false;

                // Configure DataGridView
                ConfigureDataGridView();

                // Get contacts and bind to DataGridView
                var contacts = await _contactRepository.GetAllContactsAsync();
                dgvContact.DataSource = null;
                dgvContact.DataSource = contacts;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading contacts: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                Cursor = Cursors.Default;
                dgvContact.Enabled = true;
            }
        }
        private async Task LoadGendersAsync()
        {
            try
            {
                var genders = await _genderRepository.GetAllGendersAsync();

                cmbGender.DataSource = genders.ToList();
                cmbGender.ValueMember = "gender_id";
                cmbGender.DisplayMember = "gender_name";
                cmbGender.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading genders: {ex.Message}",
                              "Error",
                              MessageBoxButtons.OK,
                              MessageBoxIcon.Error);
            }
        }


        private async Task LoadCityAsync()
        {
            try
            {
                var cities = await _cityRepository.GetAllCitiesAsync();

                cmbCity.DataSource = cities.ToList();
                cmbCity.ValueMember = "city_id";
                cmbCity.DisplayMember = "city_name";
                cmbCity.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading genders: {ex.Message}",
                              "Error",
                              MessageBoxButtons.OK,
                              MessageBoxIcon.Error);
            }
        }

        private async Task LoadPositionsAsync()
        {
            try
            {
                var positions = await _positionRepository.GetAllPositionsAsync();

                cmbPosition.DataSource = positions.ToList(); // Ensure it's a list
                cmbPosition.DisplayMember = "position_name";
                cmbPosition.ValueMember = "position_id";
                cmbPosition.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading positions: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }



        private async Task LoadSimAsync()
        {
            try
            {
                var sims = await _simRepository.GetAllSimsAsync();

                cmbSimCard.DataSource = sims.ToList();
                cmbSimCard.ValueMember = "sim_id";
                cmbSimCard.DisplayMember = "sim_name";
                cmbSimCard.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading genders: {ex.Message}",
                              "Error",
                              MessageBoxButtons.OK,
                              MessageBoxIcon.Error);
            }
        }



        private async Task LoadCompanyAsync()
        {
            try
            {
                var sims = await _companyRepository.GetAllCompaniesAsync();

                cmbCompany.DataSource = sims.ToList();
                cmbCompany.ValueMember = "company_id";
                cmbCompany.DisplayMember = "company_name";
                cmbCompany.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading genders: {ex.Message}",
                              "Error",
                              MessageBoxButtons.OK,
                              MessageBoxIcon.Error);
            }
        }

        private void ConfigureDataGridView()
        {
            dgvContact.AutoGenerateColumns = false;
            dgvContact.AllowUserToAddRows = false;
            dgvContact.AllowUserToDeleteRows = false;
            dgvContact.ReadOnly = true;
            dgvContact.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvContact.MultiSelect = false;
            dgvContact.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;

            // Clear existing columns
            dgvContact.Columns.Clear();

            // Add columns
            dgvContact.Columns.AddRange(
                new DataGridViewTextBoxColumn
                {
                    Name = "colUserId",
                    DataPropertyName = "user_id",
                    HeaderText = "ID",
                    Width = 30
                },
                new DataGridViewTextBoxColumn
                {
                    Name = "colUserName",
                    DataPropertyName = "user_name",
                    HeaderText = "Name",
                    Width = 250
                },
                new DataGridViewTextBoxColumn
                {
                    Name = "colBday",
                    DataPropertyName = "bday",
                    HeaderText = "Birthday",
                    Width = 100,
                    DefaultCellStyle = { Format = "dd/MM/yyyy" }
                },
                new DataGridViewTextBoxColumn
                {
                    Name = "colCityId",
                    DataPropertyName = "city_id",
                    HeaderText = "City ID",
                    Width = 80
                },
                new DataGridViewTextBoxColumn
                {
                    Name = "colCompanyId",
                    DataPropertyName = "company_id",
                    HeaderText = "Company ID",
                    Width = 100
                },
                new DataGridViewTextBoxColumn
                {
                    Name = "colPositionId",
                    DataPropertyName = "position_id",
                    HeaderText = "Position ID",
                    Width = 100
                },
                new DataGridViewTextBoxColumn
                {
                    Name = "colGenderId",
                    DataPropertyName = "gender_id",
                    HeaderText = "Gender ID",
                    Width = 80
                },
                new DataGridViewTextBoxColumn
                {
                    Name = "colSimId",
                    DataPropertyName = "sim_id",
                    HeaderText = "SIM ID",
                    Width = 80
                },
                new DataGridViewTextBoxColumn
                {
                    Name = "colContactImage",
                    DataPropertyName = "contact_image",
                    HeaderText = "Image Path",
                    Width = 150
                }
            );

            // Optional: Style the header
            dgvContact.EnableHeadersVisualStyles = false;
            dgvContact.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(45, 66, 91);
            dgvContact.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvContact.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            dgvContact.ColumnHeadersHeight = 30;

            // Optional: Style the grid
            dgvContact.BackgroundColor = Color.White;
            dgvContact.BorderStyle = BorderStyle.None;
            dgvContact.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgvContact.GridColor = Color.FromArgb(224, 224, 224);

            // Optional: Style the rows
            dgvContact.RowTemplate.Height = 25;
            dgvContact.DefaultCellStyle.Font = new Font("Segoe UI", 9F);
            dgvContact.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(247, 247, 247);
        }
        // Helper method to set ComboBox values
        private void SetComboBoxValue(ComboBox comboBox, int value)
        {
            try
            {
                // Find the item in the ComboBox where the ValueMember equals the provided value
                for (int i = 0; i < comboBox.Items.Count; i++)
                {
                    var item = comboBox.Items[i];
                    var propertyInfo = item.GetType().GetProperty(comboBox.ValueMember);
                    if (propertyInfo != null)
                    {
                        var itemValue = Convert.ToInt32(propertyInfo.GetValue(item, null));
                        if (itemValue == value)
                        {
                            comboBox.SelectedIndex = i;
                            break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error setting ComboBox value: {ex.Message}",
                               "Error",
                               MessageBoxButtons.OK,
                               MessageBoxIcon.Error);
            }
        }
        private void GetImage()
        {
                //openFileDialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.gif;*.bmp";
                openFileDialog1.Filter = "Image File|*.png";
                openFileDialog1.Title = "Select Contact Image";

                if (openFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    txtImageFile.Text = openFileDialog1.FileName;
                    ShowImage();
            }
        }

        private void ShowImage()
        {
            try
            {
                if (string.IsNullOrEmpty(txtImageFile.Text))
                {
                    pbxImage.Image = null;
                    return;
                }

                if (File.Exists(txtImageFile.Text))
                {
                    using (var stream = new FileStream(txtImageFile.Text, FileMode.Open, FileAccess.Read))
                    {
                        pbxImage.Image?.Dispose(); // Dispose of the old image
                        pbxImage.Image = Image.FromStream(stream);
                    }
                    // Set the PictureBox size mode
                    pbxImage.SizeMode = PictureBoxSizeMode.Zoom;
                }
                else
                {
                    pbxImage.Image = null;
                    MessageBox.Show("Image file not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading image: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                pbxImage.Image = null;
            }
        }
        // Handle text changes in txtImageFile
        private void txtImageFile_TextChanged(object sender, EventArgs e)
        {
            
        }

        // Optional: Add this if you have a refresh button
        private async void Refresh()
        {
            await LoadContactsAsync();
        }


        private async void AddContact()
        {
            try
            {
                // Validate required fields
                if (string.IsNullOrWhiteSpace(txtName.Text))
                {
                    MessageBox.Show("Please enter a name.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Validate combo box selections
                if (cmbGender.SelectedValue == null ||
                    cmbCity.SelectedValue == null ||
                    cmbPosition.SelectedValue == null ||
                    cmbSimCard.SelectedValue == null ||
                    cmbCompany.SelectedValue == null)
                {
                    MessageBox.Show("Please select all required fields.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Collect data from input fields
                var name = txtName.Text.Trim();
                var genderId = (int)cmbGender.SelectedValue;
                var cityId = (int)cmbCity.SelectedValue;
                var positionId = (int)cmbPosition.SelectedValue;
                var simId = (int)cmbSimCard.SelectedValue;
                var companyId = (int)cmbCompany.SelectedValue;
                var birthday = dtpBday.Value;
                var contactImage = txtImageFile.Text.Trim();

                // Check if the contact image field is empty (optional)
                if (string.IsNullOrWhiteSpace(contactImage))
                {
                    contactImage = null;  // or set a default value
                }


                // Create SQL insert command using MySqlParameters
                string query = "INSERT INTO tbl_contact (user_name, gender_id, city_id, position_id, sim_id, company_id, bday, contact_image) " +
                               "VALUES (@user_name, @gender_id, @city_id, @position_id, @sim_id, @company_id, @bday, @contact_image)";

                using (MySqlConnection connection = new MySqlConnection(connectionString))  // Use class-level connectionString
                {
                    await connection.OpenAsync();
                    using (MySqlCommand cmd = new MySqlCommand(query, connection))
                    {
                        // Add parameters to the command
                        cmd.Parameters.AddWithValue("@user_name", name);
                        cmd.Parameters.AddWithValue("@gender_id", genderId);
                        cmd.Parameters.AddWithValue("@city_id", cityId);
                        cmd.Parameters.AddWithValue("@position_id", positionId);
                        cmd.Parameters.AddWithValue("@sim_id", simId);
                        cmd.Parameters.AddWithValue("@company_id", companyId);
                        cmd.Parameters.AddWithValue("@bday", birthday);
                        cmd.Parameters.AddWithValue("@contact_image", contactImage);

                        // Execute the query
                        int rowsAffected = await cmd.ExecuteNonQueryAsync();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Contact added successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            ClearForm(); // Method to clear the form
                            await LoadContactsAsync(); // Refresh contact list
                        }
                        else
                        {
                            MessageBox.Show("Failed to add contact.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Log and display error
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void ClearForm()
        {
            txtID.Clear();
            txtName.Clear();
            txtImageFile.Clear();
            cmbGender.SelectedIndex = -1;
            cmbCity.SelectedIndex = -1;
            cmbPosition.SelectedIndex = -1;
            cmbSimCard.SelectedIndex = -1;
            cmbCompany.SelectedIndex = -1;
            dtpBday.Value = DateTime.Now;
            pbxImage.Image = null;
            txtSearch.Clear();
        }



        private async void UpdateContact()//HINDI PA NAGAGAWA
        {
            // Collect data from input fields
            int userId;
            if (!int.TryParse(txtID.Text, out userId))
            {
                MessageBox.Show("Please enter a valid User ID.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return; // Exit the method if the User ID is invalid
            }

            var contact = new Contact
            {
                user_id = userId,
                user_name = txtName.Text,
                bday = dtpBday.Value,
                city_id = (int)cmbCity.SelectedValue,
                company_id = (int)cmbCompany.SelectedValue,
                position_id = (int)cmbPosition.SelectedValue,
                gender_id = (int)cmbGender.SelectedValue,
                sim_id = (int)cmbSimCard.SelectedValue,
                contact_image = txtImageFile.Text
            };

            // Call the repository method to update the contact
            var success = await _contactRepository.UpdateContactAsync(contact);

            if (success)
            {
                MessageBox.Show("Contact updated successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                await LoadContactsAsync(); // Refresh the contact list
            }
            else
            {
                MessageBox.Show("Error updating contact.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private async void DeleteContact()
        {
            int userId;
            if (!int.TryParse(txtID.Text, out userId))
            {
                MessageBox.Show("Please enter a valid User ID.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return; // Exit the method if the User ID is invalid
            }

            // Confirmation dialog
            var confirmResult = MessageBox.Show("Are you sure you want to delete this contact?",
                                                 "Confirm Delete",
                                                 MessageBoxButtons.YesNo,
                                                 MessageBoxIcon.Warning);
            if (confirmResult == DialogResult.No)
            {
                return; // Exit the method if the user chooses not to delete
            }

            // Call the repository method to delete the contact
            var success = await _contactRepository.DeleteContactAsync(userId);

            if (success)
            {
                MessageBox.Show("Contact deleted successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                await LoadContactsAsync(); // Refresh the contact list
            }
            else
            {
                MessageBox.Show("Error deleting contact.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void SearchbyName()
        {
            string userName = txtSearch.Text.Trim(); // Get the username from the TextBox

            if (string.IsNullOrEmpty(userName))
            {
                MessageBox.Show("Please enter a username to search.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return; // Exit if the input is empty
            }

            // Call the repository method to search for contacts by username
            var searchResults = await _contactRepository.SearchContactsByUserNameAsync(userName);

            if (searchResults != null && searchResults.Any())
            {

                dgvContact.DataSource = searchResults.ToList(); // Bind the results to the DataGridView
            }
            else
            {
                MessageBox.Show("No contacts found with that username.", "Search Result", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        /*---------------------------------------------------------EVENTS----------------------------------------------------------------*/

        private void btnGetImage_Click(object sender, EventArgs e)
        {
            GetImage();
        }


        private void btnRefresh_Click(object sender, EventArgs e)//WORKING
        {
            Refresh();
            MessageBox.Show("you click Refresh", "Hello", MessageBoxButtons.OK,MessageBoxIcon.Information);
        }

        private void btnADD_Click(object sender, EventArgs e)
        {
            if(btnADD.Text == "UPDATE")
            {
                UpdateContact();
                btnADD.Text = "ADD";
                pbxImage.Image = null;
            }
            else
            {
                AddContact();
                pbxImage.Image = null;
            }
            
        }

        private void btnCLEAR_Click(object sender, EventArgs e)//WORKING
        {
            ClearForm();
            MessageBox.Show("Clear");
        }

        private void btnDELETE_Click(object sender, EventArgs e)//WORKING
        {
            DeleteContact();
            HideButton();
            ClearForm();
            btnADD.Text = "ADD";
        }

        private void dgvContact_CellClick_1(object sender, DataGridViewCellEventArgs e)
        {
            btnCANCEL.Enabled = true;
            btnADD.Text = "UPDATE"; // sets the text in button to UPDATE
            try
            {
                if (e.RowIndex >= 0) // Make sure a valid row is clicked
                {
                    DataGridViewRow row = dgvContact.Rows[e.RowIndex];

                    // Populate text boxes
                    txtID.Text = row.Cells["colUserId"].Value?.ToString();
                    txtName.Text = row.Cells["colUserName"].Value?.ToString();
                    txtImageFile.Text = row.Cells["colContactImage"].Value.ToString();

                    // Set DateTimePicker value with validation
                    var bdayValue = row.Cells["colBday"].Value;
                    if (bdayValue != null && bdayValue != DBNull.Value)
                    {
                        if (DateTime.TryParse(bdayValue.ToString(), out DateTime bday))
                        {
                            // Check if the date is within valid range
                            if (bday >= dtpBday.MinDate && bday <= dtpBday.MaxDate)
                            {
                                dtpBday.Value = bday;
                            }
                            else
                            {
                                // If date is out of range, set to current date or MinDate
                                dtpBday.Value = DateTime.Now;
                            }
                        }
                    }

                    // Set ComboBox selections
                    if (row.Cells["colGenderId"].Value != null)
                    {
                        int genderId = Convert.ToInt32(row.Cells["colGenderId"].Value);
                        SetComboBoxValue(cmbGender, genderId);
                    }

                    if (row.Cells["colCityId"].Value != null)
                    {
                        int cityId = Convert.ToInt32(row.Cells["colCityId"].Value);
                        SetComboBoxValue(cmbCity, cityId);
                    }

                    if (row.Cells["colPositionId"].Value != null)
                    {
                        int positionId = Convert.ToInt32(row.Cells["colPositionId"].Value);
                        SetComboBoxValue(cmbPosition, positionId);
                    }

                    if (row.Cells["colSimId"].Value != null)
                    {
                        int simId = Convert.ToInt32(row.Cells["colSimId"].Value);
                        SetComboBoxValue(cmbSimCard, simId);
                    }

                    if (row.Cells["colCompanyId"].Value != null)
                    {
                        int companyId = Convert.ToInt32(row.Cells["colCompanyId"].Value);
                        SetComboBoxValue(cmbCompany, companyId);
                    }

                    // Show the image if available
                    ShowImage();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading contact details: {ex.Message}",
                               "Error",
                               MessageBoxButtons.OK,
                               MessageBoxIcon.Error);
            }
        }

        private void btnCANCEL_Click(object sender, EventArgs e)
        {
            ClearForm();
            Refresh();
            btnCANCEL.Enabled = false;
            txtID.Text = "";
            btnADD.Text = "ADD";

            
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            SearchbyName();
        }
    }
}
