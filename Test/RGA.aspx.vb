Imports System.Data.OleDb
Imports System.Data

Partial Class Test_RGA
    Inherits System.Web.UI.Page



    Public Shared Function Load_Info(ByVal strDatabaseFilePath As String, _
    ByVal strQuery As String, ByVal strTable As String) As DataTable

        On Error Resume Next
        Dim cn As OleDbConnection, da As OleDbDataAdapter


        'Set cn
        cn = New OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Mode=Read;Data Source=" & strDatabaseFilePath)

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

        Return dt
    End Function

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        GridView1.DataSource = Load_Info("\\pithos\company\Databases\Warranty Database.mdb", "SELECT [RGA TABLE].[DATE EVALUATED], [RGA TABLE].RGA, [RGA TABLE].SEQ, [RGA TABLE].[CUST RETURN NUM], [RGA TABLE].[P/N], [RGA TABLE].[QTY RECEIVED], [RGA TABLE].[DATE MANUFACTURED], [RGA TABLE].[TYPE OF FAILURE], [RGA TABLE].WARRANTY, [RGA TABLE].[EXPECTED FAILURE], [RGA TABLE].[NOT RETURNED], [RGA TABLE].[OUT OF WARRANTY], [RGA TABLE].[DATE PARTS  RECEIVED], [RGA TABLE].[DATE RESPONSE SENT] FROM [RGA TABLE] WHERE ((([RGA TABLE].[DATE EVALUATED])>#1/1/2012#) AND (([RGA TABLE].[TYPE OF FAILURE])<3) AND (([RGA TABLE].WARRANTY)>=1) AND (([RGA TABLE].CUSTOMER) Like 'HEIL*') AND (([RGA TABLE].STATUS)>=1)) ORDER BY [RGA TABLE].[DATE EVALUATED];", "Test")
    End Sub
End Class
