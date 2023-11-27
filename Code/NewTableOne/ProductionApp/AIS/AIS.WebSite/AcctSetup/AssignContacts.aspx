<%@ Page Language="C#" MasterPageFile="~/Retro.master" AutoEventWireup="true" Inherits="AcctSetup_AssignContacts"
    Title="Untitled Page" CodeBehind="AssignContacts.aspx.cs" %>

<%@ Register Src="~/App_Shared/AccountInfoHeader.ascx" TagName="MasterValues" TagPrefix="MV" %>
<%@ Register Src="~/App_Shared/ProgramPeriod.ascx" TagName="ProgramPeriod" TagPrefix="PP" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeaderPlaceHolder" runat="Server">
    
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="MainPlaceHolder" runat="Server" >
 <script language="javascript" type="text/javascript">

    var scrollTop1;
    
    if (Sys.WebForms.PageRequestManager != null)
    {
        Sys.WebForms.PageRequestManager.getInstance().add_beginRequest(BeginRequestHandler);
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);
    }

    function BeginRequestHandler(sender, args) 
    {
        var mef = $get('<%=panContactInfo.ClientID%>');
        if(mef!=null)
        scrollTop1 = mef.scrollTop;
    }

    function EndRequestHandler(sender, args)
    {
        var mef = $get('<%=panContactInfo.ClientID%>');
        if(mef!=null)
        mef.scrollTop = scrollTop1;
    } 
