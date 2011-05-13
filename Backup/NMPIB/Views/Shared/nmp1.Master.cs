using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using NMPIB.Models;
using System.Web.Mvc;


public partial class _nmp1 : System.Web.UI.MasterPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        
        Literal myLiteral = (Literal)this.FindControl("ltlMenu");
        myLiteral.Text = "Hi";

        var sb = new StringBuilder();
        string[] userroles = new NMPRoleProvider().GetRolesForUser(HttpContext.Current.User.Identity.Name.ToString());
        var topLevelNodes = SiteMap.RootNode.ChildNodes;


        try
        {
            sb.Append("<ul class=\"cssMenu cssMenum0\">");

            LoopBranch(topLevelNodes, ref sb,  false, userroles);

            sb.Append("</ul>");
        }
        catch
        {
            //Do nothing
        }

        myLiteral.Text = sb.ToString();
    }

    private void LoopBranch(SiteMapNodeCollection nodeCollection, ref StringBuilder sb,  bool isSub, string[] userroles)
    {

        string menuclass = "cssMenui0";
        if (isSub) menuclass = "cssMenui";
        foreach (SiteMapNode node in nodeCollection)
        {
            bool nodeDisplayStatus = false;
            foreach (var role in node.Roles)
            {
                if (role.Equals("*") || userroles.Contains(role))
                {
                    nodeDisplayStatus = true;
                    break;
                }
            }

            if (nodeDisplayStatus)
            {

                sb.AppendLine("<li class=\"" + menuclass + "\">");

                //if (SiteMap.CurrentNode == node)
                //{
                //    if (node.ChildNodes.Count > 0)
                //        sb.AppendFormat("<a class=\"cssMenui0\" href='{0}'>{1}</a>", node.Url, helper.Encode(node.Title));
                //    else
                //        sb.AppendFormat("<a class=\"cssMenui0\" href='{0}'>{1}</a>", node.Url, helper.Encode(node.Title));
                //}
                //else
                //{
                if (node.ChildNodes.Count > 0)
                    sb.AppendFormat("<a class=\"" + menuclass + "\" href='{0}'><span>{1}</span><![if gt IE 6]></a><![endif]><!--[if lte IE 6]><table><tr><td><![endif]-->", 
                        node.Url, 
                        node.Title);
                else
                    sb.AppendFormat("<a class=\"" + menuclass + "\" href='{0}'>{1}</a>", 
                        node.Url, 
                        node.Title);

                //}

                if (node.ChildNodes.Count > 0)
                {
                    sb.Append("<ul class=\"cssMenum\">");
                    LoopBranch(node.ChildNodes, ref sb, true, userroles);
                    sb.Append("</ul>");
                }
                sb.AppendLine("<!--[if lte IE 6]></td></tr></table></a><![endif]--></li>");
            }
        }



    }

}

