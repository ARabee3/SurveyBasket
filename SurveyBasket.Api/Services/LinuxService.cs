namespace SurveyBasket.Api.Services;

public class LinuxService  : IOperationTransient, IOperationScoped, IOperationSingleton
{
    public string OperationId => throw new NotImplementedException();

    public string RunApp() => "This app is running on Linux";
}
