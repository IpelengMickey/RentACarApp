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
    public partial class ManageUsers : Form
    {
        private readonly Car_RentalsEntities1 car_RentalsEntities1;
        public ManageUsers()
        {
            InitializeComponent();
            car_RentalsEntities1 = new Car_RentalsEntities1();
        }

        private void btnAddUser_Click(object sender, EventArgs e)
        {
            if(!Utils.FormIsOpen("AddUser"))
            {
                var addUser = new AddUser(this);
                addUser.MdiParent = this.MdiParent;
                addUser.Show();
            }
        }

        private void btnResetPassword_Click(object sender, EventArgs e)
        {
            try
            {
                //Get Id of selected row
                var id = (int)gvUserList.SelectedRows[0].Cells["id"].Value;

                //query database for record
                var user = car_RentalsEntities1.Users.FirstOrDefault(n => n.id == id);
               
                var hashed_password =Utils.DefaultHashedPassword();
                user.password = hashed_password;
                car_RentalsEntities1.SaveChanges();

                MessageBox.Show($"{user.username}'s Password has been reset!");
                PopulateGrid();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }

        private void btnDeactivateUser_Click(object sender, EventArgs e)
        {
            try
            {
                //Get Id of selected row
                var id = (int)gvUserList.SelectedRows[0].Cells["id"].Value;

                //query database for record
                var user = car_RentalsEntities1.Users.FirstOrDefault(n => n.id == id);
                //if (user.isActive == true)
                //    user.isActive = false;
                //else
                //    user.isActive = true;
                user.isActive = user.isActive == true ? false : true;
                car_RentalsEntities1.SaveChanges();

                MessageBox.Show($"{user.username}'s Active Satus has changed");
                PopulateGrid();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }

        private void ManageUsers_Load(object sender, EventArgs e)
        {
            PopulateGrid();
        }

        public void PopulateGrid()
        {
            var users = car_RentalsEntities1.Users
                .Select(n => new
                {
                    n.id,
                    n.username,
                    n.UserRoles.FirstOrDefault().Role.name,
                    n.isActive
                })
                .ToList();
            gvUserList.DataSource = users;
            gvUserList.Columns["username"].HeaderText = "Username";
            gvUserList.Columns["name"].HeaderText = "Role Name";
            gvUserList.Columns["isActive"].HeaderText = "Active";

            gvUserList.Columns["id"].Visible = false;
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            PopulateGrid();
        }
    }
}
