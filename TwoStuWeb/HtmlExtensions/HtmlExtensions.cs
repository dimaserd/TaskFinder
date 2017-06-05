using Extensions.Reflection;
using Extensions.String;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace TwoStuWeb.HtmlExtensions
{
    public static partial class HtmlExtensions
    {
        

        #region UI Elements


        #region ValidationSummary
        public static MvcHtmlString ValidationErrorsToAlerts(this HtmlHelper html)
        {

            List<ModelError> ModelErrors = new List<ModelError>();
            foreach (ModelState modelState in html.ViewData.ModelState.Values)
            {
                foreach (ModelError error in modelState.Errors)
                {
                    ModelErrors.Add(error);
                }
            }


            StringBuilder sb = new StringBuilder();
            foreach (ModelError error in ModelErrors)
            {
                sb.Append($"<div class='alert alert-danger'><i class='fa fa-frown-o'></i>")
                .Append($"<strong>{error.ErrorMessage}</strong></div>");
            }


            return MvcHtmlString.Create(sb.ToString());
        }

        public static MvcHtmlString FirstValidationErrorToAlert(this HtmlHelper html)
        {
            List<ModelError> ModelErrors = new List<ModelError>();
            foreach (ModelState modelState in html.ViewData.ModelState.Values)
            {
                foreach (ModelError error in modelState.Errors)
                {
                    ModelErrors.Add(error);
                }
            }

            if (ModelErrors.Count == 0)
            {
                return null;
            }

            StringBuilder sb = new StringBuilder();
            sb.Append($"<div class='alert alert-danger'><i class='fa fa-frown-o'></i>")
            .Append($"<strong>{ModelErrors[0].ErrorMessage}</strong></div>");

            return MvcHtmlString.Create(sb.ToString());
        }
        #endregion

        #region With Label
        public static MvcHtmlString LabelFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, IDictionary<string, object> htmlAttributes)
        {
            ModelMetadata metadata = ModelMetadata.FromLambdaExpression(expression, html.ViewData);
            string htmlFieldName = ExpressionHelper.GetExpressionText(expression);
            string labelText = metadata.DisplayName ?? metadata.PropertyName ?? htmlFieldName.Split('.').Last();
            if (string.IsNullOrEmpty(labelText))
            {
                return MvcHtmlString.Empty;
            }

            TagBuilder tag = new TagBuilder("label");
            tag.MergeAttributes(htmlAttributes);
            tag.Attributes.Add("for", html.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldId(htmlFieldName));

            TagBuilder span = new TagBuilder("span");
            span.SetInnerText(labelText);

            // assign <span> to <label> inner html
            tag.InnerHtml = span.ToString(TagRenderMode.Normal);

            return MvcHtmlString.Create(tag.ToString(TagRenderMode.Normal));
        }

        public static MvcHtmlString LabelAndEditorFor<TModel, TValue>(this HtmlHelper<TModel> html,
            Expression<Func<TModel, TValue>> expression, object htmlAttributes = null)
        {

            var member = expression.Body as MemberExpression;
            var prop = member.Member as PropertyInfo;

            

            StringBuilder sb = new StringBuilder();
            sb.Append($"<div class='form-group {htmlAttributes.GetPropertyValue("class")}' ");
            if (htmlAttributes != null)
            {
                sb.Append(htmlAttributes.RenderAttributesKeyValuePairExcept("class"));
            }
            sb.Append(" >");

            sb.Append(html.LabelFor(expression, htmlAttributes: new { @class = "control-label col-md-2" }).ToString())
            .Append("<div class='col-md-5'>");
            
             sb.Append(html.EditorFor(expression, new { id = expression.Type.Name.ToId(), htmlAttributes = new { @class = "form-control" } }))
             .Append(html.ValidationMessageFor(expression, "", new { @class = "text-danger" }));
            

            sb.Append("</div>")
            .Append("</div>");
            return MvcHtmlString.Create(sb.ToString());
        }


        
        #endregion

        #region Popover methods

        

        
        #endregion

        

        #endregion

    }
}