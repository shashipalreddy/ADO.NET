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
   
- Now we can use the connectionString directly in the sql connection  
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
---

## SQLDataReader

- SqlDataReader reads data in most efficient manner possible.

- SqlDataReader is read-only and forward-only, meaning oce you read a record and go to next record there is no way of going back
  
- The forward-only is what makes it efficient choice to read data.

- SqlDataReader is connection-oriented meaning it requires an active connection to the data source  while reading the data.

- We cannot create a instance using a new operator for SqlDataReader.

- ExecuteReader() method creates and returns the instance of SqlDataReader.

- If there is no open connection before SqlDataReader then it will generate run time error.

- SqlDataReader must be closed in a timely fashion, there are two ways to close the SqlDataReader **Using** and **Finally** block.

- Usage of SqlDataReader with example code:

      string connectionString = ConfigurationManager.ConnectionStrings["myConnectionString"].ConnectionString;
      using (SqlConnection con = new SqlConnection(connectionString))
      {
          SqlCommand cmd = new SqlCommand("Select * from Employee", con);
          con.Open();
          using (SqlDataReader rdr = cmd.ExecuteReader())
          {
              DataTable dataTable = new DataTable();
              dataTable.Columns.Add("EmployeeID");
              dataTable.Columns.Add("Name");
              dataTable.Columns.Add("Gender");
              dataTable.Columns.Add("Salary");
              dataTable.Columns.Add("IncrementedSalary");
              while (rdr.Read())
              {
                  DataRow dataRow = dataTable.NewRow();
                  int originalSalary = Convert.ToInt32(rdr["Salary"]);
                  double incrementSalary = originalSalary + originalSalary * 0.15;

                  dataRow["EmployeeID"] = rdr["EmployeeID"];
                  dataRow["Name"] = rdr["Name"];
                  dataRow["Gender"] = rdr["Gender"];
                  dataRow["Salary"] = rdr["Salary"];
                  dataRow["IncrementedSalary"] = incrementSalary;
                  dataTable.Rows.Add(dataRow);

              }
              GridView1.DataSource = dataTable;
              GridView1.DataBind();
          }

      }

- In the above code example i am adding an extra column named *IncrementedSalary* to the gridview.

## SqlDataReader using NextResult

- If your statement/proc is returning multiple result sets, For example, if you have two select statements in single Command object,       then you will get back two result sets

- When there are two select statements which are getting the data to the **rdr** we use NextResult to get the other table data.

- In the below example the data of the first table is assigned to gridview1 and the NextResult will move the cursor to the 
  other table so we bind that table to gridview2.

         string connectionString = ConfigurationManager.ConnectionStrings["myConnectionString"].ConnectionString;
         using (SqlConnection con = new SqlConnection(connectionString))
         {
             SqlCommand cmd = new SqlCommand("select * from Employee; Select * from inventory;", con);
             con.Open();
             using (SqlDataReader rdr = cmd.ExecuteReader())
             {
                 GridView1.DataSource = rdr;
                 GridView1.DataBind();

                 while (rdr.NextResult())
                 {
                     GridView2.DataSource = rdr;
                     GridView2.DataBind();
                 }
             }

         }

---

## SqlDataAdapter

- SqlDataReader is connection-oriented, meaning it requires an active and open connection to the data source.

- SqlDataAdapter and Dataset provides us with a disconnected data access model.

- We can create a instance of SqlDataAdapter using a new keyword.

- SqlDataAdapter contains
   
   1. The Sql Command that we want to execute.
   2. The Connection on which we execute the command.
  
**DataSet**

- Dataset is an in memory representation of database.

- Dataset can store tables and relation between them same as Database.

- But Database store them in the hard disk but dataset store them in the memory of web server.

**How fill method is used?**

- It open the connection.

- executes the command on the sql server

- reads the data and fills that in the dataset.

- closes the connection immediately.

