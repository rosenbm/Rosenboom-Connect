<%@ Page Language="VB" AutoEventWireup="false" CodeFile="material-admin.aspx.vb" Inherits="EFS_material_admin_new_" MasterPageFile="~/MasterPage.master" Title="Rosenboom Connect - EFS" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<div class="wrapper col3">
    <div id="intro">
    <h2>Material Issues - Admin</h2>
    <asp:Button ID="btnRefresh" runat="server" Text="Refresh"/><br /><br />
        <asp:Label ID="lblMessage" runat="server" Text=""></asp:Label>
    <br /><br />
        <asp:GridView ID="grdvOpenIssues" runat="server" AutoGenerateColumns="False" 
            DataSourceID="sqlUD02" AllowSorting="True" CellPadding="4" ForeColor="#333333" 
            GridLines="None" DataKeyNames="Key1">
            <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
            <Columns>
                <asp:CommandField ShowSelectButton="True" />
                <asp:BoundField DataField="Submitted" HeaderText="Submitted" 
                    SortExpression="Submitted" DataFormatString="{0:d}" />
                <asp:BoundField DataField="Requestor" HeaderText="Requestor" 
                    SortExpression="Requestor" />
                <asp:BoundField DataField="RG" HeaderText="RG" SortExpression="RG" />
                <asp:BoundField DataField="Supervisor" HeaderText="Supervisor" 
                    SortExpression="Supervisor" />
                <asp:BoundField DataField="Issue" HeaderText="Issue" SortExpression="Issue" />
                <asp:BoundField DataField="PartNum" HeaderText="PartNum" 
                    SortExpression="PartNum" />
                <asp:BoundField DataField="EmpID" HeaderText="EmpID" SortExpression="EmpID" />
            </Columns>
            <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
            <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
            <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
            <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
            <EditRowStyle BackColor="#999999" />
            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
        </asp:GridView>
        <asp:DetailsView ID="DetailsView1" runat="server" AutoGenerateRows="False" 
            CellPadding="4" DataSourceID="sqlUD02Detail" ForeColor="#333333" 
            GridLines="None" Height="50px" Width="636px">
            <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
            <CommandRowStyle BackColor="#E2DED6" Font-Bold="True" />
            <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
            <FieldHeaderStyle BackColor="#E9ECF1" Font-Bold="True" />
            <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
            <Fields>
                <asp:BoundField DataField="Info" HeaderText="Info" SortExpression="Info" />
                <asp:BoundField DataField="Character04" HeaderText="Reason" 
                    SortExpression="Character04" />
                <asp:BoundField DataField="Character03" HeaderText="Communicated With" 
                    SortExpression="Character03" />
            </Fields>
            <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
            <EditRowStyle BackColor="#999999" />
            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
        </asp:DetailsView>
        <asp:SqlDataSource ID="sqlUD02" runat="server" 
            ConnectionString="<%$ ConnectionStrings:Epicor9RU %>" 
            ProviderName="<%$ ConnectionStrings:Epicor9RU.ProviderName %>" 
            SelectCommand="SELECT &quot;Date01&quot; AS &quot;Submitted&quot;, &quot;Character02&quot; AS &quot;Requestor&quot;, &quot;Key2&quot; AS &quot;RG&quot;,  &quot;Key3&quot; AS &quot;Supervisor&quot;, &quot;Key4&quot; AS &quot;Issue&quot;, &quot;Character05&quot; AS &quot;PartNum&quot;, &quot;Key1&quot;, &quot;Key5&quot; AS &quot;EmpID&quot;
FROM MFGSYS.PUB.UD02 
WHERE (&quot;CheckBox01&quot; = '0')"></asp:SqlDataSource>
        <asp:SqlDataSource ID="sqlUD02Detail" runat="server" 
            ConnectionString="<%$ ConnectionStrings:Epicor9RU %>" 
            ProviderName="<%$ ConnectionStrings:Epicor9RU.ProviderName %>" SelectCommand="SELECT &quot;Character01&quot; AS &quot;Info&quot;, &quot;Character04&quot;, &quot;Character03&quot;
FROM MFGSYS.PUB.UD02
WHERE &quot;Key1&quot; = ?">
            <SelectParameters>
                <asp:ControlParameter ControlID="grdvOpenIssues" Name="?" 
                    PropertyName="SelectedValue" />
            </SelectParameters>
        </asp:SqlDataSource>
<br />
Communicated With:<br /><asp:TextBox ID="txtCommunicatedWith" runat="server"></asp:TextBox><br /><br />
Reason:<br /><asp:TextBox ID="txtReason" runat="server"  Height="100" Width="400" TextMode="multiline"></asp:TextBox><br /><br />
<asp:CheckBox ID="chkComplete" runat="server" Text="Complete" Checked=false/><br /><br />
<asp:Button ID="btnSubmit" runat="server" Text="Submit" /><br /><br />
    </div>
</div>
</asp:Content>

