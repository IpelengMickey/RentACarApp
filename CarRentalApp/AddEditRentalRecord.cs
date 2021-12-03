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
    public partial class AddEditRentalRecord : Form
    {
        private bool isEditMode;
        private readonly Car_RentalsEntities1 car_RentalsEntities1;
        public AddEditRentalRecord()
        {
            InitializeComponent();
            lblTitle.Text = "Add New Rental Record";
            this.Text = "Add New Rental Record";
            isEditMode = false;
            car_RentalsEntities1 = new Car_RentalsEntities1(); //database
        }

        //type ctor and press Tab twice to geerate a new constructor
        public AddEditRentalRecord(CarRentalRecord recordToEdit)
        {
            InitializeComponent();
            lblTitle.Text = "Edit Rental Record";
            this.Text = "Edit Rental Record";
            if (recordToEdit == null)
            {
                MessageBox.Show("Please ensure that you selected a valid record to edit");
                Close();
            }
            else
            {
                isEditMode = true;
                car_RentalsEntities1 = new Car_RentalsEntities1();
                populateFields(recordToEdit);
            }
        }

        private void populateFields(CarRentalRecord recordToEdit)
        {
            tbCustomerName.Text = recordToEdit.CustomerName;
            dtRented.Value = (DateTime)recordToEdit.DateRented;
            dtReturned.Value = (DateTime)recordToEdit.DateReturned;
            tbCost.Text = recordToEdit.Cost.ToString();
            lblRecordId.Text = recordToEdit.id.ToString();
           
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                string CustomerName = tbCustomerName.Text;
                var dateOut = dtRented.Value;
                var dateIn = dtReturned.Value;
                double cost = Convert.ToDouble(tbCost.Text);

                var CarType = cbTypeOfCar.Text;
                var isValid = true;
                var errorMessage = "";

                if (string.IsNullOrWhiteSpace(CustomerName) || string.IsNullOrWhiteSpace(CarType))
                {
                    isValid = false;
                    errorMessage += "Error: Please enter missing data\n\r";
                }

                if (dateOut > dateIn)
                {
                    isValid = false;
                    errorMessage += "Error: Illegal Date selection\n\r";
                }


                //if(isValid == true)
                if (isValid)
                {
                    //Declair an object of the record to be added
                    var rentalRecord = new CarRentalRecord();
                    if (isEditMode)
                    {
                        //If in edit mode, then get the ID and retieve the record from the
                        //result in the record object
                        var id = int.Parse(lblRecordId.Text);
                        rentalRecord = car_RentalsEntities1.CarRentalRecords.FirstOrDefault(n => n.id == id);
                    }
                        //Populate record object with values from the form
                        rentalRecord.CustomerName = CustomerName;
                        rentalRecord.DateRented = dateOut;
                        rentalRecord.DateReturned = dateIn;
                        rentalRecord.Cost = (decimal)cost;  //casting
                        rentalRecord.TypeOfCarId = (int)cbTypeOfCar.SelectedValue;
                     
                    //if not in edit mode, then add the record object tothe database
                    if(!isEditMode)
                        car_RentalsEntities1.CarRentalRecords.Add(rentalRecord);
                    
                    //Save changes made to the entity
                    car_RentalsEntities1.SaveChanges();


                    MessageBox.Show($"Customer Name: {CustomerName}\n\r" +
                          $"Date Rented: {dateOut}\n\r" +
                          $"Date Returned: {dateIn}\n\r" +
                          $"Cost: {cost}\n\r" +
                          $"Type of Car: {CarType}\n\r" +
                          $"THANK YOU FOR YOUR BUSINESS");
                    Close();
                }
                else
                {
                    MessageBox.Show(errorMessage);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }



        }

        //link database values to the car rental diagram
        private void Form1_Load(object sender, EventArgs e)
        {
            //Select cars from TypeOfCars
            var cars = car_RentalsEntities1.TypesOfCars
                .Select(n => new { Id = n.Id, Name = n.Make + " " + n.Model })
                .ToList();
            cbTypeOfCar.DisplayMember = "Name";
            cbTypeOfCar.ValueMember = "Id";
            cbTypeOfCar.DataSource = cars;
        }

    }
}
