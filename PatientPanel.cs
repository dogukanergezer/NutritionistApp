using System;
using System.Windows.Forms;

namespace Clinic_Management_System
{
    public partial class PatientPanel : Form
    {
        int account_id;
        public PatientPanel(int id)
        {
            InitializeComponent();
            account_id = id;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Hide();
            EditProfile editProfile = new EditProfile(account_id);
            editProfile.ShowDialog();
            Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Hide();
            ViewReservations viewReservations = new ViewReservations(account_id);
            viewReservations.ShowDialog();
            Show();
        }
    }
}
