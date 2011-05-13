<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/nmp.Master" Inherits="System.Web.Mvc.ViewPage<NMPIB.Models.tbl_Image>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Create2
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="titleBox">
        <div class="inputDiv">
            <h2>
                Add Image</h2>
        </div>
    </div>
    <div>
    </div>
    <% using (Html.BeginForm("Create2", "Image", FormMethod.Post, new { enctype = "multipart/form-data" }))
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
                                <%=Html.TextBox("magazine_issue", null, new{@autocomplete="off"}) %>
                            </li>
                            <li>
                                <label for="shoot_description">Shoot Description:</label>
                                <%=Html.TextArea("shoot_description", null, new { @autocomplete = "off" })%>
                            </li>
                            <li>
                                <label for="shoot_date">Shoot Date:</label>
                                <%=Html.TextBox("shoot_date", null, new { @autocomplete = "off" })%>
                            </li>
                            <li>
                                <label for="description">Description:</label>
                                <%=Html.TextArea("description", null, new { @autocomplete = "off" })%>
                            </li>
                            <li>
                                <label for="keywords">Keywords:</label>
                                <%=Html.TextArea("keywords", null, new { @autocomplete = "off" })%>
                            </li>
                            <li>
                                <label for="photographer">Uploaded By:</label>
                                <%=User.Identity.Name.ToString().ToUpper() %>
                                <%= Html.DropDownList("photographer", (SelectList)ViewData["Users"], new { @style = "font-size:8pt;" })%>
                            </li>
                            <li>
                                <input type="submit" value="Create" name="Create" />
                                <input type="submit" value="Create and Add Another" name="Another" />
                            </li>
                        </ul>
                    </div>
                </td>
                <td style="vertical-align: top">
                    <div class="inputDiv" style="width: 392px; height: 400px;">
                        <center>
                            <input type="file" id="FileUp" name="FileUp" />
                            <img src="<%=HttpRuntime.AppDomainAppVirtualPath.TrimEnd('/') %>/Content/blank.gif" title="" alt="" id="imgPreview" />
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

    <script type="text/javascript">
            var hash = {  '.jpg'  : 1,  '.jpeg' : 1};
        $(function() {
            $("#photographer").hide();
            $("#shoot_date").datepicker({ "dateFormat": "d-M-yy", "duration": "fast" });
            $("input[type=submit]").click(function() {
                if(Checkform().length == 0)
                return true;
                else
                {
                alert(Checkform());
                return false;
                }
            });
        });
        
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
            
            if ($("#FileUp").val() != "") {
                    var filename = $("#FileUp").val();
                    var re = /\..+$/;
                    var ext = filename.match(re);
                    if (hash[ext]) {
                       //do nothing
                    }
                    else {
                        
                        error += "Invalid filename, please select another file\n";
                    }
                } 
           else {
                   error += "Please select file";
                   
                }
                
                
            return error;
        }

    </script>

</asp:Content>
