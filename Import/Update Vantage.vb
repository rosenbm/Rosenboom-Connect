Imports System.Data.Odbc
Imports System.Data

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
