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

**SQL Connections**
   > - A SqlConnection objects represents a unique session to the SQL server data source.
   
   > - With a client/server database system, it is equivalent to a network connection to the server.
   
   > - SqlConnection is used together with SqlDataAdapter and SqlCommand to increase performance when 
       connecting to a Microsoft SQL Server database.
       
   > - If the SqlConnection goes out of scope, it won't be closed. Therefore, you must explicitly close
       the connection by calling **Close or Dispose**.
       
   > - To ensure that connections are always closed, open the connection inside of a using block, 
       as shown in the following code fragment. Doing so ensures that the connection is automatically closed
       when the code exits the block. So there won't be any need for using the connection.close().
       
           using (SqlConnection connection = new SqlConnection(connectionString))
           {
              connection.Open();
              // Do work here; connection closed on following line.
           }
       
   > - You can also do it using exception handling where we will close the connection in the finally block so it will always execute 
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
   > - To import namespaces . Example: using System;
   > - To close connection properly as show in the above example.
