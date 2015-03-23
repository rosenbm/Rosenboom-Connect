<%@ Page Language="VB" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<script runat="server">

</script>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Deviation History</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:SqlDataSource ID="SqlDataSource1" runat="server" 
            ConnectionString="<%$ ConnectionStrings:DeviationDB %>" 
            ProviderName="<%$ ConnectionStrings:DeviationDB.ProviderName %>" SelectCommand="SELECT IDNum, RequestedDate, RequestorName, ComponentPN, CylinderPN, ResourceGroup, RequestDesc, Approved, Status, Plant, Type, ApprovalDate, ApprovedBy 
FROM Deviation.dbo.Main
WHERE ComponentPN = ? AND Status = 'Closed'
ORDER BY IDNum">
            <SelectParameters>
                <asp:QueryStringParameter Name="?" QueryStringField="partnum" />
            </SelectParameters>
        </asp:SqlDataSource>
        <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" 
            CellPadding="4" DataSourceID="SqlDataSource1" ForeColor="#333333" 
            GridLines="None">
            <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
            <Columns>
                <asp:BoundField DataField="IDNum" HeaderText="IDNum" InsertVisible="False" 
                    ReadOnly="True" SortExpression="IDNum" />
                <asp:BoundField DataField="RequestedDate" HeaderText="RequestedDate" 
                    SortExpression="RequestedDate" />
                <asp:BoundField DataField="RequestorName" HeaderText="RequestorName" 
                    SortExpression="RequestorName" />
                <asp:BoundField DataField="ComponentPN" HeaderText="ComponentPN" 
                    SortExpression="ComponentPN" />
                <asp:BoundField DataField="CylinderPN" HeaderText="CylinderPN" 
                    SortExpression="CylinderPN" />
                <asp:BoundField DataField="ResourceGroup" HeaderText="RG" 
                    SortExpression="ResourceGroup" />
                <asp:BoundField DataField="RequestDesc" HeaderText="RequestDesc" 
                    SortExpression="RequestDesc" />
                <asp:BoundField DataField="Approved" HeaderText="Approved" 
                    SortExpression="Approved" />
                <asp:BoundField DataField="Status" HeaderText="Status" 
                    SortExpression="Status" />
                <asp:BoundField DataField="Plant" HeaderText="Plant" SortExpression="Plant" />
                <asp:BoundField DataField="Type" HeaderText="Type" SortExpression="Type" />
                <asp:BoundField DataField="ApprovalDate" HeaderText="ApprovalDate" 
                    SortExpression="ApprovalDate" />
                <asp:BoundField DataField="ApprovedBy" HeaderText="ApprovedBy" 
                    SortExpression="ApprovedBy" />
            </Columns>
            <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
            <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
            <EmptyDataTemplate>
                There are no historical items for the selected part number.
            </EmptyDataTemplate>
            <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
            <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
            <EditRowStyle BackColor="#999999" />
            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
        </asp:GridView>
    </div>
    </form>
</body>
</html>
