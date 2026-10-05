#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

/// <summary>
/// Put in Assets/Scripts/Editor. Add these fields to ResourceDefinition first:
/// public bool IsSpeciesResource;
/// public SpeciesID SourceSpeciesID;
/// public string SpeciesProductKey;
///
/// Stable identity = SourceSpeciesID + SpeciesProductKey (not DisplayName).
/// Existing resources are never overwritten or deleted. New IDs append after
/// the highest existing ID. Populate species tags before running.
/// </summary>
public static class SpeciesResourceGenerator
{
    private const string MenuPath = "Tools/Game Data/Generate Species Resources";

    // Product keys must remain stable after generation, even if suffixes change.
    // Parent names are resolved from the asset, so regenerated ResourceRefs are
    // not required to compile or run this tool.
    private static readonly Rule[] Rules =
    {
        new Rule("Meat", "Meat", "Meat", null),
        new Rule("Pelt", "Pelt", "Pelt", "Furry"),
        new Rule("Wool", "Wool", "Wool", "Woolly"),
        new Rule("Hide", "Hide", "Hide", "HideBearing"),
        new Rule("Feathers", "Feathers", "Feathers", "Feathered"),
        new Rule("Horn", "Ivory", "Horn", "Horned"),
        new Rule("Tusks", "Ivory", "Tusks", "Tusked"),
        new Rule("Antler", "Antler", "Antlers", "Antlered")
    };

    [MenuItem(MenuPath)]
    private static void Generate()
    {
        if (EditorApplication.isPlayingOrWillChangePlaymode)
        {
            Debug.LogError("Generate species resources outside Play Mode.");
            return;
        }

        try
        {
            ResourceDatabase resources = FindDatabase<ResourceDatabase>();
            SpeciesDatabase species = FindDatabase<SpeciesDatabase>();
            if (resources == null || species == null) return;
            Generate(resources, species);
        }
        catch (Exception exception)
        {
            Debug.LogError("Species resource generation stopped: " + exception.Message);
        }
    }

    private static T FindDatabase<T>() where T : ScriptableObject
    {
        T[] selected = Selection.objects.OfType<T>().ToArray();
        if (selected.Length == 1) return selected[0];
        if (selected.Length > 1)
            throw new InvalidOperationException("Select only one " + typeof(T).Name + ".");

        string[] guids = AssetDatabase.FindAssets("t:" + typeof(T).Name);
        if (guids.Length != 1)
        {
            Debug.LogError("Expected one " + typeof(T).Name + " asset; found " + guids.Length +
                           ". Select the intended resource and species assets together if there are multiple.");
            return null;
        }
        return AssetDatabase.LoadAssetAtPath<T>(AssetDatabase.GUIDToAssetPath(guids[0]));
    }

