<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" Title="Rosenboom Connect - Golden Ticket" %>

<script runat="server">


    Protected Sub btnSubmit_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        
        'Clear Form
        Clear_Textboxes()
        
        lblMessage.ForeColor = Drawing.Color.Black
        lblMessage.Text = "Material ticket has been submitted."
        

    End Sub
   
    Protected Sub Clear_Textboxes()
        txtOverProduction.Text = ""
        txtMaterialSub.Text = ""
        txtDamagedMtl.Text = ""
        txtScrap.Text = ""
        txtDrop.Text = ""
        txtNonUsable.Text = ""
        txtEngYield.Text = ""
        txtDept.Text = ""
        txtJobNum.Text = ""
        txtAsm.Text = ""
        txtMtlNum.Text = ""
        txtEmpID.Text = ""
        txtPONum.Text = ""
        txtHeatNum.Text = ""
        txtFromBin.Text = ""
        txtShortEnds.Text = ""
    End Sub
    
    
    Protected Function Check_for_blank(ByVal strTextboxText As String) As Decimal
        If strTextboxText = "" Then
            Return 0
        Else
            Return strTextboxText
        End If
    End Function
    

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs)

    End Sub

    Protected Sub btnClear_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        lblMessage.Text = ""
    End Sub
</script>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div class="wrapper col3 covergolden">
<div id="intro">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
            <h1>
                Issue Material to Job (TEST)</h1>
                <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                <ContentTemplate>
                <asp:Label ID="lblMessage" runat="server" Text=""></asp:Label>
                 </ContentTemplate>
                <Triggers>
                <asp:AsyncPostBackTrigger ControlID="btnSubmit" EventName="Click" />
                </Triggers>
                </asp:UpdatePanel>
                
                <div class="col3 golden">
                    
            <table class="golden">
            <tr>
                    <td class="right">
                        *Resource Group (WC)
                    </td>
                    <td>
                        <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                <ContentTemplate>
               <asp:TextBox ID="txtDept" runat="server"></asp:TextBox>
                 </ContentTemplate>
                <Triggers>
                <asp:AsyncPostBackTrigger ControlID="btnSubmit" EventName="Click" />
                </Triggers>
                </asp:UpdatePanel>
                        
                    </td>
                </tr>
                <tr>
                    <td class="right">
                        *Job #
                    </td>
                    <td>
                    <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                <ContentTemplate>
               <asp:TextBox ID="txtJobNum" runat="server"></asp:TextBox>
                 </ContentTemplate>
                <Triggers>
                <asp:AsyncPostBackTrigger ControlID="btnSubmit" EventName="Click" />
                </Triggers>
                </asp:UpdatePanel>
                        
                    </td>
                </tr>
                <tr>
                    <td class="right">
                        *Assembly #
                    </td>
                    <td>
                    <asp:UpdatePanel ID="UpdatePanel6" runat="server">
                <ContentTemplate>
               <asp:TextBox ID="txtAsm" runat="server"></asp:TextBox>
                 </ContentTemplate>
                <Triggers>
                <asp:AsyncPostBackTrigger ControlID="btnSubmit" EventName="Click" />
                </Triggers>
                </asp:UpdatePanel>
                        
                    </td>
                </tr>
                <tr>
                    <td class="right">
                        *Mtl Seq #
                    </td>
                    <td>
                    <asp:UpdatePanel ID="UpdatePanel7" runat="server">
                <ContentTemplate>
               <asp:TextBox ID="txtMtlNum" runat="server"></asp:TextBox>
                 </ContentTemplate>
                <Triggers>
                <asp:AsyncPostBackTrigger ControlID="btnSubmit" EventName="Click" />
                </Triggers>
                </asp:UpdatePanel>
                        
                    </td>
                </tr>
                <tr>
                    <td class="right">
                        *Employee ID
                    </td>
                    <td>
                    <asp:UpdatePanel ID="UpdatePanel8" runat="server">
                <ContentTemplate>
                    
               <asp:TextBox ID="txtEmpID" runat="server"></asp:TextBox>
                 </ContentTemplate>
                <Triggers>
                <asp:AsyncPostBackTrigger ControlID="btnSubmit" EventName="Click" />
                </Triggers>
                </asp:UpdatePanel>
                        
                    </td>
                </tr>
                <tr>
                    <td class="right">
                        *PO #
                    </td>
                    <td>
                    <asp:UpdatePanel ID="UpdatePanel9" runat="server">
                <ContentTemplate>
               <asp:TextBox ID="txtPONum" runat="server"></asp:TextBox>
                 </ContentTemplate>
                <Triggers>
                <asp:AsyncPostBackTrigger ControlID="btnSubmit" EventName="Click" />
                </Triggers>
                </asp:UpdatePanel>
                        
                    </td>
                </tr><tr>
                    <td class="right">
                        *Heat #
                    </td>
                    <td>
                    <asp:UpdatePanel ID="UpdatePanel10" runat="server">
                <ContentTemplate>
               <asp:TextBox ID="txtHeatNum" runat="server"></asp:TextBox>
                 </ContentTemplate>
                <Triggers>
                <asp:AsyncPostBackTrigger ControlID="btnSubmit" EventName="Click" />
                </Triggers>
                </asp:UpdatePanel>
                        
                    </td>
                </tr>
                <tr>
                    <td class="right">
                        From Bin (if not RG/WC)
                    </td>
                    <td>
                    <asp:UpdatePanel ID="UpdatePanel11" runat="server">
                <ContentTemplate>
               <asp:TextBox ID="txtFromBin" runat="server"></asp:TextBox>
                 </ContentTemplate>
                <Triggers>
                <asp:AsyncPostBackTrigger ControlID="btnSubmit" EventName="Click" />
                </Triggers>
                </asp:UpdatePanel>
                        
                    </td>
                </tr>
            </table>
            * Denotes required field
            </div>
            <div class="col3 golden">
                
            <table class="golden">
            <tr><td class="right">Required Qty</td><td>
            <asp:UpdatePanel ID="UpdatePanel12" runat="server">
                <ContentTemplate>
               <asp:TextBox ID="txtEngYield" runat="server"></asp:TextBox>
                 </ContentTemplate>
                <Triggers>
                <asp:AsyncPostBackTrigger ControlID="btnSubmit" EventName="Click" />
                </Triggers>
                </asp:UpdatePanel></td></tr>
            <tr><td class="right">Non-Usable Material</td><td>
            <asp:UpdatePanel ID="UpdatePanel13" runat="server">
                <ContentTemplate>
               <asp:TextBox ID="txtNonUsable" runat="server"></asp:TextBox>
                 </ContentTemplate>
                <Triggers>
                <asp:AsyncPostBackTrigger ControlID="btnSubmit" EventName="Click" />
                </Triggers>
                </asp:UpdatePanel></td></tr>
            <tr><td class="right">Drop</td><td>
            <asp:UpdatePanel ID="UpdatePanel14" runat="server">
                <ContentTemplate>
               <asp:TextBox ID="txtDrop" runat="server"></asp:TextBox>
                 </ContentTemplate>
                <Triggers>
                <asp:AsyncPostBackTrigger ControlID="btnSubmit" EventName="Click" />
                </Triggers>
                </asp:UpdatePanel>
            </td></tr>
            <tr><td class="right">Scrap</td><td>
            <asp:UpdatePanel ID="UpdatePanel15" runat="server">
                <ContentTemplate>
               <asp:TextBox ID="txtScrap" runat="server"></asp:TextBox>
                 </ContentTemplate>
                <Triggers>
                <asp:AsyncPostBackTrigger ControlID="btnSubmit" EventName="Click" />
                </Triggers>
                </asp:UpdatePanel>
            </td></tr>
            <tr><td class="right">Damaged Material</td><td>
            <asp:UpdatePanel ID="UpdatePanel16" runat="server">
                <ContentTemplate>
               <asp:TextBox ID="txtDamagedMtl" runat="server"></asp:TextBox>
                 </ContentTemplate>
                <Triggers>
                <asp:AsyncPostBackTrigger ControlID="btnSubmit" EventName="Click" />
                </Triggers>
                </asp:UpdatePanel>
            </td></tr>
            <tr><td class="right">Material Substitution</td><td>
            <asp:UpdatePanel ID="UpdatePanel17" runat="server">
                <ContentTemplate>
               <asp:TextBox ID="txtMaterialSub" runat="server"></asp:TextBox>
                 </ContentTemplate>
                <Triggers>
                <asp:AsyncPostBackTrigger ControlID="btnSubmit" EventName="Click" />
                </Triggers>
                </asp:UpdatePanel>
            </td></tr>
            <tr><td class="right">Overproduction</td><td>
            <asp:UpdatePanel ID="UpdatePanel18" runat="server">
                <ContentTemplate>
               <asp:TextBox ID="txtOverProduction" runat="server"></asp:TextBox>
                 </ContentTemplate>
                <Triggers>
                <asp:AsyncPostBackTrigger ControlID="btnSubmit" EventName="Click" />
                </Triggers>
                </asp:UpdatePanel>
            </td></tr>
                        <tr><td class="right">Consumed Short Ends</td><td>
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
               <asp:TextBox ID="txtShortEnds" runat="server"></asp:TextBox>
                 </ContentTemplate>
                <Triggers>
                <asp:AsyncPostBackTrigger ControlID="btnSubmit" EventName="Click" />
                </Triggers>
                </asp:UpdatePanel>
            </td></tr>      
            </table>

            <div class="center">
            <asp:CheckBox ID="chkIssuedComplete" runat="server" text="Issued Complete"/>
            <br /><br />

<asp:Button ID="btnSubmit" runat="server" Text="Submit Ticket"   onclick="btnSubmit_Click" /> &nbsp;&nbsp;&nbsp;
<asp:Button ID="btnClear" runat="server" Text="Clear" onclick="btnClear_Click" />
                </div>
                                               
                <asp:UpdateProgress ID="UpdateProgress1" runat="server">
                <ProgressTemplate>
                <asp:Image ID="imgLoading" runat="server" ImageUrl="/Images/loading.gif" />
                </ProgressTemplate>
                </asp:UpdateProgress>
            </div>

    
    
    
    
            </div>
            </div>
           
    
</asp:Content>

