<%@ Page Language="C#" MasterPageFile="~/Retro.master" AutoEventWireup="true" Inherits="AcctInfo"
    CodeBehind="AcctInfo.aspx.cs" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Src="~/App_Shared/SaveCancel.ascx" TagPrefix="uc1" TagName="Sc" %>
<%@ Register Src="~/App_Shared/AccountList.ascx" TagPrefix="AL" TagName="AccountList" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeaderPlaceHolder" runat="Server">
    <table width="910" style="text-align: right">
        <tr>
            <td align="left">
                <asp:Label ID="AILabel" runat="server" Text="Account Information" CssClass="h1"></asp:Label>
            </td>
            <td align="right">
                <uc1:Sc ID="btnSaveCancel" runat="server" />
            </td>
        </tr>
    </table>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainPlaceHolder" runat="Server">
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
    <script type="text/javascript" language="javascript">
        window.history.forward(1);
    </script>
    <script type="text/javascript">

        var scrollTop1;
        var scrollTop2;
        var scrollTop3;

        if (Sys.WebForms.PageRequestManager != null) {
            Sys.WebForms.PageRequestManager.getInstance().add_beginRequest(BeginRequestHandler);
            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);
        }

        function BeginRequestHandler(sender, args) {
            var mef = $get('<%=divra.ClientID%>');
            if (mef != null)
                scrollTop1 = mef.scrollTop;

            var dv2 = $get('<%=Div2.ClientID%>');
            if (dv2 != null)
                scrollTop2 = dv2.scrollTop;

            var dv1 = $get('<%=Div1.ClientID%>');
            if (dv1 != null)
                scrollTop3 = dv1.scrollTop;
        }

        function EndRequestHandler(sender, args) {
            var mef = $get('<%=divra.ClientID%>');
            if (mef != null)
                mef.scrollTop = scrollTop1;

            var dv2 = $get('<%=Div2.ClientID%>');
            if (dv2 != null)
                dv2.scrollTop = scrollTop2;

            var dv1 = $get('<%=Div1.ClientID%>');
            if (dv1 != null)
                dv1.scrollTop = scrollTop3;
        }

        function ActiveTabChanged(sender, e) {
            __doPostBack('<%= TabConAcctinfo.ClientID %>', sender.get_activeTab().get_headerText());
        }
        function LSIPrimaryAccountPopup() {
            $get('<%=btnLSIPrimaryAccountClose.ClientID%>').click();
            $get('<%=btnLSIPrimaryAccount.ClientID%>').click();
        }

    </script>
    <asp:ValidationSummary ID="ValSave" ValidationGroup="SaveDate" runat="server"></asp:ValidationSummary>
    <!-- DataSource for the Related Account Dropdown box-->
    <asp:ObjectDataSource ID="relAccDataSource" runat="server" SelectMethod="GetAccountData"
        TypeName="ZurichNA.AIS.Business.Logic.BLAccess"></asp:ObjectDataSource>
    <!-- DataSource for the Related LSI Accounts Dropdown box-->
    <asp:ObjectDataSource ID="relLSIAccDataSource" runat="server" SelectMethod="GetAllLSICustomers"
        TypeName="ZurichNA.AIS.Business.Logic.BLAccess"></asp:ObjectDataSource>
    <asp:UpdatePanel ID="updpnlAccountInfo" runat="server">
        <ContentTemplate>
            <table style="width: 37%">
                <tr>
                    <td>
                    </td>
                </tr>
                <tr>
                    <td>
                        <!--OnClientActiveTabChanged="ActiveTabChanged"-->
                        <ajaxToolkit:TabContainer ID="TabConAcctinfo" runat="server" ActiveTabIndex="0" CssClass="CustomTabs"
                            OnActiveTabChanged="LoadData" OnClientActiveTabChanged="ActiveTabChanged">
                            <ajaxToolkit:TabPanel runat="server" HeaderText="TabPanel1" ID="TabAccountInfo" TabIndex="0">
                                <HeaderTemplate>
                                    <div style="width: 150px">
                                        Account Setup</div>
                                </HeaderTemplate>
                                <ContentTemplate>
                                    <table>
                                        <br>
                                        <tr>
                                            <td style="width: 111px" nowrap>
                                                <asp:Label ID="lblAccountName" runat="server" Text="Account Name"></asp:Label>
                                            </td>
                                            <td style="width: 168px">
                                                <asp:TextBox ID="txtAccountName" ValidationGroup="Save" runat="server" MaxLength="100"
                                                    Width="210px"></asp:TextBox>
                                            </td>
                                            <td nowrap>
                                                <asp:CheckBox ID="chkMasterAccount" ValidationGroup="Save" runat="server" Text="Master Account" />
                                            </td>
                                            <td nowrap>
                                                <asp:CheckBox ID="chkInActive" ValidationGroup="Save" runat="server" Enabled="false"
                                                    Text="In-Active" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 111px">
                                                <asp:Label ID="lblAccountNumber" runat="server" Text="Account Number"></asp:Label>
                                            </td>
                                            <td style="width: 168px">
                                                <asp:TextBox ID="txtAccountNumber" runat="server" Enabled="False" ReadOnly="True"
                                                    Width="210px"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:CheckBox ID="chkPEO" ValidationGroup="Save" runat="server" Text="PEO" />
                                            </td>
                                            <td nowrap>
                                                <asp:CheckBox ID="chkMDRetro" ValidationGroup="Save" runat="server" Text="MD Retro" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 111px">
                                                <asp:Label ID="lblBktcyBuyout" runat="server" Text="Bktcy/Buyout"></asp:Label>
                                            </td>
                                            <td style="width: 200px">
                                                <asp:ObjectDataSource ID="BktcyBuyoutDataSource" runat="server" SelectMethod="GetLookUpActiveData"
                                                    TypeName="ZurichNA.AIS.Business.Logic.BLAccess">
                                                    <SelectParameters>
                                                        <asp:Parameter DefaultValue="BKTCY/BUYOUT" Name="lookUpTypeName" Type="String" />
                                                    </SelectParameters>
                                                </asp:ObjectDataSource>
                                                <asp:DropDownList ID="ddlBktcyBuyout" ValidationGroup="Save" runat="server" DataSourceID="BktcyBuyoutDataSource"
                                                    DataTextField="LookUpName" DataValueField="LookUpID" AutoPostBack="true" OnSelectedIndexChanged="ddlBktcyBuyout_SelectedIndexChanged"
                                                    Width="215px">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblBktcyBuyoutEffDate" runat="server" Text="Bktcy/Buyout <br> (Eff. Date)"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtBktcyBuyoutEffDt" ValidationGroup="SaveDate" runat="server" Width="80px"></asp:TextBox>
                                                <asp:ImageButton ID="imgBktcyBuyoutEffDt" Enabled="false" ValidationGroup="Save"
                                                    runat="server" ImageUrl="~/images/Calendar_scheduleHS.png" CausesValidation="False" />
                                                <ajaxToolkit:MaskedEditExtender ID="mskBktcyBuyoutEffDt" runat="server" TargetControlID="txtBktcyBuyoutEffDt"
                                                    Mask="99/99/9999" MessageValidatorTip="true" MaskType="Date" ErrorTooltipEnabled="True" />
                                                <ajaxToolkit:CalendarExtender ID="CalExtBktcyBuyoutEffDt" runat="server" TargetControlID="txtBktcyBuyoutEffDt"
                                                    PopupButtonID="imgBktcyBuyoutEffDt" />
                                                <asp:RegularExpressionValidator ID="regBktcyBuyoutEffDt" runat="server" ControlToValidate="txtBktcyBuyoutEffDt"
                                                    ValidationExpression="(0?[1-9]|1[012])[- /.](0?[1-9]|[12][0-9]|3[01])[- /.](1[8-9]|2[01])\d\d"
                                                    ErrorMessage="Invalid Bktcy/Buyout Effective  Date" Text="*" ValidationGroup="SaveDate"></asp:RegularExpressionValidator>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 111px">
                                                <asp:Label ID="lblTPAFundedFirstValDt" runat="server" Text="TPA Funded <BR> (First Val)"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtTPAFundedFirstValDt" ValidationGroup="SaveDate" runat="server"
                                                    Width="80px"></asp:TextBox>
                                                <asp:ImageButton ID="imgTPAFundedFirstValDt" Enabled="false" ValidationGroup="Save"
                                                    runat="server" ImageUrl="~/images/Calendar_scheduleHS.png" CausesValidation="False" />
                                                <ajaxToolkit:MaskedEditExtender ID="mskTPAFundedFirstValDt" runat="server" TargetControlID="txtTPAFundedFirstValDt"
                                                    Mask="99/99/9999" MessageValidatorTip="true" MaskType="Date" ErrorTooltipEnabled="True" />
                                                <ajaxToolkit:CalendarExtender ID="calTPAFundedFirstValDt" runat="server" TargetControlID="txtTPAFundedFirstValDt"
                                                    PopupButtonID="imgTPAFundedFirstValDt" />
                                                <asp:RegularExpressionValidator ID="regTPAFundedFirstValDt" runat="server" ControlToValidate="txtTPAFundedFirstValDt"
                                                    ValidationExpression="(0?[1-9]|1[012])[- /.](0?[1-9]|[12][0-9]|3[01])[- /.](1[8-9]|2[01])\d\d"
                                                    ErrorMessage="Invalid TPA Funded Date" Text="*" ValidationGroup="SaveDate"></asp:RegularExpressionValidator>
                                            </td>
                                            <td nowrap>
                                                <asp:CheckBox ID="chkTPAFunded" ValidationGroup="Save" runat="server" AutoPostBack="true"
                                                    Checked="false" OnCheckedChanged="chkTPAFunded_CheckedChanged" Text="TPA Funded" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 111px">
                                                <asp:Label ID="lblSSCGID" runat="server" Text="SSCGID"></asp:Label>
                                            </td>
                                            <td style="width: 168px">
                                                <asp:TextBox ID="txtSSCGID" ValidationGroup="Save" runat="server" MaxLength="10"
                                                    Width="210px"></asp:TextBox>
                                                <ajaxToolkit:MaskedEditExtender ID="mskSSCGID" runat="server" TargetControlID="txtSSCGID"
                                                    Mask="9999999999" MessageValidatorTip="true" OnFocusCssClass="MaskedEditFocus"
                                                    InputDirection="LeftToRight" OnInvalidCssClass="MaskedEditError" MaskType="None"
                                                    AcceptNegative="Left" ErrorTooltipEnabled="True" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 111px">
                                                <asp:Label ID="lblBPNumber" runat="server" Text="BP Number"></asp:Label>
                                            </td>
                                            <td style="width: 168px">
                                                <asp:TextBox ID="txtBPNumber" ValidationGroup="Save" runat="server" MaxLength="10"
                                                    Width="210px"></asp:TextBox>
                                                <ajaxToolkit:MaskedEditExtender ID="mskBPNumber" runat="server" TargetControlID="txtBPNumber"
                                                    Mask="9999999999" MessageValidatorTip="true" OnFocusCssClass="MaskedEditFocus"
                                                    InputDirection="LeftToRight" OnInvalidCssClass="MaskedEditError" MaskType="None"
                                                    AcceptNegative="Left" ErrorTooltipEnabled="True" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 111px">
                                                <asp:Label ID="lblCompany" runat="server" Text="Company"></asp:Label>
                                            </td>
                                            <td style="width: 200px">
                                                <asp:ObjectDataSource ID="CompanyDataSource" runat="server" SelectMethod="GetLookUpActiveDataWithoutSelect"
                                                    TypeName="ZurichNA.AIS.Business.Logic.BLAccess">
                                                    <SelectParameters>
                                                        <asp:Parameter DefaultValue="COMPANY CODE" Name="lookUpTypeName" Type="String" />
                                                    </SelectParameters>
                                                </asp:ObjectDataSource>
                                                <asp:DropDownList ID="ddlCompany" ValidationGroup="Save" runat="server" DataSourceID="CompanyDataSource"
                                                    DataTextField="LookUpName" DataValueField="LookUpID" AutoPostBack="true" OnSelectedIndexChanged="ddlCompany_SelectedIndexChanged"
                                                    Width="215px">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 111px">
                                                <asp:Label ID="lblCurrency" runat="server" Text="Currency"></asp:Label>
                                            </td>
                                            <td style="width: 200px">
                                                <asp:ObjectDataSource ID="CurrencyDataSource" runat="server" SelectMethod="GetLookUpActiveDataWithoutSelect"
                                                    TypeName="ZurichNA.AIS.Business.Logic.BLAccess">
                                                    <SelectParameters>
                                                        <asp:Parameter DefaultValue="CURRENCY CODE" Name="lookUpTypeName" Type="String" />
                                                    </SelectParameters>
                                                </asp:ObjectDataSource>
                                                <asp:DropDownList ID="ddlCurrency" ValidationGroup="Save" runat="server" DataSourceID="CurrencyDataSource"
                                                    DataTextField="LookUpName" DataValueField="LookUpID" AutoPostBack="true" OnSelectedIndexChanged="ddlCurrency_SelectedIndexChanged"
                                                    Width="215px">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td nowrap>
                                                <asp:Label ID="lblInsContact" runat="server" Text="Ins. Contact"></asp:Label>
                                            </td>
                                            <td colspan="2">
                                                <asp:DropDownList ID="ddlInsContact" ValidationGroup="Save" runat="server" Width="350px">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 111px">
                                                <asp:Label ID="lblEnter" runat="server" Text="Enter"></asp:Label>
                                            </td>
                                            <td align="left">
                                                <asp:HyperLink ValidationGroup="Save" ID="hypComments" runat="server" NavigateUrl="~/AdjParams/CustomerComments.aspx"
                                                    Text="Comments"></asp:HyperLink>&nbsp;&nbsp;&nbsp;
                                                <asp:HyperLink ValidationGroup="Save" ID="hypEnterContacts" runat="server" NavigateUrl="~/AcctSetup/ExtContacts.aspx"
                                                    Text="Contacts"></asp:HyperLink>
                                            </td>
                                        </tr>
                                    </table>
                                </ContentTemplate>
                            </ajaxToolkit:TabPanel>
                            <ajaxToolkit:TabPanel ID="TabRelatedAccountDetails" runat="server" HeaderText="TabPanel2">
                                <HeaderTemplate>
                                    Related Retro Accounts</HeaderTemplate>
                                <ContentTemplate>
                                    <asp:UpdatePanel ID="udpRelatedAccounts" runat="server">
                                        <ContentTemplate>
                                            <asp:Panel ID="pnlRelatedAccounts" runat="server" CssClass="content">
                                                <br />
                                                <asp:Label ID="lblRetMsg" runat="server" Width="350px"><font size="1">Please choose related accounts from the dropdown below :</font></asp:Label>
                                                <br />
                                                <div id="divra" style="height: 300px; width: 527px; overflow: auto" runat="server">
                                                    <asp:AISListView ID="lstRelatedAccounts" runat="server" InsertItemPosition="FirstItem"
                                                        OnItemEditing="lstRelatedAccounts_ItemEdit" OnItemDataBound="lstRelatedAccounts_DataBoundList"
                                                        OnItemUpdating="lstRelatedAccounts_ItemUpdate" OnItemCommand="lstRelatedAccounts_ItemCommand"
                                                        OnItemInserting="lstRelatedAccounts_ItemInserting" OnItemCanceling="lstRelatedAccounts_ItemCancel">
                                                        <LayoutTemplate>
                                                            <table class="panelContents" runat="server">
                                                                <tr class="LayoutTemplate">
                                                                    <th>
                                                                        Select
                                                                    </th>
                                                                    <th>
                                                                        Account
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
                                                            <tr runat="server" class="ItemTemplate">
                                                                <td>
                                                                    <asp:LinkButton ID="lbRelatedAccountEdit" CommandName="Edit" Text="Edit" runat="server"
                                                                        Visible="true" Width="40px" />
                                                                </td>
                                                                <td align="left">
                                                                    <asp:Label ID="lblAccountName" Width="165px" runat="server" Text='<%# Bind("FULL_NM") %>'></asp:Label>
                                                                </td>
                                                                <td align="Center">
                                                                    <asp:ImageButton ID="imgDisable" CommandArgument='<%# Bind("CUSTMR_ID") %>' CommandName="Disable"
                                                                        runat="server" ToolTip='<%# Bind("IS_CUSTMR_REL_ACTV_IND") %>'></asp:ImageButton>
                                                                </td>
                                                            </tr>
                                                        </ItemTemplate>
                                                        <AlternatingItemTemplate>
                                                            <tr runat="server" class="AlternatingItemTemplate">
                                                                <td>
                                                                    <asp:LinkButton ID="lbRelatedAccountEdit" CommandName="Edit" Text="Edit" runat="server"
                                                                        Visible="true" Width="40px" />
                                                                </td>
                                                                <td align="left">
                                                                    <asp:Label ID="lblAccountName" Width="165px" runat="server" Text='<%# Bind("FULL_NM") %>'></asp:Label>
                                                                </td>
                                                                <td align="Center">
                                                                    <asp:ImageButton ID="imgDisable" CommandArgument='<%# Bind("CUSTMR_ID") %>' CommandName="Disable"
                                                                        runat="server" ToolTip='<%# Bind("IS_CUSTMR_REL_ACTV_IND") %>'></asp:ImageButton>
                                                                </td>
                                                            </tr>
                                                        </AlternatingItemTemplate>
                                                        <EditItemTemplate>
                                                            <tr>
                                                                <td align="left">
                                                                    <asp:LinkButton ID="lbRelatedAccountUpdate" CommandName="Update" Text="Update" runat="server"
                                                                        Visible="true" Width="40px" />
                                                                    <asp:LinkButton ID="lbRelatedAccountCancel" CommandName="Cancel" runat="server" Text="Cancel"
                                                                        Visible="true" Width="40px" />
                                                                </td>
                                                                <td align="left">
                                                                    <asp:Label ID="lblAccountName" Width="165px" runat="server" Text='<%# Bind("FULL_NM") %>'
                                                                        ToolTip='<%# Bind("CUSTMR_ID") %>'></asp:Label>
                                                                </td>
                                                                <td align="center">
                                                                    <asp:CheckBox ID="chkRetroActive" runat="server" Checked='<%# Eval("CUSTMR_REL_ACTV_IND") %>' />
                                                                </td>
                                                            </tr>
                                                        </EditItemTemplate>
                                                        <InsertItemTemplate>
                                                            <tr>
                                                                <td align="center" style="vertical-align: middle">
                                                                    <asp:LinkButton ID="lbRelatedAccountInsert" CommandName="Save" Text="Save" runat="server"
                                                                        Visible="true" Width="40px" />
                                                                </td>
                                                                <td align="center">
                                                                    <AL:AccountList ID="ddlAccountlist" runat="server" AccountType="3" />
                                                                </td>
                                                                <td align="center">
                                                                    <asp:Label ID="lblRetroBlank" runat="server"></asp:Label>
                                                                </td>
                                                            </tr>
                                                        </InsertItemTemplate>
                                                    </asp:AISListView>
                                                </div>
                                            </asp:Panel>
                                            <asp:Panel ID="pnlRelatedAccountsRO" runat="server" CssClass="content">
                                                <br>
                                                <br>
                                                <div id="Div2" style="height: 300px; width: 527px; overflow: auto" runat="server">
                                                    <asp:AISListView ID="lstRelatedAccountsRO" runat="server">
                                                        <LayoutTemplate>
                                                            <table id="Table1" class="panelContents" runat="server">
                                                                <tr class="LayoutTemplate">
                                                                    <th>
                                                                        Account
                                                                    </th>
                                                                    <th>
                                                                        Master
                                                                    </th>
                                                                </tr>
                                                                <tr id="ItemPlaceHolder" runat="server">
                                                                </tr>
                                                            </table>
                                                        </LayoutTemplate>
                                                        <ItemTemplate>
                                                            <tr id="Tr3" runat="server" class="ItemTemplate">
                                                                <td align="left">
                                                                    <asp:Label ID="lblAccountName" Width="165px" runat="server" Text='<%# Bind("FULL_NM") %>'></asp:Label>
                                                                </td>
                                                                <td align="left">
                                                                    <asp:Label ID="lblMaster" Width="165px" runat="server" Text='<%# Bind("IS_MSTR_ACCT_IND") %>'></asp:Label>
                                                                </td>
                                                            </tr>
                                                        </ItemTemplate>
                                                        <AlternatingItemTemplate>
                                                            <tr id="Tr4" runat="server" class="AlternatingItemTemplate">
                                                                <td align="left">
                                                                    <asp:Label ID="lblAccountName" Width="165px" runat="server" Text='<%# Bind("FULL_NM") %>'></asp:Label>
                                                                </td>
                                                                <td align="left">
                                                                    <asp:Label ID="lblMaster" Width="165px" runat="server" Text='<%# Bind("IS_MSTR_ACCT_IND") %>'></asp:Label>
                                                                </td>
                                                            </tr>
                                                        </AlternatingItemTemplate>
                                                    </asp:AISListView>
                                                </div>
                                            </asp:Panel>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </ContentTemplate>
                            </ajaxToolkit:TabPanel>
                            <ajaxToolkit:TabPanel ID="TabRelatedLSIAccountDetails" runat="server" HeaderText="TabPanel3">
                                <HeaderTemplate>
                                    Related LSI Accounts</HeaderTemplate>
                                <ContentTemplate>
                                    <br />
                                    <asp:Label ID="lblLSIMsg" runat="server" Width="350px"><font size="1">Please choose related LSI accounts from the dropdown below :</font></asp:Label>
                                    <br />
                                    <asp:UpdatePanel ID="udpRelatedLSIAccounts" runat="server">
                                        <ContentTemplate>
                                            <asp:Panel ID="pnlRelatedLSIAccounts" runat="server" CssClass="content">
                                                <div id="Div1" style="height: 300px; width: 700px; overflow: auto" runat="server">
                                                    <asp:AISListView ID="lstRelatedLSIAccounts" runat="server" InsertItemPosition="FirstItem"
                                                        OnItemEditing="lstRelatedLSIAccounts_ItemEdit" OnItemDataBound="lstRelatedLSIAccounts_DataBoundList"
                                                        OnItemUpdating="lstRelatedLSIAccounts_ItemUpdate" OnItemCommand="lstRelatedLSIAccounts_ItemCommand"
                                                        OnItemInserting="lstRelatedLSIAccounts_ItemInserting" OnItemCanceling="lstRelatedLSIAccounts_ItemCancel"
                                                        OnItemCreated="lstRelatedLSIAccounts_ItemCreated">
                                                        <LayoutTemplate>
                                                            <table  >
                                                                <tr>
                                                                    <th style="background-color:White">
                                                                    </th>
                                                                    <th style="background-color:White">
                                                                    </th>
                                                                    <th style="background-color:White">
                                                                    </th>
                                                                    <th style="background-color:White">
                                                                    </th>
                                                                    <th style="background-color:White">
                                                                    </th>
                                                                    <th colspan="2">
                                                                        Inferface Indicator
                                                                    </th>
                                                                    <th style="background-color:White">
                                                                    </th>
                                                                </tr>
                                                                <tr class="LayoutTemplate">
                                                                    <th>
                                                                        Select
                                                                    </th>
                                                                    <th>
                                                                        LSI Account Name
                                                                    </th>
                                                                    <th>
                                                                        LSI Account ID
                                                                    </th>
                                                                    <th>
                                                                        LSI Acct Type
                                                                    </th>
                                                                    <th>
                                                                        Primary
                                                                    </th>
                                                                    <th>
                                                                        PLB
                                                                    </th>
                                                                    <th>
                                                                        CHF Setup
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
                                                                <td align="center">
                                                                    <asp:LinkButton ID="lbRelatedLSIAccountEdit" CommandName="Edit" Text="Edit" runat="server"
                                                                        Visible="true" Width="40px" />
                                                                    <asp:Label ID="lblLSICustmrID" Width="165px" runat="server" Visible="false" Text='<%# Bind("LSI_CUSTMR_ID") %>'></asp:Label>
                                                                </td>
                                                                <td align="left">
                                                                    <asp:Label ID="lblLSIAccountName" Width="165px" runat="server" Text='<%# Bind("FULL_NAME") %>'></asp:Label>
                                                                </td>
                                                                <td align="right">
                                                                    <asp:Label ID="lblLSIAccountID" runat="server" Text='<%# Bind("LSI_ACCT_ID") %>'></asp:Label>
                                                                </td>
                                                                <td align="left">
                                                                    <asp:Label ID="lblAcctType" runat="server" Text='<%# Bind("ACCT_TYP") %>'></asp:Label>
                                                                </td>
                                                                <td align="left">
                                                                    <asp:Label ID="lblLSIPrimary" runat="server" Text='<%# Bind("IS_PRIM_IND") %>'></asp:Label>
                                                                </td>
                                                                <td align="left">
                                                                    <asp:Label ID="lblPLB" runat="server" Text='<%# Bind("IS_PLB_IND") %>'></asp:Label>
                                                                </td>
                                                                <td align="left">
                                                                    <asp:Label ID="lblCHF" runat="server" Text='<%# Bind("IS_CHF_IND") %>'></asp:Label>
                                                                </td>
                                                                <td align="center">
                                                                    <asp:ImageButton ID="imgDisable" CommandArgument='<%# Bind("LSI_CUSTMR_ID") %>' CommandName="Disable"
                                                                        runat="server" ToolTip='<%# Bind("IS_ACTV_IND") %>'></asp:ImageButton>
                                                                </td>
                                                            </tr>
                                                        </ItemTemplate>
                                                        <AlternatingItemTemplate>
                                                            <tr id="Tr2" runat="server" class="AlternatingItemTemplate">
                                                                <td align="center">
                                                                    <asp:LinkButton ID="lbRelatedLSIAccountEdit" CommandName="Edit" Text="Edit" runat="server"
                                                                        Visible="true" Width="40px" />
                                                                    <asp:Label ID="lblLSICustmrID" Width="165px" runat="server" Visible="false" Text='<%# Bind("LSI_CUSTMR_ID") %>'></asp:Label>
                                                                </td>
                                                                <td align="left">
                                                                    <asp:Label ID="lblLSIAccountName" Width="165px" runat="server" Text='<%# Bind("FULL_NAME") %>'></asp:Label>
                                                                </td>
                                                                <td align="right">
                                                                    <asp:Label ID="lblLSIAccountID" runat="server" Text='<%# Bind("LSI_ACCT_ID") %>'></asp:Label>
                                                                </td>
                                                                <td align="left">
                                                                    <asp:Label ID="lblAcctType" runat="server" Text='<%# Bind("ACCT_TYP") %>'></asp:Label>
                                                                </td>
                                                                <td align="left">
                                                                    <asp:Label ID="lblLSIPrimary" runat="server" Text='<%# Bind("IS_PRIM_IND") %>'></asp:Label>
                                                                </td>
                                                                <td align="left">
                                                                    <asp:Label ID="lblPLB" runat="server" Text='<%# Bind("IS_PLB_IND") %>'></asp:Label>
                                                                </td>
                                                                <td align="left">
                                                                    <asp:Label ID="lblCHF" runat="server" Text='<%# Bind("IS_CHF_IND") %>'></asp:Label>
                                                                </td>
                                                                <td align="center">
                                                                    <asp:ImageButton ID="imgDisable" CommandArgument='<%# Bind("LSI_CUSTMR_ID") %>' CommandName="Disable"
                                                                        runat="server" ToolTip='<%# Bind("IS_ACTV_IND") %>'></asp:ImageButton>
                                                                </td>
                                                            </tr>
                                                        </AlternatingItemTemplate>
                                                        <EditItemTemplate>
                                                            <tr>
                                                                <td align="left">
                                                                    <asp:LinkButton ID="lbRelatedLSIAccountUpdate" CommandName="Update" Text="Update"
                                                                        runat="server" Visible="true" Width="40px" />
                                                                    <asp:LinkButton ID="lbRelatedLSIAccountCancel" CommandName="Cancel" runat="server"
                                                                        Text="Cancel" Visible="true" Width="40px" />
                                                                    <asp:Label ID="lblLSICustmrID" Width="165px" runat="server" Visible="false" Text='<%# Bind("LSI_CUSTMR_ID") %>'></asp:Label>
                                                                </td>
                                                                <td align="left">
                                                                    <asp:Label ID="lblLSIAccountName" Width="165px" runat="server" Text='<%# Bind("FULL_NAME") %>'
                                                                        ToolTip='<%# Bind("LSI_ACCT_ID") %>'></asp:Label>
                                                                </td>
                                                                <td align="right">
                                                                    <asp:Label ID="lblLSIAccountID" runat="server" Text='<%# Bind("LSI_ACCT_ID") %>'></asp:Label>
                                                                </td>
                                                                <td align="left">
                                                                <asp:Label ID="lblAcctType" runat="server"></asp:Label>
                                                                </td>
                                                                <td align="center">
                                                                    <asp:CheckBox ID="chkLSIPrimary" Checked='<%# Eval("PRIM_IND") %>' runat="server" />
                                                                </td>
                                                                <td align="center">
                                                                    <asp:CheckBox ID="chkPLB" Checked='<%# Eval("PLB_IND") %>' runat="server" />
                                                                </td>
                                                                <td align="center">
                                                                    <asp:CheckBox ID="chkCHF" Checked='<%# Eval("CHF_IND") %>' runat="server" />
                                                                </td>
                                                                <td align="center">
                                                                    <asp:CheckBox ID="chkLSIActive" Checked='<%# Eval("ACTV_IND") %>' runat="server" />
                                                                </td>
                                                            </tr>
                                                        </EditItemTemplate>
                                                        <InsertItemTemplate>
                                                            <tr>
                                                                <td align="center" style="vertical-align: middle">
                                                                    <asp:LinkButton ID="lbRelatedLSIAccountInsert" CommandName="Save" Text="Save" runat="server"
                                                                        Visible="true" Width="40px" />
                                                                </td>
                                                                <td align="center">
                                                                    <AL:AccountList ID="ddlAccountlist" runat="server" AccountType="4" />
                                                                </td>
                                                                <td align="center">
                                                                    <asp:Label ID="lblLSIBlank1" runat="server"></asp:Label>
                                                                </td>
                                                                <td>
                                                                </td>
                                                                <td align="center">
                                                                    <asp:CheckBox ID="chkLSIPrimary" runat="server" />
                                                                </td>
                                                                <td align="center">
                                                                    <asp:CheckBox ID="chkPLB" runat="server" />
                                                                </td>
                                                                <td align="center">
                                                                    <asp:CheckBox ID="chkCHF" runat="server" />
                                                                </td>
                                                                <td align="center">
                                                                    <asp:Label ID="lblLSIBlank3" runat="server"></asp:Label>
                                                                </td>
                                                            </tr>
                                                        </InsertItemTemplate>
                                                    </asp:AISListView>
                                                </div>
                                            </asp:Panel>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </ContentTemplate>
                            </ajaxToolkit:TabPanel>
                            <ajaxToolkit:TabPanel ID="TabAccountResponsibility" runat="server" HeaderText="TabPanel2">
                                <HeaderTemplate>
                                    Account Responsibilities</HeaderTemplate>
                                <ContentTemplate>
                                    <br />
                                    <br />
                                    <asp:UpdatePanel ID="udpAccountResponsibility" runat="server">
                                        <ContentTemplate>
                                            <asp:Panel ID="pnlAccountResponsibility" runat="server" CssClass="content">
                                                <asp:AISListView ID="lstAccountResponsibility" runat="server">
                                                    <LayoutTemplate>
                                                        <table id="Table1" class="panelContents" runat="server">
                                                            <tr class="LayoutTemplate">
                                                                <th>
                                                                    Responsibility
                                                                </th>
                                                                <th>
                                                                    Name
                                                                </th>
                                                            </tr>
                                                            <tr id="ItemPlaceHolder" runat="server">
                                                            </tr>
                                                        </table>
                                                    </LayoutTemplate>
                                                    <EmptyDataTemplate>
                                                        <table id="tblAccntinfo" class="panelContents" runat="server" width="280px">
                                                            <tr runat="server" class="ItemTemplate">
                                                                <th>
                                                                    Responsibility
                                                                </th>
                                                                <th>
                                                                    Name
                                                                </th>
                                                            </tr>
                                                            <tr runat="server" class="ItemTemplate">
                                                                <td align="center" colspan="2">
                                                                    <asp:Label ID="lblEmptyMessage" Text="---No Records Found ---" Font-Bold="true" runat="server"
                                                                        Style="text-align: center" />
                                                                </td>
                                                            </tr>
                                                            </tr>
                                                        </table>
                                                    </EmptyDataTemplate>
                                                    <ItemTemplate>
                                                        <tr runat="server" class="ItemTemplate">
                                                            <td align="left">
                                                                <asp:Label ID="lblResponsibility" Width="265px" runat="server" Text='<%# Bind("RESP_NAME") %>'></asp:Label>
                                                            </td>
                                                            <td align="left">
                                                                <asp:Label ID="lblName" Width="265px" runat="server" Text='<%# Bind("FULLNAME") %>'></asp:Label>
                                                            </td>
                                                        </tr>
                                                    </ItemTemplate>
                                                    <AlternatingItemTemplate>
                                                        <tr runat="server" class="AlternatingItemTemplate">
                                                            <td align="left">
                                                                <asp:Label ID="lblResponsibility" Width="265px" runat="server" Text='<%# Bind("RESP_NAME") %>'></asp:Label>
                                                            </td>
                                                            <td align="left">
                                                                <asp:Label ID="lblName" Width="265px" runat="server" Text='<%# Bind("FULLNAME") %>'></asp:Label>
                                                            </td>
                                                        </tr>
                                                    </AlternatingItemTemplate>
                                                </asp:AISListView>
                                            </asp:Panel>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </ContentTemplate>
                            </ajaxToolkit:TabPanel>
                        </ajaxToolkit:TabContainer>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="TabConAcctinfo" EventName="ActiveTabChanged" />
        </Triggers>
    </asp:UpdatePanel>
    <asp:UpdatePanel runat="server" ID="upLSIPrimaryAccount">
        <ContentTemplate>
            <asp:Button Style="display: none" runat="server" ID="btnFinalTemp" />
            <ajaxToolkit:ModalPopupExtender runat="server" ID="modalLSIPrimaryAccount" TargetControlID="btnFinalTemp"
                PopupControlID="pnlLSIPrimaryAccount" BackgroundCssClass="modalBackground" DropShadow="true"
                CancelControlID="btnLSIPrimaryAccountClose">
            </ajaxToolkit:ModalPopupExtender>
            <div style="float: left;">
                <asp:Panel runat="server" CssClass="modalPopup" ID="pnlLSIPrimaryAccount" Style="border: solid 1px black;
                    display: none; width: 650px; padding: 0px" HorizontalAlign="Center">
                    <asp:Panel runat="Server" ID="Panel4" Style="width: 100%; cursor: move; padding: 0px;
                        background-color: #CCCCCC; height: 20px; border: solid 1px Gray; color: Black;
                        text-align: center; vertical-align: middle">
                    </asp:Panel>
                    <div style="text-align: left; width: 100%; padding-bottom: 5px; background-color: White;">
                        <br />
                        <ul>
                            <div style="text-align: left; width: 100%; padding-bottom: 5px; background-color: White;">
                                There can be multiple related LSI accounts, but only one can be primary. A primary
                                account already exists.
                            </div>
                            <div style="text-align: left; width: 100%; padding-bottom: 5px; background-color: White;">
                                Do you want to proceed to enable this primary account?
                            </div>
                        </ul>
                    </div>
                    <div style="text-align: center; width: 100%; padding-bottom: 2px; background-color: White;">
                        <asp:Button Width="60px" OnClientClick="LSIPrimaryAccountPopup()" ID="btnLSIPrimaryAccount"
                            runat="server" Text="Yes" OnClick="btnLSIPrimaryAccount_Click" />
                        <asp:Button Width="60px" ID="btnLSIPrimaryAccountClose" runat="server" Text="No"
                            OnClick="btnLSIPrimaryAccountClose_Click" />
                        <br />
                    </div>
                </asp:Panel>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
