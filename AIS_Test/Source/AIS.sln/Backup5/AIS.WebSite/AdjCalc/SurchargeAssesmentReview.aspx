<%@ Page Language="C#" MasterPageFile="~/Retro.master" AutoEventWireup="true" CodeBehind="SurchargeAssesmentReview.aspx.cs"
    Inherits="AdjCalc_SurchargeAssesmentReview" Title="Untitled Page" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ACT" %>
<%@ Register Src="~/App_Shared/AdjustmentReviewSearch.ascx" TagPrefix="ARS" TagName="AdjustmentReviewSearch" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeaderPlaceHolder" runat="server">
    <asp:Label ID="lblAdjReview" runat="server" Text="Adjustment Review" CssClass="h1"></asp:Label>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainPlaceHolder" runat="server">
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

      
        //Function to Redirect to differenet Tabs
       
        //window.onbeforeunload = closeIt;

        var scrollTop1;

        if (Sys.WebForms.PageRequestManager != null) {
            Sys.WebForms.PageRequestManager.getInstance().add_beginRequest(BeginRequestHandler);
            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);
        }

        function BeginRequestHandler(sender, args) {
            var mef = $get('<%=pnlSurchargeReviewList.ClientID%>');
            if (mef != null)
                scrollTop1 = mef.scrollTop;
        }

        function EndRequestHandler(sender, args) {
            var mef = $get('<%=pnlSurchargeReviewList.ClientID%>');
            if (mef != null)
                mef.scrollTop = scrollTop1;
        }
        function EditClick() {
            $get('<%=hidPopupflag.ClientID%>').value="Yes";
        }
        function saveClick() {
            $get('<%=hidPopupflag.ClientID%>').value="";
        }
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

                    strURL += "SurchargeAssesmentReview.aspx";
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
                else if (Pagename == "ARBB") {
                    strURL += "BuBrokerReview.aspx";
                }
                if (selectedValues.value != "") {
                    //             if(hidCommShown.value=="1")
                    //             {
                    //               selectedValues.value += ";1";
                    //             }
                    strURL += "?SelectedValues=" + selectedValues.value + "&wID=<%= WindowName%>";
                }
                else {
                    strURL += "?wID=<%= WindowName%>";
                }
                $get('<%=txtPageURL.ClientID%>').value = strURL;
                if ($get('<%=hidPopupflag.ClientID%>').value !="") {
                    check();
                }
                else {
                    window.location.href = strURL;
            }
        }


        
        function check() {
            if ($get('<%=hidPopupflag.ClientID%>').value !="")
                $get('<%=btnPopup.ClientID%>').click();

        }