**Example with usage of SqlDataAdapter and DataSet but with multiples resultset(i.e.., using two select statements)**

      string connectionString = ConfigurationManager.ConnectionStrings["myConnectionString"].ConnectionString;
      using (SqlConnection con = new SqlConnection(connectionString))
      {
          SqlDataAdapter da = new SqlDataAdapter("select * from Employee; Select * from inventory;", con);
          DataSet ds = new DataSet();
          da.Fill(ds);

          //To give meaningful names to a table when it returns multiple result sets
          ds.Tables[0].TableName = "Employee";
          ds.Tables[1].TableName = "Inventory";

          GridView1.DataSource = ds.Tables["Employee"];
          GridView1.DataBind();

          GridView2.DataSource = ds.Tables["Inventory"];
          GridView2.DataBind();

      }
---

## SqlCommandBuilder

- SqlCommandBuilder automatically generates Insert, Update and Delete SQL Statements based on the SELECT Statement for a single table.

- It eliminates the need to write the commands. It reduces the likelihood of errors.

- The OleDbCommandBuilder, SqlCommonBuilder, and OdbcCommandBuilder classes represent the CommonBuilder object in the OleDb, Sql, and     ODBC data providers. 

- Creating a CommonedBuider object is simple. You pass a DataAdapter as an argument of the CommandBuilder constructor. For example:
  `SqlCommandBuilder builder = new SqlCommandBuilder(adapter);`

- Example of inserting data using SqlCommandBuilder
  
      string connectionString = ConfigurationManager.ConnectionStrings["myConnectionString"].ConnectionString;
      using (SqlConnection con = new SqlConnection(connectionString))
      {
          SqlDataAdapter da = new SqlDataAdapter("select * from Employee;", con);

          SqlCommandBuilder commandBuilder = new SqlCommandBuilder(da);

          DataSet ds = new DataSet();
          da.Fill(ds, "Employee");

          DataTable EmployeeTable = ds.Tables["Employee"];
          DataRow row = EmployeeTable.NewRow();
          row["Name"] = "Shashi";
          row["Gender"] = "Male";
          row["Salary"] = 4500;
          EmployeeTable.Rows.Add(row);

          da.Update(ds, "Employee");


      }

- Example of deleting data using SqlCommandBuilder

      string connectionString = ConfigurationManager.ConnectionStrings["myConnectionString"].ConnectionString;
      using (SqlConnection con = new SqlConnection(connectionString))
      {
          SqlDataAdapter MyDataAdapter = new SqlDataAdapter("SELECT * FROM Employee where EmployeeID = 3", con);
          SqlCommandBuilder MyCmd = new SqlCommandBuilder(MyDataAdapter);
          DataSet MyDataSet = new DataSet();

          MyDataAdapter.Fill(MyDataSet);

          DataColumn[] MyKey = new DataColumn[1];

          MyKey[0] = MyDataSet.Tables[0].Columns[0];
          MyDataSet.Tables[0].PrimaryKey = MyKey;

          DataRow FindMyRow = MyDataSet.Tables[0].Rows.Find(1);

          FindMyRow.Delete();
          MyDataAdapter.Update(MyDataSet);


      }

---

## AcceptChanges() and RejectChanges()

