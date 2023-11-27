<%@ Page Language="C#" MasterPageFile="~/Retro.master" AutoEventWireup="true" CodeBehind="LossFundInfo.aspx.cs"
    Inherits="LossFundInfo" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="../App_Shared/AccountInfoHeader.ascx" TagName="AccountInfoHeader"
    TagPrefix="uc1" %>
<%@ Register Src="../App_Shared/ProgramPeriod.ascx" TagName="ProgramPeriod" TagPrefix="uc2pp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeaderPlaceHolder" runat="Server">
    <asp:Label ID="lblILRFSetup" runat="server" Text="Loss Fund Information" CssClass="h1"></asp:Label>

    <script language="javascript" type="text/javascript">
        function ValidateESCROWCheckBox(oSource, oArgs) {
            var result = false;
            // get an array of all controls on the form
            var aControls = document.forms[0];
            // iterate through array looking for checkboxes the ID of CheckBoxList is "lstBoxPolicy" so each checkbox
            // control will have the name lstBoxPolicy:n where n is a value from zero to the number of checkboxes minus one
            for (var i = 0; i < aControls.length; i++) {
                if (aControls[i].name.substring(47, 62) == 'PolicyNumLstBox') {
                    // increment counter if checkbox is "ticked"
                    if (aControls[i].checked)
                    { result = true; }
                }
            }
            oArgs.IsValid = result;
        }

        function ValidateCheckBox(oSource, oArgs) {
            var result = false;
            // get an array of all controls on the form
            var aControls = document.forms[0];
            // iterate through array looking for checkboxes the ID of CheckBoxList is "lstBoxPolicy" so each checkbox
            // control will have the name lstBoxPolicy:n where n is a value from zero to the number of checkboxes minus one
            for (var i = 0; i < aControls.length; i++) {
                if (aControls[i].name.substring(48, 60) == 'lstBoxPolicy') {
                    // increment counter if checkbox is "ticked"
                    if (aControls[i].checked)
                    { result = true; }
                }
            }
            oArgs.IsValid = result;
        }
        function EnableDisableCheckBox(chkUsePaidLossUseLBA, chkUsePaidLossUseLCF, chkUsePaidLoss) {
            //      alert($get(chkUsePaidLoss).checked);
            if ($get(chkUsePaidLoss).checked) {
                $get(chkUsePaidLossUseLBA).disabled = false;
                $get(chkUsePaidLossUseLCF).disabled = false;
            }
            else {
                $get(chkUsePaidLossUseLBA).disabled = true;
                $get(chkUsePaidLossUseLCF).disabled = true;
                $get(chkUsePaidLossUseLBA).checked = false;
                $get(chkUsePaidLossUseLCF).checked = false;
            }
        }

        function EnableDisableCheckBoxes() {
            var result = false;
            // get an array of all controls on the form
            var aControls = document.forms[0];

            // iterate through array looking for checkboxes the ID of CheckBoxList is "lstBoxPolicy" so each checkbox
            // control will have the name lstBoxPolicy:n where n is a value from zero to the number of checkboxes minus one
            for (var i = 0; i < aControls.length; i++) {

                switch (aControls[i].name) {

                    case 'ctl00$MainPlaceHolder$TabContainer1$tplILRFStup$lstILRFFormulaSetup$ctrl2$chkUsePaidLoss':
                        if (aControls[i].checked) {
                            $get('ctl00$MainPlaceHolder$TabContainer1$tplILRFStup$lstILRFFormulaSetup$ctrl3$chkUsePaidLoss').disabled = false;
                            $get('ctl00$MainPlaceHolder$TabContainer1$tplILRFStup$lstILRFFormulaSetup$ctrl4$chkUsePaidLoss').disabled = false;
                        }
                        else {
                            $get('ctl00$MainPlaceHolder$TabContainer1$tplILRFStup$lstILRFFormulaSetup$ctrl3$chkUsePaidLoss').checked = false;
                            $get('ctl00$MainPlaceHolder$TabContainer1$tplILRFStup$lstILRFFormulaSetup$ctrl4$chkUsePaidLoss').checked = false;
                            $get('ctl00$MainPlaceHolder$TabContainer1$tplILRFStup$lstILRFFormulaSetup$ctrl3$chkUsePaidLoss').disabled = true;
                            $get('ctl00$MainPlaceHolder$TabContainer1$tplILRFStup$lstILRFFormulaSetup$ctrl4$chkUsePaidLoss').disabled = true;
                        }
                        break;
                    case 'ctl00$MainPlaceHolder$TabContainer1$tplILRFStup$lstILRFFormulaSetup$ctrl2$chkUsePaidALAE':
                        if (aControls[i].checked) {
                            $get('ctl00$MainPlaceHolder$TabContainer1$tplILRFStup$lstILRFFormulaSetup$ctrl3$chkUsePaidALAE').disabled = false;
                            $get('ctl00$MainPlaceHolder$TabContainer1$tplILRFStup$lstILRFFormulaSetup$ctrl4$chkUsePaidALAE').disabled = false;
                        }
                        else {
                            $get('ctl00$MainPlaceHolder$TabContainer1$tplILRFStup$lstILRFFormulaSetup$ctrl3$chkUsePaidALAE').checked = false;
                            $get('ctl00$MainPlaceHolder$TabContainer1$tplILRFStup$lstILRFFormulaSetup$ctrl4$chkUsePaidALAE').checked = false;
                            $get('ctl00$MainPlaceHolder$TabContainer1$tplILRFStup$lstILRFFormulaSetup$ctrl3$chkUsePaidALAE').disabled = true;
                            $get('ctl00$MainPlaceHolder$TabContainer1$tplILRFStup$lstILRFFormulaSetup$ctrl4$chkUsePaidALAE').disabled = true;
                        }
                        break;
                    case 'ctl00$MainPlaceHolder$TabContainer1$tplILRFStup$lstILRFFormulaSetup$ctrl2$chkUseReserveLosses':
                        if (aControls[i].checked) {
                            $get('ctl00$MainPlaceHolder$TabContainer1$tplILRFStup$lstILRFFormulaSetup$ctrl3$chkUseReserveLosses').disabled = false;
                            $get('ctl00$MainPlaceHolder$TabContainer1$tplILRFStup$lstILRFFormulaSetup$ctrl4$chkUseReserveLosses').disabled = false;
                        }
                        else {
                            $get('ctl00$MainPlaceHolder$TabContainer1$tplILRFStup$lstILRFFormulaSetup$ctrl3$chkUseReserveLosses').checked = false;
                            $get('ctl00$MainPlaceHolder$TabContainer1$tplILRFStup$lstILRFFormulaSetup$ctrl4$chkUseReserveLosses').checked = false;
                            $get('ctl00$MainPlaceHolder$TabContainer1$tplILRFStup$lstILRFFormulaSetup$ctrl3$chkUseReserveLosses').disabled = true;
                            $get('ctl00$MainPlaceHolder$TabContainer1$tplILRFStup$lstILRFFormulaSetup$ctrl4$chkUseReserveLosses').disabled = true;
                        }
                        break;
                    case 'ctl00$MainPlaceHolder$TabContainer1$tplILRFStup$lstILRFFormulaSetup$ctrl2$chkUseReserveALAE':
                        if (aControls[i].checked) {
                            $get('ctl00$MainPlaceHolder$TabContainer1$tplILRFStup$lstILRFFormulaSetup$ctrl3$chkUseReserveALAE').disabled = false;
                            $get('ctl00$MainPlaceHolder$TabContainer1$tplILRFStup$lstILRFFormulaSetup$ctrl4$chkUseReserveALAE').disabled = false;
                        }
                        else {
                            $get('ctl00$MainPlaceHolder$TabContainer1$tplILRFStup$lstILRFFormulaSetup$ctrl3$chkUseReserveALAE').checked = false;
                            $get('ctl00$MainPlaceHolder$TabContainer1$tplILRFStup$lstILRFFormulaSetup$ctrl4$chkUseReserveALAE').checked = false;
                            $get('ctl00$MainPlaceHolder$TabContainer1$tplILRFStup$lstILRFFormulaSetup$ctrl3$chkUseReserveALAE').disabled = true;
                            $get('ctl00$MainPlaceHolder$TabContainer1$tplILRFStup$lstILRFFormulaSetup$ctrl4$chkUseReserveALAE').disabled = true;
                        }
                        break;
                }
            }

        }
        function AllowAggUnlimitTextbox() {
            var flag = document.getElementById('<%=chkAggrLimitUnlimited.ClientID%>').checked;

            if (flag == true) {
                document.getElementById('<%=txtAggregateLimit.ClientID%>').disabled = true;
                document.getElementById('<%=txtAggregateLimit.ClientID%>').value = 0;
            }
            else {
                document.getElementById('<%=txtAggregateLimit.ClientID%>').disabled = false;
            }
        }
        function AllowMinUnlimitTextbox() {
            var flag = document.getElementById('<%=chkMiniLimitUnlimited.ClientID%>').checked;

            if (flag == true) {
                document.getElementById('<%=txtMinimumLimit.ClientID%>').disabled = true;
                document.getElementById('<%=txtMinimumLimit.ClientID%>').value = 0;
            }
            else {
                document.getElementById('<%=txtMinimumLimit.ClientID%>').disabled = false;
            }
        }

        function validateDropDownList() {
            //alert("Hi");
            var FactorID = document.getElementById('<%=hidPremAdjPgmSetupID.ClientID%>').value;
            //alert(FactorID);
            if (FactorID != "") {
                if (document.getElementById('<%=ddlIBNRLDFNONE.ClientID%>').value != 0) {
                    var answer = confirm("Changes will be automatically saved, And corresponding Factors will be deleted. \r\n Are you sure you want to continue?")
                    if (answer) {
                        document.getElementById('<%=btnHidden.ClientID%>').click();
                        //__doPostback('<%=ddlIBNRLDFNONE.ClientID%>','');
                    }
                    else {
                        var IBNRLDFEarlierSelectedID = document.getElementById('<%=hidIBNRLDFSelVal.ClientID%>').value;
                        document.getElementById('<%=ddlIBNRLDFNONE.ClientID%>').value = IBNRLDFEarlierSelectedID;
                    }
                }
                else {
                    var IBNRLDFEarlierSelID = document.getElementById('<%=hidIBNRLDFSelVal.ClientID%>').value;
                    document.getElementById('<%=ddlIBNRLDFNONE.ClientID%>').value = IBNRLDFEarlierSelID;
                }
            }
        }   
    
    </script>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainPlaceHolder" runat="Server">
    <table style="height: 600px; width: 920px">
        <tr>
            <td>
                <asp:ValidationSummary ID="ValSave" runat="server" CssClass="ValidationSummary" ValidationGroup="SaveESCROW" />
                <asp:ValidationSummary ID="VSSaveILRFSetup" runat="server" CssClass="ValidationSummary"
                    ValidationGroup="SaveGroup" />
                <uc1:AccountInfoHeader ID="AccountInfoHeader1" runat="server" />
                <asp:Button ID="btnHidden" runat="server" Width="0px" Height="0px" OnClick="btnHidden_Click" />
                <asp:Panel ID="pnlppEscrow" runat="server">
                    <asp:Label ID="lblProgramPeriodsHeader" runat="server" Font-Size="12px" font-weight="bold"
                        ForeColor="Navy" Font-Bold="true" Text="Program Periods"></asp:Label>
                    <uc2pp:ProgramPeriod ID="uc2ProgramPeriod" runat="server" />
                </asp:Panel>
                <!-- Object Data Source-->
                <asp:ObjectDataSource ID="Escrowinfodatasource" runat="server" TypeName="ZurichNA.AIS.Business.Logic.BLAccess"
                    SelectMethod="GetPolicyData" OnSelecting="Escrowinfomdatasource_selecting">
                    <SelectParameters>
                        <asp:Parameter Name="ProgramPeriodID" Type="Int32" />
                    </SelectParameters>
                </asp:ObjectDataSource>
                <cc1:TabContainer ID="TabContainer1" runat="server" ActiveTabIndex="0" CssClass="CustomTabs"
                    Width="350px">
                    <cc1:TabPanel runat="server" HeaderText="Escrow Setup" ID="tplEscStup">
                        <HeaderTemplate>
                            <asp:Label ID="Label1" runat="server" Text="Escrow Setup"></asp:Label></HeaderTemplate>
                        <ContentTemplate>
                            <div class="gridESCROW">
                                <asp:UpdatePanel ID="UpdatepanelESCROW" runat="server" UpdateMode="Conditional" Visible="true">
                                    <ContentTemplate>
                                        <asp:Panel ID="pnlESCROWDtls" runat="server" Visible="false" Width="910px">
                                            <table id="tblESCROWDtls" visible="true" runat="server" class="panelContents" width="100%">
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
                                                        Divided By
                                                    </th>
                                                    <th>
                                                        Months Held
                                                    </th>
                                                    <th>
                                                        Previous ESCROW
                                                    </th>
                                                    <th>
                                                        Disable
                                                    </th>
                                                </tr>
                                                <tr class="ItemTemplate" style="vertical-align: middle">
                                                    <td style="vertical-align: middle">
                                                        <asp:LinkButton ID="lnkBtnESCROW" runat="server" Text="Save" ValidationGroup="SaveESCROW"
                                                            Visible="true" Width="40px" OnClick="btnESCROWInfoDetailsSave_Click" />
                                                        <asp:LinkButton ID="lnkBtnESCROWCancel" runat="server" Text="Cancel" Visible="true"
                                                            Width="50px" OnClick="ESCROWCancel_Click" />
                                                    </td>
                                                    <td style="vertical-align: middle">
                                                        <table>
                                                            <tr>
                                                                <td>
                                                                    <asp:Panel ID="pnlPolicyNumberListESCROW" runat="server" ScrollBars="Auto" CssClass="content"
                                                                        Height="50px" Width="160px">
                                                                        <asp:CheckBoxList ID="PolicyNumLstBox" ValidationGroup="SaveESCROW" runat="server"
                                                                            ForeColor="Black" ReadOnly="true" BorderStyle="inset" BackColor="White" BorderWidth="1px"
                                                                            Style="text-align: left">
                                                                        </asp:CheckBoxList>
                                                                    </asp:Panel>
                                                                </td>
                                                                <td style="vertical-align: middle">
                                                                    <asp:CustomValidator ID="CstValdPolicyNumLstBox" runat="server" ErrorMessage="Please select at least one Policy from ESCROW Policy CheckBox List"
                                                                        ClientValidationFunction="ValidateESCROWCheckBox" ValidationGroup="SaveESCROW"
                                                                        Display="Dynamic" Text="*" />
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                    <td style="vertical-align: middle">
                                                        <asp:TextBox ID="txtPLBmnts" runat="server" MaxLength="2" Enabled="True" Width="85px"></asp:TextBox><cc1:FilteredTextBoxExtender
                                                            ID="FilteredtxtPLBmnts" runat="server" TargetControlID="txtPLBmnts" FilterType="Custom"
                                                            FilterMode="ValidChars" ValidChars="0123456789" />
                                                        <asp:RequiredFieldValidator ID="RequiredtxtPLBmnts" runat="server" Text="*" Display="Dynamic"
                                                            ErrorMessage="Please Enter PLB Months" ValidationGroup="SaveESCROW" ControlToValidate="txtPLBmnts"></asp:RequiredFieldValidator>
                                                    </td>
                                                    <td style="vertical-align: middle">
                                                        <asp:TextBox ID="txtDivedBy" runat="server" MaxLength="3" Enabled="True" Width="85px"></asp:TextBox><cc1:FilteredTextBoxExtender
                                                            ID="FilteredtxtDivedBy" runat="server" TargetControlID="txtDivedBy" FilterType="Custom"
                                                            FilterMode="ValidChars" ValidChars="0123456789" />
                                                        <asp:RequiredFieldValidator ID="RequiredtxtDivedBy" runat="server" Text="*" Display="Dynamic"
                                                            ErrorMessage="Please Enter Divisor value" ValidationGroup="SaveESCROW" ControlToValidate="txtDivedBy"></asp:RequiredFieldValidator>
                                                    </td>
                                                    <td style="vertical-align: middle">
                                                        <asp:TextBox ID="txtMnthsHeld" runat="server" MaxLength="3" Enabled="True" Width="85px"></asp:TextBox><cc1:MaskedEditExtender
                                                            ID="MaskedEdittxtMnthsHeld" runat="server" TargetControlID="txtMnthsHeld" Mask="99.9"
                                                            MessageValidatorTip="true" MaskType="Number" DisplayMoney="None" OnFocusCssClass="MaskedEditFocus"
                                                            OnInvalidCssClass="MaskedEditError" InputDirection="LeftToRight" AcceptNegative="None"
                                                            AutoComplete="false" />
                                                        <asp:RequiredFieldValidator ID="RequiredtxtMnthsHeld" runat="server" Text="*" Display="Dynamic"
                                                            ErrorMessage="Please Enter Months held value" ValidationGroup="SaveESCROW" ControlToValidate="txtMnthsHeld"></asp:RequiredFieldValidator>
                                                    </td>
                                                    <td style="vertical-align: middle">
                                                        <asp:AISAmountTextbox ID="txtPrevEscrowAmt" runat="server" Enabled="True" Width="90px"></asp:AISAmountTextbox>
                                                        <%--<cc1:FilteredTextBoxExtender runat="server" TargetControlID="txtPrevEscrowAmt" FilterType="Custom"
                                ValidChars="0123456789," ID="fltAmount">
                            </cc1:FilteredTextBoxExtender>--%>
                                                    </td>
                                                    <td style="vertical-align: middle">
                                                        <asp:ImageButton ID="imgDisablerow" OnCommand='DisablefirstRow' CommandArgument='<%# Bind("adjt_param_dtl_id") %>'
                                                            runat="server" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </asp:Panel>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                        </ContentTemplate>
                    </cc1:TabPanel>
                    <cc1:TabPanel ID="tplILRFStup" runat="server" HeaderText="ILRF Setup">
                        <HeaderTemplate>
                            <asp:Label ID="lblILRF" runat="server" Text="ILRF Setup"></asp:Label></HeaderTemplate>
                        <ContentTemplate>
                            <asp:Panel ID="pnlILRFSetup" runat="server" CssClass="content" Visible="false" Width="910px">
                                <table id="Table1" runat="server" class="panelContents" width="100%">
                                    <tr>
                                        <th>
                                        </th>
                                        <th align="center">
                                            POLICY #'s
                                        </th>
                                        <th align="center">
                                            Initial Fund Amt
                                        </th>
                                        <th align="center">
                                            Aggr. Limit Unlimited?
                                        </th>
                                        <th align="center">
                                            Aggregate Limit
                                        </th>
                                        <th align="center">
                                            Min Limit Unlimited?
                                        </th>
                                        <th align="center">
                                            Minimum Limit
                                        </th>
                                        <th align="center">
                                            IBNR/ LDF/ None
                                        </th>
                                        <th align="center">
                                            Invoice In LSI
                                        </th>
                                        <th>
                                            Details
                                        </th>
                                        <th>
                                            Disable
                                        </th>
                                    </tr>
                                    <tr class="ItemTemplate">
                                        <td style="vertical-align: middle">
                                            <asp:LinkButton ID="lbILRFSave" runat="server" Text="SAVE" Visible="true" Width="50px"
                                                ValidationGroup="SaveGroup" OnClick="ILRFSave_Click" />
                                            <asp:HiddenField ID="hidPremAdjPgmSetupID" runat="server"></asp:HiddenField>
                                            <asp:LinkButton ID="lblILRFCancel" runat="server" Text="Cancel" Visible="true" Width="50px"
                                                OnClick="ILRFCancel_Click" />
                                        </td>
                                        <td style="vertical-align: middle">
                                            <table>
                                                <tr>
                                                    <td>
                                                        <asp:Panel ID="pnlPolicyList" runat="server" ScrollBars="Auto" CssClass="content"
                                                            Height="50px" Width="160px">
                                                            <asp:CheckBoxList ID="lstBoxPolicy" runat="server" ForeColor="Black" ReadOnly="true"
                                                                ValidationGroup="SaveGroup" BackColor="White" BorderStyle="inset" BorderWidth="1px"
                                                                Style="text-align: left">
                                                            </asp:CheckBoxList>
                                                        </asp:Panel>
                                                    </td>
                                                    <td style="vertical-align: middle">
                                                        <asp:CustomValidator ID="CustlstBoxPolicy" runat="server" ErrorMessage="Please select at least one Policy ILRF Policy CheckBox List"
                                                            ClientValidationFunction="ValidateCheckBox" ValidationGroup="SaveGroup" Display="Dynamic"
                                                            Text="*" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                        <td style="width: 102px; vertical-align: middle">
                                            <asp:AISAmountTextbox ID="txtInitialFundAmt" runat="server" Width="90px"></asp:AISAmountTextbox>
                                            <%--<cc1:FilteredTextBoxExtender runat="server" TargetControlID="txtInitialFundAmt" FilterType="Custom"
                                ValidChars="0123456789," ID="fltInitialFundAmt">
                            </cc1:FilteredTextBoxExtender>--%>
                                        </td>
                                        <td style="vertical-align: middle">
                                            <asp:CheckBox ID="chkAggrLimitUnlimited" onclick="AllowAggUnlimitTextbox()" runat="server"
                                                Checked="false" Enabled="True" />
                                        </td>
                                        <td style="vertical-align: middle">
                                            <asp:TextBox ID="txtAggregateLimit" runat="server" Width="90px" MaxLength="11" Enabled="true"
                                                onblur="FormatNumNoDecAmt(this,11)" onfocus="RemoveCommas(this)"></asp:TextBox>
                                            <cc1:FilteredTextBoxExtender runat="server" TargetControlID="txtAggregateLimit" FilterType="Custom"
                                                ValidChars="0123456789," ID="fltAggregateLimit">
                                            </cc1:FilteredTextBoxExtender>
                                        </td>
                                        <td style="vertical-align: middle">
                                            <asp:CheckBox ID="chkMiniLimitUnlimited" onclick="AllowMinUnlimitTextbox()" runat="server"
                                                Checked="false" Enabled="True" />
                                        </td>
                                        <td style="vertical-align: middle">
                                            <asp:AISAmountTextbox ID="txtMinimumLimit" runat="server" Width="90px"></asp:AISAmountTextbox>
                                            <%--  <cc1:FilteredTextBoxExtender runat="server" TargetControlID="txtMinimumLimit" FilterType="Custom"
                                                ValidChars="0123456789," ID="fltMinimumLimit">
                                            </cc1:FilteredTextBoxExtender>--%>
                                        </td>
                                        <td style="width: 200px; vertical-align: middle">
                                            <asp:ObjectDataSource ID="ILRFPARAMETERDataSource" runat="server" SelectMethod="GetLookUpActiveData"
                                                TypeName="ZurichNA.AIS.Business.Logic.BLAccess">
                                                <SelectParameters>
                                                    <asp:Parameter DefaultValue="ILRF PARAMETER" Name="lookUpTypeName" Type="String" />
                                                </SelectParameters>
                                            </asp:ObjectDataSource>
                                            <asp:DropDownList ID="ddlIBNRLDFNONE" runat="server" Width="75px" DataSourceID="ILRFPARAMETERDataSource"
                                                DataTextField="LookUpName" DataValueField="LookUpID" ValidationGroup="SaveGroup"
                                                onchange="validateDropDownList();">
                                            </asp:DropDownList>
                                            <asp:HiddenField ID="hidIBNRLDFSelVal" runat="server"></asp:HiddenField>
                                            <asp:CompareValidator ID="CompareIBNRLDFNONE" runat="server" ControlToValidate="ddlIBNRLDFNONE"
                                                Display="Dynamic" ErrorMessage="Please Select IBNR , LDF or NONE" Text="*" Operator="NotEqual"
                                                ValueToCompare="0" ValidationGroup="SaveGroup" />
                                        </td>
                                        <td style="vertical-align: middle">
                                            <asp:CheckBox ID="chkInvoiceLSI" runat="server" Checked="false" Enabled="True" />
                                        </td>
                                        <td style="vertical-align: middle">
                                            <asp:LinkButton ID="lbDetails" runat="server" CommandName="Edit" Text="Details" Visible="true"
                                                Width="50px" OnClick="ILRFDetails_Click" />
                                        </td>
                                        <td style="vertical-align: middle">
                                            <asp:ImageButton ID="imgEnableDisable" runat="server" OnClick="imgEnableDisable_Click" />
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                            <table width="100%">
                                <tr>
                                    <td align="left">
                                        <asp:Panel ID="pnlFormulaSetup" runat="server" CssClass="content" Visible="false"
                                            Width="910px">
                                            <asp:Label ID="lblILRFFormulaSetup" runat="server" CssClass="h3" Text="ILRF Formula Setup"></asp:Label>
                                            &nbsp;
                                            <asp:LinkButton ID="lbILRFFormulaSetupClose" runat="server" Text="Close" OnClick="lbILRFFormulaSetupClose_Click" />
                                            <asp:AISListView ID="lstILRFFormulaSetup" runat="server">
                                                <LayoutTemplate>
                                                    <table id="Table1" runat="server" class="panelContents" width="100%">
                                                        <tr>
                                                            <th align="center" style="background-color: #CCCCCC">
                                                                Factor
                                                            </th>
                                                            <th align="center">
                                                                Use Paid Losses
                                                            </th>
                                                            <th align="center">
                                                                Use Paid ALAE
                                                            </th>
                                                            <th align="center">
                                                                Use Reserve Losses
                                                            </th>
                                                            <th align="center">
                                                                Use Reserve ALAE
                                                            </th>
                                                        </tr>
                                                        <tr id="ItemPlaceHolder" runat="server">
                                                        </tr>
                                                    </table>
                                                </LayoutTemplate>
                                                <ItemTemplate>
                                                    <tr class="ItemTemplate">
                                                        <td align="center" valign="middle" style="background-color: #CCCCCC">
                                                            <asp:Label Width="100px" ID="lblFactor" Font-Bold="true" Text='<%# Bind("LOSS_REIM_FUND_FACTOR_TYPE") %>'
                                                                runat="server"></asp:Label>
                                                            <asp:TextBox runat="server" Width="60" ID="txtILRFFormulaSetupID" Visible="false"
                                                                Text='<%# Bind("INCURRED_LOSS_REIM_FUND_FRMLA_ID") %>' />
                                                            <asp:TextBox runat="server" Width="60" ID="txtFactorID" Visible="false" Text='<%# Bind("LOSS_REIM_FUND_FACTOR_TYPE_ID") %>' />
                                                        </td>
                                                        <td align="center" valign="middle">
                                                            <asp:CheckBox ID="chkUsePaidLoss" Checked='<%# Bind("USE_PAID_LOSS_INDICATOR") %>'
                                                                runat="server" />
                                                        </td>
                                                        <td align="center" valign="middle">
                                                            <asp:CheckBox ID="chkUsePaidALAE" runat="server" Checked='<%# Bind("USE_PAID_ALLOCATED_LOSS_ADJ_EXP_INDICATOR") %>' />
                                                        </td>
                                                        <td align="center" valign="middle">
                                                            <asp:CheckBox ID="chkUseReserveLosses" runat="server" Checked='<%# Bind("USE_RESERVE_LOSS_INDICATOR") %>' />
                                                        </td>
                                                        <td align="center" valign="middle">
                                                            <asp:CheckBox ID="chkUseReserveALAE" runat="server" Checked='<%# Bind("USE_RESERVE_ALLOCATED_LOSS_ADJ_EXP_INDICATOR") %>' />
                                                        </td>
                                                    </tr>
                                                </ItemTemplate>
                                                <AlternatingItemTemplate>
                                                    <tr class="AlternatingItemTemplate">
                                                        <td align="center" valign="middle" style="background-color: #CCCCCC">
                                                            <asp:Label Width="90px" ID="lblFactor" Font-Bold="true" Text='<%# Bind("LOSS_REIM_FUND_FACTOR_TYPE") %>'
                                                                runat="server"></asp:Label>
                                                            <asp:TextBox runat="server" Width="60" ID="txtILRFFormulaSetupID" Visible="false"
                                                                Text='<%# Bind("INCURRED_LOSS_REIM_FUND_FRMLA_ID") %>' />
                                                            <asp:TextBox runat="server" Width="60" ID="txtFactorID" Visible="false" Text='<%# Bind("LOSS_REIM_FUND_FACTOR_TYPE_ID") %>' />
                                                        </td>
                                                        <td align="center" valign="middle">
                                                            <asp:CheckBox ID="chkUsePaidLoss" Checked='<%# Bind("USE_PAID_LOSS_INDICATOR") %>'
                                                                runat="server" />
                                                        </td>
                                                        <td align="center" valign="middle">
                                                            <asp:CheckBox ID="chkUsePaidALAE" runat="server" Checked='<%# Bind("USE_PAID_ALLOCATED_LOSS_ADJ_EXP_INDICATOR") %>' />
                                                        </td>
                                                        <td align="center" valign="middle">
                                                            <asp:CheckBox ID="chkUseReserveLosses" runat="server" Checked='<%# Bind("USE_RESERVE_LOSS_INDICATOR") %>' />
                                                        </td>
                                                        <td align="center" valign="middle">
                                                            <asp:CheckBox ID="chkUseReserveALAE" runat="server" Checked='<%# Bind("USE_RESERVE_ALLOCATED_LOSS_ADJ_EXP_INDICATOR") %>' />
                                                        </td>
                                                    </tr>
                                                </AlternatingItemTemplate>
                                            </asp:AISListView>
                                            <table width="100%">
                                                <tr>
                                                    <td align="right">
                                                        <asp:Button ID="btnSaveILRFFormulaSetup" Text="Save" ValidationGroup="Save" runat="server"
                                                            OnClick="btnSaveILRFFormulaSetup_Click" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </asp:Panel>
                                    </td>
                                </tr>
                            </table>
                        </ContentTemplate>
                    </cc1:TabPanel>
                </cc1:TabContainer>
            </td>
        </tr>
    </table>
</asp:Content>
