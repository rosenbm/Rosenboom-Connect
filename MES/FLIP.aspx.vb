Imports Epicor.Mfg.Core
Imports Epicor.Mfg.Shared
Imports Epicor.Mfg.UI
Imports Epicor.Mfg.BO
Imports System.Configuration
Imports System.Xml
Imports System.IO
Imports System.Text
Imports System.Data
Partial Class MES_FLIP
    Inherits System.Web.UI.Page

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
        'Check for valid employee
        Try
            dsEmpBasic = BO_EmpBasic_FLIP.Get_By_ID(txtEmpID.Text.ToLower)
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
        dsUD01 = BO_UD01.Get_New
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
        dsUD01 = BO_UD01.Update(dsUD01)
    End Sub

    Protected Sub btnRefreshOps_Click(sender As Object, e As EventArgs) Handles btnRefreshOps.Click

    End Sub
End Class
Public Class BO_UD01
    Public Shared Function Get_New() As UD01DataSet
        Dim sUser As String = "sc"
        Dim sPass As String = "DEMETER@!"
        Dim sServer As String = "zeus"
        Dim sPort As String = "9408"
        Dim sAppServer As String = String.Format("AppServerDC://{0}:{1}", sServer, sPort)
        Dim sCompany As String = "RMT"
        Dim session As Object = New Epicor.Mfg.Core.Session(sUser, sPass, sAppServer, Epicor.Mfg.Core.Session.LicenseType.Default)
        Dim connPool As New Epicor.Mfg.Core.BLConnectionPool(sUser, sPass, sAppServer)

        Dim myUD01 As New UD01(connPool)
        Dim myUD01DS As New UD01DataSet

        myUD01.GetaNewUD01(myUD01DS)

        connPool.Dispose()

        Return myUD01DS
    End Function
    Public Shared Function Get_Rows(strUD01Filter As String) As UD01DataSet
        Dim sUser As String = "sc"
        Dim sPass As String = "DEMETER@!"
        Dim sServer As String = "zeus"
        Dim sPort As String = "9408"
        Dim sAppServer As String = String.Format("AppServerDC://{0}:{1}", sServer, sPort)
        Dim sCompany As String = "RMT"
        Dim session As Object = New Epicor.Mfg.Core.Session(sUser, sPass, sAppServer, Epicor.Mfg.Core.Session.LicenseType.Default)
        Dim connPool As New Epicor.Mfg.Core.BLConnectionPool(sUser, sPass, sAppServer)

        Dim myUD01 As New UD01(connPool)
        Dim myUD01DS As New UD01DataSet

        myUD01.GetRows(strUD01Filter, "", 0, 0, True)

        connPool.Dispose()

        Return myUD01DS
    End Function
    Public Shared Function Update(dsUD01 As UD01DataSet) As UD01DataSet
        Dim sUser As String = "sc"
        Dim sPass As String = "DEMETER@!"
        Dim sServer As String = "zeus"
        Dim sPort As String = "9408"
        Dim sAppServer As String = String.Format("AppServerDC://{0}:{1}", sServer, sPort)
        Dim sCompany As String = "RMT"
        Dim session As Object = New Epicor.Mfg.Core.Session(sUser, sPass, sAppServer, Epicor.Mfg.Core.Session.LicenseType.Default)
        Dim connPool As New Epicor.Mfg.Core.BLConnectionPool(sUser, sPass, sAppServer)

        Dim myUD01 As New UD01(connPool)

        myUD01.Update(dsUD01)

        connPool.Dispose()

        Return dsUD01
    End Function
End Class
Public Class BO_EmpBasic_FLIP
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

        Dim strNewID As String
        strNewID = Left(strEmpID, 3)
        strNewID = LCase(strNewID)
        strNewID &= Right(strEmpID, 2)

        myEmpBasicDS = myEmpBasic.GetByID(strNewID)

        connPool.Dispose()

        Return myEmpBasicDS
    End Function
End Class
