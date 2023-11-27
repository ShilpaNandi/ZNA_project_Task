<%--/*-----	Page:	PolicyInfo
-----
-----	Created:		CSC (Venkata Kolimi)

-----
-----	Description:	This page is used to create/Modify policies for a particular program period.
-----
-----	On Exit:	
-----			
-----
-----   Created Date : 2/16/09 (AS part of Retro Project)

-----	Modified:	MM/DD/YY	first & last name of modifier
-----			- Description of Modification

-----               06/16/09	Zakir Hussain
-----				Code modified in Policy Info to add a new Text Amount field NY Premium discount.

*/--%>

<%@ Page Language="C#" MasterPageFile="~/Retro.master" AutoEventWireup="true" Inherits="ZurichNA.AIS.WebSite.AcctSetup.PolicyInfo"
    Title="Policy Information" CodeBehind="PolicyInfo.aspx.cs" EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/App_Shared/AccountInfoHeader.ascx" TagName="MasterValues" TagPrefix="MV" %>
<%@ Register Src="~/App_Shared/ProgramPeriod.ascx" TagName="ProgramPeriod" TagPrefix="PP" %>
<asp:Content ID="Content2" ContentPlaceHolderID="HeaderPlaceHolder" runat="Server">
    <style type="text/css">
        .modalBackground
        {
            background-color: Gray;
            filter: alpha(opacity=70);
            opacity: 0.7;
        }
        .modalPopup
        {
            width: 250px;
        }
    </style>
    <table width="910" style="text-align: right">
        <tr>
            <td align="left">
                <asp:Label ID="GAILabel" runat="server" Text="Policy Information" CssClass="h1"></asp:Label>
            </td>
            <td align="right">
                <asp:Button ID="btnAdd" runat="server" Text="Add New" Enabled="false" ToolTip="Please click here to add new Policy Information"
                    OnClick="btnAdd_Click" />
            </td>
        </tr>
    </table>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="MainPlaceHolder" runat="Server">

<script type="text/javascript" src="../JavaScript/RetroScript.js"></script>
<script language="javascript" type="text/javascript">
    var scrollTop1;

    if (Sys.WebForms.PageRequestManager != null) {
        Sys.WebForms.PageRequestManager.getInstance().add_beginRequest(BeginRequestHandler);
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);
    }

    function BeginRequestHandler(sender, args) {
        var mef = $get('<%=pnlPolicyInfo.ClientID%>');
        if (mef != null)
            scrollTop1 = mef.scrollTop;
    }

    function EndRequestHandler(sender, args) {
        var mef = $get('<%=pnlPolicyInfo.ClientID%>');
        if (mef != null)
            mef.scrollTop = scrollTop1;
    }

    // Declaring valid date character, minimum year and maximum year
    var dtCh = "/";
    var minYear = 1900;
    var maxYear = 2100;

    function isInteger(s) {
        var i;
        for (i = 0; i < s.length; i++) {
            // Check that current character is number.
            var c = s.charAt(i);
            if (((c < "0") || (c > "9"))) return false;
        }
        // All characters are numbers.
        return true;
    }

    function stripCharsInBag(s, bag) {
        var i;
        var returnString = "";
        // Search through string's characters one by one.
        // If character is not in bag, append to returnString.
        for (i = 0; i < s.length; i++) {
            var c = s.charAt(i);
            if (bag.indexOf(c) == -1) returnString += c;
        }
        return returnString;
    }

    function daysInFebruary(year) {
        // February has 29 days in any year evenly divisible by four,
        // EXCEPT for centurial years which are not also divisible by 400.
        return (((year % 4 == 0) && ((!(year % 100 == 0)) || (year % 400 == 0))) ? 29 : 28);
    }
    function DaysArray(n) {
        for (var i = 1; i <= n; i++) {
            this[i] = 31
            if (i == 4 || i == 6 || i == 9 || i == 11) { this[i] = 30 }
            if (i == 2) { this[i] = 29 }
        }
        return this
    }

    function isDate(dtStr) {
        var daysInMonth = DaysArray(12)
        var pos1 = dtStr.indexOf(dtCh)
        var pos2 = dtStr.indexOf(dtCh, pos1 + 1)
        var strMonth = dtStr.substring(0, pos1)
        var strDay = dtStr.substring(pos1 + 1, pos2)
        var strYear = dtStr.substring(pos2 + 1)
        strYr = strYear
        if (strDay.charAt(0) == "0" && strDay.length > 1) strDay = strDay.substring(1)
        if (strMonth.charAt(0) == "0" && strMonth.length > 1) strMonth = strMonth.substring(1)
        for (var i = 1; i <= 3; i++) {
            if (strYr.charAt(0) == "0" && strYr.length > 1) strYr = strYr.substring(1)
        }
        month = parseInt(strMonth)
        day = parseInt(strDay)
        year = parseInt(strYr)
        if (pos1 == -1 || pos2 == -1) {
            //alert("The date format should be : mm/dd/yyyy")
            return false
        }
        if (strMonth.length < 1 || month < 1 || month > 12) {
            //alert("Please enter a valid month")
            return false
        }
        if (strDay.length < 1 || day < 1 || day > 31 || (month == 2 && day > daysInFebruary(year)) || day > daysInMonth[month]) {
            //alert("Please enter a valid day")
            return false
        }
        if (strYear.length != 4 || year == 0 || year < minYear || year > maxYear) {
            //alert("Please enter a valid 4 digit year between "+minYear+" and "+maxYear)
            return false
        }
        if (dtStr.indexOf(dtCh, pos2 + 1) != -1 || isInteger(stripCharsInBag(dtStr, dtCh)) == false) {
            //alert("Please enter a valid date")
            return false
        }
        return true
    }
    function ValidateEffDate(oSource, oArgs) {
        var effdt = document.getElementById('<%=txtPolicyEff.ClientID%>').value;
        var expdt = document.getElementById('<%=txtPolicyExp.ClientID%>').value;
        var CompareEffExpDate = document.getElementById('<%=CompareEffExpDate.ClientID%>');
        var compareEfftoProPreEff = document.getElementById('<%=compareEfftoProPreEff.ClientID%>');
        var compareEfftoProPreExp = document.getElementById('<%=compareEfftoProPreExp.ClientID%>');
        var compareExptoProPreEff = document.getElementById('<%=compareExptoProPreEff.ClientID%>');
        var compareExptoProPreExp = document.getElementById('<%=compareExptoProPreExp.ClientID%>');
        //var CustEffDate=document.getElementById('<%=CustEffDate.ClientID%>');
        var EffFlag = isDate(effdt)
        var ExpFlag = isDate(expdt)
        if ((effdt != "") && (effdt != "__/__/____")) {
            if ((EffFlag == true) && (ExpFlag == true))
            { var flag = true; }
            else
            { var flag = false; }
        }
        else {
            var flag = false;
            var EffFlag = true;
        }
        ValidatorEnable(CompareEffExpDate, flag);
        ValidatorEnable(compareEfftoProPreEff, flag);
        ValidatorEnable(compareEfftoProPreExp, flag);
        ValidatorEnable(compareExptoProPreEff, flag);
        ValidatorEnable(compareExptoProPreExp, flag);
        oArgs.IsValid = EffFlag;

    }
    function ValidateExpDate(oSource, oArgs) {
        var effdt = document.getElementById('<%=txtPolicyEff.ClientID%>').value;
        var expdt = document.getElementById('<%=txtPolicyExp.ClientID%>').value;
        var CompareEffExpDate = document.getElementById('<%=CompareEffExpDate.ClientID%>');
        var compareEfftoProPreEff = document.getElementById('<%=compareEfftoProPreEff.ClientID%>');
        var compareEfftoProPreExp = document.getElementById('<%=compareEfftoProPreExp.ClientID%>');
        var compareExptoProPreEff = document.getElementById('<%=compareExptoProPreEff.ClientID%>');
        var compareExptoProPreExp = document.getElementById('<%=compareExptoProPreExp.ClientID%>');
        var EffFlag = isDate(effdt)
        var ExpFlag = isDate(expdt)
        if ((expdt != "") && (expdt != "__/__/____")) {
            if ((EffFlag == true) && (ExpFlag == true))
            { var flag = true }
            else
            { var flag = false }
        }
        else {
            var flag = false;
            var ExpFlag = true;
        }
        ValidatorEnable(CompareEffExpDate, flag);
        ValidatorEnable(compareEfftoProPreEff, flag);
        ValidatorEnable(compareEfftoProPreExp, flag);
        ValidatorEnable(compareExptoProPreEff, flag);
        ValidatorEnable(compareExptoProPreExp, flag);
        oArgs.IsValid = ExpFlag;
    }

    function EnableDisableALAECappedAmt() {
        var oDDL = document.all('<%=ddlALAE.ClientID%>');
        var ddlText = oDDL.options[oDDL.selectedIndex].text;

        if (ddlText == "Capped Amount")
            document.getElementById('<%=txtALAECAPPED.ClientID%>').disabled = false;
        else {
            document.getElementById('<%=txtALAECAPPED.ClientID%>').disabled = true;
            document.getElementById('<%=txtALAECAPPED.ClientID%>').value = "0";
        }
    }
    function EnableTxtDedPolLimAmt() {
        var flag = document.getElementById('<%=chkUnLimDedPolLim.ClientID%>').checked;

        if (flag == false)
            document.getElementById('<%=txtDedPolLimAmt.ClientID%>').disabled = false;
        else {
            document.getElementById('<%=txtDedPolLimAmt.ClientID%>').value = "0";
            document.getElementById('<%=txtDedPolLimAmt.ClientID%>').disabled = true;
        }
    }
    function EnableTxtOvrDedLimAmt() {
        var flag = document.getElementById('<%=chkUnLimOvrDedLim.ClientID%>').checked;

        if (flag == false)
            document.getElementById('<%=txtOvrDedLimAmt.ClientID%>').disabled = false;
        else {
            document.getElementById('<%=txtOvrDedLimAmt.ClientID%>').value = "0";
            document.getElementById('<%=txtOvrDedLimAmt.ClientID%>').disabled = true;
        }

    }

    function EnableTxtLDFIBNR() {
        var flag = document.getElementById('<%=chkLDFIBNRStepped.ClientID%>').checked;

        if (flag == false) {
            document.getElementById('<%=txtIBNR.ClientID%>').disabled = false;
            document.getElementById('<%=txtLDF.ClientID%>').disabled = false;
        }
        else {
            document.getElementById('<%=txtIBNR.ClientID%>').disabled = true;
            document.getElementById('<%=txtLDF.ClientID%>').disabled = true;
            document.getElementById('<%=txtIBNR.ClientID%>').value = "";
            document.getElementById('<%=txtLDF.ClientID%>').value = "";
        }
    }
    function EnablechkTPADirect() {
        var flag = document.getElementById('<%=chkTPA.ClientID%>').checked;

        if (flag == true)
            document.getElementById('<%=chkTPADirect.ClientID%>').disabled = false;
        else
            document.getElementById('<%=chkTPADirect.ClientID%>').disabled = true;
    }

    function ValidateEffExpDates() {
        var PolEff = document.getElementById('<%=txtPolicyEff.ClientID%>').value;
        var ProPreEff = document.getElementById('<%=txtProgramPeriodEffDate.ClientID%>').value;
        var PolExp = document.getElementById('<%=txtPolicyExp.ClientID%>').value;
        var ProPreExp = document.getElementById('<%=txtProgramPeriodExpDate.ClientID%>').value;

        var PolEff = new Date(PolEff);
        var ProPreEff = new Date(ProPreEff);
        var PolExp = new Date(PolExp);
        var ProPreExp = new Date(ProPreExp);

        if (PolEff < ProPreEff)
            return false;
        else if (PolEff > ProPreExp)
            return false;
        else
            return true;

    }

    function DisableOtherFactor(txtLDFFactor, txtIBNRFactor) {

        if (((txtLDFFactor.value == "") || (txtLDFFactor.value == "_.______")) && ((txtIBNRFactor.value == "") || (txtIBNRFactor.value == "_.______"))) {
            txtLDFFactor.disabled = false;
            txtIBNRFactor.disabled = false;
        }
        else if ((txtLDFFactor.value != "") && (txtLDFFactor.value != "_.______")) {
            txtLDFFactor.disabled = false;
            txtIBNRFactor.disabled = true;
        }
        else if ((txtIBNRFactor.value != "") && (txtIBNRFactor.value != "_.______")) {
            txtLDFFactor.disabled = true;
            txtIBNRFactor.disabled = false;
        }
    }
    function disableTextBox(chkLDFIBNRStepped) {
        document.getElementById('<%=txtLDF.ClientID%>').disabled = chkLDFIBNRStepped.checked;
        document.getElementById('<%=txtIBNR.ClientID%>').disabled = chkLDFIBNRStepped.checked;
    }
    function CheckSymbolLength() {
        var txtPolicySymbol = document.getElementById('<%=txtPolicySymbol.ClientID%>').value;
        var regPolicySymbol = document.getElementById('<%=regPolicySymbol.ClientID%>');
        if (txtPolicySymbol.length >= 2) {
            ValidatorEnable(regPolicySymbol, false);
        }
        else {
            ValidatorEnable(regPolicySymbol, true);
        }
    }
    function DisablePolicyOtherFactor() {
        var txtLDFFactor = document.getElementById('<%=txtLDF.ClientID%>');
        var txtIBNRFactor = document.getElementById('<%=txtIBNR.ClientID%>');

        if (((txtLDFFactor.value == "") || (txtLDFFactor.value == "_.______")) && ((txtIBNRFactor.value == "") || (txtIBNRFactor.value == "_.______"))) {
            txtLDFFactor.disabled = false;
            txtIBNRFactor.disabled = false;
        }
        else if ((txtLDFFactor.value != "") && (txtLDFFactor.value != "_.______")) {
            txtLDFFactor.disabled = false;
            txtIBNRFactor.disabled = true;
        }
        else if ((txtIBNRFactor.value != "") && (txtIBNRFactor.value != "_.______")) {
            txtLDFFactor.disabled = true;
            txtIBNRFactor.disabled = false;
        }
    }
    </script>


    <asp:HiddenField ID="hidPopShow" runat="server" />
    <asp:HiddenField ID="hodPopOk" runat="server" />
    <cc1:ModalPopupExtender runat="server" ID="modalPolDetails" TargetControlID="hidPopShow"
        PopupControlID="pnlPolDetails" BackgroundCssClass="modalBackground" DropShadow="true"
         OkControlID="hodPopOk">
    </cc1:ModalPopupExtender>
    <div style="float: left;">
        <asp:UpdatePanel ID="updPolicyUpload" runat="server">
            <ContentTemplate>
                 
                <asp:Panel runat="server" CssClass="modalPopup" ID="pnlPolDetails" Style="border: solid 1px black;
                    display: none; width: 650px; padding: 0px" HorizontalAlign="Center">
                    <asp:Panel runat="Server" ID="Panel3" Style="width: 100%; cursor: move; padding: 0px;
                        background-color: #CCCCCC; height: 20px; border: solid 1px Gray; color: Black;
                        text-align: center; vertical-align: middle; font-weight: bold; vertical-align: middle;
                        color: black; font-size: 12px">
                        Policy Upload
                    </asp:Panel>
                    <div style="text-align: center; width: 100%; padding-bottom: 5px; background-color: White;">
                        <table align="center">
                            <tr>
                                <td>
                                    <br />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lblErrorLog" runat="server" Visible="True"></asp:Label>
                                    <%--Upload completed successfully. Please <a runat="server" id="lnkErrorLog">Click Here</a> for Error log--%>
                                    <br />
                                    <br />
                                </td>
                            </tr>
                            <tr>
                                <td align="Center">
                                    <asp:FileUpload ID="flUpload" runat="server" EnableViewState="false" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <br />
                                </td>
                            </tr>
                            <tr>
                                <td align="Center">
                                    <asp:Button ID="btnPopUpLoad" runat="server" Text="Upload" OnClick="PolicyUpload_Click" />
                                    &nbsp;&nbsp;&nbsp;
                                    <asp:Button ID="btnPopCancel" runat="server" Text="Cancel" OnClick="btnPopCancel_Click" />
                                    &nbsp;&nbsp;&nbsp;
                                    <ContentTemplate>
                                   <asp:Button ID="btnErrorLog_Policy" runat="server" Text="Error Log" OnClick="btnErrorLog_PolicyClick"  Visible="false"/>
                                       </ContentTemplate>
                                    <asp:HiddenField ID="hidForModel" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <br />
                                    <br />
                                </td>
                            </tr>
                            <tr>
                                <td align="Center">
                                    <asp:Button ID="btnViewPolicies" runat="server" Text="View Policies" OnClick="btnViewPolicies_Click" />
                                    &nbsp;&nbsp;&nbsp;
                                    <asp:Button ID="btnViewFactors" runat="server" Text="View Stepped Factors" OnClick="btnViewFactors_Click"/>
                                </td>
                            </tr>
                        </table>
                    </div>
                </asp:Panel>
            </ContentTemplate>
            <Triggers>
                <asp:PostBackTrigger ControlID="btnPopUpLoad" />
                <asp:PostBackTrigger ControlID="btnErrorLog_Policy" />
                <asp:PostBackTrigger ControlID="btnViewPolicies" />
                <asp:PostBackTrigger ControlID="btnViewFactors"/>
