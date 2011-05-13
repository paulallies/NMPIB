<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CreateWebForm.aspx.cs" MasterPageFile="~/Views/Shared/nmp1.Master" Inherits="NMPIB.CreateWebForm" %>
<asp:Content ID="Content2" ContentPlaceHolderID="TitleContent" runat="server">
    Create
</asp:Content>

<asp:Content ID="Content1" runat="server" contentplaceholderid="MainContent">
    <div class="titleBox">
        <div class="inputDiv">
            <h2>Add Image</h2>
        </div>
    </div>
    <div class="titleBox">
    <table cellpadding="0" cellspacing="0">
        <tr>
            <td style="vertical-align: top">
                <div class="inputDiv" style="width:550px; height:400px">
                    <p>
                        <label for="magazine_id">Magazine:</label>
                        <asp:DropDownList ID="ddlMag" runat="server"></asp:DropDownList>
                    </p>
                    <p>
                        <label for="magazine_issue">Magazine Issue:</label>
                        <asp:TextBox ID="txtMagIssue" runat="server"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="validMagIssue" runat="server" ControlToValidate="txtMagIssue" ErrorMessage="Mag Issue Required" ValidationGroup="1">*</asp:RequiredFieldValidator>
                    </p>
                    <p>
                        <label for="shoot_description">Shoot Description:</label>
                        <asp:TextBox ID="txtShootDescription" runat="server"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="validShootDescription" runat="server" ControlToValidate="txtShootDescription" ErrorMessage="Shoot Description Required" ValidationGroup="1">*</asp:RequiredFieldValidator>
                    </p>
                    <p>
                        <label for="shoot_date">Shoot Date:</label>
                        <asp:TextBox ID="txtShootDate" runat="server"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="validShootDate" runat="server" ControlToValidate="txtShootDate" ErrorMessage="Shoot Date Required" ValidationGroup="1">*</asp:RequiredFieldValidator>
                        
                    </p>
                    <p>
                        <label for="description">Description:</label>
                       <asp:TextBox ID="txtDescription" runat="server"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="validDescription" runat="server" ControlToValidate="txtDescription" ErrorMessage="Description Required" ValidationGroup="1">*</asp:RequiredFieldValidator>

                    </p>
                    <p>
                        <label for="keywords">Keywords:</label>

                    </p>
                    <p>
                        <label for="photographer">Uploaded By:</label>

                    </p>
                    <p>
                        <asp:Button ID="btnSave" runat="server" Text="Save" ValidationGroup="1" />
                    </p>
                </div>
            </td>
             <td style="vertical-align: top">
                    <div class="inputDiv" style="width: 392px; height: 400px;">
                        <center>
                            <asp:FileUpload ID="FileUpload" runat="server" />
                            <img src="/Content/blank.gif" title="" alt="" id="imgPreview"  width="380px" />
                        </center>
                    </div>
                </td>
        </tr>
    </table>
    </div>
            
</asp:Content>

<asp:Content ID="ContentHead" runat="server" ContentPlaceHolderID="HeadContent">

</asp:Content>
