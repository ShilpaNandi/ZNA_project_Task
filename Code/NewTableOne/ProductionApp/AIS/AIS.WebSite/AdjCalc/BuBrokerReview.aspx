<%@ Page Title="Bu Broker Change" Language="C#" MasterPageFile="~/Retro.master" AutoEventWireup="true"
    CodeBehind="BuBrokerReview.aspx.cs" Inherits="BuBrokerReview" EnableViewStateMac="true"
    Buffer="true" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/App_Shared/AdjustmentReviewSearch.ascx" TagPrefix="ARS" TagName="AdjustmentReviewSearch" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeaderPlaceHolder" runat="server">
    <asp:Label ID="lblAdjReview" runat="server" Text="Adjustment Review" CssClass="h1"></asp:Label>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainPlaceHolder" runat="server">

    <script language="javascript" type="text/javascript">
        //Function to Redirect to differenet Tabs

        function Tabnavigation(Pagename) {
            var selectedValues = $get('<%=hidSelectedValues.ClientID%>');

            var strURL = "../AdjCalc/";
            if (Pagename == "ARC") {
                strURL += "AdjustmentReview.aspx";
            }
            else if (Pagename == "ARPLB") {
                strURL += "PaidLossBilling.aspx";
            }
            else if (Pagename == "AREA") {
                strURL += "EscrowAdjustment.aspx";
            }
            else if (Pagename == "ARCE") {
                strURL += "CombinedElements.aspx";
            }
            else if (Pagename == "ARNYSIF") {
                strURL += "NY-SIF.aspx";
            }
            else if (Pagename == "ARMI") {
                strURL += "MiscInvoicing.aspx";
            }
            else if (Pagename == "ARLRFP") {
                strURL += "LRFPostingDetails.aspx";
            }
            else if (Pagename == "ARAPCL") {
                strURL += "AdjustProcessingChklst.aspx";
            }
            else if (Pagename == "ARANTM") {
                strURL += "AdjustmentNumberTextMaintenance.aspx";
            }
            if (selectedValues.value != "") {
                strURL += "?SelectedValues=" + selectedValues.value;
            }
            window.location.href = strURL;
        }  

  
    </script>

    <asp:ValidationSummary ID="VSSaveAdjNumber" runat="server" ValidationGroup="AdjNumberGroup"
        Height="40px" CssClass="ValidationSummary" />
    <table>
        <tr>
            <td>
                <cc1:TabContainer ID="TabContainer1" runat="server" CssClass="VariableTabs" SkinID="tabVariable"
                    ActiveTabIndex="9">
                    <cc1:TabPanel runat="server" ID="tblpnlLBA">
                        <HeaderTemplate>
                            <div onclick="Tabnavigation('ARC')">
                                Review Comments
                            </div>
                        </HeaderTemplate>
                        <ContentTemplate>
                        </ContentTemplate>
                    </cc1:TabPanel>
                    <cc1:TabPanel runat="server" ID="tblpnlLCF">
                        <HeaderTemplate>
                            <div onclick="Tabnavigation('ARPLB')">
                                Adj. PLB
                            </div>
                        </HeaderTemplate>
                        <ContentTemplate>
                        </ContentTemplate>
                    </cc1:TabPanel>
                    <!--Start of  Escrow Adjustment Tab Section-->
                    <cc1:TabPanel runat="server" ID="tblpnlTM">
                        <HeaderTemplate>
                            Escrow Adj.
                        </HeaderTemplate>
                        <ContentTemplate>
                        </ContentTemplate>
                    </cc1:TabPanel>
                    <!--End of  Escrow Adjustment Tab Section-->
                    <cc1:TabPanel runat="server" ID="tblpnlCE">
                        <HeaderTemplate>
                            <div onclick="Tabnavigation('ARCE')">
                                Comb.Elements
                            </div>
                        </HeaderTemplate>
                        <ContentTemplate>
                        </ContentTemplate>
                    </cc1:TabPanel>
                    <cc1:TabPanel runat="server" ID="tblpnlNYSIF">
                        <HeaderTemplate>
                            <div onclick="Tabnavigation('ARNYSIF')">
                                NY-SIF
                            </div>
                        </HeaderTemplate>
                        <ContentTemplate>
                        </ContentTemplate>
                    </cc1:TabPanel>
                    <cc1:TabPanel runat="server" ID="tblpnlmscInvL">
                        <HeaderTemplate>
                            <div onclick="Tabnavigation('ARMI')">
                                Misc.Invoicing
                            </div>
                        </HeaderTemplate>
                        <ContentTemplate>
                        </ContentTemplate>
                    </cc1:TabPanel>
                    <cc1:TabPanel runat="server" ID="tblpnlLRF">
                        <HeaderTemplate>
                            <div onclick="Tabnavigation('ARLRFP')">
                                LRF Posting
                            </div>
                        </HeaderTemplate>
                        <ContentTemplate>
                        </ContentTemplate>
                    </cc1:TabPanel>
                    <cc1:TabPanel runat="server" ID="tblpnlAdjchklist">
                        <HeaderTemplate>
                            <div onclick="Tabnavigation('ARAPCL')">
                                Adj.Checklist
                            </div>
                        </HeaderTemplate>
                        <ContentTemplate>
                        </ContentTemplate>
                    </cc1:TabPanel>
                    <cc1:TabPanel runat="server" ID="tblpnlAdjNumberText">
                        <HeaderTemplate>
                            <div onclick="Tabnavigation('ARANTM')">
                                Adj.Number
                            </div>
                        </HeaderTemplate>
                        <ContentTemplate>
                        </ContentTemplate>
                    </cc1:TabPanel>
                    <cc1:TabPanel runat="server" ID="tblpnlBuBroker">
                        <HeaderTemplate>
                            <div onclick="Tabnavigation('ARBB')">
                                BU Broker
                            </div>
                        </HeaderTemplate>
                        <ContentTemplate>
                            <style type="text/css">
                                .modalBackground
                                {
                                    background-color: Gray;
                                    filter: alpha(opacity=70);
                                    opacity: 0.7;
                                }
                                .modalPopup
                                {
                                    width: 250px;
                                }
                            </style>

                            <script language="javascript" type="text/javascript">
                                function ReviewPopup() {
                                    $get('<%=btnReviewClose.ClientID%>').click();
                                    $get('<%=btnReviewpopup.ClientID%>').click();
                                }
       
    
                            </script>

                            <table>
                                <tr>
                                    <td style="height: 8px">
                                    </td>
                                </tr>
                            </table>
                            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                <ContentTemplate>
                                    <!--Start of Search Panel Section -->
                                    <asp:Panel ID="pnlSelectionHeader" BorderColor="Black" BorderWidth="1px" Width="60%"
                                        runat="server" class="panelExtContents">
                                        <table width="100%" border="0" align="center" cellpadding="2" cellspacing="1">
                                            <tr style="background-color: #608CC8; color: White">
                                                <td width="26%" height="15" align="center" valign="top">
                                                    <asp:Label ID="lblselectMessage" Font-Bold="true" Font-Size="Small" runat="server"
                                                        Text="Please make selection" Style="font-family: Verdana; font-size: 11px"></asp:Label>
                                                </td>
                                            </tr>
                                        </table>
                                    </asp:Panel>
                                    <asp:Panel ID="pnlSearch" runat="server" BorderColor="Black" BorderWidth="1px" Width="60%">
                                        <table width="100%" cellpadding="0" cellspacing="0">
                                            <tr>
                                                <td>
                                                    <ARS:AdjustmentReviewSearch ID="ARS" runat="server" />
                                                    <asp:HiddenField ID="hidSelectedValues" runat="server" />
                                                </td>
                                            </tr>
                                        </table>
                                    </asp:Panel>
                                    <!--End of Search Panel Section -->
                                    <br />
                                    <asp:Label ID="lblnote" runat="server" Font-Bold="true" Text="Note: If BU (or) Broker is updated, Please recalculate to include entry in adjustment."
                                        CssClass="h4"></asp:Label><br />
                                    <br />
                                    <asp:Label ID="lblAdjNumberDetails" runat="server" CssClass="h3"></asp:Label>
                                    <div id="Div1" style="overflow: auto; width: 910px;" runat="server">
                                        <asp:AISListView ID="Adjlistview" runat="server" OnItemDataBound="DataBoundList">
                                            <LayoutTemplate>
                                                <table id="Table1" class="panelContents" runat="server" width="98%">
                                                    <tr class="LayoutTemplate">
                                                        <th>
                                                            Acct. Num.
                                                        </th>
                                                        <th>
                                                            Account Name
                                                        </th>
                                                        <th>
                                                            Valuation date
                                                        </th>
                                                        <th>
                                                            <asp:LinkButton ID="lnkBroker" runat="server">Broker</asp:LinkButton>
                                                        </th>
                                                        <th>
                                                            <asp:LinkButton ID="lnkBUOffice" runat="server">BU/Office</asp:LinkButton>
                                                        </th>
                                                        <th>
                                                            Adj. No.
                                                        </th>
                                                        <th>
                                                            Adj. Status
                                                        </th>
                                                        <th>
                                                            Calc Adj. Status
                                                        </th>
                                                    </tr>
                                                    <tr id="itemPlaceholder" runat="server">
                                                    </tr>
                                                </table>
                                            </LayoutTemplate>
                                            <ItemTemplate>
                                                <tr id="test" runat="server" class="ItemTemplate">
                                                    <td>
                                                        <%# Eval("CUSTOMERID")%>
                                                    </td>
                                                    <td>
                                                        <%# Eval("CUSTMR_NAME")%>
                                                    </td>
                                                    <td>
                                                        <%# Eval("VALN_DT", "{0:d}")%>
                                                    </td>
                                                    <td>
                                                        <%# Eval("BROKERNAME")%>
                                                    </td>
                                                    <td>
                                                        <%# Eval("BUNAME")%>
                                                    </td>
                                                    <td>
                                                        <%# Eval("PREMIUM_ADJ_ID")%>
                                                    </td>
                                                    <td>
                                                        <%# Eval("ADJ_STATUS")%>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblCalcAdjSts" runat="server" Text='<%# (Eval("CALC_ADJ_STS_CODE")==null?"":Eval("CALC_ADJ_STS_CODE").ToString() == "ERR" ? "Error" : Eval("CALC_ADJ_STS_CODE").ToString() == "CMP" ? "Complete" : Eval("CALC_ADJ_STS_CODE").ToString() == "INP" ? "Inprocess" : "")%>' />
                                                    </td>
                                                </tr>
                                            </ItemTemplate>
                                            <AlternatingItemTemplate>
                                                <tr id="test" runat="server" class="AlternatingItemTemplate">
                                                    <td>
                                                        <%# Eval("CUSTOMERID")%>
                                                    </td>
                                                    <td>
                                                        <%# Eval("CUSTMR_NAME")%>
                                                    </td>
                                                    <td>
                                                        <%# Eval("VALN_DT", "{0:d}")%>
                                                    </td>
                                                    <td>
                                                        <%# Eval("BROKERNAME")%>
                                                    </td>
                                                    <td>
                                                        <%# Eval("BUNAME")%>
                                                    </td>
                                                    <td>
                                                        <%# Eval("PREMIUM_ADJ_ID")%>
                                                    </td>
                                                    <td>
                                                        <%# Eval("ADJ_STATUS")%>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblCalcAdjSts" runat="server" Text='<%# (Eval("CALC_ADJ_STS_CODE")==null?"":Eval("CALC_ADJ_STS_CODE").ToString() == "ERR" ? "Error" : Eval("CALC_ADJ_STS_CODE").ToString() == "CMP" ? "Complete" : Eval("CALC_ADJ_STS_CODE").ToString() == "INP" ? "Inprocess" : "")%>' />
                                                    </td>
                                                </tr>
                                            </AlternatingItemTemplate>
                                            <EmptyDataTemplate>
                                                <table id="Table1" class="panelContents" runat="server" style="width: 100%">
                                                    <tr>
                                                        <th>
                                                            Acct. No
                                                        </th>
                                                        <th>
                                                            Account Name
                                                        </th>
                                                        <th>
                                                            Valuation Date
                                                        </th>
                                                        <th>
                                                            Broker
                                                        </th>
                                                        <th>
                                                            BU/Office
                                                        </th>
                                                        <th>
                                                            Adj. No.
                                                        </th>
                                                        <th>
                                                            Adj. Status
                                                        </th>
                                                        <th>
                                                            Calc Adj. Status
                                                        </th>
                                                    </tr>
                                                    <tr class="ItemTemplate">
                                                        <td align="center" colspan="12">
                                                            <asp:Label ID="lblEmptyMessage" Text="---No Records Found ---" Font-Bold="true" runat="server"
                                                                Style="text-align: center" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </EmptyDataTemplate>
                                        </asp:AISListView>
                                    </div>
                                    <br />
                                    <asp:Label ID="lblProgramPeriodDetails" runat="server" CssClass="h3"></asp:Label>
                                    <asp:Panel ID="pnlPGMlistview" Height="120px" Width="910px" runat="server" ScrollBars="Auto">
                                        <asp:AISListView ID="PGMlistview" runat="server" OnSorting="PGMlistview_Sorting">
                                            <LayoutTemplate>
                                                <table id="Table1" class="panelContents" runat="server" width="98%">
                                                    <tr class="LayoutTemplate">
                                                        <th>
                                                            Eff Date
                                                        </th>
                                                        <th>
                                                            Exp Date
                                                        </th>
                                                        <th>
                                                            Account Name
                                                        </th>
                                                        <th>
                                                            Program Type
                                                        </th>
                                                        <th>
                                                            Valuation date
                                                        </th>
                                                        <th>
                                                            <asp:LinkButton ID="lnkBroker" runat="server" CommandName="Sort" CommandArgument="BROKERNAME">Broker</asp:LinkButton>
                                                            <asp:Image ID="imgBROKERNAME" runat="server" ImageUrl="~/images/Ascending.gif" ToolTip="Ascending"
                                                                Visible="false" />
                                                        </th>
                                                        <th>
                                                            Broker Contact
                                                        </th>
                                                        <th>
                                                            <asp:LinkButton ID="lnkBUOffice" runat="server" CommandName="Sort" CommandArgument="BUSINESSUNITNAME">BU/Office</asp:LinkButton>
                                                            <asp:Image ID="imgBUSINESSUNITNAME" runat="server" ImageUrl="~/images/Ascending.gif"
                                                                ToolTip="Ascending" Visible="false" />
                                                        </th>
                                                    </tr>
                                                    <tr id="itemPlaceholder" runat="server">
                                                    </tr>
                                                </table>
                                            </LayoutTemplate>
                                            <ItemTemplate>
                                                <tr id="test" runat="server" class="ItemTemplate">
                                                    <td>
                                                        <%# Eval("STRT_DT", "{0:d}")%>
                                                    </td>
                                                    <td>
                                                        <%# Eval("PLAN_END_DT", "{0:d}")%>
                                                    </td>
                                                    <td>
                                                        <%# Eval("CUSTMR_NAME")%>
                                                    </td>
                                                    <td>
                                                        <%# Eval("PROGRAMTYPENAME") %>
                                                    </td>
                                                    <td>
                                                        <%# Eval("VALN_MM_DT", "{0:d}")%>
                                                    </td>
                                                    <td>
                                                        <%# Eval("BROKER") %>
                                                    </td>
                                                    <td>
                                                        <%# Eval("BROKER_CONTACT_NM")%>
                                                    </td>
                                                    <td>
                                                        <%# Eval("BUOFFFICE")%>
                                                    </td>
                                                </tr>
                                            </ItemTemplate>
                                            <AlternatingItemTemplate>
                                                <tr id="test" runat="server" class="AlternatingItemTemplate">
                                                    <td>
                                                        <%# Eval("STRT_DT", "{0:d}")%>
                                                    </td>
                                                    <td>
                                                        <%# Eval("PLAN_END_DT", "{0:d}")%>
                                                    </td>
                                                    <td>
                                                        <%# Eval("CUSTMR_NAME")%>
                                                    </td>
                                                    <td>
                                                        <%# Eval("PROGRAMTYPENAME") %>
                                                    </td>
                                                    <td>
                                                        <%# Eval("VALN_MM_DT", "{0:d}")%>
                                                    </td>
                                                    <td>
                                                        <%# Eval("BROKER") %>
                                                    </td>
                                                    <td>
                                                        <%# Eval("BROKER_CONTACT_NM")%>
                                                    </td>
                                                    <td>
                                                        <%# Eval("BUOFFFICE")%>
                                                    </td>
                                                </tr>
                                            </AlternatingItemTemplate>
                                            <EmptyDataTemplate>
                                                <table id="Table1" class="panelContents" runat="server" style="width: 100%">
                                                    <tr>
                                                        <th>
                                                            Eff Date
                                                        </th>
                                                        <th>
                                                            Exp Date
                                                        </th>
                                                        <th>
                                                            Account Name
                                                        </th>
                                                        <th>
                                                            Program Type
                                                        </th>
                                                        <th>
                                                            Valuation date
                                                        </th>
                                                        <th>
                                                            Broker
                                                        </th>
                                                        <th>
                                                            Broker Contact
                                                        </th>
                                                        <th>
                                                            BU/Office
                                                        </th>
                                                    </tr>
                                                    <tr class="ItemTemplate">
                                                        <td align="center" colspan="12">
                                                            <asp:Label ID="lblEmptyMessage" Text="---No Records Found ---" Font-Bold="true" runat="server"
                                                                Style="text-align: center" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </EmptyDataTemplate>
                                        </asp:AISListView>
                                    </asp:Panel>
                                    <br />
                                    <asp:ObjectDataSource ID="BrokerDataSource" runat="server" SelectMethod="GetOnlyBrokersForLookupsNA"
                                        TypeName="ZurichNA.AIS.Business.Logic.BrokerBS"></asp:ObjectDataSource>
                                    <asp:ObjectDataSource ID="BUOfficeDataSource" runat="server" SelectMethod="GetBUOffForLookups"
                                        TypeName="ZurichNA.AIS.Business.Logic.BusinessUnitOfficeBS"></asp:ObjectDataSource>
                                    <asp:Label ID="lblBuBrokerDetails" runat="server" CssClass="h3"></asp:Label>
                                    <asp:Panel ID="pnlBuBrokerDetails" runat="server" Visible="false" BorderColor="Black"
                                        BorderWidth="1px" Width="99%">
                                        <table>
                                            <tr>
                                                <td width="80%">
                                                    <table cellpadding="0">
                                                        <tr>
                                                            <td align="left" width="20%">
                                                                <asp:Label ID="lblBroker" runat="server" Text="Broker"></asp:Label>
                                                            </td>
                                                            <td align="left" valign="top" width="30%" style="padding-left: 46px">
                                                                <asp:DropDownList ID="ddlBroker" runat="server" AutoPostBack="true" Width="253px"
                                                                    OnSelectedIndexChanged="ddlBroker_SelectedIndexChanged" ValidationGroup="UpdateGroup">
                                                                    <asp:ListItem Value="0" Text="(Select)"></asp:ListItem>
                                                                </asp:DropDownList>
                                                                <%--<asp:CompareValidator ID="CompareBrokerUpdate" runat="server" ControlToValidate="ddlBroker"
                                                                Display="Dynamic" ErrorMessage="Please Select Broker" Text="*" Operator="NotEqual"
                                                                ValueToCompare="0" ValidationGroup="UpdateGroup" />--%>
                                                            </td>
                                                            <td align="left" width="20%">
                                                                <asp:Label ID="lblBrokerContact" runat="server" Text="Broker Contact"></asp:Label>
                                                            </td>
                                                            <td align="right" valign="top" width="50%" style="padding-left: 46px">
                                                                <asp:DropDownList ID="ddlBrokerContact" runat="server" AutoPostBack="true" Width="253px">
                                                                    <asp:ListItem Value="0" Text="(Select)"></asp:ListItem>
                                                                </asp:DropDownList>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="left" valign="top" width="20%" style="padding-top: 3px;">
                                                                <asp:Label ID="lblBUOffice" runat="server" Text="BU/Office"></asp:Label>
                                                            </td>
                                                            <td align="left" valign="top" width="50%" style="padding-left: 46px; padding-top: 3px;">
                                                                <asp:DropDownList ID="ddlBUOffice" runat="server" AutoPostBack="true" Width="253px"
                                                                    ValidationGroup="UpdateGroup">
                                                                    <asp:ListItem Value="0" Text="(Select)"></asp:ListItem>
                                                                </asp:DropDownList>
                                                                <%-- <asp:CompareValidator ID="CompareBU" runat="server" ControlToValidate="ddlBUOffice" Display="Dynamic"
                                                                ErrorMessage="Please Select BU/Office" Text="*" Operator="NotEqual" ValueToCompare="0"
                                                                ValidationGroup="UpdateGroup" />--%>
                                                            </td>
                                                            <td align="left" width="30%">
                                                            </td>
                                                            <td align="left" valign="top" width="60%" style="padding-top: 3px;">
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="left" width="20%">
                                                            </td>
                                                            <td align="right" valign="top" width="50%" style="padding-top: 3px;">
                                                                <asp:Button ID="btnUpdate" runat="server" Text="Update" OnClick="btnUpdate_Click"
                                                                    ToolTip />
                                                                <asp:Button ID="btnCancel" runat="server" Text="Cancel" OnClick="btnCancel_Click" />
                                                            </td>
                                                            <td align="left" width="30%">
                                                            </td>
                                                            <td align="left" valign="top" width="60%" style="padding-top: 3px;">
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                        </table>
                                    </asp:Panel>
                                    <asp:UpdatePanel runat="server" ID="upBuBrokerReview">
                                        <ContentTemplate>
                                            <asp:Button Width="0px" runat="server" ID="btnFinalTemp" />
                                            <cc1:ModalPopupExtender runat="server" ID="modalReview" TargetControlID="btnFinalTemp"
                                                PopupControlID="pnlReviewPopup" BackgroundCssClass="modalBackground" DropShadow="true"
                                                CancelControlID="btnReviewClose">
                                            </cc1:ModalPopupExtender>
                                            <div style="float: left;">
                                                <asp:Panel runat="server" CssClass="modalPopup" ID="pnlReviewPopup" Style="border: solid 1px black;
                                                    display: none; width: 650px; padding: 0px" HorizontalAlign="Center">
                                                    <asp:Panel runat="Server" ID="Panel4" Style="width: 100%; cursor: move; padding: 0px;
                                                        background-color: #CCCCCC; height: 20px; border: solid 1px Gray; color: Black;
                                                        text-align: center; vertical-align: middle">
                                                    </asp:Panel>
                                                    <div style="text-align: left; width: 100%; padding-bottom: 5px; background-color: White;">
                                                        <br />
                                                        <ul>
                                                            <div style="text-align: left; width: 100%; padding-bottom: 5px; background-color: White;">
                                                                Information updated successfully. Please remember that:
                                                            </div>
                                                            <div style="text-align: center; width: 100%; padding-bottom: 5px; background-color: White;">
                                                                <li>All future program periods will need to be updated in the program period screen.&nbsp;&nbsp;&nbsp;
                                                                </li>
                                                                <li>The adjustment will need to be recalculated in order for the changes to take effect.
                                                                </li>
                                                            </div>
                                                            <div style="text-align: left; width: 100%; padding-bottom: 5px; background-color: White;">
                                                                Do you want to Proceed to Invoicing Dashboard screen to recalculate now?
                                                            </div>
                                                        </ul>
                                                    </div>
                                                    <div style="text-align: center; width: 100%; padding-bottom: 2px; background-color: White;">
                                                        <asp:Button Width="60px" OnClientClick="ReviewPopup()" ID="btnReviewpopup" runat="server"
                                                            Text="Yes" OnClick="btnReviewpopup_Click" />
                                                        <asp:Button Width="60px" ID="btnReviewClose" runat="server" Text="No" OnClick="btnReviewClose_Click" />
                                                        <br />
                                                    </div>
                                                </asp:Panel>
                                                
                                            </div>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </ContentTemplate>
                    </cc1:TabPanel>
                </cc1:TabContainer>
            </td>
        </tr>
    </table>
</asp:Content>
