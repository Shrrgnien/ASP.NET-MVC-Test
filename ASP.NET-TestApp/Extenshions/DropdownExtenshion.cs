﻿using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System.Linq.Expressions;

namespace ASP.NET_TestApp.Extenshions
{
    public static class DropdownExtenshion
    {
        public static HtmlString EnumDropDownListFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> modelExpression, string firstElement)
        {
            var typeOfProperty = modelExpression.ReturnType;
            if (!typeOfProperty.IsEnum)
                throw new ArgumentException(string.Format("Type {0} is not an enum", typeOfProperty));
            var enumValues = new SelectList(Enum.GetValues(typeOfProperty));
            return (HtmlString)htmlHelper.DropDownListFor(modelExpression, enumValues, firstElement);
        }
    }
}
