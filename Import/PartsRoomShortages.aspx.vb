Imports System.Data
Imports System.Data.OleDb
Imports System.Data.Odbc
Imports System.Configuration
Imports System.Xml
Imports System.IO
Imports System.Text
Imports Erp.BO
Imports Erp.Proxy.BO
Imports Ice.Core.Session
Imports Ice.Lib.Framework
Imports Ice.Proxy.BO

Partial Class Import_PPP
    Inherits System.Web.UI.Page

    Dim E10session As Ice.Core.Session
    Dim iLaunch As Ice.Lib.Framework.ILauncher
    Dim JobEntryBO As JobEntryImpl

    Protected Sub Start_E10_Session()
        Dim sUser As String = "sc"
        Dim sPass As String = "DEMETER@!"
        E10session = New Ice.Core.Session(sUser, sPass, LicenseType.Default, "\\olympus\ERP10\ERP10.0.700\ClientDeployment\Client\Config\RMT-SHIA-APP03.sysconfig")
        iLaunch = New Ice.Lib.Framework.ILauncher(E10session)
        JobEntryBO = WCFServiceSupport.CreateImpl(Of JobEntryImpl)(E10session, JobEntryImpl.UriPath)
    End Sub


    Protected Sub btnUpdate_Click(sender As Object, e As EventArgs) Handles btnUpdate.Click
        Dim strJobNum As String, strComponent As String, intQty As Integer, strComments As String, dsJob As New JobEntryDataSet
        Dim bolWasReleased As Boolean, bolWasEngineered As Boolean

        'OPEN SESSION
        Start_E10_Session()

        'TEMP VALUES
        strJobNum = txtJobNum.Text
        strComponent = txtComponent.Text
        intQty = CInt(txtQty.Text)
        strComments = txtComments.Text

        'Get Job Data.
        dsJob = JobEntry_GetByID(strJobNum, "sc")

        'Gather data on engineered and released status.
        bolWasEngineered = dsJob.Tables("JobHead").Rows(0)("JobEngineered")
        bolWasReleased = dsJob.Tables("JobHead").Rows(0)("JobReleased")

        'Unrelease if necessary
        If bolWasReleased = False Then
        Else
            dsJob.Tables("JobHead").Rows(0)("JobReleased") = False
            dsJob = JobEntry_ChangeJobHeadJobReleased(dsJob, "sc")
            dsJob = JobEntry_Update(dsJob, "sc")
        End If

        'Unengineer if necessary
        If bolWasEngineered = False Then
        Else
            dsJob.Tables("JobHead").Rows(0)("JobEngineered") = False
            dsJob = JobEntry_ChangeJobHeadJobEngineered(dsJob, "sc")
            dsJob = JobEntry_Update(dsJob, "sc")
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
        dsJob = JobEntry_Update(dsJob, "sc")

        'Engineer if necessary
        If bolWasEngineered = False Then
        Else
            dsJob.Tables("JobHead").Rows(0)("JobEngineered") = True
            dsJob = JobEntry_ChangeJobHeadJobEngineered(dsJob, "sc")
            dsJob = JobEntry_Update(dsJob, "sc")
        End If

        'Release if necessary
        If bolWasReleased = False Then
        Else
            dsJob.Tables("JobHead").Rows(0)("JobReleased") = True
            dsJob = JobEntry_ChangeJobHeadJobReleased(dsJob, "sc")
            dsJob = JobEntry_Update(dsJob, "sc")
        End If

        'Clear Boxes
        txtJobNum.Text = ""
        txtComponent.Text = ""
        txtQty.Text = ""
        txtComments.Text = ""

        'Send Messaage
        lblStatus.Text = "Shortage Updated."
    End Sub

    Public Sub JobEntry_DeleteByID(strJobNum As String, strUser As String)
        JobEntryBO.DeleteByID(strJobNum)
    End Sub
    Public Function JobEntry_GetByID(strJobNum As String, strUser As String) As JobEntryDataSet
        Dim dsJob As New JobEntryDataSet
        dsJob = JobEntryBO.GetByID(strJobNum)
        Return dsJob
    End Function
    Public Function JobEntry_ChangeJobHeadJobReleased(dsJob As JobEntryDataSet, strUser As String) As JobEntryDataSet
        JobEntryBO.ChangeJobHeadJobReleased(dsJob)
        Return dsJob
    End Function
    Public Function JobEntry_ChangeJobHeadJobEngineered(dsJob As JobEntryDataSet, strUser As String) As JobEntryDataSet
        JobEntryBO.ChangeJobHeadJobEngineered(dsJob)
        Return dsJob
    End Function
    Public Function JobEntry_Update(dsJob As JobEntryDataSet, strUser As String) As JobEntryDataSet
        JobEntryBO.Update(dsJob)
        Return dsJob
    End Function
End Class