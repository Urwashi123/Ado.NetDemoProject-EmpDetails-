using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics.Eventing.Reader;
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
            GetCountofEmployees();
        }
        //Execute Reader
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
        //Execute Reader
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
        //Execute scalar
        private void GetCountofEmployees()
        {
            SqlConnection con=new SqlConnection(ConStr);
            try
            {
                string GetEmployeeCountSql = "select Count(*) As 'Total Employee' from Emp;";
                SqlCommand cmd = new SqlCommand(GetEmployeeCountSql, con);
                con.Open();
                int EmployeeCount=Convert.ToInt32(cmd.ExecuteScalar());
                if(EmployeeCount > 0)
                {
                    lblTotalEmployees.Text = "Total num of employee present is " + EmployeeCount;
                }
                else
                {
                    lblTotalEmployees.Text = "No employee hired yet";
                }
            }
            catch(Exception ex)

            {
                MessageBox.Show(ex.Message );
            }
            finally
            {
              con.Close();
            }
        }
        private void FillDataGridView() 
        {
           // SqlConnection con = new SqlConnection(ConStr);
            try
            {
                string getAllEmployeeSql = "select * from Emp";
                SqlDataAdapter da = new SqlDataAdapter(getAllEmployeeSql, ConStr);
                DataSet ds = new DataSet();
                da.Fill(ds);
                dataGridView1.DataSource = ds.Tables[0];

                //string getEmployeeSql = "[dbo].[GetEmployees]";
                //SqlCommand cmd = new SqlCommand(getEmployeeSql, con);
                //cmd.CommandType = CommandType.StoredProcedure;
                //cmd.CommandText = getEmployeeSql;
                //SqlDataAdapter da = new SqlDataAdapter(cmd);
                //DataSet ds= new DataSet();
                //da.Fill(ds);
                //dataGridView1.DataSource = ds.Tables[0];
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                //con.Close();
            }
            
        }
        //Execute Reader
        private void btnSelect_Click(object sender, EventArgs e)
        {
            SqlConnection con = new SqlConnection(ConStr);
           
          
                string SqlQuery = "Select * from [dbo].[Emp] where Id=@Id";

                try
                {
                    if (!string.IsNullOrEmpty(txtId.Text)) { 
                        SqlCommand cmd = new SqlCommand(SqlQuery, con);
                    cmd.Parameters.AddWithValue("@Id", txtId.Text);

                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        txtFirstName.Text = reader.GetString(1);
                        txtLastName.Text = reader.GetString(2);
                        txtGender.Text = reader.GetString(3);
                        cmbCity.Text = reader.GetString(4);
                        cmbIsActive.Text = reader.GetString(5);

                    }

                }



            else {
                    MessageBox.Show("Employee cannot be left Blank");
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
        
        //Execute Nonquery
        private void btnAdd_Click(object sender, EventArgs e)
        {
            SqlConnection con = new SqlConnection(ConStr);
            try
            {
                string addEmployeeSql = @"Insert into Emp Values(@FirstName,@LastName,@Gender,@City,@IsActive)";
                SqlCommand cmd = new SqlCommand(addEmployeeSql, con);
                 cmd.Parameters.AddWithValue("@FirstName",txtFirstName.Text);
                 cmd.Parameters.AddWithValue("@LastName", txtLastName.Text);
                 cmd.Parameters.AddWithValue("@Gender", txtGender.Text);
                 cmd.Parameters.AddWithValue("@City", cmbCity.SelectedValue);
                 cmd.Parameters.AddWithValue("@IsActive", cmbIsActive.SelectedValue);
                con.Open();
                int rowAffected = cmd.ExecuteNonQuery();
                if (rowAffected > 0)
                {
                    MessageBox.Show("Record inserted Sucessfully");
                    GetCountofEmployees();
                    FillDataGridView();
                }
                else
                {
                    MessageBox.Show("Record not inserted");
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

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            SqlConnection con = new SqlConnection(ConStr); 
            try
            {
                string UpdateEmpSql = @"Update Emp Set [FirstName]=@FirstName,
                                                         [LastName]=@LastName,
                                                            [Gender]=@Gender,
                                                               [City]=@City,
                                                        [IsActive]=@IsActive where Id=@ID";
                SqlCommand cmd = new SqlCommand(UpdateEmpSql, con);
                cmd.Parameters.AddWithValue("@Id", txtId.Text);
                cmd.Parameters.AddWithValue("@FirstName", txtFirstName.Text);
                cmd.Parameters.AddWithValue("@LastName", txtLastName.Text);
                cmd.Parameters.AddWithValue("@Gender", txtGender.Text);
                cmd.Parameters.AddWithValue("@City", cmbCity.Text); 
                cmd.Parameters.AddWithValue("@IsActive", cmbIsActive.Text);
                con.Open();
                int rowAffected = cmd.ExecuteNonQuery();
                if (rowAffected > 0)
                {
                    MessageBox.Show("Record updated sucessfully");
                    FillDataGridView();
                }
                else
                {
                    MessageBox.Show("Record not updated sucessfully");
                }
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

        private void btnDelete_Click(object sender, EventArgs e)
        {
            SqlConnection con = new SqlConnection(ConStr);
            try
            {
                string DeleteEmpSql = @"Delete from Emp  where Id=@ID";
                SqlCommand cmd = new SqlCommand(DeleteEmpSql, con);
                cmd.Parameters.AddWithValue("@Id", txtId.Text);
                
                con.Open();
                int rowAffected = cmd.ExecuteNonQuery();
                if (rowAffected > 0)
                {
                    MessageBox.Show("Record deleted sucessfully");
                    FillDataGridView();
                }
                else
                {
                    MessageBox.Show("Record not deleted sucessfully");
                }
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
}
}
