<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" Title="Rosenboom Connect - MES" %>

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
    
    Protected Sub btnRefresh_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        lblMessage.Text = ""
        GridView1.DataBind()
    End Sub

    Protected Sub GridView1_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        txtJobNum.Text = GridView1.SelectedRow.Cells(1).Text
        txtAsm.Text = GridView1.SelectedRow.Cells(2).Text
        txtOp.Text = GridView1.SelectedRow.Cells(3).Text
    End Sub

    Protected Sub btnStartActivity_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim wsConnect As New connect.Service1, strLaborType As String, strPlant As String, strRG As String, strJobNum As String
        
        'Get RG
        If txtRG.Text = "" Then
            strRG = "NONE"
        Else
            strRG = txtRG.Text
        End If

        'Clear RG
        txtRG.Text = ""
        
        'Check for blanks
        If Check_For_Blanks(strRG) = False Then
            lblMessage.ForeColor = Drawing.Color.Red
            lblMessage.Font.Bold = False
            lblMessage.Text = "Please check for empty fields."
            Exit Sub
        Else 'no blanks
        End If 'check for blanks
        
        'Make sure labor type is selected
        If Check_For_LaborType() = "NONE" Then
            lblMessage.ForeColor = Drawing.Color.Red
            lblMessage.Font.Bold = False
            lblMessage.Text = "Please select a labor type."
            Exit Sub
        Else
            strLaborType = Check_For_LaborType()
        End If
        
        'Check for real operation
        'If wsConnect.Check_Real_Op(txtJobNum.Text, txtAsm.Text, txtOp.Text) = False Then
        'lblMessage.ForeColor = Drawing.Color.Red
        'lblMessage.Font.Bold = False
        'lblMessage.Text = "Please enter a valid job, asm, and op."
        'Exit Sub
        'Else 'valid
        'End If
        
        'Check for open job, if not indirect
        If rbIndirect.Checked = True Then
        Else 'not indirect
            If wsConnect.Check_Open_Job(txtJobNum.Text) = False Then
                lblMessage.ForeColor = Drawing.Color.Red
                lblMessage.Font.Bold = False
                lblMessage.Text = "Please enter a valid job, asm, and op."
                Exit Sub
            Else 'valid
            End If
        End If 'check if indirect
        
        'Check if clocked in
        If wsConnect.Get_Employee_Shift(txtEmpID.Text) = 0 Then
            lblMessage.ForeColor = Drawing.Color.Red
            lblMessage.Font.Bold = False
            lblMessage.Text = "Please enter a correct employee ID."
            Exit Sub
        Else 'They are clocked in.
        End If
        
        'Check for first article, if not indirect
        If rbIndirect.Checked = True Then
        Else 'not indirect
            
            If wsConnect.Check_FirstArt(txtJobNum.Text, txtAsm.Text, txtOp.Text) = True Then
                lblMessage.ForeColor = Drawing.Color.Red
                lblMessage.Font.Bold = False
                lblMessage.Text = "There is a first article remaining on this operation. Please call QA."
                Exit Sub
            Else 'No first article
            End If 'first art check
        End If 'check for indirect
        
        'Get Plant
        If strRG.Substring(0, 1) = "S" Or strRG.Substring(0, 1) = "s" Then
            strPlant = "SPIRITLA"
        Else
            strPlant = "MfgSys"
        End If
        'Determine if job or indirect code
        If rbIndirect.Checked = True Then
            strJobNum = ddlIndirectCode.Text
        Else
            strJobNum = txtJobNum.Text
        End If
        
        'This is a good file. Write the CSV
        Write_CSV(strLaborType, strPlant, strJobNum, strRG)
        
        'Tell user
        lblMessage.Text = "Labor activity started successfully."
        lblMessage.ForeColor = Drawing.Color.Black
        lblMessage.Font.Bold = True
        
        'Clear textboxes
        Clear_Textboxes()
        
    End Sub
    
    Protected Sub Clear_Textboxes()
        txtAsm.Text = ""
        txtEmpID.Text = ""
        txtJobNum.Text = ""
        txtOp.Text = ""
    End Sub
    
    Protected Function Check_For_Blanks(ByVal strRG As String) As Boolean
        If rbProduction.Checked = True Or rbSetup.Checked = True Then
            If txtAsm.Text = "" Or txtEmpID.Text = "" Or txtJobNum.Text = "" Or txtOp.Text = "" Then
                Return False
            Else
                Return True
            End If 'check fields for prod or setup
        Else 'indirect
            If txtEmpID.Text = "" Or strRG = "NONE" Or ddlIndirectCode.Text = "" Then
                Return False
            Else
                Return True
            End If 'check fields for indirect
        End If 'determine labor type
    End Function
    
    Protected Function Check_For_LaborType() As String
        If rbIndirect.Checked = True Then
            Return "I"
        ElseIf rbProduction.Checked = True Then
            Return "P"
        ElseIf rbSetup.Checked = True Then
            Return "S"
        Else
            Return "NONE"
        End If
    End Function
    
    Protected Sub Write_CSV(ByVal strLaborType As String, ByVal strPlant As String, ByVal strJobNum As String, strRG As string)
        
        'Set File Path
        Dim rand As New Random, letter As String = "", n As Integer, strFilePath As String = "\\pithos\company\service connect\" & _
            "Rosenboom Connect\Start Activity\"
        'Get random file name
        For n = 0 To 8
            letter &= ChrW(rand.Next(Asc("A"), Asc("Z") + 1))
        Next
        strFilePath &= letter & ".csv"

        'Start writing the file
        Dim CSVFile As New IO.StreamWriter(strFilePath)
        CSVFile.WriteLine("EmpID,JobNum,AsmSeq,OprSeq,LaborType,Plant")
        CSVFile.WriteLine(txtEmpID.Text & "," & strJobNum & "-" & strRG & "," & txtAsm.Text & "," & txtOp.Text & "," & strLaborType & "," & strPlant)
        CSVFile.Close()
    End Sub
