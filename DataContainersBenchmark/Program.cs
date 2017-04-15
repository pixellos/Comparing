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
    public class Program
    {
        public abstract class Result
        {
            public string SomeData { get; }
        }

        public class SucceededResult : Result
        {
            public string Result { get; }
            public SucceededResult(string result)
            {
                this.Result = result;
            }
        }

        public class FailedResult : Result
        {
            public string FailureReason { get; }
            public FailedResult(string message)
            {
                this.FailureReason = message;
            }
        }

        public class FailedException : Exception
        {
            public string FailureReason { get; }
            public FailedException(string failureReason)
            {
                this.FailureReason = failureReason;
            }
        }

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

        [Benchmark]
        public static string IsHard()
        {
            var result = Program.SomeServiceCall();
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
                var result = Program.SomeNullWithExceptionLogicDrivenServiceCall();
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
                var result = Program.SomeNullWithExceptionLogicDrivenServiceCallThrow();
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
            var result = Program.SomeNullServiceCall();
            if (result != null)
            {
                return result.SomeData;
            }
            return "Something goes wrong.";
        }

        [Benchmark]
        public static string AsNull()
        {
            var result = Program.SomeServiceCall();
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
            var result = Program.SomeServiceCall();
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
            var result = Program.SomeServiceCall();
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
            var result = Program.SomeServiceCall();
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
            AppDomain.CurrentDomain.FirstChanceException += CurrentDomain_FirstChanceException;
            BenchmarkSwitcher.FromTypes(new[] { typeof(Program) }).RunAll();
            Console.ReadKey();
        }

        private static void CurrentDomain_FirstChanceException(object sender, System.Runtime.ExceptionServices.FirstChanceExceptionEventArgs e)
        {
            //Some logging, windows log, saving to external api with big delay
            Thread.Sleep(1000);
        }
    }
}
