using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CarRentalApp
{
    public partial class Login : Form
    {
        private readonly Car_RentalsEntities1 car_RentalsEntities1;
        public Login()
        {
            InitializeComponent();
            car_RentalsEntities1 = new  Car_RentalsEntities1();

        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            try
            {
                SHA256 sha = SHA256.Create();

                var username = tbUserName.Text.Trim();
                var password = tbPassword.Text;


                var hashed_password = Utils.HashPassword(password);

                //Check for matching username, password and active flag
                var user = car_RentalsEntities1.Users.FirstOrDefault(q => q.username == username && q.password == hashed_password 
                            && q.isActive == true);
                if(user == null)
                {
                    MessageBox.Show("Please provide valid credentials");

                }
                else
                {
                    var mainWindow = new MainWindow(this, user );
                    mainWindow.Show();
                    Hide();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Something went wrong. Please try again");
            }
        }
    }
}
