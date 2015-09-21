<%@ Page Language="VB" AutoEventWireup="false" CodeFile="IssueMaterialToJob.aspx.vb" Inherits="MES_NEW_IssueMaterial" MasterPageFile="~/MasterPage.master" Title="Rosenboom Connect - Golden Ticket"%>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

    <div class="wrapper col3">
<div id="intro">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
            <h1>
                Issue Material to Job</h1>
                <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                <ContentTemplate>
                <asp:Label ID="lblMessage" runat="server" Text=""></asp:Label>
                 </ContentTemplate>
                <Triggers>
                <asp:AsyncPostBackTrigger ControlID="btnSubmit" EventName="Click" />
                </Triggers>
                </asp:UpdatePanel>
                
                <div class="col3 golden">
                    
            <table class="golden">
            <tr>
                    <td class="right">
                        *Resource Group (WC)
                    </td>
                    <td>
                        <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                <ContentTemplate>
               <asp:TextBox ID="txtDept" runat="server"></asp:TextBox>
                 </ContentTemplate>
                <Triggers>
                <asp:AsyncPostBackTrigger ControlID="btnSubmit" EventName="Click" />
                </Triggers>
                </asp:UpdatePanel>
                        
                    </td>
                </tr>
                <tr>
                    <td class="right">
                        *Job #
                    </td>
                    <td>
                    <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                <ContentTemplate>
               <asp:TextBox ID="txtJobNum" runat="server"></asp:TextBox>
                 </ContentTemplate>
                <Triggers>
                <asp:AsyncPostBackTrigger ControlID="btnSubmit" EventName="Click" />
                </Triggers>
                </asp:UpdatePanel>
                        
                    </td>
                </tr>
                <tr>
                    <td class="right">
                        *Assembly #
                    </td>
                    <td>
                    <asp:UpdatePanel ID="UpdatePanel6" runat="server">
                <ContentTemplate>
               <asp:TextBox ID="txtAsm" runat="server"></asp:TextBox>
                 </ContentTemplate>
                <Triggers>
                <asp:AsyncPostBackTrigger ControlID="btnSubmit" EventName="Click" />
                </Triggers>
                </asp:UpdatePanel>
                        
                    </td>
                </tr>
                <tr>
                    <td class="right">
                        *Mtl Seq #
                    </td>
                    <td>
                    <asp:UpdatePanel ID="UpdatePanel7" runat="server">
                <ContentTemplate>
               <asp:TextBox ID="txtMtlNum" runat="server"></asp:TextBox>
                 </ContentTemplate>
                <Triggers>
                <asp:AsyncPostBackTrigger ControlID="btnSubmit" EventName="Click" />
                </Triggers>
                </asp:UpdatePanel>
                        
                    </td>
                </tr>
                <tr>
                    <td class="right">
                        *Employee ID
                    </td>
                    <td>
                    <asp:UpdatePanel ID="UpdatePanel8" runat="server">
                <ContentTemplate>
                    
               <asp:TextBox ID="txtEmpID" runat="server"></asp:TextBox>
                 </ContentTemplate>
                <Triggers>
                <asp:AsyncPostBackTrigger ControlID="btnSubmit" EventName="Click" />
                </Triggers>
                </asp:UpdatePanel>
                        
                    </td>
                </tr>
                <tr>
                    <td class="right">
                        *PO #
                    </td>
                    <td>
                    <asp:UpdatePanel ID="UpdatePanel9" runat="server">
                <ContentTemplate>
               <asp:TextBox ID="txtPONum" runat="server"></asp:TextBox>
                 </ContentTemplate>
                <Triggers>
                <asp:AsyncPostBackTrigger ControlID="btnSubmit" EventName="Click" />
                </Triggers>
                </asp:UpdatePanel>
                        
                    </td>
                </tr><tr>
                    <td class="right">
                        *Heat #
                    </td>
                    <td>
                    <asp:UpdatePanel ID="UpdatePanel10" runat="server">
                <ContentTemplate>
               <asp:TextBox ID="txtHeatNum" runat="server"></asp:TextBox>
                 </ContentTemplate>
                <Triggers>
                <asp:AsyncPostBackTrigger ControlID="btnSubmit" EventName="Click" />
                </Triggers>
                </asp:UpdatePanel>
                        
                    </td>
                </tr>
                <tr>
                    <td class="right">
                        From Bin (if not RG/WC)
                    </td>
                    <td>
                    <asp:UpdatePanel ID="UpdatePanel11" runat="server">
                <ContentTemplate>
               <asp:TextBox ID="txtFromBin" runat="server"></asp:TextBox>
                 </ContentTemplate>
                <Triggers>
                <asp:AsyncPostBackTrigger ControlID="btnSubmit" EventName="Click" />
                </Triggers>
                </asp:UpdatePanel>
                        
                    </td>
                </tr>
            </table>
            * Denotes required field
            </div>
            <div class="col3 golden">
                
            <table class="golden">
            <tr><td class="right">Required Qty</td><td>
            <asp:UpdatePanel ID="UpdatePanel12" runat="server">
                <ContentTemplate>
               <asp:TextBox ID="txtEngYield" runat="server"></asp:TextBox>
                 </ContentTemplate>
                <Triggers>
                <asp:AsyncPostBackTrigger ControlID="btnSubmit" EventName="Click" />
                </Triggers>
                </asp:UpdatePanel></td></tr>
            <tr><td class="right">Non-Usable Material</td><td>
            <asp:UpdatePanel ID="UpdatePanel13" runat="server">
                <ContentTemplate>
               <asp:TextBox ID="txtNonUsable" runat="server"></asp:TextBox>
                 </ContentTemplate>
                <Triggers>
                <asp:AsyncPostBackTrigger ControlID="btnSubmit" EventName="Click" />
                </Triggers>
                </asp:UpdatePanel></td></tr>
            <tr><td class="right">Drop</td><td>
            <asp:UpdatePanel ID="UpdatePanel14" runat="server">
                <ContentTemplate>
               <asp:TextBox ID="txtDrop" runat="server"></asp:TextBox>
                 </ContentTemplate>
                <Triggers>
                <asp:AsyncPostBackTrigger ControlID="btnSubmit" EventName="Click" />
                </Triggers>
                </asp:UpdatePanel>
            </td></tr>
            <tr><td class="right">Scrap</td><td>
            <asp:UpdatePanel ID="UpdatePanel15" runat="server">
                <ContentTemplate>
               <asp:TextBox ID="txtScrap" runat="server"></asp:TextBox>
                 </ContentTemplate>
                <Triggers>
                <asp:AsyncPostBackTrigger ControlID="btnSubmit" EventName="Click" />
                </Triggers>
                </asp:UpdatePanel>
            </td></tr>
            <tr><td class="right">Damaged Material</td><td>
            <asp:UpdatePanel ID="UpdatePanel16" runat="server">
                <ContentTemplate>
               <asp:TextBox ID="txtDamagedMtl" runat="server"></asp:TextBox>
                 </ContentTemplate>
                <Triggers>
                <asp:AsyncPostBackTrigger ControlID="btnSubmit" EventName="Click" />
                </Triggers>
                </asp:UpdatePanel>
            </td></tr>
            <%--<tr><td class="right">Material Substitution</td><td>
            <asp:UpdatePanel ID="UpdatePanel17" runat="server">
                <ContentTemplate>
               <asp:TextBox ID="txtMaterialSub" runat="server"></asp:TextBox>
                 </ContentTemplate>
                <Triggers>
                <asp:AsyncPostBackTrigger ControlID="btnSubmit" EventName="Click" />
                </Triggers>
                </asp:UpdatePanel>
            </td></tr>--%>
            <tr><td class="right">Overproduction</td><td>
            <asp:UpdatePanel ID="UpdatePanel18" runat="server">
                <ContentTemplate>
               <asp:TextBox ID="txtOverProduction" runat="server"></asp:TextBox>
                 </ContentTemplate>
                <Triggers>
                <asp:AsyncPostBackTrigger ControlID="btnSubmit" EventName="Click" />
                </Triggers>
                </asp:UpdatePanel>
            </td></tr>
                        <%--<tr><td class="right">Consumed Short Ends</td><td>
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
               <asp:TextBox ID="txtShortEnds" runat="server"></asp:TextBox>
                 </ContentTemplate>
                <Triggers>
                <asp:AsyncPostBackTrigger ControlID="btnSubmit" EventName="Click" />
                </Triggers>
                </asp:UpdatePanel>
            </td></tr>--%>      
            </table>

            <div class="center">
            <asp:CheckBox ID="chkIssuedComplete" runat="server" text="Issued Complete"/>
            <br /><br />

