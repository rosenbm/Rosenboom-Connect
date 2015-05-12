Imports Epicor.Mfg.Core
Imports Epicor.Mfg.Shared
Imports Epicor.Mfg.UI
Imports Epicor.Mfg.BO
Imports System.Configuration
Imports System.Xml
Imports System.IO
Imports System.Text
Imports System.Data

Partial Class MES_NEW_IssueMaterial
    Inherits System.Web.UI.Page

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
        dsIssueReturn = BO_IssueReturn.GetNewIssueReturnToJob(strUser, txtJobNum.Text, txtAsm.Text)
        'dsIssueReturn.WriteXml("Y:\_PCB Transfer\Austin\IssueReturn1.xml")

        'On Change To Job Seq
        dsIssueReturn.Tables(0).Rows(0)("ToJobSeq") = txtMtlNum.Text
        dsIssueReturn.Tables(0).Rows(0)("RowMod") = "U"
        dsIssueReturn = BO_IssueReturn.OnChangeToJobSeq(strUser, dsIssueReturn)
        'dsIssueReturn.WriteXml("Y:\_PCB Transfer\Austin\IssueReturn2.xml")

        'On Change Tran Qty
        dsIssueReturn.Tables(0).Rows(0)("TranQty") = decTranQty
        dsIssueReturn.Tables(0).Rows(0)("TranReference") = strReference & "-" & txtEmpID.Text
        dsIssueReturn = BO_IssueReturn.OnChangeTranQty(strUser, dsIssueReturn, decTranQty)
        'dsIssueReturn.WriteXml("Y:\_PCB Transfer\Austin\IssueReturn3.xml")

        'On Change From Warehouse
        dsIssueReturn.Tables(0).Rows(0)("FromWarehouseCode") = strWarehouse
        dsIssueReturn = BO_IssueReturn.OnChangeFromWarehouse(strUser, dsIssueReturn, strWarehouse)
        'dsIssueReturn.WriteXml("Y:\_PCB Transfer\Austin\IssueReturn4.xml")

        'On Change From BinNum
        dsIssueReturn.Tables(0).Rows(0)("FromBinNum") = strBinNum
        dsIssueReturn = BO_IssueReturn.OnChangeFromBinNum(strUser, dsIssueReturn, strBinNum)
        'dsIssueReturn.WriteXml("Y:\_PCB Transfer\Austin\IssueReturn5.xml")

        'PrePerform Material Movement
        dsIssueReturn = BO_IssueReturn.PrePerformMaterialMovement(strUser, dsIssueReturn)
        'dsIssueReturn.WriteXml("Y:\_PCB Transfer\Austin\IssueReturn6.xml")

        'Perform Material Movement
        dsIssueReturn = BO_IssueReturn.PerformMaterialMovement(strUser, dsIssueReturn)
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
            output As Integer

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
        dsJob = BO_JobEntry.Get_By_ID(txtJobNum.Text)
        For Each row As DataRow In dsJob.Tables("JobMtl").Rows
            If row("MtlSeq") = txtMtlNum.Text And row("AssemblySeq") = txtAsm.Text Then
                strPartNum = row("PartNum")
                decReqQty = row("RequiredQty")
                strUOM = row("IUM")
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

        'Check EA for whole number
        If strUOM = "EA" Then
            If (Integer.TryParse(decTransQty, output)) Then
            Else
                Return "Please issue a whole number."
            End If
        End If


        'Check Bin Qty
        Try
            dsPartBin = BO_PartBin.Get_Part_Bin_Search(strPartNum, strWarehouse)
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
        dsEmpBasic = BO_EmpBasic.Get_By_ID(txtEmpID.Text)
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
    End Sub

    Protected Function EA_Check() As Boolean
        Dim dsJob As New JobEntryDataSet, strUOM As String = "", output As Integer

        dsJob = BO_JobEntry.Get_By_ID(txtJobNum.Text)
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
        Dim dsJob As New JobEntryDataSet
        Try
            dsJob = BO_JobEntry.Get_By_ID(txtJobNum.Text)
            For Each row As DataRow In dsJob.Tables("JobMtl").Rows
                If row("AssemblySeq") = txtAsm.Text And row("MtlSeq") = txtMtlNum.Text Then
                    lblMessage.Text = row("PartNum") & " - Previously Issued: " & row("IssuedQty")
                    Exit Sub
                End If
            Next
            lblMessage.Text = "Please enter a correct job, assembly, and material sequence."
        Catch ex As Exception
            lblMessage.Text = "Please enter a correct job, assembly, and material sequence."
        End Try
    End Sub
End Class

