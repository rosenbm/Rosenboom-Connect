﻿<%@ Page Language="VB" AutoEventWireup="false" CodeFile="FixtureLookup.aspx.vb" Inherits="MES_FLIP" MasterPageFile="~/MasterPage.master" Title="Rosenboom Portal - Record a FLIP" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <script type="text/javascript">
        function autotab(original, destination) {
            if (original.getAttribute && original.value.length == original.getAttribute("maxlength"))
                destination.focus()
        }</script>

    <div class="wrapper col3">
        <div id="intro">
            <h2>Fixture Lookup</h2>
            <br /><br />
            &nbsp;Part Number: &nbsp; <asp:TextBox ID="txtEmpID" runat="server" ></asp:TextBox>
            &nbsp; <br /><br />
            <asp:Button ID="btnSearch" runat="server" Text="Search"/>&nbsp;&nbsp;&nbsp;
            <br /><br />
            <asp:GridView ID="gvwJobOpers" runat="server" DataSourceID="SqlDataSource1" AutoGenerateColumns="False" BackColor="White" 
        BorderColor="#336666" BorderStyle="Double" BorderWidth="3px" CellPadding="4"  GridLines="Horizontal" EnableModelValidation="True">
         <FooterStyle BackColor="White" ForeColor="#333333" />
         <PagerStyle BackColor="#336666" ForeColor="White" HorizontalAlign="Center" />
         <SelectedRowStyle CssClass="SelectedRowStyle" BackColor="#E2DED6" 
            Font-Bold="True" ForeColor="#333333" /> 
         <RowStyle BackColor="White" ForeColor="#333333" />
        <Columns>
            <asp:BoundField DataField="GageNumber" HeaderText="GageNumber" 
                SortExpression="GageNumber" />
            <asp:BoundField DataField="GageDescription" HeaderText="GageDescription" SortExpression="GageDescription" />
            <asp:BoundField DataField="CurrentLocation" HeaderText="CurrentLocation" 
                SortExpression="CurrentLocation" />
            <asp:CheckBoxField DataField="Active" HeaderText="Active" SortExpression="Active" />
            <asp:BoundField DataField="CalibProcedure" HeaderText="CalibProcedure" 
                SortExpression="CalibProcedure" />
        </Columns>
         <HeaderStyle BackColor="#336666" Font-Bold="True" ForeColor="White" />
    </asp:GridView>
    <asp:SqlDataSource ID="SqlDataSource1" runat="server" 
        ConnectionString="<%$ ConnectionStrings:GagepackConnectionString %>" SelectCommand=" SELECT &quot;Gages&quot;.&quot;GageNumber&quot;, &quot;Gages&quot;.&quot;GageDescription&quot;, &quot;Gages&quot;.&quot;CurrentLocation&quot;, &quot;Gages&quot;.&quot;Active&quot;, &quot;Gages&quot;.&quot;CalibProcedure&quot;
 FROM   &quot;Gagepack&quot;.&quot;dbo&quot;.&quot;Gages&quot; &quot;Gages&quot;
WHERE &quot;Gages&quot;.&quot;CalibProcedure&quot; LIKE '%' + @PN + '%'">
        <SelectParameters>
            <asp:ControlParameter ControlID="txtEmpID" Name="PN" PropertyName="Text" />
        </SelectParameters>
    </asp:SqlDataSource>
            <br />
        </div>
    </div>
 </asp:Content>