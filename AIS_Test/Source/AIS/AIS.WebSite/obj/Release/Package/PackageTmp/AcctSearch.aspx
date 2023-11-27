<%@ Page Language="C#" MasterPageFile="~/Retro.master" AutoEventWireup="true" Inherits="AcctSetup_AcctSearch"
    Title="Adjustment Invoicing System" CodeBehind="AcctSearch.aspx.cs" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/App_Shared/AccountList.ascx" TagPrefix="UC1" TagName="ddl" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeaderPlaceHolder" runat="Server">
    <asp:Label ID="GAILabel" runat="server" Text="Account Search" CssClass="h1"></asp:Label>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainPlaceHolder" runat="Server">

    <script language="javascript">
      window.history.forward(1);
    </script>

    <asp:UpdatePanel ID="upMainPanel" runat="server">
        <ContentTemplate>
            <table>
                <tr>
                    <td style="width: 111px">
                        <asp:Label ID="lblInsuredName" runat="server" Text="Account Name"></asp:Label>
                    </td>
                    <td style="width: 168px">
                        <asp:TextBox ID="txtInsuredName" runat="server" OnTextChanged="txtInsuredName_TextChanged" onKeyPress="if(window.event && window.event.keyCode == 13){ __doPostBack('ctl00$MainPlaceHolder$btnSearch', ''); return false; }"
                            Width="210px"></asp:TextBox>
                    </td>
                    <td>
                        <asp:Button ID="btnSearch" runat="server" Text="Search" OnClick="cmdSearch_OnClick"
                            Width="60px" ValidationGroup="AISE" />
                    </td>
                </tr>
                <tr>
                    <td style="width: 111px">
                        <asp:Label ID="Label3" runat="server" Text="BP Number"></asp:Label>
                    </td>
                    <td style="width: 168px">
                        <%--<cc1:FilteredTextBoxExtender TargetControlID="txtBPNumber" 
                    FilterType="Numbers,LowerCaseLetters,UpperCaseLetters"
                        ID="fteBPNumber" runat="server" />--%>
                        <asp:TextBox ID="txtBPNumber" runat="server" onKeyPress="if(window.event && window.event.keyCode == 13){ __doPostBack('ctl00$MainPlaceHolder$btnSearch', ''); return false; }" Width="210px"></asp:TextBox>
                        <cc1:MaskedEditExtender ID="mskBPNumber" runat="server" TargetControlID="txtBPNumber"
                            Mask="9999999999" MessageValidatorTip="true" OnFocusCssClass="MaskedEditFocus"
                            InputDirection="LeftToRight" OnInvalidCssClass="MaskedEditError" MaskType="None"
                            AcceptNegative="Left" ErrorTooltipEnabled="True" />
                    </td>
                    <td>
                    </td>
                </tr>
                <tr>
                    <td style="width: 111px">
                        <asp:Label ID="Label4" runat="server" Text="Account Number"></asp:Label>
                    </td>
                    <td style="width: 168px">
                        <cc1:FilteredTextBoxExtender TargetControlID="txtAccountNumber" FilterType="Numbers"
                            ID="fteAccountNumber" runat="server" />
                        <asp:TextBox ID="txtAccountNumber" onKeyPress="if(window.event && window.event.keyCode == 13){ __doPostBack('ctl00$MainPlaceHolder$btnSearch', ''); return false; }" runat="server" Width="210px"></asp:TextBox>
                    </td>
                    <td>
                        
                    </td>
                </tr>
                <tr>
                    <td style="width: 111px">
                        <asp:Label ID="Label5" runat="server" Text="SSCGID"></asp:Label>
                    </td>
                    <td style="width: 168px;">
                        <asp:TextBox ID="txtSSCGID" runat="server" onKeyPress="if(window.event && window.event.keyCode == 13){ __doPostBack('ctl00$MainPlaceHolder$btnSearch', ''); return false; }" Width="210px"></asp:TextBox>
                        <cc1:MaskedEditExtender ID="mskSSCGID" runat="server" TargetControlID="txtSSCGID"
                            Mask="9999999999" MessageValidatorTip="true" OnFocusCssClass="MaskedEditFocus"
                            InputDirection="LeftToRight" OnInvalidCssClass="MaskedEditError" MaskType="None"
                            AcceptNegative="Left" ErrorTooltipEnabled="True" />
                    </td>
                    <td style="">
                       <asp:Button ID="btnClear" runat="server" Text="Clear" OnClick="btnClear_Click" Width="60px" />
                    </td>
                </tr>
                 <tr>
                    <td style="height: 42px; width: 111px">
                        <asp:Label ID="Label1" runat="server" Text="Policy No"></asp:Label>
                    </td>
                    <td style="width: 168px; height: 42px;">
                        <asp:TextBox ID="txtPolicyNo" onKeyPress="if(window.event && window.event.keyCode == 13){ __doPostBack('ctl00$MainPlaceHolder$btnSearch', ''); return false; }" runat="server" Width="210px"></asp:TextBox>
                        
                    </td>
                    <td style="height: 42px">
                         <asp:Button ID="btnCreate" runat="server" Text="Create New Account" OnClick="btnCreate_Click"
                            Width="138px" Enabled="False" />
                    </td>
                </tr>
            </table>
            <div style="overflow: auto; height: 330px; width: 910px;" runat="server">
            
                <asp:AISListView ID="testlistview" runat="server" OnSelectedIndexChanged="testlistview_SelectedIndexChanged"
                    OnSelectedIndexChanging="testlistview_SelectedIndexChanging" DataKeyNames="CUSTMR_ID"
                    OnItemCommand="testlistview_ItemCommand" OnSorting="testlistview_Sorting">
                    <LayoutTemplate>
                        <table style="width: 98%" class="panelContents" runat="server">
                            <tr class="LayoutTemplate">
                                <th>
                                    Select
                                </th>
                                <th>
                                    Insured Name
                                </th>
                                <th>
                                    Master Account
                                </th>
                                <th>
                                    <asp:LinkButton ID="hlBPNoSort" runat="server" CommandName="Sort" CommandArgument="FINC_PTY_ID">
                            BP #
                                    </asp:LinkButton>
                                    <asp:ImageButton ID="imgBPNoSort" Visible="false" ToolTip="Ascending" ImageUrl="~/images/ascending.gif"
                                        runat="server" />
                                </th>
                                <th>
                                    <asp:LinkButton ID="hlacctNoSort" runat="server" CommandName="Sort" CommandArgument="CUSTMR_ID">
                            Account Number
                                    </asp:LinkButton>
                                    <asp:ImageButton ID="imgAcctNoSort" Visible="false" ToolTip="Ascending" ImageUrl="~/images/ascending.gif"
                                        runat="server" />
                                </th>
                                <th>
                                    <asp:LinkButton ID="hlSSCGSort" runat="server" CommandName="Sort" CommandArgument="SUPRT_SERV_CUSTMR_GP_ID">
                            SSCG ID
                                    </asp:LinkButton>
                                    <asp:ImageButton ID="imgSSCGSort" Visible="false" ToolTip="Ascending" ImageUrl="~/images/ascending.gif"
                                        runat="server" />
                                </th>
                                <th>
                                    Insured's Address
                                </th>
                            </tr>
                            <tr id="itemPlaceholder" runat="server">
                            </tr>
                        </table>
                    </LayoutTemplate>
                    <EmptyDataTemplate>
                        <table id="Table1" class="panelContents" runat="server" width="98%">
                            <tr class="LayoutTemplate">
                                <th>
                                    Select
                                </th>
                                <th>
                                    Insured Name
                                </th>
                                <th>
                                    Master Account
                                </th>
                                <th>
                                    BP #
                                </th>
                                <th>
                                    Account Number
                                </th>
                                <th>
                                    SSCG ID
                                </th>
                                <th>
                                    Insured's Address
                                </th>
                            </tr>
                            <tr class="ItemTemplate">
                                <td align="center" colspan="10">
                                    <asp:Label ID="lblEmptyMessage" Text="No Record(s) Found" Font-Bold="true" runat="server"
                                        Style="text-align: center" />
                                </td>
                            </tr>
                        </table>
                    </EmptyDataTemplate>
                    <ItemTemplate>
                        <tr id="test" runat="server" class="ItemTemplate">
                            <td align="left" runat="server">
                                <asp:LinkButton ID="lbSelect" CommandName="Select" CommandArgument='<%# Bind("CUSTMR_ID") %>'
                                    runat="server" Text="select"></asp:LinkButton>
                            </td>
                            <td align="left" runat="server" id="test1">
                                <asp:Label ID="lblMasterAccount" Visible="false" runat="server" Text='<%# Bind("CUSTMR_REL_ID") %>'></asp:Label>
                                <asp:Label ID="AcctNM1" Width="165px" runat="server" Text='<%# Bind("FULL_NM") %>'></asp:Label>
                            </td>
                            <td runat="server" id="Td2">
                                <asp:Label ID="IsMasterAccount" Width="65px" runat="server" Text='<%# Bind("IS_MSTR_ACCT") %>'></asp:Label>
                            </td>
                            <td runat="server" id="test2">
                                <asp:Label ID="BPNumber" Width="65px" runat="server" Text='<%# Bind("FINC_PTY_ID") %>'></asp:Label>
                            </td>
                            <td runat="server" id="test3">
                                <asp:Label ID="AcctID1" Width="65px" runat="server" Text='<%# Bind("CUSTMR_ID") %>'></asp:Label>
                            </td>
                            <td runat="server" id="test4">
                                <asp:Label ID="SSCGID1" Width="65px" runat="server" Text='<%# Bind("SUPRT_SERV_CUSTMR_GP_ID") %>'></asp:Label>
                            </td>
                            <td runat="server" id="Td3">
                                <asp:Label ID="INSUREDADDRESS" Width="165px" runat="server" Text='<%# Bind("POSTAL_ADDRESS") %>'></asp:Label>
                            </td>
                        </tr>
                    </ItemTemplate>
                    <AlternatingItemTemplate>
                        <tr id="test" runat="server" class="AlternatingItemTemplate">
                            <td align="left" id="Td1" runat="server">
                                <asp:LinkButton ID="lbSelect" CommandName="Select" CommandArgument='<%# Bind("CUSTMR_ID") %>'
                                    runat="server" Text="select"></asp:LinkButton>
                            </td>
                            <td align="left">
                                <asp:Label ID="lblMasterAccount" Visible="false" runat="server" Text='<%# Bind("CUSTMR_REL_ID") %>'></asp:Label>
                                <asp:Label ID="AcctNM1" Width="165px" runat="server" Text='<%# Bind("FULL_NM") %>'></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="IsMasterAccount" Width="65px" runat="server" Text='<%# Bind("IS_MSTR_ACCT") %>'></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="BPNumber" Width="65px" runat="server" Text='<%# Bind("FINC_PTY_ID") %>'></asp:Label>
                            </td>
                            <td id="acct_id" runat="server">
                                <asp:Label ID="AcctID1" Width="65px" runat="server" Text='<%# Bind("CUSTMR_ID") %>'></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="SSCGID1" Width="65px" runat="server" Text='<%# Bind("SUPRT_SERV_CUSTMR_GP_ID") %>'></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="INSUREDADDRESS" Width="165px" runat="server" Text='<%# Bind("POSTAL_ADDRESS") %>'></asp:Label>
                            </td>
                        </tr>
                    </AlternatingItemTemplate>
                </asp:AISListView>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
