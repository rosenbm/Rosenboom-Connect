<?xml version="1.0" encoding="ISO-8859-1"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
	xmlns:ms="urn:schemas-microsoft-com:xslt">
  <xsl:param name="filterNode" select="'CustID'"/>
  <xsl:param name="filterValue" select="''"/>
  <xsl:param name="filterFunction" select="'StartsWith'"/>
  <xsl:template match="/">
    <xsl:for-each select="*">
      <xsl:copy>
        <xsl:for-each select="*">
	        <xsl:for-each select="*[local-name() = $filterNode]">
			  <xsl:choose>
			     <xsl:when test="$filterFunction='GreaterThanOrEqual'">
			       <xsl:call-template name="GreaterThanOrEqual"/>
			     </xsl:when>
			     <xsl:when test="$filterFunction='LessThanOrEqual'">
			       <xsl:call-template name="LessThanOrEqual"/>
			     </xsl:when>
			     <xsl:when test="$filterFunction='GreaterThan'">
			       <xsl:call-template name="GreaterThan"/>
			     </xsl:when>
			     <xsl:when test="$filterFunction='LessThan'">
			       <xsl:call-template name="LessThan"/>
			     </xsl:when>
			     <xsl:when test="$filterFunction='Equals'">
			       <xsl:call-template name="Equals"/>
			     </xsl:when>
			     <xsl:when test="$filterFunction='StartsWith'">
			       <xsl:call-template name="StartsWith"/>
			     </xsl:when>
			     <xsl:when test="$filterFunction='AsNumber'">
			       <xsl:call-template name="AsNumber"/>
			     </xsl:when>
			     <xsl:otherwise>
			       <xsl:copy-of select=".."></xsl:copy-of>
			     </xsl:otherwise>
			  </xsl:choose>	  
	      	</xsl:for-each>
        </xsl:for-each>
      </xsl:copy>
    </xsl:for-each>
  </xsl:template>
  
  <xsl:template name="GreaterThanOrEqual">
      <xsl:if test="''=$filterValue or number(current())=number($filterValue) or number(current())&gt;number($filterValue)">
        <xsl:copy-of select=".."></xsl:copy-of>
      </xsl:if>
  </xsl:template>
  <xsl:template name="LessThanOrEqual">
      <xsl:if test="number(current())=number($filterValue) or number(current())&lt;number($filterValue)">
        <xsl:copy-of select=".."></xsl:copy-of>
      </xsl:if>
  </xsl:template>
  <xsl:template name="GreaterThan">
      <xsl:if test="''=$filterValue or number(current())&gt;number($filterValue)">
        <xsl:copy-of select=".."></xsl:copy-of>
      </xsl:if>
  </xsl:template>
  <xsl:template name="LessThan">
      <xsl:if test="number(current())&lt;number($filterValue)">
        <xsl:copy-of select=".."></xsl:copy-of>
      </xsl:if>
  </xsl:template>
  <xsl:template name="Equals">
        <xsl:if test="translate(current(),'abcdefghijklmnopqrstuvwxyz','ABCDEFGHIJKLMNOPQRSTUVWXYZ')
        	      = translate($filterValue,'abcdefghijklmnopqrstuvwxyz','ABCDEFGHIJKLMNOPQRSTUVWXYZ')">
          <xsl:copy-of select=".."></xsl:copy-of>
        </xsl:if>
  </xsl:template>
  <xsl:template name="StartsWith">
    <xsl:if test="''=$filterValue or starts-with(translate(current()
                   				,'abcdefghijklmnopqrstuvwxyz'
                   				,'ABCDEFGHIJKLMNOPQRSTUVWXYZ'),
                   				translate($filterValue
                   				,'abcdefghijklmnopqrstuvwxyz'
                   				,'ABCDEFGHIJKLMNOPQRSTUVWXYZ'))">
      <xsl:copy-of select=".."></xsl:copy-of>
    </xsl:if>
  </xsl:template>
  <xsl:template name="AsNumber">
    
     <xsl:for-each select="..">
       <xsl:copy>
       <xsl:for-each select="*">
    	<xsl:if test="local-name() = $filterNode">
      		<xsl:copy>
      		    <!--<xsl:value-of select="translate(current(),',$','')"/>-->
      			<xsl:call-template name="string-replace">
          			<xsl:with-param name="string" select="current()"/>
          			<xsl:with-param name="from" select="','"/>
        			<xsl:with-param name="to" select="''"/>
        		</xsl:call-template>
      		</xsl:copy>
    	</xsl:if>
    	<xsl:if test="local-name() != $filterNode">
      		<xsl:copy-of select="."></xsl:copy-of>
    	</xsl:if>
    	</xsl:for-each>
    	</xsl:copy>
     </xsl:for-each>
    
  </xsl:template>
  
  <!-- replace all occurences of the character(s) `from'
     by the string `to' in the string `string'.-->
  <xsl:template name="string-replace" >
    <xsl:param name="string"/>
    <xsl:param name="from"/>
    <xsl:param name="to"/>
    <xsl:choose>
      <xsl:when test="contains($string,$from)">
        <xsl:value-of select="substring-before($string,$from)"/>
        <xsl:value-of select="$to"/>
        <xsl:call-template name="string-replace">
          <xsl:with-param name="string" 
           select="substring-after($string,$from)"/>
          <xsl:with-param name="from" select="$from"/>
        <xsl:with-param name="to" select="$to"/>
        </xsl:call-template>
      </xsl:when>
      <xsl:otherwise>
        <xsl:value-of select="$string"/>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>


  
</xsl:stylesheet>

