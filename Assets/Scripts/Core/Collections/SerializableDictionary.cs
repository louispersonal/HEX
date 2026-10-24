using System;
using System.Collections.Generic;
using System.Linq;

public class SerializableDictionary<TKey, TValue> : Dictionary<TKey, TValue>,
    ISavable<SerializableDictionarySaveData<TKey, TValue>>
{
    public SerializableDictionary() { }

    public SerializableDictionary(SerializableDictionary<TKey, TValue> original) : base(original) { }
    
    public SerializableDictionary(SerializableDictionarySaveData<TKey, TValue> saveData)
    {
        if (saveData.Keys.Length != saveData.Values.Length)
            throw new ArgumentException("Mismatched keys and values.");

        for (int i = 0; i < saveData.Keys.Length; i++)
        {
            Add(saveData.Keys[i], saveData.Values[i]);
        }
    }

    public SerializableDictionarySaveData<TKey, TValue> ToSaveData()
    {
        return new SerializableDictionarySaveData<TKey, TValue>(
            Keys.ToArray(),
            Values.ToArray()
        );
    }
}

[Serializable]
public class SerializableDictionarySaveData<TKey, TValue>
{
    public TKey[] Keys;
    public TValue[] Values;

    public SerializableDictionarySaveData(TKey[] keys, TValue[] values)
    {
        Keys = keys;
        Values = values;
    }
}