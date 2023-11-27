<%@ Page Language="C#" MasterPageFile="~/Retro.master" AutoEventWireup="true" Inherits="AcctSetup_Aries_Clearing"
    Title="Adjustment Invoicing System" CodeBehind="AriesClearing.aspx.cs" %>

<%@ Register Src="~/App_Shared/AccountInfoHeader.ascx" TagName="AccountInfoHeader"
    TagPrefix="AH" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeaderPlaceHolder" runat="Server">
    <asp:Label ID="lblRetroInfo" runat="server" Text="ARiES Clearing" CssClass="h1"></asp:Label>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainPlaceHolder" runat="Server">

    <script type="text/javascript" src="../JavaScript/RetroScript.js"></script>

    <script type="text/javascript">
 // Declaring valid date character, minimum year and maximum year
var dtCh= "/";
var minYear=1900;
var maxYear=2100;

function isInteger(s){
	var i;
    for (i = 0; i < s.length; i++){   
        // Check that current character is number.
        var c = s.charAt(i);
        if (((c < "0") || (c > "9"))) return false;
    }
    // All characters are numbers.
    return true;
}

function stripCharsInBag(s, bag){
	var i;
    var returnString = "";
    // Search through string's characters one by one.
    // If character is not in bag, append to returnString.
    for (i = 0; i < s.length; i++){   
        var c = s.charAt(i);
        if (bag.indexOf(c) == -1) returnString += c;
    }
    return returnString;
}

function daysInFebruary (year){
	// February has 29 days in any year evenly divisible by four,
    // EXCEPT for centurial years which are not also divisible by 400.
    return (((year % 4 == 0) && ( (!(year % 100 == 0)) || (year % 400 == 0))) ? 29 : 28 );
}
function DaysArray(n) {
	for (var i = 1; i <= n; i++) {
		this[i] = 31
		if (i==4 || i==6 || i==9 || i==11) {this[i] = 30}
		if (i==2) {this[i] = 29}
   } 
   return this
}

