using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using SWOF.Api.Models;
using SWOF.Api.Utils;

namespace SWOF.Api.Filters
{
	public class ValidateStartDateInput : ActionFilterAttribute
	{
		public override void OnActionExecuting(ActionExecutingContext context)
		{
			var startDate = DateHelpers.ParseDateInput(context.RouteData.Values["startDate"].ToString());

			if (!startDate.HasValue)
				context.Result = new BadRequestObjectResult(new ValidationError("Start date input is invalid"));
			else
				context.ActionArguments["startDate"] = startDate.Value;

			base.OnActionExecuting(context);
		}
	}
}
