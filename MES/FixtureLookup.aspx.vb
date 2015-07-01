Imports Epicor.Mfg.Core
Imports Epicor.Mfg.Shared
Imports Epicor.Mfg.UI
Imports Epicor.Mfg.BO
Imports System.Configuration
Imports System.Xml
Imports System.IO
Imports System.Text
Imports System.Data
Partial Class MES_FLIP
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        'Set the auto tab
        'Me.txtEmpID.Attributes("onkeyup") = "autotab(" + Me.txtEmpID.ClientID + ", " + Me.txtJobNum.ClientID + ")"
        'Highlight MES Tab
        Dim MESTab As System.Web.UI.HtmlControls.HtmlGenericControl
        MESTab = Master.FindControl("mestab")
        MESTab.Attributes.Add("class", "active")

    End Sub



    Protected Sub btnSearch_Click(sender As Object, e As EventArgs) Handles btnSearch.Click

    End Sub


    Protected Sub gvwJobOpers_RowDataBound(sender As Object, e As GridViewRowEventArgs)
        e.Row.Attributes("onmouseover") = "onMouseOver('" & (e.Row.RowIndex + 1) & "')"
        'e.Row.Attributes("onmouseout") = "onMouseOut('" & (e.Row.RowIndex + 1) & "')"

    End Sub
End Class
Public Class BO_UD01
    Public Shared Function Get_New() As UD01DataSet
        Dim sUser As String = "sc"
        Dim sPass As String = "DEMETER@!"
        Dim sServer As String = "zeus"
        Dim sPort As String = "9408"
        Dim sAppServer As String = String.Format("AppServerDC://{0}:{1}", sServer, sPort)
        Dim sCompany As String = "RMT"
        Dim session As Object = New Epicor.Mfg.Core.Session(sUser, sPass, sAppServer, Epicor.Mfg.Core.Session.LicenseType.Default)
        Dim connPool As New Epicor.Mfg.Core.BLConnectionPool(sUser, sPass, sAppServer)

        Dim myUD01 As New UD01(connPool)
        Dim myUD01DS As New UD01DataSet

        myUD01.GetaNewUD01(myUD01DS)

        connPool.Dispose()

        Return myUD01DS
    End Function
    Public Shared Function Get_Rows(strUD01Filter As String) As UD01DataSet
        Dim sUser As String = "sc"
        Dim sPass As String = "DEMETER@!"
        Dim sServer As String = "zeus"
        Dim sPort As String = "9408"
        Dim sAppServer As String = String.Format("AppServerDC://{0}:{1}", sServer, sPort)
        Dim sCompany As String = "RMT"
        Dim session As Object = New Epicor.Mfg.Core.Session(sUser, sPass, sAppServer, Epicor.Mfg.Core.Session.LicenseType.Default)
        Dim connPool As New Epicor.Mfg.Core.BLConnectionPool(sUser, sPass, sAppServer)

        Dim myUD01 As New UD01(connPool)
        Dim myUD01DS As New UD01DataSet

        myUD01.GetRows(strUD01Filter, "", 0, 0, True)

        connPool.Dispose()

        Return myUD01DS
    End Function
    Public Shared Function Update(dsUD01 As UD01DataSet) As UD01DataSet
        Dim sUser As String = "sc"
        Dim sPass As String = "DEMETER@!"
        Dim sServer As String = "zeus"
        Dim sPort As String = "9408"
        Dim sAppServer As String = String.Format("AppServerDC://{0}:{1}", sServer, sPort)
        Dim sCompany As String = "RMT"
        Dim session As Object = New Epicor.Mfg.Core.Session(sUser, sPass, sAppServer, Epicor.Mfg.Core.Session.LicenseType.Default)
        Dim connPool As New Epicor.Mfg.Core.BLConnectionPool(sUser, sPass, sAppServer)

        Dim myUD01 As New UD01(connPool)

        myUD01.Update(dsUD01)

        connPool.Dispose()

        Return dsUD01
    End Function
End Class
Public Class BO_EmpBasic_FLIP
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

        Dim strNewID As String
        strNewID = Left(strEmpID, 3)
        strNewID = LCase(strNewID)
        strNewID &= Right(strEmpID, 2)

        myEmpBasicDS = myEmpBasic.GetByID(strNewID)

        connPool.Dispose()

        Return myEmpBasicDS
    End Function
End Class
