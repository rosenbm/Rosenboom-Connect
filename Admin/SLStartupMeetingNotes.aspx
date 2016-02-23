<%@ Page Language="VB" AutoEventWireup="false" CodeFile="SLStartupMeetingNotes.aspx.vb" Inherits="MES_NEW_IssueMaterial" MasterPageFile="~/MasterPage.master" Title="Rosenboom Connect - Golden Ticket"%>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

    <div class="wrapper col3 covergolden">
<div id="intro">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
            <h1>
                SL Startup Meeting Notes</h1>
    <asp:Label ID="lblMessage" runat="server" Text=""></asp:Label><br />
                <br />
              
    <asp:DropDownList ID="DropDownList1" runat="server" DataSourceID="DeptList" DataTextField="Label" DataValueField="Department">
        <asp:ListItem>CI</asp:ListItem>
        <asp:ListItem Value="SL CS">Customer Service</asp:ListItem>
        <asp:ListItem Value="HR">Human Resources</asp:ListItem>
        <asp:ListItem Value="SL Maintenance">Maintenance</asp:ListItem>
        <asp:ListItem Value="SL Weld Support">Mfg Eng / Weld Support</asp:ListItem>
        <asp:ListItem Value="SL Lathe Support">Preventable Damage</asp:ListItem>
        <asp:ListItem Value="SL Plant Manager">Plant Manager</asp:ListItem>
        <asp:ListItem Value="SL Production TM1">Production TM1</asp:ListItem>
        <asp:ListItem Value="SL Production TM2">Production TM2</asp:ListItem>
        <asp:ListItem Value="SL Purchasing">Purchasing</asp:ListItem>
        <asp:ListItem>Quality</asp:ListItem>
        <asp:ListItem>Safety</asp:ListItem>
        <asp:ListItem Value="SL TPAM">TPAM</asp:ListItem>
        <asp:ListItem>Training</asp:ListItem>
        
    </asp:DropDownList><br />
            <asp:TextBox ID="txtNotes" runat="server" MaxLength="450" Height="200px" Width="300px" TextMode="MultiLine"></asp:TextBox><br /><br />
            <asp:Button ID="btnUpdate" runat="server" Text="Update" />&nbsp
    <asp:Button ID="btnLoadCurrent" runat="server" Text="Load Current" />&nbsp
    <asp:Button ID="btnCharCount" runat="server" Text="Character Count" Visible="False" /><asp:TextBox ID="txtCharCount" runat="server" Enabled="false" Visible="False"></asp:TextBox>
            </div>
            </div>
           
    <asp:SqlDataSource ID="DeptList" runat="server" ConnectionString="<%$ ConnectionStrings:RosenboomConnectionString %>" SelectCommand="SELECT [Department], [Label] FROM [StartupBlog]"></asp:SqlDataSource>
    <asp:SqlDataSource ID="Notes" runat="server" ConnectionString="<%$ ConnectionStrings:RosenboomConnectionString %>" SelectCommand="SELECT [Message] FROM [StartupBlog] WHERE ([Department] = @Department)">
        <SelectParameters>
            <asp:ControlParameter ControlID="DropDownList1" Name="Department" PropertyName="SelectedValue" Type="String" />
        </SelectParameters>
    </asp:SqlDataSource>

</asp:Content>
