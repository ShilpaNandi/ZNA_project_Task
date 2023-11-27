<%--/*-----	Page:	Program Period Page.
-----
-----	Created:		CSC (venkat Kolimi)

-----
-----	Description:	The page is used by the users to enter the program period information related to the particular accounts.	
-----  On Exit:	
-----			
-----
-----   Created Date : 2/19/2009 (AS part of Retro Project)

-----	Modified:	MM/DD/YY	first & last name of modifier
-----			- Description of Modification
-----          06/29/2010 Zakir Hussain:Two new checkboxes have been added for the surcharges project.
------         

--%>



<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Retro.master" CodeBehind="ProgramPeriod.aspx.cs"
    Inherits="ZurichNA.AIS.WebSite.AcctSetup.ProgramPeriod" EnableEventValidation="false" %>

<%@ Register Src="~/App_Shared/AccountInfoHeader.ascx" TagName="MasterValues" TagPrefix="MV" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeaderPlaceHolder" runat="Server">
    <table width="910" style="text-align: right">
        <tr>
            <td align="left">
                <asp:Label ID="Label1" runat="server" Text="Program Period Setup" CssClass="h1"></asp:Label>
            </td>
            <td align="right">
                <asp:Button ID="btnAdd" runat="server" Text="Add New" OnClick="btnAdd_Click" ToolTip="Please click here to add new Program Periods"
                    Width="60px" />
            </td>
        </tr>
    </table>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainPlaceHolder" runat="Server">

    <script language="javascript" type="text/javascript">
    var scrollTop1;
    
    if (Sys.WebForms.PageRequestManager != null)
    {
        Sys.WebForms.PageRequestManager.getInstance().add_beginRequest(BeginRequestHandler);
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);
    }

    function BeginRequestHandler(sender, args) 
    {
        var mef = $get('<%=pnlProgramPeriod.ClientID%>');
        if(mef!=null)
        scrollTop1 = mef.scrollTop;
    }

    function EndRequestHandler(sender, args)
    {
        var mef = $get('<%=pnlProgramPeriod.ClientID%>');
        if(mef!=null)
        mef.scrollTop = scrollTop1;
    } 
    
function OnEffDateChange()
{
    var EffDate = document.getElementById('<%=txtEffectiveDate.ClientID%>').value;
    var LSIEffDate=document.getElementById('<%=txtLSIStartDate.ClientID%>').value;
    if ((EffDate!="") && (EffDate!="__/__/____") )
    {
        if (LSIEffDate=="") {document.getElementById('<%=txtLSIStartDate.ClientID%>').value=EffDate;}        
    }
    OnFirstAdjChange();
}    
function OnFirstAdjChange()
{
    
    var EffDate = document.getElementById('<%=txtEffectiveDate.ClientID%>').value;
    var EffDateOld = document.getElementById('<%=txtEffDtHidden.ClientID%>').value;
    var FiAdj = document.getElementById('<%=txtFirstAdj.ClientID%>').value;   
    var FiAdjOld = document.getElementById('<%=txtFstAdjHidden.ClientID%>').value;
    //alert (EffDate + ' ' + EffDateOld + ' ' + FiAdj + ' ' + FiAdjOld);
    if ((EffDate==EffDateOld) && (FiAdj==FiAdjOld)) { return; }
    //alert('change');
    
    if ((EffDate!="") && (EffDate!="__/__/____") )
    {        
        var LSIRetFromDt = document.getElementById('<%=txtLSIRetriveFromDt.ClientID%>').value;
        var PreValDt = document.getElementById('<%=txtPrevValDt.ClientID%>').value
        if ((LSIRetFromDt == "") && (PreValDt =="")) {document.getElementById('<%=txtLSIRetriveFromDt.ClientID%>').value=EffDate;}
        var dt = new Date(EffDate);           
        if (FiAdj!="") var dt = new Date(AddMonthsToDt(FiAdj,dt));    
        var y=dt.getFullYear();        
	    var M=dt.getMonth()+1;
	    var d=dt.getDate();		    
	    if (d>15) {d=getLastDayOfMonth(M,y)}
	    if (d<16) 
	        { if (M==1) {M=12; y-=1;}
	        else {M -= 1;}
	        d=getLastDayOfMonth(M,y)
		    }  	
		    if (M.toString().length==1) {M="0"+M};
       document.getElementById('<%=txtValDt.ClientID%>').value = M + "/" + d ; 
       document.getElementById('<%=txtHidenValDt.ClientID%>').value = M + "/" + d + "/" + y ; 
       
       var PrevValDt = document.getElementById('<%=txtPrevValDt.ClientID%>').value;
       
       if (PrevValDt=="") 
       {       
        document.getElementById('<%=txtFirstAdjDt.ClientID%>').value = M + "/" + d + "/" + y ;
        document.getElementById('<%=txtNextValDt.ClientID%>').value= M + "/" + d + "/" + y ;         
       }        
       
       document.getElementById('<%=txtFstAdjHidden.ClientID%>').value=FiAdj;
       document.getElementById('<%=txtEffDtHidden.ClientID%>').value=EffDate;
     
        OnFirstAdjNPChange();
   } 
} 
function OnExpDateChange()
{
    var ExpDate = document.getElementById('<%=txtExpirationDate.ClientID%>').value;
    var LSIExpDate=document.getElementById('<%=txtLSIEndDate.ClientID%>').value;
    if ((ExpDate!="") && (ExpDate!="__/__/____") )
    {
        if (LSIExpDate=="") {document.getElementById('<%=txtLSIEndDate.ClientID%>').value=ExpDate;} 
    }
}
function OnFirstAdjNPChange()
{

    var EffDate = document.getElementById('<%=txtEffectiveDate.ClientID%>').value;
    var PrevValDtNP = document.getElementById('<%=txtPrevValDtNP.ClientID%>').value;
    var FiAdj = document.getElementById('<%=txtFirstAdj.ClientID%>').value;   
    var FiAdjNP = document.getElementById('<%=txtFirstAdjNONPREM.ClientID%>').value;   
    if ((FiAdjNP=="") && (FiAdj!=""))
    {
        document.getElementById('<%=txtFirstAdjNONPREM.ClientID%>').value=FiAdj;
        var FiAdjNP = document.getElementById('<%=txtFirstAdjNONPREM.ClientID%>').value;   
    }   
    if ((EffDate!="") &&  (PrevValDtNP==""))
    {                 
        var dt = new Date(EffDate);           
        if (FiAdjNP!="") var dt = new Date(AddMonthsToDt(FiAdjNP,dt));    
        var y=dt.getFullYear();        
	    var M=dt.getMonth()+1;
	    var d=dt.getDate();		    
	    if (d>15) {d=getLastDayOfMonth(M,y)}
	    if (d<16) 
	        { if (M==1) {M=12; y-=1;}
	        else {M -= 1;}
	        d=getLastDayOfMonth(M,y)
		    }  	
		    if (M.toString().length==1) {M="0"+M};      
       
        document.getElementById('<%=txtFirstAdjDtNP.ClientID%>').value = M + "/" + d + "/" + y ; 
        document.getElementById('<%=txtNextValDtNP.ClientID%>').value= M + "/" + d + "/" + y ;        

   } 
  
} 
function onFreqChange()
{
    var Freq = document.getElementById('<%=txtFreq.ClientID%>').value;  
    var FreqNP = document.getElementById('<%=txtFreqNonPrem.ClientID%>').value;      
    var PrevValDt = document.getElementById('<%=txtPrevValDt.ClientID%>').value;
    
    if ((PrevValDt!="") && (Freq!=""))
    {        
        var dt = new Date(PrevValDt);
        var dt = new Date(AddMonthsToDt(Freq,dt));
        var y=dt.getFullYear();        
	    var M=dt.getMonth()+1;
	    var d=dt.getDate();	
	    if (d>15) {d=getLastDayOfMonth(M,y)}
	    if (d<16) 
	        { if (M==1) {M=12; y-=1;}
	        else {M -= 1;}
	        d=getLastDayOfMonth(M,y)
		    }  	
		    if (M.toString().length==1) {M="0"+M};   
        document.getElementById('<%=txtNextValDt.ClientID%>').value= M + "/" + d + "/" + y ; 
    }      
    if ((FreqNP=="") && (Freq!=""))
    {
        document.getElementById('<%=txtFreqNonPrem.ClientID%>').value=Freq; 
        onFreqNPChange();
    }
}
function onFreqNPChange()
{
    var FreqNP = document.getElementById('<%=txtFreqNonPrem.ClientID%>').value;    
    var PrevValDtNP = document.getElementById('<%=txtPrevValDtNP.ClientID%>').value;
    
    if ((PrevValDtNP!="") && (FreqNP!=""))
    {        
        var dtNP = new Date(PrevValDtNP);
        var dtNP = new Date(AddMonthsToDt(FreqNP,dtNP));
        var y=dtNP.getFullYear();        
	    var M=dtNP.getMonth()+1;
	    var d=dtNP.getDate();	
	    if (d>15) {d=getLastDayOfMonth(M,y)}
	    if (d<16) 
	        { if (M==1) {M=12; y-=1;}
	        else {M -= 1;}
	        d=getLastDayOfMonth(M,y)
		    }  	
		    if (M.toString().length==1) {M="0"+M};   
        document.getElementById('<%=txtNextValDtNP.ClientID%>').value= M + "/" + d + "/" + y ;
    }  
}
function AddMonthsToDt(NoOfMonths,Dt)
 {
    var myDate=new Date(Dt);
    var y=myDate.getFullYear();
	var M=myDate.getMonth()+1;
	var d=myDate.getDate();	
	M = parseInt(M) + parseInt(NoOfMonths);
	var MonthsDev = M/12;
	for(var i=0;i<MonthsDev;i++) 
    {
      if (M>12) { M -= 12; y= y + 1;}      	
    }		
    return (M + "/" + d + "/" + y);
 } 
function getLastDayOfMonth(month,year)
 {
    var m = [31,28,31,30,31,30,31,31,30,31,30,31];
    if (month != 2) return m[month - 1];
    if (year%4 != 0) return m[1];
    if (year%100 == 0 && year%400 != 0) return m[1];
    return m[1] + 1;
 } 
 /* This function is used to disable the Update/Save button and click
    on the hidden button.
 */
 function disableControl()
 {
   document.getElementById('<%=btnSaveHidden.ClientID%>').click();
   if(Page_IsValid==true)//disable the button only if validation is success
   {
      document.getElementById('<%=btnSave.ClientID%>').disabled=true;
   } 
       return true;
 }