    private static void Generate(ResourceDatabase resources, SpeciesDatabase speciesDatabase)
    {
        SerializedObject serialized = new SerializedObject(resources);
        serialized.Update();
        SerializedProperty items = Require(serialized.FindProperty("_items"), "_items");
        if (!items.isArray) throw new InvalidOperationException("_items must be an array.");

        var usedIDs = new HashSet<int>();
        var existingProducts = new HashSet<string>(StringComparer.Ordinal);
        var existingNames = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        var parents = new Dictionary<string, List<Parent>>(StringComparer.OrdinalIgnoreCase);
        int maximumID = -1;

        for (int i = 0; i < items.arraySize; i++)
        {
            SerializedProperty row = items.GetArrayElementAtIndex(i);
            int id = ID(row, "_id").intValue;
            if (!usedIDs.Add(id)) throw new InvalidOperationException("Duplicate resource ID: " + id);
            maximumID = Math.Max(maximumID, id);
            string name = Field(row, "DisplayName").stringValue;
            existingNames.Add(name);
            bool generated = Field(row, "IsSpeciesResource").boolValue;
            int sourceID = ID(row, "SourceSpeciesID").intValue;
            string productKey = Field(row, "SpeciesProductKey").stringValue;

            if (generated)
            {
                if (string.IsNullOrWhiteSpace(productKey))
                    throw new InvalidOperationException("Generated resource " + id + " has an empty product key.");
                if (!existingProducts.Add(Key(sourceID, productKey)))
                    throw new InvalidOperationException("Duplicate species product: " + sourceID + "/" + productKey);
            }
            else
            {
                if (!parents.TryGetValue(name, out List<Parent> matches))
                    parents.Add(name, matches = new List<Parent>());
                matches.Add(new Parent(id, Field(row, "Thumbnail").objectReferenceValue,
                    Field(row, "IsAbstract").boolValue));
            }
        }

        if (items.arraySize == 0)
            throw new InvalidOperationException("Add resource parent groups before generation.");

        var additions = new List<Addition>();
        var speciesIDs = new HashSet<int>();
        int skipped = 0;
        int untagged = 0;
        var unusedTags = new HashSet<string>(StringComparer.Ordinal);
        var tagsDefined = new HashSet<string>(Enum.GetNames(typeof(AnimalTags)), StringComparer.Ordinal);
        if (speciesDatabase.Items == null)
            throw new InvalidOperationException("Species database has no items array.");
        if (speciesDatabase.Items.Any(s => s == null))
            throw new InvalidOperationException("Null species entry.");

        // Build and validate the complete plan before modifying any assets.
        foreach (SpeciesDefinition species in speciesDatabase.Items.OrderBy(s => s.Id.Value))
        {
            if (species == null) throw new InvalidOperationException("Null species entry.");
            int speciesID = species.Id.Value;
            if (!speciesIDs.Add(speciesID))
                throw new InvalidOperationException("Duplicate species ID: " + speciesID);
            if (string.IsNullOrWhiteSpace(species.SpeciesName))
                throw new InvalidOperationException("Species " + speciesID + " has no name.");

            var tags = new HashSet<string>((species.Tags ?? new List<AnimalTags>())
                .Select(tag => tag.ToString()), StringComparer.Ordinal);
            if (tags.Count == 0) untagged++;

            foreach (Rule rule in Rules)
            {
                if (rule.Tag != null && !tagsDefined.Contains(rule.Tag))
                {
                    unusedTags.Add(rule.Tag);
                    continue;
                }
                if (rule.Tag != null && !tags.Contains(rule.Tag)) continue;
                string key = Key(speciesID, rule.ProductKey);
                if (existingProducts.Contains(key))
                {
                    skipped++;
                    continue;
                }

                if (!parents.TryGetValue(rule.ParentName, out List<Parent> matches) || matches.Count != 1)
                    throw new InvalidOperationException("Add exactly one parent resource named '" +
                        rule.ParentName + "' before generation. No rows were added.");
                Parent parent = matches[0];
                if (!parent.IsAbstract)
                    throw new InvalidOperationException("Mark parent '" + rule.ParentName + "' abstract first.");

                string name = species.SpeciesName.Trim() + " " + rule.Suffix;
                // Do not silently adopt a manually created row based on its name.
                if (!existingNames.Add(name))
                    throw new InvalidOperationException("Resource name collision: '" + name +
                        "'. If this is the same species product, set its IsSpeciesResource, " +
                        "SourceSpeciesID and SpeciesProductKey fields first.");
                additions.Add(new Addition(speciesID, rule.ProductKey, name, parent));
                existingProducts.Add(key);
            }
        }

        if (maximumID + (long)additions.Count > ushort.MaxValue)
            throw new InvalidOperationException("Not enough unused ushort resource IDs for this batch.");

        if (additions.Count > 0)
        {
            Undo.IncrementCurrentGroup();
            int undoGroup = Undo.GetCurrentGroup();
            Undo.SetCurrentGroupName("Generate Species Resources");
            Undo.RegisterCompleteObjectUndo(resources, "Generate Species Resources");
            try
            {
                int start = items.arraySize;
                items.arraySize += additions.Count;
                for (int i = 0; i < additions.Count; i++)
                {
                    Addition addition = additions[i];
                    SerializedProperty row = items.GetArrayElementAtIndex(start + i);
                    ID(row, "_id").intValue = ++maximumID;
                    Field(row, "DisplayName").stringValue = addition.Name;
                    Field(row, "Thumbnail").objectReferenceValue = addition.Parent.Thumbnail;
                    ID(row, "ParentID").intValue = addition.Parent.ID;
                    Field(row, "IsAbstract").boolValue = false;
                    Field(row, "IsSpeciesResource").boolValue = true;
                    ID(row, "SourceSpeciesID").intValue = addition.SpeciesID;
                    Field(row, "SpeciesProductKey").stringValue = addition.ProductKey;
                }
                serialized.ApplyModifiedPropertiesWithoutUndo();
                InvalidateLookup(resources);
                EditorUtility.SetDirty(resources);
                AssetDatabase.SaveAssets();
                Undo.CollapseUndoOperations(undoGroup);
            }
            catch
            {
                Undo.RevertAllDownToGroup(undoGroup);
                InvalidateLookup(resources);
                throw;
            }
        }

        Debug.Log("Species resources: added " + additions.Count + ", skipped " + skipped +
                  " existing products. " + untagged + " species have no tags. " +
                  "Existing IDs and customizations were preserved. Regenerate ResourceRefs if needed.", resources);
        foreach (string tag in unusedTags)
            Debug.LogWarning("Optional generator tag '" + tag + "' is not defined. " +
                             "Append it to AnimalTags to enable its rule.", speciesDatabase);
    }