Public Class BO_PartBin
    Public Shared Function Get_Part_Bin_Search(strPartNum As String, strWarehouse As String) As PartBinSearchDataSet
        Dim sUser As String = "sc"
        Dim sPass As String = "DEMETER@!"
        Dim sServer As String = "zeus"
        Dim sPort As String = "9408"
        Dim sAppServer As String = String.Format("AppServerDC://{0}:{1}", sServer, sPort)
        Dim sCompany As String = "RMT"
        Dim session As Object = New Epicor.Mfg.Core.Session(sUser, sPass, sAppServer, Epicor.Mfg.Core.Session.LicenseType.Default)
        Dim connPool As New Epicor.Mfg.Core.BLConnectionPool(sUser, sPass, sAppServer)

        Dim myPart As New PartBinSearch(connPool)
        Dim myPartDS As New PartBinSearchDataSet

        myPartDS = myPart.GetPartBinSearch(strPartNum, strWarehouse, "", "", False, "", "", 0, 0, 0, False)

        connPool.Dispose()

        Return myPartDS
    End Function
End Class

Public Class BO_JobEntry
    Public Shared Function Get_By_ID(strJobNum As String) As JobEntryDataSet
        Dim sUser As String = "sc"
        Dim sPass As String = "DEMETER@!"
        Dim sServer As String = "zeus"
        Dim sPort As String = "9408"
        Dim sAppServer As String = String.Format("AppServerDC://{0}:{1}", sServer, sPort)
        Dim sCompany As String = "RMT"
        Dim session As Object = New Epicor.Mfg.Core.Session(sUser, sPass, sAppServer, Epicor.Mfg.Core.Session.LicenseType.Default)
        Dim connPool As New Epicor.Mfg.Core.BLConnectionPool(sUser, sPass, sAppServer)

        Dim myJob As New JobEntry(connPool)
        Dim myJobDS As New JobEntryDataSet

        myJobDS = myJob.GetByID(strJobNum)

        connPool.Dispose()

        Return myJobDS
    End Function
End Class

Public Class BO_EmpBasic
    Public Shared Function Get_By_ID(strEmpID As String) As EmpBasicDataSet
        Dim sUser As String = "sc"
        Dim sPass As String = "DEMETER@!"
        Dim sServer As String = "zeus"
        Dim sPort As String = "9408"
        Dim sAppServer As String = String.Format("AppServerDC://{0}:{1}", sServer, sPort)
        Dim sCompany As String = "RMT"
        Dim session As Object = New Epicor.Mfg.Core.Session(sUser, sPass, sAppServer, Epicor.Mfg.Core.Session.LicenseType.Default)
        Dim connPool As New Epicor.Mfg.Core.BLConnectionPool(sUser, sPass, sAppServer)

        Dim myEmpBasic As New EmpBasic(connPool)
        Dim myEmpBasicDS As New EmpBasicDataSet

        myEmpBasicDS = myEmpBasic.GetByID(strEmpID)

        connPool.Dispose()

        Return myEmpBasicDS
    End Function
End Class

