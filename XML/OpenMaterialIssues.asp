<%@ Language=VBScript %>
<html>
<head>
<link href="exports.css" rel="stylesheet" type="text/css"><title>Open Material Issues</title>
</head>
<body>
<%
  Dim sortXSL
  sortBy = Request.QueryString("SortBy")

  Dim filterUD02_Date01
  filterUD02_Date01 = Request.QueryString("UD02_Date01")

  Dim filterUD02_Character02
  filterUD02_Character02 = Request.QueryString("UD02_Character02")

  Dim filterUD02_Key2
  filterUD02_Key2 = Request.QueryString("UD02_Key2")

  Dim filterUD02_Key4
  filterUD02_Key4 = Request.QueryString("UD02_Key4")

  Dim filterUD02_Character01
  filterUD02_Character01 = Request.QueryString("UD02_Character01")

  Dim filterUD02_Key3
  filterUD02_Key3 = Request.QueryString("UD02_Key3")

  Dim filterUD02_Key5
  filterUD02_Key5 = Request.QueryString("UD02_Key5")

  Dim filterUD02_Character03
  filterUD02_Character03 = Request.QueryString("UD02_Character03")

  Dim filterUD02_Character04
  filterUD02_Character04 = Request.QueryString("UD02_Character04")

  Dim filterUD02_CheckBox01
  filterUD02_CheckBox01 = Request.QueryString("UD02_CheckBox01")

  Dim filterUD02_Date02
  filterUD02_Date02 = Request.QueryString("UD02_Date02")

%>

<script>
function sortData(sortBy){
  document.location.href = document.location.pathname + "?sortBy=" + sortBy 
           + "&UD02_Date01=" + document.queryForm.filterUD02_Date01.value
           + "&UD02_Character02=" + document.queryForm.filterUD02_Character02.value
           + "&UD02_Key2=" + document.queryForm.filterUD02_Key2.value
           + "&UD02_Key4=" + document.queryForm.filterUD02_Key4.value
           + "&UD02_Character01=" + document.queryForm.filterUD02_Character01.value
           + "&UD02_Key3=" + document.queryForm.filterUD02_Key3.value
           + "&UD02_Key5=" + document.queryForm.filterUD02_Key5.value
           + "&UD02_Character03=" + document.queryForm.filterUD02_Character03.value
           + "&UD02_Character04=" + document.queryForm.filterUD02_Character04.value
           + "&UD02_CheckBox01=" + document.queryForm.filterUD02_CheckBox01.value
           + "&UD02_Date02=" + document.queryForm.filterUD02_Date02.value;
}
</script>
<h2>Open Material Issues</h2>
<form name="queryForm" onsubmit="sortData('<%= sortBy %>'); return false;" >
<table class="searchtable">
  <tr><td class="label">Date Submitted</td><td><input type="text" name="filterUD02_Date01" value="<%= filterUD02_Date01 %>"/></td></tr>
  <tr><td class="label">Requestor</td><td><input type="text" name="filterUD02_Character02" value="<%= filterUD02_Character02 %>"/></td></tr>
  <tr><td class="label">Resource Group</td><td><input type="text" name="filterUD02_Key2" value="<%= filterUD02_Key2 %>"/></td></tr>
  <tr><td class="label">Issue</td><td><input type="text" name="filterUD02_Key4" value="<%= filterUD02_Key4 %>"/></td></tr>
  <tr><td class="label">Info</td><td><input type="text" name="filterUD02_Character01" value="<%= filterUD02_Character01 %>"/></td></tr>
  <tr><td class="label">Supervisor</td><td><input type="text" name="filterUD02_Key3" value="<%= filterUD02_Key3 %>"/></td></tr>
  <tr><td class="label">EmpID</td><td><input type="text" name="filterUD02_Key5" value="<%= filterUD02_Key5 %>"/></td></tr>
  <tr><td class="label">Communicated With</td><td><input type="text" name="filterUD02_Character03" value="<%= filterUD02_Character03 %>"/></td></tr>
  <tr><td class="label">Reason</td><td><input type="text" name="filterUD02_Character04" value="<%= filterUD02_Character04 %>"/></td></tr>
  <tr><td class="label">Complete</td><td><input type="text" name="filterUD02_CheckBox01" value="<%= filterUD02_CheckBox01 %>"/></td></tr>
  <tr><td class="label">Date Completed</td><td><input type="text" name="filterUD02_Date02" value="<%= filterUD02_Date02 %>"/></td></tr>
