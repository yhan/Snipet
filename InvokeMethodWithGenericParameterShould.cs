namespace My.Tests
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using NFluent;
    using NUnit.Framework;

    public class InvokeMethodWithGenericParameterShould
    {
        [Test]
        public void Get_method_with_generic_parameters()
        {
            MethodInfo ploofMethod = GetType()
                .GetMethods()
                .Where(x => x.Name == "Ploof")
                .Select(methodInfo => new { MethodInfo = methodInfo, P = methodInfo.GetParameters() })
                .Where(x => x.P.Length == 1 && x.P[0]
                                .ParameterType.IsGenericType && x.P[0]
                                .ParameterType.GetGenericTypeDefinition() == typeof(IEnumerable<>))
                //.Select(x => new { x.MethodInfo, Arguments = x.P[0].ParameterType.GetGenericArguments() })
                //.Where(x => x.Arguments[0].IsGenericParameter && x.Arguments[0] == typeof(int))
                .Select(x => x.MethodInfo)
                .SingleOrDefault();

            Check.That(ploofMethod).IsNotNull();

            MethodInfo bindedMethod = ploofMethod.MakeGenericMethod(typeof(int));


            var result = bindedMethod.Invoke(this, new[] { new int[] { 42, 100, 3 } });

            Check.That(result).IsEqualTo("3");
        }

        [Test]
        public void Get_method_with_generic_parameters_v2()
        {
            var bindedMethod = this.GetType().GetMethod("Ploof").MakeGenericMethod(typeof(int));
            var result = bindedMethod.Invoke(this, new[] { new int[] { 42, 100, 3 } });

            Check.That(result).IsEqualTo("3");
        }
        

        public string Ploof<T>(IEnumerable<T> source)
        {
            return source.Count().ToString();
        }

        public string Ploof<T>(IEnumerable<T> source, string break2ndTest)
        {
            return source.Count().ToString();
        }
    }
}
