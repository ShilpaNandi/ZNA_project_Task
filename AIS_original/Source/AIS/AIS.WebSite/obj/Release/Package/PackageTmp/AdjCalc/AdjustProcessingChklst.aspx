<%@ Page Language="C#" MasterPageFile="~/Retro.master" AutoEventWireup="true" Inherits="ZurichNA.AIS.WebSite.AdjCalc.AdjustProcessingChklst"
    Title="AdjustProcessingChklst" CodeBehind="AdjustProcessingChklst.aspx.cs" %>

<%@ Register Src="~/App_Shared/AccountList.ascx" TagPrefix="AL" TagName="AccountList" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="AjaxToolkit" %>
<%@ Register Src="../App_Shared/SaveCancel.ascx" TagName="SaveCancel" TagPrefix="SC" %>
<%@ Register Src="~/App_Shared/AdjustmentReviewSearch.ascx" TagPrefix="ARS" TagName="AdjustmentReviewSearch" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeaderPlaceHolder" runat="Server">
    <table width="910" style="text-align: right">
        <tr>
            <td align="left">
                <asp:Label ID="lblAdjReview" runat="server" Text="Adjustment Review" CssClass="h1"></asp:Label>
            </td>
            <td align="right">
                <div id="UCSave">
                    <SC:SaveCancel ID="ucSaveCancel" runat="server" Visible="true" />
                </div>
            </td>
        </tr>
    </table>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainPlaceHolder" runat="Server">

    <script language="javascript" type="text/javascript">

    var scrollTop1;
    var scrollTop2;

    if (Sys.WebForms.PageRequestManager != null) 
    {
        Sys.WebForms.PageRequestManager.getInstance().add_beginRequest(BeginRequestHandler);
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);
    }
    
    function BeginRequestHandler(sender, args) 
    {
        var mef = $get('<%=pnlAdjChklst.ClientID%>');
        if(mef!=null)
        scrollTop1 = mef.scrollTop;

        var aic = $get('<%=PnlApprovedInvChklist.ClientID%>');
        if(aic!=null)
        scrollTop2 = aic.scrollTop;

    }

    function EndRequestHandler(sender, args)
    {
        var mef = $get('<%=pnlAdjChklst.ClientID%>');
        if(mef!=null)
        mef.scrollTop = scrollTop1;

        var aic = $get('<%=PnlApprovedInvChklist.ClientID%>');
        if(aic!=null)
        aic.scrollTop = scrollTop2;

    }          
     function ActiveTabChanged(sender, e)
    {
      // use the client side active tab changed 
      // event to trigger a callback thereby 
      // triggering the server side event.
      __doPostBack('<%= TabContainerAdjChecklist.ClientID %>', sender.get_activeTab().get_headerText());
    }
        function HideButtons()
        {
        $get('UCSave').style.display="none";
        
        }
        function ShowButtons()
        {
        $get('UCSave').style.display="block";
        
        }
        function Tabnavigation(Pagename)
        {
            var selectedValues= $get('<%=hidSelectedValues.ClientID%>');
            var strURL="../AdjCalc/";
            if(Pagename=="ARC")
            {
                strURL +="AdjustmentReview.aspx";
            }
            else if(Pagename=="ARPLB")
            {
                strURL +="PaidLossBilling.aspx";
            }
            else if(Pagename=="AREA")
            {
                strURL +="EscrowAdjustment.aspx";
            }
            else if(Pagename=="ARCE")
            {
                strURL +="CombinedElements.aspx";
            }
            else if(Pagename=="ARNYSIF")
            { 
            strURL +="SurchargeAssesmentReview.aspx";
              
            }
            else if(Pagename=="ARMI")
            {
                strURL +="MiscInvoicing.aspx";
            }
 	        else if(Pagename=="ARLRFP")
            {
                strURL +="LRFPostingDetails.aspx";
            }
            else if(Pagename=="ARAPCL")
            {
                strURL +="AdjustProcessingChklst.aspx";
            }
            else if(Pagename=="ARANTM")
            {
                strURL +="AdjustmentNumberTextMaintenance.aspx";
            }
            else if (Pagename == "ARBB") 
            {
                strURL += "BuBrokerReview.aspx";
            }
            if(selectedValues.value!="")
            {
                strURL += "?SelectedValues=" + selectedValues.value + "&wID=<%= WindowName%>";
            }
            else {
                strURL += "?wID=<%= WindowName%>";
            }
            window.location.href=strURL;
        } 
    </script>

    <AjaxToolkit:TabContainer ID="TabContainer1" runat="server" CssClass="VariableTabs"
        SkinID="tabVariable" ActiveTabIndex="7">
        <AjaxToolkit:TabPanel runat="server" ID="tblpnlLBA">
            <HeaderTemplate>
                <div onclick="Tabnavigation('ARC')">
                    Review 
                </div>
            </HeaderTemplate>
            <ContentTemplate>
            </ContentTemplate>
        </AjaxToolkit:TabPanel>
        <AjaxToolkit:TabPanel runat="server" ID="tblpnlLCF">
            <HeaderTemplate>
                <div onclick="Tabnavigation('ARPLB')">
                    Adj. PLB
                </div>
            </HeaderTemplate>
            <ContentTemplate>
            </ContentTemplate>
        </AjaxToolkit:TabPanel>
        <AjaxToolkit:TabPanel runat="server" ID="tblpnlTM">
            <HeaderTemplate>
                <div onclick="Tabnavigation('AREA')">
                    Loss Fund Adj.
                </div>
            </HeaderTemplate>
            <ContentTemplate>
            </ContentTemplate>
        </AjaxToolkit:TabPanel>
        <AjaxToolkit:TabPanel runat="server" ID="tblpnlCE">
            <HeaderTemplate>
                <div onclick="Tabnavigation('ARCE')">
                    Comb.Elements
                </div>
            </HeaderTemplate>
            <ContentTemplate>
            </ContentTemplate>
        </AjaxToolkit:TabPanel>
        <AjaxToolkit:TabPanel runat="server" ID="tblpnlNYSIF">
            <HeaderTemplate>
                <div onclick="Tabnavigation('ARNYSIF')">
                   Surch & Assmt
                </div>
            </HeaderTemplate>
            <ContentTemplate>
            </ContentTemplate>
        </AjaxToolkit:TabPanel>
        <AjaxToolkit:TabPanel runat="server" ID="tblpnlmscInvL">
            <HeaderTemplate>
                <div onclick="Tabnavigation('ARMI')">
                    Misc.Invoicing
                </div>
            </HeaderTemplate>
            <ContentTemplate>
            </ContentTemplate>
        </AjaxToolkit:TabPanel>
        <AjaxToolkit:TabPanel runat="server" ID="tblpnlLRF">
            <HeaderTemplate>
                <div onclick="Tabnavigation('ARLRFP')">
                    LRF Posting
                </div>
            </HeaderTemplate>
            <ContentTemplate>
            </ContentTemplate>
        </AjaxToolkit:TabPanel>
        <AjaxToolkit:TabPanel runat="server" ID="tblpnlAdjchklist">
            <HeaderTemplate>
               Adj.Checklist
            </HeaderTemplate>
            <ContentTemplate>
                <table>
                    <tr>
                        <td style="height: 8px">
                        </td>
                    </tr>
                </table>
                <asp:ObjectDataSource ID="odsAdjNumber" runat="server" SelectMethod="GetAdjNumberSearch"
                    TypeName="ZurichNA.AIS.Business.Logic.PremAdjustmentBS">
                    <SelectParameters>
                        <asp:Parameter Name="straccountID" Type="String" />
                        <asp:Parameter Name="strValDate" Type="String" />
                        <asp:Parameter Name="intPremAdjPgmID" Type="Int32" />
                    </SelectParameters>
                </asp:ObjectDataSource>
                <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                    <ContentTemplate>
                        <asp:Panel ID="Panel1" BorderColor="Black" BorderWidth="1px" Width="60%" runat="server"
                            class="panelExtContents">
                            <table width="100%" border="0" align="center" cellpadding="2" cellspacing="1">
                                <tr style="background-color: #608CC8; color: White">
                                    <td width="26%" height="20" align="center" valign="top">
                                        <asp:Label ID="Label1" Font-Bold="true" Font-Size="Small" runat="server" Text="Please make selection" style="font-family:Verdana;font-size:11px"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                        <asp:Panel ID="Panel2" runat="server" BorderColor="Black" BorderWidth="1px" Width="60%">
                            <table width="100%" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td>
                                        <ARS:AdjustmentReviewSearch ID="ARS" runat="server" />
                                         <asp:HiddenField ID="hidSelectedValues" runat="server"  />
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                        <table width="100%" cellpadding="0">
                           <%-- <tr>
                                <td>
                                    <table width="910" style="text-align: right">
                                        <tr>
                                            <td align="left">
                                                <asp:Label ID="lblAdjProcChklst" Visible="false" runat="server" CssClass="h3" Text="Adjustment Processing Checklist" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>--%>
                            <tr>
                                <td>
                                <br />
                                <ajaxToolkit:TabContainer ID="TabContainerAdjChecklist" runat="server" 
                            CssClass="VariableTabs" SkinID="tabVariable">
                            <ajaxToolkit:TabPanel runat="server" ID="tplAdjProcessCheckListInfo">
                            <HeaderTemplate>
                             Calculation Checklist
                                </HeaderTemplate>
                                <ContentTemplate>
                                    <asp:Panel ID="pnlAdjChklst"  Visible="false" runat="server" Height="240px" ScrollBars="Auto" Width="910px">
                                        <asp:AISListView ID="lstAdjProcChklst" runat="server" OnItemDataBound="lstAdjProcChklst_ItemDataBound">
                                            <EmptyDataTemplate>
                                                <table id="Table1" class="panelContents" runat="server" width="98%">
                                                    <tr class="LayoutTemplate">
                                                        <th style="width: 75%">
                                                            Calculation Checklist
                                                        </th>
                                                        <th style="width: 25%">
                                                            Select
                                                        </th>
                                                    </tr>
                                                </table>
                                                <table width="98%">
                                                    <tr id="Tr1" runat="server" class="ItemTemplate">
                                                        <td align="center">
                                                            <asp:Label ID="lblEmptyMessage" Text="---No Records Found ---" Font-Bold="true" Width="600px"
                                                                runat="server" Style="text-align: center" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </EmptyDataTemplate>
                                            <LayoutTemplate>
                                                <table id="Table1" class="panelContents" runat="server" width="98%">
                                                    <tr class="LayoutTemplate">
                                                        <th style="width: 75%">
                                                            Calculation Checklist
                                                        </th>
                                                        <th style="width: 25%">
                                                            Select
                                                        </th>
                                                    </tr>
                                                    <tr id="itemPlaceholder" runat="server">
                                                    </tr>
                                                </table>
                                            </LayoutTemplate>
                                            <ItemTemplate>
                                                <tr id="Tr1" runat="server" class="ItemTemplate">
                                                    <td align="left">
                                                        <asp:Label ID="lblhidQualitycntrl" runat="server" Visible="false" Text='<% # Bind("LOOKUPID")%>'></asp:Label>
                                                        
                                                        <asp:Label ID="lblIssue_txt" runat="server" Visible="true" Text='<% # Bind("AdjChkLstItems")%>'></asp:Label>
                                                    </td>
                                                    <td>
                                                     <asp:Label ID="lblchklistcd" runat="server" Text='<% # Bind("CHKLIST_STS_CD")%>' Visible="false"></asp:Label>
                                                        <asp:CheckBox ID="chkSelectAdjProc" runat="server" GroupName="Select" Checked='<% # Bind("ACTIVE")%>' Visible="false" />
                                                     <asp:DropDownList ID="ddlSelectAcctProc" runat="server" AutoPostBack="false" Width="100px" >
                                                                 <asp:ListItem Text="(Select)" Value="0"></asp:ListItem>
                                                                 <asp:ListItem Text="Yes" Value="Yes"></asp:ListItem>
                                                                 <asp:ListItem Text="No" Value="No" Selected="True"></asp:ListItem>
                                                                 <asp:ListItem Text="N/A" Value="N/A"></asp:ListItem>
                                                                  </asp:DropDownList>
                                                    </td>
                                                </tr>
                                            </ItemTemplate>
                                            <AlternatingItemTemplate>
                                                <tr id="Tr2" runat="server" class="AlternatingItemTemplate">
                                                    <td align="left">
                                                        <asp:Label ID="lblhidQualitycntrl" runat="server" Visible="false" Text='<% # Bind("LOOKUPID")%>'></asp:Label>
                                                        <asp:Label ID="lblIssue_txt" runat="server" Visible="true" Text='<% # Bind("AdjChkLstItems")%>'></asp:Label>
                                                    </td>
                                                    <td>
                                                     <asp:Label ID="lblchklistcd" runat="server" Text='<% # Bind("CHKLIST_STS_CD")%>' Visible="false"></asp:Label>
                                                        <asp:CheckBox ID="chkSelectAdjProc" GroupName="Select" Checked='<% # Bind("ACTIVE")%>'
                                                            runat="server" Visible="false"/>
                                                             <asp:DropDownList ID="ddlSelectAcctProc" runat="server" AutoPostBack="false" Width="100px" >
                                                                 <asp:ListItem Text="(Select)" Value="0"></asp:ListItem>
                                                                 <asp:ListItem Text="Yes" Value="Yes"></asp:ListItem>
                                                                 <asp:ListItem Text="No" Value="No" Selected="True"></asp:ListItem>
                                                                 <asp:ListItem Text="N/A" Value="N/A"></asp:ListItem>
                                                                  </asp:DropDownList>
                                                    </td>
                                                </tr>
                                            </AlternatingItemTemplate>
                                        </asp:AISListView>
                                    </asp:Panel>
                                    </ContentTemplate>
                                 </ajaxToolkit:TabPanel>
                                 <ajaxToolkit:TabPanel runat="server" ID="tplAdjustmentQCInfo">
                                <HeaderTemplate>
                                    Adjustment Approval
                                </HeaderTemplate>
                                <ContentTemplate>
                                <asp:Panel ID="PnlApprovedInvChklist"  Visible="false" runat="server" Height="240px" ScrollBars="Auto" Width="910px">
                                        <asp:AISListView ID="lstApprovedInvChklist" runat="server">
                                            <EmptyDataTemplate>
                                                <table id="Table1" class="panelContents" runat="server" width="98%">
                                                    <tr class="LayoutTemplate">
                                                        <th style="width: 75%">
                                                            Adjustment Approval
                                                        </th>
                                                        <th style="width: 25%">
                                                            Select
                                                        </th>
                                                    </tr>
                                                </table>
                                                <table width="98%">
                                                    <tr id="Tr1" runat="server" class="ItemTemplate">
                                                        <td align="center">
                                                            <asp:Label ID="lblEmptyMessage" Text="---No Records Found ---" Font-Bold="true" Width="600px"
                                                                runat="server" Style="text-align: center" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </EmptyDataTemplate>
                                            <LayoutTemplate>
                                                <table id="Table1" class="panelContents" runat="server" width="98%">
                                                    <tr class="LayoutTemplate">
                                                        <th style="width: 75%">
                                                            Adjustment Approval
                                                        </th>
                                                        <th style="width: 25%">
                                                            Select
                                                        </th>
                                                    </tr>
                                                    <tr id="itemPlaceholder" runat="server">
                                                    </tr>
                                                </table>
                                            </LayoutTemplate>
                                            <ItemTemplate>
                                                <tr id="Tr1" runat="server" class="ItemTemplate">
                                                    <td align="left">
                                                        <asp:Label ID="lblhidQualitycntrl" runat="server" Visible="false" Text='<% # Bind("LOOKUPID")%>'></asp:Label>
                                                        <%# Eval("AdjChkLstItems")%>
                                                    </td>
                                                    <td>
                                                     <asp:Label ID="lblchklistcd" runat="server" Text='<% # Bind("CHKLIST_STS_CD")%>' Visible="false"></asp:Label>
                                                        <asp:CheckBox ID="chkSelectAdjProc" runat="server" GroupName="Select" Checked='<% # Bind("ACTIVE")%>' Visible="true"/>
                                                    <%--<asp:DropDownList ID="ddlSelectAcctProc" runat="server" AutoPostBack="false" Width="100px" >
                                                                 <asp:ListItem Text="(Select)" Value="0"></asp:ListItem>
                                                                 <asp:ListItem Text="Yes" Value="Yes"></asp:ListItem>
                                                                 <asp:ListItem Text="No " Value="No "></asp:ListItem>
                                                                 <asp:ListItem Text="N/A" Value="N/A"></asp:ListItem>
                                                                  </asp:DropDownList>--%>
                                                    </td>
                                                </tr>
                                            </ItemTemplate>
                                            <AlternatingItemTemplate>
                                                <tr id="Tr2" runat="server" class="AlternatingItemTemplate">
                                                    <td align="left">
                                                        <asp:Label ID="lblhidQualitycntrl" runat="server" Visible="false" Text='<% # Bind("LOOKUPID")%>'></asp:Label>
                                                        <%# Eval("AdjChkLstItems")%>
                                                    </td>
                                                    <td>
                                                     <asp:Label ID="lblchklistcd" runat="server" Text='<% # Bind("CHKLIST_STS_CD")%>' Visible="false"></asp:Label>
                                                        <asp:CheckBox ID="chkSelectAdjProc" GroupName="Select" Checked='<% # Bind("ACTIVE")%>'
                                                            runat="server" Visible="true"/>
                                                         <%--<asp:DropDownList ID="ddlSelectAcctProc" runat="server" AutoPostBack="false" Width="100px" >
                                                                 <asp:ListItem Text="(Select)" Value="0"></asp:ListItem>
                                                                 <asp:ListItem Text="Yes" Value="Yes"></asp:ListItem>
                                                                 <asp:ListItem Text="No " Value="No "></asp:ListItem>
                                                                 <asp:ListItem Text="N/A" Value="N/A"></asp:ListItem>
                                                                  </asp:DropDownList>--%>
                                                    </td>
                                                </tr>
                                            </AlternatingItemTemplate>
                                        </asp:AISListView>
                                    </asp:Panel>
                                </ContentTemplate>
                                </ajaxToolkit:TabPanel>
                                    </ajaxToolkit:TabContainer>
                                     <triggers>
      <asp:AsyncPostBackTrigger ControlID="TabContainerAdjChecklist" 
            EventName="ActiveTabChanged"  />
    </triggers>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                </td>
                            </tr>
                        </table>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </ContentTemplate>
        </AjaxToolkit:TabPanel>
        <AjaxToolkit:TabPanel runat="server" ID="tblpnlAdjNumberText">
                        <HeaderTemplate>
                       
                            <div onclick="Tabnavigation('ARANTM')">
                                Adj.Number 
                            </div>
                        </HeaderTemplate>
                        <ContentTemplate>
                        </ContentTemplate>
                    </AjaxToolkit:TabPanel>
                    <AjaxToolkit:TabPanel runat="server" ID="tblpnlBuBroker">
                        <HeaderTemplate>
                            <div onclick="Tabnavigation('ARBB')">
                                BU Broker
                            </div>
                        </HeaderTemplate>
                        <ContentTemplate>
                        </ContentTemplate>
                    </AjaxToolkit:TabPanel>
    </AjaxToolkit:TabContainer>
    <%-- <asp:ValidationSummary ID="ValSumSave" ValidationGroup="Save" CssClass="ValidationSummary"
        runat="server"></asp:ValidationSummary>
    <asp:ValidationSummary ID="ValSumSaveDetail" CssClass="ValidationSummary" ValidationGroup="QCDetailSave"
        runat="server"></asp:ValidationSummary>--%>
</asp:Content>
