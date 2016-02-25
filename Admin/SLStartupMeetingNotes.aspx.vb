Imports Epicor.Mfg.Core
Imports Epicor.Mfg.Shared
Imports Epicor.Mfg.UI
Imports Epicor.Mfg.BO
Imports System.Configuration
Imports System.Xml
Imports System.IO
Imports System.Text
Imports System.Data
Imports System.Data.Odbc

Partial Class MES_NEW_IssueMaterial
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim MESTab As System.Web.UI.HtmlControls.HtmlGenericControl
        MESTab = Master.FindControl("admintab")
        MESTab.Attributes.Add("class", "active")


    End Sub


    Protected Sub btnUpdate_Click(sender As Object, e As EventArgs) Handles btnUpdate.Click

        'UPDATE NOTES
        Update_Database("UPDATE Rosenboom.dbo.StartupBlog SET Message='" & Replace(txtNotes.Text.ToString, "'", "") & "' WHERE Department='" & DropDownList1.SelectedValue.ToString & "' AND Plant='SPIRITLA'")

        If lblMessage.Text = "" Then
            lblMessage.Text = "Update Complete!"
        End If
        txtNotes.Text = ""

    End Sub

    Protected Sub btnLoadCurrent_Click(sender As Object, e As EventArgs) Handles btnLoadCurrent.Click
        
        Dim dv As DataView = DirectCast(Notes.Select(DataSourceSelectArguments.Empty), DataView)
        For Each drv As DataRowView In dv
            txtNotes.Text = drv("Message").ToString()
        Next

    End Sub

    Protected Sub btnCharCount_Click(sender As Object, e As EventArgs) Handles btnCharCount.Click
        txtCharCount.Text = txtNotes.Text.Length
    End Sub
    Protected Sub Update_Database(ByVal Query As String)
        Dim cn As OdbcConnection, cmd As OdbcCommand

        'Set cn
        cn = New OdbcConnection("DSN=Rosenboom;Driver={SQL Server};Server=olympus;Database=Rosenboom;Uid=ERP10;Pwd=ERP10;")

        'Open Connection
        cn.Open()

        cmd = New OdbcCommand

        Try
            With cmd
                .CommandText = Query
                .CommandType = CommandType.Text
                .Connection = cn
            End With

            cmd.ExecuteNonQuery()
            lblMessage.Text = ""
        Catch ex As Exception
            'MsgBox(ex.Message)
            lblMessage.Text = ex.Message
        Finally
            cn.Close()
        End Try
    End Sub

    Protected Sub DropDownList1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles DropDownList1.SelectedIndexChanged
        txtNotes.Text = DropDownList1.SelectedValue.ToString
    End Sub
End Class
