using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEditor;
using System.Reflection;

static class InterfaceGenerator
{
	private static Dictionary<string, string> typeConversion = new Dictionary<string, string>
	{
		{ "System.Void", "void" }
	};

	private static string Convert(System.Type input)
	{
		string returnString = input.ToString();
		if (typeConversion.ContainsKey(returnString))
		{
			returnString = typeConversion[returnString];
		}
		return returnString;
	}

	public static void Generate(System.Type classType)
	{
		StringBuilder sb = new StringBuilder();

		string className = "I" + classType.Name;

		//write file
		Include(sb);

		Class(sb, className);

		//send file to disk
		Debug.Log(sb.ToString());
		StreamWriter sw = new StreamWriter(Path.Combine(Application.dataPath, "Scripts/Generated/" + className + ".cs"));
		sw.Write(sb);
		sw.Flush();
		sw.Close();

		AssetDatabase.Refresh();
	}

	private static void Include(StringBuilder sb)
	{
		sb.AppendLine("using System.Collections;");
		sb.AppendLine("using System.Collections.Generic;");
		sb.AppendLine("using UnityEngine;");
		sb.AppendLine("");
	}

	private static void Class(StringBuilder sb, string className)
	{
		sb.AppendLine("public interface " + className + "\t{");

		MethodInfo[] methods = typeof(ExampleClass).GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);
		foreach (var method in methods)
		{
			if (method.GetParameters().Length == 0)
			{
				sb.AppendLine("\t" + Convert(method.ReturnType) + " " + method.Name + "();");
			}
			else
			{
				sb.AppendLine("\t" + Convert(method.ReturnType) + " " + method.Name + "(" + method.GetParameters()[0] + ");");
			}
		}

		sb.AppendLine("}");
	}
}