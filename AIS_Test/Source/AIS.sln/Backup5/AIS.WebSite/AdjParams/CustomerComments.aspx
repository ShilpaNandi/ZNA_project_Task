<%@ Page Language="C#" MasterPageFile="~/Retro.master" AutoEventWireup="true" Inherits="AIS.WebSite.AdjParams.CustomerComments"
    CodeBehind="CustomerComments.aspx.cs" %>

<%@ Register Src="../App_Shared/AccountInfoHeader.ascx" TagName="AccountInfoHeader"
    TagPrefix="AI" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeaderPlaceHolder" runat="Server">
    <table width="910" style="text-align: right">
        <tr>
            <td style="width: 25%" align="left">
                <asp:Label ID="GAILabel" runat="server" Text="Comments" CssClass="h1"></asp:Label>
            </td>
            <td style="width: 75%" align="right">
                <asp:Button ID="btnNEW" runat="server" Text="Add New" OnClick="btnNEW_Click" />
            </td>
        </tr>
    </table>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainPlaceHolder" runat="Server">

    <script language="javascript" type="text/javascript">


    var scrollTop1;
    
    if(Sys.WebForms.PageRequestManager!=null)
    {
        Sys.WebForms.PageRequestManager.getInstance().add_beginRequest(BeginRequestHandler);
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);
    }
    
    function BeginRequestHandler(sender, args) 
    {
        var mef = $get('<%=pnlComments.ClientID%>');
        if(mef!=null)
        scrollTop1 = mef.scrollTop;
    }

    function EndRequestHandler(sender, args)
    {
        var mef = $get('<%=pnlComments.ClientID%>');
        if(mef!=null)
        mef.scrollTop = scrollTop1;
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

    function doKeypress(control, max) {
        maxLength = parseInt(max);
        value = control.value;
        if (maxLength && value.length > maxLength - 1) {
            event.returnValue = false;
            maxLength = parseInt(maxLength);
        }
    }
    // Cancel default behavior
    function doBeforePaste(control, max) {
        maxLength = parseInt(max);
        if (maxLength) {
            event.returnValue = false;
        }
    }
    // Cancel default behavior and create a new paste routine
    function doPaste(control, max) {
        maxLength = parseInt(max);
        value = control.value;
        if (maxLength) {
            event.returnValue = false;
            maxLength = parseInt(maxLength);
            var oTR = control.document.selection.createRange();
            var iInsertLength = maxLength - value.length + oTR.text.length;
            var sData = window.clipboardData.getData("Text").substr(0, iInsertLength);
            oTR.text = sData;
        }
    }

    </script>

    <table>
        <tr>
            <td>
                <AI:AccountInfoHeader ID="ucAccountInfoHeader1" runat="server" />
            </td>
        </tr>
    </table>
    <asp:ValidationSummary ID="ValSumSave" ValidationGroup="Save" CssClass="ValidationSummary"
        runat="server"></asp:ValidationSummary>
    <table>
        <tr>
            <td>
                <asp:Label ID="lblPrevComments" runat="server" Text="Previous Comments" CssClass="h3"></asp:Label>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Panel ID="pnlComments" runat="server" ScrollBars="Auto" Width="910px" Height="170px">
                    <asp:AISListView ID="lstComments" runat="server" OnItemDataBound="DataBoundList" OnSelectedIndexChanging="lstComments_SelectedIndexChanging"
                        OnItemCommand="lstComments_ItemCommand">
                        <EmptyDataTemplate>
                            <table id="Table1" class="panelContents" runat="server" width="100%">
                                <tr class="LayoutTemplate">
                                    <th>
                                        Detail
                                    </th>
                                    <th>
                                        Date
                                    </th>
                                    <th>
                                        Category
                                    </th>
                                    <th>
                                        Comment
                                    </th>
                                    <th>
                                        Comment By
                                    </th>
                                </tr>
                            </table>
                            <table width="100%">
                                <tr id="Tr1" runat="server" class="ItemTemplate">
                                    <td align="center">
                                        <asp:Label ID="lblEmptyMessage" Text="---No Records Found ---" Font-Bold="true" Width="600px"
                                            runat="server" Style="text-align: center" />
                                    </td>
                                </tr>
                            </table>
                        </EmptyDataTemplate>
                        <LayoutTemplate>
                            <table id="Table1" class="panelContents" runat="server" width="98%">
                                <tr class="LayoutTemplate">
                                    <th>
                                        Detail
                                    </th>
                                    <th>
                                        Date
                                    </th>
                                    <th>
                                        Category
                                    </th>
                                    <th>
                                        Comment
                                    </th>
                                    <th>
                                        Comment By
                                    </th>
                                </tr>
                                <tr id="itemPlaceholder" runat="server">
                                </tr>
                            </table>
                        </LayoutTemplate>
                        <ItemTemplate>
                            <tr runat="server" class="ItemTemplate" id="trItemTemplate">
                                <td>
                                    <asp:LinkButton ID="lnkDetail" CommandName="Select" CommandArgument='<%# Bind("CommentID") %>'
                                        runat="server" Text="Details" />
                                </td>
                                <td>
                                    <asp:Label ID="lblCommentDate" runat="server" Text='<%# Bind("CommentDate") %>'></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="lblCommentCategoryID" runat="server" Text='<%# Bind("CommentCategoryName") %>'></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="lblCommentText" runat="server" Text='<%# Bind("CommentText") %>'></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="lblCommentBY" runat="server" Text='<%# Bind("CommentUserName") %>'></asp:Label>
                                </td>
                            </tr>
                        </ItemTemplate>
                        <AlternatingItemTemplate>
                            <tr runat="server" class="AlternatingItemTemplate" id="trItemTemplate">
                                <td>
                                    <asp:LinkButton ID="lnkDetail" CommandName="Select" CommandArgument='<%# Bind("CommentID") %>'
                                        runat="server" Text="Details" />
                                </td>
                                <td>
                                    <asp:Label ID="lblCommentDate" runat="server" Text='<%# Bind("CommentDate") %>'></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="lblCommentCategoryID" runat="server" Text='<%# Bind("CommentCategoryName") %>'></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="lblCommentText" runat="server" Text='<%# Bind("CommentText") %>'></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="lblCommentBY" runat="server" Text='<%# Bind("CommentUserName") %>'></asp:Label>
                                </td>
                            </tr>
                        </AlternatingItemTemplate>
                    </asp:AISListView>
                </asp:Panel>
            </td>
        </tr>
    </table>
    <br />
    <table>
        <tr>
            <td>
                <asp:Label ID="lblDetails" runat="server"  Text="Comment Details" CssClass="h2"></asp:Label>
                <asp:LinkButton ID="lnkClose" runat="server" Visible="false" Text="Close" OnClick="lnkClose_Click"></asp:LinkButton>
            </td>
        </tr>
    </table>
    <asp:Panel BorderColor="Black" ID="pnlDetails" Visible="false" BorderWidth="1" Width="910px"
        runat="server">
        <table>
            <tr>
                <td style="width: 111px">
                    <asp:Label ID="lblCategory" runat="server" Text="Category:" Width="150px"></asp:Label>
                </td>
                <td style="width: 150px">
                    <asp:ObjectDataSource ID="dsCommentsCategory" runat="server" SelectMethod="GetLookUpActiveData"
                        TypeName="ZurichNA.AIS.Business.Logic.BLAccess">
                        <SelectParameters>
                            <asp:Parameter DefaultValue="COMMENT CATEGORY" Name="lookUpTypeName" />
                        </SelectParameters>
                    </asp:ObjectDataSource>
                    <asp:DropDownList ID="ddlCategory" DataSourceID="dsCommentsCategory" ValidationGroup="Save"
                        DataTextField="LookUpName" DataValueField="LookUpID" runat="server" Width="200px">
                    </asp:DropDownList>
                    <asp:CompareValidator ID="compLkup" runat="server" ControlToValidate="ddlCategory"
                        ValidationGroup="Save" ValueToCompare="0" Text="*" ErrorMessage="Please select a Category"
                        Operator="NotEqual"></asp:CompareValidator>
                </td>
            </tr>
            <caption>
                <br />
                <tr>
                    <td>
                        <asp:Label ID="lblComment" runat="server" Text="Comment:"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtComment" runat="server" Height="50px" TextMode="MultiLine"
                         ValidationGroup="Save" Width="400px" Rows="3" onpaste="doPaste(this,500);" 
                         MaxLength="500" onkeypress="doKeypress(this,500);" onbeforepaste="doBeforePaste(this,500);">
                         </asp:TextBox>
                        <%--<asp:RequiredFieldValidator ID="reqComment" runat="server" ControlToValidate="txtComment"
                            Text="*" ErrorMessage="Please Enter Comments" ValidationGroup="Save"></asp:RequiredFieldValidator>--%>
                    </td>
                </tr>
            </caption>
        </table>
        <br />
        <table width="50%">
            <tr>
                <td align="right">
                    <asp:Button ID="btnCancel" runat="server" Text="Cancel" OnClick="btnCancel_Click" />
                    <asp:Button ID="btnSave" runat="server" Text="Save" OnClick="btnSave_Click" ValidationGroup="Save" />
                </td>
            </tr>
        </table>
    </asp:Panel>
</asp:Content>
