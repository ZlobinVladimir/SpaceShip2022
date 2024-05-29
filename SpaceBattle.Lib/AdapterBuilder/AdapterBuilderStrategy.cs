namespace SpaceBattle.Lib;
using System.Reflection;
public class AdapterBuilderStrategy: IStrategy
{
    public object StartStrategy(params object[] args)
    {
        Type adapterType = (Type) args[0];
        Type targetType = (Type) args[1];
        AdapterBuilder builder = new AdapterBuilder(adapterType, targetType);
        PropertyInfo[] properties = adapterType.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

        foreach(PropertyInfo p in properties)
        {
            builder.CreateProperty(p);
        }

        IEnumerable<MethodInfo> methods = adapterType.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic).Where(m => !m.IsSpecialName);
        foreach(MethodInfo m in methods)
        {
            builder.CreateMethod(m);
        }
        return builder.Build();
    }
}