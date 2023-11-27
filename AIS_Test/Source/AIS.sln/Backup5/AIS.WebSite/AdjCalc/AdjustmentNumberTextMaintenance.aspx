<%@ Page Language="C#" MasterPageFile="~/Retro.master" AutoEventWireup="true" CodeBehind="AdjustmentNumberTextMaintenance.aspx.cs"
    Inherits="AdjCalc_AdjustmentNumberTextMaintenance" Title="Untitled Page" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/App_Shared/AdjustmentReviewSearch.ascx" TagPrefix="ARS" TagName="AdjustmentReviewSearch" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeaderPlaceHolder" runat="server">
    <asp:Label ID="lblAdjReview" runat="server" Text="Adjustment Review" CssClass="h1"></asp:Label>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainPlaceHolder" runat="server">

    <script language="javascript" type="text/javascript">
     //Function to Redirect to differenet Tabs

    var scrollTop1;

    if(Sys.WebForms.PageRequestManager!=null)
    {
        Sys.WebForms.PageRequestManager.getInstance().add_beginRequest(BeginRequestHandler);
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);
    }
    
    function BeginRequestHandler(sender, args) 
    {
        var mef = $get('<%=pnlAdjNumberList.ClientID%>');
        if(mef!=null)
        scrollTop1 = mef.scrollTop;
    }

    function EndRequestHandler(sender, args)
    {
        var mef = $get('<%=pnlAdjNumberList.ClientID%>');
        if(mef!=null)
        mef.scrollTop = scrollTop1;
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
            if (selectedValues.value != "") {
                strURL += "?SelectedValues=" + selectedValues.value + "&wID=<%= WindowName%>";
            }
            else {
                strURL += "?wID=<%= WindowName%>";
            }
            window.location.href=strURL;
        }  
    </script>

    <br />
    <asp:ValidationSummary ID="VSSaveAdjNumber" runat="server" ValidationGroup="AdjNumberGroup"
        Height="40px" CssClass="ValidationSummary" />
    <table>
        <tr>
            <td>
                <cc1:TabContainer ID="TabContainer1" runat="server" CssClass="VariableTabs" SkinID="tabVariable"
                    ActiveTabIndex="8">
                    <cc1:TabPanel runat="server" ID="tblpnlLBA">
                        <HeaderTemplate>
                            <div onclick="Tabnavigation('ARC')">
                                Review
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
                            Loss Fund Adj.
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
                               Surch & Assmt
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
                            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                <ContentTemplate>
                                    <!--Start of Search Panel Section -->
                                    <asp:Panel ID="pnlSelectionHeader" BorderColor="Black" BorderWidth="1px" Width="60%"
                                        runat="server" class="panelExtContents">
                                        <table width="100%" border="0" align="center" cellpadding="2" cellspacing="1">
                                            <tr style="background-color: #608CC8; color: White">
                                                <td width="26%" height="20" align="center" valign="top">
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
                                    <asp:Label ID="lblAdjNumberDetails" runat="server" CssClass="h3"></asp:Label>
                                    <!--Start of ListView Panel Section-->
                                    <asp:Panel ID="pnlAdjNumberList" Width="100%" runat="server" CssClass="content" ScrollBars="Auto"
                                        Height="160px">
                                        <asp:AISListView ID="lstAdjNumber" runat="server" OnItemCommand="lstAdjNumber_ItemCommand"
                                            OnItemDataBound="lstAdjNumber_DataBoundList" OnItemEditing="lstAdjNumber_ItemEdit"
                                            OnItemCanceling="lstAdjNumber_ItemCancel" OnItemUpdating="lstAdjNumber_ItemUpdating" OnSorting="lstAdjNumber_Sorting">
                                            <LayoutTemplate>
                                                <table id="lstMITable" class="panelExtContents" runat="server" width="100%">
                                                    <tr class="LayoutTemplate">
                                                        <th>
                                                            Select
                                                        </th>
                                                        <th>
                                                            Customer Name
                                                        </th>
                                                        <th>
                                                            Valuation Date
                                                        </th>
                                                        <th>
                                                            Program Period
                                                        </th>
                                                        <th>
                                                            <asp:LinkButton ID="lnkAdjNumbers" runat="server" CommandName="Sort" CommandArgument="ADJUSTMENTNUMBER">
                                                              Adjustment Number</asp:LinkButton>
                                                            <asp:Image ID="imgADJUSTMENTNUMBER" runat="server" ImageUrl="~/images/Ascending.gif"
                                                                ToolTip="Ascending" Visible="false" />
                                                        </th>
                                                        <th>
                                                            Adj. Number Text
                                                        </th>
                                                    </tr>
                                                    <tr id="ItemPlaceHolder" runat="server">
                                                    </tr>
                                                </table>
                                            </LayoutTemplate>
                                            <EmptyDataTemplate>
                                                <table id="lstMITable" class="panelExtContents" runat="server" width="100%">
                                                    <tr class="LayoutTemplate">
                                                        <th>
                                                            Select
                                                        </th>
                                                        <th>
                                                            Customer Name
                                                        </th>
                                                        <th>
                                                            Valuation Date
                                                        </th>
                                                        <th>
                                                            Program Period
                                                        </th>
                                                        <th>
                                                            Adjustment Number
                                                        </th>
                                                        <th>
                                                            Adj. Number Text
                                                        </th>
                                                    </tr>
                                                    <tr id="ItemPlaceHolder" runat="server">
                                                    </tr>
                                                </table>
                                                <table width="910px">
                                                    <tr id="Tr1" runat="server" class="ItemTemplate">
                                                        <td align="center">
                                                            <asp:Label ID="lblEmptyMessage" Text="---Records are not allowed to Add as status is not in CALC ---"
                                                                Font-Bold="true" Width="600px" runat="server" Style="text-align: center" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </EmptyDataTemplate>
                                            <AlternatingItemTemplate>
                                                <tr id="trItemTemplate" runat="server" class="AlternatingItemTemplate">
                                                    <td>
                                                        <asp:LinkButton ID="lblItemEdit" CommandName="Edit" Text="Edit" runat="server" Visible="true"
                                                            Width="40px" ToolTip="Click here to Edit" />
                                                    </td>
                                                    <td>
                                                        <%# Eval("CUSTMR_NAME")%>
                                                    </td>
                                                    <td>
                                                        <%# Eval("VALUATIONDATE")%>
                                                    </td>
                                                    <td>
                                                       <%# Eval("START_DATE")%>:<%# Eval("END_DATE")%>
                                                    </td>
                                                    <td>
                                                        <%# Eval("ADJ_NBR")%>
                                                    </td>
                                                    <td>
                                                        <%# Eval("ADJ_NBR_TXT")%>
                                                    </td>
                                                </tr>
                                            </AlternatingItemTemplate>
                                            <ItemTemplate>
                                                <tr id="trItemTemplate" runat="server" class="ItemTemplate">
                                                    <td>
                                                        <asp:LinkButton ID="lblItemEdit" CommandName="Edit" Text="Edit" runat="server" Visible="true"
                                                            Width="40px" ToolTip="Click here to Edit" />
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblPremAdjPerdID" Width="165px" Visible="false" runat="server" Text='<%# Bind("PREM_ADJ_PERD_ID") %>'></asp:Label>
                                                        <%# Eval("CUSTMR_NAME")%>
                                                    </td>
                                                    <td>
                                                        <%# Eval("VALUATIONDATE")%>
                                                    </td>
                                                    <td>
                                                        <%# Eval("START_DATE")%>:<%# Eval("END_DATE")%>
                                                    </td>
                                                    <td>
                                                        <%# Eval("ADJ_NBR")%>
                                                    </td>
                                                    <td>
                                                        <%# Eval("ADJ_NBR_TXT")%>
                                                    </td>
                                                </tr>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <tr class="ItemTemplate">
                                                    <td>
                                                        <asp:LinkButton ID="lbUpdate" CommandName="Update" Text="Update" runat="server" Visible="true"
                                                            Width="40px" ToolTip="Click here to Update" />
                                                        <asp:LinkButton ID="lbCancel" CommandName="Cancel" runat="server" Text="Cancel" Visible="true"
                                                            Width="40px" ToolTip="Click here to Cancel" />
                                                    </td>
                                                    <td align="center">
                                                        <asp:Label ID="lblPremAdjPerdID" Width="165px" Visible="false" runat="server" Text='<%# Bind("PREM_ADJ_PERD_ID") %>'></asp:Label>
                                                        <%# Eval("CUSTMR_NAME")%>
                                                    </td>
                                                    <td align="center">
                                                        <%# Eval("VALUATIONDATE")%>
                                                    </td>
                                                    <td>
                                                         <%# Eval("START_DATE")%>:<%# Eval("END_DATE")%>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtAdjNbr" runat="server" MaxLength="10" Text='<%# Eval("ADJ_NBR")%>'
                                                            Width="100px" />
                                                        <cc1:FilteredTextBoxExtender ID="fltrAdjNbr" runat="server" TargetControlID="txtAdjNbr"
                                                            FilterType="Custom" FilterMode="ValidChars" ValidChars="0123456789" />
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtAdjNbrTxt" runat="server" MaxLength="100" Text='<%# Eval("ADJ_NBR_TXT")%>'
                                                            Width="215px" />
                                                        <%--<cc1:FilteredTextBoxExtender TargetControlID="txtAdjNbrTxt" FilterType="LowercaseLetters,UppercaseLetters"
                                                            ID="fteAdjNbrTxt" runat="server" />--%>
                                                    </td>
                                                </tr>
                                            </EditItemTemplate>
                                        </asp:AISListView>
                                    </asp:Panel>
                                    <!--End of ListView Panel Section-->
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </ContentTemplate>
                    </cc1:TabPanel>
                    <cc1:TabPanel runat="server" ID="tblpnlBuBroker">
                        <HeaderTemplate>
                            <div onclick="Tabnavigation('ARBB')">
                                BU Broker
                            </div>
                        </HeaderTemplate>
                        <ContentTemplate>
                        </ContentTemplate>
                    </cc1:TabPanel>
                </cc1:TabContainer>
            </td>
        </tr>
    </table>
</asp:Content>
