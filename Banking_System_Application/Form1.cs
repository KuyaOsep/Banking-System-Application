using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using Org.BouncyCastle.Asn1.Esf;

namespace Banking_System_Application
{
    public partial class Form1 : Form
    {
        MySqlConnection connection;
        MySqlCommand command;
        dbConnection connectionClass = new dbConnection();
        MySqlDataReader read;

        string gender;
        int toEnable = 1;
        int currentBalance = 0;

        public Form1()
        {
            InitializeComponent();

            connection = new MySqlConnection(connectionClass.dbConnect());
        }

        public void clearRegisterField()
        {
            textBox1.Text = string.Empty;
            textBox2.Text = string.Empty;
            textBox3.Text = string.Empty;
            dateTimePicker1.Value = DateTime.Today;
            radioButton1.Checked = false;
            radioButton2.Checked = false;
        }

        public void clearUpdateField()
        {
            textBox5.Text = string.Empty;
            textBox6.Text = string.Empty;
            textBox7.Text = string.Empty;
            dateTimePicker2.Value = DateTime.Today;
            radioButton4.Checked = false;
            radioButton3.Checked = false;
        }

        public void clearSlipField()
        {
            textBox9.Clear();
            textBox10.Clear();
            textBox11.Clear();
            textBox12.Clear();
            textBox13.Clear();
        }

        public void LoadRecord()
        {
            dataGridView1.Rows.Clear();
            connection.Open(); 

            command = new MySqlCommand("SELECT account_number, account_name, gender, birthday, address " +
                "FROM bank_accounts", connection);
            read = command.ExecuteReader();
            while (read.Read())
            {
                dataGridView1.Rows.Add(dataGridView1.Rows.Count + 1, read["account_number"].ToString(), 
                    read["account_name"].ToString(), read["gender"].ToString(), read["birthday"].ToString(), 
                    read["address"].ToString());
            }
            read.Close();
            connection.Close();
        }

        public void LoadTransaction()
        {
            dataGridView2.Rows.Clear();
            connection.Open();

            command = new MySqlCommand("SELECT transaction_date, account_number, account_name, current_balance, " +
                "withdrawal, deposit, updated_balance FROM transactions", connection);
            read = command.ExecuteReader(); 
            while (read.Read()) 
            {
                dataGridView2.Rows.Add(dataGridView2.Rows.Count + 1, read["transaction_date"].ToString(), 
                    read["account_number"].ToString(), read["account_name"].ToString(), read["current_balance"].ToString(), 
                    read["withdrawal"].ToString(), read["deposit"].ToString(), read["updated_balance"].ToString());
            }
            read.Close();   
            connection.Close(); 
        }

        public void LoadRecordOffline()
        {
            dataGridView1.Rows.Clear();

            command = new MySqlCommand("SELECT account_number, account_name, gender, birthday, address " +
                "FROM bank_accounts", connection);
            read = command.ExecuteReader();
            while (read.Read())
            {
                dataGridView1.Rows.Add(dataGridView1.Rows.Count + 1, read["account_number"].ToString(), 
                    read["account_name"].ToString(), read["gender"].ToString(), read["birthday"].ToString(), 
                    read["address"].ToString());
            }
            read.Close();
        }

        public void clearWithdrawSlipField()
        {
            textBox17.Clear();
            textBox16.Clear();
            textBox15.Clear();
            textBox14.Clear();
        }

        public bool deposit()
        {
            groupBox4.Visible = true;
            return true;
        }

        public bool withdraw()
        {
            groupBox4.Visible = true;
            return true;
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            LoadRecord(); 
            LoadTransaction();
        }

        private void tabPage1_Click(object sender, EventArgs e)
        {
            
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            clearRegisterField();
        }

        private void button4_Click(object sender, EventArgs e) 
        {

            if (dataGridView1.CurrentRow.Cells[3].Value.ToString() == "Male")
            {
                radioButton3.Checked = true;
            }
            else
            {
                radioButton4.Checked = true;
            }
            textBox5.Text = (dataGridView1.CurrentRow.Cells[1].Value.ToString());
            textBox6.Text = (dataGridView1.CurrentRow.Cells[2].Value.ToString());
            dateTimePicker2.Text = (dataGridView1.CurrentRow.Cells[4].Value.ToString());
            textBox7.Text = (dataGridView1.CurrentRow.Cells[5].Value.ToString());
        } 

