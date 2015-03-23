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

    
    Protected Sub btnSubmit_Click(sender As Object, e As EventArgs) Handles btnSubmit.Click

        Dim dsMoveRequest As New MoveRequestDataSet, dsMaterialQueue As New MaterialQueueDataSet, strGoodTicket As String

        'Check for good request
        strGoodTicket = Good_Ticket()
        If strGoodTicket <> "true" Then
            lblMessage.Text = Good_Ticket()
            Exit Sub
        End If

        'Get New
        dsMoveRequest = BO_MoveRequest.GetNewMoveRequest

        'Change PN
        dsMoveRequest = BO_MoveRequest.OnChangePartNum(dsMoveRequest, "MOVE")
        dsMoveRequest.Tables(0).Rows(0)("RequestQty") = 1
        dsMoveRequest.Tables(0).Rows(0)("EmpID") = txtEmpID.Text


        'Change From Location
        dsMoveRequest = BO_MoveRequest.OnChangeFromWhse(dsMoveRequest, "SPL")
        dsMoveRequest = BO_MoveRequest.OnChangeFromBin(dsMoveRequest, txtBin.Text)

        'Change To Location
        dsMoveRequest = BO_MoveRequest.OnChangeToWhse(dsMoveRequest, "SPL")
        dsMoveRequest = BO_MoveRequest.OnChangeToBin(dsMoveRequest, "X")

        'Process Request
        dsMoveRequest = BO_MoveRequest.ProcessRequest(dsMoveRequest)

        'Find Material Queue Record
        dsMaterialQueue = BO_MaterialQueue.GetRows(txtEmpID.Text)
        'MsgBox(dsMaterialQueue.Tables(0).Rows(0)("MtlQueueSeq"))

        'Update Ref
        dsMaterialQueue.Tables(0).Rows(0)("Reference") = dropType.Text & " " & txtNotes.Text

        'Update
        dsMaterialQueue = BO_MaterialQueue.Update(dsMaterialQueue)

        'Clear Textboxes
        Clear_Textboxes()

        'Show Success Message
        lblMessage.ForeColor = Drawing.Color.Black
        lblMessage.Text = "Move request has been submitted."
    End Sub

    Protected Sub Clear_Textboxes()
        txtBin.Text = ""
        txtEmpID.Text = ""
        txtNotes.Text = ""
    End Sub

    Protected Function Good_Ticket() As String
        Dim dsEmpBasic As New EmpBasicDataSet, dsBin As New WhseBinDataSet

        'CHECK FOR EMPTY FIELDS
        If txtBin.Text = "" Then
            Return "Please enter a work center."
        ElseIf txtEmpID.Text = "" Then
            Return "Please enter an employee ID."
        Else
            'no blanks
        End If

        Try
            'Check if bin exists
            dsBin = BO_WhseBin.GetByID(txtBin.Text)
            If dsBin.Tables(0).Rows.Count < 1 Then
                Return "Please enter a valid work center."
            End If
        Catch ex As Exception
            Return "Please enter a valid work center."
        End Try

        Try
            'Check if employee exists
            dsEmpBasic = BO_EmpBasic.Get_By_ID(txtEmpID.Text)
            If dsEmpBasic.Tables("EmpBasic").Rows.Count = 0 Then
                Return "Employee is not valid."
            Else
                Return "true"
            End If
        Catch ex As Exception
            Return "Employee is not valid."
        End Try


    End Function

    Protected Sub Page_Load1(sender As Object, e As EventArgs) Handles Me.Load
        Dim MESTab As System.Web.UI.HtmlControls.HtmlGenericControl
        MESTab = Master.FindControl("mestab")
        MESTab.Attributes.Add("class", "active")
    End Sub
End Class

