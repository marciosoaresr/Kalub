using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using ProfitManager.Fabrica.Entidade;
using ProfitManager.Negocio.Implementacao;

namespace ProfitManager.Web.App_Code.CustomHelper
{
    public static class SelectExtensions
    {
        public static string GetInputName<TModel, TProperty>(Expression<Func<TModel, TProperty>> expression)
        {
            if (expression.Body.NodeType == ExpressionType.Call)
            {
                var methodCallExpression = (MethodCallExpression)expression.Body;
                string name = GetInputName(methodCallExpression);
                return name.Substring(expression.Parameters[0].Name.Length + 1);

            }
            return expression.Body.ToString().Substring(expression.Parameters[0].Name.Length + 1);
        }


        public static MvcHtmlString EnumDropDownListFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper,
            Expression<Func<TModel, TProperty>> expression, object htmlAttributes)
        {
            string inputName = GetInputName(expression);
            var value = htmlHelper.ViewData.Model == null
                ? default(TProperty)
                : expression.Compile()(htmlHelper.ViewData.Model);

            return htmlHelper.DropDownList(inputName, ToSelectList(typeof(TProperty), value.ToString()), htmlAttributes);
        }

        public static MvcHtmlString EnumDropDownListFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper,
            Expression<Func<TModel, TProperty>> expression)
        {
            string inputName = GetInputName(expression);
            var value = htmlHelper.ViewData.Model == null
                ? default(TProperty)
                : expression.Compile()(htmlHelper.ViewData.Model);

            return htmlHelper.DropDownList(inputName, ToSelectList(typeof(TProperty), value.ToString()));
        }



        public static MvcHtmlString EnumDropDownList<TModel>(this HtmlHelper<TModel> htmlHelper, Type enumType)
        {
            var list = Enumerador.GetValues(enumType);
            var listItems = new List<SelectListItem>();

            foreach (var item in list)
            {
                var keyValue = (KeyValuePair<Enum, string>)item;
                listItems.Add(new SelectListItem{ Text = keyValue.Value , Value = keyValue.Key.ToString().Substring(0, 1)});
            }

            return htmlHelper.DropDownList("nameItem", listItems, "Selecione um item");
        }

        private static string GetInputName(MethodCallExpression expression)
        {
            var methodCallExpression = expression.Object as MethodCallExpression;
            if (methodCallExpression != null)
            {
                return GetInputName(methodCallExpression);
            }
            return expression.Object.ToString();
        }

        public static SelectList ToSelectList(Type enumType, string selectedItem)
        {
            var items = new List<SelectListItem>();
            foreach (var item in Enum.GetValues(enumType))
            {
                FieldInfo fi = enumType.GetField(item.ToString());
                var attribute = fi.GetCustomAttributes(typeof(DescriptionAttribute), true).FirstOrDefault();
                var title = attribute == null ? item.ToString() : ((DescriptionAttribute)attribute).Description;
                var listItem = new SelectListItem
                {
                    Value = item.ToString(),
                    Text = title,
                    Selected = selectedItem == item.ToString()
                };
                items.Add(listItem);
            }

            return new SelectList(items, "Value", "Text", selectedItem);
        }

    }
}