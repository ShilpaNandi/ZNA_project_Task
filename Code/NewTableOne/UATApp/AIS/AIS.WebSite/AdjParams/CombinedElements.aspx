<%@ Page Language="C#" MasterPageFile="~/Retro.master" AutoEventWireup="true" Inherits="AdjParameters_CombinedElements"
    CodeBehind="CombinedElements.aspx.cs" %>

<%@ Register Src="~/App_Shared/AccountInfoHeader.ascx" TagName="MasterValues" TagPrefix="MV" %>
<%@ Register Src="~/App_Shared/ProgramPeriod.ascx" TagName="ProgramPeriod" TagPrefix="PP" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Src="~/App_Shared/SaveCancel.ascx" TagPrefix="uc1" TagName="Sc" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeaderPlaceHolder" runat="Server">
    <table width="910" style="text-align: right">
        <tr>
            <td align="left">
                <asp:Label ID="GAILabel" runat="server" Text="Combined Elements" CssClass="h1"></asp:Label>
            </td>
        </tr>
    </table>
</asp:Content>
<asp:Content ID="ContMaincontent" ContentPlaceHolderID="MainPlaceHolder" runat="Server">

    <script type="text/javascript">

        var scrollTop1;

        if (Sys.WebForms.PageRequestManager != null) {
            Sys.WebForms.PageRequestManager.getInstance().add_beginRequest(BeginRequestHandler);
            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);
        }

        function BeginRequestHandler(sender, args) {
            var mef = $get('<%=divCombinedelements.ClientID%>');
            if (mef != null)
                scrollTop1 = mef.scrollTop;
        }

        function EndRequestHandler(sender, args) {
            var mef = $get('<%=divCombinedelements.ClientID%>');
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
                var ctrl = $find('<%=tabcontCombinedElements.ClientID%>');
                ctrl.set_activeTab(ctrl.get_tabs()[2]);
                __doPostBack('<%=tabcontCombinedElements.ClientID %>', ctrl.get_activeTab().get_headerText());
            }

            if (proceed) {
                if (Pagename == "AI") {
                    strURL += "AuditInfo.aspx";
                }
                else if (Pagename == "RI") {
                    strURL += "RetroInfo.aspx";
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
           
        
    </script>

    <table>
        <tr>
            <td>
                <asp:ValidationSummary ID="valCombined" runat="server" ValidationGroup="save" />
                <asp:ValidationSummary ID="valCombinedElems" runat="server" ValidationGroup="update" />
                <br />
                <MV:MasterValues ID="UcMastervalues" runat="server" />
            </td>
        </tr>
        <tr>
            <td>
                <div id="divPrgmperiods" runat="server">
                    <table id="tblPrmperiods" runat="server" width="100%">
                        <tr>
                            <td>
                                <asp:Label ID="lblProgramPeriodsHeader" runat="server" CssClass="h2" Text="Program Periods"></asp:Label><br />
                                <PP:ProgramPeriod ID="UcProgramperiods" runat="server" />
                                <asp:HiddenField ID="hidProgPerdID" runat="server" Value="0" />
                            </td>
                        </tr>
                    </table>
                </div>
            </td>
        </tr>
        <tr>
            <td>
                <ajaxToolkit:TabContainer ID="tabcontCombinedElements" runat="server" CssClass="CustomTabs"
                    ActiveTabIndex="2" Width="623px">
                    <ajaxToolkit:TabPanel runat="server" ID="tabRetroInfo" HeaderText="Retro Information">
                        <HeaderTemplate>
                            <div onclick="Tabnavigation('RI')">
                                Retro Information</div>
                        </HeaderTemplate>
                        <ContentTemplate>
                        </ContentTemplate>
                    </ajaxToolkit:TabPanel>
                    <ajaxToolkit:TabPanel runat="server" ID="tabAssignERP" HeaderText="Assign ERP Formula">
                        <HeaderTemplate>
                            <div onclick="Tabnavigation('AE')">
                                Assign ERP Formula</div>
                        </HeaderTemplate>
                    </ajaxToolkit:TabPanel>
                    <ajaxToolkit:TabPanel runat="server" ID="tabCombinedElement" HeaderText="Combined Elements">
                        <HeaderTemplate>
                            Combined Elements
                        </HeaderTemplate>
                        <ContentTemplate>
                            <asp:ObjectDataSource ID="relAccDataSourcetst" runat="server" SelectMethod="GetAccountData"
                                TypeName="ZurichNA.AIS.Business.Logic.BLAccess"></asp:ObjectDataSource>
                            <asp:ObjectDataSource ID="PolicyDataSource" runat="server" TypeName="ZurichNA.AIS.Business.Logic.CombinedElementsBS"
                                SelectMethod="getCombelemsPolicylist" OnSelecting="PolicyInfoDataSource_Selecting">
                                <SelectParameters>
                                    <asp:Parameter Name="ProgramPeriodID" Type="Int32" />
                                </SelectParameters>
                            </asp:ObjectDataSource>
                            <asp:ObjectDataSource ID="ExposureDataSource" runat="server" SelectMethod="GetLookUpActiveData"
                                TypeName="ZurichNA.AIS.Business.Logic.BLAccess">
                                <SelectParameters>
                                    <asp:Parameter Name="lookUpTypeName" Type="String" DefaultValue="EXPOSURE TYPE" />
                                </SelectParameters>
                            </asp:ObjectDataSource>
                            <asp:ObjectDataSource ID="PerDataSource" runat="server" SelectMethod="GetLookUpActiveData"
                                TypeName="ZurichNA.AIS.Business.Logic.BLAccess">
                                <SelectParameters>
                                    <asp:Parameter Name="LookUPTypeName" Type="String" DefaultValue="PER" />
                                </SelectParameters>
                            </asp:ObjectDataSource>
                            <div id="divContent" runat="server" visible="false">
                                <table>
                                    <br />
                                    <tr>
                                        <table>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblRetroInfo" runat="server" CssClass="h2" Text="Combined Elements -- "
                                                        Visible="False"></asp:Label>
                                                </td>
                                                <td align="left">
                                                    <asp:Label ID="lblEffdt" runat="server" CssClass="h2"></asp:Label>
                                                </td>
                                                <td align="left">
                                                    <asp:Label ID="lblExpdt" runat="server" CssClass="h2"></asp:Label>
                                                </td>
                                                <td>
                                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:LinkButton ID="lnkClose"
                                                        runat="server" Text="Close" OnClick="lnkClose_Click"></asp:LinkButton>
                                                </td>
                                            </tr>
                                        </table>
                                    </tr>
                                    <tr>
                                        <td>
                                            <ajaxToolkit:TabContainer ID="tabCombinedElements" runat="server" CssClass="CustomTabs">
                                                <ajaxToolkit:TabPanel ID="tpDeductible" runat="server">
                                                    <HeaderTemplate>
                                                        Retro and Deductible</HeaderTemplate>
                                                    <ContentTemplate>
                                                        <div id="divCombinedelements" runat="server" style="overflow: auto; height: 105px;
                                                            width: 910px;" class="panelContents">
                                                            <asp:HiddenField ID="hidTotal" runat="server" Value="0" />
                                                            <asp:AISListView ID="lstCombinedelements" InsertItemPosition="FirstItem" runat="server"
                                                                OnItemEditing="lstCombinedelements_ItemEdit" OnItemCanceling="lstCombinedelements_ItemCancel"
                                                                OnItemUpdating="lstCombinedelements_ItemUpdating" OnItemCommand="lstCombinedelements_ItemCommand"
                                                                OnItemDataBound="DataBoundList">
                                                                <AlternatingItemTemplate>
                                                                    <tr class="AlternatingItemTemplate">
                                                                        <td>
                                                                            <asp:LinkButton ID="lnkEdit" runat="server" CommandName="Edit" Text="Edit" Enabled='<%# Eval("ACTV_IND")==null?true:(Eval("ACTV_IND").ToString()== "False"?false:true) %>'></asp:LinkButton>
                                                                        </td>
                                                                        <td>
                                                                            <asp:Label ID="lblID" runat="server" Text='<%# Bind("COMB_ELEMTS_SETUP_ID") %>' Visible="false"></asp:Label><asp:Label
                                                                                ID="lblPolicy" runat="server" Text='<%# Eval("PerfectPolicyNumber")%>'></asp:Label>
                                                                        </td>
                                                                        <td>
                                                                            <asp:Label ID="lblAuditexp" runat="server" Text='<%#Eval("AUDIT_EXPO_AMT")==null?String.Empty:decimal.Parse(Eval("AUDIT_EXPO_AMT").ToString()).ToString("#,##0") %>'></asp:Label>
                                                                        </td>
                                                                        <td>
                                                                            <asp:Label ID="lblExpTyp" runat="server" Text='<%# Bind("EXPOSURETYPE")%>'></asp:Label>
                                                                        </td>
                                                                        <td>
                                                                            <asp:Label ID="lblPER" runat="server" Text='<%#Bind("PERTEXT") %>'></asp:Label>
                                                                        </td>
                                                                        <td>
                                                                            <asp:Label ID="lblRate" runat="server" Text='<%#Bind("ADJ_RT")%>'></asp:Label>
                                                                        </td>
                                                                        <td align="right">
                                                                            <asp:Label ID="lblTotal" runat="server" Text='<%# Eval("TOT_AMT") != null ? (Eval("TOT_AMT").ToString() != "" ? (decimal.Parse(Eval("TOT_AMT").ToString())).ToString("#,##.00") : "0") : "0"%>'></asp:Label>
                                                                        </td>
                                                                        <td align="center">
                                                                            <asp:HiddenField ID="hidActive" runat="server" Value='<%# Bind("ACTV_IND")%>' />
                                                                            <asp:ImageButton ID="imgDisable" CommandArgument='<%# Bind("COMB_ELEMTS_SETUP_ID") %>'
                                                                                runat="server" ImageUrl='<%# Eval("ACTV_IND").ToString()=="True"?"~/images/disabled.GIF":"~/images/enabled.GIF" %>'>
                                                                            </asp:ImageButton>
                                                                        </td>
                                                                    </tr>
                                                                </AlternatingItemTemplate>
                                                                <EditItemTemplate>
                                                                    <tr>
                                                                        <td>
                                                                            <asp:LinkButton ID="lnkCombElemsUpdate" CommandName="Update" Text="Update" runat="server"
                                                                                Visible="true" ValidationGroup="update" />&nbsp;<asp:LinkButton ID="lnkCombElemsCancel"
                                                                                    CommandName="Cancel" runat="server" Text="Cancel" Visible="true" />
                                                                        </td>
                                                                        <td>
                                                                            <asp:Label ID="lblID" runat="server" Text='<%# Bind("COMB_ELEMTS_SETUP_ID") %>' Visible="false"></asp:Label><asp:Label
                                                                                ID="lblPolicynum" runat="server" Text='<%# Bind("PerfectPolicyNumber") %>'></asp:Label>
                                                                        </td>
                                                                        <td align="center">
                                                                            <asp:AISAmountTextbox ID="txtAuditexp"  ValidationGroup="update"
                                                                                runat="server" Text='<%#Bind("AUDIT_EXPO_AMT","{0:0}") %>'></asp:AISAmountTextbox>
                                                                            <%--<ajaxToolkit:FilteredTextBoxExtender runat="server" TargetControlID="txtAuditexp"
                                                                                FilterType="Custom" ValidChars="0123456789," ID="fltrAudexp">
                                                                            </ajaxToolkit:FilteredTextBoxExtender>--%>
                                                                            <asp:RequiredFieldValidator ValidationGroup="update" ID="reqAuditExp" runat="server"
                                                                                ControlToValidate="txtAuditexp" ErrorMessage="Please enter Audit Exposure Amount."
                                                                                Text="*"></asp:RequiredFieldValidator>
                                                                        </td>
                                                                        <td>
                                                                            <asp:Label ID="lblExpTyp" runat="server" Text='<%# Bind("EXPOSURETYPE")%>' Visible="false"></asp:Label>
                                                                            <asp:Label ID="lblPER" runat="server" Text='<%#Bind("PERTEXT") %>' Visible="false"></asp:Label><asp:DropDownList
                                                                                ID="ddlExptype" runat="server" DataSourceID="ExposureDataSource" DataTextField="LookUpName"
                                                                                DataValueField="LookUpID">
                                                                            </asp:DropDownList>
                                                                            <asp:CompareValidator ID="CompareddlExptype" runat="server" ControlToValidate="ddlExptype"
                                                                                ValueToCompare="0" ErrorMessage="Please select the exposure type." Text="*" Operator="NotEqual"
                                                                                ValidationGroup="update"></asp:CompareValidator>
                                                                        </td>
                                                                        <td align="center">
                                                                            <asp:DropDownList ID="ddlPer" runat="server" DataSourceID="PerDataSource" DataTextField="LookupName"
                                                                                DataValueField="LookUpId">
                                                                            </asp:DropDownList>
                                                                            <asp:CompareValidator ID="compareddlPer" runat="server" ControlToValidate="ddlPer"
                                                                                ValueToCompare="0" ErrorMessage="Please select the percentage." Text="*" Operator="NotEqual"
                                                                                ValidationGroup="update"></asp:CompareValidator>
                                                                        </td>
                                                                        <td align="center">
                                                                            <asp:TextBox ID="txtRate" runat="server" Text='<%#Eval("ADJ_RT")==null?String.Empty:decimal.Parse(Eval("ADJ_RT").ToString()).ToString("00.000000")%>'></asp:TextBox>
                                                                            <ajaxToolkit:MaskedEditExtender ID="mskRate" runat="server" TargetControlID="txtRate"
                                                                                Mask="99.999999" MaskType="Number" AutoComplete="false">
                                                                            </ajaxToolkit:MaskedEditExtender>
                                                                            <asp:RequiredFieldValidator ValidationGroup="update" ID="reqRate" runat="server"
                                                                                ControlToValidate="txtRate" ErrorMessage="Please enter Rate." Text="*"></asp:RequiredFieldValidator>
                                                                        </td>
                                                                        <td align="center">
                                                                            <asp:Label ID="lblTotal" runat="server" Text='<%# Eval("TOT_AMT") != null ? (Eval("TOT_AMT").ToString() != "" ? (decimal.Parse(Eval("TOT_AMT").ToString())).ToString("#,##.00") : "0") : "0"%>'></asp:Label>
                                                                        </td>
                                                                        <td align="center">
                                                                            <asp:CheckBox ID="chkActive" runat="server" Checked="true" />
                                                                        </td>
                                                                    </tr>
                                                                </EditItemTemplate>
                                                                <InsertItemTemplate>
                                                                    <tr class="AlternatingItemTemplate">
                                                                        <td>
                                                                            <asp:LinkButton ID="lnkSave" runat="server" CommandName="Save" ValidationGroup="save"
                                                                                Text="Save" Enabled='<%# Eval("ACTV_IND")==null?true:(Eval("ACTV_IND").ToString()== "False"?false:true) %>'></asp:LinkButton>
                                                                        </td>
                                                                        <td>
                                                                            <asp:DropDownList ID="ddlPolicy" runat="server" DataSourceID="PolicyDataSource" DataTextField="Policy"
                                                                                DataValueField="COML_AGMT_ID">
                                                                            </asp:DropDownList>
                                                                            <asp:CompareValidator ID="CompareddlPolicy" runat="server" ControlToValidate="ddlPolicy"
                                                                                ValueToCompare="0" ErrorMessage="Please select policy number" Text="*" Operator="NotEqual"
                                                                                ValidationGroup="save"></asp:CompareValidator>
                                                                        </td>
                                                                        <td>
                                                                            <asp:AISAmountTextbox ID="txtAuditexp"  runat="server" ValidationGroup="save"></asp:AISAmountTextbox>
                                                                            <%--<ajaxToolkit:FilteredTextBoxExtender runat="server" TargetControlID="txtAuditexp"
                                                                                FilterType="Custom" ValidChars="-0123456789," ID="fltrAudexpamt">
                                                                            </ajaxToolkit:FilteredTextBoxExtender>--%>
                                                                            <asp:RequiredFieldValidator ValidationGroup="save" ID="reqAuditExp" runat="server"
                                                                                ControlToValidate="txtAuditexp" ErrorMessage="Pelase enter Audit exposure amount."
                                                                                Text="*"></asp:RequiredFieldValidator>
                                                                        </td>
                                                                        <td>
                                                                            <asp:DropDownList ID="ddlExptype" runat="server" DataSourceID="ExposureDataSource"
                                                                                DataTextField="LookUpName" DataValueField="LookUpID">
                                                                            </asp:DropDownList>
                                                                            <asp:CompareValidator ID="CompareddlExptype" runat="server" ControlToValidate="ddlExptype"
                                                                                ValueToCompare="0" ErrorMessage="Please select Exposure type" Text="*" Operator="NotEqual"
                                                                                ValidationGroup="save"></asp:CompareValidator>
                                                                        </td>
                                                                        <td>
                                                                            <asp:DropDownList ID="ddlPer" runat="server" DataSourceID="PerDataSource" DataTextField="LookupName"
                                                                                DataValueField="LookUpId">
                                                                            </asp:DropDownList>
                                                                            <asp:CompareValidator ID="CompareddlPer" runat="server" ControlToValidate="ddlPer"
                                                                                ValueToCompare="0" ErrorMessage="Please select the percentage." Text="*" Operator="NotEqual"
                                                                                ValidationGroup="save"></asp:CompareValidator>
                                                                        </td>
                                                                        <td>
                                                                            <asp:TextBox ID="txtRate" runat="server" ValidationGroup="save"></asp:TextBox>
                                                                            <ajaxToolkit:MaskedEditExtender ID="mskRate" runat="server" TargetControlID="txtRate"
                                                                                Mask="99.999999" MaskType="Number" AutoComplete="false">
                                                                            </ajaxToolkit:MaskedEditExtender>
                                                                            <asp:RequiredFieldValidator ValidationGroup="save" ID="reqRate" runat="server" ControlToValidate="txtRate"
                                                                                ErrorMessage="Please enter Rate." Text="*"></asp:RequiredFieldValidator>
                                                                        </td>
                                                                        <td>
                                                                        </td>
                                                                        <td>
                                                                        </td>
                                                                    </tr>
                                                                </InsertItemTemplate>
                                                                <ItemTemplate>
                                                                    <tr class="ItemTemplate">
                                                                        <td>
                                                                            <asp:LinkButton ID="lnkEdit" runat="server" CommandName="Edit" Text="Edit" Enabled='<%# Eval("ACTV_IND")==null?true:(Eval("ACTV_IND").ToString()== "False"?false:true) %>'></asp:LinkButton>
                                                                        </td>
                                                                        <td>
                                                                            <asp:Label ID="lblID" runat="server" Text='<%# Bind("COMB_ELEMTS_SETUP_ID") %>' Visible="false"></asp:Label><asp:Label
                                                                                ID="lblPolicy" runat="server" Text='<%# Bind("PerfectPolicyNumber") %>'></asp:Label>
                                                                        </td>
                                                                        <td>
                                                                            <asp:Label ID="lblAuditexp" runat="server" Text='<%#Eval("AUDIT_EXPO_AMT")==null?String.Empty:decimal.Parse(Eval("AUDIT_EXPO_AMT").ToString()).ToString("#,##0") %>'></asp:Label>
                                                                        </td>
                                                                        <td>
                                                                            <asp:Label ID="lblExpTyp" runat="server" Text='<%# Bind("EXPOSURETYPE")%>'></asp:Label>
                                                                        </td>
                                                                        <td>
                                                                            <asp:Label ID="lblPER" runat="server" Text='<%#Bind("PERTEXT") %>'></asp:Label>
                                                                        </td>
                                                                        <td>
                                                                            <asp:Label ID="lblRate" runat="server" Text='<%#Bind("ADJ_RT")%>'></asp:Label>
                                                                        </td>
                                                                        <td align="right">
                                                                            <asp:Label ID="lblTotal" runat="server" Text='<%# Eval("TOT_AMT") != null ? (Eval("TOT_AMT").ToString() != "" ? (decimal.Parse(Eval("TOT_AMT").ToString())).ToString("#,##.00") : "0") : "0"%>'></asp:Label>
                                                                        </td>
                                                                        <td align="center">
                                                                            <asp:HiddenField ID="hidActive" runat="server" Value='<%# Bind("ACTV_IND")%>' />
                                                                            <asp:ImageButton ID="imgDisable" CommandArgument='<%# Bind("COMB_ELEMTS_SETUP_ID") %>'
                                                                                runat="server" ImageUrl='<%# Eval("ACTV_IND").ToString()=="True"?"~/images/disabled.GIF":"~/images/enabled.GIF" %>'>
                                                                            </asp:ImageButton>
                                                                        </td>
                                                                    </tr>
                                                                </ItemTemplate>
                                                                <LayoutTemplate>
                                                                    <table id="tblCombinedelements" class="panelContents" width="98%">
                                                                        <tr>
                                                                            <th style="width: 12%" align="center">
                                                                                Select
                                                                            </th>
                                                                            <th style="width: 15%" align="center">
                                                                                Policy
                                                                            </th>
                                                                            <th style="width: 18%" align="center">
                                                                                Audit Exposure
                                                                            </th>
                                                                            <th style="width: 20%" align="center">
                                                                                Exposure Type
                                                                            </th>
                                                                            <th style="width: 13%" align="center">
                                                                                Exposure Basis
                                                                            </th>
                                                                            <th style="width: 20%" align="center">
                                                                                Rate
                                                                            </th>
                                                                            <th align="right">
                                                                                Total
                                                                            </th>
                                                                            <th align="center">
                                                                                Disable
                                                                            </th>
                                                                        </tr>
                                                                        <tr id="itemPlaceholder" runat="server">
                                                                        </tr>
                                                                        <tr>
                                                                            <td colspan="6" align="right">
                                                                                Total Amount
                                                                            </td>
                                                                            <td style="border: solid 1 black;" align="right">
                                                                                <asp:Label ID="lblTotalAmount" runat="server" Text='<%= decimal.Parse(Val.ToString()).ToString("#,##0")%>'></asp:Label>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </LayoutTemplate>
                                                            </asp:AISListView>
                                                        </div>
                                                    </ContentTemplate>
                                                </ajaxToolkit:TabPanel>
                                                <ajaxToolkit:TabPanel ID="tpLSIElements" runat="server">
                                                    <HeaderTemplate>
                                                        Agreement/LSI Elements</HeaderTemplate>
                                                    <ContentTemplate>
                                                        <div>
                                                            <table width="100%" class="panelcontents">
                                                                <tr>
                                                                    <th align="center">
                                                                        Program Elements
                                                                    </th>
                                                                    <th align="center">
                                                                        Per Agreement
                                                                    </th>
                                                                    <th align="center">
                                                                        Per LSI
                                                                    </th>
                                                                </tr>
                                                                <tr class="ItemTemplate">
                                                                    <td>
                                                                        <asp:Label ID="lblALAE" runat="server" Text="ALAE"></asp:Label>
                                                                    </td>
                                                                    <td align="center">
                                                                        <asp:CheckBox ID="chkAlagreement" runat="server" Text="INCLUDED" />
                                                                    </td>
                                                                    <td align="center">
                                                                        <asp:CheckBox ID="chkLsialae" runat="server" Text="INCLUDED" />
                                                                    </td>
                                                                </tr>
                                                                <tr class="AlternatingItemTemplate">
                                                                    <td>
                                                                        <asp:Label ID="lblULAE" runat="server" Text="ULAE"></asp:Label>
                                                                    </td>
                                                                    <td align="center">
                                                                        <asp:CheckBox ID="chkUlaeagreement" runat="server" Text="INCLUDED" />
                                                                    </td>
                                                                    <td align="center">
                                                                        <asp:CheckBox ID="chkUlaeLSIIncluded" runat="server" Text="INCLUDED" />
                                                                    </td>
                                                                </tr>
                                                                <tr class="ItemTemplate">
                                                                    <td>
                                                                        <asp:Label ID="lblLBA" runat="server" Text="LBA"></asp:Label>
                                                                    </td>
                                                                    <td align="center">
                                                                        <asp:CheckBox ID="chkLBAagreement" runat="server" Text="INCLUDED" />
                                                                    </td>
                                                                    <td align="center">
                                                                        <asp:CheckBox ID="chkLBALSI" runat="server" Text="INCLUDED" />
                                                                    </td>
                                                                </tr>
                                                                <tr class="AlternatingItemTemplate">
                                                                    <td align="center">
                                                                        <asp:Label ID="lblPaidincurred" Text="Paid/Incurred" runat="server"></asp:Label>
                                                                    </td>
                                                                    <td align="center">
                                                                        <asp:CheckBox ID="chkPaidagreement" runat="server" Text="PAID" />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                                    </td>
                                                                    <td align="center">
                                                                        <asp:Label ID="lblPaidLsi" runat="server"></asp:Label>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                            <table width="910">
                                                                <tr>
                                                                    <td align="center">
                                                                        <uc1:Sc ID="btnSaveCancel" runat="server" />
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </div>
                                                    </ContentTemplate>
                                                </ajaxToolkit:TabPanel>
                                            </ajaxToolkit:TabContainer>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </ContentTemplate>
                    </ajaxToolkit:TabPanel>
                    <ajaxToolkit:TabPanel runat="server" ID="tabAuditinfo" HeaderText="Audit Information">
                        <HeaderTemplate>
                            <div onclick="Tabnavigation('AI')">
                                Audit Information</div>
                        </HeaderTemplate>
                        <ContentTemplate>
                        </ContentTemplate>
                    </ajaxToolkit:TabPanel>
                </ajaxToolkit:TabContainer>
            </td>
        </tr>
    </table>
</asp:Content>
