using HttpResults = Microsoft.AspNetCore.Http.Results;
namespace NutritionService.Comman.Results
{
    public static class ResultHttpExtensions
    {
        public static IResult ToHttpError(this Error error)
        {
            return error.StatusCode switch
            {
                400 => HttpResults.BadRequest(error),
                404 => HttpResults.NotFound(error),
                409 => HttpResults.Conflict(error),
                422 => HttpResults.UnprocessableEntity(
                    error),
                _ => HttpResults.Json(
                    error,
                    statusCode: error.StatusCode)
            };
        }
    }
}
