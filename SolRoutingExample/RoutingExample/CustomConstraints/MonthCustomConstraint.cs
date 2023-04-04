using System.Text.RegularExpressions;

namespace RoutingExample.CustomConstraints
{
    //EG: sales-report/2020/apr
    public class MonthCustomConstraint : IRouteConstraint
    {
        public bool Match(HttpContext? httpContext, IRouter? route, string routeKey, RouteValueDictionary values, RouteDirection routeDirection)
        {
            //checking does parameter exists or not
            if (!values.ContainsKey(routeKey))
            {
                //Doesen't match
                return false;
            }

            Regex regex = new Regex("^(apr|jul|oct|jan)$");

            string? month = Convert.ToString(values[routeKey]);
            if (regex.IsMatch(month))
            {
                return true;
            }
            return false;
        }
    }
}
