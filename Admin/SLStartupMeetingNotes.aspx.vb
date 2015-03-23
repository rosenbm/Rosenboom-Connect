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

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim MESTab As System.Web.UI.HtmlControls.HtmlGenericControl
        MESTab = Master.FindControl("admintab")
        MESTab.Attributes.Add("class", "active")


    End Sub


    Protected Sub btnUpdate_Click(sender As Object, e As EventArgs) Handles btnUpdate.Click
        'CHECK THE LENGTH
        If txtNotes.Text.Length > 470 Then
            lblMessage.Text = "Your notes are too long. Please reduce to 470 characters."
        Else
            'UPDATE NOTES
            Dim MyNotes As New IO.StreamWriter("\\pithos\company\Startup Meeting Notes\" & DropDownList1.SelectedValue.ToString & ".txt", False, Encoding.ASCII)
            MyNotes.WriteLine(txtNotes.Text.Replace("'", ""))
            MyNotes.Close()
            MyNotes.Dispose()

            lblMessage.Text = "Update Complete!"
            txtNotes.Text = ""
        End If
    End Sub

    Protected Sub btnLoadCurrent_Click(sender As Object, e As EventArgs) Handles btnLoadCurrent.Click
        Dim txtFile As New IO.StreamReader("\\pithos\company\Startup Meeting Notes\" & DropDownList1.SelectedValue.ToString & ".txt")
        txtNotes.Text = txtFile.ReadToEnd
        txtFile.Close()
        lblMessage.Text = ""
    End Sub

    Protected Sub btnCharCount_Click(sender As Object, e As EventArgs) Handles btnCharCount.Click
        txtCharCount.Text = txtNotes.Text.Length
    End Sub
End Class
