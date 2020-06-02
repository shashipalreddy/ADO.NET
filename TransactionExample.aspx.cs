using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplication2
{
    public partial class TransactionExample : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            getData();
        }

        private void getData()
        {
            string cs = ConfigurationManager.ConnectionStrings["DBCS"].ConnectionString;
            using (SqlConnection con = new SqlConnection(cs))
            {
                SqlCommand cmd = new SqlCommand("Select * from Accounts", con);
                con.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                GridView1.DataSource = rdr;
                GridView1.DataBind();
            }
        }

        protected void btnTransfer_Click(object sender, EventArgs e)
        {
            string cs = ConfigurationManager.ConnectionStrings["DBCS"].ConnectionString;
            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();
                SqlTransaction transaction = con.BeginTransaction();
                try
                {
                    SqlCommand cmd = new SqlCommand("update Accounts set Balance = Balance - 10 where AccountNumber = 'A1';", con, transaction);
                    cmd.ExecuteNonQuery();
                    SqlCommand cmd1 = new SqlCommand("update Accounts set Balance = Balance + 10 where AccountNumber = 'A2';", con, transaction);
                    cmd1.ExecuteNonQuery();
                    transaction.Commit();
                    lblMessage.Text = "Transaction Succesfull";
                    lblMessage.ForeColor = System.Drawing.Color.Green;
                }
                catch
                {
                    transaction.Rollback();
                    lblMessage.Text = "Transaction was not Succesfull";
                    lblMessage.ForeColor = System.Drawing.Color.Red;
                }
                getData();
            }
        }

        //Example using Stored Procedure
        protected void btnTransferFromA2toA1_Click(object sender, EventArgs e)
        {
            string cs = ConfigurationManager.ConnectionStrings["DBCS"].ConnectionString;
            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();
                SqlTransaction transaction = con.BeginTransaction();
                try
                {
                    SqlCommand cmd = new SqlCommand("spUpdateAccount", con, transaction);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.ExecuteNonQuery();
                   
                    transaction.Commit();
                    lblMessage.Text = "Transaction Succesfull";
                    lblMessage.ForeColor = System.Drawing.Color.Green;
                }
                catch
                {
                    transaction.Rollback();
                    lblMessage.Text = "Transaction was not Succesfull";
                    lblMessage.ForeColor = System.Drawing.Color.Red;
                }
                getData();
            }
        }
    }
}