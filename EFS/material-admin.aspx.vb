Imports Ice.Core.Session
Imports Ice.Lib.Framework
Imports Ice.Proxy.BO
Imports System.Configuration
Imports System.Xml
Imports System.IO
Imports System.Text

Partial Class EFS_material_admin_new_
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        Dim FeedbackTab As System.Web.UI.HtmlControls.HtmlGenericControl
        FeedbackTab = Master.FindControl("admintab")
        FeedbackTab.Attributes.Add("class", "active")
    End Sub

    Protected Sub btnRefresh_Click(sender As Object, e As EventArgs) Handles btnRefresh.Click
        lblMessage.Text = ""
        grdvOpenIssues.DataBind()
    End Sub

    Protected Sub btnSubmit_Click(sender As Object, e As EventArgs) Handles btnSubmit.Click
        Dim sUser As String = "sc"
        Dim sPass As String = "DEMETER@!"
        Dim session As Object = New Ice.Core.Session(sUser, sPass, LicenseType.Default, "\\olympus\ERP10\ERP10.0.700\ClientDeployment\Client\Config\RMT-SHIA-APP03.sysconfig")
        Dim iLaunch As New Ice.Lib.Framework.ILauncher(session)
        Dim sessionMod As Ice.Proxy.BO.UD02Impl = WCFServiceSupport.CreateImpl(Of UD02Impl)(session, UD02Impl.UriPath)
        Dim dsUD02 As New Ice.BO.UD02DataSet

        'Get dataset
        dsUD02 = sessionMod.GetByID(grdvOpenIssues.SelectedDataKey.Value, grdvOpenIssues.SelectedRow.Cells(3).Text, _
                            grdvOpenIssues.SelectedRow.Cells(4).Text, grdvOpenIssues.SelectedRow.Cells(5).Text, _
                            grdvOpenIssues.SelectedRow.Cells(7).Text)
        'Update Values
        dsUD02.Tables(0).Rows(0)("RowMod") = "U"
        dsUD02.Tables(0).Rows(0)("Character03") = txtCommunicatedWith.Text
        dsUD02.Tables(0).Rows(0)("Character04") = txtReason.Text
        'Determine if completed
        If chkComplete.Checked = True Then
            dsUD02.Tables(0).Rows(0)("CheckBox01") = True
            dsUD02.Tables(0).Rows(0)("Date02") = Today
        Else 'Not Complete
        End If
        'Update
        sessionMod.Update(dsUD02)
        'Tell the user
        lblMessage.Text = "Update Successful."
        'Clear boxes
        txtCommunicatedWith.Text = ""
        txtReason.Text = ""
        chkComplete.Checked = False
        sessionMod.Close()
        'Refresh
        grdvOpenIssues.DataBind()
    End Sub
End Class
