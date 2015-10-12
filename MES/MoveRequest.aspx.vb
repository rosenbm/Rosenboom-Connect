Imports System.Configuration
Imports System.Xml
Imports System.IO
Imports System.Text
Imports System.Data
Imports Erp.BO
Imports Erp.Proxy.BO
Imports Ice.Lib.Framework
Imports Ice.Core.Session


Partial Class MES_NEW_IssueMaterial
    Inherits System.Web.UI.Page

    Dim E10session As Ice.Core.Session
    Dim iLaunch As Ice.Lib.Framework.ILauncher
    Dim MaterialQueueBO As MaterialQueueImpl
    Dim MoveRequestBO As MoveRequestImpl
    Dim EmpBasicBO As EmpBasicImpl
    Dim WhseBinBO As WhseBinImpl

    Protected Sub Start_E10_Session()
        Dim sUser As String = "sc"
        Dim sPass As String = "DEMETER@!"
        E10session = New Ice.Core.Session(sUser, sPass, LicenseType.Default, "\\olympus\ERP10\ERP10.0.700\ClientDeployment\Client\Config\RMT-SHIA-APP03.sysconfig")
        iLaunch = New Ice.Lib.Framework.ILauncher(E10session)
        MaterialQueueBO = WCFServiceSupport.CreateImpl(Of MaterialQueueImpl)(E10session, MaterialQueueImpl.UriPath)
        MoveRequestBO = WCFServiceSupport.CreateImpl(Of MoveRequestImpl)(E10session, MoveRequestImpl.UriPath)
        EmpBasicBO = WCFServiceSupport.CreateImpl(Of EmpBasicImpl)(E10session, EmpBasicImpl.UriPath)
        WhseBinBO = WCFServiceSupport.CreateImpl(Of WhseBinImpl)(E10session, WhseBinImpl.UriPath)

    End Sub
    Protected Sub btnSubmit_Click(sender As Object, e As EventArgs) Handles btnSubmit.Click

        Dim dsMoveRequest As New MoveRequestDataSet, dsMaterialQueue As New MaterialQueueDataSet, strGoodTicket As String

        Start_E10_Session()

        'Check for good request
        strGoodTicket = Good_Ticket()
        If strGoodTicket <> "true" Then
            lblMessage.Text = Good_Ticket()
            Exit Sub
        End If

        'Get New
        dsMoveRequest = MoveRequest_GetNewMoveRequest()

        'Change PN
        dsMoveRequest = MoveRequest_OnChangePartNum(dsMoveRequest, "MOVE")
        dsMoveRequest.Tables(0).Rows(0)("RequestQty") = 1
        dsMoveRequest.Tables(0).Rows(0)("EmpID") = txtEmpID.Text


        'Change From Location
        dsMoveRequest = MoveRequest_OnChangeFromWhse(dsMoveRequest, "SPL")
        dsMoveRequest = MoveRequest_OnChangeFromBin(dsMoveRequest, txtBin.Text)

        'Change To Location
        dsMoveRequest = MoveRequest_OnChangeToWhse(dsMoveRequest, "SPL")
        dsMoveRequest = MoveRequest_OnChangeToBin(dsMoveRequest, "X")

        'Process Request
        dsMoveRequest = MoveRequest_ProcessRequest(dsMoveRequest)

        'Find Material Queue Record
        dsMaterialQueue = MaterialQueue_GetRows(txtEmpID.Text)
        'MsgBox(dsMaterialQueue.Tables(0).Rows(0)("MtlQueueSeq"))

        'Update Ref
        dsMaterialQueue.Tables(0).Rows(0)("Reference") = dropType.Text & " " & txtNotes.Text

        'Update
        dsMaterialQueue = MaterialQueue_Update(dsMaterialQueue)

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
            dsBin = WhseBin_GetByID(txtBin.Text)
            If dsBin.Tables(0).Rows.Count < 1 Then
                Return "Please enter a valid work center."
            End If
        Catch ex As Exception
            Return "Please enter a valid work center."
        End Try

        Try
            'Check if employee exists
            dsEmpBasic = EmpBasic_GetByID(txtEmpID.Text)
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

    Function MaterialQueue_GetNew() As MaterialQueueDataSet
        Dim MaterialQueueBODS As New MaterialQueueDataSet
        MaterialQueueBO.GetNewMtlQueue(MaterialQueueBODS)
        Return MaterialQueueBODS
    End Function

    Function MaterialQueue_Update(dsMaterialQueue As MaterialQueueDataSet) As MaterialQueueDataSet
        MaterialQueueBO.Update(dsMaterialQueue)
        Return dsMaterialQueue
    End Function

    Function MaterialQueue_GetRows(strEmpId As String) As MaterialQueueDataSet
        Dim dsMaterialQueue As New MaterialQueueDataSet
        dsMaterialQueue = MaterialQueueBO.GetRows("PartNum = 'MOVE' AND RequestedByEmpID = '" & strEmpId & "' BY MtlQueueSeq DESC", 1, 0, False)
        Return dsMaterialQueue
    End Function

    Function MoveRequest_GetNewMoveRequest() As MoveRequestDataSet
        Dim MoveRequestBODS As New MoveRequestDataSet
        MoveRequestBO.GetNewMoveRequest("MI", MoveRequestBODS)
        Return MoveRequestBODS
    End Function

    Function MoveRequest_OnChangePartNum(dsMoveRequest As MoveRequestDataSet, strPartNum As String) As MoveRequestDataSet
        MoveRequestBO.OnChangePartNum(strPartNum, dsMoveRequest)
        Return dsMoveRequest
    End Function

    Function MoveRequest_OnChangeFromWhse(dsMoveRequest As MoveRequestDataSet, strWhse As String) As MoveRequestDataSet
        MoveRequestBO.OnChangeFromWhse(strWhse, dsMoveRequest)
        Return dsMoveRequest
    End Function

    Function MoveRequest_OnChangeFromBin(dsMoveRequest As MoveRequestDataSet, strBinNum As String) As MoveRequestDataSet
        MoveRequestBO.OnChangeFromBin(strBinNum, dsMoveRequest)
        Return dsMoveRequest
    End Function

    Function MoveRequest_OnChangeToWhse(dsMoveRequest As MoveRequestDataSet, strWhse As String) As MoveRequestDataSet
        MoveRequestBO.OnChangeToWhse(strWhse, dsMoveRequest)
        Return dsMoveRequest
    End Function

    Function MoveRequest_OnChangeToBin(dsMoveRequest As MoveRequestDataSet, strBinNum As String) As MoveRequestDataSet
        MoveRequestBO.OnChangeToBin(strBinNum, dsMoveRequest)
        Return dsMoveRequest
    End Function

    Function MoveRequest_ProcessRequest(dsMoveRequest As MoveRequestDataSet) As MoveRequestDataSet
        MoveRequestBO.ProcessRequest(dsMoveRequest)
        Return dsMoveRequest
    End Function

    Function EmpBasic_GetByID(strEmpID As String) As EmpBasicDataSet
        Dim myEmpBasicDS As New EmpBasicDataSet
        myEmpBasicDS = EmpBasicBO.GetByID(strEmpID)
        Return myEmpBasicDS
    End Function

    Function WhseBin_GetByID(strBinNum As String) As WhseBinDataSet
        Dim myBinDS As New WhseBinDataSet
        myBinDS = WhseBinBO.GetByID("SPL", strBinNum)
        Return myBinDS
    End Function
End Class