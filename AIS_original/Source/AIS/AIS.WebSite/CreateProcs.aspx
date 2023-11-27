<%@ Page Language="C#" MasterPageFile="~/Retro.master" AutoEventWireup="true" Inherits="CreateProcs"
    Title="Generate Stored Procs" CodeBehind="CreateProcs.aspx.cs" %>

<asp:Content ID="Content3" ContentPlaceHolderID="HeaderPlaceHolder" runat="Server">
    <p>
        (</p>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="MainPlaceHolder" runat="Server">
    <table>
        <tr>
            <td valign="bottom">
                <asp:Label ID="Label3" runat="server" Text="Target Directory Name:">    </asp:Label>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="Label4" runat="server" Font-Size="Small" Font-Italic="true" Text="(Example: C:\projects)">    </asp:Label>
            </td>
            <td style="width: 242px">
                <asp:TextBox ID="txtPath" runat="server" Text="C:\temp" Width="261px"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="lblInvalidPath" runat="server" Font-Size="Large" ForeColor="red" Font-Italic="true"
                    Text="Invalid path has been entered.">    </asp:Label>
            </td>
        </tr>
    </table>
    <asp:Button ID="Button2" runat="server" Text="Create All SPs" OnClick="cmdOpen_OnClick3" />
    <table>
        <tr>
            <td>
                <asp:Label ID="Label1" runat="server" Text="CreatePair" Font-Size="Large" ForeColor="Blue"></asp:Label>
                <br />
                <asp:ListBox ID="ListBox1" runat="server" Width="284px" Height="413px" Enabled="False">
                    <asp:ListItem Value="PREM_ADJ">AddPREM_ADJCpy</asp:ListItem>
                    <asp:ListItem Value="PREM_ADJ_PERD">AddPREM_ADJ_PERDCpy</asp:ListItem>
                    <asp:ListItem Value="PREM_ADJ_ARIES_CLRING">AddPREM_ADJ_ARIES_CLRINGCpy</asp:ListItem>
                    <asp:ListItem Value="PREM_ADJ_COMB_ELEMTS">AddPREM_ADJ_COMB_ELEMTSCpy</asp:ListItem>
                    <asp:ListItem Value="PREM_ADJ_LOS_REIM_FUND_POST">AddPREM_ADJ_LOS_REIM_FUND_POSCpy</asp:ListItem>
                    <asp:ListItem Value="PREM_ADJ_MISC_INVC">AddPREM_ADJ_MISC_INVCCpy</asp:ListItem>
                    <asp:ListItem Value="PREM_ADJ_NY_SCND_INJR_FUND">AddPREM_ADJ_NY_SCND_INJR_FUNDCpy</asp:ListItem>
                    <asp:ListItem Value="PREM_ADJ_PAID_LOS_BIL">AddPREM_ADJ_PAID_LOS_BILCpy</asp:ListItem>
                    <asp:ListItem Value="PREM_ADJ_PARMET_DTL">AddPREM_ADJ_PARMET_DTLCpy</asp:ListItem>
                    <asp:ListItem Value="PREM_ADJ_PARMET_SETUP">AddPREM_ADJ_PARMET_SETUPCpy</asp:ListItem>
                    <asp:ListItem Value="PREM_ADJ_RETRO">AddPREM_ADJ_RETROCpy</asp:ListItem>
                    <asp:ListItem Value="PREM_ADJ_RETRO_DTL">AddPREM_ADJ_RETRO_DTLCpy</asp:ListItem>
                    <asp:ListItem Value="ARMIS_LOS_POL">AddARMIS_LOS_POLCpy</asp:ListItem>
                    <asp:ListItem Value="ARMIS_LOS_EXC">AddARMIS_LOS_EXCCpy</asp:ListItem>
                    <asp:ListItem Value="PREM_ADJ_PERD_TOT">AddPREM_ADJ_PERD_TOTCpy</asp:ListItem>
                </asp:ListBox>
                <br />
                <asp:Button ID="Create" runat="server" Text="Create 'CreatePair' SPs" OnClick="cmdOpen_OnClick1" />
                <br />
            </td>
        </tr>
    </table>
</asp:Content>
