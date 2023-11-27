<%@ Control Language="C#" AutoEventWireup="true" Inherits="App_Shared_ProgramPeriod"
    CodeBehind="ProgramPeriod.ascx.cs" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<script src="../JavaScript/RetroScript.js" type="text/javascript">

 var scrollTop1;
    
    Sys.WebForms.PageRequestManager.getInstance().add_beginRequest(BeginRequestHandler);
    Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);

    function BeginRequestHandler(sender, args) 
    {
        var mef = $get('<%=divProgramperiod.ClientID%>');
        if(mef!=null)
        scrollTop1 = mef.scrollTop;
    }

    function EndRequestHandler(sender, args)
    {
        var mef = $get('<%=divProgramperiod.ClientID%>');
        if(mef!=null)
        mef.scrollTop = scrollTop1;
    }      
    

</script>
<asp:UpdatePanel ID="upProgramPeriod" runat="server" EnableViewState="true">
    <ContentTemplate>
        <div id="divProgramperiod" runat="server" style="overflow: auto; height: 75px; width: 910px;">
            <asp:AISListView ID="lstProgramPeriod" runat="server" DataKeyNames="PREM_ADJ_PGM_ID"
                OnItemCommand="lstProgramPeriod_ItemCommand" OnSelectedIndexChanged="lstProgramPeriod_SelectedIndexChanged"
                OnItemDataBound="lstProgramPeriod_ItemDataBound" OnSelectedIndexChanging="lstProgramPeriod_SelectedIndexChanging">
                <EmptyDataTemplate>
                    <table id="tblEmpty" class="panelContents" runat="server" width="100%">
                        <tr>
                            <th align="center">
                                Details
                            </th>
                            <th align="center">
                                Effective Period
                            </th>
                            <th align="center">
                                Expiration Period
                            </th>
                            <th align="center">
                                Program Type
                            </th>
                            <th align="center">
                                Broker
                            </th>
                            <th align="center">
                                BU/Office
                            </th>
                            <th align="center">
                                Valuation Date
                            </th>
                        </tr>
                        <tr id="T3" runat="server" class="ItemTemplate">
                            <td align="center" colspan="7">
                                <asp:Label ID="lblEmptyMessage" Text="--- No Records Found ---" Font-Bold="true"
                                    runat="server" Style="text-align: center" />
                            </td>
                        </tr>
                    </table>
                </EmptyDataTemplate>
                <LayoutTemplate>
                    <table id="tblProgramPeriod" class="panelContents" runat="server" width="98%">
                        <thead>
                        <tr>
                            <th align="center">
                                Details
                            </th>
                            <th align="center">
                                Effective Period
                            </th>
                            <th align="center">
                                Expiration Period
                            </th>
                            <th align="center">
                                Program Type
                            </th>
                            <th align="center">
                                Broker
                            </th>
                            <th align="center">
                                BU/Office
                            </th>
                            <th align="center">
                                Valuation Date
                            </th>
                        </tr>
                        </thead>
                        <tr id="itemPlaceholder" runat="server">
                        </tr>
                    </table>
                   </LayoutTemplate>
                <ItemTemplate>
                    <tr id="trPrgPeriod" runat="server" class="ItemTemplate">
                        <td id="tdDetails" align="left" style="vertical-align: middle" runat="server">
                            <asp:LinkButton ID="lbSelect" CommandArgument='<%# Bind("PREM_ADJ_PGM_ID") %>' CommandName="Select"
                                runat="server" Text="Details"></asp:LinkButton>
                        </td>
                        <td align="left" style="vertical-align: middle" runat="server" id="tdstrtDate">
                            <asp:Label ID="lblstartDate" runat="server" Text='<%# Bind("STRT_DT") %>'></asp:Label>
                        </td>
                        <td align="left" runat="server" style="vertical-align: middle" id="tdplnendDate">
                            <asp:Label ID="lblendDate" runat="server" Text='<%# Bind("PLAN_END_DT") %>'></asp:Label>
                        </td>
                        <td align="left" runat="server" style="vertical-align: middle" id="tdprgTypid">
                            <asp:Label ID="lblprgTypID" runat="server" Text='<%# Bind("PROGRAMTYPENAME") %>'></asp:Label>
                        </td>
                        <td align="left" runat="server" style="vertical-align: middle" id="td1">
                            <asp:Label ID="lblbrkr" runat="server" Text='<%# Bind("BROKERNAME") %>'></asp:Label>
                        </td>
                        <td align="left" runat="server" style="vertical-align: middle" id="tdbsn_unt">
                            <asp:Label ID="lblbsn_unt" runat="server" Text='<%# Bind("BUSINESSUNITNAME") %>'></asp:Label>
                        </td>
                        <td runat="server" id="tdvalnDate" style="vertical-align: middle">
                            <asp:Label ID="lblvalnmmDate" runat="server" Text='<%# Bind("VALN_MM_DT") %>'></asp:Label>
                        </td>
                    </tr>
                    </tr>
                </ItemTemplate>
                <AlternatingItemTemplate>
                    <tr id="trPrgPeriod" runat="server" class="AlternatingItemTemplate">
                        <td id="tdDetails" align="left" style="vertical-align: middle" runat="server">
                            <asp:LinkButton ID="lbSelect" CommandArgument='<%# Bind("PREM_ADJ_PGM_ID") %>' CommandName="Select"
                                runat="server" Text="Details"></asp:LinkButton>
                        </td>
                        <td align="left" style="vertical-align: middle" runat="server" id="tdstrtDate">
                            <asp:Label ID="lblstartDate" runat="server" Text='<%# Bind("STRT_DT") %>'></asp:Label>
                        </td>
                        <td align="left" runat="server" style="vertical-align: middle" id="tdplnendDate">
                            <asp:Label ID="lblendDate" runat="server" Text='<%# Bind("PLAN_END_DT") %>'></asp:Label>
                        </td>
                        <td align="left" runat="server" style="vertical-align: middle" id="tdprgTypid">
                            <asp:Label ID="lblprgTypID" runat="server" Text='<%# Bind("PROGRAMTYPENAME") %>'></asp:Label>
                        </td>
                        <td align="left" runat="server" id="td1">
                            <asp:Label ID="lblbrkr" runat="server" Text='<%# Bind("BROKERNAME") %>'></asp:Label>
                        </td>
                        <td align="left" runat="server" id="tdbsn_unt">
                            <asp:Label ID="lblbsn_unt" runat="server" Text='<%# Bind("BUSINESSUNITNAME") %>'></asp:Label>
                        </td>
                        <td runat="server" style="vertical-align: middle" id="tdvalnDate">
                            <asp:Label ID="lblvalnmmDate" runat="server" Text='<%# Bind("VALN_MM_DT") %>'></asp:Label>
                        </td>
                    </tr>
                </AlternatingItemTemplate>
            </asp:AISListView>
        </div>

    </ContentTemplate>
</asp:UpdatePanel>