/* INC5489578 fix starts here
        function CheckWithoutSave(pageUrl) {
            pageUrl = AddWindowNametoURL(pageUrl);
            $get('<%=txtPageURL.ClientID%>').value = pageUrl;  
            if ($get('<%=hidPopupflag.ClientID%>').value !="")
                $get('<%=btnPopup.ClientID%>').click();
            else {
                window.location.href = pageUrl;
                submitted = 0;
            }
        } INC5489578 fix ends here */

        function AddWindowNametoURL(url) {
            if (!url.indexOf("?wID") > 0 && !url.indexOf("&wID") > 0) {
                if (url.indexOf("?") > 0) {
                    return url + '&wID=<%= WindowName%>';
                }
                else {
                    return url + '?wID=<%= WindowName%>';
                }
            }
        }
         
    </script>

    <br />
    <asp:ValidationSummary ID="VSSaveAdjNumber" runat="server" ValidationGroup="AdjNumberGroup"
        Height="40px" CssClass="ValidationSummary" />
    <table>
        <tr>
            <td>
                <ACT:TabContainer ID="AdjRevTabContainer" runat="server" CssClass="VariableTabs"
                    SkinID="tabVariable" ActiveTabIndex="4">
                    <ACT:TabPanel runat="server" ID="tblpnlLBA">
                        <HeaderTemplate>
                            <div id="div1" runat="server" onclick="Tabnavigation('ARC')">
                                Review
                            </div>
                        </HeaderTemplate>
                        <ContentTemplate>
                        </ContentTemplate>
                    </ACT:TabPanel>
                    <ACT:TabPanel runat="server" ID="tblpnlLCF">
                        <HeaderTemplate>
                            <div onclick="Tabnavigation('ARPLB')">
                                Adj. PLB
                            </div>
                        </HeaderTemplate>
                        <ContentTemplate>
                        </ContentTemplate>
                    </ACT:TabPanel>
                    <!--Start of  Escrow Adjustment Tab Section-->
                    <ACT:TabPanel runat="server" ID="tblpnlTM">
                        <HeaderTemplate>
                            Loss Fund Adj.
                        </HeaderTemplate>
                        <ContentTemplate>
                        </ContentTemplate>
                    </ACT:TabPanel>
                    <!--End of  Escrow Adjustment Tab Section-->
                    <ACT:TabPanel runat="server" ID="tblpnlCE">
                        <HeaderTemplate>
                            <div onclick="Tabnavigation('ARCE')">
                                Comb.Elements
                            </div>
                        </HeaderTemplate>
                        <ContentTemplate>
                        </ContentTemplate>
                    </ACT:TabPanel>
                    <ACT:TabPanel runat="server" ID="tblpnlNYSIF">
                        <HeaderTemplate>
                            <div id="divNY" runat="server" onclick="Tabnavigation('ARNYSIF')">
                                Surch & Assmt
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
                            <asp:UpdatePanel ID="ContentUpdatePanel" runat="server">
                                <ContentTemplate>
                                    <asp:Button style="display:none" runat="server" ID="AriesTemp" />
                                    <asp:Button ID="btnPopup" style="display:none" runat="server" 
                                        OnClick="btnPopup_Click" />
                                        <asp:Button ID="btnHide" style="display:none" runat="server" 
                                        />
                                    <ACT:ModalPopupExtender runat="server" ID="modalSave" TargetControlID="AriesTemp"
                                        PopupControlID="pnlSavePopup" BackgroundCssClass="modalBackground" DropShadow="true"
                                        CancelControlID="btnHide">
                                    </ACT:ModalPopupExtender>
                                    <div style="float: left;">
                                        <asp:Panel runat="server" CssClass="modalPopup" ID="pnlSavePopup" Style="border: solid 1px black;
                                            display: none; width: 650px; padding: 0px" HorizontalAlign="Center">
                                            <asp:Panel runat="Server" ID="Panel1" Style="width: 100%; cursor: move; padding: 0px;
                                                background-color: #CCCCCC; height: 20px; border: solid 1px Gray; color: Black;
                                                text-align: center; vertical-align: middle">
                                                 <asp:ValidationSummary ID="valCombinedElems" runat="server" ValidationGroup="Save"
                BorderColor="Red"                   BorderWidth="1" BorderStyle="Solid" />
                                            </asp:Panel>
                                            <div style="text-align: center; width: 100%; padding-bottom: 5px; background-color: White;">
                                                <br />
                                                There has been a change to the "Other Surcharges & Credits" field.  
                                                <br />
                                                A comment is required.  
                                                <br />
                                                <asp:TextBox ID="txtComments" runat="server" Height="82px" TextMode="MultiLine" Width="210px"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="reqtxtComments" runat="server" ErrorMessage="Please enter comments."
                                                Text="*" ValidationGroup="Save" ControlToValidate="txtComments"></asp:RequiredFieldValidator>
                                                <br />
                                                <asp:Button ID="btnSave" runat="server" Text="Save" OnClick="btnSave_Click" ValidationGroup="Save" OnClientClick="saveClick();" />
                                            </div>
                                        </asp:Panel>
                                    </div>
                                    <asp:TextBox ID="txtPageURL" runat="server" Style="display: none"></asp:TextBox>
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
                                                    <asp:HiddenField ID="hidPopupflag" runat="server" />
                                                </td>
                                            </tr>
                                        </table>
                                    </asp:Panel>
                                    <!--End of Search Panel Section -->
                                    <br />
                                    <asp:Label ID="lblSurchargeReviewDetails" runat="server" CssClass="h3"></asp:Label>
                                    <!--Start of ListView Panel Section-->
                                    <asp:Panel ID="pnlSurchargeReviewList" Width="98%" runat="server" CssClass="content"
                                        ScrollBars="Auto" Height="160px">
                                        <asp:AISListView ID="lstSurchargeReview" runat="server" OnItemEditing="lstSurchargeReview_ItemEdit">
                                            <LayoutTemplate>
                                                <table id="lstMITable" class="panelExtContents" runat="server" width="98%">
                                                    <tr class="LayoutTemplate">
                                                        <th>
                                                            Surcharge/ </tr> Assessment Code
                                                        </th>
                                                        <th>
                                                            Surcharge/Assessment Description
                                                        </th>
                                                        <th>
                                                            Other Surcharges & Credits
                                                        </th>
                                                    </tr>
                                                    <tr id="ItemPlaceHolder" runat="server">
                                                    </tr>
                                                </table>
                                            </LayoutTemplate>
                                            <EmptyDataTemplate>
                                                <table id="lstMITable" class="panelExtContents" runat="server" width="98%">
                                                    <tr class="LayoutTemplate">
                                                        <th>
                                                            Surcharge/ </tr> Assessment Code
                                                        </th>
                                                        <th>
                                                            Surcharge/Assessment Description
                                                        </th>
                                                        <th>
                                                            Other Surcharges & Credits
                                                        </th>
                                                    </tr>
                                                    <tr id="ItemPlaceHolder" runat="server">
                                                    </tr>
                                                </table>
                                                <table width="100%">
                                                    <tr id="Tr1" runat="server" class="ItemTemplate">
                                                        <td align="center">
                                                            <asp:Label ID="lblEmptyMessage" Text="---No Records Found---"
                                                                Font-Bold="true" Width="600px" runat="server" Style="text-align: center" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </EmptyDataTemplate>
                                            <AlternatingItemTemplate>
                                                <tr id="trItemTemplate" runat="server" class="AlternatingItemTemplate">
                                                    <td>
                                                        <asp:Label ID="lblSurDTLID" Width="165px" Visible="false" runat="server" Text='<%# Bind("Surcharge_Code") %>'></asp:Label>
                                                        <%# Eval("Surcharge_Code")%>
                                                    </td>
                                                    <td>
                                                        <%# Eval("Surcharge_Desc")%>
                                                    </td>
                                                    <td>
                                                         <asp:LinkButton ID="lblSurItemEdit" CommandName="Edit"  Text='<%# Eval("other_surchrg_amt") != null ? (Eval("other_surchrg_amt").ToString() != "" ?(decimal.Parse(Eval("other_surchrg_amt").ToString())).ToString("#,##0") : "0"): "0"%>' runat="server"
                                                            Visible="true" Width="150px"  ToolTip="Click here to Edit" />                    
                                                        
                                                    </td>
                                                </tr>
                                            </AlternatingItemTemplate>
                                            <ItemTemplate>
                                                <tr id="trItemTemplate" runat="server" class="ItemTemplate">
                                                    <td>
                                                        <asp:Label ID="lblSurDTLID" Width="165px" Visible="false" runat="server" Text='<%# Bind("Surcharge_Code") %>'></asp:Label>
                                                        <%# Eval("Surcharge_Code")%>
                                                    </td>
                                                    <td>
                                                        <%# Eval("Surcharge_Desc")%>
                                                    </td>
                                                    <td>
                                                        <asp:LinkButton ID="lblSurItemEdit" CommandName="Edit"  Text='<%# Eval("other_surchrg_amt") != null ? (Eval("other_surchrg_amt").ToString() != "" ?(decimal.Parse(Eval("other_surchrg_amt").ToString())).ToString("#,##0") : "0"): "0"%>' runat="server" 
                                                            Visible="true" Width="150px" ToolTip="Click here to Edit"  /> 
                                                    </td>
                                                </tr>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <tr class="ItemTemplate">
                                                    <td align="center">
                                                        <asp:Label ID="lblSurDTLID" Width="0px" Height="0px" Visible="false" runat="server"
                                                            Text='<%# Bind("Surcharge_Code") %>'></asp:Label>
                                                        <%# Eval("Surcharge_Code")%>
                                                    </td>
                                                    <td align="center">
                                                        <%# Eval("Surcharge_Desc")%>
                                                    </td>
                                                    <td>
                                                        <asp:LinkButton ID="lblSurItemEdit" CommandName="Edit"  Text='<%# Eval("other_surchrg_amt") != null ? (Eval("other_surchrg_amt").ToString() != "" ?(decimal.Parse(Eval("other_surchrg_amt").ToString())).ToString("#,##0") : "0"): "0"%>' runat="server" 
                                                            Visible="true" Width="150px" ToolTip="Click here to Edit" /> 
                                                    </td>
                                                </tr>
                                            </EditItemTemplate>
                                        </asp:AISListView>
                                    </asp:Panel>
                                    <asp:Panel ID="pnlPol" runat="server" Visible="false">
                                        <asp:HiddenField ID="hid1" runat="server" />
                                        <asp:Label ID="lblLstPolReviewHeader" runat="server" CssClass="h3" Visible="false"></asp:Label>
                                        <asp:LinkButton ID="lbClosePolicyDetails" Text="Close" runat="server" OnClick="lbClosePolicyDetails_Click"
                                           Visible="False" />
                                        <asp:AISListView ID="lstPolReview" runat="server" OnItemCommand="lstPolReview_ItemCommand"
                                            OnItemDataBound="lstPolReview_DataBoundList" OnItemEditing="lstPolReview_ItemEdit"
                                            OnItemCanceling="lstPolReview_ItemCancel" OnItemUpdating="lstPolReview_ItemUpdating"
                                            Visible="false">
                                            <LayoutTemplate>
                                                <table id="lstMITable" class="panelExtContents" runat="server" width="100%">
                                                    <tr class="LayoutTemplate">
                                                        <th>
                                                            Select
                                                        </th>
                                                        <th>
                                                            Policy Number
                                                        </th>
                                                        <th>
                                                            Total Surcharge/ </tr> Assessment Base
                                                        </th>
                                                        <th>
                                                            Factor
                                                        </th>
                                                        <th>
                                                            Other Surcharges & Credits
                                                        </th>
                                                        <th>
                                                            Total Additional/Return
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
                                                            Policy Number
                                                        </th>
                                                        <th>
                                                            Total Surcharge/ </tr> Assessment Base
                                                        </th>
                                                        <th>
                                                            Factor
                                                        </th>
                                                        <th>
                                                            Other Surcharges & Credits
                                                        </th>
                                                        <th>
                                                            Total Additional/Return
                                                        </th>
                                                    </tr>
                                                    <tr id="ItemPlaceHolder" runat="server">
                                                    </tr>
                                                </table>
                                                <table width="100%">
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
                                                        <asp:Label ID="lblSurDTLID" Width="165px" Visible="false" runat="server" Text='<%# Bind("prem_adj_surchrg_dtl_id") %>'></asp:Label>
                                                        <%# Eval("pol_name")%>
                                                    </td>
                                                    <td>
                                                        <%# Eval("tot_surchrg_asses_base")%>
                                                    </td>
                                                    <td>
                                                        <%# Eval("surchrg_rt")%>
                                                    </td>
                                                    <td>
                                                      <%# Eval("other_surchrg_amt") != null ? (Eval("other_surchrg_amt").ToString() != "" ?(decimal.Parse(Eval("other_surchrg_amt").ToString())).ToString("#,##0") : "0"): "0"%>
                                                    </td>
                                                    <td>
                                                        <%# Eval("tot_addn_rtn")%>
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
                                                        <asp:Label ID="lblSurDTLID" Width="165px" Visible="false" runat="server" Text='<%# Bind("prem_adj_surchrg_dtl_id") %>'></asp:Label>
                                                        <%# Eval("pol_name")%>
                                                    </td>
                                                    <td>
                                                        <%# Eval("tot_surchrg_asses_base")%>
                                                    </td>
                                                    <td>
                                                        <%# Eval("surchrg_rt")%>
                                                    </td>
                                                    <td>
                                                            <%# Eval("other_surchrg_amt") != null ? (Eval("other_surchrg_amt").ToString() != "" ?(decimal.Parse(Eval("other_surchrg_amt").ToString())).ToString("#,##0") : "0"): "0"%>                                                
                                                            </td>
                                                    <td>
                                                        <%# Eval("tot_addn_rtn")%>
                                                    </td>
                                                </tr>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <tr class="ItemTemplate">
                                                    <td>
                                                        <asp:LinkButton ID="lblItemEdit" CommandName="Update" Text="Update" runat="server"
                                                            Visible="true" Width="40px" ToolTip="Click here to Update" OnClientClick="EditClick();" />
                                                        <asp:LinkButton ID="LinkButton5" CommandName="Cancel" Text="Cancel" runat="server"
                                                            Visible="true" Width="40px" ToolTip="Click here to Cancel" />
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblSurDTLID" Width="165px" Visible="false" runat="server" Text='<%# Bind("prem_adj_surchrg_dtl_id") %>'></asp:Label>
                                                        <%# Eval("pol_name")%>
                                                    </td>
                                                    <td>
                                                        <%# Eval("tot_surchrg_asses_base")%>
                                                    </td>
                                                    <td>
                                                        <%# Eval("surchrg_rt")%>
                                                    </td>
                                                    <td>
                                                      <asp:AISAmountTextbox ID="txtOtherAmt" runat="server" AllowNegetive="true" Text='<%# Eval("other_surchrg_amt") != null ? (Eval("other_surchrg_amt").ToString() != "" ?(decimal.Parse(Eval("other_surchrg_amt").ToString())).ToString("#,##0") : "0"): "0"%>'>
                                                     </asp:AISAmountTextbox>
                                                    </td>
                                                    <td>
                                                        <%# Eval("tot_addn_rtn")%>
                                                    </td>
                                                </tr>
                                            </EditItemTemplate>
                                        </asp:AISListView>
                                    </asp:Panel>
                                    <!--End of ListView Panel Section-->
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </ContentTemplate>
                    </ACT:TabPanel>
                    <ACT:TabPanel runat="server" ID="tblpnlmscInvL">
                        <HeaderTemplate>
                            <div onclick="Tabnavigation('ARMI')">
                                Misc.Invoicing
                            </div>
                        </HeaderTemplate>
                        <ContentTemplate>
                        </ContentTemplate>
                    </ACT:TabPanel>
                    <ACT:TabPanel runat="server" ID="tblpnlLRF">
                        <HeaderTemplate>
                            <div onclick="Tabnavigation('ARLRFP')">
                                LRF Posting
                            </div>
                        </HeaderTemplate>
                        <ContentTemplate>
                        </ContentTemplate>
                    </ACT:TabPanel>
                    <ACT:TabPanel runat="server" ID="tblpnlAdjchklist">
                        <HeaderTemplate>
                            <div onclick="Tabnavigation('ARAPCL')">
                                Adj.Checklist
                            </div>
                        </HeaderTemplate>
                        <ContentTemplate>
                        </ContentTemplate>
                    </ACT:TabPanel>
                    <ACT:TabPanel runat="server" ID="tblpnlAdjNumberText">
                        <HeaderTemplate>
                            <div onclick="Tabnavigation('ARANTM')">
                                Adj.Number
                            </div>
                        </HeaderTemplate>
                        <ContentTemplate>
                        </ContentTemplate>
                    </ACT:TabPanel>
                    <ACT:TabPanel runat="server" ID="tblpnlBuBroker">
                        <HeaderTemplate>
                            <div onclick="Tabnavigation('ARBB')">
                                BU Broker
                            </div>
                        </HeaderTemplate>
                        <ContentTemplate>
                        </ContentTemplate>
                    </ACT:TabPanel>
                </ACT:TabContainer>
            </td>
        </tr>
    </table>
</asp:Content>
