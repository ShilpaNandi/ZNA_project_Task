<%@ Page Language="C#" MasterPageFile="~/Retro.master" AutoEventWireup="true" CodeBehind="ExcessNonBillable.aspx.cs"
    Inherits="ExcessNonBillable" Title="Untitled Page" %>

<%@ Register Src="../App_Shared/AccountInfoHeader.ascx" TagName="AccountInfoHeader"
    TagPrefix="uc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeaderPlaceHolder" runat="server">
    <table width="920px">
        <tr>
            <td align="left">
                <asp:Label ID="GAILabel" runat="server" Text="Excess/Non Billable" CssClass="h1"></asp:Label>
            </td>
            <td align="right">
                <asp:Button ID="btnAdd" runat="server" Text="Add New" OnClick="btnAdd_Click" />
            
                <asp:Button ID="btnBack" runat="server" Text="Back"  OnClick="btnBack_Click"
                    ToolTip="Please click here to go previous page." />
            </td>
       
    </table>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainPlaceHolder" runat="server">
   <script type="text/javascript" src="../JavaScript/RetroScript.js"></script> 
   <script language="javascript" type="text/javascript">
    
    var scrollTop1;
        
    if(Sys.WebForms.PageRequestManager != null)
    {
        Sys.WebForms.PageRequestManager.getInstance().add_beginRequest(BeginRequestHandler);
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);
    }

    function BeginRequestHandler(sender, args) 
    {
        var mef = $get('<%=Panel1.ClientID%>');
        if(mef!=null)
        scrollTop1 = mef.scrollTop;
    }

    function EndRequestHandler(sender, args)
    {
        var mef = $get('<%=Panel1.ClientID%>');
        if(mef!=null)
        mef.scrollTop = scrollTop1;
    }    
    
    function ClearAdditionalClaims()
    {
        var flag=document.getElementById('<%=chkAddClaims.ClientID%>').checked;
        if (flag==true)    
        {            
            document.getElementById('<%=txtAdditionalClaims.ClientID%>').disabled=false;            
        }
        else  
        {              
            document.getElementById('<%=txtAdditionalClaims.ClientID%>').value=''; 
            document.getElementById('<%=txtAdditionalClaims.ClientID%>').disabled=true;                                
        }
    }
    
    //Written by Suneel 20 Feb 2009
    /*function CheckNumerics()
    {
        var strString = document.getElementById('<%=txtAdditionalClaims.ClientID%>').value
        var strValidChars = "0123456789,";
        var strChar;
        var blnResult = true;
        
        if (strString.length == 0) return;

        //  test strString consists of valid characters listed above
        for (i = 0; i < strString.length && blnResult == true; i++)
        {
            strChar = strString.charAt(i);
            if (strValidChars.indexOf(strChar) == -1)
            {
                alert('Please enter valid text');
                document.getElementById('<%=txtAdditionalClaims.ClientID%>').value = '';
                blnResult = false;
            }
        }
        
    }*/
    
    
    </script>
    
     <asp:UpdatePanel runat="server" ID="UpdatePanel1">
        <ContentTemplate>
         <asp:ValidationSummary ID="ValidationSummary1" ValidationGroup="Save" runat="server" BorderColor="Red" BorderWidth="1" BorderStyle="Solid" >
            </asp:ValidationSummary>
            
    <table>
        <tr>
            <td>
                <uc1:AccountInfoHeader ID="AccountInfoHeader1" runat="server" />
                <br />
            </td>
        </tr>
    </table>
    <table cellpadding="0" cellspacing="0">
        <tr>
            <td>
                <asp:Panel ID="pnlInfo" runat="server" BorderColor="Black" BorderWidth="1px" Width="244px">
                    <table>
                        <tr>
                            <td>
                                <asp:Label ID="lblLOB" runat="server" Text="LOB: " Font-Bold="true"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblLOBText" runat="server" Text=""></asp:Label>
                            </td>
                            <td>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblPolicyNumber" runat="server" Text="Policy Number: " Font-Bold="true"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblPolicyNumberText" runat="server" Text=""></asp:Label>
                            </td>
                            <td>
                              
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblState" runat="server" Text="State: " Font-Bold="true"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblStateText" runat="server" Text=""></asp:Label>
                            </td>
                            <td>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
            </td>
            
            <td>
            
                
            </td>
            
            <td>
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
              <asp:CheckBox ID="chkHideDisLines" TabIndex="2" runat="server" Text="Hide Disabled Lines"
                                    OnCheckedChanged="chkHideDisLines_CheckedChanged" AutoPostBack="true" />
            </td>
        </tr>
    </table>
     <table >
        <tr>
            <td>
    <asp:Panel ID="pnlDate" runat="server">
                    <table>
                        <tr>
                            <td>
                               
                                <asp:Label ID="lblStartDate" runat="server" Text="Program Period "
                                    Font-Bold="true" ></asp:Label>
                            </td>
                            <td>
                                 <asp:Label ID="lblStartDateText" runat="server" Text="" ></asp:Label>
                                                                 
                            </td>
                        </tr>
                        <tr>
                            <td>
                                
                                <asp:Label ID="lblEndDate" Visible="false"  runat="server" Text="Policy Expiration Date: " Font-Bold="true"></asp:Label>
                            </td>
                            <td>
                               <asp:Label ID="lblEndDateText" runat="server" Visible="false" Text=""></asp:Label>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
                </td>
                </tr>
                </table>
              
    <%--  <table width="900px">
        <tr>
         <td align="right">
         <asp:Button ID="btnAdd" runat="server" Text="Add New" OnClick="btnAdd_Click" />
         </td>
         </tr>
        </table>--%>
        <br />
    <asp:UpdatePanel runat="server" ID="updInternal">
        <ContentTemplate>
           
            <asp:Panel ID="Panel1" Width="910px" runat="server" ScrollBars="Auto" Height="70px">
                <asp:AISListView ID="lsvExcessNonBillable" runat="server" DataKeyNames="ARMIS_LOS_EXC_ID"  onsorting="lsvExcessNonBillable_Sorting"
                    OnItemDataBound="DataBoundList" OnItemCommand="CommandList" OnSelectedIndexChanging="lsvExcessNonBillable__SelectedIndexChanging">
                   <EmptyDataTemplate>
                        
             <table id="tblLayout" class="panelContents" runat="server" width="100%">
                            <tr class="LayoutTemplate">
                                <th>
                                </th>
                                <th>
                                    Claim Numbers
                                </th>
                                <th>
                                    Claimant Name
                                </th>
                                <th>
                                    Coverage Trigger Dt
                                        
                                </th>
                                <th>
                                    Claim Status
                                </th>
                                <th>
                                    Policy Limit
                                </th>
                                <th>
                                   ALAE
                                        
                                </th>
                                <th>
                                    ALAE Cap
                                </th>
                                <th>
                                    Sys. Gen
                                </th>
                                <th>
                                Copy Losses
                                </th>
                                <th>
                                    Disable
                                </th>
                                <th>
                                    DETAILS
                                </th>
                            </tr>
                            
                        </table>
                         <table width="910px">
                                           <tr id="Tr1" runat="server" class="ItemTemplate">
                        <td align="center">
                            <asp:Label ID="lblEmptyMessage" Text="---Excess/Non Billable Not Done---" Font-Bold="true" 
                                runat="server" Style="text-align: center" />
                        </td>
                    </tr>
                                            </table>
            
                    </EmptyDataTemplate>
                    <LayoutTemplate>
                        <table id="tblLayout" class="panelContents" runat="server" width="98%">
                            <tr class="LayoutTemplate">
                                <th>
                                </th>
                                <th>
                                    Claim Numbers
                                </th>
                                <th>
                                    Claimant Name
                                </th>
                                <th>
                                    <asp:LinkButton ID="lbCovgDt" CommandName="Sort" CommandArgument="COVG_TRIGGER_DATE"
                                        runat="server">Coverage Trigger Dt</asp:LinkButton>
                                        <asp:Image ID="imgSortByCovgDt" runat="server" ImageUrl="~/images/descending.gif" 
                                                                    ToolTip="Descending" Visible="false" />
                                </th>
                                <th>
                                    Claim Status
                                </th>
                                <th>
                                    Policy Limit
                                </th>
                                <th>
                                   <asp:LinkButton ID="lbALAE" CommandName="Sort" CommandArgument="ALAE_TYP"
                                        runat="server">ALAE</asp:LinkButton>
                                         <asp:Image ID="imgSortByALAE" runat="server" ImageUrl="~/images/Ascending.gif" 
                                                                    ToolTip="Ascending" Visible="false" />
                                </th>
                                <th>
                                    ALAE Cap
                                </th>
                                <th>
                                    System Generated
                                </th>
                                <th>
                                Copy Losses
                                </th>
                                <th>
                                    Disable
                                </th>
                                <th>
                                    DETAILS
                                </th>
                            </tr>
                            <tr id="itemPlaceholder" runat="server">
                            </tr>
                        </table>
                    </LayoutTemplate>
                    <ItemTemplate>
                        <tr class="ItemTemplate" runat="server" id="trItemTemplate">
                            <td>
                            <asp:HiddenField ID="hidStatus" runat="server" Value='<%# Bind("CLAIMSTATUS") %>' />
                                <asp:HiddenField ID="HidArmisID" runat="server" Value='<%# Bind("ARMIS_LOS_EXC_ID") %>' />
                                <asp:LinkButton ID="lnkSelect" CommandArgument='<%# Bind("ARMIS_LOS_EXC_ID") %>'
                                    CommandName="Select" runat="server" Text="Select" Enabled='<%# Eval("ACTV_IND")%>'></asp:LinkButton>
                            </td>
                            <td>
                                <%# Eval("CLAIM_NBR_TXT")%>
                            </td>
                            <td>
                                <%# Eval("CLAIMANT_NM")%>
                            </td>
                            <td>
                                <%# Eval("COVG_TRIGGER_DATE", "{0:d}")%>
                            </td>
                            <td>
                                <%# Eval("CLAIMSTATUS")%>
                            </td>
                            <td>
                          <%# Eval("POLICY_AMT") != null ? (Eval("POLICY_AMT").ToString() != "" ?(decimal.Parse(Eval("POLICY_AMT").ToString())).ToString("#,##0") : "0"): "0"%>
                            </td>
                            <td>
                             <%# Eval("ALAE_TYP")%>
                            </td>
                            <td>
                             <%# Eval("ALAE_CAP")%>
                            </td>
                            <td>
                                <%# Eval("SYS_GENRT_IND").ToString() == "True" ? "Yes" : "No"%>
                            </td>
                            <td>
                                <%# Eval("COPY_IND").ToString() == "True" ? "Yes" : "No"%>
                            </td>
                            <td>
                                <asp:HiddenField ID="hidActive" runat="server" Value='<%# Bind("ACTV_IND")%>' />
                                <asp:ImageButton ID="imgDisable" CommandArgument='<%# Bind("ARMIS_LOS_EXC_ID") %>'
                                    runat="server" ImageUrl='<%# Eval("ACTV_IND").ToString()=="True"?"~/images/disabled.GIF":"~/images/enabled.GIF" %>'>
                                </asp:ImageButton>
                            </td>
                            <td>
                                <asp:LinkButton ID="lnkDetail" CommandArgument='<%# Bind("ARMIS_LOS_EXC_ID") %>'
                                    CommandName="DETAILS" runat="server" Text="DETAILS"></asp:LinkButton>
                            </td>
                        </tr>
                        </tr><tr>
                    </ItemTemplate>
                    <AlternatingItemTemplate>
                        <tr class="AlternatingItemTemplate" runat="server" id="trItemTemplate">
                            <td>
                                <asp:HiddenField ID="hidStatus" runat="server" Value='<%# Bind("CLAIMSTATUS") %>' />
                                <asp:HiddenField ID="HidArmisID" runat="server" Value='<%# Bind("ARMIS_LOS_EXC_ID") %>' />
                                <asp:LinkButton ID="lnkSelect" CommandArgument='<%# Bind("ARMIS_LOS_EXC_ID") %>'
                                    CommandName="Select" runat="server" Text="Select" Enabled='<%# Eval("ACTV_IND")%>'></asp:LinkButton>
                            </td>
                            <td>
                                <%# Eval("CLAIM_NBR_TXT")%>
                            </td>
                            <td>
                                <%# Eval("CLAIMANT_NM")%>
                            </td>
                            <td>
                                <%# Eval("COVG_TRIGGER_DATE", "{0:d}")%>
                            </td>
                            <td>
                                <%# Eval("CLAIMSTATUS")%>
                            </td>
                             <td>
                           <%# Eval("POLICY_AMT") != null ? (Eval("POLICY_AMT").ToString() != "" ?(decimal.Parse(Eval("POLICY_AMT").ToString())).ToString("#,##0") : "0"): "0"%>
                            </td>
                            <td>
                             <%# Eval("ALAE_TYP")%>
                            </td>
                            <td>
                             <%# Eval("ALAE_CAP")%>
                            </td>
                            <td>
                                <%# Eval("SYS_GENRT_IND").ToString() == "True" ? "Yes" : "No"%>
                            </td>
                             <td>
                                <%# Eval("COPY_IND").ToString() == "True" ? "Yes" : "No"%>
                            </td>
                            <td>
                                <asp:HiddenField ID="hidActive" runat="server" Value='<%# Bind("ACTV_IND")%>' />
                                <asp:ImageButton ID="imgDisable" CommandArgument='<%# Bind("ARMIS_LOS_EXC_ID") %>'
                                    runat="server" ImageUrl='<%# Eval("ACTV_IND").ToString()=="True"?"~/images/disabled.GIF":"~/images/enabled.GIF" %>'>
                                </asp:ImageButton>
                            </td>
                            <td>
                                <asp:LinkButton ID="lnkDetail" CommandArgument='<%# Bind("ARMIS_LOS_EXC_ID") %>'
                                    CommandName="DETAILS" runat="server" Text="DETAILS"></asp:LinkButton>
                            </td>
                        </tr>
                        </tr><tr>
                    </AlternatingItemTemplate>
                </asp:AISListView>
            </asp:Panel>
            
            <div style="padding-top: 10px">
                <asp:Label ID="lblExcNonBilDetails" Visible="false" runat="server" Text="Excess Non Billable Details"
                    CssClass="h2"></asp:Label>
                     <asp:LinkButton ID="lnkClose" Visible="false" runat="server" Text="Close"
                    OnClick="lnkClose_Click"></asp:LinkButton>
                    </div>
            <asp:Panel BorderColor="Black" ID="pnlDetails" Visible="false" BorderWidth="1" Width="910px"
                runat="server" DefaultButton="btnSave">
                <table width="100%">
                    <tr>
                        <td align="right">
                            <asp:HiddenField ID="hidindex" runat="server" Value="-1" />
                        </td>
                    </tr>
                </table>
                <table width="100%">
                    <tr>
                        <td>
                            Claim Number:
                        </td>
                        <td>
                            <asp:TextBox ID="txtClaimNumber" runat="server" MaxLength="10" Width="246px" ValidationGroup="Save"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="reqClaimNumber" runat="server" ErrorMessage="Please Enter Claim Number."
                                ValidationGroup="Save" ControlToValidate="txtClaimNumber" Text="*"></asp:RequiredFieldValidator>
                            <cc1:FilteredTextBoxExtender ID="fltrtxtClaimNumber" runat="server" TargetControlID="txtClaimNumber"
                                   InvalidChars=".!@#$%^&*()_+{}:<>?/][;',|\-" FilterMode="InvalidChars"       FilterType="Custom"  />
                            <%--<cc1:MaskedEditExtender ID="MaskedEditClaimNumber" runat="server" TargetControlID="txtClaimNumber"
                                Mask="9999999999" MessageValidatorTip="true" MaskType="Number" DisplayMoney="None"
                                OnFocusCssClass="MaskedEditFocus" OnInvalidCssClass="MaskedEditError" InputDirection="RightToLeft"
                                AcceptNegative="None" />--%>                      
                                 
                        </td>               
                        <td>
                            Claims:
                            <asp:CheckBox ID="chkAddClaims" runat="server" onclick="ClearAdditionalClaims()" />
                          
                            Additional Claims:
                        </td>
                        <td>
                            <%--<asp:CheckBox ID="chkAdditionalClaims" TabIndex="2" runat="server" Text="Add More Claims" 
                            OnCheckedChanged="chkAdditionalClaims_CheckedChanged"/>--%>
                            <asp:TextBox ID="txtAdditionalClaims" runat="server" MaxLength="50" Width="246px" ></asp:TextBox>
                            <cc1:FilteredTextBoxExtender ID="ftbeNYPremiumDiscount" runat="server" TargetControlID="txtAdditionalClaims"
                                InvalidChars=".!@#$%^&*()_+{}:<>?/][;'|\-" FilterMode="InvalidChars"       FilterType="Custom"  />
                           
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Claimant Name:
                        </td>
                        <td>
                            <asp:TextBox ID="txtClaimantName" runat="server" MaxLength="35" Width="246px" ValidationGroup="Save"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="reqClaimantName" runat="server" ErrorMessage="Please Enter Claimant Name."
                                ValidationGroup="Save" ControlToValidate="txtClaimantName" Text="*"></asp:RequiredFieldValidator>
                            <cc1:FilteredTextBoxExtender ID="fltrClaimantName" runat="server" TargetControlID="txtClaimantName"
                                FilterType="Custom" FilterMode="InvalidChars" InvalidChars=".!@#$%^&*()_+{}:<>?/][;'" />
                        </td>
                        <td>
                            Coverage Trigger Dt:
                        </td>
                        <td>
                            <asp:TextBox ID="TxtCovgTriggerDt" runat="server" Width="246px"></asp:TextBox>
                             <cc1:CalendarExtender ID="CalendarCovgTriggerDt" runat="server" PopupPosition="TopRight"
                                TargetControlID="TxtCovgTriggerDt" PopupButtonID="imgValidFrom"/>
                                <asp:ImageButton ID="imgValidFrom" runat="server" ImageUrl="~/images/Calendar_scheduleHS.png"
                                CausesValidation="False" />
                             <asp:RequiredFieldValidator ID="reqCovgTriggerDt" runat="server" ErrorMessage="Coverage Trigger Date is required."
                                ValidationGroup="Save" ControlToValidate="TxtCovgTriggerDt" Text="*"></asp:RequiredFieldValidator>
                             <cc1:MaskedEditExtender ID="MaskedEditCovgTriggerDt" runat="server" TargetControlID="TxtCovgTriggerDt"
                              Mask="99/99/9999" MessageValidatorTip="true" OnFocusCssClass="MaskedEditFocus"
                                OnInvalidCssClass="MaskedEditError" MaskType="Date" DisplayMoney="Left" AcceptNegative="Left"
                                ErrorTooltipEnabled="True" />
                                <asp:RegularExpressionValidator ID="regValidFrom" runat="server" ControlToValidate="TxtCovgTriggerDt"
                                ValidationExpression="(0?[1-9]|1[012])[- /.](0?[1-9]|[12][0-9]|3[01])[- /.](1[3-9]|2[01])\d\d"
                                ErrorMessage="Invalid Coverage Trigger Date" Text="*" ValidationGroup="Save"></asp:RegularExpressionValidator>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Claim Status:
                        </td>
                        <td>
                         <asp:ObjectDataSource ID="objDataSourceClaim" runat="server" SelectMethod="GetLookUpActiveData"
                        TypeName="ZurichNA.AIS.Business.Logic.BLAccess">
                        <SelectParameters>
                            <asp:Parameter DefaultValue="CLAIM STATUS" Name="lookUpTypeName" 
                                Type="String" />
                        </SelectParameters>
                    </asp:ObjectDataSource>
                            <asp:DropDownList ID="ddlClaimsStatus" runat="server" DataSourceID="objDataSourceClaim"
                                        DataTextField="LookUpName" DataValueField="LookUpID" Width="253px">
                            </asp:DropDownList>
                             <asp:CompareValidator ID="CompareddlClaimsStatus" runat="server" ControlToValidate="ddlClaimsStatus"
                                                                                ValueToCompare="0" ErrorMessage="Please select the Claim Status." Text="*" Operator="NotEqual"
                                                                                ValidationGroup="Save"></asp:CompareValidator>
                        </td>
                       <td>
                            Limit 2:
                        </td>
                        <td>
                            <asp:AISAmountTextbox ID="txtLimit" runat="server"  Width="246px" ValidationGroup="Save" ></asp:AISAmountTextbox>
                            <%--<asp:RequiredFieldValidator ID="reqLimit" runat="server" ErrorMessage="Please Enter Limit2."
                                ValidationGroup="Save" ControlToValidate="txtLimit" Text="*"></asp:RequiredFieldValidator>--%>
                             <%--<cc1:FilteredTextBoxExtender ID="fltrtxtLimit" runat="server" TargetControlID="txtLimit"
                                       ValidChars="0123456789,"            FilterType="Custom"  />--%>
                            <%--<cc1:MaskedEditExtender ID="MaskedEditLimit" runat="server" TargetControlID="txtLimit"
                                Mask="99,999,999,999" MessageValidatorTip="true" MaskType="Number" DisplayMoney="None"
                                OnFocusCssClass="MaskedEditFocus" OnInvalidCssClass="MaskedEditError" InputDirection="RightToLeft"
                                AcceptNegative="None" AutoComplete="false"/>--%>
                        </td>
                    </tr>
                    
                    <tr>
                        
                        <td>
                            Total Paid Indemnity:
                        </td>
                        <td>
                            <asp:AISAmountTextbox ID="txtTotPaidIdmnt" runat="server" Width="246px"    ValidationGroup="Save" ></asp:AISAmountTextbox>
                            <asp:RequiredFieldValidator ID="reqTotPaidIdmnt" runat="server" ErrorMessage="Please Enter Paid Indemnity"
                                ValidationGroup="Save" ControlToValidate="txtTotPaidIdmnt" Text="*"></asp:RequiredFieldValidator>
                             <%--<cc1:FilteredTextBoxExtender ID="fltrTotPaidIdmnt" runat="server" TargetControlID="txtTotPaidIdmnt"
                                         ValidChars="0123456789,"            FilterType="Custom" />--%>
                           <%--<cc1:MaskedEditExtender ID="MaskedEditTotPaidIdmnt" runat="server" TargetControlID="txtTotPaidIdmnt"
                                Mask="99,999,999,999" MessageValidatorTip="true" MaskType="Number" DisplayMoney="None"
                                OnFocusCssClass="MaskedEditFocus" OnInvalidCssClass="MaskedEditError" InputDirection="RightToLeft"
                                AcceptNegative="None" AutoComplete="false"/>--%>
                        </td>
                        <td>
                            Total Paid Expense:
                        </td>
                        <td>
                            <asp:AISAmountTextbox ID="txtTotPaidExp" runat="server" Width="246px" ValidationGroup="Save" ></asp:AISAmountTextbox>
                            <asp:RequiredFieldValidator ID="reqTotPaidExp" runat="server" ErrorMessage="Please Enter Paid Expense"
                                ValidationGroup="Save" ControlToValidate="txtTotPaidExp" Text="*"></asp:RequiredFieldValidator>
                             <%--<cc1:FilteredTextBoxExtender ID="fltrTotPaidExp" runat="server" TargetControlID="txtTotPaidExp"
                             ValidChars="0123456789,"            FilterType="Custom" />--%>
                            <%--<cc1:MaskedEditExtender ID="MaskedEditTotPaidExp" runat="server" TargetControlID="txtTotPaidExp"
                                Mask="99,999,999,999" MessageValidatorTip="true" MaskType="Number" DisplayMoney="None"
                                OnFocusCssClass="MaskedEditFocus" OnInvalidCssClass="MaskedEditError" InputDirection="RightToLeft"
                                AcceptNegative="None" AutoComplete="false"/>--%>
                        </td>
                    </tr>
                    <tr>
                        
                        <td>
                            Total Rsv Indemnity:
                        </td>
                        <td>
                            <asp:AISAmountTextbox ID="txtTotResrvIdmnt" runat="server" Width="246px"  ValidationGroup="Save" ></asp:AISAmountTextbox>
                            <asp:RequiredFieldValidator ID="reqTotResrvIdmnt" runat="server" ErrorMessage="Please Enter Rsv Indemnity"
                                ValidationGroup="Save" ControlToValidate="txtTotResrvIdmnt" Display="Dynamic"
                                Text="*"></asp:RequiredFieldValidator>
                             <%--<cc1:FilteredTextBoxExtender ID="fltrTotResrvIdmnt" runat="server" TargetControlID="txtTotResrvIdmnt"
                             ValidChars="0123456789,"            FilterType="Custom"  />--%> 
                            <%--<cc1:MaskedEditExtender ID="MaskedEditTotResrvIdmnt" runat="server" TargetControlID="txtTotResrvIdmnt"
                                Mask="99,999,999,999" MessageValidatorTip="true" MaskType="Number" DisplayMoney="None"
                                OnFocusCssClass="MaskedEditFocus" OnInvalidCssClass="MaskedEditError" InputDirection="RightToLeft"
                                AcceptNegative="None" AutoComplete="false"/>--%>
                        </td>
                        <td>
                            Total Rsv Expense:
                        </td>
                        <td>
                            <asp:AISAmountTextbox ID="txtTotResrvExp" runat="server" Width="246px"   ValidationGroup="Save" ></asp:AISAmountTextbox>
                            <asp:RequiredFieldValidator ID="reqTotResrvExp" runat="server" ErrorMessage="Please Enter Rsv Expense"
                                ValidationGroup="Save" ControlToValidate="txtTotResrvExp" Text="*"></asp:RequiredFieldValidator>
                             <%--<cc1:FilteredTextBoxExtender ID="fltrTotResrvExp" runat="server" TargetControlID="txtTotResrvExp"
                                 ValidChars="0123456789,"            FilterType="Custom"/>--%>
                            <%--<cc1:MaskedEditExtender ID="MaskedEditTotResrvExp" runat="server" TargetControlID="txtTotResrvExp"
                                Mask="99,999,999,999" MessageValidatorTip="true" MaskType="Number" DisplayMoney="None"
                                OnFocusCssClass="MaskedEditFocus" OnInvalidCssClass="MaskedEditError" InputDirection="RightToLeft"
                                AcceptNegative="None" AutoComplete="false"/>--%>
                        </td>
                    </tr>
                    <tr>
                        
                        <td>
                            NonBillable Paid Indemnity:
                        </td>
                        <td>
                            <asp:AISAmountTextbox ID="txtNonBilPaidIdmnt" runat="server" Width="246px"  ValidationGroup="Save" ></asp:AISAmountTextbox>
                            <%--<asp:RequiredFieldValidator ID="reqNonBilPaidIdmnt" runat="server" ErrorMessage="Please Enter NonBillable Paid Indemnity"
                                ValidationGroup="Save" ControlToValidate="txtNonBilPaidIdmnt" Display="Dynamic"
                                Text="*"></asp:RequiredFieldValidator>--%>
                            <%--<cc1:FilteredTextBoxExtender ID="fltrNonBilPaidIdmnt" runat="server" TargetControlID="txtNonBilPaidIdmnt"
                                    ValidChars="0123456789,"            FilterType="Custom"  />--%>
                            <%--<cc1:MaskedEditExtender ID="MaskedEditNonBilPaidIdmnt" runat="server" TargetControlID="txtNonBilPaidIdmnt"
                                Mask="99,999,999,999" MessageValidatorTip="true" MaskType="Number" DisplayMoney="None"
                                OnFocusCssClass="MaskedEditFocus" OnInvalidCssClass="MaskedEditError" InputDirection="RightToLeft"
                                AcceptNegative="None" AutoComplete="false"/>--%>
                        </td>
                        <td>
                            NonBillable Paid Expense:
                        </td>
                        <td>
                            <asp:AISAmountTextbox ID="txtNonBilPaidExp" runat="server" Width="246px"  ValidationGroup="Save" ></asp:AISAmountTextbox>
                            <%--<asp:RequiredFieldValidator ID="reqNonBilPaidExp" runat="server" ErrorMessage="Please Enter NonBillable Paid Expense"
                                ValidationGroup="Save" ControlToValidate="txtNonBilPaidExp" Text="*"></asp:RequiredFieldValidator>--%>
                              <%--<cc1:FilteredTextBoxExtender ID="fltrNonBilPaidExp" runat="server" TargetControlID="txtNonBilPaidExp"
                                  ValidChars="0123456789,"            FilterType="Custom"  />--%>
                            <%--<cc1:MaskedEditExtender ID="MaskedEditNonBilPaidExp" runat="server" TargetControlID="txtNonBilPaidExp"
                                Mask="99,999,999,999" MessageValidatorTip="true" MaskType="Number" DisplayMoney="None"
                                OnFocusCssClass="MaskedEditFocus" OnInvalidCssClass="MaskedEditError" InputDirection="RightToLeft"
                                AcceptNegative="None" AutoComplete="false"/>--%>
                        </td>
                    </tr>
                    <tr>
                        
                        <td>
                            NonBillable Rsv Indemnity:
                        </td>
                        <td>
                            <asp:AISAmountTextbox ID="txtNonBilResrvIdmnt" runat="server" Width="246px"  ValidationGroup="Save" ></asp:AISAmountTextbox>
                            <%--<asp:RequiredFieldValidator ID="reqNonBilResrvIdmnt" runat="server" ErrorMessage="Please Enter NonBillable Resrv Indemnity"
                                ValidationGroup="Save" ControlToValidate="txtNonBilResrvIdmnt" Display="Dynamic"
                                Text="*"></asp:RequiredFieldValidator>--%>
                             <%--<cc1:FilteredTextBoxExtender ID="fltrNonBilResrvIdmnt" runat="server" TargetControlID="txtNonBilResrvIdmnt"
                                  ValidChars="0123456789,"            FilterType="Custom"  />--%> 
                            <%--<cc1:MaskedEditExtender ID="MaskedEditNonBilResrvIdmnt" runat="server" TargetControlID="txtNonBilResrvIdmnt"
                                Mask="99,999,999,999" MessageValidatorTip="true" MaskType="Number" DisplayMoney="None"
                                OnFocusCssClass="MaskedEditFocus" OnInvalidCssClass="MaskedEditError" InputDirection="RightToLeft"
                                AcceptNegative="None" AutoComplete="false"/>--%>
                        </td>
                        <td>
                            NonBillable Rsv Expense:
                        </td>
                        <td>
                            <asp:AISAmountTextbox ID="txtNonBilResrvExp" runat="server" Width="246px"  ValidationGroup="Save" ></asp:AISAmountTextbox>
                            <%--<asp:RequiredFieldValidator ID="reqNonBilResrvExp" runat="server" ErrorMessage="Please Enter NonBillable Resrv Expense"
                                ValidationGroup="Save" ControlToValidate="txtNonBilResrvExp" Text="*"></asp:RequiredFieldValidator>--%>
                             <%--<cc1:FilteredTextBoxExtender ID="fltrNonBilResrvExp" runat="server" TargetControlID="txtNonBilResrvExp"
                                 ValidChars="0123456789,"            FilterType="Custom" />--%>
                            <%--<cc1:MaskedEditExtender ID="MaskedEditNonBilResrvExp" runat="server" TargetControlID="txtNonBilResrvExp"
                                Mask="99,999,999,999" MessageValidatorTip="true" MaskType="Number" DisplayMoney="None"
                                OnFocusCssClass="MaskedEditFocus" OnInvalidCssClass="MaskedEditError" InputDirection="RightToLeft"
                                AcceptNegative="None" AutoComplete="false"/>--%>
                        </td>
                    </tr>
                    <tr>
                        
                        <td>
                        </td>
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4" style="text-align: center">
                            <asp:Button ID="btnSave" Enabled="false" Text="Save" ValidationGroup="Save" runat="server"
                                OnClick="btnSave_Click" />
                                <asp:Button ID="btnCancel" Enabled="true" runat="server" Text="Cancel" OnClick="btnCancel_Click" />
                            <asp:Button ID="btnCopy" Enabled="false" runat="server" Text="Copy" OnClick="btnCopy_Click" Visible="false"/>
                            &nbsp;&nbsp;&nbsp;
                        </td>
                    </tr>
                </table>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
    </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
