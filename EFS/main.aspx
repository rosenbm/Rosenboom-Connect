<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" Title="Rosenboom Connect - EFS" %>

<script runat="server">
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim FeedbackTab As System.Web.UI.HtmlControls.HtmlGenericControl
        FeedbackTab = Master.FindControl("feedbacktab")
        FeedbackTab.Attributes.Add("class", "active")
    End Sub
</script>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<div class="wrapper col3">
    <div id="intro">
    <h2>Employee Feedback</h2>
    <ul>
    <li><a href="materialissues.aspx">Material Issues</a></li>
    <li><a href="processupdates.aspx">Process Updates</a></li></ul>
    </div>
</div>
</asp:Content>

