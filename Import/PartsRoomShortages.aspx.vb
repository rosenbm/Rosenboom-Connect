Imports System.Data
Imports System.Data.OleDb
Imports System.Data.Odbc
Imports Epicor.Mfg.Core
Imports Epicor.Mfg.Shared
Imports Epicor.Mfg.UI
Imports Epicor.Mfg.BO
Imports System.Configuration
Imports System.Xml
Imports System.IO
Imports System.Text
Partial Class Import_PPP
    Inherits System.Web.UI.Page




    

    Protected Sub btnUpdate_Click(sender As Object, e As EventArgs) Handles btnUpdate.Click
        Dim strJobNum As String, strComponent As String, intQty As Integer, strComments As String, dsJob As New JobEntryDataSet
        Dim bolWasReleased As Boolean, bolWasEngineered As Boolean

        'TEMP VALUES
        strJobNum = txtJobNum.Text
        strComponent = txtComponent.Text
        intQty = CInt(txtQty.Text)
        strComments = txtComments.Text

        'Get Job Data.
        dsJob = BO_JobEntry.GetByID(strJobNum, "sc")

        'Gather data on engineered and released status.
        bolWasEngineered = dsJob.Tables("JobHead").Rows(0)("JobEngineered")
        bolWasReleased = dsJob.Tables("JobHead").Rows(0)("JobReleased")

        'Unrelease if necessary
        If bolWasReleased = False Then
        Else
            dsJob.Tables("JobHead").Rows(0)("JobReleased") = False
            dsJob = BO_JobEntry.ChangeJobHeadJobReleased(dsJob, "sc")
            dsJob = BO_JobEntry.Update(dsJob, "sc")
        End If

        'Unengineer if necessary
        If bolWasEngineered = False Then
        Else
            dsJob.Tables("JobHead").Rows(0)("JobEngineered") = False
            dsJob = BO_JobEntry.ChangeJobHeadJobEngineered(dsJob, "sc")
            dsJob = BO_JobEntry.Update(dsJob, "sc")
        End If

        'Make changes
        For Each row As DataRow In dsJob.Tables("JobMtl").Rows
            'Check if this is the component row
            If UCase(row("PartNum")) = UCase(strComponent) Then
                row("RowMod") = "U"
                row("Character05") = strComments
                row("Number01") = intQty
                Exit For
            End If
        Next

        'Update
        dsJob = BO_JobEntry.Update(dsJob, "sc")

        'Engineer if necessary
        If bolWasEngineered = False Then
        Else
            dsJob.Tables("JobHead").Rows(0)("JobEngineered") = True
            dsJob = BO_JobEntry.ChangeJobHeadJobEngineered(dsJob, "sc")
            dsJob = BO_JobEntry.Update(dsJob, "sc")
        End If

        'Release if necessary
        If bolWasReleased = False Then
        Else
            dsJob.Tables("JobHead").Rows(0)("JobReleased") = True
            dsJob = BO_JobEntry.ChangeJobHeadJobReleased(dsJob, "sc")
            dsJob = BO_JobEntry.Update(dsJob, "sc")
        End If

        'Clear Boxes
        txtJobNum.Text = ""
        txtComponent.Text = ""
        txtQty.Text = ""
        txtComments.Text = ""

        'Send Messaage
        lblStatus.Text = "Shortage Updated."
    End Sub
