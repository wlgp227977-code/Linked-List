using UnityEngine;

public class TreeMode<TKey, TValue>
{
    public TKey Key { get; set; }
    public TValue Value { get; set; }

    public TreeMode<TKey, TValue> Left { get; set; }
    public TreeMode<TKey, TValue> Right { get; set; }

    public int Height { get; set; }

    public TreeMode(TKey key, TValue value)
    {
        Key = key;
        Value = value;
        Height = 1;
    }
}