</script>
<asp:Label ID="lblHeader" runat="server" Text="Contact Assignment" CssClass="h1"></asp:Label>
    <asp:ValidationSummary ID="valsEdit" ValidationGroup="EditGroup" runat="server">
    </asp:ValidationSummary>
    <asp:ValidationSummary ID="valsInsert" ValidationGroup="InsertGroup" runat="server">
    </asp:ValidationSummary>
    <MV:MasterValues ID="UcMastervalues" runat="server" />
    
    <asp:Label ID="lblProgramPeriodsHeader" runat="server" CssClass="h2" Text="Program Periods" style="padding-top :5"></asp:Label>
    <asp:ObjectDataSource ID="ContactNamesDataSource" runat="server" SelectMethod="getPersonNames"
        TypeName="ZurichNA.AIS.Business.Logic.AssignContactsBS">
        <SelectParameters>
            <asp:Parameter DefaultValue="0" Name="ContTypId" Type="Int32" />
            <asp:Parameter DefaultValue="0" Name="CustmrID" Type="Int32" />
        </SelectParameters>
    </asp:ObjectDataSource>
    <asp:ObjectDataSource ID="ContactTypeDataSource" runat="server" SelectMethod="GetLookUpActiveData"
        TypeName="ZurichNA.AIS.Business.Logic.BLAccess">
        <SelectParameters>
            <asp:Parameter DefaultValue="CONTACT TYPE" Name="lookUpTypeName" Type="String" />
        </SelectParameters>
    </asp:ObjectDataSource>
    <asp:ObjectDataSource ID="DelMethodDataSource" runat="server" SelectMethod="GetLookUpActiveData"
        TypeName="ZurichNA.AIS.Business.Logic.BLAccess">
        <SelectParameters>
            <asp:Parameter DefaultValue="DELIVERY METHOD" Name="lookUpTypeName" Type="String" />
        </SelectParameters>
    </asp:ObjectDataSource>
    <div class="content" style="width: 1000px; overflow: auto" >
        <asp:Panel ID="PnlGridView" BorderWidth="0" Width="910px" Height="150px" runat="server" ScrollBars ="Auto" >
            <asp:UpdatePanel ID="upGridView" runat="server">
                <ContentTemplate>
                    <PP:ProgramPeriod ID="ProgramPeriod" runat="server" OnOnItemCommand="ProgramPeriod_ItemCommand" />
                </ContentTemplate>
            </asp:UpdatePanel>
        </asp:Panel> 
    </div>
    
    <table>
        <tr>
            <td>
                <asp:Label ID="lblCustomerContactInfo" runat="server" CssClass="h2" Visible="false"
                    Text="Customer Contact Information - "></asp:Label>
            </td>
            <td>
                <asp:Label ID="lblEffdt" runat="server" CssClass="h2" Text=""></asp:Label>
            </td>
            <td>
                <asp:Label ID="lblExpdt" runat="server" CssClass="h2" Text=""></asp:Label>
            </td>
            <td>
                <asp:Label ID="lblProgramPeriodId" Width="100px" Visible="false" runat="server" Text=""></asp:Label>
            </td>
            <td width="100px" align="right">
                <asp:LinkButton ID="lbClose" runat="server" Visible="false" Text="Close" OnClick="CloseContactInfo"></asp:LinkButton>
            </td>
        </tr>
    </table>
    
    
    <asp:Panel ID="panContactInfo" runat="server" Height="200px" Width="910px" ScrollBars="Auto"
        Visible="true">
        <asp:AISListView ID="LstContacts" runat="server" InsertItemPosition="FirstItem" OnItemEditing="LstContacts_ItemEdit"
            OnItemDataBound="LstContacts_DataBoundList" OnItemCanceling="LstContacts_ItemCancel"
            OnItemCommand="LstContacts_ItemCommand" OnItemInserting="LstContacts_ItemInserting" OnItemUpdating="LstContacts_ItemUpdating"
            >
            <LayoutTemplate>
                <table id="Table1" class="panelContents" runat="server" width = "98%">
                    <tr class="LayoutTemplate">
                        <th>
                        </th>
                        <th>
                            Contact Type
                        </th>
                        <th>
                            Contact
                        </th>
                        <th>
                            Delivery Method
                        </th>
                        <th>
                            Send Invoice?
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
                <tr id="Tr1" runat="server" class="ItemTemplate">
                    <td>
                        <asp:LinkButton ID="lbContactEdit" CommandName="Edit" Text="Edit" runat="server"
                            Visible="true" Width="100px"  Enabled='<%# Bind("ACTV_IND") %>'  />
                    </td>
                    <td>
                        <asp:Label ID="lblContactType" Width="100px" runat="server" Text='<%# Bind("ContTyp") %>'></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="lblContactName" Width="100px" runat="server" Text='<%# Bind("FullName") %>'></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="lblDeliveryMethod" Width="100px" runat="server" Text='<%# Bind("ComTypText") %>'></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="lblSendInvoice" Width="100px" runat="server" Text='<%# Bind("SND_INVC_IND_txt") %>'></asp:Label>
                    </td>
                    <td>
                     
                    <asp:Label ID="lblActive" runat="server" Text='<%# Bind("ACTV_IND") %>' visible="false"></asp:Label>
                    <asp:ImageButton ID="ibtnEnableDisable" runat="server" CommandArgument='<%# Bind("PREM_ADJ_PGM_PERS_REL_ID") %>' />  
                    
                    </td>
                </tr>
            </ItemTemplate>
            <AlternatingItemTemplate>
                <tr id="Tr2" runat="server" class="AlternatingItemTemplate">
                    <td>
                        <asp:LinkButton ID="lbtnContactEdit" CommandName="Edit" Text="Edit" runat="server"
                            Enabled='<%# Bind("ACTV_IND") %>' Visible="true" Width="100px" />
                    </td>
                    <td>
                        <asp:Label ID="lblContactType" Width="100px" runat="server" Text='<%# Bind("ContTyp") %>'></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="lblContactName" Width="100px" runat="server" Text='<%# Bind("FullName") %>'></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="lblDeliveryMethod" Width="100px" runat="server" Text='<%# Bind("ComTypText") %>'> </asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="lblSendInvoice" Width="100px" runat="server" Text='<%# Bind("SND_INVC_IND_txt") %>'> </asp:Label>
                    </td>
                    <td>
                    <asp:Label ID="lblActive" runat="server" Text='<%# Bind("ACTV_IND") %>' visible="false"></asp:Label>
                    <asp:ImageButton ID="ibtnEnableDisable" runat="server" CommandArgument='<%# Bind("PREM_ADJ_PGM_PERS_REL_ID") %>' />  
                    </td>
                </tr>
            </AlternatingItemTemplate>
            <EditItemTemplate>
                <tr>
                    <td align="center">
                        <asp:LinkButton ID="lbtnContactUpdate" CommandName="Update" Text="Update" runat="server"
                            Visible="true" Width="50px" ValidationGroup="EditGroup" />
                        <asp:LinkButton ID="lbtnContactCancel" CommandName="Cancel" runat="server" Text="Cancel"
                            Visible="true" Width="50px" />
                    </td>
                    <td align="center">
                        <asp:Label ID="lblhdPreAdjPgmRelId" Width="100px" Visible="false" runat="server" Text='<%# Bind("PREM_ADJ_PGM_PERS_REL_ID") %>'></asp:Label>
                        <asp:Label ID="lblContactID" Width="100px" Visible="false" runat="server" Text=""></asp:Label>
                        <asp:Label ID="lblContactNameId" Width="100px" Visible="false" runat="server" Text='<%# Bind("PERS_ID")%>'></asp:Label>
                        <asp:Label ID="lblContactTypeID" Width="100px" Visible="false" runat="server" Text='<%# Bind("ContTypID")%>'></asp:Label>
                        <asp:Label ID="lblDelMethodID" Width="100px" Visible="false" runat="server" Text='<%# Bind("COMMU_MEDUM_ID") %>'></asp:Label>
                        <asp:DropDownList ID="ddlContactType" runat="server" DataSourceID="ContactTypeDataSource"
                            AutoPostBack="true" OnSelectedIndexChanged="ddlContactType_OnSelectedIndexChanged"
                            DataTextField="LookUpName" DataValueField="LookUpID" Width="100px">
                        </asp:DropDownList>
                        <asp:CompareValidator ID="valContactType" runat="server" ControlToValidate="ddlContactType"
                            ErrorMessage="Please Select Contact Type" Text ="*" Operator="NotEqual" ValueToCompare="0" ValidationGroup="EditGroup"></asp:CompareValidator>
                    </td>
                    <td align="center">
                        <asp:DropDownList ID="ddlContactName" runat="server" Width="140px" DataSourceID="ContactNamesDataSource"
                            DataTextField="FULLNAME" DataValueField="PERSON_ID">
                        </asp:DropDownList>
                        <asp:CompareValidator ID="valContactName" runat="server" ControlToValidate="ddlContactName"
                            ErrorMessage="Please Select Contact Name" Text ="*" Operator="NotEqual" ValueToCompare="0" ValidationGroup="EditGroup"></asp:CompareValidator>
                    </td>
                    <td align="center">
                        <asp:DropDownList ID="ddlDelMethod" runat="server" DataSourceID="DelMethodDataSource"
                            DataTextField="LookUpName" DataValueField="LookUpID" Width="100px">
                        </asp:DropDownList>
                        
                    </td>
                    <td align="center">
                        <asp:CheckBox ID="chkSendInv" runat="server" Checked='<%# Bind("SND_INVC_IND") %>' />
                    </td>
                </tr>
            </EditItemTemplate>
            <InsertItemTemplate>
                <tr class="AlternatingItemTemplate">
                    <td align="center">
                        <asp:LinkButton ID="lblContactInsert" CommandName="Save" Text="Save" runat="server"
                            Visible="true" Width="100px" ValidationGroup="InsertGroup" />
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlContactType" runat="server" DataSourceID="ContactTypeDataSource"
                            DataTextField="LookUpName" DataValueField="LookUpID" Width="100px" AutoPostBack="true"
                            OnSelectedIndexChanged="ddlContactType_OnSelectedIndexChanged">
                        </asp:DropDownList>
                        <asp:CompareValidator ID="valContactType" runat="server" ControlToValidate="ddlContactType"
                            ErrorMessage="Please Select ContactType" Text ="*" Operator="NotEqual" ValueToCompare="0" ValidationGroup="InsertGroup"></asp:CompareValidator>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlContactName" runat="server" Width="140px" DataSourceID="ContactNamesDataSource"
                            DataTextField="FULLNAME" DataValueField="PERSON_ID">
                            
                        </asp:DropDownList>
                        <asp:CompareValidator ID="valContactNamelist" runat="server" ControlToValidate="ddlContactName"
                            ErrorMessage="Please Select Contact Name" Text ="*" Operator="NotEqual" ValueToCompare="0" ValidationGroup="InsertGroup"></asp:CompareValidator>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlDelMethod" runat="server" DataSourceID="DelMethodDataSource"
                            DataTextField="LookUpName" DataValueField="LookUpID" Width="100px">
                        </asp:DropDownList>
                        
                    </td>
                    <td align="center">
                        <asp:CheckBox ID="chkSendInv" runat="server" Text="" />
                    </td>
                    <td>
                    </td>
                </tr>
            </InsertItemTemplate>
        </asp:AISListView>
    </asp:Panel>
</asp:Content>
