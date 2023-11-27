<%@ Page Language="C#" MasterPageFile="~/Retro.master" AutoEventWireup="true" Inherits="Invoice_InvoiceInquiry"
    Title="Invoice Inquiry" CodeBehind="InvoiceInquiry.aspx.cs" EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Src="../App_Shared/AccountList.ascx" TagName="AccountList" TagPrefix="AC1" %>
<asp:Content ID="Content2" ContentPlaceHolderID="HeaderPlaceHolder" runat="Server">
    <asp:Label ID="lnlInvmdtls" runat="server" Text="Invoice Inquiry" CssClass="h1"></asp:Label>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="MainPlaceHolder" runat="Server">
    <!--Start of Javascript Section-->

    <script src="../JavaScript/RetroScript.js" type="text/javascript"></script>

    <script language="javascript" type="text/javascript">
    
    var scrollTop1;
    
    if(Sys.WebForms.PageRequestManager != null)
    {
        Sys.WebForms.PageRequestManager.getInstance().add_beginRequest(BeginRequestHandler);
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);
    }

    function BeginRequestHandler(sender, args) 
    {
        var mef = $get('<%=pnlInvInqist.ClientID%>');
        if(mef!=null)
        scrollTop1 = mef.scrollTop;
    }

    function EndRequestHandler(sender, args)
    {
        var mef = $get('<%=pnlInvInqist.ClientID%>');
        if(mef!=null)
        mef.scrollTop = scrollTop1;
    }    
        
    function ValidateProgramTypeSearch(oSource, oArgs)
    {  
    var selAcc=$get('ctl00_MainPlaceHolder_ddlAcctlist_ddlAccountlist').value;
    var selProgramTyp=$get('<%=ddlProgramType.ClientID%>').value;
    var selCFS2Name=$get('<%=ddlCFS2Name.ClientID%>').value;
    var txtAccNo=$get('<%=txtAccountNumber.ClientID%>').value;        
    var selBroker=$get('<%=ddlBrokerName.ClientID%>').value;    
    var selBU=$get('<%=ddlBusinessUnit.ClientID%>').value;
    var txtInvoiceDate=$get('<%=txtInvoiceDate.ClientID%>').value;
    var txtValuationDate=$get('<%=txtValuationDate.ClientID%>').value;
    var txtInvoiceNumber= $get('<%=txtInvoiceNumber.ClientID%>').value;
    var result=true;
          if(selProgramTyp>0)
          {
          if(selAcc>0 || selCFS2Name>0 || selBroker>0 || selBU>0 || txtAccNo!="" || txtInvoiceDate!="" || txtValuationDate!="" || txtInvoiceNumber!="")
          result=true; 
          else
          result=false;
          }
        oArgs.IsValid = result;
    }
    function ValidateInvoiceNumberSearch(oSource, oArgs)
    {  
   
    var selAcc=$get('ctl00_MainPlaceHolder_ddlAcctlist_ddlAccountlist').value;
    var selProgramTyp=$get('<%=ddlProgramType.ClientID%>').value;
    var selCFS2Name=$get('<%=ddlCFS2Name.ClientID%>').value;
    var txtAccNo=$get('<%=txtAccountNumber.ClientID%>').value;        
    var selBroker=$get('<%=ddlBrokerName.ClientID%>').value;    
    var selBU=$get('<%=ddlBusinessUnit.ClientID%>').value;
    var txtInvoiceDate=$get('<%=txtInvoiceDate.ClientID%>').value;
    var txtValuationDate=$get('<%=txtValuationDate.ClientID%>').value;
    var txtInvoiceNumber= $get('<%=txtInvoiceNumber.ClientID%>').value;
    var result = true;

    
          if(txtInvoiceNumber!="")
          {
          if(txtInvoiceNumber.length<10)
          {
              if (txtInvoiceNumber.startsWith("RTI") || txtInvoiceNumber.startsWith("RTC") || txtInvoiceNumber.startsWith("RTV") || txtInvoiceNumber.startsWith("RTR") || txtInvoiceNumber.startsWith("RTD")) {
                  if (selAcc > 0 || txtAccNo != "")
                      result = true;
                  else
                      result = false;
              }
              else 
              {
                  if (selAcc > 0 || txtAccNo != "")
                      result = true;
                  else
                      result = false;
              }
          }
          else
          result=true;
          }
        oArgs.IsValid = result;
    }
    function ValidateSearch(oSource, oArgs)
    {  
   
    var selAcc=$get('ctl00_MainPlaceHolder_ddlAcctlist_ddlAccountlist').value;
    var selProgramTyp=$get('<%=ddlProgramType.ClientID%>').value;
    var selCFS2Name=$get('<%=ddlCFS2Name.ClientID%>').value;
    var txtAccNo=$get('<%=txtAccountNumber.ClientID%>').value;        
    var selBroker=$get('<%=ddlBrokerName.ClientID%>').value;    
    var selBU=$get('<%=ddlBusinessUnit.ClientID%>').value;
    var txtInvoiceDate=$get('<%=txtInvoiceDate.ClientID%>').value;
    var txtValuationDate=$get('<%=txtValuationDate.ClientID%>').value;
    var txtInvoiceNumber= $get('<%=txtInvoiceNumber.ClientID%>').value;
    var result=true;
      if(selProgramTyp>0 || selAcc>0 || selCFS2Name>0 || selBroker>0 || selBU>0 || txtAccNo!="" || txtInvoiceDate!="" || txtValuationDate!="" || txtInvoiceNumber!="")
          result=true; 
          else
          result=false;    
    oArgs.IsValid = result;
    }
    </script>

    <!--End of Javascript Section-->
    <asp:ValidationSummary Width="910px" ID="VSSearchInvoice" runat="server" ValidationGroup="InvoiceSearchGroup" />
    <asp:UpdatePanel ID="udpInvoice" runat="server">
        <ContentTemplate>
            <!--Start of  Invoice Inquiry Search Section -->
            <asp:Panel ID="pnlSearch" runat="server" DefaultButton="btnSearch">
            <div class="content">
                <asp:ObjectDataSource ID="ProgramTypeDataSource" runat="server" SelectMethod="GetLookUpActiveData"
                    TypeName="ZurichNA.AIS.Business.Logic.BLAccess">
                    <SelectParameters>
                        <asp:Parameter DefaultValue="PROGRAM TYPE" Name="lookUpTypeName" Type="String" />
                    </SelectParameters>
                </asp:ObjectDataSource>
                <asp:ObjectDataSource ID="ContactNamesDataSource" runat="server" SelectMethod="getPersonNames"
                    TypeName="ZurichNA.AIS.Business.Logic.AssignContactsBS">
                    <SelectParameters>
                        <asp:Parameter DefaultValue="0" Name="ContTypId" Type="Int32" />
                    </SelectParameters>
                </asp:ObjectDataSource>
                <asp:ObjectDataSource ID="BrokerDataSource" runat="server" SelectMethod="GetBrokersList"
                    TypeName="ZurichNA.AIS.Business.Logic.BrokerBS"></asp:ObjectDataSource>
                <asp:ObjectDataSource ID="BUOfficeDataSource" runat="server" SelectMethod="GetBUOffList"
                    TypeName="ZurichNA.AIS.Business.Logic.BusinessUnitOfficeBS"></asp:ObjectDataSource>
                <table width="910px">
                    <tr>
                        <td>
                            <table id="tblInvoiceinquiry" style="border-color: Black; height: 130px; text-align: center"
                                width="450px" border="1" cellpadding="0">
                                <tr style="background-color: #003399; color: White">
                                    <td align="center">
                                        <asp:Label ID="lblParameter" runat="server" Text="Parameter" Font-Bold="true"></asp:Label>
                                    </td>
                                    <td align="center">
                                        <asp:Label ID="lblValue" runat="server" Text="Value" Font-Bold="true"></asp:Label>
                                    </td>
                                </tr>
                                <asp:Panel ID="pnlSearchParams1" runat="server" >
                                    <tr>
                                        <td style="width: 130px; padding-top: 19px">
                                            <asp:Label ID="Label1" Text="Account Name" runat="server"></asp:Label>
                                        </td>
                                        <td align="left">
                                            <AC1:AccountList ID="ddlAcctlist" runat="server" />
                                            <asp:CustomValidator ID="cvSearch" runat="server" ErrorMessage="Please select atleast one parameter"
                                                ClientValidationFunction="ValidateSearch" ValidationGroup="InvoiceSearchGroup"
                                                Display="None" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="center">
                                            <asp:Label ID="lblInvoiceDate" runat="server" Text="Invoice Date"></asp:Label>
                                        </td>
                                        <td align="left">
                                            <asp:TextBox runat="server" ID="txtInvoiceDate" Width="65%" ValidationGroup="InvoiceSearchGroup"></asp:TextBox>
                                            <ajaxToolkit:MaskedEditExtender ID="mskInvoiceDate" runat="server" TargetControlID="txtInvoiceDate"
                                                Mask="99/99/9999" MessageValidatorTip="true" OnFocusCssClass="MaskedEditFocus"
                                                OnInvalidCssClass="MaskedEditError" MaskType="Date" DisplayMoney="Left" AcceptNegative="Left"
                                                ErrorTooltipEnabled="True" />
                                            <ajaxToolkit:CalendarExtender ID="calInvoiceDate" runat="server" PopupPosition="TopRight"
                                                TargetControlID="txtInvoiceDate" PopupButtonID="imgInvoiceDate" />
                                            <asp:ImageButton ID="imgInvoiceDate" runat="server" ImageUrl="~/images/Calendar_scheduleHS.png"
                                                CausesValidation="False" />
                                            <asp:RegularExpressionValidator ID="regInvoiceDate" runat="server" ControlToValidate="txtInvoiceDate"
                                                Display="Dynamic" ValidationExpression="(0?[1-9]|1[012])[- /.](0?[1-9]|[12][0-9]|3[01])[- /.](1[8-9]|2[01])\d\d"
                                                ErrorMessage="Invalid Invoice Date" Text="*" ValidationGroup="InvoiceSearchGroup"></asp:RegularExpressionValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="center">
                                            <asp:Label ID="lblValuationDate" runat="server" Text="Valuation Date"></asp:Label>
                                        </td>
                                        <td align="left">
                                            <asp:TextBox runat="server" ID="txtValuationDate" Width="65%" ValidationGroup="InvoiceSearchGroup"></asp:TextBox>
                                            <ajaxToolkit:MaskedEditExtender ID="mskValDateFrom" runat="server" TargetControlID="txtValuationDate"
                                                Mask="99/99/9999" MessageValidatorTip="true" OnFocusCssClass="MaskedEditFocus"
                                                OnInvalidCssClass="MaskedEditError" MaskType="Date" DisplayMoney="Left" AcceptNegative="Left"
                                                ErrorTooltipEnabled="True" />
                                            <ajaxToolkit:CalendarExtender ID="calValDateFrom" runat="server" PopupPosition="TopRight"
                                                TargetControlID="txtValuationDate" PopupButtonID="imgValDateFrom" />
                                            <asp:ImageButton ID="imgValDateFrom" runat="server" ImageUrl="~/images/Calendar_scheduleHS.png"
                                                CausesValidation="False" />
                                            <asp:RegularExpressionValidator ID="regValuationDate" runat="server" ControlToValidate="txtValuationDate"
                                                Display="Dynamic" ValidationExpression="(0?[1-9]|1[012])[- /.](0?[1-9]|[12][0-9]|3[01])[- /.](1[8-9]|2[01])\d\d"
                                                ErrorMessage="Invalid Valuation Date " Text="*" ValidationGroup="InvoiceSearchGroup"></asp:RegularExpressionValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="vertical-align: middle">
                                            <asp:Label ID="lblInvoiceNumber" runat="server" Text="Invoice Number"></asp:Label>
                                        </td>
                                        <td align="left">
                                            <asp:TextBox runat="server" ID="txtInvoiceNumber" Width="70%" ValidationGroup="InvoiceSearchGroup"></asp:TextBox>
                                            <asp:CustomValidator ID="cvInvSearch" runat="server" ErrorMessage="Please select Account Name "
                                                ClientValidationFunction="ValidateInvoiceNumberSearch" ValidationGroup="InvoiceSearchGroup"
                                                Display="None" />
                                        </td>
                                    </tr>
                                </asp:Panel>
                            </table>
                        </td>
                        <td>
                            <table id="tblInvInq" style="border-color: Black; height: 130px; text-align: center"
                                width="450" border="1" cellpadding="0">
                                <tr style="background-color: #003399; color: White">
                                    <td align="center" style="width: 150px">
                                        <asp:Label ID="lblParam" runat="server" Text="Parameter" Font-Bold="true"></asp:Label>
                                    </td>
                                    <td align="center">
                                        <asp:Label ID="lblVal" runat="server" Text="Value" Font-Bold="true"></asp:Label>
                                    </td>
                                </tr>
                                <asp:Panel ID="pnlSearchParams2" runat="server" >
                                    <tr>
                                        <td align="center" style="width: 130px; padding-top: 8px">
                                            <asp:Label ID="lblProgramType" runat="server" Text="Program Type"></asp:Label>
                                        </td>
                                        <td align="center">
                                            <asp:DropDownList ID="ddlProgramType" Width="100%" runat="server" DataSourceID="ProgramTypeDataSource"
                                                DataTextField="LookUpName" DataValueField="LookUpID" AutoPostBack="true">
                                            </asp:DropDownList>
                                            <asp:CustomValidator ID="CustValidatorSearch" runat="server" ErrorMessage="Please select another Parameter"
                                                ClientValidationFunction="ValidateProgramTypeSearch" ValidationGroup="InvoiceSearchGroup"
                                                Display="None" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="center" style="width: 130px; padding-top: 8px">
                                            <asp:Label ID="lblCFS2Name" runat="server" Text="CFS2 Role"></asp:Label>
                                        </td>
                                        <td align="center">
                                            <asp:DropDownList ID="ddlCFS2Name" Width="100%" DataSourceID="ContactNamesDataSource"
                                                DataTextField="FULLNAME" DataValueField="PERSON_ID" runat="server" AutoPostBack="true">
                                                <asp:ListItem>(Select)</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="center" style="width: 130px; padding-top: 9px">
                                            <asp:Label ID="lblAccountNumber" runat="server" Text="Account Number"></asp:Label>
                                        </td>
                                        <td align="center">
                                            <asp:TextBox ID="txtAccountNumber" runat="server" MaxLength="10" Width="100%"></asp:TextBox>
                                            <ajaxToolkit:FilteredTextBoxExtender ID="fltrAccNo" runat="server" TargetControlID="txtAccountNumber"
                                                FilterType="Custom" FilterMode="ValidChars" ValidChars="0123456789" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="center" style="width: 130px; padding-top: 9px">
                                            <asp:Label ID="lblBrokerName" runat="server" Text="Broker Name"></asp:Label>
                                        </td>
                                        <td align="center">
                                            <asp:DropDownList ID="ddlBrokerName" Width="100%" runat="server" AutoPostBack="true"
                                                DataSourceID="BrokerDataSource" DataTextField="FULL_NAME" DataValueField="EXTRNL_ORG_ID">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="center" style="width: 130px; padding-top: 9px">
                                            <asp:Label ID="lblBusinessUnit" runat="server" Text="Business Unit"></asp:Label>
                                        </td>
                                        <td align="center">
                                            <asp:DropDownList ID="ddlBusinessUnit" Width="100%" runat="server" AutoPostBack="true"
                                                DataSourceID="BUOfficeDataSource" DataTextField="FULL_NAME" DataValueField="INTRNL_ORG_ID">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                </asp:Panel>
                            </table>
                        </td>
                    </tr>
                </table>
                <table>
                    <tr>
                        <td align="center" style="padding-left: 375px">
                            <asp:Panel ID="pnlButtons" runat="server">
                                <asp:Button ID="btnSearch" runat="server" Text="Search" OnClick="btnSearch_Click"
                                    ValidationGroup="InvoiceSearchGroup" />
                                <asp:Button ID="btnClear" runat="server" Text="Clear" OnClick="btnClear_Click" />
                            </asp:Panel>
                        </td>
                    </tr>
                </table>
            </div>
            </asp:Panel>
            <!--Start of ListView Section-->
            <table width="100%">
                <tr>
                    <td>
                        <asp:Panel ID="pnlInvInqist" runat="server" CssClass="content" ScrollBars="Auto"
                            Height="230px" Visible="false">
                            <asp:AISListView ID="lstInvoiceInquiry" runat="server" OnSelectedIndexChanging="lstInvoiceInquiry_SelectedIndexChanging"
                                OnItemDataBound="DataBoundList" OnItemCommand="CommandList" OnSorting="lstInvoiceInquiry_Sorting"
                                DataKeyNames="PREM_ADJ_ID">
                                <LayoutTemplate>
                                    <table id="tblInvinq" class="panelContents" runat="server" width="98%">
                                        <tr class="LayoutTemplate">
                                            <th style="width: 3%">
                                                Details
                                            </th>
                                            <th style="width: 14%">
                                                Account Name
                                            </th>
                                            <th style="width: 6%">
                                                Valuation Date
                                            </th>
                                            <th style="width: 6%">
                                                Invoice #
                                            </th>
                                            <th style="width: 8%">
                                                <asp:LinkButton ID="lnkAdjStatus" runat="server" CommandName="Sort" CommandArgument="ADJUSTMENT_STATUS">Adjustment 
                                        Status</asp:LinkButton>
                                                <asp:Image ID="imgAdjStatus" runat="server" ImageUrl="~/images/Ascending.gif" ToolTip="Ascending"
                                                    Visible="false" />
                                            </th>
                                            <th style="width: 30%">
                                                Draft Invoice
                                            </th>
                                            <th style="width: 30%">
                                                Final Invoice
                                            </th>
                                        </tr>
                                        <tr id="ItemPlaceHolder" runat="server">
                                        </tr>
                                    </table>
                                </LayoutTemplate>
                                <EmptyDataTemplate>
                                    <table id="tblCombinedelements" class="panelContents">
                                        <tr class="LayoutTemplate">
                                            <th style="width: 3%">
                                                Details
                                            </th>
                                            <th style="width: 14%">
                                                Account Name
                                            </th>
                                            <th style="width: 6%">
                                                Valuation Date
                                            </th>
                                            <th style="width: 6%">
                                                Invoice #
                                            </th>
                                            <th style="width: 8%">
                                                Adjustment Status
                                            </th>
                                            <th style="width: 30%">
                                                Draft Invoice
                                            </th>
                                            <th style="width: 30%">
                                                Final Invoice
                                            </th>
                                        </tr>
                                        <tr id="Tr1" runat="server" class="ItemTemplate">
                                            <td align="center" colspan="7">
                                                <asp:Label ID="lblEmptyMessage" Text="---No Records Found ---" Font-Bold="true" runat="server"
                                                    Style="text-align: center" />
                                            </td>
                                        </tr>
                                    </table>
                                </EmptyDataTemplate>
                                <ItemTemplate>
                                    <tr class="ItemTemplate" runat="server" id="trItemTemplate">
                                        <td>
                                            <asp:LinkButton ID="lnkSelect" CommandArgument='<%# Bind("PREM_ADJ_ID") %>' CommandName="Select"
                                                runat="server" Text="Details"></asp:LinkButton>
                                        </td>
                                        <td>
                                            <%# Eval("ACCOUNT_NAME")%>
                                        </td>
                                        <td>
                                            <%# Eval("VALUATION_DATE", "{0:d}")%>
                                        </td>
                                        <td>
                                            <%# Eval("FNL_INVOICE_NUMBER") != null ? (Eval("FNL_INVOICE_NUMBER").ToString() != "" ? (Eval("FNL_INVOICE_NUMBER")) : ((Eval("DRFT_INVOICE_NUMBER") != null ? (Eval("DRFT_INVOICE_NUMBER")) : ""))) : ((Eval("DRFT_INVOICE_NUMBER") != null ? (Eval("DRFT_INVOICE_NUMBER")) : ""))%>
                                        </td>
                                        <td>
                                            <%# Eval("ADJUSTMENT_STATUS")%>
                                        </td>
                                        <td>
                                            <asp:LinkButton ID="lnkDraftInternal" runat="server" Text="Int PDF" Enabled='<%#(Eval("ADJUSTMENT_STATUS")!=null?(Eval("ADJUSTMENT_STATUS").ToString()!=""?(Eval("ADJUSTMENT_STATUS").ToString()!="CALC"?(Eval("ADJUSTMENT_STATUS").ToString()!="CANCELLED"?true:false):false):false):false)%>'
                                                CommandName="GetPDF" CommandArgument='<%# Bind("DRAFT_INTERNAL_KEY") %>'></asp:LinkButton>
                                            <asp:LinkButton ID="lnkDraftExternal" runat="server" Text="Ext PDF" Enabled='<%#(Eval("ADJUSTMENT_STATUS")!=null?(Eval("ADJUSTMENT_STATUS").ToString()!=""?(Eval("ADJUSTMENT_STATUS").ToString()!="CALC"?(Eval("ADJUSTMENT_STATUS").ToString()!="CANCELLED"?true:false):false):false):false)%>'
                                                CommandName="GetPDF" CommandArgument='<%# Bind("DRAFT_EXTERNAL_KEY") %>'></asp:LinkButton>
                                            <asp:LinkButton ID="lnkDraftCDWorksheet" runat="server" Text="Coding" Enabled='<%#(Eval("ADJUSTMENT_STATUS")!=null?(Eval("ADJUSTMENT_STATUS").ToString()!=""?(Eval("ADJUSTMENT_STATUS").ToString()!="CALC"?(Eval("ADJUSTMENT_STATUS").ToString()!="CANCELLED"?true:false):false):false):false)%>'
                                                CommandName="GetPDF" CommandArgument='<%# Bind("DRAFT_CW_KEY") %>'></asp:LinkButton>
					    <asp:LinkButton ID="lnkDraftPSWorksheet" runat="server" Text="Summary" Enabled='<%#(Eval("ADJUSTMENT_STATUS")!=null?(Eval("ADJUSTMENT_STATUS").ToString()!=""?(Eval("ADJUSTMENT_STATUS").ToString()!="CALC"?(Eval("ADJUSTMENT_STATUS").ToString()!="CANCELLED"?true:false):false):false):false)%>'
                                                CommandName="GetPDF" CommandArgument='<%# Bind("DRAFT_PS_KEY") %>'></asp:LinkButton>
                                        </td>
                                        <td>
                                            <asp:LinkButton ID="lnkFinalInternal" runat="server" Text="Int PDF" Enabled='<%#(Eval("ADJUSTMENT_STATUS")!=null?(Eval("ADJUSTMENT_STATUS").ToString()!=""? (Eval("ADJUSTMENT_STATUS").ToString()=="FINAL INVOICE"?true:(Eval("ADJUSTMENT_STATUS").ToString()=="TRANSMITTED"?true:false)):false):false)%>'
                                                CommandName="GetPDF" CommandArgument='<%# Bind("FNL_INTERNAL_KEY") %>'></asp:LinkButton>
                                            <asp:LinkButton ID="lnkFinalExternal" runat="server" Text="Ext PDF" Enabled='<%#(Eval("ADJUSTMENT_STATUS")!=null?(Eval("ADJUSTMENT_STATUS").ToString()!=""? (Eval("ADJUSTMENT_STATUS").ToString()=="FINAL INVOICE"?true:(Eval("ADJUSTMENT_STATUS").ToString()=="TRANSMITTED"?true:false)):false):false)%>'
                                                CommandName="GetPDF" CommandArgument='<%# Bind("FNL_EXTERNAL_KEY") %>'></asp:LinkButton>
                                            <asp:LinkButton ID="lnkFinalCDWorksheet" runat="server" Text="Coding" Enabled='<%#(Eval("ADJUSTMENT_STATUS")!=null?(Eval("ADJUSTMENT_STATUS").ToString()!=""? (Eval("ADJUSTMENT_STATUS").ToString()=="FINAL INVOICE"?true:(Eval("ADJUSTMENT_STATUS").ToString()=="TRANSMITTED"?true:false)):false):false)%>'
                                                CommandName="GetPDF" CommandArgument='<%# Bind("FNL_CW_KEY") %>'></asp:LinkButton>
					    <asp:LinkButton ID="lnkFinalPSWorksheet" runat="server" Text="Summary" Enabled='<%#(Eval("ADJUSTMENT_STATUS")!=null?(Eval("ADJUSTMENT_STATUS").ToString()!=""? (Eval("ADJUSTMENT_STATUS").ToString()=="FINAL INVOICE"?true:(Eval("ADJUSTMENT_STATUS").ToString()=="TRANSMITTED"?true:false)):false):false)%>'
                                                CommandName="GetPDF" CommandArgument='<%# Bind("FNL_PS_KEY") %>'></asp:LinkButton>
                                        </td>
                                    </tr>
                                </ItemTemplate>
                                <AlternatingItemTemplate>
                                    <tr class="AlternatingItemTemplate" runat="server" id="trItemTemplate">
                                        <td>
                                            <asp:LinkButton ID="lnkSelect" CommandArgument='<%# Bind("PREM_ADJ_ID") %>' CommandName="Select"
                                                runat="server" Text="Details"></asp:LinkButton>
                                        </td>
                                        <td>
                                            <%# Eval("ACCOUNT_NAME")%>
                                        </td>
                                        <td>
                                            <%# Eval("VALUATION_DATE", "{0:d}")%>
                                        </td>
                                        <td>
                                            <%# Eval("FNL_INVOICE_NUMBER") != null ? (Eval("FNL_INVOICE_NUMBER").ToString() != "" ? (Eval("FNL_INVOICE_NUMBER")) : ((Eval("DRFT_INVOICE_NUMBER") != null ? (Eval("DRFT_INVOICE_NUMBER")) : ""))) : ((Eval("DRFT_INVOICE_NUMBER") != null ? (Eval("DRFT_INVOICE_NUMBER")) : ""))%>
                                        </td>
                                        <td>
                                            <%# Eval("ADJUSTMENT_STATUS")%>
                                        </td>
                                        <td>
                                            <asp:LinkButton ID="lnkDraftInternal" runat="server" Text="Int PDF" Enabled='<%#(Eval("ADJUSTMENT_STATUS")!=null?(Eval("ADJUSTMENT_STATUS").ToString()!=""?(Eval("ADJUSTMENT_STATUS").ToString()!="CALC"?(Eval("ADJUSTMENT_STATUS").ToString()!="CANCELLED"?true:false):false):false):false)%>'
                                                CommandName="GetPDF" CommandArgument='<%# Bind("DRAFT_INTERNAL_KEY") %>'></asp:LinkButton>
                                            <asp:LinkButton ID="lnkDraftExternal" runat="server" Text="Ext PDF" Enabled='<%#(Eval("ADJUSTMENT_STATUS")!=null?(Eval("ADJUSTMENT_STATUS").ToString()!=""?(Eval("ADJUSTMENT_STATUS").ToString()!="CALC"?(Eval("ADJUSTMENT_STATUS").ToString()!="CANCELLED"?true:false):false):false):false)%>'
                                                CommandName="GetPDF" CommandArgument='<%# Bind("DRAFT_EXTERNAL_KEY") %>'></asp:LinkButton>
                                            <asp:LinkButton ID="lnkDraftCDWorksheet" runat="server" Text="Coding" Enabled='<%#(Eval("ADJUSTMENT_STATUS")!=null?(Eval("ADJUSTMENT_STATUS").ToString()!=""?(Eval("ADJUSTMENT_STATUS").ToString()!="CALC"?(Eval("ADJUSTMENT_STATUS").ToString()!="CANCELLED"?true:false):false):false):false)%>'
                                                CommandName="GetPDF" CommandArgument='<%# Bind("DRAFT_CW_KEY") %>'></asp:LinkButton>
					    <asp:LinkButton ID="lnkDraftPSWorksheet" runat="server" Text="Summary" Enabled='<%#(Eval("ADJUSTMENT_STATUS")!=null?(Eval("ADJUSTMENT_STATUS").ToString()!=""?(Eval("ADJUSTMENT_STATUS").ToString()!="CALC"?(Eval("ADJUSTMENT_STATUS").ToString()!="CANCELLED"?true:false):false):false):false)%>'
                                                CommandName="GetPDF" CommandArgument='<%# Bind("DRAFT_PS_KEY") %>'></asp:LinkButton>
                                        </td>
                                        <td>
                                            <asp:LinkButton ID="lnkFinalInternal" runat="server" Text="Int PDF" Enabled='<%#(Eval("ADJUSTMENT_STATUS")!=null?(Eval("ADJUSTMENT_STATUS").ToString()!=""? (Eval("ADJUSTMENT_STATUS").ToString()=="FINAL INVOICE"?true:(Eval("ADJUSTMENT_STATUS").ToString()=="TRANSMITTED"?true:false)):false):false)%>'
                                                CommandName="GetPDF" CommandArgument='<%# Bind("FNL_INTERNAL_KEY") %>'></asp:LinkButton>
                                            <asp:LinkButton ID="lnkFinalExternal" runat="server" Text="Ext PDF" Enabled='<%#(Eval("ADJUSTMENT_STATUS")!=null?(Eval("ADJUSTMENT_STATUS").ToString()!=""? (Eval("ADJUSTMENT_STATUS").ToString()=="FINAL INVOICE"?true:(Eval("ADJUSTMENT_STATUS").ToString()=="TRANSMITTED"?true:false)):false):false)%>'
                                                CommandName="GetPDF" CommandArgument='<%# Bind("FNL_EXTERNAL_KEY") %>'></asp:LinkButton>
                                            <asp:LinkButton ID="lnkFinalCDWorksheet" runat="server" Text="Coding" Enabled='<%#(Eval("ADJUSTMENT_STATUS")!=null?(Eval("ADJUSTMENT_STATUS").ToString()!=""? (Eval("ADJUSTMENT_STATUS").ToString()=="FINAL INVOICE"?true:(Eval("ADJUSTMENT_STATUS").ToString()=="TRANSMITTED"?true:false)):false):false)%>'
                                                CommandName="GetPDF" CommandArgument='<%# Bind("FNL_CW_KEY") %>'></asp:LinkButton>
					    <asp:LinkButton ID="lnkFinalPSWorksheet" runat="server" Text="Summary" Enabled='<%#(Eval("ADJUSTMENT_STATUS")!=null?(Eval("ADJUSTMENT_STATUS").ToString()!=""? (Eval("ADJUSTMENT_STATUS").ToString()=="FINAL INVOICE"?true:(Eval("ADJUSTMENT_STATUS").ToString()=="TRANSMITTED"?true:false)):false):false)%>'
                                                CommandName="GetPDF" CommandArgument='<%# Bind("FNL_PS_KEY") %>'></asp:LinkButton>
                                        </td>
                                    </tr>
                                </AlternatingItemTemplate>
                            </asp:AISListView>
                        </asp:Panel>
                    </td>
                </tr>
            </table>
            <!--End of ListView Section-->
            <table style="width: 910px">
                <tr>
                    <td>
                        <asp:Label ID="lblInvoiceInquiryDetails" Visible="false" runat="server" Text="Invoice Inquiry Details"
                            CssClass="h2"></asp:Label>
                        &nbsp;
                        <asp:LinkButton ID="lbCloseDetails" Text="Close" runat="server" OnClick="lbCloseDetails_Click"
                            Visible="false" Width="60px" />
                        <asp:Panel ID="pnlDetails" runat="server" BorderColor="Black" BorderWidth="1px" Visible="false"
                            Width="910px" Style="padding-top: 5">
                            <table width="100%">
                                <tr>
                                    <td style="width: 20%" align="right">
                                        Account Number:
                                    </td>
                                    <td style="width: 30%" align="left">
                                        <asp:Label ID="lblAccNo" runat="server" Font-Bold="true"></asp:Label>
                                    </td>
                                    <td style="width: 20%" align="right">
                                        Invoice Due Date:
                                    </td>
                                    <td style="width: 30%" align="left">
                                        <asp:Label ID="lblIVDate" runat="server" Font-Bold="true"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 20%" align="right">
                                        Account Name:
                                    </td>
                                    <td style="width: 30%" align="left">
                                        <asp:Label ID="lblAccName" runat="server" Font-Bold="true"></asp:Label>
                                    </td>
                                    <td style="width: 20%" align="right">
                                        Invoice Amount:
                                    </td>
                                    <td style="width: 30%" align="left">
                                        <asp:Label ID="lblInvoiceAmount" runat="server" Font-Bold="true"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 20%" align="right">
                                        CFS2 Role:
                                    </td>
                                    <td style="width: 30%" align="left">
                                        <asp:Label ID="lblCFS2role" runat="server" Font-Bold="true"></asp:Label>
                                    </td>
                                    <td style="width: 20%" align="right">
                                        Adjustmnt Status:
                                    </td>
                                    <td style="width: 30%" align="left">
                                        <asp:Label ID="lblAdjStatus" runat="server" Font-Bold="true"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 20%" align="right">
                                        Insured Contact:
                                    </td>
                                    <td style="width: 30%" align="left">
                                        <asp:Label ID="lblInsuredContatct" runat="server" Font-Bold="true"></asp:Label>
                                    </td>
                                    <%--<td style="width: 20%" align="right">
                                Policy #:
                            </td>
                            <td style="width: 30%" align="left">
                                <asp:Label ID="lblPolicyNo" runat="server" Font-Bold="true"></asp:Label>
                            </td>--%>
                                    <td style="width: 20%" align="right">
                                        Invoice #:
                                    </td>
                                    <td style="width: 30%" align="left">
                                        <asp:Label ID="lblInvoiceNo" runat="server" Font-Bold="true"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 20%" align="right">
                                        Valuation Date:
                                    </td>
                                    <td style="width: 30%" align="left">
                                        <asp:Label ID="lblVDate" runat="server" Font-Bold="true"></asp:Label>
                                    </td>
                                    <td style="width: 20%" align="right">
                                        Adjustment Status Date:
                                    </td>
                                    <td style="width: 30%" align="left">
                                        <asp:Label ID="lblDateAdjusted" runat="server" Font-Bold="true"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 20%" align="right">
                                        Broker:
                                    </td>
                                    <td style="width: 30%" align="left">
                                        <asp:Label ID="lblBroker" runat="server" Font-Bold="true"></asp:Label>
                                    </td>
                                    <%-- <td style="width: 20%" align="right">
                                Date QC:
                            </td>
                            <td style="width: 30%" align="left">
                                <asp:Label ID="lblDateQc" runat="server" Font-Bold="true"></asp:Label>
                            </td>--%>
                                    <td style="width: 20%" align="right">
                                        Final Invoice Date:
                                    </td>
                                    <td style="width: 30%" align="left">
                                        <asp:Label ID="lblFnlIvDate" runat="server" Font-Bold="true"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 20%" align="right">
                                        Broker Contact Name:
                                    </td>
                                    <td style="width: 30%" align="left">
                                        <asp:Label ID="lblBCName" runat="server" Font-Bold="true"></asp:Label>
                                    </td>
                                    <td style="width: 20%" align="right">
                                        Draft Invoice Date:
                                    </td>
                                    <td style="width: 30%" align="left">
                                        <asp:Label ID="lblDrftIVDate" runat="server" Font-Bold="true"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 20%" align="right">
                                        BU Office:
                                    </td>
                                    <td style="width: 30%" align="left">
                                        <asp:Label ID="lblBUOffice" runat="server" Font-Bold="true"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </td>
                </tr>
            </table>
            <table>
                <tr>
                    <td align="center" style="padding-left: 405px">
                        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                            <ContentTemplate>
                                <asp:Button ID="btnExpExcel" runat="server" Text="EXPORT TO EXCEL" Visible="false"
                                    OnClick="btnExpExcel_Click" UseSubmitBehavior="false" />
                            </ContentTemplate>
                            <Triggers>
                                <asp:PostBackTrigger ControlID="btnExpExcel" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </td>
                </tr>
            </table>
            <!--End of  Invoice Inquiry Search Section -->
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
