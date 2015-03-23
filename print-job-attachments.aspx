<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" Title="Untitled Page" %>

<script runat="server">

</script>



<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true" />
    
    <script type="text/javascript">  
    
    function AddItem(Text,Value)
    {
        // Create an Option object
        var opt = document.createElement("option");
        // Add an Option object to Drop Down/List Box
        document.getElementById("Select1").options.add(opt);
        // Assign text and value to Option object
        opt.text = Text;
        opt.value = Value;
               
    }
    </script>

    <div class="wrapper col3">
        <div id="intro">

        </div>
    </div>

</asp:Content>

