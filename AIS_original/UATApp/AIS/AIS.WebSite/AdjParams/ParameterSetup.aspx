<%@ Page Language="C#" MasterPageFile="~/Retro.master" AutoEventWireup="true" Inherits="AdjParams_ParameterSetup"
    Title="Parameter Setup" CodeBehind="ParameterSetup.aspx.cs" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="../App_Shared/AccountInfoHeader.ascx" TagName="AccountInfoHeader"
    TagPrefix="uc1" %>
<%@ Register Src="../App_Shared/ProgramPeriod.ascx" TagName="ProgramPeriod" TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeaderPlaceHolder" runat="Server">
    <asp:Label ID="lblParamsetup" runat="server" Text="Parameter Setup" CssClass="h1"></asp:Label>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainPlaceHolder" runat="Server">

    <script language="javascript" type="text/javascript">

function ValidateLBACheckBox(oSource, oArgs) 
    {
      var result=false;     
      // get an array of all controls on the form
      var aControls = document.forms[0];
      // iterate through array looking for checkboxes the ID of CheckBoxList is "lstBoxPolicy" so each checkbox
      // control will have the name lstBoxPolicy:n where n is a value from zero to the number of checkboxes minus one
      for (var i=0; i < aControls.length; i++) 
      {        
        if (aControls[i].name.substring(46, 61) == 'PolicyNumLstBox')
         {
          // increment counter if checkbox is "ticked"
          if (aControls[i].checked) 
          {result = true;}
         }
      }
     oArgs.IsValid = result;
}

function verifyCheckList(sender, myargs) 
{
   
    var frm = $get('ctl00_MainPlaceHolder_TabContainer1_tblpnlLBA_pnlPolicyNumberListLBA_PolicyNumLstBox');
    aler(frm.elments.length);
        for (i=0;i<frm.elements.length;i++) 
        {

            if (frm.elements[i].type == "checkbox") 
            {

               if(frm.elements[i].checked)
               {
               myargs.IsValid=true;
               }
               else
               {
               myargs.IsValid=false;
               }

            }

        }

  
}

