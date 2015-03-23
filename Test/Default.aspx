<%@ Page Title="" Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeFile="Default.aspx.vb" Inherits="Test_Default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

    <div class="wrapper col3">

        <asp:TreeView ID="TreeView1" runat="server" ImageSet="Simple" NodeIndent="10">
            <HoverNodeStyle Font-Underline="True" ForeColor="#DD5555" />
            <Nodes>
                <asp:TreeNode Text="Test" Value="Test">
                    <asp:TreeNode Text="Test 1" Value="Test 1"></asp:TreeNode>
                    <asp:TreeNode Text="Test 2" Value="Test 2"></asp:TreeNode>
                </asp:TreeNode>
            </Nodes>
            <NodeStyle Font-Names="Verdana" Font-Size="8pt" ForeColor="Black" HorizontalPadding="0px" NodeSpacing="0px" VerticalPadding="0px" />
            <ParentNodeStyle Font-Bold="False" />
            <SelectedNodeStyle Font-Underline="True" ForeColor="#DD5555" HorizontalPadding="0px" VerticalPadding="0px" />
        </asp:TreeView>

        <asp:TextBox ID="TextBox1" runat="server"></asp:TextBox>

    </div>


</asp:Content>

