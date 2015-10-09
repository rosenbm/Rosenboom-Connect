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

    Dim dtImportList As New DataTable, strErrorLog As String = ""

    Function Check_Boolean(intVal As Integer) As Boolean
        If intVal = 0 Then
            Return False
        Else
            Return True
        End If
    End Function

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        Dim MESTab As System.Web.UI.HtmlControls.HtmlGenericControl
        MESTab = Master.FindControl("importtab")
        MESTab.Attributes.Add("class", "active")

        'Check to see if the loading div needs to be displayed
        If lbProgress.Text = "" Then
            progresspanel.Visible = False
        Else
            progresspanel.Visible = True
        End If
    End Sub

    'Public Shared Function GetClassDesc(strClassID As String) As String
    '    Dim sUser As String = "sc"
    '    Dim sPass As String = "DEMETER@!"
    '    Dim sServer As String = "zeus"
    '    Dim sPort As String = "9408"
    '    Dim sAppServer As String = String.Format("AppServerDC://{0}:{1}", sServer, sPort)
    '    Dim sCompany As String = "RMT"
    '    Dim session As Object = New Epicor.Mfg.Core.Session(sUser, sPass, sAppServer, Epicor.Mfg.Core.Session.LicenseType.Default)
    '    Dim connPool As New Epicor.Mfg.Core.BLConnectionPool(sUser, sPass, sAppServer)


    '    Dim dq As New Epicor.Mfg.BO.DynamicQuery(connPool)
    '    Dim qds As New Epicor.Mfg.BO.QueryDesignDataSet
    '    Dim dsDataSet As DataSet

    '    qds = dq.GetByID("RMT-GetPartClass")
    '    qds.QueryWhereItem(0).RValue = strClassID
    '    dsDataSet = dq.Execute(qds)

    '    connPool.Dispose()
    '    Return dsDataSet.Tables(0).Rows(0)(0)
    'End Function
    'Public Shared Function GetProdCodeDesc(strProdCode As String) As String
    '    Dim sUser As String = "sc"
    '    Dim sPass As String = "DEMETER@!"
    '    Dim sServer As String = "zeus"
    '    Dim sPort As String = "9408"
    '    Dim sAppServer As String = String.Format("AppServerDC://{0}:{1}", sServer, sPort)
    '    Dim sCompany As String = "RMT"
    '    Dim session As Object = New Epicor.Mfg.Core.Session(sUser, sPass, sAppServer, Epicor.Mfg.Core.Session.LicenseType.Default)
    '    Dim connPool As New Epicor.Mfg.Core.BLConnectionPool(sUser, sPass, sAppServer)


    '    Dim dq As New Epicor.Mfg.BO.DynamicQuery(connPool)
    '    Dim qds As New Epicor.Mfg.BO.QueryDesignDataSet
    '    Dim dsDataSet As DataSet

    '    qds = dq.GetByID("RMT-GetProdCode")
    '    qds.QueryWhereItem(0).RValue = strProdCode
    '    dsDataSet = dq.Execute(qds)

    '    connPool.Dispose()
    '    Return dsDataSet.Tables(0).Rows(0)(0)
    'End Function

    Protected Sub btnOpen_Click(sender As Object, e As EventArgs) Handles btnOpen.Click
        Update_Vantage.Update_Database("UPDATE ERP.NonConf SET InspectionPending = '1' WHERE Company='RMT' AND TranID='" & txtTranID.Text & "'")
    End Sub

    Protected Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        Update_Vantage.Update_Database("UPDATE ERP.NonConf SET InspectionPending = '0' WHERE Company='RMT' AND TranID='" & txtTranID.Text & "'")
    End Sub

    Protected Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick

    End Sub
End Class
Public Class Update_Vantage
    Public Shared Sub Update_Database(ByVal Query As String)
        Dim cn As OdbcConnection, cmd As OdbcCommand

        'Set cn
        cn = New OdbcConnection("DSN=ERP10RC;Driver={SQL Server};Server=olympus;Database=ERP10;Uid=ERP10;Pwd=ERP10;")

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
        Catch ex As Exception
            MsgBox(ex.Message)
        Finally
            cn.Close()
        End Try
    End Sub
End Class

Public Class BO_PartClass

End Class
Public Class CSV_File_Reader
    Public Shared Function Import_CSV_Data(ByVal strQuery As String, ByVal strTable As String, _
    ByVal strDatabaseFilePath As String) As DataTable

        Try
            Dim cn As OleDbConnection, da As OleDbDataAdapter

            'Set cn
            cn = New OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0; Data Source=" & strDatabaseFilePath & _
                                     "; Extended Properties=text;HDR=YES;FMT=Delimited"";")

            'Open Connection
            cn.Open()

            'Create Data Adapter
            da = New OleDbDataAdapter(strQuery, cn)

            'Create and Fill Dataset
            Dim ds As New DataSet
            da.Fill(ds, strTable)

            'Get Data Table
            Dim dt As DataTable = ds.Tables(strTable)

            'Close Connection
            cn.Close()

            'Return the new Excel Data Table
            Return dt
        Catch ex As Exception
            MsgBox(ex.Message)

        End Try
    End Function
End Class
Public Class Excel_2007_File_Reader
    Public Shared Function Import_Excel_Data(ByVal strQuery As String, ByVal strTable As String, _
        ByVal strDatabaseFilePath As String) As DataTable

        Try
            Dim cn As OleDbConnection, da As OleDbDataAdapter

            'Set cn
            cn = New OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0; Data Source=" & strDatabaseFilePath & _
                                     "; Extended Properties=""Excel 12.0;IMEX=1;HDR=NO;TypeGuessRows=0;ImportMixedTypes=Text""")

            'Open Connection
            cn.Open()

            'Create Data Adapter
            da = New OleDbDataAdapter(strQuery, cn)

            'Create and Fill Dataset
            Dim ds As New DataSet
            da.Fill(ds, strTable)

            'Get Data Table
            Dim dt As DataTable = ds.Tables(strTable)

            'Close Connection
            cn.Close()

            'Return the new Excel Data Table
            Return dt
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Function

End Class