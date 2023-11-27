<%@ Page Language="C#" MasterPageFile="~/Retro.master" AutoEventWireup="true" Inherits="AdjParameters_RetroInfo"
    CodeBehind="RetroInfo.aspx.cs" %>

<%@ Register Src="~/App_Shared/AccountInfoHeader.ascx" TagName="MasterValues" TagPrefix="MV" %>
<%@ Register Src="~/App_Shared/ProgramPeriod.ascx" TagName="ProgramPeriod" TagPrefix="PP" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/App_Shared/SaveCancel.ascx" TagPrefix="uc1" TagName="Sc" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeaderPlaceHolder" runat="Server">
    <table width="910px">
        <tr>
            <td align="left">
                <asp:Label ID="lblRetroInfoHeading" runat="server" Text="Retro Information" CssClass="h2"></asp:Label>
            </td>
            <td align="right">
            </td>
        </tr>
    </table>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainPlaceHolder" runat="Server">
    <asp:ObjectDataSource ID="ExposureTypeDataSource" runat="server" SelectMethod="GetLookUpActiveData"
        TypeName="ZurichNA.AIS.Business.Logic.BLAccess">
        <SelectParameters>
            <asp:Parameter DefaultValue="EXPOSURE TYPE" Name="lookUpTypeName" Type="String" />
        </SelectParameters>
    </asp:ObjectDataSource>
    <asp:ObjectDataSource ID="PerTypeDataSource" runat="server" SelectMethod="GetLookUpActiveData"
        TypeName="ZurichNA.AIS.Business.Logic.BLAccess">
        <SelectParameters>
            <asp:Parameter DefaultValue="PER" Name="lookUpTypeName" Type="String" />
        </SelectParameters>
    </asp:ObjectDataSource>
    <script type="text/javascript">
        function addCommas(nStr) {
            nStr += '';
            x = nStr.split('.');
            x1 = x[0];
            x2 = x.length > 1 ? '.' + x[1] : '';
            var rgx = /(\d+)(\d{3})/;
            while (rgx.test(x1)) {
                x1 = x1.replace(rgx, '$1' + ',' + '$2');
            }
            return x1 + x2;
        }
        function Tabnavigation(Pagename) {
            var progPeridID = $get('<%=hidProgPerdID.ClientID%>');
            var Flag = $get('ctl00_MainPlaceHolder_UcMastervalues_CollapseAccountHeader_ClientState');
            var strURL = "../AdjParams/";

            var flag = document.getElementById('ctl00_hdnControlDirty').value;
            var proceed = true;

            if (flag == '1') {
                if (confirm('You are trying to navigate out of this tab without saving.\n'
                            + 'Do you want to proceed without saving?\n\n'
                            + 'Press OK to continue, or Cancel to stay on the current tab.')) {
                    proceed = true;
                    document.getElementById('ctl00_hdnControlDirty').value = '0';
                }
                else {
                    proceed = false;
                    var ctrl = $find('<%=TabContainer1.ClientID%>');
                    ctrl.set_activeTab(ctrl.get_tabs()[0]);
                    __doPostBack('<%=TabContainer1.ClientID %>', ctrl.get_activeTab().get_headerText());
                }
            }

            if (proceed) {
                if (Pagename == "CE") {
                    strURL += "CombinedElements.aspx";
                }
                else if (Pagename == "AI") {
                    strURL += "AuditInfo.aspx";
                }
                else if (Pagename == "AE") {
                    strURL += "AssignERPFormula.aspx";
                }
                if (progPeridID.value > 0) {
                    strURL += "?ProgPerdID=" + progPeridID.value + "&Flag=" + Flag.value;
                }
                else {
                    strURL += "?Flag=" + Flag.value;
                }
                window.location.href = strURL;
            }
        }
        function ExpoChanged(eaclid, fclid, taclid, tauditclid, faclid, faclid1, etclid, audexpclid, ddlper, hdtxtTotalagr, hdtxtAudExp, hdtxtTotalAudit) {
            var myRegExp = /,/g;
            var vsplit = fclid.split('_');
            var rowno = vsplit[vsplit.length - 2].replace(/ctrl/, "");
            var ddlExposureType = document.getElementById(etclid);
            var txtExposureAgr = document.getElementById(eaclid);
            var txtFactor = document.getElementById(fclid);
            var txtFactorAgr = document.getElementById(faclid);
            var txtFactorAgrAct = document.getElementById(faclid1);
            var txtTotalAgr = document.getElementById(taclid);
            var txtAudExp = document.getElementById(audexpclid);
            var txtTotalAudit = document.getElementById(tauditclid);
            var hidtxtTotalAgr = document.getElementById(hdtxtTotalagr);
            var hidtxtAudExp = document.getElementById(hdtxtAudExp);
            var hidtxtTotalAudit = document.getElementById(hdtxtTotalAudit);

            var Audit_Manual_Text = document.getElementById("ctl00_MainPlaceHolder_TabContainer1_tblpnlRetroInfo_hdtxtAudExpManualText");
            var index = ddlExposureType.selectedIndex;
            var TypText = ddlExposureType.options[index].text;

            var ddlperType = document.getElementById(ddlper);
            var ddlexpbasistext = ddlperType.options[ddlperType.selectedIndex].text;

            document.getElementById('ctl00_hdnControlDirty').value = '1';

            txtTotalAgr.innerText = Math.round(txtExposureAgr.value.replace(myRegExp, '') * txtFactor.value.replace(myRegExp, ''));
            txtTotalAudit.innerText = Math.round(txtExposureAgr.value.replace(myRegExp, '') * txtFactorAgr.value.replace(myRegExp, ''));

            if (TypText == Audit_Manual_Text.value) {
                oldFactorAgr = txtFactorAgr.value;
                txtAudExp.innerText = txtExposureAgr.value;
                txtFactorAgr.value = txtFactor.value;

            }
            if (ddlexpbasistext != "(Select)") {
                txtTotalAudit.innerText = Math.round((parseFloat(txtAudExp.innerText.replace(myRegExp, '')) * parseFloat(txtFactorAgr.value.replace(myRegExp, ''))) / ddlexpbasistext);
            }
            else {
                txtTotalAudit.innerText = "";
            }
            if (txtTotalAudit.innerText == "NaN") {
                txtTotalAudit.innerText = "";
            }
            if (txtAudExp.innerText == "NaN") {
                txtAudExp.innerText = "";
            }
            if (txtFactorAgr.value == "NaN") {
                txtFactorAgr.value = "";
            }
            if (txtAudExp.innerText != "") {
                txtAudExp.innerText = Math.round(txtAudExp.innerText.replace(myRegExp, ''));
                txtAudExp.innerText = addCommas(txtAudExp.innerText);
            }
            if (txtTotalAudit.innerText != "") {
                txtTotalAudit.innerText = Math.round(txtTotalAudit.innerText.replace(myRegExp, ''));
                txtTotalAudit.innerText = addCommas(txtTotalAudit.innerText);
            }
            if (txtTotalAgr.innerText != "") {
                txtTotalAgr.innerText = Math.round(txtTotalAgr.innerText.replace(myRegExp, ''));
                txtTotalAgr.innerText = addCommas(txtTotalAgr.innerText);
            }
            hidtxtTotalAgr.value = txtTotalAgr.innerText;
            hidtxtAudExp.value = txtAudExp.innerText;
            hidtxtTotalAudit.value = txtTotalAudit.innerText;
            if (rowno == "0") {
                BasicPremium();
                BasicRow();
            }

        }
        var Maxval;
        function BasicPremium() {
            var varTotAgramt = $get(varTotAgr0).innerText.replace(myRegExp, '');
            var valTotAudtamt = $get(varTotAud0).innerText.replace(myRegExp, '');
            if (valTotAudtamt == "") {
                valTotAudtamt = 0;
            }
            if (varTotAgramt == "") {
                varTotAgramt = 0;
            }
            if (parseInt(varTotAgramt) > parseInt(valTotAudtamt)) {
                Maxval = varTotAgramt;
            }
            else {
                Maxval = valTotAudtamt;
            }
        }
        var DynamicID = "ctl00_MainPlaceHolder_TabContainer1_tblpnlRetroinfo_LstRetroinfo_";
        var myRegExp = /,/g;
        var varTotAgr0 = DynamicID + "ctrl0_txtTotalAgr";

        var varTotAud0 = DynamicID + "ctrl0_txtTotalAudit";
        var varTotAud1 = DynamicID + "ctrl1_txtTotalAudit";
        var varTotAud2 = DynamicID + "ctrl2_txtTotalAudit";
        var varTotAud3 = DynamicID + "ctrl3_txtTotalAudit";
        var varhidTotAud0 = DynamicID + "ctrl0_hidtxtTotalAudit";
        var varhidTotAud1 = DynamicID + "ctrl1_hidtxtTotalAudit";
        var varhidTotAud2 = DynamicID + "ctrl2_hidtxtTotalAudit";
        var varhidTotAud3 = DynamicID + "ctrl3_hidtxtTotalAudit";

        var varAudExp1 = DynamicID + "ctrl1_txtAudExp";
        var varhidAudExp1 = DynamicID + "ctrl1_hidtxtAudExp";
        var varAudExp2 = DynamicID + "ctrl2_txtAudExp";
        var varhidAudExp2 = DynamicID + "ctrl2_hidtxtAudExp";
        var varAudExp3 = DynamicID + "ctrl3_txtAudExp";
        var varhidAudExp3 = DynamicID + "ctrl3_hidtxtAudExp";

        var varddlperType1 = DynamicID + "ctrl1_ddlPerList";
        var varddlperType2 = DynamicID + "ctrl2_ddlPerList";
        var varddlperType3 = DynamicID + "ctrl3_ddlPerList";

        var varddlExpoType1 = DynamicID + "ctrl1_ddlExpTypelist";
        var varddlExpoType2 = DynamicID + "ctrl2_ddlExpTypelist";
        var varddlExpoType3 = DynamicID + "ctrl3_ddlExpTypelist";

        var varFactAgr1 = DynamicID + "ctrl1_txtFactorAgr";
        var varFactAgr2 = DynamicID + "ctrl2_txtFactorAgr";
        var varFactAgr3 = DynamicID + "ctrl3_txtFactorAgr";

        var varchkNA1 = DynamicID + "ctrl1_chkDntApply";
        var varchkNA2 = DynamicID + "ctrl2_chkDntApply";
        var varchkNA3 = DynamicID + "ctrl3_chkDntApply";

        function ExpoTypChanged(eaclid, fclid, etclid, audexpclid, tauditclid, faclid, faclid1, ddlper, hdtxtAudExp, hdtxtTotalAudit) {

            var vsplit = fclid.split('_');
            var rowno = vsplit[vsplit.length - 2].replace(/ctrl/, "");
            var Audit_Standard = document.getElementById("<%= hdtxtAudExpStandard.ClientID%>");
            var Audit_Payroll = document.getElementById("<%=hdtxtAudExpPayroll.ClientID%>");
            var Audit_Combined = document.getElementById("<%=hdtxtAudExpCombined.ClientID %>");
            var Audit_Other = document.getElementById("<%=hdtxtAudExpOther.ClientID %>");
            var Audit_Standard_Text = document.getElementById("<%=hdtxtAudExpStandardText.ClientID %>");
            var Audit_Payroll_Text = document.getElementById("<%=hdtxtAudExpPayrollText.ClientID %>");
            var Audit_Combined_Text = document.getElementById("<%=hdtxtAudExpCombinedText.ClientID %>");
            var Audit_Manual_Text = document.getElementById("<%=hdtxtAudExpManualText.ClientID%>");
            var ddlExposureType = document.getElementById(etclid);
            var txtAudExp = document.getElementById(audexpclid);
            var txtExposureAgr = document.getElementById(eaclid);
            var txtFactor = document.getElementById(fclid);
            var txtFactorAgr = document.getElementById(faclid);
            var txtTotalAudit = document.getElementById(tauditclid);
            var index = ddlExposureType.selectedIndex;
            var TypText = ddlExposureType.options[index].text;
            var ddlperType = document.getElementById(ddlper);
            var ddlexpbasistext = ddlperType.options[ddlperType.selectedIndex].text;
            var hidtxtAudExp = document.getElementById(hdtxtAudExp);
            var hidtxtTotalAudit = document.getElementById(hdtxtTotalAudit);
            txtFactorAgr.value = faclid1;
            document.getElementById('ctl00_hdnControlDirty').value = '1';
            if (TypText == Audit_Manual_Text.value) {
                txtAudExp.innerText = txtExposureAgr.value
                txtFactorAgr.value = txtFactor.value
                txtTotalAudit.innerText = Math.round(parseFloat(txtExposureAgr.value.replace(myRegExp, '')) * parseFloat(txtFactorAgr.value.replace(myRegExp, '')));
            }
            else if (TypText == Audit_Standard_Text.value) {
                txtAudExp.innerText = Audit_Standard.value;
            }
            else if (TypText == Audit_Payroll_Text.value) {
                txtAudExp.innerText = Audit_Payroll.value;
            }
            else if (TypText == Audit_Combined_Text.value) {
                txtAudExp.innerText = Audit_Combined.value;
            }
            else if (TypText == "Basic Premium" && rowno != "0") {
                BasicPremium();
                txtAudExp.innerText = Maxval;
            }
            else {
                txtAudExp.innerText = Audit_Other.value;
            }
            if (ddlexpbasistext == "(Select)") {
                txtTotalAudit.innerText = "";
            }
            else {
                txtTotalAudit.innerText = (parseFloat(txtAudExp.innerText.replace(myRegExp, '')) * parseFloat(txtFactorAgr.value.replace(myRegExp, ''))) / ddlexpbasistext;
            }
            if (txtTotalAudit.innerText == "NaN") {
                txtTotalAudit.innerText = "";
            }
            if (txtAudExp.innerText == "NaN") {
                txtAudExp.innerText = "";
            }
            if (txtFactorAgr.value == "NaN") {
                txtFactorAgr.value = "";
            }
            if (txtAudExp.innerText != "") {
                txtAudExp.innerText = Math.round(txtAudExp.innerText.replace(myRegExp, ''));
                txtAudExp.innerText = addCommas(txtAudExp.innerText);
            }
            if (txtTotalAudit.innerText != "") {
                txtTotalAudit.innerText = Math.round(txtTotalAudit.innerText.replace(myRegExp, ''));
                txtTotalAudit.innerText = addCommas(txtTotalAudit.innerText);
            }
            hidtxtAudExp.value = txtAudExp.innerText;
            hidtxtTotalAudit.value = txtTotalAudit.innerText;
            if (rowno == "0") {
                BasicPremium();
                BasicRow();
            }

        }
        function BasicRow() {
            if ($get(varddlExpoType1).options[$get(varddlExpoType1).selectedIndex].text == "Basic Premium" && (!$get(varchkNA1).checked)) {
                $get(varAudExp1).innerText = addCommas(Maxval);
                $get(varhidAudExp1).value = Maxval;
                if ($get(varddlperType1).options[$get(varddlperType1).selectedIndex].text != "(Select)") {
                    var var1 = $get(varFactAgr1).value.replace(myRegExp, '');
                    var var2 = $get(varddlperType1).options[$get(varddlperType1).selectedIndex].text;
                    $get(varTotAud1).innerText = addCommas(Math.round((parseFloat(Maxval) * parseFloat(var1)) / parseFloat(var2)));
                    $get(varhidTotAud1).value = $get(varTotAud1).innerText;
                }
            }
            if ($get(varddlExpoType2).options[$get(varddlExpoType2).selectedIndex].text == "Basic Premium" && (!$get(varchkNA2).checked)) {
                $get(varAudExp2).innerText = addCommas(Maxval);
                $get(varhidAudExp2).value = Maxval;
                if ($get(varddlperType2).options[$get(varddlperType2).selectedIndex].text != "(Select)") {
                    var var1 = $get(varFactAgr2).value.replace(myRegExp, '');
                    var var2 = $get(varddlperType2).options[$get(varddlperType2).selectedIndex].text;
                    $get(varTotAud2).innerText = addCommas(Math.round((parseFloat(Maxval) * parseFloat(var1)) / parseFloat(var2)));
                    $get(varhidTotAud2).value = $get(varTotAud2).innerText;
                }
            }
            if ($get(varddlExpoType3).options[$get(varddlExpoType3).selectedIndex].text == "Basic Premium" && (!$get(varchkNA3).checked)) {
                $get(varAudExp3).innerText = addCommas(Maxval);
                $get(varhidAudExp3).value = Maxval;
                if ($get(varddlperType3).options[$get(varddlperType3).selectedIndex].text != "(Select)") {
                    var var1 = $get(varFactAgr3).value.replace(myRegExp, '');
                    var var2 = $get(varddlperType3).options[$get(varddlperType3).selectedIndex].text;
                    $get(varTotAud3).innerText = addCommas(Math.round((parseFloat(Maxval) * parseFloat(var1)) / parseFloat(var2)));
                    $get(varhidTotAud3).value = $get(varTotAud3).innerText;
                }
            }
        }
        function ChkNAClick(cmpExpid, chkNa, txt1, txt2, txt3, ddl1, ddl2) {

            var v1 = $get(cmpExpid);
            var v3 = $get(chkNa);

            document.getElementById('ctl00_hdnControlDirty').value = '1';

            $get(txt1).disabled = v3.checked;
            $get(txt2).disabled = v3.checked;
            $get(txt3).disabled = v3.checked;
            $get(ddl1).disabled = v3.checked;
            $get(ddl2).disabled = v3.checked;
            if (v3.checked) {
                ValidatorEnable(v1, false);

            }
            else {
                ValidatorEnable(v1, true);
                BasicPremium();
                BasicRow();
            }
        }
        function ChkNoLimitClick(cmpExpid, chkNa) {

            var v1 = $get(cmpExpid);
            var v3 = $get(chkNa);

            document.getElementById('ctl00_hdnControlDirty').value = '1';

            if (v3.checked) {
                ValidatorEnable(v1, false);

            }
            else {
                ValidatorEnable(v1, true);
            }
        }     
       
        
    </script>

    <table width="910px">
        <tr>
            <td>
                <MV:MasterValues ID="UcMastervalues" runat="server" />
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="lblProgramPeriodsHeader" runat="server" CssClass="h2" Text="Program Periods"></asp:Label>
                <div class="content" style="width: 910px; overflow: auto">
                    <asp:Panel ID="panGridView" runat="server" BorderWidth="0px" Width="910px">
                        <asp:UpdatePanel ID="upGridView" runat="server">
                            <ContentTemplate>
                                <PP:ProgramPeriod ID="ProgramPeriod" runat="server" OnOnItemCommand="ProgramPeriod_ItemCommand" />
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </asp:Panel>
                </div>
            </td>
        </tr>
        <tr>
            <td>
                <cc1:TabContainer ID="TabContainer1" runat="server" CssClass="CustomTabs" ActiveTabIndex="0">
                    <cc1:TabPanel runat="server" ID="tblpnlRetroInfo" HeaderText="Retro Information">
                        <HeaderTemplate>
                            Retro Information</HeaderTemplate>
                        <ContentTemplate>
                            <asp:ValidationSummary ID="valsEdit" runat="server" ValidationGroup="ValGroup" />
                            <table>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblRetroInfo" runat="server" CssClass="h2" Text="Retro Information -- "
                                            Visible="False"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblEffdt" runat="server" CssClass="h2"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblExpdt" runat="server" CssClass="h2"></asp:Label>
                                    </td>
                                    <td width="100px" align="right">
                                        <asp:LinkButton ID="lbClose" runat="server" Visible="false" Text="Close" OnClick="CloseContactInfo"></asp:LinkButton>
                                    </td>
                                </tr>
                            </table>
                            <asp:HiddenField ID="hidProgPerdID" runat="server" Value="0" />
                            <asp:HiddenField ID="hdtxtAudExpStandard" runat="server" />
                            <asp:HiddenField ID="hdtxtAudExpPayroll" runat="server" />
                            <asp:HiddenField ID="hdtxtAudExpCombined" runat="server" />
                            <asp:HiddenField ID="hdtxtAudExpOther" runat="server" />
                            <asp:HiddenField ID="hdtxtAudExpStandardText" runat="server" />
                            <asp:HiddenField ID="hdtxtAudExpPayrollText" runat="server" />
                            <asp:HiddenField ID="hdtxtAudExpCombinedText" runat="server" />
                            <asp:HiddenField ID="hdtxtAudExpManualText" runat="server" />
                            <asp:HiddenField ID="hidBasicMax" runat="server" />
                            <asp:Panel ID="panRetroInfo" runat="server" Width="910px" ScrollBars="Auto" Visible="true">
                                <asp:Label ID="hdPgmPerId" runat="server" Visible="False"></asp:Label>
                                <asp:AISListView ID="LstRetroInfo" runat="server" OnItemDataBound="LstRetroInfo_DataBoundList"
                                    OnItemCommand="LstRetroInfo_ItemCommand">
                                    <LayoutTemplate>
                                        <table id="Table1" runat="server" class="panelContents">
                                            <tr class="LayoutTemplate">
                                                <th>
                                                    N/A
                                                </th>
                                                <th>
                                                    No Limit
                                                </th>
                                                <th>
                                                    Retro Elements
                                                </th>
                                                <th>
                                                    Exposure(AGR)
                                                </th>
                                                <th>
                                                    Factor
                                                </th>
                                                <th>
                                                    Total(AGR)
                                                </th>
                                                <th>
                                                    Exposure Type
                                                </th>
                                                <th>
                                                    Audit Exposure
                                                </th>
                                                <th>
                                                    Factor(AGR)
                                                </th>
                                                <th>
                                                    Exposure Basis
                                                </th>
                                                <th>
                                                    Total (Audit)
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
                                        <tr id="Tr1" runat="server" class="ItemTemplate">
                                            <td valign="middle">
                                                <asp:Label ID="hdAdjRetroInfoId" runat="server" Text='<%# Bind("ADJ_RETRO_INFO_ID")%>'
                                                    Visible="false"></asp:Label>
                                                <asp:Label ID="hdCustId" runat="server" Text='<%# Bind("CUSTMR_ID")%>' Visible="false"></asp:Label>
                                                <asp:CheckBox ID="chkDntApply" runat="server" Checked='<%# Bind("RETRO_ADJ_FCTR_APLCBL_IND")%>' />
                                            </td>
                                            <td valign="middle">
                                                <asp:CheckBox ID="chkNoLimit" runat="server" Checked='<%# Bind("NO_LIM_IND")%>' />
                                            </td>
                                            <td valign="middle">
                                                <asp:Label ID="hdRetroElemTypId" runat="server" Text='<%# Bind("RETRO_ELEMT_TYP_ID")  %>'
                                                    Visible="false" Width="50px"></asp:Label>
                                                <asp:Label ID="lblRetroElements" runat="server" Text='<%# Bind("RETRO_ELEMT")  %>'
                                                    Width="50px"></asp:Label>
                                            </td>
                                            <td valign="middle">
                                                <%-- <cc1:FilteredTextBoxExtender runat="server" TargetControlID="txtExposureAgr" FilterType="Custom"
                                                    ValidChars="0123456789," ID="fltAmount">
                                                </cc1:FilteredTextBoxExtender>--%>
                                                <asp:AISAmountTextbox ID="txtExposureAgr" runat="server" Text='<%#Eval("EXPO_AGMT_AMT")==null?String.Empty:decimal.Parse(Eval("EXPO_AGMT_AMT").ToString()).ToString("#,##0") %>'
                                                    Width="95px" Style="text-align: right">
                                                </asp:AISAmountTextbox>
                                            </td>
                                            <td valign="middle">
                                                <cc1:MaskedEditExtender ID="mskFactor" runat="server" TargetControlID="txtFactor"
                                                    Mask="99.999999" MaskType="Number" AutoComplete="false" />
                                                <asp:TextBox ID="txtFactor" runat="server" Text='<%#Eval("RETRO_ADJ_FCTR_RT")%>'
                                                    Width="60px"></asp:TextBox>
                                            </td>
                                            <td valign="middle" align="right">
                                                <asp:Label ID="txtTotalAgr" runat="server" Text='<%#Eval("TOT_AGMT_AMT")==null?String.Empty:decimal.Parse(Eval("TOT_AGMT_AMT").ToString()).ToString("#,##0") %>'>
                                                </asp:Label>
                                                <asp:TextBox ID="hidtxtTotalAgr" Width="0px" Style="visibility: hidden" runat="server"
                                                    Text='<%#Eval("TOT_AGMT_AMT")==null?String.Empty:decimal.Parse(Eval("TOT_AGMT_AMT").ToString()).ToString("#,##0") %>'>
                                                </asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlExpTypelist" runat="server" DataSourceID="ExposureTypeDataSource"
                                                    DataTextField="LookUpName" DataValueField="LookUpID" Width="120px">
                                                </asp:DropDownList>
                                                <asp:Label ID="lblhdExpType" runat="server" Text='<%#(Eval("EXPO_TYP_ID")==null)?("0"):(Eval("EXPO_TYP_ID"))%>'
                                                    Visible="false"></asp:Label>
                                                <asp:CompareValidator ID="valExpType" runat="server" ControlToValidate="ddlExpTypelist"
                                                    ErrorMessage="Please select Exposure Type" Operator="NotEqual" Text="*" ValidationGroup="ValGroup"
                                                    ValueToCompare="0"></asp:CompareValidator>
                                            </td>
                                            <td valign="middle" align="right">
                                                <asp:Label ID="txtAudExp" runat="server" Text='<%#Eval("AUDT_EXPO_AMT")==null?String.Empty:decimal.Parse(Eval("AUDT_EXPO_AMT").ToString()).ToString("#,##0") %>'>
                                                </asp:Label>
                                                <asp:TextBox ID="hidtxtAudExp" Width="0px" Style="visibility: hidden" runat="server"
                                                    Text='<%#Eval("AUDT_EXPO_AMT")==null?String.Empty:decimal.Parse(Eval("AUDT_EXPO_AMT").ToString()).ToString("#,##0") %>'>
                                                </asp:TextBox>
                                            </td>
                                            <td valign="middle">
                                                <cc1:MaskedEditExtender ID="mskFactorAgr" runat="server" TargetControlID="txtFactorAgr"
                                                    Mask="99.999999" MaskType="Number" AutoComplete="false" />
                                                <asp:TextBox ID="txtFactorAgr" runat="server" Text='<%#Eval("AGGR_FCTR_PCT")%>' Width="60px"></asp:TextBox>
                                            </td>
                                            <td valign="middle">
                                                <asp:DropDownList ID="ddlPerList" runat="server" DataSourceID="PerTypeDataSource"
                                                    DataTextField="LookUpName" DataValueField="LookUpID" Width="70px">
                                                </asp:DropDownList>
                                                <asp:Label ID="lblhdPerType" runat="server" Text='<%#(Eval("EXPO_TYP_INCREMNT_NBR_ID")==null)?("0"):(Eval("EXPO_TYP_INCREMNT_NBR_ID"))%>'
                                                    Visible="false"></asp:Label>
                                            </td>
                                            <td valign="middle" align="right">
                                                <asp:Label ID="txtTotalAudit" runat="server" Text='<%#(Eval("TOT_AUDT_AMT")==null)?String.Empty:decimal.Parse(Eval("TOT_AUDT_AMT").ToString()).ToString("#,##0") %>'>
                                                </asp:Label>
                                                <asp:TextBox ID="hidtxtTotalAudit" Width="0px" Style="visibility: hidden" runat="server"
                                                    Text='<%#(Eval("TOT_AUDT_AMT")==null)?String.Empty:decimal.Parse(Eval("TOT_AUDT_AMT").ToString()).ToString("#,##0") %>'>
                                                </asp:TextBox>
                                            </td>
                                            <td valign="middle">
                                                <asp:Label ID="lblActive" runat="server" Text='<%# Bind("ACTV_IND") %>' Visible="false"></asp:Label>
                                                <asp:ImageButton ID="ibtnEnableDisable" runat="server" CommandArgument='<%# Bind("ADJ_RETRO_INFO_ID") %>'
                                                    Visible="false" />
                                                <asp:TextBox Style="visibility: hidden" Width="0px" ID="txtFactorAgrAct" runat="server"
                                                    Text='<%#Eval("AGGR_FCTR_PCT")%>'></asp:TextBox>
                                            </td>
                                        </tr>
                                    </ItemTemplate>
                                    <AlternatingItemTemplate>
                                        <tr id="Tr1" runat="server" class="AlternatingItemTemplate">
                                            <td valign="middle">
                                                <asp:Label ID="hdAdjRetroInfoId" runat="server" Text='<%# Bind("ADJ_RETRO_INFO_ID")%>'
                                                    Visible="false"></asp:Label>
                                                <asp:Label ID="hdCustId" runat="server" Text='<%# Bind("CUSTMR_ID")%>' Visible="false"></asp:Label>
                                                <asp:CheckBox ID="chkDntApply" runat="server" Checked='<%# Bind("RETRO_ADJ_FCTR_APLCBL_IND")%>' />
                                            </td>
                                            <td valign="middle">
                                                <asp:CheckBox ID="chkNoLimit" runat="server" Checked='<%# Bind("NO_LIM_IND")%>' />
                                            </td>
                                            <td valign="middle">
                                                <asp:Label ID="hdRetroElemTypId" runat="server" Text='<%# Bind("RETRO_ELEMT_TYP_ID")  %>'
                                                    Visible="false" Width="50px"></asp:Label>
                                                <asp:Label ID="lblRetroElements" runat="server" Text='<%# Bind("RETRO_ELEMT")  %>'
                                                    Width="50px"></asp:Label>
                                            </td>
                                            <td valign="middle">
                                                <%-- <cc1:FilteredTextBoxExtender runat="server" TargetControlID="txtExposureAgr" FilterType="Custom"
                                                    ValidChars="0123456789," ID="fltAmount">
                                                </cc1:FilteredTextBoxExtender>
                                                <asp:TextBox ID="txtExposureAgr" runat="server" MaxLength="11" Text='<%#Eval("EXPO_AGMT_AMT")==null?String.Empty:decimal.Parse(Eval("EXPO_AGMT_AMT").ToString()).ToString("#,##0") %>'
                                                    Width="95px" Style="text-align: right" onblur="FormatNumNoDecAmt(this,11)" onfocus="RemoveCommas(this)">
                                                </asp:TextBox>--%>
                                                <asp:AISAmountTextbox ID="txtExposureAgr" runat="server" Text='<%#Eval("EXPO_AGMT_AMT")==null?String.Empty:decimal.Parse(Eval("EXPO_AGMT_AMT").ToString()).ToString("#,##0") %>'
                                                    Width="95px" Style="text-align: right">
                                                </asp:AISAmountTextbox>
                                            </td>
                                            <td valign="middle">
                                                <cc1:MaskedEditExtender ID="mskFactor" runat="server" TargetControlID="txtFactor"
                                                    Mask="99.999999" MaskType="Number" AutoComplete="false" />
                                                <asp:TextBox ID="txtFactor" runat="server" Text='<%#Eval("RETRO_ADJ_FCTR_RT")%>'
                                                    Width="60px"></asp:TextBox>
                                            </td>
                                            <td valign="middle" align="right">
                                                <asp:Label ID="txtTotalAgr" runat="server" Text='<%#Eval("TOT_AGMT_AMT")==null?String.Empty:decimal.Parse(Eval("TOT_AGMT_AMT").ToString()).ToString("#,##0") %>'>
                                                </asp:Label>
                                                <asp:TextBox ID="hidtxtTotalAgr" Width="0px" Style="visibility: hidden" runat="server"
                                                    Text='<%#Eval("TOT_AGMT_AMT")==null?String.Empty:decimal.Parse(Eval("TOT_AGMT_AMT").ToString()).ToString("#,##0") %>'>
                                                </asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlExpTypelist" runat="server" DataSourceID="ExposureTypeDataSource"
                                                    DataTextField="LookUpName" DataValueField="LookUpID" Width="120px">
                                                </asp:DropDownList>
                                                <asp:Label ID="lblhdExpType" runat="server" Text='<%#(Eval("EXPO_TYP_ID")==null)?("0"):(Eval("EXPO_TYP_ID"))%>'
                                                    Visible="false"></asp:Label>
                                                <asp:CompareValidator ID="valExpType" runat="server" ControlToValidate="ddlExpTypelist"
                                                    ErrorMessage="Please select Exposure Type" Operator="NotEqual" Text="*" ValidationGroup="ValGroup"
                                                    ValueToCompare="0"></asp:CompareValidator>
                                            </td>
                                            <td valign="middle" align="right">
                                                <asp:Label ID="txtAudExp" runat="server" Text='<%#Eval("AUDT_EXPO_AMT")==null?String.Empty:decimal.Parse(Eval("AUDT_EXPO_AMT").ToString()).ToString("#,##0") %>'>
                                                </asp:Label>
                                                <asp:TextBox ID="hidtxtAudExp" Width="0px" Style="visibility: hidden" runat="server"
                                                    Text='<%#Eval("AUDT_EXPO_AMT")==null?String.Empty:decimal.Parse(Eval("AUDT_EXPO_AMT").ToString()).ToString("#,##0") %>'>
                                                </asp:TextBox>
                                            </td>
                                            <td valign="middle">
                                                <cc1:MaskedEditExtender ID="mskFactorAgr" runat="server" TargetControlID="txtFactorAgr"
                                                    Mask="99.999999" MaskType="Number" AutoComplete="false" />
                                                <asp:TextBox ID="txtFactorAgr" runat="server" Text='<%#Eval("AGGR_FCTR_PCT")%>' Width="60px"></asp:TextBox>
                                            </td>
                                            <td valign="middle">
                                                <asp:DropDownList ID="ddlPerList" runat="server" DataSourceID="PerTypeDataSource"
                                                    DataTextField="LookUpName" DataValueField="LookUpID" Width="70px">
                                                </asp:DropDownList>
                                                <asp:Label ID="lblhdPerType" runat="server" Text='<%#(Eval("EXPO_TYP_INCREMNT_NBR_ID")==null)?("0"):(Eval("EXPO_TYP_INCREMNT_NBR_ID"))%>'
                                                    Visible="false"></asp:Label>
                                            </td>
                                            <td valign="middle" align="right">
                                                <asp:Label ID="txtTotalAudit" runat="server" Text='<%#(Eval("TOT_AUDT_AMT")==null)?String.Empty:decimal.Parse(Eval("TOT_AUDT_AMT").ToString()).ToString("#,##0") %>'>
                                                </asp:Label>
                                                <asp:TextBox ID="hidtxtTotalAudit" Width="0px" Style="visibility: hidden" runat="server"
                                                    Text='<%#(Eval("TOT_AUDT_AMT")==null)?String.Empty:decimal.Parse(Eval("TOT_AUDT_AMT").ToString()).ToString("#,##0") %>'>
                                                </asp:TextBox>
                                            </td>
                                            <td valign="middle">
                                                <asp:Label ID="lblActive" runat="server" Text='<%# Bind("ACTV_IND") %>' Visible="false"></asp:Label>
                                                <asp:ImageButton ID="ibtnEnableDisable" runat="server" CommandArgument='<%# Bind("ADJ_RETRO_INFO_ID") %>'
                                                    Visible="false" />
                                                <asp:TextBox Style="visibility: hidden" Width="0px" ID="txtFactorAgrAct" runat="server"
                                                    Text='<%#Eval("AGGR_FCTR_PCT")%>'></asp:TextBox>
                                            </td>
                                        </tr>
                                    </AlternatingItemTemplate>
                                </asp:AISListView>
                            </asp:Panel>
                            <asp:Panel ID="pnlSaveClose" runat="server" Visible="False">
                                <table style="text-align: center; width: 910px">
                                    <tr>
                                        <td>
                                        </td>
                                    </tr>
                                    <tr align="center">
                                        <td align="center">
                                            <asp:Button ID="btnSave" Width="60px" UseSubmitBehavior="true" runat="server" Text="Save"
                                                OnClick="btnSave_Click" ValidationGroup="ValGroup" TabIndex="1" />
                                            <asp:Button ID="btnCancel" Width="60px" runat="server" Text="Cancel" OnClick="btnCancel_Click"
                                                TabIndex="2" />
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                        </ContentTemplate>
                    </cc1:TabPanel>
                    <cc1:TabPanel runat="server" ID="tblpnlAssignERP" HeaderText="Assign ERP Formula">
                        <HeaderTemplate>
                            <div id="dvAssignERP" onclick="Tabnavigation('AE')" runat="server">
                                Assign ERP Formula</div>
                        </HeaderTemplate>
                        <ContentTemplate>
                        </ContentTemplate>
                    </cc1:TabPanel>
                    <cc1:TabPanel runat="server" ID="tblpnlCE" HeaderText="Combined Elements">
                        <HeaderTemplate>
                            <div id="dvCE" onclick="Tabnavigation('CE')" runat="server">
                                Combined Elements</div>
                        </HeaderTemplate>
                        <ContentTemplate>
                        </ContentTemplate>
                    </cc1:TabPanel>
                    <cc1:TabPanel runat="server" ID="tblpnlAI" HeaderText="Audit Information">
                        <HeaderTemplate>
                            <div id="dvAI" onclick="Tabnavigation('AI')" runat="server">
                                Audit Information</div>
                        </HeaderTemplate>
                        <ContentTemplate>
                        </ContentTemplate>
                    </cc1:TabPanel>
                </cc1:TabContainer>
            </td>
        </tr>
        <tr>
            <td>
            </td>
        </tr>
    </table>
</asp:Content>
