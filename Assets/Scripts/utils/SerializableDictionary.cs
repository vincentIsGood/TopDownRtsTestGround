using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SerializableDictionary<TKey, TValue> : Dictionary<TKey, TValue>, ISerializationCallbackReceiver{
    [SerializeField]
    private List<SerializableKeyValuePair> entries = new List<SerializableKeyValuePair>();

    public void OnBeforeSerialize(){
        entries.Clear();
        foreach(KeyValuePair<TKey, TValue> entry in this){
            entries.Add(new SerializableKeyValuePair(entry.Key, entry.Value));
        }
    }

    public void OnAfterDeserialize(){
        this.Clear();
        foreach(SerializableKeyValuePair entry in entries){
            this.Add(entry.key, entry.value);
        }
    }

    public List<SerializableKeyValuePair> getEntries(){
        return entries;
    }

    [System.Serializable]
    public class SerializableKeyValuePair{
        public TKey key;
        public TValue value;

        public SerializableKeyValuePair(TKey key, TValue value){
            this.key = key;
            this.value = value;
        }
    }
}