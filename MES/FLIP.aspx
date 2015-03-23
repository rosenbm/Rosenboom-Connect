<%@ Page Language="VB" AutoEventWireup="false" CodeFile="FLIP.aspx.vb" Inherits="MES_FLIP" MasterPageFile="~/MasterPage.master" Title="Rosenboom Portal - Record a FLIP" %>

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
            &nbsp;Inspector: &nbsp; <asp:TextBox ID="txtEmpID" runat="server" MaxLength="5" ></asp:TextBox>
            &nbsp;&nbsp;&nbsp; JobNum:&nbsp; <asp:TextBox ID="txtJobNum" runat="server"></asp:TextBox> <br /><br />
            <asp:Button ID="btnRefreshOps" runat="server" Text="Get Operations"/>&nbsp;&nbsp;&nbsp;
            <asp:Button ID="btnSubmit" runat="server" Text="Submit"/><br /><br />
            <asp:GridView ID="gvwJobOpers" runat="server" DataSourceID="SqlDataSource1" AutoGenerateColumns="False" BackColor="White" 
        BorderColor="#336666" BorderStyle="Double" BorderWidth="3px" CellPadding="4"  GridLines="Horizontal" EnableModelValidation="True" DataKeyNames="OprSeq">
         <FooterStyle BackColor="White" ForeColor="#333333" />
         <PagerStyle BackColor="#336666" ForeColor="White" HorizontalAlign="Center" />
         <SelectedRowStyle CssClass="SelectedRowStyle" BackColor="#E2DED6" 
            Font-Bold="True" ForeColor="#333333" /> 
         <RowStyle BackColor="White" ForeColor="#333333" />
        <Columns>
            <asp:BoundField DataField="PartNum" HeaderText="PartNum" 
                SortExpression="PartNum" />
            <asp:BoundField DataField="ASM" HeaderText="ASM" 
                SortExpression="ASM" />
            <asp:BoundField DataField="Part Desc" HeaderText="Part Desc" 
                SortExpression="Part Desc" />
            <asp:BoundField DataField="OprSeq" HeaderText="OprSeq" 
                SortExpression="OprSeq" ReadOnly="True" />
            <asp:BoundField DataField="OpCode" HeaderText="OpCode" 
                SortExpression="OpCode" />
            <asp:BoundField DataField="OpDesc" HeaderText="OpDesc" SortExpression="OpDesc" />
            <asp:BoundField DataField="RG" HeaderText="RG" SortExpression="RG" />
            <asp:TemplateField HeaderText="First Part">
                <EditItemTemplate>
                    <asp:CheckBox ID="CheckBox1" runat="server" />
                </EditItemTemplate>
                <ItemTemplate>
                    <asp:CheckBox ID="CheckBox1" runat="server" />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Last Part">
                <EditItemTemplate>
                    <asp:CheckBox ID="CheckBox2" runat="server" />
                </EditItemTemplate>
                <ItemTemplate>
                    <asp:CheckBox ID="CheckBox2" runat="server" />
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
         <HeaderStyle BackColor="#336666" Font-Bold="True" ForeColor="White" />
    </asp:GridView>
    <asp:SqlDataSource ID="SqlDataSource1" runat="server" 
        ConnectionString="<%$ ConnectionStrings:Epicor9RU %>" 
        ProviderName="<%$ ConnectionStrings:Epicor9RU.ProviderName %>" SelectCommand=" SELECT &quot;JobAsmbl1&quot;.&quot;PartNum&quot;, &quot;JobOper1&quot;.&quot;AssemblySeq&quot; AS 'ASM', LEFT(&quot;JobAsmbl1&quot;.&quot;Description&quot;, 25) AS 'Part Desc',&quot;JobOper1&quot;.&quot;OprSeq&quot;, &quot;JobOper1&quot;.&quot;OpCode&quot;, &quot;OpMaster1&quot;.&quot;OpDesc&quot;, &quot;JobOpDtl&quot;.&quot;ResourceGrpID&quot; AS 'RG'
 FROM  &quot;MFGSYS&quot;.&quot;PUB&quot;.&quot;JobOper&quot; &quot;JobOper1&quot; 

INNER JOIN &quot;MFGSYS&quot;.&quot;PUB&quot;.&quot;JobAsmbl&quot; &quot;JobAsmbl1&quot; ON (&quot;JobOper1&quot;.&quot;Company&quot;=&quot;JobAsmbl1&quot;.&quot;Company&quot;) AND (&quot;JobOper1&quot;.&quot;JobNum&quot;=&quot;JobAsmbl1&quot;.&quot;JobNum&quot;) AND (&quot;JobOper1&quot;.&quot;AssemblySeq&quot;=&quot;JobAsmbl1&quot;.&quot;AssemblySeq&quot;) 

