<%@ Page Title="Rosenboom Connect - NonConf Update" Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeFile="NonConf.aspx.vb" Inherits="Import_PPP" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div class="wrapper col3">
    <div id="intro" >
        <h4>Non Conformance Update</h4>
        <br />
            <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>

    <!-- UpdateUpanel let the progress can be updated without updating the whole page (partial update). -->
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <Triggers>
            <asp:postbacktrigger ControlID="btnOpen" />
            <asp:postbacktrigger ControlID="btnClose" />
        </Triggers>

        <ContentTemplate>
            <br />TranID:<!-- The Label which used to display the progress and the result --><asp:TextBox ID="txtTranID" runat="server"></asp:TextBox>
            <br />
            <!-- The Label which used to display the progress and the result -->
            <asp:Timer ID="Timer1" runat="server" Interval="100" Enabled="false"></asp:Timer>

            <!-- The timer which used to update the progress. -->
            
             <asp:Label ID="lblImportComplete" runat="server" Text=""></asp:Label>
            <br /><br />

            <asp:Button ID="btnOpen" runat="server" Text="Open"/>
            <asp:Button ID="btnClose" runat="server" Text="Close" />
            <br /><br />
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

