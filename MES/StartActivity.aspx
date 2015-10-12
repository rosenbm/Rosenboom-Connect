<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" Title="Rosenboom Connect - MES" %>


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

