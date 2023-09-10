using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.DurableTask;
using Microsoft.DurableTask.Client;

namespace TBC.Samples.SimpleDurableFunction
{
    public static class HelloWorld
    {
        [Function(nameof(HelloWorld))]
        public static async Task<string> RunOrchestrator(
            [OrchestrationTrigger] TaskOrchestrationContext context)
        {
            var name = context.GetInput<string>();

            await context.CreateTimer(TimeSpan.FromSeconds(Random.Shared.Next(45, 300)), CancellationToken.None);

            return await context.CallActivityAsync<string>(nameof(SayHello), name);
        }

        [Function(nameof(SayHello))]
        public static string SayHello([ActivityTrigger] string name, FunctionContext executionContext)
        {
            return $"Hello {name}!";
        }

        [Function("HelloWorld_HttpStart")]
        public static async Task<HttpResponseData> HttpStart(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = "sayhello")] HttpRequestData req,
            [DurableClient] DurableTaskClient client,
            FunctionContext executionContext)
        {
            var parameter = await req.ReadFromJsonAsync<SayHelloTo>();
            
            // Function input comes from the request content.
            string instanceId = await client.ScheduleNewOrchestrationInstanceAsync(
                nameof(HelloWorld), parameter.Name);

            // Returns an HTTP 202 response with an instance management payload.
            // See https://learn.microsoft.com/azure/azure-functions/durable/durable-functions-http-api#start-orchestration
            return client.CreateCheckStatusResponse(req, instanceId);
        }
    }

    public record SayHelloTo(string Name);
}
