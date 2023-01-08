using System.Text.Json.Serialization;
using BudgetApp.Domain.Common;

namespace BudgetApp.Domain
{
    public class ErrorInfo
    {
        public ErrorCode ErrorCode { get; }
        public MessageCode MessageCode { get; }
        
        /// <summary>
        /// Code of error
        /// </summary>
        public string Code { get; }

        /// <summary>
        /// Error message
        /// </summary>
        public string Message { get; set; } 

        public string TranslateKey { get; }
        
        /// <summary>
        /// When returning from API, please use ErrorInfo(ErrorMessageParameters parameters) instead. This should be only used when the ExecutionResult is not returned anywhere.
        /// </summary>
        public ErrorInfo(string errorMessage)
            : this(Guid.NewGuid().ToString(), errorMessage)
        {
        }

        public ErrorInfo(ErrorCode code, MessageCode message)
        {
            ErrorCode = code;
            MessageCode = message;
        }
        
        /// <summary>
        /// When returning from API, please use ErrorInfo(ErrorMessageParameters parameters) instead. This should be only used when the ExecutionResult is not returned anywhere.
        /// </summary>
        public ErrorInfo(string code, string message)
        {
            Code = code;
            Message = message;
            TranslateKey = code;
        }

        [JsonConstructor]
        public ErrorInfo(ErrorCode errorCode, MessageCode messageCode, string code, string message, string translateKey)
        {
            ErrorCode = errorCode;
            MessageCode = messageCode;
            Code = code;
            Message = message;
            TranslateKey = translateKey;
        }
    }
}
