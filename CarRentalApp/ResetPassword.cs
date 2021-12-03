using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CarRentalApp
{
    public partial class ResetPassword : Form
    {
        private readonly Car_RentalsEntities1 car_RentalsEntities1;
        private User _user;
        public ResetPassword(User user)
        {
            InitializeComponent();
            car_RentalsEntities1 = new Car_RentalsEntities1();
            _user = user;
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            try
            {

                var password = tbNew.Text;
                var confirm_password = tbConfirm.Text;
                var user = car_RentalsEntities1.Users.FirstOrDefault(
                    n => n.id == _user.id);
                if (password != confirm_password)
                {
                    MessageBox.Show("Passwords do not match. Please try again!");
                }
                _user.password = Utils.HashPassword(password);
                car_RentalsEntities1.SaveChanges();

                MessageBox.Show("Password was reset successfully");
                Close();
            }
            catch (Exception)
            {

                MessageBox.Show("An Error had occured. Please try Again!");

            }
        }
    }
}
