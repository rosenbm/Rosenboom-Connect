Imports System.IO
Imports System.Net

Partial Class Test_Save_Page
    Inherits System.Web.UI.Page


    Dim mywebReq As WebRequest
    Dim mywebResp As WebResponse
    Dim sr As StreamReader
    Dim strHTML As String
    Dim sw As StreamWriter
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Give the Appropriate URL for .aspx page. in this case its http://www.c-sharpcorner.com/faq.aspx
        'sr = New StreamReader(Response.OutputStream)
        'strHTML = sr.ReadToEnd
        'sw = File.CreateText("C:\temp.html")
        'sw.WriteLine(strHTML)
        'sw.Close()
        'Response.WriteFile("C:\temp.html")
    End Sub

    Protected Sub Button1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button1.Click
        Dim NewFile As New IO.StreamWriter("C:\temp.doc")
        NewFile.WriteLine("<html>")
        NewFile.WriteLine("<head>")
        NewFile.WriteLine("<META HTTP-EQUIV=Content-Type CONTENT=text/html charset=UTF-8>")
        NewFile.WriteLine("<meta name=ProgId content=Word.Document>")
        NewFile.WriteLine("<meta name=Generator content=Microsoft Word 9>")
        NewFile.WriteLine("<meta name=Originator content=Microsoft Word 9>")
        NewFile.WriteLine("</head>")
        NewFile.WriteLine("<body>")
        NewFile.WriteLine("TESTING 123456789")
        NewFile.WriteLine("</body>")
        NewFile.WriteLine("</html>")
        NewFile.Close()
    End Sub
End Class

