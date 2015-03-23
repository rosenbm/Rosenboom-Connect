<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" Title="Rosenboom Connect - EFS - Material Issues" %>
 
<script runat="server">
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim FeedbackTab As System.Web.UI.HtmlControls.HtmlGenericControl
        FeedbackTab = Master.FindControl("feedbacktab")
        FeedbackTab.Attributes.Add("class", "active")
    End Sub

    Protected Sub btnSubmit_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim intShift As Integer = 0, wsConnect As New connect.Service1

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
            lblMessage.Text = "Material Issue Submitted."
            lblMessage.ForeColor = Drawing.Color.Black
            lblMessage.Font.Bold = True
        End If
        
    End Sub
    
    Protected Sub Write_CSV()
        Dim strAdditionalInfo As String = ""
        'Set File Path
        Dim rand As New Random, letter As String = "", n As Integer, strFilePath As String = "\\pithos\company\service connect\" & _
            "Rosenboom Connect\Submit Material Issue\"
        'Get random file name
        For n = 0 To 8
            letter &= ChrW(rand.Next(Asc("A"), Asc("Z") + 1))
        Next
        strFilePath &= letter & ".csv"
        'Clean up additional info
        strAdditionalInfo = txtAdditionalnfo.Text.Replace(",", " ")
        strAdditionalInfo = strAdditionalInfo.Replace(":", " ")
        strAdditionalInfo = strAdditionalInfo.Replace(";", " ")
        
        'Start writing the file
        Dim CSVFile As New IO.StreamWriter(strFilePath)
        CSVFile.WriteLine("EmpID,ResourceGrpID,Supervisor,Issue,AdditionalInfo,PartNum")
        CSVFile.WriteLine(txtRequestor.Text & "," & ddlRG.Text & "," & ddlSupervisor.Text & "," & ddlIssue.Text & "," & _
                          strAdditionalInfo & "," & txtPartNum.Text)
        CSVFile.Close()
    End Sub
</script>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<div class="wrapper col3">
    <div id="intro">
    <h2>Submit Material Issues</h2>
        <asp:Label ID="lblMessage" runat="server" Text=""></asp:Label><br /><br />
        Supervisor<br /> <asp:DropDownList ID="ddlSupervisor" runat="server">
            <asp:ListItem>Ryan Laubenthal</asp:ListItem>
            <asp:ListItem>Brent Hauge</asp:ListItem>
            <asp:ListItem>Dan Molendorp</asp:ListItem>
            <asp:ListItem>Bob Leathers</asp:ListItem>
            <asp:ListItem>Kim Dunn</asp:ListItem>
            <asp:ListItem>Trevor Otkin</asp:ListItem>
            <asp:ListItem>Dan Block</asp:ListItem>
            <asp:ListItem>Brad Jones</asp:ListItem>
            <asp:ListItem>Dick Jostand</asp:ListItem>
            <asp:ListItem>Eric Kinnetz</asp:ListItem>
            <asp:ListItem>Mike Kinney</asp:ListItem>
            <asp:ListItem>Mike Billings</asp:ListItem>
            <asp:ListItem>Scott Banks</asp:ListItem>
            <asp:ListItem>TJ Doppler</asp:ListItem>
            <asp:ListItem>Fred Year</asp:ListItem>
            <asp:ListItem>Dan Smisek</asp:ListItem>
            <asp:ListItem>Dave Perkins</asp:ListItem>
            <asp:ListItem>Derek Bailey</asp:ListItem>
            <asp:ListItem>Tony Diehm</asp:ListItem>
        </asp:DropDownList><br /><br />
        Requestor (Employee ID)<br />   <asp:TextBox ID="txtRequestor" runat="server" Width="70"></asp:TextBox><br /><br />
        Requestor Email<br /><asp:TextBox ID="txtEmail" runat="server" Width="217px"></asp:TextBox><br /><br />
        Resource Group (WC)<br />       <asp:DropDownList ID="ddlRG" runat="server" 
            DataSourceID="SqlDataSource1" DataTextField="ResourceGrpID" 
            DataValueField="ResourceGrpID">        </asp:DropDownList>        
        <asp:SqlDataSource ID="SqlDataSource1" runat="server" 
            ConnectionString="<%$ ConnectionStrings:Vantage8RU2 %>" 
            ProviderName="<%$ ConnectionStrings:Vantage8RU2.ProviderName %>" 
            SelectCommand="SELECT ResourceGrpID FROM PUB.ResourceGroup WHERE (Plant = 'SPIRITLA') AND (ResourceGrpID LIKE 'S%')">
        </asp:SqlDataSource>
        <br />
        Part Number<br /><asp:TextBox ID="txtPartNum" runat="server"></asp:TextBox><br /><br />
        Issue<br /><asp:DropDownList ID="ddlIssue" runat="server">
            <asp:ListItem>Material Shortage</asp:ListItem>
            <asp:ListItem>Delivery Timeliness</asp:ListItem>
            <asp:ListItem>Transaction Timeliness</asp:ListItem>
            <asp:ListItem>Wrong Material Delivered</asp:ListItem>
            <asp:ListItem>No Response From Driver</asp:ListItem>
            <asp:ListItem>Mislabeled Material</asp:ListItem>
            <asp:ListItem>Qty Adjustment Transaction Verification</asp:ListItem>
            <asp:ListItem>Inventory Verification</asp:ListItem>
            <asp:ListItem>Safety Concern</asp:ListItem>
            <asp:ListItem>Other</asp:ListItem>
        </asp:DropDownList><br /><br />
        Additional Information<br /> <asp:TextBox ID="txtAdditionalnfo" runat="server"  Height="100" Width="400" TextMode="multiline"></asp:TextBox><br /><br />
        <asp:Button ID="btnSubmit" runat="server" Text="Submit" 
            onclick="btnSubmit_Click" /><br /><br />
    </div>
</div>
</asp:Content>