        private void textBox5_TextChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)  
        {

            if ((textBox1.Text == string.Empty) || (textBox2.Text == string.Empty) || (gender == string.Empty) || 
                (dateTimePicker1.Value == DateTime.Today) || (textBox3.Text == string.Empty))
            {
                MessageBox.Show("Please complete all fields");
                return;
            }
            else
            {
                if (radioButton1.Checked == true)
                {
                    gender = "Male";
                }
                else if (radioButton2.Checked == false)
                {
                    gender = "Female";
                }
            }
            try
            {
                DateTime dateToSet = dateTimePicker1.Value; 
                connection.Open();
                command = new MySqlCommand("INSERT INTO bank_accounts (account_number, account_name, gender, " +
                    "birthday, address) VALUES (@number, @name, @gender, @birthday, @address)", connection);
                command.Parameters.Clear();
                command.Parameters.AddWithValue("@number", textBox1.Text);
                command.Parameters.AddWithValue("@name", textBox2.Text);
                command.Parameters.AddWithValue("@gender", gender);
                command.Parameters.AddWithValue("@birthday", dateToSet);
                command.Parameters.AddWithValue("@address", textBox3.Text);

                toEnable = command.ExecuteNonQuery();

                if (toEnable > 0)
                {
                    MessageBox.Show("Success!");
                }
                else
                {
                    clearRegisterField();
                }
            }
            catch
            {
                MessageBox.Show("Account Already Exist");
            }
            connection.Close();
            LoadRecord();
            clearRegisterField();
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void textBox4_TextChanged(object sender, EventArgs e) 
        {
            dataGridView1.Rows.Clear();
            connection.Open();
            command = new MySqlCommand("SELECT account_number, account_name, gender, birthday, address FROM " +
                "bank_accounts WHERE account_number LIKE '%" + textBox4.Text + "%' OR account_name LIKE '%" + 
                textBox4.Text + "%' OR gender LIKE '%" + textBox4.Text + "%' OR birthday LIKE '%" + textBox4.Text + 
                "%' OR address LIKE '%" + textBox4.Text + "%'", connection);
            read = command.ExecuteReader();
            while (read.Read())
            {
                dataGridView1.Rows.Add(dataGridView1.Rows.Count + 1, read["account_number"].ToString(), 
                    read["account_name"].ToString(), read["gender"].ToString(), read["birthday"].ToString(), 
                    read["address"].ToString());
            }
            read.Close();
            connection.Close();
        }

        private void button5_Click(object sender, EventArgs e) 
        {
            DialogResult result = MessageBox.Show("Do you want to delete this account?", "Delete Account Confirmation", 
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            
            if (result == DialogResult.Yes)
            {
                connection.Open();
                command = new MySqlCommand("DELETE from bank_accounts WHERE account_name=@name", connection);
                command.Parameters.AddWithValue("@name", textBox4.Text);

                toEnable = command.ExecuteNonQuery();
                if (toEnable > 0)
                {
                    MessageBox.Show("Successfully deleted the account");
                }
                else
                {
                    MessageBox.Show("Such account does not exist");
                }
                connection.Close();
                textBox4.Clear();
            }
            else
            {
                MessageBox.Show("Account Deletion Cancelled");
            }
        }

        private void button6_Click(object sender, EventArgs e) 
        {
            try
            {
                if (radioButton4.Checked == true)
                {
                    gender = "Male";
                }
                else if (radioButton3.Checked == true)
                {
                    gender = "Female";
                }

                DateTime dateToUpdate = dateTimePicker2.Value;

                connection.Open();

                command = new MySqlCommand("UPDATE bank_accounts SET account_number=@number, " +
                    "account_name=@name, gender=@gender, birthday=@birthday, address=@address " +
                    "WHERE account_name=@change", connection);

                command.Parameters.Clear();
                command.Parameters.AddWithValue("@change", textBox4.Text);
                command.Parameters.AddWithValue("@number", textBox5.Text);
                command.Parameters.AddWithValue("@name", textBox6.Text);
                command.Parameters.AddWithValue("@gender", gender);
                command.Parameters.AddWithValue("@birthday", dateToUpdate);
                command.Parameters.AddWithValue("@address", textBox7.Text);

                toEnable = command.ExecuteNonQuery();

                if (toEnable > 0 )
                {
                    MessageBox.Show("Successfully updated!");
                }
                else
                {
                    MessageBox.Show("Failed to update. Record not found or No changes were made.");
                }
            }
            catch (Exception errorShow)
            {
                MessageBox.Show("Error: " + errorShow.Message);
            }
            finally
            {
                connection.Close();
                LoadRecord();
                clearUpdateField();
                textBox4.Clear();
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            dataGridView1.CellClick += dataGridView1_CellContentClick;
            if (e.RowIndex >= 0 && e.RowIndex < dataGridView1.RowCount && e.ColumnIndex >= 0 && e.ColumnIndex < 
                dataGridView1.Columns.Count)
            {
                DataGridViewCell selectedCell = dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex];
                textBox4.Text = selectedCell.Value.ToString();
            }
        }

        private void button9_Click(object sender, EventArgs e) 
        {
            dataGridView1.Rows.Clear();

            connection.Open();

            command = new MySqlCommand("SELECT bank_accounts.account_number, bank_accounts.account_name, transactions.updated_balance " +
                "FROM bank_accounts " +
                "LEFT JOIN transactions " +
                "ON bank_accounts.account_number " +
                "WHERE bank_accounts.account_number = '" + textBox9.Text + "'", connection);

            read = command.ExecuteReader();

            while (read.Read())
            {
                textBox10.Text = read["account_number"].ToString();
                textBox11.Text = read["account_name"].ToString();
                textBox12.Text = read["updated_balance"].ToString();
            }
            read.Close();
            connection.Close();
            textBox9.Clear(); 
        }

        private void button7_Click(object sender, EventArgs e) 
        {
            groupBox4.Visible = true;
        }

        private void button8_Click(object sender, EventArgs e) 
        {
            groupBox5.Visible = true;
        }

        private void button11_Click(object sender, EventArgs e) 
        {
            clearSlipField();
        }

        private void textBox11_TextChanged(object sender, EventArgs e)
        {

        }

        private MySqlConnection GetConnection()
        {
            return connection;
        }

        private void button10_Click(object sender, EventArgs e, MySqlConnection connection) 
        {
            
        }

        private void tabPage2_Click(object sender, EventArgs e)
        {
            LoadTransaction();
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void button10_Click(object sender, EventArgs e) 
        {
            connection.Open();
            MySqlTransaction transaction;


            try
            {
                command = new MySqlCommand("INSERT INTO transactions (transaction_date, account_number, account_name, " +
                "current_balance, deposit) VALUES (@transac, @accno, @accname, @cbalance, " +
                " @deposit)", connection);

                command.Parameters.Clear();
                command.Parameters.AddWithValue("@transac", DateTime.Now);
                command.Parameters.AddWithValue("@accno", textBox10.Text);
                command.Parameters.AddWithValue("@accname", textBox11.Text);
                command.Parameters.AddWithValue("@cbalance", textBox12.Text);
                command.Parameters.AddWithValue("@deposit", textBox13.Text);
                toEnable = command.ExecuteNonQuery();

                command = new MySqlCommand("UPDATE transactions SET updated_balance = current_balance + @deposit " +
                    "WHERE account_number = @accno", connection);
                command.Parameters.AddWithValue("@deposit", textBox13.Text);
                command.Parameters.AddWithValue("@accno", textBox10.Text);
                toEnable = command.ExecuteNonQuery();

                

                if (toEnable > 0)
                {
                    MessageBox.Show("Success");

                }
                else
                {
                    clearSlipField();
                }
            } 
            catch (Exception ex)
            {
                MessageBox.Show("Error" + ex);
            } 
            finally
            {
                connection.Close();
                LoadTransaction();
                clearSlipField();
                groupBox4.Visible = false;
            }
            
        }

        private void button13_Click(object sender, EventArgs e)
        {
            connection.Open();

            try
            {
                command = new MySqlCommand("INSERT INTO transactions (transaction_date, account_number, account_name, " +
                "current_balance, withdrawal) VALUES (@transac, @accno, @accname, @cbalance, " +
                " @withdraw)", connection);

                command.Parameters.Clear();
                command.Parameters.AddWithValue("@transac", DateTime.Now);
                command.Parameters.AddWithValue("@accno", textBox17.Text);
                command.Parameters.AddWithValue("@accname", textBox16.Text);
                command.Parameters.AddWithValue("@cbalance", textBox15.Text);
                command.Parameters.AddWithValue("@withdraw", textBox14.Text);
                toEnable = command.ExecuteNonQuery();

                command = new MySqlCommand("UPDATE transactions SET updated_balance = current_balance - @withdraw " +
                    "WHERE account_number = @accno", connection);
                command.Parameters.AddWithValue("@withdraw", textBox14.Text);
                command.Parameters.AddWithValue("@accno", textBox17.Text);
                toEnable = command.ExecuteNonQuery();



                if (toEnable > 0)
                {
                    MessageBox.Show("Success");

                }
                else
                {
                    clearSlipField();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error" + ex);
            }
            finally
            {
                connection.Close();
                LoadTransaction();
                clearWithdrawSlipField();
                groupBox5.Visible = false;
            }
        }

        private void button14_Click(object sender, EventArgs e) 
        {
            dataGridView1.Rows.Clear();

            connection.Open();

            command = new MySqlCommand("SELECT bank_accounts.account_number, bank_accounts.account_name, transactions.updated_balance " +
                "FROM bank_accounts " +
                "LEFT JOIN transactions " +
                "ON bank_accounts.account_number " +
                "WHERE bank_accounts.account_number = '" + textBox18.Text + "'", connection);

            read = command.ExecuteReader();

            while (read.Read())
            {
                textBox17.Text = read["account_number"].ToString();
                textBox16.Text = read["account_name"].ToString();
                textBox15.Text = read["updated_balance"].ToString();
            }
            read.Close();
            connection.Close();
            textBox18.Clear();
        }

        private void label17_Click(object sender, EventArgs e)
        {

        }

        private void textBox15_TextChanged(object sender, EventArgs e)
        {

        }

        private void groupBox4_Enter(object sender, EventArgs e)
        {

        }
    }
}
