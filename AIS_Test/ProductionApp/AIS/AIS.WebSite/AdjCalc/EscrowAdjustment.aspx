<%@ Page Language="C#" MasterPageFile="~/Retro.master" AutoEventWireup="true" Inherits="AdjCalc_EscrowAdjustment"
    CodeBehind="EscrowAdjustment.aspx.cs" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/App_Shared/AdjustmentReviewSearch.ascx" TagPrefix="ARS" TagName="AdjustmentReviewSearch" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeaderPlaceHolder" runat="server">
    <asp:Label ID="lblAdjReview" runat="server" Text="Adjustment Review" CssClass="h1"></asp:Label>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainPlaceHolder" runat="Server">

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
            else if (Pagename == "ARBB") 
            {
                strURL += "BuBrokerReview.aspx";
            }
            if (selectedValues.value != "") {
                strURL += "?SelectedValues=" + selectedValues.value;
            }
            window.location.href = strURL;
        }  
    </script>

    <br />
    <asp:ValidationSummary ID="VSSaveEscrow" runat="server" ValidationGroup="EscrowGroup"
        Height="40px" CssClass="ValidationSummary" />
    <table>
        <tr>
            <td>
                <cc1:TabContainer ID="TabContainer1" runat="server" CssClass="VariableTabs" SkinID="tabVariable"
                    ActiveTabIndex="2">
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
                            <!--Start of Javascript Section-->

                            <script src="../JavaScript/RetroScript.js" type="text/javascript"></script>

                            <!--End of Javascript Section-->
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
                                    <asp:Label ID="lblAdjReviewEscrow" runat="server" CssClass="h3"></asp:Label>
                                    <!--Start of Escrow Data Panel Section-->
                                    <asp:Panel ID="pnlDetails" runat="server" Visible="false">
                                        <table id="tblAdjReviewEscrow" visible="true" runat="server" class="panelContents">
                                            <tr align="center">
                                                <th>
                                                </th>
                                                <th>
                                                    Policy #'s
                                                </th>
                                                <th>
                                                    PLB Months
                                                </th>
                                                <th>
                                                    Months Held
                                                </th>
                                                <th>
                                                    Divided By
                                                </th>
                                                <th nowrap>
                                                    Previous Escrow
                                                </th>
                                                <th>
                                                    Adj. Paid Loss
                                                </th>
                                                <th>
                                                    Cal. Amount
                                                </th>
                                                <th>
                                                    Adj. Amount
                                                </th>
                                                <th>
                                                    Preview
                                                </th>
                                            </tr>
                                            <tr class="ItemTemplate" style="vertical-align: middle">
                                                <td style="vertical-align: middle" runat="server" id="tdSave" nowrap>
                                                    <div id="divsave" runat="server">
                                                        <asp:LinkButton ID="lnkBtnAdjReviewEscrowSave" runat="server" Text="Save" ValidationGroup="EscrowGroup"
                                                            Width="30px" OnClick="lnkBtnAdjReviewEscrowSave_Click" ToolTip="Click here to Save" />
                                                    </div>
                                                    <div id="divUpdate" runat="server">
                                                        <asp:LinkButton ID="lnkBtnAdjReviewEscrowUpdate" runat="server" Text="Update" ValidationGroup="EscrowGroup"
                                                            Width="30px" OnClick="lnkBtnAdjReviewEscrowUpdate_Click" ToolTip="Click here to Update" />
                                                        <asp:LinkButton ID="lnkBtnAdjReviewEscrowCancel" runat="server" Text="Cancel" Width="30px"
                                                            OnClick="lnkBtnAdjReviewEscrowCancel_Click" ToolTip="Click here to Cancel" />
                                                    </div>
                                                </td>
                                                <td>
                                                    <asp:ListBox ID="lbAdjReviewscrowPolicy" runat="server" SkinID="lstAdjReview" Enabled="false"
                                                        Font-Names="Tahoma" Font-Size="8.5"></asp:ListBox>
                                                </td>
                                                <td style="vertical-align: middle">
                                                    <asp:TextBox ID="txtPLBmnts" runat="server" MaxLength="2" Enabled="false" Width="85px"></asp:TextBox>
                                                </td>
                                                <td style="vertical-align: middle">
                                                    <asp:TextBox ID="txtMnthsHeld" runat="server" MaxLength="4" Enabled="false" Width="85px"></asp:TextBox>
                                                </td>
                                                <td style="vertical-align: middle">
                                                    <asp:TextBox ID="txtDivedBy" runat="server" MaxLength="6" Enabled="false" Width="85px"></asp:TextBox>
                                                </td>
                                                <td style="vertical-align: middle">
                                                    <asp:TextBox ID="txtPrevEscrowAmt" runat="server" Enabled="false" Width="90px"></asp:TextBox>
                                                </td>
                                                <td style="vertical-align: middle">
                                                    <asp:TextBox ID="txtAdjPaidLoss" runat="server" Enabled="false" Width="90px"></asp:TextBox>
                                                </td>
                                                <td style="vertical-align: middle">
                                                    <asp:TextBox ID="txtCalAmount" runat="server" Enabled="false" Width="90px"></asp:TextBox>
                                                </td>
                                                <td style="vertical-align: middle">
                                                    <asp:AISAmountTextbox ID="txtAdjAmount" runat="server" Enabled="true" Width="90px" AllowNegetive="true"></asp:AISAmountTextbox>
                                                    <%--<cc1:FilteredTextBoxExtender ID="fltrAdjAmount" runat="server" TargetControlID="txtAdjAmount"
                                                        FilterType="Custom" FilterMode="ValidChars" ValidChars="0123456789." />--%>
                                                    <%--        <cc1:FilteredTextBoxExtender ID="ftbeNYPremiumDiscount" runat="server" TargetControlID="txtAdjAmount"
                                                        FilterType="Custom" FilterMode="ValidChars" ValidChars="-1.234567890,"/>--%>
                                                    <asp:RequiredFieldValidator ID="reqAdjAmount" Display="Dynamic" runat="server" ControlToValidate="txtAdjAmount"
                                                        ErrorMessage="Please enter Adj.Amount" ValidationGroup="EscrowGroup" Text="*" />
                                                </td>
                                                <td style="vertical-align: middle">
                                                    <asp:LinkButton ID="lnkPreview" runat="server" Text="PREVIEW" Visible="true" Width="40px"
                                                        OnClick="lnkPreview_Click" ToolTip="Click here to Preview" />
                                                </td>
                                            </tr>
                                        </table>
                                    </asp:Panel>
                                    <!--End of Escrow Daa Panel Section-->
                                    <!--Start of Escrow Empty Data Panel Section-->
                                    <asp:Panel ID="pnlAdjReviewEscrowEmptyData" runat="server" Visible="false">
                                        <table id="Table1" visible="true" runat="server" class="panelContents" width="900px">
                                            <tr align="center">
                                                <th>
                                                    Policy #'s
                                                </th>
                                                <th>
                                                    PLB Months
                                                </th>
                                                <th>
                                                    Months Held
                                                </th>
                                                <th>
                                                    Divided By
                                                </th>
                                                <th nowrap>
                                                    Previous Escrow
                                                </th>
                                                <th>
                                                    Adj. Paid Loss
                                                </th>
                                                <th>
                                                    Cal. Amount
                                                </th>
                                                <th>
                                                    Adj. Amount
                                                </th>
                                                <th>
                                                    Preview
                                                </th>
                                            </tr>
                                        </table>
                                        <table width="900px">
                                            <tr id="Tr1" runat="server" class="ItemTemplate">
                                                <td align="center">
                                                    <asp:Label ID="lblEmptyMessage" Text="---Escrow Setup Not Done for above Search ---"
                                                        Font-Bold="true" runat="server" Style="text-align: center" />
                                                </td>
                                            </tr>
                                        </table>
                                    </asp:Panel>
                                    <!--Start of Escrow Empty Data Panel Section-->
                                </ContentTemplate>
                            </asp:UpdatePanel>
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
                        </ContentTemplate>
                    </cc1:TabPanel>
                </cc1:TabContainer>
            </td>
        </tr>
    </table>
</asp:Content>
