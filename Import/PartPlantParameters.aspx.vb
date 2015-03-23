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



    Protected Sub btnStart_Click(sender As Object, e As EventArgs) Handles btnStart.Click

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
            dsPart = BO_Part.GetByID(row(0))

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
                    BO_Part.ChangePartPlantSourceType(dsPart, row(18))
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
                        dsPart = BO_Part.ChangePartPlantVendorID(row(16), dsPart)
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
                dsPart = BO_Part.Update(dsPart)
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
            dsPart = BO_Part.ChangePartTypeCode(dsPart, row(2))
            dsPart.Tables("Part").Rows(0)("UpdatePartPlant") = False
            'ProdCode
            If IsDBNull(row(20)) Then
            Else

                dsPart.Tables("Part").Rows(0)("ProdCode") = row(20)
                dsPart.Tables("Part").Rows(0)("ProdCodeDescription") = row(20)
                dsPart = BO_Part.ChangePartProdCode(dsPart, row(20))
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
                dsPart = BO_Part.Update(dsPart)
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

    Public Shared Function GetClassDesc(strClassID As String) As String
        Dim sUser As String = "sc"
        Dim sPass As String = "DEMETER@!"
        Dim sServer As String = "zeus"
        Dim sPort As String = "9408"
        Dim sAppServer As String = String.Format("AppServerDC://{0}:{1}", sServer, sPort)
        Dim sCompany As String = "RMT"
        Dim session As Object = New Epicor.Mfg.Core.Session(sUser, sPass, sAppServer, Epicor.Mfg.Core.Session.LicenseType.Default)
        Dim connPool As New Epicor.Mfg.Core.BLConnectionPool(sUser, sPass, sAppServer)


        Dim dq As New Epicor.Mfg.BO.DynamicQuery(connPool)
        Dim qds As New Epicor.Mfg.BO.QueryDesignDataSet
        Dim dsDataSet As DataSet

        qds = dq.GetByID("RMT-GetPartClass")
        qds.QueryWhereItem(0).RValue = strClassID
        dsDataSet = dq.Execute(qds)

        connPool.Dispose()
        Return dsDataSet.Tables(0).Rows(0)(0)
    End Function
    Public Shared Function GetProdCodeDesc(strProdCode As String) As String
        Dim sUser As String = "sc"
        Dim sPass As String = "DEMETER@!"
        Dim sServer As String = "zeus"
        Dim sPort As String = "9408"
        Dim sAppServer As String = String.Format("AppServerDC://{0}:{1}", sServer, sPort)
        Dim sCompany As String = "RMT"
        Dim session As Object = New Epicor.Mfg.Core.Session(sUser, sPass, sAppServer, Epicor.Mfg.Core.Session.LicenseType.Default)
        Dim connPool As New Epicor.Mfg.Core.BLConnectionPool(sUser, sPass, sAppServer)


        Dim dq As New Epicor.Mfg.BO.DynamicQuery(connPool)
        Dim qds As New Epicor.Mfg.BO.QueryDesignDataSet
        Dim dsDataSet As DataSet

        qds = dq.GetByID("RMT-GetProdCode")
        qds.QueryWhereItem(0).RValue = strProdCode
        dsDataSet = dq.Execute(qds)

        connPool.Dispose()
        Return dsDataSet.Tables(0).Rows(0)(0)
    End Function
