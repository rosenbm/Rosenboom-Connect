Imports System.ComponentModel

Partial Class Test_ProgressbarTEST
    Inherits System.Web.UI.Page

    Protected Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        'Update the label
        lbProgress.Text = Session("Progress").ToString & "%"

        'Hide when finished
        If lbProgress.Text = "100%" Then
            Image1.Visible = False
            Timer1.Enabled = False
            lbProgress.Text = "Import Complete!"
        End If
    End Sub

    Public Sub btnStart_Click(sender As Object, e As EventArgs) Handles btnStart.Click
        Dim thrDOWORK As New Threading.Thread(AddressOf DO_WORK)

        thrDOWORK.Start()
        Image1.Visible = True
        Timer1.Enabled = True

    End Sub

    Public Sub DO_WORK()

        For i = 1 To 100
            Session("Progress") = i
            Threading.Thread.Sleep(300)
        Next

    End Sub

    '    Private Sub My_BgWorker_ProgressChanged(ByVal sender As Object, ByVal e As System.ComponentModel.ProgressChangedEventArgs) Handles My_BgWorker.ProgressChanged
    '2:
    '        ' Update the progress bar
    '3:
    '        lbProgress.Text = e.ProgressPercentage
    '4:
    '    End Sub


    '    Private Sub My_BgWorker_DoWork(ByVal sender As Object, ByVal e As System.ComponentModel.DoWorkEventArgs) Handles My_BgWorker.DoWork
    '2:

    '3:
    '        For i As Integer = 0 To 100
    '4:
    '            ' Has the background worker be told to stop?
    '5:
    '            If My_BgWorker.CancellationPending Then
    '6:
    '                ' Set Cancel to True
    '7:
    '                e.Cancel = True
    '8:
    '                Exit For
    '9:
    '            End If
    '10:
    '            System.Threading.Thread.Sleep(1000) ' Sleep for 1 Second
    '11:
    '            ' Report The progress of the Background Worker.
    '12:
    '            My_BgWorker.ReportProgress(CInt((i / 100) * 100))
    '13:
    '        Next
    '14:
    '    End Sub
End Class
