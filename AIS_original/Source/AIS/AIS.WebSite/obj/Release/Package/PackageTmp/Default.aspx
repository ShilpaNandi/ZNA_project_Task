<%@ Page Language="C#" MasterPageFile="~/Retro.master" AutoEventWireup="false" Inherits="Default"
    Title="Sample Page" CodeBehind="Default.aspx.cs" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content3" ContentPlaceHolderID="HeaderPlaceHolder" runat="Server">
    <p>
        (</p>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="MainPlaceHolder" runat="Server">
    <table cellpadding="0" cellspacing="0" border="0" width="700px">
        <tr>
            <td class="h1">
                Page Under Construction
            </td>
        </tr>
    </table>
    <br />
    <table>
        <tr>
            <td>
                E-mail Recipient
            </td>
            <td style="width: 413px">
                <asp:TextBox ID="TextBox1" runat="server" Width="227px"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                E-mail Description
            </td>
            <td style="width: 413px">
                <asp:TextBox ID="TextBox2" runat="server" Width="221px"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                Subject
            </td>
            <td style="width: 413px">
                <asp:TextBox ID="TextBox3" runat="server" Width="228px"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                Body
            </td>
            <td style="width: 413px">
                <asp:TextBox ID="TextBox4" runat="server" Width="223px"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                Attachment
            </td>
            <td style="width: 413px">
                <asp:TextBox ID="TextBox5" runat="server" Width="225px"></asp:TextBox>
                <asp:Button ID="Button1" runat="server" OnClick="Button1_Click" Text="Send E-mail" />
            </td>
        </tr>
        <tr>
            <td style="width: 413px">
                <asp:Button ID="Button2" runat="server" OnClick="Button2_Click" Text="Send FTP" />
            </td>
        </tr>
        <tr>
            <td>
                &nbsp;
            </td>
            <td style="width: 413px">
                <%--                <asp:ImageButton runat="Server" ID="Image1" ImageUrl="~/images/Calendar_scheduleHS.png"
                    AlternateText="Click to show calendar" /><br />--%>
                <%--                <ajaxToolkit:CalendarExtender ID="calendarButtonExtender" runat="server" TargetControlID="TextBox6"
                    PopupButtonID="Image1" Format="MM/dd/yyyy" />
                <div style="font-size: 90%">
                    <em>(Click the image button to show the calendar; this calendar dismisses 
                    automatically when you choose a date)te)</em></div>--%>
            </td>
        </tr>
    </table>
</asp:Content>
