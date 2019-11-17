using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormSQL
{
    public partial class MainWindow : Form
    {
        private string connString = "Data Source = (LocalDB)\\MSSQLLocalDB; AttachDbFilename = C:\\Users\\Patryk\\source\\repos\\WindowsFormSQL\\WindowsFormSQL\\Measurements.mdf; Integrated Security = True";
        //string connString = ""; DODAĆ NA LAPTOPIE INNE;
        private Boolean isListBox = true;

        public MainWindow()
        {
            InitializeComponent();
            dataGridView.Visible = false;
            loadlist();

        }

        private void insertButton_Click(object sender, EventArgs e)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection())
                {
                    conn.ConnectionString = connString;
                    conn.Open();
                    SqlCommand command = new SqlCommand();
                    command.CommandText = "INSERT INTO Measurements (value, comment, time) VALUES ((@value), (@comment), (@time) )";

                    DateTime dateTimeVar = DateTime.Now;
                    command.Parameters.AddWithValue("@value", valueTextBox.Text);
                    command.Parameters.AddWithValue("@comment", commentTextBox.Text);
                    command.Parameters.AddWithValue("@time", dateTimeVar);

                    command.Connection = conn;
                    command.ExecuteNonQuery();
                    conn.Close();
                    loadlist();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }


        }
        private void deleteButton_Click(object sender, EventArgs e)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection())
                {
                    conn.ConnectionString = connString;
                    conn.Open();
                    SqlCommand command = new SqlCommand();
                    command.CommandText = "DELETE FROM Measurements WHERE Id = @id";
                    
                    command.Parameters.AddWithValue("@id", idListBox.SelectedItem);

                    command.Connection = conn;
                    command.ExecuteNonQuery();
                    conn.Close();
                    loadlist();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }
        }
        private void updateButton_Click(object sender, EventArgs e)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection())
                {
                    conn.ConnectionString = connString;
                    conn.Open();
                    SqlCommand command = new SqlCommand();
                    command.CommandText = "UPDATE Measurements SET value = @value, comment = @comment, time = @time WHERE Id = @id";
                    DateTime dateTimeVar = DateTime.Now;
                    command.Parameters.AddWithValue("@time", dateTimeVar);
                    command.Parameters.AddWithValue("@id", idListBox.SelectedItem);
                    command.Parameters.AddWithValue("@value", valueTextBox.Text);
                    command.Parameters.AddWithValue("@comment", commentTextBox.Text);

                    command.Connection = conn;
                    command.ExecuteNonQuery();
                    conn.Close();
                    loadlist();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }

        }
    
        private void loadlist()
        {
            if (isListBox)
            {
                idListBox.Items.Clear();
                timeListBox.Items.Clear();
                valueListBox.Items.Clear();
                commentListBox.Items.Clear();

                using (SqlConnection conn = new SqlConnection())
                {
                    conn.ConnectionString = connString;
                    conn.Open();
                    SqlCommand command = new SqlCommand("SELECT * FROM Measurements", conn);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            idListBox.Items.Add(reader[0].ToString());
                            timeListBox.Items.Add(reader[1].ToString());
                            valueListBox.Items.Add(reader[2].ToString());
                            commentListBox.Items.Add(reader[3].ToString());

                        }
                    }
                    conn.Close();
                }
            }
            else
            {
                SqlConnection conn = new SqlConnection();
                SqlCommand sqlCmd = new SqlCommand("SELECT * FROM Measurements", conn);
                try
                {
                    if (conn.State == ConnectionState.Closed)
                    {
                        conn.ConnectionString = connString;
                        conn.Open();
                    }
                    SqlDataAdapter sda = new SqlDataAdapter();
                    sda.SelectCommand = sqlCmd;
                    DataTable dbdataset = new DataTable();
                    sda.Fill(dbdataset);
                    BindingSource bSource = new BindingSource();

                    bSource.DataSource = dbdataset;
                    dataGridView.DataSource = dbdataset;
                    sda.Update(dbdataset);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error 2");
                }
                finally
                {
                    conn.Close();
                }
            }

        }

        private void MainWindow_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'measurementsDataSet.Measurements' table. You can move, or remove it, as needed.
            this.measurementsTableAdapter.Fill(this.measurementsDataSet.Measurements);

        }

        private void listBoxesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            isListBox = true;
            loadlist();
            dataGridView.Visible = false;

            valueTextBox.Enabled  = true;
            valueLabel.Enabled  = true;
            valueTextBox.Enabled  = true;

            commentLabel.Enabled  = true;
            commentListBox.Visible = true;
            commentTextBox.Enabled  = true;

            insertButton.Enabled  = true;
            updateButton.Enabled  = true;
            deleteButton.Enabled  = true;

            timeListBox.Visible = true;
            valueListBox.Visible = true;
            idListBox.Visible = true;
        }

        private void dataGridViewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            isListBox = false;
            loadlist();
            dataGridView.Visible = true;
            valueTextBox.Enabled  = false;
            valueLabel.Enabled  = false;
            valueTextBox.Enabled  = false;

            commentLabel.Enabled  = false;
            commentListBox.Visible = false;
            commentTextBox.Enabled  = false;

            insertButton.Enabled  = false;
            updateButton.Enabled  = false;
            deleteButton.Enabled  = false;

            timeListBox.Visible = false;
            valueListBox.Visible = false;
            idListBox.Visible = false;
        }
    }
}
