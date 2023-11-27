<%@ Page Language="C#" MasterPageFile="~/Retro.master" AutoEventWireup="true" Inherits="AppMgmt_TransactionMaintenance"
    Title="Transaction Maintenance" CodeBehind="TransactionMaintenance.aspx.cs" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxtoolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeaderPlaceHolder" runat="Server">
    <table width="910" style="text-align: right">
        <tr>
            <td align="left">
                <asp:Label ID="GAILabel" runat="server" Text="Transaction Maintenance" CssClass="h1"></asp:Label>
            </td>
        </tr>
    </table>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainPlaceHolder" runat="Server">
<script language="javascript" type="text/javascript">
    var scrollTop1;
    var scrollTop2;
    var scrollTop3;
    var scrollTop4;
    var scrollTop5;
    var scrollTop6;
    
    if(Sys.WebForms.PageRequestManager != null)
    {
        Sys.WebForms.PageRequestManager.getInstance().add_beginRequest(BeginRequestHandler);
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);
    }

    function BeginRequestHandler(sender, args) 
    {
        var lkt = $get('<%=pnlLookupType.ClientID%>');
        if(lkt!=null)
        scrollTop1 = lkt.scrollTop;
        
        var lk = $get('<%=pnlLookupDetails.ClientID%>');
        if(lk!=null)
        scrollTop2 = lk.scrollTop;

        var pt = $get('<%=pnlPostTrans.ClientID%>');
        if(pt!=null)
        scrollTop3 = pt.scrollTop;
        
        var pl = $get('<%=Panel1.ClientID%>');
        if(pl!=null)
        scrollTop4 = pl.scrollTop;

        var id = $get('<%=pnlIssueDtailsList.ClientID%>');
        if(id!=null)
        scrollTop5 = id.scrollTop;

        var bu = $get('<%=pnlBUOffice.ClientID%>');
        if (bu != null)
        scrollTop6 = bu.scrollTop;
    
    }

    function EndRequestHandler(sender, args)
    {
        var lkt = $get('<%=pnlLookupType.ClientID%>');
        if(lkt!=null)
        lkt.scrollTop = scrollTop1;

        var lk = $get('<%=pnlLookupDetails.ClientID%>');
        if(lk!=null)
        lk.scrollTop = scrollTop2;

        var pt = $get('<%=pnlPostTrans.ClientID%>');
        if(pt!=null)
        pt.scrollTop = scrollTop3;

        var pl = $get('<%=Panel1.ClientID%>');
        if(pl!=null)
        pl.scrollTop = scrollTop4;

        var id = $get('<%=pnlIssueDtailsList.ClientID%>');
        if(id!=null)
            id.scrollTop = scrollTop5;

        var bu = $get('<%=pnlBUOffice.ClientID%>');
        if (bu != null)
            bu.scrollTop = scrollTop6;
    } 
