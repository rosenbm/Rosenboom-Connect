<?xml version="1.0" encoding="ISO8859-1"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
<xsl:param name="sortBy" select="PartBin_WarehouseCode"/>
<xsl:param name="sortAs" select="&quot;text&quot;"/>
<xsl:output omit-xml-declaration="yes"/>

<xsl:decimal-format 
     name="localformat"
     decimal-separator="." 
     grouping-separator=","/>

<xsl:template match="/">
    <table class="datatable" border="1">
    <tr>

    <th class="columnheader"> <a class="columnhdrsortlink" href="javascript:sortData('PartBin_WarehouseCode');" >Warehouse</a></th>
    <th class="columnheader"> <a class="columnhdrsortlink" href="javascript:sortData('PartBin_BinNum');" >Bin</a></th>
    <th class="columnheader"> <a class="columnhdrsortlink" href="javascript:sortData('PartBin_PartNum');" >Part Number</a></th>
    <th class="columnheadernumeric"> <a class="columnhdrsortlink" href="javascript:sortData('PartBin_OnhandQty');" >On Hand</a></th>
    </tr>
    <xsl:for-each select="ExportQuery/RMT_BinTracker">
    <xsl:sort select="*[local-name() = $sortBy]" data-type="{$sortAs}"/>
    <tr>
      <td class="datacell" ><xsl:value-of select="PartBin_WarehouseCode"/><xsl:text disable-output-escaping="yes">&amp;nbsp;</xsl:text></td>
      <td class="datacell" ><xsl:value-of select="PartBin_BinNum"/><xsl:text disable-output-escaping="yes">&amp;nbsp;</xsl:text></td>
      <td class="datacell" ><xsl:value-of select="PartBin_PartNum"/><xsl:text disable-output-escaping="yes">&amp;nbsp;</xsl:text></td>
      <td class="datacellnumeric" ><xsl:value-of select="format-number(number(PartBin_OnhandQty),'###,###,###,##0.00','localformat')"/><xsl:text disable-output-escaping="yes">&amp;nbsp;</xsl:text></td>
    </tr>
    </xsl:for-each>
    </table>
</xsl:template></xsl:stylesheet>
