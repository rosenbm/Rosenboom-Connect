<%@ Page Language="VB" AutoEventWireup="false" CodeFile="ProgressbarTEST.aspx.vb" Inherits="Test_ProgressbarTEST" enableSessionState="true"%>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <p><b>Note:</b><br />
    Try to open this page in two browsers which use different sessions and then run the operation at the same time.
    You will learn that each session has its own Background Worker. It is not shared by all users.
    </p><br />

    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>

    <!-- UpdateUpanel let the progress can be updated without updating the whole page (partial update). -->
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            
            <!-- The timer which used to update the progress. -->
            <asp:Timer ID="Timer1" runat="server" Interval="100" Enabled="false" OnTick="Timer1_Tick"></asp:Timer>

            <!-- The Label which used to display the progress and the result -->
            <asp:Image ID="Image1" runat="server" ImageUrl="/Images/ajax-loader.gif" Visible="false" /><br />
            <asp:Label ID="lbProgress" runat="server" Text=""></asp:Label>
            <br />

            <!-- Start the operation by inputting value and clicking the button -->
            Input a parameter: <asp:TextBox ID="txtParameter" runat="server" Text="Hello World"></asp:TextBox><br />
            <asp:Button ID="btnStart" runat="server" Text="Click to Start the Background Worker" OnClick="btnStart_Click" />

        </ContentTemplate>
    </asp:UpdatePanel>
        
    </form>
</body>
</html>
