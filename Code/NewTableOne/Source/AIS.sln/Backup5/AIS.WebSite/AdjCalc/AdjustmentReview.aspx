<%@ Page Language="C#" MasterPageFile="~/Retro.master" AutoEventWireup="true" Inherits="Adjustment_and_Invoice_AdjustmentReview"
    Title="Adjustment Review Comments" CodeBehind="AdjustmentReview.aspx.cs" EnableViewStateMac="true" Buffer="true" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/App_Shared/AdjustmentReviewSearch.ascx" TagPrefix="ARS" TagName="AdjustmentReviewSearch" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeaderPlaceHolder" runat="Server">
    <asp:Label ID="lblAdjReview" runat="server" Text="Adjustment Review" CssClass="h1"></asp:Label>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainPlaceHolder" runat="Server">

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
                strURL +="SurchargeAssesmentReview.aspx";
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
                strURL += "?SelectedValues=" + selectedValues.value + "&wID=<%= WindowName%>";
            }
            else {
                strURL += "?wID=<%= WindowName%>";
            }
            window.location.href=strURL;
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

    <table>
        <tr>
            <td>
                <cc1:TabContainer ID="TabContainer1" runat="server" CssClass="VariableTabs" SkinID="tabVariable"
                    ActiveTabIndex="0">
                    <cc1:TabPanel runat="server" ID="tblpnlLBA">
                        <HeaderTemplate>
                                  Review                                         
                        </HeaderTemplate>
                        <ContentTemplate>
                            <br />
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
                                        <ARS:AdjustmentReviewSearch ID="ARS" runat="server" />
                                        <asp:HiddenField ID="hidSelectedValues" runat="server" />
                                        <table width="532px">
                                            <tr>
                                                <td align="left" valign="top" width="180px">
                                                   
                                                    <asp:Label ID="lblDocName" runat="server" Text="Document Name" Width="144px" />
                                                    <asp:ObjectDataSource ID="objDataSourceDocName" runat="server" SelectMethod="GetLookUpActiveData"
                                                        TypeName="ZurichNA.AIS.Business.Logic.BLAccess">
                                                        <SelectParameters>
                                                            <asp:Parameter DefaultValue="ADJUSTMENT DOCUMENT" Name="lookUpTypeName" Type="String" />
                                                        </SelectParameters>
                                                    </asp:ObjectDataSource>
                                                    </td>
                                                    <td align="left" width="265px">
                                                    <asp:DropDownList ID="ddlDocName" runat="server" AutoPostBack="true" Width="254px"
                                                        DataSourceID="objDataSourceDocName" DataTextField="LookUpName" DataValueField="LookUpID"
                                                        OnSelectedIndexChanged="ddlDocName_SelectedIndexChanged">
                                                    </asp:DropDownList>
                                                   </td>
                                                   <td align="right" style="padding-right:45px"> 
                                                    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                                        <ContentTemplate>
                                                           <asp:Button ID="btnPreview" runat="server" CommandName="Preview" Text="Preview" OnClick="btnPreview_Click" UseSubmitBehavior="false" /> 
                                                        </ContentTemplate>
                                                        <Triggers>
                                                            <asp:PostBackTrigger ControlID="btnPreview" />
                                                        </Triggers>
                                                    </asp:UpdatePanel>
                                                </td>
                                            </tr>
                                        </table>
                                    </asp:Panel>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                            <asp:Panel ID="pnlDetails" runat="server">
                                <table>
                                    <tr>
                                        <td>
                                            <caption>
                                                <br />
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblComment" runat="server" Text="EXTERNAL COMMENTS"></asp:Label>
                                                        <br />
                                                        <asp:TextBox ID="txtComment" runat="server" Height="100px" MaxLength="4000" TextMode="MultiLine"
                                                            ValidationGroup="Save" Width="800px" Wrap="False" onpaste="doPaste(this,4000);"
                                                            onkeypress="doKeypress(this,4000);" onbeforepaste="doBeforePaste(this,4000);"></asp:TextBox>
                                                        <asp:RequiredFieldValidator ID="reqComment" runat="server" ControlToValidate="txtComment"
                                                            ErrorMessage="Please Enter Comments" ValidationGroup="Save"></asp:RequiredFieldValidator>
                                                        <cc1:FilteredTextBoxExtender ID="fltrAdjExps" runat="server" TargetControlID="txtComment"
                                                            FilterType="Custom" FilterMode="InvalidChars" InvalidChars="'" />
                                                    </td>
                                                </tr>
                                            </caption>
                                        </td>
                                    </tr>
                                </table>
                                <br />
                                <table width="90%">
                                    <tr>
                                        <td align="right">
                                            <asp:Button ID="BtnSave" runat="server" CommandName="SAVE" Text="Save" OnClick="BtnSave_Click" />
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
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
                                Loss Fund Adj.
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
                    <cc1:TabPanel runat="server" ID="tblpnlNYSIF">
                        <HeaderTemplate>
                            <div onclick="Tabnavigation('ARNYSIF')">
                               Surch & Assmt
                            </div>
                        </HeaderTemplate>
                        <ContentTemplate>
                        </ContentTemplate>
                    </cc1:TabPanel>
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
