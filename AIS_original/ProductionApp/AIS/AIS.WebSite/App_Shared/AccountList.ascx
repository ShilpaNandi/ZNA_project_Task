<%@ Control Language="C#" AutoEventWireup="true" Inherits="App_Shared_UserDropdown"
    CodeBehind="AccountList.ascx.cs" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:UpdatePanel ID="drpUpdatepanel" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <asp:Panel ID="pnlAccntlist" runat="server">
            <table>
                <tr>
                    <td>
                        <asp:LinkButton ID="LinkButton7" Font-Size="Small"   runat="server" OnClick="LButton_Click"
                            ToolTip="Displays Accounts whose name starts with letters starting [0 through B]">0</asp:LinkButton>
                        &nbsp;
                        <asp:LinkButton ID="LinkButton10" Font-Size="Small" runat="server" OnClick="LButton_Click"
                            ToolTip="Displays Accounts whose name starts with letters starting [C ]">C</asp:LinkButton>
                        &nbsp;
                        <asp:LinkButton ID="LinkButton11" Font-Size="Small" runat="server" OnClick="LButton_Click"
                            ToolTip="Displays Accounts whose name starts with letters starting [D through E]">D</asp:LinkButton>
                        &nbsp;
                        <asp:LinkButton ID="LinkButton12" Font-Size="Small" runat="server" OnClick="LButton_Click"
                            ToolTip="Displays Accounts whose name starts with letters starting [F through G]">F</asp:LinkButton>
                        &nbsp;
                        <asp:LinkButton ID="LinkButton13" Font-Size="Small" runat="server" OnClick="LButton_Click"
                            ToolTip="Displays Accounts whose name starts with letters starting [H through I]">H</asp:LinkButton>
                        &nbsp;
                        <asp:LinkButton ID="LinkButton14" Font-Size="Small" runat="server" OnClick="LButton_Click"
                            ToolTip="Displays Accounts whose name starts with letters starting [J through L]">J</asp:LinkButton>
                        &nbsp;
                        <asp:LinkButton ID="LinkButton15" Font-Size="Small" runat="server" OnClick="LButton_Click"
                            ToolTip="Displays Accounts whose name starts with letters starting [M through O]">M</asp:LinkButton>
                        &nbsp;
                        <asp:LinkButton ID="LinkButton17" Font-Size="Small" runat="server" OnClick="LButton_Click"
                            ToolTip="Displays Accounts whose name starts with letters starting [P through R]">P</asp:LinkButton>
                        &nbsp;
                        <asp:LinkButton ID="LinkButton1" Font-Size="Small" runat="server" OnClick="LButton_Click"
                            ToolTip="Displays Accounts whose name starts with letters starting [S ]">S</asp:LinkButton>
                        &nbsp;
                        <asp:LinkButton ID="LinkButton19" Font-Size="Small" runat="server" OnClick="LButton_Click"
                            ToolTip="Displays Accounts whose name starts with letters starting [T through U]">T</asp:LinkButton>
                        &nbsp;
                        <asp:LinkButton ID="LinkButton20" Font-Size="Small" runat="server" OnClick="LButton_Click"
                            ToolTip="Displays Accounts whose name starts with letters starting [V through Z]">V</asp:LinkButton>
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <tr>
                        <td>
                            <asp:DropDownList ID="ddlAccountlist" runat="server" Width="230px" OnSelectedIndexChanged="ddlAccountlist_SelectedIndexChanged"
                                AutoPostBack="true">
                            </asp:DropDownList>
                        </td>
                    </tr>
            </table>
        </asp:Panel>
    </ContentTemplate>
    <Triggers>
    </Triggers>
</asp:UpdatePanel>
