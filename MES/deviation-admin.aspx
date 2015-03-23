<%@ Import Namespace="System.IO" %>

<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" Title="Material Deviation/Change Admin" %>


<script runat="server">
  
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs)
        'Highlight the current tab
        Dim MESTab As System.Web.UI.HtmlControls.HtmlGenericControl
        MESTab = Master.FindControl("mestab")
        MESTab.Attributes.Add("class", "active")
        'Set hyperlink
        If GridView1.SelectedValue Is Nothing Then
            lnkHistory.NavigateUrl = "deviation-history.aspx"
        Else
            lnkHistory.NavigateUrl = "deviation-history.aspx?partnum=" & GridView1.SelectedRow.Cells(6).Text
        End If
    End Sub
    
    
    Protected Sub btnSubmit_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim strPrimaryKey As String, wsConnect As New connect.Service1, intApproved As Integer, strPlant As String, strRequestType As String
        Dim strEmpID As String = GridView1.SelectedRow.Cells(4).Text, strRG As String = GridView1.SelectedRow.Cells(10).Text
        Dim strCylinder As String = GridView1.SelectedRow.Cells(5).Text, strComponent As String = GridView1.SelectedRow.Cells(6).Text
        Dim strQty As String = GridView1.SelectedRow.Cells(7).Text, strCustomer As String = GridView1.SelectedRow.Cells(8).Text
        Dim strJobNum As String = GridView1.SelectedRow.Cells(9).Text, strDateofChange As String = DetailsView1.Rows(3).Cells(1).Text
        Dim strRequest As String = DetailsView1.Rows(1).Cells(1).Text, strReason As String = DetailsView1.Rows(2).Cells(1).Text
        'Make sure approval decision is made
        If rbApproved.Checked = False And rbNotApproved.Checked = False Then
            NotValid("Please select an approval status.")
            Exit Sub
        End If
        'Check if a plant selection was made
        If rbSheldon.Checked = False And rbSpiritLake.Checked = False And rbBothPlants.Checked = False Then
            NotValid("Please select a plant.")
            Exit Sub
        End If
        'Get Primary Key
        Try
            strPrimaryKey = GridView1.SelectedDataKey.Value
        Catch ex As Exception
            NotValid("Please select a record to update.")
            Exit Sub
        End Try
        'Check Approved By
        If txtApprovedBy.Text = "" Then
            NotValid("Please enter approved by name.")
            Exit Sub
        End If
        'Set intApproved
        If rbApproved.Checked = True Then
            intApproved = 1
        Else
            intApproved = 0
        End If
        'Set strPlant
        If rbSheldon.Checked = True Then
            strPlant = "Sheldon"
        ElseIf rbSpiritLake.Checked = True Then
            strPlant = "Spirit Lake"
        Else
            strPlant = "Both"
        End If
        'Get Request Type
        strRequestType = GridView1.SelectedRow.Cells(3).Text
        'Update Record
        Try
            wsConnect.DEV_Update_Record(txtSpecialInstructions.Text, intApproved, strPlant, "Closed", txtApprovedBy.Text, strPrimaryKey)
        Catch ex As Exception
            NotValid(ex.Message)
            Exit Sub
        End Try
        'Create HTML File
        Create_Document(strRequestType, strRequestType, strEmpID, strRG, strCylinder, strComponent, strQty, strCustomer, strDateofChange, strJobNum, _
                        strRequest, strReason)
        'Refresh Data
        GridView1.DataBind()
    End Sub
    
    Protected Sub Create_Document(ByVal strFormTitle As String, ByVal strRequestType As String, ByVal strEmpID As String, ByVal strRG As String, _
              ByVal strCylinder As String, ByVal strComponent As String, ByVal strQty As String, ByVal strCustomer As String, _
              ByVal strDateofChange As String, ByVal strJobNum As String, ByVal strRequest As String, ByVal strReason As String)
        Dim strFilePath As String, strFlexColumn As String, strFlexValue As String, strApproved As String, strPlant As String, bolPart As Boolean
        'Determine Flex Column
        strRequestType.Trim()
        If strRequestType.Contains("Part") = True Then
            strFlexColumn = ""
            strFlexValue = ""
            bolPart = True
        ElseIf strRequestType.Contains("Material") = True Then
            strFlexColumn = "Size Change"
            strFlexValue = DetailsView1.Rows(4).Cells(1).Text
            bolPart = False
        Else
            strFlexColumn = "Target Work Center"
            strFlexValue = GridView1.SelectedRow.Cells(11).Text
            bolPart = False
        End If
        'Determine Approved
        If rbApproved.Checked = True Then
            strApproved = "Yes"
        Else
            strApproved = "No"
        End If
        'Determine Plant
        If rbSheldon.Checked = True Then
            strPlant = "Sheldon"
        ElseIf rbSpiritLake.Checked = True Then
            strPlant = "Spirit Lake"
        Else
            strPlant = "Both"
        End If
        'Determine File Path
        If bolPart = False Then
            If strFlexColumn = "Size Change" Then
                strFilePath = "\\pithos\company\Materials\Deviation_Requests\Test.html"
            Else
                strFilePath = "\\pithos\company\Quality Assurance\Change Requests\" & strComponent & " - " & strJobNum & ".html"
            End If
        Else 'it is a part deviation
            If strPlant = "Sheldon" Then
                strFilePath = "\\pithos\company\Quality Assurance\SH-Deviations\" & strComponent & " - " & strJobNum & ".html"
            Else
                strFilePath = "\\pithos\company\Quality Assurance\SL\Deviations\" & strComponent & " - " & strJobNum & ".html"
            End If
        End If
        
        'Write File
        Dim HTMLFile As New IO.StreamWriter(strFilePath)
        HTMLFile.WriteLine("<html xmlns=""http://www.w3.org/1999/xhtml""><head runat=""server""><title>Rosenboom Form</title><style type=""text/css"">")
        Try 'Write CSS Info
            Using sr As New StreamReader("\\pithos\company\Service Connect\Rosenboom Connect\Text Files\Deviation.css")
                Dim line As String
                line = sr.ReadToEnd()
                HTMLFile.WriteLine(line)
            End Using
        Catch e As Exception
            lblMessage.Text = e.Message
        End Try
        'Write Data
        HTMLFile.WriteLine("</style>")
        HTMLFile.WriteLine("</head>")
        HTMLFile.WriteLine("<body>")
        HTMLFile.WriteLine("<form id=""form1"" runat=""server"">")
        HTMLFile.WriteLine("<div id=""main"">")
        HTMLFile.WriteLine("<div class=""title"">" & strFormTitle & "</div>")
        HTMLFile.WriteLine("<div class=""header-one-third"">Requested By:</div>")
        HTMLFile.WriteLine("<div class=""header-one-third"">Cell/WorkCenter</div>")
        HTMLFile.WriteLine("<div class=""header-one-third"">" & strFlexColumn & "</div>")
        HTMLFile.WriteLine("<div class=""cell-one-third"">" & strEmpID & "</div>")
        HTMLFile.WriteLine("<div class=""cell-one-third"">" & strRG & "</div>")
        HTMLFile.WriteLine("<div class=""cell-one-third"">" & strFlexValue & "</div>")
        HTMLFile.WriteLine("<div class=""header-one-third"">Cylinder Part Number:</div>")
        HTMLFile.WriteLine("<div class=""header-one-third"">Component Part Number:</div>")
        HTMLFile.WriteLine("<div class=""header-one-third"">Number of Parts Affected:</div>")
        HTMLFile.WriteLine("<div class=""cell-one-third"">" & strCylinder & "</div>")
        HTMLFile.WriteLine("<div class=""cell-one-third"">" & strComponent & "</div>")
        HTMLFile.WriteLine("<div class=""cell-one-third"">" & strQty & "</div>")
        HTMLFile.WriteLine("<div class=""header-one-third"">Customer:</div>")
        HTMLFile.WriteLine("<div class=""header-one-third"">Requested Date of Change:</div>")
        HTMLFile.WriteLine("<div class=""header-one-third"">Job Number:</div>")
        HTMLFile.WriteLine("<div class=""cell-one-third"">" & strCustomer & "</div>")
        HTMLFile.WriteLine("<div class=""cell-one-third"">" & strDateofChange & "</div>")
        HTMLFile.WriteLine("<div class=""cell-one-third"">" & strJobNum & "</div>")
        HTMLFile.WriteLine("<div class=""header-one-hundred"">Requested Deviation or Change:</div>")
        HTMLFile.WriteLine("<div class=""cell-one-hundred"">" & strRequest & "</div>")
        HTMLFile.WriteLine("<div class=""header-one-hundred"">Reason for Deviation or Change:</div>")
        HTMLFile.WriteLine("<div class=""cell-one-hundred"">" & strReason & "</div>")
        HTMLFile.WriteLine("<div class=""header-one-hundred"">Special Instructions (if needed):</div>")
        HTMLFile.WriteLine("<div class=""cell-one-hundred"">" & txtSpecialInstructions.Text & "</div>")
        HTMLFile.WriteLine("<div class=""header-one-third"">Approved:</div>")
        HTMLFile.WriteLine("<div class=""header-one-third"">Qty Approved:</div>")
        HTMLFile.WriteLine("<div class=""header-one-third"">Plant</div>")
        HTMLFile.WriteLine("<div class=""cell-one-third"">" & strApproved & "</div>")
        HTMLFile.WriteLine("<div class=""cell-one-third"">" & txtQty.Text & "</div>")
        HTMLFile.WriteLine("<div class=""cell-one-third"">" & strPlant & "</div>")
        HTMLFile.WriteLine("<div class=""header-two-thirds"">Authorized By:</div>")
        HTMLFile.WriteLine("<div class=""header-one-third"">Date:</div>")
        HTMLFile.WriteLine("<div class=""cell-two-thirds"">" & txtApprovedBy.Text & "</div>")
        HTMLFile.WriteLine("<div class=""cell-one-third"">" & Today & "</div>")
        HTMLFile.WriteLine("</div>")
        HTMLFile.WriteLine("</form>")
        HTMLFile.WriteLine("</body>")
        HTMLFile.WriteLine("</html>")
        HTMLFile.Close()
        'Attach new file to the job
        'Set File Path
        Dim rand As New Random, letter As String = "", n As Integer, strCSVFilePath As String = _
            "\\pithos\company\Service Connect\Rosenboom Connect\Job Attachment\"
        'Get random file name
        For n = 0 To 8
            letter &= ChrW(rand.Next(Asc("A"), Asc("Z") + 1))
        Next
        strCSVFilePath &= letter & ".csv"

        'Start writing the file to attach to the Job Head
        Dim CSVFile As New IO.StreamWriter(strCSVFilePath)
        CSVFile.WriteLine("JobNum,FileName,Desc,Plant")
        CSVFile.WriteLine(strJobNum & "," & strFilePath & "," & strRequestType & "," & strPlant)
        CSVFile.Close()
    End Sub
    
    Protected Sub NotValid(ByVal strMessage As String)
        'Set the message label
        lblMessage.Font.Bold = True
        lblMessage.ForeColor = Drawing.Color.Red
        lblMessage.Text = strMessage
    End Sub

    Protected Sub GridView1_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        If GridView1.SelectedValue Is Nothing Then
            lnkHistory.NavigateUrl = "deviation-history.aspx"
        Else
            lnkHistory.NavigateUrl = "deviation-history.aspx?partnum=" & GridView1.SelectedRow.Cells(6).Text
        End If
    End Sub
