using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Ado.NetDemo
{
    public partial class Form1 : Form
    {
        string ConStr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        public Form1()
        {
            InitializeComponent();
            populateCities();
            populateEmployeeStatus();
        }
        private void populateCities()
        {
            SqlConnection con = new SqlConnection(ConStr);
            try
            {
                List<string> listofCities = new List<string>();
               
                string getCitiesSql = "select * from Cities";
                SqlCommand cmd = new SqlCommand(getCitiesSql, con);
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    listofCities.Add(reader.GetString(1));
                }
                cmbCity.DataSource = listofCities;
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally 
            {
                con.Close();
            }

        }
        private void populateEmployeeStatus()
        {
            SqlConnection con = new SqlConnection(ConStr); 
            try
            {
                List<string> listofEmployeeStatus= new List<string>();
               
                string getEmpSql = "select * from Emp_Status";
                SqlCommand cmd = new SqlCommand(getEmpSql, con);
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    listofEmployeeStatus.Add(reader.GetString(1));
                }
                cmbIsActive.DataSource = listofEmployeeStatus;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                con.Close();    
            }

        }
        private void btnSelect_Click(object sender, EventArgs e)
        {
            SqlConnection con = new SqlConnection(ConStr);
            string SqlQuery = "Select * from [dbo].[Emp]";

            try
            {
                SqlCommand cmd = new SqlCommand(SqlQuery, con);
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    txtFirstName.Text = reader.GetString(1);
                    txtLastName.Text = reader.GetString(2);
                    txtGender.Text = reader.GetString(3);
                    //txtCity.Text = reader.GetString(4);
                    //txtIsActive.Text = reader.GetString(5);

                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                con.Close();
            }
           
        }
    }
}
