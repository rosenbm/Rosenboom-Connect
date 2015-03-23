<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" Title="Rosenboom Connect" %>

<script runat="server">

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim HomeTab As System.Web.UI.HtmlControls.HtmlGenericControl
        HomeTab = Master.FindControl("hometab")
        HomeTab.Attributes.Add("class", "active")
        
    End Sub
</script>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div class="wrapper col3">
  <div id="intro">

      
    <div class="fl_left">
      <h2><%Response.WriteFile("\\pithos\company\Service Connect\Rosenboom Connect\Text Files\Default Page\Main Title.txt")%></h2>
      <p><%Response.WriteFile("\\pithos\company\Service Connect\Rosenboom Connect\Text Files\Default Page\Main Subtext.txt")%></p>
      <p class="readmore"><a href="#">Learn More</a></p>
    </div>
    <div class="fl_right"><img src="images/main.jpg" alt="" /></div>
    <br class="clear" />
  </div>
</div>
<div class="wrapper col4">
  <div id="services">
    <ul>
      <li>
        <div class="fl_left"><img src="images/newspaper.jpg" alt="" /></div>
        <div class="fl_right">
          <h2>Announcements</h2>
          <p><br /><br /><br /><br /></p>
          <p class="readmore"><a href="#">Continue Reading &raquo;</a></p>
        </div>
      </li>
      <li>
        <div class="fl_left"><img src="images/baloons.jpg" alt="" /></div>
        <div class="fl_right">
          <h2>Birthdays and Anniversaries</h2>
          <p><br /><br /><br /></p>
          <p class="readmore"><a href="birthdays.htm">Continue Reading &raquo;</a></p>
        </div>
      </li>
      <li class="last">
        <div class="fl_left"><img src="images/question mark.jpg" alt="" /></div>
        <div class="fl_right">
          <h2>Submit Feedback</h2>
          <p><br /><br /><br /><br /></p>
          <p class="readmore"><a href="/EFS/main.aspx">Submit Feedback &raquo;</a></p>
        </div>
      </li>
    </ul>
    <br class="clear" />
  </div>
</div>
<div class="wrapper col5">
  <div id="container">
    <div id="content">
      <h2>About Rosenboom Connect</h2>
      <p></p>
      <p></p>
      <p></p>
      <p></p>
      <p></p>
    </div>
    <div id="column">
      <div class="holder">
        <h2></h2>
        <ul id="latestnews">
          <li> <img class="imgl" src="images/vantage_2c.gif" alt="" />
            <p><strong><a href="MES/Main.aspx">Vantage MES</a></strong></p>
            <p><br /><br /><br /><br /></p>
          </li>
          <li class="last"> <img class="imgl" src="images/Stack-of-Paper.jpg" alt="" />
            <p><strong><a href="/Reports/Main.aspx">Reports</a></strong></p>
            <p><br /><br /><br /><br /></p>
          </li>
        </ul>
      </div>
    </div>
    <br class="clear" />
  </div>
</div>
</asp:Content>

