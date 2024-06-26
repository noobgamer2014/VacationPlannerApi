﻿using System.Runtime.Serialization;

[Serializable]
internal class AppException : Exception
{
    public AppException()
    {
    }

    public AppException(string? message) : base(message)
    {
    }

    public AppException(string? message, Exception? innerException) : base(message, innerException)
    {
    }

    protected AppException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }

    public AppException(string message, int errorCode) : base(message)
    {
        this.ErrorCode = errorCode;
    }

    public int ErrorCode { get; set; }
}