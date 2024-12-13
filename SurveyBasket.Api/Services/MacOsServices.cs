namespace SurveyBasket.Api.Services;

public class MacOsServices : IOperationTransient, IOperationScoped, IOperationSingleton
{
    public string OperationId { get; }
    public MacOsServices()
    {
        OperationId = Guid.NewGuid().ToString();
    }

    public string RunApp() => "This App is Running on MacOS";
}
