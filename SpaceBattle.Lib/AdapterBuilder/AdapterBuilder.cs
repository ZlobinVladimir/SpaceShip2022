namespace SpaceBattle.Lib;
using System.Reflection;
using System.Text.RegularExpressions;
public class AdapterBuilder {
    private string adaptername;
    private string adaptertypename;
    private string targettypename;
    private string? properties;
    private string? methods;

    public AdapterBuilder (Type adaptertype, Type targettype) {
        this.adaptername = adaptertype.Name.Substring(1);
        this.adaptertypename = adaptertype.Name;
        this.targettypename = FormatGenericType(targettype);
    }

    public void CreateMethod(MethodInfo m) {
        string methodreturnname = FormatGenericType(m.ReturnType);
        string sparameters, sparametersnames;
            
        ParameterInfo[] parameters = m.GetParameters();

        if(parameters.Length == 0) {
            sparameters = string.Empty;
            sparametersnames = string.Empty;
        }
        else {
            sparameters = ", ";
            sparametersnames = ", ";
            foreach (ParameterInfo p in parameters) {
                sparameters += @$"{FormatGenericType(p.ParameterType)} {p.Name}, ";  
                sparametersnames += @$"{p.Name}, ";
            }
            sparameters = sparameters.Remove(sparameters.Length-2);
            sparametersnames = sparametersnames.Remove(sparametersnames.Length-2);
        }

        if (methodreturnname == "Void") {
            methods +=
        $@"
        public void {m.Name} ({sparameters.Substring(2)}) {{
            IoC.Resolve<ICommand>(""{m.Name}.Command"", target{sparametersnames}).Execute();
        }}
            ";
        }
        else {
        methods +=
        $@"
        public {methodreturnname} {m.Name} ({sparameters}) {{
            return IoC.Resolve<{methodreturnname}>(""{m.Name}.Strategy"", target{sparametersnames});
        }}
        ";
        }
    }

    public void CreateProperty(PropertyInfo p) {
        string get = string.Empty, set = string.Empty;

        string propertyname = FormatGenericType(p.PropertyType);

        if (p.CanRead) {
            get = $@"   get {{ return IoC.Resolve<{propertyname}>(""{p.Name}.Get"", target); }}";
        }

        if (p.CanWrite) {
            set = $@"   set {{ IoC.Resolve<ICommand>(""{p.Name}.Set"", target, value).Execute(); }}";
        }

        properties += 
        $@"
        public {propertyname} {p.Name} {{
            {get}
            {set}
        }}
        ";
    }

    private string FormatGenericType(Type type)
    {
        string typeName = type.Name; 

        if (type.IsGenericType)
        {
            typeName = typeName.Substring(0, typeName.IndexOf('`'));

            Type[] typeArgs = type.GetGenericArguments();
            string[] typeArgNames = new string[typeArgs.Length];
            for (int i = 0; i < typeArgs.Length; i++){
                typeArgNames[i] = typeArgs[i].Name;
            }
            typeName = $"{typeName}<{string.Join(", ", typeArgNames)}>";
        }
        return typeName;
    }

    public string Build() {
        string result = @$"class {adaptername}Adapter : {adaptertypename} {{
        {targettypename} target;
        public {adaptername}Adapter({targettypename} target) => this.target = target; 
        {properties}
        {methods}
    }}";

        result = Regex.Replace(result, @"^\s+$[\r\n]*", string.Empty, RegexOptions.Multiline);
        return result;
    }
}
