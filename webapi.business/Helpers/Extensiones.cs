using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using webapi.business.Pagination;

namespace webapi.business.Helpers
{
    public static class Extensiones
    {
        public static void AddApplicationError(this HttpResponse response, string message)
        {
            response.Headers.Append("Application-Error", message);
            response.Headers.Append("Access-Control-Expose-Headers", "Application-Error");
            response.Headers.Append("Access-Control-Allow-Origin", "*");
        }
        public static void AddPagination(this HttpResponse response,
    int currentPage, int itemsPerPage, int totalItems, int totalPages)
        {
            var paginationHeader = new PaginationHeader(currentPage, itemsPerPage,
                totalItems, totalPages);
            var camelCaseFormatter = new JsonSerializerSettings
            {
                ContractResolver =
                new CamelCasePropertyNamesContractResolver()
            };

            response.Headers.Append("Pagination",
                JsonConvert.SerializeObject(paginationHeader, camelCaseFormatter));
            response.Headers.Append("Access-Control-Expose-Headers", "Pagination");
        }
        public static int CalculoEdad(this DateTime theDateTime)
        {
            var age = DateTime.Today.Year - theDateTime.Year;
            if (theDateTime.AddYears(age) > DateTime.Today)
                age--;

            return age;
        }
    }
}
