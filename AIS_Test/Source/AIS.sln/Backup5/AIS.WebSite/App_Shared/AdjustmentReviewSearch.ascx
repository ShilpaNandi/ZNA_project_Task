<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AdjustmentReviewSearch.ascx.cs"
    Inherits="App_Shared_AdjustmentReviewSearch" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:UpdatePanel ID="drpUpdatepanel" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <asp:ObjectDataSource ID="odsValuationDate" runat="server" SelectMethod="GetValDateSearch"
            TypeName="ZurichNA.AIS.Business.Logic.PremAdjustmentBS">
            <SelectParameters>
                <asp:Parameter Name="straccountID" Type="String" />
            </SelectParameters>
        </asp:ObjectDataSource>
        <asp:ObjectDataSource ID="odsAdjNumber" runat="server" SelectMethod="GetARAdjNumberSearch"
            TypeName="ZurichNA.AIS.Business.Logic.PremAdjustmentBS">
            <SelectParameters>
                <asp:Parameter Name="straccountID" Type="String" />
                <asp:Parameter Name="strValDate" Type="String" />
            </SelectParameters>
        </asp:ObjectDataSource>
        <asp:ObjectDataSource ID="odsProgramPeriod" runat="server" SelectMethod="GetARProgramPeriodSearch"
            TypeName="ZurichNA.AIS.Business.Logic.ProgramPeriodsBS">
            <SelectParameters>
                <asp:Parameter Name="strPremAdjID" Type="String" />
                 <asp:Parameter Name="straccountID" Type="String" />
            </SelectParameters>
        </asp:ObjectDataSource>
        <div>
            <table>
                <tr>
                    <td width="80%">
                        <table cellpadding="0">
                            <tr>
                                <td align="left" width="40%">
                                    <asp:Label ID="lblAccountName" runat="server" Text="Account Name "></asp:Label>
                                </td>
                                <td align="left" valign="top" width="60%" style="padding-left: 46px">
                                    <asp:DropDownList ID="ddlAccountlist" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlAccountlist_SelectedIndexChanged"
                                        Width="253px">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td align="left" valign="top" width="40%" style="padding-top: 3px;">
                                    <asp:Label ID="lblValDate" runat="server" Text="Valuation Date "></asp:Label>
                                </td>
                                <td align="left" valign="top" width="60%" style="padding-left: 46px; padding-top: 3px;">
                                    <asp:DropDownList ID="ddlValDate" runat="server" AutoPostBack="true" Width="253px"
                                        OnSelectedIndexChanged="ddlValDate_SelectedIndexChanged">
                                        <asp:ListItem Value="0" Text="(Select)"></asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td align="left" width="40%" nowrap style="padding-top: 3px;">
                                    <asp:Label ID="lblAdjNumber" runat="server" Text="Adjustment Number"></asp:Label>
                                </td>
                                <td align="left" valign="top" width="60%" style="padding-left: 46px; padding-top: 3px;">
                                    <asp:DropDownList ID="ddlAdjNumber" runat="server" AutoPostBack="true" Width="253px"
                                        OnDataBound="ddlAdjNumber_DataBound" OnSelectedIndexChanged="ddlAdjNumber_SelectedIndexChanged">
                                        <asp:ListItem Value="0" Text="(Select)"></asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td align="left" width="40%" nowrap style="padding-top: 3px;">
                                    <asp:Label ID="lblProgramPeriod" runat="server" Text="Program Period "></asp:Label>
                                </td>
                                <td align="left" valign="top" width="60%" style="padding-left: 46px; padding-top: 3px;">
                                    <asp:DropDownList ID="ddlProgramPeriod" runat="server" AutoPostBack="true" Width="253px"
                                        OnDataBound="ddlProgramPeriod_DataBound" OnSelectedIndexChanged="ddlProgramPeriod_SelectedIndexChanged">
                                        <asp:ListItem Value="0" Text="(Select)"></asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                            </tr>
                        </table>
                    </td>
                    <td width="20%" style="vertical-align: middle">
                        <asp:Button ID="btnSearch" runat="server" CommandName="SEARCH" OnCommand="UCAdjustmentReviewSearch_Click"
                            Text="Search" ValidationGroup="vgSearch" ToolTip="Click here to Search" />
                    </td>
                </tr>
            </table>
        </div>
    </ContentTemplate>
    <Triggers>
        <asp:AsyncPostBackTrigger ControlID="ddlAccountlist" EventName="SelectedIndexChanged" />
    </Triggers>
</asp:UpdatePanel>