function Index_Changed(chk1,chk2) 
  {
    var control = $get('ctl00_MainPlaceHolder_TabContainer1_tblpnlLBA_lblcheckvalidations');
    if (control != null)
    { 
       control.style.visibility='hidden';
    }
    //var chkBoxList = document.getElementById(chk1);
    var chkBoxList2 = document.getElementById(chk2);
    //if (chkBoxList2.Eabled ==true)
    //{
       var chkBoxList = document.getElementById(chk1);
       var chkBoxCount2= chkBoxList2.getElementsByTagName("input");
       var chkBoxCount = chkBoxList.getElementsByTagName("input");
        
       for(var i=0;i<chkBoxCount.length;i++) 
        {
          if ((chkBoxCount[i].checked == true)&& (chkBoxCount2[i].checked == true))
          {
           chkBoxCount[i].checked = false;
          }
        }
     //}
  }
  
  function Index_Changed2(chk1,chk2) 
  {
    var control = $get('ctl00_MainPlaceHolder_TabContainer1_tblpnlLBA_lblcheckvalidations');
    if (control != null)
    { 
        control.style.visibility='hidden';
    }
    
    var chkBoxList = document.getElementById(chk1);
    //if (chkBoxList.Eabled ==true)
    //{
      var chkBoxList2 = document.getElementById(chk2);
      var chkBoxCount2= chkBoxList2.getElementsByTagName("input");
      var chkBoxCount = chkBoxList.getElementsByTagName("input");
      for(var i=0;i<chkBoxCount2.length;i++) 
        {
          if ((chkBoxCount2[i].checked == true)&& (chkBoxCount[i].checked == true))
          {
           chkBoxCount2[i].checked = false;
          }
        } 
    //}
  }  
  
  
  function doKeypress(control,max){
    maxLength = parseInt(max);
    value = control.value;
     if(maxLength && value.length > maxLength-1){
          event.returnValue = false;
          maxLength = parseInt(maxLength);
     }
}
// Cancel default behavior
function doBeforePaste(control,max){
    maxLength = parseInt(max);
     if(maxLength)
     {
          event.returnValue = false;
     }
}
// Cancel default behavior and create a new paste routine
function doPaste(control,max){
    maxLength = parseInt(max);
    value = control.value;
     if(maxLength){
          event.returnValue = false;
          maxLength = parseInt(maxLength);
          var oTR = control.document.selection.createRange();
          var iInsertLength = maxLength - value.length + oTR.text.length;
          var sData = window.clipboardData.getData("Text").substr(0,iInsertLength);
          oTR.text = sData;
     }
}


    </script>

    <table style="height: 500px; width: 920px">
        <tr>
            <td>
                <asp:ValidationSummary ID="ValSaveLBA" runat="server" ValidationGroup="SaveLBA" />
                <asp:ValidationSummary ID="ValSaveLBA2" runat="server" ValidationGroup="SaveLBA2" />
                <asp:ValidationSummary ID="ValSaveLBADtlsUpdt" runat="server" ValidationGroup="SaveLBADetailsUpdt" />
                <asp:ValidationSummary ID="ValSaveLBADtls" runat="server" ValidationGroup="SaveLBADetails" />
                <asp:ValidationSummary ID="ValSaveLCFDtlsUpdt" runat="server" ValidationGroup="SaveLCFDtlsUpdt" />
                <asp:ValidationSummary ID="ValSaveLCFDtls" runat="server" ValidationGroup="SaveLCFDtls" />
                <asp:ValidationSummary ID="ValSaveTMDtls" runat="server" ValidationGroup="SaveTMDtls" />
                <asp:ValidationSummary ID="ValSaveTMDtlsUpdt" runat="server" ValidationGroup="SaveTMDtlsUpdt" />
                <asp:ValidationSummary ID="ValSaveRMLDtls" runat="server" ValidationGroup="SaveRML" />
                <asp:ValidationSummary ID="ValSaveRMLDtlsUpdt" runat="server" ValidationGroup="SaveRMLUpdt" />
                <asp:ValidationSummary ID="ValSaveCHF2" runat="server" ValidationGroup="SaveCHF2" />
                <asp:ValidationSummary ID="ValSaveCHF" runat="server" ValidationGroup="SaveCHF" />
                <asp:ValidationSummary ID="ValSaveCHFDtls" runat="server" ValidationGroup="SaveCHFDtls" />
                <asp:ValidationSummary ID="ValSaveCHFDtlsUpdt" runat="server" ValidationGroup="SaveCHFDtlsUpdt" />
                <uc1:AccountInfoHeader ID="AccountInfoHeader1" runat="server" />
                <!--Program Periods -->
                <asp:Panel ID="pnlpp" runat="server">
                    <asp:Label ID="lblProgramPeriodsHeader" runat="server" Font-Size="12px" font-weight="bold"
                        ForeColor="Navy" Font-Bold="true" Text="Program Periods"></asp:Label>
                    <uc2:ProgramPeriod ID="uc2ProgramPeriod" runat="server" />
                </asp:Panel>
                <!-- Object Data Source-->
                <asp:ObjectDataSource ID="lbainfodatasource" runat="server" EnablePaging="false"
                    TypeName="BLAccess" SelectMethod="GetPolicyData" OnSelecting="lbainfomdatasource_selecting">
                    <SelectParameters>
                        <asp:Parameter Name="ProgramPeriodID" Type="Int32" />
                    </SelectParameters>
                </asp:ObjectDataSource>
                <asp:ObjectDataSource ID="LBAAdjTypeDataSource" runat="server" EnablePaging="false"
                    TypeName="BLAccess" SelectMethod="GetLBAAdjustmentTypes" OnSelecting="LBAAdjustmentTypeDataSource_Selecting">
                </asp:ObjectDataSource>
                <asp:ObjectDataSource ID="LBAsetupStateDataSource" runat="server" SelectMethod="GetLookUpActiveData"
                    TypeName="ZurichNA.AIS.Business.Logic.BLAccess">
                    <SelectParameters>
                        <asp:Parameter Name="lookUpTypeName" Type="String" DefaultValue="STATE" />
                    </SelectParameters>
                </asp:ObjectDataSource>
                <asp:ObjectDataSource ID="LCFsetupLOBDataSource" runat="server" SelectMethod="GetLookUpActiveData"
                    TypeName="ZurichNA.AIS.Business.Logic.BLAccess">
                    <SelectParameters>
                        <asp:Parameter Name="lookUpTypeName" Type="String" DefaultValue="LOB" />
                    </SelectParameters>
                </asp:ObjectDataSource>
                <asp:ObjectDataSource ID="CHFLosstypeDataSource" runat="server" SelectMethod="GetLookUpActiveData"
                    TypeName="ZurichNA.AIS.Business.Logic.BLAccess">
                    <SelectParameters>
                        <asp:Parameter Name="lookUpTypeName" Type="String" DefaultValue="CHF LOSSTYPE" />
                    </SelectParameters>
                </asp:ObjectDataSource>
                <cc1:TabContainer ID="TabContainer1" runat="server" CssClass="CustomTabs" ActiveTabIndex="0"
                    Height="100px" Width="910px">
                    <cc1:TabPanel runat="server" ID="tblpnlLBA" HeaderText="LBA Setup">
                        <HeaderTemplate>
                            LBA Setup
                        </HeaderTemplate>
                        <ContentTemplate>
                            <div class="grid">
                                <asp:UpdatePanel ID="Updatepanellba" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:Label ID="lblShowMessage" runat="server" Visible="false" CssClass="ValidationSummary"
                                            ForeColor="Red" Width="450px"></asp:Label>
                                        <asp:Panel ID="pnlAdjLBAEmptyData" runat="server" Visible="False">
                                            <table id="Table1" runat="server" class="panelContents">
                                                <tr align="center" style="height: 16px">
                                                    <th>
                                                        Policy #'s
                                                    </th>
                                                    <th style="width: 165px; vertical-align: middle">
                                                        LBA Adj. Type
                                                    </th>
                                                    <th>
                                                        Included In ERP
                                                    </th>
                                                    <th>
                                                        Initial Deposit
                                                    </th>
                                                    <th>
                                                        Incl. IBNR/LDF
                                                    </th>
                                                    <th>
                                                        Details
                                                    </th>
                                                    <th>
                                                        Disable
                                                    </th>
                                                </tr>
                                            </table>
                                            <table>
                                                <tr id="Tr1" runat="server" class="ItemTemplate">
                                                    <td align="center">
                                                        <asp:Label ID="lblEmptyMessage" Text="No policies of WC, AH, or PROF coverage type exist for this program period"
                                                            Font-Bold="true" runat="server" Style="text-align: center" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </asp:Panel>
                                        <asp:Panel ID="pnlAdlLBA" runat="server" Visible="False">
                                            <table id="tbllbaadj" runat="server" class="panelContents">
                                                <tr align="center" style="height: 16px">
                                                    <th>
                                                    </th>
                                                    <th>
                                                        Policy #'s
                                                    </th>
                                                    <th style="width: 165px; vertical-align: middle">
                                                        LBA Adj. Type
                                                    </th>
                                                    <th>
                                                        Included In ERP
                                                    </th>
                                                    <th>
                                                        Initial Deposit
                                                    </th>
                                                    <th>
                                                        Incl. IBNR/LDF
                                                    </th>
                                                    <th>
                                                        Details
                                                    </th>
                                                    <th>
                                                        Disable
                                                    </th>
                                                </tr>
                                                <tr class="ItemTemplate" style="vertical-align: middle">
                                                    <td style="vertical-align: middle">
                                                        <asp:LinkButton ID="lnkBtnLBAadj" runat="server" Text="Save" Visible="false" ValidationGroup="SaveLBA"
                                                            Width="42px" OnClick="btnLBAInfoDetailsSave_Click" />
                                                        <asp:LinkButton ID="lnkBtnLBAadjUpd" runat="server" Text="Update" Visible="false"
                                                            ValidationGroup="SaveLBA" Width="42px" OnClick="btnLBAInfoDetailsSave_Click" />
                                                        <asp:LinkButton ID="lnkBtnLBASetupCancel" runat="server" Text="Cancel" Visible="true"
                                                            Width="42px" OnClick="LBASetupCancel_Click" />
                                                    </td>
                                                    <td style="vertical-align: middle">
                                                        <asp:Panel ID="pnlPolicyNumberListLBA" runat="server" ScrollBars="Auto" CssClass="content"
                                                            Height="20px">
                                                            <asp:CheckBoxList ID="PolicyNumLstBox" runat="server" ForeColor="Black" ReadOnly="true"
                                                                BackColor="White" BorderColor="Black">
                                                            </asp:CheckBoxList>
                                                        </asp:Panel>
                                                    </td>
                                                    <td style="vertical-align: middle">
                                                        <asp:DropDownList ID="LBAAdjTypeDdl" runat="server" Enabled="true" Width="170px">
                                                        </asp:DropDownList>
                                                        <asp:CompareValidator ID="CompddlLBAAdjTypeDdl" runat="server" ControlToValidate="LBAAdjTypeDdl"
                                                            ValidationGroup="SaveLBA" ValueToCompare="0" Display="Dynamic" Text="*" ErrorMessage="Please select LBA Adjustment type from Dropdown"
                                                            Operator="NotEqual"></asp:CompareValidator>
                                                    </td>
                                                    <td style="vertical-align: middle">
                                                        <asp:CheckBox ID="IncludedInERPButton" runat="server" Enabled="true"></asp:CheckBox>
                                                    </td>
                                                    <td style="vertical-align: middle">
                                                        <asp:AISAmountTextbox ID="txtlbaintdeposit" runat="server" Enabled="True" Width="90px" ></asp:AISAmountTextbox>
                                                       <%--<cc1:FilteredTextBoxExtender ID="Filteredtxtlbaintdeposit" runat="server" TargetControlID="txtlbaintdeposit"
                                                            FilterType="Custom" FilterMode="ValidChars" ValidChars="0123456789," />--%>
                                                         
                                                    </td>
                                                    <td style="vertical-align: middle">
                                                        <asp:CheckBox ID="LdfIbnrInclCheckBox" runat="server" Checked="false" Enabled="True"
                                                            Width="115px" />
                                                    </td>
                                                    <td style="vertical-align: middle">
                                                        <asp:LinkButton ID="LinkButton2" runat="server" CommandName="Edit" Text="Details"
                                                            Visible="true" Width="62px" OnClick="btnLBAInfoDetailsInfo_Click" />
                                                    </td>
                                                    <td style="vertical-align: middle">
                                                        <asp:ImageButton ID="imgDisablerow" OnCommand='DisablefirstRow' CommandArgument='<%# Bind("adjt_param_dtl_id") %>'
                                                            runat="server" />
                                                    </td>
                                                </tr>
                                                <tr class="AlternatingItemTemplate" style="vertical-align: middle">
                                                    <td style="vertical-align: middle">
                                                        <asp:LinkButton ID="LinkButtonSave1" runat="server" CommandName="Edit" Text="Save"
                                                            ValidationGroup="SaveLBA2" Visible="false" Width="42px" OnClick="btnLBAInfo2DetailsSave_Click" />
                                                        <asp:LinkButton ID="LinkButtonUpd1" runat="server" CommandName="Edit" Text="Update"
                                                            ValidationGroup="SaveLBA2" Visible="false" Width="42px" OnClick="btnLBAInfo2DetailsSave_Click" />
                                                        <asp:LinkButton ID="lnkBtnLBASetup2Cancel" runat="server" Text="Cancel" Visible="true"
                                                            Width="42px" OnClick="LBASetupCancel_Click" />
                                                    </td>
                                                    <td style="vertical-align: middle">
                                                        <asp:Panel ID="pnlPolicyNumber2ListLBA" runat="server" ScrollBars="Auto" CssClass="content"
                                                            Height="20px">
                                                            <asp:CheckBoxList ID="PolicyNumLstBox2" runat="server" ForeColor="Black" ReadOnly="true"
                                                                BackColor="White" BorderColor="Black">
                                                            </asp:CheckBoxList>
                                                        </asp:Panel>
                                                    </td>
                                                    <td style="vertical-align: middle">
                                                        <asp:DropDownList ID="LBAAdjTypeDdl2" runat="server" Enabled="True" Width="170px">
                                                        </asp:DropDownList>
                                                        <asp:CompareValidator ID="CompddlLBAAdjTypeDdl2" runat="server" ControlToValidate="LBAAdjTypeDdl2"
                                                            ValidationGroup="SaveLBA2" ValueToCompare="0" Text="*" Display="Dynamic" ErrorMessage="Please select LBA Adjustment type from Dropdown"
                                                            Operator="NotEqual"></asp:CompareValidator>
                                                    </td>
                                                    <td style="vertical-align: middle">
                                                        <asp:CheckBox ID="NotIncludedInERPbtn" runat="server" Enabled="true"></asp:CheckBox>
                                                    </td>
                                                    <td style="vertical-align: middle">
                                                        <asp:AISAmountTextbox ID="txtlbaintdeposit2" runat="server"  Width="90px" ></asp:AISAmountTextbox>
                                                       <%--<cc1:FilteredTextBoxExtender ID="Filteredtxtlbaintdeposit2" runat="server" TargetControlID="txtlbaintdeposit2"
                                                            FilterType="Custom" FilterMode="ValidChars" ValidChars="0123456789," />--%>
                                                        
                                                    </td>
                                                    <td style="vertical-align: middle">
                                                        <asp:CheckBox ID="LdfIbnrInclCheckBox2" runat="server" Checked="false" Enabled="True"
                                                            Width="115px" />
                                                    </td>
                                                    <td style="vertical-align: middle">
                                                        <asp:LinkButton ID="LinkButtonDetail" runat="server" CommandName="Edit" Text="Details"
                                                            Visible="true" Width="62px" OnClick="btnLBA2InfoDetailsInfo_Click" />
                                                    </td>
                                                    <td style="vertical-align: middle">
                                                        <asp:ImageButton ID="imgDisable2row" OnCommand='DisablesecondRow' CommandArgument='<%# Bind("adjt_param_dtl_id") %>'
                                                            runat="server" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </asp:Panel>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                                <asp:UpdatePanel ID="UpdatepanelLBADetails" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:TextBox ID="hdnAdjPrmsetup1txtBox" Visible="False" runat="server"></asp:TextBox>
                                        <asp:TextBox ID="hdnAdjPrmsetup2txtBox" Visible="False" runat="server"></asp:TextBox>
                                        <table id="tbdAdjLBAdtls" runat="server" class="panelContents" width="930px">
                                            <tr>
                                                <td>
                                                    <asp:Label ID="LBASetupDetailsLabel" runat="server" CssClass="h3" Text="LBA Setup - Details"
                                                        Width="135px" Visible="false"></asp:Label>&nbsp;&nbsp;<asp:LinkButton ID="lnkbtnLBADetailsClose"
                                                            Text="Close" runat="server" Visible="false" OnClick="btnLBAInfoClose_Click" />
                                                    <div class="content" style="height: 144px; width: 820px; overflow: auto">
                                                        <asp:Panel ID="pnlLbaPolicy" runat="server" Height="20px" Width="50px">
                                                            <asp:AISListView ID="lbasetupdetailslistview" runat="server" Visible="false" OnItemEditing="lstLBAParmsetupDtls_ItemEdit"
                                                                OnItemDataBound="lstLBAParmsetupDtls_DataBoundList" InsertItemPosition="FirstItem"
                                                                OnItemCanceling="lstLBAParmsetupDtls_ItemCancel" OnItemUpdating="lstLBAParmsetupDtls_ItemUpdating"
                                                                OnItemCommand="lstLBAParmsetupDtls_ItemCommand">
                                                                <LayoutTemplate>
                                                                    <table id="tblLayoutLBAdetails" class="ItemTemplate" runat="server" width="800px">
                                                                        <tr>
                                                                            <th>
                                                                            </th>
                                                                            <th align="center">
                                                                                State
                                                                            </th>
                                                                            <th align="center">
                                                                                Factor
                                                                            </th>
                                                                            <th align="center">
                                                                                Final LBA Amt.
                                                                            </th>
                                                                            <th align="center" style="width: 350px; height: 20px;">
                                                                                Comments
                                                                            </th>
                                                                            <th align="center" style="width: 20px; height: 20px;">
                                                                                Disable
                                                                            </th>
                                                                        </tr>
                                                                        <tr id="ItemPlaceHolder" runat="server">
                                                                        </tr>
                                                                    </table>
                                                                </LayoutTemplate>
                                                                <ItemTemplate>
                                                                    <tr class="ItemTemplate">
                                                                        <td id="tdDetailsEdit" align="center" runat="server">
                                                                            <asp:LinkButton ID="lbadetailsetupEdit" CommandName="Edit" Text="Edit" runat="server"
                                                                                Visible="true" OnClick="btnlbasetupinfodetails_click" />
                                                                        </td>
                                                                        <td>
                                                                            <div style="width: 75px; height: 20px; vertical-align: middle;">
                                                                                <asp:Label ID="lblLBAStateID" Width="74px" Text='<%# Bind("PrgParameterStateName") %>'
                                                                                    runat="server"></asp:Label>
                                                                            </div>
                                                                        </td>
                                                                        <td align="left">
                                                                            <div style="width: 12px; height: 20px; vertical-align: middle;">
                                                                                <asp:Label ID="lblAdjParmtdtlLBAID" runat="server" Text='<%# Bind("prem_adj_pgm_dtl_id") %>'
                                                                                    Visible="false"></asp:Label>
                                                                                <asp:Label ID="lblLBAFactor" Text='<%# Bind("adj_fctr_rt") %>' runat="server"></asp:Label>
                                                                            </div>
                                                                        </td>
                                                                        <td>
                                                                            <div style="width: 90px; height: 20px; vertical-align: middle;">
                                                                                <asp:Label ID="lblLBAFnlLBAamt" Width="89px" Text='<%# Eval("fnl_overrid_amt") != null ? (Eval("fnl_overrid_amt").ToString() != "" ?(decimal.Parse(Eval("fnl_overrid_amt").ToString())).ToString("#,##0") : ""): ""%>'
                                                                                    runat="server"></asp:Label>
                                                                            </div>
                                                                        </td>
                                                                        <td>
                                                                            <div style="width: 350px; height: auto; background-color: White; vertical-align: middle;">
                                                                                <asp:TextBox Width="350px" ID="lblLBAcomments" Text='<%# Bind("cmmnt_txt") %>' TextMode="MultiLine"
                                                                                    ReadOnly="true" runat="server"></asp:TextBox>
                                                                            </div>
                                                                        </td>
                                                                        <td>
                                                                            <div style="width: 20px; height: 20px; vertical-align: middle;">
                                                                                <asp:HiddenField ID="hidActive" runat="server" Value='<%# Bind("act_ind")%>' />
                                                                                <asp:ImageButton ID="imgDisableEnable" CommandArgument='<%# Bind("prem_adj_pgm_dtl_id") %>'
                                                                                    runat="server" ImageUrl='<%# Eval("act_ind").ToString()=="True"?"~/images/disabled.GIF":"~/images/enabled.GIF" %>'>
                                                                                </asp:ImageButton>
                                                                            </div>
                                                                        </td>
                                                                    </tr>
                                                                </ItemTemplate>
                                                                <AlternatingItemTemplate>
                                                                    <tr class="AlternatingItemTemplate">
                                                                        <td id="tdDetailsEdit" align="center" runat="server">
                                                                            <asp:LinkButton ID="lbadetailsetupEdit" CommandName="Edit" Text="Edit" runat="server"
                                                                                Visible="true" OnClick="btnlbasetupinfodetails_click" />
                                                                        </td>
                                                                        <td>
                                                                            <div style="width: 15px; height: 20px; vertical-align: middle;">
                                                                                <asp:Label ID="lblLBAStateID" Text='<%# Bind("PrgParameterStateName") %>' runat="server"></asp:Label>
                                                                            </div>
                                                                        </td>
                                                                        <td align="left">
                                                                            <div style="width: 75px; height: 20px; vertical-align: middle;">
                                                                                <asp:Label ID="lblAdjParmtdtlLBAID" runat="server" Text='<%# Bind("prem_adj_pgm_dtl_id") %>'
                                                                                    Visible="false"></asp:Label>
                                                                                <asp:Label ID="lblLBAFactor" Width="74px" Text='<%# Bind("adj_fctr_rt") %>' runat="server"></asp:Label>
                                                                            </div>
                                                                        </td>
                                                                        <td>
                                                                            <div style="width: 90px; height: 20px; vertical-align: middle;">
                                                                                <asp:Label ID="lblLBAFnlLBAamt" Width="89px" Text='<%# Eval("fnl_overrid_amt") != null ? (Eval("fnl_overrid_amt").ToString() != "" ?(decimal.Parse(Eval("fnl_overrid_amt").ToString())).ToString("#,##0") : ""): ""%>'
                                                                                    runat="server"></asp:Label>
                                                                            </div>
                                                                        </td>
                                                                        <td>
                                                                            <div style="width: 350px; height: auto; background-color: White; vertical-align: middle;">
                                                                                <asp:TextBox Width="350px" ID="lblLBAcomments" Text='<%# Bind("cmmnt_txt") %>' TextMode="MultiLine"
                                                                                    ReadOnly="true" runat="server"></asp:TextBox>
                                                                            </div>
                                                                        </td>
                                                                        <td>
                                                                            <div style="width: 20px; height: 20px; vertical-align: middle;">
                                                                                <asp:HiddenField ID="hidActive" runat="server" Value='<%# Bind("act_ind")%>' />
                                                                                <asp:ImageButton ID="imgDisableEnable" CommandArgument='<%# Bind("prem_adj_pgm_dtl_id") %>'
                                                                                    runat="server" ImageUrl='<%# Eval("act_ind").ToString()=="True"?"~/images/disabled.GIF":"~/images/enabled.GIF" %>'>
                                                                                </asp:ImageButton>
                                                                            </div>
                                                                        </td>
                                                                    </tr>
                                                                </AlternatingItemTemplate>
                                                                <EditItemTemplate>
                                                                    <tr>
                                                                        <td>
                                                                            <asp:LinkButton ID="lnkLbaprmtSetupUpdate" CommandName="Update" ValidationGroup="SaveLBADetailsUpdt"
                                                                                Text="Update" runat="server" Visible="true" /><br />
                                                                            <asp:LinkButton ID="lnkCombElemsCancel" CommandName="Cancel" runat="server" Text="Cancel"
                                                                                Visible="true" />
                                                                        </td>
                                                                        <td align="left">
                                                                            <div style="width: 15px; height: 20px; vertical-align: middle;">
                                                                                <asp:Label ID="hidLBAState" runat="server" Text='<%# Bind("PrgparameterStateFullname") %>'
                                                                                    Visible="false"></asp:Label>
                                                                                <asp:DropDownList ID="ddLBAState" DataSourceID="LBAsetupStateDataSource" DataTextField="LookUpName"
                                                                                    DataValueField="LookUpID" runat="server">
                                                                                </asp:DropDownList>
                                                                                <asp:CompareValidator ID="CompddLBAState" runat="server" ControlToValidate="ddLBAState"
                                                                                    ValidationGroup="SaveLBADetailsUpdt" ValueToCompare="0" Text="*" Display="Dynamic"
                                                                                    ErrorMessage="Please select a State from dropdown box" Operator="NotEqual"></asp:CompareValidator>
                                                                            </div>
                                                                        </td>
                                                                        <td align="left">
                                                                            <div style="width: 75px; height: 20px; vertical-align: middle;">
                                                                                <asp:Label ID="lblAdjParmtdtlLBAID" runat="server" Text='<%# Bind("prem_adj_pgm_dtl_id") %>'
                                                                                    Visible="false"></asp:Label>
                                                                                <cc1:MaskedEditExtender ID="MaskedEdittxtlbaintdeposit" runat="server" TargetControlID="txtLBAFactor"
                                                                                    Mask="9.999999" MessageValidatorTip="true" MaskType="Number" DisplayMoney="None"
                                                                                    OnFocusCssClass="MaskedEditFocus" OnInvalidCssClass="MaskedEditError" InputDirection="LeftToRight"
                                                                                    AcceptNegative="None" />
                                                                                <asp:TextBox ID="txtLBAFactor" Width="74px" Text='<%# Bind("adj_fctr_rt")%>' runat="server"> 
                                                                                </asp:TextBox><asp:RequiredFieldValidator ID="reqtxtLBAFactor" runat="server" ErrorMessage="Please enter Adjustment Factor"
                                                                                    Text="*" Display="Dynamic" ValidationGroup="SaveLBADetailsUpdt" ControlToValidate="txtLBAFactor"></asp:RequiredFieldValidator>
                                                                            </div>
                                                                        </td>
                                                                        <td>
                                                                            <div style="width: 90px; height: 20px; vertical-align: middle;">
                                                                                <asp:AISAmountTextbox ID="txtLBAfinalAmount" Width="89px" Text='<%# Bind("fnl_overrid_amt","{0:0}")%>'
                                                                                    runat="server"  > </asp:AISAmountTextbox>
                                                                                 <%--<cc1:FilteredTextBoxExtender runat="server" TargetControlID="txtLBAfinalAmount"
                                                                                FilterType="Custom" ValidChars="0123456789," ID="fltrAudexp">
                                                                            </cc1:FilteredTextBoxExtender>--%>
                                                                            </div>
                                                                        </td>
                                                                        <td>
                                                                            <div style="width: 350px; height: auto; background-color: White; vertical-align: middle;">
                                                                                <asp:TextBox Width="350px" ID="LBASetupComments" Text='<%# Bind("cmmnt_txt")%>' runat="server"
                                                                                    TextMode="MultiLine" MaxLength="500" onpaste="doPaste(this,500);" onkeypress="doKeypress(this,500);"
                                                                                    onbeforepaste="doBeforePaste(this,500);"> 
                                                                                </asp:TextBox>
                                                                            </div>
                                                                        </td>
                                                                        <td>
                                                                            <asp:CheckBox ID="chkActive" runat="server" Checked="true" Text="Active" />
                                                                        </td>
                                                                    </tr>
                                                                </EditItemTemplate>
                                                                <InsertItemTemplate>
                                                                    <tr class="AlternatingItemTemplate">
                                                                        <td align="left">
                                                                            <asp:LinkButton ID="lbLBAsetupdetailSave" CommandName="Save" ValidationGroup="SaveLBADetails"
                                                                                Text="Save" runat="server" Visible="true" Height="20px" Width="10px" />
                                                                        </td>
                                                                        <td align="left">
                                                                            <div style="width: 15px; height: 20px; vertical-align: middle;">
                                                                                <asp:DropDownList ID="ddLBAState" DataSourceID="LBAsetupStateDataSource" DataTextField="LookUpName"
                                                                                    DataValueField="LookUpID" runat="server">
                                                                                </asp:DropDownList>
                                                                                <asp:CompareValidator ID="CompddLBAStateInst" runat="server" ControlToValidate="ddLBAState"
                                                                                    ValidationGroup="SaveLBADetails" ValueToCompare="0" Text="*" Display="Dynamic"
                                                                                    ErrorMessage="Please select a State from dropdown box" Operator="NotEqual"></asp:CompareValidator>
                                                                            </div>
                                                                        </td>
                                                                        <td>
                                                                            <div style="width: 75px; height: 20px; vertical-align: middle;">
                                                                                <asp:TextBox ID="txtLBAFactor" Width="74px" Text="" ReadOnly="false" runat="server"
                                                                                    MaxLength="8"></asp:TextBox><cc1:MaskedEditExtender ID="MaskedEdittxtlbaintdeposit"
                                                                                        runat="server" TargetControlID="txtLBAFactor" Mask="9.999999" MessageValidatorTip="true"
                                                                                        MaskType="Number" DisplayMoney="None" OnFocusCssClass="MaskedEditFocus" OnInvalidCssClass="MaskedEditError"
                                                                                        InputDirection="LeftToRight" AcceptNegative="None" />
                                                                                <asp:RequiredFieldValidator ID="reqFirstName" runat="server" ErrorMessage="Please enter Adjustment Factor"
                                                                                    Text="*" Display="Dynamic" ValidationGroup="SaveLBADetails" ControlToValidate="txtLBAFactor"></asp:RequiredFieldValidator>
                                                                            </div>
                                                                        </td>
                                                                        <td>
                                                                            <div style="width: 90px; height: 20px; vertical-align: middle;">
                                                                                <asp:AISAmountTextbox ID="txtLBAfinalAmount" Width="89px" Text="" ReadOnly="false" runat="server"
                                                                                    ></asp:AISAmountTextbox>
                                                                                <%--<cc1:FilteredTextBoxExtender runat="server" TargetControlID="txtLBAfinalAmount"
                                                                                FilterType="Custom" ValidChars="0123456789," ID="fltrAudexp">
                                                                            </cc1:FilteredTextBoxExtender>--%>
                                                                            </div>
                                                                        </td>
                                                                        <td>
                                                                            <div style="width: 350px; height: 20px; background-color: White; vertical-align: middle;">
                                                                                <asp:TextBox Width="350px" ID="LBASetupComments" Text="" runat="server" TextMode="MultiLine"
                                                                                    MaxLength="500" onpaste="doPaste(this,500);" onkeypress="doKeypress(this,500);"
                                                                                    onbeforepaste="doBeforePaste(this,500);"></asp:TextBox>
                                                                            </div>
                                                                        </td>
                                                                        <td>
                                                                        </td>
                                                                    </tr>
                                                                </InsertItemTemplate>
                                                            </asp:AISListView>
                                                        </asp:Panel>
                                                    </div>
                                                </td>
                                            </tr>
                                        </table>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                        </ContentTemplate>
                    </cc1:TabPanel>
                    <cc1:TabPanel runat="server" ID="tblpnlLCF" HeaderText="LCF Setup">
                        <HeaderTemplate>
                            LCF Setup
                        </HeaderTemplate>
                        <ContentTemplate>
                            <div class="grid1">
                                <asp:UpdatePanel ID="UpdatepanelLCF" runat="server" UpdateMode="Conditional" Visible="true">
                                    <ContentTemplate>
                                        <asp:Panel ID="pnlAdjLCF" runat="server" Visible="False">
                                            <table id="tblAdjLCF" visible="true" runat="server" class="panelContents">
                                                <tr align="center">
                                                    <th>
                                                    </th>
                                                    <th>
                                                        Policy #'s
                                                    </th>
                                                    <th>
                                                        LCF Claim Cap
                                                    </th>
                                                    <th>
                                                        LCF Aggregate Cap
                                                    </th>
                                                    <th>
                                                        Layered LCF(Insured. Pays)
                                                    </th>
                                                    <th>
                                                        Layered LCF(Zurich Pays)
                                                    </th>
                                                    <th>
                                                        Details
                                                    </th>
                                                    <th>
                                                        Disable
                                                    </th>
                                                </tr>
                                                <tr class="ItemTemplate" style="vertical-align: middle">
                                                    <td style="vertical-align: middle">
                                                        <asp:LinkButton ID="lnkbtnSaveLCH" runat="server" Text="Save" Visible="false" Width="42px"
                                                            OnClick="btnLCFInfoDetailsSave_Click" />
                                                        <asp:LinkButton ID="lnkbtnUpdLCH" runat="server" Text="Update" Visible="false" OnClick="btnLCFInfoDetailsSave_Click" />
                                                        <asp:LinkButton ID="lnkBtnLCFCncl" runat="server" Text="Cancel" Visible="true" Width="42px"
                                                            OnClick="LBASetupCancel_Click" />
                                                    </td>
                                                    <td>
                                                        <asp:Panel ID="Panel1" runat="server" ScrollBars="Auto" CssClass="content" Height="20px">
                                                            <asp:CheckBoxList ID="ckkboxlstPolicyno" runat="server" ForeColor="Black" ReadOnly="true"
                                                                BackColor="White" BorderColor="Black">
                                                            </asp:CheckBoxList>
                                                        </asp:Panel>
                                                    </td>
                                                    <td style="vertical-align: middle">
                                                        <asp:AISAmountTextbox ID="txtLCFClmCAP" runat="server" Enabled="True" Width="101px" ></asp:AISAmountTextbox>
                                                      <%--<cc1:FilteredTextBoxExtender ID="FilteredtxtLCFClmCAP" runat="server" TargetControlID="txtLCFClmCAP"
                                                            FilterType="Custom" FilterMode="ValidChars" ValidChars="0123456789," />--%>
                                                        
                                                    </td>
                                                    <td style="vertical-align: middle">
                                                        <asp:AISAmountTextbox ID="txtLCFAggtAmt" runat="server" Enabled="True" Width="100px" ></asp:AISAmountTextbox>
                                                      <%--<cc1:FilteredTextBoxExtender ID="FilteredtxtLCFAggtAmt" runat="server" TargetControlID="txtLCFAggtAmt"
                                                            FilterType="Custom" FilterMode="ValidChars" ValidChars="0123456789," />--%>
                                                       
                                                    </td>
                                                    <td style="vertical-align: middle">
                                                        <asp:AISAmountTextbox ID="txtLayLCFIPay" runat="server" Enabled="True" Width="101px"  > </asp:AISAmountTextbox>
                                                       <%--<cc1:FilteredTextBoxExtender ID="FilteredtxtLayLCFIPay" runat="server" TargetControlID="txtLayLCFIPay"
                                                            FilterType="Custom" FilterMode="ValidChars" ValidChars="0123456789," />--%>
                                                       
                                                    </td>
                                                    <td style="vertical-align: middle">
                                                        <asp:AISAmountTextbox ID="txtLayLCFZurchPay" runat="server"  Enabled="True"
                                                            Width="100px" ></asp:AISAmountTextbox>
                                                       <%--<cc1:FilteredTextBoxExtender ID="FilteredtxtLayLCFZurchPay" runat="server" TargetControlID="txtLayLCFZurchPay"
                                                            FilterType="Custom" FilterMode="ValidChars" ValidChars="0123456789," />--%>
                                                       
                                                    </td>
                                                    <td style="vertical-align: middle">
                                                        <asp:LinkButton ID="lnkBtnLCFdtls" runat="server" CommandName="Edit" Text="Details"
                                                            Visible="true" Width="73px" OnClick="btnLCFInfoDetailsInfo_Click" />
                                                    </td>
                                                    <td style="vertical-align: middle">
                                                        <asp:ImageButton ID="imgbtnDisableLCF" OnCommand='DisableLCFRow' CommandArgument='<%# Bind("adj_paramet_setup_id") %>'
                                                            runat="server" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </asp:Panel>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                                <asp:UpdatePanel ID="UpdatepanelLCFDtls" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:TextBox ID="hdnLCFPrmsetuptxtBox" Visible="False" runat="server"></asp:TextBox>
                                        <table id="LCFsetupdtlstble" runat="server" class="panelContents" width="930px">
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblLCFdtls" runat="server" Text="LCF Setup - Details" CssClass="h3"
                                                        Width="135px" Visible="false"></asp:Label>&nbsp; &nbsp;<asp:LinkButton ID="lnkLCFClose"
                                                            Text="Close" runat="server" Visible="false" OnClick="btnLCFInfoClose_Click" />
                                                    <div class="content" style="height: 158px; width: 700px; overflow: auto">
                                                        <asp:Panel ID="LCFDtlsPanel" runat="server" Height="20px" Width="50px">
                                                            <asp:AISListView ID="lstLCFSetuplistView" runat="server" Visible="True" OnItemEditing="lstLCFParmsetupDtls_ItemEdit"
                                                                OnItemDataBound="lstLCFParmsetupDtls_DataBoundList" InsertItemPosition="FirstItem"
                                                                OnItemCanceling="lstLCFParmsetupDtls_ItemCancel" OnItemUpdating="lstLCFParmsetupDtls_ItemUpdating"
                                                                OnItemCommand="lstLCFParmsetupDtls_ItemCommand">
                                                                <LayoutTemplate>
                                                                    <table id="tblLayoutLCFdetails" class="ItemTemplate" runat="server">
                                                                        <tr style="height: 16px">
                                                                            <th>
                                                                            </th>
                                                                            <th align="center">
                                                                                LOB
                                                                            </th>
                                                                            <th align="center">
                                                                                State
                                                                            </th>
                                                                            <th align="center">
                                                                                Factor
                                                                            </th>
                                                                            <th align="center" style="width: 20px; height: 16px;">
                                                                                Disable
                                                                            </th>
                                                                        </tr>
                                                                        <tr id="ItemPlaceHolder" runat="server">
                                                                        </tr>
                                                                    </table>
                                                                </LayoutTemplate>
                                                                <ItemTemplate>
                                                                    <tr id="trLCFPrgPeriod" runat="server" style="background-color: #e7e7ff; color: #000000;
                                                                        height: 16px; vertical-align: middle">
                                                                        <td id="tdDetailsEdit" align="center" runat="server">
                                                                            <asp:LinkButton ID="lcfdetailsetupEdit" CommandName="Edit" Text="Edit" runat="server"
                                                                                Visible="true" />
                                                                        </td>
                                                                        <td>
                                                                            <div style="width: 193px; height: 16px;">
                                                                                <asp:Label ID="lblLOBdtls" Text='<%# Bind("PrgParameterLOBName") %>' runat="server"></asp:Label>
                                                                            </div>
                                                                        </td>
                                                                        <td>
                                                                            <div style="width: 193px; height: 16px;">
                                                                                <asp:Label ID="lblLCFStateID" Text='<%# Bind("PrgParameterStateName") %>' runat="server"></asp:Label>
                                                                            </div>
                                                                        </td>
                                                                        <td align="left">
                                                                            <div style="width: 193px; height: 16px;">
                                                                                <asp:Label ID="lblAdjParmtdtlLCFID" runat="server" Text='<%# Bind("prem_adj_pgm_dtl_id") %>'
                                                                                    Visible="false"></asp:Label>
                                                                                <asp:Label ID="lblLCFactor" Text='<%# Bind("adj_fctr_rt","{0:0.000000}") %>' runat="server"></asp:Label>
                                                                            </div>
                                                                        </td>
                                                                        <td>
                                                                            <div style="width: 40px; height: 16px;">
                                                                                <asp:HiddenField ID="hidActiveLCF" runat="server" Value='<%# Bind("act_ind")%>' />
                                                                                <asp:ImageButton ID="imgDisableEnableLCF" CommandArgument='<%# Bind("prem_adj_pgm_dtl_id") %>'
                                                                                    runat="server" ImageUrl='<%# Eval("act_ind").ToString()=="True"?"~/images/disabled.GIF":"~/images/enabled.GIF" %>'>
                                                                                </asp:ImageButton>
                                                                            </div>
                                                                        </td>
                                                                    </tr>
                                                                </ItemTemplate>
                                                                <AlternatingItemTemplate>
                                                                    <tr class="AlternatingItemTemplate">
                                                                        <td id="tdDetailsEdit" align="center" runat="server">
                                                                            <asp:LinkButton ID="lcfdetailsetupEdit" CommandName="Edit" Text="Edit" runat="server"
                                                                                Visible="true" />
                                                                        </td>
                                                                        <td>
                                                                            <div style="width: 193px; height: 16px;">
                                                                                <asp:Label ID="lblLOBdtls" Text='<%# Bind("PrgParameterLOBName") %>' runat="server"></asp:Label>
                                                                            </div>
                                                                        </td>
                                                                        <td>
                                                                            <div style="width: 193px; height: 16px;">
                                                                                <asp:Label ID="lblLCFStateID" Text='<%# Bind("PrgParameterStateName") %>' runat="server"></asp:Label>
                                                                            </div>
                                                                        </td>
                                                                        <td align="left">
                                                                            <div style="width: 193px; height: 16px;">
                                                                                <asp:Label ID="lblAdjParmtdtlLCFID" runat="server" Text='<%# Bind("prem_adj_pgm_dtl_id") %>'
                                                                                    Visible="false"></asp:Label>
                                                                                <asp:Label ID="lblLCFactor" Text='<%# Bind("adj_fctr_rt","{0:0.000000}") %>' runat="server"></asp:Label>
                                                                            </div>
                                                                        </td>
                                                                        <td>
                                                                            <div style="width: 40px; height: 16px;">
                                                                                <asp:HiddenField ID="hidActiveLCF" runat="server" Value='<%# Bind("act_ind")%>' />
                                                                                <asp:ImageButton ID="imgDisableEnableLCF" CommandArgument='<%# Bind("prem_adj_pgm_dtl_id") %>'
                                                                                    runat="server" ImageUrl='<%# Eval("act_ind").ToString()=="True"?"~/images/disabled.GIF":"~/images/enabled.GIF" %>'>
                                                                                </asp:ImageButton>
                                                                            </div>
                                                                        </td>
                                                                    </tr>
                                                                </AlternatingItemTemplate>
                                                                <EditItemTemplate>
                                                                    <tr style="width: 100%">
                                                                        <td>
                                                                            <asp:LinkButton ID="lnkLcfprmtSetupUpdate" CommandName="Update" Text="Update" ValidationGroup="SaveLCFDtlsUpdt"
                                                                                runat="server" Visible="true" /><br /><asp:LinkButton ID="lnkCombElemsCancel" CommandName="Cancel"
                                                                                    runat="server" Text="Cancel" Visible="true" />
                                                                        </td>
                                                                        <td>
                                                                            <div style="width: 193px; height: 16px;">
                                                                                <asp:Label ID="hidLCFLOB" runat="server" Text='<%# Bind("PrgParameterLOBName") %>'
                                                                                    Visible="false"></asp:Label>
                                                                                <asp:DropDownList ID="ddLCFLOB" DataSourceID="LCFsetupLOBDataSource" DataTextField="LookUpName"
                                                                                    DataValueField="LookUpID" Width="192px" runat="server">
                                                                                </asp:DropDownList>
                                                                                <asp:CompareValidator ID="CompddLCFLOB" runat="server" ControlToValidate="ddLCFLOB"
                                                                                    ValidationGroup="SaveLCFDtlsUpdt" ValueToCompare="0" Text="*" Display="Dynamic"
                                                                                    ErrorMessage="Please select a Line Of Business from dropdown box" Operator="NotEqual"></asp:CompareValidator>
                                                                            </div>
                                                                        </td>
                                                                        <td>
                                                                            <div style="width: 193px; height: 16px;">
                                                                                <asp:Label ID="hidLCFState" runat="server" Text='<%# Bind("PrgparameterStateFullname") %>'
                                                                                    Visible="false"></asp:Label>
                                                                                <asp:DropDownList ID="ddlLCFState" DataSourceID="LBAsetupStateDataSource" ValidationGroup="SaveLCFDtlsUpdt"
                                                                                    DataTextField="LookUpName" DataValueField="LookUpID" Width="192px" runat="server">
                                                                                </asp:DropDownList>
                                                                                <asp:CompareValidator ID="CompddlLCFState" runat="server" ControlToValidate="ddlLCFState"
                                                                                    ValidationGroup="SaveLCFDtlsUpdt" ValueToCompare="0" Text="*" Display="Dynamic"
                                                                                    ErrorMessage="Please select a State from dropdown box" Operator="NotEqual"></asp:CompareValidator>
                                                                            </div>
                                                                        </td>
                                                                        <td align="left">
                                                                            <div style="width: 193px; height: 16px;">
                                                                                <asp:Label ID="lblAdjParmtdtlLCFID" runat="server" Text='<%# Bind("prem_adj_pgm_dtl_id") %>'
                                                                                    Visible="false"></asp:Label>
                                                                                <asp:TextBox ID="txtLCFFactor" Text='<%# Bind("adj_fctr_rt","{0:0.000000}")%>' Width="192px" MaxLength="7"
                                                                                    runat="server"> </asp:TextBox>
                                                                                <cc1:MaskedEditExtender ID="MaskedEdittxtLCFFactor" runat="server" TargetControlID="txtLCFFactor"
                                                                                    Mask="9.999999" MessageValidatorTip="true" MaskType="Number" DisplayMoney="None"
                                                                                    OnFocusCssClass="MaskedEditFocus" OnInvalidCssClass="MaskedEditError" InputDirection="LeftToRight"
                                                                                    AcceptNegative="None" />
                                                                                <asp:RequiredFieldValidator ID="txtLCFFactorEdit" runat="server" Text="*" Display="Dynamic"
                                                                                    ErrorMessage="Please enter LCF Factor" ValidationGroup="SaveLCFDtlsUpdt" ControlToValidate="txtLCFFactor"></asp:RequiredFieldValidator>
                                                                            </div>
                                                                        </td>
                                                                        <td>
                                                                            <asp:CheckBox ID="chkActiveLCF" runat="server" Checked="true" Text="Active" />
                                                                        </td>
                                                                    </tr>
                                                                </EditItemTemplate>
                                                                <InsertItemTemplate>
                                                                    <tr class="AlternatingItemTemplate">
                                                                        <td align="center">
                                                                            <asp:LinkButton ID="lbLCFsetupdetailSave" CommandName="Save" ValidationGroup="SaveLCFDtls"
                                                                                Text="Save" runat="server" Visible="true" Height="16px" Width="30px" />
                                                                        </td>
                                                                        <td>
                                                                            <div style="width: 193px; height: 16px;">
                                                                                <asp:DropDownList ID="ddLCFLOB" DataSourceID="LCFsetupLOBDataSource" DataTextField="LookUpName"
                                                                                    DataValueField="LookUpID" Width="192px" runat="server">
                                                                                </asp:DropDownList>
                                                                                <asp:CompareValidator ID="CompddLCFLOB" runat="server" ControlToValidate="ddLCFLOB"
                                                                                    ValidationGroup="SaveLCFDtls" ValueToCompare="0" Text="*" Display="Dynamic" ErrorMessage="Please select a Line Of Business from dropdown box"
                                                                                    Operator="NotEqual"></asp:CompareValidator>
                                                                            </div>
                                                                        </td>
                                                                        <td>
                                                                            <div style="width: 193px; height: 16px;">
                                                                                <asp:DropDownList ID="ddlLCFState" DataSourceID="LBAsetupStateDataSource" DataTextField="LookUpName"
                                                                                    DataValueField="LookUpID" Width="192px" runat="server">
                                                                                </asp:DropDownList>
                                                                                <asp:CompareValidator ID="CompddlLCFState" runat="server" ControlToValidate="ddlLCFState"
                                                                                    ValidationGroup="SaveLCFDtls" ValueToCompare="0" Text="*" Display="Dynamic" ErrorMessage="Please select a State from dropdown box"
                                                                                    Operator="NotEqual"></asp:CompareValidator>
                                                                            </div>
                                                                        </td>
                                                                        <td>
                                                                            <div style="width: 193px; height: 16px;">
                                                                                <asp:TextBox ID="txtLCFFactor" Text="" ReadOnly="false" MaxLength="7" Width="191px"
                                                                                    runat="server"></asp:TextBox>
                                                                                <cc1:MaskedEditExtender ID="MaskedEdittxtLCFFactor" runat="server" TargetControlID="txtLCFFactor"
                                                                                    Mask="9.999999" MessageValidatorTip="true" MaskType="Number" DisplayMoney="None"
                                                                                    OnFocusCssClass="MaskedEditFocus" OnInvalidCssClass="MaskedEditError" InputDirection="LeftToRight"
                                                                                    AcceptNegative="None" />
                                                                                <asp:RequiredFieldValidator ID="txtLCFFactorRate" runat="server" Text="*" Display="Dynamic"
                                                                                    ErrorMessage="Please enter LCF factor" ValidationGroup="SaveLCFDtls" ControlToValidate="txtLCFFactor"></asp:RequiredFieldValidator>
                                                                            </div>
                                                                        </td>
                                                                        <td>
                                                                        </td>
                                                                    </tr>
                                                                </InsertItemTemplate>
                                                            </asp:AISListView>
                                                        </asp:Panel>
                                                    </div>
                                                </td>
                                            </tr>
                                        </table>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                        </ContentTemplate>
                    </cc1:TabPanel>
                    <cc1:TabPanel runat="server" ID="tblpnlTM" HeaderText="Tax Multiplier">
                        <HeaderTemplate>
                            WA TM & Assess
                        </HeaderTemplate>
                        <ContentTemplate>
                            <div class="gridTM">
                                <asp:UpdatePanel ID="UpdatepanelTM" runat="server" UpdateMode="Conditional" Visible="true">
                                    <ContentTemplate>
                                        <asp:Panel ID="pnlAdjTM" runat="server" Visible="False">
                                            <table id="tblAdjTM" visible="true" runat="server" class="panelContents">
                                                <tr align="center">
                                                    <th>
                                                    </th>
                                                    <th>
                                                        Policy #'s
                                                    </th>
                                                    <th>
                                                        Details
                                                    </th>
                                                    <th>
                                                        Disable
                                                    </th>
                                                </tr>
                                                <tr class="ItemTemplate" style="vertical-align: middle">
                                                    <td style="vertical-align: middle; width: 80px">
                                                        <asp:LinkButton ID="lnkBtnTMSave" runat="server" Text="Save" Visible="false" Width="80px"
                                                            OnClick="btnTMInfoDetailsSave_Click" />
                                                        <asp:LinkButton ID="lnkBtnTMUpd" runat="server" Text="Update" Visible="false" Width="80px"
                                                            OnClick="btnTMInfoDetailsSave_Click" />
                                                        <asp:LinkButton ID="lnkBtnTMCancel" runat="server" Text="Cancel" Visible="true" Width="80px"
                                                            OnClick="LBASetupCancel_Click" />
                                                    </td>
                                                    <td>
                                                        <asp:Panel ID="ChkPolicyPanelTM" runat="server" ScrollBars="Auto" CssClass="content"
                                                            Height="70px">
                                                            <asp:CheckBoxList ID="ChkBoxLstTMpolicy" runat="server" ForeColor="Black" ReadOnly="true"
                                                                BackColor="White" BorderColor="Black">
                                                            </asp:CheckBoxList>
                                                        </asp:Panel>
                                                    </td>
                                                    <td style="vertical-align: middle">
                                                        <asp:LinkButton ID="LnkbtnTMdtls" runat="server" CommandName="Edit" Text="Details"
                                                            Visible="true" Width="120px" OnClick="btnTMInfoDetailsInfo_Click" />
                                                    </td>
                                                    <td style="vertical-align: middle;">
                                                        <asp:ImageButton ID="imgbtnDisableTM" OnCommand='DisableTMRow' CommandArgument='<%# Bind("adj_paramet_setup_id") %>'
                                                            runat="server" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </asp:Panel>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                                <asp:UpdatePanel ID="UpdatepanelTMdtls" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:TextBox ID="hdnTMPrmsetuptxtBox" Visible="False" runat="server"></asp:TextBox>
                                        <table id="tblTMdtls" runat="server" class="panelContents" width="220px">
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblTMdtls" runat="server" Text="Tax Multiplier" CssClass="h3" Width="120px"
                                                        Visible="false"></asp:Label>&nbsp;&nbsp;<asp:LinkButton ID="lnkBtnCloseTMdtlsID"
                                                            Text="Close" runat="server" Visible="false" OnClick="btnTMInfoClose_Click" />
                                                    <div id="lstviewTMDiv" class="content" style="height: 157px; width: 420px; overflow: auto">
                                                        <asp:Panel ID="pnlTMdtls" runat="server" Height="20px" Width="100px">
                                                            <asp:AISListView ID="lstTMSetuplistView" runat="server" Visible="True" OnItemEditing="lstTMParmsetupDtls_ItemEdit"
                                                                OnItemDataBound="lstTMParmsetupDtls_DataBoundList" InsertItemPosition="FirstItem"
                                                                OnItemCanceling="lstTMParmsetupDtls_ItemCancel" OnItemUpdating="lstTMParmsetupDtls_ItemUpdating"
                                                                OnItemCommand="lstTMParmsetupDtls_ItemCommand">
                                                                <LayoutTemplate>
                                                                    <table id="tblLayoutTMdetails" class="ItemTemplate" runat="server">
                                                                        <tr>
                                                                            <th>
                                                                            </th>
                                                                            <th align="center">
                                                                                State
                                                                            </th>
                                                                            <th align="center">
                                                                                TM Factor
                                                                            </th>
                                                                            <th align="center">
                                                                                Premium Assess. Factor
                                                                            </th>
                                                                            <th align="center" style="width: 20px; height: 20px;">
                                                                                Disable
                                                                            </th>
                                                                        </tr>
                                                                        <tr id="ItemPlaceHolder" runat="server">
                                                                        </tr>
                                                                    </table>
                                                                </LayoutTemplate>
                                                                <ItemTemplate>
                                                                    <tr class="ItemTemplate">
                                                                        <td id="tdDetailsEdit" align="center" runat="server">
                                                                            <asp:LinkButton ID="TMdetailsetupEdit" CommandName="Edit" Text="Edit" runat="server"
                                                                                Visible="true" />
                                                                        </td>
                                                                        <td>
                                                                            <div style="width: 121px; height: 16px;">
                                                                                <asp:Label ID="lblTMStateID" Text='<%# Bind("PrgParameterStateName") %>' runat="server"></asp:Label>
                                                                            </div>
                                                                        </td>
                                                                        <td>
                                                                            <div style="width: 76px; height: 16px;">
                                                                                <asp:Label ID="lblTMPercent" Text='<%# Bind("adj_fctr_rt","{0:0.000000}") %>' runat="server"></asp:Label>
                                                                            </div>
                                                                        </td>
                                                                        <td>
                                                                            <div style="width: 76px; height: 16px;">
                                                                                <asp:Label ID="lblAdjParmtdtlTMID" runat="server" Text='<%# Bind("prem_adj_pgm_dtl_id") %>'
                                                                                    Visible="false"></asp:Label>
                                                                                <asp:Label ID="lblTMpremAssessment" Text='<%# Bind("PremAssementAmt","{0:0.000000}") %>' runat="server"></asp:Label>
                                                                            </div>
                                                                        </td>
                                                                        <td>
                                                                            <div style="width: 20px; height: 16px;">
                                                                                <asp:HiddenField ID="hidActiveTM" runat="server" Value='<%# Bind("act_ind")%>' />
                                                                                <asp:ImageButton ID="imgDisableEnableTM" CommandArgument='<%# Bind("prem_adj_pgm_dtl_id") %>'
                                                                                    runat="server" ImageUrl='<%# Eval("act_ind").ToString()=="True"?"~/images/disabled.GIF":"~/images/enabled.GIF" %>'>
                                                                                </asp:ImageButton>
                                                                            </div>
                                                                        </td>
                                                                    </tr>
                                                                </ItemTemplate>
                                                                <AlternatingItemTemplate>
                                                                    <tr class="AlternatingItemTemplate">
                                                                        <td id="tdDetailsEdit" align="center" runat="server">
                                                                            <asp:LinkButton ID="TMdetailsetupEdit" CommandName="Edit" Text="Edit" runat="server"
                                                                                Visible="true" />
                                                                        </td>
                                                                        <td>
                                                                            <div style="width: 121px; height: 16px;">
                                                                                <asp:Label ID="lblTMStateID" Text='<%# Bind("PrgParameterStateName") %>' runat="server"></asp:Label>
                                                                            </div>
                                                                        </td>
                                                                        <td>
                                                                            <div style="width: 76px; height: 16px;">
                                                                                <asp:Label ID="lblTMPercent" Text='<%# Bind("adj_fctr_rt","{0:0.000000}") %>' runat="server"></asp:Label>
                                                                            </div>
                                                                        </td>
                                                                        <td>
                                                                            <div style="width: 76px; height: 16px;">
                                                                                <asp:Label ID="lblAdjParmtdtlTMID" runat="server" Text='<%# Bind("prem_adj_pgm_dtl_id") %>'
                                                                                    Visible="false"></asp:Label>
                                                                                <asp:Label ID="lblTMpremAssessment" Text='<%# Bind("PremAssementAmt","{0:0.000000}") %>' runat="server"></asp:Label>
                                                                            </div>
                                                                        </td>
                                                                        <td>
                                                                            <div style="width: 20px; height: 16px;">
                                                                                <asp:HiddenField ID="hidActiveTM" runat="server" Value='<%# Bind("act_ind")%>' />
                                                                                <asp:ImageButton ID="imgDisableEnableTM" CommandArgument='<%# Bind("prem_adj_pgm_dtl_id") %>'
                                                                                    runat="server" ImageUrl='<%# Eval("act_ind").ToString()=="True"?"~/images/disabled.GIF":"~/images/enabled.GIF" %>'>
                                                                                </asp:ImageButton>
                                                                            </div>
                                                                        </td>
                                                                    </tr>
                                                                </AlternatingItemTemplate>
                                                                <EditItemTemplate>
                                                                    <tr style="width: 100%">
                                                                        <td>
                                                                            <asp:LinkButton ID="lnkTMprmtSetupUpdate" CommandName="Update" ValidationGroup="SaveTMDtlsUpdt"
                                                                                Text="Update" runat="server" Visible="true" /><br><asp:LinkButton ID="lnkCombElemsCancel"
                                                                                    CommandName="Cancel" runat="server" Text="Cancel" Visible="true" />
                                                                        </td>
                                                                        <td>
                                                                            <div style="width: 121px; height: 16px;">
                                                                                <asp:Label ID="hidTMState" runat="server" Text='<%# Bind("PrgparameterStateFullname") %>'
                                                                                    Visible="false"></asp:Label>
                                                                                <asp:DropDownList ID="ddlTMState" DataSourceID="LBAsetupStateDataSource" DataTextField="LookUpName"
                                                                                    DataValueField="LookUpID" Width="120px" runat="server">
                                                                                </asp:DropDownList>
                                                                                <asp:CompareValidator ID="CompddlTMState" runat="server" ControlToValidate="ddlTMState"
                                                                                    ValidationGroup="SaveTMDtlsUpdt" ValueToCompare="0" Text="*" Display="Dynamic"
                                                                                    ErrorMessage="Please select a State from dropdown box" Operator="NotEqual"></asp:CompareValidator>
                                                                            </div>
                                                                        </td>
                                                                        <td>
                                                                            <div style="width: 76px; height: 16px;">
                                                                                <asp:TextBox ID="txtTMFactor" Text='<%# Bind("adj_fctr_rt","{0:0.000000}")%>' Width="75px" runat="server"> </asp:TextBox>
                                                                                <cc1:MaskedEditExtender ID="MaskedEdittxtTMFactor" runat="server" TargetControlID="txtTMFactor"
                                                                                    Mask="9.999999" MessageValidatorTip="true" MaskType="Number" DisplayMoney="None"
                                                                                    OnFocusCssClass="MaskedEditFocus" OnInvalidCssClass="MaskedEditError" InputDirection="LeftToRight"
                                                                                    AcceptNegative="None" />
                                                                                <asp:RequiredFieldValidator ID="txtTMFactorEdit" runat="server" Text="*" Display="Dynamic"
                                                                                    ErrorMessage="Please enter Tax Multiplier Percentage" ValidationGroup="SaveTMDtlsUpdt"
                                                                                    ControlToValidate="txtTMFactor"></asp:RequiredFieldValidator>
                                                                            </div>
                                                                        </td>
                                                                        <td>
                                                                            <div style="width: 76px; height: 16px;">
                                                                                <asp:Label ID="lblAdjParmtdtlTMID" runat="server" Text='<%# Bind("prem_adj_pgm_dtl_id") %>'
                                                                                    Visible="false"></asp:Label>
                                                                                <asp:TextBox ID="txtPremAssessmentTM" Text='<%# Bind("PremAssementAmt","{0:0.000000}")%>' Width="75px"
                                                                                    runat="server"> </asp:TextBox>
                                                                                <cc1:MaskedEditExtender ID="MaskedtxtPremAssessmentTM" runat="server" TargetControlID="txtPremAssessmentTM"
                                                                                    Mask="9.999999" MessageValidatorTip="true" MaskType="Number" DisplayMoney="None"
                                                                                    OnFocusCssClass="MaskedEditFocus" OnInvalidCssClass="MaskedEditError" InputDirection="LeftToRight"
                                                                                    AcceptNegative="None" />
                                                                            </div>
                                                                        </td>
                                                                        <td>
                                                                            <asp:CheckBox ID="chkActiveTM" runat="server" Checked="true" Text="Active" />
                                                                        </td>
                                                                    </tr>
                                                                </EditItemTemplate>
                                                                <InsertItemTemplate>
                                                                    <tr class="AlternatingItemTemplate">
                                                                        <td align="center">
                                                                            <asp:LinkButton ID="lblTMsetupdetailSave" CommandName="Save" ValidationGroup="SaveTMDtls"
                                                                                Text="Save" runat="server" Visible="true" Height="16px" Width="30px" />
                                                                        </td>
                                                                        <td>
                                                                            <div style="width: 121px; height: 16px;">
                                                                                <asp:DropDownList ID="ddlTMState" DataSourceID="LBAsetupStateDataSource" DataTextField="LookUpName"
                                                                                    DataValueField="LookUpID" Width="120px" runat="server">
                                                                                </asp:DropDownList>
                                                                                <asp:CompareValidator ID="CompddlTMState" runat="server" ControlToValidate="ddlTMState"
                                                                                    ValidationGroup="SaveTMDtls" ValueToCompare="0" Text="*" Display="Dynamic" ErrorMessage="Please select a State from dropdown box"
                                                                                    Operator="NotEqual"></asp:CompareValidator>
                                                                            </div>
                                                                        </td>
                                                                        <td>
                                                                            <div style="width: 76px; height: 16px;">
                                                                                <asp:TextBox ID="txtTMFactor" Text="" ReadOnly="false" ValidationGroup="SaveTMDtls"
                                                                                    Width="75px" runat="server"></asp:TextBox>
                                                                                <cc1:MaskedEditExtender ID="MaskedEdittxtTMFactor" runat="server" TargetControlID="txtTMFactor"
                                                                                    Mask="9.999999" MessageValidatorTip="true" MaskType="Number" DisplayMoney="None"
                                                                                    OnFocusCssClass="MaskedEditFocus" OnInvalidCssClass="MaskedEditError" InputDirection="LeftToRight"
                                                                                    AcceptNegative="None" />
                                                                                <asp:RequiredFieldValidator ID="txtTMFactoRate" runat="server" Text="*" Display="Dynamic"
                                                                                    ErrorMessage="Please enter Tax Multiplier Percentage" ValidationGroup="SaveTMDtls"
                                                                                    ControlToValidate="txtTMFactor"></asp:RequiredFieldValidator>
                                                                            </div>
                                                                        </td>
                                                                        <td>
                                                                            <div style="width: 76px; height: 16px;">
                                                                                <asp:TextBox ID="txtPremAssessmentTM" Text='<%# Bind("PremAssementAmt")%>' Width="75px"
                                                                                    runat="server"> </asp:TextBox>
                                                                                <cc1:MaskedEditExtender ID="MaskedtxtPremAssessmentTM" runat="server" TargetControlID="txtPremAssessmentTM"
                                                                                    Mask="9.999999" MessageValidatorTip="true" MaskType="Number" DisplayMoney="None"
                                                                                    OnFocusCssClass="MaskedEditFocus" OnInvalidCssClass="MaskedEditError" InputDirection="LeftToRight"
                                                                                    AcceptNegative="None" />
                                                                            </div>
                                                                        </td>
                                                                        <td>
                                                                        </td>
                                                                    </tr>
                                                                </InsertItemTemplate>
                                                            </asp:AISListView>
                                                        </asp:Panel>
                                                    </div>
                                                </td>
                                            </tr>
                                        </table>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                        </ContentTemplate>
                    </cc1:TabPanel>
                    <cc1:TabPanel runat="server" ID="tblpnlCHF" HeaderText="CHF">
                        <HeaderTemplate>
                            CHF
                        </HeaderTemplate>
                        <ContentTemplate>
                            <div class="gridCHF">
                                <asp:UpdatePanel ID="UpdatepanelCHF" runat="server" UpdateMode="Conditional" Visible="true">
                                    <ContentTemplate>
                                        <asp:Panel ID="pnlADlCHF" runat="server" Visible="false">
                                            <table id="tblchfadj" visible="true" runat="server" class="panelContents">
                                                <tr align="center">
                                                    <th>
                                                    </th>
                                                    <th>
                                                        Policy #'s
                                                    </th>
                                                    <th style="width: 50px">
                                                        Part Of ERP
                                                    </th>
                                                    <th>
                                                        Basis Charged
                                                    </th>
                                                    <th>
                                                        CHF Deposit
                                                    </th>
                                                    <th>
                                                        Details
                                                    </th>
                                                    <th>
                                                        DISABLE
                                                    </th>
                                                </tr>
                                                <tr class="ItemTemplate" style="vertical-align: middle">
                                                    <td style="vertical-align: middle">
                                                        <asp:LinkButton ID="lnkBtnCHFadj" runat="server" Text="Save" ValidationGroup="SaveCHF"
                                                            Visible="False" Width="42px" OnClick="btnCHFInfoDetailsSave_Click" />
                                                        <asp:LinkButton ID="lnkBtnCHFadjUpd" runat="server" ValidationGroup="SaveCHF" Text="Update"
                                                            Visible="False" Width="42px" OnClick="btnCHFInfoDetailsSave_Click" />
                                                        <asp:LinkButton ID="lnkBtnCHFCancel" runat="server" Text="Cancel" Visible="true"
                                                            Width="42px" OnClick="LBASetupCancel_Click" />
                                                    </td>
                                                    <td>
                                                        <asp:Panel ID="PnlCHFpolicynumberlist" runat="server" ScrollBars="Auto" CssClass="content"
                                                            Height="20px">
                                                            <asp:CheckBoxList ID="chkbxCHFPolicynolst" runat="server" ForeColor="Black" ReadOnly="true"
                                                                BackColor="White" BorderColor="Black">
                                                            </asp:CheckBoxList>
                                                        </asp:Panel>
                                                    </td>
                                                    <td style="vertical-align: middle">
                                                        <asp:CheckBox ID="rbtnCHFinclude" runat="server" Enabled="true" Width="50px"></asp:CheckBox>
                                                    </td>
                                                    <td style="vertical-align: middle">
                                                        <asp:DropDownList ID="ddCHFBasisCharged" runat="server" Enabled="true" Width="120px">
                                                        </asp:DropDownList>
                                                        <asp:CompareValidator ID="CompddCHFBasisCharged" runat="server" ControlToValidate="ddCHFBasisCharged"
                                                            ValidationGroup="SaveCHF" ValueToCompare="0" Text="*" Display="Dynamic" ErrorMessage="Please select Basis Charged from dropdown"
                                                            Operator="NotEqual"></asp:CompareValidator>
                                                    </td>
                                                    <td style="vertical-align: middle">
                                                        <asp:AISAmountTextbox ID="txtCHFDeposit" runat="server"  Enabled="True" Width="92px" ></asp:AISAmountTextbox>
                                                       <%--<cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" TargetControlID="txtCHFDeposit"
                                                            FilterType="Custom" FilterMode="ValidChars" ValidChars="0123456789," />--%>
                                                       
                                                    </td>
                                                    <td style="vertical-align: middle">
                                                        <asp:LinkButton ID="lnkbtnCHFinfoDetails" runat="server" CommandName="Edit" Text="Details"
                                                            Visible="true" Width="81px" OnClick="btnCHFInfoDetailsInfo_Click" />
                                                    </td>
                                                    <td style="vertical-align: middle">
                                                        <asp:ImageButton ID="imgbtnCHFdisable" OnCommand='DisableCHFfirstRow' CommandArgument='<%# Bind("adjt_param_dtl_id") %>'
                                                            runat="server" />
                                                    </td>
                                                </tr>
                                                <tr class="AlternatingItemTemplate" style="vertical-align: middle">
                                                    <td style="vertical-align: middle">
                                                        <asp:LinkButton ID="lnkBtn2CHFadj" runat="server" ValidationGroup="SaveCHF2" CommandName="Edit"
                                                            Text="Save" Visible="False" Width="42px" OnClick="btnCHFInfo2DetailsSave_Click" />
                                                        <asp:LinkButton ID="lnkBtn2CHFadjUpd" runat="server" ValidationGroup="SaveCHF2" CommandName="Edit"
                                                            Text="Update" Visible="False" Width="42px" OnClick="btnCHFInfo2DetailsSave_Click" />
                                                        <asp:LinkButton ID="lnkBtnCHF2Cancel" runat="server" Text="Cancel" Visible="true"
                                                            Width="42px" OnClick="LBASetupCancel_Click" />
                                                    </td>
                                                    <td>
                                                        <asp:Panel ID="PnlCHF2policynumberlist" runat="server" ScrollBars="Auto" CssClass="content"
                                                            Height="20px">
                                                            <asp:CheckBoxList ID="chkbxCHF2Policynolst" runat="server" ForeColor="Black" ReadOnly="true"
                                                                BackColor="White" BorderColor="Black">
                                                            </asp:CheckBoxList>
                                                        </asp:Panel>
                                                    </td>
                                                    <td style="vertical-align: middle">
                                                        <asp:CheckBox ID="rbtnCHFNotinclude" runat="server" Enabled="true" Width="50px">
                                                        </asp:CheckBox>
                                                    </td>
                                                    <td style="vertical-align: middle">
                                                        <asp:DropDownList ID="ddCHFBasisCharged2" runat="server" Enabled="true" Width="120px">
                                                        </asp:DropDownList>
                                                        <asp:CompareValidator ID="CompddCHFBasisCharged2" runat="server" ControlToValidate="ddCHFBasisCharged2"
                                                            ValidationGroup="SaveCHF2" ValueToCompare="0" Text="*" Display="Dynamic" ErrorMessage="Please select Basis Charged from dropdown"
                                                            Operator="NotEqual"></asp:CompareValidator>
                                                    </td>
                                                    <td style="vertical-align: middle">
                                                        <asp:AISAmountTextbox ID="txtchfDeposit2" runat="server"  Enabled="True" Width="92px" ></asp:AISAmountTextbox>
                                                       
                                                             <%--<cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server" TargetControlID="txtCHFDeposit"
                                                            FilterType="Custom" FilterMode="ValidChars" ValidChars="0123456789," />--%>
                                                       
                                                    </td>
                                                    <td style="vertical-align: middle">
                                                        <asp:LinkButton ID="lnkbtnCHF2infoDetails" runat="server" CommandName="Edit" Text="Details"
                                                            Visible="true" Width="81px" OnClick="btnCHF2InfoDetailsInfo_Click" />
                                                    </td>
                                                    <td style="vertical-align: middle">
                                                        <asp:ImageButton ID="imgbtn2CHFdisable" OnCommand='DisableCHFSecndRow' CommandArgument='<%# Bind("adjt_param_dtl_id") %>'
                                                            runat="server" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </asp:Panel>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                                <asp:UpdatePanel ID="UpdatepanelCHFDetails" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:TextBox ID="hdnCHFPrmsetup1txtBox" Visible="False" runat="server"></asp:TextBox>
                                        <asp:TextBox ID="hdnCHFPrmsetup2txtBox" Visible="False" runat="server"></asp:TextBox>
                                        <table id="tbdAdjCHFdtls" runat="server" class="panelContents" width="930px">
                                            <tr>
                                                <td>
                                                    <asp:Label ID="CHFSetupDetailsLabel" runat="server" Text="CHF Setup - Details" CssClass="h3"
                                                        Width="135px" Visible="false"></asp:Label>&nbsp;&nbsp;<asp:LinkButton ID="lnkbtnCHFDetailsClose"
                                                            Text="Close" runat="server" Visible="false" OnClick="btnCHFInfoClose_Click" />
                                                    <div class="content" style="height: 124px; width: 718px; overflow: auto">
                                                        <asp:Panel ID="pnlChfLbaPolicy" runat="server" Height="20px" Width="50px">
                                                            <asp:AISListView ID="chfsetupdetailslistview" runat="server" Visible="false" OnItemEditing="lstCHFParmsetupDtls_ItemEdit"
                                                                OnItemDataBound="lstCHFParmsetupDtls_DataBoundList" InsertItemPosition="FirstItem"
                                                                OnItemCanceling="lstCHFParmsetupDtls_ItemCancel" OnSorting="lstCHFParmsetupDtls_Sorting"
                                                                OnItemUpdating="lstCHFParmsetupDtls_ItemUpdating" OnItemCommand="lstCHFParmsetupDtls_ItemCommand">
                                                                <LayoutTemplate>
                                                                    <table id="tblLayoutCHFdetails" class="ItemTemplate" runat="server" width="700px">
                                                                        <tr>
                                                                            <th>
                                                                            </th>
                                                                            <th align="center">
                                                                                State
                                                                            </th>
                                                                            <th align="center">
                                                                                <asp:LinkButton ID="hlCHFSort" runat="server" CommandName="Sort" CommandArgument="PrgParameterCHFLName">
                                                                    CHF LossType
                                                                                </asp:LinkButton>
                                                                                <asp:ImageButton ID="imgCHFSort" Visible="false" ToolTip="Ascending" ImageUrl="~/images/ascending.gif"
                                                                                    runat="server" />
                                                                            </th>
                                                                            <th align="center">
                                                                                Claim(ant)'s(#Of)
                                                                            </th>
                                                                            <th align="center">
                                                                                Claim Rate
                                                                            </th>
                                                                            <th align="center" style="width: 18px; height: 20px;">
                                                                                Disable
                                                                            </th>
                                                                        </tr>
                                                                        <tr id="ItemPlaceHolder" runat="server">
                                                                        </tr>
                                                                    </table>
                                                                </LayoutTemplate>
                                                                <ItemTemplate>
                                                                    <tr class="ItemTemplate">
                                                                        <td id="tdCHFDetailsEdit" align="center" runat="server">
                                                                            <asp:LinkButton ID="ChfdetailsetupEdit" CommandName="Edit" Text="Edit" runat="server"
                                                                                Visible="true" OnClick="btnchfsetupinfodetails_click" />
                                                                        </td>
                                                                        <td>
                                                                            <div style="width: 75px; height: 16px;">
                                                                                <asp:Label ID="lblCHFStateID" Text='<%# Bind("PrgParameterStateName") %>' runat="server"></asp:Label>
                                                                            </div>
                                                                        </td>
                                                                        <td>
                                                                            <div style="width: 200px; height: 16px;">
                                                                                <asp:Label ID="lblCHFLosstype" Text='<%# Bind("PrgParameterCHFLName") %>' runat="server"></asp:Label>
                                                                            </div>
                                                                        </td>
                                                                        <td>
                                                                            <div style="width: 80px; height: 16px;">
                                                                                <asp:Label ID="lblAdjParmtdtlCHFID" runat="server" Text='<%# Bind("prem_adj_pgm_dtl_id") %>'
                                                                                    Visible="false"></asp:Label>
                                                                                <asp:Label ID="lblCHFnumber" Width="79px" Text='<%#Eval("CHF_CLMT_NUMBER")==null?String.Empty:decimal.Parse(Eval("CHF_CLMT_NUMBER").ToString()).ToString("#,##0") %>'
                                                                                    runat="server"></asp:Label>
                                                                            </div>
                                                                        </td>
                                                                        <td>
                                                                            <div style="width: 90px; height: 16px;">
                                                                                <asp:Label ID="lblCHFClaimRate" Width="89px" Text='<%#Eval("Clm_hndlfee_clmrate")==null?String.Empty:decimal.Parse(Eval("Clm_hndlfee_clmrate").ToString()).ToString("#,##0") %>'
                                                                                    runat="server"></asp:Label>
                                                                            </div>
                                                                        </td>
                                                                        <td>
                                                                            <div style="width: 20px; height: 16px;">
                                                                                <asp:HiddenField ID="hidActiveCHF" runat="server" Value='<%# Bind("act_ind")%>' />
                                                                                <asp:ImageButton ID="imgDisableEnableCHF" CommandArgument='<%# Bind("prem_adj_pgm_dtl_id") %>'
                                                                                    runat="server" ImageUrl='<%# Eval("act_ind").ToString()=="True"?"~/images/disabled.GIF":"~/images/enabled.GIF" %>'>
                                                                                </asp:ImageButton>
                                                                            </div>
                                                                        </td>
                                                                    </tr>
                                                                </ItemTemplate>
                                                                <AlternatingItemTemplate>
                                                                    <tr class="AlternatingItemTemplate">
                                                                        <td id="tdDetailsEdit" align="center" runat="server">
                                                                            <asp:LinkButton ID="ChfdetailsetupEdit" CommandName="Edit" Text="Edit" runat="server"
                                                                                Visible="true" OnClick="btnchfsetupinfodetails_click" />
                                                                        </td>
                                                                        <td>
                                                                            <div style="width: 75px; height: 16px;">
                                                                                <asp:Label ID="lblCHFStateID" Text='<%# Bind("PrgParameterStateName") %>' runat="server"></asp:Label>
                                                                            </div>
                                                                        </td>
                                                                        <td>
                                                                            <div style="width: 200px; height: 16px;">
                                                                                <asp:Label ID="lblCHFLosstype" Text='<%# Bind("PrgParameterCHFLName") %>' runat="server"></asp:Label>
                                                                            </div>
                                                                        </td>
                                                                        <td>
                                                                            <div style="width: 80px; height: 16px;">
                                                                                <asp:Label ID="lblAdjParmtdtlCHFID" runat="server" Text='<%# Bind("prem_adj_pgm_dtl_id") %>'
                                                                                    Visible="false"></asp:Label>
                                                                                <asp:Label ID="lblCHFnumber" Width="79px" Text='<%#Eval("CHF_CLMT_NUMBER")==null?String.Empty:decimal.Parse(Eval("CHF_CLMT_NUMBER").ToString()).ToString("#,##0") %>'
                                                                                    runat="server"></asp:Label>
                                                                            </div>
                                                                        </td>
                                                                        <td>
                                                                            <div style="width: 90px; height: 16px;">
                                                                                <asp:Label ID="lblCHFClaimRate" Width="89px" Text='<%#Eval("Clm_hndlfee_clmrate")==null?String.Empty:decimal.Parse(Eval("Clm_hndlfee_clmrate").ToString()).ToString("#,##0") %>'
                                                                                    runat="server"></asp:Label>
                                                                            </div>
                                                                        </td>
                                                                        <td>
                                                                            <div style="width: 20px; height: 16px;">
                                                                                <asp:HiddenField ID="hidActiveCHF" runat="server" Value='<%# Bind("act_ind")%>' />
                                                                                <asp:ImageButton ID="imgDisableEnableCHF" CommandArgument='<%# Bind("prem_adj_pgm_dtl_id") %>'
                                                                                    runat="server" ImageUrl='<%# Eval("act_ind").ToString()=="True"?"~/images/disabled.GIF":"~/images/enabled.GIF" %>'>
                                                                                </asp:ImageButton>
                                                                            </div>
                                                                        </td>
                                                                    </tr>
                                                                </AlternatingItemTemplate>
                                                                <EditItemTemplate>
                                                                    <tr style="width: 100%">
                                                                        <td>
                                                                            <asp:LinkButton ID="lnkChfprmtSetupUpdate" CommandName="Update" ValidationGroup="SaveCHFDtlsUpdt"
                                                                                Text="Update" runat="server" Visible="true" /><br /><asp:LinkButton ID="lnkCHFCancel" CommandName="Cancel"
                                                                                    runat="server" Text="Cancel" Visible="true" />
                                                                        </td>
                                                                        <td>
                                                                            <div style="width: 75px; height: 16px;">
                                                                                <asp:Label ID="hidCHFState" runat="server" Text='<%# Bind("PrgparameterStateFullname") %>'
                                                                                    Visible="false"></asp:Label>
                                                                                <asp:DropDownList ID="ddCHFState" DataSourceID="LBAsetupStateDataSource" ValidationGroup="SaveCHFDtlsUpdt"
                                                                                    DataTextField="LookUpName" DataValueField="LookUpID" runat="server">
                                                                                </asp:DropDownList>
                                                                                <asp:CompareValidator ID="CompddlCHFState" runat="server" ControlToValidate="ddCHFState"
                                                                                    ValidationGroup="SaveCHFDtlsUpdt" ValueToCompare="0" Text="*" Display="Dynamic"
                                                                                    ErrorMessage="Please select a State from dropdown box" Operator="NotEqual"></asp:CompareValidator>
                                                                            </div>
                                                                        </td>
                                                                        <td>
                                                                            <div style="width: 75px; height: 16px;">
                                                                                <asp:Label ID="hidCHFLosstype" runat="server" Text='<%# Bind("PrgParameterCHFLName") %>'
                                                                                    Visible="false"></asp:Label>
                                                                                <asp:DropDownList ID="ddCHFLosstype" DataSourceID="CHFLosstypeDataSource" ValidationGroup="SaveCHFDtlsUpdt"
                                                                                    DataTextField="LookUpName" DataValueField="LookUpID" runat="server">
                                                                                </asp:DropDownList>
                                                                                <asp:CompareValidator ID="CompddCHFLosstype" runat="server" ControlToValidate="ddCHFLosstype"
                                                                                    ValidationGroup="SaveCHFDtlsUpdt" ValueToCompare="0" Text="*" Display="Dynamic"
                                                                                    ErrorMessage="Please select CHF Loss Type from dropdown box" Operator="NotEqual"></asp:CompareValidator>
                                                                            </div>
                                                                        </td>
                                                                        <td>
                                                                            <div style="width: 80px; height: 16px;">
                                                                                <asp:Label ID="lblAdjParmtdtlCHFID" runat="server" Text='<%# Bind("prem_adj_pgm_dtl_id") %>'
                                                                                    Visible="false"></asp:Label>
                                                                                <asp:AISAmountTextbox ID="txtCHFclaimntsnum" Width="90px" Text='<%# Bind("CHF_CLMT_NUMBER","{0:0}")%>'
                                                                                    runat="server" > </asp:AISAmountTextbox>
                                                                                <%--<cc1:FilteredTextBoxExtender ID="FilteredtxtCHFFactorExtender" runat="server" TargetControlID="txtCHFclaimntsnum"
                                                                                    FilterType="Custom" FilterMode="ValidChars" ValidChars="0123456789," />--%>
                                                                            </div>
                                                                        </td>
                                                                        <td>
                                                                            <div style="width: 90px; height: 16px;">
                                                                                <asp:AISAmountTextbox ID="txtCHFClaimRate" Width="89px" Text='<%# Bind("Clm_hndlfee_clmrate","{0:0}")%>'
                                                                                    runat="server" > </asp:AISAmountTextbox>
                                                                                    <%--<cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender1"
                                                                                        runat="server" TargetControlID="txtCHFClaimRate" FilterType="Custom" FilterMode="ValidChars"
                                                                                        ValidChars="0123456789," />--%>
                                                                                <asp:RequiredFieldValidator ID="RequiredCHFClaimText" runat="server" Text="*" Display="Dynamic"
                                                                                    ErrorMessage="Please enter CHF claim rate" ValidationGroup="SaveCHFDtlsUpdt"
                                                                                    ControlToValidate="txtCHFClaimRate"></asp:RequiredFieldValidator>
                                                                            </div>
                                                                        </td>
                                                                        <td>
                                                                            <asp:CheckBox ID="chkActiveCHF" runat="server" Checked="true" Text="Active" />
                                                                        </td>
                                                                    </tr>
                                                                </EditItemTemplate>
                                                                <InsertItemTemplate>
                                                                    <tr class="AlternatingItemTemplate">
                                                                        <td align="center">
                                                                            <asp:LinkButton ID="lbCHFsetupdetailSave" CommandName="Save" ValidationGroup="SaveCHFDtls"
                                                                                Text="Save" runat="server" Visible="true" Height="16px" Width="10px" />
                                                                        </td>
                                                                        <td>
                                                                            <div style="width: 75px; height: 16px;">
                                                                                <asp:DropDownList ID="ddCHFState" DataSourceID="LBAsetupStateDataSource" ValidationGroup="SaveCHFDtls"
                                                                                    DataTextField="LookUpName" DataValueField="LookUpID" runat="server">
                                                                                </asp:DropDownList>
                                                                                <asp:CompareValidator ID="CompddlCHFState" runat="server" ControlToValidate="ddCHFState"
                                                                                    ValidationGroup="SaveCHFDtls" ValueToCompare="0" Text="*" Display="Dynamic" ErrorMessage="Please select a State from dropdown box"
                                                                                    Operator="NotEqual"></asp:CompareValidator>
                                                                            </div>
                                                                        </td>
                                                                        <td>
                                                                            <div style="width: 75px; height: 16px;">
                                                                                <asp:DropDownList ID="ddCHFLosstype" DataSourceID="CHFLosstypeDataSource" ValidationGroup="SaveCHFDtls"
                                                                                    DataTextField="LookUpName" DataValueField="LookUpID" runat="server">
                                                                                </asp:DropDownList>
                                                                                <asp:CompareValidator ID="CompddCHFLosstype" runat="server" ControlToValidate="ddCHFLosstype"
                                                                                    ValidationGroup="SaveCHFDtls" ValueToCompare="0" Text="*" Display="Dynamic" ErrorMessage="Please select CHF Loss Type from dropdown box"
                                                                                    Operator="NotEqual"></asp:CompareValidator>
                                                                            </div>
                                                                        </td>
                                                                        <td>
                                                                            <div style="width: 80px; height: 16px;">
                                                                                <asp:AISAmountTextbox ID="txtCHFclaimntsnum" Text="" Width="90px" ReadOnly="false" 
                                                                                    runat="server" ></asp:AISAmountTextbox>
                                                                                <%--<cc1:FilteredTextBoxExtender ID="FilteredtxtCHFFactorExtender" runat="server" TargetControlID="txtCHFclaimntsnum"
                                                                                    FilterType="Custom" FilterMode="ValidChars" ValidChars="0123456789," />--%>
                                                                            </div>
                                                                        </td>
                                                                        <td>
                                                                            <div style="width: 90px; height: 16px;">
                                                                                <asp:AISAmountTextbox ID="txtCHFClaimRate" Text="" Width="89px" ReadOnly="false" 
                                                                                    ValidationGroup="SaveCHFDtls" runat="server" >
                                                                                    </asp:AISAmountTextbox>
                                                                                    <%--<cc1:FilteredTextBoxExtender
                                                                                        ID="FilteredTextBoxExtender1" runat="server" TargetControlID="txtCHFClaimRate"
                                                                                        FilterType="Custom" FilterMode="ValidChars" ValidChars="0123456789," />--%>
                                                                                <asp:RequiredFieldValidator ID="RequiredCHFClaimText" runat="server" Text="*" Display="Dynamic"
                                                                                    ErrorMessage="Please enter CHF Claim rate" ValidationGroup="SaveCHFDtls" ControlToValidate="txtCHFClaimRate"></asp:RequiredFieldValidator>
                                                                            </div>
                                                                        </td>
                                                                        <td>
                                                                        </td>
                                                                    </tr>
                                                                </InsertItemTemplate>
                                                            </asp:AISListView>
                                                        </asp:Panel>
                                                    </div>
                                                </td>
                                            </tr>
                                        </table>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                        </ContentTemplate>
                    </cc1:TabPanel>
                    <cc1:TabPanel runat="server" ID="tblpnlRML" HeaderText="RML">
                        <HeaderTemplate>
                            RML
                        </HeaderTemplate>
                        <ContentTemplate>
                            <div class="gridRML">
                                <asp:UpdatePanel ID="updtpnlRML" runat="server" UpdateMode="Conditional" Visible="true">
                                    <ContentTemplate>
                                        <asp:Panel ID="pnlAdjRML" runat="server" Visible="false">
                                            <table id="tblAdjRML" visible="true" runat="server" class="panelContents">
                                                <tr align="center">
                                                    <th>
                                                    </th>
                                                    <th>
                                                        Policy #'s
                                                    </th>
                                                    <th>
                                                        Details
                                                    </th>
                                                    <th>
                                                        Disable
                                                    </th>
                                                </tr>
                                                <tr class="ItemTemplate" style="vertical-align: middle">
                                                    <td style="vertical-align: middle; width: 60px">
                                                        <asp:LinkButton ID="lnkbtnSAVERML" runat="server" Text="Save" Visible="false" Width="60px"
                                                            OnClick="btnRMLInfoDetailsSave_Click" />
                                                        <asp:LinkButton ID="lnkbtnUPDRML" runat="server" Text="Update" Visible="false" Width="60px"
                                                            OnClick="btnRMLInfoDetailsSave_Click" />
                                                        <asp:LinkButton ID="lnkBtnRMLSetupCancel" runat="server" Text="Cancel" Visible="true"
                                                            Width="60px" OnClick="LBASetupCancel_Click" />
                                                    </td>
                                                    <td>
                                                        <asp:Panel ID="Panel3" runat="server" ScrollBars="Auto" CssClass="content" Height="20px">
                                                            <asp:CheckBoxList ID="ChkBoxLstRMLpolicy" runat="server" ForeColor="Black" ReadOnly="true"
                                                                BackColor="White" BorderColor="Black">
                                                            </asp:CheckBoxList>
                                                        </asp:Panel>
                                                    </td>
                                                    <td style="vertical-align: middle">
                                                        <asp:LinkButton ID="lnkbtnRMLdtlsView" runat="server" CommandName="Edit" Text="Details"
                                                            Visible="true" Width="110px" OnClick="btnRMLInfoDetailsInfo_Click" />
                                                    </td>
                                                    <td style="vertical-align: middle; width: 110px">
                                                        <asp:ImageButton ID="imgbtnDisableRML" OnCommand='DisableRMLRow' CommandArgument='<%# Bind("adj_paramet_setup_id") %>'
                                                            runat="server" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </asp:Panel>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                                <asp:UpdatePanel ID="updtpnlRMLinfodtls" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:TextBox ID="hdnRMLPrmsetuptxtBox" Visible="False" runat="server"></asp:TextBox>
                                        <table id="tblRMLinfodtls" runat="server" class="panelContents" width="180px">
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblRMLdtls" runat="server" Text="RML Setup - Details" CssClass="h3"
                                                        Width="180px" Visible="false"></asp:Label>&nbsp;&nbsp;<asp:LinkButton ID="lnkBtnCloseRMLdtlsID"
                                                            Text="Close" runat="server" Visible="false" OnClick="btnRMLInfoClose_Click" />
                                                    <div id="divRMLinfodtls" class="content" style="height: 170px; width: 795px; overflow: auto">
                                                        <asp:Panel ID="pnlRMLdtls" runat="server" Height="20px" Width="50px">
                                                            <asp:AISListView ID="lstRMLSetuplistView" runat="server" Visible="True" OnItemEditing="lstRMLParmsetupDtls_ItemEdit"
                                                                OnItemDataBound="lstRMLParmsetupDtls_DataBoundList" InsertItemPosition="FirstItem"
                                                                OnItemCanceling="lstRMLParmsetupDtls_ItemCancel" OnSorting="lstRMLParmsetupDtls_Sorting"
                                                                OnItemUpdating="lstRMLParmsetupDtls_ItemUpdating" OnItemCommand="lstRMLParmsetupDtls_ItemCommand">
                                                                <LayoutTemplate>
                                                                    <table id="tblLayoutRMLdetails" class="ItemTemplate" runat="server">
                                                                        <tr style="height: 18px;">
                                                                            <th>
                                                                            </th>
                                                                            <th align="center">
                                                                                LOB
                                                                            </th>
                                                                            <th align="center">
                                                                                <asp:LinkButton ID="hlRMLStateSort" runat="server" CommandName="Sort" CommandArgument="PrgParameterStateName">
                                                                    State
                                                                                </asp:LinkButton>
                                                                                <asp:ImageButton ID="imgRMLStateSort" Visible="false" ToolTip="Ascending" ImageUrl="~/images/ascending.gif"
                                                                                    runat="server" />
                                                                            </th>
                                                                            <th align="center">
                                                                                Factor
                                                                            </th>
                                                                            <th align="center">
                                                                                Final RML Amount
                                                                            </th>
                                                                            <th align="center">
                                                                                Comments
                                                                            </th>
                                                                            <th align="center" style="width: 20px; height: 18px;">
                                                                                Disable
                                                                            </th>
                                                                        </tr>
                                                                        <tr id="ItemPlaceHolder" runat="server">
                                                                        </tr>
                                                                    </table>
                                                                </LayoutTemplate>
                                                                <ItemTemplate>
                                                                    <tr class="ItemTemplate">
                                                                        <td id="tdRMLDetailsEdit" align="center" runat="server">
                                                                            <asp:LinkButton ID="lnkbtnRMLdetailsetupEdit" CommandName="Edit" Text="Edit" runat="server"
                                                                                Visible="true" OnClick="btnRMLsetupinfodetails_click" />
                                                                        </td>
                                                                        <td>
                                                                            <div style="width: 60px; height: 20px;">
                                                                                <asp:Label ID="lblRMLLOBdtls" Text='<%# Bind("PrgParameterLOBName") %>' runat="server"></asp:Label>
                                                                            </div>
                                                                        </td>
                                                                        <td>
                                                                            <div style="width: 64px; height: 20px;">
                                                                                <asp:Label ID="lblRMLStateID" Text='<%# Bind("PrgParameterStateName") %>' runat="server"></asp:Label>
                                                                            </div>
                                                                        </td>
                                                                        <td>
                                                                            <div style="width: 69px; height: 20px;">
                                                                                <asp:Label ID="lblRMLfactor" Text='<%# Bind("adj_fctr_rt") %>' runat="server"></asp:Label>
                                                                            </div>
                                                                        </td>
                                                                        <td>
                                                                            <div style="width: 90px; height: 20px;">
                                                                                <asp:Label ID="lblAdjParmtdtlRMLID" runat="server" Text='<%# Bind("prem_adj_pgm_dtl_id") %>'
                                                                                    Visible="false"></asp:Label>
                                                                                <asp:Label ID="lblRMLFnlRMLamt" Text='<%# Eval("fnl_overrid_amt") == null? "":(decimal.Parse(Eval("fnl_overrid_amt").ToString())).ToString("#,##0") %>'
                                                                                    runat="server"></asp:Label>
                                                                            </div>
                                                                        </td>
                                                                        <td>
                                                                            <div style="width: 297px; height: auto; background-color: White">
                                                                                <asp:TextBox Width="296px" ID="lblRMLcomments" Text='<%# Bind("cmmnt_txt") %>' TextMode="MultiLine"
                                                                                    ReadOnly="true" runat="server"></asp:TextBox>
                                                                            </div>
                                                                        </td>
                                                                        <td>
                                                                            <div style="width: 40px; height: 20px;">
                                                                                <asp:HiddenField ID="hidActiveRML" runat="server" Value='<%# Bind("act_ind")%>' />
                                                                                <asp:ImageButton ID="imgDisableEnableRML" CommandArgument='<%# Bind("prem_adj_pgm_dtl_id") %>'
                                                                                    runat="server" ImageUrl='<%# Eval("act_ind").ToString()=="True"?"~/images/disabled.GIF":"~/images/enabled.GIF" %>'>
                                                                                </asp:ImageButton>
                                                                            </div>
                                                                        </td>
                                                                    </tr>
                                                                </ItemTemplate>
                                                                <AlternatingItemTemplate>
                                                                    <tr class="AlternatingItemTemplate">
                                                                        <td id="tdRMLDetailsEdit" align="center" runat="server">
                                                                            <asp:LinkButton ID="lnkbtnRMLdetailsetupEdit" CommandName="Edit" Text="Edit" runat="server"
                                                                                Visible="true" OnClick="btnRMLsetupinfodetails_click" />
                                                                        </td>
                                                                        <td>
                                                                            <div style="width: 60px; height: 20px;">
                                                                                <asp:Label ID="lblRMLLOBdtls" Text='<%# Bind("PrgParameterLOBName") %>' runat="server"></asp:Label>
                                                                            </div>
                                                                        </td>
                                                                        <td>
                                                                            <div style="width: 64px; height: 20px;">
                                                                                <asp:Label ID="lblRMLStateID" Text='<%# Bind("PrgParameterStateName") %>' runat="server"></asp:Label>
                                                                            </div>
                                                                        </td>
                                                                        <td>
                                                                            <div style="width: 69px; height: 20px;">
                                                                                <asp:Label ID="lblRMLfactor" Text='<%# Bind("adj_fctr_rt") %>' runat="server"></asp:Label>
                                                                            </div>
                                                                        </td>
                                                                        <td>
                                                                            <div style="width: 90px; height: 20px;">
                                                                                <asp:Label ID="lblAdjParmtdtlRMLID" runat="server" Text='<%# Bind("prem_adj_pgm_dtl_id") %>'
                                                                                    Visible="false"></asp:Label>
                                                                                <asp:Label ID="lblRMLFnlRMLamt" Text='<%# Eval("fnl_overrid_amt") == null? "":(decimal.Parse(Eval("fnl_overrid_amt").ToString())).ToString("#,##0") %>'
                                                                                    runat="server"></asp:Label>
                                                                            </div>
                                                                        </td>
                                                                        <td>
                                                                            <div style="width: 297px; height: auto; background-color: White">
                                                                                <asp:TextBox Width="296px" ID="lblRMLcomments" Text='<%# Bind("cmmnt_txt") %>' TextMode="MultiLine"
                                                                                    ReadOnly="true" runat="server"></asp:TextBox>
                                                                            </div>
                                                                        </td>
                                                                        <td>
                                                                            <div style="width: 20px; height: 20px;">
                                                                                <asp:HiddenField ID="hidActiveRML" runat="server" Value='<%# Bind("act_ind")%>' />
                                                                                <asp:ImageButton ID="imgDisableEnableRML" CommandArgument='<%# Bind("prem_adj_pgm_dtl_id") %>'
                                                                                    runat="server" ImageUrl='<%# Eval("act_ind").ToString()=="True"?"~/images/disabled.GIF":"~/images/enabled.GIF" %>'>
                                                                                </asp:ImageButton>
                                                                            </div>
                                                                        </td>
                                                                    </tr>
                                                                </AlternatingItemTemplate>
                                                                <EditItemTemplate>
                                                                    <tr style="width: 100%">
                                                                        <td>
                                                                            <asp:LinkButton ID="lnkRMLprmtSetupUpdate" CommandName="Update" Text="Update" ValidationGroup="SaveRMLUpdt"
                                                                                runat="server" Visible="true" /><br /><asp:LinkButton ID="lnkRMLCancel" CommandName="Cancel"
                                                                                    runat="server" Text="Cancel" Visible="true" />
                                                                        </td>
                                                                        <td>
                                                                            <div style="width: 70px; height: 20px;">
                                                                                <asp:Label ID="hidRMLLOB" runat="server" Text='<%# Bind("PrgParameterLOBName") %>'
                                                                                    Visible="false"></asp:Label>
                                                                                <asp:DropDownList ID="ddRMLLOB" DataSourceID="LCFsetupLOBDataSource" DataTextField="LookUpName"
                                                                                    DataValueField="LookUpID" Width="69px" runat="server">
                                                                                </asp:DropDownList>
                                                                                <asp:CompareValidator ID="CompddRMLLOB" runat="server" ControlToValidate="ddRMLLOB"
                                                                                    ValidationGroup="SaveRMLUpdt" ValueToCompare="0" Text="*" Display="Dynamic" ErrorMessage="Please select a Line Of Business from dropdown box"
                                                                                    Operator="NotEqual"></asp:CompareValidator>
                                                                            </div>
                                                                        </td>
                                                                        <td>
                                                                            <div style="width: 121px; height: 20px;">
                                                                                <asp:Label ID="hidRMLState" runat="server" Text='<%# Bind("PrgparameterStateFullname") %>'
                                                                                    Visible="false"></asp:Label>
                                                                                <asp:DropDownList ID="ddlRMLState" DataSourceID="LBAsetupStateDataSource" DataTextField="LookUpName"
                                                                                    DataValueField="LookUpID" Width="120px" runat="server">
                                                                                </asp:DropDownList>
                                                                                <asp:CompareValidator ID="CompddlRMLState" runat="server" ControlToValidate="ddlRMLState"
                                                                                    ValidationGroup="SaveRMLUpdt" ValueToCompare="0" Text="*" Display="Dynamic" ErrorMessage="Please select a State from dropdown box"
                                                                                    Operator="NotEqual"></asp:CompareValidator>
                                                                            </div>
                                                                        </td>
                                                                        <td>
                                                                            <div style="width: 69px; height: 20px;">
                                                                                <asp:TextBox ID="txtRMLFactor" Text='<%# Bind("adj_fctr_rt")%>' Width="68px" runat="server"> </asp:TextBox>
                                                                               <%--<cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender1"
                                                                                        runat="server" TargetControlID="txtRMLFactor" FilterType="Custom" FilterMode="ValidChars"
                                                                                        ValidChars="0123456789," />--%>
                                                                            <cc1:MaskedEditExtender ID="MaskedEdittxtRMLFactor" runat="server" TargetControlID="txtRMLFactor"
                                                                                    Mask="9.999999" MessageValidatorTip="true" MaskType="Number" DisplayMoney="None"
                                                                                    OnFocusCssClass="MaskedEditFocus" OnInvalidCssClass="MaskedEditError" InputDirection="LeftToRight"
                                                                                    AcceptNegative="None" />
                                                                                <asp:RequiredFieldValidator ID="txtRMLFactorEdit" runat="server" Text="*" Display="Dynamic"
                                                                                    ErrorMessage="Please enter RML Factor" ValidationGroup="SaveRMLUpdt" ControlToValidate="txtRMLFactor"></asp:RequiredFieldValidator>
                                                                            </div>
                                                                        </td>
                                                                        <td>
                                                                            <div style="width: 90px; height: 20px;">
                                                                                <asp:Label ID="lblAdjParmtdtlRMLID" runat="server" Text='<%# Bind("prem_adj_pgm_dtl_id") %>'
                                                                                    Visible="false"></asp:Label>
                                                                                <asp:AISAmountTextbox ID="txtRMLfinalAmount" Width="89px" Text='<%# Bind("fnl_overrid_amt","{0:0}")%>'
                                                                                    runat="server" > </asp:AISAmountTextbox>
                                                                               <%--<cc1:FilteredTextBoxExtender runat="server" TargetControlID="txtRMLfinalAmount"
                                                                                FilterType="Custom" ValidChars="0123456789," ID="fltrAudexp">
                                                                            </cc1:FilteredTextBoxExtender>--%>
                                                                            </div>
                                                                        </td>
                                                                        <td>
                                                                            <div style="width: 297px; height: auto; background-color: White">
                                                                                <asp:TextBox Width="296px" ID="RMLSetupComments" Text='<%# Bind("cmmnt_txt")%>' runat="server"
                                                                                    TextMode="MultiLine" MaxLength="500" onpaste="doPaste(this,500);" onkeypress="doKeypress(this,500);"
                                                                                    onbeforepaste="doBeforePaste(this,500);"> 
                                                                                </asp:TextBox>
                                                                            </div>
                                                                        </td>
                                                                        <td>
                                                                            <asp:CheckBox ID="chkActiveRML" runat="server" Checked="true" Text="Active" />
                                                                        </td>
                                                                    </tr>
                                                                </EditItemTemplate>
                                                                <InsertItemTemplate>
                                                                    <tr class="AlternatingItemTemplate">
                                                                        <td align="center">
                                                                            <asp:LinkButton ID="lblRMLsetupdetailSave" CommandName="Save" Text="Save" ValidationGroup="SaveRML"
                                                                                runat="server" Visible="true" Height="20px" Width="30px" />
                                                                        </td>
                                                                        <td>
                                                                            <div style="width: 70px; height: 20px;">
                                                                                <asp:DropDownList ID="ddRMLLOB" DataSourceID="LCFsetupLOBDataSource" DataTextField="LookUpName"
                                                                                    DataValueField="LookUpID" Width="69px" runat="server">
                                                                                </asp:DropDownList>
                                                                                <asp:CompareValidator ID="CompddRMLLOB" runat="server" ControlToValidate="ddRMLLOB"
                                                                                    ValidationGroup="SaveRML" ValueToCompare="0" Text="*" Display="Dynamic" ErrorMessage="Please select a Line Of Business from dropdown box"
                                                                                    Operator="NotEqual"></asp:CompareValidator>
                                                                            </div>
                                                                        </td>
                                                                        <td>
                                                                            <div style="width: 121px; height: 20px;">
                                                                                <asp:DropDownList ID="ddlRMLState" DataSourceID="LBAsetupStateDataSource" DataTextField="LookUpName"
                                                                                    DataValueField="LookUpID" Width="120px" runat="server">
                                                                                </asp:DropDownList>
                                                                                <asp:CompareValidator ID="CompddlRMLState" runat="server" ControlToValidate="ddlRMLState"
                                                                                    ValidationGroup="SaveRML" ValueToCompare="0" Text="*" Display="Dynamic" ErrorMessage="Please select a State from dropdown box"
                                                                                    Operator="NotEqual"></asp:CompareValidator>
                                                                            </div>
                                                                        </td>
                                                                        <td>
                                                                            <div style="width: 69px; height: 20px;">
                                                                                <asp:TextBox ID="txtRMLFactor" Text="" ReadOnly="false" Width="68px" runat="server"></asp:TextBox>
                                                                                <cc1:MaskedEditExtender ID="MaskedEdittxtRMLFactor" runat="server" TargetControlID="txtRMLFactor"
                                                                                    Mask="9.999999" MessageValidatorTip="true" MaskType="Number" DisplayMoney="None"
                                                                                    OnFocusCssClass="MaskedEditFocus" OnInvalidCssClass="MaskedEditError" InputDirection="LeftToRight"
                                                                                    AcceptNegative="None" />
                                                                                <asp:RequiredFieldValidator ID="txtTMFactoRate" runat="server" Text="*" Display="Dynamic"
                                                                                    ErrorMessage="Please enter RML Factor" ValidationGroup="SaveRML" ControlToValidate="txtRMLFactor"></asp:RequiredFieldValidator>
                                                                            </div>
                                                                        </td>
                                                                        <td>
                                                                            <div style="width: 90px; height: 20px;">
                                                                                <asp:Label ID="lblAdjParmtdtlRMLID" runat="server" Text='<%# Bind("prem_adj_pgm_dtl_id") %>'
                                                                                    Visible="false"></asp:Label>
                                                                                <asp:AISAmountTextbox ID="txtRMLfinalAmount" Width="89px" Text='<%# Bind("fnl_overrid_amt")%>'
                                                                                    runat="server" > </asp:AISAmountTextbox>
                                                                                <%--<cc1:FilteredTextBoxExtender runat="server" TargetControlID="txtRMLfinalAmount"
                                                                                FilterType="Custom" ValidChars="0123456789,"  id="fltrAudexp">
                                                                            </cc1:FilteredTextBoxExtender>--%>
                                                                            
                                                                            </div>
                                                                        </td>
                                                                        <td>
                                                                            <div style="width: 297px; height: 20px; background-color: White">
                                                                                <asp:TextBox Width="296px" ID="RMLSetupComments" Text="" runat="server" TextMode="MultiLine"
                                                                                    MaxLength="500" onpaste="doPaste(this,500);" onkeypress="doKeypress(this,500);"
                                                                                    onbeforepaste="doBeforePaste(this,500);"> 
                                                                                </asp:TextBox>
                                                                            </div>
                                                                        </td>
                                                                        <td>
                                                                        </td>
                                                                    </tr>
                                                                </InsertItemTemplate>
                                                            </asp:AISListView>
                                                        </asp:Panel>
                                                    </div>
                                                </td>
                                            </tr>
                                        </table>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                        </ContentTemplate>
                    </cc1:TabPanel>
                </cc1:TabContainer>
            </td>
        </tr>
    </table>
</asp:Content>
