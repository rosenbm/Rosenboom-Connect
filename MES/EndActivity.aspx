<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" Title="Rosenboom Connect - End Labor Activity" %>

<script runat="server">

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs)
        'Highlight MES Tab
        Dim MESTab As System.Web.UI.HtmlControls.HtmlGenericControl
        MESTab = Master.FindControl("mestab")
        MESTab.Attributes.Add("class", "active")
        
        'Check for EmpID
        If Request.QueryString("empid") = "" Then
        Else
            txtEmpID.Text = Request.QueryString("empid")
        End If
    End Sub

    Protected Sub btnGetActivities_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Try
            GridView1.DataBind()
        Catch ex As Exception
        End Try
    End Sub

    Protected Sub GridView1_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        If GridView1.SelectedRow.Cells(7).Text = "S" Then
            chkComplete.Visible = True
            lblLaborQty.Visible = False
            txtLaborQty.Visible = False
        ElseIf GridView1.SelectedRow.Cells(7).Text = "P" Then
            lblLaborQty.Visible = True
            txtLaborQty.Visible = True
            chkComplete.Visible = True
        End If
    End Sub

    Protected Sub btnEndActivity_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim strComplete As String = ""
        
        'Set strComplete string
        If chkComplete.Checked = True Then
            strComplete = "true"
        Else
            strComplete = "false"
        End If
        
        Write_CSV(strComplete)
        
        lblMessage.Text = "Activity ended."
    End Sub
    
    Protected Sub Write_CSV(ByVal strComplete As String)
        
        'Set File Path
        Dim rand As New Random, letter As String = "", n As Integer, strFilePath As String = "\\pithos\company\service connect\" & _
            "Rosenboom Connect\End Activity\"
        'Get random file name
        For n = 0 To 8
            letter &= ChrW(rand.Next(Asc("A"), Asc("Z") + 1))
        Next
        strFilePath &= letter & ".csv"

        'Start writing the file
        Dim CSVFile As New IO.StreamWriter(strFilePath)
        CSVFile.WriteLine("LaborHedSeq,LaborDtlSeq,LaborQty,LaborType,Complete")
        CSVFile.WriteLine(GridView1.SelectedRow.Cells(1).Text & "," & GridView1.SelectedRow.Cells(2).Text & "," & txtLaborQty.Text & "," & _
                GridView1.SelectedRow.Cells(7).Text & "," & strComplete)
        CSVFile.Close()
    End Sub

    Protected Sub GridView1_DataBinding(ByVal sender As Object, ByVal e As System.EventArgs)
        Try
            
        Catch ex As Exception

        End Try
    End Sub
</script>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<div class="wrapper col3">
    <div id="intro">
    
    <h2>End Labor Activity</h2>
        <asp:Label ID="lblMessage" runat="server" Text=""></asp:Label><br /><br />
        EmpID: &nbsp;<asp:TextBox ID="txtEmpID" runat="server" MaxLength="5" Width="50"></asp:TextBox>
    <br />
    
        <asp:Button ID="btnGetActivities" runat="server" Text="Get Activities" 
            onclick="btnGetActivities_Click" />&nbsp;&nbsp;<asp:Button ID="btnEndActivity" 
            runat="server" Text="End Activity" onclick="btnEndActivity_Click" /><br /><br />
        <asp:Label ID="lblLaborQty" runat="server" Text="LaborQty:" Visible="false"></asp:Label>
        &nbsp;
        <asp:TextBox ID="txtLaborQty" runat="server" MaxLength="6" Width="50" Visible="false"></asp:TextBox>
        <asp:CheckBox ID="chkComplete" runat="server" Text="Complete" Visible="false"  Checked="true"/>
        <br /><br />
        <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" 
            DataKeyNames="LaborHedSeq" DataSourceID="SqlDataSource1" BackColor="White" 
            BorderColor="#336666" BorderStyle="Double" BorderWidth="3px" CellPadding="4" 
            GridLines="Horizontal" 
            OnSelectedIndexChanged="GridView1_SelectedIndexChanged" OnDataBinding="GridView1_DataBinding">
            <RowStyle BackColor="White" ForeColor="#333333" />
            <Columns>
                <asp:CommandField ShowSelectButton="True" />
                <asp:BoundField DataField="LaborHedSeq" HeaderText="LaborHedSeq" 
                    ReadOnly="True" SortExpression="LaborHedSeq" />
                <asp:BoundField DataField="LaborDtlSeq" HeaderText="LaborDtlSeq" 
                    SortExpression="LaborDtlSeq" />
                <asp:BoundField DataField="JobNum" HeaderText="JobNum" 
                    SortExpression="JobNum" />
                <asp:BoundField DataField="Asm" HeaderText="Asm" 
                    SortExpression="Asm" />
                <asp:BoundField DataField="Op" HeaderText="Op" 
                    SortExpression="Op" />
                <asp:BoundField DataField="ResourceGrpID" HeaderText="ResourceGrpID" 
                    SortExpression="ResourceGrpID" />
                <asp:BoundField DataField="LaborType" HeaderText="LaborType" 
                    SortExpression="LaborType" />
            </Columns>
            <FooterStyle BackColor="White" ForeColor="#333333" />
            <PagerStyle BackColor="#336666" ForeColor="White" HorizontalAlign="Center" />
            <EmptyDataTemplate>
                No Current Activities. Please click &quot;Get Activities&quot; to refresh.
            </EmptyDataTemplate>
            <SelectedRowStyle BackColor="#339966" Font-Bold="True" ForeColor="White" />
            <HeaderStyle BackColor="#336666" Font-Bold="True" ForeColor="White" />
        </asp:GridView>
        <asp:SqlDataSource ID="SqlDataSource1" runat="server" 
            ConnectionString="<%$ ConnectionStrings:Vantage8RU2 %>" 
            ProviderName="<%$ ConnectionStrings:Vantage8RU2.ProviderName %>" SelectCommand="SELECT LaborHedSeq, LaborDtlSeq, JobNum, AssemblySeq AS 'Asm', OprSeq AS 'Op', ResourceGrpID, LaborType FROM PUB.LaborDtl
WHERE ActiveTrans = '1' AND EmployeeNum = ?">
            <SelectParameters>
                <asp:ControlParameter ControlID="txtEmpID" Name="?" PropertyName="Text" />
            </SelectParameters>
        </asp:SqlDataSource>
    </div>
</div>
</asp:Content>

