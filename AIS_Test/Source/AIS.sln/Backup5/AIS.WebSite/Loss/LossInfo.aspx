<%@ Page Language="C#" MasterPageFile="~/Retro.master" AutoEventWireup="true" CodeBehind="LossInfo.aspx.cs"
    Inherits="LossInfo" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="../App_Shared/AccountInfoHeader.ascx" TagName="AccountInfoHeader"
    TagPrefix="uc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeaderPlaceHolder" runat="Server">
    <asp:Label ID="lblLossInfo" runat="server" Text="Loss Information" CssClass="h1"></asp:Label>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainPlaceHolder" runat="Server">
    <script type="text/javascript" src="../JavaScript/RetroScript.js"></script>
    <script language="javascript" type="text/javascript">

        var scrollTop1;

        if (Sys.WebForms.PageRequestManager != null) {
            Sys.WebForms.PageRequestManager.getInstance().add_beginRequest(BeginRequestHandler);
            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);
        }

        function BeginRequestHandler(sender, args) {
            var mef = $get('<%=pnlLossInfo.ClientID%>');
            if (mef != null)
                scrollTop1 = mef.scrollTop;
        }

        function EndRequestHandler(sender, args) {
            var mef = $get('<%=pnlLossInfo.ClientID%>');
            if (mef != null)
                mef.scrollTop = scrollTop1;
        }    
    </script>
    <script type="text/javascript">
        function isLastDay() {
            var dt = document.getElementById('<%=txtFutureValDate.ClientID%>').value;
            if (dt != "") {
                var d = new Date(dt);
                var test = new Date(d.getTime()),
        month = test.getMonth();

                test.setDate(test.getDate() + 1);

                if (!(test.getMonth() !== month)) {
                    alert("Future Val Date should be month end date");
                    return false;
                }
            }
            
        }
    </script>
    <style type="text/css">
        .modelbackground
        {
            background-color: Gray;
            filter: alpha(opacity=80) !important;
            opacity: 0.80 !important;
            z-index: 11000 !important;
        }
        
        div[id*="UpdateProgress1_pnlPop"]
        {
            width: 250px;
            z-index: 11001 !important;
        }
        
        .modalBackgroundP
        {
            background-color: Gray;
            filter: alpha(opacity=70);
            opacity: 0.7;
            z-index: 10000;
        }
        
        .modalPopup1
        {
            width: 250px;
            z-index: 10001;
        }
    </style>
    <asp:HiddenField ID="btnHidden" runat="server" />
    <asp:HiddenField ID="hidPopShow" runat="server" />
    <asp:HiddenField ID="hodPopOk" runat="server" />
    <cc1:ModalPopupExtender runat="server" ID="modalLossInfoDetails" TargetControlID="btnHidden"
        PopupControlID="pnlPolDetails" BackgroundCssClass="modalBackgroundP" DropShadow="true"
        OkControlID="hodPopOk">
    </cc1:ModalPopupExtender>
    <div style="float: left;">
        <asp:UpdatePanel ID="updPolicyUpload" runat="server">
            <ContentTemplate>
                <asp:Panel runat="server" CssClass="modalPopup1" ID="pnlPolDetails" Style="border: solid 1px black;
                    display: none; width: 650px; padding: 0px;" HorizontalAlign="Center">
                    <asp:Panel runat="Server" ID="Panel3" Style="width: 100%; cursor: move; padding: 0px;
                        background-color: #CCCCCC; height: 20px; border: solid 1px Gray; color: Black;
                        text-align: center; vertical-align: middle; font-weight: bold; vertical-align: middle;
                        color: black; font-size: 12px">
                        Loss Info
                    </asp:Panel>
                    <div style="text-align: center; width: 100%; padding-bottom: 5px; background-color: White;">
                        <table align="center" border="0px">
                            <tr>
                                <td>
                                    <br />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3">
                                    <asp:Label ID="lblErrorLog" runat="server" Visible="True"></asp:Label>
                                    <%--Upload completed successfully. Please <a runat="server" id="lnkErrorLog">Click Here</a> for Error log--%>
                                    <br />
                                    <br />
                                </td>
                            </tr>
                            <tr>
                            <td>
                            <table>
                            <tr>
                             <td>
                            <asp:RadioButton ID="rbDirectCopy" Text="Direct Copy" runat="server" Checked="false" AutoPostBack="true" GroupName="lossCopy" OnCheckedChanged="rbDirectCopy_CheckedChanged"/>
                            </td>
                            <td>
                            <asp:RadioButton ID="rbSpreadSheetCopy" Text="Spreadsheet Copy" runat="server" Checked="false" AutoPostBack="true" GroupName="lossCopy" OnCheckedChanged="rbSpreadSheetCopy_CheckedChanged" />

                            </td>
                            </tr>
                            </table>
                            </td>
                           
                            </tr>
                            <tr>
                            <td>
                            <asp:Panel ID="pnlDirectCopy" runat="server" Visible="false" >
                            <table>
                            <tr>
                              <td align="right">
                                    Current Valuation Date : 
                                </td>
                                <td align="left" colspan="2">
                                    <asp:TextBox ID="txtFutureValDate" runat="server" Width="194px" onblur="isLastDay()"></asp:TextBox>
                                    <asp:ImageButton ID="imgValDate" runat="server" ImageUrl="~/images/Calendar_scheduleHS.png"
                                        CausesValidation="False" />
                                    <cc1:MaskedEditExtender ID="MaskedEditExtender6" runat="server" AcceptNegative="Left"
                                        DisplayMoney="Left" ErrorTooltipEnabled="True" Mask="99/99/9999" MaskType="Date"
                                        MessageValidatorTip="true" OnFocusCssClass="MaskedEditFocus" OnInvalidCssClass="MaskedEditError"
                                        TargetControlID="txtFutureValDate" />
                                    <cc1:CalendarExtender ID="CalendarExtender12" runat="server" PopupButtonID="imgValDate"
                                        TargetControlID="txtFutureValDate" />
                                </td>
                            </tr>
                            <tr>
                            <td align="right">
                                   Previous Valuation Date :
                                </td>
                            <td align="left" >
                                    <asp:DropDownList ID="ddlValuationDateDC" runat="server" AutoPostBack="true"  OnSelectedIndexChanged="ddlValuationDateDC_SelectedIndexChanged" Width="200px">                                        
                                        <asp:ListItem Value="0">Select</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                            
                            </tr>
                            <tr>
                            <td align="right">
                                    Adj No :
                                </td>                       
                            <td align="left">
                                    <asp:DropDownList ID="ddlAdjNoDC" runat="server" AutoPostBack="true" Width="200px" OnSelectedIndexChanged="ddlAdjNoDC_SelectedIndexChanged">
                                        <asp:ListItem Value="0">Select</asp:ListItem>
                                    </asp:DropDownList>
                                    </td>
                            <td align="left">
                                <asp:Button ID="btnDirectCopy" runat="server" Visible="True" Text="Copy Losses" OnClientClick="return isLastDay()" OnClick="btnDirectCopy_Click" />
                                </td>
                            </tr>

                            </table>
                            </asp:Panel>
                            </td>                              
                            </tr>
                            <tr>
                             <td>
                            <asp:Panel ID="pnlSpreadSheetCopy" runat="server" Visible="false" >
                            <table>
                            <tr>
                                <td align="right">
                                  Previous Valuation Date :
                                </td>
                                <td align="left" >
                                    <asp:DropDownList ID="ddlValuationDate" runat="server" AutoPostBack="true" Width="241px"
                                        OnSelectedIndexChanged="ddlValuationDate_SelectedIndexChanged">
                                        <asp:ListItem Value="0">Select</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                                <td align="left">
                                <%--<asp:Button ID="btnDirectCopy" runat="server" Text="Copy Losses" OnClick="btnPGMSelectAll_Click" />--%>
                                </td>
                            </tr>
                            <tr>
                                <td align="right">
                                    Adj No :
                                </td>
                                <td align="left">
                                    <asp:DropDownList ID="ddlAdjNo" runat="server" AutoPostBack="true" Width="241px"
                                        OnSelectedIndexChanged="ddlAdjNo_SelectedIndexChanged">
                                        <asp:ListItem Value="0">Select</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                                
                            </tr>
                            <tr>
                                <td align="right">
                                    Program Period :
                                </td>
                                <td align="left">
                                    <%--    <asp:DropDownList ID="ddlPgmPrd" runat="server" AutoPostBack="true" Width="200px" OnSelectedIndexChanged="ddlPgmPrd_SelectedIndexChanged">
                                                                <asp:ListItem Value="0">Select</asp:ListItem> 
                                                            </asp:DropDownList>--%>
                                    <asp:Panel ID="pnlPGMList" runat="server" Style="border: 1px; max-height: 120px"
                                        ScrollBars="Auto" Width="241px" CssClass="content">
                                        <asp:CheckBoxList ID="chkPGMList" runat="server" AutoPostBack="true" OnSelectedIndexChanged="chkPGMList_SelectedIndexChanged">
                                        </asp:CheckBoxList>
                                    </asp:Panel>
                                </td>
                                <td align="left" style="vertical-align: middle">
                                    <asp:Button ID="btnPGMSelectAll" runat="server" Text="Select All" OnClick="btnPGMSelectAll_Click" />
                                </td>
                            </tr>
                            <tr>
                                <td align="right">
                                    Policies :
                                </td>
                                <td align="left">
                                    <asp:Panel ID="pnlPolicyNumberListLBA" runat="server" Style="border: 1px; max-height: 120px"
                                        ScrollBars="Auto" Width="241px" CssClass="content">
                                        <asp:CheckBoxList ID="chkpollist" runat="server" AutoPostBack="true" OnSelectedIndexChanged="chkpollist_SelectedIndexChanged">
                                        </asp:CheckBoxList>
                                    </asp:Panel>
                                </td>
                                <td align="left" style="vertical-align: middle">
                                    <asp:Button ID="btnSelectAll" runat="server" Text="Select All" OnClick="btnSelectAll_Click" />
                                </td>
                            </tr>
                            <tr>
                                <td align="right">
                                    System Generated/Manual losses :
                                </td>
                                <td align="left">
                                    <asp:DropDownList ID="ddlLossType" runat="server" Width="241px">
                                        <asp:ListItem Value="2">All</asp:ListItem>
                                        <asp:ListItem Value="1">System Generated</asp:ListItem>
                                        <asp:ListItem Value="0">Manual</asp:ListItem>
                                    </asp:DropDownList>
                                    &nbsp;&nbsp;
                                </td>
                                <td align="left">
                                    <asp:Button ID="btnLossDownload" runat="server" Text="Download Loss Info" OnClick="btnLossDownload_Click" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <br />
                                </td>
                            </tr>
                            <tr>
                                <td align="right">
                                    Upload Excel Sheet :
                                </td>
                                <td align="left" colspan="2">
                                    <asp:FileUpload ID="flUpload" runat="server" EnableViewState="false" Width="241px" />
                                </td>
                            </tr>
                            <tr>
                                <td align="Center" colspan="3">
                                    <asp:Button ID="btnValidateLoss" runat="server" OnClick="btnValidateLoss_Click" Text="Validate Loss Info" />
                                    &nbsp;&nbsp;&nbsp;
                                    <asp:Button ID="btnUploadLoss" runat="server" Text="Upload Loss Info" OnClick="btnUploadLoss_Click"
                                        Enabled="false" />
                                    
                                </td>
                            </tr>
                            </table>
                            </asp:Panel>
                            </td>
                            </tr>
                            
                         <tr>
                         
                        <td>
                        <br />
                        <br />
                        <table>
                        <tr>
                         <td>
                         <asp:Button ID="btnPopCancel" runat="server" Text="Cancel" OnClick="btnPopCancel_Click" />
                                    <asp:HiddenField ID="hidForModel" runat="server" />
                                    </td>
                           <td>
                                    <asp:Button ID="btnClose" runat="server" Text="Close" OnClick="btnClose_Click" />
                                    </td>

                        </tr>
                        </table>
                        </td>
                         </tr>
                           
                            <tr>
                                <td>
                                    <br />
                                </td>
                            </tr>
           
                            <tr>
                                <td>
                                    <br />
                                    <br />
                                </td>
                            </tr>
                            <%-- <tr>
                                                    <td align="Center">
                                                        <asp:Button ID="btnViewPolicies" runat="server" Text="View Policies" OnClick="btnViewPolicies_Click" />
                                                        &nbsp;&nbsp;&nbsp;
                                                        <asp:Button ID="btnViewFactors" runat="server" Text="View Stepped Factors" OnClick="btnViewFactors_Click" />
                                                    </td>
                                                </tr>--%>
                        </table>
                    </div>
                </asp:Panel>
            </ContentTemplate>
            <Triggers>
                <asp:PostBackTrigger ControlID="btnValidateLoss" />
            </Triggers>
        </asp:UpdatePanel>
    </div>
    <asp:UpdatePanel runat="server" ID="updInternal">
        <ContentTemplate>
            <asp:ValidationSummary ID="ValSearch" runat="server" ValidationGroup="Search" BorderColor="Red"
                BorderWidth="1" BorderStyle="Solid" />
            <asp:ValidationSummary ID="valCombined" runat="server" ValidationGroup="Save" BorderColor="Red"
                BorderWidth="1" BorderStyle="Solid" />
            <asp:ValidationSummary ID="valCombinedElems" runat="server" ValidationGroup="Update"
                BorderColor="Red" BorderWidth="1" BorderStyle="Solid" />
            <table>
                <tr>
                    <td>
                        <uc1:AccountInfoHeader ID="AccountInfoHeader1" runat="server" />
                        <br />
                    </td>
                </tr>
            </table>
            <table>
                <tr>
                    <asp:Label ID="lblErrorMessage" runat="server" Text="" ForeColor="Red"></asp:Label>
                    <td>
                        <asp:Label ID="lblStartDate" runat="server" Text="Program Period"></asp:Label>
                    </td>
                    <td>
                        <%--<asp:ObjectDataSource ID="EffDateDataSource" runat="server" SelectMethod="getPolicyLookUpData" TypeName="ZurichNA.AIS.Business.Logic.PolicyBS">
                </asp:ObjectDataSource>--%>
                        <asp:DropDownList ID="ddlProgramPeriod" TabIndex="1" runat="server" DataTextField="STARTDATE_ENDDATE_PGMTYP"
                            DataValueField="PREM_ADJ_PGM_ID" AutoPostBack="false" Width="240px" ValidationGroup="Search"
                            OnDataBound="ddlProgramPeriod_DataBound">
                            <asp:ListItem Text="(Select)" Value="0"></asp:ListItem>
                        </asp:DropDownList>
                        <asp:CompareValidator ID="CompareddlEffDate" runat="server" Display="Dynamic" ControlToValidate="ddlProgramPeriod"
                            ValueToCompare='(Select)' ErrorMessage="Please select the Program Period" Text="*"
                            Operator="NotEqual" ValidationGroup="Search"></asp:CompareValidator>
                    </td>
                    <td>
                        <asp:CheckBox ID="chkHideDisLines" TabIndex="2" runat="server" ValidationGroup="Search"
                            Text="Hide Disabled Lines" OnCheckedChanged="chkHideDisLines_CheckedChanged"
                            AutoPostBack="true" />
                    </td>
                    <td>
                        <asp:Button ID="btnDisplayLossesByPol" runat="server" Text="Display Losses By Policy Report"
                            Width="200px" ValidationGroup="Search" Enabled="false" OnClick="btnDisplayLossesByPol_Click" />
                    </td>
                    <td>
                        <asp:Button ID="btnLossinfo" runat="server" Text="Copy Losses" Width="200px" OnClick="btnLossinfo_Click" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <%--<asp:Label ID="lblEndDate" runat="server" Text="Policy Expiration Date"></asp:Label>--%>
                    </td>
                    <td>
                        <%--<asp:DropDownList ID="ddlEndDate" TabIndex="1" runat="server"  DataTextField="POLICY_END_DATE"
                    DataValueField="POLICY_END_DATE" AutoPostBack="false" Width="100px" ValidationGroup="Search">
                </asp:DropDownList>
                <asp:CompareValidator ID="CompareddlEndDate" runat="server" Display="Dynamic" ControlToValidate="ddlEndDate"
                                                                                ValueToCompare='(Select)' ErrorMessage="Please select the Policy Expiration Date." Text="*" Operator="NotEqual"
                                                                                ValidationGroup="Search"></asp:CompareValidator>--%>
                    </td>
                    <td>
                        <asp:Button ID="btnSearch" runat="server" Text="Search" Width="53px" OnClick="btnSearch_Click"
                            ValidationGroup="Search" />
                    </td>
                    <td>
                        <asp:Button ID="btnDisplayLossesByLob" runat="server" Text="Display Losses By LOB Report"
                            Width="200px" ValidationGroup="Search" Enabled="false" OnClick="btnDisplayLossesByLob_Click" />
                    </td>
                </tr>
            </table>
            <table>
                <tr>
                    <td>
                        <asp:Panel ID="pnlLossInfo" runat="server" Height="230px" ScrollBars="Auto" Width="920px">
                            <asp:ObjectDataSource ID="objDataSourceState" runat="server" SelectMethod="GetLookUpActiveDataWithSelect"
                                TypeName="ZurichNA.AIS.Business.Logic.BLAccess">
                                <SelectParameters>
                                    <asp:Parameter DefaultValue="STATE" Name="lookUpTypeName" Type="String" />
                                </SelectParameters>
                            </asp:ObjectDataSource>
                            <%-- <asp:ObjectDataSource ID="objDataSourceLOB" runat="server" SelectMethod="getLOBLookUpData"
                        TypeName="ZurichNA.AIS.Business.Logic.LossInfoBS">                        
                    </asp:ObjectDataSource>--%>
                            <asp:ObjectDataSource ID="objDataSourceLOB" runat="server" SelectMethod="GetLookUpActiveData"
                                TypeName="ZurichNA.AIS.Business.Logic.BLAccess">
                                <SelectParameters>
                                    <asp:Parameter DefaultValue="LOB" Name="lookUpTypeName" Type="String" />
                                </SelectParameters>
                            </asp:ObjectDataSource>
                            <asp:ObjectDataSource ID="objDataSourcePolicy" runat="server" SelectMethod="getPolicyData"
                                TypeName="ZurichNA.AIS.Business.Logic.PolicyBS"></asp:ObjectDataSource>
                            <asp:AISListView ID="lsvLossInfo" runat="server" InsertItemPosition="FirstItem" OnItemEditing="EditList"
                                DataKeyNames="ARMIS_LOS_ID" OnItemCanceling="CancelList" OnItemInserting="InsertList"
                                OnItemCommand="CommandList" OnItemUpdating="UpdateList" OnItemDataBound="DataBoundList"
                                OnItemCreated="lsvLossInfo_ItemCreated" OnSorting="lsvLossInfo_Sorting">
                                <LayoutTemplate>
                                    <table id="Table1" class="panelContents" runat="server" width="98%">
                                        <tr class="LayoutTemplate">
                                            <th>
                                            </th>
                                            <th>
                                                <asp:LinkButton ID="lbExcNonBil" CommandName="Sort" CommandArgument="EXC_NON_BIL"
                                                    runat="server" Font-Size="XX-Small">Excess<br />/Non<br />billable</asp:LinkButton>
                                                <asp:Image ID="imgSortByExcNonBil" runat="server" ImageUrl="~/images/descending.gif"
                                                    ToolTip="Descending" Visible="false" />
                                            </th>
                                            <th font-size="XX-Small">
                                                LOB
                                            </th>
                                            <th>
                                                <asp:LinkButton ID="lbPolicySort" CommandName="Sort" CommandArgument="POLICY" runat="server"
                                                    Font-Size="XX-Small">Policy</asp:LinkButton>
                                                <asp:Image ID="imgSortByPolicySort" runat="server" ImageUrl="~/images/Ascending.gif"
                                                    ToolTip="Ascending" Visible="false" />
                                            </th>
                                            <th font-size="XX-Small">
                                                State
                                            </th>
                                            <th font-size="XX-Small">
                                                Total Paid<br />
                                                Indemnity
                                            </th>
                                            <th font-size="XX-Small">
                                                Total Paid<br />
                                                Expense
                                            </th>
                                            <th font-size="XX-Small">
                                                Total Reserved<br />
                                                Indemnity
                                            </th>
                                            <th font-size="XX-Small">
                                                Total Reserved<br />
                                                Expense
                                            </th>
                                            <th font-size="XX-Small">
                                                Incurred
                                            </th>
                                            <th font-size="XX-Small">
                                                Sys.<br />
                                                Gen
                                            </th>
                                            <th font-size="XX-Small">
                                                Copy<br />
                                                Losses
                                            </th>
                                            <th font-size="XX-Small">
                                                Disable
                                            </th>
                                            <th font-size="XX-Small">
                                                Details
                                            </th>
                                        </tr>
                                        <tr id="itemPlaceholder" runat="server">
                                        </tr>
                                    </table>
                                </LayoutTemplate>
                                <AlternatingItemTemplate>
                                    <tr class="AlternatingItemTemplate">
                                        <td align="center">
                                            <asp:LinkButton ID="lnkEdit" CommandName="Edit" runat="server" Text="Edit" Font-Size="XX-Small"></asp:LinkButton>
                                        </td>
                                        <td align="center">
                                            <asp:HiddenField ID="hidPremAdJID" runat="server" Value='<%# Bind("PREM_ADJ_ID") %>' />
                                            <asp:HiddenField ID="hidAdjStatus" runat="server" Value='<%# Bind("ADJ_STATUS") %>' />
                                            <asp:HiddenField ID="hidComlAgmtID" runat="server" Value='<%# Bind("COML_AGMT_ID") %>' />
                                            <asp:LinkButton ID="lnkExcNonBil" runat="server" Font-Size="XX-Small" CommandArgument='<%# Bind("ARMIS_LOS_ID") %>'
                                                Text='<%# Eval("EXC_NON_BIL") != null ? "Yes" : "No" %>' Enabled='<%# Eval("ACTV_IND")%>'
                                                CommandName="SelectExcRow"></asp:LinkButton>
                                        </td>
                                        <td align="center">
                                            <asp:Label ID="lblLOB" runat="server" Text='<%# Bind("POLICYSYMBOL") %>' Font-Size="XX-Small"></asp:Label>
                                        </td>
                                        <td align="center">
                                            <asp:Label ID="lblPolicy" runat="server" Text='<%# Bind("POLICY") %>' Font-Size="XX-Small"></asp:Label>
                                        </td>
                                        <td align="center">
                                            <asp:Label ID="lblState" runat="server" Text='<%# Bind("STATETYPE") %>' Font-Size="XX-Small"></asp:Label>
                                        </td>
                                        <td align="center">
                                            <asp:Label ID="lblTotalPaidIndem" runat="server" Font-Size="XX-Small" Text='<%# Eval("PAID_IDNMTY_AMT") != null ? (Eval("PAID_IDNMTY_AMT").ToString() != "" ?(decimal.Parse(Eval("PAID_IDNMTY_AMT").ToString())).ToString("#,##0") : "0"): "0"%>'></asp:Label>
                                        </td>
                                        <td align="center">
                                            <asp:Label ID="lblTotalPaidExp" runat="server" Font-Size="XX-Small" Text='<%# Eval("PAID_EXPS_AMT") != null ? (Eval("PAID_EXPS_AMT").ToString() != "" ?(decimal.Parse(Eval("PAID_EXPS_AMT").ToString())).ToString("#,##0") : "0"): "0"%>'></asp:Label>
                                        </td>
                                        <td align="center">
                                            <asp:Label ID="lblTotalResrvIndem" runat="server" Font-Size="XX-Small" Text='<%# Eval("RESRV_IDNMTY_AMT") != null ? (Eval("RESRV_IDNMTY_AMT").ToString() != "" ?(decimal.Parse(Eval("RESRV_IDNMTY_AMT").ToString())).ToString("#,##0") : "0"): "0"%>'></asp:Label>
                                        </td>
                                        <td align="center">
                                            <asp:Label ID="lblTotalResrvExp" runat="server" Font-Size="XX-Small" Text='<%# Eval("RESRV_EXPS_AMT") != null ? (Eval("RESRV_EXPS_AMT").ToString() != "" ?(decimal.Parse(Eval("RESRV_EXPS_AMT").ToString())).ToString("#,##0") : "0"): "0"%>'></asp:Label>
                                        </td>
                                        <td align="center">
                                            <asp:Label ID="lblIncurred" runat="server" Font-Size="XX-Small" Text='<%# Eval("Incurred") != null ? (Eval("Incurred").ToString() != "" ?(decimal.Parse(Eval("Incurred").ToString())).ToString("#,##0") : "0"): "0"%>'></asp:Label>
                                        </td>
                                        <td align="center">
                                            <asp:Label ID="lblSysGen" runat="server" Font-Size="XX-Small" Text='<%# Eval("SYS_GENRT_IND").ToString() == "True" ? "Yes" : "No" %>'></asp:Label>
                                        </td>
                                        <td align="center">
                                            <asp:Label ID="lblCpyLos" runat="server" Font-Size="XX-Small" Text='<%# Eval("COPY_IND").ToString() == "True" ? "Yes" : "No" %>'></asp:Label>
                                            
                                        </td>
                                        <td align="center">
                                            <asp:HiddenField ID="hidActive" runat="server" Value='<%# Bind("ACTV_IND")%>' />
                                            <asp:ImageButton ID="imgDisable" CommandArgument='<%# Bind("ARMIS_LOS_ID") %>' runat="server"
                                                ImageUrl='<%# Eval("ACTV_IND").ToString()=="True"?"~/images/disabled.GIF":"~/images/enabled.GIF" %>'>
                                            </asp:ImageButton>
                                        </td>
                                        <td align="center">
                                            <asp:LinkButton ID="lnkSelect" CommandArgument='<%# Bind("ARMIS_LOS_ID") %>' CommandName="SelectRow"
                                                runat="server" Text="Details" Font-Size="XX-Small"></asp:LinkButton>
                                        </td>
                                    </tr>
                                    <tr>
                                    </tr>
                                </AlternatingItemTemplate>
                                <EditItemTemplate>
                                    <tr class="ItemTemplate">
                                        <td align="center">
                                            <asp:LinkButton ID="lnkUpdate" ValidationGroup="Update" CommandArgument='<%# Bind("ARMIS_LOS_ID") %>'
                                                CommandName="Update" runat="server" Text="Update" Font-Size="XX-Small"></asp:LinkButton>
                                            <asp:LinkButton ID="lnkCancel" CommandName="Cancel" Font-Size="XX-Small" runat="server"
                                                Text="Cancel"></asp:LinkButton>
                                        </td>
                                        <td align="center">
                                            <asp:HiddenField ID="hidArmisLosID" runat="server" Value='<%# Bind("ARMIS_LOS_ID") %>' />
                                            <asp:Label ID="lblExcNonBil" runat="server" Width="25px" Font-Size="XX-Small"></asp:Label>
                                        </td>
                                        <td align="center">
                                            <asp:HiddenField ID="hidLOB" runat="server" Value='<%# Bind("PolicySymbol") %>' />
                                            <asp:DropDownList ID="ddlLOB" runat="server" DataSourceID="objDataSourceLOB" DataTextField="LookUpName"
                                                Width="57px" Font-Size="XX-Small" DataValueField="LookUpName" AutoPostBack="true"
                                                ValidationGroup="Update" OnSelectedIndexChanged="ddlLOB_SelectedIndexChanged">
                                            </asp:DropDownList>
                                            <asp:CompareValidator ID="CompareddlLOB" runat="server" ControlToValidate="ddlLOB"
                                                ValueToCompare='(Select)' ErrorMessage="Please select the LOB." Text="*" Operator="NotEqual"
                                                ValidationGroup="Update"></asp:CompareValidator>
                                        </td>
                                        <td align="center">
                                            <asp:HiddenField ID="hidPolicy" runat="server" Value='<%# Bind("POLICY") %>' />
                                            <asp:HiddenField ID="hidComlAgmtID" runat="server" Value='<%# Bind("COML_AGMT_ID") %>' />
                                            <asp:DropDownList ID="ddlPolicy" ValidationGroup="Update" Font-Size="XX-Small" Width="118px"
                                                runat="server" DataSourceID="" DataTextField="PolicyNumber" DataValueField="PolicyID">
                                            </asp:DropDownList>
                                            <asp:CompareValidator ID="CompareddlPolicy" runat="server" ControlToValidate="ddlPolicy"
                                                ValueToCompare="0" ErrorMessage="Please select the Policy." Text="*" Operator="NotEqual"
                                                ValidationGroup="Update"></asp:CompareValidator>
                                        </td>
                                        <td align="center">
                                            <asp:HiddenField ID="hidState" runat="server" Value='<%# Bind("STATETYPE") %>' />
                                            <asp:DropDownList ID="ddlState" runat="server" DataSourceID="objDataSourceState"
                                                Font-Size="XX-Small" Width="42px" DataTextField="Attribute1" DataValueField="LookUpID"
                                                ValidationGroup="Update">
                                            </asp:DropDownList>
                                            <asp:CompareValidator ID="CompareddlState" runat="server" ControlToValidate="ddlState"
                                                ValueToCompare="0" ErrorMessage="Please select the State." Text="*" Operator="NotEqual"
                                                ValidationGroup="Update"></asp:CompareValidator>
                                        </td>
                                        <td align="center">
                                            <asp:AISAmountTextbox ID="txtTotalPaidIndem" ValidationGroup="Update" Font-Size="XX-Small"
                                                runat="server" Text='<%# Bind("PAID_IDNMTY_AMT","{0:0}") %>' Width="83px"></asp:AISAmountTextbox>
                                            <%--<asp:TextBox ID="txtTotalPaidIndem" ValidationGroup="Update" Font-Size="XX-Small" MaxLength="11" runat="server" Text='<%# Bind("PAID_IDNMTY_AMT","{0:0}") %>' width= "83px" onblur="FormatNumNoDecAmt(this,11)" onfocus="RemoveCommas(this)"></asp:TextBox>--%>
                                            <asp:RequiredFieldValidator ID="reqTotalPaidIndem" runat="server" ErrorMessage="Please enter Total Paid Indemnity."
                                                Text="*" ValidationGroup="Update" ControlToValidate="txtTotalPaidIndem"></asp:RequiredFieldValidator>
                                            <%--<ajaxToolkit:FilteredTextBoxExtender ID="fltrTotalPaidIndem" runat="server" TargetControlID="txtTotalPaidIndem"
                                                         ValidChars="0123456789,"   FilterType="Custom"  />--%>
                                            <%--<asp:CompareValidator ID="CompareTotalPaidIndem" runat="server" ControlToValidate="txtTotalPaidIndem"
                                                                                ValueToCompare="0" ErrorMessage="Please enter Total Paid Indemnity." Text="*" Operator="NotEqual"
                                                                                ValidationGroup="Update"></asp:CompareValidator>--%>
                                            <%--<cc1:MaskedEditExtender ID="MaskedEditTotalPaidIndem" runat="server" 
                                      TargetControlID="txtTotalPaidIndem" 
                                      Mask="99,999,999,999" MessageValidatorTip="true" MaskType="Number" DisplayMoney ="None" 
                                      OnFocusCssClass="MaskedEditFocus"  OnInvalidCssClass="MaskedEditError" 
                                      InputDirection="RightToLeft" AcceptNegative="None"  AutoComplete="false"/>--%>
                                        </td>
                                        <td align="center">
                                            <asp:AISAmountTextbox ID="txtTotalPaidExp" ValidationGroup="Update" Font-Size="XX-Small"
                                                runat="server" Text='<%# Bind("PAID_EXPS_AMT","{0:0}") %>' Width="83px"></asp:AISAmountTextbox>
                                            <asp:RequiredFieldValidator ID="reqTotalPaidExp" runat="server" ErrorMessage="Please enter Total Paid Expense Amount."
                                                Text="*" ValidationGroup="Update" ControlToValidate="txtTotalPaidExp"></asp:RequiredFieldValidator>
                                            <%--<ajaxToolkit:FilteredTextBoxExtender ID="fltrTotalPaidExp" runat="server" TargetControlID="txtTotalPaidExp"
                                                ValidChars="0123456789," FilterType="Custom" />--%>
                                            <%--<asp:CompareValidator ID="CompareTotalPaidExp" runat="server" ControlToValidate="txtTotalPaidExp"
                                                                                ValueToCompare="0" ErrorMessage="Please enter Total Paid Excess Amount." Text="*" Operator="NotEqual"
                                                                                ValidationGroup="Update"></asp:CompareValidator>--%>
                                            <%--<cc1:MaskedEditExtender ID="MaskedEditTotalPaidExp" runat="server" 
                                      TargetControlID="txtTotalPaidExp" 
                                      Mask="99,999,999,999" MessageValidatorTip="true" MaskType="Number" DisplayMoney ="None" 
                                      OnFocusCssClass="MaskedEditFocus"  OnInvalidCssClass="MaskedEditError" 
                                      InputDirection="RightToLeft" AcceptNegative="None" AutoComplete="false" />--%>
                                        </td>
                                        <td align="center">
                                            <asp:AISAmountTextbox ID="txtTotalResrvIndem" ValidationGroup="Update" Font-Size="XX-Small"
                                                runat="server" Text='<%# Bind("RESRV_IDNMTY_AMT","{0:0}") %>' Width="83px"></asp:AISAmountTextbox>
                                            <asp:RequiredFieldValidator ID="reqTotalResrvIndem" runat="server" ErrorMessage="Please enter Total Reserved Indemnity."
                                                Text="*" ValidationGroup="Update" ControlToValidate="txtTotalResrvIndem"></asp:RequiredFieldValidator>
                                            <%--<ajaxToolkit:FilteredTextBoxExtender ID="fltrTotalResrvIndem" runat="server" TargetControlID="txtTotalResrvIndem"
                                                ValidChars="0123456789," FilterType="Custom" />--%>
                                            <%--  <asp:CompareValidator ID="CompareTotalResrvIndem" runat="server" ControlToValidate="txtTotalResrvIndem"
                                                                                ValueToCompare="0" ErrorMessage="Please enter Total Reserved Indemnity." Text="*" Operator="NotEqual"
                                                                                ValidationGroup="Update"></asp:CompareValidator>--%>
                                            <%--<cc1:MaskedEditExtender ID="MaskedEditTotalResrvIndem" runat="server" 
                                      TargetControlID="txtTotalResrvIndem" 
                                      Mask="99,999,999,999" MessageValidatorTip="true" MaskType="Number" DisplayMoney ="None" 
                                      OnFocusCssClass="MaskedEditFocus"  OnInvalidCssClass="MaskedEditError" 
                                      InputDirection="RightToLeft" AcceptNegative="None" AutoComplete="false" />--%>
                                        </td>
                                        <td align="center">
                                            <asp:AISAmountTextbox ID="txtTotalResrvExp" ValidationGroup="Update" Font-Size="XX-Small"
                                                runat="server" Text='<%# Bind("RESRV_EXPS_AMT","{0:0}") %>' Width="83px"></asp:AISAmountTextbox>
                                            <asp:RequiredFieldValidator ID="reqTotalResrvExp" runat="server" ErrorMessage="Please enter Total Reserved Expense Amount."
                                                Text="*" ValidationGroup="Update" ControlToValidate="txtTotalResrvExp"></asp:RequiredFieldValidator>
                                            <%--<ajaxToolkit:FilteredTextBoxExtender ID="fltrTotalResrvExp" runat="server" TargetControlID="txtTotalResrvExp"
                                                ValidChars="0123456789," FilterType="Custom" />--%>
                                            <%--<asp:CompareValidator ID="CompareTotalResrvExp" runat="server" ControlToValidate="txtTotalResrvExp"
                                                                                ValueToCompare="0" ErrorMessage="Please Please enter Total Reserved Excess Amount." Text="*" Operator="NotEqual"
                                                                                ValidationGroup="Update"></asp:CompareValidator>--%>
                                            <%--<cc1:MaskedEditExtender ID="MaskedEditTotalResrvExp" runat="server" 
                                      TargetControlID="txtTotalResrvExp" 
                                      Mask="99,999,999,999" MessageValidatorTip="true" MaskType="Number" DisplayMoney ="None" 
                                      OnFocusCssClass="MaskedEditFocus"  OnInvalidCssClass="MaskedEditError" 
                                      InputDirection="RightToLeft" AcceptNegative="None" AutoComplete="false" />--%>
                                        </td>
                                        <td align="center">
                                            <asp:Label ID="lblIncurred" runat="server" Font-Size="XX-Small"></asp:Label>
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
                                    <tr>
                                    </tr>
                                </EditItemTemplate>
                                <InsertItemTemplate>
                                    <tr class="ItemTemplate">
                                        <td align="center">
                                            <asp:LinkButton ValidationGroup="Save" ID="lnkSave" CommandName="Save" runat="server"
                                                Text="Save" Font-Size="XX-Small" Width="20px"></asp:LinkButton>
                                        </td>
                                        <td align="center">
                                            <asp:Label ID="lblSaveExcNonBil" Width="25px" runat="server" Text="No" Font-Size="XX-Small"></asp:Label>
                                        </td>
                                        <td align="center">
                                            <asp:DropDownList ID="ddlSaveLOB" runat="server" DataSourceID="objDataSourceLOB"
                                                DataTextField="LookUpName" Font-Size="XX-Small" Width="57px" DataValueField="LookUpName"
                                                AutoPostBack="true" OnSelectedIndexChanged="ddlSaveLOB_SelectedIndexChanged"
                                                ValidationGroup="Save">
                                            </asp:DropDownList>
                                            <asp:CompareValidator ID="CompareddlSaveLOB" runat="server" ControlToValidate="ddlSaveLOB"
                                                ValueToCompare='(Select)' ErrorMessage="Please select the LOB." Text="*" Operator="NotEqual"
                                                ValidationGroup="Save"></asp:CompareValidator>
                                        </td>
                                        <td align="center">
                                            <asp:DropDownList ID="ddlSavePolicy" ValidationGroup="Save" Font-Size="XX-Small"
                                                Width="118px" Enabled="false" runat="server" DataSourceID="" DataTextField="PolicyNumber"
                                                DataValueField="PolicyID">
                                            </asp:DropDownList>
                                            <asp:CompareValidator ID="CompareddlSavePolicy" runat="server" ControlToValidate="ddlSavePolicy"
                                                ValueToCompare="0" ErrorMessage="Please select the Policy." Text="*" Operator="NotEqual"
                                                ValidationGroup="Save"></asp:CompareValidator>
                                        </td>
                                        <td align="center">
                                            <asp:DropDownList ID="ddlSaveState" runat="server" DataSourceID="objDataSourceState"
                                                ValidationGroup="Save" Font-Size="XX-Small" DataTextField="Attribute1" DataValueField="LookUpID"
                                                Width="42px">
                                            </asp:DropDownList>
                                            <asp:CompareValidator ID="CompareddlSaveState" runat="server" ControlToValidate="ddlSaveState"
                                                ValueToCompare="0" ErrorMessage="Please select the State." Text="*" Operator="NotEqual"
                                                ValidationGroup="Save"></asp:CompareValidator>
                                        </td>
                                        <td align="center">
                                            <asp:AISAmountTextbox ID="txtSaveTotalPaidIndem" ValidationGroup="Save" Font-Size="XX-Small"
                                                runat="server" Width="84px" Text='<%# Bind("PAID_IDNMTY_AMT") %>'></asp:AISAmountTextbox>
                                            <asp:RequiredFieldValidator ID="reqSaveTotalPaidIndem" runat="server" ErrorMessage="Please enter Total Paid Indemnity."
                                                Text="*" ValidationGroup="Save" ControlToValidate="txtSaveTotalPaidIndem"></asp:RequiredFieldValidator>
                                            <%--<ajaxToolkit:FilteredTextBoxExtender ID="fltrSaveTotalPaidIndem" runat="server" TargetControlID="txtSaveTotalPaidIndem"
                                                ValidChars="0123456789," FilterType="Custom" />--%>
                                            <%--<asp:CompareValidator ID="CompareSaveTotalPaidIndem" runat="server" ControlToValidate="txtSaveTotalPaidIndem"
                                                                                ValueToCompare="0" ErrorMessage="Please enter Total Paid Indemnity." Text="*" Operator="NotEqual"
                                                                                ValidationGroup="Save"></asp:CompareValidator>--%>
                                            <%--<cc1:MaskedEditExtender ID="MaskedEditSaveTotalPaidIndem" runat="server" 
                                      TargetControlID="txtSaveTotalPaidIndem" 
                                      Mask="99,999,999,999" MessageValidatorTip="true" MaskType="Number" DisplayMoney ="None" 
                                      OnFocusCssClass="MaskedEditFocus"  OnInvalidCssClass="MaskedEditError" 
                                      InputDirection="RightToLeft" AcceptNegative="None" AutoComplete="false"  />--%>
                                        </td>
                                        <td align="center">
                                            <asp:AISAmountTextbox ID="txtSaveTotalPaidExp" ValidationGroup="Save" Font-Size="XX-Small"
                                                runat="server" Width="84px" Text='<%# Bind("PAID_EXPS_AMT") %>'></asp:AISAmountTextbox>
                                            <asp:RequiredFieldValidator ID="reqSaveTotalPaidExp" runat="server" ErrorMessage="Please enter Total Paid Expense Amount."
                                                Text="*" ValidationGroup="Save" ControlToValidate="txtSaveTotalPaidExp"></asp:RequiredFieldValidator>
                                            <%--<ajaxToolkit:FilteredTextBoxExtender ID="fltrSaveTotalPaidExp" runat="server" TargetControlID="txtSaveTotalPaidExp"
                                                ValidChars="0123456789," FilterType="Custom" />--%>
                                            <%-- <asp:CompareValidator ID="CompareSaveTotalPaidExp" runat="server" ControlToValidate="txtSaveTotalPaidExp"
                                                                                ValueToCompare="0" ErrorMessage="Please enter Total Paid Excess Amount." Text="*" Operator="NotEqual"
                                                                                ValidationGroup="Save"></asp:CompareValidator>--%>
                                            <%--<cc1:MaskedEditExtender ID="MaskedEditSaveTotalPaidExp" runat="server" 
                                      TargetControlID="txtSaveTotalPaidExp" 
                                      Mask="99,999,999,999" MessageValidatorTip="true" MaskType="Number" DisplayMoney ="None" 
                                      OnFocusCssClass="MaskedEditFocus"  OnInvalidCssClass="MaskedEditError" 
                                      InputDirection="RightToLeft" AcceptNegative="None" AutoComplete="false"  />--%>
                                        </td>
                                        <td align="center">
                                            <asp:AISAmountTextbox ID="txtSaveTotalResrvIndem" ValidationGroup="Save" Font-Size="XX-Small"
                                                runat="server" Width="84px" Text='<%# Bind("RESRV_IDNMTY_AMT") %>'></asp:AISAmountTextbox>
                                            <asp:RequiredFieldValidator ID="reqSaveTotalResrvIndem" runat="server" ErrorMessage="Please enter Total Reserved Indemnity."
                                                Text="*" ValidationGroup="Save" ControlToValidate="txtSaveTotalResrvIndem"></asp:RequiredFieldValidator>
                                            <%--<ajaxToolkit:FilteredTextBoxExtender ID="fltrSaveTotalResrvIndem" runat="server"
                                                TargetControlID="txtSaveTotalResrvIndem" ValidChars="0123456789," FilterType="Custom" />--%>
                                            <%-- <asp:CompareValidator ID="CompareSaveTotalResrvIndem" runat="server" ControlToValidate="txtSaveTotalResrvIndem"
                                                                                ValueToCompare="0" ErrorMessage="Please enter Total Reserved Indemnity." Text="*" Operator="NotEqual"
                                                                                ValidationGroup="Save"></asp:CompareValidator>--%>
                                            <%--<cc1:MaskedEditExtender ID="MaskedEditSaveTotalResrvIndem" runat="server" 
                                      TargetControlID="txtSaveTotalResrvIndem" 
                                      Mask="99,999,999,999" MessageValidatorTip="true" MaskType="Number" DisplayMoney ="None" 
                                      OnFocusCssClass="MaskedEditFocus"  OnInvalidCssClass="MaskedEditError" 
                                      InputDirection="RightToLeft" AcceptNegative="None"  AutoComplete="false" />--%>
                                        </td>
                                        <td align="center">
                                            <asp:AISAmountTextbox ID="txtSaveTotalResrvExp" ValidationGroup="Save" Font-Size="XX-Small"
                                                runat="server" Width="84px" Text='<%# Bind("RESRV_EXPS_AMT") %>'></asp:AISAmountTextbox>
                                            <asp:RequiredFieldValidator ID="reqSaveTotalResrvExp" runat="server" ErrorMessage="Please enter Total Reserved Expense Amount."
                                                Text="*" ValidationGroup="Save" ControlToValidate="txtSaveTotalResrvExp"></asp:RequiredFieldValidator>
                                            <%--<ajaxToolkit:FilteredTextBoxExtender ID="fltrSaveTotalResrvExp" runat="server" TargetControlID="txtSaveTotalResrvExp"
                                                ValidChars="0123456789," FilterType="Custom" />--%>
                                            <%--  <asp:CompareValidator ID="CompareSaveTotalResrvExp" runat="server" ControlToValidate="txtSaveTotalResrvExp"
                                                                                ValueToCompare="0" ErrorMessage="Please Please enter Total Reserved Excess Amount." Text="*" Operator="NotEqual"
                                                                                ValidationGroup="Save"></asp:CompareValidator>--%>
                                            <%--<cc1:MaskedEditExtender ID="MaskedEditSaveTotalResrvExp" runat="server" 
                                      TargetControlID="txtSaveTotalResrvExp" 
                                      Mask="99,999,999,999" MessageValidatorTip="true" MaskType="Number" DisplayMoney ="None" 
                                      OnFocusCssClass="MaskedEditFocus"  OnInvalidCssClass="MaskedEditError" 
                                      InputDirection="RightToLeft" AcceptNegative="None" AutoComplete="false" />--%>
                                        </td>
                                        <td align="center">
                                            <asp:Label ID="lblSaveIncurred" runat="server" Font-Size="XX-Small"></asp:Label>
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
                                    <tr>
                                    </tr>
                                </InsertItemTemplate>
                                <ItemTemplate>
                                    <tr class="ItemTemplate">
                                        <td align="center">
                                            <asp:LinkButton ID="lnkEdit" CommandName="Edit" runat="server" Text="Edit" Font-Size="XX-Small"></asp:LinkButton>
                                        </td>
                                        <td align="center">
                                            <asp:HiddenField ID="hidPremAdJID" runat="server" Value='<%# Bind("PREM_ADJ_ID") %>' />
                                            <asp:HiddenField ID="hidAdjStatus" runat="server" Value='<%# Bind("ADJ_STATUS") %>' />
                                            <asp:HiddenField ID="hidComlAgmtID" runat="server" Value='<%# Bind("COML_AGMT_ID") %>' />
                                            <asp:LinkButton ID="lnkExcNonBil" runat="server" Font-Size="XX-Small" CommandName="SelectExcRow"
                                                CommandArgument='<%# Bind("ARMIS_LOS_ID") %>' Text='<%# Eval("EXC_NON_BIL")!= null ? "Yes" : "No" %>'
                                                Enabled='<%# Eval("ACTV_IND")%>'></asp:LinkButton>
                                        </td>
                                        <td align="center">
                                            <asp:Label ID="lblLOB" runat="server" Font-Size="XX-Small" Text='<%# Bind("POLICYSYMBOL") %>'></asp:Label>
                                        </td>
                                        <td align="center">
                                            <asp:Label ID="lblPolicy" runat="server" Font-Size="XX-Small" Text='<%# Bind("POLICY") %>'></asp:Label>
                                        </td>
                                        <td align="center">
                                            <asp:Label ID="lblState" runat="server" Font-Size="XX-Small" Text='<%# Bind("STATETYPE") %>'></asp:Label>
                                        </td>
                                        <td align="center">
                                            <asp:Label ID="lblTotalPaidIndem" runat="server" Font-Size="XX-Small" Text='<%# Eval("PAID_IDNMTY_AMT") != null ? (Eval("PAID_IDNMTY_AMT").ToString() != "" ?(decimal.Parse(Eval("PAID_IDNMTY_AMT").ToString())).ToString("#,##0") : "0"): "0"%>'></asp:Label>
                                        </td>
                                        <td align="center">
                                            <asp:Label ID="lblTotalPaidExp" runat="server" Font-Size="XX-Small" Text='<%# Eval("PAID_EXPS_AMT") != null ? (Eval("PAID_EXPS_AMT").ToString() != "" ?(decimal.Parse(Eval("PAID_EXPS_AMT").ToString())).ToString("#,##0") : "0"): "0"%>'></asp:Label>
                                        </td>
                                        <td align="center">
                                            <asp:Label ID="lblTotalResrvIndem" runat="server" Font-Size="XX-Small" Text='<%# Eval("RESRV_IDNMTY_AMT") != null ? (Eval("RESRV_IDNMTY_AMT").ToString() != "" ?(decimal.Parse(Eval("RESRV_IDNMTY_AMT").ToString())).ToString("#,##0") : "0"): "0"%>'></asp:Label>
                                        </td>
                                        <td align="center">
                                            <asp:Label ID="lblTotalResrvExp" runat="server" Font-Size="XX-Small" Text='<%# Eval("RESRV_EXPS_AMT") != null ? (Eval("RESRV_EXPS_AMT").ToString() != "" ?(decimal.Parse(Eval("RESRV_EXPS_AMT").ToString())).ToString("#,##0") : "0"): "0"%>'></asp:Label>
                                        </td>
                                        <td align="center">
                                            <asp:Label ID="lblIncurred" runat="server" Font-Size="XX-Small" Text='<%# Eval("Incurred") != null ? (Eval("Incurred").ToString() != "" ?(decimal.Parse(Eval("Incurred").ToString())).ToString("#,##0") : "0"): "0"%>'></asp:Label>
                                        </td>
                                        <td align="center">
                                            <asp:Label ID="lblSysGen" runat="server" Font-Size="XX-Small" Text='<%# Eval("SYS_GENRT_IND").ToString() == "True" ? "Yes" : "No" %>'></asp:Label>
                                        </td>
                                        <td align="center">
                                            <asp:Label ID="lblCpyLos" runat="server" Font-Size="XX-Small" Text='<%# Eval("COPY_IND").ToString() == "True" ? "Yes" : "No" %>'></asp:Label>
                                            
                                        </td>
                                        <td align="center">
                                            <asp:HiddenField ID="hidActive" runat="server" Value='<%# Bind("ACTV_IND")%>' />
                                            <asp:ImageButton ID="imgDisable" CommandArgument='<%# Bind("ARMIS_LOS_ID") %>' runat="server"
                                                ImageUrl='<%# Eval("ACTV_IND").ToString()=="True"?"~/images/disabled.GIF":"~/images/enabled.GIF" %>'>
                                            </asp:ImageButton>
                                        </td>
                                        <td align="center">
                                            <asp:LinkButton ID="lnkSelect" CommandArgument='<%# Bind("ARMIS_LOS_ID") %>' CommandName="SelectRow"
                                                runat="server" Text="Details" Font-Size="XX-Small"></asp:LinkButton>
                                        </td>
                                    </tr>
                                    <tr>
                                    </tr>
                                </ItemTemplate>
                            </asp:AISListView>
                        </asp:Panel>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
