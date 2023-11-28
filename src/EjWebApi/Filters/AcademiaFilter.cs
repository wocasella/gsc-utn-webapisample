using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace EjWebApi.Filters
{
    public class AcademiaFilter : IActionFilter
    {
        private readonly ILogger logger;

        public AcademiaFilter(ILogger<AcademiaFilter> logger) => this.logger = logger;

        public void OnActionExecuting(ActionExecutingContext context)
        {
            this.logger.LogInformation("Filtro: antes de llegar a la acción del controller");

            //context.Result = new ContentResult()
            //{
            //    Content = "No va a ejecutar la acción del controller"
            //};
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            this.logger.LogInformation("Filtro: luego de haber ejecutado la acción del controller");
        }
    }
}