</table>

<input type="submit" onclick="sortData('<%= sortBy %>'); return false;" value="Search"/>
<br/><br/>
</form>
<%

  Dim xml, xslt, xslFilter, xslSort, xslProc

  'Load XML
  set xml = Server.CreateObject("msxml2.DOMDocument")
  xml.async = false
  xml.load(Server.MapPath("OpenMaterialIssues.xml"))

  'Load XSL Filter
  set xslFilter = Server.CreateObject("msxml2.FreeThreadedDOMDocument")
  xslFilter.async = false
  xslFilter.load(Server.MapPath("filterdata.xsl"))  

  set xslt = Server.CreateObject("msxml2.XSLTemplate")

  'Filter Data
  xslt.stylesheet = xslFilter
  set xslProc = xslt.createProcessor()


  'Filter on UD02_Date01 if it was passed in the query string
  if Request.QueryString("UD02_Date01").Count > 0 and filterUD02_Date01 <> "" then 
    xslProc.input = xml
    xslProc.addParameter "filterNode", "UD02_Date01"
    xslProc.addParameter "filterValue", filterUD02_Date01
    xslProc.addParameter "filterFunction", "StartsWith"
    'Filter to only retain matches
    xslProc.transform()
    xml.loadXML(xslProc.output)
  end if

  'Filter on UD02_Character02 if it was passed in the query string
  if Request.QueryString("UD02_Character02").Count > 0 and filterUD02_Character02 <> "" then 
    xslProc.input = xml
    xslProc.addParameter "filterNode", "UD02_Character02"
    xslProc.addParameter "filterValue", filterUD02_Character02
    xslProc.addParameter "filterFunction", "StartsWith"
    'Filter to only retain matches
    xslProc.transform()
    xml.loadXML(xslProc.output)
  end if

  'Filter on UD02_Key2 if it was passed in the query string
  if Request.QueryString("UD02_Key2").Count > 0 and filterUD02_Key2 <> "" then 
    xslProc.input = xml
    xslProc.addParameter "filterNode", "UD02_Key2"
    xslProc.addParameter "filterValue", filterUD02_Key2
    xslProc.addParameter "filterFunction", "StartsWith"
    'Filter to only retain matches
    xslProc.transform()
    xml.loadXML(xslProc.output)
  end if

  'Filter on UD02_Key4 if it was passed in the query string
  if Request.QueryString("UD02_Key4").Count > 0 and filterUD02_Key4 <> "" then 
    xslProc.input = xml
    xslProc.addParameter "filterNode", "UD02_Key4"
    xslProc.addParameter "filterValue", filterUD02_Key4
    xslProc.addParameter "filterFunction", "StartsWith"
    'Filter to only retain matches
    xslProc.transform()
    xml.loadXML(xslProc.output)
  end if

  'Filter on UD02_Character01 if it was passed in the query string
  if Request.QueryString("UD02_Character01").Count > 0 and filterUD02_Character01 <> "" then 
    xslProc.input = xml
    xslProc.addParameter "filterNode", "UD02_Character01"
    xslProc.addParameter "filterValue", filterUD02_Character01
    xslProc.addParameter "filterFunction", "StartsWith"
    'Filter to only retain matches
    xslProc.transform()
    xml.loadXML(xslProc.output)
  end if

  'Filter on UD02_Key3 if it was passed in the query string
  if Request.QueryString("UD02_Key3").Count > 0 and filterUD02_Key3 <> "" then 
    xslProc.input = xml
    xslProc.addParameter "filterNode", "UD02_Key3"
    xslProc.addParameter "filterValue", filterUD02_Key3
    xslProc.addParameter "filterFunction", "StartsWith"
    'Filter to only retain matches
    xslProc.transform()
    xml.loadXML(xslProc.output)
  end if

  'Filter on UD02_Key5 if it was passed in the query string
  if Request.QueryString("UD02_Key5").Count > 0 and filterUD02_Key5 <> "" then 
    xslProc.input = xml
    xslProc.addParameter "filterNode", "UD02_Key5"
    xslProc.addParameter "filterValue", filterUD02_Key5
    xslProc.addParameter "filterFunction", "StartsWith"
    'Filter to only retain matches
    xslProc.transform()
    xml.loadXML(xslProc.output)
  end if

  'Filter on UD02_Character03 if it was passed in the query string
  if Request.QueryString("UD02_Character03").Count > 0 and filterUD02_Character03 <> "" then 
    xslProc.input = xml
    xslProc.addParameter "filterNode", "UD02_Character03"
    xslProc.addParameter "filterValue", filterUD02_Character03
    xslProc.addParameter "filterFunction", "StartsWith"
    'Filter to only retain matches
    xslProc.transform()
    xml.loadXML(xslProc.output)
  end if

  'Filter on UD02_Character04 if it was passed in the query string
  if Request.QueryString("UD02_Character04").Count > 0 and filterUD02_Character04 <> "" then 
    xslProc.input = xml
    xslProc.addParameter "filterNode", "UD02_Character04"
    xslProc.addParameter "filterValue", filterUD02_Character04
    xslProc.addParameter "filterFunction", "StartsWith"
    'Filter to only retain matches
    xslProc.transform()
    xml.loadXML(xslProc.output)
  end if

  'Filter on UD02_CheckBox01 if it was passed in the query string
  if Request.QueryString("UD02_CheckBox01").Count > 0 and filterUD02_CheckBox01 <> "" then 
    xslProc.input = xml
    xslProc.addParameter "filterNode", "UD02_CheckBox01"
    xslProc.addParameter "filterValue", filterUD02_CheckBox01
    xslProc.addParameter "filterFunction", "StartsWith"
    'Filter to only retain matches
    xslProc.transform()
    xml.loadXML(xslProc.output)
  end if

  'Filter on UD02_Date02 if it was passed in the query string
  if Request.QueryString("UD02_Date02").Count > 0 and filterUD02_Date02 <> "" then 
    xslProc.input = xml
    xslProc.addParameter "filterNode", "UD02_Date02"
    xslProc.addParameter "filterValue", filterUD02_Date02
    xslProc.addParameter "filterFunction", "StartsWith"
    'Filter to only retain matches
    xslProc.transform()
    xml.loadXML(xslProc.output)
  end if

  
  'Sort/Present Data
  set xslSort = Server.CreateObject("msxml2.FreeThreadedDOMDocument")
  xslSort.async = false
  xslSort.load(Server.MapPath("OpenMaterialIssues.xsl"))
  
  xslt.stylesheet = xslSort
  set xslProc = xslt.createProcessor()
  xslProc.input = xml
  
  if Request.QueryString("SortBy").Count > 0 then 
     xslProc.addParameter "sortBy", SortBy
     'This specifies to sort as a number or as text
     if SortBy = "UD02_Date01" then
       xslProc.addParameter "sortAs", "text"
     elseif SortBy = "UD02_Character02" then
       xslProc.addParameter "sortAs", "text"
     elseif SortBy = "UD02_Key2" then
       xslProc.addParameter "sortAs", "text"
     elseif SortBy = "UD02_Key4" then
       xslProc.addParameter "sortAs", "text"
     elseif SortBy = "UD02_Character01" then
       xslProc.addParameter "sortAs", "text"
     elseif SortBy = "UD02_Key3" then
       xslProc.addParameter "sortAs", "text"
     elseif SortBy = "UD02_Key5" then
       xslProc.addParameter "sortAs", "text"
     elseif SortBy = "UD02_Character03" then
       xslProc.addParameter "sortAs", "text"
     elseif SortBy = "UD02_Character04" then
       xslProc.addParameter "sortAs", "text"
     elseif SortBy = "UD02_CheckBox01" then
       xslProc.addParameter "sortAs", "text"
     elseif SortBy = "UD02_Date02" then
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