End Class
Public Class BO_JobEntry
    Public Shared Sub Delete_By_ID(strJobNum As String, strUser As String)
        Dim sUser As String = strUser
        Dim sPass As String = "DEMETER@!"
        Dim sServer As String = "zeus"
        Dim sPort As String = "9408"
        Dim sAppServer As String = String.Format("AppServerDC://{0}:{1}", sServer, sPort)
        Dim sCompany As String = "RMT"
        Dim session As Object = New Epicor.Mfg.Core.Session(sUser, sPass, sAppServer, Epicor.Mfg.Core.Session.LicenseType.Default)
        Dim connPool As New Epicor.Mfg.Core.BLConnectionPool(sUser, sPass, sAppServer)

        Dim myJobEntry As New JobEntry(connPool)

        myJobEntry.DeleteByID(strJobNum)

        connPool.Dispose()
    End Sub
    Public Shared Function GetByID(strJobNum As String, strUser As String) As JobEntryDataSet
        Dim sUser As String = strUser
        Dim sPass As String = "DEMETER@!"
        Dim sServer As String = "zeus"
        Dim sPort As String = "9408"
        Dim sAppServer As String = String.Format("AppServerDC://{0}:{1}", sServer, sPort)
        Dim sCompany As String = "RMT"
        Dim session As Object = New Epicor.Mfg.Core.Session(sUser, sPass, sAppServer, Epicor.Mfg.Core.Session.LicenseType.Default)
        Dim connPool As New Epicor.Mfg.Core.BLConnectionPool(sUser, sPass, sAppServer)

        Dim myJobEntry As New JobEntry(connPool)
        Dim dsJob As New JobEntryDataSet

        dsJob = myJobEntry.GetByID(strJobNum)

        connPool.Dispose()

        Return dsJob
    End Function
    Public Shared Function ChangeJobHeadJobReleased(dsJob As JobEntryDataSet, strUser As String) As JobEntryDataSet
        Dim sUser As String = strUser
        Dim sPass As String = "DEMETER@!"
        Dim sServer As String = "zeus"
        Dim sPort As String = "9408"
        Dim sAppServer As String = String.Format("AppServerDC://{0}:{1}", sServer, sPort)
        Dim sCompany As String = "RMT"
        Dim session As Object = New Epicor.Mfg.Core.Session(sUser, sPass, sAppServer, Epicor.Mfg.Core.Session.LicenseType.Default)
        Dim connPool As New Epicor.Mfg.Core.BLConnectionPool(sUser, sPass, sAppServer)

        Dim myJobEntry As New JobEntry(connPool)

        myJobEntry.ChangeJobHeadJobReleased(dsJob)

        connPool.Dispose()

        Return dsJob
    End Function
    Public Shared Function ChangeJobHeadJobEngineered(dsJob As JobEntryDataSet, strUser As String) As JobEntryDataSet
        Dim sUser As String = strUser
        Dim sPass As String = "DEMETER@!"
        Dim sServer As String = "zeus"
        Dim sPort As String = "9408"
        Dim sAppServer As String = String.Format("AppServerDC://{0}:{1}", sServer, sPort)
        Dim sCompany As String = "RMT"
        Dim session As Object = New Epicor.Mfg.Core.Session(sUser, sPass, sAppServer, Epicor.Mfg.Core.Session.LicenseType.Default)
        Dim connPool As New Epicor.Mfg.Core.BLConnectionPool(sUser, sPass, sAppServer)

        Dim myJobEntry As New JobEntry(connPool)

        myJobEntry.ChangeJobHeadJobEngineered(dsJob)

        connPool.Dispose()

        Return dsJob
    End Function
    Public Shared Function Update(dsJob As JobEntryDataSet, strUser As String) As JobEntryDataSet
        Dim sUser As String = strUser
        Dim sPass As String = "DEMETER@!"
        Dim sServer As String = "zeus"
        Dim sPort As String = "9408"
        Dim sAppServer As String = String.Format("AppServerDC://{0}:{1}", sServer, sPort)
        Dim sCompany As String = "RMT"
        Dim session As Object = New Epicor.Mfg.Core.Session(sUser, sPass, sAppServer, Epicor.Mfg.Core.Session.LicenseType.Default)
        Dim connPool As New Epicor.Mfg.Core.BLConnectionPool(sUser, sPass, sAppServer)

        Dim myJobEntry As New JobEntry(connPool)

        myJobEntry.Update(dsJob)

        connPool.Dispose()

        Return dsJob
    End Function
End Class