namespace NutritionService.Comman.Results
{
    public sealed record Error(string Code, string Description, int StatusCode = 400)
    {
        public static readonly Error None = new(string.Empty, string.Empty, 200);
        public static Error Validation(string code, string description) => new Error(code, description, 400);
        public static Error NotFound(string code, string description) => new Error(code, description, 404);
        public static Error Confilt(string code, string description)    => new Error(code, description, 409);
        public static Error UnProcessable(string code, string description) => new Error(code, description, 422);
    }
}
