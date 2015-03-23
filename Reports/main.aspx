<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" Title="Rosenboom Connect - Reports" %>

<script runat="server">
    Private Sub BindGrid()
        Dim DataDirectory As String
        Select Case Request.QueryString("folder")
            Case "earnedhours"
                DataDirectory = "\\pithos\Company\E9 Reports\Connect\Earned Hours\"
                lblTitle.Text = "Earned Hours Reports"
            Case "bom"
                DataDirectory = "\\pithos\Company\E9 Reports\BOM Reports\"
                lblTitle.Text = "BOM Reports"
                
            Case Else
                DataDirectory = "\\pithos\Company\E9 Reports\Connect\"
                lblTitle.Text = "Reports"
        End Select
        
        Dim files() As IO.FileInfo = New IO.DirectoryInfo(DataDirectory).GetFiles
        GridView1.DataSource = files
        GridView1.DataBind()

    End Sub

 
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            BindGrid()
        End If
        
        Dim ReportsTab As System.Web.UI.HtmlControls.HtmlGenericControl
        ReportsTab = Master.FindControl("reportstab")
        ReportsTab.Attributes.Add("class", "active")
       
    End Sub

    Private Sub Downloadfile(ByVal fileName As String, ByVal FullFilePath As String)
        Response.AddHeader("Content-Disposition", "attachment; filename=" & fileName)
        Response.TransmitFile(FullFilePath)
        Response.End()
    End Sub
    Private Sub ViewReport(ByVal fileName As String, ByVal FullFilePath As String)
        Select Case fileName.Substring(fileName.LastIndexOf("."))
            Case ".rpt"
                Response.Redirect("viewreport.aspx?folder=" & Request.QueryString("folder") & "&report=" & fileName)
            Case Else
                Downloadfile(fileName, FullFilePath)
        End Select
    End Sub
    Protected Sub GridView1_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles GridView1.RowCommand
        If e.CommandName = "Download" Then
            Dim fileInfo() As String = e.CommandArgument.ToString().Split(";")
            Dim FileName As String = fileInfo(1)
            Dim FullPath As String = fileInfo(0)
            Downloadfile(FileName, FullPath)
        ElseIf e.CommandName = "View" Then
            Dim fileInfo() As String = e.CommandArgument.ToString().Split(";")
            Dim FileName As String = fileInfo(1)
            Dim FullPath As String = fileInfo(0)
            ViewReport(FileName, FullPath)
        End If
    End Sub
</script>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div class="wrapper col3 reportsmain">
<div id="intro">
    <asp:Label ID="lblTitle" runat="server" Text="" Font-Size="Larger"></asp:Label><br /><br />
<asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" CellPadding="2"
                ForeColor="#333333" GridLines="None" AllowPaging="False">
                <Columns>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:LinkButton ID="lnkdownload" runat="server" Text="Download" CommandName="Download"
                                CommandArgument='<%#Eval("FullName") +";" + Eval("Name") %>'></asp:LinkButton>
                                <asp:LinkButton ID="lnkview" runat="server" Text="View" CommandName="View"
                                CommandArgument='<%#Eval("FullName") +";" + Eval("Name") %>'></asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="Name" HeaderText="File Name" />
                    <asp:BoundField DataField="Length" HeaderText="Size (Bytes)" />
                </Columns>
                <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                <EditRowStyle BackColor="#999999" />
                <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
            </asp:GridView>

                       <ul class="reportslist">
            <li><a href="/Reports/main.aspx?folder=earnedhours"><div></div><div>Earned Hours Reports</div></a></li> 
            <li><a href="/Reports/viewreport.aspx?folder=mtlandpur&report=SEOI.rpt"><div></div><div>Short End Index</div></a></li>
            <li><a href="/Reports/viewreport.aspx?folder=production&report=Updated Throughput Units v1.rpt"><div></div><div>Supervisor Report</div></a></li>
            <li><a href="/Reports/viewreport.aspx?folder=whse&report=binloc.rpt"><div></div><div>Bin Location by Part #</div></a></li>
            <li><a href="/Reports/viewreport.aspx?folder=production&report=Priority Dispatch - Mtls Available.rpt"><div></div><div>Priority Dispatch - Mtls Available</div></a></li>
            </ul>


            </div>
            </div>
</asp:Content>

