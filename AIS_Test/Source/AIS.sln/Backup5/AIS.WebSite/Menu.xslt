<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
  <xsl:output method="xml" indent="yes" encoding="utf-8"/>
  <xsl:template match="/Menus">
    <MenuItems>
      <xsl:call-template name="MenuList" />
    </MenuItems>
  </xsl:template>
  <xsl:template name="MenuList">
    <xsl:apply-templates select="Menu" />
  </xsl:template>
  <xsl:template match="Menu">
    <MenuItem>
      <xsl:attribute name="menu_nm_txt">
        <xsl:value-of select="menu_nm_txt"/>
      </xsl:attribute>
      <xsl:attribute name="web_page_txt">
        <xsl:value-of select="web_page_txt"/>
      </xsl:attribute>
	  <xsl:attribute name="menu_tooltip_txt">
		<xsl:value-of select="menu_tooltip_txt"/>
	  </xsl:attribute>
      <xsl:attribute name="actv_ind">
        <xsl:value-of select="actv_ind"/>
      </xsl:attribute>
      <xsl:if test="count(Menu)>0">
        <xsl:call-template name="MenuList" />
      </xsl:if>
    </MenuItem>
  </xsl:template>
</xsl:stylesheet>
