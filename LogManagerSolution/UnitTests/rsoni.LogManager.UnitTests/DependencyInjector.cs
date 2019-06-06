using Microsoft.Practices.Unity.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Unity;
using Unity.Lifetime;
using Unity.Resolution;

namespace rsoni.LogManager.UnitTests
{
    public static class DependencyInjector
    {
        private static readonly UnityContainer UnityContainer = new UnityContainer();

        public static void Register<I, T>() where T : I
        {
            UnityContainer.RegisterType<I, T>(new ContainerControlledLifetimeManager());
        }

        public static void RegisterType(string interfaceName, string className)
        {
            string assemblyFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string classDLLPath = Path.Combine(assemblyFolder, className.Substring(0, className.LastIndexOf('.')) + ".dll");
            string interfaceDLLPath = Path.Combine(assemblyFolder, interfaceName.Substring(0, interfaceName.LastIndexOf('.')) + ".dll");

            var classDLL = Assembly.LoadFile(classDLLPath);
            var classType = classDLL.GetType(className);
            var InterfaceDLL = Assembly.LoadFile(interfaceDLLPath);
            var interfaceType = InterfaceDLL.GetType(interfaceName);
            UnityContainer.RegisterType(interfaceType, classType);
        }

        public static void InjectStub<I>(I instance)
        {
            UnityContainer.RegisterInstance(instance, new ContainerControlledLifetimeManager());
        }

        public static T Retrieve<T>()
        {
            return UnityContainer.Resolve<T>();
        }

        public static T Retrieve<T>(string ParameterName1, object ParameterValue1)
        {
            return UnityContainer.Resolve<T>(new ResolverOverride[]
                                   {
                                       new ParameterOverride(ParameterName1, ParameterValue1)
                                   });
        }

        public static T Retrieve<T>(string ParameterName1, object ParameterValue1, string ParameterName2, object ParameterValue2)
        {
            return UnityContainer.Resolve<T>(new ResolverOverride[]
                                   {
                                       new ParameterOverride(ParameterName1, ParameterValue1),
                                       new ParameterOverride(ParameterName2, ParameterValue2)
                                   });
        }

        public static T Retrieve<T>(string ParameterName1, object ParameterValue1, string ParameterName2, object ParameterValue2, string ParameterName3, object ParameterValue3)
        {
            return UnityContainer.Resolve<T>(new ResolverOverride[]
                                   {
                                       new ParameterOverride(ParameterName1, ParameterValue1),
                                       new ParameterOverride(ParameterName2, ParameterValue2),
                                       new ParameterOverride(ParameterName3, ParameterValue3)
                                   });
        }

        public static T Retrieve<T>(string ParameterName1, object ParameterValue1, string ParameterName2, object ParameterValue2
            , string ParameterName3, object ParameterValue3
            , string ParameterName4, object ParameterValue4
            , string ParameterName5, object ParameterValue5
            , string ParameterName6, object ParameterValue6
            )
        {
            return UnityContainer.Resolve<T>(new ResolverOverride[]
                                   {
                                       new ParameterOverride(ParameterName1, ParameterValue1),
                                       new ParameterOverride(ParameterName2, ParameterValue2),
                                       new ParameterOverride(ParameterName3, ParameterValue3),
                                       new ParameterOverride(ParameterName4, ParameterValue4),
                                       new ParameterOverride(ParameterName5, ParameterValue5),
                                       new ParameterOverride(ParameterName6, ParameterValue6)
                                   });
        }


        public static T GetOtherClasses<T>(string registerName, string parameterName1 = null, object parameterValue1 = null
                                                            , string parameterName2 = null, object parameterValue2 = null
                                                            , string parameterName3 = null, object parameterValue3 = null
                                                            , string parameterName4 = null, object parameterValue4 = null
                                                            , string parameterName5 = null, object parameterValue5 = null
                                                            , string parameterName6 = null, object parameterValue6 = null)
        {

            return GetClassesFromUnity<T>("Others", registerName, parameterName1, parameterValue1
                                                            , parameterName2, parameterValue2
                                                            , parameterName3, parameterValue3
                                                            , parameterName4, parameterValue4
                                                            , parameterName5, parameterValue5
                                                            , parameterName6, parameterValue6);

        }

