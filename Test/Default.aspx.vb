
Partial Class Test_Default
    Inherits System.Web.UI.Page

    Protected Sub TreeView1_SelectedNodeChanged(sender As Object, e As EventArgs) Handles TreeView1.SelectedNodeChanged
        TextBox1.Text = TreeView1.SelectedNode.Depth.ToString()
    End Sub
End Class
