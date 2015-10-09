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

    Dim dtImportList As New DataTable, strErrorLog As String = ""
    Dim E10session As Ice.Core.Session
    Dim iLaunch As Ice.Lib.Framework.ILauncher
    Dim PartBO As PartImpl
    Dim DynamicQueryBO As DynamicQueryImpl

    Protected Sub Start_E10_Session()
        Dim sUser As String = "sc"
        Dim sPass As String = "DEMETER@!"
        E10session = New Ice.Core.Session(sUser, sPass, LicenseType.Default, "\\olympus\ERP10\ERP10.0.700\ClientDeployment\Client\Config\RMT-SHIA-APP03.sysconfig")
        iLaunch = New Ice.Lib.Framework.ILauncher(E10session)
        PartBO = WCFServiceSupport.CreateImpl(Of PartImpl)(E10session, PartImpl.UriPath)
        DynamicQueryBO = WCFServiceSupport.CreateImpl(Of DynamicQueryImpl)(E10session, DynamicQueryImpl.UriPath)
    End Sub

    Protected Sub btnStart_Click(sender As Object, e As EventArgs) Handles btnStart.Click

        Start_E10_Session()

        'Clear "Import Complete"
        lblImportComplete.Text = ""
        strErrorLog = ""

        'Save the file
        Dim UploadFileName As String = FileUpload1.PostedFile.FileName, strFilePath As String = ""
        Dim c As String = System.IO.Path.GetFileName(UploadFileName), strArchive As String = ""

        ''Set File Path
        strFilePath = "\\pithos\company\Service Connect\Rosenboom Connect\Part Plant Parameter Update\Temp\"

        'Attempt to upload the file
        FileUpload1.PostedFile.SaveAs(strFilePath & c)
        Threading.Thread.Sleep(100)
        'Attempt to read the file
        'dtImportList = CSV_File_Reader.Import_CSV_Data("SELECT * FROM [" & c & "]", "Excel", strFilePath)
        dtImportList = ReturnData(True, strFilePath & c)
        'dtImportList.TableName = "ImportList"
        'dtImportList.WriteXml("C:\XMLTest.xml")

        'Start the import process
        lbProgress.Text = ""
        Session.Add("Progress", "0")
        Session.Add("EndingMessage", "")
        Dim thrDOWORK As New Threading.Thread(AddressOf DO_WORK)
        lblImportComplete.Text = ""
        thrDOWORK.Start()
        Timer1.Enabled = True

    End Sub

    Public Function getColumns(ByVal ColumnNames As Boolean, FileName As String) As String()
        Try
            Dim fileReader As New StreamReader(FileName)
            Dim line As String = fileReader.ReadLine
            fileReader.Close()
            Dim Columns() As String = line.Split(",")
            If ColumnNames Then
                Return Columns
            End If
            Dim i As Integer = 1
            Dim c As Integer = 0
            Dim columnsNames(Columns.Count - 1) As String
            For Each column As String In Columns
                columnsNames(c) = "column" & i
                i += 1
                c += 1
            Next
            Return columnsNames
        Catch ex As Exception
            'log to file    
        End Try
        Return Nothing
    End Function

    Public Function ReturnData(ByVal ColumnNames As Boolean, FileName As String) As DataTable
        Try
            Dim dt As New DataTable
            For Each columnName In getColumns(ColumnNames, FileName)
                dt.Columns.Add(columnName)
            Next
            Dim fileReader As New StreamReader(FileName)
            If ColumnNames Then
                fileReader.ReadLine()
            End If
            Dim line As String = fileReader.ReadLine
            While Not IsNothing(line)
                line = line.Replace(Chr(34), "")
                dt.Rows.Add(line.Split(","))
                line = fileReader.ReadLine
            End While
            fileReader.Close()
            Return dt
        Catch ex As Exception
            'log to file
        End Try
        Return Nothing
    End Function

    Protected Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        Try
            'Update the label
            lbProgress.Text = Session("Progress").ToString & "%"

            'Hide when finished
            If lbProgress.Text = "100%" Then
                Timer1.Enabled = False
                lbProgress.Text = ""
                progresspanel.Visible = False
                'lblImportComplete.Text = Session("EndingMessage").ToString
                txtErrorLog.Text = Session("EndingMessage").ToString
            End If
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Public Sub DO_WORK()
        Dim intRow As Integer = 1, dsPart As New PartDataSet
        For Each row As DataRow In dtImportList.Rows

            'Get Data
            dsPart = Part_GetByID(row(0))

            'Update Plant Parameters
            For Each row2 As DataRow In dsPart.Tables("PartPlant").Rows
                If row2("Plant") = row(3) Then
                    row2("RowMod") = "U"
                    row2("MinimumQty") = row(4)
                    row2("MaximumQty") = row(5)
                    row2("SafetyQty") = row(6)
                    row2("MinOrderQty") = row(7)
                    row2("LeadTime") = row(8)
                    row2("ReceiveTime") = row(9)
                    row2("MfgLotMultiple") = row(10)
                    row2("ProcessMRP") = Check_Boolean(row(11))
                    row2("GenerateSugg") = Check_Boolean(row(12))
                    If IsDBNull(row(13)) = True Then
                    Else
                        row2("BuyerID") = row(13)
                    End If
                    If IsDBNull(row(14)) = True Then
                    Else
                        row2("PersonID") = row(14)

                    End If
                    row2("DaysOfSupply") = row(15)

                    'UPDATE SOURCE TYPE
                    Part_ChangePartPlantSourceType(dsPart, row(18))
                    row2("SourceType") = row(18)
                    If row(18) = "T" Then
                        If row(3) = "MfgSys" Then
                            row2("TransferPlant") = "SPIRITLA"
                        Else
                            row2("TransferPlant") = "MfgSys"
                        End If
                        row2("TransferLeadTime") = 2
                    Else
                    End If

                    'Change Plant Vendor
                    If IsDBNull(row(16)) Then
                    Else
                        row2("VendorNumVendorID") = row(16)
                        dsPart = Part_ChangePartPlantVendorID(row(16), dsPart)
                        Try
                            If IsDBNull(row(17)) Then
                            Else
                                row2("PurPoint") = row(17)
                            End If
                        Catch ex As Exception
                        End Try
                    End If

                Else
                End If
            Next
            'dsPart.AcceptChanges()


            'UPDATE PLANT
            Try
                'Update the file
                dsPart = Part_Update(dsPart)
            Catch ex As Exception
                strErrorLog &= dsPart.Tables(0).Rows(0)("PartNum") & " - " & ex.Message & vbCrLf
            End Try

            'UPDATE PART HEADER
            dsPart.Tables("Part").Rows(0)("RowMod") = "U"
            'Change Class
            If IsDBNull(row(1)) Then
            Else
                Try
                    dsPart.Tables("Part").Rows(0)("ClassDescription") = GetClassDesc(row(1))
                    dsPart.Tables("Part").Rows(0)("ClassID") = row(1)
                Catch ex As Exception
                End Try
            End If
            'TypeCode
            dsPart = Part_ChangePartTypeCode(dsPart, row(2))
            dsPart.Tables("Part").Rows(0)("UpdatePartPlant") = False
            'ProdCode
            If IsDBNull(row(20)) Then
            Else

                dsPart.Tables("Part").Rows(0)("ProdCode") = row(20)
                dsPart.Tables("Part").Rows(0)("ProdCodeDescription") = row(20)
                dsPart = Part_ChangePartProdCode(dsPart, row(20))
            End If
            'Outsourced
            dsPart.Tables("Part").Rows(0)("Checkbox03") = Check_Boolean(row(19))
            'Forecast VendorID
            If IsDBNull(row(21)) Then
            Else
                dsPart.Tables("Part").Rows(0)("UserChar3") = row(21)
            End If

            Try
                'Update the file
                dsPart = Part_Update(dsPart)
            Catch ex As Exception
                strErrorLog &= dsPart.Tables(0).Rows(0)("PartNum") & " - " & ex.Message & vbCrLf
            End Try

            'Update User and Reset Variables
            Session("Progress") = Math.Round((intRow / dtImportList.Rows.Count) * 100, 0)
            If strErrorLog = "" Then
                Session("EndingMessage") = "Import Complete!"
            Else
                Session("EndingMessage") = "Import completed with errors. " & vbCrLf & vbCrLf & strErrorLog
            End If
            intRow += 1
            dsPart.Clear()
        Next

        Session("Progress") = 100
        ''Write Error Log
        'If strErrorLog = "" Then
        'Else
        '    Dim rand As New Random, letter As String = "", n As Integer
        '    'Get random file name
        '    For n = 0 To 8
        '        letter &= ChrW(rand.Next(Asc("A"), Asc("Z") + 1))
        '    Next
        '    Dim strMyFilePath As String = "\\pithos\company\Service Connect\Rosenboom Connect\Part Plant Parameter Update\Temp\Part Parameter Update Error Log " & letter & ".txt"
        '    Dim TextFile As New IO.StreamWriter(strMyFilePath)
        '    TextFile.Write(strErrorLog)
        '    TextFile.Close()
        '    'txtErrorLog.Text = strErrorLog
        '    'Download_File("Error Log", strMyFilePath)

        'End If
    End Sub

    Protected Sub Download_File(ByVal strFileName As String, ByVal strFilePath As String)

        'Try

        Dim filename As String = strFileName

        'set the http content type to "APPLICATION/OCTET-STREAM
        Response.ContentType = "APPLICATION/OCTET-STREAM"

        'initialize the http content-disposition header to
        'indicate a file attachment with the default filename
        '"myFile.txt"
        Dim disHeader As String = "Attachment; Filename=""" & filename & """"
        Response.AppendHeader("Content-Disposition", disHeader)

        'transfer the file byte-by-byte to the response object
        Dim fileToDownload As New System.IO.FileInfo(strFilePath)

        Response.TransmitFile(fileToDownload.FullName)
        Response.Flush()

        'Catch ex As Exception

        'MsgBox(ex.Message)
        'End Try
        Response.End()
    End Sub

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

    Public Function GetClassDesc(strClassID As String) As String
        Dim _dqDS As New Ice.BO.QueryExecutionDataSet()
        _dqDS.ExecutionParameter.AddExecutionParameterRow("xClassID", "002", "nvarchar", False, Guid.NewGuid(), "A")

        Dim dsDataSet As DataSet = DynamicQueryBO.ExecuteByID("RMT-GetPartClass", _dqDS)

        Return dsDataSet.Tables(0).Rows(0)(0)
    End Function
    Public Function GetProdCodeDesc(strProdCode As String) As String
        Dim _dqDS As New Ice.BO.QueryExecutionDataSet()
        _dqDS.ExecutionParameter.AddExecutionParameterRow("xProdCode", strProdCode, "nvarchar", False, Guid.NewGuid(), "A")

        Dim dsDataSet As DataSet = DynamicQueryBO.ExecuteByID("RMT-GetProdCode", _dqDS)

        Return dsDataSet.Tables(0).Rows(0)(0)
    End Function


    Public Function Part_GetByID(strPartNum As String) As PartDataSet
        Dim myPartDS As New PartDataSet
        myPartDS = PartBO.GetByID(strPartNum)
        Return myPartDS
    End Function
    Public Function Part_ChangePartPlantVendorID(strVendorID As String, dsPart As PartDataSet) As PartDataSet
        PartBO.ChangePartPlantVendorID(strVendorID, dsPart)
        Return dsPart
    End Function
    Public Function Part_Update(dsPart As PartDataSet) As PartDataSet
        PartBO.Update(dsPart)
        Return dsPart
    End Function
    Public Function Part_ChangePartPlantSourceType(dsPart As PartDataSet, strType As String) As PartDataSet
        PartBO.ChangePartPlantSourceType(strType, "", "", dsPart)
        Return dsPart
    End Function
    Public Function Part_ChangePartTypeCode(dsPart As PartDataSet, strType As String) As PartDataSet
        PartBO.ChangePartTypeCode(strType, dsPart)
        Return dsPart
    End Function
    Public Function Part_ChangePartProdCode(dsPart As PartDataSet, strProdCode As String) As PartDataSet
        PartBO.ChangePartProdCode(strProdCode, dsPart)
        Return dsPart
    End Function
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