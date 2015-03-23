Imports Epicor.Mfg.Core
Imports Epicor.Mfg.Shared
Imports Epicor.Mfg.UI
Imports Epicor.Mfg.BO
Imports System.Configuration
Imports System.Xml
Imports System.IO
Imports System.Text

Partial Class EFS_TestCodeBehind
    Inherits System.Web.UI.Page

    Protected Sub btnSubmit_Click(sender As Object, e As EventArgs) Handles btnSubmit.Click
        Dim intShift As Integer = 0, dsUD02 As New UD02DataSet, dsEmpBasic As New EmpBasicDataSet

        'Determine if the employee exists
        Try
            dsEmpBasic = BO_EmpBasic.Get_By_ID(txtRequestor.Text)
        Catch ex As Exception
            If ex.Message = "Record not found." Then
                lblMessage.Text = "Please enter a valid employee ID."
                lblMessage.ForeColor = Drawing.Color.Red
                lblMessage.Font.Bold = False
            Else 'other error
                lblMessage.Text = "There was an error retrieving your employee information."
                lblMessage.ForeColor = Drawing.Color.Red
                lblMessage.Font.Bold = False
            End If
            Exit Sub
        End Try

        'Create new record
        dsUD02 = BO_UD02.Get_New()
        'Set values
        dsUD02.Tables(0).Rows(0)("Key1") = Now
        dsUD02.Tables(0).Rows(0)("Key2") = ddlRG.Text
        dsUD02.Tables(0).Rows(0)("Key3") = ddlSupervisor.Text
        dsUD02.Tables(0).Rows(0)("Key4") = ddlIssue.Text
        dsUD02.Tables(0).Rows(0)("Key5") = txtRequestor.Text
        dsUD02.Tables(0).Rows(0)("Character01") = txtAdditionalnfo.Text
        dsUD02.Tables(0).Rows(0)("Character02") = dsEmpBasic.Tables("EmpBasic").Rows(0)("Name")
        dsUD02.Tables(0).Rows(0)("Character05") = txtPartNum.Text
        dsUD02.Tables(0).Rows(0)("Character06") = txtEmail.Text
        dsUD02.Tables(0).Rows(0)("Date01") = Today
        'Submit
        dsUD02 = BO_UD02.Update(dsUD02)
        'Clear Temp Data
        dsEmpBasic.Clear()
        dsUD02.Clear()
        'Clear the Additional info box
        txtAdditionalnfo.Text = ""
        'Send message to the user
        lblMessage.Text = "Material Issue Submitted."
        lblMessage.ForeColor = Drawing.Color.Black
        lblMessage.Font.Bold = True
    End Sub

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        Dim FeedbackTab As System.Web.UI.HtmlControls.HtmlGenericControl
        FeedbackTab = Master.FindControl("feedbacktab")
        FeedbackTab.Attributes.Add("class", "active")
    End Sub
End Class
Public Class BO_UD02
    Public Shared Function Get_New() As UD02DataSet
        Dim sUser As String = "sc"
        Dim sPass As String = "DEMETER@!"
        Dim sServer As String = "zeus"
        Dim sPort As String = "9408"
        Dim sAppServer As String = String.Format("AppServerDC://{0}:{1}", sServer, sPort)
        Dim sCompany As String = "RMT"
        Dim session As Object = New Epicor.Mfg.Core.Session(sUser, sPass, sAppServer, Epicor.Mfg.Core.Session.LicenseType.Default)
        Dim connPool As New Epicor.Mfg.Core.BLConnectionPool(sUser, sPass, sAppServer)

        Dim myUD02 As New UD02(connPool)
        Dim myUD02DS As New UD02DataSet

        myUD02.GetaNewUD02(myUD02DS)

        connPool.Dispose()

        Return myUD02DS
    End Function
    Public Shared Function Update(dsUD02 As UD02DataSet) As UD02DataSet
        Dim sUser As String = "sc"
        Dim sPass As String = "DEMETER@!"
        Dim sServer As String = "zeus"
        Dim sPort As String = "9408"
        Dim sAppServer As String = String.Format("AppServerDC://{0}:{1}", sServer, sPort)
        Dim sCompany As String = "RMT"
        Dim session As Object = New Epicor.Mfg.Core.Session(sUser, sPass, sAppServer, Epicor.Mfg.Core.Session.LicenseType.Default)
        Dim connPool As New Epicor.Mfg.Core.BLConnectionPool(sUser, sPass, sAppServer)

        Dim myUD02 As New UD02(connPool)

        myUD02.Update(dsUD02)

        connPool.Dispose()

        Return dsUD02
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