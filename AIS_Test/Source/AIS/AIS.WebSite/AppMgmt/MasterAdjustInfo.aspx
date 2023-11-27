<%@ Page Language="C#" MasterPageFile="~/Retro.master" AutoEventWireup="true" Inherits="AppMgmt_MasterAdjustInfo"
    Title="Master Adjustment Infomation" CodeBehind="MasterAdjustInfo.aspx.cs" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeaderPlaceHolder" runat="Server">
    <table width="910" style="text-align: right">
        <tr>
            <td align="left">
                <asp:Label ID="GAILabel" runat="server" Text="Master Adjustment Information" CssClass="h1"></asp:Label>
            </td>
            <td align="right">
                <asp:Button ID="btnAdd" Text="Add New" runat="server" OnClick="btnAdd_Click" />
            </td>
        </tr>
    </table>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainPlaceHolder" runat="Server">

    <script language="javascript" type="text/javascript">

        var scrollTop1;
        var scrollTop2;

        if (Sys.WebForms.PageRequestManager != null) {
            Sys.WebForms.PageRequestManager.getInstance().add_beginRequest(BeginRequestHandler);
            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);
        }

        function BeginRequestHandler(sender, args) {
            var mef = $get('<%=pnlMasterERPFormula.ClientID%>');
            if (mef != null)
                scrollTop1 = mef.scrollTop;

            var pnl = $get('<%=Panel2.ClientID%>');
            if (pnl != null)
                scrollTop2 = pnl.scrollTop;
        }

        function EndRequestHandler(sender, args) {
            var mef = $get('<%=pnlMasterERPFormula.ClientID%>');
            if (mef != null)
                mef.scrollTop = scrollTop1;

            var pnl = $get('<%=Panel2.ClientID%>');
            if (pnl != null)
                pnl.scrollTop = scrollTop2;
        }


        function ERPFormula(Component) {
            if ($get('<%=hidSecurity.ClientID%>').value == '1') {
                return false;
            }
            document.getElementById('ctl00_hdnControlDirty').value = '1';
            document.getElementById('btnremove').disabled = false;
            var tabPanel = $get('<%=pnlDetails.ClientID%>');
            //if (tabPanel.disabled == false) {
                var hidText = $get('<%=hidText.ClientID%>');
                var txtFormula = $get('<%=txtFormulaTwo.ClientID%>');
                txtFormula.value = txtFormula.value + Component;
                if (hidText.value == "") {
                    hidText.value = hidText.value + Component;
                }
                else {
                    hidText.value = hidText.value + "@" + Component;
                }
            //}
        }
        function removeall() {
            if ($get('<%=hidSecurity.ClientID%>').value == '1') {
                return false;
            }
            var tabPanel = $get('<%=pnlDetails.ClientID%>');
            //if (tabPanel.disabled == false) {
                document.getElementById('ctl00_hdnControlDirty').value = '1';
                $get('<%=txtFormulaTwo.ClientID%>').value = "";
                $get('<%=hidText.ClientID%>').value = "";
            //}
        }
        function removeone() {
            if ($get('<%=hidSecurity.ClientID%>').value == '1') {
                return false;
            }
            var tabPanel = $get('<%=pnlDetails.ClientID%>');
            //if (tabPanel.disabled == false) {
                var txtFormula = $get('<%=txtFormulaTwo.ClientID%>');
                document.getElementById('ctl00_hdnControlDirty').value = '1';
                txtFormula.value = "";
                var hidText = $get('<%=hidText.ClientID%>');
                var aryTest = hidText.value.split('@');
                hidText.value = "";
                var i = 0;
                for (i = 0; i < aryTest.length - 1; i++) {
                    txtFormula.value = txtFormula.value + aryTest[i];
                    if (hidText.value == "") {
                        hidText.value = aryTest[i];
                    }
                    else {
                        hidText.value = hidText.value + "@" + aryTest[i];
                    }
                }
            //}
        }

    </script>

    <table>
        <tr>
            <td>
                <asp:ValidationSummary ID="valSumaSave" CssClass="ValidationSummary" ValidationGroup="Save"
                    runat="server"></asp:ValidationSummary>
                <asp:ValidationSummary ID="valSumaSaveKYOR" CssClass="ValidationSummary" ValidationGroup="SaveKYOR"
                    runat="server"></asp:ValidationSummary>
                <asp:ValidationSummary ID="valSumaSaveDTaxes" CssClass="ValidationSummary" ValidationGroup="SaveDTaxes"
                    runat="server"></asp:ValidationSummary>
                <asp:ValidationSummary ID="ValSummaryEditDtaxes" CssClass="ValidationSummary" ValidationGroup="EditDTaxes"
                    runat="server"></asp:ValidationSummary>
                <asp:ValidationSummary ID="valSummarySaveSurTaxes" CssClass="ValidationSummary" ValidationGroup="SaveSurTaxes"
                    runat="server"></asp:ValidationSummary>
                <asp:ValidationSummary ID="ValSummaryEditSurtaxes" CssClass="ValidationSummary" ValidationGroup="EditSurTaxes"
                    runat="server"></asp:ValidationSummary>
                <ajaxToolkit:TabContainer ID="TabContainer1" runat="server" CssClass="CustomTabs"
                    ActiveTabIndex="0" OnActiveTabChanged="TabContainer1_ActiveTabChanged" AutoPostBack="true">
                    <ajaxToolkit:TabPanel runat="server" ID="tblpnlERP">
                        <HeaderTemplate>
                            Master ERP Formulas
                        </HeaderTemplate>
                        <ContentTemplate>
                            <asp:Panel runat="server" ID="pnlERP" Width="910px">
                                <asp:HiddenField ID="hidSecurity" runat="server" Value="0" />
                                <br />
                                <asp:Panel ID="pnlMasterERPFormula" runat="server" Height="140px" ScrollBars="Auto"
                                    Width="910px">
                                    <asp:AISListView ID="lstMasterERPFormula" runat="server" DataKeyNames="FormulaID"
                                        OnItemCommand="ERPCommandList" OnItemDataBound="DataBoundList" OnSelectedIndexChanging="lstMasterERPFormula_SelectedIndexChanging">
                                        <AlternatingItemTemplate>
                                            <tr class="AlternatingItemTemplate" id="trItemTemplate" runat="server">
                                                <td align="left">
                                                    <asp:LinkButton ID="lnkerpSelect" CommandArgument='<%# Bind("FormulaID") %>' CommandName="Select"
                                                        runat="server" Text="Select"></asp:LinkButton>
                                                </td>
                                                <td align="left">
                                                    <asp:Label ID="lblFormulaTwo" runat="server" Visible="false" Text='<%# Bind("FormulaOneText") %>'></asp:Label>
                                                    <asp:Label ID="lblFormulaOne" runat="server" Text='<%# Bind("FormulaTwoText") %>'></asp:Label>
                                                </td>
                                                <td align="left">
                                                    <asp:Label ID="lblDesc" runat="server" Text='<%# Bind("FormulaDescription") %>'></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:HiddenField ID="hidFormulaTwo" runat="server" Value='<%# Bind("FormulaTwoText")%>' />
                                                    <asp:HiddenField ID="hidActive" runat="server" Value='<%# Bind("IsActive")%>' />
                                                    <asp:ImageButton ID="imgDisable" CommandArgument='<%# Bind("FormulaID") %>' runat="server"
                                                        ImageUrl='<%# Eval("IsActive").ToString()=="True"?"~/images/disabled.GIF":"~/images/enabled.GIF" %>'>
                                                    </asp:ImageButton>
                                                </td>
                                            </tr>
                                        </AlternatingItemTemplate>
                                        <ItemTemplate>
                                            <tr class="ItemTemplate" id="trItemTemplate" runat="server">
                                                <td align="left">
                                                    <asp:LinkButton ID="lnkerpSelect" runat="server" CommandArgument='<%# Bind("FormulaID") %>'
                                                        CommandName="Select" Text="Select"></asp:LinkButton>
                                                </td>
                                                <td align="left">
                                                    <asp:Label ID="lblFormulaOne" runat="server" Text='<%# Bind("FormulaTwoText") %>'></asp:Label>
                                                    <asp:Label ID="lblFormulaTwo" runat="server" Visible="false" Text='<%# Bind("FormulaOneText") %>'></asp:Label>
                                                </td>
                                                <td align="left">
                                                    <asp:Label ID="lblDesc" runat="server" Text='<%# Bind("FormulaDescription") %>'></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:HiddenField ID="hidFormulaTwo" runat="server" Value='<%# Bind("FormulaTwoText")%>' />
                                                    <asp:HiddenField ID="hidActive" runat="server" Value='<%# Bind("IsActive")%>' />
                                                    <asp:ImageButton ID="imgDisable" CommandArgument='<%# Bind("FormulaID") %>' runat="server"
                                                        ImageUrl='<%# Eval("IsActive").ToString()=="True"?"~/images/disabled.GIF":"~/images/enabled.GIF" %>'>
                                                    </asp:ImageButton>
                                                </td>
                                            </tr>
                                        </ItemTemplate>
                                        <LayoutTemplate>
                                            <table id="Table1" class="panelContents" runat="server" width="98%">
                                                <tr class="LayoutTemplate">
                                                    <th>
                                                    </th>
                                                    <th>
                                                        ERP Formula
                                                    </th>
                                                    <th>
                                                        Description
                                                    </th>
                                                    <th>
                                                        Disable
                                                    </th>
                                                </tr>
                                                <tr id="itemPlaceholder" runat="server">
                                                </tr>
                                            </table>
                                        </LayoutTemplate>
                                    </asp:AISListView>
                                </asp:Panel>
                                <br />
                                <div>
                                    <asp:Label CssClass="h2" Visible="false" ID="lblDetails" runat="server" Text="ERP Formula Details"></asp:Label>
                                    &nbsp;&nbsp;
                                    <asp:LinkButton Visible="false" Text="Close" runat="server" ID="lnkClose" OnClick="lnkClose_Click"></asp:LinkButton>
                                </div>
                                <asp:Panel Visible="false" Enabled="false" runat="server" ID="pnlDetails" BorderWidth="1px">
                                    <table width="100%">
                                        <tr>
                                            <td style="width: 20%">
                                                <asp:Panel ID="pnlERPList" runat="server" Height="210px" ScrollBars="Auto" Width="215px">
                                                    <asp:AISListView ID="LstERP" runat="server" DataKeyNames="LookupID" OnItemDataBound="LstERP_DataBoundList">
                                                        <AlternatingItemTemplate>
                                                            <tr class="AlternatingItemTemplate" id="trItemTemplate" runat="server">
                                                                <td align="center">
                                                                    <asp:Label ID="lblFormula" Text='<%#Bind("LookUpName") %>' runat="server"></asp:Label>
                                                                </td>
                                                            </tr>
                                                        </AlternatingItemTemplate>
                                                        <ItemTemplate>
                                                            <tr class="AlternatingItemTemplate" id="trItemTemplate" runat="server">
                                                                <td align="center">
                                                                    <asp:Label ID="lblFormula" Text='<%#Bind("LookUpName") %>' runat="server"></asp:Label>
                                                                </td>
                                                            </tr>
                                                        </ItemTemplate>
                                                        <LayoutTemplate>
                                                            <table id="Table1" class="panelContents" runat="server" width="195px" style="font-weight: bold;
                                                                border-color: Black; cursor: hand;" cellpadding="0" border="1" cellspacing="0">
                                                                <tr class="LayoutTemplate">
                                                                    <th>
                                                                        <b>Formula Components</b>
                                                                    </th>
                                                                </tr>
                                                                <tr id="itemPlaceholder" runat="server">
                                                                </tr>
                                                            </table>
                                                        </LayoutTemplate>
                                                    </asp:AISListView>
                                                </asp:Panel>
                                            </td>
                                            <td style="width: 10%; padding-left: 5px">
                                                <br />
                                                <br />
                                                <br />
                                                <br />
                                                <input onclick="removeall()" style="color: White; background-color: #9494C8; width: 25px"
                                                    type="button" value="<<" name="btnremoveall" id="btnremoveall" />
                                                <asp:HiddenField ID="hidText" runat="server"></asp:HiddenField>
                                                <br />
                                                <br />
                                                <input onclick="removeone()" style="color: White; background-color: #9494C8; width: 25px"
                                                    type="button" value="<" name="btnremove" id="btnremove" />
                                            </td>
                                            <td style="width: 70%">
                                                ERP FORMULA DESCRIPTION
                                                <br />
                                                <asp:TextBox MaxLength="100" ID="txtDescription" ValidationGroup="Save" runat="server"
                                                    Width="350px"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="reqDesc" runat="server" ValidationGroup="Save" ControlToValidate="txtDescription"
                                                    ErrorMessage="Please Enter ERP Formula Description" Text="*"></asp:RequiredFieldValidator>
                                                <ajaxToolkit:FilteredTextBoxExtender TargetControlID="txtDescription" FilterType="UppercaseLetters,Custom, LowercaseLetters, Numbers"
                                                    ValidChars="_ " ID="fltrtxtDescription" runat="server" />
                                                <br />
                                                <br />
                                                ERP FORMULA - Displayed in Invoice Exhibit
                                                <br />
                                                <asp:TextBox ID="txtFormulaOne" runat="server" ValidationGroup="Save" ToolTip="The formula entered in this textbox is displayed in Invoice Exhibits."
                                                    Width="600px"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="reqtxtFormulaOne" runat="server" ValidationGroup="Save"
                                                    ControlToValidate="txtFormulaOne" ErrorMessage="Please Enter ERP FORMULA - Displayed in Invoice Exhibit"
                                                    Text="*"></asp:RequiredFieldValidator>
                                                <br />
                                                ERP FORMULA - Used by Adjustment Calculation Module
                                                <br />
                                                <asp:TextBox ID="txtFormulaTwo" runat="server" ValidationGroup="Save" ToolTip="The formula entered in this textbox is used by the ERP Calculation Module."
                                                    TextMode="MultiLine" Width="600px" Height="50px" contenteditable="false"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="reqtxtFormulaTwo" runat="server" ValidationGroup="Save"
                                                    ControlToValidate="txtFormulaTwo" ErrorMessage="Please Enter  ERP FORMULA - Used by Adjustment Calculation Module"
                                                    Text="*"></asp:RequiredFieldValidator>
                                                <div style="text-align: right">
                                                    <asp:Button ID="btnSave" Text="Save" ValidationGroup="Save" runat="server" OnClick="btnSave_Click" />
                                                    <asp:Button ID="btnCancel" Text="Cancel" runat="server" OnClick="btnCancel_Click" />
                                                    &nbsp;&nbsp;
                                                </div>
                                            </td>
                                        </tr>
                                    </table>
                                </asp:Panel>
                            </asp:Panel>
                        </ContentTemplate>
                    </ajaxToolkit:TabPanel>
                    <ajaxToolkit:TabPanel runat="server" ID="tabKY" Enabled =false>
                        <HeaderTemplate>
                            KY &amp; OR Setup
                        </HeaderTemplate>
                        <ContentTemplate>
                            <br />
                            <br />
                            <asp:Label ID="lblHeader" runat="server" CssClass="h2" Text="KY & OR Setup"></asp:Label>
                            <asp:Panel ID="Panel2" runat="server" Height="200px" Width="910px" ScrollBars="Auto"
                                Visible="true">
                                <asp:AISListView ID="lstKOSetup" runat="server" OnItemDataBound="lstKOSetup_DataBoundList"
                                    InsertItemPosition="FirstItem" OnItemEditing="lstKOSetup_ItemEdit" OnItemCanceling="lstKOSetup_ItemCancel"
                                    OnItemCommand="lstKOSetup_ItemCommand" OnItemInserting="lstKOSetup_ItemInserting"
                                    OnItemUpdating="lstKOSetup_ItemUpdate">
                                    <LayoutTemplate>
                                        <table id="Table1" class="panelContents" runat="server" width="98%">
                                            <tr class="LayoutTemplate">
                                                <th>
                                                </th>
                                                <th>
                                                    Effective Date
                                                </th>
                                                <th>
                                                    Kentucky
                                                </th>
                                                <th>
                                                    Oregon
                                                </th>
                                            </tr>
                                            <tr id="ItemPlaceHolder" runat="server">
                                            </tr>
                                        </table>
                                    </LayoutTemplate>
                                    <ItemTemplate>
                                        <tr id="Tr1" runat="server" class="ItemTemplate">
                                            <td>
                                                <asp:LinkButton ID="lbContactEdit" CommandName="Edit" Text="Edit" runat="server"
                                                    Visible="true" Width="100px" />
                                            </td>
                                            <td>
                                                <asp:Label ID="lblEffDt" runat="server" Text='<%# Eval("EFF_DT", "{0:d}") %>'></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="lblKentucky" runat="server" Text='<%# Bind("KY_FCTR_RT") %>' />
                                            </td>
                                            <td>
                                                <asp:Label ID="lblOregon" runat="server" Text='<%# Bind("OR_FCTR_RT") %>' />
                                            </td>
                                        </tr>
                                    </ItemTemplate>
                                    <AlternatingItemTemplate>
                                        <tr id="Tr1" runat="server" class="AlternatingItemTemplate">
                                            <td>
                                                <asp:LinkButton ID="lbContactEdit" CommandName="Edit" Text="Edit" runat="server"
                                                    Visible="true" Width="100px" />
                                            </td>
                                            <td>
                                                <asp:Label ID="lblEffDt" runat="server" Text='<%# Eval("EFF_DT", "{0:d}") %>'></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="lblKentucky" runat="server" Text='<%# Bind("KY_FCTR_RT") %>' />
                                            </td>
                                            <td>
                                                <asp:Label ID="lblOregon" runat="server" Text='<%# Bind("OR_FCTR_RT") %>' />
                                            </td>
                                        </tr>
                                    </AlternatingItemTemplate>
                                    <InsertItemTemplate>
                                        <tr id="tr2" runat="server" class="AlternatingItemTemplate">
                                            <td align="center">
                                                <asp:LinkButton ID="lblContactInsert" CommandName="Save" Text="Save" runat="server"
                                                    Visible="true" Width="100px" ValidationGroup="SaveKYOR" />
                                            </td>
                                            <td align="center">
                                                <asp:TextBox ID="txtEffDt" MaxLength="10" runat="server" SkinID="largeTextbox" TabIndex="4"
                                                    ContentEditable="false" ValidationGroup="SaveKYOR"></asp:TextBox>
                                                <ajaxToolkit:CalendarExtender ID="txtEffDt_CalendarExtender" runat="server" Enabled="True"
                                                    TargetControlID="txtEffDt" Format="MM/dd/yyyy" PopupPosition="BottomRight">
                                                </ajaxToolkit:CalendarExtender>
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtEffDt"
                                                    ErrorMessage="Please enter Effective Date" ValidationGroup="SaveKYOR">*</asp:RequiredFieldValidator>
                                            </td>
                                            <td align="center">
                                                <asp:TextBox ID="txtKentucky" runat="server" />
                                                <ajaxToolkit:MaskedEditExtender ID="KentuckyTextBoxExtender" runat="server" Mask="9999.999999"
                                                    MaskType="Number" TargetControlID="txtKentucky" InputDirection="RightToLeft"
                                                    AutoComplete="false">
                                                </ajaxToolkit:MaskedEditExtender>
                                            </td>
                                            <td align="center">
                                                <asp:TextBox ID="txtOregon" runat="server" />
                                                <ajaxToolkit:MaskedEditExtender ID="OregonTextBoxExtender" runat="server" Mask="9999.999999"
                                                    MaskType="Number" TargetControlID="txtOregon" InputDirection="RightToLeft" AutoComplete="false">
                                                </ajaxToolkit:MaskedEditExtender>
                                            </td>
                                        </tr>
                                    </InsertItemTemplate>
                                    <EditItemTemplate>
                                        <tr>
                                            <td align="center">
                                                <asp:LinkButton ID="lblContactUpdate" CommandName="Update" Text="Update" runat="server"
                                                    Visible="true" Width="50px" ValidationGroup="EditGroup" />
                                                <asp:LinkButton ID="lblContactCancel" CommandName="Cancel" runat="server" Text="Cancel"
                                                    Visible="true" Width="50px" ValidationGroup="EditGroup" />
                                            </td>
                                            <td align="center">
                                                <asp:Label ID="hdKyOrSetupId" Width="100px" Visible="false" runat="server" Text='<%# Bind("KY_OR_SETUP_ID") %>'></asp:Label>
                                                <asp:TextBox ID="txtEffDt" MaxLength="10" runat="server" SkinID="largeTextbox" TabIndex="4"
                                                    ContentEditable="false" Text='<%# Eval("EFF_DT", "{0:d}") %>'></asp:TextBox>
                                                <ajaxToolkit:CalendarExtender ID="txtEffDt_CalendarExtender" runat="server" Enabled="True"
                                                    TargetControlID="txtEffDt" Format="MM/dd/yyyy" PopupPosition="BottomRight">
                                                </ajaxToolkit:CalendarExtender>
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="txtEffDt"
                                                    ErrorMessage="Please Enter Contact Name" ValidationGroup="EditGroup">*</asp:RequiredFieldValidator>
                                            </td>
                                            <td align="center">
                                                <asp:TextBox ID="txtKentucky" runat="server" MaxLength="5" Text='<%# Bind("KY_FCTR_RT") %>' />
                                                <ajaxToolkit:MaskedEditExtender ID="Kentucky1TextBoxExtender" runat="server" Mask="9999.999999"
                                                    MaskType="Number" TargetControlID="txtKentucky" InputDirection="RightToLeft"
                                                    AutoComplete="false">
                                                </ajaxToolkit:MaskedEditExtender>
                                            </td>
                                            <td align="center">
                                                <asp:TextBox ID="txtOregon" runat="server" Text='<%# Bind("OR_FCTR_RT") %>' />
                                                <ajaxToolkit:MaskedEditExtender ID="Oregon1TextBoxExtender" runat="server" Mask="9999.999999"
                                                    MaskType="Number" TargetControlID="txtOregon" InputDirection="RightToLeft" AutoComplete="false">
                                                </ajaxToolkit:MaskedEditExtender>
                                            </td>
                                        </tr>
                                    </EditItemTemplate>
                                </asp:AISListView>
                            </asp:Panel>
                        </ContentTemplate>
                    </ajaxToolkit:TabPanel>
                    <ajaxToolkit:TabPanel runat="server" ID="tabSurcharges">
                        <HeaderTemplate>
                            Surcharges Setup
                        </HeaderTemplate>
                        <ContentTemplate>
                            <br />
                            <br />
                            <asp:ObjectDataSource ID="SurStatesDataSource" runat="server" SelectMethod="GetStates"
                                TypeName="ZurichNA.AIS.Business.Logic.SurchargesBS"></asp:ObjectDataSource>
                            <asp:ObjectDataSource ID="SurodsStates" runat="server" SelectMethod="GetStatesForEdit"
                                TypeName="ZurichNA.AIS.Business.Logic.SurchargesBS">
                                <SelectParameters>
                                    <asp:Parameter Name="iLkupId" Type="Int32" />
                                </SelectParameters>
                            </asp:ObjectDataSource>
                            <asp:ObjectDataSource ID="FactorDescriptionDS" runat="server" SelectMethod="FactorDescription"
                                TypeName="ZurichNA.AIS.Business.Logic.SurchargesBS"></asp:ObjectDataSource>
                                <asp:ObjectDataSource ID="odsFactorDescriptionEdit" runat="server" SelectMethod="FactorDescriptionForEdit"
                                TypeName="ZurichNA.AIS.Business.Logic.SurchargesBS">
                                <SelectParameters>
                                    <asp:Parameter Name="iFactorId" Type="Int32" />
                                </SelectParameters>
                                </asp:ObjectDataSource>
                            <asp:ObjectDataSource ID="ObjectCovTypeSur" runat="server" SelectMethod="GetLookUpLOBActiveData"
                                TypeName="ZurichNA.AIS.Business.Logic.BLAccess">
                                <SelectParameters>
                                    <asp:Parameter DefaultValue="LOB" Name="lookUpTypeName" Type="String" />
                                </SelectParameters>
                            </asp:ObjectDataSource>
                            <asp:Label ID="lblSurcharges" runat="server" CssClass="h2" Text="Surcharges Setup"></asp:Label>
                            <asp:Panel ID="pnlSurcharges" runat="server" Height="400px" Width="910px" ScrollBars="Auto"
                                Visible="true">
                                <asp:AISListView ID="lstSurcharges" runat="server" OnItemDataBound="lstSurcharges_DataBoundList"
                                    InsertItemPosition="FirstItem" OnItemEditing="lstSurcharges_ItemEdit" OnItemCanceling="lstSurcharges_ItemCancel"
                                     OnItemUpdating="lstSurcharges_ItemUpdate" OnItemCommand="lstSurcharges_ItemCommand"
                                    OnItemInserting="lstSurcharges_ItemInserting">
                                    <LayoutTemplate>
                                        <table id="tblSurcharges" class="panelContents" runat="server" width="98%">
                                            <tr class="LayoutTemplate">
                                                <th>
                                                </th>
                                                <th>
                                                    LOB
                                                </th>
                                                <th>
                                                    State
                                                </th>
                                                <th>
                                                    Surcharge<br /> Code
                                                </th>
                                                 <th>
                                                    Surcharge/Assessment<br /> Description
                                                </th>
                                                <th>
                                                    Surcharge Eff. Date
                                                </th>
                                                <th>
                                                    Surcharge<br /> Factor
                                                </th>
                                                <th>
                                                    Surcharge<br /> Date<br /> Indicator
                                                </th>
                                                <th>
                                                    Disable
                                                </th>
                                            </tr>
                                            <tr id="ItemPlaceHolder" runat="server">
                                            </tr>
                                        </table>
                                    </LayoutTemplate>
                                    <ItemTemplate>
                                        <tr id="Tr4" runat="server" class="ItemTemplate">
                                            <td>
                                                <asp:LinkButton ID="lnkSurcharges" CommandName="Edit" Text="Edit" runat="server"
                                                    Visible="true" Enabled='<%# Eval("ACTV_IND")%>' />
                                            </td>
                                            <td>
                                                <asp:Label ID="lblLOB" runat="server" Text='<%# Bind("LN_OF_BSN_TXT") %>'></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="lblState" runat="server" Text='<%# Bind("STATEDESCRIPTION") %>'></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="lblSurCode" runat="server" Text='<%# Bind("STATE_SURCHARGE_CODE") %>' />
                                            </td>
                                            <td>
                                                <asp:Label ID="lblSurDescription" runat="server" Text='<%# Bind("SURCHARGE_DESCRIPTION") %>' />
                                            </td>
                                            <td>
                                                <asp:Label ID="lblSurEffDate" runat="server" Text='<%# Eval("SURCHARGE_EFF_DT", "{0:d}")  %>'></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="lblSurRate" runat="server" Text='<%# Bind("SURCHARGE_RATE") %>' />
                                            </td>
                                            <td>
                                                <asp:Label ID="lblSurFactInd" runat="server" Text='<%# Bind("SURCHARGE_FACTOR") %>'></asp:Label>
                                            </td>
                                            <td>
                                                <asp:HiddenField ID="hidActive" runat="server" Value='<%# Bind("ACTV_IND")%>' />
                                                <asp:ImageButton ID="imgDisable" CommandArgument='<%# Bind("SURCHARGE_ASSESS_SETUP_ID") %>'
                                                    runat="server" ToolTip='<%#Eval("ACTV_IND").ToString()=="True"?"Click here to Disable":"Click here to Enable" %>'
                                                    ImageUrl='<%# Eval("ACTV_IND").ToString()=="True"?"~/images/disabled.GIF":"~/images/enabled.GIF" %>'>
                                                </asp:ImageButton>
                                            </td>
                                        </tr>
                                    </ItemTemplate>
                                    <AlternatingItemTemplate>
                                        <tr id="Tr5" runat="server" class="AlternatingItemTemplate">
                                            <td>
                                                <asp:LinkButton ID="lnkEdit" CommandName="Edit" Enabled='<%# Eval("ACTV_IND")%>'
                                                    Text="Edit" runat="server" Visible="true" />
                                            </td>
                                            <td>
                                                <asp:Label ID="lblLOB" runat="server" Text='<%# Bind("LN_OF_BSN_TXT") %>'></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="lblState" runat="server" Text='<%# Bind("STATEDESCRIPTION") %>'></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="lblSurCode" runat="server" Text='<%# Bind("STATE_SURCHARGE_CODE") %>' />
                                            </td>
                                            <td>
                                                <asp:Label ID="lblSurDescription" runat="server" Text='<%# Bind("SURCHARGE_DESCRIPTION") %>' />
                                            </td>
                                            <td>
                                                <asp:Label ID="lblSurEffDate" runat="server" Text='<%# Eval("SURCHARGE_EFF_DT", "{0:d}")  %>'></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="lblSurRate" runat="server" Text='<%# Bind("SURCHARGE_RATE") %>' />
                                            </td>
                                            <td>
                                                <asp:Label ID="lblSurFactInd" runat="server" Text='<%# Bind("SURCHARGE_FACTOR") %>'></asp:Label>
                                            </td>
                                            <td>
                                                <asp:HiddenField ID="hidActive" runat="server" Value='<%# Bind("ACTV_IND")%>' />
                                                <asp:ImageButton ID="imgDisable" CommandArgument='<%# Bind("SURCHARGE_ASSESS_SETUP_ID") %>'
                                                    runat="server" ToolTip='<%#Eval("ACTV_IND").ToString()=="True"?"Click here to Disable":"Click here to Enable" %>'
                                                    ImageUrl='<%# Eval("ACTV_IND").ToString()=="True"?"~/images/disabled.GIF":"~/images/enabled.GIF" %>'>
                                                </asp:ImageButton>
                                            </td>
                                        </tr>
                                    </AlternatingItemTemplate>
                                    <InsertItemTemplate>
                                        <tr id="tr2" runat="server" class="AlternatingItemTemplate">
                                            <td align="center">
                                                <asp:LinkButton ID="lnkSurchargesInsert" CommandName="Save" Text="Save" runat="server"
                                                    Visible="true" ValidationGroup="SaveSurTaxes" />
                                            </td>
                                            <td>
                                                <asp:Label ID="lblLOB" Width="120px" Visible="false" runat="server" Text='<%# Bind("LN_OF_BSN_ID") %>'></asp:Label>
                                                <asp:DropDownList ID="ddlLOB" runat="server" DataSourceID="ObjectCovTypeSur" ValidationGroup="SaveSurTaxes"
                                                    Width="70px" DataTextField="LookUpName" DataValueField="LookUpID" TabIndex="1">
                                                </asp:DropDownList>
                                                <asp:RequiredFieldValidator ID="rfvSurCoverageType" runat="server" ControlToValidate="ddlLOB"
                                                    InitialValue="0" ErrorMessage="Please select at least one Line of Business." ValidationGroup="SaveSurTaxes">*</asp:RequiredFieldValidator>
                                            </td>
                                            <td align="center">
                                                <asp:DropDownList ID="ddlState" runat="server" DataTextField="Attribute1" DataValueField="LookUpID"
                                                    AutoPostBack="true" OnSelectedIndexChanged="ddlState_SelectedIndexChangedSur" Width="80px" ValidationGroup="SaveSurTaxes" DataSourceID="SurStatesDataSource"
                                                     TabIndex="2">
                                                </asp:DropDownList>
                                                <asp:RequiredFieldValidator ID="rfvState" runat="server" ControlToValidate="ddlState"
                                                    InitialValue="0" ErrorMessage="Please select at least one State." ValidationGroup="SaveSurTaxes">*</asp:RequiredFieldValidator>
                                            </td>
                                            <td align="center">
                                                <asp:DropDownList ID="ddlSurCode" runat="server" DataTextField="LookUpTypeName"
                                                    DataValueField="LookUpID" OnSelectedIndexChanged="ddlCode_SelectedIndexChanged" AutoPostBack="true" Width="75px" ValidationGroup="SaveSurTaxes"
                                                     TabIndex="3">
                                                </asp:DropDownList>
                                                <asp:RequiredFieldValidator ID="rfvSurCode" runat="server" ControlToValidate="ddlSurCode"
                                                    ErrorMessage="Please select at least one Surcharge code." InitialValue="0" ValidationGroup="SaveSurTaxes">*</asp:RequiredFieldValidator>
                                            </td>
                                            <td align="center">
                                                <asp:Label ID="lblSurDescription" runat="server"  Text='<%# Bind("SURCHARGE_DESCRIPTION") %>' />
                                                <asp:HiddenField ID="hidSurDescriptionID" runat="server" Value='<%# Bind("SURCHARGE_TYPE_ID") %>' />
                                            </td>
                                            <td align="center" nowrap>
                                                <asp:TextBox ID="txtSurEffDate" MaxLength="10" runat="server" SkinID="largeTextbox" Width="90px"
                                                    TabIndex="5" ValidationGroup="SaveSurTaxes"></asp:TextBox>
                                                <asp:ImageButton ID="imgEffDate" runat="server" ImageUrl="~/images/Calendar_scheduleHS.png"
                                                    CausesValidation="False" />
                                                <ajaxToolkit:CalendarExtender ID="CalendarExtender1" runat="server" Enabled="True"
                                                    TargetControlID="txtSurEffDate" Format="MM/dd/yyyy" PopupPosition="BottomRight"
                                                    PopupButtonID="imgEffDate">
                                                </ajaxToolkit:CalendarExtender>
                                                <ajaxToolkit:MaskedEditExtender ID="mskEffDate" runat="server" AcceptNegative="Left"
                                                    DisplayMoney="Left" ErrorTooltipEnabled="True" Mask="99/99/9999" MaskType="Date"
                                                    MessageValidatorTip="true" OnFocusCssClass="MaskedEditFocus" OnInvalidCssClass="MaskedEditError"
                                                    TargetControlID="txtSurEffDate" />
                                                <asp:RegularExpressionValidator ID="regSurEffDate" runat="server" ControlToValidate="txtSurEffDate"
                                                    ValidationExpression="(^(?:(?:(?:0?[13578]|1[02])(\/|-)31)|(?:(?:0?[1,3-9]|1[0-2])(\/|-)(?:29|30)))(\/|-)(?:(1[8-9]|[2-8]\d)\d\d)$|^(?:(?:0?[1-9]|1[0-2])(\/|-)(?:0?[1-9]|1\d|2[0-8]))(\/|-)(?:(1[8-9]|[2-8]\d)\d\d)$|^(0?2(\/|-)29)(\/|-)(?:(?:0[48]00|[13579][26]00|[2468][048]00)|(?:\d\d)?(?:0[48]|[2468][048]|[13579][26]))$)"
                                                    ErrorMessage="Please enter valid Surcharge Effective Date." Text="*" ValidationGroup="SaveSurTaxes"></asp:RegularExpressionValidator>
                                                <asp:RequiredFieldValidator ID="rfvSurEffDate" runat="server" ControlToValidate="txtSurEffDate"
                                                    ErrorMessage="Please enter Surcharge Effective Date." ValidationGroup="SaveSurTaxes">*</asp:RequiredFieldValidator>
                                                
                                            </td>
                                            <td align="center">
                                                <asp:TextBox ID="txtSurRate" runat="server" Width="88px" TabIndex="6"></asp:TextBox>
                                                <ajaxToolkit:MaskedEditExtender ID="maskSurFactor" runat="server" Mask="9999.99999999"
                                                    MaskType="Number" InputDirection="RightToLeft" TargetControlID="txtSurRate" />
                                                <asp:RequiredFieldValidator ID="rfvSurRate" runat="server" ControlToValidate="txtSurRate"
                                                    ErrorMessage="Please enter Surcharge Factor." ValidationGroup="SaveSurTaxes">*</asp:RequiredFieldValidator>
                                            </td>
                                            
                                            <td align="center">
                                                <asp:DropDownList ID="ddlChargedDateInd" runat="server" Width="72px" ValidationGroup="SaveSurTaxes"
                                                    TabIndex="7" DataSourceID="FactorDescriptionDS" DataTextField="LookUpName" DataValueField="LookUpID">
                                                </asp:DropDownList>
                                                <asp:RequiredFieldValidator ID="rfvChargedDateInd" runat="server" ControlToValidate="ddlChargedDateInd"
                                                    ErrorMessage="Please select Surcharge Date Indicator." InitialValue="0"
                                                    ValidationGroup="SaveSurTaxes">*</asp:RequiredFieldValidator>
                                            </td>
                                            <td>
                                            </td>
                                        </tr>
                                    </InsertItemTemplate>
                                    <EditItemTemplate>
                                        <tr id="tr2" runat="server" class="AlternatingItemTemplate">
                                            <td align="center">
                                                <asp:LinkButton ID="lblSurchargesUpdate" CommandName="Update" Text="Update" runat="server"
                                                    Visible="true" ValidationGroup="EditSurTaxes" />
                                                <asp:LinkButton ID="lblSurchargeCancel" CommandName="Cancel" runat="server" Text="Cancel"
                                                    Visible="true" />
                                            </td>
                                            <td>
                                                <asp:Label ID="lblLOB" Width="120px" Visible="false" runat="server" Text='<%# Bind("LN_OF_BSN_ID") %>'></asp:Label>
                                                <asp:DropDownList ID="ddlLOB" runat="server" DataSourceID="ObjectCovTypeSur" ValidationGroup="EditSurTaxes"
                                                    Width="70px" DataTextField="LookUpName" DataValueField="LookUpID" TabIndex="1">
                                                </asp:DropDownList>
                                                <asp:RequiredFieldValidator ID="rfvCoverageType" runat="server" ControlToValidate="ddlLOB"
                                                    InitialValue="0" ErrorMessage="Please select at least one Line of Business." ValidationGroup="EditSurTaxes">*</asp:RequiredFieldValidator>
                                            </td>
                                            <td align="center">
                                                <asp:Label ID="lblSurSetupId" Width="80px" Visible="false" runat="server" Text='<%# Bind("SURCHARGE_ASSESS_SETUP_ID") %>'></asp:Label>
                                                <asp:DropDownList ID="ddlState" runat="server" DataTextField="Attribute1" DataValueField="LookUpID"
                                                    AutoPostBack="true" OnSelectedIndexChanged="ddlState_SelectedIndexChangedSur" Width="80px" ValidationGroup="EditSurTaxes" DataSourceID="SurodsStates"
                                                     TabIndex="2">
                                                </asp:DropDownList>
                                                <asp:RequiredFieldValidator ID="rfvState" runat="server" ControlToValidate="ddlState"
                                                    ErrorMessage="Please select at least one State." ValidationGroup="EditSurTaxes">*</asp:RequiredFieldValidator>
                                            </td>
                                            <td align="center">
                                                <asp:Label ID="lblSurCode" Width="150px" Visible="false" runat="server" Text='<%# Bind("SURCHARGE_CODE") %>'></asp:Label>
                                                <asp:Label ID="lblSurCodeId" Width="150px" Visible="false" runat="server" Text='<%# Bind("LOOKUPID") %>'></asp:Label>
                                                <asp:DropDownList ID="ddlSurCode" runat="server" DataTextField="LookUpTypeName"
                                                    DataValueField="LookUpID" OnSelectedIndexChanged="ddlCode_SelectedIndexChanged" AutoPostBack="true" Width="75px" ValidationGroup="EditSurTaxes"
                                                    TabIndex="3">
                                                </asp:DropDownList>
                                                <asp:RequiredFieldValidator ID="rfvSurCode" runat="server" ControlToValidate="ddlSurCode"
                                                    ErrorMessage="Please select at least one Surcharge code." InitialValue="0" ValidationGroup="EditSurTaxes">*</asp:RequiredFieldValidator>
                                            </td>
                                            <td align="center">
                                            <asp:Label ID="lblSurDescription" runat="server" Text='<%# Bind("SURCHARGE_DESCRIPTION") %>' />
                                            <asp:HiddenField ID="hidSurDescriptionIDEdit" runat="server"  Value='<%# Bind("SURCHARGE_TYPE_ID") %>' />
                                            </td>
                                            <td align="center" nowrap>
                                                <asp:TextBox ID="txtSurEffDate" MaxLength="10" runat="server" SkinID="largeTextbox" Width="90px"
                                                    TabIndex="5" ValidationGroup="EditSurTaxes"></asp:TextBox>
                                                <asp:ImageButton ID="imgEffDate" runat="server" ImageUrl="~/images/Calendar_scheduleHS.png"
                                                    CausesValidation="False" />
                                                <ajaxToolkit:CalendarExtender ID="CalendarExtender1" runat="server" Enabled="True"
                                                    TargetControlID="txtSurEffDate" Format="MM/dd/yyyy" PopupPosition="BottomRight"
                                                    PopupButtonID="imgEffDate">
                                                </ajaxToolkit:CalendarExtender>
                                                <ajaxToolkit:MaskedEditExtender ID="mskEffDate" runat="server" AcceptNegative="Left"
                                                    DisplayMoney="Left" ErrorTooltipEnabled="True" Mask="99/99/9999" MaskType="Date"
                                                    MessageValidatorTip="true" OnFocusCssClass="MaskedEditFocus" OnInvalidCssClass="MaskedEditError"
                                                    TargetControlID="txtSurEffDate" />
                                                    <asp:RegularExpressionValidator ID="regSurEffDate" runat="server" ControlToValidate="txtSurEffDate"
                                                    ValidationExpression="(^(?:(?:(?:0?[13578]|1[02])(\/|-)31)|(?:(?:0?[1,3-9]|1[0-2])(\/|-)(?:29|30)))(\/|-)(?:(1[8-9]|[2-8]\d)\d\d)$|^(?:(?:0?[1-9]|1[0-2])(\/|-)(?:0?[1-9]|1\d|2[0-8]))(\/|-)(?:(1[8-9]|[2-8]\d)\d\d)$|^(0?2(\/|-)29)(\/|-)(?:(?:0[48]00|[13579][26]00|[2468][048]00)|(?:\d\d)?(?:0[48]|[2468][048]|[13579][26]))$)"
                                                    ErrorMessage="Please enter valid Surcharge Effective Date." Text="*" ValidationGroup="EditSurTaxes"></asp:RegularExpressionValidator>
                                                <asp:RequiredFieldValidator ID="rfvSurEffDate" runat="server" ControlToValidate="txtSurEffDate"
                                                    ErrorMessage="Please enter Surcharge Effective Date." ValidationGroup="EditSurTaxes">*</asp:RequiredFieldValidator>
                                                
                                            </td>
                                            <td align="center">
                                                <asp:TextBox ID="txtSurRate" runat="server" Width="88px" TabIndex="6"></asp:TextBox>
                                                <ajaxToolkit:MaskedEditExtender ID="MaskedEditExtender2" runat="server" Mask="9999.99999999"
                                                    MaskType="Number" InputDirection="RightToLeft" TargetControlID="txtSurRate" />
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtSurRate"
                                                    ErrorMessage="Please enter Surcharge Factor." ValidationGroup="EditSurTaxes">*</asp:RequiredFieldValidator>
                                            </td>
                                            
                                            <td align="center">
                                                <asp:Label ID="lblChargedDateInd" runat="server" Text='<%# Bind("SURCHARGE_FACTOR_ID") %>'
                                                    Visible="false" />
                                                <asp:DropDownList ID="ddlChargedDateInd" runat="server" Width="72px" ValidationGroup="SaveSurTaxes"
                                                    TabIndex="7" DataSourceID="odsFactorDescriptionEdit" DataTextField="LookUpName" DataValueField="LookUpID">
                                                </asp:DropDownList>
                                                <asp:RequiredFieldValidator ID="rfvChargedDateInd" runat="server" ControlToValidate="ddlChargedDateInd"
                                                    ErrorMessage="Please select Surcharge Date Indicator." ValidationGroup="EditSurTaxes">*</asp:RequiredFieldValidator>
                                            </td>
                                        </tr>
                                    </EditItemTemplate>
                                    <EmptyDataTemplate>
                                        <table id="lstSurchargesSetup1" class="panelContents" runat="server" width="100%">
                                            <tr class="LayoutTemplate">
                                                <th>
                                                </th>
                                                <th>
                                                    LOB
                                                </th>
                                                <th>
                                                    State
                                                </th>
                                                <th>
                                                    Surcharge/Assessment<br /> Code
                                                </th>
                                                 <th>
                                                    Surcharge/Assessment<br /> Description
                                                </th>
                                                <th>
                                                    Surcharge Eff. Date
                                                </th>
                                                <th>
                                                    Surcharge/Assessment<br /> Factor
                                                </th>
                                                <th>
                                                    Surcharge <br /> Date<br /> Indicator
                                                </th>
                                            </tr>
                                            <tr id="Tr6" runat="server">
                                            </tr>
                                        </table>
                                        <table width="910px">
                                            <tr id="Tr7" runat="server">
                                                <td align="center">
                                                    <asp:Label ID="lblEmptyMessage" Text="---Records are not available.---"
                                                        Font-Bold="true" Width="600px" runat="server" Style="text-align: center" />
                                                </td>
                                            </tr>
                                        </table>
                                    </EmptyDataTemplate>
                                </asp:AISListView>
                            </asp:Panel>
                        </ContentTemplate>
                    </ajaxToolkit:TabPanel>





                    <ajaxToolkit:TabPanel runat="server" ID="tabNewLookUp">
                        <HeaderTemplate>
                            Deductible Taxes
                        </HeaderTemplate>
                        <ContentTemplate>
                            <br />
                            <br />
                            <asp:ObjectDataSource ID="StatesDataSource" runat="server" SelectMethod="GetStates"
                                TypeName="ZurichNA.AIS.Business.Logic.DeductibleTaxesBS"></asp:ObjectDataSource>
                            <asp:ObjectDataSource ID="odsStates" runat="server" SelectMethod="GetStatesForEdit"
                                TypeName="ZurichNA.AIS.Business.Logic.DeductibleTaxesBS">
                                <SelectParameters>
                                    <asp:Parameter Name="iLkupId" Type="Int32" />
                                </SelectParameters>
                            </asp:ObjectDataSource>
                            <asp:ObjectDataSource ID="CompDescriptionDS" runat="server" SelectMethod="GetComponentDescription"
                                TypeName="ZurichNA.AIS.Business.Logic.DeductibleTaxesBS"></asp:ObjectDataSource>
                                <asp:ObjectDataSource ID="odsCompDescriptionEdit" runat="server" SelectMethod="GetComponentDescriptionForEdit"
                                TypeName="ZurichNA.AIS.Business.Logic.DeductibleTaxesBS">
                                <SelectParameters>
                                    <asp:Parameter Name="iComponentId" Type="Int32" />
                                </SelectParameters>
                                </asp:ObjectDataSource>
                            <asp:ObjectDataSource ID="ObjectCovType" runat="server" SelectMethod="GetLookUpLOBActiveData"
                                TypeName="ZurichNA.AIS.Business.Logic.BLAccess">
                                <SelectParameters>
                                    <asp:Parameter DefaultValue="LOB" Name="lookUpTypeName" Type="String" />
                                </SelectParameters>
                            </asp:ObjectDataSource>
                            <asp:Label ID="lblDeductibleTaxes" runat="server" CssClass="h2" Text="Deductible Taxes"></asp:Label>
                            <asp:Panel ID="pnlDeductibleTaxes" runat="server" Height="200px" Width="910px" ScrollBars="Auto"
                                Visible="true">
                                <asp:AISListView ID="lstDeductibleTaxes" runat="server" OnItemDataBound="lstDeductibleTaxes_DataBoundList"
                                    InsertItemPosition="FirstItem" OnItemEditing="lstDeductibleTaxes_ItemEdit" OnItemCanceling="lstDeductibleTaxes_ItemCancel"
                                    OnItemCommand="lstDeductibleTaxes_ItemCommand" OnItemUpdating="lstDeductibleTaxes_ItemUpdate"
                                    OnItemInserting="lstDeductibleTaxes_ItemInserting">
                                    <LayoutTemplate>
                                        <table id="Table2" class="panelContents" runat="server" width="98%">
                                            <tr class="LayoutTemplate">
                                                <th>
                                                </th>
                                                <th>
                                                    State
                                                </th>
                                                <th>
                                                    Description
                                                </th>
                                                <th>
                                                    LOB
                                                </th>
                                                <th>
                                                    Tax Rate
                                                </th>
                                                <th>
                                                    Policy Eff. Date
                                                </th>
                                                <th>
                                                    Comp. <br />Applies to
                                                </th>
                                                <th>
                                                    Tax End Date
                                                </th>
                                                <th>
                                                    Main/Sub
                                                </th>
                                                <th>
                                                    Disable
                                                </th>
                                            </tr>
                                            <tr id="ItemPlaceHolder" runat="server">
                                            </tr>
                                        </table>
                                    </LayoutTemplate>
                                    <ItemTemplate>
                                        <tr id="Tr4" runat="server" class="ItemTemplate">
                                            <td>
                                                <asp:LinkButton ID="lnkDeductibleTaxes" CommandName="Edit" Text="Edit" runat="server"
                                                    Visible="true" Enabled='<%# Eval("ACTV_IND")%>' />
                                            </td>
                                            <td>
                                                <asp:Label ID="lblState" runat="server" Text='<%# Bind("STATEDESCRIPTION") %>'></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="lblDescription" runat="server" Text='<%# Bind("DTAXDESCRIPTION") %>' />
                                            </td>
                                            <td>
                                                <asp:Label ID="lblLOB" runat="server" Text='<%# Bind("LN_OF_BSN_TXT") %>'></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="lblTaxRate" runat="server" Text='<%# Bind("TAX_RATE") %>' />
                                            </td>
                                            <td>
                                                <asp:Label ID="lblPolEffDate" runat="server" Text='<%# Eval("POL_EFF_DT", "{0:d}")  %>'></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="lblComponentAppliesTo" runat="server" Text='<%# Bind("DTAXCOMDESCRIPTION") %>' />
                                            </td>
                                            <td>
                                                <asp:Label ID="lblTaxEndDate" runat="server" Text='<%# Eval("TAX_END_DT", "{0:d}")%>' />
                                            </td>
                                            <td>
                                                <asp:Label ID="lblMainSub" runat="server" Text='<%# Eval("MAIN_NBR_TXT")+"/"+Eval("SUB_NBR_TXT")%>' />
                                            </td>
                                            <td>
                                                <asp:HiddenField ID="hidActive" runat="server" Value='<%# Bind("ACTV_IND")%>' />
                                                <asp:ImageButton ID="imgDisable" CommandArgument='<%# Bind("DED_TAXES_SETUP_ID") %>'
                                                    runat="server" ToolTip='<%#Eval("ACTV_IND").ToString()=="True"?"Click here to Disable":"Click here to Enable" %>'
                                                    ImageUrl='<%# Eval("ACTV_IND").ToString()=="True"?"~/images/disabled.GIF":"~/images/enabled.GIF" %>'>
                                                </asp:ImageButton>
                                            </td>
                                        </tr>
                                    </ItemTemplate>
                                    <AlternatingItemTemplate>
                                        <tr id="Tr5" runat="server" class="AlternatingItemTemplate">
                                            <td>
                                                <asp:LinkButton ID="lnkEdit" CommandName="Edit" Enabled='<%# Eval("ACTV_IND")%>'
                                                    Text="Edit" runat="server" Visible="true" />
                                            </td>
                                            <td>
                                                <asp:Label ID="lblState" runat="server" Text='<%# Bind("STATEDESCRIPTION") %>'></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="lblDescription" runat="server" Text='<%# Bind("DTAXDESCRIPTION") %>' />
                                            </td>
                                            <td>
                                                <asp:Label ID="lblLOB" runat="server" Text='<%# Bind("LN_OF_BSN_TXT") %>'></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="lblTaxRate" runat="server" Text='<%# Bind("TAX_RATE") %>' />
                                            </td>
                                            <td>
                                                <asp:Label ID="lblPolEffDate" runat="server" Text='<%# Eval("POL_EFF_DT", "{0:d}")  %>'></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="lblComponentAppliesTo" runat="server" Text='<%# Bind("DTAXCOMDESCRIPTION") %>' />
                                            </td>
                                            <td>
                                                <asp:Label ID="lblTaxEndDate" runat="server" Text='<%# Eval("TAX_END_DT", "{0:d}") %>' />
                                            </td>
                                            <td>
                                                <asp:Label ID="lblMainSub" runat="server" Text='<%# Eval("MAIN_NBR_TXT")+"/"+Eval("SUB_NBR_TXT")%>' />
                                            </td>
                                            <td>
                                                <asp:HiddenField ID="hidActive" runat="server" Value='<%# Bind("ACTV_IND")%>' />
                                                <asp:ImageButton ID="imgDisable" CommandArgument='<%# Bind("DED_TAXES_SETUP_ID") %>'
                                                    runat="server" ImageUrl='<%# Eval("ACTV_IND").ToString()=="True"?"~/images/disabled.GIF":"~/images/enabled.GIF" %>'>
                                                </asp:ImageButton>
                                            </td>
                                        </tr>
                                    </AlternatingItemTemplate>
                                    <InsertItemTemplate>
                                        <tr id="tr2" runat="server" class="AlternatingItemTemplate">
                                            <td align="center">
                                                <asp:LinkButton ID="lnkDTaxesInsert" CommandName="Save" Text="Save" runat="server"
                                                    Visible="true" ValidationGroup="SaveDTaxes" />
                                            </td>
                                            <td align="center">
                                                <asp:DropDownList ID="ddlState" runat="server" DataTextField="Attribute1" DataValueField="LookUpID"
                                                    AutoPostBack="true" Width="92px" ValidationGroup="SaveDTaxes" DataSourceID="StatesDataSource"
                                                    OnSelectedIndexChanged="ddlState_SelectedIndexChanged" TabIndex="1">
                                                </asp:DropDownList>
                                                <asp:RequiredFieldValidator ID="rfvState" runat="server" ControlToValidate="ddlState"
                                                    InitialValue="0" ErrorMessage="Please select at least one State" ValidationGroup="SaveDTaxes">*</asp:RequiredFieldValidator>
                                            </td>
                                            <td align="center">
                                                <asp:DropDownList ID="ddlDescription" runat="server" DataTextField="LookUpTypeName"
                                                    DataValueField="LookUpID" AutoPostBack="true" Width="125px" ValidationGroup="SaveDTaxes"
                                                    OnSelectedIndexChanged="ddlDescription_SelectedIndexChanged" TabIndex="2">
                                                </asp:DropDownList>
                                                <asp:RequiredFieldValidator ID="rfvDescription" runat="server" ControlToValidate="ddlDescription"
                                                    ErrorMessage="Please select at least one Description" InitialValue="0" ValidationGroup="SaveDTaxes">*</asp:RequiredFieldValidator>
                                            </td>
                                            <td>
                                                <asp:Label ID="lblLOB" Width="120px" Visible="false" runat="server" Text='<%# Bind("LN_OF_BSN_ID") %>'></asp:Label>
                                                <asp:DropDownList ID="ddlLOB" runat="server" DataSourceID="ObjectCovType" ValidationGroup="SaveDTaxes"
                                                    Width="70px" DataTextField="LookUpName" DataValueField="LookUpID">
                                                </asp:DropDownList>
                                                <asp:RequiredFieldValidator ID="rfvCoverageType" runat="server" ControlToValidate="ddlLOB"
                                                    InitialValue="0" ErrorMessage="Please select at least one Coverage Type" ValidationGroup="SaveDTaxes">*</asp:RequiredFieldValidator>
                                            </td>
                                            <td align="center">
                                                <asp:TextBox ID="txtTaxRate" runat="server" Width="73px" MaxLength="6" TabIndex="3"></asp:TextBox>
                                                <ajaxToolkit:MaskedEditExtender ID="maskTMFactor" runat="server" Mask="9999.999999"
                                                    MaskType="Number" TargetControlID="txtTaxRate" InputDirection="RightToLeft"
                                                    AutoComplete="false" />
                                                <asp:RequiredFieldValidator ID="rfvTaxRate" runat="server" ControlToValidate="txtTaxRate"
                                                    ErrorMessage="Please enter Tax Rate" ValidationGroup="SaveDTaxes">*</asp:RequiredFieldValidator>
                                            </td>
                                            <td align="center">
                                                <asp:TextBox ID="txtPolEffDate" MaxLength="10" runat="server" SkinID="largeTextbox" Width="75px"
                                                    TabIndex="4" ValidationGroup="SaveDTaxes"></asp:TextBox>
                                                <asp:ImageButton ID="imgEffDate" runat="server" ImageUrl="~/images/Calendar_scheduleHS.png"
                                                    CausesValidation="False" />
                                                <ajaxToolkit:CalendarExtender ID="CalendarExtender1" runat="server" Enabled="True"
                                                    TargetControlID="txtPolEffDate" Format="MM/dd/yyyy" PopupPosition="BottomRight"
                                                    PopupButtonID="imgEffDate">
                                                </ajaxToolkit:CalendarExtender>
                                                <ajaxToolkit:MaskedEditExtender ID="mskEffDate" runat="server" AcceptNegative="Left"
                                                    DisplayMoney="Left" ErrorTooltipEnabled="True" Mask="99/99/9999" MaskType="Date"
                                                    MessageValidatorTip="true" OnFocusCssClass="MaskedEditFocus" OnInvalidCssClass="MaskedEditError"
                                                    TargetControlID="txtPolEffDate" />
                                                <asp:RegularExpressionValidator ID="regPolEffDate" runat="server" ControlToValidate="txtPolEffDate"
                                                    ValidationExpression="(^(?:(?:(?:0?[13578]|1[02])(\/|-)31)|(?:(?:0?[1,3-9]|1[0-2])(\/|-)(?:29|30)))(\/|-)(?:(1[8-9]|[2-8]\d)\d\d)$|^(?:(?:0?[1-9]|1[0-2])(\/|-)(?:0?[1-9]|1\d|2[0-8]))(\/|-)(?:(1[8-9]|[2-8]\d)\d\d)$|^(0?2(\/|-)29)(\/|-)(?:(?:0[48]00|[13579][26]00|[2468][048]00)|(?:\d\d)?(?:0[48]|[2468][048]|[13579][26]))$)"
                                                    ErrorMessage="Please enter valid Policy Effective Date." Text="*" ValidationGroup="SaveDTaxes"></asp:RegularExpressionValidator>
                                                <asp:RequiredFieldValidator ID="rfvPolEffDate" runat="server" ControlToValidate="txtPolEffDate"
                                                    ErrorMessage="Please enter Policy Effective Date" ValidationGroup="SaveDTaxes">*</asp:RequiredFieldValidator>
                                                
                                            </td>
                                            <td align="center">
                                                <asp:DropDownList ID="ddlComponentAppliesTo" runat="server" Width="60px" ValidationGroup="SaveDTaxes"
                                                    TabIndex="5" DataSourceID="CompDescriptionDS" DataTextField="LookUpName" DataValueField="LookUpID">
                                                </asp:DropDownList>
                                                <asp:RequiredFieldValidator ID="rfvComponentAppliesTo" runat="server" ControlToValidate="ddlComponentAppliesTo"
                                                    ErrorMessage="Please select at least one Component Description" InitialValue="0"
                                                    ValidationGroup="SaveDTaxes">*</asp:RequiredFieldValidator>
                                            </td>
                                            <td align="center">
                                                <asp:TextBox ID="txtTaxEndDate" MaxLength="10" runat="server" SkinID="largeTextbox" Width="75px"
                                                    TabIndex="6" ValidationGroup="SaveDTaxes"></asp:TextBox>
                                                <asp:ImageButton ID="imgTaxEndDate" runat="server" ImageUrl="~/images/Calendar_scheduleHS.png"
                                                    CausesValidation="False" />
                                                <ajaxToolkit:CalendarExtender ID="CalendarExtender2" runat="server" Enabled="True"
                                                    TargetControlID="txtTaxEndDate" Format="MM/dd/yyyy" PopupPosition="BottomRight"
                                                    PopupButtonID="imgTaxEndDate">
                                                </ajaxToolkit:CalendarExtender>
                                                <ajaxToolkit:MaskedEditExtender ID="MaskedEditExtender1" runat="server" AcceptNegative="Left"
                                                    DisplayMoney="Left" ErrorTooltipEnabled="True" Mask="99/99/9999" MaskType="Date"
                                                    MessageValidatorTip="true" OnFocusCssClass="MaskedEditFocus" OnInvalidCssClass="MaskedEditError"
                                                    TargetControlID="txtTaxEndDate" />
                                                 <asp:RegularExpressionValidator ID="regTaxEndDate" runat="server" ControlToValidate="txtTaxEndDate"
                                                    ValidationExpression="(^(?:(?:(?:0?[13578]|1[02])(\/|-)31)|(?:(?:0?[1,3-9]|1[0-2])(\/|-)(?:29|30)))(\/|-)(?:(1[8-9]|[2-8]\d)\d\d)$|^(?:(?:0?[1-9]|1[0-2])(\/|-)(?:0?[1-9]|1\d|2[0-8]))(\/|-)(?:(1[8-9]|[2-8]\d)\d\d)$|^(0?2(\/|-)29)(\/|-)(?:(?:0[48]00|[13579][26]00|[2468][048]00)|(?:\d\d)?(?:0[48]|[2468][048]|[13579][26]))$)"
                                                    ErrorMessage="Please enter valid Tax End Date." Text="*" ValidationGroup="SaveDTaxes"></asp:RegularExpressionValidator>
                                                <asp:CompareValidator ID="CompareTaxEndDate" runat="server" ControlToValidate="txtTaxEndDate"
                                                    Display="Dynamic" ErrorMessage="Tax End Date must not be less than Policy Effective Date, Please Enter Valid Tax End Date"
                                                    Text="*" Operator="GreaterThan" ControlToCompare="txtPolEffDate" ValidationGroup="SaveDTaxes"
                                                    Type="Date" />
                                            </td>
                                            <td>
                                                <asp:Label ID="lblMainSub" runat="server" Text='<%# Eval("MAIN_NBR_TXT")+"/"+Eval("SUB_NBR_TXT")%>' />
                                            </td>
                                            <td>
                                            </td>
                                        </tr>
                                    </InsertItemTemplate>
                                    <EditItemTemplate>
                                        <tr id="tr2" runat="server" class="AlternatingItemTemplate">
                                            <td align="center">
                                                <asp:LinkButton ID="lblDTaxesUpdate" CommandName="Update" Text="Update" runat="server"
                                                    Visible="true" ValidationGroup="EditDTaxes" />
                                                <asp:LinkButton ID="lblContactCancel" CommandName="Cancel" runat="server" Text="Cancel"
                                                    Visible="true" />
                                            </td>
                                            <td align="center">
                                                <asp:Label ID="lbldTaxesSetupId" Width="165px" Visible="false" runat="server" Text='<%# Bind("DED_TAXES_SETUP_ID") %>'></asp:Label>
                                                <asp:DropDownList ID="ddlState" runat="server" DataTextField="Attribute1" DataValueField="LookUpID"
                                                    AutoPostBack="true" Width="92px" ValidationGroup="EditDTaxes" DataSourceID="odsStates"
                                                    OnSelectedIndexChanged="ddlState_SelectedIndexChanged" TabIndex="1">
                                                </asp:DropDownList>
                                                <asp:RequiredFieldValidator ID="rfvState" runat="server" ControlToValidate="ddlState"
                                                    ErrorMessage="Please select State" ValidationGroup="EditDTaxes">*</asp:RequiredFieldValidator>
                                            </td>
                                            <td align="center">
                                                <asp:Label ID="lblDescription" Width="165px" Visible="false" runat="server" Text='<%# Bind("dTaxDescription") %>'></asp:Label>
                                                <asp:Label ID="lblDescriptionId" Width="165px" Visible="false" runat="server" Text='<%# Bind("LOOKUPID") %>'></asp:Label>
                                                <asp:DropDownList ID="ddlDescription" runat="server" DataTextField="LookUpTypeName"
                                                    DataValueField="LookUpID" AutoPostBack="true" Width="125px" ValidationGroup="EditDTaxes"
                                                    OnSelectedIndexChanged="ddlDescription_SelectedIndexChanged" TabIndex="2">
                                                </asp:DropDownList>
                                                <asp:RequiredFieldValidator ID="rfvDescription" runat="server" ControlToValidate="ddlDescription"
                                                    ErrorMessage="Please select at least one Description" InitialValue="0" ValidationGroup="EditDTaxes">*</asp:RequiredFieldValidator>
                                            </td>
                                            <td>
                                                <asp:Label ID="lblLOB" Width="165px" Visible="false" runat="server" Text='<%# Bind("LN_OF_BSN_ID") %>'></asp:Label>
                                                <asp:DropDownList ID="ddlLOB" runat="server" DataSourceID="ObjectCovType" ValidationGroup="SaveDTaxes"
                                                    Width="70px" DataTextField="LookUpName" DataValueField="LookUpID">
                                                </asp:DropDownList>
                                                <asp:RequiredFieldValidator ID="rfvCoverageType" runat="server" ControlToValidate="ddlLOB"
                                                    InitialValue="0" ErrorMessage="Please select at least one Line of Business" ValidationGroup="EditDTaxes">*</asp:RequiredFieldValidator>
                                            </td>
                                            <td align="center">
                                                <asp:TextBox ID="txtTaxRate" runat="server" Width="73px" MaxLength="6" TabIndex="3"></asp:TextBox>
                                                <ajaxToolkit:MaskedEditExtender ID="MaskedEditExtender2" runat="server" Mask="9999.999999"
                                                    MaskType="Number" TargetControlID="txtTaxRate" InputDirection="RightToLeft"
                                                    AutoComplete="false" />
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtTaxRate"
                                                    ErrorMessage="Please enter Tax Rate" ValidationGroup="EditDTaxes">*</asp:RequiredFieldValidator>
                                            </td>
                                            <td align="center">
                                                <asp:TextBox ID="txtPolEffDate" MaxLength="10" runat="server" SkinID="largeTextbox" Width="75px"
                                                    TabIndex="4" ValidationGroup="EditDTaxes"></asp:TextBox>
                                                <asp:ImageButton ID="imgEffDate" runat="server" ImageUrl="~/images/Calendar_scheduleHS.png"
                                                    CausesValidation="False" />
                                                <ajaxToolkit:CalendarExtender ID="CalendarExtender1" runat="server" Enabled="True"
                                                    TargetControlID="txtPolEffDate" Format="MM/dd/yyyy" PopupPosition="BottomRight"
                                                    PopupButtonID="imgEffDate">
                                                </ajaxToolkit:CalendarExtender>
                                                <ajaxToolkit:MaskedEditExtender ID="mskEffDate" runat="server" AcceptNegative="Left"
                                                    DisplayMoney="Left" ErrorTooltipEnabled="True" Mask="99/99/9999" MaskType="Date"
                                                    MessageValidatorTip="true" OnFocusCssClass="MaskedEditFocus" OnInvalidCssClass="MaskedEditError"
                                                    TargetControlID="txtPolEffDate" />
                                                    
                                                    <asp:RegularExpressionValidator ID="regPolEffDate" runat="server" ControlToValidate="txtPolEffDate"
                                                    ValidationExpression="(^(?:(?:(?:0?[13578]|1[02])(\/|-)31)|(?:(?:0?[1,3-9]|1[0-2])(\/|-)(?:29|30)))(\/|-)(?:(1[8-9]|[2-8]\d)\d\d)$|^(?:(?:0?[1-9]|1[0-2])(\/|-)(?:0?[1-9]|1\d|2[0-8]))(\/|-)(?:(1[8-9]|[2-8]\d)\d\d)$|^(0?2(\/|-)29)(\/|-)(?:(?:0[48]00|[13579][26]00|[2468][048]00)|(?:\d\d)?(?:0[48]|[2468][048]|[13579][26]))$)"
                                                    ErrorMessage="Please enter valid Policy Effective Date." Text="*" ValidationGroup="EditDTaxes"></asp:RegularExpressionValidator>
                                                <asp:RequiredFieldValidator ID="rfvPolEffDate" runat="server" ControlToValidate="txtPolEffDate"
                                                    ErrorMessage="Please enter Policy Effective Date" ValidationGroup="EditDTaxes">*</asp:RequiredFieldValidator>
                                                
                                            </td>
                                            <td align="center">
                                                <asp:Label ID="lblComponentAppliesTo" runat="server" Text='<%# Bind("DED_TAX_COMPONENT_ID") %>'
                                                    Visible="false" />
                                                <asp:DropDownList ID="ddlComponentAppliesTo" runat="server" Width="60px" ValidationGroup="SaveDTaxes"
                                                    TabIndex="5" DataSourceID="odsCompDescriptionEdit" DataTextField="LookUpName" DataValueField="LookUpID">
                                                </asp:DropDownList>
                                                <asp:RequiredFieldValidator ID="rfvComponentAppliesTo" runat="server" ControlToValidate="ddlComponentAppliesTo"
                                                    ErrorMessage="Please select Component Description" ValidationGroup="EditDTaxes">*</asp:RequiredFieldValidator>
                                            </td>
                                            <td align="center">
                                                <asp:TextBox ID="txtTaxEndDate" MaxLength="10" runat="server" SkinID="largeTextbox" Width="75px"
                                                    TabIndex="6" ValidationGroup="EditDTaxes"></asp:TextBox>
                                                <asp:ImageButton ID="imgTaxEndDate" runat="server" ImageUrl="~/images/Calendar_scheduleHS.png"
                                                    CausesValidation="False" />
                                                <ajaxToolkit:CalendarExtender ID="CalendarExtender2" runat="server" Enabled="True"
                                                    TargetControlID="txtTaxEndDate" Format="MM/dd/yyyy" PopupPosition="BottomRight"
                                                    PopupButtonID="imgTaxEndDate">
                                                </ajaxToolkit:CalendarExtender>
                                                <ajaxToolkit:MaskedEditExtender ID="MaskedEditExtender1" runat="server" AcceptNegative="Left"
                                                    DisplayMoney="Left" ErrorTooltipEnabled="True" Mask="99/99/9999" MaskType="Date"
                                                    MessageValidatorTip="true" OnFocusCssClass="MaskedEditFocus" OnInvalidCssClass="MaskedEditError"
                                                    TargetControlID="txtTaxEndDate" />
                                                <asp:RegularExpressionValidator ID="regTaxEndDate" runat="server" ControlToValidate="txtTaxEndDate"
                                                    ValidationExpression="(^(?:(?:(?:0?[13578]|1[02])(\/|-)31)|(?:(?:0?[1,3-9]|1[0-2])(\/|-)(?:29|30)))(\/|-)(?:(1[8-9]|[2-8]\d)\d\d)$|^(?:(?:0?[1-9]|1[0-2])(\/|-)(?:0?[1-9]|1\d|2[0-8]))(\/|-)(?:(1[8-9]|[2-8]\d)\d\d)$|^(0?2(\/|-)29)(\/|-)(?:(?:0[48]00|[13579][26]00|[2468][048]00)|(?:\d\d)?(?:0[48]|[2468][048]|[13579][26]))$)"
                                                    ErrorMessage="Please enter valid Tax End Date." Text="*" ValidationGroup="EditDTaxes"></asp:RegularExpressionValidator>
                                                <asp:CompareValidator ID="CompareTaxEndDate" runat="server" ControlToValidate="txtTaxEndDate"
                                                    Display="Dynamic" ErrorMessage="Tax End Date must not be less than Policy Effective Date, Please Enter Valid Tax End Date"
                                                    Text="*" Operator="GreaterThan" ControlToCompare="txtPolEffDate" ValidationGroup="EditDTaxes"
                                                    Type="Date" />
                                            </td>
                                            <td>
                                                <asp:Label ID="lblMainSub" runat="server" Text='<%# Eval("MAIN_NBR_TXT")+"/"+Eval("SUB_NBR_TXT")%>' />
                                            </td>
                                        </tr>
                                    </EditItemTemplate>
                                    <EmptyDataTemplate>
                                        <table id="lstDeductibleTaxesTable" class="panelContents" runat="server" width="100%">
                                            <tr class="LayoutTemplate">
                                                <th>
                                                </th>
                                                <th>
                                                    State
                                                </th>
                                                <th>
                                                    Description
                                                </th>
                                                <th>
                                                    LOB
                                                </th>
                                                <th>
                                                    Tax Rate
                                                </th>
                                                <th>
                                                    Policy Effective Date
                                                </th>
                                                <th>
                                                    Component Applies to
                                                </th>
                                                <th>
                                                    Tax End Date
                                                </th>
                                                <th>
                                                    Main/Sub
                                                </th>
                                            </tr>
                                            <tr id="Tr6" runat="server">
                                            </tr>
                                        </table>
                                        <table width="910px">
                                            <tr id="Tr7" runat="server">
                                                <td align="center">
                                                    <asp:Label ID="lblEmptyMessage" Text="---Records are not allowed to Add as status is not in CALC ---"
                                                        Font-Bold="true" Width="600px" runat="server" Style="text-align: center" />
                                                </td>
                                            </tr>
                                        </table>
                                    </EmptyDataTemplate>
                                </asp:AISListView>
                            </asp:Panel>
                        </ContentTemplate>
                    </ajaxToolkit:TabPanel>
                </ajaxToolkit:TabContainer>
            </td>
        </tr>
    </table>
</asp:Content>
