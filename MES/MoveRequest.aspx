<%@ Page Language="VB" AutoEventWireup="false" CodeFile="MoveRequest.aspx.vb" Inherits="MES_NEW_IssueMaterial" MasterPageFile="~/MasterPage.master" Title="Rosenboom Connect - Move Request"%>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

    <div class="wrapper col3">
<div id="intro">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
            <h1>Move Request</h1>
                <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                <ContentTemplate>
                <asp:Label ID="lblMessage" runat="server" Text=""></asp:Label>
                 </ContentTemplate>
                <Triggers>
                <asp:AsyncPostBackTrigger ControlID="btnSubmit" EventName="Click" />
                </Triggers>
                </asp:UpdatePanel>
<br />
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
Move Type:<br /><asp:dropdownlist runat="server" ID="dropType">
        <asp:ListItem>Chip Dumpster</asp:ListItem>
        <asp:ListItem>Pallet</asp:ListItem>
        <asp:ListItem>Move Parts</asp:ListItem>
                        <asp:ListItem>Cut Board</asp:ListItem>
                        <asp:ListItem>Banding Material</asp:ListItem>
                        <asp:ListItem>Other</asp:ListItem>
    </asp:dropdownlist>
           <br /><br />
Emp ID:<br /><asp:textbox ID="txtEmpID" runat="server" MaxLength="5"></asp:textbox>
    <br /><br />
                    Work Center:<br /><asp:textbox ID="txtBin" runat="server" MaxLength="10"></asp:textbox>
    <br /><br />
                    Notes:<br /><asp:textbox ID="txtNotes" runat="server" MaxLength="50"></asp:textbox>
    <br /><br />
<asp:Button ID="btnSubmit" runat="server" Text="Submit Ticket"  />
                
                </ContentTemplate>
                <Triggers>
                <asp:AsyncPostBackTrigger ControlID="btnSubmit" EventName="Click" />
                </Triggers>
                </asp:UpdatePanel>
                                                    
                <asp:UpdateProgress ID="UpdateProgress1" runat="server">
                <ProgressTemplate>
                <asp:Image ID="imgLoading" runat="server" ImageUrl="/Images/loading.gif" />
                </ProgressTemplate>
                </asp:UpdateProgress>
                
            </div>
    </div>
    
 
           
    
</asp:Content>
