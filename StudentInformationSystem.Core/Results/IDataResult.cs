using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentInformationSystem.Core.Results
{
    public interface IDataResult<out T> : IResult
    {
        public T Data { get; }
    }
}