function isDate(dtStr){
	var daysInMonth = DaysArray(12)
	var pos1=dtStr.indexOf(dtCh)
	var pos2=dtStr.indexOf(dtCh,pos1+1)
	var strMonth=dtStr.substring(0,pos1)
	var strDay=dtStr.substring(pos1+1,pos2)
	var strYear=dtStr.substring(pos2+1)
	strYr=strYear
	if (strDay.charAt(0)=="0" && strDay.length>1) strDay=strDay.substring(1)
	if (strMonth.charAt(0)=="0" && strMonth.length>1) strMonth=strMonth.substring(1)
	for (var i = 1; i <= 3; i++) {
		if (strYr.charAt(0)=="0" && strYr.length>1) strYr=strYr.substring(1)
	}
	month=parseInt(strMonth)
	day=parseInt(strDay)
	year=parseInt(strYr)
	if (pos1==-1 || pos2==-1){
		//alert("The date format should be : mm/dd/yyyy")
		return false
	}
	if (strMonth.length<1 || month<1 || month>12){
		//alert("Please enter a valid month")
		return false
	}
	if (strDay.length<1 || day<1 || day>31 || (month==2 && day>daysInFebruary(year)) || day > daysInMonth[month]){
		//alert("Please enter a valid day")
		return false
	}
	if (strYear.length != 4 || year==0 || year<minYear || year>maxYear){
		//alert("Please enter a valid 4 digit year between "+minYear+" and "+maxYear)
		return false
	}
	if (dtStr.indexOf(dtCh,pos2+1)!=-1 || isInteger(stripCharsInBag(dtStr, dtCh))==false){
		//alert("Please enter a valid date")
		return false
	}
    return true
    }

 function test1()
 {
 
        /*var txtamount=document.getElementById('<%=txtAmount.ClientID%>').value;       
        var txtdate=document.getElementById('<%=txtBilleddate.ClientID%>').value;   
        var txtcheque=document.getElementById('<%=txtCheck.ClientID%>').value
       //alert(txtamount.length);
         alert('test');
      if ((txtamount.length > 0)&& (txtdate.length==10) && (txtdate!="__/__/____"))
        {
      
         document.getElementById('<%=btnfinalise.ClientID%>').disabled=false;
        
        }
       else
       {
      
       
       document.getElementById('<%=btnfinalise.ClientID%>').disabled=true;
       }*/
       
    
 }
 
    </script>

    <AH:AccountInfoHeader ID="ucAccountHeader" runat="server" />
    <div>
        <asp:ObjectDataSource ID="PersonDatasource" runat="server" SelectMethod="getPersonsList"
            TypeName="ZurichNA.AIS.Business.Logic.PersonBS"></asp:ObjectDataSource>
        <asp:ObjectDataSource ID="BusinessUnitDataSource" runat="server" SelectMethod="GetBusinessUnits">
        </asp:ObjectDataSource>
        <asp:ObjectDataSource ID="ExposureDataSource" runat="server" SelectMethod="GetLookUpActiveData"
            TypeName="ZurichNA.AIS.Business.Logic.BLAccess">
            <SelectParameters>
                <asp:Parameter Name="lookUpTypeName" Type="String" DefaultValue="EXPOSURE TYPE" />
            </SelectParameters>
        </asp:ObjectDataSource>
        <br />
        <asp:ValidationSummary ID="valdnSave" runat="server" ValidationGroup="save" />
        <asp:ValidationSummary ID="valdnPayment" runat="server" ValidationGroup="PaymentSave" />
        <asp:ValidationSummary ID="valnfinalise" runat="server" ValidationGroup="Finalise" />
        <table>
            <tr style="height: 3px;">
                <td>
                </td>
            </tr>
            <tr>
                <td>
                    <cc1:TabContainer ID="tcAriesClearing" runat="server" CssClass="CustomTabs" ActiveTabIndex="0" AutoPostBack="true">
                         
                        <cc1:TabPanel ID="tpClearingInfo" runat="server" TabIndex="0">
                            <HeaderTemplate>
                                Aries QC Details</HeaderTemplate>
                            <ContentTemplate>
                            <div id="divAriesclring" runat="server">
                                <table width="100%" id="tblAries" runat="server">
                                    <tr style="height: 5px;">
                                        <td>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <table width="100%" class="panelcontents">
                                                <tr>
                                                    <th align="center" colspan="2">
                                                        Aries QC
                                                    </th>
                                                </tr>
                                                <tr class="ItemTemplate">
                                                    <td align="left">
                                                        Reconcilation Due Date:
                                                    </td>
                                                    <td align="left">
                                                        <asp:Label ID="lblreconduedate" runat="server"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr class="AlternatingItemTemplate">
                                                    <td align="left">
                                                        Reconcilation Date:
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtRecondate" runat="server" Width="175px" ValidationGroup="save"></asp:TextBox>
                                                        <asp:RequiredFieldValidator ID="reqRecondate" runat="server" ValidationGroup="save"
                                                            ControlToValidate="txtRecondate" Text="*" ErrorMessage="Please enter Reconcilation date."></asp:RequiredFieldValidator>
                                                        <cc1:MaskedEditExtender ID="mskDaterecieved" runat="server" TargetControlID="txtRecondate"
                                                            Mask="99/99/9999" MaskType="Date" DisplayMoney="Left" AcceptNegative="Left" ErrorTooltipEnabled="True"
                                                            CultureAMPMPlaceholder="" CultureCurrencySymbolPlaceholder="" CultureDateFormat=""
                                                            CultureDatePlaceholder="" CultureDecimalPlaceholder="" CultureThousandsPlaceholder=""
                                                            CultureTimePlaceholder="" Enabled="True" />
                                                        <asp:RegularExpressionValidator ID="regValidFrom" runat="server" ControlToValidate="txtRecondate"
                                                            ValidationExpression="(0?[1-9]|1[012])[- /.](0?[1-9]|[12][0-9]|3[01])[- /.](1[3-9]|2[01])\d\d"
                                                            ErrorMessage="Invalid Reconcilaton Date" Text="*" ValidationGroup="save"></asp:RegularExpressionValidator>
                                                        <cc1:CalendarExtender ID="txtDtreciev_CalendarExtender1" runat="server" TargetControlID="txtRecondate"
                                                            PopupButtonID="imgDtrecieved" Enabled="True">
                                                        </cc1:CalendarExtender>
                                                        <asp:ImageButton ID="imgDtrecieved" runat="server" ImageUrl="~/images/Calendar_scheduleHS.png"
                                                            CausesValidation="False" />
                                                    </td>
                                                </tr>
                                                <tr class="ItemTemplate">
                                                    <td align="left">
                                                        ARiES Recon Rep:
                                                    </td>
                                                    <td align="left">
                                                        <asp:Label ID="lblReconrep" runat="server"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr class="AlternatingItemTemplate">
                                                    <td align="left">
                                                        ARiES QC By:
                                                    </td>
                                                    <td align="left">
                                                        <asp:Label ID="lblQCby" runat="server"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr class="ItemTemplate">
                                                    <td align="left">
                                                        ARiES QC Date:
                                                    </td>
                                                    <td align="left">
                                                        <asp:TextBox runat="server" ID="txtAriesQCdate" autocomplete="off" Width="175px" />
                                                        <cc1:MaskedEditExtender ID="mskEditQCdate" runat="server" TargetControlID="txtAriesQCdate"
                                                            Mask="99/99/9999" MaskType="Date" DisplayMoney="Left" AcceptNegative="Left" ErrorTooltipEnabled="True"
                                                            CultureAMPMPlaceholder="" CultureCurrencySymbolPlaceholder="" CultureDateFormat=""
                                                            CultureDatePlaceholder="" CultureDecimalPlaceholder="" CultureThousandsPlaceholder=""
                                                            CultureTimePlaceholder="" Enabled="True" />
                                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtAriesQCdate"
                                                            ValidationExpression="(0?[1-9]|1[012])[- /.](0?[1-9]|[12][0-9]|3[01])[- /.](1[3-9]|2[01])\d\d"
                                                            ErrorMessage="Invalid Aries QC Date" Text="*" ValidationGroup="save"></asp:RegularExpressionValidator>
                                                        <cc1:CalendarExtender ID="txtQCdate_Calenderextender" runat="server" TargetControlID="txtAriesQCdate"
                                                            PopupButtonID="imgQCdate" Enabled="True">
                                                        </cc1:CalendarExtender>
                                                        &nbsp;&nbsp;
                                                        <asp:ImageButton ID="imgQCdate" runat="server" ImageUrl="~/images/Calendar_scheduleHS.png"
                                                            CausesValidation="False" />
                                                    </td>
                                                </tr>
                                                <tr class="ItemTemplate">
                                                    <td align="left">
                                                        Comments:
                                                    </td>
                                                    <td align="left">
                                                        <asp:TextBox ID="txtComments" TextMode="MultiLine" runat="server"></asp:TextBox>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                        <td style="padding-left: 35px;">
                                            <asp:AISListView ID="lstAriesissues" runat="server" InsertItemPosition="FirstItem"
                                                DataKeyNames="LookUpID" OnItemCommand="CommandList" OnItemDataBound="DataBoundList"
                                                Enabled="false">
                                                <LayoutTemplate>
                                                    <table id="tblIssues" class="panelContents" runat="server" width="98%">
                                                        <tr class="LayoutTemplate">
                                                            <th>
                                                            </th>
                                                            <th>
                                                                Issues
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
                                                    <tr class="ItemTemplate">
                                                        <td align="left">
                                                        </td>
                                                        <td align="left">
                                                            <asp:HiddenField ID="HidQltyChkID" runat="server" />
                                                            <%# Eval("CHKLISTNAME")%>
                                                        </td>
                                                        <td>
                                                            <asp:HiddenField ID="hidActive" runat="server" Value='<%# Bind("ACTIVE")%>' />
                                                            <asp:ImageButton ID="imgDisable" CommandArgument='<%# Bind("QualityControlChklst_ID") %>'
                                                                runat="server" ImageUrl='<%# Eval("ACTIVE").ToString()=="True"?"~/images/disabled.GIF":"~/images/enabled.GIF" %>'>
                                                            </asp:ImageButton>
                                                        </td>
                                                    </tr>
                                                </ItemTemplate>
                                                <AlternatingItemTemplate>
                                                    <tr class="AlternatingItemTemplate">
                                                        <td align="left">
                                                        </td>
                                                        <td align="left">
                                                            <asp:HiddenField ID="HidQltyChkID" runat="server" />
                                                            <%# Eval("CHKLISTNAME")%>
                                                        </td>
                                                        <td>
                                                            <asp:HiddenField ID="hidActive" runat="server" Value='<%# Bind("ACTIVE")%>' />
                                                            <asp:ImageButton ID="imgDisable" CommandArgument='<%# Bind("QualityControlChklst_ID") %>'
                                                                runat="server" ImageUrl='<%# Eval("ACTIVE").ToString()=="True"?"~/images/disabled.GIF":"~/images/enabled.GIF" %>'>
                                                            </asp:ImageButton>
                                                        </td>
                                                    </tr>
                                                </AlternatingItemTemplate>
                                                <InsertItemTemplate>
                                                    <tr class="ItemTemplate">
                                                        <td align="left">
                                                            <asp:LinkButton ID="lnkSave" runat="server" ValidationGroup="SaveDetail" CommandName="Save"
                                                                Text="SAVE"></asp:LinkButton>
                                                        </td>
                                                        <td align="left">
                                                            <asp:DropDownList ID="ddlIssue" runat="server" ValidationGroup="SaveDetail">
                                                            </asp:DropDownList>
                                                            <asp:CompareValidator ID="compIssue" runat="server" ControlToValidate="ddlIssue"
                                                                ValidationGroup="SaveDetail" ValueToCompare="0" Text="*" ErrorMessage="Please select an Item"
                                                                Operator="NotEqual"></asp:CompareValidator>
                                                        </td>
                                                        <td>
                                                        </td>
                                                    </tr>
                                                </InsertItemTemplate>
                                            </asp:AISListView>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="center" class="AlternatingItemTemplate">
                                            <asp:Button ID="btnDetailssave" Text=" Save " runat="server" OnClick="btnDetailssave_Click"
                                                ValidationGroup="save" />
                                        </td>
                                    </tr>
                                </table>
                                </div>
                            </ContentTemplate>
                        </cc1:TabPanel>
                        <cc1:TabPanel ID="tpIssues" runat="server" CssClass="CustomTabs" TabIndex="1">
                            <HeaderTemplate>
                                Payment Information
                            </HeaderTemplate>
                            <ContentTemplate>
                                <table width="100%">
                                    <tr class="Panelconents">
                                        <th align="center" colspan="2">
                                            Payment Information
                                        </th>
                                    </tr>
                                    <tr class="ItemTemplate">
                                        <td align="left">
                                            ARiES Posting Date:
                                        </td>
                                        <td align="left">
                                            <asp:TextBox ID="txtPostingdate" runat="server" Width="175px"></asp:TextBox>
                                            <cc1:MaskedEditExtender ID="mskPostingdate" runat="server" TargetControlID="txtPostingdate"
                                                Mask="99/99/9999" MaskType="Date" DisplayMoney="Left" AcceptNegative="Left" ErrorTooltipEnabled="True"
                                                CultureAMPMPlaceholder="" CultureCurrencySymbolPlaceholder="" CultureDateFormat=""
                                                CultureDatePlaceholder="" CultureDecimalPlaceholder="" CultureThousandsPlaceholder=""
                                                CultureTimePlaceholder="" Enabled="True" />
                                            <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ControlToValidate="txtPostingdate"
                                                ValidationExpression="(0?[1-9]|1[012])[- /.](0?[1-9]|[12][0-9]|3[01])[- /.](1[3-9]|2[01])\d\d"
                                                ErrorMessage="Invalid Posting Date" Text="*" ValidationGroup="PaymentSave"></asp:RegularExpressionValidator>
                                            <cc1:CalendarExtender ID="txtPostdate_CalendarExtender" runat="server" TargetControlID="txtPostingdate"
                                                PopupButtonID="imgCalender" Enabled="True">
                                            </cc1:CalendarExtender>
                                            &nbsp;&nbsp;&nbsp;&nbsp;
                                            <asp:ImageButton ID="imgCalender" runat="server" ImageUrl="~/images/Calendar_scheduleHS.png"
                                                CausesValidation="False" />
                                        </td>
                                    </tr>
                                    <tr class="AlternatingItemTemplate">
                                        <td align="left">
                                            Check#:
                                        </td>
                                        <td align="left">
                                            <asp:TextBox ID="txtCheck" runat="server" Width="175px" MaxLength="25"></asp:TextBox>
                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" FilterMode="ValidChars"
                                                TargetControlID="txtCheck" FilterType="LowercaseLetters, Numbers,UppercaseLetters"
                                                runat="server">
                                            </cc1:FilteredTextBoxExtender>
                                        </td>
                                    </tr>
                                    <tr class="ItemTemplate">
                                        <td align="left">
                                            Amount$:
                                        </td>
                                        <td align="left">
                                            <asp:AISAmountTextbox ID="txtAmount" autocomplete="off" runat="server" Width="175px"  ValidationGroup="Finalise" AllowNegetive=true></asp:AISAmountTextbox>
                                                                                            
                                             <%--<cc1:FilteredTextBoxExtender runat="server" TargetControlID="txtAmount" FilterType="Custom"
                                ValidChars="-0123456789," ID="fltAmount">
                            </cc1:FilteredTextBoxExtender>--%>

                                            <asp:RequiredFieldValidator ID="reqAmount" runat="server" ControlToValidate="txtAmount"
                                                ValidationGroup="Finalise" ErrorMessage="Please enter amount" Text="*"></asp:RequiredFieldValidator>
                                        </td>
                                    </tr>
                                    <tr class="AlternatingItemTemplate">
                                        <td align="left">
                                            Clearing Date:
                                        </td>
                                        <td align="left">
                                            <asp:TextBox runat="server" ID="txtBilleddate" autocomplete="off" Width="175px" onkeyup="test1()"
                                                onblur="test1()" onkeypress="test1()" />
                                            <asp:RequiredFieldValidator ID="reqBilleddate" runat="server" ValidationGroup="Finalise"
                                                ControlToValidate="txtBilleddate" ErrorMessage="Please enter Billed item clearing Date" Text="*"></asp:RequiredFieldValidator>
                                            <asp:RegularExpressionValidator ID="RegBilleddate" runat="server" ControlToValidate="txtBilleddate"
                                                ValidationExpression="(0?[1-9]|1[012])[- /.](0?[1-9]|[12][0-9]|3[01])[- /.](1[3-9]|2[01])\d\d"
                                                ErrorMessage="Invalid Clearing Date" Text="*" ValidationGroup="PaymentSave"></asp:RegularExpressionValidator>
                                            <asp:RegularExpressionValidator ID="RegFbilled" runat="server" ControlToValidate="txtBilleddate"
                                                ValidationExpression="(0?[1-9]|1[012])[- /.](0?[1-9]|[12][0-9]|3[01])[- /.](1[3-9]|2[01])\d\d"
                                                ErrorMessage="Invalid Clearing Date" Text="*" ValidationGroup="Finalise"></asp:RegularExpressionValidator>
                                            <cc1:MaskedEditExtender ID="mskBilleddate" runat="server" TargetControlID="txtBilleddate"
                                                Mask="99/99/9999" MaskType="Date" DisplayMoney="Left" AcceptNegative="Left" ErrorTooltipEnabled="True"
                                                CultureAMPMPlaceholder="" CultureCurrencySymbolPlaceholder="" CultureDateFormat=""
                                                CultureDatePlaceholder="" CultureDecimalPlaceholder="" CultureThousandsPlaceholder=""
                                                CultureTimePlaceholder="" Enabled="True" />
                                            <cc1:CalendarExtender ID="txtPostingdate_CalendarExtender" runat="server" TargetControlID="txtBilleddate"
                                                PopupButtonID="imgBilleddate" Enabled="True">
                                            </cc1:CalendarExtender>
                                            <asp:ImageButton ID="imgBilleddate" runat="server" ImageUrl="~/images/Calendar_scheduleHS.png"
                                                CausesValidation="False" />
                                        </td>
                                    </tr>
                                    <tr class="ItemTemplate">
                                        <td align="center" colspan="2">
                                            <asp:Button ID="btnPaymentsave" runat="server" Text="Save" OnClick="Paymentsave"
                                                ValidationGroup="PaymentSave"  CausesValidation="false"/>
                                            <asp:Button ID="btnfinalise" runat="server" Text="Finalize" Enabled="true" OnClick="finalise"
                                                ValidationGroup="Finalise" />
                                        </td>
                                    </tr>
                                </table>
                            </ContentTemplate>
                        </cc1:TabPanel>
                    </cc1:TabContainer>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
