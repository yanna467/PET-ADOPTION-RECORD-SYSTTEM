using System;
using System.Data;
using System.Data.OleDb;
using System.Windows.Forms;

namespace Database_Connection
{
    public partial class Form1 : Form
    {
        private readonly string connectionString =
        @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\Users\acer\OneDrive\Documents\Week7_Activity.accdb";

        OleDbConnection conn;
        OleDbDataAdapter da;
        DataSet ds;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        // CONNECTION TEST
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                using (var conn = new OleDbConnection(connectionString))
                {
                    conn.Open();
                    MessageBox.Show("Connected Successfully!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Connection Failed: " + ex.Message);
            }
        }

        // LOAD BUTTON
        private void loadBtn_Click(object sender, EventArgs e)
        {
            LoadData();
            ClearFields();
        }

        // LOAD DATA METHOD
        private void LoadData()
        {
            try
            {
                conn = new OleDbConnection(connectionString);
                da = new OleDbDataAdapter("SELECT * FROM StudentQuery ", conn);
                ds = new DataSet();

                conn.Open();
                da.Fill(ds, "StudentQuery");

                dataGridView1.DataSource = ds.Tables["StudentQuery"];

                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Load Failed: " + ex.Message);
            }
        }

        // INSERT
        private void Insert_Click(object sender, EventArgs e)
        {
            if (textBox2.Text == "" || textBox3.Text == "" || coursetbx.Text == "")
            {
                MessageBox.Show("Please fill all fields first.");
                return;
            }

            string query = "INSERT INTO Student (LastName, FirstName, YearLevel, Course) VALUES (?,?,?,?)";

            try
            {
                using (var conn = new OleDbConnection(connectionString))
                using (var cmd = new OleDbCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("?", textBox2.Text);
                    cmd.Parameters.AddWithValue("?", textBox3.Text);
                    cmd.Parameters.AddWithValue("?", GetYearLevel());
                    cmd.Parameters.AddWithValue("?", coursetbx.Text);

                    conn.Open();
                    cmd.ExecuteNonQuery();

                    MessageBox.Show("Student Added Successfully!");

                    LoadData();
                    ClearFields();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Insert Failed: " + ex.Message);
            }
        }

        // DELETE
        private void Delete_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(textBox1.Text, out int id))
            {
                MessageBox.Show("Select a student first.");
                return;
            }

            DialogResult confirm = MessageBox.Show(
                "Are you sure you want to delete this student?",
                "Confirm Delete",
                MessageBoxButtons.YesNo);

            if (confirm == DialogResult.No) return;

            string query = "DELETE FROM Student WHERE StudentID=?";

            try
            {
                using (var conn = new OleDbConnection(connectionString))
                using (var cmd = new OleDbCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("?", id);

                    conn.Open();
                    cmd.ExecuteNonQuery();

                    MessageBox.Show("Student Deleted!");

                    LoadData();
                    ClearFields();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Delete Failed: " + ex.Message);
            }
        }

        // UPDATE
        private void Update_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(textBox1.Text, out int id))
            {
                MessageBox.Show("Invalid Student ID.");
                return;
            }

            string query = "UPDATE Student SET LastName=?, FirstName=?, YearLevel=?, Course=? WHERE StudentID=?";

            try
            {
                using (var conn = new OleDbConnection(connectionString))
                using (var cmd = new OleDbCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("?", textBox2.Text);
                    cmd.Parameters.AddWithValue("?", textBox3.Text);
                    cmd.Parameters.AddWithValue("?", GetYearLevel());
                    cmd.Parameters.AddWithValue("?", coursetbx.Text);
                    cmd.Parameters.AddWithValue("?", id);

                    conn.Open();
                    cmd.ExecuteNonQuery();

                    MessageBox.Show("Student Updated!");

                    LoadData();
                    ClearFields();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Update Failed: " + ex.Message);
            }
        }

        // REFRESH
        private void Refreshbtn_Click(object sender, EventArgs e)
        {
            LoadData();
            ClearFields();
            MessageBox.Show("Data Refreshed!");
        }

        // GET YEAR LEVEL
        private int GetYearLevel()
        {
            if (radioButton1.Checked) return 1;
            if (radioButton2.Checked) return 2;
            if (radioButton3.Checked) return 3;
            if (radioButton4.Checked) return 4;

            return 0;
        }

        // CLEAR FIELDS
        private void ClearFields()
        {
            textBox1.Clear();
            textBox2.Clear();
            textBox3.Clear();
            coursetbx.Clear();

            radioButton1.Checked = false;
            radioButton2.Checked = false;
            radioButton3.Checked = false;
            radioButton4.Checked = false;
        }

        // CLICK ROW
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            DataGridViewRow row = dataGridView1.Rows[e.RowIndex];

            textBox1.Text = row.Cells[0].Value.ToString();
            textBox2.Text = row.Cells[1].Value.ToString();
            textBox3.Text = row.Cells[2].Value.ToString();

            int year = Convert.ToInt32(row.Cells[3].Value);

            radioButton1.Checked = year == 1;
            radioButton2.Checked = year == 2;
            radioButton3.Checked = year == 3;
            radioButton4.Checked = year == 4;

            coursetbx.Text = row.Cells[4].Value.ToString();
        }

        // EMPTY EVENTS (fix designer errors)

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e) { }
        private void textBox3_TextChanged(object sender, EventArgs e) { }
        private void groupBox1_Enter(object sender, EventArgs e) { }
        private void textBox5_TextChanged(object sender, EventArgs e) { }
        private void label4_Click(object sender, EventArgs e) { }
        private void radioButton2_CheckedChanged(object sender, EventArgs e) { }
    }
}