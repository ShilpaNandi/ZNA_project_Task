<%@ Page Language="C#" MasterPageFile="~/Retro.master" AutoEventWireup="true" Inherits="ExtContacts"
    EnableEventValidation="false" CodeBehind="ExtContacts.aspx.cs" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="ConPageHeading" ContentPlaceHolderID="HeaderPlaceHolder" runat="Server">
    <table width="910" style="text-align: right">
        <tr>
            <td align="left">
                <asp:Label ID="GAILabel" runat="server" Text="External Contacts and Contacts Master Setup"
                    CssClass="h1"></asp:Label>
            </td>
            <td align="right">
                <asp:Button ID="btnAdd" runat="server" Text="Add New" ToolTip="Please click here to add new External Contacts"
                    OnClick="btnAdd_Click" Width="60px" />
            </td>
        </tr>
    </table>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainPlaceHolder" runat="Server">

    <script language="javascript" type="text/javascript">
    function summary()
{
$get('<%=VSSaveExternal.ClientID %>').style.BorderWidht="1px";
var hi=$get('<%=VSSaveExternal.ClientID %>');
hi.style.display='block';
}
    
    var scrollTop1;
    var scrollTop2;
      function ActiveTabChanged(sender, e)
    {
      __doPostBack('<%= tcExternalContacts.ClientID %>', sender.get_activeTab().get_headerText());
    }
    
    if (Sys.WebForms.PageRequestManager != null)
    {
        Sys.WebForms.PageRequestManager.getInstance().add_beginRequest(BeginRequestHandler);
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);
    }
    
    function BeginRequestHandler(sender, args) 
    {
        var mef = $get('<%=pnlExtContactsList.ClientID%>');
        if(mef!=null)
        scrollTop1 = mef.scrollTop;

        var pnl = $get('<%=pnlContactNames.ClientID%>');
        if(pnl!=null)
        scrollTop2 = pnl.scrollTop;
    }

    function EndRequestHandler(sender, args)
    {
        var mef = $get('<%=pnlExtContactsList.ClientID%>');
        if(mef!=null)
        mef.scrollTop = scrollTop1;

        var pnl = $get('<%=pnlContactNames.ClientID%>');
        if(pnl!=null)
        pnl.scrollTop = scrollTop2;
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
    </script>
    
    
    <table style="width: 100%">
        <tr>
            <td>
                <!--Datasource for Contact Type-->
                <asp:ObjectDataSource ID="ContactTypeDataSource" runat="server" SelectMethod="GetLookUpActiveData"
                    TypeName="ZurichNA.AIS.Business.Logic.BLAccess">
                    <SelectParameters>
                        <asp:Parameter DefaultValue="CONTACT TYPE" Name="lookUpTypeName" Type="String" />
                        <asp:Parameter DefaultValue="E" Name="attribute" Type="String" />
                    </SelectParameters>
                </asp:ObjectDataSource>
                <asp:ObjectDataSource ID="ContactTypeListDataSource" runat="server" SelectMethod="getExternalContactTypeLookUp"
                    TypeName="ZurichNA.AIS.Business.Logic.PersonBS"></asp:ObjectDataSource>
            </td>
        </tr>
        <tr>
            <td>
                <asp:UpdatePanel ID="udpExtContacts" runat="server">
                <ContentTemplate>                
                    <asp:ValidationSummary ID="VSEditContacts" runat="server" ValidationGroup="ContactEditGroup"/>
                    <asp:ValidationSummary ID="VSSaveContacts"  runat="server" ValidationGroup="ContactSaveGroup"/>
                    <asp:ValidationSummary ID="VSSaveExternal" runat="server" ValidationGroup="ExternalSaveGroup"/>
                    <asp:ValidationSummary ID="valSearch" runat="server" ValidationGroup="Search"/>
                <!--Tab Container-->
                <cc1:TabContainer ID="tcExternalContacts" runat="server" ActiveTabIndex="0" CssClass="CustomTabs"
                    Width="100%" OnActiveTabChanged="LoadData" OnClientActiveTabChanged="ActiveTabChanged" >
                    <!--External Contacts Tab-->
                    <cc1:TabPanel runat="server" HeaderText="External Contacts" ID="tpExternalContacts"
                        Width="100%">
                        <HeaderTemplate>
                            <div >
                                External Contacts</div>
                        </HeaderTemplate>
                        <ContentTemplate>
                            <table>
                                <tr>
                                    <td>
                                        Contact Type
                                    </td>
                                    <td colspan="2">
                                        <asp:Label ID="lblType" runat="server" Text="Name"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:DropDownList ID="ddlExtContacts" runat="server" DataSourceID="ContactTypeDataSource"
                                            ValidationGroup="Search" DataTextField="LookUpName" DataValueField="LookUpID"
                                            AutoPostBack="true" Width="153px" OnSelectedIndexChanged="ddlExtContacts_Selected">
                                        </asp:DropDownList>
                                        <asp:CompareValidator ID="compDdlContact" runat="server" ControlToValidate="ddlExtContacts"
                                            ValueToCompare="0" ValidationGroup="Search" Text="*" ErrorMessage="Please select Contact Type"
                                            Operator="NotEqual"></asp:CompareValidator>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlContactNameSearch"  runat="server" ValidationGroup="Search"
                                             Width="253px">
                                        </asp:DropDownList>
                                        <asp:CompareValidator ID="compDdlName" runat="server" ControlToValidate="ddlContactNameSearch"
                                            ValueToCompare="0" ValidationGroup="Search" Text="*" ErrorMessage="Please select Name"
                                            Operator="NotEqual"></asp:CompareValidator>
                                    </td>
                                    <td>
                                        <asp:Button ID="btnSearch" ValidationGroup="Search" runat="server" Text="Search" OnClick="btnSearch_Click" />
                                    </td>
                                </tr>
                                <br />
                            </table>
                            <br />
                            <asp:Panel ID="pnlExtContacts" runat="server" CssClass="content">
                                        <asp:Panel ID="pnlExtContactsList" Width="900px" runat="server" CssClass="content"
                                            ScrollBars="Auto" Height="100px">
                                            <asp:AISListView ID="lstExtContact" runat="server" OnItemDataBound="lstExtContact_DataBoundList"
                                                OnSelectedIndexChanged="lstExtContact_SelectedIndexChanged" OnSelectedIndexChanging="lstExtContact_SelectedIndexChanging"
                                                OnItemCommand="lstExtContact_ItemCommand" >
                                                <EmptyDataTemplate>
                                                    <table id="lstExtTable" class="panelExtContents" runat="server" width="98%">
                                                        <tr class="LayoutTemplate">
                                                            <th>
                                                                Contact Type
                                                            </th>
                                                            <th>
                                                                Name
                                                            </th>
                                                            <th>
                                                                First Name
                                                            </th>
                                                            <th>
                                                                Last Name
                                                            </th>
                                                            <th>
                                                                City
                                                            </th>
                                                            <th>
                                                                State
                                                            </th>
                                                            <th>
                                                                Disable
                                                            </th>
                                                        </tr>
                                                        <tr id="ItemPlaceHolder" runat="server">
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
                                                    <table id="lstExtTable" class="panelExtContents" runat="server" width="98%">
                                                        <tr class="LayoutTemplate">
                                                            <th>
                                                            </th>
                                                            <th>
                                                                Contact Type
                                                            </th>
                                                            <th>
                                                                <%--<asp:LinkButton ID="SortByName" runat="server" CommandArgument="Name" CommandName="Sort"
                                                                    Text="Name" />
                                                                <asp:Image ID="imgSortByName" runat="server" ImageUrl="~/images/ascending.gif" ToolTip="ascending"
                                                                    Visible="false" />--%>
                                                                    Name
                                                            </th>
                                                            <th>
                                                                First Name
                                                            </th>
                                                            <th>
                                                                Last Name
                                                            </th>
                                                            <th>
                                                                City
                                                            </th>
                                                            <th>
                                                                State
                                                            </th>
                                                            <th>
                                                                Disable
                                                            </th>
                                                        </tr>
                                                        <tr id="ItemPlaceHolder" runat="server">
                                                        </tr>
                                                    </table>
                                                </LayoutTemplate>
                                                <AlternatingItemTemplate>
                                                    <tr id="trItemTemplate" runat="server" class="AlternatingItemTemplate">
                                                        <td>
                                                            <asp:LinkButton ID="lbContactSelect" CommandName="Select" Text="Select" runat="server"
                                                                Visible="true" Width="50px" />
                                                            <asp:Label ID="lblPersonID" Visible="false" runat="server" Text='<%# Bind("PERSON_ID") %>' />
                                                            <asp:Label ID="lblPostAddID" Visible="false" runat="server" Text='<%# Bind("selPOSTALADDRESSID") %>' />
                                                        </td>
                                                        <td>
                                                            <%# Eval("CONTACTTYPE") %>
                                                        </td>
                                                        <td>
                                                            <%# Eval("EXTERNAL_ORGN_TXT") %>
                                                        </td>
                                                        <td>
                                                            <%# Eval("FORENAME") %>
                                                        </td>
                                                        <td>
                                                            <%# Eval("SURNAME") %>
                                                        </td>
                                                        <td>
                                                            <%# Eval("CityName") %>
                                                        </td>
                                                        <td>
                                                            <%# Eval("StateName") %>
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="lblExtActive" runat="server" Text='<%# Bind("ACTIVE") %>' Visible="false"></asp:Label>
                                                            <asp:ImageButton ID="imgExtEnableDisable" runat="server" CommandArgument='<%# Bind("PERSON_ID") %>' />
                                                        </td>
                                                    </tr>
                                                </AlternatingItemTemplate>
                                                <ItemTemplate>
                                                    <tr id="trItemTemplate" runat="server" class="ItemTemplate">
                                                        <td>
                                                            <asp:LinkButton ID="lbContactSelect" runat="server" CommandName="Select" Text="Select"
                                                                Visible="true" Width="50px" />
                                                            <asp:Label ID="lblPersonID" runat="server" Text='<%# Bind("PERSON_ID") %>' Visible="false" />
                                                            <asp:Label ID="lblPostAddID" runat="server" Text='<%# Bind("selPOSTALADDRESSID") %>'
                                                                Visible="false" />
                                                        </td>
                                                        <td>
                                                            <%# Eval("CONTACTTYPE") %>
                                                        </td>
                                                        <td>
                                                            <%# Eval("EXTERNAL_ORGN_TXT") %>
                                                        </td>
                                                        <td>
                                                            <%# Eval("FORENAME") %>
                                                        </td>
                                                        <td>
                                                            <%# Eval("SURNAME") %>
                                                        </td>
                                                        <td>
                                                            <%# Eval("CityName") %>
                                                        </td>
                                                        <td>
                                                            <%# Eval("StateName") %>
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="lblExtActive" runat="server" Text='<%# Bind("ACTIVE") %>' Visible="false"></asp:Label>
                                                            <asp:ImageButton ID="imgExtEnableDisable" runat="server" CommandArgument='<%# Bind("PERSON_ID") %>' />
                                                        </td>
                                                    </tr>
                                                </ItemTemplate>
                                            </asp:AISListView>
                                        </asp:Panel>
                                        <br />
                                        <asp:Label ID="lblExternalContactsDetails" Visible="False" runat="server" Text="External Contacts Details"
                                            CssClass="h2"></asp:Label>
                                        &nbsp;
                                        <asp:LinkButton ID="lbCloseDetails" Text="Close" runat="server" CssClass="h2" OnClick="lbCloseDetails_Click"
                                            Visible="False" />
                                        <asp:Panel BorderColor="Black" ID="pnlDetails" Visible="False" BorderWidth="1px"
                                            Width="100%" runat="server">
                                            <table width="100%" cellpadding="0">
                                                <tr>
                                                    <br />
                                                </tr>
                                                <tr>
                                                    <td>
                                                        Contact Type:
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="ddlContactType" runat="server" DataSourceID="ContactTypeDataSource"
                                                            ValidationGroup="ExternalSaveGroup" DataTextField="LookUpName" DataValueField="LookUpID"
                                                            AutoPostBack="true" OnSelectedIndexChanged="ddlContactType_SelectedIndexChanged"
                                                            Width="253px">
                                                        </asp:DropDownList>
                                                        <asp:CompareValidator ID="CompareContactType" runat="server" ControlToValidate="ddlContactType"
                                                             ErrorMessage="Please Select Contact Type" Text="*" Operator="NotEqual"
                                                            ValueToCompare="0" ValidationGroup="ExternalSaveGroup"></asp:CompareValidator>
                                                    </td>
                                                    <td>
                                                        Name:
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="ddlContactNames" runat="server" Width="253px" ValidationGroup="ExternalSaveGroup">
                                                            <asp:ListItem Value="0">(Select)</asp:ListItem>
                                                        </asp:DropDownList>
                                                        <asp:CompareValidator ID="CompareContactNames" runat="server" ControlToValidate="ddlContactNames"
                                                             ErrorMessage="Please Select External Contact Name" Text="*"
                                                            Operator="NotEqual" ValueToCompare="0" ValidationGroup="ExternalSaveGroup"></asp:CompareValidator>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        Title:
                                                    </td>
                                                    <td>
                                                        <asp:ObjectDataSource ID="TitleDataSource" runat="server" SelectMethod="GetLookUpActiveData"
                                                            TypeName="ZurichNA.AIS.Business.Logic.BLAccess">
                                                            <SelectParameters>
                                                                <asp:Parameter DefaultValue="TITLE" Name="lookUpTypeName" Type="String" />
                                                            </SelectParameters>
                                                        </asp:ObjectDataSource>
                                                        <asp:DropDownList ID="ddlTitle" runat="server" DataSourceID="TitleDataSource" DataTextField="LookUpName"
                                                            ValidationGroup="ExternalSaveGroup" DataValueField="LookUpID" Width="253px">
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td>
                                                    </td>
                                                    <td>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        First Name:
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtFirstName" runat="server" MaxLength="35" Width="246px" ValidationGroup="ExternalSaveGroup"></asp:TextBox>
                                                        <asp:RequiredFieldValidator ID="reqFirstName" runat="server" ErrorMessage="Please enter First Name"
                                                            ValidationGroup="ExternalSaveGroup"  ControlToValidate="txtFirstName"
                                                            Text="*"></asp:RequiredFieldValidator>
                                                    </td>
                                                    <td>
                                                        Last Name:
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtLastName" runat="server" MaxLength="0" Width="246px" ValidationGroup="ExternalSaveGroup"></asp:TextBox><asp:RequiredFieldValidator
                                                            ID="reqLastName" runat="server" ErrorMessage="Please enter Last Name" ValidationGroup="ExternalSaveGroup"
                                                             ControlToValidate="txtLastName" Text="*"></asp:RequiredFieldValidator>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        Address1:
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtAddress1" runat="server" MaxLength="50" Width="246px" ValidationGroup="ExternalSaveGroup"></asp:TextBox><asp:RequiredFieldValidator
                                                            Text="*"  ID="ReqAddr1" runat="server" ErrorMessage="Please Enter Address1"
                                                            ValidationGroup="ExternalSaveGroup" ControlToValidate="txtAddress1"></asp:RequiredFieldValidator>
                                                    </td>
                                                    <td>
                                                        Address2:
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtAddress2" runat="server" Width="246px" ValidationGroup="ExternalSaveGroup"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        City:
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtCity" runat="server" MaxLength="50" ValidationGroup="ExternalSaveGroup"
                                                            Width="246px"></asp:TextBox><asp:RequiredFieldValidator Text="*" 
                                                                ID="reqCity" runat="server" ErrorMessage="Please Enter City" ValidationGroup="ExternalSaveGroup"
                                                                ControlToValidate="txtCity"></asp:RequiredFieldValidator>
                                                    </td>
                                                    <td>
                                                        State:
                                                    </td>
                                                    <td>
                                                        <asp:ObjectDataSource ID="objDataSourceState" runat="server" SelectMethod="GetLookUpActiveData"
                                                            TypeName="ZurichNA.AIS.Business.Logic.BLAccess">
                                                            <SelectParameters>
                                                                <asp:Parameter DefaultValue="STATE" Name="lookUpTypeName" Type="String" />
                                                            </SelectParameters>
                                                        </asp:ObjectDataSource>
                                                        <asp:DropDownList ID="ddlState" runat="server" DataSourceID="objDataSourceState"
                                                            DataTextField="LookUpName" DataValueField="LookUpID" Width="253px">
                                                        </asp:DropDownList>
                                                        <asp:CompareValidator ID="CompareState"  runat="server" ControlToValidate="ddlState"
                                                            Text="*" ErrorMessage="Please Select State" Operator="NotEqual" ValueToCompare="0"
                                                            ValidationGroup="ExternalSaveGroup"></asp:CompareValidator>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        ZIP Code:
                                                    </td>
                                                    <td>
                                                         <asp:TextBox ID="txtZipCode" runat="server" ValidationGroup="ExternalSaveGroup" Width="246px"></asp:TextBox>
                                                        <asp:CompareValidator ID="compZipCode" runat="server" ControlToValidate="txtZipCode"
                                                        ValueToCompare="_____-____" ErrorMessage="Please enter Zip Code" Text="*"
                                                          Operator="NotEqual" ValidationGroup="ExternalSaveGroup"></asp:CompareValidator>
                                                        <cc1:MaskedEditExtender ID="mskZipCode" runat="server" TargetControlID="txtZipCode"
                                                            Mask="99999-9999" ErrorTooltipEnabled="True" CultureAMPMPlaceholder="" CultureCurrencySymbolPlaceholder=""
                                                            CultureDateFormat="" CultureDatePlaceholder="" CultureDecimalPlaceholder="" CultureThousandsPlaceholder=""
                                                            CultureTimePlaceholder="" Enabled="True" ClearMaskOnLostFocus="False" />
                                                        <cc1:MaskedEditValidator ID="MaskedEditValidator2" ControlExtender="mskZipCode" runat="server"
                                                            ControlToValidate="txtZipCode" IsValidEmpty="True" ErrorMessage="error" InvalidValueBlurredMessage="*"
                                                            InvalidValueMessage="Please Enter valid Zip Code" ValidationExpression="\d{5}-\_{4}|\d{5}-\d{4}|\_{5}-\_{4}"
                                                            ValidationGroup="ExternalSaveGroup">
                                                        </cc1:MaskedEditValidator>
                                                    </td>
                                                    <td>
                                                        Telephone1:
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtTelephone1" runat="server" Width="246px" ValidationGroup="ExternalSaveGroup"></asp:TextBox>
                                                        <cc1:MaskedEditExtender ID="mskTelephone1" runat="server" TargetControlID="txtTelephone1"
                                                        Mask="(999)999-9999" ClearMaskOnLostFocus="false" />
                                                       <cc1:MaskedEditValidator ID="mskValPhone1" ControlExtender="mskTelephone1"
                                                            runat="server" ControlToValidate="txtTelephone1" IsValidEmpty="True" ErrorMessage="error"
                                                            InvalidValueBlurredMessage="*" InvalidValueMessage="Please enter valid Telephone number1"
                                                            ValidationExpression="((\(\d{3}\) ?)|(\d{3}-))?\d{3}-\d{4}|\(\_{3}\)\_{3}\-\_{4}"
                                                             ValidationGroup="ExternalSaveGroup">
                                                        </cc1:MaskedEditValidator>
                                                        
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        Telephone2:
                                                    </td>
                                                    <td>                                                      
                                                       <asp:TextBox ID="txtTelephone2" runat="server" MaxLength="10" Width="246px" ValidationGroup="ExternalSaveGroup"></asp:TextBox>                                                       
                                                        <cc1:MaskedEditExtender ID="mskTelephone2" runat="server" TargetControlID="txtTelephone2"
                                                        Mask="(999)999-9999" ClearMaskOnLostFocus="false" />
                                                       <cc1:MaskedEditValidator ID="mskValPhone2" ControlExtender="mskTelephone2"
                                                            runat="server" ControlToValidate="txtTelephone2" IsValidEmpty="True" ErrorMessage="error"
                                                            InvalidValueBlurredMessage="*" InvalidValueMessage="Please enter valid Telephone number2"
                                                            ValidationExpression="((\(\d{3}\) ?)|(\d{3}-))?\d{3}-\d{4}|\(\_{3}\)\_{3}\-\_{4}"
                                                             ValidationGroup="ExternalSaveGroup">
                                                        </cc1:MaskedEditValidator>  
                                                    </td>
                                                    <td>
                                                        FAX:
                                                    </td>
                                                    <td>
                                                       <asp:TextBox ID="txtFax" runat="server" MaxLength="10" Width="246px" ValidationGroup="ExternalSaveGroup"></asp:TextBox>
                                                        <cc1:MaskedEditExtender ID="mskFax" runat="server" TargetControlID="txtFax" AutoComplete="false"
                                                            Mask="(999)999-9999" MaskType="None" InputDirection="LeftToRight" ClearMaskOnLostFocus="False"
                                                            CultureAMPMPlaceholder="" CultureCurrencySymbolPlaceholder="" CultureDateFormat=""
                                                            CultureDatePlaceholder="" CultureDecimalPlaceholder="" CultureThousandsPlaceholder=""
                                                            CultureTimePlaceholder="" />
                                                          <cc1:MaskedEditValidator ID="MaskedEditValidator1" ControlExtender="mskFax" runat="server"
                                                            ControlToValidate="txtFax" IsValidEmpty="True" ErrorMessage="error" InvalidValueBlurredMessage="*"
                                                            InvalidValueMessage="Please Enter valid FAX Number" ValidationExpression="((\(\d{3}\) ?)|(\d{3}-))?\d{3}-\d{4}|\(\_{3}\)\_{3}\-\_{4}"
                                                             ValidationGroup="ExternalSaveGroup">
                                                        </cc1:MaskedEditValidator>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        EMail:
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtEmail" runat="server" Width="246px" ValidationGroup="ExternalSaveGroup" MaxLength="50"></asp:TextBox>                                                                                                                        
                                                      <asp:RegularExpressionValidator ID="regValEmail" runat="server" ErrorMessage="Enter valid Email"
                                                             ValidationGroup="ExternalSaveGroup" Text="*" ControlToValidate="txtEmail" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"></asp:RegularExpressionValidator>
                                                    </td>
                                                    <td>
                                                    </td>
                                                    <td>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="4" style="text-align: center">
                                                        <asp:HiddenField ID="hidSelValue" runat="server" />
                                                        <asp:HiddenField ID="hidSelPostAddID" runat="server" />
                                                        <asp:Button ID="btnSave" Enabled="False" Text="Save" ValidationGroup="ExternalSaveGroup"
                                                         OnClientClick="summary()"   runat="server" OnClick="SAVE_Click" Width="60px" />
                                                        <asp:Button ID="btnCopy" Enabled="False" runat="server" Text="Copy" OnClick="btnCopy_Click"
                                                            Width="60px" />
                                                        <asp:Button ID="btnCancel" runat="server" Text="Cancel" OnClick="btnCancel_Click"
                                                            Width="60px" />
                                                        &nbsp;&nbsp;&nbsp;
                                                    </td>
                                                </tr>
                                            </table>
                                        </asp:Panel>
                            </asp:Panel>
                        </ContentTemplate>
                    </cc1:TabPanel>
                    <cc1:TabPanel ID="tpContactNames" runat="server" HeaderText="ContactNames">
                        <HeaderTemplate>
                            <div >
                                Contact Names</div>
                        </HeaderTemplate>
                        <ContentTemplate>
                            <asp:Panel ID="pnlContactNames" runat="server" CssClass="content" Width="900px" ScrollBars="Auto"
                                Height="300px">
                                <asp:AISListView ID="lstBroker" runat="server" InsertItemPosition="FirstItem" OnItemEditing="lstBroker_ItemEdit"
                                    OnItemDataBound="lstBroker_DataBoundList" OnItemCommand="lstBroker_ItemCommand"
                                    OnItemCanceling="lstBroker_ItemCancel" DataKeyNames="EXTRNL_ORG_ID" OnItemUpdating="lstBroker_ItemUpdating">
                                    <LayoutTemplate>
                                        <table id="lstBrokerTable" class="panelContents" runat="server" width="98%">
                                            <tr class="LayoutTemplate">
                                                <th>
                                                    Select
                                                </th>
                                                <th>
                                                    Contact Type
                                                </th>
                                                <th>
                                                    Name
                                                </th>
                                                <th>
                                                    Disable
                                                </th>
                                            </tr>
                                            <tr id="ItemPlaceHolder" runat="server">
                                            </tr>
                                        </table>
                                    </LayoutTemplate>
                                    <AlternatingItemTemplate>
                                        <tr id="Tr2" runat="server" class="AlternatingItemTemplate">
                                            <td>
                                                <asp:LinkButton ID="lbBrokerEdit" CommandName="Edit" Text="Edit" runat="server" Visible="true"
                                                    Width="40px" />
                                            </td>
                                            <td>
                                                <asp:Label ID="lblBrokerID" Width="165px" Visible="false" runat="server" Text='<%# Bind("EXTRNL_ORG_ID") %>'></asp:Label><asp:Label
                                                    ID="lblContactType" Width="165px" runat="server" Text='<%# Bind("CONTACT_TYPE") %>'></asp:Label>
                                            </td>
                                            <td>
                                                <%# Eval("FULL_NAME") %>
                                            </td>
                                            <td>
                                                <asp:Label ID="lblActive" runat="server" Text='<%# Bind("ACTV_IND") %>' Visible="false"></asp:Label>
                                                <asp:ImageButton ID="imgEnableDisable" runat="server" CommandArgument='<%# Bind("EXTRNL_ORG_ID") %>' />
                                            </td>
                                        </tr>
                                    </AlternatingItemTemplate>
                                    <EditItemTemplate>
                                        <tr>
                                            <td align="center">
                                                <asp:LinkButton ID="lbBrokerUpdate" CommandName="Update" Text="Update" runat="server"
                                                    Visible="true" Width="40px" ValidationGroup="ContactEditGroup" />
                                                <asp:LinkButton ID="lbBrokerCancel" CommandName="Cancel" runat="server" Text="Cancel"
                                                    Visible="true" Width="40px" />
                                            </td>
                                            <td align="center">
                                                <asp:Label ID="lblBrokerID" Width="165px" Visible="false" runat="server" Text='<%# Bind("EXTRNL_ORG_ID") %>'></asp:Label><asp:Label
                                                    ID="lblContactTypeID" Width="165px" Visible="false" runat="server" Text='<%# Bind("CONTACT_TYPE_ID") %>'></asp:Label>
                                                <asp:DropDownList ID="ddlContactTypelist" runat="server" DataSourceID="ContactTypeListDataSource"
                                                    DataTextField="LookUpName" DataValueField="LookUpID" Width="215px">
                                                </asp:DropDownList>
                                                <asp:CompareValidator ID="CompareContactTypelist" runat="server" ControlToValidate="ddlContactTypelist"
                                                    ErrorMessage="Please select Contact Type" Text="*" Display="Dynamic" Operator="NotEqual"
                                                    ValueToCompare="0" ValidationGroup="ContactEditGroup"></asp:CompareValidator>
                                            </td>
                                            <td align="center">
                                                <asp:TextBox ID="txtBrokerName" runat="server" MaxLength="100" Text='<%# Bind("FULL_NAME") %>'
                                                    ValidationGroup="EditGroup" Width="215px" />
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" Display="Dynamic" runat="server"
                                                    ControlToValidate="txtBrokerName" ErrorMessage="Please Enter Contact Name" ValidationGroup="ContactEditGroup"
                                                    Text="*" />
                                            </td>
                                            <td>
                                            </td>
                                        </tr>
                                    </EditItemTemplate>
                                    <InsertItemTemplate>
                                        <tr>
                                            <td align="center">
                                                <asp:LinkButton ID="lbBrokerInsert" CommandName="Save" Text="Save" runat="server"
                                                    Visible="true" Width="40px" ValidationGroup="ContactSaveGroup" />
                                                <%--<br />
                                                             <asp:LinkButton ID="lbBrokerCancel" CommandName="Cancel" Text="Cancel" runat="server"
                                                            Visible="true" Width="40px"  />--%>
                                            </td>
                                            <td align="center">
                                                <asp:DropDownList ID="ddlContactTypelist" runat="server" DataSourceID="ContactTypeListDataSource"
                                                    DataTextField="LookUpName" DataValueField="LookUpID" Width="215px">
                                                </asp:DropDownList>
                                                <asp:CompareValidator ID="CompareContactTypelist" runat="server" ControlToValidate="ddlContactTypelist"
                                                    ErrorMessage="Please Select Contact Type" Display="Dynamic" Operator="NotEqual"
                                                    ValueToCompare="0" Text="*" ValidationGroup="ContactSaveGroup"></asp:CompareValidator>
                                            </td>
                                            <td align="center">
                                                <asp:TextBox ID="txtBrokerName" runat="server" MaxLength="100" ValidationGroup="ContactSaveGroup"
                                                    Width="215px" />
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" Display="Dynamic"
                                                    ControlToValidate="txtBrokerName" ErrorMessage="Please Enter Contact Name" Text="*"
                                                    ValidationGroup="ContactSaveGroup" />
                                            </td>
                                            <td align="Center">
                                            </td>
                                        </tr>
                                    </InsertItemTemplate>
                                    <ItemTemplate>
                                        <tr id="Tr1" runat="server" class="ItemTemplate">
                                            <td>
                                                <asp:LinkButton ID="lbBrokerEdit" CommandName="Edit" Text="Edit" runat="server" Visible="true"
                                                    Width="40px" />
                                            </td>
                                            <td>
                                                <asp:Label ID="lblBrokerID" Width="165px" Visible="false" runat="server" Text='<%# Bind("EXTRNL_ORG_ID") %>'></asp:Label><asp:Label
                                                    ID="lblContactType" Width="165px" runat="server" Text='<%# Bind("CONTACT_TYPE") %>'></asp:Label>
                                            </td>
                                            <td>
                                                <%# Eval("FULL_NAME") %>
                                            </td>
                                            <td>
                                                <asp:Label ID="lblActive" runat="server" Text='<%# Bind("ACTV_IND") %>' Visible="false"></asp:Label>
                                                <asp:ImageButton ID="imgEnableDisable" runat="server" CommandArgument='<%# Bind("EXTRNL_ORG_ID") %>' />
                                            </td>
                                        </tr>
                                    </ItemTemplate>
                                </asp:AISListView>
                            </asp:Panel>
                        </ContentTemplate>
                    </cc1:TabPanel>
                </cc1:TabContainer>
                </ContentTemplate>
                </asp:UpdatePanel>
            </td>
        </tr>
    </table>
</asp:Content>
