<%@ Page Language="C#" MasterPageFile="~/Retro.master" Inherits="AcctMgmt_DcoumentTracking"
    Title="Untitled Page" CodeBehind="DocumentTracking.aspx.cs" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Src="~/App_Shared/AccountList.ascx" TagName="AccountsList" TagPrefix="AL" %>
<%@ Register Src="~/App_Shared/NonAISAcctList.ascx" TagName="Nonaislist" TagPrefix="Nonais" %>
<asp:Content ID="HeaderPlaceHolder" ContentPlaceHolderID="HeaderPlaceHolder" runat="Server">
    <table width="910" style="text-align: right">
        <tr>
            <td align="left">
                <asp:Label ID="lblRetroInfo" runat="server" Text="Document Tracking" CssClass="h1"></asp:Label>
            </td>
            <td align="right">
                <asp:Button ID="btnAdd" runat="server" Text="Add New" OnClick="btnAdd_Click" ToolTip="Please click here to insert a new record."
                    Enabled="false" />
            </td>
        </tr>
    </table>
</asp:Content>
<asp:Content ID="contentMaincontent" ContentPlaceHolderID="MainPlaceHolder" runat="Server">

    <script language="javascript" type="text/javascript">
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

    <div id="divdoctracking" runat="server">
        <table width="910px" id="tbldoctacking" runat="server">
            <tr>
                <td colspan="2">
                    <asp:ValidationSummary ID="valSavedt" runat="server" ValidationGroup="save" BorderColor="Red"
                        BorderStyle="Solid" BorderWidth="1" />
                    <asp:ValidationSummary ID="valUpdatedt" runat="server" ValidationGroup="update" BorderColor="Red"
                        BorderStyle="Solid" BorderWidth="1" />
                </td>
            </tr>
            <tr>
                <td id="tdRetro" runat="server">
                    <table id="tblaacnts" runat="server">
                        <tr>
                            <td style="width: 90px; padding-top: 22px;">
                                <asp:RadioButton ID="radRetro" runat="server" GroupName="radDC" OnCheckedChanged="radRetro_CheckedChanged"
                                    AutoPostBack="True" />
                                Retro
                            </td>
                            <td align="left" id="tdaccntlst" runat="server">
                                <div id="divaactlst" runat="server">
                                    <AL:AccountsList ID="ucAccountList" runat="server" />
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 90px; padding-top: 20px;">
                                <asp:RadioButton ID="radLSI" runat="server" GroupName="radDC" OnCheckedChanged="radLSI_CheckedChanged"
                                    AutoPostBack="True" />
                                Non-AIS
                            </td>
                            <td align="left">
                                <table>
                                    <tr>
                                        <td>
                                            <div id="divnonais" runat="server">
                                                <Nonais:Nonaislist ID="ucNonaisacctlst" runat="server" />
                                            </div>
                                        </td>
                                        <td style="padding-top: 13px;">
                                            &nbsp;&nbsp;&nbsp;<asp:Label ID="lblNonais" Text="New Non-AIS Account" runat="server"></asp:Label><br />
                                            &nbsp;&nbsp;&nbsp;<asp:TextBox ID="txtNonais" runat="server" Width="156px" MaxLength="50"></asp:TextBox>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </td>
                <td align="center" style="padding-top: 45px;">
                    <asp:Button ID="btnSearch" runat="server" Text="Search" OnClick="btnSearch_Click"
                        Enabled="false" />
                </td>
            </tr>
        </table>
    </div>
    <div style="padding-top: 5px">
        <asp:Label ID="lblDocuments" Visible="false" runat="server" Text="Document Tracking"
            CssClass="h2"></asp:Label></div>
    <div id="divDocumenttracking" runat="server" style="overflow: auto; height: 76px;"
        class="panelcontents">
        <asp:AISListView ID="lstDocumentTracking" runat="server" EnableTheming="true" DataKeyNames="CUSTOMER_DOCUMENT_ID"
            OnItemCommand="CommandList" OnItemDataBound="DataBoundList" Visible="true" OnSelectedIndexChanging="lstDocumentTracking_SelectedIndexChanging"
            OnSorting="lstDocuments_Sorting">
            <LayoutTemplate>
                <table width="910px">
                    <tr>
                        <th>
                            Edit
                        </th>
                        <th align="center">
                            <asp:LinkButton ID="SortByName" runat="server" CommandArgument="Name" CommandName="Sort"
                                Text=" Forms Received" />
                            <asp:Image ID="imgSortByName" runat="server" ImageUrl="~/images/Ascending.gif" ToolTip="Ascending"
                                Visible="false" />
                        </th>
                        <th>
                            Date Received
                        </th>
                        <th>
                            Program Effective Date
                        </th>
                        <th>
                            Program Expiry Date
                        </th>
                        <th>
                            Date Entered
                        </th>
                        <th>
                            QC Date
                        </th>
                        <th>
                            Valuation Date
                        </th>
                    </tr>
                    <tr id="itemPlaceholder" runat="server">
                    </tr>
                </table>
            </LayoutTemplate>
            <EmptyDataTemplate>
                <table width="910px" class="panelContents">
                    <tr>
                        <th>
                            Edit
                        </th>
                        <th align="center">
                            Forms Received
                        </th>
                        <th>
                            Date Received
                        </th>
                        <th>
                            Program Effective Date
                        </th>
                        <th>
                            Program Expiry Date
                        </th>
                        <th>
                            Date Entered
                        </th>
                        <th>
                            QC Date
                        </th>
                        <th>
                            Valuation Date
                        </th>
                    </tr>
                    <tr>
                        <td align="center" colspan="8" class="ItemTemplate">
                            <asp:Label ID="lblEmptyMessage" Text="---No Records Found ---" Font-Bold="true" runat="server"
                                Style="text-align: center" />
                        </td>
                    </tr>
                    <tr id="itemPlaceholder" runat="server">
                    </tr>
                </table>
            </EmptyDataTemplate>
            <ItemTemplate>
                <tr class="ItemTemplate" runat="server" id="trItemTemplate">
                    <td>
                        <asp:HiddenField ID="hdforms" runat="server" Value='<%# Eval("FORM_NAME") %>' />
                        <asp:LinkButton ID="lnkSelect" CommandArgument='<%# Bind("CUSTOMER_DOCUMENT_ID") %>'
                            CommandName="Select" runat="server" Text="Select"></asp:LinkButton>
                    </td>
                    <td>
                        <%# Eval("FORM_NAME")%>
                    </td>
                    <td>
                        <%# Eval("RECEVD_DATE","{0:d}")%>
                    </td>
                    <td>
                        <%# Eval("PROGM_EFF_DATE", "{0:d}")%>
                    </td>
                    <td>
                        <%# Eval("PROG_EXP_DATE", "{0:d}")%>
                    </td>
                    <td>
                        <%# Eval("ENTRY_DATE", "{0:d}")%>
                    </td>
                    <td>
                        <%# Eval("QUALITY_CNTRL_DATE", "{0:d}")%>
                    </td>
                    <td>
                        <%# Eval("VALUATION_DATE", "{0:d}")%>
                    </td>
                </tr>
            </ItemTemplate>
            <AlternatingItemTemplate>
                <tr class="AlternatingItemTemplate" runat="server" id="trItemTemplate">
                    <td>
                        <asp:HiddenField ID="hdforms" runat="server" Value='<%# Eval("FORM_NAME") %>' />
                        <asp:LinkButton ID="lnkSelect" CommandArgument='<%# Bind("CUSTOMER_DOCUMENT_ID") %>'
                            CommandName="Select" runat="server" Text="Select"></asp:LinkButton>
                    </td>
                    <td>
                        <%# Eval("FORM_NAME")%>
                    </td>
                    <td>
                        <%# Eval("RECEVD_DATE", "{0:d}")%>
                    </td>
                    <td>
                        <%# Eval("PROGM_EFF_DATE", "{0:d}")%>
                    </td>
                    <td>
                        <%# Eval("PROG_EXP_DATE", "{0:d}")%>
                    </td>
                    <td>
                        <%# Eval("ENTRY_DATE", "{0:d}")%>
                    </td>
                    <td>
                        <%# Eval("QUALITY_CNTRL_DATE", "{0:d}")%>
                    </td>
                    <td>
                        <%# Eval("VALUATION_DATE", "{0:d}")%>
                    </td>
                </tr>
            </AlternatingItemTemplate>
        </asp:AISListView>
    </div>
    <!-- DaataSource Region -->
    <div>
        <asp:ObjectDataSource ID="objTrackingForms" runat="server" SelectMethod="GetLookUpActiveData"
            TypeName="ZurichNA.AIS.Business.Logic.BLAccess">
            <SelectParameters>
                <asp:Parameter DefaultValue="TRACKING FORMS" Name="lookUpTypeName" Type="String" />
            </SelectParameters>
        </asp:ObjectDataSource>
        <asp:ObjectDataSource ID="objTrackingIssues" runat="server" SelectMethod="GetLookUpActiveData"
            TypeName="ZurichNA.AIS.Business.Logic.BLAccess">
            <SelectParameters>
                <asp:Parameter DefaultValue="TRACKING ISSUES" Name="lookUpTypeName" Type="String" />
            </SelectParameters>
        </asp:ObjectDataSource>
        <asp:ObjectDataSource ID="BUOfficeDataSource" runat="server" SelectMethod="GetBUOffForLookups"
            TypeName="ZurichNA.AIS.Business.Logic.BusinessUnitOfficeBS"></asp:ObjectDataSource>
        <!--<asp:ObjectDataSource ID="objPersonDatasource" runat="server" SelectMethod="FillPersons"
            TypeName="ZurichNA.AIS.Business.Logic.PersonBS"></asp:ObjectDataSource> -->
        <asp:ObjectDataSource ID="PersonDataSource" runat="server" SelectMethod="getPersonNames"
            TypeName="ZurichNA.AIS.Business.Logic.AssignContactsBS">
            <SelectParameters>
                <asp:Parameter DefaultValue="0" Name="ContTypId" Type="Int32" />
            </SelectParameters>
        </asp:ObjectDataSource>
        <asp:ObjectDataSource ID="ContactNamesDataSource" runat="server" SelectMethod="getPersonNames"
            TypeName="ZurichNA.AIS.Business.Logic.AssignContactsBS">
            <SelectParameters>
                <asp:Parameter DefaultValue="0" Name="ContTypId" Type="Int32" />
            </SelectParameters>
        </asp:ObjectDataSource>
    </div>
    <div style="padding-top: 10px" id="divHeading" runat="server" visible="false">
        <asp:Label ID="lblDocumentTracking" runat="server" Text="Document Tracking Details"
            CssClass="h2" Visible="false"></asp:Label>&nbsp;&nbsp;&nbsp;&nbsp;<asp:LinkButton
                ID="lnkClose" runat="server" Text="Close" OnClick="lnkClose_Click"></asp:LinkButton></div>
    <asp:Panel BorderColor="Black" ID="pnlDetails" Visible="false" BorderWidth="1" Width="910px"
        runat="server">
        <table width="100%" cellspacing="1">
            <tr style="padding-top: 25px;">
                <td>
                    <asp:HiddenField ID="hidindex" runat="server" Value="-1" />
                </td>
            </tr>
            <tr>
                <td>
                    Forms Received
                </td>
                <td>
                    <asp:DropDownList ID="ddlFormsRecieved" runat="server" DataSourceID="objTrackingForms"
                        DataTextField="LookUpName" DataValueField="LookUpID" AutoPostBack="true" OnSelectedIndexChanged="ddlFormsRecieved_SelectedIndexChanged">
                    </asp:DropDownList>
                    <asp:CompareValidator ID="Compareddlforms" runat="server" ControlToValidate="ddlFormsRecieved"
                        ValueToCompare="0" ErrorMessage="Please select the forms." Text="*" Operator="NotEqual"
                        ValidationGroup="save"></asp:CompareValidator>
                </td>
                <td>
                    Date Received
                </td>
                <td>
                    <asp:TextBox ID="txtDtrecieved" runat="server" Width="175px"></asp:TextBox>
                    <asp:ImageButton ID="imgDtrecieved" runat="server" ImageUrl="~/images/Calendar_scheduleHS.png"
                        CausesValidation="False" />
                    <ajaxToolkit:MaskedEditExtender ID="mskDaterecieved" runat="server" TargetControlID="txtDtrecieved"
                        Mask="99/99/9999" MessageValidatorTip="true" OnFocusCssClass="MaskedEditFocus"
                        OnInvalidCssClass="MaskedEditError" MaskType="Date" DisplayMoney="Left" AcceptNegative="Left"
                        ErrorTooltipEnabled="True" />
                    <asp:RequiredFieldValidator ID="reqtxtDtrecieved" runat="server" ControlToValidate="txtDtrecieved"
                        ValidationGroup="save" Text="*" ErrorMessage="Please enter Date Received">
                    </asp:RequiredFieldValidator>
                    <asp:RegularExpressionValidator ID="regDtrecieved" runat="server" ControlToValidate="txtDtrecieved"
                        ValidationExpression="(0?[1-9]|1[012])[- /.](0?[1-9]|[12][0-9]|3[01])[- /.](1[3-9]|2[01])\d\d"
                        ErrorMessage="Invalid Received Date" Text="*" ValidationGroup="save"></asp:RegularExpressionValidator>
                    <ajaxToolkit:CalendarExtender ID="txtDtreciev_CalendarExtender" runat="server" TargetControlID="txtDtrecieved"
                        PopupButtonID="imgDtrecieved">
                    </ajaxToolkit:CalendarExtender>
                </td>
            </tr>
            <tr>
                <td>
                    Program Effective Date
                </td>
                <td>
                    <asp:TextBox ID="txtPrgmeffdate" MaxLength="10" runat="server" Width="175px"></asp:TextBox>
                    <asp:ImageButton ID="imgPrgeffdate" runat="server" ImageUrl="~/images/Calendar_scheduleHS.png"
                        CausesValidation="False" />
                    <ajaxToolkit:MaskedEditExtender ID="mskPgmeffdate" runat="server" TargetControlID="txtPrgmeffdate"
                        Mask="99/99/9999" MessageValidatorTip="true" OnFocusCssClass="MaskedEditFocus"
                        OnInvalidCssClass="MaskedEditError" MaskType="Date" DisplayMoney="Left" AcceptNegative="Left"
                        ErrorTooltipEnabled="True" />
                    <asp:RegularExpressionValidator ID="regEffectivedate" runat="server" ControlToValidate="txtPrgmeffdate"
                        ValidationExpression="(0?[1-9]|1[012])[- /.](0?[1-9]|[12][0-9]|3[01])[- /.](1[3-9]|2[01])\d\d"
                        ErrorMessage="Invalid Program Effective Date" Text="*" ValidationGroup="save"></asp:RegularExpressionValidator>
                    <ajaxToolkit:CalendarExtender ID="txtPrgmdt_CalendarExtender" runat="server" TargetControlID="txtPrgmeffdate"
                        PopupButtonID="imgPrgeffdate">
                    </ajaxToolkit:CalendarExtender>
                    <asp:RequiredFieldValidator ID="reqpgmeffdate" ControlToValidate="txtPrgmeffdate"
                        runat="server" Text="*" ErrorMessage="Please enter Program effective date" ValidationGroup="save"></asp:RequiredFieldValidator>
                </td>
                <td>
                    Program Expiry
                </td>
                <td>
                    <asp:TextBox ID="txtPrgmexpdate" MaxLength="10" runat="server" Width="175px" ValidationGroup="save"></asp:TextBox>
                    <asp:ImageButton ID="imgExpirydate" runat="server" ImageUrl="~/images/Calendar_scheduleHS.png"
                        CausesValidation="False" />
                    <ajaxToolkit:MaskedEditExtender ID="mskPgmexpdate" runat="server" TargetControlID="txtPrgmexpdate"
                        Mask="99/99/9999" MessageValidatorTip="true" OnFocusCssClass="MaskedEditFocus"
                        OnInvalidCssClass="MaskedEditError" MaskType="Date" DisplayMoney="Left" AcceptNegative="Left"
                        ErrorTooltipEnabled="True" />
                    <asp:RegularExpressionValidator ID="regexpdate" runat="server" ControlToValidate="txtPrgmexpdate"
                        ValidationExpression="(0?[1-9]|1[012])[- /.](0?[1-9]|[12][0-9]|3[01])[- /.](1[3-9]|2[01])\d\d"
                        ErrorMessage="Program Expiry Date is invalid" Text="*" ValidationGroup="save"></asp:RegularExpressionValidator>
                    <ajaxToolkit:CalendarExtender ID="txtPrgmexp_CalendarExtender" runat="server" TargetControlID="txtPrgmexpdate"
                        Format="MM/dd/yyyy" PopupPosition="BottomRight" PopupButtonID="imgExpirydate">
                    </ajaxToolkit:CalendarExtender>
                    <asp:RequiredFieldValidator ID="reqexpdate" runat="server" ControlToValidate="txtPrgmexpdate"
                        ValidationGroup="save" ErrorMessage="Please enter Program expiration date" Text="*">
                    </asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td>
                    Date Entered
                </td>
                <td>
                    <asp:TextBox ID="txtDateentered" MaxLength="10" runat="server" Width="175px"></asp:TextBox>
                    <asp:ImageButton ID="imgdtEntered" runat="server" ImageUrl="~/images/Calendar_scheduleHS.png"
                        CausesValidation="False" />
                    <ajaxToolkit:MaskedEditExtender ID="mskDateentered" runat="server" TargetControlID="txtDateentered"
                        Mask="99/99/9999" MessageValidatorTip="true" OnFocusCssClass="MaskedEditFocus"
                        OnInvalidCssClass="MaskedEditError" MaskType="Date" DisplayMoney="Left" AcceptNegative="Left"
                        ErrorTooltipEnabled="True" />
                    <asp:RegularExpressionValidator ID="regdtentered" runat="server" ControlToValidate="txtDateentered"
                        ValidationExpression="(0?[1-9]|1[012])[- /.](0?[1-9]|[12][0-9]|3[01])[- /.](1[3-9]|2[01])\d\d"
                        ErrorMessage="Date Entered is invalid." Text="*" ValidationGroup="save"></asp:RegularExpressionValidator>
                    <ajaxToolkit:CalendarExtender ID="txtDtentered_CalendarExtender" runat="server" Enabled="True"
                        TargetControlID="txtDateentered" Format="MM/dd/yyyy" PopupPosition="BottomRight"
                        PopupButtonID="imgdtEntered">
                    </ajaxToolkit:CalendarExtender>
                </td>
                <td>
                    QC Date
                </td>
                <td>
                    <asp:TextBox ID="txtQcdate" MaxLength="10" runat="server" Width="175px"></asp:TextBox>
                    <asp:ImageButton ID="imgQCdate" runat="server" ImageUrl="~/images/Calendar_scheduleHS.png"
                        CausesValidation="False" />
                    <ajaxToolkit:MaskedEditExtender ID="mskQCdate" runat="server" TargetControlID="txtQcdate"
                        Mask="99/99/9999" MessageValidatorTip="true" OnFocusCssClass="MaskedEditFocus"
                        OnInvalidCssClass="MaskedEditError" MaskType="Date" DisplayMoney="Left" AcceptNegative="Left"
                        ErrorTooltipEnabled="True" />
                    <asp:RegularExpressionValidator ID="regQCdate" runat="server" ControlToValidate="txtQcdate"
                        ValidationExpression="(0?[1-9]|1[012])[- /.](0?[1-9]|[12][0-9]|3[01])[- /.](1[3-9]|2[01])\d\d"
                        ErrorMessage="QC Date is invalid." Text="*" ValidationGroup="save"></asp:RegularExpressionValidator>
                    <ajaxToolkit:CalendarExtender ID="txtQCDt_CalendarExtender" runat="server" Enabled="True"
                        TargetControlID="txtQcdate" Format="MM/dd/yyyy" PopupPosition="BottomRight" PopupButtonID="imgQCdate">
                    </ajaxToolkit:CalendarExtender>
                    <asp:CompareValidator ID="compareQcdate" runat="server" ControlToCompare="txtdtvalidate"
                        ControlToValidate="txtQcdate" Operator="GreaterThan" Type="Date" Text="*" ErrorMessage="QC date cannnot be less than current date."
                        ValidationGroup="save"></asp:CompareValidator>
                </td>
            </tr>
            <tr>
                <td>
                    Valuation Date
                </td>
                <td>
                    <asp:TextBox ID="txtValuationdate" runat="server" Width="175px"></asp:TextBox>
                    <asp:ImageButton ID="imgValnDate" runat="server" ImageUrl="~/images/Calendar_scheduleHS.png"
                        CausesValidation="False" />
                    <ajaxToolkit:MaskedEditExtender ID="mskEditTo" runat="server" TargetControlID="txtValuationdate"
                        Mask="99/99/9999" MessageValidatorTip="true" OnFocusCssClass="MaskedEditFocus"
                        OnInvalidCssClass="MaskedEditError" MaskType="Date" DisplayMoney="Left" AcceptNegative="Left"
                        ErrorTooltipEnabled="True" />
                    <asp:RegularExpressionValidator ID="regvalndate" runat="server" ControlToValidate="txtValuationdate"
                        ValidationExpression="(0?[1-9]|1[012])[- /.](0?[1-9]|[12][0-9]|3[01])[- /.](1[3-9]|2[01])\d\d"
                        ErrorMessage="Valuation Date is invalid." Text="*" ValidationGroup="save"></asp:RegularExpressionValidator>
                    <ajaxToolkit:CalendarExtender ID="txtVlndt_CalendarExtender" runat="server" Enabled="True"
                        TargetControlID="txtValuationdate" Format="MM/dd/yyyy" PopupPosition="BottomRight"
                        PopupButtonID="imgValnDate">
                    </ajaxToolkit:CalendarExtender>
                </td>
                <td>
                    Amount
                </td>
                <td>
                    <asp:AISAmountTextbox ID="txtAmount" AllowNegetive="true" runat="server" Width="175px" ></asp:AISAmountTextbox>
                    <%-- <asp:TextBox ID="TextBox1" runat="server" Width="175px" onblur="FormatNumNoDecAmt(this,11)"  onfocus="RemoveCommas(this)" MaxLength="11"></asp:TextBox>
                   <AjaxToolKit:FilteredTextBoxExtender runat="server" TatrgetControlID="txtAmount" FilterType="Custom"
                                ValidChars="0123456789," ID="fltAmount">
                            </AjaxToolKit:FilteredTextBoxExtender>
