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

Partial Class MES_FLIP
    Inherits System.Web.UI.Page
    Dim E10session As Ice.Core.Session
    Dim iLaunch As Ice.Lib.Framework.ILauncher
    Dim EmpBasicBO As EmpBasicImpl
    Dim UD01BO As UD01Impl

    Protected Sub Start_E10_Session()
        Dim sUser As String = "sc"
        Dim sPass As String = "DEMETER@!"
        E10session = New Ice.Core.Session(sUser, sPass, LicenseType.Default, "\\olympus\ERP10\ERP10.0.700\ClientDeployment\Client\Config\RMT-SHIA-APP03.sysconfig")
        iLaunch = New Ice.Lib.Framework.ILauncher(E10session)
        EmpBasicBO = WCFServiceSupport.CreateImpl(Of EmpBasicImpl)(E10session, EmpBasicImpl.UriPath)
        UD01BO = WCFServiceSupport.CreateImpl(Of UD01Impl)(E10session, UD01Impl.UriPath)
    End Sub

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        'Set the auto tab
        Me.txtEmpID.Attributes("onkeyup") = "autotab(" + Me.txtEmpID.ClientID + ", " + Me.txtJobNum.ClientID + ")"
        'Highlight MES Tab
        Dim MESTab As System.Web.UI.HtmlControls.HtmlGenericControl
        MESTab = Master.FindControl("mestab")
        MESTab.Attributes.Add("class", "active")
    End Sub


    Protected Sub btnSubmit_Click(sender As Object, e As EventArgs) Handles btnSubmit.Click
        Dim dsEmpBasic As New EmpBasicDataSet, intAsm As Integer, intOprSeq As Integer, strOpDesc As String = "", strOpCode As String = "", _
            ds As New DataSet, strInspectorName As String = ""

        'CREATE EPICOR CONNECTION
        Start_E10_Session()

        'Check for valid employee
        Try
            dsEmpBasic = EmpBasic_GetByID(txtEmpID.Text.ToLower)
            If dsEmpBasic.Tables(0).Rows(0)("Number01") < 1 Then
                lblMessage.ForeColor = Drawing.Color.Red
                lblMessage.Font.Bold = False
                lblMessage.Text = "Employee is not a valid FLIP inspector."
                Exit Sub
            End If
            strInspectorName = dsEmpBasic.Tables(0).Rows(0)("Name")
        Catch ex As Exception
            lblMessage.ForeColor = Drawing.Color.Red
            lblMessage.Font.Bold = False
            lblMessage.Text = "Employee code is entered incorrectly."
            Exit Sub
        End Try

        'Check for valid job
        If txtJobNum.Text = "" Or gvwJobOpers.Rows.Count = 0 Then
            lblMessage.ForeColor = Drawing.Color.Red
            lblMessage.Font.Bold = False
            lblMessage.Text = "Please enter a valid job number and get operations to select first and last part inspections."
            Exit Sub
        End If

        'Good transaction, continue.
        For Each row As GridViewRow In gvwJobOpers.Rows
            'Get row values
            Dim chkFP As CheckBox = CType(row.Cells(7).Controls(1), CheckBox)
            Dim chkLP As CheckBox = CType(row.Cells(8).Controls(1), CheckBox)
            intAsm = row.Cells(1).Text
            intOprSeq = row.Cells(3).Text
            strOpCode = row.Cells(4).Text
            strOpDesc = row.Cells(5).Text
            'Send to new flip
            New_Flip(chkFP.Checked, "FirstPart", intAsm, intOprSeq, strOpDesc, strOpCode, strInspectorName)
            New_Flip(chkLP.Checked, "LastPart", intAsm, intOprSeq, strOpDesc, strOpCode, strInspectorName)
        Next

        'Update User
        lblMessage.ForeColor = Drawing.Color.Black
        lblMessage.Font.Bold = True
        lblMessage.Text = "FLIP(s) recorded."
        txtJobNum.Text = ""
        txtEmpID.Text = ""
        txtComments.Text = ""
    End Sub

    Protected Sub New_Flip(bolChecked As Boolean, strType As String, intAsm As Integer, intOprSeq As Integer, strOpDesc As String, strOpCode As String, _
                           strInspectorName As String)
        If bolChecked = False Then 'Not an inspection
            Exit Sub
        End If

        Dim dsUD01 As New UD01DataSet
        'Get new data set
        dsUD01 = UD01_GetNew()
        'Change values
        dsUD01.Tables(0).Rows(0)("Key1") = txtJobNum.Text
        dsUD01.Tables(0).Rows(0)("Key2") = intAsm
        dsUD01.Tables(0).Rows(0)("Key3") = intOprSeq
        dsUD01.Tables(0).Rows(0)("Key4") = strType
        dsUD01.Tables(0).Rows(0)("Key5") = Now
        dsUD01.Tables(0).Rows(0)("Date01") = Today
        dsUD01.Tables(0).Rows(0)("Character01") = FormatDateTime(Now, DateFormat.ShortTime)
        dsUD01.Tables(0).Rows(0)("Character02") = strOpCode
        dsUD01.Tables(0).Rows(0)("Character03") = strOpDesc
        dsUD01.Tables(0).Rows(0)("Character04") = txtEmpID.Text
        dsUD01.Tables(0).Rows(0)("Character05") = txtComments.Text
        dsUD01.Tables(0).Rows(0)("Character06") = strInspectorName
        dsUD01.Tables(0).Rows(0)("Number01") = intAsm
        dsUD01.Tables(0).Rows(0)("Number02") = intOprSeq
        'Update
        dsUD01 = UD01_Update(dsUD01)
    End Sub

    Protected Sub btnRefreshOps_Click(sender As Object, e As EventArgs) Handles btnRefreshOps.Click

    End Sub


    Function UD01_GetNew() As UD01DataSet
        Dim UD01BODS As New UD01DataSet
        UD01BO.GetaNewUD01(UD01BODS)
        Return UD01BODS
    End Function
    Function UD01_GetRows(strUD01Filter As String) As UD01DataSet
        Dim UD01BODS As New UD01DataSet
        UD01BO.GetRows(strUD01Filter, "", 0, 0, True)
        Return UD01BODS
    End Function
    Function UD01_Update(dsUD01 As UD01DataSet) As UD01DataSet
        UD01BO.Update(dsUD01)
        Return dsUD01
    End Function

    Function EmpBasic_GetByID(strEmpID As String) As EmpBasicDataSet
        Dim myEmpBasicDS As New EmpBasicDataSet
        Dim strNewID As String
        strNewID = Left(strEmpID, 3)
        strNewID = LCase(strNewID)
        strNewID &= Right(strEmpID, 2)

        myEmpBasicDS = EmpBasicBO.GetByID(strNewID)

        Return myEmpBasicDS
    End Function
End Class
