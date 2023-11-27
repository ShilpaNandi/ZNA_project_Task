<%@ Page Language="C#" MasterPageFile="~/Retro.master" AutoEventWireup="true" Inherits="AppMgmt_InternalContacts"
    Title="Internal Master" CodeBehind="InternalContacts.aspx.cs" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="AjaxToolKit" %>
<%@ Register Src="../App_Shared/SaveCancel.ascx" TagName="SaveCancel" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeaderPlaceHolder" runat="Server">
    <table width="910" style="text-align: right" cellpadding="0" cellspacing="0">
        <tr>
            <td align="left">
                <asp:Label ID="GAILabel" runat="server" Text="Internal Masters" CssClass="h1"></asp:Label>
            </td>
            <td align="right">
                <asp:Button ID="btnAdd" runat="server" ToolTip="Please click here to add new Internal Contact"
                    Text="Add New" OnClick="btnAdd_Click" />
            </td>
        </tr>
    </table>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainPlaceHolder" runat="Server">
    <script language="javascript" type="text/javascript">
        var scrollTop1;
        if (Sys.WebForms.PageRequestManager != null) {
            Sys.WebForms.PageRequestManager.getInstance().add_beginRequest(BeginRequestHandler);
            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);
        }

        function summary() {
            $get('<%=valSummInternal.ClientID %>').style.BorderWidht = "1px";
            var hi = $get('<%=valSummInternal.ClientID %>');
            hi.style.display = 'block';
        }
        function BeginRequestHandler(sender, args) {
            var mef = $get('<%=Panel1.ClientID%>');
            if (mef != null)
                scrollTop1 = mef.scrollTop;
            var intern = $get('<%=valSummInternal.ClientID%>');
        }

        function EndRequestHandler(sender, args) {
            var mef = $get('<%=Panel1.ClientID%>');
            if (mef != null)
                mef.scrollTop = scrollTop1;
            var intern = $get('<%=valSummInternal.ClientID%>');
        }

        var oldgridSelectedColor;
        var oldgridClickedColor;
        var oldElement;

        function setMouseOverColor(element) {
            oldgridSelectedColor = element.style.backgroundColor;
            element.style.backgroundColor = 'lightblue';
            element.style.cursor = 'hand';
            element.style.textDecoration = 'underline';
        }

        function setMouseOutColor(element) {
            element.style.backgroundColor = oldgridSelectedColor;
            element.style.textDecoration = 'none';
        }
        function SetMouseClickColor(element) {
            if (oldElement != null) {
                oldElement.style.backgroundColor = oldgridSelectedColor;
            }
            oldElement = element;
            oldgridSelectedColor = element.style.backgroundColor;
            element.style.backgroundColor = 'yellow';
            element.style.cursor = 'hand';
        }
  

   
    </script>
    <asp:UpdatePanel runat="server" ID="updInternal">
        <ContentTemplate>
            <asp:ValidationSummary ID="valSummInternal" ValidationGroup="Save" runat="server">
            </asp:ValidationSummary>
            <br />
            <asp:ValidationSummary ID="valSearch" ValidationGroup="Search" runat="server"></asp:ValidationSummary>
            Contact Type:&nbsp;
            <asp:DropDownList ID="ddlSearchContact" runat="server" DataSourceID="ContactTypeDataSource"
                DataTextField="LookUpName" DataValueField="LookUpID">
            </asp:DropDownList>
            <asp:CompareValidator ID="CompareValidator1" runat="server" ControlToValidate="ddlSearchContact"
                ValidationGroup="Search" ValueToCompare="0" Text="*" ErrorMessage="Please select Contact Type"
                Operator="NotEqual"></asp:CompareValidator>
            &nbsp;<asp:Button ID="btnSearch" runat="server" ValidationGroup="Search" Text="Search"
                OnClick="btnSearch_Click" />
            <br />
            <br />
            <asp:Panel ID="Panel1" Width="910px" runat="server" ScrollBars="Auto" Height="130px">
                <asp:AISListView ID="lstInternalMasters" runat="server" DataKeyNames="PERSON_ID"
                    OnItemDataBound="DataBoundList" OnSelectedIndexChanging="lstInternalMasters_SelectedIndexChanging"
                    OnItemCommand="CommandList">
                    <LayoutTemplate>
                        <table id="tblLayout" class="panelContents" runat="server" width="98%">
                            <tr class="LayoutTemplate">
                                <th>
                                </th>
                                <th>
                                    Contact Type
                                </th>
                                <th>
                                    Title
                                </th>
                                <th>
                                    First Name
                                </th>
                                <th>
                                    Last Name
                                </th>
                                <th>
                                    UserID
                                </th>
                                <th>
                                    Valid From
                                </th>
                                <th>
                                    Valid To
                                </th>
                                <th>
                                    Disable
                                </th>
                            </tr>
                            <tr id="itemPlaceholder" runat="server">
                            </tr>
                        </table>
                    </LayoutTemplate>
                    <ItemTemplate>
                        <tr class="ItemTemplate" runat="server" id="trItemTemplate">
                            <td>
                                <asp:HiddenField ID="HidPostID" runat="server" Value='<%# Bind("POST_ADDR_ID") %>' />
                                <asp:LinkButton ID="lnkSelect" CommandArgument='<%# Bind("PERSON_ID") %>' CommandName="Select"
                                    runat="server" Text="Select"></asp:LinkButton>
                            </td>
                            <td>
                                <%# Eval("CONTACTTYPE")%>
                            </td>
                            <td>
                                <%# Eval("TITLE")%>
                            </td>
                            <td>
                                <%# Eval("FORENAME")%>
                            </td>
                            <td>
                                <%# Eval("SURNAME")%>
                            </td>
                            <td>
                                <%# Eval("USERID")%>
                            </td>
                            <td>
                                <%# Eval("EFFECTIVEDATE","{0:d}")%>
                            </td>
                            <td>
                                <%# Eval("EXPIRYDATE", "{0:d}")%>
                            </td>
                            <td>
                                <asp:HiddenField ID="hidActive" runat="server" Value='<%# Bind("ACTIVE")%>' />
                                <asp:ImageButton ID="imgDisable" CommandArgument='<%# Bind("PERSON_ID") %>' runat="server"
                                    ImageUrl='<%# Eval("ACTIVE").ToString()=="True"?"~/images/disabled.GIF":"~/images/enabled.GIF" %>'>
                                </asp:ImageButton>
                            </td>
                        </tr>
                    </ItemTemplate>
                    <AlternatingItemTemplate>
                        <tr class="AlternatingItemTemplate" runat="server" id="trItemTemplate">
                            <td>
                                <asp:HiddenField ID="HidPostID" runat="server" Value='<%# Bind("POST_ADDR_ID") %>' />
                                <asp:LinkButton ID="lnkSelect" CommandArgument='<%# Bind("PERSON_ID") %>' CommandName="Select"
                                    runat="server" Text="Select"></asp:LinkButton>
                            </td>
                            <td>
                                <%# Eval("CONTACTTYPE")%>
                            </td>
                            <td>
                                <%# Eval("TITLE")%>
                            </td>
                            <td>
                                <%# Eval("FORENAME")%>
                            </td>
                            <td>
                                <%# Eval("SURNAME")%>
                            </td>
                            <td>
                                <%# Eval("USERID")%>
                            </td>
                            <td>
                                <%# Eval("EFFECTIVEDATE","{0:d}")%>
                            </td>
                            <td>
                                <%# Eval("EXPIRYDATE", "{0:d}")%>
                            </td>
                            <td>
                                <asp:HiddenField ID="hidActive" runat="server" Value='<%# Bind("ACTIVE")%>' />
                                <asp:ImageButton ID="imgDisable" CommandArgument='<%# Bind("PERSON_ID") %>' runat="server"
                                    ImageUrl='<%# Eval("ACTIVE").ToString()=="True"?"~/images/disabled.GIF":"~/images/enabled.GIF" %>'>
                                </asp:ImageButton>
                            </td>
                        </tr>
                    </AlternatingItemTemplate>
                    <EmptyDataTemplate>
                        <table id="Table1" class="panelContents" runat="server" style="width: 100%">
                            <tr>
                                <th>
                                    Contact Type
                                </th>
                                <th>
                                    Title
                                </th>
                                <th>
                                    First Name
                                </th>
                                <th>
                                    Last Name
                                </th>
                                <th>
                                    UserID
                                </th>
                                <th>
                                    Valid From
                                </th>
                                <th>
                                    Valid To
                                </th>
                                <th>
                                    Disable
                                </th>
                            </tr>
                            <tr class="ItemTemplate">
                                <td align="center" colspan="8">
                                    <asp:Label ID="lblEmptyMessage" Text="---No Records Found ---" Font-Bold="true" runat="server"
                                        Style="text-align: center" />
                                </td>
                            </tr>
                        </table>
                    </EmptyDataTemplate>
                </asp:AISListView>
            </asp:Panel>
            <div style="padding-top: 10px">
                <asp:Label ID="lblInternalContactsDetails" Visible="false" runat="server" Text="Internal Masters Details"
                    CssClass="h2"></asp:Label>
                <asp:LinkButton ID="lnkClose" runat="server" Visible="false" Text="Close" OnClick="lnkClose_Click"></asp:LinkButton>
            </div>
            <asp:Panel BorderColor="Black" ID="pnlDetails" Visible="false" BorderWidth="1" Width="910px"
                runat="server" DefaultButton="btnSave">
                <table width="100%">
                    <tr>
                        <td align="right">
                            <asp:HiddenField ID="hidindex" runat="server" Value="-1" />
                        </td>
                    </tr>
                </table>
                <table width="100%">
                    <tr>
                        <td>
                            Contact Type:
                        </td>
                        <td>
                            <asp:ObjectDataSource ID="ContactTypeDataSource" runat="server" SelectMethod="GetLookUpActiveData"
                                TypeName="ZurichNA.AIS.Business.Logic.BLAccess">
                                <SelectParameters>
                                    <asp:Parameter DefaultValue="CONTACT TYPE" Name="lookUpTypeName" Type="String" />
                                    <asp:Parameter DefaultValue="I" Name="attribute" Type="String" />
                                </SelectParameters>
                            </asp:ObjectDataSource>
                            <asp:DropDownList ID="ddlContactType" runat="server" DataSourceID="ContactTypeDataSource"
                                DataTextField="LookUpName" DataValueField="LookUpID">
                            </asp:DropDownList>
                            <asp:CompareValidator ID="compContact" runat="server" ControlToValidate="ddlContactType"
                                ValidationGroup="Save" ValueToCompare="0" Text="*" ErrorMessage="Please select Contact Type"
                                Operator="NotEqual"></asp:CompareValidator>
                        </td>
                        <td>
                            Title:
                        </td>
                        <td>
                            <asp:ObjectDataSource ID="objTitle" runat="server" SelectMethod="GetLookUpActiveData"
                                TypeName="ZurichNA.AIS.Business.Logic.BLAccess">
                                <SelectParameters>
                                    <asp:Parameter DefaultValue="TITLE" Name="lookUpTypeName" Type="String" />
                                </SelectParameters>
                            </asp:ObjectDataSource>
                            <asp:DropDownList ID="ddlTitle" runat="server" DataSourceID="objTitle" DataTextField="LookUpName"
                                DataValueField="LookUpID">
                            </asp:DropDownList>
                            <%--<asp:CompareValidator ID="compvalddlTitle" runat="server" ControlToValidate="ddlTitle"
                                ValueToCompare="0" ValidationGroup="Save" Text="*" ErrorMessage="Please select Title"
                                Operator="NotEqual"></asp:CompareValidator>--%>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            First Name:
                        </td>
                        <td>
                            <asp:TextBox ID="txtFirstName" runat="server" MaxLength="50" Width="246px"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="reqFirstName" runat="server" ErrorMessage="Please enter First name"
                                ValidationGroup="Save" ControlToValidate="txtFirstName" Text="*"></asp:RequiredFieldValidator>
                        </td>
                        <td>
                            Last Name:
                        </td>
                        <td>
                            <asp:TextBox ID="txtLastName" runat="server" MaxLength="50" Width="246px"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="reqLastName" runat="server" ErrorMessage="Please enter Last name"
                                ValidationGroup="Save" ControlToValidate="txtLastName" Text="*"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Address1:
                        </td>
                        <td>
                            <asp:TextBox ID="txtAddress1" runat="server" MaxLength="50" Width="246px"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="ReqAddr1" runat="server" ErrorMessage="Please enter Address"
                                ValidationGroup="Save" ControlToValidate="txtAddress1" Text="*"></asp:RequiredFieldValidator>
                        </td>
                        <td>
                            Address2:
                        </td>
                        <td>
                            <asp:TextBox ID="txtAddress2" runat="server" Width="246px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            City:
                        </td>
                        <td>
                            <asp:TextBox ID="txtCity" runat="server" MaxLength="35" Width="246px"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="reqCity" runat="server" ErrorMessage="Please enter City"
                                ValidationGroup="Save" ControlToValidate="txtCity" Text="*"></asp:RequiredFieldValidator>
                        </td>
                        <td>
                            State/Province:
                        </td>
                        <td>
                            <asp:ObjectDataSource ID="objDataSourceState" runat="server" SelectMethod="GetLookUpActiveData"
                                TypeName="ZurichNA.AIS.Business.Logic.BLAccess">
                                <SelectParameters>
                                    <asp:Parameter DefaultValue="STATE" Name="lookUpTypeName" Type="String" />
                                </SelectParameters>
                            </asp:ObjectDataSource>
                            <asp:DropDownList ID="ddlState" runat="server" DataSourceID="objDataSourceState"
                                OnSelectedIndexChanged="ddlState_SelectedIndexChanged" DataTextField="LookUpName"
                                DataValueField="LookUpID" AutoPostBack="true">
                            </asp:DropDownList>
                            <asp:CompareValidator ID="CompareddlState" runat="server" ControlToValidate="ddlState"
                                ValueToCompare="0" ErrorMessage="Please select State/Province" Text="*" Operator="NotEqual"
                                ValidationGroup="Save"></asp:CompareValidator>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Zip Code:
                        </td>
                        <td>
                            <asp:TextBox ID="txtZipCode" runat="server" Width="246px"></asp:TextBox>
                            <asp:CompareValidator ID="compZipCode" runat="server" ControlToValidate="txtZipCode"
                                ValueToCompare="_____-____" ErrorMessage="Please enter Zip Code" Text="*" Operator="NotEqual"
                                ValidationGroup="Save"></asp:CompareValidator>
                            <AjaxToolKit:MaskedEditExtender id="mskZipCode" runat="server" targetcontrolid="txtZipCode"
                                mask="99999-9999" errortooltipenabled="True" cultureampmplaceholder="" culturecurrencysymbolplaceholder=""
                                culturedateformat="" culturedateplaceholder="" culturedecimalplaceholder="" culturethousandsplaceholder=""
                                culturetimeplaceholder="" enabled="True" clearmaskonlostfocus="False" />
                            <AjaxToolKit:MaskedEditValidator id="MaskedEditValidator2" controlextender="mskZipCode" runat="server"
                                controltovalidate="txtZipCode" isvalidempty="True" errormessage="error" invalidvalueblurredmessage="*"
                                invalidvaluemessage="Please Enter valid Zip Code" validationexpression="\d{5}-\_{4}|\d{5}-\d{4}|\_{5}-\_{4}"
                                validationgroup="Save">
                                                        </AjaxToolKit:MaskedEditValidator>
                        </td>
                        <td>
                            Telephone
                        </td>
                        <td>
                            <asp:TextBox ID="txtTelephone" runat="server" Width="116px"></asp:TextBox>
                            <%--<asp:CompareValidator ID="compTelephone" runat="server" ControlToValidate="txtTelephone"
                                ValueToCompare="(___)___-____" ErrorMessage="Please enter Telephone" Text="*"
                                Operator="NotEqual" ValidationGroup="Save"></asp:CompareValidator>--%>
                            <AjaxToolKit:MaskedEditValidator ID="mskValPhone" ControlExtender="mskTelephone"
                                runat="server" ControlToValidate="txtTelephone" IsValidEmpty="True" ErrorMessage="error"
                                InvalidValueBlurredMessage="*" InvalidValueMessage="Please enter valid Telephone number"
                                ValidationExpression="((\(\d{3}\) ?)|(\d{3}-))?\d{3}-\d{4}|\(\_{3}\)\_{3}\-\_{4}"
                                Display="Dynamic" ValidationGroup="Save">
                            </AjaxToolKit:MaskedEditValidator>
                            <AjaxToolKit:MaskedEditExtender ID="mskTelephone" runat="server" TargetControlID="txtTelephone"
                                Mask="(999)999-9999" ClearMaskOnLostFocus="false" />
                            Ext
                            <asp:TextBox ID="txtExtension" runat="server" Width="90px" MaxLength="4"></asp:TextBox>
                            <AjaxToolKit:FilteredTextBoxExtender runat="server" TargetControlID="txtExtension"
                                FilterType="Custom" ValidChars="0123456789" ID="fltExtension">
                            </AjaxToolKit:FilteredTextBoxExtender>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Fax:
                        </td>
                        <td>
                            <asp:TextBox ID="txtFax" runat="server" Width="246px"></asp:TextBox>
                            <AjaxToolKit:MaskedEditExtender ID="mskFax" runat="server" TargetControlID="txtFax"
                                Mask="(999)999-9999" ClearMaskOnLostFocus="false" />
                            <AjaxToolKit:MaskedEditValidator ID="mskValFax" ControlExtender="mskFax" runat="server"
                                ControlToValidate="txtFax" IsValidEmpty="True" ErrorMessage="error" InvalidValueBlurredMessage="*"
                                InvalidValueMessage="Please enter valid FAX number" ValidationExpression="((\(\d{3}\) ?)|(\d{3}-))?\d{3}-\d{4}|\(\_{3}\)\_{3}\-\_{4}"
                                Display="Dynamic" ValidationGroup="Save">
                            </AjaxToolKit:MaskedEditValidator>
                        </td>
                        <td>
                            Email:
                        </td>
                        <td>
                            <asp:TextBox ID="txtEmail" runat="server" Width="246px" MaxLength="50"></asp:TextBox>
                            <asp:RegularExpressionValidator ID="regValEmail" runat="server" ErrorMessage="Enter valid Email"
                                ValidationGroup="Save" Text="*" ControlToValidate="txtEmail" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"></asp:RegularExpressionValidator>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Valid From:
                        </td>
                        <td>
                            <asp:TextBox ID="txtValidFrom" runat="server"></asp:TextBox>
                            <AjaxToolKit:CalendarExtender ID="calValidFrom" runat="server" PopupPosition="TopRight"
                                TargetControlID="txtValidFrom" PopupButtonID="imgValidFrom" />
                            <asp:ImageButton ID="imgValidFrom" runat="server" ImageUrl="~/images/Calendar_scheduleHS.png"
                                CausesValidation="False" />
                            <AjaxToolKit:MaskedEditExtender ID="mskEditFrom" runat="server" TargetControlID="txtValidFrom"
                                Mask="99/99/9999" MessageValidatorTip="true" OnFocusCssClass="MaskedEditFocus"
                                OnInvalidCssClass="MaskedEditError" MaskType="Date" CultureName="en-US" DisplayMoney="Left"
                                AcceptNegative="Left" ErrorTooltipEnabled="True" AutoComplete="false" />
                            <asp:RegularExpressionValidator ID="regValidFrom" runat="server" ControlToValidate="txtValidFrom"
                                ValidationExpression="(^(?:(?:(?:0?[13578]|1[02])(\/|-)31)|(?:(?:0?[1,3-9]|1[0-2])(\/|-)(?:29|30)))(\/|-)(?:(1[8-9]|[2-8]\d)\d\d)$|^(?:(?:0?[1-9]|1[0-2])(\/|-)(?:0?[1-9]|1\d|2[0-8]))(\/|-)(?:(1[8-9]|[2-8]\d)\d\d)$|^(0?2(\/|-)29)(\/|-)(?:(?:0[48]00|[13579][26]00|[2468][048]00)|(?:\d\d)?(?:0[48]|[2468][048]|[13579][26]))$)"
                                ErrorMessage="Invalid Valid From Date" Text="*" ValidationGroup="Save"></asp:RegularExpressionValidator>
                        </td>
                        <td>
                            Manager:
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlManager" runat="server">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Valid To:
                        </td>
                        <td>
                            <asp:TextBox ID="txtValidTo" runat="server"></asp:TextBox>
                            <AjaxToolKit:CalendarExtender ID="calValidTO" runat="server" PopupPosition="TopRight"
                                TargetControlID="txtValidTo" PopupButtonID="imgValidTo" />
                            <asp:ImageButton ID="imgValidTo" runat="server" ImageUrl="~/images/Calendar_scheduleHS.png"
                                CausesValidation="False" />
                            <AjaxToolKit:MaskedEditExtender ID="mskEditTo" runat="server" TargetControlID="txtValidTo"
                                Mask="99/99/9999" MessageValidatorTip="true" OnFocusCssClass="MaskedEditFocus"
                                OnInvalidCssClass="MaskedEditError" MaskType="Date" DisplayMoney="Right" AcceptNegative="Left"
                                AutoComplete="false" ErrorTooltipEnabled="True" />
                            <asp:RegularExpressionValidator ID="regValidTo" runat="server" ControlToValidate="txtValidTo"
                                ValidationExpression="(^(?:(?:(?:0?[13578]|1[02])(\/|-)31)|(?:(?:0?[1,3-9]|1[0-2])(\/|-)(?:29|30)))(\/|-)(?:(1[8-9]|[2-8]\d)\d\d)$|^(?:(?:0?[1-9]|1[0-2])(\/|-)(?:0?[1-9]|1\d|2[0-8]))(\/|-)(?:(1[8-9]|[2-8]\d)\d\d)$|^(0?2(\/|-)29)(\/|-)(?:(?:0[48]00|[13579][26]00|[2468][048]00)|(?:\d\d)?(?:0[48]|[2468][048]|[13579][26]))$)"
                                ErrorMessage="Invalid Valid To Date" Text="*" ValidationGroup="Save"></asp:RegularExpressionValidator>
                        </td>
                        <td>
                            UserID:
                        </td>
                        <td>
                            <asp:TextBox ID="txtUserID" runat="server" Width="246px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td nowrap>
                            Account Setup QC (%):
                        </td>
                        <td>
                            <asp:ObjectDataSource ID="objAcctSetup" runat="server" SelectMethod="GetLookUpActiveData"
                                TypeName="ZurichNA.AIS.Business.Logic.BLAccess">
                                <SelectParameters>
                                    <asp:Parameter DefaultValue="ACCOUNT SETUP QC" Name="lookUpTypeName" Type="String" />
                                </SelectParameters>
                            </asp:ObjectDataSource>
                            <asp:DropDownList ID="ddlActSetup" runat="server" DataSourceID="objAcctSetup" DataTextField="LookUpName"
                                DataValueField="LookUpID">
                            </asp:DropDownList>
                        </td>
                        <td nowrap>
                            Adjustment QC (%) :
                        </td>
                        <td>
                            <asp:ObjectDataSource ID="objadj" runat="server" SelectMethod="GetLookUpActiveData"
                                TypeName="ZurichNA.AIS.Business.Logic.BLAccess">
                                <SelectParameters>
                                    <asp:Parameter DefaultValue="ADJUSTMENT SETUP QC" Name="lookUpTypeName" Type="String" />
                                </SelectParameters>
                            </asp:ObjectDataSource>
                            <asp:DropDownList ID="ddlAdj" runat="server" DataSourceID="objadj" DataTextField="LookUpName"
                                DataValueField="LookUpID">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td nowrap>
                            Aries QC (%):
                        </td>
                        <td>
                            <asp:ObjectDataSource ID="objAriesQC" runat="server" SelectMethod="GetLookUpActiveData"
                                TypeName="ZurichNA.AIS.Business.Logic.BLAccess">
                                <SelectParameters>
                                    <asp:Parameter DefaultValue="ACCOUNT SETUP QC" Name="lookUpTypeName" Type="String" />
                                </SelectParameters>
                            </asp:ObjectDataSource>
                            <asp:DropDownList ID="ddlAries" runat="server" DataSourceID="objAriesQC" DataTextField="LookUpName"
                                DataValueField="LookUpID">
                            </asp:DropDownList>
                        </td>
                        <td nowrap>
                        </td>
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4" style="text-align: center">
                            <asp:Button ID="btnSave" Enabled="false" Text="Save" ValidationGroup="Save" runat="server"
                                OnClientClick="summary()" OnClick="btnSave_Click" ToolTip="Please click here to save Internal Contact" />
                            <asp:Button ID="btnCopy" ToolTip="Please click here to copy selected Internal Contact"
                                Enabled="false" runat="server" Text="Copy" OnClick="btnCopy_Click" />
                            <asp:Button ID="btnCancel" ToolTip="Please click here to Cancel" runat="server" Text="Cancel"
                                OnClick="btnCancel_Click" />
                            &nbsp;&nbsp;&nbsp;
                        </td>
                    </tr>
                </table>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
