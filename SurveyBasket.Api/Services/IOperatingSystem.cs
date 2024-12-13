namespace SurveyBasket.Api.Services;

public interface IOperatingSystem
{
    string RunApp();
    string OperationId { get; }
}

public interface IOperationTransient:IOperatingSystem { }
public interface IOperationScoped : IOperatingSystem { }
public interface IOperationSingleton : IOperatingSystem { }