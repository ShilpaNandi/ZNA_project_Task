<%@ Page Language="C#" MasterPageFile="~/Retro.master" AutoEventWireup="true" CodeBehind="LRFPostingDetails.aspx.cs"
    Inherits="ZurichNA.AIS.WebSite.AdjCalc.LRFPostingDetails" Title="Untitled Page" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/App_Shared/AdjustmentReviewSearch.ascx" TagPrefix="ARS" TagName="AdjustmentReviewSearch" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeaderPlaceHolder" runat="server">
    <asp:Label ID="lblAdjReview" runat="server" Text="Adjustment Review" CssClass="h1"></asp:Label>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainPlaceHolder" runat="server">

    <script type="text/javascript" src="../JavaScript/RetroScript.js"></script>

    <script language="javascript" type="text/javascript">
        //Formule:Posting Amount=Current + Aggregate Credit - Adj. Prior Year
        function AddPostingAmount(lblCurrent, txtAggtAmt, txtAdjPriorYrAmt, txtPostingAmt) {
            var myRegExp = /,|_/g;
            var Current = $get(lblCurrent).innerText.replace(myRegExp, '');
            var AggtAmt = $get(txtAggtAmt).value.replace(myRegExp, '');
            var AdjPriorYrAmt = $get(txtAdjPriorYrAmt).value.replace(myRegExp, '');
            var result = 0;
            if (Current != "") {
                result += parseFloat(Current);
            }
            if (AggtAmt != "") {
                result += parseFloat(AggtAmt);
            }
            if (AdjPriorYrAmt != "") {
                result -= parseFloat(AdjPriorYrAmt);
            }
            $get(txtPostingAmt).value = fix(result);
            FormatNumNoDecAmt($get(txtPostingAmt), 11);
            FormatNumNoDecAmt($get(txtAggtAmt), 11);
            FormatNumNoDecAmt($get(txtAdjPriorYrAmt), 11);


        }
        function AddTaxGridPostingAmount(lblCurrent, txtAdjPriorYrAmt, txtPostingAmt) {
            var myRegExp = /,|_/g;
            var Current = $get(lblCurrent).innerText.replace(myRegExp, '');
            var AdjPriorYrAmt = $get(txtAdjPriorYrAmt).value.replace(myRegExp, '');
            var result = 0;
            if (Current != "") {
                result += parseFloat(Current);
            }
            if (AdjPriorYrAmt != "") {
                result -= parseFloat(AdjPriorYrAmt);
            }
            $get(txtPostingAmt).value = fix(result);
            FormatNumNoDecAmt($get(txtPostingAmt), 11);
            FormatNumNoDecAmt($get(txtAdjPriorYrAmt), 11);
        }
        function Getfocus(txtfocus, lblTranstxt) {
            if ($get(lblTranstxt).innerText != "Reserves") {
                $get(txtfocus).focus();
                return false;
            }
        }
        function GetTaxfocus(txtfocus) {
            $get(txtfocus).focus();
            return false;

        }

        function CalcReserve(txtAdjPriorYrAmt, hidAdjPriorYrAmt, lblTranstxt) {

            if ($get(lblTranstxt).innerText != "Reserves") {
                var result = 0;
                var PostingResult = 0;

                var AdjPriorYrAmt = $get(txtAdjPriorYrAmt).value.replace(myRegExp, '');

                var AdjPriorYrAmtActual = $get(hidAdjPriorYrAmt).value.replace(myRegExp, '');

                var Reserve = $get('ctl00_MainPlaceHolder_TabContainer1_tblpnlLRF_lstLRFPosting_ctrl6_lableAdjPriorYrAmt').innerText.replace(myRegExp, '');
                result = parseInt(Reserve) + (parseInt(AdjPriorYrAmtActual) - parseInt(AdjPriorYrAmt));
                $get('ctl00_MainPlaceHolder_TabContainer1_tblpnlLRF_lstLRFPosting_ctrl6_lableAdjPriorYrAmt').innerText = fix(result);
                var CurrentAmount = $get('ctl00_MainPlaceHolder_TabContainer1_tblpnlLRF_lstLRFPosting_ctrl6_lableCurrent').innerText.replace(myRegExp, '');
                var AggregateCredit = $get('ctl00_MainPlaceHolder_TabContainer1_tblpnlLRF_lstLRFPosting_ctrl6_lableAggtAmt').innerText.replace(myRegExp, '');
                var PostingAmount = $get('ctl00_MainPlaceHolder_TabContainer1_tblpnlLRF_lstLRFPosting_ctrl6_lablePostingAmt').innerText.replace(myRegExp, '');
                PostingResult = parseInt(CurrentAmount) + parseInt(AggregateCredit) - parseInt(result);
                $get('ctl00_MainPlaceHolder_TabContainer1_tblpnlLRF_lstLRFPosting_ctrl6_lablePostingAmt').innerText = fix(PostingResult);

            }
        }
        //Function to round the given decimal number
        //Math.pow(10,N), Based on N Value that many Places this will round off
        function fix(fixNumber) {
            var div = Math.pow(10, 2);
            fixNumber = Math.round(fixNumber * div) / div;
            return fixNumber;
        }
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
            else if(Pagename=="ARNYSIF")
            {
                strURL +="SurchargeAssesmentReview.aspx";
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
                strURL += "?SelectedValues=" + selectedValues.value + "&wID=<%= WindowName%>";
            }
            else {
                strURL += "?wID=<%= WindowName%>";
            }
            window.location.href = strURL;
        }  
    </script>

    <table>
        <tr>
            <td>
                <cc1:TabContainer ID="TabContainer1" runat="server" CssClass="VariableTabs" SkinID="tabVariable"
                    ActiveTabIndex="6">
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
                    <cc1:TabPanel runat="server" ID="tblpnlTM">
                        <HeaderTemplate>
                            <div onclick="Tabnavigation('AREA')">
                                Loss Fund Adj.
                            </div>
                        </HeaderTemplate>
                        <ContentTemplate>
                        </ContentTemplate>
                    </cc1:TabPanel>
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
                            LRF Posting
                        </HeaderTemplate>
                        <ContentTemplate>

                            <script src="../JavaScript/RetroScript.js" type="text/javascript"></script>

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
                                    <table width="100%" cellpadding="0">
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblLRFPosting" Visible="false" runat="server" CssClass="h3" Text="LRF Posting Details" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Panel ID="pnlLRF" runat="server" Height="220px" ScrollBars="Auto" Width="910px">
                                                    <asp:AISListView ID="lstLRFPosting" runat="server" OnItemDataBound="ItemDataBoundList"
                                                        OnItemEditing="EditList" OnItemUpdating="UpdateList" OnItemCanceling="CancelList">
                                                        <AlternatingItemTemplate>
                                                            <tr class="AlternatingItemTemplate">
                                                                <td align="left">
                                                                    <asp:LinkButton ID="lnkEdit" runat="server" CommandName="Edit" Text="EDIT"></asp:LinkButton>
                                                                </td>
                                                                <td align="center">
                                                                    <asp:Label ID="lblLRFID" runat="server" Text='<%# Eval("PREM ADJ LOS REIM FUND POST ID") %>'
                                                                        Visible="false"></asp:Label>
                                                                    <asp:Label ID="lblRecvableTyp" runat="server" Text='<%# Eval("TRNS_NM_TXT") %>'></asp:Label>
                                                                </td>
                                                                <td align="center">
                                                                    <asp:Label ID="lableCurrent" Text='<%# Eval("CURRENT AMOUNT") != null ? (Eval("CURRENT AMOUNT").ToString() != "" ?(decimal.Parse(Eval("CURRENT AMOUNT").ToString())).ToString("#,##0") : "0"): "0"%>'
                                                                        runat="server" ReadOnly="true"></asp:Label>
                                                                </td>
                                                                <td align="center">
                                                                    <asp:Label ID="lableAggtAmt" Text='<%# Eval("AGGR AMT") != null ? (Eval("AGGR AMT").ToString() != "" ?(decimal.Parse(Eval("AGGR AMT").ToString())).ToString("#,##0") : "0"): "0"%>'
                                                                        runat="server"></asp:Label>
                                                                </td>
                                                                <td align="center">
                                                                    <asp:Label ID="lablePriorYr" Text='<%# Eval("PRIOR YY AMT") != null ? (Eval("PRIOR YY AMT").ToString() != "" ?(decimal.Parse(Eval("PRIOR YY AMT").ToString())).ToString("#,##0") : "0"): "0"%>'
                                                                        runat="server" ReadOnly="true"></asp:Label>
                                                                </td>
                                                                <td align="center">
                                                                    <asp:Label ID="lableAdjPriorYrAmt" Text='<%# Eval("ADJ PRIOR YY AMT") != null ? (Eval("ADJ PRIOR YY AMT").ToString() != "" ?(decimal.Parse(Eval("ADJ PRIOR YY AMT").ToString())).ToString("#,##0") : "0"): "0"%>'
                                                                        runat="server"></asp:Label>
                                                                </td>
                                                                <td align="center">
                                                                    <asp:Label ID="lablePostingAmt" Text='<%# Eval("POST AMT") != null ? (Eval("POST AMT").ToString() != "" ?(decimal.Parse(Eval("POST AMT").ToString())).ToString("#,##0") : "0"): "0"%>'
                                                                        runat="server" ReadOnly="true"></asp:Label>
                                                                </td>
                                                            </tr>
                                                        </AlternatingItemTemplate>
                                                        <ItemTemplate>
                                                            <tr class="ItemTemplate">
                                                                <td align="left">
                                                                    <asp:LinkButton ID="lnkEdit" runat="server" CommandName="Edit" Text="EDIT"></asp:LinkButton>
                                                                </td>
                                                                <td align="center">
                                                                    <asp:Label ID="lblLRFID" runat="server" Text='<%# Eval("PREM ADJ LOS REIM FUND POST ID") %>'
                                                                        Visible="false"></asp:Label>
                                                                    <asp:Label ID="lblRecvableTyp" runat="server" Text='<%# Eval("TRNS_NM_TXT") %>'></asp:Label>
                                                                </td>
                                                                <td align="center">
                                                                    <asp:Label ID="lableCurrent" Text='<%# Eval("CURRENT AMOUNT") != null ? (Eval("CURRENT AMOUNT").ToString() != "" ?(decimal.Parse(Eval("CURRENT AMOUNT").ToString())).ToString("#,##0") : "0"): "0"%>'
                                                                        runat="server" ReadOnly="true"></asp:Label>
                                                                </td>
                                                                <td align="center">
                                                                    <asp:Label ID="lableAggtAmt" Text='<%# Eval("AGGR AMT") != null ? (Eval("AGGR AMT").ToString() != "" ?(decimal.Parse(Eval("AGGR AMT").ToString())).ToString("#,##0") : "0"): "0"%>'
                                                                        runat="server"></asp:Label>
                                                                </td>
                                                                <td align="center">
                                                                    <asp:Label ID="lablePriorYr" Text='<%# Eval("PRIOR YY AMT") != null ? (Eval("PRIOR YY AMT").ToString() != "" ?(decimal.Parse(Eval("PRIOR YY AMT").ToString())).ToString("#,##0") : "0"): "0"%>'
                                                                        runat="server" ReadOnly="true"></asp:Label>
                                                                </td>
                                                                <td align="center">
                                                                    <asp:Label ID="lableAdjPriorYrAmt" Text='<%# Eval("ADJ PRIOR YY AMT") != null ? (Eval("ADJ PRIOR YY AMT").ToString() != "" ?(decimal.Parse(Eval("ADJ PRIOR YY AMT").ToString())).ToString("#,##0") : "0"): "0"%>'
                                                                        runat="server"></asp:Label>
                                                                </td>
                                                                <td align="center">
                                                                    <asp:Label ID="lablePostingAmt" Text='<%# Eval("POST AMT") != null ? (Eval("POST AMT").ToString() != "" ?(decimal.Parse(Eval("POST AMT").ToString())).ToString("#,##0") : "0"): "0"%>'
                                                                        runat="server" ReadOnly="true"></asp:Label>
                                                                </td>
                                                            </tr>
                                                        </ItemTemplate>
                                                        <EditItemTemplate>
                                                            <tr class="ItemTemplate">
                                                                <td align="left">
                                                                    <asp:LinkButton ID="lnkupdate" runat="server" CommandName="Update" Text="UPDATE"></asp:LinkButton>
                                                                    <asp:LinkButton ID="lnkCancel" runat="server" CommandName="Cancel" Text="CANCEL"></asp:LinkButton>
                                                                </td>
                                                                <td align="center">
                                                                    <asp:HiddenField ID="hidPremAdjLRFID" runat="server" Value='<%# Eval("PREM ADJ LOS REIM FUND POST ID") %>' />
                                                                    <asp:Label ID="lblRecvableTyp" runat="server" Text='<%# Eval("TRNS_NM_TXT") %>'></asp:Label>
                                                                </td>
                                                                <td align="center">
                                                                    <asp:Label ID="lblCurrent" Text='<%# Eval("CURRENT AMOUNT") != null ? (Eval("CURRENT AMOUNT").ToString() != "" ?(decimal.Parse(Eval("CURRENT AMOUNT").ToString())).ToString("#,##0") : "0"): "0"%>'
                                                                        runat="server"></asp:Label>
                                                                    <%-- <cc1:MaskedEditExtender ID="MaskedEditTotalPaidIndem" runat="server" 
                                                                    TargetControlID="txtCurrent" 
                                                                     Mask="99,999,999,999.99" MessageValidatorTip="true" MaskType="Number" DisplayMoney ="None" 
                                                                    OnFocusCssClass="MaskedEditFocus"  OnInvalidCssClass="MaskedEditError" 
                                                                InputDirection="RightToLeft" AcceptNegative="Left"  AutoComplete="false"/> --%>
                                                                </td>
                                                                <td>
                                                                    <asp:AISAmountTextbox AllowNegetive="true" ID="txtAggtAmt" Text='<%# Eval("AGGR AMT")!= null ? (Eval("AGGR AMT").ToString() != "" ?(decimal.Parse(Eval("AGGR AMT").ToString())).ToString("#,##0") : "0"): "0"%>'
                                                                        runat="server" MaxLength="11" TabIndex="0"></asp:AISAmountTextbox>
                                                                    <%-- <cc1:MaskedEditExtender ID="MaskedEditExtender1" runat="server" 
                                                                    TargetControlID="txtAggtAmt" 
                                                                     Mask="99,999,999,999.99" MessageValidatorTip="true" MaskType="Number" DisplayMoney ="None" 
                                                                    OnFocusCssClass="MaskedEditFocus"  OnInvalidCssClass="MaskedEditError" 
                                                                InputDirection="RightToLeft" AcceptNegative="Left"  AutoComplete="false"/> --%>
                                                                    <cc1:FilteredTextBoxExtender ID="ftbeAggtAmt" runat="server" TargetControlID="txtAggtAmt"
                                                                        FilterType="Custom" FilterMode="ValidChars" ValidChars="-1234567890," />
                                                                </td>
                                                                <td>
                                                                    <asp:Label ID="lblPriorYr" Text='<%# Eval("PRIOR YY AMT") != null ? (Eval("PRIOR YY AMT").ToString() != "" ?(decimal.Parse(Eval("PRIOR YY AMT").ToString())).ToString("#,##0") : "0"): "0"%>'
                                                                        runat="server"></asp:Label>
                                                                    <%--<cc1:MaskedEditExtender ID="MaskedEditExtender2" runat="server" 
                                                                    TargetControlID="txtPriorYr" 
                                                                     Mask="99,999,999,999.99" MessageValidatorTip="true" MaskType="Number" DisplayMoney ="None" 
                                                                    OnFocusCssClass="MaskedEditFocus"  OnInvalidCssClass="MaskedEditError" 
                                                                InputDirection="RightToLeft" AcceptNegative="Left"  AutoComplete="false"/> --%>
                                                                </td>
                                                                <td>
                                                                    <asp:AISAmountTextbox AllowNegetive="true" ID="txtAdjPriorYrAmt" Text='<%# Eval("ADJ PRIOR YY AMT")!= null ? (Eval("ADJ PRIOR YY AMT").ToString() != "" ?(decimal.Parse(Eval("ADJ PRIOR YY AMT").ToString())).ToString("#,##0") : "0"): "0"%>'
                                                                        runat="server" MaxLength="11" TabIndex="1"></asp:AISAmountTextbox>
                                                                    <asp:HiddenField ID="hidAdjPriorYrAmt" Value='<%# Eval("ADJ PRIOR YY AMT")!= null ? (Eval("ADJ PRIOR YY AMT").ToString() != "" ?(decimal.Parse(Eval("ADJ PRIOR YY AMT").ToString())).ToString("#,##0") : "0"): "0"%>'
                                                                        runat="server"></asp:HiddenField>
                                                                    <%--<cc1:MaskedEditExtender ID="MaskedEditExtender3" runat="server" 
                                                                    TargetControlID="txtAdjPriorYrAmt" 
                                                                     Mask="99,999,999,999.99" MessageValidatorTip="true" MaskType="Number" DisplayMoney ="None" 
                                                                    OnFocusCssClass="MaskedEditFocus"  OnInvalidCssClass="MaskedEditError" 
                                                                InputDirection="RightToLeft" AcceptNegative="Left"  AutoComplete="false"/> --%>
                                                                    <cc1:FilteredTextBoxExtender ID="ftbeAdjPriorYrAmt" runat="server" TargetControlID="txtAdjPriorYrAmt"
                                                                        FilterType="Custom" FilterMode="ValidChars" ValidChars="-1234567890," />
                                                                </td>
                                                                <td>
                                                                    <asp:AISAmountTextbox AllowNegetive="true" ID="txtPostingAmt" Text='<%# Eval("POST AMT")!= null ? (Eval("POST AMT").ToString() != "" ?(decimal.Parse(Eval("POST AMT").ToString())).ToString("#,##0") : "0"): "0"%>'
                                                                        runat="server" Enabled="false" />
                                                                    <%--<cc1:MaskedEditExtender ID="MaskedEditExtender4" runat="server" 
                                                                    TargetControlID="txtPostingAmt" 
                                                                     Mask="99,999,999,999.99" MessageValidatorTip="true" MaskType="Number" DisplayMoney ="None" 
                                                                    OnFocusCssClass="MaskedEditFocus"  OnInvalidCssClass="MaskedEditError" 
                                                                InputDirection="RightToLeft" AcceptNegative="Left"  AutoComplete="false"/>--%>
                                                                    <cc1:FilteredTextBoxExtender ID="ftbePostingAmt" runat="server" TargetControlID="txtPostingAmt"
                                                                        FilterType="Custom" FilterMode="ValidChars" ValidChars="-1234567890," />
                                                                </td>
                                                            </tr>
                                                        </EditItemTemplate>
                                                        <LayoutTemplate>
                                                            <table id="tblCombinedelements" class="panelContents" width="98%">
                                                                <tr>
                                                                    <th>
                                                                    </th>
                                                                    <th align="center">
                                                                        Receivable Type
                                                                    </th>
                                                                    <th align="center">
                                                                        Current
                                                                    </th>
                                                                    <th align="center">
                                                                        Aggregate Credit
                                                                    </th>
                                                                    <th align="center">
                                                                        Prior Year
                                                                    </th>
                                                                    <th align="center">
                                                                        Adjusted Prior Year
                                                                    </th>
                                                                    <th align="center">
                                                                        Posting Amount
                                                                    </th>
                                                                </tr>
                                                                <tr id="itemPlaceholder" runat="server">
                                                                </tr>
                                                                <tfoot>
                                                                    <tr class="ItemTemplate">
                                                                        <td>
                                                                        </td>
                                                                        <td>
                                                                            Total Amount
                                                                        </td>
                                                                        <td align="center">
                                                                            <asp:Label ID="lblCurrntAmt" runat="server"></asp:Label>
                                                                        </td>
                                                                        <td align="center">
                                                                            <asp:Label ID="lblAggtCrdt" runat="server"></asp:Label>
                                                                        </td>
                                                                        <td align="center">
                                                                            <asp:Label ID="lblPriorYrAmt" runat="server"></asp:Label>
                                                                        </td>
                                                                        <td align="center">
                                                                            <asp:Label ID="lblAdjPriorYrAmt" runat="server"></asp:Label>
                                                                        </td>
                                                                        <td align="center">
                                                                            <asp:Label ID="lblPostingAmt" runat="server"></asp:Label>
                                                                        </td>
                                                                    </tr>
                                                                </tfoot>
                                                            </table>
                                                        </LayoutTemplate>
                                                        <EmptyDataTemplate>
                                                            <table id="tblCombinedelements" class="panelContents" width="98%">
                                                                <tr class="LayoutTemplate">
                                                                    <th align="center">
                                                                        Receivable Type
                                                                    </th>
                                                                    <th align="center">
                                                                        Current
                                                                    </th>
                                                                    <th align="center">
                                                                        Aggregate Credit
                                                                    </th>
                                                                    <th align="center">
                                                                        Prior Year
                                                                    </th>
                                                                    <th align="center">
                                                                        Adjusted Prior Year
                                                                    </th>
                                                                    <th align="center">
                                                                        Posting Amount
                                                                    </th>
                                                                </tr>
                                                                <table width="98%">
                                                                    <tr id="Tr1" runat="server" class="ItemTemplate">
                                                                        <td align="center" colspan="6">
                                                                            <asp:Label ID="lblEmptyMessage" Text="---No Records Found ---" Font-Bold="true" Width="600px"
                                                                                runat="server" Style="text-align: center" />
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                                <%--<tfoot>
                                                                <tr>
                                                                 <td>
                                                                    </td>
                                                                    <td>
                                                                        Total Amount
                                                                    </td>
                                                                    <td align="center">
                                                                        <asp:Label ID="lblCurrntAmt" runat="server"></asp:Label>
                                                                    </td>
                                                                    <td align="center">
                                                                        <asp:Label ID="lblAggtCrdt" runat="server"></asp:Label>
                                                                    </td>
                                                                    <td align="center">
                                                                        <asp:Label ID="lblPriorYrAmt" runat="server"></asp:Label>
                                                                    </td>
                                                                    <td align="center">
                                                                        <asp:Label ID="lblAdjPriorYrAmt" runat="server"></asp:Label>
                                                                    </td>
                                                                    <td align="center">
                                                                        <asp:Label ID="lblPostingAmt" runat="server"></asp:Label>
                                                                    </td>
                                                                    
                                                                    
                                                                   
                                                                </tr>
                                                                </tfoot>--%>
                                                            </table>
                                                        </EmptyDataTemplate>
                                                    </asp:AISListView>
                                                </asp:Panel>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <table width="98%" cellpadding="0">
                                                    <tr>
                                                        <td>
                                                        <br />
                                                        <br />
                                                        <asp:Label ID="lblnote" runat="server" Font-Bold="true" Text="Note: If adjusted prior year column changed for tax grid, please re-calc and CANCEL re-import of ILRF data for posting to be correct ."      
                                                        CssClass="h4" Visible="false"></asp:Label><br />
                                          <br />
                                                            <asp:Label ID="lblLRFPostingTax" runat="server" CssClass="h3" Text="LRF Posting Tax Details" Visible="false" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                        
                                               
                                                            <asp:Panel ID="pnlLRFTaxGrid" runat="server" Height="260px" ScrollBars="Auto" Width="910px"
                                                                Visible="false">
                                                                <asp:AISListView ID="lstLRFTaxGrid" runat="server" OnItemDataBound="lstLRFTaxGrid_ItemDataBound"
                                                                    OnItemEditing="lstLRFTaxGrid_ItemEditing" OnItemUpdating="lstLRFTaxGrid_ItemUpdate"
                                                                    OnItemCanceling="lstLRFTaxGrid_ItemCanceling" OnItemCommand="lstLRFTaxGrid_ItemCommand">
                                                                    <EditItemTemplate>
                                                                        <tr class="ItemTemplate">
                                                                            <td align="left">
                                                                                <asp:LinkButton ID="lnkupdate" runat="server" CommandName="Update" Text="UPDATE"></asp:LinkButton>
                                                                                <asp:LinkButton ID="lnkCancel" runat="server" CommandName="Cancel" Text="CANCEL"></asp:LinkButton>
                                                                            </td>
                                                                            <td align="center">
                                                                                <asp:HiddenField ID="hidPremAdjLRFID" runat="server" Value='<%# Eval("PREM_ADJ_LOS_REIM_FUND_POST_TAX_ID") %>' />
                                                                                <asp:Label ID="lblPremPostTaxId" runat="server" Text='<%# Eval("PREM_ADJ_LOS_REIM_FUND_POST_TAX_ID") %>'
                                                                                    Visible="false"></asp:Label>
                                                                                <asp:Label ID="lblTaxType" runat="server" Text='<%# Eval("TAXTYPE") %>'></asp:Label>
                                                                            </td>
                                                                            <%--<td align="center">
                                                                                <asp:Label ID="lblLOB" runat="server" Text='<%#Eval("LN_OF_BSN_TXT") %>'></asp:Label>
                                                                            </td>--%>
                                                                            <td align="center">
                                                                                <asp:Label ID="lblCurrent" Text='<%# Eval("CURR_AMT") != null ? (Eval("CURR_AMT").ToString() != "" ?(decimal.Parse(Eval("CURR_AMT").ToString())).ToString("#,##0") : "0"): "0"%>'
                                                                                    runat="server"></asp:Label>
                                                                            </td>
                                                                            <td align="center">
                                                                                <asp:Label ID="lblPriorYear" Text='<%# Eval("PRIOR_YY_AMT") != null ? (Eval("PRIOR_YY_AMT").ToString() != "" ?(decimal.Parse(Eval("PRIOR_YY_AMT").ToString())).ToString("#,##0") : "0"): "0"%>'
                                                                                    runat="server"></asp:Label>
                                                                            </td>
                                                                            <td>
                                                                                <asp:AISAmountTextbox AllowNegetive="true" ID="txtAdjPriorYrAmt" Text='<%# Eval("ADJ_PRIOR_YY_AMT")!= null ? (Eval("ADJ_PRIOR_YY_AMT").ToString() != "" ?(decimal.Parse(Eval("ADJ_PRIOR_YY_AMT").ToString())).ToString("#,##0") : "0"): "0"%>'
                                                                                    runat="server" MaxLength="11" TabIndex="4"></asp:AISAmountTextbox>
                                                                                <asp:HiddenField ID="hidAdjPriorYrAmt" Value='<%# Eval("ADJ_PRIOR_YY_AMT")!= null ? (Eval("ADJ_PRIOR_YY_AMT").ToString() != "" ?(decimal.Parse(Eval("ADJ_PRIOR_YY_AMT").ToString())).ToString("#,##0") : "0"): "0"%>'
                                                                                    runat="server"></asp:HiddenField>
                                                                                <cc1:FilteredTextBoxExtender ID="ftbeAdjPriorYrAmt" runat="server" TargetControlID="txtAdjPriorYrAmt"
                                                                                    FilterType="Custom" FilterMode="ValidChars" ValidChars="-1234567890," />
                                                                            </td>
                                                                            <td align="center">
                                                                                <asp:Label ID="lblPostingAmount" Text='<%# Eval("POST_AMT") != null ? (Eval("POST_AMT").ToString() != "" ?(decimal.Parse(Eval("POST_AMT").ToString())).ToString("#,##0") : "0"): "0"%>'
                                                                                    runat="server" Visible="false"></asp:Label>
                                                                                <asp:AISAmountTextbox AllowNegetive="true" ID="txtPostingAmt" Text='<%# Eval("POST_AMT") != null ? (Eval("POST_AMT").ToString() != "" ?(decimal.Parse(Eval("POST_AMT").ToString())).ToString("#,##0") : "0"): "0"%>'
                                                                                    runat="server" Enabled="false" />
                                                                                <cc1:FilteredTextBoxExtender ID="ftbePostingAmt" runat="server" TargetControlID="txtPostingAmt"
                                                                                    FilterType="Custom" FilterMode="ValidChars" ValidChars="-1234567890," />
                                                                            </td>
                                                                        </tr>
                                                                    </EditItemTemplate>
                                                                    <AlternatingItemTemplate>
                                                                        <tr class="AlternatingItemTemplate">
                                                                            <td align="left">
                                                                                <asp:LinkButton ID="lnkEdit" runat="server" CommandName="Edit" Text="EDIT"></asp:LinkButton>
                                                                            </td>
                                                                            <td align="center">
                                                                                <asp:Label ID="lblTaxType" runat="server" Text='<%#Eval("TAXTYPE") %>'></asp:Label>
                                                                            </td>
                                                                           <%-- <td align="center">
                                                                                <asp:Label ID="lblLOB" runat="server" Text='<%#Eval("LN_OF_BSN_TXT") %>'></asp:Label>
                                                                            </td>--%>
                                                                            <td align="center">
                                                                                <asp:Label ID="lblCurrent" Text='<%# Eval("CURR_AMT") != null ? (Eval("CURR_AMT").ToString() != "" ?(decimal.Parse(Eval("CURR_AMT").ToString())).ToString("#,##0") : "0"): "0"%>'
                                                                                    runat="server" ReadOnly="true"></asp:Label>
                                                                            </td>
                                                                            <td align="center">
                                                                                <asp:Label ID="lblPriorYear" Text='<%# Eval("PRIOR_YY_AMT") != null ? (Eval("PRIOR_YY_AMT").ToString() != "" ?(decimal.Parse(Eval("PRIOR_YY_AMT").ToString())).ToString("#,##0") : "0"): "0"%>'
                                                                                    runat="server"></asp:Label>
                                                                            </td>
                                                                            <td align="center">
                                                                                <asp:Label ID="lblAdjustedPriorYear" Text='<%# Eval("ADJ_PRIOR_YY_AMT") != null ? (Eval("ADJ_PRIOR_YY_AMT").ToString() != "" ?(decimal.Parse(Eval("ADJ_PRIOR_YY_AMT").ToString())).ToString("#,##0") : "0"): "0"%>'
                                                                                    runat="server"></asp:Label>
                                                                            </td>
                                                                            <td align="center">
                                                                                <asp:Label ID="lblPostingAmount" Text='<%# Eval("POST_AMT") != null ? (Eval("POST_AMT").ToString() != "" ?(decimal.Parse(Eval("POST_AMT").ToString())).ToString("#,##0") : "0"): "0"%>'
                                                                                    runat="server"></asp:Label>
                                                                            </td>
                                                                        </tr>
                                                                    </AlternatingItemTemplate>
                                                                    <ItemTemplate>
                                                                        <tr class="ItemTemplate">
                                                                            <td align="left">
                                                                                <asp:LinkButton ID="lnkEdit" runat="server" CommandName="Edit" Text="EDIT"></asp:LinkButton>
                                                                            </td>
                                                                            <td align="center">
                                                                                <asp:Label ID="lblTaxType" runat="server" Text='<%#Eval("TAXTYPE") %>'></asp:Label>
                                                                            </td>
                                                                           <%-- <td align="center">
                                                                                <asp:Label ID="lblLOB" runat="server" Text='<%#Eval("LN_OF_BSN_TXT") %>'></asp:Label>
                                                                            </td>--%>
                                                                            <td align="center">
                                                                                <asp:Label ID="lblCurrent" Text='<%# Eval("CURR_AMT") != null ? (Eval("CURR_AMT").ToString() != "" ?(decimal.Parse(Eval("CURR_AMT").ToString())).ToString("#,##0") : "0"): "0"%>'
                                                                                    runat="server" ReadOnly="true"></asp:Label>
                                                                            </td>
                                                                            <td align="center">
                                                                                <asp:Label ID="lblPriorYear" Text='<%# Eval("PRIOR_YY_AMT") != null ? (Eval("PRIOR_YY_AMT").ToString() != "" ?(decimal.Parse(Eval("PRIOR_YY_AMT").ToString())).ToString("#,##0") : "0"): "0"%>'
                                                                                    runat="server"></asp:Label>
                                                                            </td>
                                                                            <td align="center">
                                                                                <asp:Label ID="lblAdjustedPriorYear" Text='<%# Eval("ADJ_PRIOR_YY_AMT") != null ? (Eval("ADJ_PRIOR_YY_AMT").ToString() != "" ?(decimal.Parse(Eval("ADJ_PRIOR_YY_AMT").ToString())).ToString("#,##0") : "0"): "0"%>'
                                                                                    runat="server"></asp:Label>
                                                                            </td>
                                                                            <td align="center">
                                                                                <asp:Label ID="lblPostingAmount" Text='<%# Eval("POST_AMT") != null ? (Eval("POST_AMT").ToString() != "" ?(decimal.Parse(Eval("POST_AMT").ToString())).ToString("#,##0") : "0"): "0"%>'
                                                                                    runat="server"></asp:Label>
                                                                            </td>
                                                                        </tr>
                                                                    </ItemTemplate>
                                                                    <LayoutTemplate>
                                                                        <table id="tblCombinedelements" class="panelContents" width="98%">
                                                                            <tr>
                                                                                <th>
                                                                                </th>
                                                                                <th align="center">
                                                                                    Tax Type
                                                                                </th>
                                                                               <%-- <th align="center">
                                                                                    LOB
                                                                                </th>--%>
                                                                                <th align="center">
                                                                                    Current
                                                                                </th>
                                                                                <th align="center">
                                                                                    Prior Year
                                                                                </th>
                                                                                <th align="center">
                                                                                    Adjusted Prior Year
                                                                                </th>
                                                                                <th align="center">
                                                                                    Posting Amount
                                                                                </th>
                                                                            </tr>
                                                                            <tr id="itemPlaceholder" runat="server">
                                                                            </tr>
                                                                            <tfoot>
                                                                                <tr class="ItemTemplate">
                                                                                    <td>
                                                                                    </td>
                                                                                    <td>
                                                                                        Tax Total
                                                                                    </td>
                                                                                    <%--<td>
                                                                                    </td>--%>
                                                                                    <td align="center">
                                                                                        <asp:Label ID="lblCurrentTotal" runat="server"></asp:Label>
                                                                                    </td>
                                                                                    <%--<td align="center">
                                                                                        <asp:Label ID="lblAggregateCreditTotal" runat="server"></asp:Label>
                                                                                    </td>--%>
                                                                                    <td align="center">
                                                                                        <asp:Label ID="lblPriorYearTotal" runat="server"></asp:Label>
                                                                                    </td>
                                                                                    <td align="center">
                                                                                        <asp:Label ID="lblAdjustedPriorYearTotal" runat="server"></asp:Label>
                                                                                    </td>
                                                                                    <td align="center">
                                                                                        <asp:Label ID="lblPostingAmountTotal" runat="server"></asp:Label>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td>
                                                                                    </td>
                                                                                </tr>
                                                                                <%--<tr class="ItemTemplate" id="trTotal">
                                                                                    <td>
                                                                                    </td>
                                                                                    <td>
                                                                                        <b>Total</b>
                                                                                    </td>
                                                                                    <td></td>
                                                                                    <td align="center">
                                                                                        <asp:Label ID="lblGrandTotCurrent" runat="server" Font-Bold="true"></asp:Label>
                                                                                    </td>
                                                                                    <td align="center">
                                                                                        <asp:Label ID="lblGrandTotAggrCredit" runat="server" Font-Bold="true"></asp:Label>
                                                                                    </td>
                                                                                    <td align="center">
                                                                                        <asp:Label ID="lblGrandTotPriorYear" runat="server" Font-Bold="true"></asp:Label>
                                                                                    </td>
                                                                                    <td align="center">
                                                                                        <asp:Label ID="lblGrandTotAdjPriorYear" runat="server" Font-Bold="true"></asp:Label>
                                                                                    </td>
                                                                                    <td align="center">
                                                                                        <asp:Label ID="lblGrandTotPostingAmount" runat="server" Font-Bold="true"></asp:Label>
                                                                                    </td>
                                                                                </tr>--%>
                                                                            </tfoot>
                                                                        </table>
                                                                    </LayoutTemplate>
                                                                </asp:AISListView>
                                                            </asp:Panel>
                                                        </td>
                                                    </tr>
                                                </table>
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
                        </ContentTemplate>
                    </cc1:TabPanel>
                </cc1:TabContainer>
            </td>
        </tr>
    </table>
</asp:Content>