<asp:Button ID="btnSubmit" runat="server" Text="Submit Ticket"  /> &nbsp;&nbsp;
<asp:Button ID="btnCheckIssue" runat="server" Text="Check Qty Issued" style="height: 26px"  />&nbsp;&nbsp;
<asp:Button ID="btnClear" runat="server" Text="Clear" onclick="btnClear_Click" />
                </div>
                                               
                <asp:UpdateProgress ID="UpdateProgress1" runat="server">
                <ProgressTemplate>
                <asp:Image ID="imgLoading" runat="server" ImageUrl="/Images/loading.gif" />
                </ProgressTemplate>
                </asp:UpdateProgress>
                
            </div>
            <div>
                <table>
                    <tr><td>1"</td><td>.08</td></tr>
                    <tr><td>2"</td><td>.17</td></tr>
                    <tr><td>3"</td><td>.25</td></tr>
                    <tr><td>4"</td><td>.33</td></tr>
                    <tr><td>5"</td><td>.42</td></tr>
                    <tr><td>6"</td><td>.50</td></tr>
                    <tr><td>7"</td><td>.58</td></tr>
                    <tr><td>8"</td><td>.67</td></tr>
                    <tr><td>9"</td><td>.75</td></tr>
                    <tr><td>10"</td><td>.83</td></tr>
                    <tr><td>11"</td><td>.92</td></tr>
                </table>
            </div>
    <br />
    <br />
                <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" DataSourceID="SqlDataSource1" EnableModelValidation="True" CellPadding="4" ForeColor="#333333" GridLines="None">
                    <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
        <Columns>
            <asp:BoundField DataField="TranDate" HeaderText="TranDate" SortExpression="TranDate" DataFormatString="{0:d}" />
            <asp:BoundField DataField="PartNum" HeaderText="PartNum" SortExpression="PartNum" />
            <asp:BoundField DataField="TranQty" HeaderText="TranQty" SortExpression="TranQty" DataFormatString="{0:f2}" />
            <asp:BoundField DataField="TranReference" HeaderText="TranReference" SortExpression="TranReference" />
        </Columns>
                    <EditRowStyle BackColor="#999999" />
                    <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                    <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                    <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                    <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                    <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
    </asp:GridView>
                
    <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:Epicor9RU %>" ProviderName="<%$ ConnectionStrings:Epicor9RU.ProviderName %>" SelectCommand="SELECT TranDate, PartNum, TranQty, TranReference
