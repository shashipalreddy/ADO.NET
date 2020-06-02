using Confluent.Kafka;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Caching;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplication2
{
    public partial class Employee : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            getData();
        }

        public void getData()
        {
            
            string CS = ConfigurationManager.ConnectionStrings["masterConnectionString"].ConnectionString;
            using(SqlConnection con = new SqlConnection(CS))
            {
                SqlCommand cmd = new SqlCommand("spGetEmployee", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                con.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                GridView1.DataSource = rdr;
                GridView1.DataBind();
            }
        }

        protected void GridView1_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            GridView1.EditIndex = -1;
            getData();
        }

        protected void GridView1_RowEditing(object sender, GridViewEditEventArgs e)
        {
            GridView1.EditIndex = e.NewEditIndex;
            getData();
        }

        protected void GridView1_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            GridViewRow row = (GridViewRow)GridView1.Rows[e.RowIndex];
            int EmployeeId = Convert.ToInt32(GridView1.DataKeys[e.RowIndex].Values[0]);
            string Name = (row.Cells[2].Controls[0] as TextBox).Text;
            string Gender = (row.Cells[3].Controls[0] as TextBox).Text;
            int Salary = Convert.ToInt32((row.Cells[4].Controls[0] as TextBox).Text);
            string CS = ConfigurationManager.ConnectionStrings["DBCS"].ConnectionString;
            using (SqlConnection con = new SqlConnection(CS))
            {
                SqlCommand cmd = new SqlCommand("spUpdateEmployee", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@EmpID", EmployeeId);
                cmd.Parameters.AddWithValue("@Name", Name);
                cmd.Parameters.AddWithValue("@Gender", Gender);
                cmd.Parameters.AddWithValue("@Salary", Salary);
                con.Open();
                cmd.ExecuteReader();
                
            }
            GridView1.EditIndex = -1;
            getData();

        }

        protected void GridView1_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            int EmployeeId = Convert.ToInt32(GridView1.DataKeys[e.RowIndex].Values[0]);
            string CS = ConfigurationManager.ConnectionStrings["DBCS"].ConnectionString;
            using (SqlConnection con = new SqlConnection(CS))
            {
                SqlCommand cmd = new SqlCommand("spRemoveEmployee", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@EmpID", EmployeeId);
                con.Open();
                cmd.ExecuteReader();
            }
            getData();
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            string CS = ConfigurationManager.ConnectionStrings["masterConnectionString"].ConnectionString;
            using (SqlConnection con = new SqlConnection(CS))
            {
                SqlCommand cmd = new SqlCommand("spAddEmployee", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                con.Open();
                cmd.Parameters.AddWithValue("@Name", txtName.Text);
                cmd.Parameters.AddWithValue("@Gender", ddlGender.SelectedValue);
                cmd.Parameters.AddWithValue("@Salary", txtSalary.Text);


                SqlParameter outputParameter = new SqlParameter();
                outputParameter.ParameterName = "@EmpID";
                outputParameter.SqlDbType = System.Data.SqlDbType.Int;
                outputParameter.Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters.Add(outputParameter);

                cmd.ExecuteNonQuery();

                Response.Redirect("Employee.aspx");
            }
        }
    }
}