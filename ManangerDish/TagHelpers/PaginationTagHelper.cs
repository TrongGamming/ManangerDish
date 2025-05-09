// Trong thư mục TagHelpers
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace ManagerDish.TagHelpers
{
    [HtmlTargetElement("pagination")]
    public class PaginationTagHelper : TagHelper
    {
        private readonly IUrlHelperFactory _urlHelperFactory;

        [ViewContext]
        [HtmlAttributeNotBound]
        public ViewContext ViewContext { get; set; }

        // Thuộc tính để nhận dữ liệu PagedList từ View
        [HtmlAttributeName("page-model")]
        public dynamic PagedList { get; set; } // Dùng dynamic hoặc interface nếu muốn tổng quát hơn

        public PaginationTagHelper(IUrlHelperFactory urlHelperFactory)
        {
            _urlHelperFactory = urlHelperFactory;
        }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if (PagedList == null || PagedList.TotalPages <= 1)
            {
                output.SuppressOutput(); // Không hiển thị gì nếu chỉ có 1 trang hoặc không có dữ liệu
                return;
            }

            var urlHelper = _urlHelperFactory.GetUrlHelper(ViewContext);
            var action = ViewContext.RouteData.Values["action"].ToString();
            var controller = ViewContext.RouteData.Values["controller"].ToString();

            output.TagName = "nav"; // Thẻ bao ngoài
            output.Attributes.SetAttribute("aria-label", "Page navigation");
            var ul = new TagBuilder("ul");
            ul.AddCssClass("pagination"); // Thêm class CSS (ví dụ: Bootstrap)

            // Nút Previous
            var prevLi = new TagBuilder("li");
            prevLi.AddCssClass("page-item");
            if (!PagedList.HasPreviousPage)
            {
                prevLi.AddCssClass("disabled");
            }
            var prevLink = new TagBuilder("a");
            prevLink.AddCssClass("page-link");
            // Tạo link cho trang trước, giữ nguyên các query string khác nếu có
            var prevRouteValues = ViewContext.HttpContext.Request.Query.ToDictionary(kvp => kvp.Key, kvp => (object)kvp.Value.ToString());;
            prevRouteValues["pageNumber"] = PagedList.PageNumber - 1;
            prevLink.Attributes["href"] = urlHelper.Action(action, controller, prevRouteValues);
            prevLink.InnerHtml.Append("Previous");
            prevLi.InnerHtml.AppendHtml(prevLink);
            ul.InnerHtml.AppendHtml(prevLi);

            // Các nút số trang (có thể thêm logic để chỉ hiển thị một khoảng trang)
            for (int i = 1; i <= PagedList.TotalPages; i++)
            {
                var li = new TagBuilder("li");
                li.AddCssClass("page-item");
                if (i == PagedList.PageNumber)
                {
                    li.AddCssClass("active"); // Đánh dấu trang hiện tại
                }

                var link = new TagBuilder("a");
                link.AddCssClass("page-link");
                var routeValues = ViewContext.HttpContext.Request.Query.ToDictionary(kvp => kvp.Key, kvp => (object)kvp.Value.ToString());;
                routeValues["pageNumber"] = i;
                link.Attributes["href"] = urlHelper.Action(action, controller, routeValues);
                link.InnerHtml.Append(i.ToString());

                li.InnerHtml.AppendHtml(link);
                ul.InnerHtml.AppendHtml(li);
            }

            // Nút Next
            var nextLi = new TagBuilder("li");
            nextLi.AddCssClass("page-item");
            if (!PagedList.HasNextPage)
            {
                nextLi.AddCssClass("disabled");
            }
            var nextLink = new TagBuilder("a");
            nextLink.AddCssClass("page-link");
            var nextRouteValues = ViewContext.HttpContext.Request.Query.ToDictionary(kvp => kvp.Key, kvp => (object)kvp.Value.ToString());;
            nextRouteValues["pageNumber"] = PagedList.PageNumber + 1;
            nextLink.Attributes["href"] = urlHelper.Action(action, controller, nextRouteValues);
            nextLink.InnerHtml.Append("Next");
            nextLi.InnerHtml.AppendHtml(nextLink);
            ul.InnerHtml.AppendHtml(nextLi);


            output.Content.AppendHtml(ul);
        }
    }
}