FROM MFGSYS.PUB.PartTran
WHERE Company = 'RMT' AND TranType = 'STK-MTL' AND JobNum = ? AND AssemblySeq = ? AND JobSeq = ?">
        <SelectParameters>
            <asp:ControlParameter ControlID="txtJobNum" Name="?" PropertyName="Text" />
            <asp:ControlParameter ControlID="txtAsm" Name="?" PropertyName="Text" Type="Int32" />
            <asp:ControlParameter ControlID="txtMtlNum" Name="?" PropertyName="Text" Type="Int32" />
        </SelectParameters>
    </asp:SqlDataSource>
    <br />
        <table>
        <tR><td>Reason Code</td><td>Abbreviation</td>           </tR>
        <tr><td>Non-Usable Material</td><td>numtl</td></tr>
        <tr><td>Scrap</td><td>Scrap</td></tr>
        <tr><td>Drop</td><td>Drop</td></tr>
        <tr><td>Damaged Material</td><td>dammtl</td></tr>
        <tr><td>Overproduction</td><td>overpro</td></tr>
        <tr><td>Short Ends</td><td>ser</td></tr>
        <tr><td>Engineering Yield</td><td>PO - HT</td></tr>

    </table>
                <br />
            </div>
            </div>
           
    
</asp:Content>
