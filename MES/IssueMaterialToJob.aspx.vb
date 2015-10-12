Imports System.Configuration
Imports System.Xml
Imports System.IO
Imports System.Text
Imports System.Data
Imports Ice.Lib.Framework
Imports Erp.Proxy.BO
Imports Ice.Core.Session
Imports Erp.BO
Imports Ice.Proxy.BO
Imports Ice.BO

Partial Class MES_NEW_IssueMaterial
    Inherits System.Web.UI.Page
    Dim E10session As Ice.Core.Session
    Dim iLaunch As Ice.Lib.Framework.ILauncher
    Dim PartBinSearchBO As PartBinSearchImpl
    Dim JobEntryBO As JobEntryImpl
    Dim EmpBasicBO As EmpBasicImpl
    Dim IssueReturnBO As IssueReturnImpl


    Protected Sub Start_E10_Session(sUser As String)
        Dim sPass As String = "DEMETER@!"
        E10session = New Ice.Core.Session(sUser, sPass, LicenseType.Default, "\\olympus\ERP10\ERP10.0.700\ClientDeployment\Client\Config\RMT-SHIA-APP03.sysconfig")
        iLaunch = New Ice.Lib.Framework.ILauncher(E10session)
        PartBinSearchBO = WCFServiceSupport.CreateImpl(Of PartBinSearchImpl)(E10session, PartBinSearchImpl.UriPath)
        JobEntryBO = WCFServiceSupport.CreateImpl(Of JobEntryImpl)(E10session, JobEntryImpl.UriPath)
        EmpBasicBO = WCFServiceSupport.CreateImpl(Of EmpBasicImpl)(E10session, EmpBasicImpl.UriPath)
        IssueReturnBO = WCFServiceSupport.CreateImpl(Of IssueReturnImpl)(E10session, IssueReturnImpl.UriPath)
    End Sub

    Protected Sub btnSubmit_Click(sender As Object, e As EventArgs) Handles btnSubmit.Click
        Dim strWarehouse As String, strPlant As String, strBinNum As String, strUser As String

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
            strUser = "sc"
        Else
            strPlant = "MfgSys"
            strWarehouse = "SHE"
            strUser = "shsc"
        End If

        If E10session.PlantID <> strPlant Then
            Start_E10_Session(strUser)
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

        'Dim CSVFile As New IO.StreamWriter(strFilePath)
        'CSVFile.WriteLine("PartNum,TranQty,WarehouseCode,BinNum,IssuedComplete,JobNum,AssemblySeq,MtlSeq,Plant,EmpID,ResourceGroup,Reference")

        'Check each box for zeros
        'If txtShortEnds.Text = "" Then
        'ElseIf txtShortEnds.Text = 0 Then
        'Else
        '    strReference = "ser"
        '    'CSVFile.WriteLine(txtMtlNum.Text & "," & txtShortEnds.Text & "," & strWarehouse & "," & strBinNum & ",false," & _
        '    'txtJobNum.Text & "," & txtAsm.Text & "," & txtMtlNum.Text & "," & strPlant & "," & _
        '    'txtEmpID.Text & "," & txtDept.Text & "," & strReference)

        '    IssueMaterial(strReference, strUser, txtShortEnds.Text, strWarehouse, strBinNum)


        'End If
        If txtOverProduction.Text = "" Then
        ElseIf txtOverProduction.Text = 0 Then
        Else
            strReference = "overpro"
            'CSVFile.WriteLine(txtMtlNum.Text & "," & txtOverProduction.Text & "," & strWarehouse & "," & strBinNum & ",false," & _
            '                  txtJobNum.Text & "," & txtAsm.Text & "," & txtMtlNum.Text & "," & strPlant & "," & _
            '                  txtEmpID.Text & "," & txtDept.Text & "," & strReference)
            IssueMaterial(strReference, strUser, txtOverProduction.Text, strWarehouse, strBinNum)

        End If
        'If txtMaterialSub.Text = "" Then
        'ElseIf txtMaterialSub.Text = 0 Then
        'Else
        '    strReference = "matsub"
        '    'CSVFile.WriteLine(txtMtlNum.Text & "," & txtMaterialSub.Text & "," & strWarehouse & "," & strBinNum & ",false," & _
        '    '                  txtJobNum.Text & "," & txtAsm.Text & "," & txtMtlNum.Text & "," & strPlant & "," & _
        '    '                  txtEmpID.Text & "," & txtDept.Text & "," & strReference)
        '    IssueMaterial(strReference, strUser, txtMaterialSub.Text, strWarehouse, strBinNum)

        'End If
        If txtDamagedMtl.Text = "" Then
        ElseIf txtDamagedMtl.Text = 0 Then
        Else
            strReference = "dammat"
            'CSVFile.WriteLine(txtMtlNum.Text & "," & txtDamagedMtl.Text & "," & strWarehouse & "," & strBinNum & ",false," & _
            '                  txtJobNum.Text & "," & txtAsm.Text & "," & txtMtlNum.Text & "," & strPlant & "," & _
            '                  txtEmpID.Text & "," & txtDept.Text & "," & strReference)
            IssueMaterial(strReference, strUser, txtDamagedMtl.Text, strWarehouse, strBinNum)

        End If
        If txtScrap.Text = "" Then
        ElseIf txtScrap.Text = 0 Then
        Else
            strReference = "scrap"
            'CSVFile.WriteLine(txtMtlNum.Text & "," & txtScrap.Text & "," & strWarehouse & "," & strBinNum & ",false," & _
            '                  txtJobNum.Text & "," & txtAsm.Text & "," & txtMtlNum.Text & "," & strPlant & "," & _
            '                  txtEmpID.Text & "," & txtDept.Text & "," & strReference)
            IssueMaterial(strReference, strUser, txtScrap.Text, strWarehouse, strBinNum)

        End If
        If txtDrop.Text = "" Then
        ElseIf txtDrop.Text = 0 Then
        Else
            strReference = "drop"
            'CSVFile.WriteLine(txtMtlNum.Text & "," & txtDrop.Text & "," & strWarehouse & "," & strBinNum & ",false," & _
            '                  txtJobNum.Text & "," & txtAsm.Text & "," & txtMtlNum.Text & "," & strPlant & "," & _
            '                  txtEmpID.Text & "," & txtDept.Text & "," & strReference)
            IssueMaterial(strReference, strUser, txtDrop.Text, strWarehouse, strBinNum)

        End If
        If txtNonUsable.Text = "" Then
        ElseIf txtNonUsable.Text = 0 Then
        Else
            strReference = "numtl"
            'CSVFile.WriteLine(txtMtlNum.Text & "," & txtNonUsable.Text & "," & strWarehouse & "," & strBinNum & ",false," & _
            '                  txtJobNum.Text & "," & txtAsm.Text & "," & txtMtlNum.Text & "," & strPlant & "," & _
            '                  txtEmpID.Text & "," & txtDept.Text & "," & strReference)
            IssueMaterial(strReference, strUser, txtNonUsable.Text, strWarehouse, strBinNum)

        End If
        If txtEngYield.Text = "" Then
        ElseIf txtEngYield.Text = 0 Then
        Else
            strReference = "PO#" & txtPONum.Text.Replace(",", "/") & "HT#" & txtHeatNum.Text.Replace(",", "/")
            'CSVFile.WriteLine(txtMtlNum.Text & "," & txtEngYield.Text & "," & strWarehouse & "," & strBinNum & "," & _
            '                  strIssuedComplete & "," & _
            '                  txtJobNum.Text & "," & txtAsm.Text & "," & txtMtlNum.Text & "," & strPlant & "," & _
            '                  txtEmpID.Text & "," & txtDept.Text & "," & strReference)
            IssueMaterial(strReference, strUser, txtEngYield.Text, strWarehouse, strBinNum)
        End If

        'CSVFile.Close()


        'Clear Form
        Clear_Textboxes()

        lblMessage.ForeColor = Drawing.Color.Black
        lblMessage.Text = "Material ticket has been submitted."
    End Sub

    Protected Sub btnClear_Click(sender As Object, e As EventArgs) Handles btnClear.Click
        lblMessage.Text = ""
        Clear_Textboxes()
    End Sub

    Protected Sub Clear_Textboxes()
        txtOverProduction.Text = ""
        'txtMaterialSub.Text = ""
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
        'txtShortEnds.Text = ""
    End Sub

    Protected Sub IssueMaterial(strReference As String, strUser As String, decTranQty As Decimal, strWarehouse As String, strBinNum As String)
        Dim dsIssueReturn As New IssueReturnDataSet
        'Get New Issue Return
        dsIssueReturn = IssueReturn_GetNewIssueReturnToJob(strUser, txtJobNum.Text, txtAsm.Text)

        'Change Job
        dsIssueReturn.Tables(0).Rows(0)("ToJobNum") = txtJobNum.Text
        IssueReturnBO.OnChangeToJobNum(dsIssueReturn, "IssueMaterial", "")

        'Change Asm
        dsIssueReturn.Tables(0).Rows(0)("ToAssemblySeq") = txtAsm.Text
        IssueReturnBO.OnChangeToAssemblySeq(dsIssueReturn, "IssueMaterial")

        decTranQty = Math.Round(decTranQty, 2)
        'On Change To Job Seq
        dsIssueReturn.Tables(0).Rows(0)("ToJobSeq") = txtMtlNum.Text
        dsIssueReturn.Tables(0).Rows(0)("RowMod") = "U"
        dsIssueReturn = IssueReturn_OnChangeToJobSeq(strUser, dsIssueReturn)
        'dsIssueReturn.WriteXml("Y:\_PCB Transfer\Austin\IssueReturn2.xml")

        'On Change Tran Qty
        dsIssueReturn.Tables(0).Rows(0)("TranQty") = decTranQty
        dsIssueReturn.Tables(0).Rows(0)("TranReference") = strReference & "-" & txtEmpID.Text
        dsIssueReturn = IssueReturn_OnChangeTranQty(strUser, dsIssueReturn, decTranQty)
        'dsIssueReturn.WriteXml("Y:\_PCB Transfer\Austin\IssueReturn3.xml")

        'On Change From Warehouse
        dsIssueReturn.Tables(0).Rows(0)("FromWarehouseCode") = strWarehouse
        dsIssueReturn = IssueReturn_OnChangeFromWarehouse(strUser, dsIssueReturn, strWarehouse)
        'dsIssueReturn.WriteXml("Y:\_PCB Transfer\Austin\IssueReturn4.xml")

        'On Change From BinNum
        dsIssueReturn.Tables(0).Rows(0)("FromBinNum") = strBinNum
        dsIssueReturn = IssueReturn_OnChangeFromBinNum(strUser, dsIssueReturn, strBinNum)
        'dsIssueReturn.WriteXml("Y:\_PCB Transfer\Austin\IssueReturn5.xml")

        'PrePerform Material Movement
        dsIssueReturn = IssueReturn_PrePerformMaterialMovement(strUser, dsIssueReturn)
        'dsIssueReturn.WriteXml("Y:\_PCB Transfer\Austin\IssueReturn6.xml")

        'Perform Material Movement
        dsIssueReturn = IssueReturn_PerformMaterialMovement(strUser, dsIssueReturn)
        'dsIssueReturn.WriteXml("Y:\_PCB Transfer\Austin\IssueReturn7.xml")

    End Sub

    Function Get_Toal_Material() As Decimal
        Dim decEngYield As Decimal, decDrop As Decimal, decNonUsableMtl As Decimal, decScrap As Decimal, decDamaged As Decimal, _
    decMtlSub As Decimal, decOverProd As Decimal, decShortEnds As Decimal
        decEngYield = Check_for_blank(txtEngYield.Text)
        decNonUsableMtl = Check_for_blank(txtNonUsable.Text)
        decDrop = Check_for_blank(txtDrop.Text)
        decScrap = Check_for_blank(txtScrap.Text)
        decDamaged = Check_for_blank(txtDamagedMtl.Text)
        decMtlSub = 0 'Check_for_blank(txtMaterialSub.Text)
        decOverProd = Check_for_blank(txtOverProduction.Text)
        decShortEnds = 0 'Check_for_blank(txtShortEnds.Text)
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
        Dim decBinQty As Decimal, decTransQty As Decimal, dsPartBin As New PartBinSearchDataSet, _
            strPlant As String, strWarehouse As String, strBinNum As String, strPartNum As String
        Dim dsJob As New JobEntryDataSet, dsEmpBasic As New EmpBasicDataSet, dsBAQ As New DataSet, _
            strUser As String, decReqQty As Decimal, decEngYield As Decimal, strUOM As String = "", _
            output As Integer, bolBackflush As Boolean = False

        'CHECK FOR EMPTY FIELDS
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
            strUser = "sc"
        Else
            strPlant = "MfgSys"
            strWarehouse = "SHE"
            strUser = "shsc"
        End If

        'Determine from bin
        If txtFromBin.Text = "" Then
            strBinNum = txtDept.Text
        Else
            strBinNum = txtFromBin.Text
        End If

        'Determine part number
        dsJob = JobEntry_GetByID(txtJobNum.Text)
        For Each row As DataRow In dsJob.Tables("JobMtl").Rows
            If row("MtlSeq") = txtMtlNum.Text And row("AssemblySeq") = txtAsm.Text Then
                strPartNum = row("PartNum")
                decReqQty = row("RequiredQty")
                strUOM = row("IUM")
                bolBackflush = row("Backflush")
                Exit For
            End If
        Next
        If strPartNum = "" Then
            strPartNum = "false"
        End If
        'Check part number
        If strPartNum = "false" Then
            Return "Please enter a valid mtlseq for this job assembly."
        Else
        End If

        'Check if backflushed
        If bolBackflush = True Then
            Return "This is a backflushed material. This is issued by claiming."
        End If

        'Check EA for whole number
        If strUOM = "EA" Then
            If (Integer.TryParse(decTransQty, output)) Then
            Else
                Return "Please issue a whole number."
            End If
        End If


        'Check Bin Qty
        Try
            dsPartBin = PartBinSearch_GetPartBinSearch(strPartNum, strWarehouse)
            For Each row As DataRow In dsPartBin.Tables(0).Rows
                If row("BinNum").ToString.ToUpper = strBinNum.ToUpper Then
                    decBinQty = row("QtyOnHand")
                    Exit For
                End If
            Next
        Catch ex As Exception
            decBinQty = 0
        End Try
        If decBinQty < Get_Toal_Material() Then
            Return "Bin does not have enough qty to process this transaction."
        Else
            'we have enough onhandqty
        End If

        'Check if job is real and open
        If dsJob.Tables("JobHead").Rows(0)("JobClosed") = True Then
            Return "Job entered is either incorrect or closed."
        Else
            'open and exists
        End If

        'Check Eng Yield
        decEngYield = Check_for_blank(txtEngYield.Text)
        If decEngYield > decReqQty And decReqQty > 0 Then
            Return "The maximum engineering yield that can be issued is " & decReqQty & ". The rest must be issued using the other categories below."
        End If

        'Check EA
        If EA_Check() = False Then
            Return "Please enter whole numbers in each box."
        End If

        'Check if employee exists
        dsEmpBasic = EmpBasic_GetByID(txtEmpID.Text)
        If dsEmpBasic.Tables("EmpBasic").Rows.Count = 0 Then
            Return "Employee is not valid."
        Else
            Return "true"
        End If



    End Function

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim MESTab As System.Web.UI.HtmlControls.HtmlGenericControl
        MESTab = Master.FindControl("mestab")
        MESTab.Attributes.Add("class", "active")
        Start_E10_Session("sc")
    End Sub

    Protected Function EA_Check() As Boolean
        Dim dsJob As New JobEntryDataSet, strUOM As String = "", output As Integer

        dsJob = JobEntry_GetByID(txtJobNum.Text)
        For Each row As DataRow In dsJob.Tables("JobMtl").Rows
            If row("MtlSeq") = txtMtlNum.Text And row("AssemblySeq") = txtAsm.Text Then
                strUOM = row("IUM")
                Exit For
            End If
        Next

        'Check EA for whole number
        If strUOM = "EA" Then

            If txtOverProduction.Text = "" Then
            ElseIf txtOverProduction.Text = 0 Then
            Else
                If (Integer.TryParse(txtOverProduction.Text, output)) Then
                Else
                    Return False
                End If
            End If

            If txtDamagedMtl.Text = "" Then
            ElseIf txtDamagedMtl.Text = 0 Then
            Else
                If (Integer.TryParse(txtDamagedMtl.Text, output)) Then
                Else
                    Return False
                End If
            End If

            If txtScrap.Text = "" Then
            ElseIf txtScrap.Text = 0 Then
            Else
                If (Integer.TryParse(txtScrap.Text, output)) Then
                Else
                    Return False
                End If
            End If

            If txtDrop.Text = "" Then
            ElseIf txtDrop.Text = 0 Then
            Else
                If (Integer.TryParse(txtDrop.Text, output)) Then
                Else
                    Return False
                End If
            End If

            If txtNonUsable.Text = "" Then
            ElseIf txtNonUsable.Text = 0 Then
            Else
                If (Integer.TryParse(txtNonUsable.Text, output)) Then
                Else
                    Return False
                End If
            End If

            If txtEngYield.Text = "" Then
            ElseIf txtEngYield.Text = 0 Then
            Else
                If (Integer.TryParse(txtEngYield.Text, output)) Then
                Else
                    Return False
                End If
            End If


        Else 'This is not an EACH. Let it go.
            Return True
        End If 'IF EA

        Return True

    End Function

    Protected Sub btnCheckIssue_Click(sender As Object, e As EventArgs) Handles btnCheckIssue.Click
        Dim dsJob As New JobEntryDataSet, strPN As String = "", myPartDS As New PartBinSearchDataSet, bolFOundOne As Boolean = False, strWhse As String = "", _
            strPlant As String = "", strFromBin As String
        Try
            dsJob = JobEntry_GetByID(txtJobNum.Text)
            strPlant = dsJob.Tables("JobHead").Rows(0)("Plant")
            For Each row As DataRow In dsJob.Tables("JobMtl").Rows
                If row("AssemblySeq") = txtAsm.Text And row("MtlSeq") = txtMtlNum.Text Then
                    lblMessage.Text = row("PartNum") & " - Previously Issued: " & row("IssuedQty") & vbCrLf & " --- Bin Contains: "
                    strPN = row("PartNum")
                    bolFOundOne = True
                    Exit For
                End If
            Next
            If bolFOundOne = False Then
                lblMessage.Text = "Please enter a correct job, assembly, and material sequence."
            End If
        Catch ex As Exception
            lblMessage.Text = "Please enter a correct job, assembly, and material sequence."
        End Try

        Try
            If strPlant = "MfgSys" Then
                strWhse = "SHE"
            Else
                strWhse = "SPL"
            End If

            myPartDS = PartBinSearch_GetPartBinSearch(strPN, strWhse)
            bolFOundOne = False
            If txtFromBin.Text = "" Then
                strFromBin = txtDept.Text
            Else
                strFromBin = txtFromBin.Text
            End If
            For Each row As DataRow In myPartDS.Tables(0).Rows
                If row("BinNum").ToString.ToUpper = strFromBin.ToUpper Then
                    bolFOundOne = True
                    lblMessage.Text &= Math.Round(row("QtyOnHand"), 2)
                    Exit For
                End If
            Next
            If bolFOundOne = False Then
                lblMessage.Text &= 0
            End If
        Catch ex As Exception
            lblMessage.Text &= 0
        End Try
    End Sub

    Function PartBinSearch_GetPartBinSearch(strPartNum As String, strWarehouse As String) As PartBinSearchDataSet
        Dim myPartDS As New PartBinSearchDataSet
        myPartDS = PartBinSearchBO.GetPartBinSearch(0, 0, "WarehouseCode='" & strWarehouse & "' AND PartNum='" & strPartNum & "'", False)
        Return myPartDS
    End Function

    Function JobEntry_GetByID(strJobNum As String) As JobEntryDataSet
        Dim myJobDS As New JobEntryDataSet
        myJobDS = JobEntryBO.GetByID(strJobNum)
        Return myJobDS
    End Function

    Function EmpBasic_GetByID(strEmpID As String) As EmpBasicDataSet
        Dim myEmpBasicDS As New EmpBasicDataSet
        myEmpBasicDS = EmpBasicBO.GetByID(strEmpID)
        Return myEmpBasicDS
    End Function

    Function IssueReturn_GetNewIssueReturnToJob(strUser As String, strJobNum As String, intAsmSeq As Integer) As IssueReturnDataSet
        Dim myIssueReturnDS As New IssueReturnDataSet, dsSelectedJobAsmbl As New SelectedJobAsmblDataSet

        Dim NewRow As DataRow = dsSelectedJobAsmbl.Tables("SelectedJobAsmbl").NewRow
        NewRow("Company") = "RMT"
        NewRow("JobNum") = strJobNum
        NewRow("AssemblySeq") = intAsmSeq
        dsSelectedJobAsmbl.Tables("SelectedJobAsmbl").Rows.Add(NewRow)


        'IssueReturnBO.GetNewIssueReturnToJob(strJobNum, intAsmSeq, "STK-MTL", Guid.NewGuid, "", myIssueReturnDS)
        myIssueReturnDS = IssueReturnBO.GetNewJobAsmblMultiple("STK-MTL", Guid.NewGuid(), "IssueMaterial", dsSelectedJobAsmbl, "")


        Return myIssueReturnDS
    End Function

    Function IssueReturn_OnChangeToJobSeq(strUser As String, dsIssueReturn As IssueReturnDataSet) As IssueReturnDataSet
        IssueReturnBO.OnChangeToJobSeq(dsIssueReturn, "", "")
        Return dsIssueReturn
    End Function

    Function IssueReturn_OnChangeTranQty(strUser As String, dsIssueReturn As IssueReturnDataSet, decTranQty As Decimal) As IssueReturnDataSet
        IssueReturnBO.OnChangeTranQty(decTranQty, dsIssueReturn)
        Return dsIssueReturn
    End Function

    Function IssueReturn_OnChangeFromWarehouse(strUser As String, dsIssueReturn As IssueReturnDataSet, strWarehouse As String) As IssueReturnDataSet
        IssueReturnBO.OnChangeFromWarehouse(dsIssueReturn, "")
        Return dsIssueReturn
    End Function

    Function IssueReturn_OnChangeFromBinNum(strUser As String, dsIssueReturn As IssueReturnDataSet, strBinNum As String) As IssueReturnDataSet
        IssueReturnBO.OnChangeFromBinNum(True, dsIssueReturn)
        Return dsIssueReturn
    End Function

    Function IssueReturn_PrePerformMaterialMovement(strUser As String, dsIssueReturn As IssueReturnDataSet) As IssueReturnDataSet
        IssueReturnBO.PrePerformMaterialMovement(dsIssueReturn, False)
        Return dsIssueReturn
    End Function

    Function IssueReturn_PerformMaterialMovement(strUser As String, dsIssueReturn As IssueReturnDataSet) As IssueReturnDataSet
        IssueReturnBO.PerformMaterialMovement(False, dsIssueReturn, "", "")
        Return dsIssueReturn
    End Function
End Class
