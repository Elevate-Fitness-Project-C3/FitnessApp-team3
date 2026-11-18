using System.Net;

namespace NutritionService.Services.FitnessProfile
{
    public class FakeFitnessProfileHttpHandler : DelegatingHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var fakePayload = new
            {
                Tdee = 200m,
                CalculatedAt = DateTime.UtcNow.AddDays(-5)
            };
            var response = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = JsonContent.Create(fakePayload)
            };
            return Task.FromResult(response);
        }
    }
}
