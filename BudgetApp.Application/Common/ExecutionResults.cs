using System.Diagnostics;

namespace BudgetApp.Core.Common
{
    [DebuggerDisplay("IsSuccess: {Success}")]
    public class ExecutionResult
    {
        public ExecutionResult()
            : this((ExecutionResult)null)
        {
        }

        public ExecutionResult(ErrorInfo error)
            : this((ExecutionResult)null)
        {
            Errors.Add(error);
        }
        
        public ExecutionResult(IEnumerable<ErrorInfo> errors)
            : this((ExecutionResult)null)
        {
            foreach (ErrorInfo errorInfo in errors)
            {
                Errors.Add(errorInfo);
            }
        }

        public ExecutionResult(ExecutionResult result)
        {
            Errors = new List<ErrorInfo>();
            if (result == null)
            {
                return;
            }
            foreach (ErrorInfo errorInfo in result.Errors)
            {
                Errors.Add(errorInfo);
            }
            Success = result.Success;
        }

        private bool? success;

        /// <summary>
        ///     Indicates if result is successful.
        /// </summary>
        public bool Success
        {
            get { return this.success == true || Errors.Count == 0; }
            set { this.success = value; }
        }

        /// <summary>
        /// 	Gets list of errors.
        /// </summary>
        public IList<ErrorInfo> Errors { get; set; }

        public string ErrorTexts
        {
            get { return Errors != null ? string.Join(", ", Errors.Select(e => e.Message)) : string.Empty; }
        }
    }

    /// <summary>
    /// Represents result of an action that returns any value
    /// </summary>
    /// <typeparam name="T">Type of value to be returned with action</typeparam>
    [DebuggerDisplay("IsSuccess: {Success}")]
    public class ExecutionResult<T> : ExecutionResult
    {
        public ExecutionResult()
            : this((ExecutionResult)null)
        {
        }

        public ExecutionResult(T result)
            : this((ExecutionResult)null)
        {
            Value = result;
        }
        
        public ExecutionResult(ErrorInfo error)
            : this((ExecutionResult)null)
        {
            Errors.Add(error);
        }
        
        public ExecutionResult(IEnumerable<ErrorInfo> errors)
            : this((ExecutionResult)null)
        {
            foreach (ErrorInfo errorInfo in errors)
            {
                Errors.Add(errorInfo);
            }
        }

        public ExecutionResult(ExecutionResult result)
            : base(result)
        {
            if (result is ExecutionResult<T> r)
            {
                Value = r.Value;
            }
        }


        public T Value { get; set; }
    }
}
