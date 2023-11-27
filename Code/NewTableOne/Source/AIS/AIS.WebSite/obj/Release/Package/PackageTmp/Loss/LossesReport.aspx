<%@ Page Language="C#" MasterPageFile="~/Retro.master" AutoEventWireup="true" CodeBehind="LossesReport.aspx.cs"
    Inherits="LossesReport" %>

<%@ Register Src="../App_Shared/AccountInfoHeader.ascx" TagName="AccountInfoHeader"
    TagPrefix="uc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeaderPlaceHolder" runat="server">
    <table width="910" style="text-align: right">
        <tr>
            <td align="left">
                <asp:Label ID="lblLossByHeader" runat="server" CssClass="h1"></asp:Label>
            </td>
            <td align="right">
                <asp:Button ID="btnAdd" runat="server" Text="Back" OnClick="btnBack_Click" ToolTip="Please click here to go previous page." />
            </td>
        </tr>
    </table>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainPlaceHolder" runat="server">
    <table>
        <tr>
            <td >
                <uc1:AccountInfoHeader ID="AccountInfoHeader1" runat="server" />
                <br />
            </td>
        </tr>
    </table>
    <br />
    <div style="padding-left: 10px">
        <asp:Label ID="lblPolicyEffectDate" runat="server" CssClass="h3"></asp:Label>
    </div>
    <asp:Panel ID="pnlLossByPolicyList" Width="930px" runat="server" ScrollBars="Auto"
        Height="350px">
        <asp:AISListView ID="lstMasterLossesByPolicy" runat="server">
            <LayoutTemplate>
                <asp:PlaceHolder ID="itemPlaceholder" runat="server"></asp:PlaceHolder>
            </LayoutTemplate>
            <EmptyDataTemplate>
                <table id="lstLossesByPolicyTable" runat="server" width="900px">
                    <tr class="LayoutTemplate">
                        <th class="LossReport">
                            Policy Number
                        </th>
                        <th class="LossReport">
                            SUB/COV
                        </th>
                        <th class="LossReport">
                            Limit
                        </th>
                        <th class="LossReport">
                            ALAE Handling
                        </th>
                        <th class="LossReport">
                            IND
                        </th>
                        <th >
                            Paid Indemnity
                        </th>
                        <th class="LossReport">
                            Paid Expense
                        </th>
                        <th class="LossReport">
                            Reserved Indemnity
                        </th>
                        <th class="LossReport">
                            Reserved Expense
                        </th>
                        <th class="LossReport">
                            Incurred
                        </th>
                    </tr>
                </table>
                <table width="900px">
                    <tr id="Tr1" runat="server" class="ItemTemplate">
                        <td align="center">
                            <asp:Label ID="lblEmptyMessage" Text="---No Records Found ---" Font-Bold="true" Width="600px"
                                runat="server" Style="text-align: center" />
                        </td>
                    </tr>
                </table>
            </EmptyDataTemplate>
            <ItemTemplate>
                <table width="900px">
                    <tr id="trItemTemplate" runat="server">
                        <td>
                            <asp:Label ID="lblID" runat="server" Visible="false" Text='<%# Eval("COML_AGMT_ID")%>'></asp:Label>
                        </td>
                        <td width="900px">
                            <asp:AISListView ID="lstChildLossesByPoicy" runat="server">
                                <LayoutTemplate>
                                    <table id="lstLossesByPolicyTable" runat="server" width="900px">
                                        <tr class="LayoutTemplate">
                                            <th class="LossReport">
                                                Policy Number
                                            </th>
                                            <th class="LossReport">
                                                SUB/COV
                                            </th>
                                            <th class="LossReport">
                                                Limit
                                            </th>
                                            <th class="LossReport">
                                                ALAE Handling
                                            </th>
                                            <th class="LossReport">
                                                IND
                                            </th>
                                            <th class="LossReport">
                                                Paid Indemnity
                                            </th>
                                            <th class="LossReport">
                                                Paid Expense
                                            </th>
                                            <th class="LossReport">
                                                Reserved Indemnity
                                            </th>
                                            <th class="LossReport">
                                                Reserved Expense
                                            </th>
                                            <th class="LossReport">
                                                Incurred
                                            </th>
                                        </tr>
                                        <tr id="ItemPlaceHolder" runat="server">
                                        </tr>
                                    </table>
                                </LayoutTemplate>
                                <ItemTemplate>
                                    <tr id="trItemTemplate" runat="server" class="ItemTemplate">
                                        <td>
                                            <%# Eval("POLICY")%>
                                        </td>
                                        <td>
                                            <%# Eval("POLICYSYMBOL")%>
                                        </td>
                                        <td>
                                        
                                            
                                             $<%# (Eval("LIMIT2_AMT") == null || Eval("LIMIT2_AMT").ToString() == "0.00") ? (Eval("POLICY_AMT") != null ? (Eval("POLICY_AMT").ToString() != "" ? (decimal.Parse(Eval("POLICY_AMT").ToString())).ToString("#,##0") : "0") : "0") : (decimal.Parse(Eval("LIMIT2_AMT").ToString())).ToString("#,##0")%>
                                            
                                        </td>
                                        <td>
                                            <%# Eval("ALAE_TYP")%>
                                        </td>
                                        <td>
                                            <b>Total</b>
                                        </td>
                                        <td nowrap>
                                            $<%#  Eval("PAID_IDNMTY_AMT") != null ? (Eval("PAID_IDNMTY_AMT").ToString() != "" ? (decimal.Parse(Eval("PAID_IDNMTY_AMT").ToString())).ToString("#,##0") : "0") : "0"%>
                                        </td>
                                        <td nowrap>
                                            $<%# Eval("PAID_EXPS_AMT") != null ? (Eval("PAID_EXPS_AMT").ToString() != "" ? (decimal.Parse(Eval("PAID_EXPS_AMT").ToString())).ToString("#,##0") : "0") : "0"%>
                                        </td>
                                        <td nowrap>
                                            $<%# Eval("RESRV_IDNMTY_AMT") != null ? (Eval("RESRV_IDNMTY_AMT").ToString() != "" ? (decimal.Parse(Eval("RESRV_IDNMTY_AMT").ToString())).ToString("#,##0") : "0") : "0"%>
                                        </td>
                                        <td nowrap>
                                            $<%# Eval("RESRV_EXPS_AMT") != null ? (Eval("RESRV_EXPS_AMT").ToString() != "" ? (decimal.Parse(Eval("RESRV_EXPS_AMT").ToString())).ToString("#,##0") : "0") : "0"%>
                                        </td>
                                        <td nowrap>
                                            $<%# Eval("TOTAL_INCURRED") != null ? (Eval("TOTAL_INCURRED").ToString() != "" ? (decimal.Parse(Eval("TOTAL_INCURRED").ToString())).ToString("#,##0") : "0") : "0"%>
                                        </td>
                                    </tr>
                                    <tr id="trAlternatingItemTemplate" runat="server" class="AlternatingItemTemplate">
                                        <td>
                                        </td>
                                        <td>
                                        </td>
                                        <td>
                                        </td>
                                        <td>
                                        </td>
                                        <td>
                                            <b>NonBillable</b>
                                        </td>
                                        <td nowrap>
                                            $<%#  Eval("NON_BILABL_PAID_IDNMTY_AMT") != null ? (Eval("NON_BILABL_PAID_IDNMTY_AMT").ToString() != "" ? (decimal.Parse(Eval("NON_BILABL_PAID_IDNMTY_AMT").ToString())).ToString("#,##0") : "0") : "0"%>
                                        </td>
                                        <td nowrap>
                                            $<%#  Eval("NON_BILABL_PAID_EXPS_AMT") != null ? (Eval("NON_BILABL_PAID_EXPS_AMT").ToString() != "" ? (decimal.Parse(Eval("NON_BILABL_PAID_EXPS_AMT").ToString())).ToString("#,##0") : "0") : "0"%>
                                        </td>
                                        <td nowrap>
                                            $<%#  Eval("NON_BILABL_RESRV_IDNMTY_AMT") != null ? (Eval("NON_BILABL_RESRV_IDNMTY_AMT").ToString() != "" ? (decimal.Parse(Eval("NON_BILABL_RESRV_IDNMTY_AMT").ToString())).ToString("#,##0") : "0") : "0"%>
                                        </td>
                                        <td nowrap>
                                            $<%#  Eval("NON_BILABL_RESRV_EXPS_AMT") != null ? (Eval("NON_BILABL_RESRV_EXPS_AMT").ToString() != "" ? (decimal.Parse(Eval("NON_BILABL_RESRV_EXPS_AMT").ToString())).ToString("#,##0") : "0") : "0"%>
                                        </td>
                                        <td nowrap>
                                            $<%#  Eval("NON_BILABL_INCURRED") != null ? (Eval("NON_BILABL_INCURRED").ToString() != "" ? (decimal.Parse(Eval("NON_BILABL_INCURRED").ToString())).ToString("#,##0") : "0") : "0"%>
                                        </td>
                                    </tr>
                                    <tr id="trItemTemplate1" runat="server" class="ItemTemplate">
                                        <td>
                                        </td>
                                        <td>
                                        </td>
                                        <td>
                                        </td>
                                        <td>
                                        </td>
                                        <td>
                                            <b>Subject</b>
                                        </td>
                                        <td nowrap>
                                            $<%#  Eval("SUBJ_PAID_IDNMTY_AMT") != null ? (Eval("SUBJ_PAID_IDNMTY_AMT").ToString() != "" ? (decimal.Parse(Eval("SUBJ_PAID_IDNMTY_AMT").ToString())).ToString("#,##0") : "0") : "0"%>
                                        </td>
                                        <td nowrap>
                                            $<%#  Eval("SUBJ_PAID_EXPS_AMT") != null ? (Eval("SUBJ_PAID_EXPS_AMT").ToString() != "" ? (decimal.Parse(Eval("SUBJ_PAID_EXPS_AMT").ToString())).ToString("#,##0") : "0") : "0"%>
                                        </td>
                                        <td nowrap>
                                            $<%#  Eval("SUBJ_RESRV_IDNMTY_AMT") != null ? (Eval("SUBJ_RESRV_IDNMTY_AMT").ToString() != "" ? (decimal.Parse(Eval("SUBJ_RESRV_IDNMTY_AMT").ToString())).ToString("#,##0") : "0") : "0"%>
                                        </td>
                                        <td nowrap>
                                            $<%#  Eval("SUBJ_RESRV_EXPS_AMT") != null ? (Eval("SUBJ_RESRV_EXPS_AMT").ToString() != "" ? (decimal.Parse(Eval("SUBJ_RESRV_EXPS_AMT").ToString())).ToString("#,##0") : "0") : "0"%>
                                        </td>
                                        <td nowrap>
                                            $<%#  Eval("SUBJ_INCURRED") != null ? (Eval("SUBJ_INCURRED").ToString() != "" ? (decimal.Parse(Eval("SUBJ_INCURRED").ToString())).ToString("#,##0") : "0") : "0"%>
                                        </td>
                                    </tr>
                                    <tr id="trAlternateItemTemplate1" runat="server" class="AlternatingItemTemplate">
                                        <td>
                                        </td>
                                        <td>
                                        </td>
                                        <td>
                                        </td>
                                        <td>
                                        </td>
                                        <td>
                                            <b>Excess</b>
                                        </td>
                                        <td nowrap>
                                            $<%#  Eval("EXC_PAID_IDNMTY_AMT") != null ? (Eval("EXC_PAID_IDNMTY_AMT").ToString() != "" ? (decimal.Parse(Eval("EXC_PAID_IDNMTY_AMT").ToString())).ToString("#,##0") : "0") : "0"%>
                                        </td>
                                        <td nowrap>
                                            $<%#  Eval("EXC_PAID_EXPS_AMT") != null ? (Eval("EXC_PAID_EXPS_AMT").ToString() != "" ? (decimal.Parse(Eval("EXC_PAID_EXPS_AMT").ToString())).ToString("#,##0") : "0") : "0"%>
                                        </td>
                                        <td nowrap>
                                            $<%#  Eval("EXC_RESRV_IDNMTY_AMT") != null ? (Eval("EXC_RESRV_IDNMTY_AMT").ToString() != "" ? (decimal.Parse(Eval("EXC_RESRV_IDNMTY_AMT").ToString())).ToString("#,##0") : "0") : "0"%>
                                        </td>
                                        <td nowrap>
                                            $<%# Eval("EXC_RESRV_EXPS_AMT") != null ? (Eval("EXC_RESRV_EXPS_AMT").ToString() != "" ? (decimal.Parse(Eval("EXC_RESRV_EXPS_AMT").ToString())).ToString("#,##0") : "0") : "0"%>
                                        </td>
                                        <td nowrap>
                                            $<%# Eval("EXC_INCURRED") != null ? (Eval("EXC_INCURRED").ToString() != "" ? (decimal.Parse(Eval("EXC_INCURRED").ToString())).ToString("#,##0") : "0") : "0"%>
                                        </td>
                                    </tr>
                                    <tr id="trItemtemplate2" runat="server" class="ItemTemplate">
                                        <td>
                                        </td>
                                        <td>
                                        </td>
                                        <td>
                                        </td>
                                        <td>
                                        </td>
                                        <td>
                                        </td>
                                        <td>
                                        </td>
                                        <td>
                                        </td>
                                        <td>
                                        </td>
                                        <td>
                                        </td>
                                        <td>
                                        </td>
                                    </tr>
                                </ItemTemplate>
                            </asp:AISListView>
                        </td>
                    </tr>
                </table>
            </ItemTemplate>
        </asp:AISListView>
    </asp:Panel>
    <asp:Panel ID="pnlLossByLOB" Width="930px" runat="server" CssClass="content" ScrollBars="Auto"
        Height="350px">
        <asp:AISListView ID="lstMasterLossByLOB" runat="server">
            <LayoutTemplate>
                <asp:PlaceHolder ID="itemPlaceholder" runat="server"></asp:PlaceHolder>
            </LayoutTemplate>
            <EmptyDataTemplate>
                <table id="lstLossesByPolicyTable" runat="server" width="900px">
                    <tr class="LayoutTemplate">
                        <th class="LossReport">
                            SUB/COV
                        </th>
                        <th class="LossReport">
                            IND
                        </th>
                        <th class="LossReport">
                            Paid Indemnity
                        </th>
                        <th class="LossReport">
                            Paid Expense
                        </th>
                        <th class="LossReport">
                            Reserved Indemnity
                        </th>
                        <th class="LossReport">
                            Reserved Expense
                        </th>
                        <th class="LossReport">
                            Incurred
                        </th>
                    </tr>
                </table>
                <table width="900px">
                    <tr id="Tr1" runat="server" class="ItemTemplate">
                        <td align="center">
                            <asp:Label ID="lblEmptyMessage" Text="---No Records Found ---" Font-Bold="true" Width="600px"
                                runat="server" Style="text-align: center" />
                        </td>
                    </tr>
                </table>
            </EmptyDataTemplate>
            <ItemTemplate>
                <table width="900px">
                    <tr id="trItemTemplate" runat="server">
                        <td>
                            <asp:Label ID="lblID" runat="server" Visible="false" Text='<%# Eval("POLICYSYMBOL")%>'></asp:Label>
                        </td>
                        <td width="910px">
                            <asp:AISListView ID="lstChildLossesByLOB" runat="server">
                                <LayoutTemplate>
                                    <table id="lstLossesByPolicyTable" runat="server" width="910px">
                                        <tr class="LayoutTemplate">
                                            <th class="LossReport">
                                                SUB/COV
                                            </th>
                                            <th class="LossReport">
                                                IND
                                            </th>
                                            <th class="LossReport">
                                                Paid Indemnity
                                            </th>
                                            <th class="LossReport">
                                                Paid Expense
                                            </th>
                                            <th class="LossReport">
                                                Reserved Indemnity
                                            </th>
                                            <th class="LossReport">
                                                Reserved Expense
                                            </th>
                                            <th class="LossReport">
                                                Incurred
                                            </th>
                                        </tr>
                                        <tr id="ItemPlaceHolder" runat="server">
                                        </tr>
                                    </table>
                                </LayoutTemplate>
                                <ItemTemplate>
                                    <tr id="trItemTemplate" runat="server" class="ItemTemplate">
                                        <td>
                                            <%# Eval("POLICYSYMBOL")%>
                                        </td>
                                        <td>
                                            <b>Total</b>
                                        </td>
                                        <td>
                                            $<%# Eval("PAID_IDNMTY_AMT") != null ? (Eval("PAID_IDNMTY_AMT").ToString() != "" ? (decimal.Parse(Eval("PAID_IDNMTY_AMT").ToString())).ToString("#,##0") : "0") : "0"%>
                                        </td>
                                        <td>
                                            $<%# Eval("PAID_EXPS_AMT") != null ? (Eval("PAID_EXPS_AMT").ToString() != "" ? (decimal.Parse(Eval("PAID_EXPS_AMT").ToString())).ToString("#,##0") : "0") : "0"%>
                                        </td>
                                        <td>
                                            $<%# Eval("RESRV_IDNMTY_AMT") != null ? (Eval("RESRV_IDNMTY_AMT").ToString() != "" ? (decimal.Parse(Eval("RESRV_IDNMTY_AMT").ToString())).ToString("#,##0") : "0") : "0"%>
                                        </td>
                                        <td>
                                            $<%# Eval("RESRV_EXPS_AMT") != null ? (Eval("RESRV_EXPS_AMT").ToString() != "" ? (decimal.Parse(Eval("RESRV_EXPS_AMT").ToString())).ToString("#,##0") : "0") : "0"%>
                                        </td>
                                        <td>
                                            $<%# Eval("TOTAL_INCURRED") != null ? (Eval("TOTAL_INCURRED").ToString() != "" ? (decimal.Parse(Eval("TOTAL_INCURRED").ToString())).ToString("#,##0") : "0") : "0"%>
                                        </td>
                                    </tr>
                                    <tr id="trAlternatingItemTemplate" runat="server" class="AlternatingItemTemplate">
                                        <td>
                                        </td>
                                        <td>
                                            <b>NonBillable</b>
                                        </td>
                                        <td>
                                            $<%# Eval("NON_BILABL_PAID_IDNMTY_AMT") != null ? (Eval("NON_BILABL_PAID_IDNMTY_AMT").ToString() != "" ? (decimal.Parse(Eval("NON_BILABL_PAID_IDNMTY_AMT").ToString())).ToString("#,##0") : "0") : "0"%>
                                        </td>
                                        <td>
                                            $<%# Eval("NON_BILABL_PAID_EXPS_AMT") != null ? (Eval("NON_BILABL_PAID_EXPS_AMT").ToString() != "" ? (decimal.Parse(Eval("NON_BILABL_PAID_EXPS_AMT").ToString())).ToString("#,##0") : "0") : "0"%>
                                        </td>
                                        <td>
                                            $<%# Eval("NON_BILABL_RESRV_IDNMTY_AMT") != null ? (Eval("NON_BILABL_RESRV_IDNMTY_AMT").ToString() != "" ? (decimal.Parse(Eval("NON_BILABL_RESRV_IDNMTY_AMT").ToString())).ToString("#,##0") : "0") : "0"%>
                                        </td>
                                        <td>
                                            $<%# Eval("NON_BILABL_RESRV_EXPS_AMT") != null ? (Eval("NON_BILABL_RESRV_EXPS_AMT").ToString() != "" ? (decimal.Parse(Eval("NON_BILABL_RESRV_EXPS_AMT").ToString())).ToString("#,##0") : "0") : "0"%>
                                        </td>
                                        <td>
                                            $<%# Eval("NON_BILABL_INCURRED") != null ? (Eval("NON_BILABL_INCURRED").ToString() != "" ? (decimal.Parse(Eval("NON_BILABL_INCURRED").ToString())).ToString("#,##0") : "0") : "0"%>
                                        </td>
                                    </tr>
                                    <tr id="trItemTemplate1" runat="server" class="ItemTemplate">
                                        <td>
                                        </td>
                                        <td>
                                            <b>Subject</b>
                                        </td>
                                        <td>
                                            $<%# Eval("SUBJ_PAID_IDNMTY_AMT") != null ? (Eval("SUBJ_PAID_IDNMTY_AMT").ToString() != "" ? (decimal.Parse(Eval("SUBJ_PAID_IDNMTY_AMT").ToString())).ToString("#,##0") : "0") : "0"%>
                                        </td>
                                        <td>
                                            $<%# Eval("SUBJ_PAID_EXPS_AMT") != null ? (Eval("SUBJ_PAID_EXPS_AMT").ToString() != "" ? (decimal.Parse(Eval("SUBJ_PAID_EXPS_AMT").ToString())).ToString("#,##0") : "0") : "0"%>
                                        </td>
                                        <td>
                                            $<%# Eval("SUBJ_RESRV_IDNMTY_AMT") != null ? (Eval("SUBJ_RESRV_IDNMTY_AMT").ToString() != "" ? (decimal.Parse(Eval("SUBJ_RESRV_IDNMTY_AMT").ToString())).ToString("#,##0") : "0") : "0"%>
                                        </td>
                                        <td>
                                            $<%# Eval("SUBJ_RESRV_EXPS_AMT") != null ? (Eval("SUBJ_RESRV_EXPS_AMT").ToString() != "" ? (decimal.Parse(Eval("SUBJ_RESRV_EXPS_AMT").ToString())).ToString("#,##0") : "0") : "0"%>
                                        </td>
                                        <td>
                                            $<%# Eval("SUBJ_INCURRED") != null ? (Eval("SUBJ_INCURRED").ToString() != "" ? (decimal.Parse(Eval("SUBJ_INCURRED").ToString())).ToString("#,##0") : "0") : "0"%>
                                        </td>
                                    </tr>
                                    <tr id="trAlternateItemTemplate1" runat="server" class="AlternatingItemTemplate">
                                        <td>
                                        </td>
                                        <td>
                                            <b>Excess</b>
                                        </td>
                                        <td>
                                         $<%# Eval("EXC_PAID_IDNMTY_AMT") != null ? (Eval("EXC_PAID_IDNMTY_AMT").ToString() != "" ? (decimal.Parse(Eval("EXC_PAID_IDNMTY_AMT").ToString())).ToString("#,##0") : "0") : "0"%>
                                           
                                        </td>
                                        <td>
                                        $<%# Eval("EXC_PAID_EXPS_AMT") != null ? (Eval("EXC_PAID_EXPS_AMT").ToString() != "" ? (decimal.Parse(Eval("EXC_PAID_EXPS_AMT").ToString())).ToString("#,##0") : "0") : "0"%>
                                            
                                        </td>
                                        <td>
                                         $<%# Eval("EXC_RESRV_IDNMTY_AMT") != null ? (Eval("EXC_RESRV_IDNMTY_AMT").ToString() != "" ? (decimal.Parse(Eval("EXC_RESRV_IDNMTY_AMT").ToString())).ToString("#,##0") : "0") : "0"%>
                                            
                                        </td>
                                        <td>
                                        $<%# Eval("EXC_RESRV_EXPS_AMT") != null ? (Eval("EXC_RESRV_EXPS_AMT").ToString() != "" ? (decimal.Parse(Eval("EXC_RESRV_EXPS_AMT").ToString())).ToString("#,##0") : "0") : "0"%>
                                           
                                        </td>
                                        <td>
                                         $<%# Eval("EXC_INCURRED") != null ? (Eval("EXC_INCURRED").ToString() != "" ? (decimal.Parse(Eval("EXC_INCURRED").ToString())).ToString("#,##0") : "0") : "0"%>
                                           
                                        </td>
                                    </tr>
                                    <tr id="trItemtemplate2" runat="server" class="ItemTemplate">
                                        <td>
                                        </td>
                                        <td>
                                        </td>
                                        <td>
                                        </td>
                                        <td>
                                        </td>
                                        <td>
                                        </td>
                                        <td>
                                        </td>
                                        <td>
                                        </td>
                                    </tr>
                                </ItemTemplate>
                            </asp:AISListView>
                        </td>
                    </tr>
                </table>
            </ItemTemplate>
        </asp:AISListView>
    </asp:Panel>
    <asp:Panel ID="pnlLossByState" Width="930px" runat="server" CssClass="content" ScrollBars="Auto"
        Height="350px">
        <asp:AISListView ID="lstMasterLossByState" runat="server">
            <LayoutTemplate>
                <asp:PlaceHolder ID="itemPlaceholder" runat="server"></asp:PlaceHolder>
            </LayoutTemplate>
            <EmptyDataTemplate>
                <table id="lstLossesByPolicyTable" runat="server" width="910px">
                    <tr class="LayoutTemplate">
                        <th class="LossReport">
                            State
                        </th>
                        <th class="LossReport">
                            Policy Number
                        </th>
                        <th class="LossReport">
                            SUB/COV
                        </th>
                        <th class="LossReport">
                            Limit
                        </th>
                        <th class="LossReport">
                            ALAE Handling
                        </th>
                        <th class="LossReport">
                            IND
                        </th>
                        <th class="LossReport">
                            Paid Indemnity
                        </th>
                        <th class="LossReport">
                            Paid Expense
                        </th>
                        <th class="LossReport">
                            Reserved Indemnity
                        </th>
                        <th class="LossReport">
                            Reserved Expense
                        </th>
                        <th class="LossReport">
                            Incurred
                        </th>
                    </tr>
                </table>
                <table width="910px">
                    <tr id="Tr1" runat="server" class="ItemTemplate">
                        <td align="center">
                            <asp:Label ID="lblEmptyMessage" Text="---No Records Found ---" Font-Bold="true" Width="600px"
                                runat="server" Style="text-align: center" />
                        </td>
                    </tr>
                </table>
            </EmptyDataTemplate>
            <ItemTemplate>
                <table width="900px">
                    <tr id="trItemTemplate" runat="server">
                        <td>
                            <asp:Label ID="lblID" runat="server" Visible="false" Text='<%# Eval("ARMIS_LOS_ID")%>'></asp:Label>
                        </td>
                        <td width="910px">
                            <asp:AISListView ID="lstChildLossesByState" runat="server">
                                <LayoutTemplate>
                                    <table id="lstLossesByPolicyTable" runat="server" width="910px">
                                        <tr class="LayoutTemplate">
                                            <th class="LossReport">
                                                State
                                            </th>
                                            <th class="LossReport">
                                                Policy Number
                                            </th>
                                            <th class="LossReport">
                                                SUB/COV
                                            </th>
                                            <th class="LossReport">
                                                Limit
                                            </th>
                                            <th class="LossReport">
                                                ALAE Handling
                                            </th>
                                            <th class="LossReport">
                                                IND
                                            </th>
                                            <th class="LossReport">
                                                Paid Indemnity
                                            </th>
                                            <th class="LossReport">
                                                Paid Expense
                                            </th>
                                            <th class="LossReport">
                                                Reserved Indemnity
                                            </th>
                                            <th class="LossReport">
                                                Reserved Expense
                                            </th>
                                            <th class="LossReport">
                                                Incurred
                                            </th>
                                        </tr>
                                        <tr id="ItemPlaceHolder" runat="server">
                                        </tr>
                                    </table>
                                </LayoutTemplate>
                                <ItemTemplate>
                                    <tr id="trItemTemplate" runat="server" class="ItemTemplate">
                                        <td>
                                            <%# Eval("STATETYPE")%>
                                        </td>
                                        <td>
                                            <%# Eval("POLICY")%>
                                        </td>
                                        <td>
                                            <%# Eval("POLICYSYMBOL")%>
                                        </td>
                                        <td>
                                          $<%# (Eval("LIMIT2_AMT") == null || Eval("LIMIT2_AMT").ToString() == "0.00") ? (Eval("POLICY_AMT") != null ? (Eval("POLICY_AMT").ToString() != "" ? (decimal.Parse(Eval("POLICY_AMT").ToString())).ToString("#,##0") : "0") : "0") : (decimal.Parse(Eval("LIMIT2_AMT").ToString())).ToString("#,##0")%>
                                            
                                        </td>
                                        <td>
                                            <%# Eval("ALAE_TYP")%>
                                        </td>
                                        <td>
                                            <b>Total</b>
                                        </td>
                                        <td nowrap>
                                         $<%# Eval("PAID_IDNMTY_AMT") != null ? (Eval("PAID_IDNMTY_AMT").ToString() != "" ? (decimal.Parse(Eval("PAID_IDNMTY_AMT").ToString())).ToString("#,##0") : "0") : "0"%>
                                           
                                        </td>
                                        <td nowrap>
                                             $<%# Eval("PAID_EXPS_AMT") != null ? (Eval("PAID_EXPS_AMT").ToString() != "" ? (decimal.Parse(Eval("PAID_EXPS_AMT").ToString())).ToString("#,##0") : "0") : "0"%>
                                            
                                        </td>
                                        <td nowrap>
                                        $<%# Eval("RESRV_IDNMTY_AMT") != null ? (Eval("RESRV_IDNMTY_AMT").ToString() != "" ? (decimal.Parse(Eval("RESRV_IDNMTY_AMT").ToString())).ToString("#,##0") : "0") : "0"%>
                                           
                                        </td>
                                        <td nowrap>
                                         $<%# Eval("RESRV_EXPS_AMT") != null ? (Eval("RESRV_EXPS_AMT").ToString() != "" ? (decimal.Parse(Eval("RESRV_EXPS_AMT").ToString())).ToString("#,##0") : "0") : "0"%>
                                            
                                        </td>
                                        <td nowrap>
                                         $<%# Eval("TOTAL_INCURRED") != null ? (Eval("TOTAL_INCURRED").ToString() != "" ? (decimal.Parse(Eval("TOTAL_INCURRED").ToString())).ToString("#,##0") : "0") : "0"%>
                                            
                                        </td>
                                    </tr>
                                    <tr id="trAlternatingItemTemplate" runat="server" class="AlternatingItemTemplate">
                                        <td>
                                        </td>
                                        <td>
                                        </td>
                                        <td>
                                        </td>
                                        <td>
                                        </td>
                                        <td>
                                        </td>
                                        <td>
                                            <b>NonBillable</b>
                                        </td>
                                        <td nowrap>
                                         $<%# Eval("NON_BILABL_PAID_IDNMTY_AMT") != null ? (Eval("NON_BILABL_PAID_IDNMTY_AMT").ToString() != "" ? (decimal.Parse(Eval("NON_BILABL_PAID_IDNMTY_AMT").ToString())).ToString("#,##0") : "0") : "0"%>
                                            
                                        </td>
                                        <td nowrap>
                                        $<%# Eval("NON_BILABL_PAID_EXPS_AMT") != null ? (Eval("NON_BILABL_PAID_EXPS_AMT").ToString() != "" ? (decimal.Parse(Eval("NON_BILABL_PAID_EXPS_AMT").ToString())).ToString("#,##0") : "0") : "0"%>
                                        
                            
                                        </td>
                                        <td nowrap>
                                        $<%# Eval("NON_BILABL_RESRV_IDNMTY_AMT") != null ? (Eval("NON_BILABL_RESRV_IDNMTY_AMT").ToString() != "" ? (decimal.Parse(Eval("NON_BILABL_RESRV_IDNMTY_AMT").ToString())).ToString("#,##0") : "0") : "0"%>
                                            
                                        </td>
                                        <td nowrap>
                                        $<%# Eval("NON_BILABL_RESRV_EXPS_AMT") != null ? (Eval("NON_BILABL_RESRV_EXPS_AMT").ToString() != "" ? (decimal.Parse(Eval("NON_BILABL_RESRV_EXPS_AMT").ToString())).ToString("#,##0") : "0") : "0"%>
                                            
                                        </td>
                                        <td nowrap>
                                        $<%# Eval("NON_BILABL_INCURRED") != null ? (Eval("NON_BILABL_INCURRED").ToString() != "" ? (decimal.Parse(Eval("NON_BILABL_INCURRED").ToString())).ToString("#,##0") : "0") : "0"%>
                                            
                                        </td>
                                    </tr>
                                    <tr id="trItemTemplate1" runat="server" class="ItemTemplate">
                                        <td>
                                        </td>
                                        <td>
                                        </td>
                                        <td>
                                        </td>
                                        <td>
                                        </td>
                                        <td>
                                        </td>
                                        <td>
                                            <b>Subject</b>
                                        </td>
                                        <td nowrap>
                                         $<%# Eval("SUBJ_PAID_IDNMTY_AMT") != null ? (Eval("SUBJ_PAID_IDNMTY_AMT").ToString() != "" ? (decimal.Parse(Eval("SUBJ_PAID_IDNMTY_AMT").ToString())).ToString("#,##0") : "0") : "0"%>
                                           
                                        </td>
                                        <td nowrap>
                                        $<%# Eval("SUBJ_PAID_EXPS_AMT") != null ? (Eval("SUBJ_PAID_EXPS_AMT").ToString() != "" ? (decimal.Parse(Eval("SUBJ_PAID_EXPS_AMT").ToString())).ToString("#,##0") : "0") : "0"%>
                                           
                                        </td>
                                        <td nowrap>
                                        $<%# Eval("SUBJ_RESRV_IDNMTY_AMT") != null ? (Eval("SUBJ_RESRV_IDNMTY_AMT").ToString() != "" ? (decimal.Parse(Eval("SUBJ_RESRV_IDNMTY_AMT").ToString())).ToString("#,##0") : "0") : "0"%>
                                            
                                        </td>
                                        <td nowrap>
                                         $<%# Eval("SUBJ_RESRV_EXPS_AMT") != null ? (Eval("SUBJ_RESRV_EXPS_AMT").ToString() != "" ? (decimal.Parse(Eval("SUBJ_RESRV_EXPS_AMT").ToString())).ToString("#,##0") : "0") : "0"%>
                                           
                                        </td>
                                        <td nowrap>
                                        $<%# Eval("SUBJ_INCURRED") != null ? (Eval("SUBJ_INCURRED").ToString() != "" ? (decimal.Parse(Eval("SUBJ_INCURRED").ToString())).ToString("#,##0") : "0") : "0"%>
                                           
                                        </td>
                                    </tr>
                                    <tr id="trAlternateItemTemplate1" runat="server" class="AlternatingItemTemplate">
                                        <td>
                                        </td>
                                        <td>
                                        </td>
                                        <td>
                                        </td>
                                        <td>
                                        </td>
                                        <td>
                                        </td>
                                        <td>
                                            <b>Excess</b>
                                        </td>
                                        <td nowrap>
                                         $<%# Eval("EXC_PAID_IDNMTY_AMT") != null ? (Eval("EXC_PAID_IDNMTY_AMT").ToString() != "" ? (decimal.Parse(Eval("EXC_PAID_IDNMTY_AMT").ToString())).ToString("#,##0") : "0") : "0"%>
                                            
                                        </td>
                                        <td nowrap>
                                        $<%# Eval("EXC_PAID_EXPS_AMT") != null ? (Eval("EXC_PAID_EXPS_AMT").ToString() != "" ? (decimal.Parse(Eval("EXC_PAID_EXPS_AMT").ToString())).ToString("#,##0") : "0") : "0"%>
                                           
                                        </td>
                                        <td nowrap>
                                         $<%# Eval("EXC_RESRV_IDNMTY_AMT") != null ? (Eval("EXC_RESRV_IDNMTY_AMT").ToString() != "" ? (decimal.Parse(Eval("EXC_RESRV_IDNMTY_AMT").ToString())).ToString("#,##0") : "0") : "0"%>
                                            
                                        </td>
                                        <td nowrap>
                                        $<%# Eval("EXC_RESRV_EXPS_AMT") != null ? (Eval("EXC_RESRV_EXPS_AMT").ToString() != "" ? (decimal.Parse(Eval("EXC_RESRV_EXPS_AMT").ToString())).ToString("#,##0") : "0") : "0"%>
                                            
                                        </td>
                                        <td nowrap>
                                        $<%# Eval("EXC_INCURRED") != null ? (Eval("EXC_INCURRED").ToString() != "" ? (decimal.Parse(Eval("EXC_INCURRED").ToString())).ToString("#,##0") : "0") : "0"%>
                                            
                                        </td>
                                    </tr>
                                    <tr id="trItemtemplate2" runat="server" class="ItemTemplate">
                                        <td>
                                        </td>
                                        <td>
                                        </td>
                                        <td>
                                        </td>
                                        <td>
                                        </td>
                                        <td>
                                        </td>
                                        <td>
                                        </td>
                                        <td>
                                        </td>
                                        <td>
                                        </td>
                                        <td>
                                        </td>
                                        <td>
                                        </td>
                                        <td>
                                        </td>
                                    </tr>
                                </ItemTemplate>
                            </asp:AISListView>
                        </td>
                    </tr>
                </table>
            </ItemTemplate>
        </asp:AISListView>
    </asp:Panel>
    <div>
    <asp:Panel ID="pnlExcNonBillableLoss" Width="910px" runat="server" 
       Height="155px">
        <asp:AISListView ID="lstMasterExcNonBillableLoss" runat="server">
            <LayoutTemplate>
            
                <asp:PlaceHolder ID="itemPlaceholder" runat="server"></asp:PlaceHolder>
            </LayoutTemplate>
            <EmptyDataTemplate>
                <table id="lstLossesByPolicyTable" runat="server" width="910px">
                    <tr class="LayoutTemplate">
                        <th class="LossReport">
                            St.
                        </th>
                        <th class="LossReport">
                            Claim Number
                        </th>
                        <th class="LossReport">
                            Policy Number
                        </th>
                        <th class="LossReport">
                            SUB/<br />COV
                        </th>
                        <th class="LossReport">
                            Limit
                        </th>
                        <th class="LossReport">
                            ALAE Handling
                        </th>
                        <th class="LossReport">
                            IND
                        </th>
                        <th class="LossReport">
                            Paid Indemnity
                        </th>
                        <th class="LossReport">
                            Paid Expense
                        </th>
                        <th class="LossReport">
                            Reserved Indemnity
                        </th>
                        <th class="LossReport">
                            Reserved Expense
                        </th>
                        <th class="LossReport">
                            Incurred
                        </th>
                    </tr>
                    <tr id="ItemPlaceHolder" runat="server">
                    </tr>
                </table>
                <table width="910px">
                    <tr id="Tr1" runat="server" class="ItemTemplate">
                        <td align="center">
                            <asp:Label ID="lblEmptyMessage" Text="---No Records Found ---" Font-Bold="true" Width="600px"
                                runat="server" Style="text-align: center" />
                        </td>
                    </tr>
                </table>
            </EmptyDataTemplate>
            <ItemTemplate>
                <table width="910px">
                    <tr id="trItemTemplate" runat="server">
                        <td>
                            <asp:Label ID="lblID" runat="server" Visible="false" Text='<%# Eval("ARMIS_LOS_EXC_ID")%>'></asp:Label>
                        </td>
                        <td>
                            <asp:AISListView ID="lstChildExcNonBillableLosses" runat="server">
                                <LayoutTemplate>
                                    <table id="lstLossesByPolicyTable" runat="server" width="910px">
                                        <tr class="LayoutTemplate" style="font-size:9px">
                                            <th class="LossReport">
                                                State
                                            </th>
                                            <th class="LossReport">
                                                Claim Number
                                            </th>
                                            <th class="LossReport">
                                                Policy Number
                                            </th>
                                            <th class="LossReport">
                                                SUB/<br />COV
                                            </th>
                                            <th class="LossReport">
                                                Limit
                                            </th>
                                            <th class="LossReport">
                                                ALAE Handling
                                            </th>
                                            <th class="LossReport">
                                                IND
                                            </th>
                                            <th class="LossReport">
                                                Paid Indemnity
                                            </th>
                                            <th class="LossReport">
                                                Paid Expense
                                            </th>
                                            <th class="LossReport">
                                                Reserved Indemnity
                                            </th>
                                            <th class="LossReport">
                                                Reserved Expense
                                            </th>
                                            <th class="LossReport">
                                                Incurred
                                            </th>
                                        </tr>
                                        <tr id="ItemPlaceHolder" runat="server">
                                        </tr>
                                    </table>
                                </LayoutTemplate>
                                <ItemTemplate>
                                    <tr id="trItemTemplate" runat="server" class="ItemTemplate" >
                                        <td>
                                            <%# AISMasterEntities.ExcessLoss.State%>
                                        </td>
                                        <td style="font-size:9px" align="left">
                                            <%# Eval("CLAIM_NBR_TXT")%>
                                        </td>
                                        <td style="font-size:9px" align="left" nowrap>
                                            <%# AISMasterEntities.ExcessLoss.PolicyNumber%>
                                        </td>
                                        <td style="font-size:9px" align="left" nowrap>
                                            <%# AISMasterEntities.ExcessLoss.LOB%>
                                        </td>
                                        <td style="font-size:9px" align="left" nowrap>
                                            $<%# (Eval("LIMIT2_AMT") == null || Eval("LIMIT2_AMT").ToString() == "0.00") ? (Eval("POLICY_AMT") != null ? (Eval("POLICY_AMT").ToString() != "" ? (decimal.Parse(Eval("POLICY_AMT").ToString())).ToString("#,##0") : "0") : "0") : (decimal.Parse(Eval("LIMIT2_AMT").ToString())).ToString("#,##0")%>
                                        </td>
                                        <td style="font-size:9px" align="left" nowrap>
                                            <%# Eval("ALAE_TYP")%>
                                        </td>
                                        <td style="font-size:9px" align="left" nowrap>
                                            <b>Total</b>
                                        </td>
                                        <td style="font-size:9px" align="left" nowrap>
                                         $<%# Eval("PAID_IDNMTY_AMT") != null ? (Eval("PAID_IDNMTY_AMT").ToString() != "" ? (decimal.Parse(Eval("PAID_IDNMTY_AMT").ToString())).ToString("#,##0") : "0") : "0"%>
                                            
                                        </td>
                                        <td style="font-size:9px" align="left" nowrap>
                                        $<%# Eval("PAID_EXPS_AMT") != null ? (Eval("PAID_EXPS_AMT").ToString() != "" ? (decimal.Parse(Eval("PAID_EXPS_AMT").ToString())).ToString("#,##0") : "0") : "0"%>
                                            
                                        </td>
                                        <td style="font-size:9px;" align="left" nowrap>
                                         $<%# Eval("RESRV_IDNMTY_AMT") != null ? (Eval("RESRV_IDNMTY_AMT").ToString() != "" ? (decimal.Parse(Eval("RESRV_IDNMTY_AMT").ToString())).ToString("#,##0") : "0") : "0"%>
                                            
                                        </td>
                                        <td style="font-size:9px" align="left" nowrap>
                                        $<%# Eval("RESRV_EXPS_AMT") != null ? (Eval("RESRV_EXPS_AMT").ToString() != "" ? (decimal.Parse(Eval("RESRV_EXPS_AMT").ToString())).ToString("#,##0") : "0") : "0"%>
                                        
                                           
                                        </td >
                                        <td style="font-size:9px" align="left" nowrap>
                                         $<%# Eval("TOTAL_INCURRED") != null ? (Eval("TOTAL_INCURRED").ToString() != "" ? (decimal.Parse(Eval("TOTAL_INCURRED").ToString())).ToString("#,##0") : "0") : "0"%>
                                           
                                        </td>
                                    </tr>
                                    <tr id="trAlternatingItemTemplate" runat="server" class="AlternatingItemTemplate" style="font-size:8px">
                                        <td>
                                        </td>
                                        <td>
                                        </td>
                                        <td>
                                        </td>
                                        <td>
                                        </td>
                                        <td>
                                        </td>
                                        <td>
                                        </td>
                                        <td style="font-size:9px" align="left">
                                            <b>Non Billable</b>
                                        </td>
                                        <td style="font-size:9px" align="left">
                                         $<%# Eval("NON_BILABL_PAID_IDNMTY_AMT") != null ? (Eval("NON_BILABL_PAID_IDNMTY_AMT").ToString() != "" ? (decimal.Parse(Eval("NON_BILABL_PAID_IDNMTY_AMT").ToString())).ToString("#,##0") : "0") : "0"%>
                                            
                                        </td>
                                        <td style="font-size:9px" align="left">
                                         $<%# Eval("NON_BILABL_PAID_EXPS_AMT") != null ? (Eval("NON_BILABL_PAID_EXPS_AMT").ToString() != "" ? (decimal.Parse(Eval("NON_BILABL_PAID_EXPS_AMT").ToString())).ToString("#,##0") : "0") : "0"%>
                                            
                                        </td>
                                        <td style="font-size:9px" align="left">
                                        $<%# Eval("NON_BILABL_RESRV_IDNMTY_AMT") != null ? (Eval("NON_BILABL_RESRV_IDNMTY_AMT").ToString() != "" ? (decimal.Parse(Eval("NON_BILABL_RESRV_IDNMTY_AMT").ToString())).ToString("#,##0") : "0") : "0"%>
                                           
                                        </td>
                                        <td style="font-size:9px" align="left">
                                         $<%# Eval("NON_BILABL_RESRV_EXPS_AMT") != null ? (Eval("NON_BILABL_RESRV_EXPS_AMT").ToString() != "" ? (decimal.Parse(Eval("NON_BILABL_RESRV_IDNMTY_AMT").ToString())).ToString("#,##0") : "0") : "0"%>
                                            
                                        </td>
                                        <td style="font-size:9px" align="left">
                                        $<%# Eval("NON_BILABL_INCURRED") != null ? (Eval("NON_BILABL_INCURRED").ToString() != "" ? (decimal.Parse(Eval("NON_BILABL_INCURRED").ToString())).ToString("#,##0") : "0") : "0"%>
                                        
                                           
                                        </td>
                                    </tr>
                                    <tr id="trItemTemplate1" runat="server" class="ItemTemplate" style="font-size:8px">
                                        <td>
                                        </td>
                                        <td>
                                        </td>
                                        <td>
                                        </td>
                                        <td>
                                        </td>
                                        <td>
                                        </td>
                                        <td>
                                        </td>
                                        <td style="font-size:9px" align="left">
                                            <b>Subject</b>
                                        </td>
                                        <td style="font-size:9px" align="left">
                                         $<%# Eval("SUBJ_PAID_IDNMTY_AMT") != null ? (Eval("SUBJ_PAID_IDNMTY_AMT").ToString() != "" ? (decimal.Parse(Eval("SUBJ_PAID_IDNMTY_AMT").ToString())).ToString("#,##0") : "0") : "0"%>
                                            
                                        </td>
                                        <td style="font-size:9px" align="left">
                                        $<%# Eval("SUBJ_PAID_EXPS_AMT") != null ? (Eval("SUBJ_PAID_EXPS_AMT").ToString() != "" ? (decimal.Parse(Eval("SUBJ_PAID_EXPS_AMT").ToString())).ToString("#,##0") : "0") : "0"%>
                                            
                                        </td>
                                        <td style="font-size:9px" align="left">
                                         $<%# Eval("SUBJ_RESRV_IDNMTY_AMT") != null ? (Eval("SUBJ_RESRV_IDNMTY_AMT").ToString() != "" ? (decimal.Parse(Eval("SUBJ_RESRV_IDNMTY_AMT").ToString())).ToString("#,##0") : "0") : "0"%>
                                           
                                        </td>
                                        <td style="font-size:9px" align="left">
                                         $<%# Eval("SUBJ_RESRV_EXPS_AMT") != null ? (Eval("SUBJ_RESRV_EXPS_AMT").ToString() != "" ? (decimal.Parse(Eval("SUBJ_RESRV_EXPS_AMT").ToString())).ToString("#,##0") : "0") : "0"%>
                                            
                                        </td>
                                        <td style="font-size:9px" align="left">
                                         $<%# Eval("SUBJ_INCURRED") != null ? (Eval("SUBJ_INCURRED").ToString() != "" ? (decimal.Parse(Eval("SUBJ_INCURRED").ToString())).ToString("#,##0") : "0") : "0"%>
                                           
                                        </td>
                                    </tr>
                                    <tr id="trAlternateItemTemplate1" runat="server" class="AlternatingItemTemplate" style="font-size:8px">
                                        <td>
                                        </td>
                                        <td>
                                        </td>
                                        <td>
                                        </td>
                                        <td>
                                        </td>
                                        <td>
                                        </td>
                                        <td>
                                        </td>
                                        <td style="font-size:9px" align="left">
                                            <b>Excess</b>
                                        </td>
                                        <td style="font-size:9px" align="left" nowrap>
                                          $<%# Eval("EXC_PAID_IDNMTY_AMT") != null ? (Eval("EXC_PAID_IDNMTY_AMT").ToString() != "" ? (decimal.Parse(Eval("EXC_PAID_IDNMTY_AMT").ToString())).ToString("#,##0") : "0") : "0"%>
                                           
                                        </td>
                                        <td style="font-size:9px" align="left" nowrap>
                                        $<%# Eval("EXC_PAID_EXPS_AMT") != null ? (Eval("EXC_PAID_EXPS_AMT").ToString() != "" ? (decimal.Parse(Eval("EXC_PAID_EXPS_AMT").ToString())).ToString("#,##0") : "0") : "0"%>
                                            
                                        </td>
                                        <td style="font-size:9px" align="left" nowrap>
                                        $<%# Eval("EXC_RESRV_IDNMTY_AMT") != null ? (Eval("EXC_RESRV_IDNMTY_AMT").ToString() != "" ? (decimal.Parse(Eval("EXC_RESRV_IDNMTY_AMT").ToString())).ToString("#,##0") : "0") : "0"%>
                                            
                                        </td>
                                        <td style="font-size:9px" align="left" nowrap>
                                         $<%# Eval("EXC_RESRV_EXPS_AMT") != null ? (Eval("EXC_RESRV_EXPS_AMT").ToString() != "" ? (decimal.Parse(Eval("EXC_RESRV_EXPS_AMT").ToString())).ToString("#,##0") : "0") : "0"%>
                                           
                                        </td>
                                        <td style="font-size:9px" align="left" nowrap>
                                         $<%# Eval("EXC_INCURRED") != null ? (Eval("EXC_INCURRED").ToString() != "" ? (decimal.Parse(Eval("EXC_INCURRED").ToString())).ToString("#,##0") : "0") : "0"%>
                                            
                                        </td>
                                    </tr>
                                    <tr id="trItemtemplate2" runat="server" class="ItemTemplate" style="font-size:8px">
                                        <td>
                                        </td>
                                        <td>
                                        </td>
                                        <td>
                                        </td>
                                        <td>
                                        </td>
                                        <td>
                                        </td>
                                        <td>
                                        </td>
                                        <td>
                                        </td>
                                        <td>
                                        </td>
                                        <td>
                                        </td>
                                        <td>
                                        </td>
                                        <td>
                                        </td>
                                        <td>
                                        </td>
                                    </tr>
                                </ItemTemplate>
                            </asp:AISListView>
                        </td>
                    </tr>
                </table>
            </ItemTemplate>
        </asp:AISListView>
    </asp:Panel>
    </div>
</asp:Content>