// function FillFirstAdjDt(effDt)
// {
//    var dt = new Date(effDt); 
//    var y=dt.getFullYear();
//	var M=dt.getMonth()+1;
//	var d=dt.getDate();
//	
//	if (d>15) {d=getLastDayOfMonth(M,y)}
//	if (d<16) 
//	    { if (M==1) {M=12; y-=1;}
//	    else {M -= 1;}
//	    d=getLastDayOfMonth(M,y)
//		}  	
//		if (M.toString().length==1) {M="0"+M};
//   document.getElementById('<%=txtFirstAdjDt.ClientID%>').value = M + "/" + d + "/" + y ; 
//   document.getElementById('<%=txtNextValDt.ClientID%>').value= M + "/" + d + "/" + y ; 
// }
//function FillFirstAdjDtNP(effDt)
// {
//    var dt = new Date(effDt); 
//    var y=dt.getFullYear();
//	var M=dt.getMonth()+1;
//	var d=dt.getDate();
//	
//	if (d>15) {d=getLastDayOfMonth(M,y)}
//	if (d<16) 
//	    { if (M==1) {M=12; y-=1;}
//	    else {M -= 1;}
//	    d=getLastDayOfMonth(M,y)
//		}  	
//		if (M.toString().length==1) {M="0"+M};
//   // has to update first AdjNonPremDt -- document.getElementById('<%=txtFinalAdjNONPREM.ClientID%>').value = M + "/" + d + "/" + y ; 
//   document.getElementById('<%=txtNextValDtNP.ClientID%>').value= M + "/" + d + "/" + y ; 
// }
function CheckBoxListSelect(cbControl, state)
{    
       var chkBoxList = document.getElementById(cbControl);
        var chkBoxCount= chkBoxList.getElementsByTagName("input");
        for(var i=0;i<chkBoxCount.length;i++) 
        {
            chkBoxCount[i].checked = state;
        }
        
        return false; 
}
function SelectAllProgPer(spanChk)
	    {
            // Added as ASPX uses SPAN for checkbox 
            var theBox=spanChk;
            xState=theBox.checked;	
            elm=document.getElementsByName("chkSelProgPer");
            for(i=0;i<elm.length;i++)
            {
                if (elm[i].checked != xState) {
                    if (!elm[i].disabled)
                        elm[i].checked = xState;
                }
            }
	    }
	
	    
function EnableDisableUpdatePanel()
{
    var flag=document.getElementById('<%=chkUpdate.ClientID%>').checked;
    if (flag==true)    
    {
    document.getElementById('<%=ddlBU.ClientID%>').disabled=false; 
    document.getElementById('<%=ddlBroker.ClientID%>').disabled=false; 
    document.getElementById('<%=btnUpdate.ClientID%>').disabled=false; 
    }
    else  
    {  
    document.getElementById('<%=ddlBU.ClientID%>').disabled=true;
    document.getElementById('<%=ddlBroker.ClientID%>').disabled=true; 
    document.getElementById('<%=btnUpdate.ClientID%>').disabled=true;         
    }
}

function EnableBKTCYBUYOUTEffDt()
{
    var ddlval=document.getElementById('<%=ddlBKTCYBUYOUT.ClientID%>').value;    
    if (ddlval==0)    
    {    
        document.getElementById('<%=txtBKTCYBUYOUTEffDate.ClientID%>').disabled=true;   
        document.getElementById('<%=imgBKTCYBUYOUTEffDate.ClientID%>').disabled=true;   
    }
    else  
    {  
        document.getElementById('<%=txtBKTCYBUYOUTEffDate.ClientID%>').disabled=false;    
        document.getElementById('<%=imgBKTCYBUYOUTEffDate.ClientID%>').disabled=false;    
    }
}

function EnableTMTextbox()
{
    var flag=document.getElementById('<%=chkAvgTM.ClientID%>').checked;

    if (flag==true)
        document.getElementById('<%=txtTMFactor.ClientID%>').disabled=false;
    else
        document.getElementById('<%=txtTMFactor.ClientID%>').disabled=true;
}


    var oldgridSelectedColor;
    var oldgridClickedColor;
    var oldElement;

    function setMouseOverColor(element)
    {
        oldgridSelectedColor = element.style.backgroundColor;
        element.style.backgroundColor='lightblue';
        element.style.cursor='hand';
        element.style.textDecoration='underline';
    }

    function setMouseOutColor(element)
    {
        element.style.backgroundColor=oldgridSelectedColor;
        element.style.textDecoration='none';
    }
    function SetMouseClickColor(element)
    {
        if(oldElement != null)
        {
        oldElement.style.backgroundColor=oldgridSelectedColor;
        }
        oldElement=element;
        oldgridSelectedColor= element.style.backgroundColor;
        element.style.backgroundColor='yellow';
        element.style.cursor='hand';
    }
   function ValidateUpdate(oSource, oArgs)
    {      
    var selBroker=document.getElementById('<%=ddlBroker.ClientID%>').value;    
    var selBU=document.getElementById('<%=ddlBU.ClientID%>').value;      
    var result=false;
        if (selBroker > 0)  result=true;
        else if (selBU > 0) result = true;       
        oArgs.IsValid = result;
    }
    function CheckProgPerSel(oSource, oArgs)
    {       
        var result = false;
        elm = document.getElementsByName("chkSelProgPer");
        for (i = 0; i < elm.length; i++) 
        {
            if (elm[i].checked == true) 
            {
                result = true;
                break;
            }
        }
        oArgs.IsValid = result;
    }

