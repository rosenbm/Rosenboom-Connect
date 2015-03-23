<%@ Page Title="Rosenboom Connect - Part Parameter Update" Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeFile="PartPlantParameters.aspx.vb" Inherits="Import_PPP" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div class="wrapper col3">
    <div id="intro" >
        <h4>Part Plant Parameters Import</h4>
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
            <asp:Timer ID="Timer1" runat="server" Interval="100" Enabled="false"></asp:Timer>

            <!-- The Label which used to display the progress and the result -->
            
             <asp:Label ID="lblImportComplete" runat="server" Text=""></asp:Label>
            <br /><br />

            <asp:Button ID="btnStart" runat="server" Text="Import Data"/>
            <br /><br />
            <asp:TextBox ID="txtErrorLog" runat="server" TextMode="MultiLine" Height="178px" Width="371px"></asp:TextBox>
            <asp:Panel ID="progresspanel" runat="server">
            <div id="progressdiv" >
                Loading...
                <br /><br /><br /><br />
                <asp:Label ID="lbProgress" runat="server" Text=""></asp:Label><br />
                <asp:Image ID="Image1" runat="server" ImageUrl="/images/ajax-loader.gif"/>

            </div>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
        
    </div>
</div>
</asp:Content>

