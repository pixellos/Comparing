using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataContainersBenchmark
{
    public static class Service
    {
        public static Result SomeServiceCall()
        {
            return new SucceededResult("Result");
        }

        public static SucceededResult SomeNullServiceCall()
        {
            return new SucceededResult("Result");
        }

        public static SucceededResult SomeNullWithExceptionLogicDrivenServiceCallThrow()
        {
            throw new FailedException("Result");
        }
        public static SucceededResult SomeNullWithExceptionLogicDrivenServiceCall()
        {
            return new SucceededResult("Result");
        }
    }
}