Public Class BO_IssueReturn
    Public Shared Function GetNewIssueReturnToJob(strUser As String, strJobNum As String, intAsmSeq As Integer) As IssueReturnDataSet
        Dim sUser As String = strUser
        Dim sPass As String = "DEMETER@!"
        Dim sServer As String = "zeus"
        Dim sPort As String = "9408"
        Dim sAppServer As String = String.Format("AppServerDC://{0}:{1}", sServer, sPort)
        Dim sCompany As String = "RMT"
        Dim session As Object = New Epicor.Mfg.Core.Session(sUser, sPass, sAppServer, Epicor.Mfg.Core.Session.LicenseType.Default)
        Dim connPool As New Epicor.Mfg.Core.BLConnectionPool(sUser, sPass, sAppServer)

        Dim myIssueReturn As New IssueReturn(connPool)
        Dim myIssueReturnDS As New IssueReturnDataSet

        myIssueReturn.GetNewIssueReturnToJob(strJobNum, intAsmSeq, "STK-MTL", "?", "", myIssueReturnDS)

        connPool.Dispose()

        Return myIssueReturnDS
    End Function

    Public Shared Function OnChangeToJobSeq(strUser As String, dsIssueReturn As IssueReturnDataSet) As IssueReturnDataSet
        Dim sUser As String = strUser
        Dim sPass As String = "DEMETER@!"
        Dim sServer As String = "zeus"
        Dim sPort As String = "9408"
        Dim sAppServer As String = String.Format("AppServerDC://{0}:{1}", sServer, sPort)
        Dim sCompany As String = "RMT"
        Dim session As Object = New Epicor.Mfg.Core.Session(sUser, sPass, sAppServer, Epicor.Mfg.Core.Session.LicenseType.Default)
        Dim connPool As New Epicor.Mfg.Core.BLConnectionPool(sUser, sPass, sAppServer)

        Dim myIssueReturn As New IssueReturn(connPool)

        myIssueReturn.OnChangeToJobSeq(dsIssueReturn, "", "")

        connPool.Dispose()

        Return dsIssueReturn
    End Function

    Public Shared Function OnChangeTranQty(strUser As String, dsIssueReturn As IssueReturnDataSet, decTranQty As Decimal) As IssueReturnDataSet
        Dim sUser As String = strUser
        Dim sPass As String = "DEMETER@!"
        Dim sServer As String = "zeus"
        Dim sPort As String = "9408"
        Dim sAppServer As String = String.Format("AppServerDC://{0}:{1}", sServer, sPort)
        Dim sCompany As String = "RMT"
        Dim session As Object = New Epicor.Mfg.Core.Session(sUser, sPass, sAppServer, Epicor.Mfg.Core.Session.LicenseType.Default)
        Dim connPool As New Epicor.Mfg.Core.BLConnectionPool(sUser, sPass, sAppServer)

        Dim myIssueReturn As New IssueReturn(connPool)

        myIssueReturn.OnChangeTranQty(decTranQty, dsIssueReturn)

        connPool.Dispose()

        Return dsIssueReturn
    End Function

    Public Shared Function OnChangeFromWarehouse(strUser As String, dsIssueReturn As IssueReturnDataSet, strWarehouse As String) As IssueReturnDataSet
        Dim sUser As String = strUser
        Dim sPass As String = "DEMETER@!"
        Dim sServer As String = "zeus"
        Dim sPort As String = "9408"
        Dim sAppServer As String = String.Format("AppServerDC://{0}:{1}", sServer, sPort)
        Dim sCompany As String = "RMT"
        Dim session As Object = New Epicor.Mfg.Core.Session(sUser, sPass, sAppServer, Epicor.Mfg.Core.Session.LicenseType.Default)
        Dim connPool As New Epicor.Mfg.Core.BLConnectionPool(sUser, sPass, sAppServer)

        Dim myIssueReturn As New IssueReturn(connPool)

        myIssueReturn.onChangeFromWarehouse(dsIssueReturn, "")

        connPool.Dispose()

        Return dsIssueReturn
    End Function

    Public Shared Function OnChangeFromBinNum(strUser As String, dsIssueReturn As IssueReturnDataSet, strBinNum As String) As IssueReturnDataSet
        Dim sUser As String = strUser
        Dim sPass As String = "DEMETER@!"
        Dim sServer As String = "zeus"
        Dim sPort As String = "9408"
        Dim sAppServer As String = String.Format("AppServerDC://{0}:{1}", sServer, sPort)
        Dim sCompany As String = "RMT"
        Dim session As Object = New Epicor.Mfg.Core.Session(sUser, sPass, sAppServer, Epicor.Mfg.Core.Session.LicenseType.Default)
        Dim connPool As New Epicor.Mfg.Core.BLConnectionPool(sUser, sPass, sAppServer)

        Dim myIssueReturn As New IssueReturn(connPool)

        myIssueReturn.OnChangeFromBinNum(True, dsIssueReturn)

        connPool.Dispose()

        Return dsIssueReturn
    End Function

    Public Shared Function PrePerformMaterialMovement(strUser As String, dsIssueReturn As IssueReturnDataSet) As IssueReturnDataSet
        Dim sUser As String = strUser
        Dim sPass As String = "DEMETER@!"
        Dim sServer As String = "zeus"
        Dim sPort As String = "9408"
        Dim sAppServer As String = String.Format("AppServerDC://{0}:{1}", sServer, sPort)
        Dim sCompany As String = "RMT"
        Dim session As Object = New Epicor.Mfg.Core.Session(sUser, sPass, sAppServer, Epicor.Mfg.Core.Session.LicenseType.Default)
        Dim connPool As New Epicor.Mfg.Core.BLConnectionPool(sUser, sPass, sAppServer)

        Dim myIssueReturn As New IssueReturn(connPool)

        myIssueReturn.PrePerformMaterialMovement(dsIssueReturn, False)

        connPool.Dispose()

        Return dsIssueReturn
    End Function

    Public Shared Function PerformMaterialMovement(strUser As String, dsIssueReturn As IssueReturnDataSet) As IssueReturnDataSet
        Dim sUser As String = strUser
        Dim sPass As String = "DEMETER@!"
        Dim sServer As String = "zeus"
        Dim sPort As String = "9408"
        Dim sAppServer As String = String.Format("AppServerDC://{0}:{1}", sServer, sPort)
        Dim sCompany As String = "RMT"
        Dim session As Object = New Epicor.Mfg.Core.Session(sUser, sPass, sAppServer, Epicor.Mfg.Core.Session.LicenseType.Default)
        Dim connPool As New Epicor.Mfg.Core.BLConnectionPool(sUser, sPass, sAppServer)

        Dim myIssueReturn As New IssueReturn(connPool)

        myIssueReturn.PerformMaterialMovement(False, dsIssueReturn, "", "")

        connPool.Dispose()

        Return dsIssueReturn
    End Function
End Class
