<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/nmp.Master" Inherits="System.Web.Mvc.ViewPage<NMPIB.Models.tbl_Image>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Create
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="titleBox">
        <div class="inputDiv">
            <h2>Add Image</h2>
        </div>
    </div>
    <div>
    </div>
        <%= Html.ValidationSummary("Create was unsuccessful. Please correct the errors and try again.") %>

    <% using (Html.BeginForm())
       {%>
       <div class="titleBox">
    <table cellpadding="0" cellspacing="0">
        <tr>
            <td style="vertical-align: top">
                <div class="inputDiv" style="width:550px; height:400px">
                    <p>
                        <label for="magazine_id">Magazine:</label>
                        <%= Html.DropDownList("magazine_id", (SelectList)ViewData["Magazines"])%>
                    </p>
                    <p>
                        <label for="magazine_issue">Magazine Issue:</label>
                        <%= Html.TextBox("magazine_issue")%>
                        <%= Html.ValidationMessage("magazine_issue", "*") %>
                    </p>
                    <p>
                        <label for="shoot_description">Shoot Description:</label>
                        <%= Html.TextBox("shoot_description")%>   
                        <%= Html.ValidationMessage("shoot_description", "*")%>
                    </p>
                    <p>
                        <label for="shoot_date">Shoot Date:</label>
                        <%= Html.TextBox("shoot_date", null, new { autocomplete="off" })%>
                        <%= Html.ValidationMessage("shoot_date", "*")%>
                        
                    </p>
                    <p>
                        <label for="description">Description:</label>
                        <%= Html.TextBox("description")%>
                        <%= Html.ValidationMessage("description", "*")%>
                    </p>
                    <p>
                        <label for="keywords">Keywords:</label>
                        <%= Html.TextBox("keywords")%>
                        <%= Html.ValidationMessage("keywords", "*")%>
                    </p>
                    <p>
                        <label for="photographer">Uploaded By:</label>
                        <%=User.Identity.Name.ToString().ToUpper() %>
                    </p>
                    <p>
                        <%= Html.TextBox("name")%>
                        <%= Html.DropDownList("photographer", (SelectList)ViewData["Users"], new { @style = "font-size:8pt" })%>
                        <input type="submit" value="Create" />
                    </p>
                </div>
            </td>
             <td style="vertical-align: top">
                    <div class="inputDiv" style="width: 392px; height: 400px;">
                        <center>
                            <input type="file" id="FileUp" />
                            <img src="<%=HttpRuntime.AppDomainAppVirtualPath.TrimEnd('/') %>/Content/blank.gif" title="" alt="" id="imgPreview"  width="380px" />
                        </center>
                    </div>
                </td>
        </tr>
    </table>
    </div>
    <% } %>
    
    <div>
        <%=Html.ActionLink("Back to List", "Index") %>
    </div>
   
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="<%=HttpRuntime.AppDomainAppVirtualPath.TrimEnd('/') %>/Content/uploadify.css" rel="stylesheet" type="text/css" />

    <script src="<%=HttpRuntime.AppDomainAppVirtualPath.TrimEnd('/') %>/Scripts/jquery.uploadify.v2.1.0.min.js" type="text/javascript"></script>

    <script src="<%=HttpRuntime.AppDomainAppVirtualPath.TrimEnd('/') %>/Scripts/swfobject.js" type="text/javascript"></script>

    <script type="text/javascript">

       // window.onunload = function() { DeleteCurrentImage(); }
        var currentnmpibfile = "";

        $(function() {
       
            if ($("#name").val() != "") {
                currentnmpibfile = $("#name").val();
                $("#imgPreview").attr('src', "<%=HttpRuntime.AppDomainAppVirtualPath.TrimEnd('/') %>/photos/" + $("#name").val());
            }
            $("#name").hide();
            $("#photographer").hide();
            $("#shoot_date").datepicker({ "dateFormat": "d-M-yy", "duration": "fast" });
            $("#FileUp").uploadify(
              {
                  uploader: "<%=HttpRuntime.AppDomainAppVirtualPath.TrimEnd('/') %>/Content/flash/uploadify.swf",
                  script: "<%=HttpRuntime.AppDomainAppVirtualPath.TrimEnd('/') %>/Image/Uploadify",
                  fileDesc: '.jpg',
                  fileExt: '*.jpg',
                  cancelImg: "<%=HttpRuntime.AppDomainAppVirtualPath.TrimEnd('/') %>/Content/images/cancel.png",
                  auto: true,
                  folder: "<%=HttpRuntime.AppDomainAppVirtualPath.TrimEnd('/') %>/photos",
                  onError: function(a, b, c, d) {
                      if (d.status == 404)
                          alert("Could not find the upload script. Use path relative to: " + "<? getcwd() ?>");
                      else if (d.type === "HTTP")
                          alert("error " + d.type + ": " + d.status);
                      else if (d.type === "File Size")
                          alert(c.name + " " + d.type + " Limit: " + Math.round(d.sizeLimit / 1024) + "KB");
                      else
                          alert("error " + d.type + ": " + d.text);
                  },
                  onComplete: function(event, queueID, FileObj, response, data) {
                      currentnmpibfile = js_RemoveChar(response);
                      $("#name").val(currentnmpibfile);
                      $("#imgPreview").attr('src', "<%=HttpRuntime.AppDomainAppVirtualPath.TrimEnd('/') %>/photos/" + currentnmpibfile);
                  },
                  onSelect: function(event, queueID, fileObj) {
                      DeleteCurrentImage();
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
                folder: '<%=HttpRuntime.AppDomainAppVirtualPath.TrimEnd(' / ') %>/photos',
                currentFilename: currentnmpibfile
            });
        }

    </script>

</asp:Content>
