<%@ Page Title="Rosenboom Connect - Parts Room Shortages" Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeFile="PartsRoomShortages.aspx.vb" Inherits="Import_PPP" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div class="wrapper col3">
    <div id="intro" >
        <h4>Parts Room Shortages</h4>
        <table class="green">
        <tr><td>JobNum</td><td><asp:textbox runat="server" ID="txtJobNum"></asp:textbox></td></tr>
        <tr><td>Component</td><td><asp:textbox runat="server" ID="txtComponent"></asp:textbox></td></tr>
        <tr><td>Qty</td><td><asp:textbox runat="server" ID="txtQty"></asp:textbox></td></tr>
        <tr><td>Comments</td><td><asp:textbox runat="server" ID="txtComments"></asp:textbox></td></tr>
        </table>
        <br /><br />
        <asp:Button ID="btnUpdate" runat="server" Text="Update" />
        <br /><br />
        <asp:Label ID="lblStatus" runat="server" Text=""></asp:Label>
        <br />
    </div>
</div>
</asp:Content>

