using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Web.Mvc;
using NMPIB.Models;

namespace NMPIB.Helpers
{
    public static class PureCssMenu
    {

        public static string CssMenu(this HtmlHelper helper)
        {
            var sb = new StringBuilder();
            string[] userroles = new NMPRoleProvider().GetRolesForUser(HttpContext.Current.User.Identity.Name.ToString());
            var topLevelNodes = SiteMap.RootNode.ChildNodes;

            try
            {
                sb.Append("<ul class=\"cssMenu cssMenum0\">");

                LoopBranch(topLevelNodes, ref sb, ref helper, false, userroles);

                sb.Append("</ul>");
            }
            catch
            {
                //Do nothing
            }

            return sb.ToString();

        }

        private static void LoopBranch(SiteMapNodeCollection nodeCollection, ref StringBuilder sb, ref HtmlHelper helper, bool isSub, string[] userroles)
        {
            string newUrl = HttpRuntime.AppDomainAppVirtualPath.TrimEnd('/');

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
                    newUrl += node.Url;
                        if (node.ChildNodes.Count > 0)
                            sb.AppendFormat("<a class=\"" + menuclass + "\" href='{0}'><span>{1}</span><![if gt IE 6]></a><![endif]><!--[if lte IE 6]><table><tr><td><![endif]-->", newUrl, helper.Encode(node.Title));
                        else
                            sb.AppendFormat("<a class=\"" + menuclass + "\" href='{0}'>{1}</a>", newUrl, helper.Encode(node.Title));

                    //}

                    if (node.ChildNodes.Count > 0)
                    {
                        sb.Append("<ul class=\"cssMenum\">");
                        LoopBranch(node.ChildNodes, ref sb, ref helper, true, userroles);
                        sb.Append("</ul>");
                    }
                    sb.AppendLine("<!--[if lte IE 6]></td></tr></table></a><![endif]--></li>");
                }
            }



        }

    }
}
