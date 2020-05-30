# ADO.NET NOTES

**what is ADO.NET?** 
   > ADO.NET stands for Microsoft ActiveX Data Objects.  
   > ADO.NET is not a different technology.  
   > It is just a framewok (set of classes) which can be used to interact with the data sources like Database and XML files.  

**DotNET Data Providers**  
   > Data Provider for SQL Server - System.Data.SqlClient  
   > Data Provider for Oracle Server - System.Data.OracleClient  
   > Data Provider for OLEDB - System.Data.OleDb  
   > Data Provider for ODBC - System.Data.Odbc  
---

**SQL Connections**

- A SqlConnection objects represents a unique session to the SQL server data source.

- With a client/server database system, it is equivalent to a network connection to the server.

- SqlConnection is used together with SqlDataAdapter and SqlCommand to increase performance when 
 connecting to a Microsoft SQL Server database.

- If the SqlConnection goes out of scope, it won't be closed. Therefore, you must explicitly close
 the connection by calling **Close or Dispose**.

- To ensure that connections are always closed, open the connection inside of a using block, 
 as shown in the following code fragment. Doing so ensures that the connection is automatically closed
 when the code exits the block. So there won't be any need for using the connection.close().

      using (SqlConnection connection = new SqlConnection(connectionString))  
      {  
        connection.Open();  
        // Do work here; connection closed on following line.  
      }

- You can also do it using exception handling where we will close the connection in the finally block so it will always execute 
 irrespective of the scenario.   

      try  
      {  
         SqlConnection connection = new SqlConnection(connectionString);  
         con.Open();  
         // more code goes here
      }  
      Catch (Exception ex)  
      {  
         console.WriteLine("Log Exeption ", ex);  
      }  
      finally  
      {  
         con.Close();  
      } 
         
**What are the 2 uses of an using statement in c# ?**
- To import namespaces . Example: using System;
- To close connection properly as show in the above example.
---

**Storing and Reading connection strings in a configuration file**

- we can also store the connection string in the configuration file. 

- So that if there are any changes we can make a change in the configuration file directly and only
  in one file place instead of finding it in multiple location of the code.
  
- For web forms it will be **web.config** and for windows applications it will be **app.config**.

        1. <connectionStrings>
             <add name="myConnectionString" connectionString="server=localhost;database=myDb;uid=myUser;password=myPass;" />
           </connectionStrings>
   
        (or)
        2. <connectionStrings>
             <add name="myConnectionString" connectionString="data source=.; database=sample; integrated security=SSPI" />
           </connectionStrings>
        
- The 1st way is used with SQL authentication and 2nd way is used with Windows Authentication.

- we will read the connection string from the configuration file by first importing the  
   `using System.Configuration;`.

- Then in the code to read connection string use the configurationManager class  
   `string connectionString = ConfigurationManager.ConnectionStrings["myConnectionString"].ConnectionString;`
   
- Now we can use the connectionString directly in the sql connection as below
   `SqlConnection connection = new SqlConnection(connectionString)`
   
---

## SQL COMMAND in ADO.NET

- SqlCommand is used to prepare SQL statement or stored procedure that we want to execute on a SQL Server Database.

### ExecuteReader

      - we use it when our query returns more than single value. 
      - For example, if the query returns rows of data.
      
### ExecuteNonQuery

      - Used to perform insert, update or delete operation.
      - ExecuteNonQuery is integer type it returns the total number of rows affected.

### ExecuteScalar 

      - used when query retuns a single value. 
      - For example, query returns total number of rows in a table.  
      - Generally ExecuteScalar is object type we type cast it depending upon the output. 
      - In the below example we type cast it to int becuase we expect int to returned by the query.
      
- Example:

            string connectionString = ConfigurationManager.ConnectionStrings["myConnectionString"].ConnectionString;
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand();

                //Example for ExecuteReader
                cmd.CommandText = "select * from inventory";
                cmd.Connection = con;
                con.Open();
                GridView1.DataSource = cmd.ExecuteReader();
                GridView1.DataBind();
                con.Close();

                //Example for ExecuteNonQuery
                cmd.CommandText = "Insert into inventory values(2,'Pingili', 'Shashipal Reddy')";
                con.Open();
                int rowsEffected = cmd.ExecuteNonQuery();
                Response.Write("Execute Non Query: " + rowsEffected.ToString());
                con.Close();

                //Example for ExecuteScalar
                cmd.CommandText = "Select count(PersonID) from inventory";
                con.Open();
                int rowsValue = (int)cmd.ExecuteScalar();
                Response.Write("Execute Scalar: " + rowsValue.ToString());
            }
---

## Sql Injection

- The Sql injection will happen if we concatenate the value to the query.

- So in order to eliminate the SQL injection we use either Parameterized Query or a Stored Procedure.

- Best Practice will be to use the stored procedure.

---

## How to use Stored Procedure with output parameters

- First create a table Named Employee using create query where we have employee id which is identity.

         create table Employee(
            EmployeeID int identity primary key,
            Name varchar(250),
            Gender varchar(250),
            Salary int
         )
            
- Next Create a stored Procedure for inserting Employee into the table.

         Create Procedure spAddEmployee
         @Name varchar(250),
         @Gender varchar(250),
         @Salary int,
         @EmpID int out
         as
         Begin 
            Insert into Employee values(@Name,@Gender,@Salary);
            select @EmpID = SCOPE_IDENTITY();
         End
         
- Now we can make use of the stored procedure in the code to insert values into the table

          cmd.CommandText = "spAddEmployee";
          cmd.Connection = con;
          cmd.CommandType = System.Data.CommandType.StoredProcedure;

          cmd.Parameters.AddWithValue("@Name", "Shashipal reddy Pingili");
          cmd.Parameters.AddWithValue("@Gender", "Male");
          cmd.Parameters.AddWithValue("@Salary", 5000);

          SqlParameter outputParameter = new SqlParameter();
          outputParameter.ParameterName = "@EmpID";
          outputParameter.SqlDbType = System.Data.SqlDbType.Int;
          outputParameter.Direction = System.Data.ParameterDirection.Output;
          cmd.Parameters.Add(outputParameter);

          con.Open();
          cmd.ExecuteNonQuery();
          
- we have EmpID as a output parameter so we make use of 
            
            - ParameterName to know which parameter is an output parameter
            - SqlDbType to know the parameter of which data type
            - Direction to know wheter it is an input or output





















