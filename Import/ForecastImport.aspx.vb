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

    Dim dtForecast As New DataTable, dtCrossRef As New DataTable

    Protected Sub btnStart_Click(sender As Object, e As EventArgs) Handles btnStart.Click
        Dim strFileType As String = ""
        'Clear "Import Complete"
        lblImportComplete.Text = ""

        'Save the file
        Dim UploadFileName As String = FileUpload1.PostedFile.FileName, strFilePath As String = ""
        Dim c As String = System.IO.Path.GetFileName(UploadFileName), strArchive As String = ""

        'Set File Path
        strFilePath = "\\pithos\company\Service Connect\Rosenboom Connect\Forecast Import\"

        'Attempt to upload the file
        FileUpload1.PostedFile.SaveAs(strFilePath & c)

        'Read the uploaded file
        strFileType = c.Substring(c.LastIndexOf(".") + 1)
        If strFileType = "xlsx" Then
            dtForecast = Excel_2007_File_Reader.Import_Excel_Data("SELECT * FROM [" & txtExcelSheetName.Text & "$]", "Excel", strFilePath & c)
        Else
            dtForecast = Excel_97_03_File_Reader.Import_Old_Excel_Data("SELECT * FROM [" & txtExcelSheetName.Text & "$]", "Excel", strFilePath & c)
        End If

        'Start the import process
        lbProgress.Text = ""
        Session.Add("Progress", "0")
        Session.Add("Stage", "")
        Dim thrDOWORK As New Threading.Thread(AddressOf DO_WORK)
        thrDOWORK.Start()
        Timer1.Enabled = True

    End Sub

    Protected Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick

        'Update the label
        lbProgress.Text = Session("Progress").ToString & "%"
        lblStage.Text = Session("Stage").ToString

        'Hide when finished
        If lbProgress.Text = "100%" Then
            Timer1.Enabled = False
            lbProgress.Text = ""
            progresspanel.Visible = False
            lblImportComplete.Text = "Import Complete!"
            txtErrorLog.Text = Session("Stage").ToString
        End If
    End Sub

    Public Sub DO_WORK()
        Dim intCustNum As Integer = Get_Cust_Num(), dsForecastsToDelete As New ForecastDataSet, i As Integer = 0, _
            intStatus As Integer

        'Delete the old forecasts if clear and reload

        If rbClearReload.Checked = True Then
            Session("Stage") = "Deleting Old Forecasts"
            dsForecastsToDelete = BO_Forecast.Get_Rows("CustNum = " & intCustNum & " AND Checkbox01 = false")
            'Go through each row and delete
            For Each row As DataRow In dsForecastsToDelete.Tables("Forecast").Rows
                BO_Forecast.Delete_By_ID(row("PartNum"), row("Plant"), row("CustNum"), row("ForeDate"))
                intStatus = Math.Round(((i + 1) / dsForecastsToDelete.Tables("Forecast").Rows.Count) * 100, 0)
                If intStatus = 100 Then
                    intStatus = 99
                End If
                Session("Progress") = intStatus
                i += 1
            Next
        Else
        End If

        'Load new forecasts
        Select Case ddlCustomer.Text
            Case "AGCO"
                AGCO(Get_Cust_Num)
            Case "AGCO Sunflower"
                AGCO(Get_Cust_Num)
            Case "Altec"
                Altec()
            Case "Allmand"
                Manual_Format(Get_Cust_Num)
            Case "Ariens"
                Manual_Format(Get_Cust_Num)
            Case "CAT"
                CAT(Get_Cust_Num)
            Case "CNH Belo Horizonte"
                CNH(Get_Cust_Num)
            Case "CNH Benson"
                CNH(Get_Cust_Num)
            Case "CNH Burlington"
                CNH(Get_Cust_Num)
            Case "CNH Canada"
                CNH(Get_Cust_Num)
            Case "CNH Calhoun"
                CNH(Get_Cust_Num)
            Case "CNH Cordoba"
                CNH(Get_Cust_Num)
            Case "CNH Curitiba"
                CNH(Get_Cust_Num)
            Case "CNH Italia"
                CNH(Get_Cust_Num)
            Case "CNH Latin America"
                CNH(Get_Cust_Num)
            Case "CNH Lecce"
                CNH(Get_Cust_Num)
            Case "CNH Parts"
                CNH_Parts(Get_Cust_Num)
            Case "CNH Piracicaba"
                CNH(Get_Cust_Num)
            Case "CNH Saskatoon"
                CNH(Get_Cust_Num)
            Case "CNH Wichita"
                CNH(Get_Cust_Num)
            Case "Gehl Madison"
                Gehl_Madison(Get_Cust_Num)
            Case "Gehl Yankton"
                Gehl_Yankton(Get_Cust_Num)
            Case "Hagie"
                Hagie(Get_Cust_Num)
            Case "Heil"
                Heil(Get_Cust_Num)
            Case "JLG"
                OTC(intCustNum)
            Case "John Deere"
                DEERE(Get_Cust_Num)
            Case "Lift Tek"
                LiftTek(Get_Cust_Num)
            Case "McNeilus"
                OTC(intCustNum)
            Case "OshKosh Mexico"
                OshKosh_Mexico(Get_Cust_Num)
            Case "OTC"
                OTC(intCustNum)
            Case "Pierce"
                OTC(intCustNum)
            Case "Scranton"
                Scranton(intCustNum)
            Case "Vermeer"
                Vermeer(Get_Cust_Num)
            Case "Volvo"
                Volvo(intCustNum)
            Case "Wacker-Neuson"
                WackerNeuson(Get_Cust_Num)
            Case "Weiler"
                Weiler(Get_Cust_Num)
            Case "World Class"
                Manual_Format(Get_Cust_Num)
            Case "Xtreme"
                Xtreme(Get_Cust_Num)
            Case Else
        End Select


        'For i = 0 To dtForecast.Rows.Count - 1
        '    Threading.Thread.Sleep(200)
        '    'Update the % complete
        '    Session("Progress") = Math.Round(((i + 1) / dtForecast.Rows.Count) * 100, 0)
        'Next

    End Sub

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

    Private Function Get_Cust_Num() As Integer
        Select Case ddlCustomer.Text
            Case "AGCO"
                Return 3
            Case "AGCO Sunflower"
                Return 722
            Case "Ariens"
                Return 533
            Case "Altec"
                Return 18
            Case "Allmand"
                Return 14
            Case "CAT"
                Return 348
            Case "CNH Belo Horizonte"
                Return 824
            Case "CNH Benson"
                Return 825
            Case "CNH Burlington"
                Return 827
            Case "CNH Canada"
                Return 809
            Case "CNH Calhoun"
                Return 828
            Case "CNH Cordoba"
                Return 836
            Case "CNH Curitiba"
                Return 699
            Case "CNH Italia"
                Return 775
            Case "CNH Parts"
                Return 796
            Case "CNH Piracicaba"
                Return 829
            Case "CNH Wichita"
                Return 826
            Case "Gehl Madison"
                Return 82
            Case "Gehl Yankton"
                Return 82
            Case "Hagie"
                Return 334
            Case "Heil"
                Return 574
            Case "JLG"
                Return 225
            Case "John Deere"
                Return 390
            Case "Lift Tek"
                Return 430
            Case "McNeilus"
                Return 337
            Case "OshKosh Mexico"
                Return 839
            Case "OTC"
                Return 332
            Case "Pierce"
                Return 362
            Case "Scranton"
                Return 299
            Case "Vermeer"
                Return 485
            Case "Volvo"
                Return 392
            Case "Wacker-Neuson"
                Return 776
            Case "Weiler"
                Return 831
            Case "World Class"
                Return 728
            Case "Xtreme"
                Return 582
            Case Else
                Return 0
        End Select
    End Function

    Private Sub Import_CrossRef(ByVal strCustomer As String)
        'Clear anything remainng in the table
        dtCrossRef.Clear()
        'Import Cross Ref Table
        Try
            dtCrossRef = Excel_2007_File_Reader.Import_Excel_Data("SELECT * FROM [" & strCustomer & "$]", "CrossRef", "\\Pithos\Company\Customer Service\Forecast Cross Reference.xlsx")
        Catch ex As Exception
            'MsgBox(ex.Message)
            lblImportComplete.Text = ex.Message
        End Try

    End Sub

    Private Sub AGCO(intCustNum As Integer)
        Dim strPlant As String = "", strOurPartNum As String, strCustomerPartNum As String, dteForecastDate As DateTime, dteCutOffDate As Date = Today.AddDays(21), _
             intOrderQty As Integer, i As Integer = 0, decTest As Decimal = 0, intPNRow As Integer = 1, _
             strErrorLog As String = ""

        'Import Cross Ref
        Import_CrossRef(ddlCustomer.Text)

        Session("Stage") = "Importing Forecasts"

        'Start Generating Forecast
        For Each row As DataRow In dtForecast.Rows
            If i <= 4 Then 'Skip first row
                'ElseIf i <> intPNRow Then
            Else
                If IsDBNull(row("F1")) = True Then
                Else
                    strCustomerPartNum = Trim(row("F1").ToString)

                    'Get our PN
                    For Each row2 As DataRow In dtCrossRef.Rows
                        'Bypass null values
                        If IsDBNull(row2("F1")) Then
                        Else
                            If strCustomerPartNum = row2("F1") Then
                                strPlant = row2("F3")
                                strOurPartNum = row2("F2")
                                Exit For
                            Else
                            End If 'If part number match in cross ref
                        End If 'if cross ref row is blank
                    Next 'for each row in cross ref

                    'Go through each column
                    'dteForecastDate = Today()
                    For intCol As Integer = 5 To dtForecast.Columns.Count - 1

                        If IsDBNull(dtForecast.Rows(i)("F" & intCol)) = True Then
                        ElseIf IsNumeric(dtForecast.Rows(i)("F" & intCol)) = False Then

                        ElseIf strPlant = "" Then
                        ElseIf strPlant = "GMI" Then
                            'ElseIf IsDate(dtForecast.Rows(4)("F" & intCol)) = False Then
                        Else
                            'There is a number here
                            intOrderQty = CInt(dtForecast.Rows(i)("F" & intCol))
                            dteForecastDate = New Date(CInt(Left(dtForecast.Rows(4)("F" & intCol), 4)), CInt(Mid(dtForecast.Rows(4)("F" & intCol), 6, 2)), CInt(Right(dtForecast.Rows(4)("F" & intCol), 2)))
                            If dteForecastDate < dteCutOffDate Then
                            Else

                                'Create Forecast
                                Create_Forecast(strPlant, strOurPartNum, dteForecastDate, intOrderQty, intCustNum)
                            End If
                        End If

                    Next 'column in the spreadsheet

                    'Check for error
                    If strPlant = "" Then
                        If strErrorLog = "" Then
                            strErrorLog = "The following customer parts had errors:" & vbCrLf
                        End If
                        strErrorLog &= strCustomerPartNum & vbCrLf
                    End If 'check for errors
                End If 'If blank row in forecast

                'increment to the next PN row
                intPNRow += 4
            End If 'if first row

            'UPDATE STATUS
            i += 1
            Session("Progress") = Math.Round(((i + 1) / dtForecast.Rows.Count) * 100, 0)
            strPlant = ""

        Next 'Next row in forecast

        Session("Progress") = 100
        Session("Stage") = strErrorLog
    End Sub

    Private Sub Altec()
        Dim strPlant As String, strOurPartNum As String = "", strCustomerPartNum As String, _
             intCustomerID As Integer = 26, dteForecastDate As DateTime, dteCutOffDate As Date = Today.AddDays(21), _
             intOrderQty As Integer, intRowCount As Integer, strDateString As String = "", bolV8 As Boolean = True, _
             intCustNum As Integer = Get_Cust_Num(), strErrorLog As String = "", i As Integer, intStatus As Integer, _
             dblOnHand As Double = 0, intDayMod As Integer = 0, strLastPN As String = "", dblMinOrder As Double = 0, _
             dblRemOrd As Double = 0, intMoveUp As Integer = 0

            'Import Cross Ref
            Import_CrossRef(ddlCustomer.Text)

            Session("Stage") = "Importing Forecasts"

            'Combine Rows
        'For intRowCount = 2 To dtForecast.Rows.Count - 2
        'If dtForecast.Rows(intRowCount)("F1") = dtForecast.Rows(intRowCount + 1)("F1") Then
        '    For intCols As Integer = 5 To 7
        '        dtForecast.Rows(intRowCount + 1)("F" & intCols) = CInt(dtForecast.Rows(intRowCount + 1)("F" & intCols)) + CInt(dtForecast.Rows(intRowCount)("F" & intCols))
        '        dtForecast.Rows(intRowCount)("F" & intCols) = 0
        '    Next
        '    For intCols As Integer = 15 To 33
        '        dtForecast.Rows(intRowCount + 1)("F" & intCols) = CInt(dtForecast.Rows(intRowCount + 1)("F" & intCols)) + CInt(dtForecast.Rows(intRowCount)("F" & intCols))
        '        dtForecast.Rows(intRowCount)("F" & intCols) = 0
        '    Next
        'End If
        'Next

        Dim intMax As Integer = dtForecast.Rows.Count


        intRowCount = 1

        For Each row As DataRow In dtForecast.Rows
            If intRowCount = 1 Then
            Else

                'Get PN from Crossref
                strCustomerPartNum = row("F1")
                For Each row2 As DataRow In dtCrossRef.Rows
                    If IsDBNull(row2("F1")) Then
                    ElseIf strCustomerPartNum = row2("F1") Then
                        strPlant = row2("F3")
                        strOurPartNum = row2("F2")
                        intMoveUp = CInt(row2("F4"))
                        Exit For
                    Else
                    End If 'If customer part matches in cross ref
                Next 'For each row in the cross ref table

                'Get Onhand Qty and Min Order
                dblOnHand = CInt(row("F5")) + CInt(row("F7"))
                dblMinOrder = CInt(row("F9"))
                dblRemOrd = 0

                'Determine day mod
                If strOurPartNum = strLastPN Then
                    intDayMod += 1
                Else
                    intDayMod = 0
                End If

                'Go through each column
                For intColDates As Integer = 15 To 33

                    'Get forecast date of this column
                    strDateString = dtForecast.Rows(0)("F" & intColDates).ToString.Substring(dtForecast.Rows(0)("F" & intColDates).ToString.IndexOf("-") + 1)
                    strDateString = strDateString.TrimStart(" ")
                    dteForecastDate = DateValue(strDateString)
                    dteForecastDate = dteForecastDate.AddDays(intDayMod - intMoveUp)
                    dteForecastDate = FormatDateTime(dteForecastDate, DateFormat.ShortDate)

                    If row("F" & intColDates) = "0" Then
                    ElseIf IsDBNull(row("F1")) = True Then
                    Else

                        'Get order qty
                        intOrderQty = row("F" & intColDates)

                        'Calculate correct onhand and order qtys
                        If dblOnHand >= intOrderQty Then
                            dblOnHand -= intOrderQty
                            intOrderQty = 0
                        ElseIf dblOnHand > 0 Then
                            intOrderQty -= dblOnHand
                            dblOnHand = 0
                        End If

                        'Calculate if order is needed
                        If dblMinOrder = 0 Then
                        ElseIf intOrderQty = 0 Then
                        ElseIf dblRemOrd < intOrderQty Then
                            'Not enough to cover demand, need a new order
                            dblRemOrd -= intOrderQty

                            If dblMinOrder > intOrderQty Then
                                intOrderQty = dblMinOrder
                            Else
                            End If

                            'Add our new order qty to the bank
                            dblRemOrd += intOrderQty
                        Else
                            dblRemOrd -= intOrderQty
                            intOrderQty = 0

                        End If

                        'Check for blank plant
                        If strPlant = "" Then
                            If strErrorLog = "" Then
                                strErrorLog = "The following customer parts had errors:" & vbCrLf
                            End If
                            strErrorLog &= strCustomerPartNum & vbCrLf
                            'Check for OHIO
                        ElseIf strPlant = "GMI" Then
                        ElseIf intOrderQty = 0 Then
                        ElseIf dblOnHand > 0 Then
                        ElseIf dteForecastDate < dteCutOffDate Then
                        Else
                            'Create Forecast
                            Create_Forecast(strPlant, strOurPartNum, dteForecastDate, intOrderQty, intCustNum)
                        End If 'Plant Check
                    End If 'Check Forecast date and if bad qty
                Next 'Col
            End If 'intRowCount = 1

            strPlant = ""
            intRowCount += 1
            i += 1
            dblOnHand = 0
            strLastPN = strOurPartNum

            'UPDATE STATUS
            intStatus = Math.Round((i / intMax) * 100, 0)
            If intStatus = 100 Then
                intStatus = 99
            End If
            Session("Progress") = intStatus

        Next 'Row



        Session("Stage") = strErrorLog
        Session("Progress") = 100


    End Sub

    Private Sub CAT(intCustNum As Integer)
        Dim strPlant As String = "", strOurPartNum As String = "", strCustomerPartNum As String, _
        dteForecastDate As DateTime, dteCutOffDate As Date = Today.AddDays(21), _
        intOrderQty As Integer, i As Integer = 1, strErrorLog As String = "", intStatus As Integer = 0

        'Import Cross Ref
        Import_CrossRef("CAT")

        'Update status
        Session("Stage") = "Importing Forecasts"


        For Each row As DataRow In dtForecast.Rows
            If i = 1 Then 'skip first row
            ElseIf IsDBNull(row("F1")) = True Then 'skip if null part
            Else
                'Grab initial values
                strCustomerPartNum = row("F2")
                dteForecastDate = row("F8")
                intOrderQty = CInt(row("F6"))

                'Skip dates that are before the cutoff date or if it is not a forecast
                If dteForecastDate < dteCutOffDate Then 'do nothing
                Else
                    For Each row2 As DataRow In dtCrossRef.Rows
                        If strCustomerPartNum = row2("F1") Then
                            strPlant = row2("F3")
                            strOurPartNum = row2("F2")
                            Exit For
                        Else
                        End If 'If customer part matches in cross ref
                    Next 'For each row in the cross ref table

                    'Check for blank plant
                    If strPlant = "" Then
                        If strErrorLog = "" Then
                            strErrorLog = "The following customer parts had errors:" & vbCrLf
                        End If
                        strErrorLog &= strCustomerPartNum & vbCrLf
                        'Check for OHIO
                    ElseIf strPlant = "GMI" Then
                    Else
                        'Create Forecast
                        Try
                            Create_Forecast(strPlant, strOurPartNum, dteForecastDate, intOrderQty, intCustNum)
                        Catch ex As Exception
                            If strErrorLog = "" Then
                                strErrorLog = "The following customer parts had errors:" & vbCrLf
                            End If
                            strErrorLog &= strCustomerPartNum & vbCrLf
                        End Try
                    End If 'Plant Check


                End If 'Cutoff date
            End If 'If first row

            'VARIABLE CLEAN UP
            strPlant = ""
            i += 1
            intStatus = Math.Round(((i + 1) / dtForecast.Rows.Count) * 100, 0)
            If intStatus = 100 Then
                intStatus = 99
            End If
            Session("Progress") = intStatus

        Next
        Session("Progress") = 100
        Session("Stage") = strErrorLog
    End Sub

    Private Sub CNH(intCustNum As Integer)
        Dim strPlant As String = "", strOurPartNum As String = "", strCustomerPartNum As String, _
        dteForecastDate As DateTime, dteCutOffDate As Date = Today.AddDays(21), strForecastDate As String = "", _
        intOrderQty As Integer, i As Integer = 1, strParsedDate As String = "", strErrorLog As String = "", intStatus As Integer = 0

        'Import Cross Ref
        Import_CrossRef("CNH")

        'Update status
        Session("Stage") = "Importing Forecasts"


        For Each row As DataRow In dtForecast.Rows
            If i = 1 Then 'skip first row
            ElseIf IsDBNull(row("F1")) = True Then 'skip if null part
            Else
                'Grab initial values
                strCustomerPartNum = row("F1")
                strForecastDate = row("F7").ToString
                intOrderQty = CInt(row("F8"))
                strParsedDate = Mid(strForecastDate, 5, 2) & "/"
                strParsedDate &= Mid(strForecastDate, 7, 2) & "/"
                strParsedDate &= Mid(strForecastDate, 1, 4)
                dteForecastDate = DateValue(strParsedDate)
                dteForecastDate = FormatDateTime(dteForecastDate, DateFormat.ShortDate)

                'Skip dates that are before the cutoff date or if it is not a forecast
                If dteForecastDate < dteCutOffDate Then 'do nothing
                Else
                    For Each row2 As DataRow In dtCrossRef.Rows
                        If strCustomerPartNum = row2("F1") Then
                            strPlant = row2("F3")
                            strOurPartNum = row2("F2")
                            Exit For
                        Else
                        End If 'If customer part matches in cross ref
                    Next 'For each row in the cross ref table

                    'Check for blank plant
                    If strPlant = "" Then
                        'strOurPartNum = InputBox("Please enter the RMT part for AGCO part #" & strCustomerPartNum)
                        'strPlant = InputBox("Please enter plant for " & strOurPartNum & vbCrLf & vbCrLf & _
                        '                        "Sheldon = MfgSys" & vbCrLf & "Spirit Lake = SPIRITLA" & vbCrLf & _
                        '                        "Ohio = GMI")
                        'dteForecastDate = dteForecastDate.AddDays(-7)
                        'dteForecastDate = FormatDateTime(dteForecastDate, DateFormat.ShortDate)
                        If strErrorLog = "" Then
                            strErrorLog = "The following customer parts had errors:" & vbCrLf
                        End If
                        strErrorLog &= strCustomerPartNum & vbCrLf
                        'Check for OHIO
                    ElseIf strPlant = "GMI" Then
                    Else
                        'Create Forecast
                        Try
                            Create_Forecast(strPlant, strOurPartNum, dteForecastDate, intOrderQty, intCustNum)
                        Catch ex As Exception
                            If strErrorLog = "" Then
                                strErrorLog = "The following customer parts had errors:" & vbCrLf
                            End If
                            strErrorLog &= strCustomerPartNum & vbCrLf
                        End Try
                    End If 'Plant Check


                End If 'Cutoff date
            End If 'If first row

            'VARIABLE CLEAN UP
            strPlant = ""
            i += 1
            intStatus = Math.Round(((i + 1) / dtForecast.Rows.Count) * 100, 0)
            If intStatus = 100 Then
                intStatus = 99
            End If
            Session("Progress") = intStatus

        Next
        Session("Progress") = 100
        Session("Stage") = strErrorLog
    End Sub

    Private Sub CNH_Parts(intCustNum As Integer)
        Dim strPlant As String, strOurPartNum As String, _
             dteForecastDate As DateTime, dteCutOffDate As Date = Today.AddDays(21), _
             intOrderQty As Integer, strDateString As String = "", bolV8 As Boolean = True, _
             strErrorLog As String = "", i As Integer = 0, intStatus As Integer, strCustPartNum As String

        'Import Cross Ref
        Import_CrossRef("CNH")

        Session("Stage") = "Importing Forecasts"

        'START
        For Each row As DataRow In dtForecast.Rows

            If i = 0 Then
            Else

                'Get Values
                strCustPartNum = row(1)
                dteForecastDate = row(5)
                intOrderQty = row(3)

                'Check cutoff date
                If dteForecastDate >= dteCutOffDate Then

                    'Get our PN and Plant from crossref
                    For Each row2 As DataRow In dtCrossRef.Rows
                        If strCustPartNum = row2("F1") Then
                            strPlant = row2("F3")
                            strOurPartNum = row2("F2")
                            Exit For
                        Else
                        End If 'If customer part matches in cross ref
                    Next 'For each row in the cross ref table

                    'Check Plant
                    If strPlant = "" Then
                        If strErrorLog = "" Then
                            strErrorLog = "The following customer parts had errors:" & vbCrLf
                        End If
                        strErrorLog &= strCustPartNum & vbCrLf
                        'Check for OHIO
                    ElseIf strPlant = "GMI" Then
                    Else
                        'Create Forecast
                        Try
                            Create_Forecast(strPlant, strOurPartNum, dteForecastDate, intOrderQty, intCustNum)
                        Catch ex As Exception
                            If strErrorLog = "" Then
                                strErrorLog = "The following customer parts had errors:" & vbCrLf
                            End If
                            strErrorLog &= strOurPartNum & vbCrLf
                        End Try
                    End If 'Check plant
                End If 'Check against cutoff date
            End If 'Check if first row

            'VARIABLE CLEAN UP
            strPlant = ""
            i += 1
            intStatus = Math.Round(((i + 1) / dtForecast.Rows.Count) * 100, 0)
            If intStatus = 100 Then
                intStatus = 99
            End If
            Session("Progress") = intStatus

        Next
        Session("Progress") = 100
        Session("Stage") = strErrorLog


    End Sub

    Private Sub DEERE(intCustomerNum As Integer)
        Dim strPlant As String = "", strOurPartNum As String = "", strCustomerPartNum As String, strErrorLog As String = "", _
        dteForecastDate As DateTime, dteCutOffDate As Date = Today.AddDays(21), strForecastDate As String = "", _
        intOrderQty As Integer, i As Integer = 1, intCrossRefRowCount As Integer, strParsedDate As String = "", _
        intStatus As Integer = 0, dteCompareDate As Date = Today(), bolTrigger As Boolean = False, intQty1 As Integer, intQty2 As Integer, _
        dteThisRow As Date = Today(), dteNextRow As Date = Today()

        'Set Trigger Parts
        Dim strTriggerPart(8) As String
        strTriggerPart(0) = "100DJ0013-2"
        strTriggerPart(1) = "100DJ0014-2"
        strTriggerPart(2) = "100DJ0026-2"
        strTriggerPart(3) = "100EM0153-2"
        strTriggerPart(4) = "100EM0155-2"
        strTriggerPart(5) = "100EM0156-2"
        strTriggerPart(6) = "100ER0020-2"
        strTriggerPart(7) = "100EN0200-2"
        strTriggerPart(8) = "100GP0118-2"

        'Import Cross Ref
        Import_CrossRef("DEERE")

        'Update status
        Session("Stage") = "Importing Forecasts"


        'Get Dates in one column
        'For intRowCount = 3 To dtForecast.Rows.Count
        '    Try
        '        If IsDBNull(dtForecast.Rows(intRowCount)("F2")) Then
        '            dtForecast.Rows(intRowCount)("F2") = dtForecast.Rows(intRowCount)("F3")
        '        End If
        '    Catch ex As Exception
        '    End Try
        'Next
        
        'dtForecast.AcceptChanges()

        'Combine Rows
        For intRowCount = 3 To dtForecast.Rows.Count - 1

            'Get Dates
            Try
                If IsDBNull(dtForecast.Rows(intRowCount)("F3")) Then
                    dteThisRow = dtForecast.Rows(intRowCount)("F2")
                Else
                    dteThisRow = dtForecast.Rows(intRowCount)("F3")
                End If
            Catch ex As Exception
                dteThisRow = Today
            End Try
            Try
                If IsDBNull(dtForecast.Rows(intRowCount + 1)("F3")) Then
                    dteNextRow = dtForecast.Rows(intRowCount + 1)("F2")
                Else
                    dteNextRow = dtForecast.Rows(intRowCount + 1)("F3")
                End If
            Catch ex As Exception
                dteNextRow = Today
            End Try

            Try
                If dtForecast.Rows(intRowCount)("F7") = dtForecast.Rows(intRowCount + 1)("F7") And _
                    dteThisRow = dteNextRow Then
                    If IsDBNull(dtForecast.Rows(intRowCount + 1)("F8")) Then
                        intQty1 = 0
                    Else
                        intQty1 = CInt(dtForecast.Rows(intRowCount + 1)("F8"))
                    End If
                    If IsDBNull(dtForecast.Rows(intRowCount)("F8")) Then
                        intQty2 = 0
                    Else
                        intQty2 = CInt(dtForecast.Rows(intRowCount)("F8"))
                    End If
                    dtForecast.Rows(intRowCount + 1)("F8") = intQty1 + intQty2
                    dtForecast.Rows(intRowCount)("F8") = ""
                End If
            Catch ex As Exception
            End Try

        Next

            For Each row As DataRow In dtForecast.Rows
                bolTrigger = False
                If i < 3 Then 'skip first two rows
                ElseIf IsDBNull(row("F7")) = True Then 'skip if null part
                ElseIf row("F8").ToString = "" Then 'skip if null 
                Else
                    'Grab initial values
                    strCustomerPartNum = row("F7").ToString
                    intOrderQty = CInt(row("F8"))

                If IsDBNull(row("F3")) Then
                    dteForecastDate = row("F2")
                Else
                    dteForecastDate = row("F3")
                End If

                    dteForecastDate = FormatDateTime(dteForecastDate, DateFormat.ShortDate)

                    'Skip dates that are before the cutoff date or if it is not a forecast
                    For Each row2 As DataRow In dtCrossRef.Rows
                        If strCustomerPartNum = row2("F1") Then
                            strPlant = row2("F3")
                            strOurPartNum = row2("F2")
                            Exit For
                        Else
                        End If 'If customer part matches in cross ref
                    Next 'For each row in the cross ref table

                    For arr As Integer = 0 To 8
                        If strOurPartNum = strTriggerPart(arr) Then
                            bolTrigger = True
                            Exit For
                        End If
                    Next

                    If bolTrigger = True Then
                        dteCompareDate = Today.AddDays(5)
                    Else
                        dteCompareDate = dteCutOffDate
                    End If


                    If dteForecastDate < dteCompareDate Then 'do nothing
                        'Check for blank plant
                    ElseIf strPlant = "" Then
                        'strOurPartNum = InputBox("Please enter the RMT part for AGCO part #" & strCustomerPartNum)
                        'strPlant = InputBox("Please enter plant for " & strOurPartNum & vbCrLf & vbCrLf & _
                        '                        "Sheldon = MfgSys" & vbCrLf & "Spirit Lake = SPIRITLA" & vbCrLf & _
                        '                        "Ohio = GMI")
                        'dteForecastDate = dteForecastDate.AddDays(-7)
                        'dteForecastDate = FormatDateTime(dteForecastDate, DateFormat.ShortDate)
                        If strErrorLog = "" Then
                            strErrorLog = "The following customer parts had errors:" & vbCrLf
                        End If
                        strErrorLog &= strCustomerPartNum & vbCrLf
                        'Check for OHIO
                    ElseIf strPlant = "GMI" Then
                    Else
                        'Create Forecast
                        Try
                            Create_Forecast(strPlant, strOurPartNum, dteForecastDate, intOrderQty, intCustomerNum)
                        Catch ex As Exception
                            If strErrorLog = "" Then
                                strErrorLog = "The following customer parts had errors:" & vbCrLf
                            End If
                            strErrorLog &= strCustomerPartNum & vbCrLf
                        End Try
                    End If 'Plant Check


                End If 'If first row

                'VARIABLE CLEAN UP
                strPlant = ""
                i += 1
                intStatus = Math.Round(((i + 1) / dtForecast.Rows.Count) * 100, 0)
                If intStatus = 100 Then
                    intStatus = 99
                End If
                Session("Progress") = intStatus
            Next
            Session("Progress") = 100
            Session("Stage") = strErrorLog
    End Sub

    Private Sub Gehl_Madison(intCustNum As Integer)
        Dim strPlant As String = "", strOurPartNum As String = "", strCustomerPartNum As String, _
        dteForecastDate As DateTime, dteCutOffDate As Date = Today.AddDays(21), strForecastDate As String = "", _
        intOrderQty As Integer, i As Integer = 1, strParsedDate As String = "", strErrorLog As String = "", intStatus As Integer = 0

        'Import Cross Ref
        Import_CrossRef("Gehl")

        'Update status
        Session("Stage") = "Importing Forecasts"

        For Each row As DataRow In dtForecast.Rows
            If i = 1 Then 'skip first row
            ElseIf IsDBNull(row("F1")) = True Then 'skip if null part
            ElseIf row("F1").ToString = "" Then 'skip if null 
            Else
                'Grab initial values
                strCustomerPartNum = row("F1").ToString
                intOrderQty = CInt(row("F6"))
                dteForecastDate = row("F8")
                dteForecastDate = FormatDateTime(dteForecastDate, DateFormat.ShortDate)

                'Skip dates that are before the cutoff date or if it is not a forecast
                If dteForecastDate < dteCutOffDate Then 'do nothing
                Else
                    For Each row2 As DataRow In dtCrossRef.Rows
                        If strCustomerPartNum = row2("F1") Then
                            strPlant = row2("F3")
                            strOurPartNum = row2("F2")
                            Exit For
                        Else
                        End If 'If customer part matches in cross ref
                    Next 'For each row in the cross ref table

                    'Check for blank plant
                    If strPlant = "" Then
                        'strOurPartNum = InputBox("Please enter the RMT part for AGCO part #" & strCustomerPartNum)
                        'strPlant = InputBox("Please enter plant for " & strOurPartNum & vbCrLf & vbCrLf & _
                        '                        "Sheldon = MfgSys" & vbCrLf & "Spirit Lake = SPIRITLA" & vbCrLf & _
                        '                        "Ohio = GMI")
                        'dteForecastDate = dteForecastDate.AddDays(-7)
                        'dteForecastDate = FormatDateTime(dteForecastDate, DateFormat.ShortDate)
                        If strErrorLog = "" Then
                            strErrorLog = "The following customer parts had errors:" & vbCrLf
                        End If
                        strErrorLog &= strCustomerPartNum & vbCrLf
                        'Check for OHIO
                    ElseIf strPlant = "GMI" Then
                    Else
                        'Create Forecast
                        Try
                            Create_Forecast(strPlant, strOurPartNum, dteForecastDate, intOrderQty, intCustNum)
                        Catch ex As Exception
                            If strErrorLog = "" Then
                                strErrorLog = "The following customer parts had errors:" & vbCrLf
                            End If
                            strErrorLog &= strCustomerPartNum & vbCrLf
                        End Try
                    End If 'Plant Check


                End If 'Cutoff date
            End If 'If first row

            'VARIABLE CLEAN UP
            strPlant = ""
            i += 1
            intStatus = Math.Round(((i + 1) / dtForecast.Rows.Count) * 100, 0)
            If intStatus = 100 Then
                intStatus = 99
            End If
            Session("Progress") = intStatus
        Next
        Session("Progress") = 100
        Session("Stage") = strErrorLog
    End Sub

    Private Sub Gehl_Yankton(intCustNum As Integer)
        Dim strPlant As String = "", strOurPartNum As String = "", strCustomerPartNum As String, _
        dteForecastDate As DateTime, dteCutOffDate As Date = Today.AddDays(21), strForecastDate As String = "", _
        intOrderQty As Integer, i As Integer = 1, strParsedDate As String = "", strErrorLog As String = "", intStatus As Integer = 0

        'Import Cross Ref
        Import_CrossRef("Gehl")

        'Update status
        Session("Stage") = "Importing Forecasts"

        For Each row As DataRow In dtForecast.Rows
            If i = 1 Then 'skip first row
            ElseIf IsDBNull(row("F1")) = True Then 'skip if null part
            ElseIf row("F1").ToString = "" Then 'skip if null 
            Else
                'Grab initial values
                strCustomerPartNum = row("F1").ToString
                intOrderQty = CInt(row("F6"))
                dteForecastDate = row("F7")
                dteForecastDate = FormatDateTime(dteForecastDate, DateFormat.ShortDate)

                'Skip dates that are before the cutoff date or if it is not a forecast
                If dteForecastDate < dteCutOffDate Then 'do nothing
                Else
                    For Each row2 As DataRow In dtCrossRef.Rows
                        If strCustomerPartNum = row2("F1") Then
                            strPlant = row2("F3")
                            strOurPartNum = row2("F2")
                            Exit For
                        Else
                        End If 'If customer part matches in cross ref
                    Next 'For each row in the cross ref table

                    'Check for blank plant
                    If strPlant = "" Then
                        'strOurPartNum = InputBox("Please enter the RMT part for AGCO part #" & strCustomerPartNum)
                        'strPlant = InputBox("Please enter plant for " & strOurPartNum & vbCrLf & vbCrLf & _
                        '                        "Sheldon = MfgSys" & vbCrLf & "Spirit Lake = SPIRITLA" & vbCrLf & _
                        '                        "Ohio = GMI")
                        'dteForecastDate = dteForecastDate.AddDays(-7)
                        'dteForecastDate = FormatDateTime(dteForecastDate, DateFormat.ShortDate)
                        If strErrorLog = "" Then
                            strErrorLog = "The following customer parts had errors:" & vbCrLf
                        End If
                        strErrorLog &= strCustomerPartNum & vbCrLf
                        'Check for OHIO
                    ElseIf strPlant = "GMI" Then
                    Else
                        'Create Forecast
                        Try
                            Create_Forecast(strPlant, strOurPartNum, dteForecastDate, intOrderQty, intCustNum)
                        Catch ex As Exception
                            If strErrorLog = "" Then
                                strErrorLog = "The following customer parts had errors:" & vbCrLf
                            End If
                            strErrorLog &= strCustomerPartNum & vbCrLf
                        End Try
                    End If 'Plant Check


                End If 'Cutoff date
            End If 'If first row

            'VARIABLE CLEAN UP
            strPlant = ""
            i += 1
            intStatus = Math.Round(((i + 1) / dtForecast.Rows.Count) * 100, 0)
            If intStatus = 100 Then
                intStatus = 99
            End If
            Session("Progress") = intStatus
        Next
        Session("Progress") = 100
        Session("Stage") = strErrorLog
    End Sub

    Private Sub Hagie(intCustNum As Integer)
        Dim strPlant As String = "", strOurPartNum As String = "", strCustomerPartNum As String, _
        dteForecastDate As DateTime, dteCutOffDate As Date = Today.AddDays(21), strForecastDate As String = "", _
        intOrderQty As Integer, i As Integer = 1, strParsedDate As String = "", strErrorLog As String = "", intStatus As Integer = 0

        'Import Cross Ref
        Import_CrossRef("Hagie")

        'Update status
        Session("Stage") = "Importing Forecasts"

        For Each row As DataRow In dtForecast.Rows
            If i = 1 Then 'skip first row
            ElseIf IsDBNull(row("F3")) = True Then 'skip if null part
            ElseIf row("F3").ToString = "" Then 'skip if null 
            Else
                'Grab initial values
                strCustomerPartNum = row("F3").ToString
                intOrderQty = CInt(row("F11"))
                dteForecastDate = row("F10")
                dteForecastDate = FormatDateTime(dteForecastDate, DateFormat.ShortDate)

                'Skip dates that are before the cutoff date or if it is not a forecast
                If dteForecastDate < dteCutOffDate Then 'do nothing
                Else
                    For Each row2 As DataRow In dtCrossRef.Rows
                        If strCustomerPartNum = row2("F1") Then
                            strPlant = row2("F3")
                            strOurPartNum = row2("F2")
                            Exit For
                        Else
                        End If 'If customer part matches in cross ref
                    Next 'For each row in the cross ref table

                    'Check for blank plant
                    If strPlant = "" Then
                        'strOurPartNum = InputBox("Please enter the RMT part for AGCO part #" & strCustomerPartNum)
                        'strPlant = InputBox("Please enter plant for " & strOurPartNum & vbCrLf & vbCrLf & _
                        '                        "Sheldon = MfgSys" & vbCrLf & "Spirit Lake = SPIRITLA" & vbCrLf & _
                        '                        "Ohio = GMI")
                        'dteForecastDate = dteForecastDate.AddDays(-7)
                        'dteForecastDate = FormatDateTime(dteForecastDate, DateFormat.ShortDate)
                        If strErrorLog = "" Then
                            strErrorLog = "The following customer parts had errors:" & vbCrLf
                        End If
                        strErrorLog &= strCustomerPartNum & vbCrLf
                        'Check for OHIO
                    ElseIf strPlant = "GMI" Then
                    Else
                        'Create Forecast
                        Try
                            Create_Forecast(strPlant, strOurPartNum, dteForecastDate, intOrderQty, intCustNum)
                        Catch ex As Exception
                            If strErrorLog = "" Then
                                strErrorLog = "The following customer parts had errors:" & vbCrLf
                            End If
                            strErrorLog &= strCustomerPartNum & vbCrLf
                        End Try
                    End If 'Plant Check


                End If 'Cutoff date
            End If 'If first row

            'VARIABLE CLEAN UP
            strPlant = ""
            i += 1
            intStatus = Math.Round(((i + 1) / dtForecast.Rows.Count) * 100, 0)
            If intStatus = 100 Then
                intStatus = 99
            End If
            Session("Progress") = intStatus
        Next
        Session("Progress") = 100
        Session("Stage") = strErrorLog
    End Sub

    Private Sub Heil(intCustNum As Integer)
        Dim dblStartRow As Double = 0, strCustomerPN As String = "", strOurPartNum As String = "", dblQOH As Double = 0, dblPFEP As Double = 0
        Dim dblBlankRow As Double = 0, strPlant As String = "", intCol As Integer = 1, dblPFEPOrig As Double, _
            dblForecastQty As Double = 0, dblPFEP_RT As Double = 0, dteForecastDate As Date, _
            intCustomerID As Integer = 375, dblMonthly As Double = 0, strErrorLog As String = "", intStatus As Integer = 0, dblQOHRem As Double = 0

        'Import Cross Ref
        Import_CrossRef("Heil")

        'Update status
        Session("Progress") = 0
        Session("Stage") = "Importing Forecasts"

        'FIND PN ROW
        Do
            If IsDBNull(dtForecast.Rows(dblStartRow)("F30")) Then
            ElseIf dtForecast.Rows(dblStartRow)("F30") = "PN" Then
                Exit Do
            End If

            dblStartRow += 1
        Loop

        'Add two rows
        dblStartRow += 2

        'START
        For i As Integer = dblStartRow To dtForecast.Rows.Count - 1
            If IsDBNull(dtForecast.Rows(i)("F30")) Then
            ElseIf dtForecast.Rows(i)("F30") = "" Then
            ElseIf dtForecast.Rows(i)("F30") = "PN" Then
            Else 'This row has a real part number on it
                dblBlankRow = 0
                intCol = 1
                dblPFEP_RT = 0

                'GET CUST PN, QOH
                strCustomerPN = dtForecast.Rows(i)("F30")
                Try
                    dblQOH = CDbl(dtForecast.Rows(i)("F20"))
                Catch ex As Exception
                    dblQOH = 0
                End Try

                Try
                    dblPFEP = CDbl(dtForecast.Rows(i)("F23"))
                Catch ex As Exception
                    dblPFEP = 0
                End Try
                Try
                    dblMonthly = CDbl(dtForecast.Rows(i)("F16"))
                Catch ex As Exception
                    dblMonthly = 0
                End Try
                dblPFEPOrig = dblPFEP
                If dblQOH >= dblPFEP Then
                    dblPFEP = 0
                    dblQOHRem = dblQOH - dblPFEPOrig
                Else
                    dblPFEP = dblPFEP - dblQOH
                    dblQOHRem = 0
                End If


                'CROSS REF
                For Each row2 As DataRow In dtCrossRef.Rows
                    If strCustomerPN = row2("F1") Then
                        strPlant = row2("F3")
                        strOurPartNum = row2("F2")
                        Exit For
                    Else
                    End If 'If customer part matches in cross ref
                Next 'For each row in the cross ref table

                'CHECK PLANT
                If strPlant = "" Then
                    If strErrorLog = "" Then
                        strErrorLog = "The following customer parts had errors:" & vbCrLf
                    End If
                    strErrorLog &= strCustomerPN & vbCrLf
                    'Check for OHIO
                ElseIf strPlant = "GMI" Then
                Else
                    'PUT IN TODAY'S INFO
                    Try
                        dblForecastQty = CDbl(dtForecast.Rows(i)("F" & intCol + 31))
                    Catch ex As Exception
                        dblForecastQty = 0
                    End Try

                    intCol += 1
                    Try
                        dblForecastQty += CDbl(dtForecast.Rows(i)("F" & intCol + 31))
                    Catch ex As Exception

                    End Try

                    If dblForecastQty > 0 And dblForecastQty > dblQOHRem Then
                        dblForecastQty = dblForecastQty - dblQOHRem
                        dblQOHRem = 0
                        If dblPFEP_RT >= dblPFEP Or dblPFEP = 0 Then
                        Else
                            dblForecastQty += Math.Round(dblPFEP / 4)
                            dblPFEP_RT += Math.Round(dblPFEP / 4)
                        End If 'PFEP CHECK

                        'Create Forecast
                        Try
                            Create_Forecast(strPlant, strOurPartNum, Today, dblForecastQty, intCustNum)
                        Catch ex As Exception
                            If strErrorLog = "" Then
                                strErrorLog = "The following customer parts had errors:" & vbCrLf
                            End If
                            strErrorLog &= strCustomerPN & vbCrLf
                        End Try
                    Else
                        dblQOHRem = dblQOHRem - dblForecastQty
                    End If 'FORECAST > 0
                    intCol += 1

                    'GO THROUGH EACH WEEKLY DATE COL
                    dteForecastDate = Today
                    If Weekday(dteForecastDate) = 2 Then
                        dteForecastDate = dteForecastDate.AddDays(7)
                    Else
                        dteForecastDate = Find_Weekday(dteForecastDate, 2)
                    End If
                    For intCol = 3 To 9
                        Try
                            dblForecastQty = CDbl(dtForecast.Rows(i)("F" & intCol + 31))
                        Catch ex As Exception
                            dblForecastQty = 0
                        End Try

                        If dblForecastQty > 0 And dblForecastQty > dblQOHRem Then
                            dblForecastQty = dblForecastQty - dblQOHRem
                            dblQOHRem = 0
                            If dblPFEP_RT >= dblPFEP Or dblPFEP = 0 Then

                            Else
                                dblForecastQty += Math.Round(dblPFEP / 4)
                                dblPFEP_RT += Math.Round(dblPFEP / 4)
                            End If 'PFEP CHECK

                            'Create Forecast
                            Try
                                Create_Forecast(strPlant, strOurPartNum, dteForecastDate, dblForecastQty, intCustNum)
                            Catch ex As Exception
                                If strErrorLog = "" Then
                                    strErrorLog = "The following customer parts had errors:" & vbCrLf
                                End If
                                strErrorLog &= strCustomerPN & vbCrLf
                            End Try
                        Else
                            dblQOHRem = dblQOHRem - dblForecastQty
                        End If 'FORECAST > 0

                        dteForecastDate = dteForecastDate.AddDays(7)
                    Next

                    'GO UNTIL 1 YR
                    dblForecastQty = 0
                    Do Until dteForecastDate > Today.AddDays(365)
                        dblForecastQty += dblMonthly

                        If dblForecastQty < 1 Then
                        Else
                            'Create Forecast
                            Try
                                Create_Forecast(strPlant, strOurPartNum, dteForecastDate, Math.Round(dblForecastQty, 0), intCustNum)
                                If Math.Round(dblForecastQty, 0) >= dblForecastQty Then
                                    dblForecastQty = 0
                                Else
                                    dblForecastQty = dblForecastQty - Math.Round(dblForecastQty, 0)
                                End If
                            Catch ex As Exception
                                If strErrorLog = "" Then
                                    strErrorLog = "The following customer parts had errors:" & vbCrLf
                                End If
                                strErrorLog &= strCustomerPN & vbCrLf
                            End Try
                        End If

                        dteForecastDate = dteForecastDate.AddDays(7)
                    Loop


                End If 'CHECK PLANT
            End If 'CHECK PART

            'VARIABLE CLEAN UP
            strPlant = ""
            intStatus = Math.Round(((i + 1) / (dtForecast.Rows.Count - dblStartRow)) * 100, 0)
            If intStatus = 100 Then
                intStatus = 99
            End If
            Session("Progress") = intStatus
        Next

        'END
        Session("Progress") = 100
        Session("Stage") = strErrorLog
    End Sub

    Private Sub LiftTek(intCustNum As Integer)
        Dim strPlant As String = "", strOurPartNum As String = "", strXPartNum As String, _
             dteForecastDate As DateTime, dteCutOffDate As Date = Today.AddDays(21), strErrorLog As String = "", _
             intOrderQty As Integer, intMonthlyQty As Integer = 0, bolCrossRef As Boolean = False, i As Integer = 0, _
             bolIsNumeric As Boolean = False, dteEndDate As Date = Today.AddDays(365), intOnHandQty As Integer = 0

        'Import Cross Ref
        Import_CrossRef(ddlCustomer.Text)
        Session("Stage") = "Importing Forecasts"
        Session("Progress") = 0

        'Start Generating Forecast
        For Each dgvrow As DataRow In dtForecast.Rows
            If i < 8 Then
            ElseIf IsNumeric(dgvrow(8)) = False Then
            Else
                'Get Customer PartNumber
                strXPartNum = dgvrow(2)
                intMonthlyQty = dgvrow(8)
                intOnHandQty = dgvrow(7)

                'Check monthly qty
                If intMonthlyQty < 1 Then
                Else
                    'Find Reference in Cross Reference File
                    For Each row As DataRow In dtCrossRef.Rows
                        If strXPartNum = row(0) Then
                            Try
                                strPlant = row(2)
                                strOurPartNum = row(1)
                                bolCrossRef = True
                                Exit For
                            Catch ex As Exception
                            End Try

                        End If
                    Next 'Row in Cross Ref

                    'Set Starting Forecast Date
                    dteForecastDate = Find_Weekday(Today.AddDays(30), 2)

                    'Cylce through until the end date
                    Do Until dteForecastDate > dteEndDate
                        If intOnHandQty > intMonthlyQty Then
                            intOnHandQty -= intMonthlyQty
                            intOrderQty = 0
                        Else
                            intOrderQty = intMonthlyQty - intOnHandQty
                            intOnHandQty = 0
                        End If

                        If intOrderQty = 0 Then
                        ElseIf bolCrossRef = False Then
                            If strErrorLog = "" Then
                                strErrorLog = "The following customer parts had errors:" & vbCrLf
                            End If
                            strErrorLog &= strXPartNum & vbCrLf
                        Else
                            Create_Forecast(strPlant, strOurPartNum, dteForecastDate, intOrderQty, intCustNum)
                        End If

                        'Add days
                        dteForecastDate = Find_Weekday(dteForecastDate.AddDays(30), 2)
                    Loop

                End If 'Check monthly qty
            End If 'Check for first 8 rows
            'Cleanup Variables
            bolCrossRef = False
            i += 1
            'UPDATE STATUS
            Session("Progress") = Math.Round(((i + 1) / dtForecast.Rows.Count) * 100, 0)
            strPlant = ""
        Next 'Row in Datagridview

        Session("Progress") = 100
        Session("Stage") = strErrorLog

    End Sub

    Private Sub OshKosh_Mexico(ByVal intCustNum As Integer)
        Dim strPlant As String = "", strOurPartNum As String, strCustomerPartNum As String, _
             dteForecastDate As DateTime, dteCutOffDate As Date = Today.AddDays(21), _
             intOrderQty As Integer, dteColumnDate As Date, intRowCount As Integer, i As Integer = 0, intMax As Integer, _
             intStatus As Integer = 0, strErrorLog As String = ""

        'Import Cross Ref
        Import_CrossRef(ddlCustomer.Text)
        'Update Status
        Session("Stage") = "Importing Forecasts"
        Session("Progress") = 0

        'Start Generating Forecast
        intMax = dtForecast.Rows.Count * 50
        'Go through all the rows for each column
        For colDates As Integer = 9 To 43
            intRowCount = 1
            For Each row As DataRow In dtForecast.Rows
                If intRowCount < 4 Then
                ElseIf intRowCount = 4 Then
                    dteColumnDate = row("F" & colDates)
                Else
                    If IsDBNull(row("F1")) = True Then
                    Else
                        strCustomerPartNum = row("F1")
                        dteForecastDate = dteColumnDate
                        intOrderQty = row(colDates - 1)

                        'Skip dates that are before the cutoff date or if it is not a forecast
                        If dteForecastDate < dteCutOffDate Or intOrderQty = 0 Then
                        Else
                            For Each row2 As DataRow In dtCrossRef.Rows
                                If strCustomerPartNum = row2("F1") Then
                                    strPlant = row2("F3")
                                    strOurPartNum = row2("F2")
                                    dteForecastDate = FormatDateTime(dteForecastDate, DateFormat.ShortDate)
                                    Exit For
                                Else
                                End If 'If customer part matches in cross ref

                            Next 'For each row in the cross ref table

                            'Check for blank plant
                            If strPlant = "" Then
                                'strOurPartNum = InputBox("Please enter the RMT part for AGCO part #" & strCustomerPartNum)
                                'strPlant = InputBox("Please enter plant for " & strOurPartNum & vbCrLf & vbCrLf & _
                                '                        "Sheldon = MfgSys" & vbCrLf & "Spirit Lake = SPIRITLA" & vbCrLf & _
                                '                        "Ohio = GMI")
                                'dteForecastDate = dteForecastDate.AddDays(-7)
                                'dteForecastDate = FormatDateTime(dteForecastDate, DateFormat.ShortDate)
                                If strErrorLog = "" Then
                                    strErrorLog = "The following customer parts had errors:" & vbCrLf
                                End If
                                strErrorLog &= strCustomerPartNum & vbCrLf
                                'Check for OHIO
                            ElseIf strPlant = "GMI" Then
                            Else
                                Try
                                    'Create Forecast
                                    Create_Forecast(strPlant, strOurPartNum, dteForecastDate, intOrderQty, intCustNum)
                                Catch ex As Exception
                                    If strErrorLog = "" Then
                                        strErrorLog = "The following customer parts had errors:" & vbCrLf
                                    End If
                                    strErrorLog &= strCustomerPartNum & vbCrLf
                                End Try

                            End If 'Plant Check

                        End If 'If the forecast date is before the cutoff or no qty
                    End If 'If there is no part number in the forecast row
                End If 'If 1st row
                strPlant = ""
                intRowCount += 1
                i += 1
                intStatus = Math.Round((i / intMax) * 100, 0)
                If intStatus = 100 Then
                    intStatus = 99
                End If
                Session("Progress") = intStatus
            Next 'For each row in the forecast
        Next 'for each column in the dates portion


        Session("Progress") = 100
        Session("Stage") = strErrorLog
    End Sub

    Private Sub OTC(ByVal intCustNum As Integer)
        Dim strPlant As String = "", strOurPartNum As String, strCustomerPartNum As String, _
             dteForecastDate As DateTime, dteCutOffDate As Date = Today.AddDays(21), _
             intOrderQty As Integer, dteColumnDate As Date, intRowCount As Integer, i As Integer = 0, intMax As Integer, _
             intStatus As Integer = 0, strErrorLog As String = ""

        'Import Cross Ref
        Import_CrossRef(ddlCustomer.Text)
        'Update Status
        Session("Stage") = "Importing Forecasts"
        Session("Progress") = 0

        'Start Generating Forecast
        intMax = dtForecast.Rows.Count * 50
        'Go through all the rows for each column
        For colDates As Integer = 7 To 58
            intRowCount = 1
            For Each row As DataRow In dtForecast.Rows
                If intRowCount = 1 Then
                    dteColumnDate = row("F" & colDates + 1)
                Else
                    If IsDBNull(row("F1")) = True Then
                    Else
                        strCustomerPartNum = row("F1")
                        dteForecastDate = dteColumnDate
                        intOrderQty = row(colDates)

                        If intCustNum = 337 And intOrderQty < 0 Then
                            intOrderQty = intOrderQty * -1
                        End If

                        'Skip dates that are before the cutoff date or if it is not a forecast
                        If dteForecastDate < dteCutOffDate Or intOrderQty = 0 Then
                        Else
                            For Each row2 As DataRow In dtCrossRef.Rows
                                If strCustomerPartNum = row2("F1") Then
                                    strPlant = row2("F3")
                                    strOurPartNum = row2("F2")
                                    dteForecastDate = FormatDateTime(dteForecastDate, DateFormat.ShortDate)
                                    Exit For
                                Else
                                End If 'If customer part matches in cross ref

                            Next 'For each row in the cross ref table

                            'Check for blank plant
                            If strPlant = "" Then
                                'strOurPartNum = InputBox("Please enter the RMT part for AGCO part #" & strCustomerPartNum)
                                'strPlant = InputBox("Please enter plant for " & strOurPartNum & vbCrLf & vbCrLf & _
                                '                        "Sheldon = MfgSys" & vbCrLf & "Spirit Lake = SPIRITLA" & vbCrLf & _
                                '                        "Ohio = GMI")
                                'dteForecastDate = dteForecastDate.AddDays(-7)
                                'dteForecastDate = FormatDateTime(dteForecastDate, DateFormat.ShortDate)
                                If strErrorLog = "" Then
                                    strErrorLog = "The following customer parts had errors:" & vbCrLf
                                End If
                                strErrorLog &= strCustomerPartNum & vbCrLf
                                'Check for OHIO
                            ElseIf strPlant = "GMI" Then
                            Else
                                Try
                                    'Create Forecast
                                    Create_Forecast(strPlant, strOurPartNum, dteForecastDate, intOrderQty, intCustNum)
                                Catch ex As Exception
                                    If strErrorLog = "" Then
                                        strErrorLog = "The following customer parts had errors:" & vbCrLf
                                    End If
                                    strErrorLog &= strCustomerPartNum & vbCrLf
                                End Try
                                
                            End If 'Plant Check

                        End If 'If the forecast date is before the cutoff or no qty
                    End If 'If there is no part number in the forecast row
                End If 'If 1st row
                strPlant = ""
                intRowCount += 1
                i += 1
                intStatus = Math.Round((i / intMax) * 100, 0)
                If intStatus = 100 Then
                    intStatus = 99
                End If
                Session("Progress") = intStatus
            Next 'For each row in the forecast
        Next 'for each column in the dates portion


        Session("Progress") = 100
        Session("Stage") = strErrorLog
    End Sub

    Private Sub Scranton(intCustNum As Integer)
        Dim strPlant As String = "", strOurPartNum As String, strCustomerPartNum As String, _
             intCustomerID As Integer = 773, dteForecastDate As DateTime, dteCutOffDate As Date = Today.AddDays(21), _
             intOrderQty As Integer, intRowCount As Integer, strDateString As String = "", bolV8 As Boolean = True, _
             strErrorLog As String = "", i As Integer, intStatus As Integer

        'Import Cross Ref
        Import_CrossRef(ddlCustomer.Text)

        Session("Stage") = "Importing Forecasts"

        For intColDates As Integer = 6 To 17
            intRowCount = 5

            'Determine Column Date
            dteForecastDate = Get_Col_Date(dtForecast.Rows(2)(intColDates - 1), dtForecast.Rows(3)(intColDates - 1))

            'Check if the forecast date is earlier than the cutoff date
            If dteForecastDate < dteCutOffDate Then
            Else

                For Each row As DataRow In dtForecast.Rows
                    If IsDBNull(row("F1")) = True Then
                    ElseIf IsDBNull(row("F" & intColDates)) Then
                    ElseIf row("F" & intColDates) = "" Then
                    ElseIf row("F" & intColDates) = "0" Then
                        'ElseIf Convert.ToInt32(row("F" & intColDates)) = 0 Then
                    Else
                        'Get Info
                        strCustomerPartNum = row("F2")
                        Try
                            intOrderQty = row("F" & intColDates)
                        Catch ex As Exception
                            intOrderQty = 0
                        End Try

                        For Each row2 As DataRow In dtCrossRef.Rows
                            If strCustomerPartNum = row2("F1") Then
                                strPlant = row2("F3")
                                strOurPartNum = row2("F2")
                                Exit For
                            Else
                            End If 'If customer part matches in cross ref
                        Next 'For each row in the cross ref table

                        'Check for blank plant
                        If strPlant = "" Then
                            If strErrorLog = "" Then
                                strErrorLog = "The following customer parts had errors:" & vbCrLf
                            End If
                            strErrorLog &= strCustomerPartNum & vbCrLf
                            'Check for OHIO
                        ElseIf strPlant = "GMI" Then
                        Else
                            'Create Forecast
                            Try
                                Create_Forecast(strPlant, strOurPartNum, dteForecastDate, intOrderQty, intCustNum)
                            Catch ex As Exception
                                If strErrorLog = "" Then
                                    strErrorLog = "The following customer parts had errors:" & vbCrLf
                                End If
                                strErrorLog &= strCustomerPartNum & vbCrLf
                            End Try
                        End If 'Plant Check

                    End If 'intRowCount = 1

                    strPlant = ""
                    intRowCount += 1
                    i += 1

                Next 'Row
            End If 'Check if this column is too early

            'UPDATE STATUS
            intStatus += 8
            If intStatus = 100 Then
                intStatus = 99
            End If
            Session("Progress") = intStatus

        Next 'Col


        'FINISH
        Session("Stage") = strErrorLog
        Session("Progress") = 100


    End Sub

    Private Function Get_Col_Date(strMonth As String, strYear As String) As Date
        Dim intMonth As Integer, dteForecastDate As Date
        strMonth = Trim(strMonth)
        Select Case strMonth
            Case "JAN"
                intMonth = 1
            Case "FEB"
                intMonth = 2
            Case "MAR"
                intMonth = 3
            Case "APR"
                intMonth = 4
            Case "MAY"
                intMonth = 5
            Case "JUN"
                intMonth = 6
            Case "JUL"
                intMonth = 7
            Case "AUG"
                intMonth = 8
            Case "SEP"
                intMonth = 9
            Case "OCT"
                intMonth = 10
            Case "NOV"
                intMonth = 11
            Case "DEC"
                intMonth = 12
        End Select

        dteForecastDate = DateValue(intMonth & "/1/" & strYear)

        Return dteForecastDate

    End Function

    Private Sub Vermeer(intCustNum As Integer)
        Dim strPlant As String = "", strOurPartNum As String = "", strCustomerPartNum As String, strType As String = "", _
        dteForecastDate As DateTime, dteCutOffDate As Date = Today.AddDays(21), strForecastDate As String = "", _
        intOrderQty As Integer, i As Integer = 1, strParsedDate As String = "", strErrorLog As String = "", intStatus As Integer = 0

        'Import Cross Ref
        Import_CrossRef("Vermeer")

        'Update status
        Session("Stage") = "Importing Forecasts"

        For Each row As DataRow In dtForecast.Rows
            If i = 1 Then 'skip first row
            ElseIf IsDBNull(row("F1")) = True Then 'skip if null part
            ElseIf row("F1").ToString = "" Then 'skip if null 
            Else
                'Grab initial values
                strCustomerPartNum = row("F1").ToString
                intOrderQty = CInt(row("F6"))
                strType = row("F7")
                dteForecastDate = row("F5")
                dteForecastDate = FormatDateTime(dteForecastDate, DateFormat.ShortDate)
                dteForecastDate = dteForecastDate.AddDays(-7)

                'Skip dates that are before the cutoff date or if it is not a forecast
                If dteForecastDate < dteCutOffDate Then 'do nothing

                Else
                    For Each row2 As DataRow In dtCrossRef.Rows
                        If strCustomerPartNum = row2("F1") Then
                            strPlant = row2("F3")
                            strOurPartNum = row2("F2")
                            Exit For
                        Else
                        End If 'If customer part matches in cross ref
                    Next 'For each row in the cross ref table

                    'Check for blank plant
                    If strPlant = "" Then
                        'strOurPartNum = InputBox("Please enter the RMT part for AGCO part #" & strCustomerPartNum)
                        'strPlant = InputBox("Please enter plant for " & strOurPartNum & vbCrLf & vbCrLf & _
                        '                        "Sheldon = MfgSys" & vbCrLf & "Spirit Lake = SPIRITLA" & vbCrLf & _
                        '                        "Ohio = GMI")
                        'dteForecastDate = dteForecastDate.AddDays(-7)
                        'dteForecastDate = FormatDateTime(dteForecastDate, DateFormat.ShortDate)
                        If strErrorLog = "" Then
                            strErrorLog = "The following customer parts had errors:" & vbCrLf
                        End If
                        strErrorLog &= strCustomerPartNum & vbCrLf
                        'Check for OHIO
                    ElseIf strPlant = "GMI" Then
                    ElseIf strType = "Removed" Then
                    Else
                        'Create Forecast
                        Try
                            Create_Forecast(strPlant, strOurPartNum, dteForecastDate, intOrderQty, intCustNum)
                        Catch ex As Exception
                            If strErrorLog = "" Then
                                strErrorLog = "The following customer parts had errors:" & vbCrLf
                            End If
                            strErrorLog &= strCustomerPartNum & vbCrLf
                        End Try
                    End If 'Plant Check


                End If 'Cutoff date
            End If 'If first row

            'VARIABLE CLEAN UP
            strPlant = ""
            i += 1
            intStatus = Math.Round(((i + 1) / dtForecast.Rows.Count) * 100, 0)
            If intStatus = 100 Then
                intStatus = 99
            End If
            Session("Progress") = intStatus
        Next
        Session("Progress") = 100
        Session("Stage") = strErrorLog
    End Sub

    Private Sub Volvo(intCustNum As Integer)
        Dim strPlant As String = "", strOurPartNum As String = "", strXPartNum As String, _
             dteForecastDate As DateTime, dteCutOffDate As Date = Today.AddDays(21), strErrorLog As String = "", _
             intOrderQty As Integer, strOrderType As String, bolCrossRef As Boolean = False, i As Integer = 0

        'Import Cross Ref
        Import_CrossRef(ddlCustomer.Text)
        Session("Stage") = "Importing Forecasts"
        Session("Progress") = 0

        'Start Generating Forecast
        For Each dgvrow As DataRow In dtForecast.Rows
            If i = 0 Then
            Else
                'Get Customer PartNumber and Date
                strXPartNum = dgvrow(3)
                'Get Forecast Dates
                dteForecastDate = dgvrow(11)
                dteForecastDate = FormatDateTime(dteForecastDate, DateFormat.ShortDate)
                'Get Order Qty and Type
                intOrderQty = dgvrow(12)
                strOrderType = dgvrow(14)

                'Determine if this is a good row or not
                If intOrderQty = 0 Or strOrderType = "LD" Or dteForecastDate < dteCutOffDate Then
                Else 'Contine on.
                    'Find Reference in Cross Reference File
                    For Each row As DataRow In dtCrossRef.Rows
                        If strXPartNum = row(0) Then
                            strPlant = row(2)
                            strOurPartNum = row(1)
                            bolCrossRef = True
                            Exit For
                        End If
                    Next 'Row in Cross Ref

                    'Check for match in cross ref
                    If bolCrossRef = True Then
                        'Create Forecast
                        Create_Forecast(strPlant, strOurPartNum, dteForecastDate, intOrderQty, intCustNum)
                    Else
                        'strOurPartNum = InputBox("Please enter the Rosenboom part number for '" & strXPartNum & "'")
                        'strPlant = InputBox("Please enter the plant for '" & strOurPartNum & "'")


                    End If

                    



                End If 'If this is a good record
            End If 'Check for first row
            'Cleanup Variables
            bolCrossRef = False
            i += 1
            'UPDATE STATUS


            Session("Progress") = Math.Round(((i + 1) / dtForecast.Rows.Count) * 100, 0)
            strPlant = ""
        Next 'Row in Datagridview

        Session("Progress") = 100
        Session("Stage") = strErrorLog

    End Sub

    Private Sub WackerNeuson(intCustNum As Integer)
        Dim strPlant As String = "", strOurPartNum As String = "", strCustomerPartNum As String, _
        dteForecastDate As DateTime, dteCutOffDate As Date = Today.AddDays(21), strForecastDate As String = "", _
        intOrderQty As Integer, i As Integer = 1, strParsedDate As String = "", strErrorLog As String = "", intStatus As Integer = 0

        'Import Cross Ref
        Import_CrossRef("Wacker Neuson")

        'Update status
        Session("Stage") = "Importing Forecasts"

        For Each row As DataRow In dtForecast.Rows
            If i = 1 Then 'skip first row
            ElseIf IsDBNull(row("F1")) = True Then 'skip if null part
            ElseIf row("F1").ToString = "" Then 'skip if null 
            ElseIf row("F7").ToString <> "Forecast" Then 'skip firm
            Else
                'Grab initial values
                strCustomerPartNum = row("F4").ToString
                intOrderQty = CInt(row("F10"))
                dteForecastDate = row("F13")
                dteForecastDate = FormatDateTime(dteForecastDate, DateFormat.ShortDate)

                'Skip dates that are before the cutoff date or if it is not a forecast
                If dteForecastDate < dteCutOffDate Then 'do nothing
                Else
                    For Each row2 As DataRow In dtCrossRef.Rows
                        If strCustomerPartNum = row2("F1") Then
                            strPlant = row2("F3")
                            strOurPartNum = row2("F2")
                            Exit For
                        Else
                        End If 'If customer part matches in cross ref
                    Next 'For each row in the cross ref table

                    'Check for blank plant
                    If strPlant = "" Then
                        'strOurPartNum = InputBox("Please enter the RMT part for AGCO part #" & strCustomerPartNum)
                        'strPlant = InputBox("Please enter plant for " & strOurPartNum & vbCrLf & vbCrLf & _
                        '                        "Sheldon = MfgSys" & vbCrLf & "Spirit Lake = SPIRITLA" & vbCrLf & _
                        '                        "Ohio = GMI")
                        'dteForecastDate = dteForecastDate.AddDays(-7)
                        'dteForecastDate = FormatDateTime(dteForecastDate, DateFormat.ShortDate)
                        If strErrorLog = "" Then
                            strErrorLog = "The following customer parts had errors:" & vbCrLf
                        End If
                        strErrorLog &= strCustomerPartNum & vbCrLf
                        'Check for OHIO
                    ElseIf strPlant = "GMI" Then
                    Else
                        'Create Forecast
                        Try
                            Create_Forecast(strPlant, strOurPartNum, dteForecastDate, intOrderQty, intCustNum)
                        Catch ex As Exception
                            If strErrorLog = "" Then
                                strErrorLog = "The following customer parts had errors:" & vbCrLf
                            End If
                            strErrorLog &= strCustomerPartNum & vbCrLf
                        End Try
                    End If 'Plant Check


                End If 'Cutoff date
            End If 'If first row

            'VARIABLE CLEAN UP
            strPlant = ""
            i += 1
            intStatus = Math.Round(((i + 1) / dtForecast.Rows.Count) * 100, 0)
            If intStatus = 100 Then
                intStatus = 99
            End If
            Session("Progress") = intStatus
        Next
        Session("Progress") = 100
        Session("Stage") = strErrorLog
    End Sub

    Private Sub Weiler(intCustNum As Integer)
        Dim strPlant As String = "", strOurPartNum As String = "", strCustomerPartNum As String = "", _
        dteForecastDate As DateTime, dteCutOffDate As Date = Today.AddDays(21), strForecastDate As String = "", _
        intOrderQty As Integer, i As Integer = 1, strParsedDate As String = "", strErrorLog As String = "", intStatus As Integer = 0, _
        intMinOrderQty As Integer = 0, intOH As Integer = 0, strLastCustPN As String = "", intOnOrder As Integer = 0

        'Import Cross Ref
        Import_CrossRef("Weiler")

        'Update status
        Session("Stage") = "Importing Forecasts"

        'Get on order qty
        Dim sUser As String = "sc"
        Dim sPass As String = "DEMETER@!"
        Dim sServer As String = "zeus"
        Dim sPort As String = "9408"
        Dim sAppServer As String = String.Format("AppServerDC://{0}:{1}", sServer, sPort)
        Dim sCompany As String = "RMT"
        Dim mysession As Object = New Epicor.Mfg.Core.Session(sUser, sPass, sAppServer, Epicor.Mfg.Core.Session.LicenseType.Default)
        Dim connPool As New Epicor.Mfg.Core.BLConnectionPool(sUser, sPass, sAppServer)

        Dim dq As New Epicor.Mfg.BO.DynamicQuery(connPool)
        Dim qds As New Epicor.Mfg.BO.QueryDesignDataSet
        Dim dsDataSet As DataSet
        qds = dq.GetByID("RMT-WeilerFirmOrders")

        dsDataSet = dq.Execute(qds)

        connPool.Dispose()


        For Each row As DataRow In dtForecast.Rows
            If i = 1 Then 'skip first row
            ElseIf IsDBNull(row("F2")) = True Then 'skip if null part
            ElseIf row("F2").ToString = "" Then 'skip if null 
            Else
                'Grab initial values
                strCustomerPartNum = row("F2").ToString
                intOrderQty = CInt(row("F7"))
                dteForecastDate = row("F5")
                dteForecastDate = FormatDateTime(dteForecastDate, DateFormat.ShortDate)

                For Each row2 As DataRow In dtCrossRef.Rows
                    If strCustomerPartNum = row2("F1") Then
                        strPlant = row2("F3")
                        strOurPartNum = row2("F2")
                        intMinOrderQty = CInt(row2("F4"))
                        Exit For
                    Else
                    End If 'If customer part matches in cross ref
                Next 'For each row in the cross ref table

                intOnOrder = 0

                'Get ON ORDER
                For Each row2 As DataRow In dsDataSet.Tables(0).Rows
                    If row2(0) = strOurPartNum Then
                        intOnOrder = row2(1)
                        Exit For
                    Else
                    End If
                Next

                'Verify OH qty
                If strCustomerPartNum = strLastCustPN Then
                Else
                    intOH = CInt(row("F6")) + intOnOrder
                End If

                If intOH >= intOrderQty Then
                    intOH -= intOrderQty
                    intOrderQty = 0
                Else
                    'Reduce OH qty
                    intOH -= intOrderQty

                    'Set correct order qty based on minimums
                    If intOrderQty > intMinOrderQty Then
                    Else
                        intOrderQty = intMinOrderQty
                    End If

                    'Add in new OH qty
                    intOH += intOrderQty
                End If


                'Skip dates that are before the cutoff date or if it is not a forecast
                If dteForecastDate < dteCutOffDate Then 'do nothing
                Else


                    'Check for blank plant
                    If strPlant = "" Then
                        'strOurPartNum = InputBox("Please enter the RMT part for AGCO part #" & strCustomerPartNum)
                        'strPlant = InputBox("Please enter plant for " & strOurPartNum & vbCrLf & vbCrLf & _
                        '                        "Sheldon = MfgSys" & vbCrLf & "Spirit Lake = SPIRITLA" & vbCrLf & _
                        '                        "Ohio = GMI")
                        'dteForecastDate = dteForecastDate.AddDays(-7)
                        'dteForecastDate = FormatDateTime(dteForecastDate, DateFormat.ShortDate)
                        If strErrorLog = "" Then
                            strErrorLog = "The following customer parts had errors:" & vbCrLf
                        End If
                        strErrorLog &= strCustomerPartNum & vbCrLf
                        'Check for OHIO
                    ElseIf strPlant = "GMI" Then
                    ElseIf intOrderQty = 0 Then
                    Else


                        'Create Forecast
                        Try
                            Create_Forecast(strPlant, strOurPartNum, dteForecastDate, intOrderQty, intCustNum)
                        Catch ex As Exception
                            If strErrorLog = "" Then
                                strErrorLog = "The following customer parts had errors:" & vbCrLf
                            End If
                            strErrorLog &= strCustomerPartNum & vbCrLf
                        End Try
                    End If 'Plant Check


                End If 'Cutoff date
                End If 'If first row

                'VARIABLE CLEAN UP
                strLastCustPN = strCustomerPartNum
                strPlant = ""
                i += 1
                intStatus = Math.Round(((i + 1) / dtForecast.Rows.Count) * 100, 0)
                If intStatus = 100 Then
                    intStatus = 99
                End If
                Session("Progress") = intStatus
        Next
        Session("Progress") = 100
        Session("Stage") = strErrorLog
    End Sub

    Private Sub Xtreme(intCustNum As Integer)
        Dim strPlant As String, strOurPartNum As String, strCustomerPartNum As String, _
             intCustomerID As Integer = 26, dteForecastDate As DateTime, dteCutOffDate As Date = Today.AddDays(21), _
             intOrderQty As Integer, intRowCount As Integer, strDateString As String = "", bolV8 As Boolean = True, _
             strErrorLog As String = "", i As Integer, intStatus As Integer, intColCheck As Integer = 3

        'Import Cross Ref
        Import_CrossRef(ddlCustomer.Text)

        Session("Stage") = "Importing Forecasts"

        'Determine end of date columns
        Try
            Do Until IsDBNull(dtForecast.Rows(0)("F" & intColCheck))
                intColCheck += 1
            Loop
        Catch ex As Exception
        End Try

        Dim intMax As Integer = dtForecast.Rows.Count * (intColCheck - 2)

        For intColDates As Integer = 3 To intColCheck - 1
            intRowCount = 1

            For Each row As DataRow In dtForecast.Rows
                If intRowCount = 1 Then
                    dteForecastDate = row("F" & intColDates)
                ElseIf IsDBNull(row("F1")) = True Then
                ElseIf IsDBNull(row("F" & intColDates)) Then
                    'ElseIf row("F" & intColDates) = "0" Then
                    'ElseIf Convert.ToInt32(row("F" & intColDates)) = 0 Then
                ElseIf dteForecastDate < dteCutOffDate Then
                Else
                    'Get Info
                    strCustomerPartNum = row("F2") '
                    Try
                        intOrderQty = row("F" & intColDates)
                    Catch ex As Exception
                        intOrderQty = DmyToExceSerial(row("F" & intColDates))
                    End Try

                    For Each row2 As DataRow In dtCrossRef.Rows
                        If strCustomerPartNum = row2("F1") Then
                            strPlant = row2("F3")
                            strOurPartNum = row2("F2")

                            dteForecastDate = FormatDateTime(dteForecastDate, DateFormat.ShortDate)
                            'Check to make sure that it is not a GMI part
                            If strPlant = "GMI" Then
                                'Check for V6
                                If bolV8 = False Then
                                    'Insert Line

                                Else
                                End If
                            Else
                                'Check for V8
                                If bolV8 = True Then
                                    'Insert Line

                                Else
                                End If
                            End If 'Check for Ohio Part
                            Exit For
                        Else
                        End If 'If customer part matches in cross ref

                    Next 'For each row in the cross ref table
                    'Check for blank plant
                    If strPlant = "" Then
                        'strOurPartNum = InputBox("Please enter the RMT part for AGCO part #" & strCustomerPartNum)
                        'strPlant = InputBox("Please enter plant for " & strOurPartNum & vbCrLf & vbCrLf & _
                        '                        "Sheldon = MfgSys" & vbCrLf & "Spirit Lake = SPIRITLA" & vbCrLf & _
                        '                        "Ohio = GMI")
                        'dteForecastDate = dteForecastDate.AddDays(-7)
                        'dteForecastDate = FormatDateTime(dteForecastDate, DateFormat.ShortDate)
                        If strErrorLog = "" Then
                            strErrorLog = "The following customer parts had errors:" & vbCrLf
                        End If
                        strErrorLog &= strCustomerPartNum & vbCrLf
                        'Check for OHIO
                    ElseIf strPlant = "GMI" Then
                    Else
                        'Create Forecast
                        Try
                            Create_Forecast(strPlant, strOurPartNum, dteForecastDate, intOrderQty, intCustNum)
                        Catch ex As Exception
                            If strErrorLog = "" Then
                                strErrorLog = "The following customer parts had errors:" & vbCrLf
                            End If
                            strErrorLog &= strCustomerPartNum & vbCrLf
                        End Try
                    End If 'Plant Check

                End If 'intRowCount = 1

                strPlant = ""
                intRowCount += 1
                i += 1
                'UPDATE STATUS
                intStatus = Math.Round((i / intMax) * 100, 0)
                If intStatus = 100 Then
                    intStatus = 99
                End If
                Session("Progress") = intStatus

            Next 'Row
        Next 'Col
        Session("Stage") = strErrorLog
        Session("Progress") = 100


    End Sub
    Public Shared Function DmyToExceSerial(ByVal InputTimeStamp As DateTime) As Double

        Dim nDay As Integer = InputTimeStamp.Day
        Dim nMonth As Integer = InputTimeStamp.Month
        Dim nYear As Integer = InputTimeStamp.Year
        Dim nHour As Integer = InputTimeStamp.Hour
        Dim nMinute As Integer = InputTimeStamp.Minute
        Dim nSecond As Integer = InputTimeStamp.Second


        ' Excel/Lotus 123 have a bug with 29-02-1900. 1900 is not a
        ' leap year, but Excel/Lotus 123 think it is...

        If nDay = 29 AndAlso nMonth = 2 AndAlso nYear = 1900 Then
            Return 60
        End If

        ' DMY to Modified Julian calculatie with an extra substraction of 2415019.

        Dim nSerialDate As Long = Int((1461 * (nYear + 4800 + Int((nMonth - 14) \ 12))) \ 4) + Int((367 * _
                    (nMonth - 2 - 12 * ((nMonth - 14) \ 12))) \ 12) - Int((3 * (Int((nYear + 4900 + _
                    Int((nMonth - 14) \ 12)) \ 100))) \ 4) + nDay - 2415019 - 32075

        If nSerialDate < 60 Then
            ' Because of the 29-02-1900 bug, any serial date 

            ' under 60 is one off... Compensate.

            nSerialDate -= 1
        End If

        Return CInt(nSerialDate) + (nHour + nMinute / 60 + nSecond / 3600) / 24

    End Function
    Private Sub Manual_Format(intCustNum As Integer)
        Dim strPlant As String, strOurPartNum As String, _
             dteForecastDate As DateTime, dteCutOffDate As Date = Today.AddDays(21), _
             intOrderQty As Integer, strDateString As String = "", bolV8 As Boolean = True, _
             strErrorLog As String = "", i As Integer = 0, intStatus As Integer

        'Import Cross Ref
        Import_CrossRef(ddlCustomer.Text)

        Session("Stage") = "Importing Forecasts"

        'Plant,Part,Date,OrderQty,CustID,ConsumedQty

        'START
        For Each row As DataRow In dtForecast.Rows
            'Get Values
            strPlant = row(0)
            strOurPartNum = row(1)
            dteForecastDate = row(2)
            intOrderQty = row(3)

            'Create Forecast
            Try
                Create_Forecast(strPlant, strOurPartNum, dteForecastDate, intOrderQty, intCustNum)
            Catch ex As Exception
                If strErrorLog = "" Then
                    strErrorLog = "The following customer parts had errors:" & vbCrLf
                End If
                strErrorLog &= strOurPartNum & vbCrLf
            End Try

            'VARIABLE CLEAN UP
            strPlant = ""
            i += 1
            intStatus = Math.Round(((i + 1) / dtForecast.Rows.Count) * 100, 0)
            If intStatus = 100 Then
                intStatus = 99
            End If
            Session("Progress") = intStatus

        Next
        Session("Progress") = 100
        Session("Stage") = strErrorLog


    End Sub


    Private Sub Create_Forecast(strPlant As String, strPartNum As String, dteForecastDate As Date, intOrderQty As Integer, intCustNum As Integer)
        Dim dsForecast As New ForecastDataSet
        'Get New
        dsForecast = BO_Forecast.Get_New(strPartNum, strPlant, intCustNum, dteForecastDate)
        'Set fields
        dsForecast.Tables(0).Rows(0)("ForeDate") = dteForecastDate
        dsForecast.Tables(0).Rows(0)("ForeQty") = intOrderQty
        'dsForecast.Tables(0).Rows(0)("Checkbox01") = True
        dsForecast.Tables(0).Rows(0)("RowMod") = "A"
        'Update
        Try
            dsForecast = BO_Forecast.Update(dsForecast)
        Catch ex As Exception
        End Try
        'Clear the temp ds
        dsForecast.Clear()
    End Sub

    Private Function Find_Weekday(dteDate As Date, intWeekday As Integer) As Date
        Do
            If Weekday(dteDate) = intWeekday Then
                Return dteDate
            Else
                dteDate = dteDate.AddDays(1)
            End If
        Loop
    End Function