End Class
Public Class BO_Part
    Public Shared Function GetByID(strPartNum As String) As PartDataSet
        Dim sUser As String = "sc"
        Dim sPass As String = "DEMETER@!"
        Dim sServer As String = "zeus"
        Dim sPort As String = "9408"
        Dim sAppServer As String = String.Format("AppServerDC://{0}:{1}", sServer, sPort)
        Dim sCompany As String = "RMT"
        Dim session As Object = New Epicor.Mfg.Core.Session(sUser, sPass, sAppServer, Epicor.Mfg.Core.Session.LicenseType.Default)
        Dim connPool As New Epicor.Mfg.Core.BLConnectionPool(sUser, sPass, sAppServer)

        Dim myPart As New Part(connPool)
        Dim myPartDS As New PartDataSet

        myPartDS = myPart.GetByID(strPartNum)

        connPool.Dispose()

        Return mypartDS
    End Function
    Public Shared Function ChangePartPlantVendorID(strVendorID As String, dsPart As PartDataSet) As PartDataSet
        Dim sUser As String = "sc"
        Dim sPass As String = "DEMETER@!"
        Dim sServer As String = "zeus"
        Dim sPort As String = "9408"
        Dim sAppServer As String = String.Format("AppServerDC://{0}:{1}", sServer, sPort)
        Dim sCompany As String = "RMT"
        Dim session As Object = New Epicor.Mfg.Core.Session(sUser, sPass, sAppServer, Epicor.Mfg.Core.Session.LicenseType.Default)
        Dim connPool As New Epicor.Mfg.Core.BLConnectionPool(sUser, sPass, sAppServer)

        Dim myPart As New Part(connPool)

        myPart.ChangePartPlantVendorID(strVendorID, dsPart)

        connPool.Dispose()

        Return dsPart
    End Function
    Public Shared Function Update(dsPart As PartDataSet) As PartDataSet
        Dim sUser As String = "sc"
        Dim sPass As String = "DEMETER@!"
        Dim sServer As String = "zeus"
        Dim sPort As String = "9408"
        Dim sAppServer As String = String.Format("AppServerDC://{0}:{1}", sServer, sPort)
        Dim sCompany As String = "RMT"
        Dim session As Object = New Epicor.Mfg.Core.Session(sUser, sPass, sAppServer, Epicor.Mfg.Core.Session.LicenseType.Default)
        Dim connPool As New Epicor.Mfg.Core.BLConnectionPool(sUser, sPass, sAppServer)

        Dim myPart As New Part(connPool)

        myPart.Update(dsPart)

        connPool.Dispose()

        Return dsPart
    End Function
    Public Shared Function ChangePartPlantSourceType(dsPart As PartDataSet, strType As String) As PartDataSet
        Dim sUser As String = "sc"
        Dim sPass As String = "DEMETER@!"
        Dim sServer As String = "zeus"
        Dim sPort As String = "9408"
        Dim sAppServer As String = String.Format("AppServerDC://{0}:{1}", sServer, sPort)
        Dim sCompany As String = "RMT"
        Dim session As Object = New Epicor.Mfg.Core.Session(sUser, sPass, sAppServer, Epicor.Mfg.Core.Session.LicenseType.Default)
        Dim connPool As New Epicor.Mfg.Core.BLConnectionPool(sUser, sPass, sAppServer)

        Dim myPart As New Part(connPool)

        myPart.ChangePartPlantSourceType(strType, "", "", dsPart)

        connPool.Dispose()

        Return dsPart
    End Function
    Public Shared Function ChangePartTypeCode(dsPart As PartDataSet, strType As String) As PartDataSet
        Dim sUser As String = "sc"
        Dim sPass As String = "DEMETER@!"
        Dim sServer As String = "zeus"
        Dim sPort As String = "9408"
        Dim sAppServer As String = String.Format("AppServerDC://{0}:{1}", sServer, sPort)
        Dim sCompany As String = "RMT"
        Dim session As Object = New Epicor.Mfg.Core.Session(sUser, sPass, sAppServer, Epicor.Mfg.Core.Session.LicenseType.Default)
        Dim connPool As New Epicor.Mfg.Core.BLConnectionPool(sUser, sPass, sAppServer)

        Dim myPart As New Part(connPool)

        myPart.ChangePartTypeCode(strType, dsPart)

        connPool.Dispose()

        Return dsPart
    End Function
    Public Shared Function ChangePartProdCode(dsPart As PartDataSet, strProdCode As String) As PartDataSet
        Dim sUser As String = "sc"
        Dim sPass As String = "DEMETER@!"
        Dim sServer As String = "zeus"
        Dim sPort As String = "9408"
        Dim sAppServer As String = String.Format("AppServerDC://{0}:{1}", sServer, sPort)
        Dim sCompany As String = "RMT"
        Dim session As Object = New Epicor.Mfg.Core.Session(sUser, sPass, sAppServer, Epicor.Mfg.Core.Session.LicenseType.Default)
        Dim connPool As New Epicor.Mfg.Core.BLConnectionPool(sUser, sPass, sAppServer)

        Dim myPart As New Part(connPool)

        myPart.ChangePartProdCode(strProdCode, dsPart)

        connPool.Dispose()

        Return dsPart
    End Function
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