<%@ Page Title="Forecast Import" Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeFile="ForecastImport.aspx.vb" Inherits="Import_PPP" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div class="wrapper col3">
    <div id="intro" >
        <h4>Customer Forecast Import</h4>
        Select File...<br />
            <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>

    <!-- UpdateUpanel let the progress can be updated without updating the whole page (partial update). -->
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <Triggers>
            <asp:postbacktrigger ControlID="btnStart" />
        </Triggers>
        <ContentTemplate>
            <asp:FileUpload ID="FileUpload1" runat="server" /><br /><br />

            <!-- The timer which used to update the progress. -->
            <asp:Timer ID="Timer1" runat="server" Interval="300" Enabled="false"></asp:Timer>

            <!-- The Label which used to display the progress and the result -->
            
            
            <asp:DropDownList ID="ddlCustomer" runat="server">
                <asp:ListItem>AGCO</asp:ListItem>
                <asp:ListItem>AGCO Sunflower</asp:ListItem>
                <asp:ListItem>Ariens</asp:ListItem>
                <asp:ListItem>Altec</asp:ListItem>
                <asp:ListItem>Allmand</asp:ListItem>
                <asp:ListItem>CAT</asp:ListItem>
                <asp:ListItem>CNH Belo Horizonte</asp:ListItem>
                <asp:ListItem>CNH Benson</asp:ListItem>
                <asp:ListItem>CNH Burlington</asp:ListItem>
                <asp:ListItem>CNH Canada</asp:ListItem>
                <asp:ListItem>CNH Cordoba</asp:ListItem>
                <asp:ListItem>CNH Italia</asp:ListItem>
                <asp:ListItem>CNH Parts</asp:ListItem>
                <asp:ListItem>CNH Piracicaba</asp:ListItem>
                <asp:ListItem>CNH Wichita</asp:ListItem>
                <asp:ListItem>Gehl Madison</asp:ListItem>
                <asp:ListItem>Gehl Waco</asp:ListItem>
                <asp:ListItem>Gehl Yankton</asp:ListItem>
                <asp:ListItem>Hagie</asp:ListItem>
                <asp:ListItem>Heil</asp:ListItem>
                <asp:ListItem>JLG</asp:ListItem>
                <asp:ListItem>John Deere</asp:ListItem>
                <asp:ListItem>Lift Tek</asp:ListItem>
                <asp:ListItem>McNeilus</asp:ListItem>
                <asp:ListItem>OshKosh Mexico</asp:ListItem>
                <asp:ListItem>OTC</asp:ListItem>
                <asp:ListItem>Pierce</asp:ListItem>
                <asp:ListItem>Scranton</asp:ListItem>
                <asp:ListItem>Vermeer</asp:ListItem>
                <asp:ListItem>Volvo</asp:ListItem>
                <asp:ListItem>Wacker-Neuson</asp:ListItem>
                <asp:ListItem>Weiler</asp:ListItem>
                <asp:ListItem>World Class</asp:ListItem>
                <asp:ListItem>Xtreme</asp:ListItem>
            </asp:DropDownList> <br /><br />
            Excel Sheet Name:<br /> <asp:TextBox ID="txtExcelSheetName" runat="server" Text="Sheet1"></asp:TextBox><br /><br />
            <asp:RadioButton ID="rbClearReload" runat="server" Text="Clear and Reload Customer" GroupName="ImportType" Checked="true"/><br />
            <asp:RadioButton ID="rbAddIfNew" runat="server" Text="Add If New" GroupName="ImportType" Checked="false"/><br /><br />
            <asp:Button ID="btnStart" runat="server" Text="Import Data"/><br /><br />

            <asp:Panel ID="progresspanel" runat="server">
            <div id="progressdiv" >
                Loading...
                <br /><br /><br /><br />
                <asp:Label ID="lblStage" runat="server" Text=""></asp:Label><br />
                <asp:Label ID="lbProgress" runat="server" Text=""></asp:Label><br />
                <asp:Image ID="Image1" runat="server" ImageUrl="/images/ajax-loader.gif"/>

            </div>
            </asp:Panel>
            <asp:Label ID="lblImportComplete" runat="server" Text="" Font-Bold="true"></asp:Label>
            <br /><br />
            <asp:TextBox ID="txtErrorLog" runat="server" Height="150px" Width="400px" TextMode="MultiLine"></asp:TextBox>
        </ContentTemplate>
    </asp:UpdatePanel>
    </div>
</div>
</asp:Content>

