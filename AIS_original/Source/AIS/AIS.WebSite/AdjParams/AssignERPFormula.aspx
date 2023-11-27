<%@ Page Language="C#" MasterPageFile="~/Retro.master" AutoEventWireup="true" CodeBehind="AssignERPFormula.aspx.cs"
    Inherits="AIS.WebSite.AdjParams.AssignERPFormula" Title="Assign ERP Formula" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="AjaxToolKit" %>
<%@ Register Src="../App_Shared/AccountInfoHeader.ascx" TagName="AccountInfoHeader"
    TagPrefix="AI" %>
<%@ Register Src="../App_Shared/ProgramPeriod.ascx" TagName="ProgramPeriod" TagPrefix="PP" %>
<%@ Register Src="../App_Shared/SaveCancel.ascx" TagName="SaveCancel" TagPrefix="SC" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeaderPlaceHolder" runat="server">
    <table width="910" style="text-align: right">
        <tr>
            <td align="left">
                <asp:Label ID="lblAsnERPFormula" runat="server" Text="Assign ERP Formula" CssClass="h1"></asp:Label>
            </td>
        </tr>
    </table>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainPlaceHolder" runat="server">

    <script language="javascript" type="text/javascript">

    var scrollTop1;
    
    if(Sys.WebForms.PageRequestManager)
    {
        Sys.WebForms.PageRequestManager.getInstance().add_beginRequest(BeginRequestHandler);
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);
    }
    
    function BeginRequestHandler(sender, args) 
    {
        var mef = $get('<%=pnlAssignERPFormula.ClientID%>');
        if(mef!=null)
        scrollTop1 = mef.scrollTop;
    }

    function EndRequestHandler(sender, args)
    {
        var mef = $get('<%=pnlAssignERPFormula.ClientID%>');
        if(mef!=null)
        mef.scrollTop = scrollTop1;
    }
    function uncheckOthers(id, index, varhidindx) {
        var elm = document.getElementsByTagName('input');
        for (var i = 0; i < elm.length; i++) {
            if (elm.item(i).type == "checkbox" && elm.item(i) != id)
                elm.item(i).checked = false;
        }
        document.getElementById('ctl00_hdnControlDirty').value = '1';
        document.getElementById(varhidindx).value = index;
    }       
     function getrowno(index, varhidindx)
      {
        document.getElementById('ctl00_hdnControlDirty').value='1';
        document.getElementById(varhidindx).value=index;
        
      }
    </script>

    <script type="text/javascript">
        
      function Tabnavigation(Pagename)
        {
            var progPeridID= $get('<%=hidProgPerdID.ClientID%>');
            var Flag=$get('ctl00_MainPlaceHolder_UcMastervalues_CollapseAccountHeader_ClientState');
            var strURL="../AdjParams/";
    
            var flag = document.getElementById('ctl00_hdnControlDirty').value;
            var proceed = true; 

            if(flag=='1')
            if(confirm('You are trying to navigate out of this tab without saving.\n'
                        +'Do you want to proceed without saving?\n\n'
                        +'Press OK to continue, or Cancel to stay on the current tab.'))
            {
               proceed = true;
               document.getElementById('ctl00_hdnControlDirty').value = '0';
            }
            else
               {
                proceed = false;
                var ctrl = $find('<%=TabContainer1.ClientID%>');
                ctrl.set_activeTab(ctrl.get_tabs()[1]);
                __doPostBack('<%=TabContainer1.ClientID %>', ctrl.get_activeTab().get_headerText());
               }
            if(proceed)
            {
                if(Pagename=="CE")
                {
                    strURL +="CombinedElements.aspx";
                }
                else if(Pagename=="AI")
                {
                    strURL +="AuditInfo.aspx";
                }
                else if(Pagename=="RI")
                {
                    strURL +="RetroInfo.aspx";
                }

                if(progPeridID.value >0)
                {
                    strURL += "?ProgPerdID=" + progPeridID.value + "&Flag=" + Flag.value + "&wID=<%= WindowName%>";
                }
                else
                {
                    strURL += "?Flag=" + Flag.value + "&wID=<%= WindowName%>";
                }
                window.location.href=strURL;
            }
        }  
        
    </script>

    <table>
        <tr>
            <td>
                <AI:AccountInfoHeader ID="UcMastervalues" runat="server" />
            </td>
        </tr>
        <tr>
        <td>
            <asp:Label ID="lblProgramperiod" runat="server" CssClass="h3" Font-Bold="True" Text="Program Periods">
            </asp:Label>
                <asp:Panel ID="pnlProgramPeriods" runat="server"  ScrollBars="Auto" Width="910px">
                    <PP:ProgramPeriod ID="ucProgramPeriod" runat="server" />
            </asp:Panel>
        </td>
        </tr>
    </table>
    <asp:ValidationSummary ID="vsAssignERPfOrmula" CssClass="ValidationSummary" runat="server" />
    <asp:HiddenField ID="hidProgPerdID" runat="server" Value="0" />
    <AjaxToolKit:TabContainer ID="TabContainer1" runat="server" CssClass="CustomTabs"
        ActiveTabIndex="1">
        <AjaxToolKit:TabPanel runat="server" ID="tblpnlRetroInfo" HeaderText="Retro Information">
            <HeaderTemplate>
                <div onclick="Tabnavigation('RI')">
                    Retro Information</div>
            </HeaderTemplate>
            <ContentTemplate>
            </ContentTemplate>
        </AjaxToolKit:TabPanel>
        <AjaxToolKit:TabPanel runat="server" ID="tblpnlAssignERP" HeaderText="Assign ERP Formula">
            <HeaderTemplate>
                Assign ERP Formula
            </HeaderTemplate>
            <ContentTemplate>
            </br>
                </br>
                <asp:Label ID="lblAssignERPFormula" runat="server" CssClass="h3" Font-Bold="True"
                                Text="Assign ERP Formula" Visible="False"></asp:Label>
                            <asp:LinkButton ID="lnkClose" runat="server" Visible="false" Text="Close" OnClick="lnkClose_Click"></asp:LinkButton>
              
                <asp:Panel ID="pnlAssignERPFormula" runat="server" Height="215px" ScrollBars="Auto"
                    Width="910px">
                    <asp:AISListView ID="lstErpFormula" runat="server" OnItemDataBound="lstErpFormula_ItemDataBound" >
                        <EmptyDataTemplate>
                            <table id="Table1" class="panelContents" runat="server" width="98%">
                                <tr class="LayoutTemplate">
                                    <th>
                                        ERP Formula
                                    </th>
                                    <th>
                                        Description
                                    </th>
                                    <th>
                                        Select
                                    </th>
                                </tr>
                            </table>
                            <table width="98%">
                                <tr id="Tr1" runat="server" class="ItemTemplate">
                                    <td align="center">
                                        <asp:Label ID="lblEmptyMessage" Text="---No Records Found ---" Font-Bold="true" Width="600px"
                                            runat="server" Style="text-align: center" />
                                    </td>
                                </tr>
                            </table>
                        </EmptyDataTemplate>
                        <LayoutTemplate>
                            <table id="Table1" runat="server" class="panelContents" width="98%">
                                <tr class="LayoutTemplate">
                                    <th>
                                        ERP Formula
                                    </th>
                                    <th>
                                        Description
                                    </th>
                                    <th>
                                        Select
                                    </th>
                                </tr>
                                <tr id="itemPlaceholder" runat="server">
                                </tr>
                            </table>
                        </LayoutTemplate>
                        <ItemTemplate>
                            <tr id="Tr1" runat="server" class="ItemTemplate">
                                <td align="left">
                                    <asp:Label ID="lblhidFormulaID" runat="server" Text='<% # Bind("FormulaID")%>' Visible="false"></asp:Label>
                                <asp:Label ID="lblFormulaText" runat="server" Text='<%# Eval("FormulaOneText").ToString()%>'></asp:Label>
                                </td>
                                <td align="left">
                                <asp:Label ID="Label1" runat="server" Text='<%# Eval("FormulaDescription")%>'></asp:Label>
                                </td>
                                <td>
                                        <asp:CheckBox ID="cbSelect" runat="server" />
                                      
                                </td>
                            </tr>
                        </ItemTemplate>
                        <AlternatingItemTemplate>
                            <tr id="Tr2" runat="server" class="AlternatingItemTemplate">
                                <td align="left">
                                    <asp:Label ID="lblhidFormulaID" runat="server" Text='<% # Bind("FormulaID")%>' Visible="false"></asp:Label>
                                <asp:Label ID="lblFormulaText" runat="server" Text='<%# Eval("FormulaOneText").ToString()%>'></asp:Label>
                                </td>
                                <td align="left">
                                <asp:Label ID="Label1" runat="server" Text='<%# Eval("FormulaDescription")%>'></asp:Label>
                                </td>
                                <td>
                                        <asp:CheckBox ID="cbSelect" runat="server" />
                                </td>
                            </tr>
                        </AlternatingItemTemplate>
                    </asp:AISListView>
                </asp:Panel>
                <asp:HiddenField ID="hidindex" runat="server" />
                <table width="910px" style ="text-align:center">
                <tr>
                   <td align="center">
                    <SC:SaveCancel ID="ucSaveCancel" runat="server" />
                    </td>
                </tr>
                </table>
                <%--<br>
                    <br></br>
                </br>--%>
            </ContentTemplate>
        </AjaxToolKit:TabPanel>
        <AjaxToolKit:TabPanel runat="server" ID="tblpnlCE" HeaderText="Combined Elements">
            <HeaderTemplate>
                <div onclick="Tabnavigation('CE')">
                    Combined Elements</div>
            </HeaderTemplate>
            <ContentTemplate>
            </ContentTemplate>
        </AjaxToolKit:TabPanel>
        <AjaxToolKit:TabPanel runat="server" ID="tblpnlAI" HeaderText="Audit Information">
            <HeaderTemplate>
                <div onclick="Tabnavigation('AI')">
                    Audit Information</div>
            </HeaderTemplate>
            <ContentTemplate>
            </ContentTemplate>
        </AjaxToolKit:TabPanel>
    </AjaxToolKit:TabContainer>
</asp:Content>
