#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

/// <summary>
/// Generates strongly typed ResourceID references from the selected ResourceDatabase asset.
/// Place this file anywhere inside an Editor folder.
/// </summary>
public static class ResourceRefsGenerator
{
    private const string OutputPath = "Assets/Scripts/Generated/ResourceRefs.g.cs";

    // Change these if the serialized field names in ResourceDatabase/ResourceDefinition differ.
    private static readonly string[] DefinitionsPropertyNames =
    {
        "_items", "Items"
    };

    private static readonly string[] IdPropertyNames =
    {
        "_id", "ID", "Id", "id", "ResourceID", "_resourceID"
    };

    private static readonly string[] CodeNamePropertyNames =
    {
        "_codeName", "CodeName", "codeName", "_displayName", "DisplayName", "displayName", "_name", "Name"
    };

    [MenuItem("Tools/Game Data/Generate Resource References")]
    private static void Generate()
    {
        ResourceDatabase database = GetDatabase();
        if (database == null)
            return;

        SerializedObject serializedDatabase = new(database);
        SerializedProperty definitions = FindProperty(serializedDatabase, DefinitionsPropertyNames);

        if (definitions == null || !definitions.isArray)
        {
            Debug.LogError(
                $"Could not find the resource-definition list on '{database.name}'. " +
                "Update DefinitionsPropertyNames in ResourceRefsGenerator to match its serialized field name.");
            return;
        }

        List<GeneratedResource> resources = new();
        HashSet<string> identifiers = new(StringComparer.Ordinal);

        for (int i = 0; i < definitions.arraySize; i++)
        {
            SerializedProperty definition = definitions.GetArrayElementAtIndex(i);
            SerializedProperty idProperty = FindRelativeProperty(definition, IdPropertyNames);
            SerializedProperty nameProperty = FindRelativeProperty(definition, CodeNamePropertyNames);

            if (!TryReadInteger(idProperty, out long id) ||
                nameProperty == null ||
                nameProperty.propertyType != SerializedPropertyType.String)
            {
                Debug.LogError(
                    $"Could not read the ID or code name for resource element {i}. " +
                    "Update IdPropertyNames/CodeNamePropertyNames in ResourceRefsGenerator.");
                return;
            }

            string identifier = ToIdentifier(nameProperty.stringValue);
            if (string.IsNullOrEmpty(identifier))
            {
                Debug.LogError($"Resource element {i} has no usable code name.");
                return;
            }

            if (!identifiers.Add(identifier))
            {
                Debug.LogError(
                    $"More than one resource produces the code identifier '{identifier}'. " +
                    "Give each resource a unique CodeName.");
                return;
            }

            resources.Add(new GeneratedResource(identifier, id));
        }

        resources.Sort((a, b) => string.CompareOrdinal(a.Identifier, b.Identifier));
        WriteFile(resources);

        AssetDatabase.ImportAsset(OutputPath, ImportAssetOptions.ForceUpdate);
        Debug.Log($"Generated {resources.Count} resource references at {OutputPath}.", database);
    }

    [MenuItem("Tools/Game Data/Generate Resource References", true)]
    private static bool CanGenerate()
    {
        return Selection.activeObject is ResourceDatabase || FindDatabase(logErrors: false) != null;
    }

    private static ResourceDatabase GetDatabase()
    {
        if (Selection.activeObject is ResourceDatabase selected)
            return selected;

        return FindDatabase(logErrors: true);
    }

    private static ResourceDatabase FindDatabase(bool logErrors)
    {
        string[] guids = AssetDatabase.FindAssets("t:Resources");

        if (guids.Length == 1)
            return AssetDatabase.LoadAssetAtPath<ResourceDatabase>(AssetDatabase.GUIDToAssetPath(guids[0]));

        if (logErrors)
        {
            string message = guids.Length == 0
                ? "No ResourceDatabase asset was found."
                : "Multiple ResourceDatabase assets were found. Select the one to generate references from.";
            Debug.LogError(message);
        }

        return null;
    }

