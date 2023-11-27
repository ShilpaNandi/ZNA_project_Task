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
            $get('btnremove').disabled = false;
            var tabPanel = $get('<%=pnlDetails.ClientID%>');
            if (tabPanel.disabled == false) {
                var hidText = $get('<%=hidText.ClientID%>');
                var txtFormula = $get('<%=txtFormulaTwo.ClientID%>');
                txtFormula.value = txtFormula.value + Component;
                if (hidText.value == "") {
                    hidText.value = hidText.value + Component;
                }
                else {
                    hidText.value = hidText.value + "@" + Component;
                }
            }
        }
        function removeall() {
            if ($get('<%=hidSecurity.ClientID%>').value == '1') {
                return false;
            }
            var tabPanel = $get('<%=pnlDetails.ClientID%>');
            if (tabPanel.disabled == false) {
                document.getElementById('ctl00_hdnControlDirty').value = '1';
                $get('<%=txtFormulaTwo.ClientID%>').value = "";
                $get('<%=hidText.ClientID%>').value = "";
            }
        }
        function removeone() {
            if ($get('<%=hidSecurity.ClientID%>').value == '1') {
                return false;
            }
            var tabPanel = $get('<%=pnlDetails.ClientID%>');
            if (tabPanel.disabled == false) {
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
            }
        }
    </script>

    <table>
        <tr>
            <td>
                <asp:ValidationSummary ID="valSumaSave" CssClass="ValidationSummary" ValidationGroup="Save"
                    runat="server"></asp:ValidationSummary>
                <asp:ValidationSummary ID="valSumaSaveKYOR" CssClass="ValidationSummary" ValidationGroup="SaveKYOR"
                    runat="server"></asp:ValidationSummary>
                <ajaxToolkit:TabContainer ID="TabContainer1" runat="server" CssClass="CustomTabs"
                    ActiveTabIndex="0">
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
                                                    type="button" value="<<" name="btnremoveall" />
                                                <asp:HiddenField ID="hidText" runat="server"></asp:HiddenField>
                                                <br />
                                                <br />
                                                <input onclick="removeone()" style="color: White; background-color: #9494C8; width: 25px"
                                                    type="button" value="<" name="btnremove" />
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
                    <ajaxToolkit:TabPanel runat="server" ID="tabKY">
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
                </ajaxToolkit:TabContainer>
            </td>
        </tr>
    </table>
</asp:Content>