--%>
                </td>
            </tr>
            <tr>
                <td>
                    20% QC
                </td>
                <td>
                    <asp:CheckBox ID="chkQC" runat="server" />
                </td>
                <td>
                    QC By
                </td>
                <td>
                    <asp:DropDownList ID="ddlQCBy" runat="server" DataSourceID="ContactNamesDataSource"
                        DataTextField="FULLNAME" DataValueField="PERSON_ID">
                    </asp:DropDownList>
                    <asp:CompareValidator ID="compddlQC" runat="server" ControlToValidate="ddlQCBy" ValueToCompare="0"
                        ErrorMessage="Please select the QC person." Text="*" Operator="NotEqual" ValidationGroup="save"></asp:CompareValidator>
                </td>
            </tr>
            <tr>
                <td>
                    CFS
                </td>
                <td>
                    <asp:DropDownList ID="ddlCfs" runat="server" DataTextField="FULLNAME" DataValueField="PERSON_ID"
                        DataSourceID="ContactNamesDataSource" Width="275px">
                    </asp:DropDownList>
                    <asp:CompareValidator ID="compddlCfs" runat="server" ControlToValidate="ddlCfs" ValueToCompare="0"
                        ErrorMessage="Please select the CFS." Text="*" Operator="NotEqual" ValidationGroup="save"></asp:CompareValidator>
                </td>
                <td>
                    BusinessUnit/Office
                </td>
                <td>
                    <asp:DropDownList ID="ddlBUoffice" runat="server" DataSourceID="BUOfficeDataSource"
                        DataTextField="LookupName" DataValueField="LookUpID">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>
                    Issues
                </td>
                <td>
                    <asp:Panel ID="pnlCustmrdoc" runat="server" ScrollBars="Auto" Height="50px" Width="275px">
                        <asp:CheckBoxList ID="chklstTrackingissues" runat="server" ForeColor="Black" ReadOnly="true"
                            BorderWidth="1" BorderColor="Black" DataSourceID="objTrackingIssues" DataTextField="LookUpName"
                            DataValueField="LookUpID" BorderStyle="Inset" OnSelectedIndexChanged="chklstTrackingissues_SelectedIndexChanged">
                        </asp:CheckBoxList>
                    </asp:Panel>
                </td>
                <td>
                    Comments
                </td>
                <td>
                    <asp:TextBox ID="txtComments" runat="server" TextMode="MultiLine" MaxLength="512"></asp:TextBox>
                    <ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server"
                        TargetControlID="txtComments" FilterType="Custom" InvalidChars="'" FilterMode="InvalidChars" />
                </td>
            </tr>
            <tr style="height: 20px;">
                <td>
                </td>
                <td>
                    <asp:TextBox ID="txtdtvalidate" runat="server" Visible="true" Width="0" Height="0"
                        BorderStyle="None"></asp:TextBox>
                </td>
            </tr>
        </table>
    </asp:Panel>
    <asp:Panel ID="pnlNonaissave" runat="server" Visible="false">
        <table width="910px;">
            <tr>
                <td align="right">
                    <asp:Button ID="btnSave" runat="server" Text=" Save " OnClick="btnSave_Click" ValidationGroup="save" />
                </td>
                <td align="left">
                    &nbsp;<asp:Button ID="btnCancel" Text="Cancel" runat="server" OnClick="btnCancel_Click" />
                </td>
            </tr>
        </table>
    </asp:Panel>
</asp:Content>
