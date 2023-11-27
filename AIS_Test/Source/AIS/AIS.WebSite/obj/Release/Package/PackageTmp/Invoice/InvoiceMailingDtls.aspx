<%@ Page Language="C#" MasterPageFile="~/Retro.master" AutoEventWireup="true" Inherits="Invoice_InvoicingMailingDtls"
    Title="Invoice Mailing Details" CodeBehind="InvoiceMailingDtls.aspx.cs" %>

<%@ Register Src="~/App_Shared/AccountList.ascx" TagPrefix="AL" TagName="AccountList" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeaderPlaceHolder" runat="Server">
    <asp:Label ID="lnlInvmdtls" runat="server" Text="Invoice Mailing Details" CssClass="h1"></asp:Label>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainPlaceHolder" runat="Server">

    <script language="javascript" type="text/javascript">
    
    var scrollTop1;
    
    if(Sys.WebForms.PageRequestManager != null)
    {
        Sys.WebForms.PageRequestManager.getInstance().add_beginRequest(BeginRequestHandler);
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);
    }

    function BeginRequestHandler(sender, args) 
    {
        var mef = $get('<%=pnlInvoiveDetails.ClientID%>');
        if(mef!=null)
        scrollTop1 = mef.scrollTop;
    }

    function EndRequestHandler(sender, args)
    {
        var mef = $get('<%=pnlInvoiveDetails.ClientID%>');
        if(mef!=null)
        mef.scrollTop = scrollTop1;
    }    
        
  function doKeypress(control,max){
    maxLength = parseInt(max);
    value = control.value;
     if(maxLength && value.length > maxLength-1){
          event.returnValue = false;
          maxLength = parseInt(maxLength);
     }
}
// Cancel default behavior
function doBeforePaste(control,max){
    maxLength = parseInt(max);
     if(maxLength)
     {
          event.returnValue = false;
     }
}
        // Cancel default behavior and create a new paste routine

        //function doPaste(control, max) {      Edge changes - created new function
        //    maxLength = parseInt(max);
        //    value = control.value;
        //    if (maxLength) {
        //        event.returnValue = false;
        //        maxLength = parseInt(maxLength);
        //        var oTR = control.document.selection.createRange();
        //        var iInsertLength = maxLength - value.length + oTR.text.length;
        //        var sData = window.clipboardData.getData("Text").substr(0, iInsertLength);
        //        oTR.text = sData;
        //    }
        //}

        function doPaste(control, max) {
            maxLength = parseInt(max);
            var sDataLength = event.clipboardData.getData("Text").length;
            var sData = event.clipboardData.getData("Text");
            if (sDataLength > maxLength) {
                var fData = sData.substring(0, maxLength);
                document.getElementById("<%=txtComments.ClientID%>").value = fData;
                event.returnValue = false;
            }
        }

    </script>

    <asp:ValidationSummary ID="ValSumSave" ValidationGroup="SaveInvoice" runat="server">
    </asp:ValidationSummary>
    <asp:Panel ID="pnlSearch" runat="server" DefaultButton="btnQueryInvoice">
    <table>
        <tr>
            <td valign="top">
                <table border="1">
                    <tr style="background-color: #003399; color: White">
                        <td align="center">
                            <asp:Label ID="lblParameter" runat="server" Text="Parameter" Font-Bold="true" ForeColor="White"></asp:Label>
                        </td>
                        <td align="center">
                            <asp:Label ID="lblValue" runat="server" Text="Values" Font-Bold="true" ForeColor="White"></asp:Label>
                        </td>
                        <td align="center">
                            <asp:Label ID="lblParameter1" runat="server" Text="Parameter" Font-Bold="true" ForeColor="White"></asp:Label>
                        </td>
                        <td align="center">
                            <asp:Label ID="lblFrom" runat="server" Text="From" Font-Bold="true" ForeColor="White"></asp:Label>
                        </td>
                        <td align="center">
                            <asp:Label ID="lblTo" runat="server" Text="To" Font-Bold="true" ForeColor="White"></asp:Label>
                        </td>
                    </tr>
                    <asp:Panel ID="pnlSearchParams" runat="server">
                        <tr>
                            <td>
                                <asp:Label ID="lblAccountName" runat="server" Text="Account Name" Font-Bold="true"></asp:Label>
                            </td>
                            <td>
                                <AL:AccountList ID="ddlAcctlist" runat="server" />
                                <%-- <asp:DropDownList ID="ddlAccountName" runat="server" Width="125px">
                            </asp:DropDownList>--%>
                            </td>
                            <td>
                                <asp:Label ID="lblValuationDate" runat="server" Text="Valuation Date" Font-Bold="true"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox runat="server" ID="txtValuationDateFrom" ValidationGroup="Save" Width="100px"></asp:TextBox>
                                <cc1:MaskedEditExtender ID="mskValDateFrom" runat="server" TargetControlID="txtValuationDateFrom"
                                    Mask="99/99/9999" MessageValidatorTip="true" OnFocusCssClass="MaskedEditFocus"
                                    OnInvalidCssClass="MaskedEditError" MaskType="Date" DisplayMoney="Left" AcceptNegative="Left"
                                    ErrorTooltipEnabled="True" />
                                <cc1:CalendarExtender ID="calValDateFrom" runat="server" PopupPosition="TopRight"
                                    TargetControlID="txtValuationDateFrom" PopupButtonID="imgValDateFrom" />
                                <asp:ImageButton ID="imgValDateFrom" runat="server" ImageUrl="~/images/Calendar_scheduleHS.png"
                                    CausesValidation="False" />
                                <asp:RegularExpressionValidator ID="regValuationDate" runat="server" ControlToValidate="txtValuationDateFrom"
                                    Display="Dynamic" ValidationExpression="(0?[1-9]|1[012])[- /.](0?[1-9]|[12][0-9]|3[01])[- /.](1[8-9]|2[01])\d\d"
                                    ErrorMessage="Invalid Valuation Date From" Text="*" ValidationGroup="Save"></asp:RegularExpressionValidator>
                            </td>
                            <td>
                                <asp:TextBox runat="server" ID="txtValuationDateTo" ValidationGroup="Save" Width="100px"></asp:TextBox>
                                <cc1:MaskedEditExtender ID="mskValDateTo" runat="server" TargetControlID="txtValuationDateTo"
                                    Mask="99/99/9999" MessageValidatorTip="true" OnFocusCssClass="MaskedEditFocus"
                                    OnInvalidCssClass="MaskedEditError" MaskType="Date" DisplayMoney="Left" AcceptNegative="Left"
                                    ErrorTooltipEnabled="True" />
                                <cc1:CalendarExtender ID="calValDateTo" runat="server" PopupPosition="TopRight" TargetControlID="txtValuationDateTo"
                                    PopupButtonID="imgValDateTo" />
                                <asp:ImageButton ID="imgValDateTo" runat="server" ImageUrl="~/images/Calendar_scheduleHS.png"
                                    CausesValidation="False" />
                                <asp:RegularExpressionValidator ID="regValuationDateTO" runat="server" ControlToValidate="txtValuationDateTo"
                                    Display="Dynamic" ValidationExpression="(0?[1-9]|1[012])[- /.](0?[1-9]|[12][0-9]|3[01])[- /.](1[8-9]|2[01])\d\d"
                                    ErrorMessage="Invalid Valuation Date To" Text="*" ValidationGroup="Save"></asp:RegularExpressionValidator>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblInvoiveNbr" runat="server" Text="Invoice Number" Font-Bold="true"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtInvoiceNbr" runat="server" Width="225px" ValidationGroup="Save">
                                </asp:TextBox>
                                <%--<asp:RequiredFieldValidator ID="compInvoiceType" runat="server" ControlToValidate="txtInvoiceNbr"
                                ValidationGroup="Save" Text="*" ErrorMessage="Please Enter Invoice Number."></asp:RequiredFieldValidator>--%>
                            </td>
                            <td>
                                <asp:Label ID="lblInvoiceDate" runat="server" Text="Invoice Date" Font-Bold="true"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox runat="server" ID="txtInvoiceDateFrom" ValidationGroup="Save" Width="100px"></asp:TextBox>
                                <cc1:MaskedEditExtender ID="mskInvoiceDateFrom" runat="server" TargetControlID="txtInvoiceDateFrom"
                                    Mask="99/99/9999" MessageValidatorTip="true" OnFocusCssClass="MaskedEditFocus"
                                    OnInvalidCssClass="MaskedEditError" MaskType="Date" DisplayMoney="Left" AcceptNegative="Left"
                                    ErrorTooltipEnabled="True" />
                                <cc1:CalendarExtender ID="calInvoiceDateFrom" runat="server" PopupPosition="TopRight"
                                    TargetControlID="txtInvoiceDateFrom" PopupButtonID="imgInvoiceDateFrom" />
                                <asp:ImageButton ID="imgInvoiceDateFrom" runat="server" ImageUrl="~/images/Calendar_scheduleHS.png"
                                    CausesValidation="False" />
                                <asp:RegularExpressionValidator ID="regInvoiceDateFrom" runat="server" ControlToValidate="txtInvoiceDateFrom"
                                    Display="Dynamic" ValidationExpression="(0?[1-9]|1[012])[- /.](0?[1-9]|[12][0-9]|3[01])[- /.](1[8-9]|2[01])\d\d"
                                    ErrorMessage="Invalid Invoice Date From" Text="*" ValidationGroup="Save"></asp:RegularExpressionValidator>
                            </td>
                            <td>
                                <asp:TextBox runat="server" ID="txtInvoiceDateTo" ValidationGroup="Save" Width="100px"></asp:TextBox>
                                <cc1:MaskedEditExtender ID="mskInvoiceDateTo" runat="server" TargetControlID="txtInvoiceDateTo"
                                    Mask="99/99/9999" MessageValidatorTip="true" OnFocusCssClass="MaskedEditFocus"
                                    OnInvalidCssClass="MaskedEditError" MaskType="Date" DisplayMoney="Left" AcceptNegative="Left"
                                    ErrorTooltipEnabled="True" />
                                <cc1:CalendarExtender ID="calInvoiceDateTo" runat="server" PopupPosition="TopRight"
                                    TargetControlID="txtInvoiceDateTo" PopupButtonID="imgInvoiceDateTo" />
                                <asp:ImageButton ID="imgInvoiceDateTo" runat="server" ImageUrl="~/images/Calendar_scheduleHS.png"
                                    CausesValidation="False" />
                                <asp:RegularExpressionValidator ID="regInvoiceDateTo" runat="server" ControlToValidate="txtInvoiceDateTo"
                                    Display="Dynamic" ValidationExpression="(0?[1-9]|1[012])[- /.](0?[1-9]|[12][0-9]|3[01])[- /.](1[8-9]|2[01])\d\d"
                                    ErrorMessage="Invalid Invoice Date To" Text="*" ValidationGroup="Save"></asp:RegularExpressionValidator>
                            </td>
                        </tr>
                    </asp:Panel>
                </table>
            </td>
            <td valign="bottom">
                <table>
                    <tr>
                        <td>
                        </td>
                        <br />
                        <td>
                        </td>
                        <br />
                        <td>
                            <asp:Panel ID="pnlButtons" runat="server">
                                <asp:Button ID="btnQueryInvoice" runat="server" Text="Query Invoice" Width="100px"
                                    OnClick="btnQueryInvoice_Click" ValidationGroup="Save" /><br />
                                <br />
                                <asp:Button ID="btnClear" runat="server" Text="Clear" Width="100px" OnClick="btnClear_Click" />
                            </asp:Panel>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    </asp:Panel>
    <br />
    <asp:UpdatePanel runat="server" ID="updInternal">
        <ContentTemplate>
            <asp:Panel ID="pnlInvoiveDetails" Width="910px" CssClass="content" runat="server"
                ScrollBars="Auto" Height="240px" Visible="false">
                <asp:ObjectDataSource ID="objDataSourceUWResp" runat="server" SelectMethod="GetLookUpActiveData"
                    TypeName="ZurichNA.AIS.Business.Logic.BLAccess">
                    <SelectParameters>
                        <asp:Parameter DefaultValue="ADJUSTMENT QC" Name="lookUpTypeName" Type="String" />
                    </SelectParameters>
                </asp:ObjectDataSource>
                <asp:AISListView ID="lsvInvoiceMailing" runat="server" DataKeyNames="PREM_ADJ_ID"
                    OnSelectedIndexChanging="lsvInvoiceMailing__SelectedIndexChanging" OnItemCommand="CommandList">
                    <LayoutTemplate>
                        <table id="tblLayout" class="panelContents" runat="server" width="98%">
                            <tr class="LayoutTemplate">
                                <th>
                                </th>
                                <th>
                                    Acc.No
                                </th>
                                <th width="300px">
                                    Account Name
                                </th>
                                <th>
                                    Valuation Date
                                </th>
                                <th>
                                    Invoice #
                                </th>
                                <th>
                                    Invoice Due<br />
                                    Date
                                </th>
                                <th width="150px">
                                    Invoice<br />
                                    Amount
                                </th>
                                <th>
                                    Draft Inv.<br />
                                    Date
                                </th>
                                <th>
                                    Final Inv.<br />
                                    Date
                                </th>
                                <th style="width: 30%">
                                    View Invoice PDF
                                </th>
                            </tr>
                            <tr id="itemPlaceholder" runat="server">
                            </tr>
                        </table>
                    </LayoutTemplate>
                    <EmptyDataTemplate>
                        <table id="tblLayout" class="panelContents" runat="server" width="98%">
                            <tr class="LayoutTemplate">
                                <th>
                                </th>
                                <th>
                                   Acc.No
                                </th>
                                <th >
                                    Account Name
                                </th>
                                <th>
                                    Valuation Date
                                </th>
                                <th>
                                    Invoice #
                                </th>
                                <th>
                                    Invoice Due<br />
                                    Date
                                </th>
                                <th>
                                    Invoice<br />
                                    Amount
                                </th>
                                <th>
                                    Draft Inv.<br />
                                    Date
                                </th>
                                <th>
                                    Final Inv.<br />
                                    Date
                                </th>
                                <th style="width: 30%">
                                    View Invoice PDF
                                </th>
                            </tr>
                            <tr id="itemPlaceholder" runat="server">
                            </tr>
                        </table>
                        <table width="98%">
                            <tr id="Tr3" runat="server" class="ItemTemplate">
                                <td align="center">
                                    <asp:Label ID="lblEmptyMessage" Text="--- No Records Found ---" Font-Bold="true"
                                        Width="600px" runat="server" Style="text-align: center" />
                                </td>
                            </tr>
                        </table>
                    </EmptyDataTemplate>
                    <ItemTemplate>
                        <tr class="ItemTemplate" runat="server" id="trItemTemplate">
                            <td>
                            <asp:HiddenField ID="HidHistorical" runat="server" Value='<%# Bind("HistoricalInd") %>' />
                                <asp:HiddenField ID="HidID" runat="server" Value='<%# Bind("PREM_ADJ_ID") %>' />
                                <asp:LinkButton ID="lnkSelect" CommandName="Select" runat="server" CommandArgument='<%# Bind("PREM_ADJ_ID") %>'
                                    Text="Select"></asp:LinkButton>
                            </td>
                            <td>
                                <asp:Label ID="lblCustmrID" runat="server" Text='<%# Bind("CUSTMER_ID") %>'></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblCustmr" runat="server" Text='<%# Bind("CUSTMRNM") %>'></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblValnDate" runat="server" Text='<%# Bind("VALUATION_DATE","{0:d}") %>'></asp:Label>
                            </td>
                            <td>
                                <%--<asp:Label ID="lblInvNbr" runat="server"></asp:Label>--%>
                                <%# Eval("FINAL_INV_TXT") != null ? (Eval("FINAL_INV_TXT").ToString() != "" ? (Eval("FINAL_INV_TXT")) : ((Eval("DRAFT_INV_TXT") != null ? (Eval("DRAFT_INV_TXT")) : ""))) : ((Eval("DRAFT_INV_TXT") != null ? (Eval("DRAFT_INV_TXT")) : ""))%>
                            </td>
                            <td>
                                <asp:Label ID="lblInvDt" runat="server" Text='<%# Bind("INV_DUE_DT","{0:d}") %>'></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblInvAmt" runat="server" Text='<%# Eval("INVC_AMT") != null ? (Eval("INVC_AMT").ToString() != "" ?(decimal.Parse(Eval("INVC_AMT").ToString())).ToString("#,##0") : "0"): "0"%>'></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblDraftDt" runat="server" Text='<%# Bind("DRAFT_INV_DATE","{0:d}") %>'></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblFnlDt" runat="server" Text='<%# Bind("FINAL_INV_DT","{0:d}") %>'></asp:Label>
                            </td>
                            <td>
                                <asp:LinkButton ID="lnkDraftInternal" runat="server" Text="Internal PDF" Enabled='<%#(Eval("DRFT_INTRNL_PDF_ZDW_KEY_TXT")!=null?(Eval("DRFT_INTRNL_PDF_ZDW_KEY_TXT").ToString()!=""? true:false):false)%>'
                                    CommandName="GetPDF" CommandArgument='<%# Bind("DRFT_INTRNL_PDF_ZDW_KEY_TXT") %>'></asp:LinkButton>
                                <asp:LinkButton ID="lnkDraftExternal" runat="server" Text="External PDF" Enabled='<%#(Eval("DRFT_EXTRNL_PDF_ZDW_KEY_TXT")!=null?(Eval("DRFT_EXTRNL_PDF_ZDW_KEY_TXT").ToString()!=""? true:false):false)%>'
                                    CommandName="GetPDF" CommandArgument='<%# Bind("DRFT_EXTRNL_PDF_ZDW_KEY_TXT") %>'></asp:LinkButton>
                                <asp:LinkButton ID="lnkDraftCDWorksheet" runat="server" Text="Coding" Enabled='<%#(Eval("DRFT_CD_WRKSHT_PDF_ZDW_KEY_TXT")!=null?(Eval("DRFT_CD_WRKSHT_PDF_ZDW_KEY_TXT").ToString()!=""? true:false):false)%>'
                                    CommandName="GetPDF" CommandArgument='<%# Bind("DRFT_CD_WRKSHT_PDF_ZDW_KEY_TXT") %>'></asp:LinkButton>
				<asp:LinkButton ID="lnkDraftPSWorksheet" runat="server" Text="Summary" Enabled='<%#(Eval("DRFT_PS_WRKSHT_PDF_ZDW_KEY_TXT")!=null?(Eval("DRFT_PS_WRKSHT_PDF_ZDW_KEY_TXT").ToString()!=""? true:false):false)%>'
                                    CommandName="GetPDF" CommandArgument='<%# Bind("DRFT_PS_WRKSHT_PDF_ZDW_KEY_TXT") %>'></asp:LinkButton>
                            </td>
                        </tr>
                        </tr><tr>
                    </ItemTemplate>
                    <AlternatingItemTemplate>
                        <tr class="AlternatingItemTemplate" runat="server" id="trItemTemplate">
                            <td>
                                <asp:HiddenField ID="HidID" runat="server" Value='<%# Bind("PREM_ADJ_ID") %>' />
                                <asp:HiddenField ID="HidHistorical" runat="server" Value='<%# Bind("HistoricalInd") %>' />
                                <asp:LinkButton ID="lnkSelect" CommandName="Select" runat="server" CommandArgument='<%# Bind("PREM_ADJ_ID") %>'
                                    Text="Select"></asp:LinkButton>
                            </td>
                            <td>
                                <asp:Label ID="lblCustmrID" runat="server" Text='<%# Bind("CUSTMER_ID") %>'></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblCustmr" runat="server" Text='<%# Bind("CUSTMRNM") %>'></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblValnDate" runat="server" Text='<%# Bind("VALUATION_DATE","{0:d}") %>'></asp:Label>
                            </td>
                            <td>
                                <%-- <asp:Label ID="lblInvNbr" runat="server"></asp:Label>--%>
                                <%# Eval("FINAL_INV_TXT") != null ? (Eval("FINAL_INV_TXT").ToString() != "" ? (Eval("FINAL_INV_TXT")) : ((Eval("DRAFT_INV_TXT") != null ? (Eval("DRAFT_INV_TXT")) : ""))) : ((Eval("DRAFT_INV_TXT") != null ? (Eval("DRAFT_INV_TXT")) : ""))%>
                            </td>
                            <td>
                                <asp:Label ID="lblInvDt" runat="server" Text='<%# Bind("INV_DUE_DT","{0:d}") %>'></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblInvAmt" runat="server" Text='<%# Eval("INVC_AMT") != null ? (Eval("INVC_AMT").ToString() != "" ?(decimal.Parse(Eval("INVC_AMT").ToString())).ToString("#,##0") : "0"): "0"%>'></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblDraftDt" runat="server" Text='<%# Bind("DRAFT_INV_DATE","{0:d}") %>'></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblFnlDt" runat="server" Text='<%# Bind("FINAL_INV_DT","{0:d}") %>'></asp:Label>
                            </td>
                            <td>
                                <asp:LinkButton ID="lnkDraftInternal" runat="server" Text="Internal PDF" Enabled='<%#(Eval("DRFT_INTRNL_PDF_ZDW_KEY_TXT")!=null?(Eval("DRFT_INTRNL_PDF_ZDW_KEY_TXT").ToString()!=""? true:false):false)%>'
                                    CommandName="GetPDF" CommandArgument='<%# Bind("DRFT_INTRNL_PDF_ZDW_KEY_TXT") %>'></asp:LinkButton>
                                <asp:LinkButton ID="lnkDraftExternal" runat="server" Text="External PDF" Enabled='<%#(Eval("DRFT_EXTRNL_PDF_ZDW_KEY_TXT")!=null?(Eval("DRFT_EXTRNL_PDF_ZDW_KEY_TXT").ToString()!=""? true:false):false)%>'
                                    CommandName="GetPDF" CommandArgument='<%# Bind("DRFT_EXTRNL_PDF_ZDW_KEY_TXT") %>'></asp:LinkButton>
                                <asp:LinkButton ID="lnkDraftCDWorksheet" runat="server" Text="Coding" Enabled='<%#(Eval("DRFT_CD_WRKSHT_PDF_ZDW_KEY_TXT")!=null?(Eval("DRFT_CD_WRKSHT_PDF_ZDW_KEY_TXT").ToString()!=""? true:false):false)%>'
                                    CommandName="GetPDF" CommandArgument='<%# Bind("DRFT_CD_WRKSHT_PDF_ZDW_KEY_TXT") %>'></asp:LinkButton>
   				<asp:LinkButton ID="lnkDraftPSWorksheet" runat="server" Text="Summary" Enabled='<%#(Eval("DRFT_PS_WRKSHT_PDF_ZDW_KEY_TXT")!=null?(Eval("DRFT_PS_WRKSHT_PDF_ZDW_KEY_TXT").ToString()!=""? true:false):false)%>'
                                    CommandName="GetPDF" CommandArgument='<%# Bind("DRFT_PS_WRKSHT_PDF_ZDW_KEY_TXT") %>'></asp:LinkButton>
                            </td>
                        </tr>
                        </tr><tr>
                    </AlternatingItemTemplate>
                </asp:AISListView>
            </asp:Panel>
            <div style="padding-top: 10px">
                <asp:Label ID="lblInvoicingDetails" Visible="false" runat="server" Text="Invoice Mailing Details"
                    CssClass="h2"></asp:Label>
                &nbsp;
                <asp:LinkButton ID="lbCloseDetails" Text="Close" runat="server" OnClick="lbCloseDetails_Click"
                    Visible="false" Width="60px" />
            </div>
            <asp:Panel BorderColor="Black" ID="pnlDetails" Visible="false" BorderWidth="1" Width="910px"
                runat="server">
                <table width="100%">
                    <tr>
                        <td align="right">
                            <asp:HiddenField ID="hidindex" runat="server" Value="-1" />
                        </td>
                    </tr>
                </table>
                <table width="100%">
                    <tr>
                        <td>
                            Broker:
                        </td>
                        <td>
                            <asp:Label ID="lblBroker" runat="server"></asp:Label>
                        </td>
                        <td>
                            BU /Office:
                        </td>
                        <td>
                            <asp:Label ID="lblBU" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Date Adjusted:
                        </td>
                        <td>
                            <asp:Label ID="lblDateAdjusted" runat="server"></asp:Label>
                        </td>
                        <td>
                            Draft Inv. Emailed to U/W:
                        </td>
                        <td>
                            <asp:TextBox ID="txtDraftInvEmailed" runat="server" Width="246px"></asp:TextBox>
                            <cc1:MaskedEditExtender ID="MaskedEditDraftInvEmailed" runat="server" TargetControlID="txtDraftInvEmailed"
                                Mask="99/99/9999" MessageValidatorTip="true" OnFocusCssClass="MaskedEditFocus"
                                OnInvalidCssClass="MaskedEditError" MaskType="Date" DisplayMoney="Left" AcceptNegative="Left"
                                ErrorTooltipEnabled="True" />
                            <cc1:CalendarExtender ID="CalendarDraftInvEmailed" runat="server" PopupPosition="TopRight"
                                TargetControlID="txtDraftInvEmailed" PopupButtonID="imgDraftInvEmailed" />
                            <asp:ImageButton ID="imgDraftInvEmailed" runat="server" ImageUrl="~/images/Calendar_scheduleHS.png"
                                CausesValidation="False" />
                            <asp:RegularExpressionValidator ID="regexpvalDraftInvEmailed" runat="server" ControlToValidate="txtDraftInvEmailed"
                                ValidationExpression="(0?[1-9]|1[012])[- /.](0?[1-9]|[12][0-9]|3[01])[- /.](1[8-9]|2[01])\d\d"
                                ErrorMessage="Draft Inv. Emailed to U/W Date" Text="*" ValidationGroup="SaveInvoice"></asp:RegularExpressionValidator>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Final Inv. Emailed to U/W:
                        </td>
                        <td>
                            <asp:TextBox ID="txtFinalInvEmailed" runat="server" Width="246px"></asp:TextBox>
                            <cc1:MaskedEditExtender ID="MaskedEditFinalInvEmailed" runat="server" TargetControlID="txtFinalInvEmailed"
                                Mask="99/99/9999" MessageValidatorTip="true" OnFocusCssClass="MaskedEditFocus"
                                OnInvalidCssClass="MaskedEditError" MaskType="Date" DisplayMoney="Left" AcceptNegative="Left"
                                ErrorTooltipEnabled="True" />
                            <cc1:CalendarExtender ID="CalendarFinalInvEmailed" runat="server" PopupPosition="TopRight"
                                TargetControlID="txtFinalInvEmailed" PopupButtonID="imgFinalInvEmailed" />
                            <asp:ImageButton ID="imgFinalInvEmailed" runat="server" ImageUrl="~/images/Calendar_scheduleHS.png"
                                CausesValidation="False" />
                            <asp:RegularExpressionValidator ID="regexpvalFinalInvEmailed" runat="server" ControlToValidate="txtFinalInvEmailed"
                                ValidationExpression="(0?[1-9]|1[012])[- /.](0?[1-9]|[12][0-9]|3[01])[- /.](1[8-9]|2[01])\d\d"
                                ErrorMessage="Invalid Final Inv. Emailed to U/W Date" Text="*" ValidationGroup="SaveInvoice"></asp:RegularExpressionValidator>
                        </td>
                        <td>
                            Final Invoice Mail Date:
                        </td>
                        <td>
                            <asp:TextBox ID="txtFinalEmailDate" runat="server" Width="246px"></asp:TextBox>
                            <cc1:MaskedEditExtender ID="MaskedEditFinalEmailDate" runat="server" TargetControlID="txtFinalEmailDate"
                                Mask="99/99/9999" MessageValidatorTip="true" OnFocusCssClass="MaskedEditFocus"
                                OnInvalidCssClass="MaskedEditError" MaskType="Date" DisplayMoney="Left" AcceptNegative="Left"
                                ErrorTooltipEnabled="True" />
                            <cc1:CalendarExtender ID="CalendarFinalEmailDate" runat="server" PopupPosition="TopRight"
                                TargetControlID="txtFinalEmailDate" PopupButtonID="imgFinalEmailDate" />
                            <asp:ImageButton ID="imgFinalEmailDate" runat="server" ImageUrl="~/images/Calendar_scheduleHS.png"
                                CausesValidation="False" />
                            <asp:RegularExpressionValidator ID="regexpvalFinalEmailDate" runat="server" ControlToValidate="txtFinalEmailDate"
                                ValidationExpression="(0?[1-9]|1[012])[- /.](0?[1-9]|[12][0-9]|3[01])[- /.](1[8-9]|2[01])\d\d"
                                ErrorMessage="Invalid Final Invoice Mail Date" Text="*" ValidationGroup="SaveInvoice"></asp:RegularExpressionValidator>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            U/W Response:
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlUWResponse" ValidationGroup="SaveInvoice" runat="server"
                                DataSourceID="objDataSourceUWResp" DataTextField="LookUpName" DataValueField="LookUpName"
                                AutoPostBack="true">
                            </asp:DropDownList>
                            <asp:CompareValidator ID="compareUW" runat="server" ControlToValidate="ddlUWResponse"
                                ValidationGroup="SaveInvoice" ValueToCompare="0" Text="*" ErrorMessage="Please select U/W Response"
                                Operator="NotEqual"></asp:CompareValidator>
                        </td>
                        <td>
                            U/W Response Date:
                        </td>
                        <td>
                            <asp:TextBox ID="txtUwRespDt" runat="server" Width="246px"></asp:TextBox>
                            <cc1:MaskedEditExtender ID="MaskedEditUwRespDt" runat="server" TargetControlID="txtUwRespDt"
                                Mask="99/99/9999" MessageValidatorTip="true" OnFocusCssClass="MaskedEditFocus"
                                OnInvalidCssClass="MaskedEditError" MaskType="Date" DisplayMoney="Left" AcceptNegative="Left"
                                ErrorTooltipEnabled="True" />
                            <cc1:CalendarExtender ID="CalendarUwRespDt" runat="server" PopupPosition="TopRight"
                                TargetControlID="txtUwRespDt" PopupButtonID="imgUwRespDt" />
                            <asp:ImageButton ID="imgUwRespDt" runat="server" ImageUrl="~/images/Calendar_scheduleHS.png"
                                CausesValidation="False" />
                            <asp:RegularExpressionValidator ID="regUwRespDt" runat="server" ControlToValidate="txtUwRespDt"
                                ValidationExpression="(0?[1-9]|1[012])[- /.](0?[1-9]|[12][0-9]|3[01])[- /.](1[8-9]|2[01])\d\d"
                                ErrorMessage="Invalid U/W Response Date" Text="*" ValidationGroup="SaveInvoice"></asp:RegularExpressionValidator>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Comments:
                        </td>
                        <td>
                            <asp:TextBox ID="txtComments" runat="server" TextMode="MultiLine" Width="246px"
                                onpaste="doPaste(this,500);" MaxLength="500" onkeypress="doKeypress(this,500);"
                                onbeforepaste="doBeforePaste(this,500);" Rows="2"></asp:TextBox>
                        </td>
                        <td>
                        </td>
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4" style="text-align: center">
                            <asp:Button ID="btnSave" Enabled="True" ValidationGroup="SaveInvoice" Text="Save"
                                runat="server" OnClick="btnSave_Click" />
                            &nbsp;&nbsp;&nbsp;&nbsp;
                            <asp:Button ID="btnFinal" Enabled="True" Text="Final Invoice" runat="server" OnClick="btnFinal_Click" />
                            <cc1:ConfirmButtonExtender ID="cbdFinal" runat="server" TargetControlID="btnFinal"
                                ConfirmText="Are you sure you want to process Final Invoice? " />
                        </td>
                    </tr>
                </table>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
