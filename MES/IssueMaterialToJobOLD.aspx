<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" Title="Rosenboom Connect - Golden Ticket" %>

<script runat="server">


    Protected Sub btnSubmit_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim strWarehouse As String, strPlant As String, strBinNum As String
        'Check to see if this is a good ticket
        If Good_Ticket() = "true" Then
        Else
            lblMessage.ForeColor = Drawing.Color.Red
            lblMessage.Text = Good_Ticket()
            Exit Sub
        End If
        'Determine plant and warehouse code
        If txtDept.Text.Substring(0, 1) = "S" Or txtDept.Text.Substring(0, 1) = "s" Then
            strPlant = "SPIRITLA"
            strWarehouse = "SPL"
        Else
            strPlant = "MfgSys"
            strWarehouse = "SHE"
        End If
        'Determine from bin
        If txtFromBin.Text = "" Then
            strBinNum = txtDept.Text
        Else
            strBinNum = txtFromBin.Text
        End If
        'Set File Path
        Dim rand As New Random, letter As String = "", n As Integer, strFilePath As String = _
            "\\pithos\Company\Service Connect\Rosenboom Connect\Issue Material\", strIssuedComplete As String, strReference As String
        'Get random file name
        For n = 0 To 8
            letter &= ChrW(rand.Next(Asc("A"), Asc("Z") + 1))
        Next
        strFilePath &= letter & ".csv"
        

        'Determine if it is issued complete or not
        If chkIssuedComplete.Checked = True Then
            strIssuedComplete = "true"
        Else
            strIssuedComplete = "false"
        End If
        
        Dim CSVFile As New IO.StreamWriter(strFilePath)
        CSVFile.WriteLine("PartNum,TranQty,WarehouseCode,BinNum,IssuedComplete,JobNum,AssemblySeq,MtlSeq,Plant,EmpID,ResourceGroup,Reference")
        
        'Check each box for zeros
        If txtShortEnds.Text = "" Then
        ElseIf txtShortEnds.Text = 0 Then
        Else
            strReference = "ser"
            CSVFile.WriteLine(txtMtlNum.Text & "," & txtShortEnds.Text & "," & strWarehouse & "," & strBinNum & ",false," & _
                              txtJobNum.Text & "," & txtAsm.Text & "," & txtMtlNum.Text & "," & strPlant & "," & _
                              txtEmpID.Text & "," & txtDept.Text & "," & strReference)
        End If
        If txtOverProduction.Text = "" Then
        ElseIf txtOverProduction.Text = 0 Then
        Else
            strReference = "overpro"
            CSVFile.WriteLine(txtMtlNum.Text & "," & txtOverProduction.Text & "," & strWarehouse & "," & strBinNum & ",false," & _
                              txtJobNum.Text & "," & txtAsm.Text & "," & txtMtlNum.Text & "," & strPlant & "," & _
                              txtEmpID.Text & "," & txtDept.Text & "," & strReference)
        End If
        If txtMaterialSub.Text = "" Then
        ElseIf txtMaterialSub.Text = 0 Then
        Else
            strReference = "matsub"
            CSVFile.WriteLine(txtMtlNum.Text & "," & txtMaterialSub.Text & "," & strWarehouse & "," & strBinNum & ",false," & _
                              txtJobNum.Text & "," & txtAsm.Text & "," & txtMtlNum.Text & "," & strPlant & "," & _
                              txtEmpID.Text & "," & txtDept.Text & "," & strReference)
        End If
        If txtDamagedMtl.Text = "" Then
        ElseIf txtDamagedMtl.Text = 0 Then
        Else
            strReference = "dammat"
            CSVFile.WriteLine(txtMtlNum.Text & "," & txtDamagedMtl.Text & "," & strWarehouse & "," & strBinNum & ",false," & _
                              txtJobNum.Text & "," & txtAsm.Text & "," & txtMtlNum.Text & "," & strPlant & "," & _
                              txtEmpID.Text & "," & txtDept.Text & "," & strReference)
        End If
        If txtScrap.Text = "" Then
        ElseIf txtScrap.Text = 0 Then
        Else
            strReference = "scrap"
            CSVFile.WriteLine(txtMtlNum.Text & "," & txtScrap.Text & "," & strWarehouse & "," & strBinNum & ",false," & _
                              txtJobNum.Text & "," & txtAsm.Text & "," & txtMtlNum.Text & "," & strPlant & "," & _
                              txtEmpID.Text & "," & txtDept.Text & "," & strReference)
        End If
        If txtDrop.Text = "" Then
        ElseIf txtDrop.Text = 0 Then
        Else
            strReference = "drop"
            CSVFile.WriteLine(txtMtlNum.Text & "," & txtDrop.Text & "," & strWarehouse & "," & strBinNum & ",false," & _
                              txtJobNum.Text & "," & txtAsm.Text & "," & txtMtlNum.Text & "," & strPlant & "," & _
                              txtEmpID.Text & "," & txtDept.Text & "," & strReference)
        End If
        If txtNonUsable.Text = "" Then
        ElseIf txtNonUsable.Text = 0 Then
        Else
            strReference = "numtl"
            CSVFile.WriteLine(txtMtlNum.Text & "," & txtNonUsable.Text & "," & strWarehouse & "," & strBinNum & ",false," & _
                              txtJobNum.Text & "," & txtAsm.Text & "," & txtMtlNum.Text & "," & strPlant & "," & _
                              txtEmpID.Text & "," & txtDept.Text & "," & strReference)
        End If
        If txtEngYield.Text = "" Then
        ElseIf txtEngYield.Text = 0 Then
        Else
            strReference = "PO#" & txtPONum.Text.Replace(",", "/") & "HT#" & txtHeatNum.Text.Replace(",", "/")
            CSVFile.WriteLine(txtMtlNum.Text & "," & txtEngYield.Text & "," & strWarehouse & "," & strBinNum & "," & _
                              strIssuedComplete & "," & _
                              txtJobNum.Text & "," & txtAsm.Text & "," & txtMtlNum.Text & "," & strPlant & "," & _
                              txtEmpID.Text & "," & txtDept.Text & "," & strReference)
        End If
        
        CSVFile.Close()
        
        
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
    Function Get_Toal_Material() As Decimal
        Dim decEngYield As Decimal, decDrop As Decimal, decNonUsableMtl As Decimal, decScrap As Decimal, decDamaged As Decimal, _
    decMtlSub As Decimal, decOverProd As Decimal, decShortEnds As Decimal
        decEngYield = Check_for_blank(txtEngYield.Text)
        decNonUsableMtl = Check_for_blank(txtNonUsable.Text)
        decDrop = Check_for_blank(txtDrop.Text)
        decScrap = Check_for_blank(txtScrap.Text)
        decDamaged = Check_for_blank(txtDamagedMtl.Text)
        decMtlSub = Check_for_blank(txtMaterialSub.Text)
        decOverProd = Check_for_blank(txtOverProduction.Text)
        decShortEnds = Check_for_blank(txtShortEnds.Text)
        Return decEngYield + decNonUsableMtl + decDrop + decScrap + decDamaged + decMtlSub + decOverProd + decShortEnds
    End Function
    
    Protected Function Check_for_blank(ByVal strTextboxText As String) As Decimal
        If strTextboxText = "" Then
            Return 0
        Else
            Return strTextboxText
        End If
    End Function
    
    Protected Function Good_Ticket() As String
        Dim decBinQty As Decimal, decTransQty As Decimal, _
            strPlant As String, strWarehouse As String, strBinNum As String, strPartNum As String

        If txtDept.Text = "" Then
            Return "Please Enter a Resource Group (WC)."
        ElseIf txtAsm.Text = "" Then
            Return "Please Enter an Assembly #"
        ElseIf txtJobNum.Text = "" Then
            Return "Please Enter a Job #."
        ElseIf txtMtlNum.Text = "" Then
            Return "Please Enter a Material #."
        ElseIf txtEmpID.Text = "" Then
            Return "Please Enter an EmployeeID."
        ElseIf txtPONum.Text = "" Then
            Return "Please Enter a PO #."
        ElseIf txtHeatNum.Text = "" Then
            Return "Please Enter a Heat #."
        Else
            'no blanks
        End If
        
        'Determine plant and warehouse code
        If txtDept.Text.Substring(0, 1) = "S" Or txtDept.Text.Substring(0, 1) = "s" Then
            strPlant = "SPIRITLA"
            strWarehouse = "SPL"
        Else
            strPlant = "MfgSys"
            strWarehouse = "SHE"
        End If
        'Determine from bin
        If txtFromBin.Text = "" Then
            strBinNum = txtDept.Text
        Else
            strBinNum = txtFromBin.Text
        End If
        'Determine part number
        strPartNum = wsConnect.Get_Material_PartNum(txtJobNum.Text, txtAsm.Text, txtMtlNum.Text)
        
        'Check part number
        If strPartNum = "false" Then
            Return "Please enter a valid mtlseq for this job assembly."
        Else
        End If
        'Check Bin Qty
        Try
            decBinQty = wsConnect.Get_Bin_Qty(strWarehouse, strBinNum, strPartNum)
        Catch ex As Exception
            decBinQty = 0
        End Try
        If decBinQty < Get_Toal_Material() Then
            Return "Bin does not have enough qty to process this transaction."
        Else
            'we have enough onhandqty
        End If
        
        'Check if job is real and open
        If wsConnect.Check_Open_Job(txtJobNum.Text) = False Then
            Return "Job entered is either incorrect or closed."
        Else
            'open and exists
        End If
        
        'Check if employee exists
        If wsConnect.Get_Employee_Shift(txtEmpID.Text) = 0 Then
            Return "Employee is not valid."
        Else
            Return "true"
        End If
        
    End Function

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim MESTab As System.Web.UI.HtmlControls.HtmlGenericControl
        MESTab = Master.FindControl("mestab")
        MESTab.Attributes.Add("class", "active")
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
                Issue Material to Job</h1>
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

