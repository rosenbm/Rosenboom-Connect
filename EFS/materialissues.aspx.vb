Imports Ice.Core.Session
Imports Ice.Lib.Framework
Imports Ice.Proxy.BO
Imports System.Configuration
Imports System.Xml
Imports System.IO
Imports System.Text
Imports Erp.Proxy.BO

Partial Class EFS_TestCodeBehind
    Inherits System.Web.UI.Page

    Protected Sub btnSubmit_Click(sender As Object, e As EventArgs) Handles btnSubmit.Click
        Dim intShift As Integer = 0, dsUD02 As New Ice.BO.UD02DataSet, dsEmpBasic As New Erp.BO.EmpBasicDataSet
        Dim sUser As String = "sc"
        Dim sPass As String = "DEMETER@!"
        Dim session As Object = New Ice.Core.Session(sUser, sPass, LicenseType.Default, "\\olympus\ERP10\ERP10.0.700\ClientDeployment\Client\Config\RMT-SHIA-APP03.sysconfig")
        Dim iLaunch As New Ice.Lib.Framework.ILauncher(session)
        Dim sessionMod As Ice.Proxy.BO.UD02Impl = WCFServiceSupport.CreateImpl(Of UD02Impl)(session, UD02Impl.UriPath)
        Dim sessionEmp As Erp.Proxy.BO.EmpBasicImpl = WCFServiceSupport.CreateImpl(Of EmpBasicImpl)(session, EmpBasicImpl.UriPath)

        'Determine if the employee exists
        Try
            dsEmpBasic = sessionEmp.GetByID(txtRequestor.Text)
            sessionEmp.Close()
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
        sessionMod.GetaNewUD02(dsUD02)
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
        sessionMod.Update(dsUD02)
        sessionMod.Close()

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