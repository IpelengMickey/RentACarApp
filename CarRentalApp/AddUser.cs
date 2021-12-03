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
    public partial class AddUser : Form
    {
        private readonly Car_RentalsEntities1 car_RentalsEntities1;
        public AddUser()
        {
            InitializeComponent();
            car_RentalsEntities1 = new Car_RentalsEntities1();
        }

        private void AddUser_Load(object sender, EventArgs e)
        {
            var roles = car_RentalsEntities1.Roles.ToList();
            cbRoles.DataSource = roles;
            cbRoles.ValueMember = "id";
            cbRoles.DisplayMember = "name";
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                var username = tbUsername.Text;
                var roleId = (int)cbRoles.SelectedValue;
                var password = Utils.DefaultHashedPassword();
                var user = new User
                {
                    username = username,
                    password = password,
                    isActive = true
                };
                car_RentalsEntities1.Users.Add(user);
                car_RentalsEntities1.SaveChanges();

                var userId = user.id;
                var userRole = new UserRole
                {
                    roleid = roleId,
                    userid = userId
                };
                car_RentalsEntities1.UserRoles.Add(userRole);
                car_RentalsEntities1.SaveChanges();

                MessageBox.Show("New User Added Successfully");
                Close();
            }
            catch (Exception)
            {

                MessageBox.Show("An Error Has Occured");
            }

        }
    }
}
