using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Runtime;
using System.Reflection;
using Microsoft.CSharp;
using System.CodeDom.Compiler;

namespace DragonNest.ResourceInspection.dnt.Test
{
    public class LinqSandbox
    {
        public static IEnumerable<DataRow> Execute(string command, IEnumerable<DataRow> Rows)
        {
            var codeProvider = new CSharpCodeProvider();
            var compilerParams = new CompilerParameters()
            {
                GenerateExecutable = false,
                GenerateInMemory = true
            };

            string source = CreateExecuteMethodTemplate(command);
            compilerParams.ReferencedAssemblies.Add("System.dll");
            compilerParams.ReferencedAssemblies.Add("System.Linq.dll");
            compilerParams.ReferencedAssemblies.Add("System.Data.dll");
            compilerParams.ReferencedAssemblies.Add("System.Core.dll");

            var compilerResults = codeProvider.CompileAssemblyFromSource(compilerParams, source);
            if (compilerResults.Errors.HasErrors)
            {
                List<Exception> exceptionList = new List<Exception>();
                foreach (CompilerError err in compilerResults.Errors)
                    exceptionList.Add(new Exception(err.ErrorText));
                throw new AggregateException(exceptionList);
            };
            Assembly assembly = compilerResults.CompiledAssembly;
            
            Type type = assembly.GetType("Lab.Cal");
            MethodInfo methodInfo = type.GetMethod("Execute", BindingFlags.Static | BindingFlags.Public);
            return (IEnumerable<DataRow>)methodInfo.Invoke(null, new object [] {Rows});
        }
        private static string CreateExecuteMethodTemplate(string linq)
        {
            var builder = new StringBuilder();

            builder.Append("using System;");
            builder.Append("using System.Linq;");
            builder.Append("using System.Data;");
            builder.Append("using System.Collections.Generic;");

            builder.Append("\r\nnamespace Lab");
            builder.Append("\r\n{");
            builder.Append("\r\npublic sealed class Cal");
            builder.Append("\r\n{");
            builder.Append("\r\npublic static IEnumerable<DataRow> Execute(IEnumerable<DataRow> Rows)");
            builder.Append("\r\n{");
            builder.AppendFormat("\r\nreturn {0};", linq);
            builder.Append("\r\n}");
            builder.Append("\r\n}");
            builder.Append("\r\n}");

            return builder.ToString();
        }



    }
}
