using RoutingExample.CustomConstraints;
using System.ComponentModel.Design;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddRouting(options => { options.ConstraintMap.Add("months", typeof(MonthCustomConstraint));
    });
var app = builder.Build();


/*
app.Use(async (context, next) =>
{
    Microsoft.AspNetCore.Http.Endpoint? endPoint = context.GetEndpoint();
    if(endPoint != null)
    {
        await context.Response.WriteAsync($"Endpoint: {endPoint.DisplayName}\n");
    }
    await next(context);
});
*/

//enable routing
app.UseRouting();

/*
app.Use(async (context, next) =>
{
    Microsoft.AspNetCore.Http.Endpoint? endPoint = context.GetEndpoint();
    if(endPoint != null)
    {
        await context.Response.WriteAsync($"Endpoint: {endPoint.DisplayName}\n");
        await next(context);
    }
});
*/
//creating endpoitns

/*
app.UseEndpoints(endpoints =>
{
    //adding endpoints
    endpoints.MapGet("/map1", async (context) =>
    {
        await context.Response.WriteAsync("In Map 1");
    });
    endpoints.MapPost("/map2", async (context) =>
    {
        await context.Response.WriteAsync("In Map 2");
    });
});
*/

app.UseEndpoints(endpoints =>
{
    //Creating endpoint for files
    endpoints.Map("files/{filename}.{extension}", async context =>
    {
        //Getting filename from Request body from route
        string? fileName = Convert.ToString(context.Request.RouteValues["filename"]);
        string? extension = Convert.ToString(context.Request.RouteValues["extension"]);

        //After executing exacly right url the messsage is going to be ->
        await context.Response.WriteAsync($"In file {fileName} - {extension}");
    });

    //Creating endpoint for employee
    //Define minlength and maxlength for empolyeename
    //Limiting only to alphabetic values
    endpoints.Map("employee/profile/{Employeename:length(3,7):alpha=DefaultValue}", async context =>
    {
        string? employeename = Convert.ToString(context.Request.RouteValues["Employeename"]);
        await context.Response.WriteAsync($"Employeename is {employeename}");
    });
    //Default values  {routevalues = somevaules} 
    //Optional parametar {someValue?} if there is not value it's going to be NULL 
    //Adding constraints using {someValue:typeOfValue?}
    endpoints.Map("products/details/{id:int:range(1,1000)?}", async context =>
    {
        if (context.Request.RouteValues.ContainsKey("id"))
        {
            int productID = Convert.ToInt32(context.Request.RouteValues["id"]);
            await context.Response.WriteAsync($"Product id is {productID}");
        }
        else
        {
            await context.Response.WriteAsync($"Products details - id is not supplied");
        }
    });

    //Eg: dailty-digest-report/{reportdate}
    endpoints.Map("daily-digest-report/{reportdate:datetime?}", async context =>
    {
        DateTime reportDate = Convert.ToDateTime(context.Request.RouteValues["reportdate"]);
        //.ToShortDateString -> showing only date withouth time
        await context.Response.WriteAsync($"In daily-digest-report {reportDate.ToShortDateString()}");
    });

    endpoints.Map("cities/{cityid:guid}", async context =>
    {
        //Globaly unique ID
        Guid cityId = Guid.Parse(Convert.ToString(context.Request.RouteValues["cityid"])!);
        await context.Response.WriteAsync($"City  information {cityId}");
    });


    //sales-report/2030/quater
    //Using custom regex
    //Checking values from the new custom class from regex
    endpoints.Map("sales-report/{year:int:min(1900)}/{month:months}", async context =>
    {

        int year = Convert.ToInt32(context.Request.RouteValues["year"]);
        string? month = Convert.ToString(context.Request.RouteValues["month"]);
        if (month == "apr" || month == "jul" || month == "oct" || month == "jan")
        {
            await context.Response.WriteAsync($"Sales report - {year} - {month}");
        }
        else
        {
            //Showing client that he can only use one of the month up there
            await context.Response.WriteAsync($"{month} is not allowed for sales report");
        }
    });


    //Specific year and month will be execute
    endpoints.Map("sales-report/2024/jan", async context =>
    {

        await context.Response.WriteAsync("Sales report exclusively for 2024 - jan");
    });
});



//Default route if there is no matching parameters
app.Run(async context =>
{
    await context.Response.WriteAsync($"Request recivec at {context.Request.Path} ");
});
app.Run();