Public Class BO_MaterialQueue
    Public Shared Function Get_New() As MaterialQueueDataSet
        Dim sUser As String = "sc"
        Dim sPass As String = "DEMETER@!"
        Dim sServer As String = "zeus"
        Dim sPort As String = "9408"
        Dim sAppServer As String = String.Format("AppServerDC://{0}:{1}", sServer, sPort)
        Dim sCompany As String = "RMT"
        Dim session As Object = New Epicor.Mfg.Core.Session(sUser, sPass, sAppServer, Epicor.Mfg.Core.Session.LicenseType.Default)
        Dim connPool As New Epicor.Mfg.Core.BLConnectionPool(sUser, sPass, sAppServer)

        Dim myMaterialQueue As New MaterialQueue(connPool)
        Dim myMaterialQueueDS As New MaterialQueueDataSet

        myMaterialQueue.GetNewMtlQueue(myMaterialQueueDS)

        connPool.Dispose()

        Return myMaterialQueueDS
    End Function

    Public Shared Function Update(dsMaterialQueue) As MaterialQueueDataSet
        Dim sUser As String = "sc"
        Dim sPass As String = "DEMETER@!"
        Dim sServer As String = "zeus"
        Dim sPort As String = "9408"
        Dim sAppServer As String = String.Format("AppServerDC://{0}:{1}", sServer, sPort)
        Dim sCompany As String = "RMT"
        Dim session As Object = New Epicor.Mfg.Core.Session(sUser, sPass, sAppServer, Epicor.Mfg.Core.Session.LicenseType.Default)
        Dim connPool As New Epicor.Mfg.Core.BLConnectionPool(sUser, sPass, sAppServer)

        Dim myMaterialQueue As New MaterialQueue(connPool)

        myMaterialQueue.Update(dsMaterialQueue)

        connPool.Dispose()

        Return dsMaterialQueue
    End Function

    Public Shared Function GetRows(strEmpId As String) As MaterialQueueDataSet
        Dim sUser As String = "sc"
        Dim sPass As String = "DEMETER@!"
        Dim sServer As String = "zeus"
        Dim sPort As String = "9408"
        Dim sAppServer As String = String.Format("AppServerDC://{0}:{1}", sServer, sPort)
        Dim sCompany As String = "RMT"
        Dim session As Object = New Epicor.Mfg.Core.Session(sUser, sPass, sAppServer, Epicor.Mfg.Core.Session.LicenseType.Default)
        Dim connPool As New Epicor.Mfg.Core.BLConnectionPool(sUser, sPass, sAppServer)

        Dim myMaterialQueue As New MaterialQueue(connPool)
        Dim dsMaterialQueue As New MaterialQueueDataSet

        dsMaterialQueue = myMaterialQueue.GetRows("PartNum = 'MOVE' AND RequestedByEmpID = '" & strEmpId & "' BY MtlQueueSeq DESC", 1, 0, False)


        connPool.Dispose()

        Return dsMaterialQueue
    End Function

End Class

