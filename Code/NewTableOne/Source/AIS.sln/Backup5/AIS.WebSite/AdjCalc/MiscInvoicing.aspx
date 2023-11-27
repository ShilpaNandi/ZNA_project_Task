<%@ Page Language="C#" MasterPageFile="~/Retro.master" AutoEventWireup="true" CodeBehind="MiscInvoicing .aspx.cs"
    Inherits="MiscInvoicing" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/App_Shared/AdjustmentReviewSearch.ascx" TagPrefix="ARS" TagName="AdjustmentReviewSearch" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeaderPlaceHolder" runat="server">
    <asp:Label ID="lblAdjReview" runat="server" Text="Adjustment Review" CssClass="h1"></asp:Label>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainPlaceHolder" runat="Server">

    <script language="javascript" type="text/javascript">


    var scrollTop1;
    
    if(Sys.WebForms.PageRequestManager !=null)
    {
        Sys.WebForms.PageRequestManager.getInstance().add_beginRequest(BeginRequestHandler);
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);
    }

    function BeginRequestHandler(sender, args) 
    {
        var mef = $get('<%=pnlMiscellaneousInvoicingList.ClientID%>');
        if(mef!=null)
        scrollTop1 = mef.scrollTop;
    }

    function EndRequestHandler(sender, args)
    {
        var mef = $get('<%=pnlMiscellaneousInvoicingList.ClientID%>');
        if(mef!=null)
        mef.scrollTop = scrollTop1;
    }      
    
    //Function to Redirect to differenet Tabs
     
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
        function EnableDisable(lblPolreq,reqsym,reqnbr,reqmod) 
        {
            var Polreq=$get(lblPolreq).value;
            if(Polreq.trim() == "True")
            {
             ValidatorEnable($get(reqsym),true);
             ValidatorEnable($get(reqnbr),true);
             ValidatorEnable($get(reqmod),true);
             
            }
            else
            {
             ValidatorEnable($get(reqsym),false);
             ValidatorEnable($get(reqnbr),false);
             ValidatorEnable($get(reqmod),false);
            }
        }
              
    </script>

    <asp:ValidationSummary ID="VSSaveMiscInvoice" runat="server" ValidationGroup="MiscInvoiceSaveGroup"
        Height="40px" CssClass="ValidationSummary" />
    <asp:ValidationSummary ID="VSEditMiscInvoice" runat="server" ValidationGroup="MiscInvoiceEditGroup"
        CssClass="ValidationSummary" />
    <table>
        <tr>
            <td>
                <cc1:TabContainer ID="TabContainer1" runat="server" CssClass="VariableTabs" SkinID="tabVariable"
                    ActiveTabIndex="5">
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
                    <!--Start of Miscellaneous Invoicing Tab Section-->
                    <cc1:TabPanel runat="server" ID="tblpnlmscInvL">
                        <HeaderTemplate>
                            Misc.Invoicing
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
                                    <asp:ObjectDataSource ID="PremAdjMiscInvoiceTypeListDataSource" runat="server" SelectMethod="getPremAdjMiscInvoiceData"
                                        TypeName="ZurichNA.AIS.Business.Logic.PostingTransactionTypeBS"></asp:ObjectDataSource>
                                         
                                                 <asp:Label ID="lblnote" runat="server" Font-Bold="true" Text="Note: If Misc.posting is added, updated or disabled, Please recalculate to include entry in adjustment."      
                                         CssClass="h4"></asp:Label><br />
                                         <br />
                                               
                                    <asp:Label ID="lblPremAdjMiscInvoiceDetails" runat="server" CssClass="h3"></asp:Label>
                                    <!--Start of ListView Panel Section-->
                                    <asp:Panel ID="pnlMiscellaneousInvoicingList" Width="910px" runat="server" CssClass="content"
                                        ScrollBars="Auto" Height="220px">
                                        <asp:AISListView ID="lstMiscellaneousInvoicing" runat="server" InsertItemPosition="FirstItem"
                                            OnItemCommand="lstMiscellaneousInvoicing_ItemCommand" OnItemDataBound="lstMiscellaneousInvoicing_DataBoundList"
                                            OnItemEditing="lstMiscellaneousInvoicing_ItemEdit" OnItemCanceling="lstMiscellaneousInvoicing_ItemCancel"
                                            OnItemUpdating="lstMiscellaneousInvoicing_ItemUpdating">
                                            <LayoutTemplate>
                                                <table id="lstMITable" class="panelExtContents" runat="server" width="98%">
                                                    <tr class="LayoutTemplate">
                                                        <th>
                                                            Select
                                                        </th>
                                                        <th>
                                                            Item
                                                        </th>
                                                        <th>
                                                            Amount
                                                        </th>
                                                        <th>
                                                            Policy Number
                                                        </th>
                                                        <th>
                                                            Posting
                                                        </th>
                                                        <th>
                                                            Adj.Sum
                                                        </th>
                                                        <th>
                                                            Disable
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
                                                            Item
                                                        </th>
                                                        <th>
                                                            Amount
                                                        </th>
                                                        <th>
                                                            Policy Number
                                                        </th>
                                                        <th>
                                                            Posting
                                                        </th>
                                                        <th>
                                                            Adj.Sum
                                                        </th>
                                                        <th>
                                                            Disable
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
                                                        <asp:LinkButton ID="lblItemEdit" Enabled='<%# Eval("ACTV_IND")%>' CommandName="Edit"
                                                            Text="Edit" runat="server" Visible="true" Width="40px" ToolTip="Click here to Edit" />
                                                    </td>
                                                    <td align="center">
                                                        <asp:Label ID="lblPremAdjMiscInvoiceID" Width="265px" Visible="false" runat="server"
                                                            Text='<%# Bind("PREM_ADJ_MISC_INVC_ID") %>'></asp:Label><asp:Label ID="lblPostTrnsType"
                                                                Width="265px" Visible="true" runat="server" Text='<%# Eval("POSTTRNSTYPE")%>'></asp:Label>
                                                    </td>
                                                    <td align="center">
                                                        <%# Eval("POST_AMT") != null ? (Eval("POST_AMT").ToString() != "" ? (decimal.Parse(Eval("POST_AMT").ToString())).ToString("#,##0") : "0") : "0"%>
                                                    </td>
                                                    <td align="center">
                                                        <%# Eval("POLICYNUMBER")%>
                                                    </td>
                                                    <td>
                                                        <%# Eval("MISC_POSTS_IND").ToString() == "True" ? "YES" : "NO"%>
                                                    </td>
                                                    <td>
                                                        <%# Eval("ADJ_SUMRY_POST_FLAG_IND").ToString()=="True"?"YES":"NO"%>
                                                    </td>
                                                    <td>
                                                        <asp:HiddenField ID="hidActive" runat="server" Value='<%# Bind("ACTV_IND")%>' />
                                                        <asp:ImageButton ID="imgDisable" CommandArgument='<%# Bind("PREM_ADJ_MISC_INVC_ID") %>'
                                                            runat="server" ImageUrl='<%# Eval("ACTV_IND").ToString()=="True"?"~/images/disabled.GIF":"~/images/enabled.GIF" %>'>
                                                        </asp:ImageButton>
                                                    </td>
                                                </tr>
                                            </AlternatingItemTemplate>
                                            <ItemTemplate>
                                                <tr id="trItemTemplate" runat="server" class="ItemTemplate">
                                                    <td>
                                                        <asp:LinkButton ID="lblItemEdit" Enabled='<%# Eval("ACTV_IND")%>' CommandName="Edit"
                                                            Text="Edit" runat="server" Visible="true" Width="40px" ToolTip="Click here to Edit" />
                                                    </td>
                                                    <td align="center">
                                                        <asp:Label ID="lblPremAdjMiscInvoiceID" Width="265px" Visible="false" runat="server"
                                                            Text='<%# Bind("PREM_ADJ_MISC_INVC_ID") %>'></asp:Label><asp:Label ID="lblPostTrnsType"
                                                                Width="265px" Visible="true" runat="server" Text='<%# Eval("POSTTRNSTYPE")%>'></asp:Label>
                                                    </td>
                                                    <td align="center">
                                                        <%# Eval("POST_AMT") != null ? (Eval("POST_AMT").ToString() != "" ? (decimal.Parse(Eval("POST_AMT").ToString())).ToString("#,##0") : "0") : "0"%>
                                                    </td>
                                                   <td align="center">
                                                        <%# Eval("POLICYNUMBER")%>
                                                    </td>
                                                    <td>
                                                        <%# Eval("MISC_POSTS_IND").ToString() == "True" ? "YES" : "NO"%>
                                                    </td>
                                                    <td>
                                                        <%# Eval("ADJ_SUMRY_POST_FLAG_IND").ToString() == "True" ? "YES" : "NO"%>
                                                    </td>
                                                    <td>
                                                        <asp:HiddenField ID="hidActive" runat="server" Value='<%# Bind("ACTV_IND")%>' />
                                                        <asp:ImageButton ID="imgDisable" CommandArgument='<%# Bind("PREM_ADJ_MISC_INVC_ID") %>'
                                                            runat="server" ToolTip='<%#Eval("ACTV_IND").ToString()=="True"?"Click here to Disable":"Click here to Enable" %>'
                                                            ImageUrl='<%# Eval("ACTV_IND").ToString()=="True"?"~/images/disabled.GIF":"~/images/enabled.GIF" %>'>
                                                        </asp:ImageButton>
                                                    </td>
                                                </tr>
                                            </ItemTemplate>
                                            <InsertItemTemplate>
                                                <tr class="ItemTemplate">
                                                    <td>
                                                        <asp:LinkButton ID="lbItemSave" CommandName="Save" Text="Save" runat="server" Visible="true"
                                                            Width="40px" ValidationGroup="MiscInvoiceSaveGroup" ToolTip="Click here to Save" />
                                                    </td>
                                                    <td align="center">
                                                        <asp:DropDownList ID="ddlMiscInvoiceTypelist" runat="server" DataSourceID="PremAdjMiscInvoiceTypeListDataSource"
                                                            DataTextField="TRANS_TXT" DataValueField="POST_TRANS_TYP_ID" AutoPostBack="true"
                                                            OnSelectedIndexChanged="ddlMiscInvoiceTypelist_SelectedIndexChanged">
                                                        </asp:DropDownList>
                                                         <asp:HiddenField ID="hfPolreq"  runat="server" ></asp:HiddenField>
                                                        <asp:CompareValidator ID="CompareMiscInvoiceTypelist" runat="server" ControlToValidate="ddlMiscInvoiceTypelist"
                                                            ErrorMessage="Please select Misc Invoice Item" Font-Size="Medium" Font-Names="Arial"
                                                            Text="*" Display="Dynamic" Operator="NotEqual" ValueToCompare="0" ValidationGroup="MiscInvoiceSaveGroup"></asp:CompareValidator>
                                                    </td>
                                                    <td align="center">
                                                        <asp:AISAmountTextbox ID="txtAmount" runat="server" AllowNegetive="true" ValidationGroup="MiscInvoiceSaveGroup"
                                                            Width="105px" />
                                                        <%--<cc1:FilteredTextBoxExtender ID="fltrAmount" runat="server" TargetControlID="txtAmount"
                                                            FilterType="Custom" FilterMode="ValidChars" ValidChars="0123456789.,-_" />--%>
                                                        <asp:RequiredFieldValidator ID="reqAmount" Display="Dynamic" runat="server" ControlToValidate="txtAmount"
                                                            ErrorMessage="Please Enter Amount" ValidationGroup="MiscInvoiceSaveGroup" Text="*" />
                                                           <%--<cc1:FilteredTextBoxExtender ID="ftbeNYPremiumDiscount" runat="server" TargetControlID="txtAmount"
                                                        FilterType="Custom" FilterMode="ValidChars" ValidChars="-1.234567890,"/>--%>
                                                    </td>
                                                    <td align="center">
                                                        <asp:TextBox ID="txtSym" runat="server" MaxLength="3" ValidationGroup="MiscInvoiceSaveGroup"
                                                            Width="40px" onkeypress="return toUpperCase(event,this)"  />
                                                        <asp:RequiredFieldValidator ID="reqSym" Display="Dynamic" runat="server" ControlToValidate="txtSym"
                                                            ErrorMessage="Please Enter Policy Symbol" ValidationGroup="MiscInvoiceSaveGroup"
                                                            Text="*" />
                                                        <asp:RegularExpressionValidator ID="regPolicySymbol" runat="server" ControlToValidate="txtSym"
                                                            Text="*" Display="Dynamic" ValidationGroup="MiscInvoiceSaveGroup" ErrorMessage="Please enter minimum two alpha characters for Policy Symbol"
                                                            ValidationExpression=".{2}.*" />
                                                        <cc1:FilteredTextBoxExtender TargetControlID="txtSym" FilterType="LowercaseLetters,UppercaseLetters"
                                                            ID="ftePolicySymbol" runat="server" />
                                                       
                                                        <asp:TextBox ID="txtNbr" runat="server" MaxLength="8" ValidationGroup="MiscInvoiceSaveGroup"
                                                            Width="60px" />
                                                        <asp:RequiredFieldValidator ID="reqNbr" Display="Dynamic" runat="server" ControlToValidate="txtNbr"
                                                            ErrorMessage="Please Enter Policy Number" ValidationGroup="MiscInvoiceSaveGroup"
                                                            Text="*" />
                                                        <asp:RegularExpressionValidator ID="regularPolicyNumber" runat="server" ControlToValidate="txtNbr"
                                                            Text="*" Display="Dynamic" ValidationGroup="MiscInvoiceSaveGroup" ErrorMessage="Please Enter minimum seven alphanumeric digits for Policy Number"
                                                            ValidationExpression=".{7}.*" />
                                                        <cc1:FilteredTextBoxExtender TargetControlID="txtNbr" FilterType="Numbers,LowercaseLetters,UppercaseLetters"
                                                            ID="ftePolicyNumber" runat="server" />
                                                        <asp:TextBox ID="txtModulus" runat="server" MaxLength="2" ValidationGroup="MiscInvoiceSaveGroup"
                                                            Width="30px" />
                                                        <asp:RequiredFieldValidator ID="reqModulus" Display="Dynamic" runat="server" ControlToValidate="txtModulus"
                                                            ErrorMessage="Please Enter Policy Module" ValidationGroup="MiscInvoiceSaveGroup"
                                                            Text="*" />
                                                        <cc1:FilteredTextBoxExtender TargetControlID="txtModulus" FilterType="Numbers,LowercaseLetters,UppercaseLetters" ID="ftePolicyModulus"
                                                            runat="server" />
                                                        <asp:RegularExpressionValidator ID="RegularPolicyMod" runat="server" ControlToValidate="txtModulus"
                                                            Text="*" Display="Dynamic" ValidationGroup="MiscInvoiceSaveGroup" ErrorMessage="Please enter minimum two alphanumeric characters for Policy Module"
                                                            ValidationExpression=".{2}.*" />
                                                       
                                                    </td>
                                                    <td>
                                                    </td>
                                                    <td>
                                                    </td>
                                                    <td>
                                                    </td>
                                                </tr>
                                            </InsertItemTemplate>
                                            <EditItemTemplate>
                                                <tr class="ItemTemplate">
                                                    <td>
                                                        <asp:LinkButton ID="lbMiscInvoiceUpdate" CommandName="Update" Text="Update" runat="server"
                                                            Visible="true" Width="40px" ValidationGroup="MiscInvoiceEditGroup" ToolTip="Click here to Update" />
                                                        <asp:LinkButton ID="lbMiscInvoiceCancel" CommandName="Cancel" runat="server" Text="Cancel"
                                                            Visible="true" Width="40px" ToolTip="Click here to Cancel" />
                                                    </td>
                                                    <td align="center">
                                                        <asp:Label ID="lblPremAdjMiscInvoiceID" Width="165px" Visible="false" runat="server"
                                                            Text='<%# Bind("PREM_ADJ_MISC_INVC_ID") %>'></asp:Label><asp:Label ID="lblPostTrnsTypeID"
                                                                Width="165px" Visible="false" runat="server" Text='<%# Bind("POST_TRANS_TYP_ID") %>'></asp:Label>
                                                        <asp:DropDownList ID="ddlMiscInvoiceTypelist" runat="server"  OnSelectedIndexChanged="ddlMiscInvoiceTypelistEdit_SelectedIndexChanged"
                                                            AutoPostBack="true">
                                                        </asp:DropDownList>
                                                         <asp:HiddenField ID="hfPolreq" Value='<%# Bind("POL_REQR_IND")%>' runat="server" ></asp:HiddenField>
                                                        <asp:CompareValidator ID="CompareMiscInvoiceTypelist" runat="server" ControlToValidate="ddlMiscInvoiceTypelist"
                                                            ErrorMessage="Please select Misc Invoice Item" Text="*" Display="Dynamic" Operator="NotEqual"
                                                            ValueToCompare="0" ValidationGroup="MiscInvoiceEditGroup"></asp:CompareValidator>
                                                    </td>
                                                    <td align="center">
                                                        <asp:AISAmountTextbox ID="txtAmount" runat="server"  
                                                        Text='<%# Eval("POST_AMT") != null ? (Eval("POST_AMT").ToString() != "" ?(decimal.Parse(Eval("POST_AMT").ToString())).ToString("#,##0") : "0"): "0"%>'
                                                            ValidationGroup="MiscInvoiceEditGroup" Width="105px" AllowNegetive="true" />
                                                       <%-- <cc1:FilteredTextBoxExtender ID="fltrAmount" runat="server" TargetControlID="txtAmount"
                                                            FilterType="Custom" FilterMode="ValidChars" ValidChars="0123456789.$,_-" />--%>
                                                        <asp:RequiredFieldValidator ID="reqAmount" Display="Dynamic" runat="server" ControlToValidate="txtAmount"
                                                            ErrorMessage="Please Enter Amount" ValidationGroup="MiscInvoiceEditGroup" Text="*" />
                                                            <%--<cc1:FilteredTextBoxExtender ID="ftbeNYPremiumDiscount" runat="server" TargetControlID="txtAmount"
                                                        FilterType="Custom" FilterMode="ValidChars" ValidChars="-1.234567890,"/>--%>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtSym" runat="server" MaxLength="3" Text='<%#Bind("POL_SYM_TXT")%>'
                                                            ValidationGroup="MiscInvoiceEditGroup" Width="40px" onkeypress="return toUpperCase(event,this)"  />
                                                        <asp:RequiredFieldValidator ID="reqSym" Display="Dynamic" runat="server" ControlToValidate="txtSym"
                                                            ErrorMessage="Please Enter Policy Symbol" ValidationGroup="MiscInvoiceEditGroup"
                                                            Text="*" />
                                                        <asp:RegularExpressionValidator ID="regPolicySymbol" runat="server" ControlToValidate="txtSym"
                                                            Text="*" Display="Dynamic" ValidationGroup="MiscInvoiceEditGroup" ErrorMessage="Please enter minimum two alpha characters for Policy Symbol"
                                                            ValidationExpression=".{2}.*" />
                                                        <cc1:FilteredTextBoxExtender TargetControlID="txtSym" FilterType="LowercaseLetters,UppercaseLetters"
                                                            ID="ftePolicySymbol" runat="server" />
                                                        
                                                        <asp:TextBox ID="txtNbr" runat="server" MaxLength="8" Text='<%#Bind("POL_NBR_TXT")%>'
                                                            ValidationGroup="MiscInvoiceEditGroup" Width="60px" />
                                                        <asp:RequiredFieldValidator ID="reqNbr" Display="Dynamic" runat="server" ControlToValidate="txtNbr"
                                                            ErrorMessage="Please Enter Policy Number" ValidationGroup="MiscInvoiceEditGroup"
                                                            Text="*" />
                                                        <asp:RegularExpressionValidator ID="regularPolicyNumber" runat="server" ControlToValidate="txtNbr"
                                                            Text="*" Display="Dynamic" ValidationGroup="MiscInvoiceEditGroup" ErrorMessage="Please Enter minimum seven alphanumeric digits for Policy Number"
                                                            ValidationExpression=".{7}.*" />
                                                        <cc1:FilteredTextBoxExtender TargetControlID="txtNbr" FilterType="Numbers,LowercaseLetters,UppercaseLetters"
                                                            ID="ftePolicyNumber" runat="server" />
                                                        <asp:TextBox ID="txtModulus" runat="server" MaxLength="2" Text='<%#Bind("POL_MODULUS_TXT")%>'
                                                            ValidationGroup="MiscInvoiceEditGroup" Width="30px" />
                                                        <asp:RequiredFieldValidator ID="reqModulus" Display="Dynamic" runat="server" ControlToValidate="txtModulus"
                                                            ErrorMessage="Please Enter Policy Module" ValidationGroup="MiscInvoiceEditGroup"
                                                            Text="*" />
                                                        <cc1:FilteredTextBoxExtender TargetControlID="txtModulus" FilterType="Numbers,LowercaseLetters,UppercaseLetters" ID="ftePolicyModulus"
                                                            runat="server" />
                                                        <asp:RegularExpressionValidator ID="RegularPolicyMod" runat="server" ControlToValidate="txtModulus"
                                                            Text="*" Display="Dynamic" ValidationGroup="MiscInvoiceEditGroup" ErrorMessage="Please enter minimum two alphanumeric characters for Policy Module"
                                                            ValidationExpression=".{2}.*" />
                                                    </td>
                                                    <td>
                                                    </td>
                                                    <td>
                                                    </td>
                                                    <td>
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
                    <!--End of Miscellaneous Invoicing Tab Section-->
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
