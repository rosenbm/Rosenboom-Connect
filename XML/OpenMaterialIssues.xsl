<?xml version="1.0" encoding="ISO8859-1"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
<xsl:param name="sortBy" select="UD02_Date01"/>
<xsl:param name="sortAs" select="&quot;text&quot;"/>
<xsl:output omit-xml-declaration="yes"/>

<xsl:decimal-format 
     name="localformat"
     decimal-separator="." 
     grouping-separator=","/>

<xsl:template match="/">
    <table class="datatable" border="1">
    <tr>

    <th class="columnheader"> <a class="columnhdrsortlink" href="javascript:sortData('UD02_Date01');" >Date Submitted</a></th>
    <th class="columnheader"> <a class="columnhdrsortlink" href="javascript:sortData('UD02_Character02');" >Requestor</a></th>
    <th class="columnheader"> <a class="columnhdrsortlink" href="javascript:sortData('UD02_Key2');" >Resource Group</a></th>
    <th class="columnheader"> <a class="columnhdrsortlink" href="javascript:sortData('UD02_Key4');" >Issue</a></th>
    <th class="columnheader"> <a class="columnhdrsortlink" href="javascript:sortData('UD02_Character01');" >Info</a></th>
    <th class="columnheader"> <a class="columnhdrsortlink" href="javascript:sortData('UD02_Key3');" >Supervisor</a></th>
    <th class="columnheader"> <a class="columnhdrsortlink" href="javascript:sortData('UD02_Key5');" >EmpID</a></th>
    <th class="columnheader"> <a class="columnhdrsortlink" href="javascript:sortData('UD02_Character03');" >Communicated With</a></th>
    <th class="columnheader"> <a class="columnhdrsortlink" href="javascript:sortData('UD02_Character04');" >Reason</a></th>
    <th class="columnheader"> <a class="columnhdrsortlink" href="javascript:sortData('UD02_CheckBox01');" >Complete</a></th>
    <th class="columnheader"> <a class="columnhdrsortlink" href="javascript:sortData('UD02_Date02');" >Date Completed</a></th>
    </tr>
    <xsl:for-each select="ExportQuery/RMT_SubmitMtlIssue">
    <xsl:sort select="*[local-name() = $sortBy]" data-type="{$sortAs}"/>
    <tr>
      <td class="datacell" ><xsl:value-of select="UD02_Date01"/><xsl:text disable-output-escaping="yes">&amp;nbsp;</xsl:text></td>
      <td class="datacell" ><xsl:value-of select="UD02_Character02"/><xsl:text disable-output-escaping="yes">&amp;nbsp;</xsl:text></td>
      <td class="datacell" ><xsl:value-of select="UD02_Key2"/><xsl:text disable-output-escaping="yes">&amp;nbsp;</xsl:text></td>
      <td class="datacell" ><xsl:value-of select="UD02_Key4"/><xsl:text disable-output-escaping="yes">&amp;nbsp;</xsl:text></td>
      <td class="datacell" ><xsl:value-of select="UD02_Character01"/><xsl:text disable-output-escaping="yes">&amp;nbsp;</xsl:text></td>
      <td class="datacell" ><xsl:value-of select="UD02_Key3"/><xsl:text disable-output-escaping="yes">&amp;nbsp;</xsl:text></td>
      <td class="datacell" ><xsl:value-of select="UD02_Key5"/><xsl:text disable-output-escaping="yes">&amp;nbsp;</xsl:text></td>
      <td class="datacell" ><xsl:value-of select="UD02_Character03"/><xsl:text disable-output-escaping="yes">&amp;nbsp;</xsl:text></td>
      <td class="datacell" ><xsl:value-of select="UD02_Character04"/><xsl:text disable-output-escaping="yes">&amp;nbsp;</xsl:text></td>
      <td class="datacell" ><xsl:value-of select="UD02_CheckBox01"/><xsl:text disable-output-escaping="yes">&amp;nbsp;</xsl:text></td>
      <td class="datacell" ><xsl:value-of select="UD02_Date02"/><xsl:text disable-output-escaping="yes">&amp;nbsp;</xsl:text></td>
    </tr>
    </xsl:for-each>
    </table>
</xsl:template></xsl:stylesheet>