Public Class BO_MoveRequest
    Public Shared Function GetNewMoveRequest() As MoveRequestDataSet
        Dim sUser As String = "sc"
        Dim sPass As String = "DEMETER@!"
        Dim sServer As String = "zeus"
        Dim sPort As String = "9408"
        Dim sAppServer As String = String.Format("AppServerDC://{0}:{1}", sServer, sPort)
        Dim sCompany As String = "RMT"
        Dim session As Object = New Epicor.Mfg.Core.Session(sUser, sPass, sAppServer, Epicor.Mfg.Core.Session.LicenseType.Default)
        Dim connPool As New Epicor.Mfg.Core.BLConnectionPool(sUser, sPass, sAppServer)

        Dim myMoveRequest As New MoveRequest(connPool)
        Dim myMoverequestDS As New MoveRequestDataSet

        myMoveRequest.GetNewMoveRequest("MI", myMoverequestDS)

        connPool.Dispose()

        Return myMoverequestDS
    End Function

    Public Shared Function OnChangePartNum(dsMoveRequest As MoveRequestDataSet, strPartNum As String) As MoveRequestDataSet
        Dim sUser As String = "sc"
        Dim sPass As String = "DEMETER@!"
        Dim sServer As String = "zeus"
        Dim sPort As String = "9408"
        Dim sAppServer As String = String.Format("AppServerDC://{0}:{1}", sServer, sPort)
        Dim sCompany As String = "RMT"
        Dim session As Object = New Epicor.Mfg.Core.Session(sUser, sPass, sAppServer, Epicor.Mfg.Core.Session.LicenseType.Default)
        Dim connPool As New Epicor.Mfg.Core.BLConnectionPool(sUser, sPass, sAppServer)

        Dim myMoveRequest As New MoveRequest(connPool)

        myMoveRequest.OnChangePartNum(strPartNum, dsMoveRequest)

        connPool.Dispose()

        Return dsMoveRequest
    End Function

    Public Shared Function OnChangeFromWhse(dsMoveRequest As MoveRequestDataSet, strWhse As String) As MoveRequestDataSet
        Dim sUser As String = "sc"
        Dim sPass As String = "DEMETER@!"
        Dim sServer As String = "zeus"
        Dim sPort As String = "9408"
        Dim sAppServer As String = String.Format("AppServerDC://{0}:{1}", sServer, sPort)
        Dim sCompany As String = "RMT"
        Dim session As Object = New Epicor.Mfg.Core.Session(sUser, sPass, sAppServer, Epicor.Mfg.Core.Session.LicenseType.Default)
        Dim connPool As New Epicor.Mfg.Core.BLConnectionPool(sUser, sPass, sAppServer)

        Dim myMoveRequest As New MoveRequest(connPool)

        myMoveRequest.OnChangeFromWhse(strWhse, dsMoveRequest)

        connPool.Dispose()

        Return dsMoveRequest
    End Function

    Public Shared Function OnChangeFromBin(dsMoveRequest As MoveRequestDataSet, strBinNum As String) As MoveRequestDataSet
        Dim sUser As String = "sc"
        Dim sPass As String = "DEMETER@!"
        Dim sServer As String = "zeus"
        Dim sPort As String = "9408"
        Dim sAppServer As String = String.Format("AppServerDC://{0}:{1}", sServer, sPort)
        Dim sCompany As String = "RMT"
        Dim session As Object = New Epicor.Mfg.Core.Session(sUser, sPass, sAppServer, Epicor.Mfg.Core.Session.LicenseType.Default)
        Dim connPool As New Epicor.Mfg.Core.BLConnectionPool(sUser, sPass, sAppServer)

        Dim myMoveRequest As New MoveRequest(connPool)

        myMoveRequest.OnChangeFromBin(strBinNum, dsMoveRequest)

        connPool.Dispose()

        Return dsMoveRequest
    End Function

    Public Shared Function OnChangeToWhse(dsMoveRequest As MoveRequestDataSet, strWhse As String) As MoveRequestDataSet
        Dim sUser As String = "sc"
        Dim sPass As String = "DEMETER@!"
        Dim sServer As String = "zeus"
        Dim sPort As String = "9408"
        Dim sAppServer As String = String.Format("AppServerDC://{0}:{1}", sServer, sPort)
        Dim sCompany As String = "RMT"
        Dim session As Object = New Epicor.Mfg.Core.Session(sUser, sPass, sAppServer, Epicor.Mfg.Core.Session.LicenseType.Default)
        Dim connPool As New Epicor.Mfg.Core.BLConnectionPool(sUser, sPass, sAppServer)

        Dim myMoveRequest As New MoveRequest(connPool)

        myMoveRequest.OnChangeToWhse(strWhse, dsMoveRequest)

        connPool.Dispose()

        Return dsMoveRequest
    End Function

    Public Shared Function OnChangeToBin(dsMoveRequest As MoveRequestDataSet, strBinNum As String) As MoveRequestDataSet
        Dim sUser As String = "sc"
        Dim sPass As String = "DEMETER@!"
        Dim sServer As String = "zeus"
        Dim sPort As String = "9408"
        Dim sAppServer As String = String.Format("AppServerDC://{0}:{1}", sServer, sPort)
        Dim sCompany As String = "RMT"
        Dim session As Object = New Epicor.Mfg.Core.Session(sUser, sPass, sAppServer, Epicor.Mfg.Core.Session.LicenseType.Default)
        Dim connPool As New Epicor.Mfg.Core.BLConnectionPool(sUser, sPass, sAppServer)

        Dim myMoveRequest As New MoveRequest(connPool)

        myMoveRequest.OnChangeToBin(strBinNum, dsMoveRequest)

        connPool.Dispose()

        Return dsMoveRequest
    End Function

    Public Shared Function ProcessRequest(dsMoveRequest As MoveRequestDataSet) As MoveRequestDataSet
        Dim sUser As String = "sc"
        Dim sPass As String = "DEMETER@!"
        Dim sServer As String = "zeus"
        Dim sPort As String = "9408"
        Dim sAppServer As String = String.Format("AppServerDC://{0}:{1}", sServer, sPort)
        Dim sCompany As String = "RMT"
        Dim session As Object = New Epicor.Mfg.Core.Session(sUser, sPass, sAppServer, Epicor.Mfg.Core.Session.LicenseType.Default)
        Dim connPool As New Epicor.Mfg.Core.BLConnectionPool(sUser, sPass, sAppServer)

        Dim myMoveRequest As New MoveRequest(connPool)

        myMoveRequest.ProcessRequest(dsMoveRequest)

        connPool.Dispose()

        Return dsMoveRequest
    End Function
End Class

Class BO_EmpBasic
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

Class BO_WhseBin
    Public Shared Function GetByID(strBinNum As String) As WhseBinDataSet
        Dim sUser As String = "sc"
        Dim sPass As String = "DEMETER@!"
        Dim sServer As String = "zeus"
        Dim sPort As String = "9408"
        Dim sAppServer As String = String.Format("AppServerDC://{0}:{1}", sServer, sPort)
        Dim sCompany As String = "RMT"
        Dim session As Object = New Epicor.Mfg.Core.Session(sUser, sPass, sAppServer, Epicor.Mfg.Core.Session.LicenseType.Default)
        Dim connPool As New Epicor.Mfg.Core.BLConnectionPool(sUser, sPass, sAppServer)

        Dim myBin As New WhseBin(connPool)
        Dim myBinDS As New WhseBinDataSet

        myBinDS = myBin.GetByID("SPL", strBinNum)

        connPool.Dispose()

        Return myBinDS
    End Function
End Class