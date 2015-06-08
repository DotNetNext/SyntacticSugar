using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.CSharp;
using System.Reflection;
using System.CodeDom.Compiler;
using System.Data;

namespace SyntacticSugar
{

    public class CreateReflection
    {
        public static Assembly NewAssembly()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("using System.Linq;");
            sb.Append("using System;");
            sb.Append("using System.Data;");
            sb.Append("using System.Collections.Generic;");
            sb.Append("namespace Test");
            sb.Append("{");
            sb.Append("public class Class1");
            sb.Append(@"{
   public Class1(){}

");

            sb.Append("public  List<aT> GetList(DataTable dt)");
           sb.Append("{"); 

            sb.Append(@"
            List<aT> reval=new List<aT> ();
            foreach(DataRow dr in dt.Rows){
                
         aT a=new aT();
         a.key=dr[""key""].ToString();
         a.value=dr[""value""].ToString();
         reval.Add(a);
            
            }
return reval;
            
            ");

            sb.Append("}");

            sb.Append("}");
            sb.Append("}");

            CSharpCodeProvider cs = new CSharpCodeProvider();

            CompilerParameters cp = new CompilerParameters();
            cp.GenerateExecutable = false;
            cp.OutputAssembly ="Test.dll";
            cp.ReferencedAssemblies.Add("System.dll");
            cp.ReferencedAssemblies.Add("System.Core.dll");
            cp.ReferencedAssemblies.Add("System.Data.dll");
            var assembly = cs.CompileAssemblyFromSource(cp, sb.ToString()).CompiledAssembly;

 

            return assembly;
        }
        private static string propertyString(string propertyName)
        {
            StringBuilder sbProperty = new StringBuilder();
            sbProperty.Append(" private   int   _" + propertyName + "   =   0;\n");
            sbProperty.Append(" public   int   " + "" + propertyName + "\n");
            sbProperty.Append(" {\n");
            sbProperty.Append(" get{   return   _" + propertyName + ";}   \n");
            sbProperty.Append(" set{   _" + propertyName + "   =   value;   }\n");
            sbProperty.Append(" }");
            return sbProperty.ToString();
        }
    }
}
