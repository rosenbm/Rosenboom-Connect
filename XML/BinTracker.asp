<%@ Language=VBScript %>
<html>
<head>
<link href="exports.css" rel="stylesheet" type="text/css"><title>RMT Bin Tracker</title>
</head>
<body>
<%
  Dim sortXSL
  sortBy = Request.QueryString("SortBy")

  Dim filterPartBin_WarehouseCode
  filterPartBin_WarehouseCode = Request.QueryString("PartBin_WarehouseCode")

  Dim filterPartBin_BinNum
  filterPartBin_BinNum = Request.QueryString("PartBin_BinNum")

  Dim filterPartBin_PartNum
  filterPartBin_PartNum = Request.QueryString("PartBin_PartNum")

%>

<script>
function sortData(sortBy){
  document.location.href = document.location.pathname + "?sortBy=" + sortBy 
           + "&PartBin_WarehouseCode=" + document.queryForm.filterPartBin_WarehouseCode.value
           + "&PartBin_BinNum=" + document.queryForm.filterPartBin_BinNum.value
           + "&PartBin_PartNum=" + document.queryForm.filterPartBin_PartNum.value;
}
</script>
<h2>RMT Bin Tracker</h2>
<form name="queryForm" onsubmit="sortData('<%= sortBy %>'); return false;" >
<table class="searchtable">
  <tr><td class="label">Warehouse</td><td><input type="text" name="filterPartBin_WarehouseCode" value="<%= filterPartBin_WarehouseCode %>"/></td></tr>
  <tr><td class="label">Bin</td><td><input type="text" name="filterPartBin_BinNum" value="<%= filterPartBin_BinNum %>"/></td></tr>
  <tr><td class="label">Part Number</td><td><input type="text" name="filterPartBin_PartNum" value="<%= filterPartBin_PartNum %>"/></td></tr>
</table>

<input type="submit" onclick="sortData('<%= sortBy %>'); return false;" value="Search"/>
<br/><br/>
</form>
<%

  Dim xml, xslt, xslFilter, xslSort, xslProc

  'Load XML
  set xml = Server.CreateObject("msxml2.DOMDocument")
  xml.async = false
  xml.load(Server.MapPath("BinTracker.xml"))

  'Load XSL Filter
  set xslFilter = Server.CreateObject("msxml2.FreeThreadedDOMDocument")
  xslFilter.async = false
  xslFilter.load(Server.MapPath("filterdata.xsl"))  

  set xslt = Server.CreateObject("msxml2.XSLTemplate")

  'Filter Data
  xslt.stylesheet = xslFilter
  set xslProc = xslt.createProcessor()


  'Filter on PartBin_WarehouseCode if it was passed in the query string
  if Request.QueryString("PartBin_WarehouseCode").Count > 0 and filterPartBin_WarehouseCode <> "" then 
    xslProc.input = xml
    xslProc.addParameter "filterNode", "PartBin_WarehouseCode"
    xslProc.addParameter "filterValue", filterPartBin_WarehouseCode
    xslProc.addParameter "filterFunction", "StartsWith"
    'Filter to only retain matches
    xslProc.transform()
    xml.loadXML(xslProc.output)
  end if

  'Filter on PartBin_BinNum if it was passed in the query string
  if Request.QueryString("PartBin_BinNum").Count > 0 and filterPartBin_BinNum <> "" then 
    xslProc.input = xml
    xslProc.addParameter "filterNode", "PartBin_BinNum"
    xslProc.addParameter "filterValue", filterPartBin_BinNum
    xslProc.addParameter "filterFunction", "StartsWith"
    'Filter to only retain matches
    xslProc.transform()
    xml.loadXML(xslProc.output)
  end if

  'Filter on PartBin_PartNum if it was passed in the query string
  if Request.QueryString("PartBin_PartNum").Count > 0 and filterPartBin_PartNum <> "" then 
    xslProc.input = xml
    xslProc.addParameter "filterNode", "PartBin_PartNum"
    xslProc.addParameter "filterValue", filterPartBin_PartNum
    xslProc.addParameter "filterFunction", "StartsWith"
    'Filter to only retain matches
    xslProc.transform()
    xml.loadXML(xslProc.output)
  end if

  
  'Sort/Present Data
  set xslSort = Server.CreateObject("msxml2.FreeThreadedDOMDocument")
  xslSort.async = false
  xslSort.load(Server.MapPath("BinTracker.xsl"))
  
  xslt.stylesheet = xslSort
  set xslProc = xslt.createProcessor()
  xslProc.input = xml
  
  if Request.QueryString("SortBy").Count > 0 then 
     xslProc.addParameter "sortBy", SortBy
     'This specifies to sort as a number or as text
     if SortBy = "PartBin_WarehouseCode" then
       xslProc.addParameter "sortAs", "text"
     elseif SortBy = "PartBin_BinNum" then
       xslProc.addParameter "sortAs", "text"
     elseif SortBy = "PartBin_PartNum" then
       xslProc.addParameter "sortAs", "text"
     else
       xslProc.addParameter "sortAs", "text"
     end if
  end if

  'Transform the XML into HTML via XSL
  xslProc.transform()

  'Output the results
  Response.Write(xslProc.output)

  'Clean up variables
  Set xml = Nothing 
  Set xslt = Nothing 
  Set xslFilter = Nothing 
  Set xslSort = Nothing 
  Set xslProc = Nothing 

%>

</body>
</html>
