using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

/*
*	AttributerLibrary v1.0
*	Updated On: 21/02/2023 (dd/MM/yyyy)
*	By Fatih KAZANCI
*   Licenced By GNU General Public License v3.0
*   https://github.com/fatihkazanci/attributer/blob/main/LICENSE
*/

namespace AttributerLibrary
{
    public class AttributerPatcher<IService, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] Service> : DispatchProxy
    {
        public Service? TargetService { get; private set; }
        private IServiceProvider? ServiceProvider { get; set; }
        public static IService Get(IServiceProvider serviceProvider)
        {
            AttributerPatcher<IService, Service>? attriPatcher = Create<IService, AttributerPatcher<IService, Service>>()
             as AttributerPatcher<IService, Service>;

            ParameterInfo[]? targetMethodParams = typeof(Service).GetConstructors()?[0].GetParameters();
            int targetMethodParamsLength = 0;
            if (targetMethodParams?.Length > 0)
            {
                targetMethodParamsLength = targetMethodParams.Length;
            }
            object[] requiredServices = new object[targetMethodParamsLength];
            if (targetMethodParams is not null)
            {
                for (int i = 0; i < targetMethodParamsLength; i++)
                {
                    object? currentService = serviceProvider.GetService(targetMethodParams[i].ParameterType);
                    if (currentService is not null)
                    {
                        requiredServices[i] = currentService;
                    }
                }
            }
            if (attriPatcher is not null)
            {
                attriPatcher.TargetService = (Service?)Activator.CreateInstance(typeof(Service), requiredServices);
                attriPatcher.ServiceProvider = serviceProvider;
            }
            object? proxy = attriPatcher;
            return (IService)proxy;
        }
        public static IService Create(params object[] args)
        {
            AttributerPatcher<IService, Service>? attriPatcher = Create<IService, AttributerPatcher<IService, Service>>()
             as AttributerPatcher<IService, Service>;
            if (attriPatcher is not null && args is not null && args.Count() > 0)
            {
                attriPatcher.TargetService = (Service?)Activator.CreateInstance(typeof(Service), args);
            }
            else if (attriPatcher is not null)
            {
                attriPatcher.TargetService = (Service?)Activator.CreateInstance(typeof(Service));
            }
            object? proxy = attriPatcher;
            return (IService)proxy;
        }
        protected override object? Invoke(MethodInfo? targetMethod, object?[]? args)
        {
            object? result = null;
            if (targetMethod is not null && args is not null)
            {
                AttributerContext dispatcherContext = new()
                {
                    Arguments = args,
                    Method = targetMethod,
                    ServiceProvider = this.ServiceProvider
                };

                Type? targetServiceType = TargetService?
                    .GetType()?
                    .GetTypeInfo();

                IAttributer[]? classAttributers = targetServiceType?
                    .GetCustomAttributes(typeof(IAttributer), true) as IAttributer[];

                IAttributer[]? methodAttributers = targetServiceType?
                    .GetMethod(targetMethod.Name, targetMethod.GetParameters().Select(i => i.ParameterType).ToArray())?
                    .GetCustomAttributes(typeof(IAttributer), true)
                    as IAttributer[];

                List<IAttributer> currentAttributers = new();

                if (classAttributers is not null && classAttributers.Length > 0)
                {
                    currentAttributers.AddRange(classAttributers);
                }
                if (methodAttributers is not null && methodAttributers.Length > 0)
                {
                    currentAttributers.AddRange(methodAttributers);
                }
                if (currentAttributers is not null)
                {
                    OnExecute(dispatcherContext, currentAttributers);
                }

                StackTrace stepStackTrace = new(true);
                List<MethodBase> trackingUserMethods = new();
                if (stepStackTrace is not null)
                {
                    IEnumerable<StackFrame> listStepStackTraceOnlyUserMethods = stepStackTrace
                    .GetFrames()
                    .Where(i => !string.IsNullOrEmpty(i.GetFileName()) && i.GetMethod()?.DeclaringType?.BaseType != typeof(DispatchProxy));
                    if (listStepStackTraceOnlyUserMethods is not null && listStepStackTraceOnlyUserMethods.Count() > 0)
                    {
                        foreach (StackFrame item in listStepStackTraceOnlyUserMethods)
                        {
                            MethodBase? currentMethod = item.GetMethod();
                            if (currentMethod is not null)
                            {
                                trackingUserMethods.Add(currentMethod);
                            }
                        }
                        dispatcherContext.CallingParentMethods = trackingUserMethods.ToArray();
                    }
                }

                if (dispatcherContext.Result)
                {
                    Exception? currentException = null;
                    try
                    {
                        result = targetMethod.Invoke(this.TargetService, args);
                        if (result is Task)
                        {
                            currentException = ((Task)result).Exception;
                        }
                    }
                    catch (TargetInvocationException ex)
                    {
                        currentException = ex.InnerException;
                    }
                    finally
                    {
                        if (currentException is not null)
                        {
                            StackTrace exceptionStackTrace = new StackTrace(currentException, true);
                            StackFrame? exceptionCurrentStackTrace = exceptionStackTrace.GetFrame(0);
                            MethodBase? errorMethod = exceptionCurrentStackTrace?.GetMethod();
                            int? exceptionLine = exceptionCurrentStackTrace?.GetFileLineNumber();

                            List<MethodBase> callingMethods = new();
                            IEnumerable<StackFrame>? tempCallingMethods = exceptionStackTrace
                                .GetFrames()
                                .Where(i => !string.IsNullOrEmpty(i.GetFileName()));
                            if (tempCallingMethods is not null && tempCallingMethods.Count() > 0)
                            {
                                foreach (StackFrame? item in tempCallingMethods)
                                {
                                    MethodBase? currentMethod = item.GetMethod();
                                    if (currentMethod is not null && currentMethod.Name != errorMethod?.Name)
                                    {
                                        callingMethods.Add(currentMethod);
                                    }
                                }
                                callingMethods.AddRange(trackingUserMethods);
                            }

                            dispatcherContext.Error = new()
                            {
                                Exception = currentException,
                                ErrorLine = exceptionLine,
                                CallingParentMethods = callingMethods?.ToArray(),
                                ErrorMethod = errorMethod
                            };
                            result = Activator.CreateInstance(targetMethod.ReturnType);
                        }
                    }
                }

                if (dispatcherContext.Result && currentAttributers is not null)
                {
                    OnExecuted(dispatcherContext, currentAttributers);
                }
            }
            return result;
        }
        public void OnExecute(AttributerContext dispatcherContext, List<IAttributer> attributers)
        {
            if (attributers is not null && attributers.Count > 0)
            {
                foreach (IAttributer item in attributers)
                {
                    item.OnBeforeExecute(dispatcherContext);
                }
            }
        }
        public void OnExecuted(AttributerContext dispatcherContext, List<IAttributer> attributers)
        {
            if (attributers is not null && attributers.Count > 0)
            {
                foreach (IAttributer item in attributers)
                {
                    item.OnAfterExecuted(dispatcherContext);
                }
            }
        }
    }
}