        public static T GetProcessEnrollmentClass<T>(string registerName, string parameterName1 = null, object parameterValue1 = null
                                                           , string parameterName2 = null, object parameterValue2 = null
                                                           , string parameterName3 = null, object parameterValue3 = null
                                                           , string parameterName4 = null, object parameterValue4 = null
                                                           , string parameterName5 = null, object parameterValue5 = null
                                                           , string parameterName6 = null, object parameterValue6 = null
                                                           , string parameterName7 = null, object parameterValue7 = null)
        {

            return GetClassesFromUnity<T>("ProcessEnrollment", registerName, parameterName1, parameterValue1
                                                            , parameterName2, parameterValue2
                                                            , parameterName3, parameterValue3
                                                            , parameterName4, parameterValue4
                                                            , parameterName5, parameterValue5
                                                            , parameterName6, parameterValue6
                                                            , parameterName7, parameterValue7);

        }

        public static T GetProductCodeClass<T>(string registerName, string parameterName1 = null, object parameterValue1 = null
                                                           , string parameterName2 = null, object parameterValue2 = null
                                                           , string parameterName3 = null, object parameterValue3 = null
                                                           , string parameterName4 = null, object parameterValue4 = null
                                                           , string parameterName5 = null, object parameterValue5 = null
                                                           , string parameterName6 = null, object parameterValue6 = null)
        {

            return GetClassesFromUnity<T>("ProductCode", registerName, parameterName1, parameterValue1
                                                            , parameterName2, parameterValue2
                                                            , parameterName3, parameterValue3
                                                            , parameterName4, parameterValue4
                                                            , parameterName5, parameterValue5
                                                            , parameterName6, parameterValue6);

        }


        public static T GetRegStateClasse<T>(string registerName, string parameterName1 = null, object parameterValue1 = null
                                                           , string parameterName2 = null, object parameterValue2 = null
                                                           , string parameterName3 = null, object parameterValue3 = null
                                                           , string parameterName4 = null, object parameterValue4 = null
                                                           , string parameterName5 = null, object parameterValue5 = null
                                                           , string parameterName6 = null, object parameterValue6 = null)
        {

            return GetClassesFromUnity<T>("RegState", registerName, parameterName1, parameterValue1
                                                            , parameterName2, parameterValue2
                                                            , parameterName3, parameterValue3
                                                            , parameterName4, parameterValue4
                                                            , parameterName5, parameterValue5
                                                            , parameterName6, parameterValue6);

        }

        static UnityContainer myContainer = null;
        static UnityConfigurationSection mySection = null;

        public static void Init()
        {
            myContainer = new UnityContainer();
            mySection = (UnityConfigurationSection)System.Configuration.ConfigurationManager.GetSection("unity");
            myContainer.AddExtension(new Diagnostic());
        }


        public static UnityContainer GetUnityContainer(string sectionName)
        {
            mySection.Configure(myContainer, sectionName);
            return myContainer;
        }

        public static T GetClassesFromUnity<T>(string sectionName, string registerName, string parameterName1 = null, object parameterValue1 = null
                                                            , string parameterName2 = null, object parameterValue2 = null
                                                            , string parameterName3 = null, object parameterValue3 = null
                                                            , string parameterName4 = null, object parameterValue4 = null
                                                            , string parameterName5 = null, object parameterValue5 = null
                                                            , string parameterName6 = null, object parameterValue6 = null
                                                            , string parameterName7 = null, object parameterValue7 = null
                                                            , string parameterName8 = null, object parameterValue8 = null)
        {

            List<ResolverOverride> lstResolverOverride = new List<ResolverOverride>();
            if (parameterName1 != null)
                lstResolverOverride.Add(new ParameterOverride(parameterName1, parameterValue1));

            if (parameterName2 != null)
                lstResolverOverride.Add(new ParameterOverride(parameterName2, parameterValue2));

            if (parameterName3 != null)
                lstResolverOverride.Add(new ParameterOverride(parameterName3, parameterValue3));

            if (parameterName4 != null)
                lstResolverOverride.Add(new ParameterOverride(parameterName4, parameterValue4));

            if (parameterName5 != null)
                lstResolverOverride.Add(new ParameterOverride(parameterName5, parameterValue5));

            if (parameterName6 != null)
                lstResolverOverride.Add(new ParameterOverride(parameterName6, parameterValue6));

            if (parameterName7 != null)
                lstResolverOverride.Add(new ParameterOverride(parameterName7, parameterValue7));

            if (parameterName8 != null)
                lstResolverOverride.Add(new ParameterOverride(parameterName8, parameterValue8));


            T returnObject = GetUnityContainer(sectionName).Resolve<T>(registerName, lstResolverOverride.ToArray());


            return returnObject;
        }


    }
}
