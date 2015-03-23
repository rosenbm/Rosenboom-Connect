<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" Title="Rosenboom Portal - Record a FLIP" %>

<script runat="server">
    
    Protected Sub btnRefreshOps_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Me.gvwJobOpers.DataBind()
        lblMessage.Text = ""
    End Sub
    
        
    Protected Sub gvwJobOpers_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        txtAsm.Text = gvwJobOpers.SelectedRow.Cells(1).Text
        txtOp.Text = gvwJobOpers.SelectedRow.Cells(2).Text
        
    End Sub

    Protected Sub btnSubmit_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim wsConnect As New connect.Service1
        'Check for valid employee 
        If wsConnect.Check_FLIP_Employee(txtEmpID.Text) = False Then
            lblMessage.ForeColor = Drawing.Color.Red
            lblMessage.Font.Bold = False
            lblMessage.Text = "Employee is either entered incorrectly or is not a valid inspector."
            Exit Sub
        Else
        End If
        'Check for valid job
        If wsConnect.Check_Real_Job(txtJobNum.Text) = False Then
            lblMessage.ForeColor = Drawing.Color.Red
            lblMessage.Font.Bold = False
            lblMessage.Text = "The job number does not exist."
            Exit Sub
        Else
        End If
        'Make sure one of the check boxes are checked
        If chkFP.Checked = False And chkLP.Checked = False Then
            lblMessage.ForeColor = Drawing.Color.Red
            lblMessage.Font.Bold = False
            lblMessage.Text = "Please select First Part, Last Part, or both."
            Exit Sub
        End If
        
        'This is a good entry. Write the file
        Write_CSV()
        
        'Clear Textboxes
        txtAsm.Text = ""
        txtOp.Text = ""
        txtEmpID.Text = ""
        txtJobNum.Text = ""
        chkFP.Checked = False
        chkLP.Checked = False
        'Set Message
        lblMessage.Font.Bold = True
        lblMessage.ForeColor = Drawing.Color.Black
        lblMessage.Text = "FLIP successfully recorded."
        
    End Sub
    
    Protected Sub Write_CSV()
        'Set File Path
        Dim rand As New Random, letter As String = "", n As Integer, strFilePath As String = "\\pithos\company\service connect\" & _
            "Rosenboom Connect\FLIP Import\"
        'Get random file name
        For n = 0 To 8
            letter &= ChrW(rand.Next(Asc("A"), Asc("Z") + 1))
        Next
        strFilePath &= letter & ".csv"

        'Start writing the file
        Dim CSVFile As New IO.StreamWriter(strFilePath)
        CSVFile.WriteLine("JobNum,AsmSeq,OprSeq,Inspector,InspectionDate,InspectionTime,Comments,Type")
        
        'Check for first part
        If chkFP.Checked = True Then
            CSVFile.WriteLine(txtJobNum.Text & "," & txtAsm.Text & "," & txtOp.Text & "," & txtEmpID.Text & "," & _
                Format(Today, "yyyy-MM-dd") & "T00:00:00," & FormatDateTime(Now, DateFormat.ShortTime) & "," & "" & "," & _
                "FirstPart")
        Else
        End If
        'Check for last part
        If chkLP.Checked = True Then
            CSVFile.WriteLine(txtJobNum.Text & "," & txtAsm.Text & "," & txtOp.Text & "," & txtEmpID.Text & "," & _
                Format(Today, "yyyy-MM-dd") & "T00:00:00," & FormatDateTime(Now, DateFormat.ShortTime) & "," & "" & "," & _
                "LastPart")
        Else
        End If
        
        CSVFile.Close()
    End Sub
    
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs)

        Me.txtEmpID.Attributes("onkeyup") = "autotab(" + Me.txtEmpID.ClientID + ", " + Me.txtJobNum.ClientID + ")"

    End Sub
</script>



<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <script type="text/javascript">
        function autotab(original, destination) {
            if (original.getAttribute && original.value.length == original.getAttribute("maxlength"))
                destination.focus()
        }</script>

    <div class="wrapper col3">