//    function EnableActiveStatusCheckBox()
//{
//    
//    var txtSubmitStsID=document.getElementById('<%=txtSubmitStsID.ClientID%>');
//    var txtSetupStsID=document.getElementById('<%=txtSetupStsID.ClientID%>');
//    
//    var chkSubmitForSetupQC=document.getElementById('<%=chkSubmitForSetupQC.ClientID%>');
//    var chkSetupQCComplete=document.getElementById('<%=chkSetupQCComplete.ClientID%>');
//    
//    var chkActive=document.getElementById('<%=chkActive.ClientID%>');
//    
//    if (((txtSubmitStsID.value!="") && (txtSetupStsID.value!="")) || ((chkSubmitForSetupQC.checked) && (chkSetupQCComplete.checked)) || ((txtSubmitStsID.value!="") && (chkSetupQCComplete.checked)) || ((chkSubmitForSetupQC.checked) && (txtSetupStsID.value!="")))
//    {        
//        chkActive.disabled=false;        
//    }
//    else
//    {        
//        chkActive.disabled=true; 
//        chkActive.checked=false;
//                  
//    }
//}
    
    </script>

    <asp:UpdatePanel runat="server" ID="upProgramPeriod">
        <ContentTemplate>        
            
                <asp:ValidationSummary ID="VSSaveProgramPeriod" runat="server" ValidationGroup="Save" />
                <asp:ValidationSummary ID="VSUpdateProgramPeriod" runat="server" ValidationGroup="UpdateGroup" />
                <MV:MasterValues ID="mvHeader" runat="server" />
                <cc1:CollapsiblePanelExtender ID="CollapsiblePanelExtender1" runat="Server" TargetControlID="pnlUpdateSelectedPeriod"
                    CollapsedSize="0" Collapsed="true" ExpandControlID="pnlCollapse" CollapseControlID="pnlCollapse"
                    AutoCollapse="False" AutoExpand="False" ScrollContents="False" TextLabelID="lblHideShow"
                    ImageControlID="imgHideShow" ExpandedImage="~/images/collapse.jpg" CollapsedImage="~/images/expand.jpg"
                    ExpandDirection="Vertical" SuppressPostBack="true" />
                <asp:Panel ID="pnlCollapse" runat="server" Width="910px">
                    <div style="text-align: right" class="policyInformationHeader">
                        <asp:Label ID="lblHeaderTitle" runat="server" Text="Update Selected Periods"></asp:Label>
                        <asp:Label ID="lblHideShow" runat="server"></asp:Label>
                        <asp:Image ID="imgHideShow" runat="server" ImageUrl="~/images/collapse.jpg" />
                    </div>
                </asp:Panel>
                <asp:Panel runat="server" ID="pnlUpdateSelectedPeriod" BorderColor="Black" BorderWidth="1px"
                    Width="910px">
                    <asp:UpdatePanel ID="UpdateUpdatePanel" runat="server">
                        <ContentTemplate>
                            <table width="100%">
                                <tr>
                                    <td>
                                        <asp:ObjectDataSource ID="BrokerDataSource" runat="server" SelectMethod="GetOnlyBrokersForLookupsNA"
                                            TypeName="ZurichNA.AIS.Business.Logic.BrokerBS"></asp:ObjectDataSource>
                                        <asp:ObjectDataSource ID="BUOfficeDataSource" runat="server" SelectMethod="GetBUOffForLookups"
                                            TypeName="ZurichNA.AIS.Business.Logic.BusinessUnitOfficeBS"></asp:ObjectDataSource>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">
                                        <table>
                                            <tr>
                                                <td style="vertical-align: middle">
                                                    <asp:CheckBox ID="chkUpdate" runat="server" CssClass="h4" Font-Bold="true" onclick="EnableDisableUpdatePanel()"
                                                        Text=" Update" />
                                                </td>
                                                <td align="center">
                                                    <asp:Panel ID="pnlUpdate" runat="server">
                                                        <table>
                                                            <tr>
                                                                <td align="right" style="vertical-align: middle">
                                                                    Broker
                                                                </td>
                                                                <td align="left" style="vertical-align: middle">
                                                                    <asp:DropDownList ID="ddlBroker" runat="server" DataSourceID="BrokerDataSource" DataTextField="LookUpName"
                                                                        DataValueField="LookUpID" Width="200px" ValidationGroup="UpdateGroup" Enabled="false">
                                                                    </asp:DropDownList>
                                                                    <%--<asp:CompareValidator ID="CompareBrokerUpdate" runat="server" ControlToValidate="ddlBroker"
                                                                Display="Dynamic" ErrorMessage="Please Select Broker" Text="*" Operator="NotEqual"
                                                                ValueToCompare="0" ValidationGroup="UpdateGroup" />--%>
                                                                <asp:CustomValidator ID="CustValidatorUpdate" runat="server" ErrorMessage="Please select at least one among Broker or BU/Office"
                                                                        ClientValidationFunction="ValidateUpdate" ValidationGroup="UpdateGroup" Display="Dynamic"
                                                                        Text="*" />                                                                
                                                                </td>
                                                                <td align="right" style="vertical-align: middle">
                                                                    BU/Office
                                                                </td>
                                                                <td align="left" style="vertical-align: middle">
                                                                    <asp:DropDownList ID="ddlBU" runat="server" DataSourceID="BUOfficeDataSource" DataTextField="LookupName"
                                                                        DataValueField="LookUpID" Width="200px" ValidationGroup="UpdateGroup" Enabled="false">
                                                                    </asp:DropDownList>
                                                                    
                                                                        <asp:CustomValidator ID="CustValidatorPropr" runat="server" ErrorMessage="Please select at least one Program Period"
                                                                        ClientValidationFunction="CheckProgPerSel" ValidationGroup="UpdateGroup" Display="Dynamic"
                                                                        Text="*" />
                                                                    <%--<asp:CompareValidator ID="CompareBU" runat="server" ControlToValidate="ddlBU" Display="Dynamic"
                                                                ErrorMessage="Please Select BU/Office" Text="*" Operator="NotEqual" ValueToCompare="0"
                                                                ValidationGroup="UpdateGroup" />--%>
                                                                </td>
                                                                <td align="left" style="vertical-align: middle">
                                                                    <asp:Button ID="btnUpdate" runat="server" Text="Update" OnClick="btnUpdate_Click" 
                                                                        ValidationGroup="UpdateGroup" />
                                                                    <%--OnClientClick="javascript:return CheckProgPerSel()" />--%>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </asp:Panel>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </asp:Panel>
                <asp:Panel ID="pnlProgramPeriod" runat="server" Width="910px" Height="400px" ScrollBars="Auto">
                    <asp:AISListView ID="lstProgramPeriod" runat="server" OnItemCommand="lstProgramPeriod_ItemCommand"
                        OnItemDataBound="lstProgramPeriod_ItemDataBound" OnSelectedIndexChanging="lstProgramPeriod_SelectedIndexChanging"
                        OnSelectedIndexChanged="lstProgramPeriod_SelectedIndexChanged" OnSorting="lstProgramPeriod_Sorting">
                        <EmptyDataTemplate>
                            <table id="tblProgramPeriod" class="panelContents" runat="server" width="98%">
                                <tr>
                                    <th align="center">
                                        Details
                                    </th>
                                    <th align="center">
                                        <asp:CheckBox ID="chkSelectAll" runat="server" AutoPostBack="false" ToolTip="Select/Deselect All" />
                                    </th>
                                    <th align="center">
                                        Eff Date
                                    </th>
                                    <th align="center">
                                        Exp Date
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
                                    <th align="center">
                                        QC
                                    </th>
                                    <th>
                                        Disable
                                    </th>
                                </tr>
                            </table>
                            <table width="98%">
                                <tr id="Tr3" runat="server" class="ItemTemplate">
                                    <td align="center">
                                        <asp:Label ID="lblEmptyMessage" Text="--- No Records Found ---" Font-Bold="true"
                                            Width="600px" runat="server" Style="text-align: center" />
                                    </td>
                                </tr>
                            </table>
                        </EmptyDataTemplate>
                        <LayoutTemplate>
                            <table id="tblProgramPeriod" class="panelContents" runat="server" width="98%">
                                <tr>
                                    <th align="center">
                                        Details
                                    </th>
                                    <th align="center">
                                        <asp:CheckBox ID="chkSelectAll" runat="server" AutoPostBack="false" onclick="javascript:SelectAllProgPer(this);"
                                            ToolTip="Select/Deselect All" />
                                    </th>
                                    <th align="center">
                                        Eff Date
                                    </th>
                                    <th align="center">
                                        Exp Date
                                    </th>
                                    <th align="center">
                                        <asp:LinkButton ID="lnkProgramType" runat="server" CommandName="Sort" CommandArgument="PROGRAMTYPENAME">Program Type</asp:LinkButton>
                                        <asp:Image ID="imgPROGRAMTYPENAME" runat="server" ImageUrl="~/images/Ascending.gif"
                                            ToolTip="Ascending" Visible="false" />
                                    </th>
                                    <th align="center">
                                        <asp:LinkButton ID="lnkBroker" runat="server" CommandName="Sort" CommandArgument="BROKERNAME">Broker</asp:LinkButton>
                                        <asp:Image ID="imgBROKERNAME" runat="server" ImageUrl="~/images/Ascending.gif" ToolTip="Ascending"
                                            Visible="false" />
                                    </th>
                                    <th align="center">
                                        <asp:LinkButton ID="lnkBUOffice" runat="server" CommandName="Sort" CommandArgument="BUSINESSUNITNAME">BU/Office</asp:LinkButton>
                                        <asp:Image ID="imgBUSINESSUNITNAME" runat="server" ImageUrl="~/images/Ascending.gif"
                                            ToolTip="Ascending" Visible="false" />
                                    </th>
                                    <th align="center">
                                        <asp:LinkButton ID="lnkValuationDate" runat="server" CommandName="Sort" CommandArgument="VALN_MM_DT">Valuation Date</asp:LinkButton>
                                        <asp:Image ID="imgVALN_MM_DT" runat="server" ImageUrl="~/images/Ascending.gif" ToolTip="Ascending"
                                            Visible="false" />
                                    </th>
                                    <th align="center">
                                        QC
                                    </th>
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
                                <td id="tdDetails" align="left" style="vertical-align: middle" runat="server">
                                    <asp:LinkButton ID="lbSelect" CommandArgument='<%# Bind("PREM_ADJ_PGM_ID") %>' CommandName="Select"
                                        runat="server" Text="Details" Width="60px" ></asp:LinkButton>
                                </td>
                                <td align="center">
                                    <%#GetProgPerCheckBoxes(Eval("PREM_ADJ_PGM_ID").ToString(), (Eval("ACTV_IND")).ToString())%>
                                </td>
                                <td align="left" style="vertical-align: middle" runat="server" id="tdstrtDate">
                                    <%# Eval("STRT_DT", "{0:d}")%>
                                </td>
                                <td align="left" runat="server" style="vertical-align: middle" id="tdplnendDate">
                                    <%# Eval("PLAN_END_DT", "{0:d}")%>
                                </td>
                                <td align="left" runat="server" style="vertical-align: middle" id="tdprgTypid">
                                    <%# Eval("PROGRAMTYPENAME") %>
                                </td>
                                <td align="left" runat="server" style="vertical-align: middle" id="td1">
                                    <%# Eval("BROKERNAME") %>
                                </td>
                                <td align="left" runat="server" style="vertical-align: middle" id="tdbsn_unt">
                                    <%# Eval("BUSINESSUNITNAME") %>
                                </td>
                                <td runat="server" id="tdvalnDate" style="vertical-align: middle;">
                                    <%# Eval("VALN_MM_DT", "{0:MM / dd}")%>
                                </td>
                                <td id="td2" align="center" style="vertical-align: middle" runat="server">
                                    <asp:LinkButton ID="lbQC" CommandArgument='<%# Bind("PREM_ADJ_PGM_ID") %>' OnCommand='GotoQC'
                                        runat="server" Text="QC" Width="80px" Enabled='<%# Bind("ACTV_IND") %>'></asp:LinkButton>
                                </td>
                                <td>
                                    <asp:Label ID="lblProPreActive" runat="server" Text='<%# Bind("ACTV_IND") %>' Visible="false"></asp:Label>
                                    <asp:ImageButton ID="imgProPreEnableDisable" runat="server" CommandArgument='<%# Bind("PREM_ADJ_PGM_ID") %>' />
                                </td>
                            </tr>
                        </ItemTemplate>
                        <AlternatingItemTemplate>
                            <tr id="trItemTemplate" runat="server" class="AlternatingItemTemplate">
                                <td id="tdDetails" align="left" style="vertical-align: middle" runat="server">
                                    <asp:LinkButton ID="lbSelect" CommandArgument='<%# Bind("PREM_ADJ_PGM_ID") %>' CommandName="Select"
                                        runat="server" Text="Details"></asp:LinkButton>
                                </td>
                                <td align="center">
                                    <%#GetProgPerCheckBoxes(Eval("PREM_ADJ_PGM_ID").ToString(), Eval("ACTV_IND").ToString())%>
                                </td>
                                <td align="left" style="vertical-align: middle" runat="server" id="tdstrtDate">
                                    <%# Eval("STRT_DT", "{0:d}")%>
                                </td>
                                <td align="left" runat="server" style="vertical-align: middle" id="tdplnendDate">
                                    <%# Eval("PLAN_END_DT", "{0:d}") %>
                                </td>
                                <td align="left" runat="server" style="vertical-align: middle" id="tdprgTypid">
                                    <%# Eval("PROGRAMTYPENAME") %>
                                </td>
                                <td align="left" runat="server" style="vertical-align: middle" id="td1">
                                    <%# Eval("BROKERNAME") %>
                                </td>
                                <td align="left" runat="server" style="vertical-align: middle" id="tdbsn_unt">
                                    <%# Eval("BUSINESSUNITNAME") %>
                                </td>
                                <td runat="server" id="tdvalnDate" style="vertical-align: middle;">
                                    <%# Eval("VALN_MM_DT", "{0:MM / dd}")%>
                                </td>
                                <td id="td2" align="center" style="vertical-align: middle" runat="server">
                                    <asp:LinkButton ID="lbQC" CommandArgument='<%# Bind("PREM_ADJ_PGM_ID") %>' OnCommand='GotoQC'
                                        runat="server" Text="QC" Width="80px" Enabled='<%# Bind("ACTV_IND") %>'></asp:LinkButton>
                                </td>
                                <td>
                                    <asp:Label ID="lblProPreActive" runat="server" Text='<%# Bind("ACTV_IND") %>' Visible="false"></asp:Label>
                                    <asp:ImageButton ID="imgProPreEnableDisable" runat="server" CommandArgument='<%# Bind("PREM_ADJ_PGM_ID") %>' />
                                </td>
                            </tr>
                            <tr>
                            </tr>
                        </AlternatingItemTemplate>
                    </asp:AISListView>
                </asp:Panel>
                <asp:Label ID="lblProgramPeriodDetails" Visible="false" runat="server" Text="Program Period Details"
                    CssClass="h2"></asp:Label>
                &nbsp;
                <asp:LinkButton ID="lbCloseDetails" Text="Close" runat="server" OnClick="lbCloseDetails_Click"
                    Visible="false" Width="60px" />
                <asp:Panel ID="pnlDetails" runat="server" BorderColor="Black" BorderWidth="1px" Visible="false"
                    Width="910px" Style="padding-top: 5">
                    <table width="100%">
                        <tr>
                            <td>
                                Broker
                            </td>
                            <td colspan="3">
                                <asp:DropDownList ID="ddlBrokerDetail" runat="server" DataSourceID="BrokerDataSource"
                                    ValidationGroup="Save" DataTextField="LookUpNAME" DataValueField="LookUpID" Width="200px"
                                    AutoPostBack="true" OnSelectedIndexChanged="ddlBrokerDetail_SelectedIndexChanged">
                                </asp:DropDownList>
                                <asp:CompareValidator ID="CompareBroker" runat="server" ControlToValidate="ddlBrokerDetail"
                                    Display="Dynamic" ErrorMessage="Please Select Broker" Text="*" Operator="NotEqual"
                                    ValueToCompare="0" ValidationGroup="Save" />
                            </td>
                            <td>
                                Broker Contact
                            </td>
                            <td colspan="3">
                                <asp:DropDownList ID="ddlBrokerContact" runat="server" Width="200px" ValidationGroup="Save">
                                    <asp:ListItem Value="0">(Select)</asp:ListItem>
                                </asp:DropDownList>
                                <asp:CompareValidator ID="CompareBrokerContact" runat="server" ControlToValidate="ddlBrokerContact"
                                    Display="Dynamic" ErrorMessage="Please Select BrokerContact" Text="*" Operator="NotEqual"
                                    ValueToCompare="0" ValidationGroup="Save" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Program Type
                            </td>
                            <td colspan="3">
                                <asp:ObjectDataSource ID="ProgramTypeDataSource" runat="server" SelectMethod="GetLookUpActiveData"
                                    TypeName="ZurichNA.AIS.Business.Logic.BLAccess">
                                    <SelectParameters>
                                        <asp:Parameter DefaultValue="PROGRAM TYPE" Name="lookUpTypeName" Type="String" />
                                    </SelectParameters>
                                </asp:ObjectDataSource>
                                <asp:DropDownList ID="ddlProgramType" runat="server" DataSourceID="ProgramTypeDataSource"
                                    DataTextField="LookUpName" DataValueField="LookUpID" Width="200px" ValidationGroup="Save">
                                </asp:DropDownList>
                                <asp:CompareValidator ID="CompareProgramType" runat="server" ControlToValidate="ddlProgramType"
                                    Display="Dynamic" ErrorMessage="Please Select Program Type" Text="*" Operator="NotEqual"
                                    ValueToCompare="0" ValidationGroup="Save" />
                            </td>
                            <td>
                                BU/Office
                            </td>
                            <td colspan="3">
                                <asp:DropDownList ID="ddlBUDetails" runat="server" DataSourceID="BUOfficeDataSource"
                                    ValidationGroup="Save" DataTextField="LookUpName" DataValueField="LookUpID" Width="200px">
                                </asp:DropDownList>
                                <asp:CompareValidator ID="CompareBUDetails" runat="server" ControlToValidate="ddlBUDetails"
                                    Display="Dynamic" ErrorMessage="Please Select BU/Office" Text="*" Operator="NotEqual"
                                    ValueToCompare="0" ValidationGroup="Save" />
                            </td>
                        </tr>
                        <tr>
                            <td style="vertical-align: middle">
                                Effective Date
                            </td>
                            <td>
                                <asp:TextBox ID="txtEffectiveDate" runat="server" Width="65px" ValidationGroup="Save"
                                    onblur="OnEffDateChange()"></asp:TextBox>
                                <asp:TextBox ID="txtEffDtHidden" runat="server" Width="65px" CssClass="Hide"></asp:TextBox>    
                                <asp:ImageButton ID="imgEffDate" runat="server" ImageUrl="~/images/Calendar_scheduleHS.png"
                                    CausesValidation="False" />
                                <cc1:MaskedEditExtender ID="mskEffDate" runat="server" AcceptNegative="Left" DisplayMoney="Left"
                                    ErrorTooltipEnabled="True" Mask="99/99/9999" MaskType="Date" MessageValidatorTip="true"
                                    OnFocusCssClass="MaskedEditFocus" OnInvalidCssClass="MaskedEditError" TargetControlID="txtEffectiveDate" />
                                <cc1:CalendarExtender ID="CalendarExtender1" runat="server" PopupButtonID="imgEffDate" 
                                    TargetControlID="txtEffectiveDate" />
                                <asp:RequiredFieldValidator ID="RequiredEffectiveDate" runat="server" ControlToValidate="txtEffectiveDate"
                                    ErrorMessage="Please Enter Effective Date" Text="*" Display="Dynamic" ValidationGroup="Save" />
                                <asp:CompareValidator ID="CompareEffExpDate" runat="server" ControlToValidate="txtEffectiveDate"
                                    Display="Dynamic" ErrorMessage="Effective Date must be less than Expiration Date, Please Enter Valid Effective Date"
                                    Text="*" Operator="LessThan" Type="Date" ControlToCompare="txtExpirationDate"
                                    ValidationGroup="Save" />
                                <asp:RegularExpressionValidator ID="regValidFrom" runat="server" ControlToValidate="txtEffectiveDate" 
                                    ValidationExpression="(0?[1-9]|1[012])[- /.](0?[1-9]|[12][0-9]|3[01])[- /.](1[8-9]|2[01])\d\d"
                                    ErrorMessage="Please Enter Valid Effective Date" Text="*" ValidationGroup="Save"></asp:RegularExpressionValidator>
                            </td>
                            <td style="vertical-align: middle">
                                Expiration Date
                            </td>
                            <td>
                                <asp:TextBox ID="txtExpirationDate" runat="server" Width="65px" ValidationGroup="Save" onblur="OnExpDateChange()"></asp:TextBox>
                                <asp:ImageButton ID="imgExpDate" runat="server" ImageUrl="~/images/Calendar_scheduleHS.png"
                                    CausesValidation="False" />
                                <cc1:MaskedEditExtender ID="MaskedEditExtender6" runat="server" AcceptNegative="Left"
                                    DisplayMoney="Left" ErrorTooltipEnabled="True" Mask="99/99/9999" MaskType="Date"
                                    MessageValidatorTip="true" OnFocusCssClass="MaskedEditFocus" OnInvalidCssClass="MaskedEditError"
                                    TargetControlID="txtExpirationDate" />
                                <cc1:CalendarExtender ID="CalendarExtender12" runat="server" PopupButtonID="imgExpDate"
                                    TargetControlID="txtExpirationDate" />
                                <asp:RequiredFieldValidator ID="RequiredExpirationDate" runat="server" ControlToValidate="txtExpirationDate"
                                    ErrorMessage="Please Enter Expiration Date" Text="*" Display="Dynamic" ValidationGroup="Save" />
                                <asp:RegularExpressionValidator ID="regExpirationDate" runat="server" ControlToValidate="txtExpirationDate"
                                    ValidationExpression="(0?[1-9]|1[012])[- /.](0?[1-9]|[12][0-9]|3[01])[- /.](1[8-9]|2[01])\d\d"
                                    ErrorMessage="Please Enter Valid Expiration Date" Text="*" ValidationGroup="Save"></asp:RegularExpressionValidator>
                            </td>
                            <td style="vertical-align: middle">
                                LSI Start Date
                            </td>
                            <td style="vertical-align: middle">
                                <asp:TextBox ID="txtLSIStartDate" runat="server" Width="65px" ValidationGroup="Save"></asp:TextBox>
                                <asp:ImageButton ID="imgLSIStDate" runat="server" ImageUrl="~/images/Calendar_scheduleHS.png"
                                    CausesValidation="False" />
                                <cc1:MaskedEditExtender ID="MaskedEditExtender5" runat="server" AcceptNegative="Left"
                                    DisplayMoney="Left" ErrorTooltipEnabled="True" Mask="99/99/9999" MaskType="Date"
                                    MessageValidatorTip="true" OnFocusCssClass="MaskedEditFocus" OnInvalidCssClass="MaskedEditError"
                                    TargetControlID="txtLSIStartDate" />
                                <cc1:CalendarExtender ID="CalendarExtender3" runat="server" PopupButtonID="imgLSIStDate"
                                    TargetControlID="txtLSIStartDate" />
                                <%--<asp:RequiredFieldValidator ID="RequiredLSIStartDate" runat="server" ControlToValidate="txtLSIStartDate"
                                    ErrorMessage="Please Enter LSI Start Date" Text="*" Display="Dynamic" ValidationGroup="Save" />--%>
                                <asp:CompareValidator ID="CompareLSIStartEndDate" runat="server" ControlToValidate="txtLSIStartDate"
                                    Display="Dynamic" ErrorMessage="LSI Start Date must be less than LSI End Date, Please Enter Valid LSI Start Date"
                                    Text="*" Operator="LessThan" ControlToCompare="txtLSIEndDate" ValidationGroup="Save"
                                    Type="Date" />
                                <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtLSIStartDate"
                                    ValidationExpression="(0?[1-9]|1[012])[- /.](0?[1-9]|[12][0-9]|3[01])[- /.](1[8-9]|2[01])\d\d"
                                    ErrorMessage="Please Enter Valid LSI Start Date" Text="*" ValidationGroup="Save"></asp:RegularExpressionValidator>
                            </td>
                            <td style="vertical-align: middle">
                                LSI End Date
                            </td>
                            <td>
                                <asp:TextBox ID="txtLSIEndDate" runat="server" Width="65px" ValidationGroup="Save"></asp:TextBox>
                                <asp:ImageButton ID="imgLSIEnDate" runat="server" ImageUrl="~/images/Calendar_scheduleHS.png"
                                    CausesValidation="False" />
                                <cc1:MaskedEditExtender ID="MaskedEditExtender4" runat="server" TargetControlID="txtLSIEndDate"
                                    Mask="99/99/9999" MessageValidatorTip="true" OnFocusCssClass="MaskedEditFocus"
                                    OnInvalidCssClass="MaskedEditError" MaskType="Date" DisplayMoney="Left" AcceptNegative="Left"
                                    ErrorTooltipEnabled="True" />
                                <cc1:CalendarExtender ID="CalendarExtender4" runat="server" TargetControlID="txtLSIEndDate"
                                    PopupButtonID="imgLSIEnDate" />
                                <%--<asp:RequiredFieldValidator ID="RequiredLSIEndDate" runat="server" ControlToValidate="txtLSIEndDate"
                                    ErrorMessage="Please Enter LSI End Date" Text="*" Display="Dynamic" ValidationGroup="Save" />--%>
                                <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ControlToValidate="txtLSIEndDate"
                                    ValidationExpression="(0?[1-9]|1[012])[- /.](0?[1-9]|[12][0-9]|3[01])[- /.](1[8-9]|2[01])\d\d"
                                    ErrorMessage="Please Enter Valid LSI End Date" Text="*" ValidationGroup="Save"></asp:RegularExpressionValidator>
                            </td>
                        </tr>
                        <tr>
                            <td style="vertical-align: middle">
                                Paid/Incurred
                            </td>
                            <td>
                                <asp:ObjectDataSource ID="PaidIncrredDataSource" runat="server" SelectMethod="GetLookUpActiveData"
                                    TypeName="ZurichNA.AIS.Business.Logic.BLAccess">
                                    <SelectParameters>
                                        <asp:Parameter DefaultValue="PAID/INCURRED (LBA ADJUSTMENT TYPE)" Name="lookUpTypeName"
                                            Type="String" />
                                    </SelectParameters>
                                </asp:ObjectDataSource>
                                <asp:DropDownList ID="ddlPaidIncurred" runat="server" DataSourceID="PaidIncrredDataSource"
                                    DataTextField="LookUpName" DataValueField="LookUpID" Width="93px" ValidationGroup="Save">
                                </asp:DropDownList>
                                <asp:CompareValidator ID="ComparePaidIncurred" runat="server" ControlToValidate="ddlPaidIncurred"
                                    Display="Dynamic" ErrorMessage="Please Select Paid/Incurred" Text="*" Operator="NotEqual"
                                    ValueToCompare="0" ValidationGroup="Save" />
                            </td>
                            <td style="vertical-align: middle">
                                Frequency(P)
                            </td>
                            <td>
                                <asp:TextBox runat="server" ID="txtFreq" Width="65px" ValidationGroup="Save" MaxLength="3"
                                    onblur="onFreqChange()" />
                                <asp:RequiredFieldValidator ID="RequiredtxtFreq" runat="server" ControlToValidate="txtFreq"
                                    ErrorMessage="Please Enter Adjustment Frequency" Text="*" Display="Dynamic" ValidationGroup="Save" />
                                <cc1:FilteredTextBoxExtender ID="FiltFreq" runat="server" TargetControlID="txtFreq"
                                    FilterType="Custom" ValidChars="1234567890" />
                            </td>
                            <td style="vertical-align: middle">
                                Val Date(P)
                            </td>
                            <td>
                                <asp:TextBox ID="txtValDt" runat="server" Width="65px" ValidationGroup="Save" Enabled="false"></asp:TextBox>
                                <asp:TextBox ID="txtHidenValDt" runat="server" Width="65px" CssClass="Hide" />
                            </td>
                            <td style="vertical-align: middle">
                                First Adj(P)
                            </td>
                            <td>
                                <asp:TextBox runat="server" ID="txtFirstAdj" Width="65px" ValidationGroup="Save"
                                    MaxLength="3" onblur="OnFirstAdjChange()" />
                                <asp:TextBox ID="txtFstAdjHidden" runat="server" Width="65px" CssClass="Hide"></asp:TextBox>        
                                <asp:RequiredFieldValidator ID="RequiredFirstAdj" runat="server" ControlToValidate="txtFirstAdj"
                                    ErrorMessage="Please Enter First Adjustment" Text="*" Display="Dynamic" ValidationGroup="Save" />
                                <cc1:FilteredTextBoxExtender ID="filterFirstAdj" runat="server" TargetControlID="txtFirstAdj"
                                    FilterType="Custom" ValidChars="1234567890" />
                            </td>
                        </tr>
                        <tr>
                            <td style="vertical-align: middle">
                                First Adj Date(P)
                            </td>
                            <td>
                                <asp:TextBox ID="txtFirstAdjDt" runat="server" Width="65px" Enabled="false"></asp:TextBox>
                            </td>
                            <td style="vertical-align: middle">
                                Next Val Date(P)
                            </td>
                            <td>
                                <asp:TextBox ID="txtNextValDt" runat="server" ValidationGroup="Save" Width="65px"
                                    Enabled="false"></asp:TextBox>
                                <%--<asp:ImageButton ID="imgNextValDt" runat="server" ImageUrl="~/images/Calendar_scheduleHS.png"
                                CausesValidation="False" />
                            <cc1:MaskedEditExtender ID="MaskedEditExtender2" runat="server" AcceptNegative="Left"
                                DisplayMoney="Left" ErrorTooltipEnabled="True" Mask="99/99/9999" MaskType="Date"
                                MessageValidatorTip="true" OnFocusCssClass="MaskedEditFocus" OnInvalidCssClass="MaskedEditError"
                                TargetControlID="txtNextValDt" />
                            <cc1:CalendarExtender ID="CalendarExtender10" runat="server" PopupButtonID="imgNextValDt"
                                TargetControlID="txtNextValDt" />--%>
                            </td>
                            <td style="vertical-align: middle">
                                Prev Val Date(P)
                            </td>
                            <td>
                                <asp:TextBox ID="txtPrevValDt" runat="server" Width="65px" ValidationGroup="Save"
                                    Enabled="false"></asp:TextBox>
                                <%--<asp:ImageButton ID="imgPrevValDt" runat="server" ImageUrl="~/images/Calendar_scheduleHS.png"
                                CausesValidation="False" />
                            <cc1:MaskedEditExtender ID="MaskedEditExtender1" runat="server" TargetControlID="txtPrevValDt"
                                Mask="99/99/9999" MessageValidatorTip="true" OnFocusCssClass="MaskedEditFocus"
                                OnInvalidCssClass="MaskedEditError" MaskType="Date" DisplayMoney="Left" AcceptNegative="Left"
                                ErrorTooltipEnabled="True" />
                            <cc1:CalendarExtender ID="CalendarExtender9" runat="server" PopupButtonID="imgPrevValDt"
                                TargetControlID="txtPrevValDt" />--%>
                            </td>
                            <td style="vertical-align: middle">
                                Converts At(P)
                            </td>
                            <td>
                                <asp:TextBox ID="txtConvertsAT" runat="server" ValidationGroup="Save" Width="65px"
                                    MaxLength="3"></asp:TextBox>
                                <cc1:FilteredTextBoxExtender ID="filterConvertsAT" runat="server" TargetControlID="txtConvertsAT"
                                    FilterType="Custom" ValidChars="1234567890" />
                            </td>
                        </tr>
                        <tr>
                            <td style="vertical-align: middle">
                                Final Adj(P)
                            </td>
                            <td>
                                <asp:TextBox runat="server" ID="txtFinalAdj" Width="65px" />
                                <asp:ImageButton ID="imgFinalAdj" runat="server" ImageUrl="~/images/Calendar_scheduleHS.png"
                                    CausesValidation="False" />
                                <cc1:MaskedEditExtender ID="MaskedEditExtender7" runat="server" AcceptNegative="Left"
                                    DisplayMoney="Left" ErrorTooltipEnabled="True" Mask="99/99/9999" MaskType="Date"
                                    MessageValidatorTip="true" OnFocusCssClass="MaskedEditFocus" OnInvalidCssClass="MaskedEditError"
                                    TargetControlID="txtFinalAdj" />
                                <cc1:CalendarExtender ID="CalendarExtender5" runat="server" PopupButtonID="imgFinalAdj"
                                    TargetControlID="txtFinalAdj" />
                                <asp:CompareValidator ID="CompareFinalAdj" runat="server" ControlToValidate="txtFinalAdj"
                                    Display="Dynamic" ErrorMessage="Final Adjustment Date must be Greater Then Expiration Date, Please Enter Valid Final Adjustment Date"
                                    Text="*" Operator="GreaterThan" ControlToCompare="txtExpirationDate" ValidationGroup="Save"
                                    Type="Date" />
                                <asp:RegularExpressionValidator ID="RegularExpressionValidator3" runat="server" ControlToValidate="txtFinalAdj"
                                    ValidationExpression="(0?[1-9]|1[012])[- /.](0?[1-9]|[12][0-9]|3[01])[- /.](1[8-9]|2[01])\d\d"
                                    ErrorMessage="Please Enter Valid Final Adjustment Date" Text="*" ValidationGroup="Save"></asp:RegularExpressionValidator>
                            </td>
                            <td style="vertical-align: middle">
                                Frequency(NP)
                            </td>
                            <td>
                                <asp:TextBox ID="txtFreqNonPrem" runat="server" Width="65px" MaxLength="3" onblur="onFreqNPChange()" />
                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="txtFreqNonPrem"
                                    FilterType="Custom" ValidChars="1234567890" />
                            </td>
                            <td style="vertical-align: middle">
                                First Adj(NP)
                            </td>
                            <td>
                                <asp:TextBox ID="txtFirstAdjNONPREM" runat="server" Width="65px" MaxLength="3" onblur="OnFirstAdjNPChange()"></asp:TextBox>
                                <cc1:FilteredTextBoxExtender ID="FilterFirstAdjNONPREM" runat="server" TargetControlID="txtFirstAdjNONPREM"
                                    FilterType="Custom" ValidChars="1234567890" />
                            </td>
                            <td style="vertical-align: middle">
                                Next Val Date(NP)
                            </td>
                            <td>
                                <asp:TextBox ID="txtNextValDtNP" runat="server" Width="65px" Enabled="false"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td style="vertical-align: middle">
                                Prev Val Date(NP)
                            </td>
                            <td>
                                <asp:TextBox ID="txtPrevValDtNP" runat="server" Width="65px" Enabled="false"></asp:TextBox>
                                <asp:TextBox ID="txtFirstAdjDtNP" runat="server" CssClass="Hide"></asp:TextBox>
                            </td>
                            <td style="vertical-align: middle">
                                Final Adj(NP)
                            </td>
                            <td style="vertical-align: middle">
                                <asp:TextBox ID="txtFinalAdjNONPREM" runat="server" Width="65px" MaxLength="3"></asp:TextBox>
                                <asp:ImageButton ID="imgFinalAdjNONPREM" runat="server" ImageUrl="~/images/Calendar_scheduleHS.png"
                                    CausesValidation="False" />
                                <cc1:MaskedEditExtender ID="MaskedEditExtender8" runat="server" AcceptNegative="Left"
                                    DisplayMoney="Left" ErrorTooltipEnabled="True" Mask="99/99/9999" MaskType="Date"
                                    MessageValidatorTip="true" OnFocusCssClass="MaskedEditFocus" OnInvalidCssClass="MaskedEditError"
                                    TargetControlID="txtFinalAdjNONPREM" />
                                <cc1:CalendarExtender ID="CalendarExtender7" runat="server" PopupButtonID="imgFinalAdjNONPREM"
                                    TargetControlID="txtFinalAdjNONPREM" />
                                <asp:CompareValidator ID="compareFinalAdjNONPREM" runat="server" ControlToValidate="txtFinalAdjNONPREM"
                                    Display="Dynamic" ErrorMessage="Final Adj Non-Prem Date must be Greater Then Expiration Date, Please Enter Valid Final Adj Non-Prem Date"
                                    Text="*" Operator="GreaterThan" ControlToCompare="txtExpirationDate" ValidationGroup="Save"
                                    Type="Date" />
                                <asp:RegularExpressionValidator ID="RegularExpressionValidator5" runat="server" ControlToValidate="txtFinalAdjNONPREM"
                                    ValidationExpression="(0?[1-9]|1[012])[- /.](0?[1-9]|[12][0-9]|3[01])[- /.](1[8-9]|2[01])\d\d"
                                    ErrorMessage="Please Enter Valid Final Adj Non-Prem Date" Text="*" ValidationGroup="Save"></asp:RegularExpressionValidator>
                            </td>
                            <td style="vertical-align: middle">
                                <asp:CheckBox ID="chkAvgTM" runat="server" Checked="true" Enabled="false" onclick="EnableTMTextbox()" />
                                Avg TM
                            </td>
                            <td>
                            </td>
                            <td>
                                TM Factor
                            </td>
                            <td>
                                <asp:TextBox ID="txtTMFactor" runat="server" Width="65px" MaxLength="6"></asp:TextBox>
                                <cc1:MaskedEditExtender ID="maskTMFactor" runat="server" Mask="9.999999" MaskType="Number"
                                    TargetControlID="txtTMFactor" />
                                <%--<cc1:FilteredTextBoxExtender ID="FilterTMFactor" runat="server" TargetControlID="txtTMFactor"
                                    FilterType="Custom" ValidChars="1234567890." />--%>
                            </td>
                        </tr>
                        <tr>
                            <td style="vertical-align: middle">
                                PEO Deposit
                            </td>
                            <td>
                                <asp:AISAmountTextbox ID="txtPEOPayIn" runat="server" Width="85px"  Font-Size="XX-Small"></asp:AISAmountTextbox>
                               <%-- <cc1:FilteredTextBoxExtender ID="FilterPEOPayIn" runat="server" TargetControlID="txtPEOPayIn"
                                 FilterType="Custom" ValidChars="1234567890." />   --%>  
                                 <%--<cc1:FilteredTextBoxExtender ID="FilterPEOPayIn" runat="server" TargetControlID="txtPEOPayIn"
                                 FilterType="Custom" FilterMode="ValidChars" ValidChars="0123456789," />--%>                                                                 
                            </td>
                            <td>
                                <asp:CheckBox ID="chkInclCptvPKCodes" runat="server" Text="Incl Captive PK" />
                            </td>
                            <td>
                                <asp:CheckBox ID="chkCHFZSC" runat="server" Text="CHF (ZSC)" />
                            </td>
                            <td style="vertical-align: middle">
                                Comb Elmts Max
                            </td>
                            <td>
                                <asp:AISAmountTextbox runat="server" ID="txtCombEleMax" Width="85px"  Font-Size="XX-Small" />
                                <%--<cc1:FilteredTextBoxExtender ID="FilterCombEleMax" runat="server" TargetControlID="txtCombEleMax"
                                    FilterType="Custom" ValidChars="1234567890." />--%>
                                    <%--<cc1:FilteredTextBoxExtender ID="FilterCombEleMax" runat="server" TargetControlID="txtCombEleMax"
                                 FilterType="Custom" FilterMode="ValidChars" ValidChars="0123456789," />--%> 
                            </td>
                            <td style="vertical-align: middle" colspan="2">
                                <asp:CheckBox ID="chkStdSubjPrem" Checked="false" runat="server"  />
                                Std Subj Prem for Surcharges
                            </td>
                        </tr>
                        <tr>
                            <td style="vertical-align: middle">
                                Bktcy/Buyout
                            </td>
                            <td>
                                <asp:ObjectDataSource ID="BKTCYBUYOUTDataSource" runat="server" SelectMethod="GetLookUpActiveData"
                                    TypeName="ZurichNA.AIS.Business.Logic.BLAccess">
                                    <SelectParameters>
                                        <asp:Parameter DefaultValue="BKTCY/BUYOUT" Name="lookUpTypeName" Type="String" />
                                    </SelectParameters>
                                </asp:ObjectDataSource>
                                <asp:DropDownList ID="ddlBKTCYBUYOUT" runat="server" DataSourceID="BKTCYBUYOUTDataSource"
                                    DataTextField="LookUpName" DataValueField="LookUpID" Width="93px" onchange="EnableBKTCYBUYOUTEffDt()">
                                </asp:DropDownList>
                            </td>
                            <td style="vertical-align: middle">
                                Bktcy/Buyout Eff Date
                            </td>
                            <td>
                                <asp:TextBox runat="server" ID="txtBKTCYBUYOUTEffDate" Width="65px" Enabled="false" />
                                <asp:ImageButton ID="imgBKTCYBUYOUTEffDate" runat="server" ImageUrl="~/images/Calendar_scheduleHS.png"
                                    CausesValidation="False" Enabled="false" />
                                <cc1:MaskedEditExtender ID="mskEditBKTCYBUYOUTEffDate" runat="server" AcceptNegative="Left"
                                    DisplayMoney="Left" ErrorTooltipEnabled="True" Mask="99/99/9999" MaskType="Date"
                                    MessageValidatorTip="true" OnFocusCssClass="MaskedEditFocus" OnInvalidCssClass="MaskedEditError"
                                    TargetControlID="txtBKTCYBUYOUTEffDate" />
                                <cc1:CalendarExtender ID="CalendarExtender2" runat="server" PopupButtonID="imgBKTCYBUYOUTEffDate"
                                    TargetControlID="txtBKTCYBUYOUTEffDate" />
                                <asp:RegularExpressionValidator ID="RegularExpressionValidator4" runat="server" ControlToValidate="txtBKTCYBUYOUTEffDate"
                                    ValidationExpression="(0?[1-9]|1[012])[- /.](0?[1-9]|[12][0-9]|3[01])[- /.](1[8-9]|2[01])\d\d"
                                    ErrorMessage="Please Enter Valid BKTCY/BUYOUT Eff Date" Text="*" ValidationGroup="Save"></asp:RegularExpressionValidator>
                            </td>
                            <td style="vertical-align: middle">
                                LSI Ret From Date
                            </td>
                            <td>
                                <asp:TextBox  ID="txtLSIRetriveFromDt" runat="server" Width="65px"  ValidationGroup="Save"></asp:TextBox>
                                <asp:ImageButton ID="imgLSIRetriveFromDt" runat="server" ImageUrl="~/images/Calendar_scheduleHS.png"
                                    CausesValidation="False" />
                                <cc1:MaskedEditExtender ID="MasLSIRetriveFromDt" runat="server" AcceptNegative="Left"
                                    DisplayMoney="Left" ErrorTooltipEnabled="True" Mask="99/99/9999" MaskType="Date"
                                    MessageValidatorTip="true" OnFocusCssClass="MaskedEditFocus" OnInvalidCssClass="MaskedEditError"
                                    TargetControlID="txtLSIRetriveFromDt" />
                                <cc1:CalendarExtender ID="CalLSIRetriveFromDt" runat="server" PopupButtonID="imgLSIRetriveFromDt"
                                    TargetControlID="txtLSIRetriveFromDt" />
                                <asp:RegularExpressionValidator ID="ReguLSIRetriveFromDt" runat="server" ControlToValidate="txtLSIRetriveFromDt"
                                    ValidationExpression="(0?[1-9]|1[012])[- /.](0?[1-9]|[12][0-9]|3[01])[- /.](1[8-9]|2[01])\d\d"
                                    ErrorMessage="Please Enter Valid LSI Retrive From Date" Text="*" ValidationGroup="Save"></asp:RegularExpressionValidator>
                            </td>
                            <td style="vertical-align: middle" colspan ="2" nowrap>
                                <asp:CheckBox ID="chkIncldAllSur"  Checked="false"  runat="server"  />
                                Include All Surcharges
                            </td>
                            <td>
                            </td>
                        </tr>
                    </table>
                    <table width="100%" border="1" cellpadding="0" cellspacing="0" style="background-color: #e7e7ff">
                        <tr>
                            <td>
                                <table width="100%" class="panelContents">
                                    <tr>
                                        <td width="20%" align="center" style="vertical-align: middle;">
                                            <asp:Label ID="lblProgramStatus" Text="Program Status:" runat="server" Font-Bold="true" />
                                        </td>
                                        <td width="20%%" align="left">
                                            <asp:TextBox ID="txtInitialStsID" runat="server" CssClass="Hide" />
                                            <asp:TextBox ID="txtInitialOldSts" runat="server" CssClass="Hide" />
                                            <asp:CheckBox ID="chkInitial" Text="Initial" Width="150px" Checked="true" Enabled="false"
                                                runat="server" Font-Bold="true" />
                                            <asp:Label ID="lblInitialPeriod" runat="server" />
                                        </td>
                                        <td width="20%" align="left">
                                            <asp:TextBox ID="txtSubmitStsID" runat="server" CssClass="Hide" />
                                            <asp:TextBox ID="txtSubmitOldSts" runat="server" CssClass="Hide" />
                                            <asp:CheckBox ID="chkSubmitForSetupQC" Text="Submit For Setup QC" runat="server" Font-Bold="true"  /> <%--onclick="EnableActiveStatusCheckBox()--%>
                                            <asp:Label ID="lblSubmitQCPeriod" runat="server" />
                                        </td>
                                        <td width="20%" align="left">
                                            <asp:TextBox ID="txtSetupStsID" runat="server" CssClass="Hide" />
                                            <asp:TextBox ID="txtSetupOldSts" runat="server" CssClass="Hide" />
                                            <asp:CheckBox ID="chkSetupQCComplete" Text="Setup QC Complete" runat="server" Font-Bold="true" /><%--onclick="EnableActiveStatusCheckBox()--%>
                                            <asp:Label ID="lblSetupQCPeriod" runat="server" />
                                        </td>
                                        <td width="20%" align="left">
                                            <asp:TextBox ID="txtActiveStsID" runat="server" CssClass="Hide" />
                                            <asp:TextBox ID="txtActiveOldsts" runat="server" CssClass="Hide" />
                                            <asp:CheckBox ID="chkActive" Text="Active" Width="150px" runat="server" Font-Bold="true" /> <%--onclick="EnableActiveStatusCheckBox()--%>                                                
                                            <asp:Label ID="lblActivePeriod" runat="server" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                    <table width="100%">
                        <tr>
                            <td colspan="4" style="text-align: center">
                                <asp:HiddenField ID="hidSelValue" runat="server" />
                                <asp:Button ID="btnSave" runat="server" Enabled="false" OnClientClick="disableControl();" 
                                    Text="Save" ValidationGroup="Save" Width="60px" />
                                <asp:Button ID="btnSaveHidden" runat="server" style="display:none" OnClick="SAVE_Click" ValidationGroup="Save" />
                                <asp:Button ID="btnCopy" Enabled="false" runat="server" Text="Copy" OnClick="btnCopy_Click"
                                    Width="60px" />
                                <asp:Button ID="btnCancel" runat="server" Text="Cancel" OnClick="btnCancel_Click"
                                     Width="60px" />
                            </td>
                        </tr>
                    </table>
                </asp:Panel>

        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