</script>


    <script src="../JavaScript/RetroScript.js" type="text/javascript"></script>
    <script type="text/javascript">
        function Tabnavigation(Pagename)
        {
        if(Pagename=="PT")
        window.location.href="../AcctMgmt/PostingTransactionType.aspx?wID=<%= WindowName%>";
        }
    </script>

    <table>
        <tr>
            <td>
                <asp:ValidationSummary ID="valSumaSave" ValidationGroup="Save" runat="server"></asp:ValidationSummary>
                <asp:ValidationSummary ID="valSumaUpdate" ValidationGroup="update" runat="server">
                </asp:ValidationSummary>
                <asp:ValidationSummary ID="valSaveLkup" CssClass="ValidationSummary" ValidationGroup="SaveLkup"
                    runat="server"></asp:ValidationSummary>
                <asp:ValidationSummary ID="valsumupdateLkup" CssClass="ValidationSummary" ValidationGroup="updateLkup"
                    runat="server"></asp:ValidationSummary>
                <asp:ValidationSummary ID="valPostingsave" ValidationGroup="PostingSave" runat="server">
                </asp:ValidationSummary>
                <asp:ValidationSummary ID="valPostingUpdate" ValidationGroup="Postingupdate" runat="server">
                </asp:ValidationSummary>
                <asp:ValidationSummary ID="valBUOfficeSave" ValidationGroup="BUSave" runat="server">
                </asp:ValidationSummary>
                <asp:ValidationSummary ID="valBUOfficeUpdate" ValidationGroup="BUUpdate" runat="server">
                </asp:ValidationSummary>
                
                <ajaxtoolkit:TabContainer ID="TabContainerTransactionMaintainance" runat="server"
                    CssClass="VariableTabs" SkinID="tabVariable" ActiveTabIndex="0">
                    <ajaxtoolkit:TabPanel runat="server" ID="tblpnlLookup">
                        <HeaderTemplate>
                            Lookup Types &amp; Lookup Codes
                        </HeaderTemplate>
                        <ContentTemplate>
                            <asp:UpdatePanel ID="updpnlLookup" runat="server">
                                <ContentTemplate>
                                    <br />
                                    <table width="910" style="text-align: right">
                                        <tr>
                                            <td align="left">
                                                <asp:Label ID="lblLookupTypes" runat="server"  Text="Lookup Types"
                                                    CssClass="h2"></asp:Label>
                                                &nbsp;
                                            </td>
                                        </tr>
                                    </table>
                                    <asp:Panel ID="pnlLookupType" runat="server" Height="200px" ScrollBars="Auto"  Width="910px">
                                        <asp:AISListView ID="lstLookupType" runat="server" InsertItemPosition="FirstItem" DataKeyNames="LOOKUPTYPE_ID"
                                            OnItemCommand="CommandList" OnItemDataBound="DataBoundList" OnItemEditing="EditList"
                                            OnItemUpdating="UpdateList" OnItemCanceling="CancelList" OnItemInserting="InsertList"
                                            OnSelectedIndexChanging="lstLookupType_SelectedIndexChanging">
                                            <LayoutTemplate>
                                                <table id="Table1" class="panelContents" runat="server" width="98%">
                                                    <tr class="LayoutTemplate">
                                                        <th>
                                                        </th>
                                                        <th>
                                                            Lookup Type
                                                        </th>
                                                        <th>
                                                            Lookup
                                                        </th>
                                                        <th>
                                                            Disable
                                                        </th>
                                                    </tr>
                                                    <tr id="itemPlaceholder" runat="server">
                                                    </tr>
                                                </table>
                                            </LayoutTemplate>
                                            <AlternatingItemTemplate>
                                                <tr class="AlternatingItemTemplate" id="trItemTemplate" runat="server">
                                                    <td align="left">
                                                        <asp:LinkButton Enabled='<%# Eval("ACTIVE")%>' ID="lnkEdit" CommandName="Edit" runat="server"
                                                            Text="EDIT"></asp:LinkButton>
                                                    </td>
                                                    <td align="left">
                                                        <asp:Label ID="lkupTypeName" runat="server" Text='<%# Bind("LOOKUPTYPE_NAME") %>'></asp:Label>
                                                    </td>
                                                    <td align="left">
                                                        <asp:LinkButton ID="lnkSelect" Enabled='<%# Eval("ACTIVE")%>' CommandArgument='<%# Bind("LOOKUPTYPE_ID") %>'
                                                            CommandName="Select" runat="server" Text="Details"></asp:LinkButton>
                                                    </td>
                                                    <td>
                                                        <asp:HiddenField ID="hidActive" runat="server" Value='<%# Bind("ACTIVE")%>' />
                                                        <asp:ImageButton ID="imgDisable" CommandArgument='<%# Bind("LOOKUPTYPE_ID") %>' runat="server"
                                                            ImageUrl='<%# Eval("ACTIVE").ToString()=="True"?"~/images/disabled.GIF":"~/images/enabled.GIF" %>'>
                                                        </asp:ImageButton>
                                                    </td>
                                                </tr>
                                            
                                            </AlternatingItemTemplate>
                                            <EditItemTemplate>
                                                <tr class="ItemTemplate">
                                                    <td align="left">
                                                        <asp:LinkButton ID="lnkUpdate" ValidationGroup="update" CommandArgument='<%# Bind("LOOKUPTYPE_ID") %>'
                                                            CommandName="Update" runat="server" Text="UPDATE"></asp:LinkButton>
                                                        <asp:LinkButton ID="lnkCancel" CommandName="Cancel" runat="server" Text="CANCEL"></asp:LinkButton>
                                                    </td>
                                                    <td align="left">
                                                        <asp:HiddenField ID="hidEditLkupTypeID" runat="server" Value='<%# Bind("LOOKUPTYPE_ID") %>' />
                                                        <asp:TextBox ID="txtEditlkupTypeName" MaxLength="50" ValidationGroup="update" runat="server"
                                                            Text='<%# Bind("LOOKUPTYPE_NAME") %>'></asp:TextBox>
                                                        <asp:RequiredFieldValidator ID="reqEditlkupTypeName" runat="server" ErrorMessage="Please enter Lookup type name"
                                                            Text="*" ValidationGroup="update" ControlToValidate="txtEditlkupTypeName"></asp:RequiredFieldValidator>
                                                        <ajaxtoolkit:FilteredTextBoxExtender TargetControlID="txtEditlkupTypeName" FilterType="UppercaseLetters,Custom, LowercaseLetters, Numbers"
                                                            ValidChars=" " ID="fltrtxtEditlkupTypeName" runat="server" />
                                                    </td>
                                                    <td>
                                                    </td>
                                                    <td>
                                                    </td>
                                                </tr>
                                             
                                            </EditItemTemplate>
                                            <InsertItemTemplate>
                                                <tr class="AlternatingItemTemplate">
                                                    <td align="left">
                                                        <asp:LinkButton ValidationGroup="Save" ID="lnkSave" CommandName="Save" runat="server"
                                                            Text="SAVE"></asp:LinkButton>
                                                    </td>
                                                    <td align="left">
                                                        <asp:TextBox ValidationGroup="Save" MaxLength="50" ID="txtlkupTypeName" runat="server"></asp:TextBox>
                                                        <asp:RequiredFieldValidator ID="reqlkupTypeName" runat="server" ErrorMessage="Please enter Lookup type name"
                                                            Text="*" ValidationGroup="Save" ControlToValidate="txtlkupTypeName"></asp:RequiredFieldValidator>
                                                        <ajaxtoolkit:FilteredTextBoxExtender TargetControlID="txtlkupTypeName" FilterType="UppercaseLetters,Custom, LowercaseLetters, Numbers"
                                                            ValidChars=" " ID="fltrtxtlkupTypeName" runat="server" />
                                                    </td>
                                                    <td>
                                                    </td>
                                                    <td>
                                                    </td>
                                                </tr>
                                              
                                            </InsertItemTemplate>
                                            <ItemTemplate>
                                                <tr class="ItemTemplate" id="trItemTemplate" runat="server">
                                                    <td align="left">
                                                        <asp:LinkButton ID="lnkEdit" Enabled='<%# Eval("ACTIVE")%>' runat="server" CommandName="Edit"
                                                            Text="EDIT"></asp:LinkButton>
                                                    </td>
                                                    <td align="left">
                                                        <asp:Label ID="lkupTypeName" runat="server" Text='<%# Bind("LOOKUPTYPE_NAME") %>'></asp:Label>
                                                    </td>
                                                    <td align="left">
                                                        <asp:LinkButton ID="lnkSelect" Enabled='<%# Eval("ACTIVE")%>' runat="server" CommandArgument='<%# Bind("LOOKUPTYPE_ID") %>'
                                                            CommandName="Select" Text="Details"></asp:LinkButton>
                                                    </td>
                                                    <td>
                                                        <asp:HiddenField ID="hidActive" runat="server" Value='<%# Bind("ACTIVE")%>' />
                                                        <asp:ImageButton ID="imgDisable" CommandArgument='<%# Bind("LOOKUPTYPE_ID") %>' runat="server"
                                                            ImageUrl='<%# Eval("ACTIVE").ToString()=="True"?"~/images/disabled.GIF":"~/images/enabled.GIF" %>'>
                                                        </asp:ImageButton>
                                                    </td>
                                                </tr>
                                               
                                            </ItemTemplate>
                                        </asp:AISListView>
                                    </asp:Panel>
                                    <br />
                                    <br />
                                    <div runat="server" id="divCancel" visible="False">
                                        <asp:Label ID="lblLookupTypeName" CssClass="h2" runat="server"></asp:Label>
                                        &nbsp;&nbsp;
                                        <asp:LinkButton ID="lnkClose" runat="server" OnClick="lnkClose_Click">Close</asp:LinkButton>
                                    </div>
                                    <asp:Panel ID="pnlLookupDetails" Visible="False" runat="server" Height="150px" ScrollBars="Auto"
                                        Width="910px">
                                        <asp:AISListView ID="lstLookupDetails" runat="server" InsertItemPosition="FirstItem"
                                            DataKeyNames="LookUpID" OnItemCommand="CommandList" OnItemDataBound="DataBoundList"
                                            OnItemEditing="EditList" OnItemUpdating="UpdateList" OnItemCanceling="CancelList"
                                            OnItemInserting="InsertList">
                                            <LayoutTemplate>
                                                <table id="Table1" class="panelContents" runat="server" width="98%">
                                                    <tr class="LayoutTemplate">
                                                        <th>
                                                        </th>
                                                        <th>
                                                            Lookup Name
                                                        </th>
                                                        <th>
                                                            Attribute1
                                                        </th>
                                                        <th>
                                                            Disable
                                                        </th>
                                                    </tr>
                                                    <tr id="itemPlaceholder" runat="server">
                                                    </tr>
                                                </table>
                                            </LayoutTemplate>
                                            <AlternatingItemTemplate>
                                                <tr class="AlternatingItemTemplate" id="trItemTemplate" runat="server">
                                                    <td align="left">
                                                        <asp:LinkButton Enabled='<%# Eval("ACTIVE")%>' ID="lnkEdit" runat="server" CommandName="Edit"
                                                            Text="EDIT"></asp:LinkButton>
                                                    </td>
                                                    <td align="left">
                                                        <asp:Label ID="lkupTypeName" runat="server" Text='<%# Bind("LookUpName") %>'></asp:Label>
                                                    </td>
                                                    <td align="left">
                                                        <asp:Label ID="lblAttribute" runat="server" Text='<%# Bind("Attribute1") %>'></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:HiddenField ID="hidActive" runat="server" Value='<%# Bind("ACTIVE")%>' />
                                                        <asp:ImageButton ID="imgDisable" CommandArgument='<%# Bind("LookUpID") %>' runat="server"
                                                            ImageUrl='<%# Eval("ACTIVE").ToString()=="True"?"~/images/disabled.GIF":"~/images/enabled.GIF" %>'>
                                                        </asp:ImageButton>
                                                    </td>
                                                </tr>
                                              
                                            </AlternatingItemTemplate>
                                            <EditItemTemplate>
                                                <tr class="ItemTemplate">
                                                    <td align="left">
                                                        <asp:LinkButton ID="lnkUpdate" runat="server" CommandArgument='<%# Bind("LookUpID") %>'
                                                            CommandName="Update" Text="UPDATE" ValidationGroup="updateLkup"></asp:LinkButton>
                                                        <asp:LinkButton ID="lnkCancel" runat="server" CommandName="Cancel" Text="CANCEL"></asp:LinkButton>
                                                    </td>
                                                    <td align="left">
                                                        <asp:HiddenField ID="hidEditLkupID" runat="server" Value='<%# Bind("LookUpID") %>' />
                                                        <asp:TextBox ID="txtEditlkupName" runat="server" Text='<%# Bind("LookUpName") %>'
                                                            ValidationGroup="updateLkup" MaxLength="75"></asp:TextBox>
                                                        <asp:RequiredFieldValidator ID="reqEditlkupTypeName" runat="server" ControlToValidate="txtEditlkupName"
                                                            ErrorMessage="Please enter Lookup name" Text="*" ValidationGroup="updateLkup"></asp:RequiredFieldValidator>
                                                        <%--<ajaxtoolkit:FilteredTextBoxExtender TargetControlID="txtEditlkupName" FilterType="UppercaseLetters,Custom, LowercaseLetters, Numbers"
                                                            ValidChars=" " ID="fltrEditLktypeName" runat="server" />--%>
                                                    </td>
                                                    <td align="left">
                                                        <asp:TextBox ID="txtAttribute" MaxLength="75" runat="server" Text='<%# Bind("Attribute1") %>'></asp:TextBox>
                                                        <%--<ajaxtoolkit:FilteredTextBoxExtender TargetControlID="txtAttribute" FilterType="UppercaseLetters,Custom, LowercaseLetters, Numbers"
                                                            ValidChars=" " ID="fltrtxtAttribute" runat="server" />--%>
                                                    </td>
                                                    <td>
                                                    </td>
                                                </tr>
                                              
                                            </EditItemTemplate>
                                            <InsertItemTemplate>
                                                <tr class="AlternatingItemTemplate">
                                                    <td align="left">
                                                        <asp:LinkButton ID="lnkSave" runat="server" CommandName="Save" Text="SAVE" ValidationGroup="SaveLkup"></asp:LinkButton>
                                                    </td>
                                                    <td align="left">
                                                        <asp:TextBox ID="txtlkupTypeName" MaxLength="75" runat="server" ValidationGroup="SaveLkup"></asp:TextBox>
                                                        <asp:RequiredFieldValidator ID="reqlkupTypeName" runat="server" ControlToValidate="txtlkupTypeName"
                                                            ErrorMessage="Please enter Lookup name" Text="*" ValidationGroup="SaveLkup"></asp:RequiredFieldValidator>
                                                        <%--<ajaxtoolkit:FilteredTextBoxExtender TargetControlID="txtlkupTypeName" FilterType="UppercaseLetters,Custom, LowercaseLetters, Numbers"
                                                            ValidChars=" " ID="fltrLktypeName" runat="server" />--%>
                                                    </td>
                                                    <td align="left">
                                                        <asp:TextBox ID="txtAttribute1" MaxLength="75" runat="server"></asp:TextBox>
                                                        <%--<ajaxtoolkit:FilteredTextBoxExtender TargetControlID="txtAttribute1" FilterType="UppercaseLetters,Custom, LowercaseLetters, Numbers"
                                                            ValidChars=" " ID="fltrtxtAttribute1" runat="server" />--%>
                                                    </td>
                                                    <td>
                                                    </td>
                                                </tr>
                                              
                                            </InsertItemTemplate>
                                            <ItemTemplate>
                                                <tr class="ItemTemplate" id="trItemTemplate" runat="server">
                                                    <td align="left">
                                                        <asp:LinkButton ID="lnkitemEdit" Enabled='<%# Eval("ACTIVE")%>' runat="server" CommandName="Edit"
                                                            Text="EDIT"></asp:LinkButton>
                                                    </td>
                                                    <td align="left">
                                                        <asp:Label ID="lkupTypeName" runat="server" Text='<%# Bind("LookUpName") %>'></asp:Label>
                                                    </td>
                                                    <td align="left">
                                                        <asp:Label ID="lblAttribute" runat="server" Text='<%# Bind("Attribute1") %>'></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:HiddenField ID="hidActive" runat="server" Value='<%# Bind("ACTIVE")%>' />
                                                        <asp:ImageButton ID="imgDisable" CommandArgument='<%# Bind("LookUpID") %>' runat="server"
                                                            ImageUrl='<%# Eval("ACTIVE").ToString()=="True"?"~/images/disabled.GIF":"~/images/enabled.GIF" %>'>
                                                        </asp:ImageButton>
                                                    </td>
                                                  
                                                </tr>
                                            </ItemTemplate>
                                        </asp:AISListView>
                                    </asp:Panel>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </ContentTemplate>
                    </ajaxtoolkit:TabPanel>
                    <ajaxtoolkit:TabPanel runat="server" ID="tblpnlPostTrans">
                        <HeaderTemplate>
                            <div>
                                Posting Transaction Setup</div>
                        </HeaderTemplate>
                        <ContentTemplate>
                            <br />
                            <br />
                            <asp:Label ID="lblHeader" runat="server" CssClass="h2" Text="Posting Transaction Setup"></asp:Label>
                            <br />
                            <br />
                            <asp:ObjectDataSource ID="TransTypeDataSource" runat="server" SelectMethod="GetLookUpActiveData"
                                TypeName="ZurichNA.AIS.Business.Logic.BLAccess">
                                <SelectParameters>
                                    <asp:Parameter DefaultValue="TRANSACTION TYPE" Name="lookUpTypeName" Type="String" />
                                </SelectParameters>
                            </asp:ObjectDataSource>
                            <asp:Panel ID="pnlPostTrans" Height="300px" Width="910px" runat="server" ScrollBars="Auto">
                                <asp:AISListView ID="lstPostTrans" runat="server" OnItemDataBound="lstPostTrans_DataBoundList"
                                    InsertItemPosition="FirstItem" OnItemEditing="lstPostTrans_ItemEdit" OnItemCanceling="lstPostTrans_ItemCancel"
                                    OnItemCommand="lstPostTrans_ItemCommand" OnItemInserting="lstPostTrans_ItemInserting"
                                    OnSorting="lstPostTrans_Sorting" OnItemUpdating="lstPostTrans_ItemUpdate">
                                    <LayoutTemplate>
                                        <table id="Table1" class="panelContents" runat="server">
                                            <tr class="LayoutTemplate">
                                                <th>
                                                </th>
                                                
                                                <th align="center">
                                        <asp:LinkButton ID="lnkTransType" runat="server" CommandName="Sort" CommandArgument="TRANSACTIONTYPE">Transaction Type</asp:LinkButton>
                                        <asp:Image ID="imgTransType" runat="server" ImageUrl="~/images/Ascending.gif"
                                            ToolTip="Ascending" Visible="false" />
                                                </th>
                                                <th>
                                                    Transaction Name
                                                </th>
                                                <th align="center">
                                        <asp:LinkButton ID="lnkMain" runat="server" CommandName="Sort" CommandArgument="MAIN">Main</asp:LinkButton>
                                        <asp:Image ID="imgMain" runat="server" ImageUrl="~/images/Ascending.gif"
                                            ToolTip="Ascending" Visible="false" />
                                                </th>
                                                <th align="center">
                                        <asp:LinkButton ID="lnkSub" runat="server" CommandName="Sort" CommandArgument="SUB">Sub</asp:LinkButton>
                                        <asp:Image ID="imgSub" runat="server" ImageUrl="~/images/Ascending.gif"
                                            ToolTip="Ascending" Visible="false" />
                                            </th>
                                                <th align="center">
                                        <asp:LinkButton ID="lnkComapny" runat="server" CommandName="Sort" CommandArgument="COMPANY">Company</asp:LinkButton>
                                        <asp:Image ID="imgCompany" runat="server" ImageUrl="~/images/Ascending.gif"
                                            ToolTip="Ascending" Visible="false" />
                                                </th>
                                               <th align="center">
                                        <asp:LinkButton ID="lnkInvoice" runat="server" CommandName="Sort" CommandArgument="INVOICE">Summary Invoice</asp:LinkButton>
                                        <asp:Image ID="imgInvoice" runat="server" ImageUrl="~/images/Ascending.gif"
                                            ToolTip="Ascending" Visible="false" />
                                            </th>
                                            <th align="center">
                                        <asp:LinkButton ID="lnkPosting" runat="server" CommandName="Sort" CommandArgument="POSTING">Posting</asp:LinkButton>
                                        <asp:Image ID="imgPosting" runat="server" ImageUrl="~/images/Ascending.gif"
                                            ToolTip="Ascending" Visible="false" />
                                            </th>
                                                <th align="center">
                                        <asp:LinkButton ID="lnkTpaManual" runat="server" CommandName="Sort" CommandArgument="TPAMANUAL">Policy IO</asp:LinkButton>
                                        <asp:Image ID="imgTpaManual" runat="server" ImageUrl="~/images/Ascending.gif"
                                            ToolTip="Ascending" Visible="false" />
                                            </th>
                                                
                                                <th>
                                                    Adjustment Summary 
                                                </th>
                                                <th>
                                                    Policy Required
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
                                        <tr id="trItemTemplate" runat="server" class="ItemTemplate">
                                            <td>
                                                <asp:LinkButton ID="lbTransEdit" Enabled='<%# Eval("ACTV_IND")%>' CommandName="Edit" Text="Edit" runat="server" Visible="true" />
                                            </td>
                                            <td>
                                                <asp:Label ID="lblTransType" runat="server" Text='<%# Bind("TRANSACTIONTYPE") %>'></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="lblTransName" runat="server" Text='<%# Bind("TRANS_TXT") %>' />
                                            </td>
                                            <td>
                                                <asp:Label ID="lblMain" runat="server" Text='<%# Bind("MAIN_NBR") %>' />
                                            </td>
                                            <td>
                                                <asp:Label ID="lblSub" runat="server" Text='<%# Bind("SUB_NBR") %>' />
                                            </td>
                                            <td>
                                                <asp:Label ID="lblCompany" runat="server" Text='<%# Bind("COMP_TXT") %>' />
                                            </td>
                                            <td>
                                                <asp:Label ID="lblInvoicable" runat="server" Text='<%# Bind("INVOICBL_IND_txt") %>' />
                                            </td>
                                            <td>
                                                <asp:Label ID="lblMisc" runat="server" Text='<%# Bind("MISC_POSTS_IND_txt") %>' />
                                            </td>
                                            <td>
                                                <asp:Label ID="lblTpaManual" runat="server" Text='<%# Bind("THRD_PTY_ADMIN_MNL_IND_txt") %>' />
                                            </td>
                                            <td>
                                                <asp:Label ID="lblAdjSummary" runat="server" Text='<%# Bind("ADJ_SUMRY_NOT_POST_IND_txt") %>' />
                                            </td>
                                            <td>
                                                <asp:Label ID="lblPolReq" runat="server" Text='<%# Bind("POL_REQR_IND_txt") %>' />
                                            </td>
                                            <td>
                                                <asp:Label ID="lblActive" runat="server" Text='<%# Bind("ACTV_IND") %>' Visible="false"></asp:Label>
                                                <asp:ImageButton ID="ibtnEnableDisable" runat="server" CommandArgument='<%# Bind("POST_TRANS_TYP_ID") %>' />
                                            </td>
                                        </tr>
                                    </ItemTemplate>
                                    <AlternatingItemTemplate>
                                        <tr id="trItemTemplate" runat="server" class="AlternatingItemTemplate">
                                            <td>
                                                <asp:LinkButton ID="lbtransEdit" Enabled='<%# Eval("ACTV_IND")%>' CommandName="Edit" Text="Edit" runat="server" Visible="true" />
                                            </td>
                                            <td>
                                                <asp:Label ID="lblTransType" runat="server" Text='<%# Bind("TRANSACTIONTYPE") %>'></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="lblTransName" runat="server" Text='<%# Bind("TRANS_TXT") %>' />
                                            </td>
                                            <td>
                                                <asp:Label ID="lblMain" runat="server" Text='<%# Bind("MAIN_NBR") %>' />
                                            </td>
                                            <td>
                                                <asp:Label ID="lblSub" runat="server" Text='<%# Bind("SUB_NBR") %>' />
                                            </td>
                                            <td>
                                                <asp:Label ID="lblCompany" runat="server" Text='<%# Bind("COMP_TXT") %>' />
                                            </td>
                                            <td>
                                                <asp:Label ID="lblInvoicable" runat="server" Text='<%# Bind("INVOICBL_IND_txt") %>' />
                                            </td>
                                            <td>
                                                <asp:Label ID="lblMisc" runat="server" Text='<%# Bind("MISC_POSTS_IND_txt") %>' />
                                            </td>
                                            <td>
                                                <asp:Label ID="lblTpaManual" runat="server" Text='<%# Bind("THRD_PTY_ADMIN_MNL_IND_txt") %>' />
                                            </td>
                                            <td>
                                                <asp:Label ID="lblAdjSummary" runat="server" Text='<%# Bind("ADJ_SUMRY_NOT_POST_IND_txt") %>' />
                                            </td>
                                            <td>
                                                <asp:Label ID="lblPolReq" runat="server" Text='<%# Bind("POL_REQR_IND_txt") %>' />
                                            </td>
                                            <td>
                                                <asp:Label ID="lblActive" runat="server" Text='<%# Bind("ACTV_IND") %>' Visible="false"></asp:Label>
                                                <asp:ImageButton ID="ibtnEnableDisable" runat="server" CommandArgument='<%# Bind("POST_TRANS_TYP_ID") %>' />
                                            </td>
                                        </tr>
                                    </AlternatingItemTemplate>
                                    <InsertItemTemplate>
                                        <tr id="tr2" runat="server" class="AlternatingItemTemplate">
                                            <td align="center">
                                                <asp:LinkButton ID="lblTransInsert" CommandName="Save" Text="Save" runat="server"
                                                    Visible="true" ValidationGroup="PostingSave" />
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlTransTypelist" runat="server" DataSourceID="TransTypeDataSource"
                                                    DataTextField="LookUpName" DataValueField="LookUpID" Width="100px" ValidationGroup="PostingSave">
                                                </asp:DropDownList>
                                                <asp:CompareValidator ID="CompareValidator1" runat="server" ControlToValidate="ddlTransTypelist"
                                                    ErrorMessage="Transaction Type required" Text="*" Operator="NotEqual" ValueToCompare="0" ValidationGroup="PostingSave">
                                                </asp:CompareValidator>
                                            </td>
                                            <td>
                                                <ajaxtoolkit:FilteredTextBoxExtender TargetControlID="txtTransName" FilterType="UppercaseLetters,Custom, LowercaseLetters, Numbers"
                                                    ValidChars=" ,&" ID="fteTransName" runat="server" />
                                                <asp:TextBox ID="txtTransName" runat="server" Width="200px" MaxLength="100" Text="" />
                                            </td>
                                            <td>
                                                <ajaxtoolkit:MaskedEditExtender ID="MainTextBoxExtender" runat="server" Mask="9999"
                                                    MaskType="Number" TargetControlID="txtMain" AutoComplete="false" InputDirection="RightToLeft">
                                                </ajaxtoolkit:MaskedEditExtender>
                                                <asp:TextBox ID="txtMain" runat="server" Width="40px" MaxLength="4" Text="" />
                                            </td>
                                            <td>
                                                <ajaxtoolkit:MaskedEditExtender ID="SubTextBoxExtender" runat="server" Mask="9999"
                                                    MaskType="Number" TargetControlID="txtSub" AutoComplete="false" InputDirection="RightToLeft">
                                                </ajaxtoolkit:MaskedEditExtender>
                                                <asp:TextBox ID="txtSub" runat="server" Width="40px" MaxLength="4" Text="" />
                                            </td>
                                            <td>
                                                <ajaxtoolkit:FilteredTextBoxExtender ID="CompanyTextBoxExtender" runat="server" FilterType="Numbers,LowercaseLetters,UppercaseLetters,Custom"
                                                    TargetControlID="txtCompany" ValidChars=" ">
                                                </ajaxtoolkit:FilteredTextBoxExtender>
                                                <asp:TextBox ID="txtCompany" runat="server" Width="40px" MaxLength="4" Text="" />
                                            </td>
                                            <td align="center">
                                                <asp:CheckBox ID="chkInv" runat="server" Text="" />
                                            </td>
                                            <td align="center">
                                                <asp:CheckBox ID="chkMisc" runat="server" Text="" />
                                            </td>
                                            <td align="center">
                                                <asp:CheckBox ID="chkTpa" runat="server" Text="" />
                                            </td>
                                            <td align="center">
                                                <asp:CheckBox ID="chkAdjSummary" runat="server" Text="" />
                                            </td>
                                            <td align="center">
                                                <asp:CheckBox ID="ChkPolReq" runat="server" Text="" />
                                            </td>
                                            <td>
                                            </td>
                                        </tr>
                                    </InsertItemTemplate>
                                    <EditItemTemplate>
                                        <tr>
                                            <td align="center">
                                                <asp:LinkButton ID="lblTransUpdate" CommandName="Update" Text="Update" runat="server"
                                                    Visible="true" ValidationGroup="Postingupdate" />
                                                <asp:LinkButton ID="lblTransCancel" CommandName="Cancel" runat="server" Text="Cancel"
                                                    Visible="true" />
                                            </td>
                                            <td align="center">
                                                <asp:Label ID="hdPostTransTypeId" Visible="false" runat="server" Text='<%# Bind("POST_TRANS_TYP_ID") %>'></asp:Label>
                                                <asp:Label ID="hdTransTypId" Visible="false" runat="server" Text='<%# Bind("TRNS_TYP_ID") %>'></asp:Label>
                                                <asp:DropDownList ID="ddlTransTypelist" runat="server" DataSourceID="TransTypeDataSource"
                                                    DataTextField="LookUpName" DataValueField="LookUpID" Width="100px" ValidationGroup="Postingupdate">
                                                </asp:DropDownList>
                                                <asp:CompareValidator ID="CompareContactTypelist" runat="server" ControlToValidate="ddlTransTypelist"
                                                    ErrorMessage="Transaction Type required"  Text="*" Operator="NotEqual" ValueToCompare="0" ValidationGroup="Postingupdate"></asp:CompareValidator>
                                            </td>
                                            <td align="center">
                                                <ajaxtoolkit:FilteredTextBoxExtender TargetControlID="txtTransName" FilterType="UppercaseLetters,Custom, LowercaseLetters, Numbers"
                                                    ValidChars=" ,&" ID="fteTransName" runat="server" />
                                                <asp:TextBox ID="txtTransName" runat="server" Width="200px" MaxLength="100" Text='<%# Bind("TRANS_TXT") %>' />
                                            </td>
                                            <td align="center">
                                                <ajaxtoolkit:MaskedEditExtender ID="MainTextBoxExtender" runat="server" Mask="9999"
                                                    MaskType="Number" TargetControlID="txtMain" InputDirection="RightToLeft" AutoComplete="false">
                                                </ajaxtoolkit:MaskedEditExtender>
                                                <asp:TextBox ID="txtMain" Width="40px" runat="server" MaxLength="4" Text='<%# Bind("MAIN_NBR") %>' />
                                            </td>
                                            <td align="center">
                                                <ajaxtoolkit:MaskedEditExtender ID="SubTextBoxExtender" runat="server" Mask="9999"
                                                    MaskType="Number" TargetControlID="txtSub" InputDirection="RightToLeft" AutoComplete="false">
                                                </ajaxtoolkit:MaskedEditExtender>
                                                <asp:TextBox ID="txtSub" runat="server" Width="40px" MaxLength="4" Text='<%# Bind("SUB_NBR") %>' />
                                            </td>
                                            <td align="center">
                                                <ajaxtoolkit:FilteredTextBoxExtender ID="CompanyTextBoxExtender" runat="server" FilterType="Numbers,Custom,LowercaseLetters,UppercaseLetters"
                                                    TargetControlID="txtCompany" ValidChars=" ">
                                                </ajaxtoolkit:FilteredTextBoxExtender>
                                                <asp:TextBox ID="txtCompany" runat="server" Width="40px" MaxLength="4" Text='<%# Bind("COMP_TXT") %>' />
                                            </td>
                                            <td align="center">
                                                <asp:CheckBox ID="chkInv" Checked='<%# Bind("INVOICBL_IND") %>' runat="server" />
                                            </td>
                                            <td align="center">
                                                <asp:CheckBox ID="chkMisc" Checked='<%# Bind("MISC_POSTS_IND") %>' runat="server" />
                                            </td>
                                            <td align="center">
                                                <asp:CheckBox ID="chkTpa" Checked='<%# Bind("THRD_PTY_ADMIN_MNL_IND") %>' runat="server" />
                                            </td>
                                            <td align="center">
                                                <asp:CheckBox ID="chkAdjSummary" Checked='<%# Bind("ADJ_SUMRY_NOT_POST_IND") %>'
                                                    runat="server" />
                                            </td>
                                            <td align="center">
                                                <asp:CheckBox ID="ChkPolReq" Checked='<%# Bind("POL_REQR_IND") %>' runat="server" />
                                            </td>
                                            <td>
                                            </td>
                                        </tr>
                                    </EditItemTemplate>
                                </asp:AISListView>
                            </asp:Panel>
                        </ContentTemplate>
                    </ajaxtoolkit:TabPanel>
                    <ajaxtoolkit:TabPanel runat="server" ID="tpIssueMaint">
                        <HeaderTemplate>
                            Issue &amp; CheckList Maintenance
                        </HeaderTemplate>
                        <ContentTemplate>
                            <asp:UpdatePanel ID="upIssueMaint" runat="server">
                                <ContentTemplate>
                                    <br />
                                    <table width="910" style="text-align: right">
                                        <tr>
                                            <td align="left">
                                                <asp:Label ID="lblIssueHeader" runat="server" Font-Bold="True" Text="Issue & CheckList Maintenance"
                                                    CssClass="h3"></asp:Label>
                                                &nbsp;
                                            </td>
                                        </tr>
                                    </table>
                                    <asp:Panel ID="Panel1" runat="server" Height="200px" ScrollBars="Auto" Width="500px">
                                        <asp:AISListView ID="lstIssues" runat="server" DataKeyNames="LookUpID" OnItemDataBound="IssuesDataBoundList"
                                            OnSelectedIndexChanging="lstIssueType_SelectedIndexChanging">
                                            <LayoutTemplate>
                                                <table id="Table1" class="panelContents" runat="server" width="98%">
                                                    <tr class="LayoutTemplate">
                                                        <th>
                                                            Issue Type
                                                        </th>
                                                        <th>
                                                            Issues
                                                        </th>
                                                    </tr>
                                                    <tr id="itemPlaceholder" runat="server">
                                                    </tr>
                                                </table>
                                            </LayoutTemplate>
                                            <AlternatingItemTemplate>
                                                <tr class="AlternatingItemTemplate" id="trItemTemplate" runat="server">
                                                    <td align="left">
                                                        <asp:Label ID="lblIssueCategoryName" runat="server" Text='<%# Bind("LookUpName") %>'></asp:Label>
                                                    </td>
                                                    <td align="center">
                                                        <asp:LinkButton ID="lnkSelect" Enabled='<%# Eval("ACTIVE")%>' CommandArgument='<%# Bind("LookUpID") %>'
                                                            CommandName="Select" runat="server" Text="Details"></asp:LinkButton>
                                                    </td>
                                                </tr>
                                            </AlternatingItemTemplate>
                                            <ItemTemplate>
                                                <tr class="ItemTemplate" id="trItemTemplate" runat="server">
                                                    <td align="left">
                                                        <asp:Label ID="lblIssueCategoryName" runat="server" Text='<%# Bind("LookUpName") %>'></asp:Label>
                                                    </td>
                                                    <td align="center">
                                                        <asp:LinkButton ID="lnkSelect" Enabled='<%# Eval("ACTIVE")%>' runat="server" CommandArgument='<%# Bind("LookUpID") %>'
                                                            CommandName="Select" Text="Details"></asp:LinkButton>
                                                    </td>
                                                </tr>
                                            </ItemTemplate>
                                        </asp:AISListView>
                                    </asp:Panel>
                                    <br />
                                    <div runat="server" id="divIssueDetails" visible="False">
                                        <asp:Label ID="lblIssueDetails" Font-Bold="True" runat="server" CssClass="h3"></asp:Label>
                                        &nbsp;&nbsp;
                                        <asp:LinkButton ID="lnkIssueDetailsClose" runat="server" OnClick="lnkIssueDetailsClose_Click">Close</asp:LinkButton>
                                    </div>
                                    <asp:Panel ID="pnlIssueDtailsList" Visible="False" runat="server" Height="150px" ScrollBars="Auto"
                                        Width="910px">
                                        <asp:AISListView ID="lstIssueDetails" runat="server" InsertItemPosition="FirstItem"
                                            DataKeyNames="IssCatgID" OnItemCommand="IssuDetailsCommandList" OnItemDataBound="IssuDetailsDataBoundList"
                                            OnItemEditing="IssuDetailsEditList" OnItemUpdating="IssuDetailsUpdateList" OnItemCanceling="IssuDetailsCancelList"
                                            OnItemInserting="InsertList">
                                            <LayoutTemplate>
                                                <table id="Table1" class="panelContents" runat="server" width="98%">
                                                    <tr class="LayoutTemplate">
                                                        <th>
                                                        </th>
                                                        <th>
                                                            Issue Name
                                                        </th>
                                                        <th>
                                                            Sort Order
                                                        </th>
                                                        <th>
                                                            Financial
                                                        </th>
                                                        <th>
                                                            Disable
                                                        </th>
                                                    </tr>
                                                    <tr id="itemPlaceholder" runat="server">
                                                    </tr>
                                                </table>
                                            </LayoutTemplate>
                                            <AlternatingItemTemplate>
                                                <tr class="AlternatingItemTemplate" id="trItemTemplate" runat="server">
                                                    <td align="left">
                                                        <asp:LinkButton Enabled='<%# Eval("ACTIVE")%>' ID="lnkEdit" runat="server" CommandName="Edit"
                                                            Text="EDIT"></asp:LinkButton>
                                                    </td>
                                                    <td align="left">
                                                        <asp:Label ID="lblIssueText" runat="server" Text='<%# Bind("IssueText") %>'></asp:Label>
                                                    </td>
                                                     <td align="center">
                                                        <asp:Label ID="lblStrNbr" runat="server" Text='<%# Bind("Str_Nbr") %>'></asp:Label>
                                                    </td>
                                                    <td align="center">
                                                        <asp:CheckBox ID="chkFinancial" runat="server" Enabled="false" Checked='<%# Bind("FinancialIndicator") %>'></asp:CheckBox>
                                                    </td>
                                                    <td>
                                                        <asp:HiddenField ID="hidActive" runat="server" Value='<%# Bind("ACTIVE")%>' />
                                                        <asp:ImageButton ID="imgDisable" CommandArgument='<%# Bind("QualityCntrlMstrIsslstID") %>' runat="server"
                                                            ImageUrl='<%# Eval("ACTIVE").ToString()=="True"?"~/images/disabled.GIF":"~/images/enabled.GIF" %>'>
                                                        </asp:ImageButton>
                                                    </td>
                                                </tr>
                                            </AlternatingItemTemplate>
                                            <EditItemTemplate>
                                                <tr class="ItemTemplate">
                                                    <td align="left">
                                                        <asp:LinkButton ID="lnkUpdate" runat="server" CommandArgument='<%# Bind("QualityCntrlMstrIsslstID") %>'
                                                            CommandName="Update" Text="UPDATE" ValidationGroup="updateLkup"></asp:LinkButton>
                                                        <asp:LinkButton ID="lnkCancel" runat="server" CommandName="Cancel" Text="CANCEL"></asp:LinkButton>
                                                    </td>
                                                    <td align="left">
                                                        <asp:HiddenField ID="hidEditQcIssuID" runat="server" Value='<%# Bind("QualityCntrlMstrIsslstID") %>' />
                                                        <asp:TextBox ID="txtEditIssueName" runat="server" Text='<%# Bind("IssueText") %>'
                                                            ValidationGroup="updateLkup" MaxLength="256" Width="600px"></asp:TextBox>
                                                        <asp:RequiredFieldValidator ID="reqEditlkupTypeName" runat="server" ControlToValidate="txtEditIssueName"
                                                            ErrorMessage="Please enter Issue name" Text="*" ValidationGroup="updateLkup"></asp:RequiredFieldValidator>
                                                        <ajaxtoolkit:FilteredTextBoxExtender TargetControlID="txtEditIssueName" FilterType="UppercaseLetters,Custom, LowercaseLetters, Numbers"
                                                            ValidChars="  ~!@#$%^&*()_+<>,./?|\-:;{}[]" ID="fltrEditLktypeName" runat="server" /> 
                                                    </td>
                                                     <td align="center">
                                                        <asp:TextBox ID="txtEditStrNbr" runat="server" Text='<%# Bind("Str_Nbr") %>'
                                                            ValidationGroup="updateLkup" MaxLength="10" Width="30px"></asp:TextBox>
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtEditStrNbr"
                                                            ErrorMessage="Please enter Sort number" Text="*" ValidationGroup="updateLkup"></asp:RequiredFieldValidator>
                                                        <ajaxtoolkit:FilteredTextBoxExtender TargetControlID="txtEditStrNbr" FilterType="Numbers"
                                                            ID="FilteredTextBoxExtender1" runat="server" /> 
                                                    </td>
                                                    <td align="center">
                                                        <asp:CheckBox ID="chkFinInd" runat="server" Checked='<%# Bind("FinancialIndicator") %>'>
                                                        </asp:CheckBox>
                                                    </td>
                                                    <td>
                                                    </td>
                                                </tr>
                                            </EditItemTemplate>
                                            <InsertItemTemplate>
                                                <tr class="AlternatingItemTemplate">
                                                    <td align="left">
                                                        <asp:LinkButton ID="lnkSave" runat="server" CommandName="Save" Text="SAVE" ValidationGroup="SaveLkup"></asp:LinkButton>
                                                    </td>
                                                    <td align="left">
                                                        <asp:TextBox ID="txtIssueName" MaxLength="256" runat="server" ValidationGroup="SaveLkup" Width="600px"></asp:TextBox>
                                                        <asp:RequiredFieldValidator ID="reqIssueName" runat="server" ControlToValidate="txtIssueName"
                                                            ErrorMessage="Please enter Issue name" Text="*" ValidationGroup="SaveLkup"></asp:RequiredFieldValidator>
                                                        <ajaxtoolkit:FilteredTextBoxExtender TargetControlID="txtIssueName" FilterType="UppercaseLetters,Custom, LowercaseLetters, Numbers"
                                                            ValidChars=" ~!@#$%^&*()_+<>,./?|\-:;{}[]" ID="fltrIssueName" runat="server" />
                                                    </td>
                                                    <td align="center">
                                                        <asp:TextBox ID="txtStrNbr" runat="server" 
                                                            ValidationGroup="SaveLkup" MaxLength="10" Width="30px"></asp:TextBox>
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtStrNbr"
                                                            ErrorMessage="Please enter Sort number" Text="*" ValidationGroup="SaveLkup"></asp:RequiredFieldValidator>
                                                        <ajaxtoolkit:FilteredTextBoxExtender TargetControlID="txtStrNbr" FilterType="Numbers"
                                                            ID="FilteredTextBoxExtender1" runat="server" /> 
                                                    </td>
                                                    <td align="center">
                                                        <asp:CheckBox ID="chkFinInd" runat="server"  >
                                                        </asp:CheckBox>
                                                    </td>
                                                    <td>
                                                    </td>
                                                </tr>
                                            </InsertItemTemplate>
                                            <ItemTemplate>
                                                <tr class="ItemTemplate" id="trItemTemplate" runat="server">
                                                    <td align="left">
                                                        <asp:LinkButton ID="lnkitemEdit" Enabled='<%# Eval("ACTIVE")%>' runat="server" CommandName="Edit"
                                                            Text="EDIT"></asp:LinkButton>
                                                    </td>
                                                    <td align="left">
                                                        <asp:Label ID="lblIssueName" runat="server" Text='<%# Bind("IssueText") %>'></asp:Label>
                                                    </td>
                                                    <td align="center">
                                                        <asp:Label ID="lblStrNbr" runat="server" Text='<%# Bind("Str_Nbr") %>'></asp:Label>
                                                    </td>
                                                    <td align="center">
                                                        <asp:CheckBox ID="chkFinanInd" runat="server" Enabled="false"
                                                         Checked='<%# Bind("FinancialIndicator") %>'></asp:CheckBox>
                                                    </td>
                                                    <td>
                                                        <asp:HiddenField ID="hidActive" runat="server" Value='<%# Bind("ACTIVE")%>' />
                                                        <asp:ImageButton ID="imgDisable" CommandArgument='<%# Bind("QualityCntrlMstrIsslstID") %>' runat="server"
                                                            ImageUrl='<%# Eval("ACTIVE").ToString()=="True"?"~/images/disabled.GIF":"~/images/enabled.GIF" %>'>
                                                        </asp:ImageButton>
                                                    </td>
                                                </tr>
                                            </ItemTemplate>
                                        </asp:AISListView>
                                    </asp:Panel>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </ContentTemplate>
                    </ajaxtoolkit:TabPanel>
                    <ajaxtoolkit:TabPanel runat="server" ID="tblpnlBUOFFICE">
                        <HeaderTemplate>
                            <div>
                                BU/Office Maintenance</div>
                        </HeaderTemplate>
                        <ContentTemplate>
                            <br />
                            <br />
                            <asp:Label ID="Label1" runat="server" CssClass="h2" Text="BU/Office Maintenance"></asp:Label>
                            <br />
                            <br />
                            <asp:Panel ID="pnlBUOffice" Height="300px" Width="500px" runat="server" ScrollBars="Auto">
                                <asp:AISListView ID="lstBUOffice" runat="server" InsertItemPosition="FirstItem"
                                DataKeyNames="INTRNL_ORG_ID" OnItemDataBound="lstBUOffice_DataBoundList" OnItemEditing="lstBUOffice_ItemEdit"
                                OnItemCanceling="lstBUOffice_ItemCancel" OnItemCommand="lstBUOffice_ItemCommand" OnItemInserting="lstBUOffice_ItemInserting" 
                                OnItemUpdating="lstBUOffice_ItemUpdate" >
                                    <LayoutTemplate>
                                        <table id="Table1" class="panelContents" runat="server">
                                            <tr class="LayoutTemplate">
                                                <th>
                                                </th>
                                                <th style="width:150px">
                                                    Business Unit Name
                                                </th>
                                                <th>
                                                    Business Unit Code
                                                </th>
                                                <th>
                                                    City Name
                                                </th>
                                                <th>
                                                    Office Code
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
                                        <tr id="trItemTemplate" runat="server" class="ItemTemplate">
                                            <td>
                                                <asp:LinkButton ID="lbBUEdit" Enabled='<%# Eval("ACTV_IND")%>' CommandName="Edit" Text="Edit" runat="server" Visible="true" />
                                            </td>
                                            <td>
                                                <asp:Label ID="lblBUName" runat="server" Text='<%# Bind("FULL_NAME") %>'></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="lblBUCode" runat="server" Text='<%# Bind("BSN_UNT_CD") %>' />
                                            </td>
                                            <td>
                                                <asp:Label ID="lblMain" runat="server" Text='<%# Bind("CITY_NM") %>' />
                                            </td>
                                            <td>
                                                <asp:Label ID="lblSub" runat="server" Text='<%# Bind("OFC_CD") %>' />
                                            </td>
                                            <td>
                                                <asp:Label ID="lblActive" runat="server" Text='<%# Bind("ACTV_IND") %>' Visible="false"></asp:Label>
                                                <asp:ImageButton ID="ibtnEnableDisable" runat="server" CommandArgument='<%# Bind("INTRNL_ORG_ID") %>'
                                                 ImageUrl='<%# Eval("ACTV_IND").ToString()=="True"?"~/images/disabled.GIF":"~/images/enabled.GIF" %>' />
                                            </td>
                                        </tr>
                                    </ItemTemplate>
                                    <AlternatingItemTemplate>
                                        <tr id="trItemTemplate" runat="server" class="AlternatingItemTemplate">
                                            <td>
                                                <asp:LinkButton ID="lbBUEdit" Enabled='<%# Eval("ACTV_IND")%>' CommandName="Edit" Text="Edit" runat="server" Visible="true" />
                                            </td>
                                            <td>
                                                <asp:Label ID="lblBUName" runat="server" Text='<%# Bind("FULL_NAME") %>'></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="lblBUCode" runat="server" Text='<%# Bind("BSN_UNT_CD") %>' />
                                            </td>
                                            <td>
                                                <asp:Label ID="lblMain" runat="server" Text='<%# Bind("CITY_NM") %>' />
                                            </td>
                                            <td>
                                                <asp:Label ID="lblSub" runat="server" Text='<%# Bind("OFC_CD") %>' />
                                            </td>
                                            <td>
                                                <asp:Label ID="lblActive" runat="server" Text='<%# Bind("ACTV_IND") %>' Visible="false"></asp:Label>
                                                <asp:ImageButton ID="ibtnEnableDisable" runat="server" CommandArgument='<%# Bind("INTRNL_ORG_ID") %>' 
                                                ImageUrl='<%# Eval("ACTV_IND").ToString()=="True"?"~/images/disabled.GIF":"~/images/enabled.GIF" %>'
                                                />
                                            </td>
                                        </tr>
                                    </AlternatingItemTemplate>
                                    <InsertItemTemplate>
                                        <tr id="tr2" runat="server" class="AlternatingItemTemplate">
                                            <td align="center">
                                                <asp:LinkButton ID="lblBUInsert" CommandName="Save" Text="Save" runat="server"
                                                    Visible="true" ValidationGroup="BUSave" />
                                            </td>
                                            <td>
                                                <ajaxtoolkit:FilteredTextBoxExtender TargetControlID="txtBUOfficeName" FilterType="UppercaseLetters,Custom, LowercaseLetters, Numbers"
                                                    ValidChars=" " ID="fteTransName" runat="server" />
                                                <asp:TextBox ID="txtBUOfficeName" MaxLength="50" runat="server"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="rfvBUOfficeName" ValidationGroup="BUSave" 
                                                ControlToValidate="txtBUOfficeName" ErrorMessage="Please enter BU/Office Name" runat="server">*</asp:RequiredFieldValidator>
                                            </td>
                                            <td>
                                                <ajaxtoolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server" FilterType="Numbers,LowercaseLetters,UppercaseLetters"
                                                    TargetControlID="txtBUUntCD" >
                                                </ajaxtoolkit:FilteredTextBoxExtender>
                                                <asp:TextBox ID="txtBUUntCD" runat="server" Width="40px" MaxLength="3" Text="" />
                                                <asp:RequiredFieldValidator ID="rfvBUUntCd" ValidationGroup="BUSave" 
                                                ControlToValidate="txtBUUntCD" ErrorMessage="Please enter Business Unit Code" runat="server">*</asp:RequiredFieldValidator>
                                            </td>
                                            <td>
                                                <ajaxtoolkit:FilteredTextBoxExtender TargetControlID="txtCityName" FilterType="UppercaseLetters,Custom, LowercaseLetters, Numbers"
                                                    ValidChars=" " ID="FilteredTextBoxExtender2" runat="server" />
                                                <asp:TextBox ID="txtCityName" runat="server" MaxLength="50" Text="" />
                                            </td>
                                            <td>
                                                <ajaxtoolkit:FilteredTextBoxExtender ID="CompanyTextBoxExtender" runat="server" FilterType="Numbers,LowercaseLetters,UppercaseLetters"
                                                    TargetControlID="txtOfcCd" >
                                                </ajaxtoolkit:FilteredTextBoxExtender>
                                                <asp:TextBox ID="txtOfcCd" runat="server" Width="40px" MaxLength="3" Text="" />
                                            </td>
                                            <td>
                                            </td>
                                        </tr>
                                    </InsertItemTemplate>
                                    <EditItemTemplate>
                                        <tr>
                                            <td align="center">
                                                <asp:LinkButton ID="lblBUUpdate" CommandName="Update" Text="Update" runat="server"
                                                    Visible="true" ValidationGroup="BUupdate" />
                                                <asp:LinkButton ID="lblBUCancel" CommandName="Cancel" runat="server" Text="Cancel"
                                                    Visible="true" />
                                            </td>
                                            <td align="center">
                                                <asp:Label ID="hdBusinessUnitOfficeId" Visible="false" runat="server" Text='<%# Bind("INTRNL_ORG_ID") %>'></asp:Label>
                                                <ajaxtoolkit:FilteredTextBoxExtender TargetControlID="txtBUOfficeName" FilterType="UppercaseLetters,Custom, LowercaseLetters, Numbers"
                                                    ValidChars=" " ID="fteTransName" runat="server" />
                                                <asp:TextBox ID="txtBUOfficeName" runat="server" MaxLength="50" Text='<%# Bind("FULL_NAME") %>' />
                                                <asp:RequiredFieldValidator ID="rfvBUOfficeName" ValidationGroup="BUUpdate" 
                                                ControlToValidate="txtBUOfficeName" ErrorMessage="Please enter BU/Office Name" runat="server">*</asp:RequiredFieldValidator>
                                            </td>
                                            <td align="center">
                                                <ajaxtoolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server" FilterType="Numbers,LowercaseLetters,UppercaseLetters"
                                                    TargetControlID="txtBUUntCD" >
                                                </ajaxtoolkit:FilteredTextBoxExtender>
                                                <asp:TextBox ID="txtBUUntCD" Width="40px" runat="server" MaxLength="3" Text='<%# Bind("BSN_UNT_CD") %>' />
                                                <asp:RequiredFieldValidator ID="rfvBUUntCd" ValidationGroup="BUUpdate" 
                                                ControlToValidate="txtBUUntCD" ErrorMessage="Please enter Business Unit Code" runat="server">*</asp:RequiredFieldValidator>
                                            </td>
                                            <td align="center">
                                                <ajaxtoolkit:FilteredTextBoxExtender TargetControlID="txtCityName" FilterType="UppercaseLetters,Custom, LowercaseLetters, Numbers"
                                                    ValidChars=" " ID="FilteredTextBoxExtender2" runat="server" />
                                                <asp:TextBox ID="txtCityName" runat="server" MaxLength="50" Text='<%# Bind("CITY_NM") %>' />
                                            </td>
                                            <td align="center">
                                                <ajaxtoolkit:FilteredTextBoxExtender ID="CompanyTextBoxExtender" runat="server" FilterType="Numbers,LowercaseLetters,UppercaseLetters"
                                                    TargetControlID="txtOfcCd" >
                                                </ajaxtoolkit:FilteredTextBoxExtender>
                                                <asp:TextBox ID="txtOfcCd" runat="server" Width="40px" MaxLength="3" Text='<%# Bind("OFC_CD") %>' />
                                            </td>
                                            <td>
                                            </td>
                                        </tr>
                                    </EditItemTemplate>
                                </asp:AISListView>
                            </asp:Panel>
                        </ContentTemplate>
                    </ajaxtoolkit:TabPanel>
                    
                </ajaxtoolkit:TabContainer>
            </td>
        </tr>
    </table>
</asp:Content>
