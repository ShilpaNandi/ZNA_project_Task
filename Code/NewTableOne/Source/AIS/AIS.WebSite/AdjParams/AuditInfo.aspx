<%@ Page Language="C#" MasterPageFile="~/Retro.master" AutoEventWireup="true" Inherits="AdjParameters_AuditInfo"
    CodeBehind="AuditInfo.aspx.cs" %>

<%@ Register Src="../App_Shared/AccountInfoHeader.ascx" TagName="AccountInfoHeader"
    TagPrefix="AI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="AjaxToolKit" %>
<%@ Register Src="../App_Shared/ProgramPeriod.ascx" TagName="ProgramPeriod" TagPrefix="PP" %>
<%@ Register Src="../App_Shared/AccountList.ascx" TagName="AccountList" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeaderPlaceHolder" runat="Server">
    <asp:Label ID="lblAuditInfo" runat="server" Text="Audit Information" CssClass="h1"></asp:Label>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainPlaceHolder" runat="Server">

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
        var scrollTop1;

        if (Sys.WebForms.PageRequestManager != null) {
            Sys.WebForms.PageRequestManager.getInstance().add_beginRequest(BeginRequestHandler);
            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);
        }

        function BeginRequestHandler(sender, args) {
            var mef = $get('<%=pnlAuditInfo.ClientID%>');
            if (mef != null)
                scrollTop1 = mef.scrollTop;
        }

        function EndRequestHandler(sender, args) {
            var mef = $get('<%=pnlAuditInfo.ClientID%>');
            if (mef != null)
                mef.scrollTop = scrollTop1;
        }

        function Tabnavigation(Pagename) {
            var progPeridID = $get('<%=hidProgPerdID.ClientID%>');
            var Flag = $get('ctl00_MainPlaceHolder_UcMastervalues_CollapseAccountHeader_ClientState');
            var strURL = "../AdjParams/";

            var flag = document.getElementById('ctl00_hdnControlDirty').value;
            var proceed = true;

            if (flag == '1')
                if (confirm('You are trying to navigate out of this tab without saving.\n'
                        + 'Do you want to proceed without saving?\n\n'
                        + 'Press OK to continue, or Cancel to stay on the current tab.')) {
                proceed = true;
                document.getElementById('ctl00_hdnControlDirty').value = '0';
            }
            else {
                proceed = false;
                var ctrl = $find('<%=TabContainer1.ClientID%>');
                ctrl.set_activeTab(ctrl.get_tabs()[3]);
                __doPostBack('<%=TabContainer1.ClientID %>', ctrl.get_activeTab().get_headerText());
            }
            if (proceed) {
                if (Pagename == "CE") {
                    strURL += "CombinedElements.aspx";
                }
                else if (Pagename == "RI") {
                    strURL += "RetroInfo.aspx";
                }
                else if (Pagename == "AE") {
                    strURL += "AssignERPFormula.aspx";
                }
                if (progPeridID.value > 0) {
                    strURL += "?ProgPerdID=" + progPeridID.value + "&Flag=" + Flag.value + "&wID=<%= WindowName%>";
                }
                else {
                    strURL += "?Flag=" + Flag.value + "&wID=<%= WindowName%>";
                }
                window.location.href = strURL;
            }
        }

        function CalculateAuditresult(var1, var2, var3, var4, var5) {
            var sum = 0;

            document.getElementById('ctl00_hdnControlDirty').value = '1';
            var myRegExp = /,/g;

            if (var2.value != "") {

                sum += parseInt(var2.value.replace(myRegExp, ''));
            }
            if (var3.value != "") {
                sum += parseInt(var3.value.replace(myRegExp, ''));
            }
            if (var4.value != "") {
                sum += parseInt(var4.value.replace(myRegExp, ''));
            }
            var5.innerText = parseInt(var1) - (sum);
            if (var5.innerText != "") {
                var5.innerText = addCommas(var5.innerText);
            }
        }
        function CalculateInsertAuditresult() {
            var1 = $get('ctl00_MainPlaceHolder_TabContainer1_tblpnlAI_lstAuditInfo_ctrl0_txtdefdepprem');
            var2 = $get('ctl00_MainPlaceHolder_TabContainer1_tblpnlAI_lstAuditInfo_ctrl0_txtsubdepPrem');
            var3 = $get('ctl00_MainPlaceHolder_TabContainer1_tblpnlAI_lstAuditInfo_ctrl0_txtNSubDepPrem');
            var4 = $get('ctl00_MainPlaceHolder_TabContainer1_tblpnlAI_lstAuditInfo_ctrl0_lblAuditResult');
            var sum = 0;

            document.getElementById('ctl00_hdnControlDirty').value = '1';
            var myRegExp = /,/g;
            if (var1.value != "") {
                sum += parseInt(var1.value.replace(myRegExp, ''));
            }
            if (var2.value != "") {
                sum += parseInt(var2.value.replace(myRegExp, ''));
            }
            if (var3.value != "") {
                sum += parseInt(var3.value.replace(myRegExp, ''));
            }
            var4.innerText = -(sum);
            if (var4.innerText != "") {
                var4.innerText = addCommas(var4.innerText);
            }
        }
        
    </script>

    <AI:AccountInfoHeader ID="UcMastervalues" runat="server" />
    <asp:UpdatePanel ID="updpnlAuditInfo" runat="server">
        <ContentTemplate>
            <asp:ValidationSummary ID="valSumUpdate" CssClass="ValidationSummary" ValidationGroup="Update"
                runat="server"></asp:ValidationSummary>
            <asp:ValidationSummary ID="valsumSave" CssClass="ValidationSummary" ValidationGroup="Save"
                runat="server"></asp:ValidationSummary>
            <asp:ValidationSummary ID="valsubjUpdate" CssClass="ValidationSummary" ValidationGroup="subjUpdate"
                runat="server"></asp:ValidationSummary>
            <asp:ValidationSummary ID="valsubjSave" CssClass="ValidationSummary" ValidationGroup="subjSave"
                runat="server"></asp:ValidationSummary>
            <asp:ValidationSummary ID="valNsubjUpdate" CssClass="ValidationSummary" ValidationGroup="NsubjUpdate"
                runat="server"></asp:ValidationSummary>
            <asp:ValidationSummary ID="valNsubjSave" CssClass="ValidationSummary" ValidationGroup="NsubjSave"
                runat="server"></asp:ValidationSummary>
            <asp:Label ID="lblProgramPeriod" CssClass="h2" runat="server" Text="Program Periods"></asp:Label>
            <PP:ProgramPeriod ID="ucProgramPeriod" runat="server" />
            <AjaxToolKit:TabContainer ID="TabContainer1" runat="server" CssClass="CustomTabs"
                ActiveTabIndex="3">
                <AjaxToolKit:TabPanel runat="server" ID="tblpnlRetroInfo" HeaderText="Retro Information">
                    <HeaderTemplate>
                        <div onclick="Tabnavigation('RI')">
                            Retro Information</div>
                    </HeaderTemplate>
                    <ContentTemplate>
                    </ContentTemplate>
                </AjaxToolKit:TabPanel>
                <AjaxToolKit:TabPanel runat="server" ID="tblpnlAssignERP" HeaderText="Assign ERP Formula">
                    <HeaderTemplate>
                        <div onclick="Tabnavigation('AE')">
                            Assign ERP Formula</div>
                    </HeaderTemplate>
                    <ContentTemplate>
                    </ContentTemplate>
                </AjaxToolKit:TabPanel>
                <AjaxToolKit:TabPanel runat="server" ID="tblpnlCE" HeaderText="Combined Elements">
                    <HeaderTemplate>
                        <div onclick="Tabnavigation('CE')">
                            Combined Elements</div>
                    </HeaderTemplate>
                    <ContentTemplate>
                    </ContentTemplate>
                </AjaxToolKit:TabPanel>
                <AjaxToolKit:TabPanel runat="server" ID="tblpnlAI" HeaderText="Audit Information">
                    <HeaderTemplate>
                        Audit Information</HeaderTemplate>
                    <ContentTemplate>
                        <asp:HiddenField ID="hidProgPerdID" runat="server" Value="0" />
                        <asp:Label ID="lblAuditinfoDate" CssClass="h2" runat="server" Text=""></asp:Label>
                        &nbsp;<asp:LinkButton ID="lnkAuditinfoClose" runat="server" Visible="false" Text="Close"
                            OnClick="CloseAuditinfo"></asp:LinkButton>
                        <asp:Panel ID="pnlAuditInfo" runat="server" Height="120px" ScrollBars="Auto" Width="910px"
                            Visible="true">
                            <asp:AISListView ID="lstAuditInfo" runat="server" InsertItemPosition="FirstItem"
                                DataKeyNames="Comm_Agr_Audit_ID" OnItemCommand="CommandList" OnItemEditing="EditList"
                                OnItemUpdating="UpdateAuditList" OnItemDataBound="DataBoundList" OnItemCanceling="CancelList"
                                OnItemInserting="InsertCategory">
                                <LayoutTemplate>
                                    <table id="Table1" class="panelContents" runat="server" width="98%">
                                        <tr class="LayoutTemplate">
                                            <th>
                                            </th>
                                            <th>
                                                Policy
                                            </th>
                                            <th>
                                                Audit Date
                                            </th>
                                            <th>
                                                Sub Audit Prem
                                            </th>
                                            <th>
                                                N-Sub Audit Prem
                                            </th>
                                            <th>
                                                Def. Dep Prem
                                            </th>
                                            <th>
                                                Sub Dep. Prem
                                            </th>
                                            <th>
                                                N-Sub Dep. Prem
                                            </th>
                                            <th nowrap>
                                                Audit Result
                                            </th>
                                            <th>
                                                Exposure Amount
                                            </th>
                                            <th style="font-size: xx-small">
                                                Revised
                                            </th>
                                            <th style="font-size: xx-small">
                                                Revision
                                            </th>
                                        </tr>
                                        <tr id="itemPlaceholder" runat="server">
                                        </tr>
                                    </table>
                                </LayoutTemplate>
                                <ItemTemplate>
                                    <tr class="ItemTemplate" id="trItemTemplate" runat="server">
                                        <td align="left">
                                            <asp:LinkButton Font-Size="XX-Small" ID="lnkEdit" runat="server" Enabled='<%# Eval("Pol_Ind")!=null && Eval("Pol_Ind").ToString()=="False"?false:(Eval("AdjustmentIndicator")==null?true:(Eval("AdjustmentIndicator").ToString()== "False"?true:false)) %>'
                                                CommandName="Edit" Text="EDIT"></asp:LinkButton>
                                        </td>
                                        <td align="center">
                                            <asp:Label Font-Size="XX-Small" Visible="false" ID="lblComAgrID" runat="server" Text='<%# Bind("Comm_Agr_ID") %>'></asp:Label>
                                            <%# Eval("POLICY") %>
                                        </td>
                                        <td align="center">
                                            <asp:Label Font-Size="XX-Small" ID="lblStartDate" runat="server" Text='<%# Bind("StartDate","{0:d}")%>'></asp:Label>
                                        </td>
                                        <td>
                                            <asp:LinkButton Font-Size="XX-Small" ID="lnkSubPrmAmt" runat="server" CommandArgument='<%# Bind("Comm_Agr_Audit_ID")%>'
                                                CommandName="Subject" Text='<%#Eval("Sub_Aud_Prm_Amt")== null?String.Empty:decimal.Parse(Eval("Sub_Aud_Prm_Amt").ToString()).ToString("#,##0") %>'></asp:LinkButton>
                                        </td>
                                        <td>
                                            <asp:LinkButton ID="lnkNonSubPrmAmt" Font-Size="XX-Small" runat="server" CommandArgument='<%# Bind("Comm_Agr_Audit_ID")%>'
                                                CommandName="NonSubject" Text='<%#Eval("Non_Sub_Aud_Prm_Amt")== null?String.Empty:decimal.Parse(Eval("Non_Sub_Aud_Prm_Amt").ToString()).ToString("#,##0") %>'></asp:LinkButton>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblDefdepAmt" Font-Size="XX-Small" runat="server" Text='<%#Eval("Def_Dep_prm_Amt")== null?String.Empty:decimal.Parse(Eval("Def_Dep_prm_Amt").ToString()).ToString("#,##0") %>'></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblSubAmt" Font-Size="XX-Small" runat="server" Text='<%#Eval("Sub_Dep_Prm_Amt")== null?String.Empty:decimal.Parse(Eval("Sub_Dep_Prm_Amt").ToString()).ToString("#,##0") %>'></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblNonsubAmt" Font-Size="XX-Small" runat="server" Text='<%#Eval("Non_Sub_Dep_Prm_Amt")== null?String.Empty:decimal.Parse(Eval("Non_Sub_Dep_Prm_Amt").ToString()).ToString("#,##0") %>'></asp:Label>
                                        </td>
                                        <td nowrap style="font-size: xx-small">
                                            <%#Eval("Audit_Reslt_Amt") == null ? String.Empty : decimal.Parse(Eval("Audit_Reslt_Amt").ToString()).ToString("#,##0")%>
                                        </td>
                                        <td>
                                            <asp:Label Font-Size="XX-Small" ID="lblExpAmt" runat="server" Text='<%#Eval("ExposureAmt")== null?String.Empty:decimal.Parse(Eval("ExposureAmt").ToString()).ToString("#,##0") %>'></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label Font-Size="XX-Small" ID="lblRevise" runat="server" Text='<%#Eval("Aud_Rev_Status") == null ? "No" : (Eval("Aud_Rev_Status").ToString()=="True"?"Yes":"No")%>'></asp:Label>
                                        </td>
                                        <td>
                                            <asp:LinkButton Font-Size="XX-Small" ID="lnkRevise" CommandArgument='<%# Bind("Comm_Agr_Audit_ID")%>'
                                                CommandName="Revise" runat="server" Text="Revise" Enabled=' <%# Eval("Pol_Ind")!=null && Eval("Pol_Ind").ToString()=="False"?false:(Eval("AdjustmentIndicator")==null)?false:(Eval("AdjustmentIndicator").ToString() =="False"?false:((Eval("Aud_Rev_Status")== null || Eval("Aud_Rev_Status").ToString()=="False")?true:false))%>'></asp:LinkButton>
                                        </td>
                                    </tr>
                                </ItemTemplate>
                                <AlternatingItemTemplate>
                                    <tr class="AlternatingItemTemplate" id="trItemTemplate" runat="server">
                                        <td align="left">
                                            <asp:LinkButton Font-Size="XX-Small" ID="lnkEdit" runat="server" Enabled='<%# Eval("Pol_Ind")!=null && Eval("Pol_Ind").ToString()=="False"?false:(Eval("AdjustmentIndicator")==null?true:(Eval("AdjustmentIndicator").ToString()== "False"?true:false)) %>'
                                                CommandName="Edit" Text="EDIT"></asp:LinkButton>
                                        </td>
                                        <td align="center">
                                            <asp:Label Visible="false" Font-Size="XX-Small" ID="lblComAgrID" runat="server" Text='<%# Bind("Comm_Agr_ID") %>'></asp:Label>
                                            <%# Eval("POLICY") %>
                                        </td>
                                        <td align="center">
                                            <asp:Label ID="lblStartDate" Font-Size="XX-Small" runat="server" Text='<%# Bind("StartDate","{0:d}")%>'></asp:Label>
                                        </td>
                                        <td>
                                            <asp:LinkButton Font-Size="XX-Small" ID="lnkSubPrmAmt" runat="server" CommandArgument='<%# Bind("Comm_Agr_Audit_ID")%>'
                                                CommandName="Subject" Text='<%#Eval("Sub_Aud_Prm_Amt")== null?String.Empty:decimal.Parse(Eval("Sub_Aud_Prm_Amt").ToString()).ToString("#,##0") %>'></asp:LinkButton>
                                        </td>
                                        <td>
                                            <asp:LinkButton Font-Size="XX-Small" ID="lnkNonSubPrmAmt" runat="server" CommandArgument='<%# Bind("Comm_Agr_Audit_ID")%>'
                                                CommandName="NonSubject" Text='<%#Eval("Non_Sub_Aud_Prm_Amt")== null?String.Empty:decimal.Parse(Eval("Non_Sub_Aud_Prm_Amt").ToString()).ToString("#,##0") %>'></asp:LinkButton>
                                        </td>
                                        <td>
                                            <asp:Label Font-Size="XX-Small" ID="lblDefdepAmt" runat="server" Text='<%#Eval("Def_Dep_prm_Amt")== null?String.Empty:decimal.Parse(Eval("Def_Dep_prm_Amt").ToString()).ToString("#,##0") %>'></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label Font-Size="XX-Small" ID="lblSubAmt" runat="server" Text='<%#Eval("Sub_Dep_Prm_Amt")== null?String.Empty:decimal.Parse(Eval("Sub_Dep_Prm_Amt").ToString()).ToString("#,##0") %>'></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label Font-Size="XX-Small" ID="lblNonsubAmt" runat="server" Text='<%#Eval("Non_Sub_Dep_Prm_Amt")== null?String.Empty:decimal.Parse(Eval("Non_Sub_Dep_Prm_Amt").ToString()).ToString("#,##0") %>'></asp:Label>
                                        </td>
                                        <td nowrap style="font-size: xx-small">
                                            <%#Eval("Audit_Reslt_Amt") == null ? String.Empty : decimal.Parse(Eval("Audit_Reslt_Amt").ToString()).ToString("#,##0")%>
                                        </td>
                                        <td>
                                            <asp:Label Font-Size="XX-Small" ID="lblExpAmt" runat="server" Text='<%#Eval("ExposureAmt")== null?String.Empty:decimal.Parse(Eval("ExposureAmt").ToString()).ToString("#,##0") %>'></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label Font-Size="XX-Small" ID="lblRevise" runat="server" Text='<%#Eval("Aud_Rev_Status") == null ? "No" : (Eval("Aud_Rev_Status").ToString()=="True"?"Yes":"No")%>'></asp:Label>
                                        </td>
                                        <td>
                                            <asp:LinkButton Font-Size="XX-Small" ID="lnkRevise" CommandArgument='<%# Bind("Comm_Agr_Audit_ID")%>'
                                                CommandName="Revise" runat="server" Text="Revise" Enabled=' <%# Eval("Pol_Ind")!=null && Eval("Pol_Ind").ToString()=="False"?false:(Eval("AdjustmentIndicator")==null)?false:(Eval("AdjustmentIndicator").ToString() =="False"?false:((Eval("Aud_Rev_Status")== null || Eval("Aud_Rev_Status").ToString()=="False")?true:false))%>'></asp:LinkButton>
                                        </td>
                                    </tr>
                                </AlternatingItemTemplate>
                                <EditItemTemplate>
                                    <tr class="ItemTemplate">
                                        <td align="left">
                                            <asp:HiddenField ID="hidAdjustIndicator" runat="server" Value='<%# Bind("AdjustmentIndicator") %>' />
                                            <asp:LinkButton Font-Size="XX-Small" ID="lnkupdate" ValidationGroup="Update" runat="server"
                                                CommandName="Update" Text="UPDATE"></asp:LinkButton>
                                            <asp:LinkButton Font-Size="XX-Small" ID="lnkCancel" runat="server" CommandName="Cancel"
                                                Text="CANCEL"></asp:LinkButton>
                                        </td>
                                        <td align="left">
                                            <asp:HiddenField ID="hidCommAgrAuditID" runat="server" Value=' <%# Bind("Comm_Agr_Audit_ID")%>' />
                                            <asp:Label Font-Size="XX-Small" Visible="true" ID="lblComlPolicy" runat="server"
                                                Text='<%# Bind("POLICY") %>'></asp:Label>
                                            <asp:Label Visible="false" ID="lblComAgrIDEdt" runat="server" Text='<%# Bind("Comm_Agr_ID") %>'></asp:Label>
                                            <asp:DropDownList Font-Size="XX-Small" ID="ddlPolicyEdt" runat="server" Visible="false"
                                                ValidationGroup="Update">
                                            </asp:DropDownList>
                                            <asp:CompareValidator ID="compPolicy" runat="server" ControlToValidate="ddlPolicyEdt"
                                                ValidationGroup="Update" ValueToCompare="0" Text="*" ErrorMessage="Please select Policy"
                                                Operator="NotEqual"></asp:CompareValidator>
                                        </td>
                                        <td align="left" nowarp>
                                            <asp:TextBox Font-Size="XX-Small" ID="txtAuditDate" Text='<%# Bind("StartDate","{0:d}")%>'
                                                ValidationGroup="Update" runat="server" Width="70"></asp:TextBox>
                                            <asp:ImageButton ID="imgDtrecieved" runat="server" ImageUrl="~/images/Calendar_scheduleHS.png"
                                                CausesValidation="False" />
                                            <AjaxToolKit:CalendarExtender ID="calAuditDate" runat="server" TargetControlID="txtAuditDate"
                                                PopupButtonID="imgDtrecieved" />
                                            <asp:RequiredFieldValidator ID="reqAuditDate" runat="server" Text="*" ErrorMessage="Please enter Audit date"
                                                ValidationGroup="Update" ControlToValidate="txtAuditDate"></asp:RequiredFieldValidator>
                                            <AjaxToolKit:MaskedEditExtender ID="mskAuditDate" runat="server" TargetControlID="txtAuditDate"
                                                Mask="99/99/9999" MessageValidatorTip="true" OnFocusCssClass="MaskedEditFocus"
                                                OnInvalidCssClass="MaskedEditError" MaskType="Date" DisplayMoney="Left" AcceptNegative="Left"
                                                ErrorTooltipEnabled="True" />
                                            <asp:RegularExpressionValidator ID="regtxtAuditDate" runat="server" ControlToValidate="txtAuditDate"
                                                ValidationExpression="(^(?:(?:(?:0?[13578]|1[02])(\/|-)31)|(?:(?:0?[1,3-9]|1[0-2])(\/|-)(?:29|30)))(\/|-)(?:(1[8-9]|[2-8]\d)\d\d)$|^(?:(?:0?[1-9]|1[0-2])(\/|-)(?:0?[1-9]|1\d|2[0-8]))(\/|-)(?:(1[8-9]|[2-8]\d)\d\d)$|^(0?2(\/|-)29)(\/|-)(?:(?:0[48]00|[13579][26]00|[2468][048]00)|(?:\d\d)?(?:0[48]|[2468][048]|[13579][26]))$)"
                                                ErrorMessage="Invalid Date" Text="*" ValidationGroup="Update"></asp:RegularExpressionValidator>
                                        </td>
                                        <td>
                                            <asp:Label Font-Size="XX-Small" ID="lblSubAudit" runat="server" Text='<%# Bind("Sub_Aud_Prm_Amt","{0:0}")%>'></asp:Label>
                                        </td>
                                        <td align="left">
                                            <asp:Label Font-Size="XX-Small" ID="lblNonSubAudit" runat="server" Text='<%# Bind("Non_Sub_Aud_Prm_Amt","{0:0}")%>'></asp:Label>
                                        </td>
                                        <td align="left">
                                            <asp:AISAmountTextbox Font-Size="XX-Small" ID="txtdefdepprem" Text=' <%# Bind("Def_Dep_prm_Amt","{0:0}")%>'
                                                Width="78px" runat="server"></asp:AISAmountTextbox>
                                            <%-- <AjaxToolKit:FilteredTextBoxExtender runat="server" TargetControlID="txtdefdepprem"
                                                FilterType="Custom" ValidChars="0123456789," ID="fltAmount">
                                            </AjaxToolKit:FilteredTextBoxExtender>--%>
                                            <%--<AjaxToolKit:FilteredTextBoxExtender ID="Fltrdefdepprem" runat="server" TargetControlID="txtdefdepprem"
                                                FilterType="Numbers">
                                            </AjaxToolKit:FilteredTextBoxExtender>--%>
                                        </td>
                                        <td align="left">
                                            <asp:AISAmountTextbox Font-Size="XX-Small" ID="txtsubdepPrem" Text=' <%# Bind("Sub_Dep_Prm_Amt","{0:0}")%>'
                                                Width="78px" runat="server"></asp:AISAmountTextbox>
                                            <%--<AjaxToolKit:FilteredTextBoxExtender runat="server" TargetControlID="txtsubdepPrem"
                                                FilterType="Custom" ValidChars="0123456789," ID="FilteredTextBoxExtender6">
                                            </AjaxToolKit:FilteredTextBoxExtender>--%>
                                        </td>
                                        <td align="left">
                                            <asp:AISAmountTextbox Font-Size="XX-Small" ID="txtNSubDepPrem" Text=' <%# Bind("Non_Sub_Dep_Prm_Amt","{0:0}")%>'
                                                Width="78px" runat="server"></asp:AISAmountTextbox>
                                            <%--<AjaxToolKit:FilteredTextBoxExtender runat="server" TargetControlID="txtNSubDepPrem"
                                                FilterType="Custom" ValidChars="0123456789," ID="FilteredTextBoxExtender5">
                                            </AjaxToolKit:FilteredTextBoxExtender>--%>
                                        </td>
                                        <td align="left" nowrap>
                                            <asp:Label Font-Size="XX-Small" ID="lblAuditResult" runat="server" Text=' <%# Bind("Audit_Reslt_Amt","{0:0}")%>'></asp:Label>
                                        </td>
                                        <td align="left">
                                            <asp:AISAmountTextbox Font-Size="XX-Small" ID="txtExpAmt" Text=' <%# Bind("ExposureAmt","{0:0}")%>'
                                                Width="78px" runat="server"></asp:AISAmountTextbox>
                                            <%--<AjaxToolKit:FilteredTextBoxExtender runat="server" TargetControlID="txtExpAmt" FilterType="Custom"
                                                ValidChars="0123456789," ID="FilteredTextBoxExtender4">
                                            </AjaxToolKit:FilteredTextBoxExtender>--%>
                                            <asp:RequiredFieldValidator ID="reqtxtExpAmt" runat="server" Text="*" ErrorMessage="Please enter Exposure amount"
                                                ValidationGroup="Update" ControlToValidate="txtExpAmt"></asp:RequiredFieldValidator>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblRevise" Font-Size="XX-Small" runat="server" Visible="false" Text='<%#Bind("Aud_Rev_Status")%>'></asp:Label>
                                        </td>
                                        <td>
                                        </td>
                                    </tr>
                                </EditItemTemplate>
                                <InsertItemTemplate>
                                    <tr class="ItemTemplate" runat="server" id="trItemTemplate">
                                        <td align="left">
                                            <asp:LinkButton Font-Size="XX-Small" ID="lnkSave" runat="server" ValidationGroup="Save"
                                                CommandName="Save" Text="SAVE"></asp:LinkButton>
                                            <AjaxToolKit:MaskedEditExtender ID="mskAuditDate" runat="server" TargetControlID="txtAuditDate"
                                                Mask="99/99/9999" MessageValidatorTip="true" OnFocusCssClass="MaskedEditFocus"
                                                OnInvalidCssClass="MaskedEditError" MaskType="Date" DisplayMoney="Left" AcceptNegative="Left"
                                                ErrorTooltipEnabled="True" />
                                        </td>
                                        <td align="left" width="110px">
                                            <asp:DropDownList Font-Size="XX-Small" ID="ddlPolicy" runat="server" ValidationGroup="Save">
                                            </asp:DropDownList>
                                            <asp:CompareValidator ID="compPolicy" runat="server" ControlToValidate="ddlPolicy"
                                                ValidationGroup="Save" ValueToCompare="0" Text="*" ErrorMessage="Please select Policy"
                                                Operator="NotEqual"></asp:CompareValidator>
                                        </td>
                                        <td align="left" width="100px" nowrap>
                                            <asp:TextBox Font-Size="XX-Small" ID="txtAuditDate" runat="server" Width="70px" ValidationGroup="Save"></asp:TextBox>
                                            <asp:ImageButton ID="imgDtrecieved" runat="server" ImageUrl="~/images/Calendar_scheduleHS.png"
                                                CausesValidation="False" />
                                            <AjaxToolKit:CalendarExtender ID="calAuditDate" runat="server" TargetControlID="txtAuditDate"
                                                PopupButtonID="imgDtrecieved" />
                                            <asp:RequiredFieldValidator ID="reqAuditDate" runat="server" Text="*" ErrorMessage="Please enter Audit Date"
                                                ValidationGroup="Save" ControlToValidate="txtAuditDate"></asp:RequiredFieldValidator>
                                            <asp:RegularExpressionValidator ID="regtxtAuditDate" runat="server" ControlToValidate="txtAuditDate"
                                                ValidationExpression="(^(?:(?:(?:0?[13578]|1[02])(\/|-)31)|(?:(?:0?[1,3-9]|1[0-2])(\/|-)(?:29|30)))(\/|-)(?:(1[8-9]|[2-8]\d)\d\d)$|^(?:(?:0?[1-9]|1[0-2])(\/|-)(?:0?[1-9]|1\d|2[0-8]))(\/|-)(?:(1[8-9]|[2-8]\d)\d\d)$|^(0?2(\/|-)29)(\/|-)(?:(?:0[48]00|[13579][26]00|[2468][048]00)|(?:\d\d)?(?:0[48]|[2468][048]|[13579][26]))$)"
                                                ErrorMessage="Invalid Date" Text="*" ValidationGroup="Save"></asp:RegularExpressionValidator>
                                        </td>
                                        <td>
                                            <a>0</a>
                                        </td>
                                        <td>
                                            <a>0</a>
                                        </td>
                                        <td align="left">
                                            <asp:AISAmountTextbox Font-Size="XX-Small" ID="txtdefdepprem" Width="78px" runat="server"></asp:AISAmountTextbox>
                                            <%--<AjaxToolKit:FilteredTextBoxExtender runat="server" TargetControlID="txtdefdepprem"
                                                FilterType="Custom" ValidChars="0123456789," ID="FilteredTextBoxExtender3">
                                            </AjaxToolKit:FilteredTextBoxExtender>--%>
                                        </td>
                                        <td align="left">
                                            <asp:AISAmountTextbox Font-Size="XX-Small" ID="txtsubdepPrem" Width="78px" runat="server"></asp:AISAmountTextbox>
                                            <%--<AjaxToolKit:FilteredTextBoxExtender runat="server" TargetControlID="txtsubdepPrem"
                                                FilterType="Custom" ValidChars="0123456789," ID="FilteredTextBoxExtender2">
                                            </AjaxToolKit:FilteredTextBoxExtender>--%>
                                            <%--<AjaxToolKit:FilteredTextBoxExtender ID="fltrsubdepPrem" runat="server" TargetControlID="txtsubdepPrem"
                                                FilterType="Numbers">
                                            </AjaxToolKit:FilteredTextBoxExtender>--%>
                                        </td>
                                        <td align="left">
                                            <asp:AISAmountTextbox Font-Size="XX-Small" ID="txtNSubDepPrem" Width="78px" runat="server"></asp:AISAmountTextbox>
                                            <%-- <AjaxToolKit:FilteredTextBoxExtender runat="server" TargetControlID="txtNSubDepPrem"
                                                FilterType="Custom" ValidChars="0123456789," ID="FilteredTextBoxExtender1">
                                            </AjaxToolKit:FilteredTextBoxExtender>--%>
                                            <%--<AjaxToolKit:FilteredTextBoxExtender ID="fltrNSubDepPrem" runat="server" TargetControlID="txtNSubDepPrem"
                                                FilterType="Numbers">
                                            </AjaxToolKit:FilteredTextBoxExtender>--%>
                                        </td>
                                        <td align="left" nowrap>
                                            <asp:Label ID="lblAuditResult" runat="server"></asp:Label>
                                        </td>
                                        <td align="left">
                                            <asp:AISAmountTextbox Font-Size="XX-Small" ID="txtExpAmt" Width="78px" runat="server"
                                                ValidationGroup="Save"></asp:AISAmountTextbox>
                                            <%--<AjaxToolKit:FilteredTextBoxExtender runat="server" TargetControlID="txtExpAmt" FilterType="Custom"
                                                ValidChars="0123456789," ID="fltAmount">
                                            </AjaxToolKit:FilteredTextBoxExtender>--%>
                                            <%--<AjaxToolKit:FilteredTextBoxExtender ID="fltrExpAmt" runat="server" TargetControlID="txtExpAmt"
                                                FilterType="Numbers">
                                            </AjaxToolKit:FilteredTextBoxExtender>--%>
                                            <asp:RequiredFieldValidator ID="reqtxtExpAmt" runat="server" Text="*" ErrorMessage="Please enter Exposure Amount"
                                                ValidationGroup="Save" ControlToValidate="txtExpAmt"></asp:RequiredFieldValidator>
                                        </td>
                                        <td>
                                        </td>
                                        <td>
                                        </td>
                                    </tr>
                                </InsertItemTemplate>
                            </asp:AISListView>
                        </asp:Panel>
                        <asp:Label ID="lblAuditPremium" CssClass="h2" runat="server" Text=""></asp:Label>
                        &nbsp;<asp:LinkButton ID="lnkAuditPremium" runat="server" Visible="false" Text="Close"
                            OnClick="CloseAuditinfo"></asp:LinkButton>
                        <asp:Panel ID="pnlDetails" runat="server" Height="100px" Visible="false" ScrollBars="Auto"
                            Width="670px">
                            <asp:AISListView ID="lstSubject" OnItemDataBound="DataBoundList" InsertItemPosition="FirstItem"
                                runat="server" DataKeyNames="Sub_Prem_Aud_ID" OnItemCommand="CommandList" OnItemEditing="EditList"
                                OnItemUpdating="UpdateSubjectList" OnItemCanceling="CancelList">
                                <LayoutTemplate>
                                    <table id="Table1" class="panelContents" runat="server" width="650px">
                                        <tr class="LayoutTemplate">
                                            <th>
                                            </th>
                                            <th style="width: 250px">
                                                State
                                            </th>
                                            <th style="width: 150px">
                                                Premium Amount
                                            </th>
                                            <th style="width: 150px">
                                                Disable
                                            </th>
                                        </tr>
                                        <tr id="itemPlaceholder" runat="server">
                                        </tr>
                                    </table>
                                </LayoutTemplate>
                                <ItemTemplate>
                                    <tr class="ItemTemplate">
                                        <td align="left">
                                            <asp:LinkButton ID="lnkEdit" runat="server" CommandName="Edit" Text="EDIT"></asp:LinkButton>
                                        </td>
                                        <td>
                                            <%# Eval("STATE")%>
                                        </td>
                                        <td>
                                            <%# (Eval("Active") != null && Eval("Active").ToString() == "True") ? GetPremiumAmt(decimal.Parse(Eval("Prem_Amt").ToString()), true).ToString("#,##0") : GetPremiumAmt(decimal.Parse(Eval("Prem_Amt").ToString()), false).ToString("#,##0")%>
                                        </td>
                                        <td>
                                            <asp:HiddenField ID="hidActive" runat="server" Value='<%# Bind("Active")%>' />
                                            <asp:ImageButton ID="imgDisable" CommandArgument='<%# Bind("Sub_Prem_Aud_ID") %>'
                                                CommandName="ISActive" runat="server" ImageUrl='<%# Eval("Active").ToString()=="True"?"~/images/disabled.GIF":"~/images/enabled.GIF" %>'>
                                            </asp:ImageButton>
                                        </td>
                                    </tr>
                                </ItemTemplate>
                                <AlternatingItemTemplate>
                                    <tr class="AlternatingItemTemplate">
                                        <td align="left">
                                            <asp:LinkButton ID="lnkEdit" runat="server" CommandName="Edit" Text="EDIT"></asp:LinkButton>
                                        </td>
                                        <td>
                                            <%# Eval("STATE")%>
                                        </td>
                                        <td>
                                            <%# (Eval("Active") != null && Eval("Active").ToString() == "True") ? GetPremiumAmt(decimal.Parse(Eval("Prem_Amt").ToString()), true).ToString("#,##0") : GetPremiumAmt(decimal.Parse(Eval("Prem_Amt").ToString()), false).ToString("#,##0")%>
                                        </td>
                                        <td>
                                            <asp:HiddenField ID="hidActive" runat="server" Value='<%# Bind("Active")%>' />
                                            <asp:ImageButton ID="imgDisable" CommandArgument='<%# Bind("Sub_Prem_Aud_ID") %>'
                                                CommandName="ISActive" runat="server" ImageUrl='<%# Eval("Active").ToString()=="True"?"~/images/disabled.GIF":"~/images/enabled.GIF" %>'>
                                            </asp:ImageButton>
                                        </td>
                                    </tr>
                                </AlternatingItemTemplate>
                                <EditItemTemplate>
                                    <tr class="ItemTemplate">
                                        <td align="left">
                                            <asp:LinkButton ValidationGroup="subjUpdate" ID="lnkupdate" runat="server" CommandName="Update"
                                                Text="UPDATE"></asp:LinkButton>
                                            <asp:LinkButton ID="lnkCancel" runat="server" CommandName="Cancel" Text="CANCEL"></asp:LinkButton>
                                        </td>
                                        <td>
                                            <asp:HiddenField ID="hidSub" runat="server" Value='<%# Bind("Sub_Prem_Aud_ID") %>' />
                                            <asp:HiddenField ID="hidStateID" runat="server" Value='<%# Bind("StateID") %>' />
                                            <asp:HiddenField ID="hidState" runat="server" Value='<%# Bind("STATE") %>' />
                                            <asp:DropDownList ID="ddlState" runat="server" ValidationGroup="subjUpdate">
                                            </asp:DropDownList>
                                            <asp:CompareValidator ID="reqddlState" runat="server" ControlToValidate="ddlState"
                                                ValidationGroup="subjUpdate" ValueToCompare="0" ErrorMessage="Please select State"
                                                Text="*" Operator="NotEqual"></asp:CompareValidator>
                                        </td>
                                        <td>
                                            <asp:AISAmountTextbox ValidationGroup="subjUpdate" ID="txtPremiumAmount" runat="server"
                                                Text='<%# Bind("Prem_Amt","{0:0}") %>'></asp:AISAmountTextbox>
                                            <asp:RequiredFieldValidator ID="reqtxtPremiumAmount" runat="server" Text="*" ErrorMessage="Please enter Premium amount"
                                                ValidationGroup="subjUpdate" ControlToValidate="txtPremiumAmount"></asp:RequiredFieldValidator>
                                            <%--<AjaxToolKit:FilteredTextBoxExtender runat="server" TargetControlID="txtPremiumAmount"
                                                FilterType="Custom" ValidChars="0123456789," ID="fltAmount">
                                            </AjaxToolKit:FilteredTextBoxExtender>--%>
                                            <%--<AjaxToolKit:FilteredTextBoxExtender ID="fltrtxtPremiumAmount" runat="server" TargetControlID="txtPremiumAmount"
                                                ValidChars="0123456789" FilterMode="ValidChars">
                                            </AjaxToolKit:FilteredTextBoxExtender>--%>
                                        </td>
                                        <td>
                                        </td>
                                    </tr>
                                </EditItemTemplate>
                                <InsertItemTemplate>
                                    <tr class="ItemTemplate" runat="server" id="trItemTemplate">
                                        <td align="left">
                                            <asp:LinkButton ID="lnkSave" ValidationGroup="subjSave" runat="server" CommandName="Save"
                                                Text="SAVE"></asp:LinkButton>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlState" runat="server" ValidationGroup="subjSave">
                                            </asp:DropDownList>
                                            <asp:CompareValidator ID="reqddlState" runat="server" ControlToValidate="ddlState"
                                                ValidationGroup="subjSave" ValueToCompare="0" ErrorMessage="Please select State"
                                                Text="*" Operator="NotEqual"></asp:CompareValidator>
                                        </td>
                                        <td>
                                            <asp:AISAmountTextbox ID="txtPremiumAmount" ValidationGroup="subjSave" runat="server"></asp:AISAmountTextbox>
                                            <asp:RequiredFieldValidator ID="reqtxtPremiumAmount" runat="server" Text="*" ErrorMessage="Please enter Premium amount"
                                                ValidationGroup="subjSave" ControlToValidate="txtPremiumAmount"></asp:RequiredFieldValidator>
                                            <%--<AjaxToolKit:FilteredTextBoxExtender runat="server" TargetControlID="txtPremiumAmount"
                                                FilterType="Custom" ValidChars="0123456789," ID="fltAmount">
                                            </AjaxToolKit:FilteredTextBoxExtender>--%>
                                            <%--<AjaxToolKit:FilteredTextBoxExtender ID="fltrtxtPremiumAmount" runat="server" TargetControlID="txtPremiumAmount"
                                                ValidChars="0123456789" FilterMode="ValidChars">
                                            </AjaxToolKit:FilteredTextBoxExtender>--%>
                                        </td>
                                        <td>
                                        </td>
                                    </tr>
                                </InsertItemTemplate>
                            </asp:AISListView>
                            <asp:AISListView ID="lstNonSubject" OnItemDataBound="DataBoundList" InsertItemPosition="FirstItem"
                                runat="server" DataKeyNames="N_Sub_Prem_Aud_ID" OnItemCommand="CommandList" OnItemEditing="EditList"
                                OnItemUpdating="UpdateNonSubjectList" OnItemCanceling="CancelList">
                                <LayoutTemplate>
                                    <table id="Table1" class="panelContents" runat="server" width="650px">
                                        <tr class="LayoutTemplate">
                                            <th>
                                            </th>
                                            <th style="width: 250px">
                                                Type
                                            </th>
                                            <th style="width: 150px">
                                                Premium Amount
                                            </th>
                                            <th style="width: 150px">
                                                Disable
                                            </th>
                                        </tr>
                                        <tr id="itemPlaceholder" runat="server">
                                        </tr>
                                        <%-- <tfoot>
                                            <tr class="ItemTemplate">
                                                <td>
                                                </td>
                                                <td>
                                                    <b>Total</b>
                                                </td>
                                                <td>
                                                    <asp:Label Font-Bold="true" ID="lblTotalSubPremAmt" runat="server"></asp:Label>
                                                </td>
                                                <td>
                                                </td>
                                            </tr>
                                        </tfoot>--%>
                                    </table>
                                </LayoutTemplate>
                                <ItemTemplate>
                                    <tr class="ItemTemplate">
                                        <td>
                                            <asp:LinkButton ID="lnkEdit" runat="server" CommandName="Edit" Text="EDIT"></asp:LinkButton>
                                        </td>
                                        <td>
                                            <%# Eval("NSATYPE")%>
                                        </td>
                                        <td>
                                            <%# (Eval("Active") != null && Eval("Active").ToString() == "True") ? GetPremiumAmt(decimal.Parse(Eval("Non_Subj_Audt_Prem_Amt").ToString()), true).ToString("#,##0") : GetPremiumAmt(decimal.Parse(Eval("Non_Subj_Audt_Prem_Amt").ToString()), false).ToString("#,##0")%>
                                        </td>
                                        <td>
                                            <asp:HiddenField ID="hidActive" runat="server" Value='<%# Bind("Active")%>' />
                                            <asp:ImageButton ID="imgDisable" CommandArgument='<%# Bind("N_Sub_Prem_Aud_ID") %>'
                                                CommandName="ISActive" runat="server" ImageUrl='<%# Eval("Active").ToString()=="True"?"~/images/disabled.GIF":"~/images/enabled.GIF" %>'>
                                            </asp:ImageButton>
                                        </td>
                                    </tr>
                                </ItemTemplate>
                                <AlternatingItemTemplate>
                                    <tr class="AlternatingItemTemplate">
                                        <td>
                                            <asp:LinkButton ID="lnkEdit" runat="server" CommandName="Edit" Text="EDIT"></asp:LinkButton>
                                        </td>
                                        <td>
                                            <%# Eval("NSATYPE")%>
                                        </td>
                                        <td>
                                            <%# (Eval("Active") != null && Eval("Active").ToString() == "True") ? GetPremiumAmt(decimal.Parse(Eval("Non_Subj_Audt_Prem_Amt").ToString()), true).ToString("#,##0") : GetPremiumAmt(decimal.Parse(Eval("Non_Subj_Audt_Prem_Amt").ToString()), false).ToString("#,##0")%>
                                        </td>
                                        <td>
                                            <asp:HiddenField ID="hidActive" runat="server" Value='<%# Bind("Active")%>' />
                                            <asp:ImageButton ID="imgDisable" CommandArgument='<%# Bind("N_Sub_Prem_Aud_ID") %>'
                                                CommandName="ISActive" runat="server" ImageUrl='<%# Eval("Active").ToString()=="True"?"~/images/disabled.GIF":"~/images/enabled.GIF" %>'>
                                            </asp:ImageButton>
                                        </td>
                                    </tr>
                                </AlternatingItemTemplate>
                                <EditItemTemplate>
                                    <tr class="ItemTemplate">
                                        <td>
                                            <asp:LinkButton ValidationGroup="NsubjUpdate" ID="lnkupdate" runat="server" CommandName="Update"
                                                Text="UPDATE"></asp:LinkButton>
                                            <asp:LinkButton ID="lnkCancel" runat="server" CommandName="Cancel" Text="CANCEL"></asp:LinkButton>
                                        </td>
                                        <td>
                                            <asp:HiddenField ID="hidNonSub" runat="server" Value='<%# Bind("N_Sub_Prem_Aud_ID") %>' />
                                            <asp:HiddenField ID="hidNSATypID" runat="server" Value='<%# Bind("Nsa_Typ_ID") %>' />
                                            <asp:HiddenField ID="hidNSATYPE" runat="server" Value='<%# Bind("NSATYPE")%>' />
                                            <asp:DropDownList ID="ddlNonSub" runat="server" ValidationGroup="NsubjUpdate">
                                            </asp:DropDownList>
                                            <asp:CompareValidator ID="reqddlNonSub" runat="server" ControlToValidate="ddlNonSub"
                                                ValidationGroup="NsubjUpdate" ValueToCompare="0" ErrorMessage="Please select Type"
                                                Text="*" Operator="NotEqual"></asp:CompareValidator>
                                        </td>
                                        <td>
                                            <asp:AISAmountTextbox ValidationGroup="NsubjUpdate" ID="txtPremiumAmount" runat="server"
                                                Text='<%# Bind("Non_Subj_Audt_Prem_Amt","{0:0}") %>'></asp:AISAmountTextbox>
                                            <asp:RequiredFieldValidator ID="reqtxtPremiumAmount" runat="server" Text="*" ErrorMessage="Please enter Premium amount"
                                                ValidationGroup="NsubjUpdate" ControlToValidate="txtPremiumAmount"></asp:RequiredFieldValidator>
                                            <%--<AjaxToolKit:FilteredTextBoxExtender runat="server" TargetControlID="txtPremiumAmount"
                                                FilterType="Custom" ValidChars="0123456789," ID="fltAmount">
                                            </AjaxToolKit:FilteredTextBoxExtender>--%>
                                            <%--<AjaxToolKit:FilteredTextBoxExtender ID="fltrtxtPremiumAmount" runat="server" TargetControlID="txtPremiumAmount"
                                                ValidChars="0123456789" FilterMode="ValidChars">
                                            </AjaxToolKit:FilteredTextBoxExtender>--%>
                                        </td>
                                        <td>
                                        </td>
                                    </tr>
                                </EditItemTemplate>
                                <InsertItemTemplate>
                                    <tr class="ItemTemplate" runat="server" id="trItemTemplate">
                                        <td align="left">
                                            <asp:LinkButton ValidationGroup="NsubjSave" ID="lnkSave" runat="server" CommandName="Save"
                                                Text="SAVE"></asp:LinkButton>
                                        </td>
                                        <td>
                                            <asp:DropDownList Width="250px" ID="ddlNonSub" runat="server" ValidationGroup="NsubjSave">
                                            </asp:DropDownList>
                                            <asp:CompareValidator ID="reqddlNonSub" runat="server" ControlToValidate="ddlNonSub"
                                                ValidationGroup="NsubjSave" ValueToCompare="0" ErrorMessage="Please select Type"
                                                Text="*" Operator="NotEqual"></asp:CompareValidator>
                                        </td>
                                        <td>
                                            <asp:AISAmountTextbox ValidationGroup="NsubjSave" ID="txtPremiumAmount" runat="server"
                                                Text='<%# Bind("PremiumAmount") %>'></asp:AISAmountTextbox>
                                            <asp:RequiredFieldValidator ID="reqtxtPremiumAmount" runat="server" Text="*" ErrorMessage="Please enter Premium amount"
                                                ValidationGroup="NsubjSave" ControlToValidate="txtPremiumAmount"></asp:RequiredFieldValidator>
                                            <%--<AjaxToolKit:FilteredTextBoxExtender runat="server" TargetControlID="txtPremiumAmount"
                                                FilterType="Custom" ValidChars="0123456789," ID="fltAmount">
                                            </AjaxToolKit:FilteredTextBoxExtender>--%>
                                            <%--<AjaxToolKit:FilteredTextBoxExtender ID="fltrtxtPremiumAmount" runat="server" TargetControlID="txtPremiumAmount"
                                                ValidChars="0123456789" FilterMode="ValidChars">
                                            </AjaxToolKit:FilteredTextBoxExtender>--%>
                                        </td>
                                        <td>
                                        </td>
                                    </tr>
                                </InsertItemTemplate>
                            </asp:AISListView>

                            <script runat="server">
                                decimal TotalPremiumAmt;
                                //Code for Calculating Total for Subject Audit Premium Amount/Subject Audit Premium Amount
                                decimal GetPremiumAmt(decimal PremiumAmt, bool Flag)
                                {
                                    if (Flag)
                                        TotalPremiumAmt += PremiumAmt;
                                    else
                                        TotalPremiumAmt += 0;
                                    lblTotalSubPremAmt.Text = decimal.Parse(TotalPremiumAmt.ToString()).ToString("#,##0");
                                    return PremiumAmt;
                                }
                                decimal GetTotal()
                                {
                                    return TotalPremiumAmt;
                                }
                            </script>

                        </asp:Panel>
                        <table width="650px" runat="server" id="tblTotal" visible="false">
                            <tr class="LayoutTemplate">
                                <td>
                                </td>
                                <td style="width: 250px" align="center">
                                    <b>Total</b>
                                </td>
                                <td style="width: 150px">
                                    <asp:Label Font-Bold="true" Text="0" ID="lblTotalSubPremAmt" runat="server"></asp:Label>
                                </td>
                                <td style="width: 150px">
                                </td>
                            </tr>
                        </table>
                    </ContentTemplate>
                </AjaxToolKit:TabPanel>
            </AjaxToolKit:TabContainer>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
