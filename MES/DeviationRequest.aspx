﻿<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" Title="Rosenboom Connect - MES" %>

<script runat="server">

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim MESTab As System.Web.UI.HtmlControls.HtmlGenericControl
        MESTab = Master.FindControl("mestab")
        MESTab.Attributes.Add("class", "active")
    End Sub
    
    
    Protected Sub btnSubmit_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim strType As String, intSizeChange As Integer, strEmail As String
        'Check for blanks
        If Check_For_Blank_Box() = "Good" Then
        Else 'Error
            NotValid(Check_For_Blank_Box)
            Exit Sub
        End If
        'Check User ID
        'Try
        '    If wsConnect.Get_Employee_Shift(txtEmpID.Text) = 0 Then
        '        NotValid("Please enter a valid user ID.")
        '        Exit Sub
        '    Else
        '    End If
        'Catch ex As Exception
        'End Try
        'Determine Size Change
        If chkSizeChange.Checked = True Then
            intSizeChange = 1
        Else
            intSizeChange = 0
        End If
        'Determine Type
        If rbPartDeviation.Checked = True Then
            strType = "Part Deviation"
            intSizeChange = 0
            txtProposedWC.Text = ""
            strEmail = "askrepak@rosenboom.com"
        ElseIf rbProcessChange.Checked = True Then
            strType = "Process Change"
            intSizeChange = 0
            strEmail = "askrepak@rosenboom.com"
        ElseIf rbMaterialDeviation.Checked = True Then
            strType = "Material Deviation"
            txtProposedWC.Text = ""
            strEmail = "askrepak@rosenboom.com"
        Else
            NotValid("Please select a request type.")
            Exit Sub
        End If

        'Insert a new record
        'wsConnect.DEV_Add_Request(txtEmpID.Text, txtWorkCenter.Text, txtCylPartNum.Text, txtComPartNum.Text, txtNumofParts.Text, txtCustomer.Text, _
        'txtDateOfChange.Text, txtJobNums.Text, txtRequest.Text, txtReason.Text, strType, txtProposedWC.Text, intSizeChange)
        
        'Send Alert
        Dim strFilePath As String = "\\pithos\company\Lift\LST\AutoPickUp\" & strEmail & "+" & strType & " Request " & txtEmpID.Text & " - " & Now.ToShortTimeString.Replace(":", " ") & " .txt"
        Dim CSVFile As New IO.StreamWriter(strFilePath)
        CSVFile.WriteLine("The following request has been sent:")
        CSVFile.WriteLine("")
        CSVFile.WriteLine("Type: " & strType)
        CSVFile.WriteLine("EmpID: " & txtEmpID.Text)
        CSVFile.WriteLine("Resource Group: " & txtWorkCenter.Text)
        CSVFile.WriteLine("Cylinder: " & txtCylPartNum.Text)
        CSVFile.WriteLine("Component:," & txtComPartNum.Text)
        CSVFile.WriteLine("Qty: " & txtNumofParts.Text)
        CSVFile.WriteLine("Customer: " & txtCustomer.Text)
        CSVFile.WriteLine("Job Number: " & txtJobNums.Text)
        CSVFile.WriteLine("Requested Date of Change: " & txtDateOfChange.Text)
        CSVFile.WriteLine("Request: " & txtRequest.Text)
        CSVFile.WriteLine("Reason: " & txtReason.Text)
        CSVFile.WriteLine("")
        CSVFile.WriteLine("This document was automatically generated by Rosenboom Connect.")
        CSVFile.Close()
        
        'Display a successful message
        lblMessage.Font.Bold = False
        lblMessage.ForeColor = Drawing.Color.Black
        lblMessage.Text = "Material deviation or request successfully submitted."
        
        'Clear all
        Clear_All()
    End Sub
    
    Protected Function Check_For_Blank_Box() As String

        If txtWorkCenter.Text = "" Then
            Return "Please enter a work center."
        ElseIf txtCylPartNum.Text = "" Then
            Return "Please enter a cylinder part number."
        ElseIf txtComPartNum.Text = "" Then
            Return "Please enter a component part number."
        ElseIf txtNumofParts.Text = "" Or IsNumeric(txtNumofParts.Text) = False Then
            Return "Please enter a valid number of parts."
        ElseIf txtCustomer.Text = "" Then
            Return "Please enter a customer."
        ElseIf txtDateOfChange.Text = "" Or IsDate(txtDateOfChange.Text) = False Then
            Return "Please enter a valid requested date of change."
        ElseIf txtRequest.Text = "" Then
            Return "Please describe the requested deviation or change."
        ElseIf txtReason.Text = "" Then
            Return "Please describe the reason for the requested change or deviation."
        Else
            Return "Good"
        End If
    End Function
    
    Protected Sub NotValid(ByVal strMessage As String)
        'Set the message label
        lblMessage.Font.Bold = True
        lblMessage.ForeColor = Drawing.Color.Red
        lblMessage.Text = strMessage
    End Sub
    
    Protected Sub Change_of_Type()
        'Display/Hide Fields
        If rbPartDeviation.Checked = True Then
            'Hide Proposed WC
            lblProposedWC.Visible = False
            txtProposedWC.Visible = False
            chkSizeChange.Visible = False
        ElseIf rbProcessChange.Checked = True Then
            'Show Proposed WC
            lblProposedWC.Text = "Proposed Work Center"
            lblProposedWC.Visible = True
            txtProposedWC.Visible = True
            chkSizeChange.Visible = False
        ElseIf rbMaterialDeviation.Checked = True Then
            lblProposedWC.Text = "Size Change"
            lblProposedWC.Visible = True
            txtProposedWC.Visible = False
            chkSizeChange.Visible = True
        End If
        'Hide Status Text
        lblMessage.Text = ""
    End Sub
    
    Protected Sub rbPartDeviation_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Change_of_Type()
    End Sub

    Protected Sub btnClear_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Clear_All()
    End Sub
    
    Protected Sub Clear_All()
        txtComPartNum.Text = ""
        txtCustomer.Text = ""
        txtCylPartNum.Text = ""
        txtDateOfChange.Text = ""
        txtEmpID.Text = ""
        txtJobNums.Text = ""
        txtNumofParts.Text = ""
        txtProposedWC.Text = ""
        txtReason.Text = ""
        txtRequest.Text = ""
        txtWorkCenter.Text = ""
        chkSizeChange.Checked = False
    End Sub