    // Database caches its lookup. Clear it after changes and Undo/Redo.
    [InitializeOnLoadMethod]
    private static void RegisterUndoHandler()
    {
        Undo.undoRedoPerformed -= OnUndoRedo;
        Undo.undoRedoPerformed += OnUndoRedo;
    }

    private static void OnUndoRedo()
    {
        foreach (ResourceDatabase database in Resources.FindObjectsOfTypeAll<ResourceDatabase>())
            InvalidateLookup(database);
    }

    private static void InvalidateLookup(ResourceDatabase database)
    {
        typeof(Database<ResourceID, ResourceDefinition>)
            .GetField("_lookup", BindingFlags.Instance | BindingFlags.NonPublic)
            ?.SetValue(database, null);
    }

    private static string Key(int speciesID, string productKey) => speciesID + ":" + productKey;

    private static SerializedProperty Require(SerializedProperty property, string name)
    {
        if (property == null)
            throw new InvalidOperationException("Missing serialized field '" + name +
                "'. Add IsSpeciesResource (bool), SourceSpeciesID (SpeciesID), and " +
                "SpeciesProductKey (string) to ResourceDefinition before running.");
        return property;
    }

    private static SerializedProperty Field(SerializedProperty row, string name) =>
        Require(row.FindPropertyRelative(name), name);

    private static SerializedProperty ID(SerializedProperty row, string name) =>
        Require(Field(row, name).FindPropertyRelative("Value"), name + ".Value");

    private sealed class Rule
    {
        public readonly string ProductKey, ParentName, Suffix, Tag;
        public Rule(string productKey, string parentName, string suffix, string tag)
        { ProductKey = productKey; ParentName = parentName; Suffix = suffix; Tag = tag; }
    }

    private sealed class Parent
    {
        public readonly int ID;
        public readonly UnityEngine.Object Thumbnail;
        public readonly bool IsAbstract;
        public Parent(int id, UnityEngine.Object thumbnail, bool isAbstract)
        { ID = id; Thumbnail = thumbnail; IsAbstract = isAbstract; }
    }

    private sealed class Addition
    {
        public readonly int SpeciesID;
        public readonly string ProductKey, Name;
        public readonly Parent Parent;
        public Addition(int speciesID, string productKey, string name, Parent parent)
        { SpeciesID = speciesID; ProductKey = productKey; Name = name; Parent = parent; }
    }
}
#endif
