using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace NetlinkCore.Generic;

[SuppressMessage("Naming", "CA1710:Identifiers should have correct suffix")]
public class StringSet : IDictionary<int, string>, IDictionary<string, int>
{
    private readonly Dictionary<int, string> _intToString = [];
    private readonly Dictionary<string, int> _stringToInt = [];

    public int Count => _intToString.Count;
    public bool IsReadOnly => false;

    public string this[int index]
    {
        get => _intToString[index];
        set => _intToString[index] = value;
    }

    public int this[string value_]
    {
        get => _stringToInt[value_];
        set => _stringToInt[value_] = value;
    }

    public ICollection<int> Indices => _stringToInt.Values;
    public ICollection<string> Values => _stringToInt.Keys;

    public IEnumerator<KeyValuePair<int, string>> GetEnumerator() => _intToString.GetEnumerator();

    public bool Contains(int index) => _intToString.ContainsKey(index);
    public bool Contains(string value) => _stringToInt.ContainsKey(value);

    public bool TryGet(int index, [MaybeNullWhen(false)] out string value) => _intToString.TryGetValue(index, out value);
    public bool TryGet(string value, out int index) => _stringToInt.TryGetValue(value, out index);

    public void Clear()
    {
        _intToString.Clear();
        _stringToInt.Clear();
    }

    public void Add(int index, string value)
    {
        _intToString.Add(index, value);
        _stringToInt.Add(value, index);
    }

    public bool Remove(int index, [MaybeNullWhen(false)] out string value)
    {
        if (!_intToString.Remove(index, out value))
            return false;
        _stringToInt.Remove(value);
        return true;
    }

    public bool Remove(int index) => Remove(index, out _);

    public bool Remove(string value, out int index)
    {
        if (!_stringToInt.Remove(value, out index))
            return false;
        _intToString.Remove(index);
        return true;
    }

    public bool Remove(string value) => Remove(value, out _);

    IEnumerator<KeyValuePair<string, int>> IEnumerable<KeyValuePair<string, int>>.GetEnumerator() => _stringToInt.GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    void ICollection<KeyValuePair<int, string>>.Add(KeyValuePair<int, string> item) => Add(item.Key, item.Value);
    void ICollection<KeyValuePair<string, int>>.Add(KeyValuePair<string, int> item) => Add(item.Value, item.Key);
    bool ICollection<KeyValuePair<int, string>>.Contains(KeyValuePair<int, string> item) => ((IDictionary<int, string>)_intToString).Contains(item);
    bool ICollection<KeyValuePair<string, int>>.Contains(KeyValuePair<string, int> item) => ((IDictionary<string, int>)_stringToInt).Contains(item);
    void ICollection<KeyValuePair<int, string>>.CopyTo(KeyValuePair<int, string>[] array, int arrayIndex) => ((IDictionary<int, string>)_intToString).CopyTo(array, arrayIndex);
    void ICollection<KeyValuePair<string, int>>.CopyTo(KeyValuePair<string, int>[] array, int arrayIndex) => ((IDictionary<string, int>)_stringToInt).CopyTo(array, arrayIndex);
    bool ICollection<KeyValuePair<int, string>>.Remove(KeyValuePair<int, string> item) => ((IDictionary<int, string>)_intToString).Remove(item);
    bool ICollection<KeyValuePair<string, int>>.Remove(KeyValuePair<string, int> item) => ((IDictionary<string, int>)_stringToInt).Remove(item);
    ICollection<int> IDictionary<int, string>.Keys => Indices;
    ICollection<string> IDictionary<string, int>.Keys => Values;
    ICollection<string> IDictionary<int, string>.Values => Values;
    ICollection<int> IDictionary<string, int>.Values => Indices;
    bool IDictionary<int, string>.ContainsKey(int index) => _intToString.ContainsKey(index);
    bool IDictionary<string, int>.ContainsKey(string value) => _stringToInt.ContainsKey(value);
    bool IDictionary<int, string>.TryGetValue(int key, [MaybeNullWhen(false)] out string value) => _intToString.TryGetValue(key, out value);
    bool IDictionary<string, int>.TryGetValue(string key, out int value) => _stringToInt.TryGetValue(key, out value);
    void IDictionary<string, int>.Add(string key, int value) => Add(value, key);
}