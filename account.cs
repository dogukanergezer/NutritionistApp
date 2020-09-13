namespace Clinic_Management_System
{
    public class Account
    {
        int id, type;
        string name;

        public Account(int id, string name, int type)
        {
            this.id = id;//
            this.name = name;
            this.type = type;
        }

        public override string ToString()
        {
            string account_type = null;
            if (type == 0)
                account_type = "Secretary";
            else if (type == 1)
                account_type = "Doctor";
            else if (type == 2)
                account_type = "Patient";

            return account_type + ":" + id.ToString() + " - " + name;
        }

        public int getID() { return id; }
    }
}
