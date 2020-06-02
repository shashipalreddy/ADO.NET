<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EmployeeDisconnectedModel.aspx.cs" Inherits="WebApplication2.EmployeeDisconnectedModel" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <asp:Button ID="btnLoadData" runat="server" Text="Get Data From Database" OnClick="btnLoadData_Click" />

            <br />

            <br />

            <asp:GridView ID="gvEmployee" runat="server" AutoGenerateColumns="False" BackColor="#DEBA84" BorderColor="#DEBA84" BorderStyle="None" BorderWidth="1px" CellPadding="3" CellSpacing="2" DataKeyNames="EmpID" OnRowCancelingEdit="gvEmployee_RowCancelingEdit" OnRowDeleting="gvEmployee_RowDeleting" OnRowEditing="gvEmployee_RowEditing" OnRowUpdating="gvEmployee_RowUpdating">
                <Columns>
                    <asp:CommandField ShowDeleteButton="True" ShowEditButton="True" />
                    <asp:BoundField DataField="EmpID" HeaderText="EmpID" InsertVisible="False" ReadOnly="True" SortExpression="EmpID" />
                    <asp:BoundField DataField="Name" HeaderText="Name" SortExpression="Name" />
                    <asp:BoundField DataField="Gender" HeaderText="Gender" SortExpression="Gender" />
                    <asp:BoundField DataField="Salary" HeaderText="Salary" SortExpression="Salary" />
                </Columns>
                <FooterStyle BackColor="#F7DFB5" ForeColor="#8C4510" />
                <HeaderStyle BackColor="#A55129" Font-Bold="True" ForeColor="White" />
                <PagerStyle ForeColor="#8C4510" HorizontalAlign="Center" />
                <RowStyle BackColor="#FFF7E7" ForeColor="#8C4510" />
                <SelectedRowStyle BackColor="#738A9C" Font-Bold="True" ForeColor="White" />
                <SortedAscendingCellStyle BackColor="#FFF1D4" />
                <SortedAscendingHeaderStyle BackColor="#B95C30" />
                <SortedDescendingCellStyle BackColor="#F1E5CE" />
                <SortedDescendingHeaderStyle BackColor="#93451F" />
            </asp:GridView>

            <br />

            <asp:Button ID="btnUpdateData" runat="server" Text="Update Data To Database" OnClick="btnUpdateData_Click" />
            
            <asp:Label ID="lblMessage" runat="server"></asp:Label>
        </div>
    </form>
</body>
</html>
