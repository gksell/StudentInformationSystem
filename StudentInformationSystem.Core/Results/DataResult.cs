using StudentInformationSystem.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentInformationSystem.Core.Results
{
    public class DataResult<T> : IDataResult<T>
    {
        public DataResult(ResultStatus resultstatus, T data)
        {
            ResultStatus = resultstatus;
            Data = data;
        }
        public DataResult(ResultStatus resultstatus, string message, T data)
        {
            ResultStatus = resultstatus;
            Message = message;
            Data = data;
        }
        public DataResult(ResultStatus resultstatus, string message, T data, Exception exception)
        {
            ResultStatus = resultstatus;
            Message = message;
            Data = data;
            Exception = exception;
        }

        public T Data { get; }

        public string Message { get; }

        public Exception Exception { get; }

        public ResultStatus ResultStatus { get; }
    }
}