</script>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div class="wrapper col3">
<div id="intro">
<h2>Part Deviation or Process Change Request Form</h2><br />
    <asp:Label ID="lblMessage" runat="server" Text=""></asp:Label><br /><br />
    Type:
    <asp:RadioButton ID="rbPartDeviation" runat="server" Text="Part Deviation" GroupName="Type" 
    oncheckedchanged="rbPartDeviation_CheckedChanged" AutoPostBack=true />
    <asp:RadioButton ID="rbProcessChange" runat="server" Text="Process Change" GroupName="Type" 
    oncheckedchanged="rbPartDeviation_CheckedChanged" AutoPostBack=true/>
    <asp:RadioButton ID="rbMaterialDeviation" runat="server" Text="Material Deviation" GroupName="Type" 
    oncheckedchanged="rbPartDeviation_CheckedChanged" AutoPostBack=true/><br /><br />
    <table>
    <tr>
    <td class="grey">Requested By:    </td>
    <td class="grey">Cell/Workcenter:</td>
    <td class="grey"><asp:Label ID="lblProposedWC" runat="server" Text="Proposed Work Center:"></asp:Label></td>
    </tr>
    
    <tr>
    <td>        <!--<asp:DropDownList ID="ddlEmpID" runat="server" DataSourceID="SqlDataSource1" DataTextField="EmpID" DataValueField="EmpID"></asp:DropDownList>
        <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:Vantage8RU2 %>" 
            ProviderName="<%$ ConnectionStrings:Vantage8RU2.ProviderName %>" SelectCommand="SELECT &quot;EmpID&quot; 
FROM &quot;MFGSYS&quot;.&quot;PUB&quot;.&quot;EmpBasic&quot; WHERE &quot;EmpStatus&quot; = 'A'"></asp:SqlDataSource>-->
        <asp:TextBox ID="txtEmpID" runat="server"></asp:TextBox>
</td>
    <td> <asp:TextBox ID="txtWorkCenter" runat="server"></asp:TextBox></td>             
    <td> <asp:TextBox ID="txtProposedWC" runat="server"></asp:TextBox>   
         <asp:CheckBox ID="chkSizeChange" runat="server" Visible=false Text="Yes"/></td>    
    </tr>
    
    <tr>
    <td class="grey">Cylinder Part Number:</td>
    <td class="grey">Component Part Number:</td>
    <td class="grey">Number of Parts Affected:</td>
    </tr>
    <tr>
    <td>        <asp:TextBox ID="txtCylPartNum" runat="server"></asp:TextBox></td>
    <td>        <asp:TextBox ID="txtComPartNum" runat="server"></asp:TextBox></td>
    <td>        <asp:TextBox ID="txtNumofParts" runat="server"></asp:TextBox></td>
    </tr>
        <tr>
    <td class="grey">Customer:</td>
    <td class="grey">Requested Date of Change:</td>
    <td class="grey">Job Number:</td>
    </tr>
        </tr>
    <tr>
    <td>        <asp:TextBox ID="txtCustomer" runat="server"></asp:TextBox></td>
    <td>        <asp:TextBox ID="txtDateOfChange" runat="server"></asp:TextBox></td>
    <td>        <asp:TextBox ID="txtJobNums" runat="server"></asp:TextBox></td>
    </tr>
    </table>
    
    Requested Deviation or Change:<br />
    <asp:TextBox ID="txtRequest" runat="server" Width=500 Height=100 TextMode=MultiLine></asp:TextBox>
    <br /><br />
    Reason for Deviation or Change:<br />
        <asp:TextBox ID="txtReason" runat="server" Width=500 Height=100 TextMode=MultiLine></asp:TextBox>
    <br /><br />
    <asp:Button ID="btnSubmit" runat="server" Text="Submit" 
        onclick="btnSubmit_Click" />&nbsp;&nbsp;<asp:Button ID="btnClear" runat="server" 
        Text="Clear" onclick="btnClear_Click" /><br /><br /><br />

</div>

</div>

</asp:Content>

