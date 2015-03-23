<%@ Page Language="VB" %>

<%@ Register Assembly="CrystalDecisions.Web, Version=10.5.3700.0, Culture=neutral, PublicKeyToken=692fbea5521e1304"
    Namespace="CrystalDecisions.Web" TagPrefix="CR" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<script runat="server">

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs)

    End Sub

    Protected Sub btnViewReport_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim strPath As String, strReport As String
        
        'Get path
        Select Case Request.QueryString("folder")
            Case "bom"
                strPath = "\\pithos\company\E9 Reports\BOM Reports\"
            Case "mtlandpur"
                strPath = "\\pithos\company\E9 Reports\Materials and Purchasing\"
            Case "production"
                strPath = "\\pithos\company\E9 Reports\Production\"
            Case "whse"
                strPath = "\\pithos\company\E9 Reports\Warehouse\"
            Case Else
                strPath = ""
        End Select
        'Get report name
        strReport = strPath & Request.QueryString("report")
        crvmain.ReportSource = strReport
        crvmain.RefreshReport()
    End Sub
</script>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Rosenboom Connect - View Report</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:Button ID="btnViewReport" runat="server" Text="Refresh Report" 
            onclick="btnViewReport_Click" /><br />
        <CR:CrystalReportViewer ID="crvmain" runat="server" AutoDataBind="true" />
    </div>
    </form>
</body>
</html>