</script>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div class="wrapper col3">
<div id="intro">
<h2>Start Labor Activity</h2>
<asp:Label ID="lblMessage" runat="server" Text=""></asp:Label><br /><br />

    Employee ID:&nbsp;<asp:TextBox ID="txtEmpID" runat="server" MaxLength="5" Width="50"></asp:TextBox>
    &nbsp;&nbsp; Resource Group:&nbsp;<asp:TextBox ID="txtRG" runat="server" MaxLength="8" Width="80"></asp:TextBox>
    &nbsp;&nbsp; Indirect Code: &nbsp;    <asp:DropDownList ID="ddlIndirectCode" runat="server" 
        DataSourceID="SqlDataSource2" DataTextField="Description" 
        DataValueField="IndirectCode"></asp:DropDownList>
    <asp:SqlDataSource ID="SqlDataSource2" runat="server" 
        ConnectionString="<%$ ConnectionStrings:Vantage8RU2 %>" 
        ProviderName="<%$ ConnectionStrings:Vantage8RU2.ProviderName %>" 
        SelectCommand="SELECT IndirectCode, Description FROM PUB.Indirect">
    </asp:SqlDataSource>
    <br />
    JobNum:&nbsp;<asp:TextBox ID="txtJobNum" runat="server" MaxLength="15" Width="120"></asp:TextBox>
    &nbsp;&nbsp; Asm:&nbsp;<asp:TextBox ID="txtAsm" runat="server" MaxLength="5" Width="50"></asp:TextBox>
    &nbsp;&nbsp; Op:&nbsp;<asp:TextBox ID="txtOp" runat="server" MaxLength="5" Width="50"></asp:TextBox>
    &nbsp;&nbsp;
     <br /><br />
    <asp:RadioButton ID="rbSetup" runat="server" Text="Setup" GroupName="LaborType" />
    <asp:RadioButton ID="rbProduction" runat="server" Text="Production" GroupName="LaborType" />
    <asp:RadioButton ID="rbIndirect" runat="server" Text="Indirect" GroupName="LaborType" />
    <br /> <br />
    <asp:Button ID="btnRefresh" runat="server" Text="Show Priority Dispatch" 
        onclick="btnRefresh_Click" /> 
    <asp:Button ID="btnStartActivity" runat="server" 
        Text="Start Activity" onclick="btnStartActivity_Click" />  <br /><br />
    <asp:GridView ID="GridView1" runat="server" AllowSorting="True" 
        AutoGenerateColumns="False" DataSourceID="SqlDataSource1" 
        BackColor="White" BorderColor="#336666" BorderStyle="Double" BorderWidth="3px" 
        CellPadding="4" GridLines="Horizontal" 
        OnSelectedIndexChanged="GridView1_SelectedIndexChanged" AllowPaging="True">
        <RowStyle BackColor="White" ForeColor="#333333" />
        <Columns>
            <asp:CommandField ShowSelectButton="True" />
            <asp:BoundField DataField="JobNum" HeaderText="JobNum" 
                SortExpression="JobNum" />
            <asp:BoundField DataField="Asm" HeaderText="Asm" SortExpression="Asm" />
            <asp:BoundField DataField="Op" HeaderText="Op" SortExpression="Op" />
            <asp:BoundField DataField="OpDesc" HeaderText="OpDesc" 
                SortExpression="OpDesc" />
            <asp:BoundField DataField="Priority" HeaderText="Priority" 
                SortExpression="Priority" />
            <asp:BoundField DataField="StartDate" HeaderText="StartDate" 
                SortExpression="StartDate" DataFormatString="{0:d}" />
            <asp:BoundField DataField="Job Part" HeaderText="Job Part" 
                SortExpression="Job Part" />
            <asp:BoundField DataField="Asm Part" HeaderText="Asm Part" 
                SortExpression="Asm Part" />
            <asp:BoundField DataField="Total Qty" HeaderText="Total Qty" 
                SortExpression="Total Qty" DataFormatString="{0:N0}" />
            <asp:BoundField DataField="Completed" DataFormatString="{0:N0}" 
                HeaderText="Completed" SortExpression="Completed" />
        </Columns>
        <FooterStyle BackColor="White" ForeColor="#333333" />
        <PagerStyle BackColor="#336666" ForeColor="White" HorizontalAlign="Center" />
        <SelectedRowStyle BackColor="#339966" Font-Bold="True" ForeColor="White" />
        <HeaderStyle BackColor="#336666" Font-Bold="True" ForeColor="White" />
    </asp:GridView>
    <asp:SqlDataSource ID="SqlDataSource1" runat="server" 
        ConnectionString="<%$ ConnectionStrings:Vantage8RU2 %>" 
        ProviderName="<%$ ConnectionStrings:Vantage8RU2.ProviderName %>" SelectCommand="SELECT JobOper.JobNum, JobOper.AssemblySeq AS 'Asm', JobOper.OprSeq AS 'Op', OpMaster.OpDesc, JobHead.SchedCode AS 'Priority', JobOper.StartDate, JobHead.PartNum AS 'Job Part', JobAsmbl.PartNum AS 'Asm Part', JobAsmbl.RequiredQty AS 'Total Qty', JobOper.QtyCompleted AS 'Completed'
