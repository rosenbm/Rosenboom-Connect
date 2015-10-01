Imports Ice.Core.Session
Imports Ice.Lib.Framework
Imports Ice.Proxy.BO
Imports System.Configuration
Imports System.Xml
Imports System.IO
Imports System.Text

Public Class BO_UD02
    Public Shared Function Get_New() As Ice.BO.UD02DataSet
        Dim sUser As String = "sc"
        Dim sPass As String = "DEMETER@!"
        Dim session As Object = New Ice.Core.Session(sUser, sPass, LicenseType.Default, "\\olympus\ERP10\ERP10.0.700\ClientDeployment\Client\Config\RMT-SHIA-APP03.sysconfig")
        Dim iLaunch As New Ice.Lib.Framework.ILauncher(session)
        Dim sessionMod As Ice.Proxy.BO.UD02Impl = WCFServiceSupport.CreateImpl(Of UD02Impl)(session, UD02Impl.UriPath)

        Dim myUD02DS As New Ice.BO.UD02DataSet
        sessionMod.GetaNewUD02(myUD02DS)

        sessionMod.Close()

        Return myUD02DS
    End Function
End Class