- you can refer it  [here](https://docs.microsoft.com/en-us/dotnet/framework/data/adonet/dataset-datatable-dataview/row-states-and-row-versions?redirectedfrom=MSDN)
---

## Strongly Typed Dataset

- Strongly typed dataset is generated based on database schema.

- Strongly typed dataset derives from dataset.

- In a strongly typed dataset the database table columns become properties and the type associated with each column is known at design     time.

### Advantage of using strongly typed datasets over untyped datasets

Since, in a strongly typed dataset the database table columns become properties and the type associated with each column is known at design time, 

- Development is much easier as we will have intellisense 

- Any errors related to misspelt column names can be detected at compile time, rather than at runtime

### Steps to generate a typed dataset using visual studio
1. Right click on the Project Name in solution explorer and select "Add - New Item"
2. Select "DataSet", give it a meaningful name and click "Add". This should add a file with .XSD extension.
3. Click on "View" menu item in Visual Studio and select "Server Explorer"
4. In "Server Explorer", expand "Data Connections", then expand the "Database", and then expand "Tables"
5. Drag and drop the table based on which you want to generate a strongly typed dataset.

ASPX code for both WebForm1.aspx and WebForm2.aspx

         <div style="font-family:Arial">
             <asp:TextBox ID="TextBox1" runat="server"></asp:TextBox>
             <asp:Button ID="Button1" runat="server" Text="Button" 
                 onclick="Button1_Click" />
             <asp:GridView ID="GridView1" runat="server">
             </asp:GridView>
         </div>

         Student class used in the demo
         public class Student
         {
             public int ID { get; set; }
             public string Name { get; set; }
             public string Gender { get; set; }
             public int TotalMarks { get; set; }
         }

         Please make sure to include the following using declarations on WebForm1.aspx.cs
         using System.Configuration;
         using System.Data;
         using System.Data.SqlClient;

         WebForm1.aspx.cs code:
         public partial class WebForm1 : System.Web.UI.Page
         {
             protected void Page_Load(object sender, EventArgs e)
             {
                 if (!IsPostBack)
                 {
                     string connectionString =
                     ConfigurationManager.ConnectionStrings["DBCS"].ConnectionString;
                     SqlConnection connection = new SqlConnection(connectionString);
                     string selectQuery = "Select * from tblStudents";
                     SqlDataAdapter dataAdapter = new SqlDataAdapter(selectQuery, connection);

                     DataSet dataSet = new DataSet();
                     dataAdapter.Fill(dataSet, "Students");

                     Session["DATASET"] = dataSet;

                     GridView1.DataSource = from dataRow in dataSet.Tables["Students"].AsEnumerable() 
                         select new Student 
                         { 
                             ID = Convert.ToInt32(dataRow["Id"]), 
                             Name = dataRow["Name"].ToString(), 
                             Gender = dataRow["Gender"].ToString(), 
                             TotalMarks = Convert.ToInt32(dataRow["TotalMarks"]) 
                         };
                     GridView1.DataBind();
                 }
             }

             protected void Button1_Click(object sender, EventArgs e)
             {
                 DataSet dataSet = (DataSet)Session["DATASET"];

                 if (string.IsNullOrEmpty(TextBox1.Text))
                 {
                     GridView1.DataSource = from dataRow in dataSet.Tables["Students"].AsEnumerable() 
                         select new Student 
                         { 
                             ID = Convert.ToInt32(dataRow["Id"]), 
                             Name = dataRow["Name"].ToString(), 
                             Gender = dataRow["Gender"].ToString(), 
                             TotalMarks = Convert.ToInt32(dataRow["TotalMarks"]) 
                         };
                     GridView1.DataBind();
                 }
                 else
                 {
                     GridView1.DataSource = from dataRow in dataSet.Tables["Students"].AsEnumerable() 
                         where dataRow["Name"].ToString().ToUpper().StartsWith(TextBox1.Text.ToUpper()) 
                         select new Student 
                         { 
                             ID = Convert.ToInt32(dataRow["Id"]), 
                             Name = dataRow["Name"].ToString(), 
                             Gender = dataRow["Gender"].ToString(), 
                             TotalMarks = Convert.ToInt32(dataRow["TotalMarks"]) 
                         };
                     GridView1.DataBind();
                 }
             }
         }

         WebForm2.aspx.cs code:
         public partial class WebForm2 : System.Web.UI.Page
         {
             protected void Page_Load(object sender, EventArgs e)
             {
                 if (!IsPostBack)
                 {
                     StudentDataSetTableAdapters.StudentsTableAdapter studentsTableAdapter = 
                         new StudentDataSetTableAdapters.StudentsTableAdapter();



                     StudentDataSet.StudentsDataTable studentsDataTable = 
                         new StudentDataSet.StudentsDataTable();
                     studentsTableAdapter.Fill(studentsDataTable);

                     Session["DATATABLE"] = studentsDataTable;

                     GridView1.DataSource = from student in studentsDataTable 
                         select new { student.ID, student.Name, student.Gender, student.TotalMarks };
                     GridView1.DataBind();
                 }
             }

             protected void Button1_Click(object sender, EventArgs e)
             {
                 StudentDataSet.StudentsDataTable studentsDataTable = 
                     (StudentDataSet.StudentsDataTable)Session["DATATABLE"];

                 if (string.IsNullOrEmpty(TextBox1.Text))
                 {
                     GridView1.DataSource = from student in studentsDataTable 
                         select new { student.ID, student.Name, student.Gender, student.TotalMarks };
                     GridView1.DataBind();
                 }
                 else
                 {
                     GridView1.DataSource = from student in studentsDataTable 
                         where student.Name.ToUpper().StartsWith(TextBox1.Text.ToUpper()) 
                         select new { student.ID, student.Name, student.Gender, student.TotalMarks };
                     GridView1.DataBind();
                 }
             }
         }
---

## What is the use of SqlBulkCopy class

- SqlBulkCopy class is used to bulk copy data from different data sources to SQL Server database.

- This class is present in System.Data.SqlClient namespace. 

- This class can be used to write data only to SQL Server tables. However, the data source is not limited to SQL Server, any data source   can be used, as long as the data can be loaded to a DataTable instance or read with a IDataReader instance. 
    
- From a performance standpoint, SqlBulkCopy makes it very easy and efficient to copy large amounts of data.

### Loading xml data into sql server table using sqlbulkcopy

Step 1 : Create the database tables using the following sql script

      Create table Departments
      (
       ID int primary key,
       Name nvarchar(50),
       Location nvarchar(50)
      )
      GO

      Create table Employees
      (
       ID int primary key,
       Name nvarchar(50),
       Gender nvarchar(50),
       DepartmentId int foreign key references Departments(Id)
      )
      GO

Step 2 : Create a new empty asp.net web application project. Name it Demo.

Step 3 : Add a new xml file to the project. Name it Data.xml. Copy and paste the following XML.

      [?xml version="1.0" encoding="utf-8" ?]
      [Data]
        [Department Id="1"]
          [Name]IT[/Name]
          [Location]New York[/Location]
        [/Department]
        [Department Id="2"]
          [Name]HR[/Name]
          [Location]London[/Location]
        [/Department]
        [Department Id="3"]
          [Name]Payroll[/Name]
          [Location]Mumbai[/Location]
        [/Department]
        [Employee Id="1"]
          [Name]Mark[/Name]
          [Gender]Male[/Gender]
          [DepartmentId]1[/DepartmentId]
        [/Employee]
        [Employee Id="2"]
          [Name]John[/Name]
          [Gender]Male[/Gender]
          [DepartmentId]1[/DepartmentId]
        [/Employee]
        [Employee Id="3"]
          [Name]Mary[/Name]
          [Gender]Female[/Gender]
          [DepartmentId]2[/DepartmentId]
        [/Employee]
        [Employee Id="4"]
          [Name]Steve[/Name]
          [Gender]Male[/Gender]
          [DepartmentId]2[/DepartmentId]
        [/Employee]
        [Employee Id="5"]
          [Name]Ben[/Name]
          [Gender]Male[/Gender]
          [DepartmentId]3[/DepartmentId]
        [/Employee]
      [/Data]

Step 4 : Include the database connection string in web.config file

      [connectionStrings]
        [add name="CS" connectionString="server=.;database=Sample;integrated security=true"/]
      [/connectionStrings]

Step 5 : Add a new WebForm to the project. Drag and drop a button control on the webform. Double click the button control to generate the click event handler. Copy and paste the following code in the the click event handler method.

      string cs = ConfigurationManager.ConnectionStrings["CS"].ConnectionString;

      using (SqlConnection con = new SqlConnection(cs))
      {
          DataSet ds = new DataSet();

          ds.ReadXml(Server.MapPath("~/Data.xml"));

          DataTable dtDept = ds.Tables["Department"];
          DataTable dtEmp = ds.Tables["Employee"];

          con.Open();

          using (SqlBulkCopy sb = new SqlBulkCopy(con))
          {
              sb.DestinationTableName = "Departments";
              sb.ColumnMappings.Add("ID", "ID");
              sb.ColumnMappings.Add("Name", "Name");
              sb.ColumnMappings.Add("Location", "Location");
              sb.WriteToServer(dtDept);
          }

          using (SqlBulkCopy sb = new SqlBulkCopy(con))
          {
              sb.DestinationTableName = "Employees";
              sb.ColumnMappings.Add("ID", "ID");
              sb.ColumnMappings.Add("Name", "Name");
              sb.ColumnMappings.Add("Gender", "Gender");
              sb.ColumnMappings.Add("DepartmentId", "DepartmentId");
              sb.WriteToServer(dtEmp);
          }
      }

## How to read data from one table and copy it to other table using SQLBulkCopy

Step 1 : Create a new database. Name it SourceDB. Execute the following sql script to create Departments and Employees tables, and to populate with data.

      Create table Departments
      (
       ID int primary key identity,
       Name nvarchar(50),
       Location nvarchar(50)
      )
      GO

      Create table Employees
      (
       ID int primary key identity,
       Name nvarchar(50),
       Gender nvarchar(50),
       DepartmentId int foreign key references Departments(Id)
      )
      GO

      Insert into Departments values ('IT', 'New York')
      Insert into Departments values ('HR', 'London')
      Insert into Departments values ('Payroll', 'Muumbai')
      GO

      Insert into Employees values ('Mark', 'Male', 1)
      Insert into Employees values ('John', 'Male', 1)
      Insert into Employees values ('Mary', 'Female', 2)
      Insert into Employees values ('Steve', 'Male', 2)
      Insert into Employees values ('Ben', 'Male', 3)
      GO

Step 2 : Create another new database. Name it DestinationDB. Execute the sql script to just create Departments and Employees tables. Here we have just the structre of the tables and no data. We will be moving data from SourceDB tables to DestinationDB tables.

      Create table Departments
      (
       ID int primary key identity,
       Name nvarchar(50),
       Location nvarchar(50)
      )
      GO

      Create table Employees
      (
       ID int primary key identity,
       Name nvarchar(50),
       Gender nvarchar(50),
       DepartmentId int foreign key references Departments(Id)
      )
      GO

Step 3 : Include the following 2 connection strings for the Source and Destination databases in the web.config file of the project 
     
      [connectionStrings]
      [add name="SourceCS" connectionString="server=.;database=SourceDB;Integrated Security=True"
            providerName="System.Data.SqlClient" /]
      [add name="DestinationCS" connectionString="server=.;database=DestinationDB;Integrated Security=True"
            providerName="System.Data.SqlClient" /]
      [/connectionStrings]

Step 4 : Copy and paste the following code in the button click event handler method in the code-behind file

      string sourceCS = ConfigurationManager.ConnectionStrings["SourceCS"].ConnectionString;
      string destinationCS = ConfigurationManager.ConnectionStrings["DestinationCS"].ConnectionString;
      using (SqlConnection sourceCon = new SqlConnection(sourceCS))
      {
          SqlCommand cmd = new SqlCommand("Select * from Departments", sourceCon);
          sourceCon.Open();
          using (SqlDataReader rdr = cmd.ExecuteReader())
          {
              using (SqlConnection destinationCon = new SqlConnection(destinationCS))
              {
                  using (SqlBulkCopy bc = new SqlBulkCopy(destinationCon))
                  {
                      bc.DestinationTableName = "Departments";
                      destinationCon.Open();
                      bc.WriteToServer(rdr);
                  }
              }
          }
          cmd = new SqlCommand("Select * from Employees", sourceCon);
          using (SqlDataReader rdr = cmd.ExecuteReader())
          {
              using (SqlConnection destinationCon = new SqlConnection(destinationCS))
              {
                  using (SqlBulkCopy bc = new SqlBulkCopy(destinationCon))
                  {
                      bc.DestinationTableName = "Employees";
                      destinationCon.Open();
                      bc.WriteToServer(rdr);
                  }
              }
          }
      }

### BatchSize property 

- Specifies the number of rows in a batch that will be copied to the destination table.

- The BatchSize property is very important as the performance of data transfer depends on it. The default batch size is 1. 

- In the example below, BatchSize is set to 10000. This means once the reader has read 10000 rows they will be sent to the database as a   single batch to perform the bulk copy operation.

### NotifyAfter property

- Defines the number of rows to be processed before raising SqlRowsCopied event. 

- In the example below, NotifyAfter property is set to 5000. This means once every 5000 rows are copied to the destination table           SqlRowsCopied event is raised.

### SqlRowsCopied event

- This event is raised every time the number of rows specified by NotifyAfter property are processed.

- This event is useful for reporting the progress of the data transfer.

Let us now understand these properties with an example.

Step 1 : Execute the following SQL script to create Products_Source table and populate it with test data.

      Create Table Products_Source
      (
       [Id] int primary key,
       [Name] nvarchar(50),
       [Description] nvarchar(250)
      )
      GO

      Declare @Id int
      Set @Id = 1

      While(@Id [= 300000)
      Begin
       Insert into Products_Source values(@Id, 'Product - ' + CAST(@Id as nvarchar(20)), 
       'Product - ' + CAST(@Id as nvarchar(20)) + ' Description')

       Print @Id
       Set @Id = @Id + 1
      End
      GO

Step 2 : Create Products_Destination table

      Create Table Products_Destination
      (
       [Id] int primary key,
       [Name] nvarchar(50),
       [Description] nvarchar(250)
      )
      GO

Step 3 : Create a new console application. Name it Demo. Include the database connection string in App.config file

      [connectionStrings]
        [add name="CS" 
              connectionString="server=.;database=Sample;integrated security=SSPI"/]
      [/connectionStrings]

Step 4 : Copy and paste the following code in Program.cs file.

      using System;
      using System.Configuration;
      using System.Data.SqlClient;

      namespace Demo
      {
          class Program
          {
              static void Main()
              {
                  string cs = ConfigurationManager.ConnectionStrings["CS"].ConnectionString;
                  using (SqlConnection sourceCon = new SqlConnection(cs))
                  {
                      SqlCommand cmd = new SqlCommand("Select * from Products_Source", sourceCon);
                      sourceCon.Open();
                      using (SqlDataReader rdr = cmd.ExecuteReader())
                      {
                          using (SqlConnection destinationCon = new SqlConnection(cs))
                          {
                              using (SqlBulkCopy bc = new SqlBulkCopy(destinationCon))
                              {
                                  bc.BatchSize = 10000;
                                  bc.NotifyAfter = 5000;
                                  bc.SqlRowsCopied += new SqlRowsCopiedEventHandler(bc_SqlRowsCopied);
                                  bc.DestinationTableName = "Products_Destination";
                                  destinationCon.Open();
                                  bc.WriteToServer(rdr);
                              }
                          }
                      }
                  }
              }

              static void bc_SqlRowsCopied(object sender, SqlRowsCopiedEventArgs e)
              {
                  Console.WriteLine(e.RowsCopied + " loaded....");
              }
          }
      }













