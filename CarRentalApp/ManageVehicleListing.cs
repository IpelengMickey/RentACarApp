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
    public partial class ManageVehicleListing : Form
    {
        
        private readonly Car_RentalsEntities1 car_RentalsEntities1;
        public ManageVehicleListing()
        {
            InitializeComponent();
            car_RentalsEntities1 = new Car_RentalsEntities1();
        }

        private void ManageVehicleListing_Load(object sender, EventArgs e)
        {
           try
            {
                PopulateGrid();


            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }

        }

        // New Function to populateGrid. Can be callled anytime we need a grid refresh
        public void PopulateGrid()
        {
            //Select Id as CarId, names as CarName from TypesOfCars
            var cars = car_RentalsEntities1.TypesOfCars
                .Select(n => new
                {
                    Make = n.Make,
                    Model = n.Model,
                    VIN = n.VIN,
                    Year = n.Year,
                    LicensePlateNumber = n.LicensePlateNumber,
                    Id = n.Id
                })
                    .ToList(); //Specify the columns you want to display from the table
            gvCarList.DataSource = cars;
            gvCarList.Columns[4].HeaderText = "License Plate Number";
            gvCarList.Columns["Id"].Visible = false;
        }

        private void btnAddCar_Click(object sender, EventArgs e)
        {
            var addEditVehicle = new AddEditVehicle(this);
            addEditVehicle.ShowDialog();
            addEditVehicle.MdiParent = this.MdiParent;
        }

        private void btnEdditCar_Click(object sender, EventArgs e)
        {
            try
            {
                //Get Id of selected row
                var id = (int)gvCarList.SelectedRows[0].Cells["Id"].Value;

                //query database for record
                var car = car_RentalsEntities1.TypesOfCars.FirstOrDefault(n => n.Id == id);

                //launch AddEdditVehicle window with data
                Form[] children = this.MdiChildren;
                var query = children.Select(c => c).Where(c => c is AddEditVehicle).ToList();
                if (query.Count == 0)
                {
                    var addEditVehicle = new AddEditVehicle(car, this);
                    addEditVehicle.MdiParent = this.MdiParent;
                    addEditVehicle.Show();
                }
                    
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }   

        private void btnDeleteCar_Click(object sender, EventArgs e)
        {
            try
            {
                var id = (int)gvCarList.SelectedRows[0].Cells["Id"].Value;

                var car = car_RentalsEntities1.TypesOfCars.FirstOrDefault(n => n.Id == id);

                DialogResult dr = MessageBox.Show("Are You Sure You Want To Delete This Record",
                    "Delete", MessageBoxButtons.YesNoCancel,
                    MessageBoxIcon.Warning);

                if(dr == DialogResult.Yes)
                {
                    //delete vehicle from table
                    car_RentalsEntities1.TypesOfCars.Remove(car);
                    car_RentalsEntities1.SaveChanges();
                }
                PopulateGrid();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            PopulateGrid();
            gvCarList.Update();
            gvCarList.Refresh();
        }

        private void gvCarList_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
