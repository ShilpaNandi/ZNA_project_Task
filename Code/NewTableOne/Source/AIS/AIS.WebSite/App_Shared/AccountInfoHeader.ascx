<%@ Control Language="C#" AutoEventWireup="true" Inherits="App_Shared_MasterUser"
    CodeBehind="AccountInfoHeader.ascx.cs" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<style type="text/css">
    .style1
    {
        height: 6px;
    }
</style>
<ajaxToolkit:CollapsiblePanelExtender ID="CollapseAccountHeader" runat="Server" TargetControlID="pnlHeaderInformation"
    ExpandControlID="pnlCollapse" CollapseControlID="pnlCollapse" TextLabelID="lblHideShow"
    ImageControlID="imgHideShow" ExpandedImage="/images/collapse.jpg" CollapsedImage="~/images/expand.jpg"
    ExpandDirection="Vertical" />
<asp:Panel ID="pnlCollapse" runat="server" Width="910px" Height="15px" CssClass="policyInformationHeader">
    <table width="910px" style=" height:auto" border="0" cellpadding="0"  cellspacing="0">
        <tr style=" height:auto">
            <td align="left" style="height: auto">
                <asp:Label ID="lblHeaderTitle" ForeColor="White" Font-Bold="true" runat="server"></asp:Label>
            </td>
            <td align="right" style="height: auto">
                <asp:Label ID="lblHideShow" runat="server"></asp:Label>
                <img id="imgHideShow" src="/images/collapse.jpg" height="13px" alt="" />
            </td>
        </tr>
    </table>
</asp:Panel>
<asp:Panel ID="pnlHeaderInformation" runat="server" BorderWidth="1px" Width="910px">
    <table style="border-style: solid; border-width: 1px; border-color: inherit; width: 910px;">
        <tr>
            <td style="width: 20%; text-align: right" id="td1" runat="server">
                <asp:Label ID="lblRow1Title1" runat="server" Text="Account Name:"></asp:Label>
            </td>
            <td style="width: 30%; font-weight: bold; text-align: left">
                <asp:Label ID="lbl1genericrow1" runat="server"></asp:Label>
            </td>
            <td style="width: 20%; text-align: right" id="td2" runat="server">
                <asp:Label ID="lblRow1Title2" runat="server" Text="Account Number:"></asp:Label>
            </td>
            <td style="width: 30%; font-weight: bold; text-align: left">
                <asp:Label ID="lbl2genericrow1" runat="server"></asp:Label>
            </td>
        </tr>
        <tr>
            <td id="td3" runat="server" style="width: 20%; text-align: right">
                <asp:Label ID="lblRow2Title1" runat="server" Text="SSCGID:"></asp:Label>
            </td>
            <td style="font-weight: bold; width: 30%; text-align: left;">
                <asp:Label ID="lbl1genericrow2" runat="server"></asp:Label>
            </td>
            <td id="td4" runat="server" style="width: 20%; text-align: right">
                <asp:Label ID="lblRow2Title2" runat="server" Text="Business Partner No:"></asp:Label>
            </td>
            <td style="font-weight: bold; width: 30%; text-align: left;">
                <asp:Label ID="lbl2genericrow2" runat="server" Text="Testing"></asp:Label>
            </td>
        </tr>
    </table>
</asp:Panel>
