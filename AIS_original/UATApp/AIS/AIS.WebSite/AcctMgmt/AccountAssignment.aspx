<%@ Page Language="C#" MasterPageFile="~/Retro.master" AutoEventWireup="true" Inherits="AcctMgmt_AccountAssignment"
    CodeBehind="AccountAssignment.aspx.cs" %>

<%@ Register Src="~/App_Shared/AccountList.ascx" TagPrefix="AL" TagName="AccountList" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeaderPlaceHolder" runat="Server">
    <asp:Label ID="lblRetroInfo" runat="server" Text="Account Assignment" CssClass="h1"></asp:Label>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="MainPlaceHolder" runat="Server">
    <asp:ObjectDataSource ID="BrokerDataSource" runat="server" SelectMethod="GetBrokersForLookups"
        TypeName="ZurichNA.AIS.Business.Logic.BrokerBS"></asp:ObjectDataSource>
    <asp:ObjectDataSource ID="PersonDatasource" runat="server" SelectMethod="FillCRMusers"
        TypeName="ZurichNA.AIS.Business.Logic.PersonBS"></asp:ObjectDataSource>
    <!--  <asp:ObjectDataSource ID="BusinessUnitDataSource" runat="server" SelectMethod="GetBusinessUnits"
                            TypeName="ZurichNA.AIS.Business.Logic.BusinessUnitOfficeBS"></asp:ObjectDataSource> -->
    <asp:ObjectDataSource ID="BUOfficeDataSource" runat="server" SelectMethod="GetBUOffForLookups"
        TypeName="ZurichNA.AIS.Business.Logic.BusinessUnitOfficeBS"></asp:ObjectDataSource>
    <asp:ObjectDataSource ID="ResponsibilityDataSource" runat="server" SelectMethod="GetLookUpActiveDataWithoutSelect"
        TypeName="ZurichNA.AIS.Business.Logic.BLAccess">
        <SelectParameters>
            <asp:Parameter DefaultValue="RESPONSIBILITY" Name="lookUpTypeName" Type="String" />
        </SelectParameters>
    </asp:ObjectDataSource>
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
                            <td style="padding-left: 5px; padding-top: 12px;">
                                <asp:CheckBox AutoPostBack="true" ID="chkRangeSearch" runat="server" OnCheckedChanged="chkRangeSearch_CheckedChanged" />
                                Range Search
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblOfficename" runat="server" Text="BU/Office Name"></asp:Label>
                            </td>
                            <td style="padding-left: 3px">
                                <asp:DropDownList ID="ddlOffice" runat="server" Width="231px" DataSourceID="BUOfficeDataSource"
                                    DataTextField="LookupName" DataValueField="LookUpID">
                                </asp:DropDownList>
                                <%--<asp:CompareValidator ID="CompareddlOffice" runat="server" ControlToValidate="ddlOffice"
                        ValueToCompare="0" ErrorMessage="*" Operator="NotEqual" ValidationGroup="Save"></asp:CompareValidator>--%>
                            </td>
                            <td style="padding-left: 5px">
                                <asp:Button ForeColor="White" BackColor="#9494C8" EnableTheming="false" ID="btnSearch"
                                    runat="server" Text="Search" OnClick="btnSearch_Click" ValidationGroup="Save" />
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
                            <td style="padding-left: 5px">
                                <asp:Button ForeColor="White" BackColor="#9494C8" EnableTheming="false" ID="btnClear"
                                    runat="server" Text=" Clear " OnClick="btnClear_Click" />
                            </td>
                        </tr>
                    </table>
                </td>
                <td style="width: 45%; vertical-align: middle; border-width: 1px; border-color: Black"
                    runat="server" id="tdRange">
                    <table cellpadding="0" border="1" cellspacing="0" width="100%" style="border-width: 1px;
                        border-color: Black" runat="server" id="tblRange" visible="false">
                        <tr>
                            <td colspan="2" align="center" class="ItemTemplate">
                                <asp:Label ID="lblRangeSearch" Text="RANGE SEARCH" runat="server"></asp:Label>
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
                </td>
            </tr>
            <tr>
                <td style="padding-top: 5px; width: 600px;" id="tdAccountsList" runat="server" visible="false">
                    <asp:Label ID="lblAcctList" runat="server" Text="Account List" CssClass="h3"></asp:Label>
                    <div id="divAcctlst" runat="server" style="overflow: auto; height: 85px;" class="panelContents">
                        <asp:AISListView ID="lstAccountList" runat="server" OnItemCommand="lstAccountList_ItemCommand"
                            OnSelectedIndexChanged="lstAccountList_SelectedIndexChanged">
                            <EmptyDataTemplate>
                                <table id="tblAccntlist" class="panelContents" runat="server">
                                    <tr>
                                        <th align="center">
                                            Select
                                        </th>
                                        <th align="center">
                                            Account Number
                                        </th>
                                        <th align="center">
                                            Account Name
                                        </th>
                                    </tr>
                                    <tr class="ItemTemplate">
                                        <td align="center" colspan="3">
                                            <asp:Label ID="lblEmptyMessage" Text="---No Records Found ---" Font-Bold="true" runat="server"
                                                Style="text-align: center" />
                                        </td>
                                    </tr>
                                </table>
                            </EmptyDataTemplate>
                            <LayoutTemplate>
                                <table id="tblAccntlist" class="panelContents" runat="server" style="width: 97%">
                                    <tr>
                                        <th>
                                        </th>
                                        <th align="center">
                                            Select
                                        </th>
                                        <th align="center">
                                            Account Number
                                        </th>
                                        <th align="center">
                                            Account Name
                                        </th>
                                    </tr>
                                    <tr id="itemPlaceholder" runat="server">
                                    </tr>
                                </table>
                            </LayoutTemplate>
                            <ItemTemplate>
                                <tr class="ItemTemplate">
                                    <td>
                                        <asp:LinkButton ID="lnkDetails" runat="server" CommandName="Details" Text="Details"
                                            CommandArgument='<%# Bind("CUSTMR_ID") %>'></asp:LinkButton>
                                    </td>
                                    <td>
                                        <asp:HiddenField ID="hidCustmrid" runat="server" Value='<%# Bind("CUSTMR_ID") %>' />
                                        <asp:CheckBox ID="chkSelect" runat="server" Checked="true" />
                                    </td>
                                    <td>
                                        <asp:Label ID="lblCustmr" runat="server" Text='<%# Bind("CUSTMR_ID")%>'></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblCustmrname" runat="server" Text='<%# Bind("CUSTMR_NAME")%>'></asp:Label>
                                    </td>
                                </tr>
                            </ItemTemplate>
                            <AlternatingItemTemplate>
                                <tr class="AlternatingItemTemplate">
                                    <td>
                                        <asp:LinkButton ID="lnkDetails" runat="server" CommandName="Details" Text="Details"
                                            CommandArgument='<%# Bind("CUSTMR_ID") %>'></asp:LinkButton>
                                    </td>
                                    <td>
                                        <asp:HiddenField ID="hidCustmrid" runat="server" Value='<%# Bind("CUSTMR_ID") %>' />
                                        <asp:CheckBox ID="chkSelect" runat="server" Checked="true" />
                                    </td>
                                    <td>
                                        <!--<%# Eval("CUSTMR_ID")%> -->
                                        <asp:Label ID="lblCustmr" runat="server" Text='<%# Bind("CUSTMR_ID")%>'></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblCustmrname" runat="server" Text='<%# Bind("CUSTMR_NAME")%>'></asp:Label>
                                        <!-- <%# Eval("CUSTMR_NAME")%> -->
                                    </td>
                                </tr>
                            </AlternatingItemTemplate>
                        </asp:AISListView>
                    </div>
                    <br />
                    <asp:Label ID="lblAccountLevel" runat="server" Visible="false" Text="" CssClass="h3"></asp:Label>
                    <asp:AISListView ID="lstViewAccountLevel" runat="server" Visible="false">
                        <EmptyDataTemplate>
                            <table id="tblResponsibilities" class="panelContents" runat="server">
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
                            <table id="Table1" class="panelContents" runat="server">
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
</asp:Content>
