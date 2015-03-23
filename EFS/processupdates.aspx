<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" Title="Rosenboom Connect - EFS - Material Issues" %>

<script runat="server">
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim FeedbackTab As System.Web.UI.HtmlControls.HtmlGenericControl
        FeedbackTab = Master.FindControl("feedbacktab")
        FeedbackTab.Attributes.Add("class", "active")
    End Sub

    Protected Sub btnSubmit_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim intShift As Integer = 0, wsConnect As New connect.Service1
        'Check to see if a type is selected
        If rbNewProcess.Checked = False And rbUpdateExisting.Checked = False Then
            lblMessage.Text = "Please Select a Request Type."
            lblMessage.ForeColor = Drawing.Color.Red
            lblMessage.Font.Bold = False
            Exit Sub
        Else
        End If
        
        'Determine if the employee exists
        Try
            intShift = wsConnect.Get_Employee_Shift(txtRequestor.Text)
        Catch ex As Exception
            intShift = 1
        End Try
        If intShift = 0 Then
            lblMessage.Text = "Please enter a valid employee ID."
            lblMessage.ForeColor = Drawing.Color.Red
            lblMessage.Font.Bold = False
        Else
            'Write the Service Connect File
            Write_CSV()
            'Clear the Additional info box
            txtAdditionalnfo.Text = ""
            'Send message to the user
            lblMessage.Text = "Process request Submitted."
            lblMessage.ForeColor = Drawing.Color.Black
            lblMessage.Font.Bold = True
        End If
        
    End Sub
    
    Protected Sub Write_CSV()
        Dim strType As String = ""
        
        'Determine Request Type
        If rbNewProcess.Checked = True Then
            strType = "New Process"
        Else
            strType = "Update Existing Process"
        End If
        
        'Set File Path
        Dim rand As New Random, letter As String = "", n As Integer, strFilePath As String = "\\pithos\company\service connect\" & _
            "Rosenboom Connect\Process Request\"
        'Get random file name
        For n = 0 To 8
            letter &= ChrW(rand.Next(Asc("A"), Asc("Z") + 1))
        Next
        strFilePath &= letter & ".csv"

        'Start writing the file
        Dim CSVFile As New IO.StreamWriter(strFilePath)
        CSVFile.WriteLine("EmpID,Type,ProcessName,Description")
        CSVFile.WriteLine(txtRequestor.Text & "," & strType & "," & txtProcessName.Text.Replace(",", " ") & "," & _
                          txtAdditionalnfo.Text.Replace(",", " "))
        CSVFile.Close()
    End Sub
</script>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<div class="wrapper col3">
    <div id="intro">
    <h2>Process Update Requests</h2>
        <asp:Label ID="lblMessage" runat="server" Text=""></asp:Label><br /><br />
        <b>Process Request Type:</b><br />
        <asp:RadioButton ID="rbNewProcess" runat="server" GroupName="OnlyOne" Text="Request for New Process"/><br />
        <asp:RadioButton ID="rbUpdateExisting" runat="server" GroupName="OnlyOne" Text="Request for Update on Existing Process"/><br /><br />
        <b>Process Name:</b><br />
        <asp:TextBox ID="txtProcessName" runat="server" Width="300"></asp:TextBox><br /><br />
        <b>Requestor (Employee ID):</b><br />   <asp:TextBox ID="txtRequestor" runat="server" Width="100"></asp:TextBox><br /><br />
        <b>Description of Change:</b><br /> <asp:TextBox ID="txtAdditionalnfo" runat="server"  Height="100" Width="400" TextMode="multiline"></asp:TextBox><br /><br />
        <asp:Button ID="btnSubmit" runat="server" Text="Submit" 
            onclick="btnSubmit_Click" /><br /><br />
    </div>
</div>
</asp:Content>

