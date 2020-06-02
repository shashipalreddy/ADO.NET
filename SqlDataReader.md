## Program which Illustartes the Connection-Oriented Model using SqlDataReader

To create a program using SqlDataAdapter step by step:

- Create a table named Employee using the query:  

        Create table Employee(
          EmpID int identity primary key, 
          Name nvarchar(100),
          Gender nvarchar(10),
          Salary int
        )
- In the above query **EmpID** is a primary key and a identity which is auto generated.

- Next Create a stored procedures for

    1.Insert Query:
    
           --- Stored Procedure For Inserting
           create procedure spAddEmployee
           @Name nvarchar(250),
           @Gender nvarchar(250),
           @Salary int,
           @EmpID int out
           as 
           Begin
             insert into Employee values(@Name, @Gender, @Salary);
             Select @EmpID = Scope_Identity();
           End
    
    2. Update Query:

           --- Stored Procedure to Update an Employee
           Create Procedure spUpdateEmployee
           @EmpID int,
           @Name nvarchar(250),
           @Gender nvarchar(25 0),
           @Salary int
           as
           Begin
           update Employee set Name = @Name, Gender = @Gender, Salary = @Salary where EmpId = @EmpID;
           End
      
  3. Delete Query:
   
          --- Stored Procedure to delete an Employee
          Create Procedure spRemoveEmployee
          @EmpID int
          as
          Begin
            Delete from Employee where EmpID = @EmpID;
          End
          
  4. Select Query:
  
          --- Stored Procedure For Select Command
          Create Procedure spGetEmployee
          as
          Begin
            select * from Employee;
          End
