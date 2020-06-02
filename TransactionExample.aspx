<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TransactionExample.aspx.cs" Inherits="WebApplication2.TransactionExample" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <asp:GridView ID="GridView1" runat="server" BackColor="#DEBA84" BorderColor="#DEBA84" BorderStyle="None" BorderWidth="1px" CellPadding="3" CellSpacing="2">
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
            <asp:Button ID="btnTransfer" runat="server" Text="Send Money From A1 to A2" OnClick="btnTransfer_Click" />
            &nbsp;
            <asp:Button ID="btnTransferFromA2toA1" runat="server" Text="Send Money From A2 to A1" OnClick="btnTransferFromA2toA1_Click" />
            <br />
            <asp:Label ID="lblMessage" runat="server"></asp:Label>
            <br />
        </div>
    </form>
</body>
</html>
