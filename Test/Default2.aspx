<%@ Page Language="VB" %>

<%@ Import Namespace="System.IO" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<script runat="server">

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs)

    End Sub
</script>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<script src="http://ajax.googleapis.com/ajax/libs/jquery/1.8.0/jquery.min.js">
</script>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
<div id="msg"> <strong><font color="red">This will blink</font></strong></p> </div>
 
<script type="text/javascript" />
function blink(selector){
$(selector).fadeOut('slow', function(){
    $(this).fadeIn('slow', function(){
        blink(this);
    });
});
}
 
blink('#msg');
</script>
    </form>
</body>
</html>
