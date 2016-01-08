<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" Title="Rosenboom Connect - MES" %>

<script runat="server">

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim MESTab As System.Web.UI.HtmlControls.HtmlGenericControl
        MESTab = Master.FindControl("mestab")
        MESTab.Attributes.Add("class", "active")
    End Sub
    
    Protected Sub btnclockin_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        'Dim wsConnect As New , intShift As Integer
        'Check EmpID
        If txtEmpID.Text = "" Then
            lblMessage.ForeColor = Drawing.Color.Red
            lblMessage.Font.Bold = False
            lblMessage.Text = "Please enter your employee ID."
            Exit Sub
        Else 'There is an employee id
        End If
        
        'Check if clocked in
        'Try
        'If wsConnect.Check_if_clocked_in(txtEmpID.Text) = True Then
        'lblMessage.ForeColor = Drawing.Color.Red
        'lblMessage.Font.Bold = False
        'lblMessage.Text = "This employee is already clocked in."
        'Exit Sub
        ' Else 'They are not clocked in.
        'End If
        'Catch ex As Exception
        'system is offline, continue
        'End Try
        
        'Get Shift
        Try
            'intShift = wsConnect.Get_Employee_Shift(txtEmpID.Text)
            'If intShift = 0 Then
            lblMessage.ForeColor = Drawing.Color.Red
            lblMessage.Font.Bold = False
            lblMessage.Text = "Please enter a valid user."
            Exit Sub
            'Else 'They are a valid user.
            'End If
        Catch ex As Exception
            'db is down, continue
            'intShift = 0
        End Try
        
        'Write service connect file
        'Clock_InOut(txtEmpID.Text, intShift, "IN")
        
        'Tell the user
        lblMessage.ForeColor = Drawing.Color.Black
        lblMessage.Font.Bold = True
        lblMessage.Text = "Clocked in successfully."
        txtEmpID.Text = ""
    End Sub
    
    Protected Sub Clock_InOut(ByRef strEmpID As String, ByVal intshift As Integer, ByVal strType As String)
        'Set File Path
        Dim rand As New Random, letter As String = "", n As Integer, strFilePath As String = "\\pithos\company\service connect\" & _
            "Rosenboom Connect\Clock In-Out\"
        'Get random file name
        For n = 0 To 8
            letter &= ChrW(rand.Next(Asc("A"), Asc("Z") + 1))
        Next
        strFilePath &= letter & ".csv"
        'Write File
        Dim CSVFile As New IO.StreamWriter(strFilePath)
        CSVFile.WriteLine("EmpID,Shift,Type")
        CSVFile.WriteLine(strEmpID & "," & intshift & "," & strType)
        CSVFile.Close()
        
    End Sub

    Protected Sub btnclockout_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        'Dim wsConnect As New connect.Service1, intShift As Integer
        
        'Check EmpID
        If txtEmpID.Text = "" Then
            lblMessage.ForeColor = Drawing.Color.Red
            lblMessage.Font.Bold = False
            lblMessage.Text = "Please enter your employee ID."
            'Exit Sub
        Else 'There is an employee id
        End If
        
        'Check if clocked in
        'Try
        'If wsConnect.Check_if_clocked_in(txtEmpID.Text) = False Then
        'lblMessage.ForeColor = Drawing.Color.Red
        'lblMessage.Font.Bold = False
        'lblMessage.Text = "This employee is not currently clocked in."
        'Exit Sub
        'Else 'They are clocked in.
        'End If
        'Catch ex As Exception
        'Vantage is down, continue on
        'End Try
        'Write service connect file
        
        'Get Shift
        Try
            'intShift = wsConnect.Get_Employee_Shift(txtEmpID.Text)
            'If intShift = 0 Then
            lblMessage.ForeColor = Drawing.Color.Red
            lblMessage.Font.Bold = False
            lblMessage.Text = "Please enter a valid user."
            'Exit Sub
            'Else 'They are a valid user.
            'End If
        Catch ex As Exception
            'db is down, continue
            'intShift = 0
        End Try
        Clock_InOut(txtEmpID.Text, 0, "OUT")
        'Tell user
        lblMessage.ForeColor = Drawing.Color.Black
        lblMessage.Font.Bold = True
        lblMessage.Text = "Clocked out successfully."
        txtEmpID.Text = ""
    End Sub

    Protected Sub btnflip_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Response.Redirect("flip.aspx")
    End Sub

    Protected Sub btnstartactivity_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Response.Redirect("StartActivity.aspx")
    End Sub

    Protected Sub btnendactivity_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        If txtEmpID.Text = "" Then
            lblMessage.Text = "Please enter your employee ID."
        Else
            Response.Redirect("EndActivity.aspx?empid=" & txtEmpID.Text)
        End If
        
    End Sub

    Protected Sub btnissuematerial_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Response.Redirect("IssueMaterialToJob.aspx")
    End Sub

    Protected Sub btnmoverequest_Click(sender As Object, e As EventArgs)
        Response.Redirect("MoveRequest.aspx")
    End Sub

    Protected Sub btnFixture_Click(sender As Object, e As EventArgs)
        Response.Redirect("FixtureLookup.aspx")
    End Sub
</script>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div class="wrapper col3 covergolden">
<div id="intro">
<h2>MES - Main</h2>
    Employee ID:&nbsp;<asp:TextBox ID="txtEmpID" runat="server" MaxLength="5" Width="50"></asp:TextBox><br /><br />
    <asp:Label ID="lblMessage" runat="server" Text=""></asp:Label>
<ul class="mes">
    
<li><asp:Button ID="btnissuematerial" runat="server" Text="Issue Material" 
        Height="150px" Width="150px" Font-Size="Large" 
        onclick="btnissuematerial_Click" /></li>
<li><asp:Button ID="btnflip" runat="server" Text="FLIP" Height="150px" 
        Width="150px" Font-Size="Large" onclick="btnflip_Click" /></li>
<li><asp:Button ID="btnmoverequest" runat="server" Text="Move Request" Height="150px" 
        Width="150px" Font-Size="Large" OnClick="btnmoverequest_Click" /></li>
<li><asp:Button ID="btnFixture" runat="server" Text="Fixture Lookup" Height="150px" 
        Width="150px" Font-Size="Large" OnClick="btnFixture_Click" /></li>
<li><asp:Button ID="btnclockin" runat="server" Text="Clock In" Height="150px" 
        Width="150px" Font-Size="Large" onclick="btnclockin_Click" Enabled="False" Visible="False" /></li>
<li><asp:Button ID="btnclockout" runat="server" Text="Clock Out" Height="150px" 
        Width="150px" Font-Size="Large" onclick="btnclockout_Click" Enabled="False" Visible="False" /></li>
<li><asp:Button ID="btnstartactivity" runat="server" Text="Start Activity" 
        Height="150px" Width="150px" Font-Size="Large" 
        onclick="btnstartactivity_Click" Enabled="False" Visible="False" /></li>
<li><asp:Button ID="btnendactivity" runat="server" Text="End Activity" 
        Height="150px" Width="150px" Font-Size="Large" onclick="btnendactivity_Click" Enabled="False" Visible="False" /></li>
</ul>
</div>

</div>

</asp:Content>

