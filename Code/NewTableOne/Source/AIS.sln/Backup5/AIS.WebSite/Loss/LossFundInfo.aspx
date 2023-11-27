<%@ Page Language="C#" MasterPageFile="~/Retro.master" AutoEventWireup="true" CodeBehind="LossFundInfo.aspx.cs"
    Inherits="LossFundInfo" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="../App_Shared/AccountInfoHeader.ascx" TagName="AccountInfoHeader"
    TagPrefix="uc1" %>
<%@ Register Src="../App_Shared/ProgramPeriod.ascx" TagName="ProgramPeriod" TagPrefix="uc2pp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeaderPlaceHolder" runat="Server">
    <asp:Label ID="lblILRFSetup" runat="server" Text="Loss Fund Information" CssClass="h1"></asp:Label>
    <!--Start of Javascript Section-->
    <script src="../JavaScript/RetroScript.js" type="text/javascript"></script>
    <!--End of Javascript Section-->
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
    <script type="text/javascript">
        function Check_Click(objRef) {
            //Get the Row based on checkbox
            var row = objRef.parentNode.parentNode;


            //Get the reference of GridView
            var GridView = row.parentNode;

            //Get all input elements in Gridview
            var inputList = GridView.getElementsByTagName("input");

            for (var i = 0; i < inputList.length; i++) {
                //The First element is the Header Checkbox
                var headerCheckBox = inputList[0];

                //Based on all or none checkboxes
                //are checked check/uncheck Header Checkbox
                var checked = true;
                if (inputList[i].type == "checkbox" && inputList[i] != headerCheckBox) {
                    if (!inputList[i].checked) {
                        checked = false;
                        break;
                    }
                }
            }
            headerCheckBox.checked = checked;

        }

        function checkAll(objRef) {
            var GridView = objRef.parentNode.parentNode.parentNode;
            var inputList = GridView.getElementsByTagName("input");
            for (var i = 0; i < inputList.length; i++) {
                //Get the Cell To find out ColumnIndex
                var row = inputList[i].parentNode.parentNode;
                if (inputList[i].type == "checkbox" && objRef != inputList[i]) {
                    if (objRef.checked) {
                        //If the header checkbox is checked
                        //check all checkboxes
                        inputList[i].checked = true;
                    }
                    else {
                        //If the header checkbox is checked
                        //uncheck all checkboxes
                        inputList[i].checked = false;
                    }
                }
            }
        }
    </script>

    <script type="text/javascript">

        var xPos, yPos;
        var prm = Sys.WebForms.PageRequestManager.getInstance();
        prm.add_beginRequest(BeginRequestHandler);
        prm.add_endRequest(EndRequestHandler);

        function BeginRequestHandler(sender, args) {
            xPos = $get('<%=pnlPolDetails.ClientID%>').scrollLeft;
            yPos = $get('<%=pnlPolDetails.ClientID%>').scrollTop;

        }

        function EndRequestHandler(sender, args) {
            $get('<%=pnlPolDetails.ClientID%>').scrollLeft = xPos;
            $get('<%=pnlPolDetails.ClientID%>').scrollTop = yPos;
        }

        function pageLoad() {
            var popup = $find('<%=modalPolDetails.ClientID%>');
            
            popup.add_shown(EndRequestHandler);
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
                <asp:ValidationSummary ID="VSSaveILRFTaxSetup" runat="server" ValidationGroup="ILRFTaxSetupSaveGroup"
                    Height="40px" CssClass="ValidationSummary" />
                <asp:ValidationSummary ID="VSEditILRFTaxSetup" runat="server" ValidationGroup="ILRFTaxSetupEditGroup"
                    CssClass="ValidationSummary" />
                <asp:ValidationSummary ID="VSSaveTaxExempt" runat="server" ValidationGroup="TaxExemptSetupSaveGroup"
                    CssClass="ValidationSummary" />
                <asp:ValidationSummary ID="VSEditTaxExempt" runat="server" ValidationGroup="TaxExemptSetupEditGroup"
                    CssClass="ValidationSummary" />
                <uc1:AccountInfoHeader ID="AccountInfoHeader1" runat="server" />
                <asp:Button ID="btnHidden" runat="server" Style="display: none" OnClick="btnHidden_Click" />
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
                    <cc1:TabPanel runat="server" HeaderText="Loss Fund Setup" ID="tplEscStup">
                        <HeaderTemplate>
                            <asp:Label ID="Label1" runat="server" Text="Loss Fund Setup"></asp:Label></HeaderTemplate>
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
                                                        Previous Loss Fund
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
                                                        <asp:TextBox ID="txtMnthsHeld" MaxLength="5" runat="server"  Enabled="True" Width="85px"></asp:TextBox>
                                                        <cc1:MaskedEditExtender
                                                            ID="MaskedEdittxtMnthsHeld" runat="server" TargetControlID="txtMnthsHeld" Mask="99.99"
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
                            <asp:HiddenField ID="hdnShowILRF" runat="server" />
                            <cc1:ModalPopupExtender runat="server" ID="modalPolDetails" TargetControlID="hdnShowILRF"
                                PopupControlID="pnlPolDetails" BackgroundCssClass="modalBackground" 
                                CancelControlID="btnCancel">
                            </cc1:ModalPopupExtender>
                            <div style="float: left;">
                                <asp:Panel runat="server" CssClass="modalPopup" ID="pnlPolDetails" Style="border: solid 1px black;overflow-x:hidden;
                                    display: none; width: 650px;position: static; padding: 0px;max-height:320px;min-height:20px" HorizontalAlign="Center" ScrollBars="Vertical" >

                                    <asp:Panel runat="Server" ID="Panel3" Style="width: 99.50%; cursor: move; padding: 0px;
                                        background-color: #CCCCCC; height: 20px; border: solid 1px Gray; color: Black;
                                        text-align: center; vertical-align: middle">
                                    </asp:Panel>
                                    <div style="text-align: center; width: 100%; padding-bottom: 5px; background-color: White;height:100%">
                                        <table align="center">
                                            <tr>
                                                <td>
                                                    <br />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    Adjustment Type:
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="ddlAdjType" runat="server" Enabled="true" Width="170px" OnSelectedIndexChanged="ddlAdjType_SelectedIndexChanged" AutoPostBack="true">
                                                        <asp:ListItem>(Select)</asp:ListItem>
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <br />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="2" align="center">
                                                    <asp:GridView ID="gvPolicy" runat="server" AutoGenerateColumns="false" HeaderStyle-CssClass="LayoutTemplate"
                                                        Width="100%" OnRowDataBound="gvPolicy_RowDataBound">
                                                        <Columns>
                                                            <asp:TemplateField>
                                                                <HeaderTemplate>
                                                                    <asp:CheckBox ID="chkSelectAll" runat="server" onclick = "checkAll(this);" />
                                                                </HeaderTemplate>
                                                                <ItemTemplate>
                                                                    <asp:CheckBox ID="chkSelect" runat="server" onclick = "Check_Click(this);" AutoPostBack="true" OnCheckedChanged="chkSelect_CheckedChanged"/>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField>
                                                                <HeaderTemplate>
                                                                    Policy Numbers
                                                                </HeaderTemplate>
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblPolicyNumber" runat="server" Text='<%# Eval("PolicyPerfectNumber")%>'></asp:Label>
                                                                    <asp:Label ID="lblPolicyID" runat="server" Text='<%# Eval("PolicyID")%>' Visible="false"></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                        </Columns>
                                                    </asp:GridView>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <br />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="right">
                                                    <asp:Button ID="btnUpdate" runat="server" Text="Update" OnClick="btnUpdate_Click"/>
                                                </td>
                                                <td align="left">
                                                    <asp:Button ID="btnCancel" runat="server" Text="Cancel" OnClick="btnCancel_Click"/>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                </asp:Panel>
                            </div>
                            <asp:Panel ID="pnlILRFSetup" runat="server" CssClass="content" Visible="false" Width="910px">
                                <table id="Table1" runat="server" class="panelContents" width="100%">
                                    <tr>
                                        <th>
                                        </th>
                                        <th align="center">
                                            POLICY #'s
                                        </th>
                                        <th align="center">
                                            Initial Fund
                                            <br />
                                            Amt
                                        </th>
                                        <th align="center">
                                            Aggr.
                                            <br />
                                            Limit
                                            <br />
                                            Un- limited?
                                        </th>
                                        <th align="center">
                                            Aggregate Limit
                                        </th>
                                        <th align="center">
                                            Min
                                            <br />
                                            Limit
                                            <br />
                                            Un-
                                            <br />
                                            limited?
                                        </th>
                                        <th align="center">
                                            Minimum Limit
                                        </th>
                                        <th align="center">
                                            IBNR/
                                            <br />
                                            LDF/
                                            <br />
                                            None
                                        </th>
                                        <th align="center">
                                            Invoice
                                            <br />
                                            In
                                            <br />
                                            LSI
                                        </th>
                                        <th align="center">
                                            Other Amount
                                        </th>
                                        <th>
                                            Details
                                        </th>
                                        <th>
                                            Taxes
                                        </th>
                                        <th>
                                            Disable
                                        </th>
                                        <th>
                                            &nbsp;
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
                                                            Height="50px" Width="130px">
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
                                        <td style="width: 160px; vertical-align: middle">
                                            <asp:ObjectDataSource ID="ILRFPARAMETERDataSource" runat="server" SelectMethod="GetLookUpActiveData"
                                                TypeName="ZurichNA.AIS.Business.Logic.BLAccess">
                                                <SelectParameters>
                                                    <asp:Parameter DefaultValue="ILRF PARAMETER" Name="lookUpTypeName" Type="String" />
                                                </SelectParameters>
                                            </asp:ObjectDataSource>
                                            <asp:DropDownList ID="ddlIBNRLDFNONE" runat="server" Width="55px" DataSourceID="ILRFPARAMETERDataSource"
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
                                            <%--added new text box as per the SR 325928--%>
                                            <asp:AISAmountTextbox ID="txtOtherAmount" runat="server" Width="92px" AllowNegetive="true"></asp:AISAmountTextbox>
                                            <cc1:FilteredTextBoxExtender runat="server" TargetControlID="txtOtherAmount" FilterType="Custom"
                                                ValidChars="0123456789,.-" ID="fltOtherAmount">
                                            </cc1:FilteredTextBoxExtender>
                                        </td>
                                        <td style="vertical-align: middle">
                                            <asp:LinkButton ID="lbDetails" runat="server" CommandName="Edit" Text="Details" Visible="true"
                                                Width="35px" OnClick="ILRFDetails_Click" />
                                        </td>
                                        <td style="vertical-align: middle">
                                            <asp:LinkButton ID="lbTaxes" runat="server" CommandName="Edit" Text="Taxes" Visible="true"
                                                Width="35px" OnClick="ILRFTaxes_Click" />
                                        </td>
                                        <td style="vertical-align: middle">
                                            <asp:ImageButton ID="imgEnableDisable" runat="server" OnClick="imgEnableDisable_Click" />
                                        </td>
                                        <td style="vertical-align: middle">
                                            <asp:LinkButton ID="lnkViewDetails" runat="server" Text="Policy Details" Visible="true"
                                                Width="80px" Enabled="true" OnClick="ILRFPolicyDetails_Click" CommandArgument='<%# Bind("adjt_param_dtl_id") %>' />
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
                                        <!--Texas Tax:Texas Tax Design Starts From here -->
                                        <asp:ObjectDataSource ID="ILRFTaxDescriptionDataSource" runat="server" SelectMethod="getTaxDescriptionList"
                                            TypeName="ZurichNA.AIS.Business.Logic.ILRFTaxSetupBS"></asp:ObjectDataSource>
                                        <asp:ObjectDataSource ID="ObjectCovType" runat="server" SelectMethod="GetLookUpLOBActiveData"
                                            TypeName="ZurichNA.AIS.Business.Logic.BLAccess">
                                            <SelectParameters>
                                                <asp:Parameter DefaultValue="LOB" Name="lookUpTypeName" Type="String" />
                                            </SelectParameters>
                                        </asp:ObjectDataSource>
                                        <asp:Panel ID="pnlTaxSetup" runat="server" CssClass="content" Visible="false" Width="910px"
                                            ScrollBars="Auto" Height="220px">
                                            <asp:Label ID="lblILRFTaxSetup" runat="server" CssClass="h3" Text="ILRF Tax Setup"></asp:Label>
                                            &nbsp;
                                            <asp:LinkButton ID="lbILRFTaxSetupClose" runat="server" Text="Close" OnClick="lbILRFTaxSetupClose_Click" />
                                            <asp:AISListView ID="lstTaxSetup" runat="server" InsertItemPosition="FirstItem" OnItemCommand="lstTaxSetup_ItemCommand"
                                                OnItemDataBound="lstTaxSetup_DataBoundList" OnItemEditing="lstTaxSetup_ItemEdit"
                                                OnItemCanceling="lstTaxSetup_ItemCancel" OnItemUpdating="lstTaxSetup_ItemUpdating">
                                                <LayoutTemplate>
                                                    <table id="lstMITable" class="panelExtContents" runat="server" width="98%">
                                                        <tr class="LayoutTemplate">
                                                            <th>
                                                                Select
                                                            </th>
                                                            <th>
                                                                Tax Description
                                                            </th>
                                                            <th>
                                                                LOB
                                                            </th>
                                                            <th>
                                                                Tax Amount
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
                                                    <table id="lstMITable" class="panelExtContents" runat="server" width="98%">
                                                        <tr class="LayoutTemplate">
                                                            <th>
                                                                Select
                                                            </th>
                                                            <th>
                                                                Tax Description
                                                            </th>
                                                            <th>
                                                                LOB
                                                            </th>
                                                            <th>
                                                                Tax Amount
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
                                                                <asp:Label ID="lblEmptyMessage" Text="---No Records found ---" Font-Bold="true" Width="600px"
                                                                    runat="server" Style="text-align: center" />
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
                                                            <asp:Label ID="lblILRFTaxID" Width="265px" Visible="false" runat="server" Text='<%# Bind("INCURRED_LOSS_REIM_FUND_TAX_ID") %>'></asp:Label>
                                                            <asp:Label ID="lblILRFTaxType" Width="265px" Visible="true" runat="server" Text='<%# Eval("INCURRED_LOSS_REIM_FUND_TAX_TYPE")%>'></asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="lblLOB" runat="server" Text='<%# Bind("LN_OF_BSN_TXT") %>'></asp:Label>
                                                        </td>
                                                        <td align="center">
                                                            <%# Eval("TAX_AMT") != null ? (Eval("TAX_AMT").ToString() != "" ? (decimal.Parse(Eval("TAX_AMT").ToString())).ToString("#,##0") : "") : ""%>
                                                        </td>
                                                        <td>
                                                            <asp:HiddenField ID="hidActive" runat="server" Value='<%# Bind("ACTV_IND")%>' />
                                                            <asp:ImageButton ID="imgDisable" CommandArgument='<%# Bind("INCURRED_LOSS_REIM_FUND_TAX_ID") %>'
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
                                                            <asp:Label ID="lblILRFTaxID" Width="265px" Visible="false" runat="server" Text='<%# Bind("INCURRED_LOSS_REIM_FUND_TAX_ID") %>'></asp:Label><asp:Label
                                                                ID="lblILRFTaxType" Width="265px" Visible="true" runat="server" Text='<%# Eval("INCURRED_LOSS_REIM_FUND_TAX_TYPE")%>'></asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="lblLOB" runat="server" Text='<%# Bind("LN_OF_BSN_TXT") %>'></asp:Label>
                                                        </td>
                                                        <td align="center">
                                                            <%# Eval("TAX_AMT") != null ? (Eval("TAX_AMT").ToString() != "" ? (decimal.Parse(Eval("TAX_AMT").ToString())).ToString("#,##0") : "") : ""%>
                                                        </td>
                                                        <td>
                                                            <asp:HiddenField ID="hidActive" runat="server" Value='<%# Bind("ACTV_IND")%>' />
                                                            <asp:ImageButton ID="imgDisable" CommandArgument='<%# Bind("INCURRED_LOSS_REIM_FUND_TAX_ID") %>'
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
                                                                Width="40px" ValidationGroup="ILRFTaxSetupSaveGroup" ToolTip="Click here to Save" />
                                                        </td>
                                                        <td align="center">
                                                            <asp:DropDownList ID="ddlTaxDescriptionlist" runat="server" DataSourceID="ILRFTaxDescriptionDataSource"
                                                                DataTextField="INCURRED_LOSS_REIM_FUND_TAX_TYPE" DataValueField="INCURRED_LOSS_REIM_FUND_TAX_ID">
                                                            </asp:DropDownList>
                                                            <asp:CompareValidator ID="CompareILRFTaxSetuplist" runat="server" ControlToValidate="ddlTaxDescriptionlist"
                                                                ErrorMessage="Please select Tax Description" Font-Size="Medium" Font-Names="Arial"
                                                                Text="*" Display="Dynamic" Operator="NotEqual" ValueToCompare="0" ValidationGroup="ILRFTaxSetupSaveGroup"></asp:CompareValidator>
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="lblLOB" Width="120px" Visible="false" runat="server" Text='<%# Bind("LN_OF_BSN_ID") %>'></asp:Label>
                                                            <asp:DropDownList ID="ddlLOB" runat="server" DataSourceID="ObjectCovType" ValidationGroup="ILRFTaxSetupSaveGroup"
                                                                Width="70px" DataTextField="LookUpName" DataValueField="LookUpID">
                                                            </asp:DropDownList>
                                                            <asp:RequiredFieldValidator ID="rfvCoverageType" runat="server" ControlToValidate="ddlLOB"
                                                                InitialValue="0" ErrorMessage="Please select at least one Coverage Type" ValidationGroup="ILRFTaxSetupSaveGroup">*</asp:RequiredFieldValidator>
                                                        </td>
                                                        <td align="center">
                                                            <asp:AISAmountTextbox ID="txtAmount" runat="server" AllowNegetive="true" ValidationGroup="ILRFTaxSetupSaveGroup"
                                                                Width="105px" />
                                                            <%--<asp:RequiredFieldValidator ID="reqAmount" Display="Dynamic" runat="server" ControlToValidate="txtAmount"
                                                                ErrorMessage="Please Enter Tax Amount" ValidationGroup="ILRFTaxSetupSaveGroup"
                                                                Text="*" />--%>
                                                        </td>
                                                        <td>
                                                        </td>
                                                    </tr>
                                                </InsertItemTemplate>
                                                <EditItemTemplate>
                                                    <tr class="ItemTemplate">
                                                        <td>
                                                            <asp:LinkButton ID="lbILRFTaxSetupUpdate" CommandName="Update" Text="Update" runat="server"
                                                                Visible="true" Width="40px" ValidationGroup="ILRFTaxSetupEditGroup" ToolTip="Click here to Update" />
                                                            <asp:LinkButton ID="lbILRFTaxSetupCancel" CommandName="Cancel" runat="server" Text="Cancel"
                                                                Visible="true" Width="40px" ToolTip="Click here to Cancel" />
                                                        </td>
                                                        <td align="center">
                                                            <asp:Label ID="lblILRFTaxID" Width="265px" Visible="false" runat="server" Text='<%# Bind("INCURRED_LOSS_REIM_FUND_TAX_ID") %>'></asp:Label><asp:Label
                                                                ID="lblILRFTaxTypeID" Width="265px" Visible="false" runat="server" Text='<%# Eval("TAX_TYP_ID")%>'></asp:Label>
                                                            <asp:DropDownList ID="ddlTaxDescriptionlist" runat="server">
                                                            </asp:DropDownList>
                                                            <asp:CompareValidator ID="CompareILRFTaxSetuplist" runat="server" ControlToValidate="ddlTaxDescriptionlist"
                                                                ErrorMessage="Please select Tax Description" Text="*" Display="Dynamic" Operator="NotEqual"
                                                                ValueToCompare="0" ValidationGroup="ILRFTaxSetupEditGroup"></asp:CompareValidator>
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="lblEditLOB" Width="165px" Visible="false" runat="server" Text='<%# Bind("LN_OF_BSN_TXT") %>'></asp:Label>
                                                            <asp:DropDownList ID="ddlLOB" runat="server" DataSourceID="ObjectCovType" ValidationGroup="ILRFTaxSetupEditGroup"
                                                                Width="70px" DataTextField="LookUpName" DataValueField="LookUpID">
                                                            </asp:DropDownList>
                                                            <asp:RequiredFieldValidator ID="rfvCoverageType" runat="server" ControlToValidate="ddlLOB"
                                                                InitialValue="0" ErrorMessage="Please select at least one Line of Business" ValidationGroup="ILRFTaxSetupEditGroup">*</asp:RequiredFieldValidator>
                                                        </td>
                                                        <td align="center">
                                                            <asp:AISAmountTextbox ID="txtAmount" runat="server" Text='<%# Eval("TAX_AMT") != null ? (Eval("TAX_AMT").ToString() != "" ?(decimal.Parse(Eval("TAX_AMT").ToString())).ToString("#,##0") : ""): ""%>'
                                                                ValidationGroup="ILRFTaxSetupEditGroup" Width="105px" AllowNegetive="true" MaxLength="11" />
                                                            <%--<asp:RequiredFieldValidator ID="reqAmount" Display="Dynamic" runat="server" ControlToValidate="txtAmount"
                                                                ErrorMessage="Please Enter Tax Amount" ValidationGroup="ILRFTaxSetupEditGroup"
                                                                Text="*" />--%>
                                                        </td>
                                                        <td>
                                                        </td>
                                                    </tr>
                                                </EditItemTemplate>
                                            </asp:AISListView>
                                        </asp:Panel>
                                        <!--Texas Tax:Texas Tax Design Ends From here -->
                                    </td>
                                </tr>
                            </table>
                        </ContentTemplate>
                    </cc1:TabPanel>
                    <cc1:TabPanel runat="server" HeaderText="Tax Exemption" ID="tplTaxexem">
                        <HeaderTemplate>
                            <asp:Label ID="lblTaxexem" runat="server" Text="Tax Exemption Setup"></asp:Label></HeaderTemplate>
                        <ContentTemplate>
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
                                function TaxExemptPopup() {
                                    $get('<%=btnTaxExemptClose.ClientID%>').click();
                                    $get('<%=btnTaxExemptpopup.ClientID%>').click();
                                }
                                function TaxExemptlistPopup() {
                                    $get('<%=btnTaxExemptlistpopup.ClientID%>').click();
                                }

                            </script>
                            <asp:ObjectDataSource ID="StatesDataSource" runat="server" SelectMethod="GetStates"
                                TypeName="ZurichNA.AIS.Business.Logic.TaxExemptionBS"></asp:ObjectDataSource>
                            <asp:ObjectDataSource ID="odsStates" runat="server" SelectMethod="GetStatesForEdit"
                                TypeName="ZurichNA.AIS.Business.Logic.TaxExemptionBS">
                                <SelectParameters>
                                    <asp:Parameter Name="iLkupId" Type="Int32" />
                                </SelectParameters>
                            </asp:ObjectDataSource>
                            <asp:Panel ID="PanelTaxExempt" runat="server" CssClass="content" Visible="true" Width="455px"
                                ScrollBars="Auto" Height="220px">
                                <asp:AISListView ID="lstTaxExemptSetup" runat="server" InsertItemPosition="FirstItem"
                                    OnItemCommand="lstTaxExemptSetup_ItemCommand" OnItemDataBound="lstTaxExemptSetup_DataBoundList"
                                    OnItemUpdating="lstTaxExemptSetup_ItemUpdating" OnItemCreated="lstTaxExemptSetup_ItemCreated">
                                    <LayoutTemplate>
                                        <table id="lstMITable" class="panelExtContents" runat="server" width="96%">
                                            <tr class="LayoutTemplate">
                                                <th>
                                                    Select
                                                </th>
                                                <th>
                                                    State
                                                </th>
                                                <th>
                                                    Disable
                                                </th>
                                            </tr>
                                            <tr id="ItemPlaceHolder" runat="server">
                                            </tr>
                                        </table>
                                    </LayoutTemplate>
                                    <AlternatingItemTemplate>
                                        <tr id="trItemTemplate" runat="server" class="AlternatingItemTemplate">
                                            <td>
                                                <asp:LinkButton ID="lblItemEdit" Enabled='<%# Eval("ACTV_IND")%>' CommandName="Edit"
                                                    Text="Edit" runat="server" Visible="false" Width="30px" ToolTip="Click here to Edit" />
                                            </td>
                                            <td>
                                                <asp:Label ID="lblState" runat="server" Text='<%# Bind("STATE_NAME") %>'></asp:Label>
                                            </td>
                                            <td>
                                                <asp:HiddenField ID="hidActive" runat="server" Value='<%# Bind("ACTV_IND")%>' />
                                                <asp:ImageButton ID="imgDisable" CommandArgument='<%# Bind("TAX_EXMP_SETUP_ID") %>'
                                                    runat="server" ImageUrl='<%# Eval("ACTV_IND").ToString()=="True"?"~/images/disabled.GIF":"~/images/enabled.GIF" %>'>
                                                </asp:ImageButton>
                                            </td>
                                        </tr>
                                    </AlternatingItemTemplate>
                                    <ItemTemplate>
                                        <tr id="trItemTemplate" runat="server" class="ItemTemplate">
                                            <td>
                                                <asp:LinkButton ID="lblItemEdit" Enabled='<%# Eval("ACTV_IND")%>' CommandName="Edit"
                                                    Text="Edit" runat="server" Visible="false" Width="30px" ToolTip="Click here to Edit" />
                                            </td>
                                            <td>
                                                <asp:Label ID="lblState" runat="server" Text='<%# Bind("STATE_NAME") %>'></asp:Label>
                                            </td>
                                            <td>
                                                <asp:HiddenField ID="hidActive" runat="server" Value='<%# Bind("ACTV_IND")%>' />
                                                <asp:ImageButton ID="imgDisable" CommandArgument='<%# Bind("TAX_EXMP_SETUP_ID") %>'
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
                                                    Width="30px" ValidationGroup="TaxExemptSetupSaveGroup" ToolTip="Click here to Save" />
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlState" runat="server" DataTextField="Attribute1" DataValueField="LookUpID"
                                                    AutoPostBack="true" Width="100px" ValidationGroup="TaxExemptSetupSaveGroup" DataSourceID="StatesDataSource"
                                                    TabIndex="1">
                                                </asp:DropDownList>
                                                <asp:RequiredFieldValidator ID="rfvState" runat="server" ControlToValidate="ddlState"
                                                    InitialValue="0" ErrorMessage="Please select at least one State" ValidationGroup="TaxExemptSetupSaveGroup">*</asp:RequiredFieldValidator>
                                            </td>
                                            <td>
                                            </td>
                                        </tr>
                                    </InsertItemTemplate>
                                    <%--<EditItemTemplate>
                                        <tr class="ItemTemplate">
                                            <td>
                                                <asp:LinkButton ID="lbTaxExemptSetupUpdate" CommandName="Update" Text="Update" runat="server"
                                                    Visible="true" Width="40px" ValidationGroup="TaxExemptSetupEditGroup" ToolTip="Click here to Update" />
                                                <asp:LinkButton ID="lbTaxExemptSetupCancel" CommandName="Cancel" runat="server" Text="Cancel"
                                                    Visible="true" Width="40px" ToolTip="Click here to Cancel" />
                                            </td>
                                            <td>
                                                <asp:Label ID="lbldTaxesExemptSetupId" Width="165px" Visible="false" runat="server"
                                                    Text='<%# Bind("TAX_EXMP_SETUP_ID") %>'></asp:Label>
                                                <asp:DropDownList ID="ddlState" runat="server" DataTextField="Attribute1" DataValueField="LookUpID"
                                                    AutoPostBack="true" Width="100px" ValidationGroup="TaxExemptSetupEditGroup" DataSourceID="odsStates"
                                                    TabIndex="1">
                                                </asp:DropDownList>
                                                <asp:RequiredFieldValidator ID="rfvState" runat="server" ControlToValidate="ddlState"
                                                    InitialValue="0" ErrorMessage="Please select State" ValidationGroup="TaxExemptSetupEditGroup">*</asp:RequiredFieldValidator>
                                            </td>
                                            <td>
                                            </td>
                                        </tr>
                                    </EditItemTemplate>--%>
                                </asp:AISListView>
                            </asp:Panel>
                            <asp:UpdatePanel runat="server" ID="upTaxExempt">
                                <ContentTemplate>
                                    <asp:Button Style="display: none" runat="server" ID="btnFinalTemp" />
                                    <cc1:ModalPopupExtender runat="server" ID="modalTaxExept" TargetControlID="btnFinalTemp"
                                        PopupControlID="pnlTaxExemptPopup" BackgroundCssClass="modalBackground" DropShadow="true"
                                        CancelControlID="btnTaxExemptClose">
                                    </cc1:ModalPopupExtender>
                                    <div style="float: left;">
                                        <asp:Panel runat="server" CssClass="modalPopup" ID="pnlTaxExemptPopup" Style="border: solid 1px black;
                                            display: none; width: 650px; padding: 0px" HorizontalAlign="Center">
                                            <asp:Panel runat="Server" ID="Panel4" Style="width: 100%; cursor: move; padding: 0px;
                                                background-color: #CCCCCC; height: 20px; border: solid 1px Gray; color: Black;
                                                text-align: center; vertical-align: middle" Font-Bold="true">
                                                <asp:Label ID="lbltaxMessage" runat="server"></asp:Label>
                                            </asp:Panel>
                                            <div style="text-align: left; width: 100%; padding-bottom: 5px; background-color: White;">
                                                <asp:Panel ID="Panel1" runat="server" CssClass="content" Visible="true" ScrollBars="Auto">
                                                    <asp:Label ID="Label2" runat="server" CssClass="h3" Text="ILRF Tax Setup"></asp:Label>
                                                    &nbsp;
                                                    <asp:AISListView ID="ilrflisttaxsetup" runat="server">
                                                        <LayoutTemplate>
                                                            <table id="lstMITable" class="panelExtContents" runat="server" width="100%">
                                                                <tr class="LayoutTemplate">
                                                                    <th>
                                                                        LOB
                                                                    </th>
                                                                    <th>
                                                                        Tax Description
                                                                    </th>
                                                                    <th>
                                                                        Tax Amount
                                                                    </th>
                                                                    <th>
                                                                        Disable
                                                                    </th>
                                                                </tr>
                                                                <tr id="ItemPlaceHolder" runat="server">
                                                                </tr>
                                                            </table>
                                                        </LayoutTemplate>
                                                        <AlternatingItemTemplate>
                                                            <tr id="trItemTemplate" runat="server" class="AlternatingItemTemplate">
                                                                <td>
                                                                    <asp:Label ID="lblLOB" runat="server" Text='<%# Bind("LN_OF_BSN_TXT") %>'></asp:Label>
                                                                </td>
                                                                <td align="center">
                                                                    <asp:Label ID="lblILRFTaxID" Width="265px" Visible="false" runat="server" Text='<%# Bind("INCURRED_LOSS_REIM_FUND_TAX_ID") %>'></asp:Label>
                                                                    <asp:Label ID="lblILRFTaxType" Width="265px" Visible="true" runat="server" Text='<%# Eval("INCURRED_LOSS_REIM_FUND_TAX_TYPE")%>'></asp:Label>
                                                                </td>
                                                                <td align="center">
                                                                    <%# Eval("TAX_AMT") != null ? (Eval("TAX_AMT").ToString() != "" ? (decimal.Parse(Eval("TAX_AMT").ToString())).ToString("#,##0") : "0") : "0"%>
                                                                </td>
                                                                <td>
                                                                    <asp:HiddenField ID="hidActive" runat="server" Value='<%# Bind("ACTV_IND")%>' />
                                                                    <asp:ImageButton ID="imgDisable" Enabled="false" CommandArgument='<%# Bind("INCURRED_LOSS_REIM_FUND_TAX_ID") %>'
                                                                        runat="server" ImageUrl='<%# Eval("ACTV_IND").ToString()=="True"?"~/images/disabled.GIF":"~/images/enabled.GIF" %>'>
                                                                    </asp:ImageButton>
                                                                </td>
                                                            </tr>
                                                        </AlternatingItemTemplate>
                                                        <ItemTemplate>
                                                            <tr id="trItemTemplate" runat="server" class="ItemTemplate">
                                                                <td>
                                                                    <asp:Label ID="lblLOB" runat="server" Text='<%# Bind("LN_OF_BSN_TXT") %>'></asp:Label>
                                                                </td>
                                                                <td align="center">
                                                                    <asp:Label ID="lblILRFTaxID" Width="265px" Visible="false" runat="server" Text='<%# Bind("INCURRED_LOSS_REIM_FUND_TAX_ID") %>'></asp:Label><asp:Label
                                                                        ID="lblILRFTaxType" Width="265px" Visible="true" runat="server" Text='<%# Eval("INCURRED_LOSS_REIM_FUND_TAX_TYPE")%>'></asp:Label>
                                                                </td>
                                                                <td align="center">
                                                                    <%# Eval("TAX_AMT") != null ? (Eval("TAX_AMT").ToString() != "" ? (decimal.Parse(Eval("TAX_AMT").ToString())).ToString("#,##0") : "0") : "0"%>
                                                                </td>
                                                                <td>
                                                                    <asp:HiddenField ID="hidActive" runat="server" Value='<%# Bind("ACTV_IND")%>' />
                                                                    <asp:ImageButton ID="imgDisable" Enabled="false" CommandArgument='<%# Bind("INCURRED_LOSS_REIM_FUND_TAX_ID") %>'
                                                                        runat="server" ToolTip='<%#Eval("ACTV_IND").ToString()=="True"?"Click here to Disable":"Click here to Enable" %>'
                                                                        ImageUrl='<%# Eval("ACTV_IND").ToString()=="True"?"~/images/disabled.GIF":"~/images/enabled.GIF" %>'>
                                                                    </asp:ImageButton>
                                                                </td>
                                                            </tr>
                                                        </ItemTemplate>
                                                    </asp:AISListView>
                                                </asp:Panel>
                                            </div>
                                            <div style="text-align: center; width: 100%; padding-bottom: 2px; background-color: White;">
                                                <asp:Button Width="60px" OnClientClick="TaxExemptPopup()" ID="btnTaxExemptpopup"
                                                    runat="server" Text="Yes" OnClick="btnTaxExemptpopup_Click" />
                                                <asp:Button Width="60px" ID="btnTaxExemptClose" runat="server" Text="No" OnClick="btnTaxExemptClose_Click" />
                                                <br />
                                            </div>
                                        </asp:Panel>
                                    </div>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                            <asp:UpdatePanel runat="server" ID="UpdatePanel1">
                                <ContentTemplate>
                                    <asp:Button Style="display: none" runat="server" ID="btnTemp" />
                                    <cc1:ModalPopupExtender runat="server" ID="modalTaxExemptList" TargetControlID="btnTemp"
                                        PopupControlID="pnlTaxExemptlistPopup" BackgroundCssClass="modalBackground" DropShadow="true">
                                    </cc1:ModalPopupExtender>
                                    <div style="float: left;">
                                        <asp:Panel runat="server" CssClass="modalPopup" ID="pnlTaxExemptlistPopup" Style="border: solid 1px black;
                                            display: none; width: 650px; padding: 0px" HorizontalAlign="Center">
                                            <asp:Panel runat="Server" ID="Panel2" Style="width: 100%; cursor: move; padding: 0px;
                                                background-color: #CCCCCC; height: 20px; border: solid 1px Gray; color: Black;
                                                text-align: center; vertical-align: middle" Font-Bold="true">
                                                <asp:Label ID="lbltaxlistmessages" runat="server"></asp:Label>
                                            </asp:Panel>
                                            <div style="text-align: left; width: 100%; padding-bottom: 5px; background-color: White;">
                                                <asp:Panel ID="pnlilrfTax" runat="server" CssClass="content" Visible="true" ScrollBars="Auto">
                                                    <asp:Label ID="lblilrfTax" runat="server" CssClass="h3" Text="ILRF Tax Setup"></asp:Label>
                                                    &nbsp;
                                                    <asp:AISListView ID="lstilrftax" runat="server">
                                                        <LayoutTemplate>
                                                            <table id="lstMITable" class="panelExtContents" runat="server" width="100%">
                                                                <tr class="LayoutTemplate">
                                                                    <th>
                                                                        LOB
                                                                    </th>
                                                                    <th>
                                                                        Tax Description
                                                                    </th>
                                                                    <th>
                                                                        Tax Amount
                                                                    </th>
                                                                    <th>
                                                                        Disable
                                                                    </th>
                                                                </tr>
                                                                <tr id="ItemPlaceHolder" runat="server">
                                                                </tr>
                                                            </table>
                                                        </LayoutTemplate>
                                                        <AlternatingItemTemplate>
                                                            <tr id="trItemTemplate" runat="server" class="AlternatingItemTemplate">
                                                                <td>
                                                                    <asp:Label ID="lblLOB" runat="server" Text='<%# Bind("LN_OF_BSN_TXT") %>'></asp:Label>
                                                                </td>
                                                                <td align="center">
                                                                    <asp:Label ID="lblILRFTaxID" Width="265px" Visible="false" runat="server" Text='<%# Bind("INCURRED_LOSS_REIM_FUND_TAX_ID") %>'></asp:Label>
                                                                    <asp:Label ID="lblILRFTaxType" Width="265px" Visible="true" runat="server" Text='<%# Eval("INCURRED_LOSS_REIM_FUND_TAX_TYPE")%>'></asp:Label>
                                                                </td>
                                                                <td align="center">
                                                                    <%# Eval("TAX_AMT") != null ? (Eval("TAX_AMT").ToString() != "" ? (decimal.Parse(Eval("TAX_AMT").ToString())).ToString("#,##0") : "0") : "0"%>
                                                                </td>
                                                                <td>
                                                                    <asp:HiddenField ID="hidActive" runat="server" Value='<%# Bind("ACTV_IND")%>' />
                                                                    <asp:ImageButton ID="imgDisable" Enabled="false" CommandArgument='<%# Bind("INCURRED_LOSS_REIM_FUND_TAX_ID") %>'
                                                                        runat="server" ImageUrl='<%# Eval("ACTV_IND").ToString()=="True"?"~/images/disabled.GIF":"~/images/enabled.GIF" %>'>
                                                                    </asp:ImageButton>
                                                                </td>
                                                            </tr>
                                                        </AlternatingItemTemplate>
                                                        <ItemTemplate>
                                                            <tr id="trItemTemplate" runat="server" class="ItemTemplate">
                                                                <td>
                                                                    <asp:Label ID="lblLOB" runat="server" Text='<%# Bind("LN_OF_BSN_TXT") %>'></asp:Label>
                                                                </td>
                                                                <td align="center">
                                                                    <asp:Label ID="lblILRFTaxID" Width="265px" Visible="false" runat="server" Text='<%# Bind("INCURRED_LOSS_REIM_FUND_TAX_ID") %>'></asp:Label><asp:Label
                                                                        ID="lblILRFTaxType" Width="265px" Visible="true" runat="server" Text='<%# Eval("INCURRED_LOSS_REIM_FUND_TAX_TYPE")%>'></asp:Label>
                                                                </td>
                                                                <td align="center">
                                                                    <%# Eval("TAX_AMT") != null ? (Eval("TAX_AMT").ToString() != "" ? (decimal.Parse(Eval("TAX_AMT").ToString())).ToString("#,##0") : "0") : "0"%>
                                                                </td>
                                                                <td>
                                                                    <asp:HiddenField ID="hidActive" runat="server" Value='<%# Bind("ACTV_IND")%>' />
                                                                    <asp:ImageButton ID="imgDisable" Enabled="false" CommandArgument='<%# Bind("INCURRED_LOSS_REIM_FUND_TAX_ID") %>'
                                                                        runat="server" ToolTip='<%#Eval("ACTV_IND").ToString()=="True"?"Click here to Disable":"Click here to Enable" %>'
                                                                        ImageUrl='<%# Eval("ACTV_IND").ToString()=="True"?"~/images/disabled.GIF":"~/images/enabled.GIF" %>'>
                                                                    </asp:ImageButton>
                                                                </td>
                                                            </tr>
                                                        </ItemTemplate>
                                                    </asp:AISListView>
                                                </asp:Panel>
                                            </div>
                                            <div style="text-align: center; width: 100%; padding-bottom: 2px; background-color: White;">
                                                <asp:Button Width="60px" OnClientClick="TaxExemptlistPopup()" ID="btnTaxExemptlistpopup"
                                                    runat="server" Text="ok" OnClick="btnTaxExemptlistpopup_Click" />
                                                <br />
                                            </div>
                                        </asp:Panel>
                                    </div>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </ContentTemplate>
                    </cc1:TabPanel>
                </cc1:TabContainer>
            </td>
        </tr>
    </table>
</asp:Content>
