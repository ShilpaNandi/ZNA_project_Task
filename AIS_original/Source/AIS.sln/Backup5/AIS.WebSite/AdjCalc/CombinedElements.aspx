<%@ Page Language="C#" MasterPageFile="~/Retro.master" AutoEventWireup="true" Inherits="AdjCalc_CombinedElements"
    CodeBehind="CombinedElements.aspx.cs" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/App_Shared/AdjustmentReviewSearch.ascx" TagPrefix="ARS" TagName="AdjustmentReviewSearch" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeaderPlaceHolder" runat="server">
    <asp:Label ID="lblAdjReview" runat="server" Text="Adjustment Review" CssClass="h1"></asp:Label>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainPlaceHolder" runat="Server">

    <script language="javascript" type="text/javascript">
        var scrollTop1;

        if (Sys.WebForms.PageRequestManager != null) {
            Sys.WebForms.PageRequestManager.getInstance().add_beginRequest(BeginRequestHandler);
            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);
        }

        function BeginRequestHandler(sender, args) {
            var mef = $get('<%=pnlCombinedElementsList.ClientID%>');
            if (mef != null)
                scrollTop1 = mef.scrollTop;
        }

        function EndRequestHandler(sender, args) {
            var mef = $get('<%=pnlCombinedElementsList.ClientID%>');
            if (mef != null)
                mef.scrollTop = scrollTop1;
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
            else if (Pagename == "ARNYSIF") {
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
            window.location.href = strURL;
        }  
    </script>

    <asp:ValidationSummary ID="VSSaveCE" runat="server" ValidationGroup="AdjReviewCESaveGroup"
        Height="60px" CssClass="ValidationSummary" />
    <table>
        <tr>
            <td>
                <cc1:TabContainer ID="TabContainer1" runat="server" CssClass="VariableTabs" SkinID="tabVariable"
                    ActiveTabIndex="3">
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
                    <!--Start of AdjCalc Combined Elements Tab Section-->
                    <cc1:TabPanel runat="server" ID="tblpnlCE">
                        <HeaderTemplate>
                            Comb.Elements
                        </HeaderTemplate>
                        <ContentTemplate>
                            <!--Start of JavaScript Section-->

                            <script src="../JavaScript/RetroScript.js" type="text/javascript"></script>

                            <script type="text/javascript" language="javascript">


                                //Function to round the given decimal number
                                //Math.pow(10,N), Based on N Value that many Places this will round off
                                function fix(fixNumber) {
                                    //var div=Math.pow(10,2);
                                    //fixNumber=Math.round(fixNumber*div)/div;
                                    fixNumber = Math.round(fixNumber);
                                    return fixNumber;

                                }
                                //Function to Calculate RetroSubTotal
                                //Formule:
                                //RetroSubTotal=(Basic Premium Amount+LossALAELBALCFAMOUNT)* Tax Multiplier
                                function CalculateRetroSubTotal() {
                                    varBasicPremium = $get('<%=txtBasicPremium.ClientID%>');
                                    varLALLAmount = $get('<%=txtLossALAELBALCFAMOUNT.ClientID%>');
                                    varTaxMultiPlier = $get('<%=txtTaxMultiplier.ClientID%>');
                                    varRetroSubTotal = $get('<%=txtRetroSubTotal.ClientID%>');
                                    var RetroSubTotal = 0;
                                    if (varBasicPremium.value != 0) {
                                        RetroSubTotal += parseFloat(varBasicPremium.value);
                                    }
                                    if (varLALLAmount.value != 0) {
                                        RetroSubTotal += parseFloat(varLALLAmount.value);
                                    }
                                    if (varTaxMultiPlier.value != 0) {
                                        RetroSubTotal = RetroSubTotal * parseFloat(varTaxMultiPlier.value);
                                    }

                                    varRetroSubTotal.value = fix(RetroSubTotal);
                                }
                                //Function to Calculate MaximumLessAmount Billed
                                //Formule:
                                //Maximum Less Amount Billed = Deductible Maximum Amount - (Retro Sub Total Amount + Deductible Sub Total Amount)
                                function CalculateMaximumLessAmountBilled() {

                                    var myRegExp = /,|_/g;
                                    varBasicPremium = $get('<%=txtBasicPremium.ClientID%>');
                                    varLALLAmount = $get('<%=txtLossALAELBALCFAMOUNT.ClientID %>');
                                    varTaxMultiPlier = $get('<%=txtTaxMultiplier.ClientID %>');
                                    varRetroSubTotal = $get('<%=txtRetroSubTotal.ClientID %>');
                                    varAmountsBilledMaximum = $get('<%=txtAmountsBilledMaximum.ClientID%>');
                                    varDeductibleSubTotal = $get('<%=txtDeductibleSubTotal.ClientID%>');
                                    varMaximumLessAmountBilled = $get('<%=txtMaximumLessAmountBilled.ClientID%>');
                                    varCalculatedTotal = $get('<%=txtCalculatedTotal.ClientID%>');
                                    varMaximum = $get('<%=txtMaximum.ClientID%>');
                                    varMinimum = $get('<%=txtMinimum.ClientID %>');
                                    varLabelMinimumApplied = $get('<%=lblMinimumApplied.ClientID %>');
                                    var RetroSubTotal = 0;
                                    var MaximumLessAmountBilled = 0;
                                    var RetroAndDeductibleSubTotal = 0;

                                    RetroAndDeductibleSubTotal = parseFloat(varRetroSubTotal.value.replace(myRegExp, '')) + parseFloat(varDeductibleSubTotal.value.replace(myRegExp, ''));

                                    MaximumLessAmountBilled = parseFloat(varAmountsBilledMaximum.value.replace(myRegExp, '')) - RetroAndDeductibleSubTotal;
                                    if (isNaN(MaximumLessAmountBilled) != true)
                                        varMaximumLessAmountBilled.value = fix(MaximumLessAmountBilled);
                                    if (isNaN(varAmountsBilledMaximum.value) != true)
                                        varCalculatedTotal.value = varAmountsBilledMaximum.value.replace(myRegExp, '');

                                    if ((varCalculatedTotal.value) >= (varMinimum.value.replace(myRegExp, ''))) {
                                        varMaximum = varCalculatedTotal;
                                        varLabelMinimumApplied.innerText = "Maximum Applies";
                                    }
                                    else {
                                        varMaximum = varMinimum.value;
                                        varLabelMinimumApplied.innerText = "Minimum Applies";
                                    }
                                }
                            </script>

                            <!--End of JavaScript Section-->
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
                                    <!--Start of Search Panel Section-->
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
                                    <!--End of Search Panel Section-->
                                    <br />
                                    <br />
                                    <table width="100%" cellpadding="0">
                                        <tr>
                                            <td width="60%">
                                                <table>
                                                    <tr>
                                                        <td width="50%">
                                                            <asp:Label ID="lblListHeader" runat="server" CssClass="h3" Text="RETRO AND DEDUCTIBLE" />
                                                            <!--Start of CombinedElementsList Panel Section-->
                                                            <asp:Panel ID="pnlCombinedElementsList" runat="server" Height="120px" ScrollBars="Auto">
                                                                <asp:AISListView ID="lstAdjReviewCombinedelements" runat="server">
                                                                    <AlternatingItemTemplate>
                                                                        <tr class="AlternatingItemTemplate">
                                                                            <td>
                                                                                <asp:Label ID="lblID" runat="server" Text='<%# Bind("COMB_ELEMTS_SETUP_ID") %>' Visible="false"></asp:Label><asp:Label
                                                                                    ID="lblPolicy" runat="server" Text='<%# Bind("PolicyNumber") %>'></asp:Label>
                                                                            </td>
                                                                            <td>
                                                                                <asp:Label ID="lblAuditexp" runat="server" Text='<%# Eval("AUDIT_EXPO_AMT") != null ? (Eval("AUDIT_EXPO_AMT").ToString() != "" ? (decimal.Parse(Eval("AUDIT_EXPO_AMT").ToString())).ToString("#,##0.00") : "0") : "0"%>'></asp:Label>
                                                                            </td>
                                                                            <td>
                                                                                <asp:Label ID="lblExpTyp" runat="server" Text='<%# Bind("EXPOSURETYPE")%>'></asp:Label>
                                                                            </td>
                                                                            <td>
                                                                                <asp:Label ID="lblPER" runat="server" Text='<%#Bind("PERTEXT") %>'></asp:Label>
                                                                            </td>
                                                                            <td>
                                                                                <asp:Label ID="lblRate" runat="server" Text='<%#Bind("ADJ_RT")%>'></asp:Label>
                                                                            </td>
                                                                            <td>
                                                                                <asp:Label ID="lblTotal" runat="server" Text='<%# Eval("TOT_AMT") != null ? (Eval("TOT_AMT").ToString() != "" ? (decimal.Parse(Eval("TOT_AMT").ToString())).ToString("#,##0.00") : "0") : "0"%>'></asp:Label>
                                                                            </td>
                                                                        </tr>
                                                                    </AlternatingItemTemplate>
                                                                    <ItemTemplate>
                                                                        <tr class="ItemTemplate">
                                                                            <td>
                                                                                <asp:Label ID="lblID" runat="server" Text='<%# Bind("COMB_ELEMTS_SETUP_ID") %>' Visible="false"></asp:Label><asp:Label
                                                                                    ID="lblPolicy" runat="server" Text='<%# Bind("PolicyNumber") %>'></asp:Label>
                                                                            </td>
                                                                            <td>
                                                                                <asp:Label ID="lblAuditexp" runat="server" Text='<%# Eval("AUDIT_EXPO_AMT") != null ? (Eval("AUDIT_EXPO_AMT").ToString() != "" ? (decimal.Parse(Eval("AUDIT_EXPO_AMT").ToString())).ToString("#,##0.00") : "0") : "0"%>'></asp:Label>
                                                                            </td>
                                                                            <td>
                                                                                <asp:Label ID="lblExpTyp" runat="server" Text='<%# Bind("EXPOSURETYPE")%>'></asp:Label>
                                                                            </td>
                                                                            <td>
                                                                                <asp:Label ID="lblPER" runat="server" Text='<%#Bind("PERTEXT") %>'></asp:Label>
                                                                            </td>
                                                                            <td>
                                                                                <asp:Label ID="lblRate" runat="server" Text='<%#Bind("ADJ_RT")%>'></asp:Label>
                                                                            </td>
                                                                            <td>
                                                                                <asp:Label ID="lblTotal" runat="server" Text='<%# Eval("TOT_AMT") != null ? (Eval("TOT_AMT").ToString() != "" ? (decimal.Parse(Eval("TOT_AMT").ToString())).ToString("#,##0.00") : "0") : "0"%>'></asp:Label>
                                                                            </td>
                                                                        </tr>
                                                                    </ItemTemplate>
                                                                    <LayoutTemplate>
                                                                        <table id="tblCombinedelements" class="panelContents">
                                                                            <tr>
                                                                                <th align="center" style="width: 22%">
                                                                                    Policy#
                                                                                </th>
                                                                                <th align="center" style="width: 18%">
                                                                                    Audit Exposure
                                                                                </th>
                                                                                <th align="center" style="width: 20%">
                                                                                    Exposure Type
                                                                                </th>
                                                                                <th align="center" style="width: 13%">
                                                                                    Exposure Basis
                                                                                </th>
                                                                                <th align="center" style="width: 13%">
                                                                                    Rate
                                                                                </th>
                                                                                <th align="center" style="width: 20%">
                                                                                    Total
                                                                                </th>
                                                                            </tr>
                                                                            <tr id="itemPlaceholder" runat="server">
                                                                            </tr>
                                                                        </table>
                                                                    </LayoutTemplate>
                                                                    <EmptyDataTemplate>
                                                                        <table id="tblCombinedelements" class="panelContents">
                                                                            <tr>
                                                                                <th align="center" style="width: 22%">
                                                                                    Policy#
                                                                                </th>
                                                                                <th align="center" style="width: 18%">
                                                                                    Audit Exposure
                                                                                </th>
                                                                                <th align="center" style="width: 20%">
                                                                                    Exposure Type
                                                                                </th>
                                                                                <th align="center" style="width: 13%">
                                                                                    Exposure Basis
                                                                                </th>
                                                                                <th align="center" style="width: 13%">
                                                                                    Rate
                                                                                </th>
                                                                                <th align="center" style="width: 20%">
                                                                                    Total
                                                                                </th>
                                                                            </tr>
                                                                            <tr id="Tr1" runat="server" class="ItemTemplate">
                                                                                <td align="center" colspan="6">
                                                                                    <asp:Label ID="lblEmptyMessage" Text="---No Records Found ---" Font-Bold="true" Width="600px"
                                                                                        runat="server" Style="text-align: center" />
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </EmptyDataTemplate>
                                                                </asp:AISListView>
                                                            </asp:Panel>
                                                            <!--End of CombinedElementsList Panel Section-->
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <!--Start of CalTotal Panel Section-->
                                                            <asp:Panel ID="pnlCalTotal" runat="server">
                                                                <table id="Table1" runat="server" class="panelContents" border="0" 
                                                                    width="80%">
                                                                    <tr>
                                                                        <td style="padding-right: 10px">
                                                                        </td>
                                                                        <td>
                                                                            <table width="100%">
                                                                                <tr class="ItemTemplate">
                                                                                    <td style="vertical-align: middle; font-weight: bold; text-align: left" width="70%">
                                                                                        <asp:Label ID="lblhCalTotal" runat="server" Text="Calculated Total" />
                                                                                    </td>
                                                                                    <td>
                                                                                        <asp:TextBox ID="txtCalculatedTotal" runat="server" contentEditable="false" Font-Bold="true"
                                                                                            BorderStyle="None" BorderWidth="0" BackColor="Transparent" Width="100%" MaxLength="20"></asp:TextBox>
                                                                                    </td>
                                                                                    <td style="width: 4px; background-color: White; vertical-align: middle">
                                                                                    </td>
                                                                                </tr>
                                                                                <tr class="AlternatingItemTemplate">
                                                                                    <td style="vertical-align: middle; font-weight: bold; text-align: left" width="70%">
                                                                                        <asp:Label ID="lblMinimum" runat="server" Text="Minimum" />
                                                                                    </td>
                                                                                    <td>
                                                                                        <asp:TextBox ID="txtMinimum" runat="server" contentEditable="false" Font-Bold="true"
                                                                                            BorderStyle="None" BorderWidth="0" BackColor="Transparent" Width="100%" MaxLength="20"></asp:TextBox>
                                                                                    </td>
                                                                                    <td style="width: 4px; background-color: White; vertical-align: middle">
                                                                                    </td>
                                                                                </tr>
                                                                                <tr class="ItemTemplate">
                                                                                    <td style="vertical-align: middle; font-weight: bold; text-align: left" width="70%">
                                                                                        <asp:Label ID="lblMaximum" runat="server" Text="Maximum" />
                                                                                    </td>
                                                                                    <td>
                                                                                        <asp:TextBox ID="txtMaximum" runat="server" contentEditable="false" Font-Bold="true"
                                                                                            BorderStyle="None" BorderWidth="0" BackColor="Transparent" Width="100%" MaxLength="20"></asp:TextBox>
                                                                                    </td>
                                                                                    <td style="width: 4px; background-color: White; vertical-align: middle">
                                                                                    </td>
                                                                                </tr>
                                                                                <tr class="AlternatingItemTemplate" runat="server" id="trMinimumApplied">
                                                                                    <td style="vertical-align: middle; font-weight: bold; text-align: center" width="70%">
                                                                                        <asp:Label ID="lblMinimumApplied" runat="server" ForeColor="Red" />
                                                                                    </td>
                                                                                    <td>
                                                                                        <%--<asp:TextBox ID="txtMinimumApplied" runat="server"    contentEditable="false" Font-Bold="true" BorderStyle="None"
                                                                                            BorderWidth="0" BackColor="Transparent" Width="100%" MaxLength="20"></asp:TextBox>--%>
                                                                                    </td>
                                                                                    <td style="width: 4px; background-color: White; vertical-align: middle">
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </asp:Panel>
                                                            <!--End of CalTotal Panel Section-->
                                                        </td>
                                                    </tr>
                                                </table>
                                                <td width="100%">
                                                    <asp:Label ID="lblAmountsBilled" runat="server" CssClass="h3" Text="AMOUNTS BILLED" />
                                                    <!--Start of AmountsBilled List Panel Section-->
                                                    <asp:Panel ID="pnlDetails" runat="server">
                                                        <table id="tblAmountsBilled" runat="server" class="panelContents" border="0" width="100%">
                                                            <tr class="ItemTemplate">
                                                                <td style="vertical-align: middle; font-weight: bold; text-align: left" width="70%">
                                                                </td>
                                                                <td>
                                                                    <asp:Label ID="lblAmountHeader" runat="server" Text=" AMOUNT($)" Font-Bold="true" />
                                                                </td>
                                                                <td style="width: 4px; background-color: White; vertical-align: middle">
                                                                </td>
                                                            </tr>
                                                            <tr class="AlternatingItemTemplate">
                                                                <td style="vertical-align: middle; font-weight: bold; text-align: center" width="70%">
                                                                    <asp:Label ID="lblhRetro" runat="server" Text="RETRO" ForeColor="Red" />
                                                                </td>
                                                                <td>
                                                                </td>
                                                                <td style="width: 4px; background-color: White; vertical-align: middle">
                                                                </td>
                                                            </tr>
                                                            <tr class="ItemTemplate">
                                                                <td style="vertical-align: middle; text-align: center" width="70%">
                                                                    <asp:Label ID="lblhBasicPremium" runat="server" Text="BASIC PREMIUM" />
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txtBasicPremium" ValidationGroup="AdjReviewCESaveGroup" runat="server"
                                                                        runat="server" contentEditable="false" Font-Bold="true" Enabled="false" BorderStyle="None"
                                                                        BorderWidth="0" BackColor="Transparent" Width="100%" onblur="CalculateRetroSubTotal();CalculateMaximumLessAmountBilled()"
                                                                        onkeypress="return AmountValidation(event,this)"></asp:TextBox>
                                                                    <%-- <cc1:FilteredTextBoxExtender ID="ftbeBasicPremium" runat="server" TargetControlID="txtBasicPremium"
                                                                        FilterType="Custom" FilterMode="ValidChars" ValidChars="1.234567890" />--%>
                                                                </td>
                                                                <td style="width: 4px; background-color: White; vertical-align: middle">
                                                                    <%-- <asp:RequiredFieldValidator ID="reqBasicPremium" runat="server" ControlToValidate="txtBasicPremium"
                                                                        Text="*" ValidationGroup="AdjReviewCESaveGroup" ErrorMessage="Please Enter Basic Premium" />--%>
                                                                </td>
                                                            </tr>
                                                            <tr class="AlternatingItemTemplate">
                                                                <td style="vertical-align: middle; text-align: center" width="70%">
                                                                    <asp:Label ID="lblhLossAmountHeader" runat="server" Text="LOSS,ALAE,LCF,LBA AMOUNT" />
                                                                </td>
                                                                <td align="left">
                                                                    <asp:TextBox ID="txtLossALAELBALCFAMOUNT" runat="server" contentEditable="false"
                                                                        Font-Bold="true" Enabled="false" BorderStyle="None" BorderWidth="0" BackColor="Transparent"
                                                                        Width="100%" onblur="CalculateRetroSubTotal();CalculateMaximumLessAmountBilled()"></asp:TextBox>
                                                                    <%-- <cc1:FilteredTextBoxExtender ID="ftbeLossALAELBALCFAMOUNT" runat="server" TargetControlID="txtLossALAELBALCFAMOUNT"
                                                                        FilterType="Custom" FilterMode="ValidChars" ValidChars="1.234567890" />--%>
                                                                </td>
                                                                <td style="width: 1px; background-color: White; vertical-align: middle">
                                                                    <%--<asp:RequiredFieldValidator ID="reqLossALAELBALCFAMOUNT" runat="server" ControlToValidate="txtLossALAELBALCFAMOUNT"
                                                                        Text="*" ValidationGroup="AdjReviewCESaveGroup" ErrorMessage="Please Enter Tax Loss,ALAE,LBA,LCF Amount" />--%>
                                                                </td>
                                                            </tr>
                                                            <tr class="ItemTemplate">
                                                                <td style="vertical-align: middle; text-align: center" width="70%">
                                                                    <asp:Label ID="lblhTaxMultiplier" runat="server" Text="TAX MULTIPLIER" />
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txtTaxMultiplier" runat="server" contentEditable="false" Font-Bold="true"
                                                                        Enabled="false" BorderStyle="None" BorderWidth="0" BackColor="Transparent" Width="100%"
                                                                        onblur="CalculateRetroSubTotal();CalculateMaximumLessAmountBilled()" onkeypress="return AmountValidation(event,this)"></asp:TextBox>
                                                                    <cc1:FilteredTextBoxExtender ID="ftbeTaxMultiplier" runat="server" TargetControlID="txtTaxMultiplier"
                                                                        FilterType="Custom" FilterMode="ValidChars" ValidChars="1.234567890" />
                                                                </td>
                                                                <td style="width: 4px; background-color: White; vertical-align: middle">
                                                                    <%--<asp:RequiredFieldValidator ID="reqTaxMultiplier" runat="server" ControlToValidate="txtTaxMultiplier"
                                                                        Text="*" ValidationGroup="AdjReviewCESaveGroup" ErrorMessage="Please Enter Tax Multiplier" />--%>
                                                                </td>
                                                            </tr>
                                                            <tr class="AlternatingItemTemplate">
                                                                <td style="vertical-align: middle; font-weight: bold; text-align: center" width="70%">
                                                                    <asp:Label ID="lblhRetroSubTotal" runat="server" Text="RETRO SUB TOTAL" />
                                                                </td>
                                                                <td align="left" style="font-weight: bold;">
                                                                    <asp:TextBox ID="txtRetroSubTotal" runat="server" contentEditable="false" Font-Bold="true"
                                                                        Enabled="false" BorderStyle="None" BorderWidth="0" BackColor="Transparent" Width="100%"></asp:TextBox>
                                                                </td>
                                                                <td style="width: 4px; background-color: White; vertical-align: middle">
                                                                </td>
                                                            </tr>
                                                            <tr class="ItemTemplate">
                                                                <td style="vertical-align: middle; font-weight: bold; text-align: center" width="70%">
                                                                    <asp:Label ID="lblhDeductible" runat="server" Text="DEDUCTIBLE" ForeColor="Red" />
                                                                </td>
                                                                <td align="left">
                                                                </td>
                                                                <td style="width: 4px; background-color: White; vertical-align: middle">
                                                                </td>
                                                            </tr>
                                                            <tr class="AlternatingItemTemplate">
                                                                <td style="vertical-align: middle; text-align: center" width="70%">
                                                                    <asp:Label ID="lblhMaximum" runat="server" Text="MAXIMUM" />
                                                                </td>
                                                                <td align="left">
                                                                    <asp:TextBox ID="txtAmountsBilledMaximum" runat="server" contentEditable="false"
                                                                        Font-Bold="true" Enabled="false" BorderStyle="None" BorderWidth="0" BackColor="Transparent"
                                                                        Width="100%" onblur="CalculateMaximumLessAmountBilled()" onkeypress="return AmountValidation(event,this)"></asp:TextBox>
                                                                    <%-- <cc1:FilteredTextBoxExtender ID="ftbeAmountsBilledMaximum" runat="server" TargetControlID="txtAmountsBilledMaximum"
                                                                        FilterType="Custom" FilterMode="ValidChars" ValidChars="1.234567890" />--%>
                                                                </td>
                                                                <td style="width: 4px; background-color: White; vertical-align: middle">
                                                                    <%--<asp:RequiredFieldValidator ID="reqAmountsBilledMaximum" runat="server" ControlToValidate="txtAmountsBilledMaximum"
                                                                        Text="*" ValidationGroup="AdjReviewCESaveGroup" ErrorMessage="Please Enter Maximum Amount" />--%>
                                                                </td>
                                                            </tr>
                                                            <tr class="ItemTemplate">
                                                                <td style="vertical-align: middle; text-align: center" width="70%">
                                                                    <asp:Label ID="lblhDeductibleSubTotal" runat="server" Text="DEDUCTIBLE SUB TOTAL" />
                                                                </td>
                                                                <td>
                                                                    <asp:AISAmountTextbox ID="txtDeductibleSubTotal" runat="server" AllowNegetive="true"
                                                                        Width="100%" onblur="CalculateMaximumLessAmountBilled();"></asp:AISAmountTextbox>
                                                                    <%--<cc1:FilteredTextBoxExtender ID="ftbeDeductibleSubTotal" runat="server" TargetControlID="txtDeductibleSubTotal"
                                                                        FilterType="Custom" FilterMode="ValidChars" ValidChars="1.234567890-" />--%>
                                                                    <cc1:FilteredTextBoxExtender ID="ftbeNYPremiumDiscount" runat="server" TargetControlID="txtDeductibleSubTotal"
                                                                        FilterType="Custom" FilterMode="ValidChars" ValidChars="-1234567890," />
                                                                </td>
                                                                <td style="width: 4px; background-color: White; vertical-align: middle">
                                                                    <asp:RequiredFieldValidator ID="reqDeductibleSubTotal" runat="server" ControlToValidate="txtDeductibleSubTotal"
                                                                        Text="*" ValidationGroup="AdjReviewCESaveGroup" ErrorMessage="Please Enter Deductible Sub Total" />
                                                                </td>
                                                            </tr>
                                                            <tr class="AlternatingItemTemplate">
                                                                <td style="vertical-align: middle; font-weight: bold; text-align: center" width="70%">
                                                                    <asp:Label ID="lblhMaximumLessAmountBilled" runat="server" Text="MAXIMUM LESS AMOUNT BILLED" />
                                                                </td>
                                                                <td align="left">
                                                                    <asp:TextBox ID="txtMaximumLessAmountBilled" runat="server" contentEditable="false"
                                                                        Font-Bold="true" BorderStyle="None" BorderWidth="0" BackColor="Transparent" Width="100%"></asp:TextBox>
                                                                </td>
                                                                <td style="width: 4px; background-color: White; vertical-align: middle">
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td style="vertical-align: middle; text-align: right" width="70%">
                                                                    <asp:Button ID="btnAdjReviewCESave" runat="server" Text="Save" ValidationGroup="AdjReviewCESaveGroup"
                                                                        OnClick="btnAdjReviewCESave_Click" ToolTip="Click here to Save" />
                                                                </td>
                                                                <td align="left">
                                                                    <asp:Button ID="btnAdjReviewPreview" runat="server" Text="Preview" OnClick="btnAdjReviewPreview_Click"
                                                                        ToolTip="Click here to Preview" />
                                                                </td>
                                                                <td style="width: 4px; background-color: White; vertical-align: middle">
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </asp:Panel>
                                                    <!--End of AmountsBilled List Panel Section-->
                                                </td>
                                        </tr>
                                    </table>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </ContentTemplate>
                    </cc1:TabPanel>
                    <!--End of AdjCalc Combined Elements Tab Section-->
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
