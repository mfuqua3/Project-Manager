//Based on Standardized API Error Model from .NET 6 Boilerplate
//https://github.com/mfuqua3/DotNet6-Boilerplate/blob/master/Utility/DataContracts/Models/ExceptionModel.cs
export interface ProblemDetails {
    type: string;
    title: string;
    status: number;
    detail?: string;
    instance?: string;
    traceId?: string;
    error?: ExceptionDetails;
}

export interface ExceptionDetails {
    name: string;
    message: string;
    stackTrace: string;
}
