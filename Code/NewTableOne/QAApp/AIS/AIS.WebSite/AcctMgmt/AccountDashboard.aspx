<%@ Page Language="C#" MasterPageFile="~/Retro.master" AutoEventWireup="true" Inherits="AcctMgmt_AccountDashboard"
    Title="Account Dashboard" CodeBehind="AccountDashboard.aspx.cs" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Src="~/App_Shared/AccountList.ascx" TagName="AcctList" TagPrefix="AL" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeaderPlaceHolder" runat="Server">
    <asp:Label ID="lblDashBoard" runat="server" Text="Account Dashboard" CssClass="h1"></asp:Label>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainPlaceHolder" runat="Server">

    <script type="text/javascript">
        function ActiveTabChanged(sender, e) {
            // use the client side active tab changed 
            // event to trigger a callback thereby 
            // triggering the server side event.
            __doPostBack('<%= TabConDashboard.ClientID %>', sender.get_activeTab().get_headerText());
        }
        function ValidateUpdate() {
            var selUser = document.getElementById('<%=ddlUser.ClientID%>').value;
            var selAccount = document.getElementById('<%=ddlAccountName.ClientID%>').value;
            var selStatus = document.getElementById('<%=ddlStatus.ClientID%>').value;
            var fromDt = document.getElementById('<%=txtFromDt.ClientID%>').value;
            var toDt = document.getElementById('<%=txtToDt.ClientID%>').value;
            var selPending = document.getElementById('<%=ddlPending%>').value;

            var result = false;
            if (selUser > 0) result = true;
            else if (selAccount > 0) result = true;
            else if (selAccount > 0) result = true;
            else if (selAccount > 0) result = true;
            else if (selAccount > 0) result = true;
            return result;
        }
    </script>

    <asp:UpdatePanel runat="server" ID="upAccountDashboard">
        <ContentTemplate>
            <asp:ValidationSummary ID="valsAccDashBrd" runat="server" ValidationGroup="Save" />
            <asp:ObjectDataSource ID="StatusDataSource" runat="server" SelectMethod="GetLookUpActiveDataDashboard"
                TypeName="ZurichNA.AIS.Business.Logic.BLAccess">
                <SelectParameters>
                    <asp:Parameter DefaultValue="PROGRAM STATUSES" Name="lookUpTypeName" Type="String" />
                </SelectParameters>
            </asp:ObjectDataSource>
            <asp:ObjectDataSource ID="UserDataSource" runat="server" SelectMethod="getUserNames"
                TypeName="ZurichNA.AIS.Business.Logic.ProgramPeriodsBS"></asp:ObjectDataSource>
            <asp:ObjectDataSource ID="AccountDataSource" runat="server" SelectMethod="getAccountNames"
                TypeName="ZurichNA.AIS.Business.Logic.AccountBS">
                <SelectParameters>
                    <asp:Parameter DefaultValue="0" Name="perId" Type="String" />
                </SelectParameters>
            </asp:ObjectDataSource>
            <table>
                <tr>
                    <td>
                        <table>
                            <tr>
                                <td>
                                    <table>
                                        <tr>
                                            <td align="right">
                                                <asp:Label ID="lblUser" Text="User :" runat="server"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlUser" runat="server" DataSourceID="UserDataSource" OnSelectedIndexChanged="ddlUser_OnSelectedIndexChanged"
                                                    AutoPostBack="True" DataTextField="FULLNAME" DataValueField="PERSON_ID" Width="225px">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td valign="bottom" align="right">
                                                <asp:Label ID="Label1" Text="Account Name:" runat="server"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlAccountName" runat="server" DataSourceID="AccountDataSource"
                                                    DataTextField="FULL_NM" DataValueField="CUSTMR_ID" Width="225px">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr id="trStatus" runat="server">
                                            <td valign="bottom" align="right">
                                                <asp:Label ID="lblStatus" Text="Status :" runat="server"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlStatus" runat="server" DataSourceID="StatusDataSource" DataTextField="LookUpName"
                                                    DataValueField="LookUpID">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                                <td>
                                    <table>
                                        <tr id="trPending" visible="false" runat="server">
                                            <td>
                                                <asp:Label ID="lblPendingHead" Text="Pending :" runat="server"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlPending" runat="server" Width="90px">
                                                    <asp:ListItem Text="All" Value="2" Selected="True"></asp:ListItem>
                                                    <asp:ListItem Text="Yes" Value="1"></asp:ListItem>
                                                    <asp:ListItem Text="No" Value="0"></asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr id="trAriesClrng" visible="false" runat="server">
                                            <td>
                                                <asp:Label ID="lblAriesClrngHeader" Text="Aries Clearing Completed:" runat="server"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlAriesClrng" runat="server" Width="90px">
                                                    <asp:ListItem Text="All" Value="2" Selected="True"></asp:ListItem>
                                                    <asp:ListItem Text="Yes" Value="1"></asp:ListItem>
                                                    <asp:ListItem Text="No" Value="0"></asp:ListItem>
                                                </asp:DropDownList>
                                                &nbsp;&nbsp; Historical Adjustment:
                                                <asp:CheckBox ID="chkHistorical" Checked="true" runat="server" />
                                            </td>
                                        </tr>
                                        <tr id="trQcComplt" visible="false" runat="server">
                                            <td>
                                                <asp:Label ID="lblQcCompltdHeader" Text="20%QC Complete:" runat="server"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlQcCompltd" runat="server" Width="90px">
                                                    <asp:ListItem Text="All" Value="2" Selected="True"></asp:ListItem>
                                                    <asp:ListItem Text="Yes" Value="1"></asp:ListItem>
                                                    <asp:ListItem Text="No" Value="0"></asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr id="trFromDt" visible="false" runat="server">
                                            <td valign="bottom" align="right">
                                                <asp:Label ID="lblFromDt" Text="Val From :" runat="server">
                                                </asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtFromDt" runat="server" SkinID="largeTextbox" TabIndex="4"></asp:TextBox>
                                                <ajaxToolkit:CalendarExtender ID="txtFromDt_CalendarExtender" runat="server" TargetControlID="txtFromDt"
                                                    PopupButtonID="imgFromdate" Enabled="True" PopupPosition="TopRight" Format="MM/dd/yyyy">
                                                </ajaxToolkit:CalendarExtender>
                                                <asp:ImageButton ID="imgFromdate" runat="server" ImageUrl="~/images/Calendar_scheduleHS.png"
                                                    CausesValidation="False" />
                                                <ajaxToolkit:MaskedEditExtender ID="maskFromDate" runat="server" ErrorTooltipEnabled="True"
                                                    Mask="99/99/9999" MaskType="Date" MessageValidatorTip="true" OnFocusCssClass="MaskedEditFocus"
                                                    OnInvalidCssClass="MaskedEditError" TargetControlID="txtFromDt" />
                                                  
                                                <asp:CompareValidator ID="compareFromDate" runat="server" ControlToValidate="txtFromDt"
                                                    Display="Dynamic" ErrorMessage="Please Enter Valid From Date" Text="*" Operator="DataTypeCheck"
                                                    Type="Date" ValidationGroup="Save" />
                                            </td>
                                        </tr>
                                        <tr id="trToDt" visible="false" runat="server">
                                            <td valign="bottom" align="right">
                                                <asp:Label ID="lblToDate" Text="Val To :" runat="server"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtToDt" runat="server" SkinID="largeTextbox" TabIndex="4"></asp:TextBox>
                                                <ajaxToolkit:CalendarExtender ID="txtToDt_CalendarExtender" runat="server" TargetControlID="txtToDt"
                                                    PopupButtonID="imgTodate" Enabled="True" PopupPosition="TopRight" Format="MM/dd/yyyy">
                                                </ajaxToolkit:CalendarExtender>
                                                <asp:ImageButton ID="imgTodate" runat="server" ImageUrl="~/images/Calendar_scheduleHS.png"
                                                    CausesValidation="False" />
                                                <ajaxToolkit:MaskedEditExtender ID="mskToDate" runat="server" AcceptNegative="Left"
                                                    DisplayMoney="Left" ErrorTooltipEnabled="True" Mask="99/99/9999" MaskType="Date"
                                                    MessageValidatorTip="true" OnFocusCssClass="MaskedEditFocus" OnInvalidCssClass="MaskedEditError"
                                                    TargetControlID="txtToDt" />
                                                 <asp:CompareValidator ID="CompareToDate" runat="server" ControlToValidate="txtToDt"
                                                    Display="Dynamic" ErrorMessage="To Date must be greater than or equal to From Date" Text="*"
                                                    Operator="GreaterThanEqual" Type="Date" ControlToCompare="txtFromDt" ValidationGroup="Save" />
                                                <asp:CompareValidator ID="compareToDate2" runat="server" ControlToValidate="txtToDt"
                                                    Display="Dynamic" ErrorMessage="Please Enter Valid To Date" Text="*" Operator="DataTypeCheck"
                                                    Type="Date" ValidationGroup="Save" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2" align="center">
                                    <table>
                                        <tr>
                                            <td align="right">
                                                <asp:Button ID="btnSearch" OnClick="btnSearch_Click" runat="server" Height="18px"
                                                    Text="Search" Width="83px" ValidationGroup="Save" />
                                            </td>
                                            <td>
                                                <asp:Button ID="btnClear" runat="server" Height="18px" Text="Clear" Width="83px"
                                                    OnClick="btnClear_Click" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>
                        <ajaxToolkit:TabContainer ID="TabConDashboard" runat="server" CssClass="VariableTabs"
                            SkinID="tabVariable" OnActiveTabChanged="TabContainer1_ActiveTabChanged" OnClientActiveTabChanged="ActiveTabChanged">
                            <ajaxToolkit:TabPanel runat="server" ID="tplPpInfo">
                                <HeaderTemplate>
                                    Upcoming Valuations &amp; Programs
                                </HeaderTemplate>
                                <ContentTemplate>
                                    <asp:Panel ID="panPpInfo" runat="server" Height="300px" Width="910px" ScrollBars="Auto"
                                        Visible="true">
                                        <asp:AISListView ID="lstPp" runat="server" OnItemDataBound="lstPp_DataBoundList"
                                            OnItemCommand="lstPp_ItemCommand">
                                            <LayoutTemplate>
                                                <table id="Table1" class="panelContents" runat="server" width="98%">
                                                    <tr class="LayoutTemplate">
                                                        <th>
                                                            Account Name
                                                        </th>
                                                        <th>
                                                            Program Period
                                                        </th>
                                                        <th>
                                                            Program Type
                                                        </th>
                                                        <th>
                                                            First Valuation Date
                                                        </th>
                                                        <th>
                                                            Next Valuation Date
                                                        </th>
                                                        <th>
                                                            BU/Office
                                                        </th>
                                                        <th style="width: 50px">
                                                            Program Status
                                                        </th>
                                                        <th style="width: auto">
                                                            Details
                                                        </th>
                                                        <th>
                                                            View Loss Info
                                                        </th>
                                                        <th style="width: 50px">
                                                            View ADJ Dashboard
                                                        </th>
                                                    </tr>
                                                    <tr id="ItemPlaceHolder" runat="server">
                                                    </tr>
                                                </table>
                                            </LayoutTemplate>
                                            <EmptyDataTemplate>
                                                <table id="Table1" class="panelContents" runat="server" width="98%">
                                                    <tr class="LayoutTemplate">
                                                        <th>
                                                            Account Name
                                                        </th>
                                                        <th>
                                                            Program Period
                                                        </th>
                                                        <th>
                                                            Program Type
                                                        </th>
                                                        <th>
                                                            First Valuation Date
                                                        </th>
                                                        <th>
                                                            Next Valuation Date
                                                        </th>
                                                        <th>
                                                            BU/Office
                                                        </th>
                                                        <th style="width: 50px">
                                                            Program Status
                                                        </th>
                                                        <th style="width: auto">
                                                            Details
                                                        </th>
                                                        <th>
                                                            View Loss Info
                                                        </th>
                                                        <th style="width: 50px">
                                                            View ADJ Dashboard
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
                                                <tr id="Tr1" runat="server" class="ItemTemplate">
                                                    <td align="left">
                                                        <asp:Label ID="lblActNum" Width="50px" Visible="false" runat="server" Text='<%# Bind("CUSTMR_ID")%>'></asp:Label>
                                                        <asp:Label ID="lblActName" Width="160px" runat="server" Text='<%# Bind("CUSTMR_NAME")%>'></asp:Label>
                                                    </td>
                                                    <td align="left" style="width: 60px">
                                                        <asp:Label ID="lblPrgmPrd" Width="50px" runat="server" Text='<%# Bind("PROGRAMPERIOD")%>'></asp:Label>
                                                    </td>
                                                    <td style="width: 50px">
                                                        <asp:Label ID="lblprgmType" Width="50px" runat="server" Text='<%# Bind("PROGRAMTYPENAME")%>'></asp:Label>
                                                    </td>
                                                    <td style="width: 50px">
                                                        <asp:Label ID="lblFirstAdjDt" Width="50px" runat="server" Text='<%# Eval("FST_ADJ_DT", "{0:d}")%>'></asp:Label>
                                                    </td>
                                                    <td style="width: 50px">
                                                        <asp:Label ID="lblFinalAdjDt" Width="50px" runat="server" Text='<%# Eval("NXT_VALN_DT", "{0:d}")%>'></asp:Label>
                                                    </td>
                                                    <td align="left" style="width: 100px">
                                                        <asp:Label ID="lblBuOffice" Width="170px" runat="server" Text='<%# Bind("BUSINESSUNITNAME")%>'></asp:Label>
                                                    </td>
                                                    <td style="width: 50px">
                                                        <asp:Label ID="lbnPrgmStatus" Width="40px" runat="server" Text='<%# Bind("PROGRAMSTATUS")%>'></asp:Label>
                                                    </td>
                                                    <td style="width: 90px">
                                                        <asp:LinkButton ID="lbnDetails" Width="90px" runat="server" Text="Program Period"
                                                            CommandName="Details" CommandArgument='<%# Bind("CUSTMR_ID") %>'></asp:LinkButton>
                                                    </td>
                                                    <td style="width: 50px">
                                                        <asp:Label ID="lblHdPremAdjPgmId" runat="server" Text='<%# Bind("PREM_ADJ_PGM_ID") %>'
                                                            Visible="false"></asp:Label>
                                                        <asp:Label ID="lblValDate" Width="50px" Visible="false" runat="server" Text='<%# Eval("VALN_MM_DT", "{0:d}")%>'></asp:Label>
                                                        <asp:LinkButton ID="lbnViewLossInfo" Width="60px" runat="server" Text="Loss Info"
                                                            CommandName="LossInfo" Enabled='<% # Eval("PREMIUMADJNUMLOSS") == null ? true : false %>'
                                                            CommandArgument='<%# Eval("CUSTMR_ID")%> '></asp:LinkButton>
                                                    </td>
                                                    <td style="width: 50px">
                                                        <asp:Label ID="lblHdAdjNum" runat="server" Text='<%# Bind("PREMIUMADJNUM") %>' Visible="false"></asp:Label>
                                                        <asp:LinkButton ID="lbnAdjDashBoard" Width="50px" runat="server" Text="Adjust" CommandName="AdjDashBoard"
                                                            CommandArgument='<%# Bind("CUSTMR_ID") %>'></asp:LinkButton>
                                                    </td>
                                                </tr>
                                            </ItemTemplate>
                                            <AlternatingItemTemplate>
                                                <tr id="Tr1" runat="server" class="AlternatingItemTemplate">
                                                    <td align="left">
                                                        <asp:Label ID="lblActNum" Width="50px" Visible="false" runat="server" Text='<%# Bind("CUSTMR_ID")%>'></asp:Label>
                                                        <asp:Label ID="lblActName" Width="160px" runat="server" Text='<%# Bind("CUSTMR_NAME")%>'></asp:Label>
                                                    </td>
                                                    <td align="left" style="width: 60px">
                                                        <asp:Label ID="lblPrgmPrd" Width="50px" runat="server" Text='<%# Bind("PROGRAMPERIOD")%>'></asp:Label>
                                                    </td>
                                                    <td style="width: 50px">
                                                        <asp:Label ID="lblprgmType" Width="50px" runat="server" Text='<%# Bind("PROGRAMTYPENAME")%>'></asp:Label>
                                                    </td>
                                                    <td style="width: 50px">
                                                        <asp:Label ID="lblFirstAdjDt" Width="50px" runat="server" Text='<%# Eval("FST_ADJ_DT", "{0:d}")%>'></asp:Label>
                                                    </td>
                                                    <td style="width: 50px">
                                                        <asp:Label ID="lblFinalAdjDt" Width="50px" runat="server" Text='<%# Eval("NXT_VALN_DT", "{0:d}")%>'></asp:Label>
                                                    </td>
                                                    <td align="left" style="width: 100px">
                                                        <asp:Label ID="lblBuOffice" Width="170px" runat="server" Text='<%# Bind("BUSINESSUNITNAME")%>'></asp:Label>
                                                    </td>
                                                    <td style="width: 50px">
                                                        <asp:Label ID="lbnPrgmStatus" Width="40px" runat="server" Text='<%# Bind("PROGRAMSTATUS")%>'></asp:Label>
                                                    </td>
                                                    <td style="width: 90px">
                                                        <asp:LinkButton ID="lbnDetails" Width="90px" runat="server" Text="Program Period"
                                                            CommandName="Details" CommandArgument='<%# Bind("CUSTMR_ID") %>'></asp:LinkButton>
                                                    </td>
                                                    <td style="width: 50px">
                                                        <asp:Label ID="lblHdPremAdjPgmId" runat="server" Text='<%# Bind("PREM_ADJ_PGM_ID") %>'
                                                            Visible="false"></asp:Label>
                                                        <asp:Label ID="lblValDate" Width="50px" Visible="false" runat="server" Text='<%# Eval("VALN_MM_DT", "{0:d}")%>'></asp:Label>
                                                        <asp:LinkButton ID="lbnViewLossInfo" Width="60px" runat="server" Text="Loss Info"
                                                            CommandName="LossInfo" Enabled='<% # Eval("PREMIUMADJNUMLOSS") == null ? true : false %>'
                                                            CommandArgument='<%# Eval("CUSTMR_ID")%> '></asp:LinkButton>
                                                    </td>
                                                    <td style="width: 50px">
                                                        <asp:Label ID="lblHdAdjNum" runat="server" Text='<%# Bind("PREMIUMADJNUM") %>' Visible="false"></asp:Label>
                                                        <asp:LinkButton ID="lbnAdjDashBoard" Width="50px" runat="server" Text="Adjust" CommandName="AdjDashBoard"
                                                            CommandArgument='<%# Bind("CUSTMR_ID") %>'></asp:LinkButton>
                                                    </td>
                                                </tr>
                                            </AlternatingItemTemplate>
                                        </asp:AISListView>
                                    </asp:Panel>
                                    <table>
                                        <tr>
                                            <asp:DataPager ID="dplstPpDataPager" runat="server"  PageSize="200" OnPreRender="lstPpDataPager_PreRender"
                                                PagedControlID="lstPp">
                                                <Fields>
                                                    <asp:NumericPagerField ButtonType="Button"  ButtonCount="20" NextPageText="more.." />
                                                </Fields>
                                            </asp:DataPager>
                                            <td width="980px" align="right">
                                                <asp:UpdatePanel ID="upExcel" runat="server">
                                                    <ContentTemplate>
                                                        <asp:Button ID="btnExcel" runat="server" Visible="false" OnClick="btnExcel_Click"
                                                            Text="EXPORT TO EXCEL" UseSubmitBehavior="false" />
                                                    </ContentTemplate>
                                                    <Triggers>
                                                        <asp:PostBackTrigger ControlID="btnExcel" />
                                                    </Triggers>
                                                </asp:UpdatePanel>
                                            </td>
                                        </tr>
                                    </table>
                                </ContentTemplate>
                            </ajaxToolkit:TabPanel>
                            <ajaxToolkit:TabPanel runat="server" ID="tplAdjustmentInfo">
                                <HeaderTemplate>
                                    Adjustment Details
                                </HeaderTemplate>
                                <ContentTemplate>
                                    <asp:Panel ID="Panel1" runat="server" Height="300px" Width="910px" ScrollBars="Auto"
                                        Visible="true">
                                        <asp:AISListView ID="lstAdj" runat="server" OnItemCommand="lstAdj_ItemCommand" OnItemDataBound="lstAdj_DataBoundList"
                                            OnSorting="lstAdj_Sorting">
                                            <LayoutTemplate>
                                                <table id="Table2" class="panelContents" runat="server" width="98%">
                                                    <tr class="LayoutTemplate">
                                                        <th style="width: 220px">
                                                            Account Name
                                                        </th>
                                                        <th>
                                                            Adj No.
                                                        </th>
                                                        <th>
                                                            <asp:LinkButton ID="hlAdjValDtSort" runat="server" CommandName="Sort" CommandArgument="VALN_DT">
                                                            Adj Val Date
                                                            </asp:LinkButton>
                                                            <asp:ImageButton ID="imgAdjValDtSort" Visible="false" CommandArgument="VALN_DT" CommandName="Sort"
                                                                ToolTip="Ascending" ImageUrl="~/images/ascending.gif" runat="server" />
                                                        </th>
                                                        <th>
                                                            Adj Status Date
                                                        </th>
                                                        <th>
                                                            Adjustment Status
                                                        </th>
                                                        <th>
                                                            Pending
                                                        </th>
                                                        <th>
                                                            Details
                                                        </th>
                                                        <th>
                                                            View Loss Info
                                                        </th>
                                                        <th>
                                                            Review Feedback
                                                        </th>
                                                    </tr>
                                                    <tr id="ItemPlaceHolder" runat="server">
                                                    </tr>
                                                </table>
                                            </LayoutTemplate>
                                            <EmptyDataTemplate>
                                                <table id="Table2" class="panelContents" runat="server" width="98%">
                                                    <tr class="LayoutTemplate">
                                                        <th style="width: 220px">
                                                            Account Name
                                                        </th>
                                                        <th>
                                                            Adj No.
                                                        </th>
                                                        <th>
                                                            Adj Val Date
                                                        </th>
                                                        <th>
                                                            Adj Status Date
                                                        </th>
                                                        <th>
                                                            Adjustment Status
                                                        </th>
                                                        <th>
                                                            Pending
                                                        </th>
                                                        <th>
                                                            Details
                                                        </th>
                                                        <th>
                                                            View Loss Info
                                                        </th>
                                                        <th>
                                                            Review Feedback
                                                        </th>
                                                    </tr>
                                                    <tr class="ItemTemplate">
                                                        <td align="center" colspan="9">
                                                            <asp:Label ID="lblEmptyMessage" Text="No Record(s) Found" Font-Bold="true" runat="server"
                                                                Style="text-align: center" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </EmptyDataTemplate>
                                            <ItemTemplate>
                                                <tr id="Tr1" runat="server" class="ItemTemplate">
                                                    <td align="left">
                                                        <asp:Label ID="lblActNum" Visible="false" runat="server" Text='<%# Bind("CUSTOMERID")%>'></asp:Label>
                                                        <asp:Label ID="lblActName" runat="server" Text='<%# Eval("CHILD_CUSTMR_NAME").ToString() =="" ? Eval("CUSTMR_NAME"): Eval("CHILD_CUSTMR_NAME")%>'></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblAdjNum" Width="50px" runat="server" Text='<%# Bind("PREMIUM_ADJ_ID")%>'></asp:Label>
                                                    </td>
                                                    <td align="center">
                                                        <asp:Label ID="lblValDate" Width="50px" runat="server" Text='<%# Eval("VALN_DT", "{0:d}")%>'></asp:Label>
                                                    </td>
                                                    <td align="center">
                                                        <asp:Label ID="lblAdjDt" Width="50px" runat="server" Text='<%# Eval("EFF_DT", "{0:d}")%>'></asp:Label>
                                                    </td>
                                                    <td align="left">
                                                        <asp:Label ID="lblAdjStatus" runat="server" Text='<%# Bind("ADJ_STATUS")%>'></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblPending" Width="50px" runat="server" Text='<%# Bind("ADJ_PENDG_IND_DESC")%>'></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:LinkButton ID="lbnDetails" Width="50px" runat="server" Text="Details" Enabled="false"
                                                            CommandName="Details" CommandArgument='<%# Eval("CUSTOMERID")%> '></asp:LinkButton>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblHdPremAdjPgmId" runat="server" Text='<%# Bind("PREM_ADJ_PGM_ID") %>'
                                                            Visible="false"></asp:Label>
                                                        <asp:LinkButton ID="lbnViewLossInfo" Width="70px" runat="server" Text="Loss Info"
                                                            CommandName="LossInfo" CommandArgument='<%# Eval("ACCOUNTID")%> '></asp:LinkButton>
                                                    </td>
                                                    <td>
                                                        <asp:LinkButton ID="lnkAdjFeedback" runat="server" CommandArgument='<%# Eval("CUSTOMERID")%> '
                                                            Text="AdjQC" Enabled='<%#Eval("ADJ_QC_IND")%> ' CommandName="AdjFeedBack"></asp:LinkButton>
                                                        <asp:LinkButton ID="lnkReconFeedback" runat="server" CommandArgument='<%# Eval("CUSTOMERID")%> '
                                                            Text="Recon" Enabled='<%# Eval("RECONCILER_REVW_IND")%> ' CommandName="ReconFeedBack"></asp:LinkButton>
                                                    </td>
                                                </tr>
                                            </ItemTemplate>
                                            <AlternatingItemTemplate>
                                                <tr id="Tr1" runat="server" class="AlternatingItemTemplate">
                                                    <td align="left">
                                                        <asp:Label ID="lblActNum" Visible="false" runat="server" Text='<%# Bind("CUSTOMERID")%>'></asp:Label>
                                                        <asp:Label ID="lblActName" runat="server" Text='<%# Eval("CHILD_CUSTMR_NAME").ToString() =="" ? Eval("CUSTMR_NAME"): Eval("CHILD_CUSTMR_NAME")%>'></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblAdjNum" Width="50px" runat="server" Text='<%# Bind("PREMIUM_ADJ_ID")%>'></asp:Label>
                                                    </td>
                                                    <td align="center">
                                                        <asp:Label ID="lblValDate" Width="50px" runat="server" Text='<%# Eval("VALN_DT", "{0:d}")%>'></asp:Label>
                                                    </td>
                                                    <td align="center">
                                                        <asp:Label ID="lblAdjDt" Width="50px" runat="server" Text='<%# Eval("EFF_DT", "{0:d}")%>'></asp:Label>
                                                    </td>
                                                    <td align="left">
                                                        <asp:Label ID="lblAdjStatus" runat="server" Text='<%# Bind("ADJ_STATUS")%>'></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblPending" Width="50px" runat="server" Text='<%# Bind("ADJ_PENDG_IND_DESC")%>'></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:LinkButton ID="lbnDetails" Width="50px" runat="server" Text="Details" Enabled="false"
                                                            CommandName="Details" CommandArgument='<%# Eval("CUSTOMERID")%> '></asp:LinkButton>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblHdPremAdjPgmId" runat="server" Text='<%# Bind("PREM_ADJ_PGM_ID") %>'
                                                            Visible="false"></asp:Label>
                                                        <asp:LinkButton ID="lbnViewLossInfo" Width="70px" runat="server" Text="Loss Info"
                                                            CommandName="LossInfo" CommandArgument='<%# Eval("ACCOUNTID")%> '></asp:LinkButton>
                                                    </td>
                                                    <td>
                                                        <asp:LinkButton ID="lnkAdjFeedback" runat="server" CommandArgument='<%# Eval("CUSTOMERID")%> '
                                                            Text="AdjQC" Enabled='<%#Eval("ADJ_QC_IND")%> ' CommandName="AdjFeedBack"></asp:LinkButton>
                                                        <asp:LinkButton ID="lnkReconFeedback" runat="server" CommandArgument='<%# Eval("CUSTOMERID")%> '
                                                            Text="Recon" Enabled='<%# Eval("RECONCILER_REVW_IND")%> ' CommandName="ReconFeedBack"></asp:LinkButton>
                                                    </td>
                                                </tr>
                                            </AlternatingItemTemplate>
                                        </asp:AISListView>
                                    </asp:Panel>
                                    <table>
                                        <tr>
                                            <td width="1000px" align="right">
                                                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                                    <ContentTemplate>
                                                        <asp:Button ID="btnAdjExcel" runat="server" Visible="False" OnClick="btnAdjExcel_Click"
                                                            Text="EXPORT TO EXCEL" UseSubmitBehavior="false" />
                                                    </ContentTemplate>
                                                    <Triggers>
                                                        <asp:PostBackTrigger ControlID="btnAdjExcel" />
                                                    </Triggers>
                                                </asp:UpdatePanel>
                                            </td>
                                        </tr>
                                    </table>
                                </ContentTemplate>
                            </ajaxToolkit:TabPanel>
                            <ajaxToolkit:TabPanel runat="server" ID="tpInvoiceDetails">
                                <HeaderTemplate>
                                    Invoice Details
                                </HeaderTemplate>
                                <ContentTemplate>
                                    <asp:Panel ID="Panel2" runat="server" Height="300px" Width="910px" ScrollBars="Auto"
                                        Visible="true">
                                        <asp:AISListView OnItemDataBound="lstInv_DataBoundList" ID="lstInv" runat="server"
                                            OnItemCommand="lstInvoice_ItemCommand">
                                            <LayoutTemplate>
                                                <table id="Table1" class="panelContents" runat="server" width="98%">
                                                    <tr class="LayoutTemplate">
                                                        <th>
                                                            Acc.No
                                                        </th>
                                                        <th>
                                                            Account Name
                                                        </th>
                                                        <th>
                                                            Valuation Date
                                                        </th>
                                                        <th>
                                                            Adj. Number
                                                        </th>
                                                        <th>
                                                            Invoice Number
                                                        </th>
                                                        <th>
                                                            Invoice Date
                                                        </th>
                                                        <th>
                                                            Invoice Amount
                                                        </th>
                                                        <th>
                                                            Aries Clearing
                                                        </th>
                                                        <th>
                                                            20% qc
                                                        </th>
                                                        <th>
                                                            View Loss Info
                                                        </th>
                                                        <th style="width: 10%">
                                                            View Invoice PDF
                                                        </th>
                                                    </tr>
                                                    <tr id="ItemPlaceHolder" runat="server">
                                                    </tr>
                                                </table>
                                            </LayoutTemplate>
                                            <EmptyDataTemplate>
                                                <table id="Table1" class="panelContents" runat="server" style="width: 98%">
                                                    <tr>
                                                        <th>
                                                            Acc.No
                                                        </th>
                                                        <th>
                                                            Account Name
                                                        </th>
                                                        <th>
                                                            Valuation Date
                                                        </th>
                                                        <th>
                                                            Adj. Number
                                                        </th>
                                                        <th>
                                                            Invoice Number
                                                        </th>
                                                        <th>
                                                            Invoice Date
                                                        </th>
                                                        <th>
                                                            Invoice Amount
                                                        </th>
                                                        <th>
                                                            Aries Clearing
                                                        </th>
                                                        <th>
                                                            20% qc
                                                        </th>
                                                        <th>
                                                            View Loss Info
                                                        </th>
                                                        <th style="width: 10%">
                                                            View Invoice PDF
                                                        </th>
                                                    </tr>
                                                    <tr class="ItemTemplate">
                                                        <td align="center" colspan="11">
                                                            <asp:Label ID="lblEmptyMessage" Text="No Record(s) Found" Font-Bold="true" runat="server"
                                                                Style="text-align: center" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </EmptyDataTemplate>
                                            <ItemTemplate>
                                                <tr id="Tr1" runat="server" class="ItemTemplate">
                                                    <td>
                                                        <asp:Label ID="lblActNum" Width="50px" runat="server" Text='<%# Bind("CUSTOMERID")%>'></asp:Label>
                                                    </td>
                                                    <td align="left">
                                                        <asp:Label ID="lblActName" Width="90px" runat="server" Text='<%# Bind("CUSTMR_NAME")%>'></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblValDate" Width="50px" runat="server" Text='<%# Eval("VALN_DT", "{0:d}")%>'></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblAdjNum" runat="server" Text='<%# Bind("PREMIUM_ADJ_ID")%>'></asp:Label>
                                                    </td>
                                                    <td align="left">
                                                        <asp:Label ID="lblInvNum" Width="50px" runat="server" Text='<%#Bind("INVC_NBR_TXT") %>'></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblInvDt" Width="50px" runat="server" Text='<%# Eval("INVC_DUE_DT", "{0:d}")%>'></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblInvoiceAmt" Width="70px" runat="server" Text='<%# Eval("INVC_AMT") != null ? (Eval("INVC_AMT").ToString() != "" ?(decimal.Parse(Eval("INVC_AMT").ToString())).ToString("#,##0") : "0"): "0"%>'></asp:Label>
                                                    </td>
                                                    <td style="width: 50px">
                                                        <asp:LinkButton ID="lbnAriesclrng" Width="50px" runat="server" Text="ARiES" CommandName="ARIES"
                                                            CommandArgument='<%# Eval("CUSTOMERID")%> '></asp:LinkButton>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblHdEffDt" runat="server" Text='<%# Bind("EFF_DT") %>' Visible="false"></asp:Label>
                                                        <asp:LinkButton ID="lbnQc" Width="50px" runat="server" Text="20% QC" CommandName="QC"
                                                            CommandArgument='<%# Eval("CUSTOMERID")%> '></asp:LinkButton>
                                                        <asp:HiddenField ID="twentypercent" runat="server" Value='<%# Eval("TWENTY_REQ_IND") %>' />
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblHdPremAdjPgmId" runat="server" Text='<%# Bind("PREM_ADJ_PGM_ID") %>'
                                                            Visible="false"></asp:Label>
                                                        <asp:LinkButton ID="lbnViewLossInfo" Width="70px" runat="server" Text="Loss Info"
                                                            CommandName="LossInfo" CommandArgument='<%# Eval("ACCOUNTID")%> '></asp:LinkButton>
                                                    </td>
                                                    <td style="width: 100px">
                                                        <asp:LinkButton ID="lnkDraftInternal" runat="server" Text="Internal PDF" Enabled='<%#(Eval("DRFT_INTRNL_PDF_ZDW_KEY_TXT")!=null?(Eval("DRFT_INTRNL_PDF_ZDW_KEY_TXT").ToString()!=""? true:false):false)%>'
                                                            CommandName="GetPDF" CommandArgument='<%# Bind("DRFT_INTRNL_PDF_ZDW_KEY_TXT") %>'></asp:LinkButton>
                                                        <asp:LinkButton ID="lnkDraftExternal" runat="server" Text="External PDF" Enabled='<%#(Eval("DRFT_EXTRNL_PDF_ZDW_KEY_TXT")!=null?(Eval("DRFT_EXTRNL_PDF_ZDW_KEY_TXT").ToString()!=""? true:false):false)%>'
                                                            CommandName="GetPDF" CommandArgument='<%# Bind("DRFT_EXTRNL_PDF_ZDW_KEY_TXT") %>'></asp:LinkButton>
                                                        <asp:LinkButton ID="lnkDraftCDWorksheet" runat="server" Text="Worksheet PDF" Enabled='<%#(Eval("DRFT_CD_WRKSHT_PDF_ZDW_KEY_TXT")!=null?(Eval("DRFT_CD_WRKSHT_PDF_ZDW_KEY_TXT").ToString()!=""? true:false):false)%>'
                                                            CommandName="GetPDF" CommandArgument='<%# Bind("DRFT_CD_WRKSHT_PDF_ZDW_KEY_TXT") %>'></asp:LinkButton>
                                                    </td>
                                                </tr>
                                            </ItemTemplate>
                                            <AlternatingItemTemplate>
                                                <tr id="Tr1" runat="server" class="AlternatingItemTemplate">
                                                    <td>
                                                        <asp:Label ID="lblActNum" Width="50px" runat="server" Text='<%# Bind("CUSTOMERID")%>'></asp:Label>
                                                    </td>
                                                    <td align="left">
                                                        <asp:Label ID="lblActName" Width="90px" runat="server" Text='<%# Bind("CUSTMR_NAME")%>'></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblValDate" Width="50px" runat="server" Text='<%# Eval("VALN_DT", "{0:d}")%>'></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblAdjNum" runat="server" Text='<%# Bind("PREMIUM_ADJ_ID")%>'></asp:Label>
                                                    </td>
                                                    <td align="left">
                                                        <asp:Label ID="lblInvNum" Width="50px" runat="server" Text='<%#Bind("INVC_NBR_TXT") %>'></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblInvDt" Width="50px" runat="server" Text='<%# Eval("INVC_DUE_DT", "{0:d}")%>'></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblInvoiceAmt" Width="70px" runat="server" Text='<%# Eval("INVC_AMT") != null ? (Eval("INVC_AMT").ToString() != "" ?(decimal.Parse(Eval("INVC_AMT").ToString())).ToString("#,##0") : "0"): "0"%>'></asp:Label>
                                                    </td>
                                                    <td style="width: 50px">
                                                        <asp:LinkButton ID="lbnAriesclrng" Width="50px" runat="server" Text="ARiES" CommandName="ARIES"
                                                            CommandArgument='<%# Eval("CUSTOMERID")%> '></asp:LinkButton>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblHdEffDt" runat="server" Text='<%# Bind("EFF_DT") %>' Visible="false"></asp:Label>
                                                        <asp:LinkButton ID="lbnQc" Width="50px" runat="server" Text="20% QC" CommandName="QC"
                                                            CommandArgument='<%# Eval("CUSTOMERID")%> '></asp:LinkButton>
                                                        <asp:HiddenField ID="twentypercent" runat="server" Value='<%# Eval("TWENTY_REQ_IND") %>' />
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblHdPremAdjId" runat="server" Text='<%# Bind("PREM_ADJ_PGM_ID") %>'
                                                            Visible="false"></asp:Label>
                                                        <asp:Label ID="lblHdPremAdjPgmId" runat="server" Text='<%# Bind("PREM_ADJ_PGM_ID") %>'
                                                            Visible="false"></asp:Label>
                                                        <asp:LinkButton ID="lbnViewLossInfo" Width="70px" runat="server" Text="Loss Info"
                                                            CommandName="LossInfo" CommandArgument='<%# Eval("ACCOUNTID")%> '></asp:LinkButton>
                                                    </td>
                                                    <td style="width: 100px">
                                                        <asp:LinkButton ID="lnkDraftInternal" runat="server" Text="Internal PDF" Enabled='<%#(Eval("DRFT_INTRNL_PDF_ZDW_KEY_TXT")!=null?(Eval("DRFT_INTRNL_PDF_ZDW_KEY_TXT").ToString()!=""? true:false):false)%>'
                                                            CommandName="GetPDF" CommandArgument='<%# Bind("DRFT_INTRNL_PDF_ZDW_KEY_TXT") %>'></asp:LinkButton>
                                                        <asp:LinkButton ID="lnkDraftExternal" runat="server" Text="External PDF" Enabled='<%#(Eval("DRFT_EXTRNL_PDF_ZDW_KEY_TXT")!=null?(Eval("DRFT_EXTRNL_PDF_ZDW_KEY_TXT").ToString()!=""? true:false):false)%>'
                                                            CommandName="GetPDF" CommandArgument='<%# Bind("DRFT_EXTRNL_PDF_ZDW_KEY_TXT") %>'></asp:LinkButton>
                                                        <asp:LinkButton ID="lnkDraftCDWorksheet" runat="server" Text="Worksheet PDF" Enabled='<%#(Eval("DRFT_CD_WRKSHT_PDF_ZDW_KEY_TXT")!=null?(Eval("DRFT_CD_WRKSHT_PDF_ZDW_KEY_TXT").ToString()!=""? true:false):false)%>'
                                                            CommandName="GetPDF" CommandArgument='<%# Bind("DRFT_CD_WRKSHT_PDF_ZDW_KEY_TXT") %>'></asp:LinkButton>
                                                    </td>
                                                </tr>
                                            </AlternatingItemTemplate>
                                        </asp:AISListView>
                                    </asp:Panel>
                                    <table>
                                        <tr>
                                            <td width="900px" align="right">
                                                <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                                    <ContentTemplate>
                                                        <asp:Button ID="btnInvExcel" runat="server" Visible="false" OnClick="btnInvExcel_Click"
                                                            Text="EXPORT TO EXCEL" UseSubmitBehavior="false" />
                                                    </ContentTemplate>
                                                    <Triggers>
                                                        <asp:PostBackTrigger ControlID="btnInvExcel" />
                                                    </Triggers>
                                                </asp:UpdatePanel>
                                            </td>
                                        </tr>
                                    </table>
                                </ContentTemplate>
                            </ajaxToolkit:TabPanel>
                        </ajaxToolkit:TabContainer>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="TabConDashboard" EventName="ActiveTabChanged" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
