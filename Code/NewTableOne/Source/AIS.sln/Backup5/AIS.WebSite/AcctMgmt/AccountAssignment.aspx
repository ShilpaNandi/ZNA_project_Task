<%@ Page Language="C#" MasterPageFile="~/Retro.master" AutoEventWireup="true" Inherits="AcctMgmt_AccountAssignment"
    CodeBehind="AccountAssignment.aspx.cs" %>

<%@ Register Src="~/App_Shared/AccountList.ascx" TagPrefix="AL" TagName="AccountList" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeaderPlaceHolder" runat="Server">
    <asp:Label ID="lblRetroInfo" runat="server" Text="Account Assignment" CssClass="h1"></asp:Label>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="MainPlaceHolder" runat="Server">
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
        
        
        .autocomplete_completionListElement
        {
            border: solid 1px #444444;
            margin: 0px;
            padding: 2px;
            height: 100px;
            overflow: auto;
            background-color: #FFFFFF;
        }
        
        /* AutoComplete highlighted item */
        
        .autocomplete_highlightedListItem
        {
            background-color: #ffff99;
        }
        
        /* AutoComplete item */
        
        .autocomplete_listItem
        {
            color: #1C1C1C;
        }
    </style>
    <script type="text/javascript">
        function toggleCheckBoxes(source) {

            var listView1 = document.getElementById('<%= tdAccountsList.ClientID %>');


            if (listView1 != null) {
                var listView = document.getElementById('tblAccntlist');

                if (listView != null) {
                    for (var i = 0; i < listView.rows.length; i++) {
                        var inputs = listView.rows[i].getElementsByTagName('input');
                        for (var j = 0; j < inputs.length; j++) {
                            if (inputs[j].type == "checkbox")
                                inputs[j].checked = source.checked;
                        }
                    }
                }
            }
        }


        function toggleCheckBoxes1() {

            var listView1 = document.getElementById('<%= tdAccountsList.ClientID %>');
            var count = 0;
            var row = 0;

            if (listView1 != null) {
                var listView = document.getElementById('tblAccntlist');

                if (listView != null) {

                    for (var i = 0; i < listView.rows.length; i++) {
                        var inputs = listView.rows[i].getElementsByTagName('input');
                        for (var j = 0; j < inputs.length; j++) {
                            if (inputs[j].type == "checkbox" && inputs[j].id != 'chkSelectAll') {
                                row++;
                                if (inputs[j].checked) {
                                    count++;
                                }
                            }
                        }
                    }

                    if (count == row) {
                        document.getElementById('chkSelectAll').checked = true;

                    }
                    else {
                        document.getElementById('chkSelectAll').checked = false;

                    }
                }
            }
        }


        function SelectSelectAllCheckButton(chk) {

            var ListViewID = document.getElementById("tblAccntlist");

            for (var i = 1; i < ListViewID.rows.length; i++) {

                var inputs = ListViewID.rows[i].getElementsByTagName('input');

                if (chk.checked == true) {

                    document.getElementById('chkSelectAll').checked = true;
                }

                else {

                    document.getElementById('chkSelectAll').checked = false;
                }

            }

        }

        function ChildClick(CheckBox) {
            //get target base & child control.
            var listView = document.getElementById('tblAccntlist');
            var TotalChkBx = listView.rows.length;
            var Counter = 0;
            var HeaderCheckBox = document.getElementById('chkSelectAll');

            //Modifiy Counter;            
            if (CheckBox.checked && Counter < TotalChkBx)
                Counter++;
            else if (Counter > 0)
                Counter--;

            //Change state of the header CheckBox.
            if (Counter < TotalChkBx)
                HeaderCheckBox.checked = false;
            else if (Counter == TotalChkBx)
                HeaderCheckBox.checked = true;
        }

    </script>
    <asp:ObjectDataSource ID="BrokerDataSource" runat="server" SelectMethod="GetBrokersForLookups"
        TypeName="ZurichNA.AIS.Business.Logic.BrokerBS"></asp:ObjectDataSource>
    <asp:ObjectDataSource ID="PersonDatasource" runat="server" SelectMethod="FillCRMusers"
        TypeName="ZurichNA.AIS.Business.Logic.PersonBS"></asp:ObjectDataSource>
    <!--  <asp:ObjectDataSource ID="BusinessUnitDataSource" runat="server" SelectMethod="GetBusinessUnits"
                            TypeName="ZurichNA.AIS.Business.Logic.BusinessUnitOfficeBS"></asp:ObjectDataSource> -->
    <asp:ObjectDataSource ID="BUOfficeDataSource" runat="server" SelectMethod="GetBUOffForLookups"
        TypeName="ZurichNA.AIS.Business.Logic.BusinessUnitOfficeBS"></asp:ObjectDataSource>
    <asp:ObjectDataSource ID="BUDataSource" runat="server" SelectMethod="GetBUForLookups"
        TypeName="ZurichNA.AIS.Business.Logic.BusinessUnitOfficeBS"></asp:ObjectDataSource>
    <asp:ObjectDataSource ID="OfficeDataSource" runat="server" SelectMethod="GetOfficeForLookups"
        TypeName="ZurichNA.AIS.Business.Logic.BusinessUnitOfficeBS"></asp:ObjectDataSource>
    <asp:ObjectDataSource ID="ResponsibilityDataSource" runat="server" SelectMethod="GetLookUpActiveDataWithoutSelect"
        TypeName="ZurichNA.AIS.Business.Logic.BLAccess">
        <SelectParameters>
            <asp:Parameter DefaultValue="RESPONSIBILITY" Name="lookUpTypeName" Type="String" />
        </SelectParameters>
    </asp:ObjectDataSource>
    <asp:UpdatePanel ID="udpAccountAssignment" runat="server">
        <ContentTemplate>
            <div>
                <table width="100%">
                    <tr style="height: 5px;">
                        <td colspan="2">
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 55%">
                            <table>
                                <tr>
                                    <td style="vertical-align: middle">
                                        <asp:Label ID="lblAccount" runat="server" Text="Account"></asp:Label>
                                    </td>
                                    <td id="tdAcctlist" runat="server">
                                        <asp:Panel ID="pnlAcctlist" runat="server">
                                            <AL:AccountList ID="ddlAcctlist" runat="server" />
                                        </asp:Panel>
                                    </td>
                                    <td style="padding-left: 3px; padding-top: 26px;">
                                        <asp:CheckBox AutoPostBack="true" ID="chkRangeSearch" runat="server" OnCheckedChanged="chkRangeSearch_CheckedChanged" />
                                        Acct Range Search
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="Label4" runat="server" Text="BU/Office Name"></asp:Label>
                                    </td>
                                    <td style="padding-left: 3px">
                                        <asp:DropDownList ID="ddlOffice" runat="server" Width="231px" DataSourceID="BUOfficeDataSource"
                                            DataTextField="LookupName" DataValueField="LookUpID" AutoPostBack="true" OnSelectedIndexChanged="ddlOffice_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                    <td style="padding-left: 3px">
                                        <asp:CheckBox AutoPostBack="true" ID="chkBURangeSearch" runat="server" OnCheckedChanged="chkBURangeSearch_CheckedChanged" />
                                        Office Range Search
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblOfficename" runat="server" Text="BU & Office Name"></asp:Label>
                                    </td>
                                    <td style="padding-left: 3px">
                                        <asp:DropDownList ID="ddlBU" runat="server" Width="70px" DataSourceID="BUDataSource"
                                            DataTextField="LookupName" DataValueField="LookUpID" AutoPostBack="true" OnSelectedIndexChanged="ddlOfficeOnly_SelectedIndexChanged">
                                        </asp:DropDownList>
                                        <asp:DropDownList ID="ddlOfficeOnly" runat="server" Width="158px" DataSourceID="OfficeDataSource"
                                            DataTextField="LookupName" DataValueField="LookUpID" AutoPostBack="true" OnSelectedIndexChanged="ddlOfficeOnly_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        BROKER
                                    </td>
                                    <td style="padding-left: 3px">
                                        <asp:DropDownList ID="ddlBroker" runat="server" Width="231px" DataSourceID="BrokerDataSource"
                                            DataTextField="LookUpName" DataValueField="LookUpID">
                                        </asp:DropDownList>
                                        <%--<asp:CompareValidator ID="CompareddlBroker" runat="server" ControlToValidate="ddlBroker"
                        ValueToCompare="0" ErrorMessage="*" Operator="NotEqual" ValidationGroup="Save"></asp:CompareValidator>--%>
                                    </td>
                                    <td style="padding-left: 3px">
                                        <asp:CheckBox AutoPostBack="true" ID="ChkBrokerRangeSearch" runat="server" OnCheckedChanged="ChkBrokerRangeSearch_CheckedChanged" />
                                        Broker Range Search
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        User
                                    </td>
                                    <td style="padding-left: 3px">
                                         <asp:DropDownList ID="ddlUser" runat="server" Width="231px" DataSourceID="PersonDatasource"
                                            DataTextField="FULLNAME" DataValueField="PERSON_ID" AutoPostBack="true" OnSelectedIndexChanged="ddlUser_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Role
                                    </td>
                                    <td style="padding-left: 3px">
                                       
                                        <asp:DropDownList ID="ddlRole" runat="server" Width="231px" DataTextField="LookUpName"
                                            DataValueField="LookUpID" Enabled="false">
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        BP#
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtBPNumber" autocomplete="off" runat="server" Width="226px">
                                        </asp:TextBox>
                                        <cc1:AutoCompleteExtender ID="autoComplete1" runat="server" EnableCaching="true"
                                            BehaviorID="AutoCompleteEx" MinimumPrefixLength="2" TargetControlID="txtBPNumber"
                                            ServicePath="~/AutoComplete.asmx" ServiceMethod="GetCompletionList" CompletionInterval="1000"
                                            CompletionSetCount="20" CompletionListCssClass="autocomplete_completionListElement"
                                            CompletionListItemCssClass="autocomplete_listItem" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                            DelimiterCharacters=";, :" ShowOnlyCurrentWordInCompletionListItem="true">
                                            <Animations>
  <OnShow>
  <Sequence>
  <%-- Make the completion list transparent and then show it --%>
  <OpacityAction Opacity="0" />
  <HideAction Visible="true" />

  <%--Cache the original size of the completion list the first time
    the animation is played and then set it to zero --%>
  <ScriptAction Script="// Cache the size and setup the initial size
                                var behavior = $find('AutoCompleteEx');
                                if (!behavior._height) {
                                    var target = behavior.get_completionList();
                                    behavior._height = target.offsetHeight - 2;
                                    target.style.height = '0px';
                                }" />
  <%-- Expand from 0px to the appropriate size while fading in --%>
  <Parallel Duration=".4">
  <FadeIn />
  <Length PropertyKey="height" StartValue="0" 
	EndValueScript="$find('AutoCompleteEx')._height" />
  </Parallel>
  </Sequence>
  </OnShow>
  <OnHide>
  <%-- Collapse down to 0px and fade out --%>
  <Parallel Duration=".4">
  <FadeOut />
  <Length PropertyKey="height" StartValueScript=
	"$find('AutoCompleteEx')._height" EndValue="0" />
  </Parallel>
  </OnHide>
                                            </Animations>
                                        </cc1:AutoCompleteExtender>
                                    </td>
                                    <td>
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="3">
                                   <%-- </td>
                                    <td>--%>
                                        <asp:Button ForeColor="White" BackColor="#9494C8" EnableTheming="false" ID="btnSearch"
                                            runat="server" Text="Search" OnClick="btnSearch_Click" ValidationGroup="Save" />
                                        <asp:Button ForeColor="White" BackColor="#9494C8" EnableTheming="false" ID="btnClear"
                                            runat="server" Text=" Clear " OnClick="btnClear_Click" />
                                        <asp:Button ForeColor="White" BackColor="#9494C8" EnableTheming="false" ID="btnMassAssign"
                                            runat="server" Text="Account Match" OnClick="btnMassAssign_Click"  />
                                            <asp:Button ForeColor="White" BackColor="#9494C8" EnableTheming="false" ID="btnBulkUpload"
                                            runat="server" Text="Multiple User Upload" OnClick="btnBulkUpload_Click"  />
                                   <%-- </td>
                                    <td>--%>
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td style="width: 45%; vertical-align: middle; border-width: 1px; border-color: Black"
                            runat="server" id="tdRange">
                            <table cellpadding="0" cellspacing="0" width="100%" style="border-width: 1px; border-style: solid;
                                border-color: Black; margin-bottom: 10px;" runat="server" id="tblAccountRange"
                                visible="false">
                                <tr>
                                    <td colspan="2" align="center" class="ItemTemplate">
                                        <asp:Label ID="lblRangeSearch" Text="Account Range Search" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 50%">
                                        <br />
                                        Account&nbsp;
                                        <asp:DropDownList ID="ddlSearchstart" runat="server" Width="70%">
                                        </asp:DropDownList>
                                        <br />
                                    </td>
                                    <td style="width: 50%">
                                        <br />
                                        To&nbsp;
                                        <asp:DropDownList ID="ddlSearchend" runat="server" Width="80%">
                                        </asp:DropDownList>
                                        <br />
                                        &nbsp;
                                    </td>
                                </tr>
                            </table>
                            <table cellpadding="0" cellspacing="0" width="100%" style="border-width: 1px; border-style: solid;
                                border-color: Black; margin-bottom: 10px;" runat="server" id="tblBURange" visible="false">
                                <tr>
                                    <td colspan="2" align="center" class="ItemTemplate">
                                        <asp:Label ID="Label2" Text="Office Range Search" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 50%">
                                        <br />
                                        Account&nbsp;
                                        <asp:DropDownList ID="ddlBUstart" runat="server" Width="70%">
                                        </asp:DropDownList>
                                        <br />
                                    </td>
                                    <td style="width: 50%">
                                        <br />
                                        To&nbsp;
                                        <asp:DropDownList ID="ddlBUend" runat="server" Width="80%">
                                        </asp:DropDownList>
                                        <br />
                                        &nbsp;
                                    </td>
                                </tr>
                            </table>
                            <table cellpadding="0" cellspacing="0" width="100%" style="border-width: 1px; border-style: solid;
                                border-color: Black; margin-bottom: 10px;" runat="server" id="tblBrokerRange"
                                visible="false">
                                <tr>
                                    <td colspan="2" align="center" class="ItemTemplate">
                                        <asp:Label ID="Label3" Text="Broker Range Search" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 50%">
                                        <br />
                                        Account&nbsp;
                                        <asp:DropDownList ID="ddlBrokerstart" runat="server" Width="70%">
                                        </asp:DropDownList>
                                        <br />
                                    </td>
                                    <td style="width: 50%">
                                        <br />
                                        To&nbsp;
                                        <asp:DropDownList ID="ddlBrokerend" runat="server" Width="80%">
                                        </asp:DropDownList>
                                        <br />
                                        &nbsp;
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <table cellpadding="0" cellspacing="0" width="100%" style="border-color: Black; border-width: 1px;
                                border-style: solid;" runat="server" id="tblMassReassign" visible="false">
                                <tr>
                                    <td colspan="2" align="center" class="ItemTemplate">
                                        <asp:Label ID="lblHeader" Text="" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2" align="center">
                                        <asp:Label ID="lblErrorLog" runat="server" Style="color: Red"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td width="35%" align="right">
                                        <br />
                                        Assign By &nbsp;
                                    </td>
                                    <td align="left">
                                        <br />
                                        <asp:DropDownList ID="ddlAssignType" runat="server" Width="203px">
                                            <asp:ListItem Value="0">(Select)</asp:ListItem>
                                            <asp:ListItem Value="1">Account Number</asp:ListItem>
                                            <asp:ListItem Value="2">BP Number</asp:ListItem>
                                        </asp:DropDownList>
                                        <br />
                                    </td>
                                </tr>
                                 <tr id="trRolesMass" runat="server" visible="false">
                                    <td width="35%" align="right">
                                        <br />
                                        Role &nbsp;
                                    </td>
                                    <td align="left">
                                        <br />
                                        <asp:DropDownList ID="ddlRoleMass" runat="server"  DataTextField="LookUpName"
                                            DataValueField="LookUpID">
                                            
                                        </asp:DropDownList>
                                        <br />
                                    </td>
                                    </tr>
                                <tr>
                                    <td width="35%" align="right">
                                        <br />
                                        Upload Excel &nbsp;
                                    </td>
                                    <td align="left">
                                        <br />
                                        <asp:FileUpload ID="fleAssign" runat="server"  Width="201px"/>
                                        <br />
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center" colspan="2">
                                        <br />
                                        <asp:Button ForeColor="White" BackColor="#9494C8" ID="btnValidate" runat="server"
                                            Text="Validate" OnClick="btnValidate_Click"  Visible="false"/>
                                        <asp:Button ForeColor="White" BackColor="#9494C8" ID="btnProcess" runat="server"
                                            Text="Upload" OnClick="btnProcess_Click"/>
                                        <asp:Button ForeColor="White" BackColor="#9494C8" ID="btnClose" runat="server" Text="Close"
                                            OnClick="btnClose_Click" />
                                            <asp:Button ForeColor="White" BackColor="#9494C8" ID="btnDownload" runat="server" Text="Download Template"
                                            OnClick="btnDownload_Click" />
                                        <br />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                       &nbsp;
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td style="padding-top: 5px; width: 600px;" id="tdAccountsList" runat="server" visible="false">
                            <table width="100%">
                                <tr>
                                    <td>
                                        <asp:Label ID="lblAcctList" runat="server" Text="Account List" CssClass="h3"></asp:Label>
                                    </td>
                                    <td align="right" style="padding-right: 0px; font-weight: bold;">
                                        <asp:Label ID="lblRecords" runat="server"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                            <div id="divAcctlst" style="overflow-x: hidden; overflow-y: auto; height: 290px;">
                                <asp:ListView ID="lstAccountList" runat="server" OnItemCommand="lstAccountList_ItemCommand"
                                    OnSelectedIndexChanged="lstAccountList_SelectedIndexChanged" ClientIDMode="Static">
                                    <EmptyDataTemplate>
                                        <table id="tblAccntlist" class="panelContents" runat="server">
                                            <tr>
                                                <th align="center">
                                                    Acct No
                                                </th>
                                                <th align="center">
                                                    BP No
                                                </th>
                                                <th align="center">
                                                    Account Name
                                                </th>
                                                <th align="center">
                                                    Broker
                                                </th>
                                                <th align="center">
                                                    Office
                                                </th>
                                            </tr>
                                            <tr class="ItemTemplate">
                                                <td align="center" colspan="5">
                                                    <asp:Label ID="lblEmptyMessage" Text="---No Records Found ---" Font-Bold="true" runat="server"
                                                        Style="text-align: center" />
                                                </td>
                                            </tr>
                                        </table>
                                    </EmptyDataTemplate>
                                    <LayoutTemplate>
                                        <table id="tblAccntlist" class="panelContents" runat="server" style="width: 100%">
                                            <tr>
                                                <th align="center">
                                                    <asp:CheckBox ID="chkSelectAll" runat="server" Checked="true" onclick="toggleCheckBoxes(this)" />
                                                </th>
                                                <th align="center">
                                                    Acct No
                                                </th>
                                                <th align="center" width="5px">
                                                    BP No
                                                </th>
                                                <th align="center">
                                                    Account Name
                                                </th>
                                                <th align="center">
                                                    Broker
                                                </th>
                                                <th align="center">
                                                    Office
                                                </th>
                                            </tr>
                                            <tr id="itemPlaceholder" runat="server">
                                            </tr>
                                        </table>
                                    </LayoutTemplate>
                                    <ItemTemplate>
                                        <tr class="ItemTemplate">
                                            <td>
                                                <asp:HiddenField ID="hidCustmrid" runat="server" Value='<%# Bind("[Account No]") %>' />
                                                <asp:CheckBox ID="chkSelect" runat="server" Checked="true" onclick="toggleCheckBoxes1()" />
                                            </td>
                                            <td>
                                                <asp:LinkButton ID="lnkDetails" runat="server" CommandName="Details" Text='<%# Bind("[Account No]")%>'
                                                    CommandArgument='<%# Bind("[Account No]") %>'></asp:LinkButton>
                                            </td>
                                            <td>
                                                <asp:Label ID="lblBpNum" runat="server" Text='<%# Bind("[BP Number]")%>'></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="lblCustmrname" runat="server" Text='<%# Bind("AccountName")%>'></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="lblBroker" runat="server" Text='<%# Bind("[Broker Name]")%>'></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="lblOffice" runat="server" Text='<%# Bind("[BU/Office]")%>'></asp:Label>
                                            </td>
                                        </tr>
                                    </ItemTemplate>
                                    <AlternatingItemTemplate>
                                        <tr class="AlternatingItemTemplate">
                                            <td>
                                                <asp:HiddenField ID="hidCustmrid" runat="server" Value='<%# Bind("[Account No]") %>' />
                                                <asp:CheckBox ID="chkSelect" runat="server" Checked="true" onclick="toggleCheckBoxes1()" />
                                            </td>
                                            <td>
                                                <asp:LinkButton ID="lnkDetails" runat="server" CommandName="Details" Text='<%# Bind("[Account No]")%>'
                                                    CommandArgument='<%# Bind("[Account No]") %>'></asp:LinkButton>
                                            </td>
                                            <td>
                                                <asp:Label ID="lblBpNum" runat="server" Text='<%# Bind("[BP Number]")%>'></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="lblCustmrname" runat="server" Text='<%# Bind("AccountName")%>'></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="lblBroker" runat="server" Text='<%# Bind("[Broker Name]")%>'></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="lblOffice" runat="server" Text='<%# Bind("[BU/Office]")%>'></asp:Label>
                                            </td>
                                        </tr>
                                    </AlternatingItemTemplate>
                                </asp:ListView>
                                <table style="width: 100%;">
                                    <tr>
                                        <td align="right">
                                            <asp:UpdatePanel ID="upExcel" runat="server">
                                                <ContentTemplate>
                                                    <asp:Button runat="server" ID="btnExport" Text="Export to Excel" Visible="false"
                                                        OnClick="btnExport_Click" />
                                                </ContentTemplate>
                                                <Triggers>
                                                    <asp:PostBackTrigger ControlID="btnExport" />
                                                </Triggers>
                                            </asp:UpdatePanel>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <br />
                            <asp:HiddenField ID="hidPopShow" runat="server" />
                            <asp:HiddenField ID="hodPopOk" runat="server" />
                            <cc1:ModalPopupExtender runat="server" ID="modalAcctDetails" TargetControlID="hidPopShow"
                                PopupControlID="pnlAcctDetails" BackgroundCssClass="modalBackground" DropShadow="true"
                                CancelControlID="btnPopCancel" OkControlID="hodPopOk">
                            </cc1:ModalPopupExtender>
                            <asp:Panel runat="server" CssClass="modalPopup" ID="pnlAcctDetails" Style="border: solid 1px black;
                                display: none; width: 650px; padding: 0px" HorizontalAlign="Center">
                                <asp:Panel runat="Server" ID="Panel3" Style="width: 100%; cursor: move; padding: 0px;
                                    background-color: #CCCCCC; height: 20px; border: solid 1px Gray; color: Black;
                                    text-align: center; vertical-align: middle; font-weight: bold; vertical-align: middle;
                                    color: black; font-size: 12px">
                                    Account Responsibilities
                                </asp:Panel>
                                <div style="text-align: center; width: 100%; padding-bottom: 5px; background-color: White;">
                                    <table width="100%" align="center">
                                        <tr>
                                            <td align="center" width="100%">
                                                <asp:Label ID="lblAccountLevel" runat="server" Visible="false" Text="" CssClass="h3"></asp:Label>
                                                <asp:AISListView ID="lstViewAccountLevel" runat="server" Visible="false" Style="width: 100%">
                                                    <EmptyDataTemplate>
                                                        <table id="tblResponsibilities" class="panelContents" runat="server" width="100%">
                                                            <tr>
                                                                <th style="width: 50%" align="center">
                                                                    Responsibilities
                                                                </th>
                                                                <th style="width: 50%" align="center">
                                                                    Name
                                                                </th>
                                                            </tr>
                                                            <tr class="ItemTemplate">
                                                                <td align="center" colspan="2">
                                                                    <asp:Label ID="lblEmptyMessage" Text="---No Records Found ---" Font-Bold="true" runat="server"
                                                                        Style="text-align: center" />
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </EmptyDataTemplate>
                                                    <LayoutTemplate>
                                                        <table id="Table1" class="panelContents" runat="server" width="100%">
                                                            <tr>
                                                                <th align="center">
                                                                    Responsibilities
                                                                </th>
                                                                <th align="center">
                                                                    Name
                                                                </th>
                                                            </tr>
                                                            <tr id="itemPlaceholder" runat="server">
                                                            </tr>
                                                        </table>
                                                    </LayoutTemplate>
                                                    <ItemTemplate>
                                                        <tr class="ItemTemplate">
                                                            <td align="center">
                                                                <%# Eval("RESP_NAME")%>
                                                            </td>
                                                            <td align="center">
                                                                <%# Eval("FULLNAME")%>
                                                            </td>
                                                        </tr>
                                                    </ItemTemplate>
                                                    <AlternatingItemTemplate>
                                                        <tr class="AlternatingItemTemplate">
                                                            <td align="center">
                                                                <%# Eval("RESP_NAME")%>
                                                            </td>
                                                            <td align="center">
                                                                <%# Eval("FULLNAME")%>
                                                            </td>
                                                        </tr>
                                                    </AlternatingItemTemplate>
                                                </asp:AISListView>
                                                <br />
                                                <asp:Button ID="btnPopCancel" runat="server" Text="Close" />
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </asp:Panel>

                            <asp:HiddenField ID="hdnTempConfirm" runat="server" />
                            <cc1:ModalPopupExtender runat="server" ID="modalConfirm" TargetControlID="hdnTempConfirm" PopupControlID="pnlConfirm"
                                BackgroundCssClass="modalBackground" DropShadow="true">
                            </cc1:ModalPopupExtender>
                            <div style="float: left;">
                                <asp:Panel runat="server" CssClass="modalPopup" ID="pnlConfirm" Style="border: solid 1px black;
                                    display: none; width: 500px; padding: 0px" HorizontalAlign="Center">
                                    <asp:Panel runat="Server" ID="pnlTMChild" Style="width: 100%; cursor: move; padding: 0px;
                                        background-color: #CCCCCC; height: 20px; border: solid 1px Gray; color: Black;
                                        text-align: center; vertical-align: middle">
                                    </asp:Panel>
                                    <div style="text-align: center; width: 100%; padding-bottom: 5px; background-color: White;">
                                        <br />
                                        Warning: You are changing the responsibilities for<br />
                                        more than one customer<br />
                                        <br />
                                        <br />
                                        Do you want to continue?
                                        <br />
                                        <br />
                                        <asp:Button Width="60px" ID="btnOKpopup" runat="server" Text="OK" OnClick="btnOKpopup_Click" />
                                        <asp:Button Width="60px" ID="btnCancelpopup" runat="server" Text="Cancel" OnClick="btnCancelpopup_Click" />
                                        <br />
                                    </div>
                                </asp:Panel>
                            </div>
                        </td>
                        <td style="padding-left: 10px; width: 100%; padding-top: 5px;" id="tdResponsibilities"
                            runat="server" visible="false" width="100%">
                            <asp:Label ID="lblResponsibilities" runat="server" Text="Assign Responsibilities"
                                CssClass="h3"></asp:Label>
                            <asp:AISListView ID="lstAssignResponsibilities" runat="server" DataSourceID="ResponsibilityDataSource">
                                <LayoutTemplate>
                                    <table id="Table1" class="panelContents" runat="server" width="400px">
                                        <tr>
                                            <th align="center">
                                                Responsibilities
                                            </th>
                                            <th align="left">
                                                Name
                                            </th>
                                        </tr>
                                        <tr id="itemPlaceholder" runat="server">
                                        </tr>
                                    </table>
                                </LayoutTemplate>
                                <ItemTemplate>
                                    <tr class="ItemTemplate">
                                        <td>
                                            <asp:HiddenField ID="hfResponseid" runat="server" Value='<%# Eval("LookUpID")%>' />
                                            <%# Eval("LookUpName")%>
                                        </td>
                                        <td style="width: 275px;">
                                            <asp:DropDownList ID="ddlName" runat="server" Width="275px" DataSourceID="PersonDatasource"
                                                DataTextField="FULLNAME" DataValueField="PERSON_ID">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                </ItemTemplate>
                                <AlternatingItemTemplate>
                                    <tr class="AlternatingItemTemplate">
                                        <td>
                                            <asp:HiddenField ID="hfResponseid" runat="server" Value='<%#Eval("LookUpID")%>' />
                                            <%# Eval("LookUpName")%>
                                        </td>
                                        <td style="width: 275px;">
                                            <asp:DropDownList ID="ddlName" runat="server" Width="275px" DataSourceID="PersonDatasource"
                                                DataTextField="FULLNAME" DataValueField="PERSON_ID">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                </AlternatingItemTemplate>
                            </asp:AISListView>
                            <br />
                            &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
                            &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;<asp:Button ID="btnSave" runat="server"
                                Text="Save" OnClick="btnSave_Click" />
                            <asp:Button ID="btnResClear" runat="server" Text="Clear" OnClick="btnResClear_Click" />
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 40%">
                        </td>
                        <td style="width: 50px">
                        </td>
                    </tr>
                </table>
            </div>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnProcess" />
            <asp:PostBackTrigger ControlID="btnMassAssign" />
            <asp:PostBackTrigger ControlID="btnDownload" />
            <asp:PostBackTrigger ControlID="btnBulkUpload" />
            <asp:PostBackTrigger ControlID="btnValidate" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
