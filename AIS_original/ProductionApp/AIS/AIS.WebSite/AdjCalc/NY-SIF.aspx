<%@ Page Language="C#" MasterPageFile="~/Retro.master" AutoEventWireup="true" Inherits="AdjCalc_NY_SIF"
    CodeBehind="NY-SIF.aspx.cs" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/App_Shared/AdjustmentReviewSearch.ascx" TagPrefix="ARS" TagName="AdjustmentReviewSearch" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeaderPlaceHolder" runat="server">
    <asp:Label ID="lblAdjReview" runat="server" Text="Adjustment Review" CssClass="h1"></asp:Label>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainPlaceHolder" runat="Server">
 <script type="text/javascript" src="../JavaScript/RetroScript.js"></script>   
    <script language="javascript" type="text/javascript">
     //Function to Redirect to differenet Tabs
 
function Tabnavigation(Pagename)
        {
            var selectedValues= $get('<%=hidSelectedValues.ClientID%>');
            var strURL="../AdjCalc/";
            if(Pagename=="ARC")
            {
                strURL +="AdjustmentReview.aspx";
            }
            else if(Pagename=="ARPLB")
            {
                strURL +="PaidLossBilling.aspx";
            }
            else if(Pagename=="AREA")
            {
                strURL +="EscrowAdjustment.aspx";
            }
            else if(Pagename=="ARCE")
            {
                strURL +="CombinedElements.aspx";
            }
            else if(Pagename=="ARNYSIF")
            {
                strURL +="NY-SIF.aspx";
            }
            else if(Pagename=="ARMI")
            {
                strURL +="MiscInvoicing.aspx";
            }
 	        else if(Pagename=="ARLRFP")
            {
                strURL +="LRFPostingDetails.aspx";
            }
            else if(Pagename=="ARAPCL")
            {
                strURL +="AdjustProcessingChklst.aspx";
            }
            else if(Pagename=="ARANTM")
            {
                strURL +="AdjustmentNumberTextMaintenance.aspx";
            }
            else if (Pagename == "ARBB") 
            {
                strURL += "BuBrokerReview.aspx";
            }
            if(selectedValues.value!="")
            {
            strURL +="?SelectedValues="+selectedValues.value;
            }
            window.location.href=strURL;
        }  
    </script>

    <asp:ValidationSummary ID="VSSaveNYSIF" runat="server" ValidationGroup="NYSIFSaveGroup"
        Height="30px" CssClass="ValidationSummary" />
    <table>
        <tr>
            <td>
                <cc1:TabContainer ID="TabContainer1" runat="server" CssClass="VariableTabs" SkinID="tabVariable"
                    ActiveTabIndex="4">
                    <cc1:TabPanel runat="server" ID="tblpnlLBA">
                        <HeaderTemplate>
                            <div onclick="Tabnavigation('ARC')">
                                Review Comments
                            </div>
                        </HeaderTemplate>
                        <ContentTemplate>
                        </ContentTemplate>
                    </cc1:TabPanel>
                    <cc1:TabPanel runat="server" ID="tblpnlLCF">
                        <HeaderTemplate>
                            <div onclick="Tabnavigation('ARPLB')">
                                Adj. PLB
                            </div>
                        </HeaderTemplate>
                        <ContentTemplate>
                        </ContentTemplate>
                    </cc1:TabPanel>
                    <cc1:TabPanel runat="server" ID="tblpnlTM">
                        <HeaderTemplate>
                            <div onclick="Tabnavigation('AREA')">
                                Escrow Adj.
                            </div>
                        </HeaderTemplate>
                        <ContentTemplate>
                        </ContentTemplate>
                    </cc1:TabPanel>
                    <cc1:TabPanel runat="server" ID="tblpnlCE">
                        <HeaderTemplate>
                            <div onclick="Tabnavigation('ARCE')">
                                Comb.Elements
                            </div>
                        </HeaderTemplate>
                        <ContentTemplate>
                        </ContentTemplate>
                    </cc1:TabPanel>
                    <!--Start of NY-SIF Tab Section-->
                    <cc1:TabPanel runat="server" ID="tblpnlNYSIF">
                        <HeaderTemplate>
                            NY-SIF
                        </HeaderTemplate>
                        <ContentTemplate>
                            <!--Start of JavaScript Section -->

                            <script src="../JavaScript/RetroScript.js" type="text/javascript"></script>

                            <script type="text/javascript" language="javascript">
                            function addCommas(nStr) {
                                    nStr += '';
                                    x = nStr.split('.');
                                    x1 = x[0];
                                    x2 = x.length > 1 ? '.' + x[1] : '';
                                    var rgx = /(\d+)(\d{3})/;
                                    while (rgx.test(x1)) {
                                        x1 = x1.replace(rgx, '$1' + ',' + '$2');
                                    }
                                    return x1 + x2;
                                }
                            function CheckBoxListSelect(cbControl,selctedindex)
                            {    
                                   var chkBoxList = document.getElementById(cbControl);
                                   var chkBoxCount= chkBoxList.getElementsByTagName("input");
                                    for(var i=0;i<chkBoxCount.length;i++) 
                                    {
                                        chkBoxCount[i].checked =false;
                                    }
                                    chkBoxCount[selctedindex].checked=true;
                                   
                            }
                            function CheckPolicy(oSource, oArgs)
                            {      
                            var chkBoxList=document.getElementById('<%=ckkboxlstPolicyno.ClientID%>'); 
                            var chkBoxCount= chkBoxList.getElementsByTagName("input");
                            var iCount=0;
                                    for(var i=0;i<chkBoxCount.length;i++) 
                                    {
                                        if(chkBoxCount[i].checked ==true)
                                        iCount++;
                                    } 
                            
                            var result=false;
                                if (iCount > 0)  result=true;  
                                else  result=false;
                                
                                oArgs.IsValid = result;
                            }
                           //Function to round the given decimal number
                           //Math.pow(10,N), Based on N Value that many Places this will round off
                            function fix(fixNumber)
                            {
                                //var div=Math.pow(10,2);
                                //fixNumber=Math.round(fixNumber*div)/div;
                                fixNumber = Math.round(fixNumber);
                            return fixNumber;
                            }
                            
                            //Function to Calculate the Converted Loss
                            //Formula:
                            //ConvertedLosses=NY Incurred Losses * LCF
                        function CalculateConvertedLosses()
                        {
                        var myRegExp = /,|_/g;
                        varNYIncurresLosses= $get('<%=txtNYIncurredLosses.ClientID%>');
                        varLCF=$get('<%=txtLCF.ClientID%>');
                        varConvertedLosses=$get('<%=txtConvertedLosses.ClientID%>');
                        var result;
                        if ((varNYIncurresLosses.value.replace(myRegExp, '') != "") && (varLCF.value.replace(myRegExp, '') != ""))
                        {
                            if (isNaN(varNYIncurresLosses.value.replace(myRegExp, '')) == true)
                                {
                                  alert("Please Enter Valid NY INCURRED LOSSES");
                                  return;
                                }
                                var myRegExp1=/_/g;
                                varLCFModified=varLCF.value.replace(myRegExp1,'0');
                                if(isNaN(varLCFModified)==true)
                                {
                                  alert("Please Enter Valid LCF");
                                  return;
                                }
                                result = parseFloat(varNYIncurresLosses.value.replace(myRegExp, '')) * parseFloat(varLCFModified);
                                   varConvertedLosses.value=addCommas(fix(result));
                                   return;
                        
                        }
                        }
                        //Function to Calculate Basic And Converted Losses
                        //Formule:
                        //(Basic+ConvertedLosses)=Converted Losses + Basic Premium
                        //i.e BasicAndConvertedLosses=(NY Incurred Losses * LCF)+Basic Premium
                        function CalculateBasicAndConvertedLosses()
                        {
                            var myRegExp = /,|_/g;
                         varNYIncurresLosses= $get('<%=txtNYIncurredLosses.ClientID%>');
                         varLCF=$get('<%=txtLCF.ClientID%>');
                         var varConvertedLosses=0;

                         if ((varNYIncurresLosses.value.replace(myRegExp, '') != "") && (varLCF.value.replace(myRegExp, '') != ""))
                        {
                                var myRegExp1=/_/g;
                                varLCFModified=varLCF.value.replace(myRegExp1,'0');
                                varConvertedLosses = parseFloat(varNYIncurresLosses.value.replace(myRegExp, '')) * parseFloat(varLCFModified);
                        
                        }
                         varBasic=$get('<%=txtBasic.ClientID%>');
                         varBasicAndConvertedLosses=$get('<%=txtBasicAndConvertedLosses.ClientID%>');
                         var BasicAndConvertedLosses=0;
                             if(varConvertedLosses!=0)
                             {
                             BasicAndConvertedLosses+=parseFloat(varConvertedLosses);
                             }
                              if(varBasic.value!=0)
                             {
                                 BasicAndConvertedLosses += parseFloat(varBasic.value.replace(myRegExp, ''));
                             }
                           
                         varBasicAndConvertedLosses.value=addCommas(fix(BasicAndConvertedLosses));
                        }
                        //Function to Calculate ConvertedTaxesLosses
                        //Formule:
                        //ConvertedTaxesLosses=(Basic + Converted Losses) * Tax Multiplier
                        //i.e ConvertedTaxesLosses=(Basic+(NY Incurred Losses * LCF))*Tax Multiplier
                        function CalculateConvetedTaxesLosses()
                        {
                            var myRegExp = /,|_/g;
                         varNYIncurresLosses= $get('<%=txtNYIncurredLosses.ClientID%>');
                         varLCF=$get('<%=txtLCF.ClientID%>');
                         var varConvertedLosses=0;
                         if ((varNYIncurresLosses.value.replace(myRegExp, '') != "") && (varLCF.value.replace(myRegExp, '') != ""))
                        {
                                var myRegExp1=/_/g;
                                varLCFModified=varLCF.value.replace(myRegExp1,'0');
                                varConvertedLosses = parseFloat(varNYIncurresLosses.value.replace(myRegExp, '')) * parseFloat(varLCFModified);
                        
                        }
                         varBasic=$get('<%=txtBasic.ClientID%>');
                         varTaxMultiplier=$get('<%=txtTaxMultiplier.ClientID%>');
                         varConvertedTaxesLosses=$get('<%=txtConvertedTaxesLosses.ClientID%>');
                         var ConvertedTaxesLosses=0;
                         var BasicAndConvertedLosses=0;
                             if(varConvertedLosses!=0)
                             {
                             BasicAndConvertedLosses+=parseFloat(varConvertedLosses);
                             }
                              if(varBasic.value!=0)
                             {
                                 BasicAndConvertedLosses += parseFloat(varBasic.value.replace(myRegExp, ''));
                             }
                             if(varTaxMultiplier.value!="")
                             {
                                var myRegExp1=/_/g;
                                varTaxMultiplierModified=varTaxMultiplier.value.replace(myRegExp1,'0');
                                ConvertedTaxesLosses=BasicAndConvertedLosses*parseFloat(varTaxMultiplierModified);
                                varConvertedTaxesLosses.value=addCommas(fix(ConvertedTaxesLosses));
                                  
                             }
                             return ConvertedTaxesLosses;
                        
                        }
                        //Function to Calculate NYEarnedRetroPremium
                        //Formule:
                        //NYEarnedRetroPremium=(Converted Taxes Losses + NY Premium Discount)
                        // i.e NYEarnedRetroPremium=(((Basic+(NY Incurred Losses * LCF))*Tax Multiplier)+NY Premium Discount)
                        function CalculateNYEarnedRetroPremium()
                        {
                        var myRegExp = /,|_/g;
                        varConvertedTaxesLosses=CalculateConvetedTaxesLosses();
                        varNYPremiumDiscount=$get('<%=txtNYPremiumDiscount.ClientID%>');
                        varNYEarnedRetroPremium=$get('<%=txtNYEarnedRetroPremium.ClientID%>');
                        var NYEarnedRetroPremium=0;
                        if(varConvertedTaxesLosses!=0)
                        {
                        NYEarnedRetroPremium+=parseFloat(varConvertedTaxesLosses);
                        }
                        if(varNYPremiumDiscount.value!="")
                        {
                            NYEarnedRetroPremium += parseFloat(varNYPremiumDiscount.value.replace(myRegExp, ''));
                        }
                        varNYEarnedRetroPremium.value=addCommas(fix(NYEarnedRetroPremium));
                        return NYEarnedRetroPremium;
                        }
                        
                        //Function to Calculate RevisedNYSecondInjuryFund
                        //Formule:
                        //RevisedNYSecondInjuryFund=(NY Earned Retro Premium * NY-SIF Factor)
                        //i.e RevisedNYSecondInjuryFund=((((Basic+(NY Incurred Losses * LCF))*Tax Multiplier)+NY Premium Discount)*NY-SIF Factor)
                        function CalculateRevisedNYSecondInjuryFund()
                        {
                            var myRegExp = /,|_/g;
                         varNYEarnedRetroPremium=CalculateNYEarnedRetroPremium();
                         varNYSecondInjuryFundFactor=$get('<%=txtNYScndInjrFundFctr.ClientID%>');
                         varRevisedNYSecondInjuryFund=$get('<%=txtRevisedNYScndInjrFund.ClientID%>');
                         var RevisedNYSecondInjuryFund=0;
                         if (varNYSecondInjuryFundFactor.value.replace(myRegExp, '') != "")
                         {
                               var myRegExp1=/_/g;
                               varNYSecondInjuryFundFactorModified=varNYSecondInjuryFundFactor.value.replace(myRegExp1,'0');
                               RevisedNYSecondInjuryFund=parseFloat(varNYEarnedRetroPremium)*parseFloat(varNYSecondInjuryFundFactorModified);
                               varRevisedNYSecondInjuryFund.value=addCommas(fix(RevisedNYSecondInjuryFund));
                              
                               
                         }
                          return RevisedNYSecondInjuryFund;
                        }
                        
                        //Function to Calculate NYTaxDue
                        //Formule:
                        //NYTaxDue=(NY Second Injury on Audit - Revised NY SIF)
                        //i.e NYTaxDue=(NY Second Injury on Audit - ((((Basic+(NY Incurred Losses * LCF))*Tax Multiplier)+NY Premium Discount)*NY-SIF Factor))
                        function CalculateNYTaxDue()
                        {
                            var myRegExp = /,|_/g;
                         varRevisedNYSecondInjuryFund=CalculateRevisedNYSecondInjuryFund();
                         varNYSecondInjuryOnAudit=$get('<%=txtNYScndInjronAudit.ClientID%>');
                         varNYTaxDue=$get('<%=txtNYTaxDue.ClientID%>');
                         var NYTaxDue=0;
                         if (varNYSecondInjuryOnAudit.value.replace(myRegExp, '') != "")
                         {
                             NYTaxDue = parseFloat(varRevisedNYSecondInjuryFund) - parseFloat(varNYSecondInjuryOnAudit.value.replace(myRegExp, ''));
                         varNYTaxDue.value=addCommas(fix(NYTaxDue));
                        
                         }
                          return NYTaxDue;
                        }
                        
                        //Function to Calculate CurrentAdjustment
                        //Formule:
                        //CurrentAdjustment=(NY Tax Due - Previous Result)
                        //i.e CurrentAdjustment=((NY Second Injury on Audit - ((((Basic+(NY Incurred Losses * LCF))*Tax Multiplier)+NY Premium Discount)*NY-SIF Factor)) - Previous Result)
                        function CalculateCurrentAdjustment() 
                        {
                            var myRegExp = /,|_/g;
                        varNYTaxDue=CalculateNYTaxDue();
                        varPreviousResult=$get('<%=txtPreviousResult.ClientID%>');
                        varCurrentAdjustment=$get('<%=txtCurrentAdj.ClientID%>');
                        var CurrentAdjustment=0;
                        if (varPreviousResult.value.replace(myRegExp, '') != "")
                        {
                        CurrentAdjustment = parseFloat(varNYTaxDue) - parseFloat(varPreviousResult.value.replace(myRegExp, ''));
                        varCurrentAdjustment.value=addCommas(fix(CurrentAdjustment));
                        return;
                        }
                        
                        }
                            </script>

                            <!--End of JavaScript Section -->
                            <table>
                                <tr>
                                    <td style="height: 8px">
                                    </td>
                                </tr>
                            </table>
                            <asp:ObjectDataSource ID="odsAdjNumber" runat="server" SelectMethod="GetAdjNumberSearch"
                                TypeName="ZurichNA.AIS.Business.Logic.PremAdjustmentBS">
                                <SelectParameters>
                                    <asp:Parameter Name="straccountID" Type="String" />
                                    <asp:Parameter Name="strValDate" Type="String" />
                                    <asp:Parameter Name="intPremAdjPgmID" Type="Int32" />
                                </SelectParameters>
                            </asp:ObjectDataSource>
                            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                <ContentTemplate>
                                    <!--Start of Search Panel Section -->
                                    <asp:Panel ID="pnlSelectionHeader" BorderColor="Black" BorderWidth="1px" Width="60%"
                                        runat="server" class="panelExtContents">
                                        <table width="100%" border="0" align="center" cellpadding="2" cellspacing="1">
                                            <tr style="background-color: #608CC8; color: White">
                                                <td width="26%" height="20" align="center" valign="top">
                                                    <asp:Label ID="lblselectMessage" Font-Bold="true" Font-Size="Small" runat="server"
                                                        Text="Please make selection" Style="font-family: Verdana; font-size: 11px"></asp:Label>
                                                </td>
                                            </tr>
                                        </table>
                                    </asp:Panel>
                                    <asp:Panel ID="pnlSearch" runat="server" BorderColor="Black" BorderWidth="1px" Width="60%">
                                        <table width="100%" cellpadding="0" cellspacing="0">
                                            <tr>
                                                <td>
                                                    <ARS:AdjustmentReviewSearch ID="ARS" runat="server" />
                                                    <asp:HiddenField ID="hidSelectedValues" runat="server" />
                                                </td>
                                            </tr>
                                        </table>
                                    </asp:Panel>
                                    <!--End of Search Panel Section -->
                                    <br />
                                    <asp:Label ID="lblPremAdjNYSIFDetails" runat="server" CssClass="h3"></asp:Label>
                                    <!--Start of NY-SIF Fields Section -->
                                    <asp:Panel ID="pnlDetails" runat="server" Width="910px" Height="250px" BorderColor="Black"
                                        BorderWidth="1">
                                        <table id="tblNYSIF" runat="server" class="panelContents" border="0" width="100%"
                                            cellpadding="0">
                                            <tr class="ItemTemplate">
                                                <td style="vertical-align: middle; font-weight: bold; text-align: left" width="27%">
                                                    <asp:Label ID="lblhPolicyIncurred" runat="server" Text="Policy Incurred" />
                                                </td>
                                                <td width="20%">
                                                    <%-- <asp:ListBox ID="lbPolicy" runat="server" Width="100%" Font-Names="Tahoma" Font-Size="8.5"
                                                        SkinID="lstAdjReview" Enabled="false"></asp:ListBox>--%>
                                                    <asp:Panel ID="pnlPolicyNumberListNYSIF" runat="server" ScrollBars="Auto" CssClass="content"
                                                        Height="30px">
                                                        <asp:CheckBoxList ID="ckkboxlstPolicyno" runat="server" ForeColor="Black" ReadOnly="true"
                                                             SkinID="Required" BackColor="White" BorderColor="Black">  
                                                        </asp:CheckBoxList>
                                                    </asp:Panel>
                                                    
                                                </td>
                                                <td style="width: 4px; background-color: White; vertical-align: middle">
                                                <asp:CustomValidator ID="CustValidatorreqPolicy" runat="server" ErrorMessage="Please select one policy"
                                                                        ClientValidationFunction="CheckPolicy" ValidationGroup="NYSIFSaveGroup" Display="Dynamic"
                                                                        Text="*" />
                                                </td>
                                                <td style="width: 6%; background-color: White">
                                                </td>
                                                <td style="vertical-align: middle; font-weight: bold; text-align: left" width="35%">
                                                    &nbsp;<asp:Label ID="lblhNYPremDisc" runat="server" Text="NY Premium Discount" />
                                                </td>
                                                <td width="12%" style="vertical-align: middle">
                                                    <asp:AISAmountTextbox ID="txtNYPremiumDiscount" runat="server"  Width="100%"
                                                        TabIndex="7" AllowNegetive="true" onblur="CalculateNYEarnedRetroPremium();CalculateNYTaxDue();CalculateCurrentAdjustment();"
                                                        ></asp:AISAmountTextbox>
                                                        <%--<cc1:MaskedEditExtender ID="MaskedtxtNYPremiumDiscount" runat="server" TargetControlID="txtNYPremiumDiscount"
                                                                Mask="99,999,999,999.99" MessageValidatorTip="true" MaskType="Number" DisplayMoney="None"
                                                                OnFocusCssClass="MaskedEditFocus" OnInvalidCssClass="MaskedEditError" InputDirection="RightToLeft"
                                                                AcceptNegative="Left" AutoComplete="false" />--%>
                                                    <%--<cc1:FilteredTextBoxExtender ID="ftbeNYPremiumDiscount" runat="server" TargetControlID="txtNYPremiumDiscount"
                                                        FilterType="Custom" FilterMode="ValidChars" ValidChars="-1234567890," />--%>
                                                </td>
                                                <td style="width: 4px; background-color: White; vertical-align: middle">
                                                    <asp:RequiredFieldValidator ID="reqNYPremiumDiscount" runat="server" ControlToValidate="txtNYPremiumDiscount"
                                                        Text="*" ValidationGroup="NYSIFSaveGroup" ErrorMessage="Please Enter NYPremiumDiscount" />
                                                </td>
                                            </tr>
                                            <tr class="AlternatingItemTemplate">
                                                <td style="vertical-align: middle; font-weight: bold; text-align: left" width="27%">
                                                    <asp:Label ID="lblhNYIncurredLosses" runat="server" Text="NY Incurred Losses" />
                                                </td>
                                                <td width="20%">
                                                    <asp:AISAmountTextbox ID="txtNYIncurredLosses" Width="100%" runat="server" 
                                                        TabIndex="3" onblur="CalculateConvertedLosses();CalculateBasicAndConvertedLosses();CalculateConvetedTaxesLosses();CalculateNYEarnedRetroPremium();CalculateRevisedNYSecondInjuryFund();CalculateNYTaxDue();CalculateCurrentAdjustment();"
                                                        AllowNegetive="true"></asp:AISAmountTextbox>
                                                         <%--<cc1:MaskedEditExtender ID="MaskedtxtNYIncurredLosses" runat="server" TargetControlID="txtNYIncurredLosses"
                                                                Mask="99,999,999,999.99" MessageValidatorTip="true" MaskType="Number" DisplayMoney="None"
                                                                OnFocusCssClass="MaskedEditFocus" OnInvalidCssClass="MaskedEditError" InputDirection="RightToLeft"
                                                                AcceptNegative="Left" AutoComplete="false" />--%>
                                                  <%--<cc1:FilteredTextBoxExtender ID="ftbeNYIncurredLosses" runat="server" TargetControlID="txtNYIncurredLosses"
                                                        FilterType="Custom" FilterMode="ValidChars" ValidChars="-1.234567890," />--%>
                                                </td>
                                                <td style="width: 4px; background-color: White; vertical-align: middle">
                                                    <asp:RequiredFieldValidator ID="reqNYIncurredLosses" runat="server" ControlToValidate="txtNYIncurredLosses"
                                                        Text="*" ValidationGroup="NYSIFSaveGroup" ErrorMessage="Please Enter NY Incurred Losses" />
                                                </td>
                                                <td style="width: 6%; background-color: White">
                                                </td>
                                                <td style="vertical-align: middle; font-weight: bold; text-align: left" width="35%">
                                                    &nbsp;<asp:Label ID="lblhNYERetroPrem" runat="server" Text="NY Earned Retro Premium" />
                                                </td>
                                                <td align="left" width="12%">
                                                    <asp:TextBox ID="txtNYEarnedRetroPremium" runat="server" contentEditable="false"
                                                        Width="100%" BorderStyle="None" Font-Bold="true" BorderWidth="0" BackColor="Transparent"></asp:TextBox>
                                                </td>
                                                <td style="width: 4px; background-color: White; vertical-align: middle">
                                                </td>
                                            </tr>
                                            <tr class="ItemTemplate">
                                                <td style="vertical-align: middle; font-weight: bold; text-align: center" width="27%">
                                                    <asp:Label ID="lblhLCF" runat="server" Text="X LCF" />
                                                </td>
                                                <td width="20%" style="vertical-align: middle">
                                                    <asp:TextBox ID="txtLCF" runat="server" Width="100%" MaxLength="20" TabIndex="4"
                                                        onblur="CalculateConvertedLosses();CalculateBasicAndConvertedLosses();CalculateConvetedTaxesLosses();CalculateNYEarnedRetroPremium();CalculateRevisedNYSecondInjuryFund();CalculateNYTaxDue();CalculateCurrentAdjustment();"
                                                        ></asp:TextBox>
                                                    <cc1:MaskedEditExtender ID="MaskedEditLCF" runat="server" TargetControlID="txtLCF"
                                                        Mask="9.9{6}" MessageValidatorTip="true" MaskType="Number" DisplayMoney="None"
                                                        OnFocusCssClass="MaskedEditFocus" OnInvalidCssClass="MaskedEditError" AcceptNegative="None" />
                                                </td>
                                                <td style="width: 4px; background-color: White; vertical-align: middle">
                                                    <%--<asp:RequiredFieldValidator ID="reqLCF" runat="server" ControlToValidate="txtLCF"
                                                        Text="*" ValidationGroup="NYSIFSaveGroup" ErrorMessage="Please Enter LCF" />--%>
                                                </td>
                                                <td style="width: 6%; background-color: White">
                                                </td>
                                                <td style="vertical-align: middle; font-weight: bold; text-align: center" width="35%">
                                                    &nbsp;
                                                    <asp:Label ID="lblhNYScndIFF" runat="server" Text="X NY Second Injury Fund Factor" />
                                                </td>
                                                <td width="12%" style="vertical-align: middle">
                                                    <asp:TextBox ID="txtNYScndInjrFundFctr" runat="server" TabIndex="8" onblur="CalculateRevisedNYSecondInjuryFund();CalculateNYTaxDue();CalculateCurrentAdjustment();"
                                                        onkeypress="return AmountValidation(event,this)"></asp:TextBox>
                                                    <cc1:MaskedEditExtender ID="MaskedEditNYScndInjrFundFctr" runat="server" TargetControlID="txtNYScndInjrFundFctr"
                                                        Mask="99.9{6}" MessageValidatorTip="true" MaskType="Number" DisplayMoney="None"
                                                        OnFocusCssClass="MaskedEditFocus" OnInvalidCssClass="MaskedEditError" AcceptNegative="None" />
                                                </td>
                                                <td style="width: 4px; background-color: White; vertical-align: middle">
                                                    <%--<asp:RequiredFieldValidator ID="reqNYScndInjrFundFctr" runat="server" ControlToValidate="txtNYScndInjrFundFctr"
                                                        Text="*" ValidationGroup="NYSIFSaveGroup" ErrorMessage="Please Enter NYScndInjrFundFctr" />--%>
                                                </td>
                                            </tr>
                                            <tr class="AlternatingItemTemplate">
                                                <td style="vertical-align: middle; font-weight: bold; text-align: left" width="35%">
                                                    <asp:Label ID="lblhCovertedLosses" runat="server" Text="Converted Losses" />
                                                </td>
                                                <td align="left" width="12%">
                                                    <asp:TextBox ID="txtConvertedLosses" runat="server" contentEditable="false" Font-Bold="true"
                                                        BorderStyle="None" BorderWidth="0" BackColor="Transparent" Width="100%"></asp:TextBox>
                                                </td>
                                                <td style="width: 4px; background-color: White; vertical-align: middle">
                                                </td>
                                                <td style="width: 6%; background-color: White">
                                                </td>
                                                <td style="vertical-align: middle; font-weight: bold; text-align: left" width="35%">
                                                    &nbsp;
                                                    <asp:Label ID="lblhRevisedNYScndIF" runat="server" Text="Revised NY Second Injury Fund" />
                                                </td>
                                                <td align="left" width="12%" style="vertical-align: middle">
                                                    <asp:TextBox ID="txtRevisedNYScndInjrFund" runat="server" contentEditable="false"
                                                        Font-Bold="true" BorderStyle="None" BorderWidth="0" BackColor="Transparent" Width="100%"></asp:TextBox>
                                                </td>
                                                <td style="width: 4px; background-color: White; vertical-align: middle">
                                                </td>
                                            </tr>
                                            <tr class="ItemTemplate">
                                                <td style="vertical-align: middle; font-weight: bold; text-align: left" width="35%">
                                                    <asp:Label ID="lblhBasic" runat="server" Text="Basic(Included in Deductible Premium)" />
                                                </td>
                                                <td width="12%">
                                                    <asp:AISAmountTextbox ID="txtBasic" runat="server" Width="100%"  TabIndex="5"
                                                        onblur="CalculateBasicAndConvertedLosses();CalculateConvetedTaxesLosses();CalculateNYEarnedRetroPremium();CalculateRevisedNYSecondInjuryFund();"
                                                        AllowNegetive="true"></asp:AISAmountTextbox>
                                                         <%--<cc1:MaskedEditExtender ID="MaskedtxtBasic" runat="server" TargetControlID="txtBasic"
                                                                Mask="99,999,999,999.99" MessageValidatorTip="true" MaskType="Number" DisplayMoney="None"
                                                                OnFocusCssClass="MaskedEditFocus" OnInvalidCssClass="MaskedEditError" InputDirection="RightToLeft"
                                                                AcceptNegative="Left" AutoComplete="false" />--%>
                                                   <%--<cc1:FilteredTextBoxExtender ID="ftbeBasic" runat="server" TargetControlID="txtBasic"
                                                        FilterType="Custom" FilterMode="ValidChars" ValidChars="-1.234567890," />--%>
                                                </td>
                                                <td style="width: 4px; background-color: White; vertical-align: middle">
                                                    <asp:RequiredFieldValidator ID="reqBasic" runat="server" ControlToValidate="txtBasic"
                                                        Text="*" ValidationGroup="NYSIFSaveGroup" ErrorMessage="Please Enter Basic" />
                                                </td>
                                                <td style="width: 6%; background-color: White">
                                                </td>
                                                <td style="vertical-align: middle; font-weight: bold; text-align: left" width="35%">
                                                    &nbsp;
                                                    <asp:Label ID="lblhNYScndIAudit" runat="server" Text="NY Second Injury on Audit" />
                                                </td>
                                                <td width="12%" style="vertical-align: middle">
                                                    <asp:AISAmountTextbox ID="txtNYScndInjronAudit" runat="server"  Width="100%"
                                                        TabIndex="9" onblur="CalculateNYTaxDue();CalculateNYTaxDue();CalculateCurrentAdjustment()"
                                                        AllowNegetive="true"></asp:AISAmountTextbox>
                                                         <%--<cc1:MaskedEditExtender ID="MaskedtxtNYScndInjronAudit" runat="server" TargetControlID="txtNYScndInjronAudit"
                                                                Mask="99,999,999,999.99" MessageValidatorTip="true" MaskType="Number" DisplayMoney="None"
                                                                OnFocusCssClass="MaskedEditFocus" OnInvalidCssClass="MaskedEditError" InputDirection="RightToLeft"
                                                                AcceptNegative="Left" AutoComplete="false" />--%>
                                                   <%-- <cc1:FilteredTextBoxExtender ID="ftbeNYScndInjronAudit" runat="server" TargetControlID="txtNYScndInjronAudit"
                                                        FilterType="Custom" FilterMode="ValidChars" ValidChars="-1.234567890," />--%>
                                                </td>
                                                <td style="width: 4px; background-color: White; vertical-align: middle">
                                                    <asp:RequiredFieldValidator ID="reqNYScndInjronAudit" runat="server" ControlToValidate="txtNYScndInjronAudit"
                                                        Text="*" ValidationGroup="NYSIFSaveGroup" ErrorMessage="Please Enter NYScndInjronAudit" />
                                                </td>
                                            </tr>
                                            <tr class="AlternatingItemTemplate">
                                                <td style="vertical-align: middle; font-weight: bold; text-align: left" width="35%">
                                                    <asp:Label ID="lblhBasicCL" runat="server" Text="Basic + Converted Losses" />
                                                </td>
                                                <td align="left" width="12%">
                                                    <asp:TextBox ID="txtBasicAndConvertedLosses" runat="server" contentEditable="false"
                                                        Font-Bold="true" BorderStyle="None" BorderWidth="0" BackColor="Transparent" Width="100%"></asp:TextBox>
                                                </td>
                                                <td style="width: 4px; background-color: White; vertical-align: middle">
                                                </td>
                                                <td style="width: 6%; background-color: White">
                                                </td>
                                                <td style="vertical-align: middle; font-weight: bold; text-align: left" width="35%">
                                                    &nbsp;
                                                    <asp:Label ID="lblhNYTaxDue" runat="server" Text="NY Tax Due(Refund)" />
                                                </td>
                                                <td align="left" width="12%" style="vertical-align: middle">
                                                    <asp:TextBox ID="txtNYTaxDue" runat="server" contentEditable="false" Font-Bold="true"
                                                        BorderStyle="None" BorderWidth="0" BackColor="Transparent" Width="100%"></asp:TextBox>
                                                </td>
                                                <td style="width: 4px; background-color: White; vertical-align: middle">
                                                </td>
                                            </tr>
                                            <tr class="ItemTemplate">
                                                <td style="vertical-align: middle; font-weight: bold; text-align: center" width="35%">
                                                    <asp:Label ID="lblhTaxMlut" runat="server" Text="X Tax Multiplier" />
                                                </td>
                                                <td align="left" width="12%">
                                                    <asp:TextBox ID="txtTaxMultiplier" runat="server" Width="100%" TabIndex="6" onblur="CalculateConvetedTaxesLosses();CalculateNYEarnedRetroPremium();"
                                                        onkeypress="return AmountValidation(event,this)"></asp:TextBox>
                                                    <cc1:MaskedEditExtender ID="MaskedEditTaxMultiplier" runat="server" TargetControlID="txtTaxMultiplier"
                                                        Mask="9.9{6}" MessageValidatorTip="true" MaskType="Number" DisplayMoney="None"
                                                        OnFocusCssClass="MaskedEditFocus" OnInvalidCssClass="MaskedEditError" AcceptNegative="None" />
                                                </td>
                                                <td style="width: 4px; background-color: White; vertical-align: middle">
                                                    <%--<asp:RequiredFieldValidator ID="reqTaxMultiplier" runat="server" ControlToValidate="txtTaxMultiplier"
                                                        Text="*" ValidationGroup="NYSIFSaveGroup" ErrorMessage="Please Enter TaxMultiplier" />--%>
                                                </td>
                                                <td style="width: 6%; background-color: White">
                                                </td>
                                                <td style="vertical-align: middle; font-weight: bold; text-align: left" width="35%">
                                                    &nbsp;
                                                    <asp:Label ID="lblhPreviousResult" runat="server" Text="Previous Result" />
                                                </td>
                                                <td width="12%" style="vertical-align: middle">
                                                    <asp:AISAmountTextbox ID="txtPreviousResult" runat="server" Width="100%" TabIndex="10"
                                                        onblur="CalculateCurrentAdjustment();CalculateNYTaxDue();CalculateCurrentAdjustment()"
                                                        AllowNegetive="true"></asp:AISAmountTextbox>
                                                    <%--<cc1:MaskedEditExtender ID="MaskedtxtPreviousResult" runat="server" TargetControlID="txtPreviousResult"
                                                                Mask="99,999,999,999.99" MessageValidatorTip="true" MaskType="Number" DisplayMoney="None"
                                                                OnFocusCssClass="MaskedEditFocus" OnInvalidCssClass="MaskedEditError" InputDirection="RightToLeft"
                                                                AcceptNegative="Left" AutoComplete="false" />--%>
                                                  <%--<cc1:FilteredTextBoxExtender ID="FilteredtxtPreviousResult" runat="server" TargetControlID="txtPreviousResult"
                                                        FilterType="Custom" FilterMode="ValidChars" ValidChars="-1.234567890," />--%>
                                                </td>
                                                <td style="width: 4px; background-color: White; vertical-align: middle">
                                                    <asp:RequiredFieldValidator ID="reqPreviousResult" runat="server" ControlToValidate="txtPreviousResult"
                                                        Text="*" ValidationGroup="NYSIFSaveGroup" ErrorMessage="Please Enter PreviousResult" />
                                                </td>
                                            </tr>
                                            <tr class="AlternatingItemTemplate">
                                                <td style="vertical-align: middle; font-weight: bold; text-align: left" width="35%">
                                                    <asp:Label ID="lblhCTaxLosses" runat="server" Text="Converted Taxes Losses" />
                                                </td>
                                                <td align="left" width="12%">
                                                    <asp:TextBox ID="txtConvertedTaxesLosses" runat="server" contentEditable="false"
                                                        Font-Bold="true" BorderStyle="None" BorderWidth="0" BackColor="Transparent" Width="100%"></asp:TextBox>
                                                </td>
                                                <td style="width: 4px; background-color: White; vertical-align: middle">
                                                </td>
                                                <td style="width: 6%; background-color: White">
                                                </td>
                                                <td style="vertical-align: middle; font-weight: bold; text-align: left" width="35%">
                                                    &nbsp;
                                                    <asp:Label ID="lblhCurrentAdj" runat="server" Text="Current Adjustment" />
                                                </td>
                                                <td align="left" width="12%" style="vertical-align: middle">
                                                    <asp:TextBox ID="txtCurrentAdj" runat="server" contentEditable="false" Font-Bold="true"
                                                        BorderStyle="None" BorderWidth="0" BackColor="Transparent" Width="100%"></asp:TextBox>
                                                </td>
                                                <td style="width: 4px; background-color: White; vertical-align: middle">
                                                </td>
                                            </tr>
                                        </table>
                                        <table width="100%">
                                            <tr>
                                                <td align="center">
                                                    <asp:Button ID="btnNYSIFSave" runat="server" Text="Save" OnClientClick="CalculateCurrentAdjustment();CalculateNYTaxDue();CalculateCurrentAdjustment();"
                                                        OnClick="btnNYSIFSave_Click" ValidationGroup="NYSIFSaveGroup" Width="60px" ToolTip="Click here to Save" />
                                                    <asp:Button ID="btnNYSIFClear" runat="server" Text="Clear" OnClick="btnNYSIFClear_Click"
                                                        Width="60px" ToolTip="Click here to clear all Contents" />
                                                </td>
                                            </tr>
                                        </table>
                                    </asp:Panel>
                                    <!--End of NY-SIF Fields Section -->
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </ContentTemplate>
                    </cc1:TabPanel>
                    <!--End of NY-SIF Tab Section-->
                    <cc1:TabPanel runat="server" ID="tblpnlmscInvL">
                        <HeaderTemplate>
                            <div onclick="Tabnavigation('ARMI')">
                                Misc.Invoicing
                            </div>
                        </HeaderTemplate>
                        <ContentTemplate>
                        </ContentTemplate>
                    </cc1:TabPanel>
                    <cc1:TabPanel runat="server" ID="tblpnlLRF">
                        <HeaderTemplate>
                            <div onclick="Tabnavigation('ARLRFP')">
                                LRF Posting
                            </div>
                        </HeaderTemplate>
                        <ContentTemplate>
                        </ContentTemplate>
                    </cc1:TabPanel>
                    <cc1:TabPanel runat="server" ID="tblpnlAdjchklist">
                        <HeaderTemplate>
                            <div onclick="Tabnavigation('ARAPCL')">
                                Adj.Checklist
                            </div>
                        </HeaderTemplate>
                        <ContentTemplate>
                        </ContentTemplate>
                    </cc1:TabPanel>
                    <cc1:TabPanel runat="server" ID="tblpnlAdjNumberText">
                        <HeaderTemplate>
                            <div onclick="Tabnavigation('ARANTM')">
                                Adj.Number
                            </div>
                        </HeaderTemplate>
                        <ContentTemplate>
                        </ContentTemplate>
                    </cc1:TabPanel>
                    <cc1:TabPanel runat="server" ID="tblpnlBuBroker">
                        <HeaderTemplate>
                            <div onclick="Tabnavigation('ARBB')">
                                BU Broker
                            </div>
                        </HeaderTemplate>
                        <ContentTemplate>
                        </ContentTemplate>
                    </cc1:TabPanel>
                </cc1:TabContainer>
            </td>
        </tr>
    </table>
</asp:Content>
