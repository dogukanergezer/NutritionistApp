using System;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Clinic_Management_System
{
    public partial class AdminPanel : Form
    {
        public AdminPanel()
        {
            InitializeComponent();
        }

        private void updateList(string query)
        {
            SqlConnection con = new SqlConnection(Properties.Resources.connectionString);
            SqlCommand command = con.CreateCommand();
            con.Open();
            command.CommandText = "SELECT account_id, account_name, account_type FROM account WHERE account_type in (0, 1, 2) AND (account_name LIKE @query OR account_phone LIKE @query) ORDER BY account_type";
            command.Parameters.AddWithValue("@query", query + "%");

            SqlDataReader reader = command.ExecuteReader();

            listBox1.Items.Clear();
            while (reader.Read())
                listBox1.Items.Add(new Account(reader.GetInt32(0), reader.GetString(1), reader.GetInt32(2)));

            con.Close();
        }

        private void AdminPanel_Load(object sender, EventArgs e)
        {
            updateList("");
        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {
            updateList(textBox5.Text);
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            int account_id;
            try
            {
                account_id = ((Account)listBox1.SelectedItem).getID();
            }
            catch (Exception)
            {
                return;
            }

            SqlConnection con = new SqlConnection(Properties.Resources.connectionString);
            SqlCommand command = con.CreateCommand();
            command.CommandText = "SELECT user_username, account_name, account_dob, account_phone, account_type, account_notes, account_creation_date FROM [user], account WHERE user_id = account_user_id AND account_id=@id";
            command.Parameters.AddWithValue("@id", account_id);
            con.Open();

            SqlDataReader reader = command.ExecuteReader();
            if (reader.Read())
            {
                textBox6.Text = account_id.ToString();
                textBox7.Text = reader.GetValue(0).ToString();
                textBox8.Text = reader.GetValue(1).ToString();
                textBox9.Text = reader.GetValue(2).ToString();
                textBox10.Text = reader.GetValue(3).ToString();

                if (reader.GetInt32(4) == 0)
                    textBox11.Text = "Secretary";
                else if (reader.GetInt32(4) == 1)
                    textBox11.Text = "Secretary";
                else if (reader.GetInt32(4) == 2)
                    textBox11.Text = "Patient";

                textBox12.Text = reader.GetValue(5).ToString();
                textBox13.Text = reader.GetValue(6).ToString();
            }

            con.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!validateInputs())
            {
                MessageBox.Show("Lütfen giriş alanını kontrol ediniz!");
                return;
            }

            SqlConnection con = new SqlConnection(Properties.Resources.connectionString);
            SqlCommand command = con.CreateCommand();
            command.CommandText = "INSERT INTO [user] (user_username, user_password) VALUES(@username, @password)";
            command.Parameters.AddWithValue("@username", textBox1.Text);
            command.Parameters.AddWithValue("@password", Utils.hashPassword(textBox2.Text));
            con.Open();
            if (command.ExecuteNonQuery() > 0)
            {
                //We created the record in user table
                command.CommandText = "SELECT user_id FROM [user] WHERE user_username = @username";
                int user_id = (int)command.ExecuteScalar();

                command.CommandText = "INSERT INTO account (account_user_id, account_name, account_type, account_notes, account_creation_date) VALUES (@user_id, @name, @type, @notes, @date)";
                command.Parameters.Clear();
                command.Parameters.AddWithValue("@user_id", user_id);
                command.Parameters.AddWithValue("@name", textBox3.Text);
                command.Parameters.AddWithValue("@type", comboBox1.SelectedIndex);
                command.Parameters.AddWithValue("@notes", textBox4.Text);
                command.Parameters.AddWithValue("@date", DateTime.Now);

                if (command.ExecuteNonQuery() > 0)
                {
                    //All good => Account created!
                    MessageBox.Show("Hesap başarıyla oluşturuldu");
                }
                else
                    MessageBox.Show("Hesap oluşturulurken bir hata meydana geldi!");
            }
            else
                MessageBox.Show("Error while creating the account!");
            con.Close();
            updateList("");
        }

        private bool validateInputs()
        {
            if (textBox1.Text == "" || textBox2.Text == "" || textBox3.Text == "")
                return false;

            if (comboBox1.SelectedIndex < 0)
                return false;

            return true;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (textBox7.Text == "")
                return;

            SqlConnection con = new SqlConnection(Properties.Resources.connectionString);
            SqlCommand command = con.CreateCommand();
            command.CommandText = "DELETE FROM [user] WHERE user_username=@username";
            command.Parameters.AddWithValue("@username", textBox7.Text);
            con.Open();
            if (command.ExecuteNonQuery() > 0)
                MessageBox.Show("Hesap silindi");
            else
                MessageBox.Show("Hesap silinemedi!");
            con.Close();
            updateList("");
            textBox6.Clear();
            textBox7.Clear();
            textBox8.Clear();
            textBox9.Clear();
            textBox10.Clear();
            textBox11.Clear();
            textBox12.Clear();
            textBox13.Clear();
        }
    }
}
