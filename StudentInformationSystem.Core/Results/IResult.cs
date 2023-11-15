using StudentInformationSystem.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentInformationSystem.Core.Results
{
    public interface IResult
    {
        string Message { get; }
        Exception Exception { get; }
        ResultStatus ResultStatus { get; }
    }
}
