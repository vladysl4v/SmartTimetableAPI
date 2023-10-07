using System.Runtime.CompilerServices;

namespace WebTimetable.Application.Exceptions;

public class InternalServiceException : Exception
{
    private readonly string _callerName;
    private readonly string _errorDetails;

    public string InformationForClient { get; init; }

    public InternalServiceException(string infoForClient, string errorDetails, [CallerMemberName] string callerName = "") : base(errorDetails)
    {
        InformationForClient = "Internal service error. " + infoForClient;
        _callerName = callerName;
        _errorDetails = errorDetails;
    }

    public InternalServiceException(Exception innerExpection, string infoForClient, string errorDetails, [CallerMemberName] string callerName = "") : base(errorDetails, innerExpection)
    {
        InformationForClient = "Internal service error. " + infoForClient;
        _callerName = callerName;
        _errorDetails = errorDetails;
    }

    public override string ToString()
    {
        return $"INTERNAL SERVICE ERROR OCCURED IN {_callerName}.\nPROVIDED INFORMATION: {_errorDetails}\n{base.ToString()}";
    }
}