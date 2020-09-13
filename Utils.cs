using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Data.SqlClient;

namespace Clinic_Management_System
{
    public class Utils
    {
        public static string hashPassword(string password)
        {
            SHA1CryptoServiceProvider sha1 = new SHA1CryptoServiceProvider();

            byte[] password_bytes = Encoding.ASCII.GetBytes(password);
            byte[] encrypted_bytes = sha1.ComputeHash(password_bytes);
            return Convert.ToBase64String(encrypted_bytes);
        }

        public static Dictionary<int, string> getSlots()
        {
            Dictionary<int, string> slots = new Dictionary<int, string>();
            slots.Add(1, "Montignac Diyeti");
            slots.Add(2, "Vejeteryan Diyeti");
            slots.Add(3, "Akdeniz Diyeti");
            slots.Add(4, "Glisemik İndeks Diyeti");
            slots.Add(5, "5:2 Diyeti (The Fast Diet)");
            slots.Add(6, "8 Saat Diyeti (The 8 Hour Diet)");
            slots.Add(7, "Özel Diyet -1");
            slots.Add(8, "Özel Diyet -2");
            slots.Add(9, "Özel Diyet -3");
            slots.Add(10, "Özel Diyet -4");
            return slots;
        }

        public static void createAdmin(string password)
        {
            SqlConnection con = new SqlConnection(Properties.Resources.connectionString);
            SqlCommand command = con.CreateCommand();

            command.CommandText = "INSERT INTO [user] (user_username, user_password) VALUES (@username, @password)";
            command.Parameters.AddWithValue("@username", "admin");
            command.Parameters.AddWithValue("@password", hashPassword(password));

            con.Open();

            try
            {
                command.ExecuteNonQuery();
            }
            catch(Exception)
            {

            }

            con.Close();
        }
    }
}
