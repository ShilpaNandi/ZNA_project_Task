<%@ Page Language="C#" MasterPageFile="~/Retro.master" AutoEventWireup="true" Inherits="AIS.WebSite.AcctMgmt.AcctSetup_AcctSetupProcChklst"
    Title="AcctSetupProcChklst" CodeBehind="AcctSetupProcChklst.aspx.cs" %>

<%@ Register Src="../App_Shared/AccountInfoHeader.ascx" TagName="AccountInfoHeader"
    TagPrefix="AI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="AjaxToolkit" %>
<%@ Register Src="../App_Shared/SaveCancel.ascx" TagName="SaveCancel" TagPrefix="SC" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeaderPlaceHolder" runat="Server">
    <table width="910" style="text-align: right">
        <tr>
            <td align="left">
                <div id="QC">
                    <asp:Label ID="lblAcctSetupProcChklst" runat="server" Text="Account Setup Processing Check List"
                        CssClass="h1"></asp:Label>
                </div>
                <%--<div id="QC">
                    <asp:Label ID="lblQC" runat="server" Text="Account Setup QC"  Visible="false" CssClass="h1"></asp:Label>
                    </div>--%>
            </td>
            <td align="right">
                <asp:HiddenField ID="hidTabindex" runat="server" Value="1" />
                <div id="UCSave" runat="server">
                    <SC:SaveCancel ID="ucSaveCancel" runat="server" Visible="true" />
                </div>
            </td>
        </tr>
    </table>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainPlaceHolder" runat="Server">

    <script language="javascript" type="text/javascript">
        function HideButtons()
        {
        $get('<%=UCSave.ClientID %>').style.display="none";
        
        $get('<%=lblAcctSetupProcChklst.ClientID%>').innerText="Account Setup QC";
        $get('<%=hidTabindex.ClientID %>').value=1;
        }
        function ShowButtons()
        {
        $get('<%=UCSave.ClientID %>').style.display="block";
        $get('<%=lblAcctSetupProcChklst.ClientID%>').innerText="Account Setup Processing Check List";
        $get('<%=hidTabindex.ClientID %>').value=0;
        }
   
    </script>

    <table>
        <tr>
            <td>
                <AI:AccountInfoHeader ID="ucAccountInfoHeader1" runat="server" />
            </td>
        </tr>
    </table>
    <asp:ValidationSummary ID="ValSumSave" ValidationGroup="Save" CssClass="ValidationSummary"
        runat="server"></asp:ValidationSummary>
    <asp:ValidationSummary ID="valSumSaveDetail" CssClass="ValidationSummary" ValidationGroup="QCDetailSave"
        runat="server"></asp:ValidationSummary>
    <table>
        <tr>
            <td>
            </td>
        </tr>
        <tr>
            <td>
                <AjaxToolkit:TabContainer ID="TabConAcctSetupProcChklst" runat="server" ActiveTabIndex="1"
                    CssClass="VariableTabs" SkinID="tabVariable">
                    <AjaxToolkit:TabPanel runat="server" ID="tblpnlAccProcChklst">
                        <HeaderTemplate>
                            <asp:Label ID="lblAccProc" runat="server" Width="150px" Text="Account Setup Processing Checklist"
                                onclick="ShowButtons()"></asp:Label>
                        </HeaderTemplate>
                        <ContentTemplate>
                            <table>
                                <caption>
                                    <br />
                                    <tr>
                                        <td>
                                            <asp:Panel ID="pnlAccProc" runat="server" Height="310px" ScrollBars="Auto" Width="910px">
                                                <asp:AISListView ID="lstAcctSetupProcChklst" runat="server" OnItemCommand="CommandList"
                                                    OnItemInserting="InsertList">
                                                    <EmptyDataTemplate>
                                                        <table id="Table1" class="panelContents" runat="server" width="98%">
                                                            <tr class="LayoutTemplate">
                                                                <th style="width: 75%">
                                                                    ACCOUNT SETUP-PROCESSING CHECKLIST ITEMS
                                                                </th>
                                                                <th style="width: 25%">
                                                                    SELECT
                                                                </th>
                                                            </tr>
                                                        </table>
                                                        <table width="98%">
                                                            <tr id="Tr1" runat="server" class="ItemTemplate">
                                                                <td align="center">
                                                                    <asp:Label ID="lblEmptyMessage" Text="---No Records Found ---" Font-Bold="true" Width="600px"
                                                                        runat="server" Style="text-align: center" />
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </EmptyDataTemplate>
                                                    <LayoutTemplate>
                                                        <table id="Table1" class="panelContents" runat="server" width="98%">
                                                            <tr class="LayoutTemplate">
                                                                <th style="width: 75%">
                                                                    ACCOUNT SETUP-PROCESSING CHECKLIST ITEMS
                                                                </th>
                                                                <th style="width: 25%">
                                                                    SELECT
                                                                </th>
                                                            </tr>
                                                            <tr id="itemPlaceholder" runat="server">
                                                            </tr>
                                                        </table>
                                                    </LayoutTemplate>
                                                    <AlternatingItemTemplate>
                                                        <tr id="Tr2" runat="server" class="AlternatingItemTemplate">
                                                            <td align="left">
                                                                <asp:Label ID="lblhidQualitycntrl" runat="server" Text='<% # Bind("LOOKUPID")%>'
                                                                    Visible="false"></asp:Label>
                                                                <%# Eval("ChkLstItems")%>
                                                            </td>
                                                            <td>
                                                            <asp:Label ID="lblchklistcd" runat="server" Text='<% # Bind("CHKLIST_STS_CD")%>' Visible="false"></asp:Label>
                                                                <asp:CheckBox ID="chkSelectAcctProc" GroupName="Select" runat="server" Checked='<% # Bind("ACTIVE")%>' Visible="false"/>
                                                                <asp:DropDownList ID="ddlSelectAcctProc" runat="server" AutoPostBack="false" Width="100px" >
                                                                 <asp:ListItem Text="(Select)" Value="0"></asp:ListItem>
                                                                 <asp:ListItem Text="Yes" Value="Yes"></asp:ListItem>
                                                                 <asp:ListItem Text="No" Value="No" Selected="True"></asp:ListItem>
                                                                 <asp:ListItem Text="N/A" Value="N/A"></asp:ListItem>
                                                                  </asp:DropDownList>
                                                            </td>
                                                        </tr>
                                                    </AlternatingItemTemplate>
                                                    <ItemTemplate>
                                                        <tr id="Tr1" runat="server" class="ItemTemplate">
                                                            <td align="left">
                                                                <asp:Label ID="lblhidQualitycntrl" runat="server" Text='<% # Bind("LOOKUPID")%>'
                                                                    Visible="false"></asp:Label>
                                                                <%# Eval("ChkLstItems")%>
                                                            </td>
                                                            <td>
                                                            <asp:Label ID="lblchklistcd" runat="server" Text='<% # Bind("CHKLIST_STS_CD")%>' Visible="false"></asp:Label>
                                                                <asp:CheckBox ID="chkSelectAcctProc" runat="server" GroupName="Select" Checked='<% # Bind("ACTIVE")%>'  Visible="false"/>
                                                                <asp:DropDownList ID="ddlSelectAcctProc" runat="server" AutoPostBack="false" Width="100px"  >
                                                                 <asp:ListItem Text="(Select)" Value="0"></asp:ListItem>
                                                                 <asp:ListItem Text="Yes" Value="Yes"></asp:ListItem>
                                                                 <asp:ListItem Text="No" Value="No" Selected="True"></asp:ListItem>
                                                                 <asp:ListItem Text="N/A" Value="N/A"></asp:ListItem>
                                                                  </asp:DropDownList>
                                                            </td>
                                                        </tr>
                                                    </ItemTemplate>
                                                </asp:AISListView>
                                            </asp:Panel>
                                        </td>
                                    </tr>
                                </caption>
                            </table>
                        </ContentTemplate>
                    </AjaxToolkit:TabPanel>
                    <AjaxToolkit:TabPanel runat="server" ID="tblpnlLtype">
                        <HeaderTemplate>
                            <asp:Label ID="lblAcctQc" runat="server" Text="Account Setup QC" onclick="HideButtons()"></asp:Label>
                        </HeaderTemplate>
                        <ContentTemplate>
                            <table width="100%">
                                <tr>
                                    <td>
                                        <asp:Label ID="lblAccQc" runat="server" Text="Account Setup QC" CssClass="h3"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Panel ID="pnlAcctSetupQC" runat="server" Height="100px" BorderColor="Black"
                                            BorderWidth="1" ScrollBars="Auto" Width="910px">
                                            <table width="100%">
                                                <tr>
                                                    <td align="right" style="width: 120px; padding-right: 10px">
                                                        QC Date:
                                                    </td>
                                                    <td>
                                                        <asp:TextBox runat="server" ID="txtQCDate" ValidationGroup="Save" />
                                                        <AjaxToolkit:CalendarExtender ID="CalendarExtender1" runat="server" PopupButtonID="imgQCDate"
                                                            TargetControlID="txtQCDate" PopupPosition="TopRight" />
                                                        <asp:ImageButton ID="imgQCDate" runat="server" ImageUrl="~/images/Calendar_scheduleHS.png"
                                                            CausesValidation="False" />
                                                        <asp:RequiredFieldValidator ID="reqDate" runat="server" ErrorMessage="Please enter QC Date"
                                                            ValidationGroup="Save" ControlToValidate="txtQCDate" Text="*"></asp:RequiredFieldValidator>
                                                        <AjaxToolkit:MaskedEditExtender ID="mskReviewDate" runat="server" TargetControlID="txtQCDate"
                                                            Mask="99/99/9999" MessageValidatorTip="true" OnFocusCssClass="MaskedEditFocus"
                                                            OnInvalidCssClass="MaskedEditError" MaskType="Date" DisplayMoney="Left" AcceptNegative="Left"
                                                            ErrorTooltipEnabled="True" />
                                                        <AjaxToolkit:MaskedEditValidator Display="Dynamic" ID="MskValReviewDate" runat="server"
                                                            ControlToValidate="txtQCDate" InvalidValueMessage="QC Date is invalid" InvalidValueBlurredMessage="*"
                                                            ControlExtender="mskReviewDate" ValidationGroup="Save"></AjaxToolkit:MaskedEditValidator>
                                                    </td>
                                                    <td rowspan="2" style="vertical-align: top">
                                                        Comments:
                                                    </td>
                                                    <td rowspan="2" style="vertical-align: top">
                                                        <asp:TextBox ID="txtComment" Width="350px" runat="server" TextMode="MultiLine" Rows="3" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="right" style="padding-right: 10px">
                                                        QC BY:
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblQCBy" runat="server" Text='<%# Bind("PersonName") %>'></asp:Label>
                                                    </td>
                                                </tr>
                                                <br />
                                                <tr>
                                                    <td colspan="3" align="right">
                                                        <asp:Button ID="btnSave" Text="Save" ValidationGroup="Save" OnClick="btnSave_Click"
                                                            runat="server" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </asp:Panel>
                                    </td>
                                </tr>
                            </table>
                            <table>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblAcctSetupQCDetails" runat="server" Text="Account Setup QC Details"
                                            CssClass="h3"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <table width="100%">
                                            <tr>
                                                <td>
                                                    <asp:Panel ID="pnlAcctSetupQCDetails" runat="server" Height="175px" ScrollBars="Auto"
                                                        Width="910px">
                                                        <asp:AISListView ID="lstAcctSetupQCDetails" runat="server" InsertItemPosition="FirstItem"
                                                            DataKeyNames="LookUpID" OnItemDataBound="DataBoundList" OnItemCommand="CommandList"
                                                            OnItemInserting="InsertList">
                                                            <LayoutTemplate>
                                                                <table id="Table1" class="panelContents" runat="server" width="98%">
                                                                    <tr class="LayoutTemplate">
                                                                        <th>
                                                                        </th>
                                                                        <th>
                                                                            QC Issues
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
                                                                <tr id="Tr1" runat="server" class="ItemTemplate">
                                                                    <td align="left">
                                                                    </td>
                                                                    <td align="left">
                                                                        <asp:HiddenField ID="hidQltyChkID" runat="server" />
                                                                        <%# Eval("CHKLISTNAME")%>
                                                                    </td>
                                                                    <td>
                                                                        <asp:HiddenField ID="hidActive" runat="server" Value='<%# Bind("ACTIVE")%>' />
                                                                        <asp:ImageButton ID="imgDisable" CommandArgument='<%# Bind("QualityControlChklst_ID") %>'
                                                                            runat="server" Enabled='<%# Eval("ENABLED")%>' ImageUrl='<%# Eval("ACTIVE").ToString()=="True"?"~/images/disabled.GIF":"~/images/enabled.GIF" %>'>
                                                                        </asp:ImageButton>
                                                                    </td>
                                                                </tr>
                                                            </ItemTemplate>
                                                            <AlternatingItemTemplate>
                                                                <tr id="Tr2" runat="server" class="AlternatingItemTemplate">
                                                                    <td align="left">
                                                                    </td>
                                                                    <td align="left">
                                                                        <asp:HiddenField ID="hidQltyChkID" runat="server" />
                                                                        <%# Eval("CHKLISTNAME")%>
                                                                    </td>
                                                                    <td >
                                                                        <asp:HiddenField ID="hidActive" runat="server" Value='<%# Bind("ACTIVE")%>' />
                                                                        <asp:ImageButton ID="imgDisable" CommandArgument='<%# Bind("QualityControlChklst_ID") %>'
                                                                            runat="server" Enabled='<%# Eval("ENABLED")%>' ImageUrl='<%# Eval("ACTIVE").ToString()=="True"?"~/images/disabled.GIF":"~/images/enabled.GIF" %>'>
                                                                        </asp:ImageButton>
                                                                    </td>
                                                                </tr>
                                                            </AlternatingItemTemplate>
                                                            <InsertItemTemplate>
                                                                <tr class="ItemTemplate" id="trItemTemplate">
                                                                    <td align="left">
                                                                        <asp:LinkButton ID="lnkSave" CommandName="Save" runat="server" ValidationGroup="QCDetailSave"
                                                                            Text="SAVE"></asp:LinkButton>
                                                                    </td>
                                                                    <td align="left">
                                                                        <asp:DropDownList ID="ddlQCIssues" runat="server" ValidationGroup="QCDetailSave" Width="700px">
                                                                        </asp:DropDownList>
                                                                        <asp:CompareValidator ID="compLkup" runat="server" ControlToValidate="ddlQCIssues"
                                                                            ValidationGroup="QCDetailSave" ValueToCompare="0" Text="*" ErrorMessage="Please select an Issue"
                                                                            Operator="NotEqual"></asp:CompareValidator>
                                                                    </td>
                                                                    <td>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                </tr>
                                                            </InsertItemTemplate>
                                                        </asp:AISListView>
                                                    </asp:Panel>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </ContentTemplate>
                    </AjaxToolkit:TabPanel>
                </AjaxToolkit:TabContainer>
            </td>
        </tr>
    </table>
</asp:Content>