FROM   ((MFGSYS.PUB.JobOper JobOper INNER JOIN MFGSYS.PUB.JobHead JobHead ON (JobOper.Company=JobHead.Company) AND (JobOper.JobNum=JobHead.JobNum)) INNER JOIN MFGSYS.PUB.OpMaster OpMaster ON (JobOper.Company=OpMaster.Company) AND (JobOper.OpCode=OpMaster.OpCode)) INNER JOIN MFGSYS.PUB.JobOpDtl JobOpDtl ON (((JobOper.Company=JobOpDtl.Company) AND (JobOper.OprSeq=JobOpDtl.OprSeq)) AND (JobOper.AssemblySeq=JobOpDtl.AssemblySeq)) AND (JobOper.JobNum=JobOpDtl.JobNum) INNER JOIN MFGSYS.PUB.JobAsmbl JobAsmbl ON ((JobOper.Company=JobAsmbl.Company)  AND (JobOper.AssemblySeq=JobAsmbl.AssemblySeq)) AND (JobOper.JobNum=JobAsmbl.JobNum) 
WHERE JobOper.OpComplete=0 AND JobHead.JobComplete=0 AND JobHead.JobReleased=1 AND JobOpDtl.ResourceGrpID=?
ORDER BY JobHead.SchedCode, JobOper.StartDate">
        <SelectParameters>
            <asp:ControlParameter ControlID="txtRG" Name="?" PropertyName="Text" />
        </SelectParameters>
    </asp:SqlDataSource>
</div>

</div>

</asp:Content>