    private static SerializedProperty FindProperty(
        SerializedObject serializedObject,
        IEnumerable<string> candidateNames)
    {
        foreach (string candidate in candidateNames)
        {
            SerializedProperty property = serializedObject.FindProperty(candidate);
            if (property != null)
                return property;
        }

        return null;
    }

    private static SerializedProperty FindRelativeProperty(
        SerializedProperty parent,
        IEnumerable<string> candidateNames)
    {
        foreach (string candidate in candidateNames)
        {
            SerializedProperty property = parent.FindPropertyRelative(candidate);
            if (property != null)
                return property;
        }

        return null;
    }

    private static bool TryReadInteger(SerializedProperty property, out long value)
    {
        value = default;
        if (property == null)
            return false;

        if (property.propertyType == SerializedPropertyType.Integer)
        {
            value = property.longValue;
            return true;
        }
        
        SerializedProperty iterator = property.Copy();
        SerializedProperty end = iterator.GetEndProperty();
        bool enterChildren = true;

        while (iterator.NextVisible(enterChildren) && !SerializedProperty.EqualContents(iterator, end))
        {
            enterChildren = false;
            if (iterator.propertyType != SerializedPropertyType.Integer)
                continue;

            value = iterator.longValue;
            return true;
        }

        return false;
    }

    private static string ToIdentifier(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return string.Empty;

        StringBuilder builder = new();
        bool capitalizeNext = true;

        foreach (char character in value.Trim())
        {
            if (!char.IsLetterOrDigit(character) && character != '_')
            {
                capitalizeNext = true;
                continue;
            }

            char output = capitalizeNext ? char.ToUpperInvariant(character) : character;
            builder.Append(output);
            capitalizeNext = false;
        }

        if (builder.Length == 0)
            return string.Empty;

        if (char.IsDigit(builder[0]))
            builder.Insert(0, '_');

        string identifier = builder.ToString();
        return CSharpKeywords.Contains(identifier) ? "@" + identifier : identifier;
    }

    private static void WriteFile(IReadOnlyList<GeneratedResource> resources)
    {
        StringBuilder builder = new();
        builder.AppendLine("// <auto-generated />");
        builder.AppendLine("// Generated by ResourceRefsGenerator. Changes will be overwritten.");
        builder.AppendLine();
        builder.AppendLine("public static class ResourceRefs");
        builder.AppendLine("{");

        foreach (GeneratedResource resource in resources)
        {
            builder.Append("    public static readonly ResourceID ")
                .Append(resource.Identifier)
                .Append(" = new(")
                .Append(resource.Id)
                .AppendLine(");");
        }

        builder.AppendLine("}");

        string directory = Path.GetDirectoryName(OutputPath);
        if (!string.IsNullOrEmpty(directory))
            Directory.CreateDirectory(directory);

        File.WriteAllText(OutputPath, builder.ToString(), new UTF8Encoding(encoderShouldEmitUTF8Identifier: false));
    }

    private readonly struct GeneratedResource
    {
        public readonly string Identifier;
        public readonly long Id;

        public GeneratedResource(string identifier, long id)
        {
            Identifier = identifier;
            Id = id;
        }
    }

    private static readonly HashSet<string> CSharpKeywords = new(StringComparer.Ordinal)
    {
        "abstract", "as", "base", "bool", "break", "byte", "case", "catch", "char", "checked",
        "class", "const", "continue", "decimal", "default", "delegate", "do", "double", "else",
        "enum", "event", "explicit", "extern", "false", "finally", "fixed", "float", "for", "foreach",
        "goto", "if", "implicit", "in", "int", "interface", "internal", "is", "lock", "long",
        "namespace", "new", "null", "object", "operator", "out", "override", "params", "private",
        "protected", "public", "readonly", "ref", "return", "sbyte", "sealed", "short", "sizeof",
        "stackalloc", "static", "string", "struct", "switch", "this", "throw", "true", "try",
        "typeof", "uint", "ulong", "unchecked", "unsafe", "ushort", "using", "virtual", "void",
        "volatile", "while"
    };
}
#endif