<div id="intro">
    <h2>Record a FLIP</h2>
    <asp:Label ID="lblMessage" runat="server" Text=""></asp:Label><br /><br />
    &nbsp;Inspector: &nbsp; <asp:TextBox ID="txtEmpID" runat="server" MaxLength="5" ></asp:TextBox> <br /><br />
    &nbsp;&nbsp;&nbsp; JobNum:&nbsp; <asp:TextBox ID="txtJobNum" runat="server"></asp:TextBox>
    &nbsp;&nbsp;&nbsp; Asm:&nbsp;<asp:TextBox ID="txtAsm" runat="server" Width="50"></asp:TextBox>
    &nbsp;&nbsp;&nbsp; Op:&nbsp;<asp:TextBox ID="txtOp" runat="server" Width="50"></asp:TextBox>
     <br /><br /><br />
    <asp:CheckBox ID="chkFP" runat="server" Text="First Part"/>&nbsp;&nbsp;<asp:CheckBox ID="chkLP" runat="server" Text="Last Part"/>
    <br /><br /><br />
    <asp:Button ID="btnRefreshOps" runat="server" Text="Get Operations" 
        onclick="btnRefreshOps_Click" />&nbsp;&nbsp;&nbsp;<asp:Button 
        ID="btnSubmit" runat="server" Text="Submit" onclick="btnSubmit_Click" /><br /><br />
    
    <asp:GridView ID="gvwJobOpers" runat="server" DataSourceID="SqlDataSource1" 
         AutoGenerateColumns="False" 
        OnSelectedIndexChanged="gvwJobOpers_SelectedIndexChanged" BackColor="White" 
        BorderColor="#336666" BorderStyle="Double" BorderWidth="3px" CellPadding="4" 
        GridLines="Horizontal">
         <FooterStyle BackColor="White" ForeColor="#333333" />
         <PagerStyle BackColor="#336666" ForeColor="White" HorizontalAlign="Center" />
         <SelectedRowStyle CssClass="SelectedRowStyle" BackColor="#E2DED6" 
            Font-Bold="True" ForeColor="#333333" /> 
         <RowStyle BackColor="White" ForeColor="#333333" />
        <Columns>
            <asp:BoundField DataField="PartNum" HeaderText="PartNum" 
                SortExpression="PartNum" />
            <asp:BoundField DataField="AssemblySeq" HeaderText="AssemblySeq" 
                SortExpression="AssemblySeq" />
            <asp:BoundField DataField="OprSeq" HeaderText="OprSeq" 
                SortExpression="OprSeq" />
            <asp:BoundField DataField="OpCode" HeaderText="OpCode" 
                SortExpression="OpCode" />
            <asp:BoundField DataField="OpDesc" HeaderText="OpDesc" 
                SortExpression="OpDesc" />
            <asp:CommandField ShowSelectButton="True" />
        </Columns>
         <HeaderStyle BackColor="#336666" Font-Bold="True" ForeColor="White" />
    </asp:GridView>
    <asp:SqlDataSource ID="SqlDataSource1" runat="server" 
        ConnectionString="<%$ ConnectionStrings:Vantage8RU2 %>" 
        ProviderName="<%$ ConnectionStrings:Vantage8RU2.ProviderName %>" SelectCommand=" SELECT &quot;JobAsmbl1&quot;.&quot;PartNum&quot;, &quot;JobOper1&quot;.&quot;AssemblySeq&quot;, &quot;JobOper1&quot;.&quot;OprSeq&quot;, &quot;JobOper1&quot;.&quot;OpCode&quot;, &quot;OpMaster1&quot;.&quot;OpDesc&quot;
 FROM   (&quot;MFGSYS&quot;.&quot;PUB&quot;.&quot;JobOper&quot; &quot;JobOper1&quot; INNER JOIN &quot;MFGSYS&quot;.&quot;PUB&quot;.&quot;JobAsmbl&quot; &quot;JobAsmbl1&quot; ON ((&quot;JobOper1&quot;.&quot;Company&quot;=&quot;JobAsmbl1&quot;.&quot;Company&quot;) AND (&quot;JobOper1&quot;.&quot;JobNum&quot;=&quot;JobAsmbl1&quot;.&quot;JobNum&quot;)) AND (&quot;JobOper1&quot;.&quot;AssemblySeq&quot;=&quot;JobAsmbl1&quot;.&quot;AssemblySeq&quot;)) INNER JOIN &quot;MFGSYS&quot;.&quot;PUB&quot;.&quot;OpMaster&quot; &quot;OpMaster1&quot; ON (&quot;JobOper1&quot;.&quot;Company&quot;=&quot;OpMaster1&quot;.&quot;Company&quot;) AND (&quot;JobOper1&quot;.&quot;OpCode&quot;=&quot;OpMaster1&quot;.&quot;OpCode&quot;)
 WHERE  &quot;JobOper1&quot;.&quot;JobNum&quot;=?">
        <SelectParameters>
            <asp:ControlParameter ControlID="txtJobNum" Name="?" PropertyName="Text" />
        </SelectParameters>
    </asp:SqlDataSource>
    </div></div>
</asp:Content>