<%--                <asp:AsyncPostBackTrigger ControlID="btnPopCancel" EventName="Click" />--%>
            </Triggers>
        </asp:UpdatePanel>
    </div>

    <asp:HiddenField ID="hidSFPopShow" runat="server" />
    <asp:HiddenField ID="hodSFPopOk" runat="server" />

    <cc1:ModalPopupExtender runat="server" ID="mpeStepped" TargetControlID="hidSFPopShow"
                PopupControlID="pnlSteppedFactorPop" BackgroundCssClass="modalBackground" DropShadow="true"
                 OkControlID="hodSFPopOk">
            </cc1:ModalPopupExtender>
            <div style="float: left;">
                <asp:Panel runat="server" CssClass="modalPopup" ID="pnlSteppedFactorPop" Style="border: solid 1px black;
                    display: none; width: 650px; padding: 0px" HorizontalAlign="Center">
                    <asp:Panel runat="Server" ID="Panel2" Style="width: 100%; cursor: move; padding: 0px;
                        background-color: #CCCCCC; height: 20px; border: solid 1px Gray; color: Black;
                        text-align: center; vertical-align: middle; font-weight: bold; vertical-align: middle;
                        color: black; font-size: 12px">
                        Stepped Factor Upload
                    </asp:Panel>
                    <div style="text-align: center; width: 100%; padding-bottom: 5px; background-color: White;">
                        <table align="center">
                            <tr>
                                <td>
                                    <br />
                                </td>
                            </tr>
                             <tr>
                                <td>
                                    <asp:Label ID="lblSFErrorMessage" runat="server" Visible="False"></asp:Label>
                                    <%--Upload completed successfully. Please <a runat="server" id="lnkErrorLog">Click Here</a> for Error log--%>
                                    <br />
                                    <br />
                                </td>
                            </tr>
                            <tr>
                                <td align="Center">
                                    <asp:FileUpload ID="fleStepped" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <br />
                                </td>
                            </tr>
                            <tr>
                                <td align="Center">
                                    <asp:Button ID="btnSFUpdate" runat="server" Text="Upload" OnClick="btnSFUpdate_Click" />
                                    &nbsp;&nbsp;&nbsp;
                                    <asp:Button ID="btnSFCancel" runat="server" Text="Cancel" OnClick="btnSFCancel_Click"/>
                                        &nbsp;&nbsp;&nbsp;
                                    <ContentTemplate>
                                   <asp:Button ID="btnSFErrorLog" runat="server" Text="Error Log" OnClick="btnSFErrorLog_Click"  Visible="false"/>
                                       </ContentTemplate>

                                </td>
                            </tr>
                        </table>
                    </div>
                </asp:Panel>
            </div>

    <asp:UpdatePanel ID="udpPolicy" runat="server">
        <ContentTemplate>
            <asp:ValidationSummary ID="VSSaveDEP" runat="server" ValidationGroup="DEPSAVE" />
            <asp:ValidationSummary ID="VSEditDEP" runat="server" ValidationGroup="DEPEDIT" />
            <asp:ValidationSummary ID="VSSaveStepped" runat="server" ValidationGroup="STEPPEDSAVE" />
            <asp:ValidationSummary ID="VSEditStepped" runat="server" ValidationGroup="STEPPEDEDIT" />
            <asp:ValidationSummary ID="VSSave" runat="server" ValidationGroup="SAVE" />
            <MV:MasterValues ID="MVMasterValues" runat="server" />
            <!--Program Periods -->
            <asp:Label ID="lblProgramPeriodsHeader" runat="server" CssClass="h2" Text="Program Periods"></asp:Label>
            <asp:Panel ID="PnlProgramperiod" Height="80px" Width="910px" runat="server" ScrollBars="Auto">
                <PP:ProgramPeriod ID="ppProgramPeriod" runat="server" />
            </asp:Panel>
            <asp:HiddenField ID="hidSelProgramPeriod" runat="server" />
            <asp:Label ID="lblPolicyInfo" Visible="false" runat="server" Text="Policy Information"
                CssClass="h2"></asp:Label>
            &nbsp; &nbsp;
            <asp:Label ID="lblDateRange" runat="server" Width="250px" CssClass="h2" />
            &nbsp; &nbsp;&nbsp; &nbsp;
            <asp:Button ID="btnUpload" Text="Policy Upload" runat="server" Width="100px" OnClick="btnUpload_Click"
                Visible="false" />
            <asp:Button ID="btnSFUpload" Text="Stepped Factor Upload" runat="server" Width="143px" OnClick="btnSFUpload_Click"
                Visible="false" />
            
            <asp:Panel ID="pnlPolicyInfo" runat="server" Width="910px" Visible="false" Height="80px"
                ScrollBars="Auto">
                <asp:AISListView ID="lstPolicyInfo" runat="server" OnSelectedIndexChanging="lstPolicyInfo_SelectedIndexChanging"
                    OnDataBound="lstPolicyInfo_RowDataBound" OnItemCommand="lstPolicyInfo_ItemCommand"
                    OnItemDataBound="lstPolicyInfo_ItemDataBound" OnSorting="lstPolicyInfo_Sorting">
                    <EmptyDataTemplate>
                        <table id="tblPolicyInfo" class="panelContents" runat="server" width="98%">
                            <tr class="LayoutTemplate">
                                <th>
                                    Policy #
                                </th>
                                <th>
                                    Policy Eff.
                                </th>
                                <th>
                                    Policy Exp.
                                </th>
                                <th>
                                    Adjustment Type
                                </th>
                                <th>
                                    Step IBNR/LDF Details
                                </th>
                                <%--  <th>
                                    DEP Info
                                </th>--%>
                                <th>
                                    Disable
                                </th>
                            </tr>
                        </table>
                        <table width="98%">
                            <tr id="Tr1" runat="server" class="ItemTemplate">
                                <td align="center">
                                    <asp:Label ID="lblEmptyMessage" Text="--- No Records Found ---" Font-Bold="true"
                                        Width="600px" runat="server" Style="text-align: center" />
                                </td>
                            </tr>
                        </table>
                    </EmptyDataTemplate>
                    <LayoutTemplate>
                        <table id="tblPolicyInfo" class="panelContents" border="0" runat="server" width="98%">
                            <tr class="LayoutTemplate">
                                <th>
                                </th>
                                <th>
                                    Policy #
                                </th>
                                <th>
                                    <asp:LinkButton ID="SortByPolicyEffDt" runat="server" CommandArgument="PolicyEffDt"
                                        CommandName="Sort" Text="Policy Eff" />
                                    <asp:Image ID="imgSortByPolicyEffDt" runat="server" ImageUrl="~/images/descending.gif"
                                        ToolTip="Descending" Visible="false" />
                                </th>
                                <th>
                                    Policy Exp.
                                </th>
                                <th>
                                    <asp:LinkButton ID="SortByAdjType" runat="server" CommandArgument="AdjType" CommandName="Sort"
                                        Text="Adjustment Type" />
                                    <asp:Image ID="imgSortByAdjType" runat="server" ImageUrl="~/images/Ascending.gif"
                                        ToolTip="Ascending" Visible="false" />
                                </th>
                                <th>
                                    Step IBNR/LDF Details
                                </th>
                                <%-- <th>
                                    DEP Info
                                </th>--%>
                                <th>
                                    Disable
                                </th>
                            </tr>
                            <tr id="itemPlaceholder" runat="server">
                            </tr>
                        </table>
                    </LayoutTemplate>
                    <ItemTemplate>
                        <tr id="trItemTemplate" runat="server" class="ItemTemplate">
                            <td>
                                <asp:LinkButton ID="lbSelect" runat="server" Text="Select" CommandArgument='<% # Bind("PolicyID")%>'
                                    CommandName="Select" />
                            </td>
                            <td>
                                <asp:Label ID="lblPolicy" runat="server" Visible="false" Text='<% # Bind("PolicyID")%>'></asp:Label>
                                <%# Eval("PolicySymbol").ToString() + " " + Eval("PolicyNumber").ToString() + " " + Eval("PolicyModulus").ToString()%>
                            </td>
                            <td>
                                <%# Eval("PolicyEffectiveDate", "{0:d}")%>
                            </td>
                            <td>
                                <%# Eval("PlanEndDate", "{0:d}")%>
                            </td>
                            <td>
                                <%# Eval("AdjustmentTypeName")%>
                            </td>
                            <td>
                                <asp:Label ID="lblSteppedcheck" runat="server" Text='<%# Bind("LDFIncurredNO63740") %>'
                                    Visible="false" />
                                <asp:LinkButton ID="lbSteppedFactor" runat="server" Text="Stepped Factor" OnCommand='SteppedFactor'
                                    CommandArgument='<% # Bind("PolicyID")%>' Enabled='<%# Bind("IsActive") %>' />
                            </td>
                            <%-- <td>
                                <asp:LinkButton ID="lbDEPPolicy" runat="server" Text="DEP Policy" OnCommand='DEPPolicy'
                                    CommandArgument='<% # Bind("PolicyID")%>' Enabled='<%# Bind("IsActive") %>' />
                            </td>--%>
                            <td>
                                <asp:Label ID="lblPolicyActive" runat="server" Text='<%# Bind("IsActive") %>' Visible="false"></asp:Label>
                                <asp:ImageButton ID="imgPolicyEnableDisable" runat="server" CommandArgument='<%# Bind("PolicyID") %>' />
                            </td>
                        </tr>
                    </ItemTemplate>
                    <AlternatingItemTemplate>
                        <tr id="trItemTemplate" runat="server" class="AlternatingItemTemplate">
                            <td>
                                <asp:LinkButton ID="lbSelect" runat="server" Text="Select" CommandArgument='<% # Bind("PolicyID")%>'
                                    CommandName="Select" />
                            </td>
                            <td>
                                <asp:Label ID="lblPolicy" runat="server" Visible="false" Text='<% # Bind("PolicyID")%>'></asp:Label>
                                <%# Eval("PolicySymbol").ToString() + " " + Eval("PolicyNumber").ToString() + " " + Eval("PolicyModulus").ToString()%>
                            </td>
                            <td>
                                <%# Eval("PolicyEffectiveDate", "{0:d}")%>
                            </td>
                            <td>
                                <%# Eval("PlanEndDate", "{0:d}")%>
                            </td>
                            <td>
                                <%# Eval("AdjustmentTypeName")%>
                            </td>
                            <td>
                                <asp:Label ID="lblSteppedcheck" runat="server" Text='<%# Bind("LDFIncurredNO63740") %>'
                                    Visible="false" />
                                <asp:LinkButton ID="lbSteppedFactor" runat="server" Text="Stepped Factor" OnCommand='SteppedFactor'
                                    CommandArgument='<% # Bind("PolicyID")%>' />
                            </td>
                            <%-- <td>
                                <asp:LinkButton ID="lbDEPPolicy" runat="server" Text="DEP Policy" OnCommand='DEPPolicy'
                                    CommandArgument='<% # Bind("PolicyID")%>' />
                            </td>--%>
                            <td>
                                <asp:Label ID="lblPolicyActive" runat="server" Text='<%# Bind("IsActive") %>' Visible="false"></asp:Label>
                                <asp:ImageButton ID="imgPolicyEnableDisable" runat="server" CommandArgument='<%# Bind("PolicyID") %>' />
                            </td>
                        </tr>
                    </AlternatingItemTemplate>
                </asp:AISListView>
            </asp:Panel>
            <asp:Label ID="lblPolicyDetails" runat="server" CssClass="h2" Text="Policy Details"
                Visible="false"></asp:Label>
            &nbsp;<asp:LinkButton ID="lbClosePolicyDetails" Text="Close" runat="server" OnClick="lbClosePolicyDetails_Click"
                Visible="False" />
            <asp:Panel ID="pnlDetails" runat="server" BorderColor="Black" BorderWidth="1px" Visible="false">
                <table width="100%" id="Table1" runat="server">
                    <tr>
                        <td>
                            Policy #:
                        </td>
                        <td>
                            <asp:TextBox ID="txtPolicySymbol" Width="35px" MaxLength="3" runat="server" ValidationGroup="SAVE"
                                onkeypress="return toUpperCase(event,this)" onblur="CheckSymbolLength()" />
                            <cc1:FilteredTextBoxExtender TargetControlID="txtPolicySymbol" FilterType="LowercaseLetters,UppercaseLetters"
                                ID="ftePolicySymbol" runat="server" />
                            <asp:RequiredFieldValidator ID="RequiredPolicySymbol" runat="server" ControlToValidate="txtPolicySymbol"
                                ErrorMessage="Please Enter Policy Symbol" Text="*" Display="Dynamic" ValidationGroup="SAVE" />
                            <asp:RegularExpressionValidator ID="regPolicySymbol" runat="server" ControlToValidate="txtPolicySymbol"
                                Text="*" Display="Dynamic" ValidationGroup="SAVE" ErrorMessage="Please Enter minimum two alpha characters for Policy Symbol"
                                ValidationExpression=".{2}.*" Enabled="false" />
                            <asp:TextBox ID="txtPolicyNumber" Width="60px" MaxLength="8" runat="server" />
                            <asp:RequiredFieldValidator ID="requiredPolicyNumber" runat="server" ControlToValidate="txtPolicyNumber"
                                ErrorMessage="Please Enter Policy Number" Text="*" Display="Dynamic" ValidationGroup="SAVE" />
                            <asp:RegularExpressionValidator ID="regularPolicyNumber" runat="server" ControlToValidate="txtPolicyNumber"
                                Text="*" Display="Dynamic" ValidationGroup="SAVE" ErrorMessage="Please Enter minimum seven alphanumeric digits for Policy Number"
                                ValidationExpression=".{7}.*" />
                            <cc1:FilteredTextBoxExtender TargetControlID="txtPolicyNumber" FilterType="Numbers,LowercaseLetters,UppercaseLetters"
                                ID="ftePolicyNumber" runat="server" />
                            <asp:TextBox ID="txtPolicyMod" Width="20px" MaxLength="2" runat="server" />
                        </td>
                        <td style="vertical-align: middle">
                            <asp:RequiredFieldValidator ID="requiredPolicyModulus" runat="server" ControlToValidate="txtPolicyMod"
                                ErrorMessage="Please Enter Policy Module" Text="*" Display="Dynamic" ValidationGroup="SAVE" />
                            <cc1:FilteredTextBoxExtender TargetControlID="txtPolicyMod" FilterType="Numbers,LowercaseLetters,UppercaseLetters"
                                ID="ftePolicyModulus" runat="server" />
                            <asp:RegularExpressionValidator ID="RegularPolicyMod" runat="server" ControlToValidate="txtPolicyMod"
                                Text="*" Display="Dynamic" ValidationGroup="SAVE" ErrorMessage="Please Enter minimum two alphanumeric characters for Policy Module"
                                ValidationExpression=".{2}.*" />
                        </td>
                        <td>
                            Policy Effective:
                        </td>
                        <td>
                            <asp:TextBox ID="txtPolicyEff" runat="server" Width="65px" ValidationGroup="SAVE"></asp:TextBox>
                            <asp:ImageButton ID="imgPolicyEff" runat="server" ImageUrl="~/images/Calendar_scheduleHS.png"
                                CausesValidation="False" />
                            <cc1:MaskedEditExtender ID="mskEffDate" runat="server" AcceptNegative="Left" DisplayMoney="Left"
                                ErrorTooltipEnabled="True" Mask="99/99/9999" MaskType="Date" MessageValidatorTip="true"
                                OnFocusCssClass="MaskedEditFocus" OnInvalidCssClass="MaskedEditError" TargetControlID="txtPolicyEff" />
                            <cc1:CalendarExtender ID="CalendarExtender1" runat="server" PopupButtonID="imgPolicyEff"
                                TargetControlID="txtPolicyEff" />
                            <asp:TextBox ID="txtProgramPeriodEffDate" runat="server" CssClass="Hide" />
                            <%--<asp:CustomValidator ID="CustEffExpDates" runat="server" ErrorMessage="Effective Date must be Greater than or Equal ProgramPeriod Effective Date, Please Enter Valid Effective Date"
                        ClientValidationFunction="ValidateEffExpDates()" ValidationGroup="SAVE" Display="Dynamic"
                        Text="*" />--%>
                        </td>
                        <td style="vertical-align: middle">
                            <asp:RequiredFieldValidator ID="RequiredEffectiveDate" runat="server" ControlToValidate="txtPolicyEff"
                                ErrorMessage="Please Enter Policy Effective Date" Text="*" Display="Dynamic"
                                ValidationGroup="SAVE" />
                            <asp:CustomValidator ID="CustEffDate" runat="server" ErrorMessage="Please Enter valid Policy Effective Date"
                                ClientValidationFunction="ValidateEffDate" ValidationGroup="SAVE" Display="Dynamic"
                                Text="*" />
                            <%--<asp:CompareValidator ID="compareEffDateFormat" runat="server" ControlToValidate="txtPolicyEff"
                                Display="Dynamic" ErrorMessage="Please enter valid Policy Effective Date " Text="*"
                                Operator="DataTypeCheck" Type="Date" ValidationGroup="SAVE" />--%>
                            <asp:CompareValidator ID="CompareEffExpDate" runat="server" ControlToValidate="txtPolicyEff"
                                Display="Dynamic" ErrorMessage="Policy Effective Date must be less than Policy  Expiration Date, Please enter a valid Effective Date"
                                Text="*" Operator="LessThanEqual" Type="Date" ControlToCompare="txtPolicyExp"
                                ValidationGroup="SAVE" />
                            <asp:CompareValidator ID="compareEfftoProPreEff" runat="server" ControlToValidate="txtPolicyEff"
                                Display="Dynamic" ErrorMessage="Policy Effective Date must be greater than or equal to Program Period Effective Date. Please enter a valid Policy Effective Date"
                                Text="*" Operator="GreaterThanEqual" Type="Date" ControlToCompare="txtProgramPeriodEffDate"
                                ValidationGroup="SAVE" />
                            <asp:CompareValidator ID="compareEfftoProPreExp" runat="server" ControlToValidate="txtPolicyEff"
                                Display="Dynamic" ErrorMessage="Policy Effective Date must be less than or equal to Program Period Expiration Date. Please enter a valid Policy Effective Date"
                                Text="*" Operator="LessThanEqual" Type="Date" ControlToCompare="txtProgramPeriodExpDate"
                                ValidationGroup="SAVE" />
                        </td>
                        <td>
                            Policy Expire:
                        </td>
                        <td>
                            <asp:TextBox ID="txtPolicyExp" runat="server" Width="65px" ValidationGroup="SAVE"></asp:TextBox>
                            <asp:ImageButton ID="imgPolicyExp" runat="server" ImageUrl="~/images/Calendar_scheduleHS.png"
                                CausesValidation="False" />
                            <cc1:MaskedEditExtender ID="MaskedEditExtender6" runat="server" TargetControlID="txtPolicyExp"
                                Mask="99/99/9999" MessageValidatorTip="true" OnFocusCssClass="MaskedEditFocus"
                                OnInvalidCssClass="MaskedEditError" MaskType="Date" DisplayMoney="Left" AcceptNegative="Left"
                                ErrorTooltipEnabled="True" />
                            <cc1:CalendarExtender ID="CalendarExtender12" runat="server" TargetControlID="txtPolicyExp"
                                PopupButtonID="imgPolicyExp" />
                            <asp:TextBox ID="txtProgramPeriodExpDate" runat="server" CssClass="Hide" />
                        </td>
                        <td style="vertical-align: middle">
                            <asp:RequiredFieldValidator ID="RequiredExpirationDate" runat="server" ControlToValidate="txtPolicyExp"
                                ErrorMessage="Please Enter Policy Expiry Date" Text="*" Display="Dynamic" ValidationGroup="SAVE" />
                            <asp:CustomValidator ID="CustExpDateFormat" runat="server" ErrorMessage="Please Enter valid Policy Expiry Date "
                                ClientValidationFunction="ValidateExpDate" ValidationGroup="SAVE" Display="Dynamic"
                                Text="*" />
                            <%-- <asp:CompareValidator ID="compareExpDateFormat" runat="server" ControlToValidate="txtPolicyExp"
                                Display="Dynamic" ErrorMessage="Please enter valid Policy Expiry Date " Text="*"
                                Operator="DataTypeCheck" Type="Date" ValidationGroup="SAVE" />--%>
                            <asp:CompareValidator ID="compareExptoProPreEff" runat="server" ControlToValidate="txtPolicyExp"
                                Display="Dynamic" ErrorMessage="Policy Expiration Date must be greater than or equal to Program Period Effective Date. Please enter a valid Policy Expiration Date."
                                Text="*" Operator="GreaterThanEqual" Type="Date" ControlToCompare="txtProgramPeriodEffDate"
                                ValidationGroup="SAVE" />
                            <asp:CompareValidator ID="compareExptoProPreExp" runat="server" ControlToValidate="txtPolicyExp"
                                Display="Dynamic" ErrorMessage="Policy Expiration Date must be less than or equal Program Period Expiration Date. Please enter a valid Policy Expiration Date."
                                Text="*" Operator="LessThanEqual" Type="Date" ControlToCompare="txtProgramPeriodExpDate"
                                ValidationGroup="SAVE" />
                        </td>
                        <td>
                            Coverage Type:
                        </td>
                        <td>
                            <asp:ObjectDataSource ID="ObjectCovType" runat="server" SelectMethod="GetLookUpActiveData"
                                TypeName="ZurichNA.AIS.Business.Logic.BLAccess">
                                <SelectParameters>
                                    <asp:Parameter DefaultValue="LOB COVERAGE" Name="lookUpTypeName" Type="String" />
                                </SelectParameters>
                            </asp:ObjectDataSource>
                            <asp:DropDownList ID="ddlCoverageType" runat="server" DataSourceID="ObjectCovType"
                                ValidationGroup="SAVE" Width="90px" DataTextField="LookUpName" DataValueField="LookUpID">
                            </asp:DropDownList>
                        </td>
                        <td style="vertical-align: middle">
                            <asp:CompareValidator ID="compareCovType" runat="server" ControlToValidate="ddlCoverageType"
                                Display="Dynamic" ErrorMessage="Please Select Coverage Type" Text="*" Operator="NotEqual"
                                ValueToCompare="0" ValidationGroup="SAVE" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Adjustment Type:
                        </td>
                        <td>
                            <asp:ObjectDataSource ID="ObjectAdjustment" runat="server" SelectMethod="GetLookUpActiveData"
                                TypeName="ZurichNA.AIS.Business.Logic.BLAccess">
                                <SelectParameters>
                                    <asp:Parameter DefaultValue="ADJUSTMENT TYPE" Name="lookUpTypeName" Type="String" />
                                </SelectParameters>
                            </asp:ObjectDataSource>
                            <asp:DropDownList ID="ddlAdjustmentType" runat="server" DataSourceID="ObjectAdjustment"
                                Width="175px" DataTextField="LookUpName" DataValueField="LookUpID" ValidationGroup="SAVE">
                            </asp:DropDownList>
                        </td>
                        <td style="vertical-align: middle">
                            <asp:CompareValidator ID="compareAdjustmentType" runat="server" ControlToValidate="ddlAdjustmentType"
                                Display="Dynamic" ErrorMessage="Please Select Adjustment Type" Text="*" Operator="NotEqual"
                                ValueToCompare="0" ValidationGroup="SAVE" />
                        </td>
                        <td>
                            Loss Source:
                        </td>
                        <td>
                            <asp:ObjectDataSource ID="objLossSource" runat="server" SelectMethod="GetLookUpActiveData"
                                TypeName="ZurichNA.AIS.Business.Logic.BLAccess">
                                <SelectParameters>
                                    <asp:Parameter DefaultValue="LOSS SOURCE" Name="lookUpTypeName" Type="String" />
                                </SelectParameters>
                            </asp:ObjectDataSource>
                            <asp:DropDownList ID="ddlLossSource" runat="server" DataSourceID="objLossSource"
                                DataTextField="LookUpName" DataValueField="LookUpID" Width="90px" ValidationGroup="SAVE">
                            </asp:DropDownList>
                        </td>
                        <td>
                        </td>
                        <td>
                            Unlimited Pol Ded Limit:
                        </td>
                        <td>
                            <asp:CheckBox ID="chkUnLimDedPolLim" runat="server" Width="95px" ValidationGroup="SAVE"
                                onclick="EnableTxtDedPolLimAmt()"></asp:CheckBox>
                        </td>
                        <td>
                        </td>
                        <td>
                            Deductible Pol Limit Amt:
                        </td>
                        <td>
                            <asp:AISAmountTextbox ID="txtDedPolLimAmt" runat="server" Width="85px" Font-Size="XX-Small"
                                ValidationGroup="SAVE"></asp:AISAmountTextbox>
                            <%-- <cc1:FilteredTextBoxExtender ID="FilterDedPolLimAmt" runat="server" TargetControlID="txtDedPolLimAmt"
                                FilterType="Custom" ValidChars="1234567890" />--%>
                            <%--<cc1:FilteredTextBoxExtender ID="FilterDedPolLimAmt" runat="server" TargetControlID="txtDedPolLimAmt"
                                 FilterType="Custom" FilterMode="ValidChars" ValidChars="0123456789," />--%>
                        </td>
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            ALAE Handling Type:
                        </td>
                        <td>
                            <asp:ObjectDataSource ID="objALAE" runat="server" SelectMethod="GetLookUpActiveData"
                                TypeName="ZurichNA.AIS.Business.Logic.BLAccess">
                                <SelectParameters>
                                    <asp:Parameter DefaultValue="ALAE TYPE" Name="lookUpTypeName" Type="String" />
                                </SelectParameters>
                            </asp:ObjectDataSource>
                            <asp:DropDownList ID="ddlALAE" runat="server" DataSourceID="objALAE" DataTextField="LookUpName"
                                DataValueField="LookUpID" Width="175px" onchange="EnableDisableALAECappedAmt()"
                                ValidationGroup="SAVE" />
                        </td>
                        <td style="vertical-align: middle">
                            <asp:CompareValidator ID="compareALAE" runat="server" ControlToValidate="ddlALAE"
                                Display="Dynamic" ErrorMessage="Please Select ALAE Handling Type" Text="*" Operator="NotEqual"
                                ValueToCompare="0" ValidationGroup="SAVE" />
                        </td>
                        <td>
                            ALAE Capped Amt:
                        </td>
                        <td>
                            <asp:AISAmountTextbox ID="txtALAECAPPED" runat="server" Width="85px" Font-Size="XX-Small"
                                ValidationGroup="SAVE" Enabled="false"></asp:AISAmountTextbox>
                            <%--<cc1:FilteredTextBoxExtender ID="filterALAECAPPED" runat="server" TargetControlID="txtALAECAPPED"
                                FilterType="Custom" ValidChars="1234567890" />--%>
                            <%--<cc1:FilteredTextBoxExtender ID="filterALAECAPPED" runat="server" TargetControlID="txtALAECAPPED"
                                 FilterType="Custom" FilterMode="ValidChars" ValidChars="0123456789," />--%>
                        </td>
                        <td>
                        </td>
                        <td>
                            Unlimited Override Ded Limit:
                        </td>
                        <td>
                            <asp:CheckBox ID="chkUnLimOvrDedLim" runat="server" Width="65px" onclick="EnableTxtOvrDedLimAmt()"
                                ValidationGroup="SAVE"></asp:CheckBox>
                        </td>
                        <td>
                        </td>
                        <td>
                            Override Ded Limit Amt:
                        </td>
                        <td>
                            <asp:AISAmountTextbox ID="txtOvrDedLimAmt" runat="server" Width="85px" Font-Size="XX-Small"
                                ValidationGroup="SAVE"> </asp:AISAmountTextbox>
                            <%--<cc1:FilteredTextBoxExtender ID="FilterOvrDedLimAmt" runat="server" TargetControlID="txtOvrDedLimAmt"
                                FilterType="Custom" ValidChars="1234567890" />--%>
                            <%--<cc1:FilteredTextBoxExtender ID="FilterOvrDedLimAmt" runat="server" TargetControlID="txtOvrDedLimAmt"
                                 FilterType="Custom" FilterMode="ValidChars" ValidChars="0123456789," />--%>
                        </td>
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            LDF/IBNR Incl Limit:
                        </td>
                        <td>
                            <asp:CheckBox ID="chkLDFIBNRInclLim" runat="server" ValidationGroup="SAVE"></asp:CheckBox>
                        </td>
                        <td>
                        </td>
                        <td>
                            LDF/IBNR Stepped:
                        </td>
                        <td>
                            <asp:CheckBox ID="chkLDFIBNRStepped" runat="server" Width="95px" ValidationGroup="SAVE"
                                onclick="EnableTxtLDFIBNR()"></asp:CheckBox>
                        </td>
                        <td>
                        </td>
                        <td>
                            LDF :
                        </td>
                        <td>
                            <asp:TextBox ID="txtLDF" runat="server" Width="85px" ValidationGroup="SAVE" onblur="DisablePolicyOtherFactor();"></asp:TextBox>
                            <cc1:MaskedEditExtender ID="maskLDF" runat="server" Mask="9.999999" MaskType="Number"
                                TargetControlID="txtLDF" />
                            <%--  <cc1:FilteredTextBoxExtender ID="filterLDF" runat="server" TargetControlID="txtLDF"
                        FilterType="Custom" ValidChars="1234567890." />--%>
                        </td>
                        <td>
                        </td>
                        <td>
                            IBNR :
                        </td>
                        <td>
                            <asp:TextBox ID="txtIBNR" runat="server" Width="85px" ValidationGroup="SAVE" onblur="DisablePolicyOtherFactor();"></asp:TextBox>
                            <cc1:MaskedEditExtender ID="maskIBNR" runat="server" Mask="9.999999" MaskType="Number"
                                TargetControlID="txtIBNR" />
                            <%-- <cc1:FilteredTextBoxExtender ID="filterIBNR" runat="server" TargetControlID="txtIBNR"
                        FilterType="Custom" ValidChars="1234567890." />--%>
                        </td>
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            DEP State:
                        </td>
                        <td>
                            <asp:ObjectDataSource ID="objDepState" runat="server" SelectMethod="GetLookUpActiveData"
                                TypeName="ZurichNA.AIS.Business.Logic.BLAccess">
                                <SelectParameters>
                                    <asp:Parameter DefaultValue="STATE" Name="lookUpTypeName" Type="String" />
                                </SelectParameters>
                            </asp:ObjectDataSource>
                            <asp:DropDownList ID="ddlDepState" runat="server" DataSourceID="objDepState" DataTextField="LookUpName"
                                DataValueField="LookUpID" Width="150px" ValidationGroup="SAVE">
                            </asp:DropDownList>
                        </td>
                        <td>
                        </td>
                        <td>
                            TPA:
                        </td>
                        <td>
                            <asp:CheckBox ID="chkTPA" runat="server" Width="65px" onclick="EnablechkTPADirect()"
                                ValidationGroup="SAVE"></asp:CheckBox>
                        </td>
                        <td>
                        </td>
                        <td>
                            TPA Direct:
                        </td>
                        <td>
                            <asp:CheckBox ID="chkTPADirect" runat="server" Width="65px" ValidationGroup="SAVE">
                            </asp:CheckBox>
                        </td>
                        <td>
                        </td>
                        <td>
                            Master PEO Policy:
                        </td>
                        <td>
                            <asp:CheckBox ID="chkPEOPolicy" runat="server" Width="65px" ValidationGroup="SAVE">
                            </asp:CheckBox>
                        </td>
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Non Conv.:
                        </td>
                        <td>
                            <asp:AISAmountTextbox ID="txtNonConv" runat="server" Width="85px" Font-Size="XX-Small"
                                ValidationGroup="SAVE"></asp:AISAmountTextbox>
                            <%--<cc1:FilteredTextBoxExtender ID="filterNonConv" runat="server" TargetControlID="txtNonConv"
                                FilterType="Custom" ValidChars="1234567890" />--%>
                            <%--<cc1:FilteredTextBoxExtender ID="filterNonConv" runat="server" TargetControlID="txtNonConv"
                                 FilterType="Custom" FilterMode="ValidChars" ValidChars="0123456789," />--%>
                        </td>
                        <td>
                        </td>
                        <td>
                            Other Amt:
                        </td>
                        <td>
                            <asp:AISAmountTextbox ID="txtOthAmt" runat="server" Width="90px" Font-Size="XX-Small"
                                ValidationGroup="SAVE" onkeypress="return AmountValidation(event,this)"></asp:AISAmountTextbox>
                            <%-- <cc1:FilteredTextBoxExtender ID="filterOthAmt" runat="server" TargetControlID="txtOthAmt"
                                FilterType="Custom" ValidChars="-1234567890"/>--%>
                            <%--<cc1:FilteredTextBoxExtender ID="filterOthAmt" runat="server" TargetControlID="txtOthAmt"
                                 FilterType="Custom" FilterMode="ValidChars" ValidChars="0123456789,-" />--%>
                        </td>
                        <td>
                        </td>
                        <td>
                            NY Premium Discount:
                        </td>
                        <td>
                            <asp:AISAmountTextbox ID="txtNYPremiumDiscAmt" runat="server" Width="90px" AllowNegetive="true"
                                Font-Size="XX-Small" ValidationGroup="SAVE">
                            </asp:AISAmountTextbox>
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
                </table>
                <table width="100%">
                    <tr>
                        <td colspan="4" style="text-align: center">
                            <asp:HiddenField ID="hidSelValue" runat="server" />
                            <asp:Button ID="btnSave" Enabled="false" Text="Save" ValidationGroup="SAVE" runat="server"
                                Width="60px" OnClick="SAVE_Click" />
                            <asp:Button ID="btnCopy" Enabled="false" runat="server" Text="Copy" OnClick="btnCopy_Click"
                                Width="60px" />
                            <asp:Button ID="btnCancel" runat="server" Text="Cancel" OnClick="btnCancel_Click"
                                Width="60px" />
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <br />
            <%--<asp:Label ID="lblDepPolicy" runat="server" CssClass="h2" Text="DEP Policy" Visible="false"></asp:Label>
            &nbsp;<asp:LinkButton ID="lbCloseDepPolicy" Text="Close" runat="server" Visible="False"
                OnClick="lbCloseDepPolicy_Click" />
            <asp:Panel ID="pnlDEPPolicy" runat="server" CssClass="content" Visible="false" Height="100px"
                Width="910px" ScrollBars="Auto">
                <!--DEP Policy Information -->
                <asp:AISListView ID="lstDEPPolicy" runat="server" InsertItemPosition="FirstItem" OnItemCanceling="lstDEPPolicy_ItemCanceling"
                    OnItemCommand="lstDEPPolicy_ItemCommand" OnItemEditing="lstDEPPolicy_ItemEditing"
                    OnItemDataBound="lstDEPPolicy_ItemDataBound" OnItemUpdating="lstDEPPolicy_ItemUpdating" >
                    <EmptyDataTemplate>
                        <table width="98%">
                            <tr>
                                <th>
                                </th>
                                <th align="center">
                                    Policy #
                                </th>
                                <th align="center">
                                    Policy Eff. Date
                                </th>
                                <th align="center">
                                    Policy Exp. Date
                                </th>
                                <th align="center">
                                    Disable
                                </th>
                            </tr>
                        </table>
                        <table width="98%">
                            <tr id="Tr1" runat="server" class="ItemTemplate">
                                <td align="center">
                                    <asp:Label ID="lblEmptyMessage" Text="--- No Records Found ---" Font-Bold="true"
                                        Width="600px" runat="server" Style="text-align: center" />
                                </td>
                            </tr>
                        </table>
                    </EmptyDataTemplate>
                    <LayoutTemplate>
                        <table width="98%">
                            <tr>
                                <th>
                                </th>
                                <th align="center">
                                    Policy #
                                </th>
                                <th align="center">
                                    Policy Eff. Date
                                </th>
                                <th align="center">
                                    Policy Exp. Date
                                </th>
                                <th align="center">
                                    Disable
                                </th>
                            </tr>
                            <tr id="ItemPlaceHolder" runat="server">
                            </tr>
                        </table>
                    </LayoutTemplate>
                    <ItemTemplate>
                        <tr class="ItemTemplate">
                            <td>
                                <asp:LinkButton ID="lbDEPPolicyEdit" CommandName="Edit" Text="Edit" runat="server"
                                    Visible="true" Width="40px" Enabled='<% # Bind("IsActive") %>' />
                            </td>
                            <td>
                                <asp:Label ID="lblSymbol" Text='<%# Bind("PolicySymbol") %> ' runat="server"></asp:Label>
                                <asp:Label ID="lblNumber" Text='<%# Bind("PolicyNumber") %> ' runat="server"></asp:Label>
                                <asp:Label ID="lblModulus" Text=' <%# Bind("PolicyModulus") %>' runat="server"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblDEPPolicyEffDate" Text='<%# Bind("PolicyEffectiveDate","{0:d}") %>'
                                    runat="server"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblDEPPolicyEndDate" Text='<%# Bind("PlanEndDate","{0:d}") %>' runat="server"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblDEPActive" runat="server" Text='<%# Bind("IsActive") %>' Visible="false"></asp:Label>
                                <asp:ImageButton ID="imgDEPPolicyDisable" runat="server" CommandArgument='<%# Bind("PolicyID") %>' />
                            </td>
                        </tr>
                    </ItemTemplate>
                    <AlternatingItemTemplate>
                        <tr class="AlternatingItemTemplate">
                            <td>
                                <asp:LinkButton ID="lbDEPPolicyEdit" CommandName="Edit" Text="Edit" runat="server"
                                    Visible="true" Width="40px" Enabled='<% # Bind("IsActive") %>' />
                            </td>
                            <td>
                                <asp:Label ID="lblSymbol" Text='<%# Bind("PolicySymbol") %> ' runat="server"></asp:Label>
                                <asp:Label ID="lblNumber" Text='<%# Bind("PolicyNumber") %> ' runat="server"></asp:Label>
                                <asp:Label ID="lblModulus" Text=' <%# Bind("PolicyModulus") %>' runat="server"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblDEPPolicyEffDate" Text='<%# Bind("PolicyEffectiveDate","{0:d}") %>'
                                    runat="server" />
                            </td>
                            <td>
                                <asp:Label ID="lblDEPPolicyEndDate" Text='<%# Bind("PlanEndDate","{0:d}") %>' runat="server" />
                            </td>
                            <td>
                                <asp:Label ID="lblDEPActive" runat="server" Text='<%# Bind("IsActive") %>' Visible="false"></asp:Label>
                                <asp:ImageButton ID="imgDEPPolicyDisable" runat="server" CommandArgument='<%# Bind("PolicyID") %>' />
                            </td>
                        </tr>
                    </AlternatingItemTemplate>
                    <EditItemTemplate>
                        <tr>
                            <td align="center">
                                <asp:LinkButton ID="lbDepPolicyUpdate" CommandName="Update" Text="Update" runat="server"
                                    Visible="true" Width="40px" ValidationGroup="DEPEDIT" />
                                <asp:LinkButton ID="lbDepPolicyCancel" CommandName="Cancel" runat="server" Text="Cancel"
                                    Visible="true" Width="40px" />
                                <asp:Label ID="lblDEPPOLICY_ID" Visible="false" runat="server" Text='<%# Bind("PolicyID") %>' />
                            </td>
                            <td align="center">
                                <asp:TextBox ID="txtPolicySymbol" Text='<%#Bind("PolicySymbol") %>' runat="server"
                                    Width="35px" MaxLength="3" ValidationGroup="DEPEDIT"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredPolicySymbol" runat="server" ControlToValidate="txtPolicySymbol"
                                    ErrorMessage="Please Enter DEP Policy Symbol" Text="*" Display="Dynamic" ValidationGroup="DEPEDIT" />
                                <cc1:FilteredTextBoxExtender TargetControlID="txtPolicySymbol" FilterType="LowercaseLetters,UppercaseLetters"
                                    ID="ftePolicySymbol" runat="server">
                                </cc1:FilteredTextBoxExtender>
                                <asp:RegularExpressionValidator ID="regPolicySymbol" runat="server" ControlToValidate="txtPolicySymbol"
                                    Text="*" Display="Dynamic" ValidationGroup="DEPEDIT" ErrorMessage="Please enter minimum two alpha characters for Policy Symbol"
                                    ValidationExpression=".{2}.*" />
                                <asp:TextBox ID="txtPolicyNumber" Text='<%#Bind("PolicyNumber") %>' runat="server"
                                    Width="60px" MaxLength="8" ValidationGroup="DEPEDIT"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="requiredPolicyNumber" runat="server" ControlToValidate="txtPolicyNumber"
                                    ErrorMessage="Please Enter DEP Policy Number" Text="*" Display="Dynamic" ValidationGroup="DEPEDIT" />
                                <cc1:FilteredTextBoxExtender TargetControlID="txtPolicyNumber" FilterType="Numbers,LowercaseLetters,UppercaseLetters"
                                    ID="ftePolicyNumber" runat="server">
                                </cc1:FilteredTextBoxExtender>
                                <asp:TextBox ID="txtPolicyModulus" Text='<%#Bind("PolicyModulus") %>' runat="server"
                                    Width="20px" MaxLength="2" ValidationGroup="DEPEDIT"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="requiredPolicyModulus" runat="server" ControlToValidate="txtPolicyModulus"
                                    ErrorMessage="Please Enter DEP Policy Modulus" Text="*" Display="Dynamic" ValidationGroup="DEPEDIT" />
                                <cc1:FilteredTextBoxExtender TargetControlID="txtPolicyModulus" FilterType="Numbers"
                                    ID="ftePolicyModulus" runat="server">
                                </cc1:FilteredTextBoxExtender>
                                <asp:RegularExpressionValidator ID="RegularPolicyModulus" runat="server" ControlToValidate="txtPolicyModulus"
                                    Text="*" Display="Dynamic" ValidationGroup="DEPEDIT" ErrorMessage="Please enter minimum two numeric digits for Policy Modulus"
                                    ValidationExpression=".{2}.*" />
                            </td>
                            <td align="center">
                                <asp:TextBox ID="txtDEPPolicyEffDate" runat="server" Text='<%# Bind("PolicyEffectiveDate","{0:d}") %>'
                                    ValidationGroup="DEPEDIT" />
                                <asp:ImageButton ID="imgDEPPolicyEffDate" runat="server" ImageUrl="~/images/Calendar_scheduleHS.png"
                                    CausesValidation="False" />
                                <asp:RequiredFieldValidator ID="requiredDEPPolicyEffDate" runat="server" ControlToValidate="txtDEPPolicyEffDate"
                                    ErrorMessage="Please Enter DEP Policy Eff Date" Text="*" Display="Dynamic" ValidationGroup="DEPEDIT" />
                                <cc1:MaskedEditExtender ID="mskEffDate" runat="server" TargetControlID="txtDEPPolicyEffDate"
                                    Mask="99/99/9999" MessageValidatorTip="true" OnFocusCssClass="MaskedEditFocus"
                                    OnInvalidCssClass="MaskedEditError" MaskType="Date" DisplayMoney="Left" AcceptNegative="Left"
                                    ErrorTooltipEnabled="True" />
                                <cc1:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtDEPPolicyEffDate"
                                    PopupButtonID="imgDEPPolicyEffDate" />
                                <asp:CompareValidator ID="compareDEPEffDateFormat" runat="server" ControlToValidate="txtDEPPolicyEffDate"
                                    Display="Dynamic" ErrorMessage="Please enter valid DEP Effective Date " Text="*"
                                    Operator="DataTypeCheck" Type="Date" ValidationGroup="DEPEDIT" />
                                <asp:CompareValidator ID="compareDEPEfftoPolicyEff" runat="server" ControlToValidate="txtDEPPolicyEffDate"
                                    Display="Dynamic" ErrorMessage="DEP Effective Date must be Greater than or Equal Policy Effective Date, Please Enter Valid Effective Date"
                                    Text="*" Operator="GreaterThanEqual" Type="Date" ControlToCompare="txtPolicyEffDate"
                                    ValidationGroup="DEPEDIT" />
                                <asp:CompareValidator ID="compareDEPEfftoProPreEff" runat="server" ControlToValidate="txtDEPPolicyEffDate"
                                    Display="Dynamic" ErrorMessage="DEP Effective Date must be Greater than or Equal ProgramPeriod Effective Date, Please Enter Valid Effective Date"
                                    Text="*" Operator="GreaterThanEqual" Type="Date" ControlToCompare="txtProgramPeriodEffDate"
                                    ValidationGroup="DEPEDIT" />
                                <asp:CompareValidator ID="compareDEPEfftoPolicyExp" runat="server" ControlToValidate="txtDEPPolicyEffDate"
                                    Display="Dynamic" ErrorMessage="DEP Effective Date must be less than or Equal Policy Expiration Date, Please Enter Valid Effective Date"
                                    Text="*" Operator="LessThanEqual" Type="Date" ControlToCompare="txtPolicyEndDate"
                                    ValidationGroup="DEPEDIT" />
                                <asp:CompareValidator ID="compareDEPEfftoProPreExp" runat="server" ControlToValidate="txtDEPPolicyEffDate"
                                    Display="Dynamic" ErrorMessage="DEP Effective Date must be less than or Equal ProgramPeriod Expiration Date, Please Enter Valid Effective Date"
                                    Text="*" Operator="LessThanEqual" Type="Date" ControlToCompare="txtProgramPeriodEndDate"
                                    ValidationGroup="DEPEDIT" />
                                <asp:TextBox ID="txtProgramPeriodEffDate" runat="server" CssClass="Hide" />
                                <asp:TextBox ID="txtPolicyEffDate" runat="server" CssClass="Hide" />
                            </td>
                            <td align="center">
                                <asp:TextBox ID="txtDEPPolicyEndDate" runat="server" Text='<%# Bind("PlanEndDate","{0:d}") %>'
                                    ValidationGroup="DEPEDIT" />
                                <asp:ImageButton ID="imgDEPPolicyEndDate" runat="server" ImageUrl="~/images/Calendar_scheduleHS.png"
                                    CausesValidation="False" />
                                <asp:RequiredFieldValidator ID="requiredDEPPolicyEndDate" runat="server" ControlToValidate="txtDEPPolicyEndDate"
                                    ErrorMessage="Please Enter DEP Policy Exp Date" Text="*" Display="Dynamic" ValidationGroup="DEPEDIT" />
                                <cc1:MaskedEditExtender ID="MaskedEditEndDate" runat="server" TargetControlID="txtDEPPolicyEndDate"
                                    Mask="99/99/9999" MessageValidatorTip="true" OnFocusCssClass="MaskedEditFocus"
                                    OnInvalidCssClass="MaskedEditError" MaskType="Date" DisplayMoney="Left" AcceptNegative="Left"
                                    ErrorTooltipEnabled="True" />
                                <cc1:CalendarExtender ID="CalendarExtender3" runat="server" TargetControlID="txtDEPPolicyEndDate"
                                    PopupButtonID="imgDEPPolicyEndDate" />
                                <asp:CompareValidator ID="compareDEPExpDateFormat" runat="server" ControlToValidate="txtDEPPolicyEndDate"
                                    Display="Dynamic" ErrorMessage="Please enter valid DEP Expiry Date " Text="*"
                                    Operator="DataTypeCheck" Type="Date" ValidationGroup="DEPEDIT" />
                                <asp:CompareValidator ID="compareDEPExptoPolicyEff" runat="server" ControlToValidate="txtDEPPolicyEndDate"
                                    Display="Dynamic" ErrorMessage="DEP Expiry Date must be Greater than or Equal Policy Effective Date, Please Enter Valid DEP Expiry Date"
                                    Text="*" Operator="GreaterThanEqual" Type="Date" ControlToCompare="txtPolicyEffDate"
                                    ValidationGroup="DEPEDIT" />
                                <asp:CompareValidator ID="compareDEPExptoProPreEff" runat="server" ControlToValidate="txtDEPPolicyEndDate"
                                    Display="Dynamic" ErrorMessage="DEP Expiry  Date must be Greater than or Equal ProgramPeriod Effective Date, Please Enter Valid DEP Expiry Date"
                                    Text="*" Operator="GreaterThanEqual" Type="Date" ControlToCompare="txtProgramPeriodEffDate"
                                    ValidationGroup="DEPEDIT" />
                                <asp:CompareValidator ID="compareDEPExptoPolicyExp" runat="server" ControlToValidate="txtDEPPolicyEndDate"
                                    Display="Dynamic" ErrorMessage="DEP Expiry Date must be less than or Equal Policy Expiration Date, Please Enter Valid DEP Expiry Date"
                                    Text="*" Operator="LessThanEqual" Type="Date" ControlToCompare="txtPolicyEndDate"
                                    ValidationGroup="DEPEDIT" />
                                <asp:CompareValidator ID="compareDEPExptoProPreExp" runat="server" ControlToValidate="txtDEPPolicyEndDate"
                                    Display="Dynamic" ErrorMessage="DEP Expiry Date must be less than or Equal ProgramPeriod Expiration Date, Please Enter Valid DEP Expiry Date"
                                    Text="*" Operator="LessThanEqual" Type="Date" ControlToCompare="txtProgramPeriodEndDate"
                                    ValidationGroup="DEPEDIT" />
                                <asp:CompareValidator ID="compareDEPExptoDEPEff" runat="server" ControlToValidate="txtDEPPolicyEndDate"
                                    Display="Dynamic" ErrorMessage="Expiration Date must be greater than Effective Date, Please Enter Valid Expiry Date"
                                    Text="*" Operator="GreaterThan" Type="Date" ControlToCompare="txtDEPPolicyEffDate"
                                    ValidationGroup="DEPEDIT" />
                                <asp:TextBox ID="txtProgramPeriodEndDate" runat="server" CssClass="Hide" />
                                <asp:TextBox ID="txtPolicyEndDate" runat="server" CssClass="Hide" />
                            </td>
                            <td>
                            </td>
                        </tr>
                    </EditItemTemplate>
                    <InsertItemTemplate>
                        <tr>
                            <td align="center">
                                <asp:LinkButton ID="lbDEPPolicySave" CommandName="Save" Text="Save" runat="server"
                                    Visible="true" Width="40px" ValidationGroup="DEPSAVE" />
                            </td>
                            <td align="center">
                                <asp:TextBox ID="txtPolicySymbol" runat="server" MaxLength="3" Width="35px" ValidationGroup="DEPSAVE" />
                                <asp:RequiredFieldValidator ID="RequiredPolicySymbol" runat="server" ControlToValidate="txtPolicySymbol"
                                    ErrorMessage="Please Enter DEP Policy Symbol" Text="*" Display="Dynamic" ValidationGroup="DEPSAVE" />
                                <cc1:FilteredTextBoxExtender TargetControlID="txtPolicySymbol" FilterType="LowercaseLetters,UppercaseLetters"
                                    ID="ftePolicySymbol" runat="server">
                                </cc1:FilteredTextBoxExtender>
                                <asp:RegularExpressionValidator ID="regPolicySymbol" runat="server" ControlToValidate="txtPolicySymbol"
                                    Text="*" Display="Dynamic" ValidationGroup="DEPSAVE" ErrorMessage="Please enter minimum two alpha characters for Policy Symbol"
                                    ValidationExpression=".{2}.*" />
                                <asp:TextBox ID="txtPolicyNumber" runat="server" MaxLength="8" Width="60px" ValidationGroup="DEPSAVE" />
                                <asp:RequiredFieldValidator ID="requiredPolicyNumber" runat="server" ControlToValidate="txtPolicyNumber"
                                    ErrorMessage="Please Enter DEP Policy Number" Text="*" Display="Dynamic" ValidationGroup="DEPSAVE" />
                                <cc1:FilteredTextBoxExtender TargetControlID="txtPolicyNumber" FilterType="UppercaseLetters,LowercaseLetters,Numbers"
                                    ID="ftePolicyNumber" runat="server">
                                </cc1:FilteredTextBoxExtender>
                                <asp:TextBox ID="txtPolicyModulus" runat="server" MaxLength="2" Width="20px" ValidationGroup="DEPSAVE" />
                                <asp:RequiredFieldValidator ID="requiredPolicyModulus" runat="server" ControlToValidate="txtPolicyModulus"
                                    ErrorMessage="Please Enter DEP Policy Modulus" Text="*" Display="Dynamic" ValidationGroup="DEPSAVE" />
                                <cc1:FilteredTextBoxExtender TargetControlID="txtPolicyModulus" FilterType="Numbers"
                                    ID="ftePolicyModulus" runat="server">
                                </cc1:FilteredTextBoxExtender>
                                <asp:RegularExpressionValidator ID="RegularPolicyModulus" runat="server" ControlToValidate="txtPolicyModulus"
                                    Text="*" Display="Dynamic" ValidationGroup="DEPSAVE" ErrorMessage="Please enter minimum two numeric digits for Policy Modulus"
                                    ValidationExpression=".{2}.*" />
                            </td>
                            <td align="center">
                                <asp:TextBox ID="txtDEPPolicyEffDate" runat="server" ValidationGroup="DEPSAVE" />
                                <asp:ImageButton ID="imgDEPPolicyEffDate" runat="server" ImageUrl="~/images/Calendar_scheduleHS.png"
                                    CausesValidation="False" />
                                <asp:RequiredFieldValidator ID="requiredDEPPolicyEffDate" runat="server" ControlToValidate="txtDEPPolicyEffDate"
                                    ErrorMessage="Please Enter DEP Policy Eff Date" Text="*" Display="Dynamic" ValidationGroup="DEPSAVE" />
                                <cc1:MaskedEditExtender ID="mskEffDate" runat="server" TargetControlID="txtDEPPolicyEffDate"
                                    Mask="99/99/9999" MessageValidatorTip="true" OnFocusCssClass="MaskedEditFocus"
                                    OnInvalidCssClass="MaskedEditError" MaskType="Date" DisplayMoney="Left" AcceptNegative="Left"
                                    ErrorTooltipEnabled="True" />
                                <cc1:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtDEPPolicyEffDate"
                                    PopupButtonID="imgDEPPolicyEffDate" />
                                <asp:CompareValidator ID="compareDEPEffDateFormat" runat="server" ControlToValidate="txtDEPPolicyEffDate"
                                    Display="Dynamic" ErrorMessage="Please enter valid DEP Effective Date " Text="*"
                                    Operator="DataTypeCheck" Type="Date" ValidationGroup="DEPSAVE" />
                                <asp:CompareValidator ID="compareDEPEfftoPolicyEff" runat="server" ControlToValidate="txtDEPPolicyEffDate"
                                    Display="Dynamic" ErrorMessage="DEP Effective Date must be Greater than or Equal Policy Effective Date, Please Enter Valid Effective Date"
                                    Text="*" Operator="GreaterThanEqual" Type="Date" ControlToCompare="txtPolicyEffDate"
                                    ValidationGroup="DEPSAVE" />
                                <asp:CompareValidator ID="compareDEPEfftoProPreEff" runat="server" ControlToValidate="txtDEPPolicyEffDate"
                                    Display="Dynamic" ErrorMessage="DEP Effective Date must be Greater than or Equal ProgramPeriod Effective Date, Please Enter Valid Effective Date"
                                    Text="*" Operator="GreaterThanEqual" Type="Date" ControlToCompare="txtProgramPeriodEffDate"
                                    ValidationGroup="DEPSAVE" />
                                <asp:CompareValidator ID="compareDEPEfftoPolicyExp" runat="server" ControlToValidate="txtDEPPolicyEffDate"
                                    Display="Dynamic" ErrorMessage="DEP Effective Date must be less than or Equal Policy Expiration Date, Please Enter Valid Effective Date"
                                    Text="*" Operator="LessThanEqual" Type="Date" ControlToCompare="txtPolicyEndDate"
                                    ValidationGroup="DEPSAVE" />
                                <asp:CompareValidator ID="compareDEPEfftoProPreExp" runat="server" ControlToValidate="txtDEPPolicyEffDate"
                                    Display="Dynamic" ErrorMessage="DEP Effective Date must be less than or Equal ProgramPeriod Expiration Date, Please Enter Valid Effective Date"
                                    Text="*" Operator="LessThanEqual" Type="Date" ControlToCompare="txtProgramPeriodEndDate"
                                    ValidationGroup="DEPSAVE" />
                                <asp:TextBox ID="txtProgramPeriodEffDate" runat="server" CssClass="Hide" />
                                <asp:TextBox ID="txtPolicyEffDate" runat="server" CssClass="Hide" />
                            </td>
                            <td align="center">
                                <asp:TextBox ID="txtDEPPolicyEndDate" runat="server" ValidationGroup="DEPSAVE" />
                                <asp:ImageButton ID="imgDEPPolicyEndDate" runat="server" ImageUrl="~/images/Calendar_scheduleHS.png"
                                    CausesValidation="False" />
                                <asp:RequiredFieldValidator ID="requiredDEPPolicyEndDate" runat="server" ControlToValidate="txtDEPPolicyEndDate"
                                    ErrorMessage="Please Enter DEP Policy Exp Date" Text="*" Display="Dynamic" ValidationGroup="DEPSAVE" />
                                <cc1:MaskedEditExtender ID="MaskedEditEndDate" runat="server" TargetControlID="txtDEPPolicyEndDate"
                                    Mask="99/99/9999" MessageValidatorTip="true" OnFocusCssClass="MaskedEditFocus"
                                    OnInvalidCssClass="MaskedEditError" MaskType="Date" DisplayMoney="Left" AcceptNegative="Left"
                                    ErrorTooltipEnabled="True" />
                                <cc1:CalendarExtender ID="CalendarExtender2" runat="server" TargetControlID="txtDEPPolicyEndDate"
                                    PopupButtonID="imgDEPPolicyEndDate" />
                                <asp:CompareValidator ID="compareDEPExpDateFormat" runat="server" ControlToValidate="txtDEPPolicyEndDate"
                                    Display="Dynamic" ErrorMessage="Please enter valid DEP Expiry Date " Text="*"
                                    Operator="DataTypeCheck" Type="Date" ValidationGroup="DEPSAVE" />
                                <asp:CompareValidator ID="compareDEPExptoDEPEff" runat="server" ControlToValidate="txtDEPPolicyEndDate"
                                    Display="Dynamic" ErrorMessage="Expiration Date must be greater than Effective Date, Please Enter Valid Expiry Date"
                                    Text="*" Operator="GreaterThan" Type="Date" ControlToCompare="txtDEPPolicyEffDate"
                                    ValidationGroup="DEPSAVE" />
                                <asp:CompareValidator ID="compareDEPExptoPolicyEff" runat="server" ControlToValidate="txtDEPPolicyEndDate"
                                    Display="Dynamic" ErrorMessage="DEP Expiry Date must be Greater than or Equal Policy Effective Date, Please Enter Valid DEP Expiry Date"
                                    Text="*" Operator="GreaterThanEqual" Type="Date" ControlToCompare="txtPolicyEffDate"
                                    ValidationGroup="DEPSAVE" />
                                <asp:CompareValidator ID="compareDEPExptoProPreEff" runat="server" ControlToValidate="txtDEPPolicyEndDate"
                                    Display="Dynamic" ErrorMessage="DEP Expiry  Date must be Greater than or Equal ProgramPeriod Effective Date, Please Enter Valid DEP Expiry Date"
                                    Text="*" Operator="GreaterThanEqual" Type="Date" ControlToCompare="txtProgramPeriodEffDate"
                                    ValidationGroup="DEPSAVE" />
                                <asp:CompareValidator ID="compareDEPExptoPolicyExp" runat="server" ControlToValidate="txtDEPPolicyEndDate"
                                    Display="Dynamic" ErrorMessage="DEP Expiry Date must be less than or Equal Policy Expiration Date, Please Enter Valid DEP Expiry Date"
                                    Text="*" Operator="LessThanEqual" Type="Date" ControlToCompare="txtPolicyEndDate"
                                    ValidationGroup="DEPSAVE" />
                                <asp:CompareValidator ID="compareDEPExptoProPreExp" runat="server" ControlToValidate="txtDEPPolicyEndDate"
                                    Display="Dynamic" ErrorMessage="DEP Expiry Date must be less than or Equal ProgramPeriod Expiration Date, Please Enter Valid DEP Expiry Date"
                                    Text="*" Operator="LessThanEqual" Type="Date" ControlToCompare="txtProgramPeriodEndDate"
                                    ValidationGroup="DEPSAVE" />
                                <asp:TextBox ID="txtProgramPeriodEndDate" runat="server" CssClass="Hide" />
                                <asp:TextBox ID="txtPolicyEndDate" runat="server" CssClass="Hide" />
                            </td>
                            <td>
                            </td>
                        </tr>
                    </InsertItemTemplate>
                </asp:AISListView>
            </asp:Panel>--%>
            <asp:Label ID="lblSteppedFactor" runat="server" CssClass="h2" Text="Stepped Factors"
                Visible="false"></asp:Label>
            &nbsp;<asp:LinkButton ID="lbCloseSteppedFactor" Text="Close" runat="server" Visible="False"
                OnClick="lbCloseSteppedFactor_Click" />
            <asp:Panel ID="pnlSteppedFactor" runat="server" Visible="false" Height="100px" Width="910px"
                ScrollBars="Auto">
                <!--Stepped Factors Information-->
                <asp:AISListView ID="lstSteppedFactor" runat="server" InsertItemPosition="FirstItem"
                    OnItemCanceling="lstSteppedFactor_ItemCanceling" OnItemCommand="lstSteppedFactor_ItemCommand"
                    OnItemEditing="lstSteppedFactor_ItemEditing" OnItemDataBound="lstSteppedFactor_ItemDataBound"
                    OnItemUpdating="lstSteppedFactor_ItemUpdating">
                    <EmptyDataTemplate>
                        <table width="98%">
                            <tr>
                                <th>
                                </th>
                                <th>
                                    Months To Val
                                </th>
                                <th>
                                    LDF Factor
                                </th>
                                <th>
                                    IBNR Factor
                                </th>
                                <th>
                                    Disable
                                </th>
                            </tr>
                        </table>
                        <table width="98%">
                            <tr id="Tr1" runat="server" class="ItemTemplate">
                                <td align="center">
                                    <asp:Label ID="lblEmptyMessage" Text="--- No Records Found ---" Font-Bold="true"
                                        Width="600px" runat="server" Style="text-align: center" />
                                </td>
                            </tr>
                        </table>
                    </EmptyDataTemplate>
                    <LayoutTemplate>
                        <table width="98%">
                            <tr align="center">
                                <th>
                                </th>
                                <th>
                                    Months To Val
                                </th>
                                <th>
                                    LDF Factor
                                </th>
                                <th>
                                    IBNR Factor
                                </th>
                                <th>
                                    Disable
                                </th>
                            </tr>
                            <tr id="ItemPlaceHolder" runat="server">
                            </tr>
                        </table>
                    </LayoutTemplate>
                    <ItemTemplate>
                        <tr class="ItemTemplate">
                            <td>
                                <asp:LinkButton ID="lbSteppedFactorEdit" CommandName="Edit" Text="Edit" runat="server"
                                    Visible="true" Width="40px" Enabled='<% # Bind("ISACTIVE") %>' />
                                <asp:HiddenField ID="hdfSteppedFactorID" Value='<%#Bind("STEPPED_FACTOR_ID") %>'
                                    runat="server" />
                            </td>
                            <td>
                                <asp:Label ID="lblMonthsToVal" Text='<%# Bind("MONTHS_TO_VAL") %>' runat="server"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblLDFFactor" Text='<%# Bind("LDF_FACTOR") %>' runat="server"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblIBNRFactor" Text='<%# Bind("IBNR_FACTOR") %>' runat="server"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblSPActive" runat="server" Text='<%# Bind("ISACTIVE") %>' Visible="false"></asp:Label>
                                <asp:ImageButton ID="imgSteppedFactorDisable" runat="server" CommandArgument='<%# Bind("STEPPED_FACTOR_ID") %>' />
                            </td>
                        </tr>
                    </ItemTemplate>
                    <AlternatingItemTemplate>
                        <tr class="AlternatingItemTemplate">
                            <td>
                                <asp:LinkButton ID="lbSteppedFactorEdit" CommandName="Edit" Text="Edit" runat="server"
                                    Visible="true" Width="40px" Enabled='<% # Bind("ISACTIVE") %>' />
                                <asp:HiddenField ID="hdfSteppedFactorID" Value='<%#Bind("STEPPED_FACTOR_ID") %>'
                                    runat="server" />
                            </td>
                            <td>
                                <asp:Label ID="lblMonthsToVal" Text='<%# Bind("MONTHS_TO_VAL") %>' runat="server"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblLDFFactor" Text='<%# Bind("LDF_FACTOR") %>' runat="server"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblIBNRFactor" Text='<%# Bind("IBNR_FACTOR") %>' runat="server"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblSPActive" runat="server" Text='<%# Bind("ISACTIVE") %>' Visible="false"></asp:Label>
                                <asp:ImageButton ID="imgSteppedFactorDisable" runat="server" CommandArgument='<%# Bind("STEPPED_FACTOR_ID") %>' />
                            </td>
                        </tr>
                    </AlternatingItemTemplate>
                    <EditItemTemplate>
                        <tr align="center">
                            <td>
                                <asp:LinkButton ID="lbSteppedUpdate" CommandName="Update" Text="Update" runat="server"
                                    Visible="true" Width="40px" ValidationGroup="STEPPEDEDIT" />
                                <asp:LinkButton ID="lbSteppedCancel" CommandName="Cancel" runat="server" Text="Cancel"
                                    Visible="true" Width="40px" />
                                <asp:Label ID="lblSTEPPED_FACTOR_ID" Visible="false" runat="server" Text='<%# Bind("STEPPED_FACTOR_ID") %>' />
                            </td>
                            <td>
                                <asp:TextBox ID="txtMonthsToVal" Text='<%#Bind("MONTHS_TO_VAL") %>' runat="server"
                                    Width="40px" MaxLength="3" ValidationGroup="STEPPEDEDIT"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="filterMonthsToVal" runat="server" ControlToValidate="txtMonthsToVal"
                                    ErrorMessage="Please Enter Months To Val" Text="*" Display="Dynamic" ValidationGroup="STEPPEDEDIT" />
                                <cc1:FilteredTextBoxExtender TargetControlID="txtMonthsToVal" FilterType="Numbers"
                                    ID="fteMonthsToVal" runat="server">
                                </cc1:FilteredTextBoxExtender>
                            </td>
                            <td>
                                <asp:TextBox ID="txtLDFFactor" Text='<%#Bind("LDF_FACTOR") %>' runat="server" ValidationGroup="STEPPEDEDIT" />
                                <cc1:MaskedEditExtender ID="maskldffactor" runat="server" Mask="9.999999" MaskType="number"
                                    TargetControlID="txtldffactor" />
                                <%--<cc1:FilteredTextBoxExtender ID="filterLDFFactor" runat="server" TargetControlID="txtLDFFactor"
                                    FilterType="Custom" ValidChars="1234567890." />--%>
                            </td>
                            <td>
                                <asp:TextBox ID="txtIBNRFactor" Text='<%#Bind("IBNR_FACTOR") %>' runat="server" ValidationGroup="STEPPEDEDIT" />
                                <cc1:MaskedEditExtender ID="maskIBNRFactor" runat="server" Mask="9.999999" MaskType="Number"
                                    TargetControlID="txtIBNRFactor" />
                                <%--<cc1:FilteredTextBoxExtender ID="filterIBNRFactor" runat="server" TargetControlID="txtIBNRFactor"
                                    FilterType="Custom" ValidChars="1234567890." />--%>
                            </td>
                            <td>
                            </td>
                        </tr>
                    </EditItemTemplate>
                    <InsertItemTemplate>
                        <tr align="center">
                            <td>
                                <asp:LinkButton ID="lbSteppedFactorSave" CommandName="Save" Text="Save" runat="server"
                                    Visible="true" Width="40px" ValidationGroup="STEPPEDSAVE" />
                            </td>
                            <td>
                                <asp:TextBox ID="txtMonthsToVal" runat="server" MaxLength="3" ValidationGroup="STEPPEDSAVE"
                                    Width="40px" />
                                <asp:RequiredFieldValidator ID="filterMonthsToVal" runat="server" ControlToValidate="txtMonthsToVal"
                                    ErrorMessage="Please Enter Months To Val" Text="*" Display="Dynamic" ValidationGroup="STEPPEDSAVE" />
                                <cc1:FilteredTextBoxExtender TargetControlID="txtMonthsToVal" FilterType="Numbers"
                                    ID="fteMonthsToVal" runat="server">
                                </cc1:FilteredTextBoxExtender>
                            </td>
                            <td>
                                <asp:TextBox ID="txtLDFFactor" runat="server" ValidationGroup="STEPPEDSAVE"></asp:TextBox>
                                <cc1:MaskedEditExtender ID="maskLDFFactor" runat="server" Mask="9.999999" MaskType="Number"
                                    TargetControlID="txtLDFFactor" />
                            </td>
                            <td>
                                <asp:TextBox ID="txtIBNRFactor" runat="server" ValidationGroup="STEPPEDSAVE"></asp:TextBox>
                                <cc1:MaskedEditExtender ID="maskIBNRFactor" runat="server" Mask="9.999999" MaskType="Number"
                                    TargetControlID="txtIBNRFactor" />
                            </td>
                            <td>
                            </td>
                        </tr>
                    </InsertItemTemplate>
                </asp:AISListView>
            </asp:Panel>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnPopUpLoad" />
            <asp:PostBackTrigger ControlID="btnSFUpdate" />
            <asp:PostBackTrigger ControlID="btnSFErrorLog" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
