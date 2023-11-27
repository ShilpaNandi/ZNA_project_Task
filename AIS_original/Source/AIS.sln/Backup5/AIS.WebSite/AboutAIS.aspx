<%@ Page Title="" Language="C#" MasterPageFile="~/Retro.master" AutoEventWireup="true" CodeBehind="AboutAIS.aspx.cs" Inherits="ZurichNA.AIS.WebSite.AboutAIS" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeaderPlaceHolder" runat="server">
    <%--<style>
        table
        {
            width: 100%;
        }
        th
        {
            border: 0.1px solid black;
            border-collapse: collapse;
            font-family: Verdana, Arial, Helvetica;
            font-size: 10px;
            background-color: #cccccc;
        }
        td
        {
            padding: 5px;
            font-family: Verdana, Arial, Helvetica;
            font-size: 10px;
        }
    </style>--%>
    <script type="text/javascript">

        var mainItemColor = "#ffffff";          //white
        var alternatingItemColor = "#f5f5f5";   //gray
        var selectedItemColor = "#ffff82";     //light yellow
        var mouseOverColor = "#e7e7ff";         //light purple - need to verify this is the correct light purple
        var selectedItem = null;

        var oldSelectedMain = null;
        var oldSelectedAlternating = null;


        function setMainItemMouseOutColor(item) {
            if (selectedItem != item && item.style != null)
                item.style.backgroundColor = mainItemColor;
        }

        function setAlternatingItemMouseOutColor(item) {
            if (selectedItem != item && item.style != null)
                item.style.backgroundColor = alternatingItemColor;
        }

        function setItemMouseOverColor(item) {
            item.style.cursor = 'hand';
            if (item.style.backgroundColor != selectedItemColor)
                item.style.backgroundColor = mouseOverColor;

        }

    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainPlaceHolder" runat="server">
    <table style="align: centre" width="100%">
        <tr>
            <th colspan="2" align="center" style="background-color:#00389C;color:White">
                Web Configuration Details
            </th>
        </tr>
        <tr>
            <td>
                <asp:ListView ID="lvConfigurations" runat="server">
                    <LayoutTemplate>
                        <table runat="server" id="tblAuditHistory" class="panelContents" cellpadding="0"
                            cellspacing="0">
                            <tr id="trHeader" runat="server" class="panelSearchHeader">
                                <th id="Th1" runat="server" width="30%">
                                    Key
                                </th>
                                <th id="Th2" runat="server" width="70%">
                                    Value
                                </th>
                            </tr>
                            <tr id="itemPlaceholder" runat="server">
                            </tr>
                        </table>
                    </LayoutTemplate>
                    <EmptyDataTemplate>
                        <table id="Table2" runat="server" style="background-color: #FFFFFF; border-collapse: collapse;
                            border-color: #999999; border-style: none; border-width: 1px;">
                            <tr>
                                <td colspan="2">
                                    No data was returned.
                                </td>
                            </tr>
                        </table>
                    </EmptyDataTemplate>
                    <ItemTemplate>
                        <tr id="trIT" style="background-color: #FFFFFF; color: #000000;" onmouseover="setItemMouseOverColor(this);"
                            onmouseout="setMainItemMouseOutColor(this);">
                            <td id="tdEvent">
                                <asp:Label ID="lblKey" runat="server" Text='<%# Eval("Key") %>' Style="cursor: default"></asp:Label>
                            </td>
                            <td id="tdEntity">
                                <asp:Label ID="lblValue" runat="server" Text='<%# Eval("Value") %>' Style="cursor: default"></asp:Label>
                            </td>
                        </tr>
                    </ItemTemplate>
                    <AlternatingItemTemplate>
                        <tr id="trIT" style="background-color: #F5F5F5; color: #000000;" onmouseover="setItemMouseOverColor(this);"
                            onmouseout="setAlternatingItemMouseOutColor(this);">
                            <td id="tdEvent">
                                <asp:Label ID="lblKey" runat="server" Text='<%# Eval("Key") %>' Style="cursor: default"></asp:Label>
                            </td>
                            <td id="tdEntity">
                                <asp:Label ID="lblValue" runat="server" Text='<%# Eval("Value") %>' Style="cursor: default"></asp:Label>
                            </td>
                        </tr>
                    </AlternatingItemTemplate>
                </asp:ListView>
            </td>
        </tr>
        <tr>
            <th colspan="2" align="center" style="background-color:#00389C;color:White">
                ARiES Configuration Details
            </th>
        </tr>
        <tr>
            <td>
            <asp:ListView ID="lvConfigurationsAries" runat="server">
                    <LayoutTemplate>
                        <table runat="server" id="tblAuditHistory" class="panelContents" cellpadding="0"
                            cellspacing="0">
                            <tr id="trHeader" runat="server" class="panelSearchHeader">
                                <th id="Th1" runat="server" width="30%">
                                    Key
                                </th>
                                <th id="Th2" runat="server" width="70%">
                                    Value
                                </th>
                            </tr>
                            <tr id="itemPlaceholder" runat="server">
                            </tr>
                        </table>
                    </LayoutTemplate>
                    <EmptyDataTemplate>
                        <table id="Table2" runat="server" style="background-color: #FFFFFF; border-collapse: collapse;
                            border-color: #999999; border-style: none; border-width: 1px;">
                            <tr>
                                <td colspan="2">
                                    No data was returned.
                                </td>
                            </tr>
                        </table>
                    </EmptyDataTemplate>
                    <ItemTemplate>
                        <tr id="trIT" style="background-color: #FFFFFF; color: #000000;" onmouseover="setItemMouseOverColor(this);"
                            onmouseout="setMainItemMouseOutColor(this);">
                            <td id="tdEvent">
                                <asp:Label ID="lblKey" runat="server" Text='<%# Eval("Key") %>' Style="cursor: default"></asp:Label>
                            </td>
                            <td id="tdEntity">
                                <asp:Label ID="lblValue" runat="server" Text='<%# Eval("Value") %>' Style="cursor: default"></asp:Label>
                            </td>
                        </tr>
                    </ItemTemplate>
                    <AlternatingItemTemplate>
                        <tr id="trIT" style="background-color: #F5F5F5; color: #000000;" onmouseover="setItemMouseOverColor(this);"
                            onmouseout="setAlternatingItemMouseOutColor(this);">
                            <td id="tdEvent">
                                <asp:Label ID="lblKey" runat="server" Text='<%# Eval("Key") %>' Style="cursor: default"></asp:Label>
                            </td>
                            <td id="tdEntity">
                                <asp:Label ID="lblValue" runat="server" Text='<%# Eval("Value") %>' Style="cursor: default"></asp:Label>
                            </td>
                        </tr>
                    </AlternatingItemTemplate>
                </asp:ListView>

            </td>
        </tr>
        <tr>
            <th colspan="2" align="center" style="background-color:#00389C;color:White">
                Cesar Configuration Details
            </th>
        </tr>
        <tr>
            <td>
                <asp:ListView ID="lvConfigurationsCesar" runat="server">
                    <LayoutTemplate>
                        <table runat="server" id="tblAuditHistory" class="panelContents" cellpadding="0"
                            cellspacing="0">
                            <tr id="trHeader" runat="server" class="panelSearchHeader">
                                <th id="Th1" runat="server" width="30%">
                                    Key
                                </th>
                                <th id="Th2" runat="server" width="70%">
                                    Value
                                </th>
                            </tr>
                            <tr id="itemPlaceholder" runat="server">
                            </tr>
                        </table>
                    </LayoutTemplate>
                    <EmptyDataTemplate>
                        <table id="Table2" runat="server" style="background-color: #FFFFFF; border-collapse: collapse;
                            border-color: #999999; border-style: none; border-width: 1px;">
                            <tr>
                                <td colspan="2">
                                    No data was returned.
                                </td>
                            </tr>
                        </table>
                    </EmptyDataTemplate>
                    <ItemTemplate>
                        <tr id="trIT" style="background-color: #FFFFFF; color: #000000;" onmouseover="setItemMouseOverColor(this);"
                            onmouseout="setMainItemMouseOutColor(this);">
                            <td id="tdEvent">
                                <asp:Label ID="lblKey" runat="server" Text='<%# Eval("Key") %>' Style="cursor: default"></asp:Label>
                            </td>
                            <td id="tdEntity">
                                <asp:Label ID="lblValue" runat="server" Text='<%# Eval("Value") %>' Style="cursor: default"></asp:Label>
                            </td>
                        </tr>
                    </ItemTemplate>
                    <AlternatingItemTemplate>
                        <tr id="trIT" style="background-color: #F5F5F5; color: #000000;" onmouseover="setItemMouseOverColor(this);"
                            onmouseout="setAlternatingItemMouseOutColor(this);">
                            <td id="tdEvent">
                                <asp:Label ID="lblKey" runat="server" Text='<%# Eval("Key") %>' Style="cursor: default"></asp:Label>
                            </td>
                            <td id="tdEntity">
                                <asp:Label ID="lblValue" runat="server" Text='<%# Eval("Value") %>' Style="cursor: default"></asp:Label>
                            </td>
                        </tr>
                    </AlternatingItemTemplate>
                </asp:ListView>
            </td>
        </tr>
        <tr>
            <th colspan="2" align="center" style="background-color:#00389C;color:White">
                ARMIS Configuration Details
            </th>
        </tr>
        <tr>
            <td>
                <asp:ListView ID="lvConfigurationsArmis" runat="server">
                    <LayoutTemplate>
                        <table runat="server" id="tblAuditHistory" class="panelContents" cellpadding="0"
                            cellspacing="0">
                            <tr id="trHeader" runat="server" class="panelSearchHeader">
                                <th id="Th1" runat="server" width="30%">
                                    Key
                                </th>
                                <th id="Th2" runat="server" width="70%">
                                    Value
                                </th>
                            </tr>
                            <tr id="itemPlaceholder" runat="server">
                            </tr>
                        </table>
                    </LayoutTemplate>
                    <EmptyDataTemplate>
                        <table id="Table2" runat="server" style="background-color: #FFFFFF; border-collapse: collapse;
                            border-color: #999999; border-style: none; border-width: 1px;">
                            <tr>
                                <td colspan="2">
                                    No data was returned.
                                </td>
                            </tr>
                        </table>
                    </EmptyDataTemplate>
                    <ItemTemplate>
                        <tr id="trIT" style="background-color: #FFFFFF; color: #000000;" onmouseover="setItemMouseOverColor(this);"
                            onmouseout="setMainItemMouseOutColor(this);">
                            <td id="tdEvent">
                                <asp:Label ID="lblKey" runat="server" Text='<%# Eval("Key") %>' Style="cursor: default"></asp:Label>
                            </td>
                            <td id="tdEntity">
                                <asp:Label ID="lblValue" runat="server" Text='<%# Eval("Value") %>' Style="cursor: default"></asp:Label>
                            </td>
                        </tr>
                    </ItemTemplate>
                    <AlternatingItemTemplate>
                        <tr id="trIT" style="background-color: #F5F5F5; color: #000000;" onmouseover="setItemMouseOverColor(this);"
                            onmouseout="setAlternatingItemMouseOutColor(this);">
                            <td id="tdEvent">
                                <asp:Label ID="lblKey" runat="server" Text='<%# Eval("Key") %>' Style="cursor: default"></asp:Label>
                            </td>
                            <td id="tdEntity">
                                <asp:Label ID="lblValue" runat="server" Text='<%# Eval("Value") %>' Style="cursor: default"></asp:Label>
                            </td>
                        </tr>
                    </AlternatingItemTemplate>
                </asp:ListView>
            </td>
        </tr>
        <tr>
            <th colspan="2" align="center" style="background-color:#00389C;color:White">
                ZDW Configuration Details
            </th>
        </tr>
        <tr>
            <td>
                <asp:ListView ID="lvConfigurationsZDW" runat="server">
                    <LayoutTemplate>
                        <table runat="server" id="tblAuditHistory" class="panelContents" cellpadding="0"
                            cellspacing="0">
                            <tr id="trHeader" runat="server" class="panelSearchHeader">
                                <th id="Th1" runat="server" width="30%">
                                    Key
                                </th>
                                <th id="Th2" runat="server" width="70%">
                                    Value
                                </th>
                            </tr>
                            <tr id="itemPlaceholder" runat="server">
                            </tr>
                        </table>
                    </LayoutTemplate>
                    <EmptyDataTemplate>
                        <table id="Table2" runat="server" style="background-color: #FFFFFF; border-collapse: collapse;
                            border-color: #999999; border-style: none; border-width: 1px;">
                            <tr>
                                <td colspan="2">
                                    No data was returned.
                                </td>
                            </tr>
                        </table>
                    </EmptyDataTemplate>
                    <ItemTemplate>
                        <tr id="trIT" style="background-color: #FFFFFF; color: #000000;" onmouseover="setItemMouseOverColor(this);"
                            onmouseout="setMainItemMouseOutColor(this);">
                            <td id="tdEvent">
                                <asp:Label ID="lblKey" runat="server" Text='<%# Eval("Key") %>' Style="cursor: default"></asp:Label>
                            </td>
                            <td id="tdEntity">
                                <asp:Label ID="lblValue" runat="server" Text='<%# Eval("Value") %>' Style="cursor: default"></asp:Label>
                            </td>
                        </tr>
                    </ItemTemplate>
                    <AlternatingItemTemplate>
                        <tr id="trIT" style="background-color: #F5F5F5; color: #000000;" onmouseover="setItemMouseOverColor(this);"
                            onmouseout="setAlternatingItemMouseOutColor(this);">
                            <td id="tdEvent">
                                <asp:Label ID="lblKey" runat="server" Text='<%# Eval("Key") %>' Style="cursor: default"></asp:Label>
                            </td>
                            <td id="tdEntity">
                                <asp:Label ID="lblValue" runat="server" Text='<%# Eval("Value") %>' Style="cursor: default"></asp:Label>
                            </td>
                        </tr>
                    </AlternatingItemTemplate>
                </asp:ListView>
            </td>
        </tr>
    </table>
</asp:Content>

