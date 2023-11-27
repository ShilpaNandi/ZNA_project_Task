<%@ Page Language="C#" MasterPageFile="~/Retro.master" AutoEventWireup="true" Inherits="AppMgmt_InvoiceExhibit"
    CodeBehind="InvoiceExhibit.aspx.cs" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeaderPlaceHolder" runat="Server">
    <asp:Label ID="GAILabel" runat="server" Text="Invoice Exhibits" CssClass="h1"></asp:Label>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainPlaceHolder" runat="Server">
<script language="javascript" type="text/javascript">
    var scrollTop1;
    
    if(Sys.WebForms.PageRequestManager!=null)
    {
        Sys.WebForms.PageRequestManager.getInstance().add_beginRequest(BeginRequestHandler);
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);
    }
    function BeginRequestHandler(sender, args) 
    {
        var isp = $get('<%=pnlInvoiceSetup.ClientID%>');
        if(isp!=null)
        scrollTop1 = isp.scrollTop;
    }

    function EndRequestHandler(sender, args)
    {
        var isp = $get('<%=pnlInvoiceSetup.ClientID%>');
        if(isp!=null)
        isp.scrollTop = scrollTop1+25;
    } 
</script>


    <asp:ValidationSummary ID="valCombinedElems" runat="server" ValidationGroup="Update"
        BorderColor="Red" BorderWidth="1" BorderStyle="Solid" />
    <asp:ValidationSummary ID="ValCombinedElem" runat="server" ValidationGroup="Save"
        BorderColor="Red" BorderWidth="1" BorderStyle="Solid" />
    <table>
        <tr>
            <td>
                <asp:Panel ID="pnlInvoiceSetup" runat="server" Height="420px" ScrollBars="Auto" Width="910px">
                    <asp:AISListView ID="lsvInvoiceSetup" runat="server" InsertItemPosition="FirstItem" 
                        OnItemEditing="EditList" DataKeyNames="INVC_EXHIBIT_SETUP_ID" OnItemCanceling="CancelList"
                        OnItemCommand="CommandList" OnItemUpdating="UpdateList" onsorting="lsvInvoiceSetup_Sorting">
                        <LayoutTemplate>
                            <table id="Table1" class="panelContents" runat="server" width="98%">
                                <tr class="LayoutTemplate">
                                    <th>
                                    </th>
                                    <th>
                                        Attachment #
                                    </th>
                                    <th>
                                        Attachment Name
                                    </th>
                                    <th>
                                        Active /<br />
                                        Inactive
                                    </th>
                                    <th>
                                        Internal /<br />
                                        External
                                    </th>
                                    <th>
                                        
                                          <asp:LinkButton ID="lbSequence" CommandName="Sort" CommandArgument="SEQ_NBR"
                                        runat="server">Sequence</asp:LinkButton>
                                        <asp:Image ID="imgSortBySequence" runat="server" ImageUrl="~/images/descending.gif" 
                                                                    ToolTip="Descending" Visible="false" />
                                    </th>
                                    <th>
                                        Cesar<br />
                                        Coding<br />
                                        (Yes/No)
                                    </th>
                                    <th>
                                        Last<br />
                                        Changed<br />
                                        Date
                                    </th>
                                    <th>
                                        Last<br />
                                        Changed By
                                    </th>
                                </tr>
                                <tr id="itemPlaceholder" runat="server">
                                </tr>
                            </table>
                        </LayoutTemplate>
                        <AlternatingItemTemplate>
                            <tr class="AlternatingItemTemplate">
                                <td align="center">
                                    <asp:LinkButton ID="lnkEdit" CommandName="Edit" runat="server" Text="EDIT"></asp:LinkButton>
                                </td>
                                <td align="center">
                                    <asp:HiddenField ID="hidID" runat="server" Value='<%# Bind("INVC_EXHIBIT_SETUP_ID") %>' />
                                    <asp:Label ID="lblAttachmentNo" runat="server" Text='<%# Bind("ATCH_CD") %>'></asp:Label>
                                </td>
                                <td align="center">
                                    <asp:Label ID="lblAttachmentName" runat="server" Text='<%# Bind("ATCH_NM") %>'></asp:Label>
                                </td>
                                <td align="center">
                                    <asp:Label ID="lblActiveInactive" runat="server" Text='<%# Eval("STS_IND").ToString()=="True"? "Active" :"Inactive" %>'></asp:Label>
                                </td>
                                <td align="center">
                                    <asp:Label ID="lblInternalExternal" runat="server" Text='<%#(Eval("INTRNL_FLAG_IND")!=null?((Eval("INTRNL_FLAG_IND").ToString()=="I")? "Internal" : ((Eval("INTRNL_FLAG_IND").ToString()=="E")? "External" : "Both")):"")%>'></asp:Label>
                                </td>
                                <td align="center">
                                    <asp:Label ID="lblSequence" runat="server" Text='<%# Bind("SEQ_NBR") %>'></asp:Label>
                                </td>
                                <td align="center">
                                    <asp:Label ID="lblCesarCoding" runat="server" Text='<%# Eval("CESAR_CD_IND").ToString()=="True"? "Yes" :"No" %>'></asp:Label>
                                </td>
                                <td align="center">
                                    <asp:Label ID="lblLastChangedDt" runat="server" Text='<%# Bind("UPDATEDDATE","{0:d}") %>'></asp:Label>
                                </td>
                                <td align="center">
                                    <asp:Label ID="lblLastChangedBy" runat="server" Text='<%# Bind("UPDATEDUSER") %>'></asp:Label>
                                </td>
                            </tr>
                            <tr>
                            </tr>
                        </AlternatingItemTemplate>
                        <EditItemTemplate>
                            <tr class="ItemTemplate">
                                <td align="center">
                                    <asp:LinkButton ID="lnkUpdate" ValidationGroup="Update" CommandArgument='<%# Bind("INVC_EXHIBIT_SETUP_ID") %>'
                                        CommandName="Update" runat="server" Text="UPDATE"></asp:LinkButton>
                                    <asp:LinkButton ID="lnkCancel" CommandName="Cancel" runat="server" Text="CANCEL"></asp:LinkButton>
                                </td>
                                <td align="center">
                                    <asp:HiddenField ID="hidID" runat="server" Value='<%# Bind("INVC_EXHIBIT_SETUP_ID") %>' />
                                    <asp:TextBox ID="txtEditAttachmentNo" ValidationGroup="Update" MaxLength="10" runat="server"
                                        Text='<%# Bind("ATCH_CD") %>' Width="50px"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="reqAttachmentNo" runat="server" ErrorMessage="Please enter Attachment No."
                                        Text="*" ValidationGroup="Update" ControlToValidate="txtEditAttachmentNo"></asp:RequiredFieldValidator>
                                </td>
                                <td align="center">
                                    <asp:TextBox ID="txtEditAttachmentName" ValidationGroup="Update" MaxLength="50" runat="server"
                                        Text='<%# Bind("ATCH_NM") %>' Width="100px"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="reqAttachmentName" runat="server" ErrorMessage="Please enter Attachment Name."
                                        Text="*" ValidationGroup="Update" ControlToValidate="txtEditAttachmentName"></asp:RequiredFieldValidator>
                                </td>
                                <td align="center">
                                    <asp:HiddenField ID="hidActInd" runat="server" Value='<%# Bind("STS_IND") %>' />
                                    <asp:DropDownList ID="ddlEditActiveInactive" ValidationGroup="Update" runat="server">
                                        <asp:ListItem Value="0">(Select)</asp:ListItem>
                                        <asp:ListItem Value="1">Active</asp:ListItem>
                                        <asp:ListItem Value="2">Inactive</asp:ListItem>
                                    </asp:DropDownList>
                                   <asp:CompareValidator ID="CompareddlActiveInactive" runat="server" ControlToValidate="ddlEditActiveInactive"
                                                                                ValueToCompare="0" ErrorMessage="Please select the Status Active/Inactive." Text="*" Operator="NotEqual"
                                                                                ValidationGroup="Update"></asp:CompareValidator>
                                </td>
                                <td align="center">
                                    <asp:HiddenField ID="hidIntFlag" runat="server" Value='<%# Bind("INTRNL_FLAG_IND") %>' />
                                    <asp:DropDownList ID="ddlEditInternalExternal" runat="server" ValidationGroup="Update">
                                        <asp:ListItem Value="0">(Select)</asp:ListItem>
                                        <asp:ListItem Value="I">Internal</asp:ListItem>
                                        <asp:ListItem Value="E">External</asp:ListItem>
                                        <asp:ListItem Value="B">Both</asp:ListItem>
                                    </asp:DropDownList>
                                     <asp:CompareValidator ID="CompareddlInternalExternal" runat="server" ControlToValidate="ddlEditInternalExternal"
                                                                                ValueToCompare="0" ErrorMessage="Please select the Status External/Both." Text="*" Operator="NotEqual"
                                                                                ValidationGroup="Update"></asp:CompareValidator>
                                </td>
                                <td align="center">
                                    <asp:TextBox ID="txtEditSequence" ValidationGroup="Update" MaxLength="10" runat="server"
                                        Text='<%# Bind("SEQ_NBR") %>' Width="50px"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="reqSequence" runat="server" ErrorMessage="Please enter Sequence."
                                        Text="*" ValidationGroup="Update" ControlToValidate="txtEditSequence"></asp:RequiredFieldValidator>
                                    <ajaxToolkit:FilteredTextBoxExtender ID="fltrTotalPaidIndem" runat="server" TargetControlID="txtEditSequence"
                                        FilterType="Numbers" />
                                </td>
                                <td align="center">
                                    <asp:HiddenField ID="hidCesar" runat="server" Value='<%# Bind("CESAR_CD_IND") %>' />
                                    <asp:DropDownList ID="ddlEditCesarCoding" runat="server" ValidationGroup="Update">
                                        <asp:ListItem Value="0">(Select)</asp:ListItem>
                                        <asp:ListItem Value="1">Yes</asp:ListItem>
                                        <asp:ListItem Value="2">No</asp:ListItem>
                                    </asp:DropDownList>
                                     <asp:CompareValidator ID="CompareddlCesarCoding" runat="server" ControlToValidate="ddlEditCesarCoding"
                                                                                ValueToCompare="0" ErrorMessage="Please select the Cesar Coding." Text="*" Operator="NotEqual"
                                                                                ValidationGroup="Update"></asp:CompareValidator>             
                                </td>
                                <td align="center">
                                    <asp:Label ID="lblLastChangedDt" runat="server" Width="50px"></asp:Label>
                                </td>
                                <td align="center">
                                    <asp:Label ID="lblLastChangedBy" runat="server" Width="50px"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                            </tr>
                        </EditItemTemplate>
                        <InsertItemTemplate>
                            <tr class="ItemTemplate">
                                <td align="center">
                                    <asp:LinkButton ValidationGroup="Save" ID="lnkSave" CommandName="Save" runat="server"
                                        Text="SAVE"></asp:LinkButton>
                                </td>
                                <td align="center">
                                    <asp:TextBox ID="txtAttachmentNo" ValidationGroup="Save" MaxLength="10" runat="server"
                                        Width="50px" ></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="reqAttachmentNo" runat="server" ErrorMessage="Please enter Attachment No."
                                        Text="*" ValidationGroup="Save" ControlToValidate="txtAttachmentNo"></asp:RequiredFieldValidator>
                                </td>
                                <td align="center">
                                    <asp:TextBox ID="txtAttachmentName" ValidationGroup="Save" MaxLength="50" runat="server"
                                        Width="100px"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="reqAttachmentName" runat="server" ErrorMessage="Please enter Attachment Name."
                                        Text="*" ValidationGroup="Save" ControlToValidate="txtAttachmentName"></asp:RequiredFieldValidator>
                                </td>
                                <td align="center">
                                    <asp:DropDownList ID="ddlActiveInactive" ValidationGroup="Save" runat="server">
                                        <asp:ListItem Value="0">(Select)</asp:ListItem>
                                        <asp:ListItem Value="1">Active</asp:ListItem>
                                        <asp:ListItem Value="2">Inactive</asp:ListItem>
                                    </asp:DropDownList>
                                    <asp:CompareValidator ID="CompareddlActiveInactive" runat="server" ControlToValidate="ddlActiveInactive"
                                        ValueToCompare="0" ErrorMessage="Please select the Status Active/Inactive." Text="*" Operator="NotEqual"
                                        ValidationGroup="Save"></asp:CompareValidator>
                                </td>
                                <td align="center">
                                    <asp:DropDownList ID="ddlInternalExternal" runat="server" ValidationGroup="Save">
                                        <asp:ListItem Value="0">(Select)</asp:ListItem>
                                       <asp:ListItem Value="I">Internal</asp:ListItem>
                                        <asp:ListItem Value="E">External</asp:ListItem>
                                        <asp:ListItem Value="B">Both</asp:ListItem>
                                    </asp:DropDownList>
                                    <asp:CompareValidator ID="CompareddlInternalExternal" runat="server" ControlToValidate="ddlInternalExternal"
                                        ValueToCompare="0" ErrorMessage="Please select the Status External/Both." Text="*" Operator="NotEqual"
                                        ValidationGroup="Save"></asp:CompareValidator>
                                </td>
                                <td align="center">
                                    <asp:TextBox ID="txtSequence" ValidationGroup="Save" MaxLength="10" runat="server"
                                        Width="50px"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="reqSequence" runat="server" ErrorMessage="Please enter Sequence."
                                        Text="*" ValidationGroup="Save" ControlToValidate="txtSequence"></asp:RequiredFieldValidator>
                                    <ajaxToolkit:FilteredTextBoxExtender ID="fltrTotalPaidIndem" runat="server" TargetControlID="txtSequence"
                                        FilterType="Numbers" />
                                </td>
                                <td align="center">
                                    <asp:DropDownList ID="ddlCesarCoding" runat="server" ValidationGroup="Save">
                                    <asp:ListItem Text="(Select)" Value="0"></asp:ListItem>
                                        <asp:ListItem Text="Yes" Value="1"></asp:ListItem>
                                        <asp:ListItem Text="No" Value="2"></asp:ListItem>
                                    </asp:DropDownList>
                                    <asp:CompareValidator ID="CompareddlCesarCoding" runat="server" ControlToValidate="ddlCesarCoding"
                                        ValueToCompare="0" ErrorMessage="Please select the Cesar Coding." Text="*" Operator="NotEqual"
                                        ValidationGroup="Save"></asp:CompareValidator>
                                </td>
                                <td align="center">
                                    <asp:Label ID="lblLastChangedDt" runat="server" Width="50px"></asp:Label>
                                </td>
                                <td align="center">
                                    <asp:Label ID="lblLastChangedBy" runat="server" Width="50px"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                            </tr>
                        </InsertItemTemplate>
                        <ItemTemplate>
                            <tr class="ItemTemplate">
                                <td align="center">
                                    <asp:LinkButton ID="lnkEdit" CommandName="Edit" runat="server" Text="EDIT"></asp:LinkButton>
                                </td>
                                <td align="center">
                                    <asp:HiddenField ID="hidID" runat="server" Value='<%# Bind("INVC_EXHIBIT_SETUP_ID") %>' />
                                    <asp:Label ID="lblAttachmentNo" runat="server" Text='<%# Bind("ATCH_CD") %>'></asp:Label>
                                </td>
                                <td align="center">
                                    <asp:Label ID="lblAttachmentName" runat="server" Text='<%# Bind("ATCH_NM") %>'></asp:Label>
                                </td>
                                <td align="center">
                                    <asp:Label ID="lblActiveInactive" runat="server" Text='<%# Eval("STS_IND").ToString()=="True"? "Active" :"Inactive" %>'></asp:Label>
                                </td>
                                <td align="center">
                                    <asp:Label ID="lblInternalExternal" runat="server" Text=
'<%#(Eval("INTRNL_FLAG_IND")!=null?((Eval("INTRNL_FLAG_IND").ToString()=="I")? "Internal" : ((Eval("INTRNL_FLAG_IND").ToString()=="E")? "External" : "Both")):"")%>'></asp:Label>
                                </td>
                                <td align="center">
                                    <asp:Label ID="lblSequence" runat="server" Text='<%# Bind("SEQ_NBR") %>'></asp:Label>
                                </td>
                                <td align="center">
                                    <asp:Label ID="lblCesarCoding" runat="server" Text='<%# Eval("CESAR_CD_IND").ToString()=="True"? "Yes" :"No" %>'></asp:Label>
                                </td>
                                <td align="center">
                                    <asp:Label ID="lblLastChangedDt" runat="server" Text='<%# Bind("UPDATEDDATE","{0:d}") %>'></asp:Label>
                                </td>
                                <td align="center">
                                    <asp:Label ID="lblLastChangedBy" runat="server" Text='<%# Bind("UPDATEDUSER") %>'></asp:Label>
                                </td>
                            </tr>
                            <tr>
                            </tr>
                        </ItemTemplate>
                    </asp:AISListView>
                </asp:Panel>
            </td>
        </tr>
    </table>
</asp:Content>
