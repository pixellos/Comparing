using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DataContainersBenchmark
{
    public partial class Program
    {

        [Benchmark]
        public static string IsHard()
        {
            var result = Service.SomeServiceCall();
            if (result is SucceededResult)
            {
                return ((SucceededResult)result).Result;
            }
            else if (result is FailedResult)
            {
                return ((FailedResult)result).FailureReason;
            }
            else
            {
                return result.SomeData;
            }
        }

        [Benchmark]
        public static string TryCatchCheck()
        {
            try
            {
                var result = Service.SomeNullWithExceptionLogicDrivenServiceCall();
                if (result != null)
                {
                    return result.SomeData;
                }
            }
            catch (FailedException fe)
            {
                return fe.FailureReason;
            }
            return "Something goes wrong.";
        }

        [Benchmark]
        public static string TryCatchCheckThrows()
        {
            try
            {
                var result = Service.SomeNullWithExceptionLogicDrivenServiceCallThrow();
                if (result != null)
                {
                    return result.SomeData;
                }
            }
            catch (FailedException fe)
            {
                return fe.FailureReason;
            }
            return "Something goes wrong.";
        }



        [Benchmark]
        public static string NullCheck()
        {
            var result = Service.SomeNullServiceCall();
            if (result != null)
            {
                return result.SomeData;
            }
            return "Something goes wrong.";
        }

        [Benchmark]
        public static string AsNull()
        {
            var result = Service.SomeServiceCall();
            var succeededResult = result as SucceededResult;
            if (succeededResult != null)
            {
                return succeededResult.Result;
            }
            var failedResult = result as FailedResult;
            if (failedResult != null)
            {
                return failedResult.FailureReason;
            }
            return result.SomeData;
        }

        [Benchmark]
        public static string IsAs()
        {
            var result = Service.SomeServiceCall();
            if (result is SucceededResult)
            {
                return (result as SucceededResult).Result;
            }
            else if (result is FailedResult)
            {
                return (result as FailedResult).FailureReason;
            }
            else
            {
                return result.SomeData;
            }
        }

        [Benchmark]
        public static string CSharp7IsSwitch()
        {
            var result = Service.SomeServiceCall();
            switch (result)
            {
                case FailedResult fr:
                    return fr.FailureReason;
                case SucceededResult succeeded:
                    return succeeded.Result;
                default:
                    return result.SomeData;
            }
        }

        [Benchmark]
        public static string CSharp7IsIfElse()
        {
            var result = Service.SomeServiceCall();
            if (result is SucceededResult succeeded)
            {
                return succeeded.Result;
            }
            else if (result is FailedResult fr)
            {
                return fr.FailureReason;
            }
            else
            {
                return result.SomeData;
            }
        }

        static void Main(string[] args)
        {
            BenchmarkSwitcher.FromTypes(new[] { typeof(Program) }).RunAll();
            Console.ReadKey();
        }
    }
}
