<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/nmp.Master" Inherits="System.Web.Mvc.ViewPage<NMPIB.Models.tbl_Image>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Edit
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="titleBox">
        <div class="inputDiv">
            <h2>Edit Image</h2>
        </div>
    </div>
    <%= Html.ValidationSummary("Create was unsuccessful. Please correct the errors and try again.") %>
    <% using (Html.BeginForm("Edit", "Image", FormMethod.Post, new { enctype = "multipart/form-data" }))
       {%>
    <div class="titleBox">
        <table cellpadding="0" cellspacing="0">
            <tr>
                <td style="vertical-align: top">
                    <div class="inputDiv" style="width: 550px; height: 400px">
                        <ul>
                            <li>
                                <label for="magazine_id">Magazine:</label>
                                <%= Html.DropDownList("magazine_id", (SelectList)ViewData["Magazines"])%>
                            </li>
                            <li>
                                <label for="magazine_issue">Magazine Issue:</label>
                                <%= Html.TextBox("magazine_issue")%>
                            </li>
                            <li>
                                <label for="shoot_description">Shoot Description:</label>
                                <%= Html.TextArea("shoot_description")%>
                            </li>
                            <li>
                                <label for="shoot_date">Shoot Date:</label>
                                <%= Html.TextBox("shoot_date", Model.shoot_date.Value.ToString("dd-MMM-yyyy"), new { autocomplete="off" })%>
                            </li>
                            <li>
                                <label for="description">Description:</label>
                                <%= Html.TextArea("description")%>
                            </li>
                            <li>
                                <label for="keywords">Keywords:</label>
                                <%= Html.TextArea("keywords")%>
                   
                            </li>
                            <li>
                                <label for="photographer">Uploaded By:</label>
                                <%= ViewData["UploadedBy"].ToString()  %>
                            </li>
                            <li>
                                <label for="ImageName">Image Name:</label>
                                <%= Html.Encode(Model.name)%>
                            </li>
                            <li>
                               
                                <%= Html.DropDownList("photographer", (SelectList)ViewData["Users"])%>
                                <input type="submit" value="Update" id="btnUpdate" />
                            </li>
                        </ul>
                    </div>
                </td>
                <td style="vertical-align: top">
                    <div class="inputDiv" style="width: 392px; height: 400px;">
                        <center>
                            <input type="file" id="FileUp" name="FileUp" />
                            <img src="<%=HttpRuntime.AppDomainAppVirtualPath.TrimEnd('/') %>/photos/preview/pv_<%= Html.Encode(Model.name)%>"
                                title="" alt="" id="imgPreview" />
                        </center>
                    </div>
                </td>
            </tr>
        </table>
    </div>
    <input type="hidden" value="" id="hdnReturnurl" name="hdnReturnurl" />
    <% } %>
    <div>
        <a id="aCancel" href="<%=HttpRuntime.AppDomainAppVirtualPath.TrimEnd('/') %>/Image/details/<%= Html.Encode(Model.id)%>" >Cancel</a>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="HeadContent" runat="server">

    <script type="text/javascript">
        
        var urlPath = rtrim('<%=ResolveUrl("~/")%>', "/");
        var SearchString = "";
        var SearchStatus = false;
        var pageIndex = 1;
        var sortOrder = "";
        
        var currentnmpibfile = "";
        var hash = {  '.jpg'  : 1,  '.jpeg' : 1};

        $(function() {
            if ($.jqURL.get('pageindex') > 1)
                pageIndex = $.jqURL.get('pageindex');

            if ($.jqURL.get('search') != "" && $.jqURL.get('search') != null)
                SearchString = $.jqURL.get('search');

            if ($.jqURL.get('status') != "" && $.jqURL.get('status') != null)
                SearchStatus = $.jqURL.get('status');

            if ($.jqURL.get('sord') != "" && $.jqURL.get('sord') != null)
                sortOrder = $.jqURL.get('sord');

            var cancelattr = $("#aCancel").attr("href");
            $("#aCancel").attr("href", cancelattr + "?pageindex=" + pageIndex + "&search=" + SearchString + "&sord=" + sortOrder);
            $("#hdnReturnurl").val( $("#aCancel").attr("href"));

            $("#photographer").hide();
            $("#shoot_date").datepicker({ "dateFormat": "d-M-yy", "duration": "fast" });
            $("#btnUpdate").click(function() {
                if (Checkform().length == 0)
                    return true;
                else {
                    alert(Checkform());
                    return false;
                }

            });
            $("#FileUp").change(function() {

                if ($(this).val() != "") {

                    var filename = $("#FileUp").val();
                    var re = /\..+$/;
                    var ext = filename.match(re);
                    if (hash[ext]) {
                        //do nothing
                    }
                    else {
                        alert("Invalid filename, please select another file");
                        $(this).val("");

                    }
                }
            });
        });

        function js_RemoveChar(str) {
            var charToRemove = '"';
            regExp = new RegExp("[" + charToRemove + "]", "g");
            return str.replace(regExp, "");
        }

        function DeleteCurrentImage() {
            $.post(
            '<%=ResolveUrl("~/Image/DeleteCurrentFile/")%>',
            {
                folder: '<%=HttpRuntime.AppDomainAppVirtualPath.TrimEnd('/') %>/photos',
                currentFilename: currentnmpibfile
            });
        }
        
        
        function Checkform()
        {
            var error = "";
            
            if($.trim($("#magazine_issue").val()).length == 0)
            {
                error += "Please provide Magazine issue\n";
                
            }
            
            if($.trim($("#shoot_description").val()).length == 0)
            {
                error += "Please provide shoot description\n";
                
            }
            
            if($.trim($("#shoot_date").val()).length == 0)
            {
                error += "Please provide shoot date\n";
                
            }
            
            try
            {
                var test = Date.parse($("#shoot_date").val());
            }
            catch(err)
            {
                error += "Date Error: "+err+"\n";
            }
            
            if($.trim($("#description").val()).length == 0)
            {
                error += "Please provide description\n";
                
            }
            
            if($.trim($("#keywords").val()).length == 0)
            {
                error += "Please provide keywords\n";

            }
            //if no current image exists the prompt user for image
            if ($("#imgPreview").attr("src").length == 0 && $("#FileUp").val() != "") {
                error += "Please select file\n";
            }


             
                
            return error;
        }

    </script>

</asp:Content>
