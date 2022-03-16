using Swashbuckle.Swagger;
using System.Web.Http.Description;

namespace Orion.WeatherApi
{
    internal class AddAuthorizationHeaderParameter : IOperationFilter
    {
        private const string Type = "Bearer ";

        public void Apply(Operation operation, SchemaRegistry schemaRegistry, ApiDescription apiDescription)
        {
           
            if (operation.parameters != null)
            {
                operation.parameters.Add(new Parameter
                {
                    name = "Authorization",
                    @in = "header",
                    description = "Bearer  ",
                    required = false,
                    //type = "string",
                    schema = new Schema
                    {
                       type = "string",
                       @default = "Bearer ",

                    }
                });
            }
        }
    }
}