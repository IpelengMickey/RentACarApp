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
    public partial class ManageRentalRecords : Form
    {
        private readonly Car_RentalsEntities1 car_RentalsEntities1;
        public ManageRentalRecords()
        {
            InitializeComponent();
            car_RentalsEntities1 = new Car_RentalsEntities1();
        }

        private void btnAddRecord_Click(object sender, EventArgs e)
        {
            var addRentalRecord = new AddEditRentalRecord
            {
                MdiParent = this.MdiParent
            };
            addRentalRecord.Show();
        }

        private void btnEdditRecord_Click(object sender, EventArgs e)
        {
            try
            {
                //Get Id of selected row
                var id = (int)gvRecordList.SelectedRows[0].Cells["Id"].Value;

                //query database for record
                var record = car_RentalsEntities1.CarRentalRecords.FirstOrDefault(n => n.id == id);

               if(!Utils.FormIsOpen("AddEditRentalRecord"))
                {
                    var addEditRentalRecord = new AddEditRentalRecord(record);
                    addEditRentalRecord.MdiParent = this.MdiParent;
                    addEditRentalRecord.Show();
                }
               
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }

        private void btnDeleteRecord_Click(object sender, EventArgs e)
        {
            try
            {
                //get Id of selected row
                var id = (int)gvRecordList.SelectedRows[0].Cells["Id"].Value;

                //query database for record
                var record = car_RentalsEntities1.CarRentalRecords.FirstOrDefault(n => n.id == id);

                DialogResult dr = MessageBox.Show("Are You Sure You Want To Delete This Record",
                    "Delete", MessageBoxButtons.YesNoCancel,
                    MessageBoxIcon.Warning);
                if(dr == DialogResult.Yes)
                {
                    car_RentalsEntities1.CarRentalRecords.Remove(record);
                    car_RentalsEntities1.SaveChanges();

                    PopulateGrid();
                }
                
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }

        private void ManageRentalRecords_Load(object sender, EventArgs e)
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

        private void PopulateGrid()
        {
            //Select Id as CarId, names as CarName from TypesOfCars
            var records = car_RentalsEntities1.CarRentalRecords
                .Select(n => new
                {
                    Customer = n.CustomerName,
                    DateOut = n.DateRented,
                    DateIn = n.DateReturned,
                    Id = n.id,
                     n.Cost,
                    Car = n.TypesOfCar.Make + " " + n.TypesOfCar.Model
                })
                    .ToList(); //Specify the columns you want to display from the table
            gvRecordList.DataSource = records;
            gvRecordList.Columns["DateIn"].HeaderText = "Date In";
            gvRecordList.Columns["DateOut"].HeaderText = "Date Out";
            gvRecordList.Columns["Id"].Visible = false;

        }
    }
}
