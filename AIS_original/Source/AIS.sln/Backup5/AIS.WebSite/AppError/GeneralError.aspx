<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Retro.master"
    Inherits="GeneralError" Title="General Error Page" Codebehind="GeneralError.aspx.cs" %>
<asp:Content ID="Content2" ContentPlaceHolderID="HeaderPlaceHolder" runat="Server">
    <table width="910" style="text-align: right" cellpadding="0" cellspacing="0">
        <tr>
            <td align="left">
                <asp:Label ID="GAILabel" runat="server" Text="Application Error Web Page" CssClass="h1"></asp:Label>
            </td>
        </tr>
    </table>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="MainPlaceHolder" runat="Server">
    <div style="padding-left: 20px">
        <table width="100px" style="border: 1px">
            <tr style="height: auto">
                <asp:Label ID="Label2" runat="server" Width="38px" Font-Size="Small" ForeColor="Black"
                    Height="19px">Error:</asp:Label>
            </tr>
            <tr>
                <asp:Label ID="lblError" runat="server" Width="623px" Font-Size="Large" ForeColor="#003399"
                    Height="37px"></asp:Label>
            </tr>
            <tr style="height: auto">
                <asp:Label ID="Label3" runat="server" Width="132px" Font-Size="Small" ForeColor="Black"
                    Height="18px">Comment/Suggestion:</asp:Label>
            </tr>
            <tr>
                <asp:Label ID="lblComment" runat="server" Width="623px" Font-Size="Medium" ForeColor="#003399"
                    Height="37px"></asp:Label>
            </tr>
            <br />
            <tr style="height: auto; padding-top: 30px">
                <asp:Label ID="Label1" runat="server" Width="232px" Font-Size="Small" ForeColor="Black"
                    Height="18px">Additional Information:</asp:Label>
            </tr>
            <tr>
                <asp:Label ID="lblErrorMsg" runat="server" Width="623px" Font-Size="Medium" ForeColor="#003399"
                    Height="400px"></asp:Label>
                <%--                <asp:TextBox ID="txtErrorMsg" runat="server" ReadOnly="true" Rows="45" TextMode="MultiLine"
                    BorderWidth="0" BackColor="#F5F5F5" Wrap="true" Width="700px" Font-Size="Medium"
                    ForeColor="#003399" Height="400px"></asp:TextBox>--%>
            </tr>
        </table>
    </div>
</asp:Content>