End Class

Public Class Excel_97_03_File_Reader
    Public Shared Function Import_Old_Excel_Data(ByVal strQuery As String, ByVal strTable As String, _
    ByVal strDatabaseFilePath As String) As DataTable

        'Try
        Dim cn As OleDbConnection, da As OleDbDataAdapter

        'Set cn
        cn = New OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0; Data Source=" & strDatabaseFilePath & _
                                 "; Extended Properties=""Excel 8.0;IMEX=1;HDR=NO;TypeGuessRows=0;ImportMixedTypes=Text""")

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
        'Catch ex As Exception
        'MsgBox(ex.Message)

        'End Try
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
Public Class Update_Vantage
    Public Shared Sub Update_Database(ByVal Query As String)
        Dim cn As OdbcConnection, cmd As OdbcCommand

        'Set cn
        cn = New OdbcConnection("DSN=Epicor9RC;UID=SYSPROGRESS;PWD=report;host=ZEUS;port=9450;db=mfgsys")

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
    Public Shared Sub Update_Test_Database(ByVal Query As String)
        Dim cn As OdbcConnection, cmd As OdbcCommand

        'Set cn
        cn = New OdbcConnection("DSN=mfgsys803400TRAIN;UID=SYSPROGRESS;PWD=report;host=zeus22;port=8360;db=mfgsys")

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
    Public Shared Sub Update_Stds_Database(ByVal Query As String)
        Dim cn As OdbcConnection, cmd As OdbcCommand

        'Set cn
        cn = New OdbcConnection("DSN=mfgsys803400TRAIN2;UID=SYSPROGRESS;PWD=report;host=zeus22;port=9350;db=mfgsys")

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
Public Class BO_Forecast
    Public Shared Function Get_New(strPartNum As String, strPlant As String, intCustNum As Integer, dteForecastDate As Date) As ForecastDataSet
        Dim sUser As String = "sc"
        Dim sPass As String = "DEMETER@!"
        Dim sServer As String = "zeus"
        Dim sPort As String = "9408"
        Dim sAppServer As String = String.Format("AppServerDC://{0}:{1}", sServer, sPort)
        Dim sCompany As String = "RMT"
        Dim session As Object = New Epicor.Mfg.Core.Session(sUser, sPass, sAppServer, Epicor.Mfg.Core.Session.LicenseType.Default)
        Dim connPool As New Epicor.Mfg.Core.BLConnectionPool(sUser, sPass, sAppServer)

        Dim myForecast As New Forecast(connPool)
        Dim myForecastDS As New ForecastDataSet

        myForecast.GetNewForecast(myForecastDS, strPartNum, strPlant, intCustNum, dteForecastDate)

        connPool.Dispose()

        Return myForecastDS
    End Function
    Public Shared Function Get_By_ID(strPartNum As String, strPlant As String, intCustNum As Integer, _
                                     dteForecastDate As Date) As ForecastDataSet
        Dim sUser As String = "sc"
        Dim sPass As String = "DEMETER@!"
        Dim sServer As String = "zeus"
        Dim sPort As String = "9408"
        Dim sAppServer As String = String.Format("AppServerDC://{0}:{1}", sServer, sPort)
        Dim sCompany As String = "RMT"
        Dim session As Object = New Epicor.Mfg.Core.Session(sUser, sPass, sAppServer, Epicor.Mfg.Core.Session.LicenseType.Default)
        Dim connPool As New Epicor.Mfg.Core.BLConnectionPool(sUser, sPass, sAppServer)

        Dim myForecast As New Forecast(connPool)
        Dim myForecastDS As New ForecastDataSet

        myForecast.GetByID(strPartNum, strPlant, intCustNum, dteForecastDate, "")

        connPool.Dispose()

        Return myForecastDS
    End Function
    Public Shared Function Get_Rows(strFilter As String) As ForecastDataSet
        Dim sUser As String = "sc"
        Dim sPass As String = "DEMETER@!"
        Dim sServer As String = "zeus"
        Dim sPort As String = "9408"
        Dim sAppServer As String = String.Format("AppServerDC://{0}:{1}", sServer, sPort)
        Dim sCompany As String = "RMT"
        Dim session As Object = New Epicor.Mfg.Core.Session(sUser, sPass, sAppServer, Epicor.Mfg.Core.Session.LicenseType.Default)
        Dim connPool As New Epicor.Mfg.Core.BLConnectionPool(sUser, sPass, sAppServer)

        Dim myForecast As New Forecast(connPool)
        Dim myForecastDS As New ForecastDataSet

        myForecastDS = myForecast.GetRows(strFilter, "", 0, 0, False)

        connPool.Dispose()

        Return myForecastDS
    End Function
    Public Shared Sub Delete_By_ID(strPartNum As String, strPlant As String, intCustNum As Integer, dteForecastDate As Date)
        Dim sUser As String = "sc"
        Dim sPass As String = "DEMETER@!"
        Dim sServer As String = "zeus"
        Dim sPort As String = "9408"
        Dim sAppServer As String = String.Format("AppServerDC://{0}:{1}", sServer, sPort)
        Dim sCompany As String = "RMT"
        Dim session As Object = New Epicor.Mfg.Core.Session(sUser, sPass, sAppServer, Epicor.Mfg.Core.Session.LicenseType.Default)
        Dim connPool As New Epicor.Mfg.Core.BLConnectionPool(sUser, sPass, sAppServer)

        Dim myForecast As New Forecast(connPool)

        myForecast.DeleteByID(strPartNum, strPlant, intCustNum, dteForecastDate, "")

        connPool.Dispose()

    End Sub
    Public Shared Function Update(dsForecast As ForecastDataSet) As ForecastDataSet
        Dim sUser As String = "sc"
        Dim sPass As String = "DEMETER@!"
        Dim sServer As String = "zeus"
        Dim sPort As String = "9408"
        Dim sAppServer As String = String.Format("AppServerDC://{0}:{1}", sServer, sPort)
        Dim sCompany As String = "RMT"
        Dim session As Object = New Epicor.Mfg.Core.Session(sUser, sPass, sAppServer, Epicor.Mfg.Core.Session.LicenseType.Default)
        Dim connPool As New Epicor.Mfg.Core.BLConnectionPool(sUser, sPass, sAppServer)

        Dim myForecast As New Forecast(connPool)

        myForecast.Update(dsForecast)

        connPool.Dispose()

        Return dsForecast
    End Function
End Class