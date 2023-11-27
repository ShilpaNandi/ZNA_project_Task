<%@ Page Language="C#" MasterPageFile="~/Retro.master" AutoEventWireup="true" Inherits="AdjCalc_InvoicingDashboard"
    Title="Untitled Page" CodeBehind="InvoicingDashboard.aspx.cs" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/App_Shared/AccountList.ascx" TagPrefix="AL" TagName="AccountList" %>
<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.4000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304"
    Namespace="CrystalDecisions.Web" TagPrefix="CR" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeaderPlaceHolder" runat="Server">
    <asp:Label ID="GAILabel" runat="server" Text="Adjustment Invoicing Dashboard" CssClass="h1"></asp:Label>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainPlaceHolder" runat="Server">
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
        // to check for count of program periods going to be recalculated
        function checkforRecalc(spanChk, LSI, ILRF) {
            var theBox = spanChk;
            xState = theBox.checked;
            var intcount = $get('<% =hidCofirmCount.ClientID%>').value;
            var LSIcount = $get('<% =hidLSICount.ClientID%>').value;
            var ILRFcount = $get('<% =hidILRFCount.ClientID%>').value;
            if (xState == true) {
                intcount++;
                if (LSI == 'yes') {
                    LSIcount++;
                }
                if (ILRF == 'yes') {
                    ILRFcount++;
                }
            }
            else {
                intcount--;
                if (LSI == 'yes') {
                    LSIcount--;
                }
                if (ILRF == 'yes') {
                    ILRFcount--;
                }
            }
            $get('<% =hidCofirmCount.ClientID%>').value = intcount;
            $get('<% =hidLSICount.ClientID%>').value = LSIcount;
            $get('<% =hidILRFCount.ClientID%>').value = ILRFcount;

        }
        //to check for reclaculation confirmation from the user
        function Cofirmcalc() {
            if ($get('<% =hidCofirmCount.ClientID%>').value > 0) {
                if (confirm('Are you sure you want recalculate the ProgramPeriod?')) {
                    if ($get('<% =hidLSICount.ClientID%>').value > 0) {
                        if ($get('<% =hidPlbInd.ClientID%>').value == "1") 
                        {
                            if (confirm('Would you like to re-import PLB data?')) 
                            {
                                $get('<%=hidConfirm.ClientID%>').value = "yes";
                            }
                        }
                        if (confirm('Would you like to import LSI CHF data?')) {
                            $get('<%=hidConfirmCHF.ClientID%>').value = "yes";
                        }
                    }
                    if ($get('<% =hidILRFCount.ClientID%>').value > 0) {
                        //if (confirm('Would you like to re-import ILRF data?')) 
                        if (confirm('Importing ILRF data removes previous allocation of priors and aggregate credits.  Review and re-enter as necessary')) 
                        {
                            $get('<%=hidConfirmILRF.ClientID%>').value = "yes";
                        }
                    }
                }

                else {
                    return false;
                }
            }
            return true;
        }

        function BPNumberPopup() {
            $get('<%=btnBPNumberClose.ClientID%>').click();
            $get('<%=btnBPNumber.ClientID%>').click();
        }

        function ILRFOtherAmountPopup() {
            $get('<%=btnILRFOtherAmountClose.ClientID%>').click();
            $get('<%=btnILRFOtherAmount.ClientID%>').click();
        }
    </script>

    <table>
        <tr>
            <td style="width: 130px; padding-top: 19px">
                <asp:Label ID="Label1" Text="ACCOUNT NAME" runat="server"></asp:Label>
            </td>
            <td>
                <AL:AccountList ID="ddlAcctlist" runat="server" AccountType="1" />
            </td>
            <td style="padding-top: 19px">
                &nbsp;
                <asp:Button ID="btnSearch" Text="Search" runat="server" OnClick="btnSearch_Click" />
                &nbsp;&nbsp;
                <asp:Button ID="btnClear" runat="server" Text="Clear" OnClick="btnClear_Click" />
                <asp:HiddenField ID="hidConfirm" runat="server" />
                <asp:HiddenField ID="hidConfirmILRF" runat="server" />
                <asp:HiddenField ID="hidConfirmCHF" runat="server" />
                <asp:HiddenField ID="hidCofirmCount" runat="server" Value="0" />
                <asp:HiddenField ID="hidLSICount" runat="server" Value="0" />
                <asp:HiddenField ID="hidILRFCount" runat="server" Value="0" />
                <asp:HiddenField ID="hidPlbInd" runat="server" Value="0" />
            </td>
        </tr>
        <tr>
            <td style="width: 130px">
                <asp:Label ID="Label2" Text="PROGRAM TYPE" runat="server"></asp:Label>
            </td>
            <td style="padding-left: 4px" colspan="2">
                <asp:DropDownList ID="ddlProgramTypeList" runat="server" DataSourceID="ProgramTypeDataSource"
                    DataTextField="LookUpName" DataValueField="LookUpID" Width="230px" Enabled="false">
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td style="width: 130px">
                <asp:Label ID="Label3" Text="ADJUSTMENT NUMBER" runat="server"></asp:Label>
            </td>
            <td style="padding-left: 4px" colspan="2">
                <asp:DropDownList ID="ddlAdjNumList" runat="server" Enabled="false" AutoPostBack="true"
                    Width="230px">
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td style="width: 130px">
                <asp:Label ID="Label4" Text="VALUATION DATE" runat="server"></asp:Label>
            </td>
            <td style="padding-left: 4px" colspan="2">
                <asp:DropDownList ID="ddlValDateList" runat="server" Enabled="false" Width="230px">
                </asp:DropDownList>
            </td>
        </tr>
    </table>
    <br />
    <div id="Div1" style="overflow: auto; height: 300px; width: 910px;" runat="server">
        <asp:AISListView ID="AdjInvDashlistview" runat="server" DataKeyNames="CUSTMR_ID"
            OnSorting="AdjInvDashlistview_Sorting" OnItemDataBound="DataBoundList">
            <LayoutTemplate>
                <table id="Table1" class="panelContents" runat="server" width="98%">
                    <tr class="LayoutTemplate">
                        <th>
                            Acct. Num.
                        </th>
                        <th>
                            Account Name
                        </th>
                        <th style="width: 150px">
                            <asp:LinkButton ID="lnkProgramPeriod" runat="server" CommandName="Sort" CommandArgument="PROGRAMPERIOD">Program 
                            Period</asp:LinkButton>
                            <asp:Image ID="imgPROGRAMPERIOD" runat="server" ImageUrl="~/images/Ascending.gif"
                                ToolTip="Ascending" Visible="false" />
                        </th>
                        <th>
                            <asp:LinkButton ID="lnkValuationDate" runat="server" CommandName="Sort" CommandArgument="VALUATIONDATE">Valuation 
                            date</asp:LinkButton>
                            <asp:Image ID="imgVALUATIONDATE" runat="server" ImageUrl="~/images/Ascending.gif"
                                ToolTip="Ascending" Visible="false" />
                        </th>
                        <th>
                            <asp:LinkButton ID="lnkProgramType" runat="server" CommandName="Sort" CommandArgument="PROGRAMTYPENAME">Program 
                            Type</asp:LinkButton>
                            <asp:Image ID="imgPROGRAMTYPENAME" runat="server" ImageUrl="~/images/Ascending.gif"
                                ToolTip="Ascending" Visible="false" />
                        </th>
                        <th>
                            Broker
                        </th>
                        <th>
                            <asp:LinkButton ID="lnkBuOffice" runat="server" CommandName="Sort" CommandArgument="BUOFFICE">BU/Office</asp:LinkButton>
                            <asp:Image ID="imgBUOFFICE" runat="server" ImageUrl="~/images/Ascending.gif" ToolTip="Ascending"
                                Visible="false" />
                        </th>
                        <th>
                            <asp:LinkButton ID="lnkProgramStatus" runat="server" CommandName="Sort" CommandArgument="PROGRAMSTATUS">Program 
                            Status</asp:LinkButton>
                            <asp:Image ID="imgPROGRAMSTATUS" runat="server" ImageUrl="~/images/Ascending.gif"
                                ToolTip="Ascending" Visible="false" />
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
                        <th style="width: 50px">
                            Select
                        </th>
                    </tr>
                    <tr id="itemPlaceholder" runat="server">
                    </tr>
                </table>
            </LayoutTemplate>
            <ItemTemplate>
                <tr id="test" runat="server" class="ItemTemplate">
                    <td>
                        <asp:HiddenField ID="hidPremAdjPERID" runat="server" Value='<%#(Eval("PREMADJSTS")!= null && Eval("PREMADJSTS").ToString()=="FINAL INVOICE")?"0":Eval("PREM_ADJ_PER_ID") %>' />
                        <asp:HiddenField ID="hidPremAdjPGMID" runat="server" Value='<%#Bind("PREM_ADJ_PGM_ID") %>' />
                        <asp:HiddenField ID="hidNxtValDate" runat="server" Value='<%#Bind("NXT_VALN_DT") %>' />
                        <asp:HiddenField ID="hidNxtValPrem" runat="server" Value='<%#Bind("NXT_VALN_DT_PREM") %>' />
                        <asp:HiddenField ID="hidNxtValNonPrem" runat="server" Value='<%#Bind("NXT_VALN_DT_NON_PREM_DT") %>' />
                        <asp:HiddenField ID="hidValMMDT" runat="server" Value='<%#Bind("VALN_MM_DT") %>' />
                        <%# Eval("CUSTMR_ID")%>
                    </td>
                    <td>
                        <%# Eval("CUSTMR_NAME")%>
                    </td>
                    <td>
                        <asp:Label ID="lblProgramPeriod" runat="server" Text='<%# Bind("PROGRAMPERIOD")%>'></asp:Label>
                    </td>
                    <td>
                        <%# Eval("NXT_VALN_DT", "{0:d}")%>
                    </td>
                    <td>
                        <%# Eval("PROGRAMTYPE")%>
                    </td>
                    <td>
                        <%# Eval("BROKER")%>
                    </td>
                    <td>
                        <%# Eval("BUOFFFICE")%>
                    </td>
                    <td>
                        <asp:Label ID="lblProgrmSts" runat="server" Text='<%#Bind("PROGRAMSTATUS") %>' />
                    </td>
                    <td>
                        <asp:Label ID="lblAdjNo" runat="server" Text='<%# (Eval("PREMADJSTS")!= null && Eval("PREMADJSTS").ToString()=="FINAL INVOICE")?"":Eval("PREMIUMADJNUM") %>' />
                    </td>
                    <td>
                        <asp:Label ID="lblAdjSts" runat="server" Text='<%# (Eval("PREMADJSTS")!= null && Eval("PREMADJSTS").ToString()=="FINAL INVOICE")?"":Eval("PREMADJSTS") %>' />
                    </td>
                    <td>
                        <asp:Label ID="lblCalcAdjSts" runat="server" Text='<%# (Eval("PREMADJSTS")!= null && Eval("PREMADJSTS").ToString()=="FINAL INVOICE")?"":(Eval("CALC_ADJ_STS_CODE")==null?"":Eval("CALC_ADJ_STS_CODE").ToString() == "ERR" ? "Error" : Eval("CALC_ADJ_STS_CODE").ToString() == "CMP" ? "Complete" : Eval("CALC_ADJ_STS_CODE").ToString() == "INP" ? "Inprocess" : "")%>' />
                    </td>
                    <td style="width: 50px">
                        <asp:CheckBox ID="chkSelect" Checked='<%# (Eval("PREMADJSTS")!= null && Eval("PREMADJSTS").ToString()=="FINAL INVOICE")?bool.Parse("False"):int.Parse(Eval("PREM_ADJ_PER_ID").ToString())>0%>'
                            runat="server"></asp:CheckBox>
                        <asp:HiddenField ID="hidLSICount" runat="server" Value='<%#Bind("LSI_Custmr_Count")%>' />
                        <asp:HiddenField ID="hidILRFCount" runat="server" Value='<%#Bind("ILRF_Setup_Count")%>' />
                    </td>
                </tr>
            </ItemTemplate>
            <AlternatingItemTemplate>
                <tr id="test" runat="server" class="AlternatingItemTemplate">
                    <td>
                        <asp:HiddenField ID="hidPremAdjPERID" runat="server" Value='<%#(Eval("PREMADJSTS")!= null && Eval("PREMADJSTS").ToString()=="FINAL INVOICE")?"0":Eval("PREM_ADJ_PER_ID") %>' />
                        <asp:HiddenField ID="hidPremAdjPGMID" runat="server" Value='<%#Bind("PREM_ADJ_PGM_ID") %>' />
                        <asp:HiddenField ID="hidNxtValDate" runat="server" Value='<%#Bind("NXT_VALN_DT") %>' />
                        <asp:HiddenField ID="hidNxtValPrem" runat="server" Value='<%#Bind("NXT_VALN_DT_PREM") %>' />
                        <asp:HiddenField ID="hidNxtValNonPrem" runat="server" Value='<%#Bind("NXT_VALN_DT_NON_PREM_DT") %>' />
                        <asp:HiddenField ID="hidValMMDT" runat="server" Value='<%#Bind("VALN_MM_DT") %>' />
                        <%# Eval("CUSTMR_ID")%>
                    </td>
                    <td>
                        <%# Eval("CUSTMR_NAME")%>
                    </td>
                    <td>
                        <asp:Label ID="lblProgramPeriod" runat="server" Text='<%# Bind("PROGRAMPERIOD")%>'></asp:Label>
                    </td>
                    <td>
                        <%# Eval("NXT_VALN_DT", "{0:d}")%>
                    </td>
                    <td>
                        <%# Eval("PROGRAMTYPE")%>
                    </td>
                    <td>
                        <%# Eval("BROKER")%>
                    </td>
                    <td>
                        <%# Eval("BUOFFFICE")%>
                    </td>
                    <td>
                        <asp:Label ID="lblProgrmSts" runat="server" Text='<%#Bind("PROGRAMSTATUS") %>' />
                    </td>
                    <td>
                        <asp:Label ID="lblAdjNo" runat="server" Text='<%# (Eval("PREMADJSTS")!= null && Eval("PREMADJSTS").ToString()=="FINAL INVOICE")?"":Eval("PREMIUMADJNUM") %>' />
                    </td>
                    <td>
                        <asp:Label ID="lblAdjSts" runat="server" Text='<%# (Eval("PREMADJSTS")!= null && Eval("PREMADJSTS").ToString()=="FINAL INVOICE")?"":Eval("PREMADJSTS") %>' />
                    </td>
                    <td>
                        <asp:Label ID="lblCalcAdjSts" runat="server" Text='<%# (Eval("PREMADJSTS")!= null && Eval("PREMADJSTS").ToString()=="FINAL INVOICE")?"":(Eval("CALC_ADJ_STS_CODE")==null?"":Eval("CALC_ADJ_STS_CODE").ToString() == "ERR" ? "Error" : Eval("CALC_ADJ_STS_CODE").ToString() == "CMP" ? "Complete" : Eval("CALC_ADJ_STS_CODE").ToString() == "INP" ? "Inprocess" : "")%>' />
                    </td>
                    <td style="width: 50px">
                        <asp:CheckBox ID="chkSelect" Checked='<%# (Eval("PREMADJSTS")!= null && Eval("PREMADJSTS").ToString()=="FINAL INVOICE")?bool.Parse("False"):int.Parse(Eval("PREM_ADJ_PER_ID").ToString())>0%>'
                            runat="server"></asp:CheckBox>
                        <asp:HiddenField ID="hidLSICount" runat="server" Value='<%#Bind("LSI_Custmr_Count")%>' />
                        <asp:HiddenField ID="hidILRFCount" runat="server" Value='<%#Bind("ILRF_Setup_Count")%>' />
                    </td>
                </tr>
            </AlternatingItemTemplate>
            <EmptyDataTemplate>
                <table class="panelContents" runat="server" style="width: 100%">
                    <tr>
                        <th>
                            Acct. No
                        </th>
                        <th>
                            Account Name
                        </th>
                        <th>
                            Program Period
                        </th>
                        <th>
                            Valuation Date
                        </th>
                        <th>
                            Program Type
                        </th>
                        <th>
                            Broker
                        </th>
                        <th>
                            BU/Office
                        </th>
                        <th>
                            Program Status
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
                        <th style="width: 50px">
                            Select
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
    <table width="100%">
        <tr>
            <td align="center">
                <asp:Button ID="btnCalculate" runat="server" Text="CALCULATE" OnClick="btnCalculate_OnClick"
                    Width="133px" ValidationGroup="AISE" ToolTip="Please click here to Calculate the Adjustment for the selected program period"
                    Visible="false" OnClientClick="javascript:return Cofirmcalc(); " />
                <asp:Button ID="btnDraft" runat="server" Text="DRAFT INVOICE" OnClick="btnDraft_OnClick"
                    Width="149px" ValidationGroup="AISE" Visible="false" />
                <cc1:ConfirmButtonExtender ID="cbdDraft" runat="server" TargetControlID="btnDraft"
                    ConfirmText="Are you sure you want to process Draft Invoice? " />
            </td>
        </tr>
    </table>
    <asp:ObjectDataSource ID="ProgramTypeDataSource" runat="server" SelectMethod="GetLookUpActiveData"
        TypeName="ZurichNA.AIS.Business.Logic.BLAccess">
        <SelectParameters>
            <asp:Parameter DefaultValue="PROGRAM TYPE" Name="lookUpTypeName" Type="String" />
        </SelectParameters>
    </asp:ObjectDataSource>
    <asp:UpdatePanel runat="server" ID="upAccountDashboard">
        <ContentTemplate>
            <asp:Button style="display:none" runat="server" ID="AriesTemp" />
            <cc1:ModalPopupExtender runat="server" ID="modalAries" TargetControlID="AriesTemp"
                PopupControlID="Panel1" BackgroundCssClass="modalBackground" DropShadow="true"
                CancelControlID="btnClosepopup">
            </cc1:ModalPopupExtender>
            <div style="float: left;">
                <asp:Panel runat="server" CssClass="modalPopup" ID="Panel1" Style="border: solid 1px black;
                    display: none; width: 650px; padding: 0px" HorizontalAlign="Center">
                    <asp:Panel runat="Server" ID="Panel2" Style="width: 100%; cursor: move; padding: 0px;
                        background-color: #CCCCCC; height: 20px; border: solid 1px Gray; color: Black;
                        text-align: center; vertical-align: middle">
                    </asp:Panel>
                    <div style="text-align: center; width: 100%; padding-bottom: 5px; background-color: White;">
                        <br />
                        A key field has been changed to one or more of the program periods. Please proceed
                        to the Adjustment Review Menu_BU Broker Tab OR Cancel the adjustment and Re-set
                        the Broker and/or BU/Office to the prior values and calculate. Please check the
                        error log for additional details.
                        <br />
                        <asp:Button Width="60px" ID="btnClosepopup" runat="server" Text="ok" OnClick="btnClosepopup_Click" />
                        <br />
                    </div>
                </asp:Panel>
            </div>
        </ContentTemplate>
        <Triggers>
            <%--<asp:PostBackTrigger ControlID="btnOk" />--%>
        </Triggers>
    </asp:UpdatePanel>
    <%--SR-321581--%>
    <asp:UpdatePanel runat="server" ID="upBPNumber">
        <ContentTemplate>
            <asp:Button style="display:none" runat="server" ID="btnFinalTemp" />
            <cc1:ModalPopupExtender runat="server" ID="modalBPNumber" TargetControlID="btnFinalTemp"
                PopupControlID="pnlBPNumber" BackgroundCssClass="modalBackground" DropShadow="true"
                CancelControlID="btnBPNumberClose">
            </cc1:ModalPopupExtender>
            <div style="float: left;">
                <asp:Panel runat="server" CssClass="modalPopup" ID="pnlBPNumber" Style="border: solid 1px black;
                    display: none; width: 650px; padding: 0px" HorizontalAlign="Center">
                    <asp:Panel runat="Server" ID="Panel4" Style="width: 100%; cursor: move; padding: 0px;
                        background-color: #CCCCCC; height: 20px; border: solid 1px Gray; color: Black;
                        text-align: center; vertical-align: middle">
                    </asp:Panel>
                    <div style="text-align: left; width: 100%; padding-bottom: 5px; background-color: White;">
                        <br />
                        <ul>
                            <div style="text-align: left; width: 100%; padding-bottom: 5px; background-color: White;">
                                Calculation will not be performed for this account as this account is not set with
                                BP Number.
                            </div>
                            <div style="text-align: left; width: 100%; padding-bottom: 5px; background-color: White;">
                                Do you want to proceed to Account Info screen to update the BP Number now?
                            </div>
                        </ul>
                    </div>
                    <div style="text-align: center; width: 100%; padding-bottom: 2px; background-color: White;">
                        <asp:Button Width="60px" OnClientClick="BPNumberPopup()" ID="btnBPNumber" runat="server"
                            Text="Yes" OnClick="btnBPNumber_Click" />
                        <asp:Button Width="60px" ID="btnBPNumberClose" runat="server" Text="No" OnClick="btnBPNumberClose_Click" />
                        <br />
                    </div>
                </asp:Panel>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <%--SR-325928--%>
    <asp:UpdatePanel runat="server" ID="UpILRFOtherAmount">
        <ContentTemplate>
            <asp:Button style="display:none" runat="server" ID="btnILRFOtherAmountTemp" />
            <cc1:ModalPopupExtender runat="server" ID="modalILRFOtherAmount" TargetControlID="btnILRFOtherAmountTemp"
                PopupControlID="pnlILRFOtherAmount" BackgroundCssClass="modalBackground" DropShadow="true"
                CancelControlID="btnILRFOtherAmountClose">
            </cc1:ModalPopupExtender>
            <div style="float: left;">
                <asp:Panel runat="server" CssClass="modalPopup" ID="pnlILRFOtherAmount" Style="border: solid 1px black;
                    display: none; width: 650px; padding: 0px" HorizontalAlign="Center">
                    <asp:Panel runat="Server" ID="Panel5" Style="width: 100%; cursor: move; padding: 0px;
                        background-color: #CCCCCC; height: 30px; border: solid 1px Gray; color: Black;
                        text-align: center; vertical-align: middle" Font-Bold="true">
                        <asp:label ID="lblILRFOtherAmountmessages" runat="server" Text="The following program periods have an 'ILRF Other Amount' shown below.  To proceed with the calculation using this amount, click 'Yes'."></asp:label> 
                    </asp:Panel>
                    <div style="text-align: left; width: 100%; padding-bottom: 5px; background-color: White;">
                        <asp:Panel ID="pnlilrfTax" runat="server" CssClass="content" Visible="true" ScrollBars="Auto">
                            <asp:Label ID="lblilrfTax" runat="server" CssClass="h3" Text=""></asp:Label>
                            &nbsp;
                            <asp:AISListView ID="lstilrfOtherAmount" runat="server">
                                <LayoutTemplate>
                                    <table id="lstMITable" class="panelExtContents" runat="server" width="100%">
                                        <tr class="LayoutTemplate">
                                           
                                            <th>
                                                Program Period
                                            </th>
                                            <th>
                                                ILRF Other Amount
                                            </th>
                                           
                                        </tr>
                                        <tr id="ItemPlaceHolder" runat="server">
                                        </tr>
                                    </table>
                                </LayoutTemplate>
                                <AlternatingItemTemplate>
                                    <tr id="trItemTemplate" runat="server" class="AlternatingItemTemplate">
                                        
                                        <td align="center">
                                            
                                            <asp:Label ID="lblProgramPeriod" Width="265px" Visible="true" runat="server" Text='<%# Eval("PROGRAM PERIOD")%>'></asp:Label>
                                        </td>
                                        <td align="center">
                                            <%# Eval("incur_los_reim_fund_othr_amt") != null ? (Eval("incur_los_reim_fund_othr_amt").ToString() != "" ? (decimal.Parse(Eval("incur_los_reim_fund_othr_amt").ToString())).ToString("#,##0") : "0") : "0"%>
                                        </td>
                                        
                                    </tr>
                                </AlternatingItemTemplate>
                                <ItemTemplate>
                                    <tr id="trItemTemplate" runat="server" class="ItemTemplate">
                                        
                                        <td align="center">
                                            <asp:Label ID="lblProgramPeriod" Width="265px" Visible="true" runat="server" Text='<%# Eval("PROGRAM PERIOD")%>'></asp:Label>
                                        </td>
                                        <td align="center">
                                            <%# Eval("incur_los_reim_fund_othr_amt") != null ? (Eval("incur_los_reim_fund_othr_amt").ToString() != "" ? (decimal.Parse(Eval("incur_los_reim_fund_othr_amt").ToString())).ToString("#,##0") : "0") : "0"%>
                                        </td>
                                        
                                    </tr>
                                </ItemTemplate>
                            </asp:AISListView>
                        </asp:Panel>
                    </div>
                    <div style="text-align: center; width: 100%; padding-bottom: 2px; background-color: White;">
                        <asp:Button Width="60px" OnClientClick="ILRFOtherAmountPopup()" ID="btnILRFOtherAmount"
                            runat="server" Text="Yes" OnClick="btnILRFOtherAmount_Click" />
                        <asp:Button Width="60px" ID="btnILRFOtherAmountClose" runat="server" Text="No" OnClick="btnILRFOtherAmountClose_Click" />
                        <br />
                    </div>
                </asp:Panel>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