</script>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div class="wrapper col3">
<div id="intro">
<h2>Material Deviation/Change Request - Admin</h2>
    <asp:Label ID="lblMessage" runat="server" Text=""></asp:Label>
    <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" CellPadding="4" 
                        DataKeyNames="PrimaryKey" DataSourceID="SqlDataSource1" ForeColor="#333333" 
                        GridLines="None" AllowSorting="True" OnSelectedIndexChanged="GridView1_SelectedIndexChanged">
        <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
        <Columns>
            <asp:CommandField ShowSelectButton="True" />
            <asp:BoundField DataField="IDNum" HeaderText="IDNum" InsertVisible="False" 
                SortExpression="IDNum" />
            <asp:BoundField DataField="RequestedDate" HeaderText="Requested" 
                SortExpression="RequestedDate" />
            <asp:BoundField DataField="Type" HeaderText="Type" SortExpression="Type" />
            <asp:BoundField DataField="RequestorID" 
                HeaderText="EmpID" SortExpression="RequestorID" />
            <asp:BoundField DataField="CylinderPN" HeaderText="Cylinder" 
                SortExpression="CylinderPN" />
            <asp:BoundField DataField="ComponentPN" HeaderText="Component" 
                SortExpression="ComponentPN" />
            <asp:BoundField DataField="NumOfParts" HeaderText="Qty" 
                SortExpression="NumOfParts" />
            <asp:BoundField DataField="Customer" HeaderText="Customer" 
                SortExpression="Customer" />
            <asp:BoundField DataField="JobNum" HeaderText="JobNum" 
                SortExpression="JobNum" />
            <asp:BoundField DataField="ResourceGroup" HeaderText="RG" 
                SortExpression="ResourceGroup" />
            <asp:BoundField DataField="TargetWC" HeaderText="TargetRG" 
                SortExpression="TargetWC" />
        </Columns>
        <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
        <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
        <EmptyDataTemplate>
            There are currently no open material deviation or change requests.
        </EmptyDataTemplate>
        <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
        <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
        <EditRowStyle BackColor="#999999" />
        <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
    </asp:GridView>
    <asp:DetailsView ID="DetailsView1" runat="server" Height="50px" Width="717px" 
                        AutoGenerateRows="False" CellPadding="4" DataKeyNames="PrimaryKey" 
                        DataSourceID="SqlDataSource2" ForeColor="#333333" GridLines="None">
        <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
        <CommandRowStyle BackColor="#E2DED6" Font-Bold="True" />
        <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
        <FieldHeaderStyle BackColor="#E9ECF1" Font-Bold="True" />
        <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
        <Fields>
            <asp:BoundField DataField="RequestorName" HeaderText="RequestorName" 
                SortExpression="RequestorName" />
            <asp:BoundField DataField="RequestDesc" HeaderText="Request" 
                SortExpression="RequestDesc" />
            <asp:BoundField DataField="ReasonDesc" HeaderText="Reason" 
                SortExpression="ReasonDesc" />
            <asp:BoundField DataField="RequestedDateOfChange" 
                HeaderText="RequestedDateOfChange" SortExpression="RequestedDateOfChange" />
            <asp:BoundField DataField="SizeChange" HeaderText="SizeChange" 
                SortExpression="SizeChange" />
        </Fields>
        <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
        <EditRowStyle BackColor="#999999" />
        <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
    </asp:DetailsView>
    
    
    <asp:HyperLink runat=server ID="lnkHistory" Target="_blank" NavigateUrl="#">View Request History</asp:HyperLink>
    <br /><br />
    
    <asp:SqlDataSource ID="SqlDataSource1" runat="server" 
                        ConnectionString="<%$ ConnectionStrings:DeviationDB %>" 
                        ProviderName="<%$ ConnectionStrings:DeviationDB.ProviderName %>" 
                        SelectCommand="SELECT * FROM [Main] WHERE ([Status] = 'Open')">
                    </asp:SqlDataSource>