INNER JOIN &quot;MFGSYS&quot;.&quot;PUB&quot;.&quot;OpMaster&quot; &quot;OpMaster1&quot; ON (&quot;JobOper1&quot;.&quot;Company&quot;=&quot;OpMaster1&quot;.&quot;Company&quot;) AND (&quot;JobOper1&quot;.&quot;OpCode&quot;=&quot;OpMaster1&quot;.&quot;OpCode&quot;)

INNER JOIN &quot;MFGSYS&quot;.&quot;PUB&quot;.&quot;JobOpDtl&quot; ON (&quot;JobOper1&quot;.&quot;Company&quot;=&quot;JobOpDtl&quot;.&quot;Company&quot;) AND (&quot;JobOper1&quot;.&quot;JobNum&quot;=&quot;JobOpDtl&quot;.&quot;JobNum&quot;) AND (&quot;JobOper1&quot;.&quot;AssemblySeq&quot;=&quot;JobOpDtl&quot;.&quot;AssemblySeq&quot;) AND (&quot;JobOper1&quot;.&quot;OprSeq&quot;=&quot;JobOpDtl&quot;.&quot;OprSeq&quot;)

 WHERE  &quot;JobOper1&quot;.&quot;Company&quot;='RMT' AND &quot;JobOper1&quot;.&quot;JobNum&quot;=?">
        <SelectParameters>
            <asp:ControlParameter ControlID="txtJobNum" Name="?" PropertyName="Text" />
        </SelectParameters>
    </asp:SqlDataSource>
    <asp:SqlDataSource ID="SqlDataSource2" runat="server" 
        ConnectionString="<%$ ConnectionStrings:Epicor9RU %>" 
        ProviderName="<%$ ConnectionStrings:Epicor9RU.ProviderName %>" SelectCommand=" SELECT Key5 AS 'Date/Time', Key1 AS 'JobNum', Key2 AS 'Asm', 
        Key3 AS 'OprSeq', Key4 AS 'FLIP Type', Character06 AS 'Inspector'
 FROM   &quot;MFGSYS&quot;.&quot;PUB&quot;.&quot;UD01&quot; 
 WHERE  &quot;UD01&quot;.&quot;Company&quot;='RMT' AND &quot;UD01&quot;.&quot;Key1&quot;=? ORDER BY Date01 DESC">
        <SelectParameters>
            <asp:ControlParameter ControlID="txtJobNum" Name="?" PropertyName="Text" />
        </SelectParameters>
    </asp:SqlDataSource>
            <br /><br />Comments<br />
            <asp:TextBox ID="txtComments" runat="server" TextMode="MultiLine" Width="345px"></asp:TextBox>
            <br /><br />
            Previous Job FLIPs<br />
            <asp:GridView ID="gvwPreviousFLIP" runat="server" EnableModelValidation="True" CellPadding="4" DataSourceID="SqlDataSource2" ForeColor="#333333" GridLines="None" AllowSorting="True" AutoGenerateColumns="False">
                <AlternatingRowStyle BackColor="White" />
                <Columns>
                    <asp:BoundField DataField="Date/Time" HeaderText="Date/Time" ReadOnly="True" SortExpression="Date/Time" />
                    <asp:BoundField DataField="JobNum" HeaderText="JobNum" SortExpression="JobNum" />
                    <asp:BoundField DataField="Asm" HeaderText="Asm" SortExpression="Asm" />
                    <asp:BoundField DataField="OprSeq" HeaderText="OprSeq" SortExpression="OprSeq" />
                    <asp:BoundField DataField="FLIP Type" HeaderText="FLIP Type" SortExpression="FLIP Type" />
                    <asp:BoundField DataField="Inspector" HeaderText="Inspector" SortExpression="Inspector" />
                </Columns>
                <EditRowStyle BackColor="#7C6F57" />
                <FooterStyle BackColor="#1C5E55" Font-Bold="True" ForeColor="White" />
                <HeaderStyle BackColor="#1C5E55" Font-Bold="True" ForeColor="White" />
                <PagerStyle BackColor="#666666" ForeColor="White" HorizontalAlign="Center" />
                <RowStyle BackColor="#E3EAEB" />
                <SelectedRowStyle BackColor="#C5BBAF" Font-Bold="True" ForeColor="#333333" />
            </asp:GridView>
        </div>
    </div>
 </asp:Content>
