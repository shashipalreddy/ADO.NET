# Illustrate the example using SqlDataAdapter

## Database

- Here we use same queries as taken in the **SqlDataReader** file.

## ADO.NET CODE

- SqlDataAdapter is a disconnected data model where we can store the data in the cache for sometime and later update it to database.


        using System;
        using System.Collections.Generic;
        using System.Configuration;
        using System.Data;
        using System.Data.SqlClient;
        using System.Linq;
        using System.Web;
        using System.Web.UI;
        using System.Web.UI.WebControls;

        namespace WebApplication2
        {
            public partial class EmployeeDisconnectedModel : System.Web.UI.Page
            {
                protected void Page_Load(object sender, EventArgs e)
                {

                }

                public void getDisconnectedData()
                {
                    string CS = ConfigurationManager.ConnectionStrings["DBCS"].ConnectionString;
                    using (SqlConnection con = new SqlConnection(CS))
                    {
                        SqlDataAdapter da = new SqlDataAdapter("spGetEmployee", con);
                        DataSet ds = new DataSet();
                        da.Fill(ds,"Employee");

                        Cache.Insert("Data", ds, null, DateTime.Now.AddHours(24), System.Web.Caching.Cache.NoSlidingExpiration);
                        ds.Tables["Employee"].PrimaryKey = new DataColumn[] { ds.Tables["Employee"].Columns["EmpID"] };

                        gvEmployee.DataSource = ds;
                        gvEmployee.DataBind();

                        lblMessage.Text = "Load Data From Database";
                    }
                }

                public void getDataCache()
                {
                    if(Cache["Data"] != null)
                    {
                        DataSet ds = (DataSet)Cache["Data"];
                        gvEmployee.DataSource = ds;
                        gvEmployee.DataBind();
                    }
                }

                protected void btnLoadData_Click(object sender, EventArgs e)
                {
                    getDisconnectedData();
                }

                protected void gvEmployee_RowUpdating(object sender, GridViewUpdateEventArgs e)
                {
                    if(Cache["Data"] != null)
                    {
                        DataSet ds = (DataSet)Cache["Data"];
                        DataRow dr = ds.Tables["Employee"].Rows.Find(e.Keys["EmpID"]);
                        dr["Name"] = e.NewValues["Name"];
                        dr["Gender"] = e.NewValues["Gender"];
                        dr["Salary"] = e.NewValues["Salary"];

                        Cache.Insert("Data", ds, null, DateTime.Now.AddHours(24), System.Web.Caching.Cache.NoSlidingExpiration);
                        gvEmployee.EditIndex = -1;
                        getDataCache();
                    }
                }

                protected void gvEmployee_RowEditing(object sender, GridViewEditEventArgs e)
                {
                    gvEmployee.EditIndex = e.NewEditIndex;
                    getDataCache();
                }

                protected void gvEmployee_RowDeleting(object sender, GridViewDeleteEventArgs e)
                {
                    if (Cache["Data"] != null)
                    {
                        DataSet ds = (DataSet)Cache["Data"];
                        DataRow dr = ds.Tables["Employee"].Rows.Find(e.Keys["EmpID"]);
                        dr.Delete();

                        Cache.Insert("Data", ds, null, DateTime.Now.AddHours(24), System.Web.Caching.Cache.NoSlidingExpiration);
                        getDataCache();
                    }
                }

                protected void gvEmployee_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
                {
                    gvEmployee.EditIndex = -1;
                    getDataCache();
                }

                protected void btnUpdateData_Click(object sender, EventArgs e)
                {
                    if (Cache["Data"] != null)
                    {
                        string CS = ConfigurationManager.ConnectionStrings["DBCS"].ConnectionString;
                        using (SqlConnection con = new SqlConnection(CS))
                        {
                            SqlDataAdapter da = new SqlDataAdapter("spGetEmployee", con);
                            DataSet ds = (DataSet)Cache["Data"];

                            //string strUpdate = "update Employee set Name = @Name, Gender = @Gender, Salary = @Salary where EmpID = @EmpID";
                            SqlCommand updateCommand = new SqlCommand("spUpdateEmployee", con);
                            updateCommand.CommandType = CommandType.StoredProcedure;
                            updateCommand.Parameters.Add("@Name", SqlDbType.NVarChar, 250, "Name");
                            updateCommand.Parameters.Add("@Gender", SqlDbType.NVarChar, 250, "Gender");
                            updateCommand.Parameters.Add("@Salary", SqlDbType.Int, 0, "Salary");
                            updateCommand.Parameters.Add("@EmpID", SqlDbType.Int, 0, "EmpID");

                            da.UpdateCommand = updateCommand;

                            SqlCommand deleteCommand = new SqlCommand("spRemoveEmployee", con);
                            deleteCommand.CommandType = CommandType.StoredProcedure;
                            deleteCommand.Parameters.Add("@EmpID", SqlDbType.Int, 0, "EmpID");

                            da.DeleteCommand = deleteCommand;

                            da.Update(ds, "Employee");

                        }
                    }

                }
            }
          }