<asp:SqlDataSource ID="SqlDataSource2" runat="server" 
                        ConnectionString="<%$ ConnectionStrings:DeviationDB %>" 
                        ProviderName="<%$ ConnectionStrings:DeviationDB.ProviderName %>" SelectCommand="SELECT *
FROM Main
WHERE PrimaryKey = ?">
    <SelectParameters>
        <asp:ControlParameter ControlID="GridView1" Name="?" 
            PropertyName="SelectedValue" />
    </SelectParameters>
                    </asp:SqlDataSource>
                            Special Instructions (if needed):<br />
            <asp:TextBox ID="txtSpecialInstructions" runat="server" Width=500 Height=100 TextMode=MultiLine></asp:TextBox>
    <br /><br />
                            Distributed To:<br />
    <asp:CheckBox ID="chkMfgEng" runat="server" Text="Manufacturing Engineering" /><br />
    <asp:CheckBox ID="chkQA" runat="server" Text="Quality Assurance" /><br />
    <asp:CheckBox ID="chkMgt" runat="server" Text="Management" /><br />
    <asp:CheckBox ID="chkSched" runat="server" Text="Scheduling" /><br />
    <asp:CheckBox ID="chkCNC" runat="server" Text="CNC Machining" /><br />
    <asp:CheckBox ID="chkMfgCells" runat="server" Text="Manufacturing Cells" /><br />
    <asp:CheckBox ID="chkToolRoom" runat="server" Text="Tool Room" /><br />
    <asp:CheckBox ID="chkWeldSup" runat="server" Text="Weld Support" /><br /><br />
    Approved:<br />
    <asp:RadioButton ID="rbApproved" runat="server" GroupName="Approved" Text="Yes" />
    <asp:RadioButton ID="rbNotApproved" runat="server" GroupName="Approved" Text="No" />
    &nbsp; Qty:&nbsp;<asp:TextBox ID="txtQty" runat="server" Width=40></asp:TextBox><br /><br />
    Plant:<br />
    <asp:RadioButton ID="rbSheldon" runat="server" GroupName="Plant" Text="Sheldon" />
    <asp:RadioButton ID="rbSpiritLake" runat="server" GroupName="Plant" Text="Spirit Lake" />
    <asp:RadioButton ID="rbBothPlants" runat="server" GroupName="Plant" Text="Both" /><br /><br />
    Approved By:&nbsp;
    <asp:TextBox ID="txtApprovedBy" runat="server" Width=200></asp:TextBox><br /><asp:Button ID="btnSubmit" runat="server" Text="Submit" 
        onclick="btnSubmit_Click" />&nbsp;&nbsp;<asp:Button ID="btnClear" runat="server" Text="Clear" /><br /><br /><br />
    <br /><br />
</div>

</div>

</asp:Content>

