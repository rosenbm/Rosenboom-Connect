Imports Epicor.Mfg.Core
Imports Epicor.Mfg.Shared
Imports Epicor.Mfg.UI
Imports Epicor.Mfg.BO
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
        Dim dsUD02 As New UD02DataSet

        'Get dataset
        dsUD02 = BO_UD02_MtlAdmin.Get_By_ID(grdvOpenIssues.SelectedDataKey.Value, grdvOpenIssues.SelectedRow.Cells(3).Text, _
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
        dsUD02 = BO_UD02_MtlAdmin.Update(dsUD02)
        'Tell the user
        lblMessage.Text = "Update Successful."
        'Clear boxes
        txtCommunicatedWith.Text = ""
        txtReason.Text = ""
        chkComplete.Checked = False
        'Refresh
        grdvOpenIssues.DataBind()
    End Sub
End Class
Public Class BO_UD02_MtlAdmin
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
    Public Shared Function Get_By_ID(strKey1 As String, strKey2 As String, strKey3 As String, strKey4 As String, strKey5 As String) As UD02DataSet
        Dim sUser As String = "sc"
        Dim sPass As String = "DEMETER@!"
        Dim sServer As String = "zeus"
        Dim sPort As String = "9408"
        Dim sAppServer As String = String.Format("AppServerDC://{0}:{1}", sServer, sPort)
        Dim sCompany As String = "RMT"
        Dim session As Object = New Epicor.Mfg.Core.Session(sUser, sPass, sAppServer, Epicor.Mfg.Core.Session.LicenseType.Default)
        Dim connPool As New Epicor.Mfg.Core.BLConnectionPool(sUser, sPass, sAppServer)

        Dim dsUD02 As New UD02DataSet
        Dim myUD02 As New UD02(connPool)

        dsUD02 = myUD02.GetByID(strKey1, strKey2, strKey3, strKey4, strKey5)

        connPool.Dispose()

        Return dsUD02
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
