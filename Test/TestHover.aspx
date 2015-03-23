<%@ Page Language="VB" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<script runat="server">

</script>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Untitled Page</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <img src="../images/blue_file.gif" id="bigPic" width="320" height="240"
        alt="" />

<a href="#"><img src="../images/blue_file.gif"
            onmouseover="document.images['bigPic'].src='../images/black_file.gif';"
            width="72" height="54" alt="Item ksc1" 
            onmouseout="document.images['bigPic'].src='../images/blue_file.gif';" /></a>

    </div>
    </form>
</body>
</html>
