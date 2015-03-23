Imports System.Data.OleDb
Imports System.Data

Public Class Excel_97_03_File_Reader
    Public Shared Function Import_Old_Excel_Data(ByVal strQuery As String, ByVal strTable As String, _
    ByVal strDatabaseFilePath As String) As DataTable

        Try
            Dim cn As OleDbConnection, da As OleDbDataAdapter

            'Set cn
            cn = New OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0; Mode=Read; Data Source=" & strDatabaseFilePath & _
                                     "; Extended Properties=Excel 8.0")

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
