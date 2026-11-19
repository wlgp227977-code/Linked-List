using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public enum ProbingStrategy
{
    Linear,
    Quadratic,
    DoubleHash,
}


public class OpenAddressingHashTable<TKey, TValue> : IDictionary<TKey, TValue>
{
    private const int DefaultCapacity = 16;
    private const double LoadFactor = 0.6;

    private KeyValuePair<TKey, TValue>[] table;
    private bool[] occupied;
    private bool[] deleted;
    private int size;
    private int count;

    private ProbingStrategy probingStrategy;


    public OpenAddressingHashTable(ProbingStrategy strategy = ProbingStrategy.Linear)
    {
        table = new KeyValuePair<TKey, TValue>[DefaultCapacity];
        occupied = new bool[DefaultCapacity];
        deleted = new bool[DefaultCapacity];
        size = DefaultCapacity;
        count = 0;

        probingStrategy = strategy;
    }

    public int GetHash(TKey key)
    {
        if (key == null)
            throw new ArgumentNullException(nameof(key));

        int hash = key.GetHashCode();
        // 부호 비트만 제거해 항상 0 이상. Math.Abs는 int.MinValue에서 음수를 그대로 돌려주므로 쓰지 않는다.
        return (hash & 0x7fffffff) % size;
    }

    public int GetSecondaryHash(TKey key)
    {
        if (key == null)
            throw new ArgumentNullException(nameof(key));

        int hash = key.GetHashCode();
        return 1 + ((hash & 0x7fffffff) % (size - 1));
    }

    public int GetProbeIndex(TKey key, int attempt)
    {
        int primaryHash = GetHash(key);

        switch (probingStrategy)
        {
            case ProbingStrategy.Linear:
                // 선형 탐사: 다음 칸으로 한 칸씩 이동
                // [빈칸 ①]
                throw new NotImplementedException("빈칸 ①");
            case ProbingStrategy.Quadratic:
                // 이차 탐사: 이동 거리를 attempt의 제곱으로 증가
                // [빈칸 ①]
                throw new NotImplementedException("빈칸 ①");
            case ProbingStrategy.DoubleHash:
                // 이중 해시: 두 번째 해시 함수로 이동 간격을 키마다 분산
                // [빈칸 ①]
                throw new NotImplementedException("빈칸 ①");
        }

        throw new ArgumentException(nameof(probingStrategy));
    }


    public TValue this[TKey key]
    {
        get
        {
            if (TryGetValue(key, out TValue value))
            {
                return value;
            }

            throw new KeyNotFoundException();
        }
        // 중복 키는 값 갱신
        set
        {
            if (key == null)
                throw new ArgumentNullException(nameof(key));

            // tombstone 뒤에 같은 키가 남아 있을 수 있으므로, 빈 칸을 찾기 전에 먼저 탐색한다.
            // [빈칸 ⑦] 같은 키가 있으면 값만 바꾸고, 없으면 Add처럼 탐사해 넣는다
            throw new NotImplementedException("빈칸 ⑦");
        }

    }

    public ICollection<TKey> Keys => Enumerable.Range(0, size)
                                        .Where(i => occupied[i] && !deleted[i])
                                        .Select(i => table[i].Key)
                                        .ToList();

    public ICollection<TValue> Values => Enumerable.Range(0, size)
                                        .Where(i => occupied[i] && !deleted[i])
                                        .Select(i => table[i].Value)
                                        .ToList();

    public int Count => count;

    public int Capacity => size;

    public bool IsReadOnly => false;

    private void Resize()
    {
        var oldTable = table;
        var oldOccupied = occupied;
        var oldDeleted = deleted;
        var oldSize = size;

        size *= 2;
        table = new KeyValuePair<TKey, TValue>[size];
        occupied = new bool[size];
        deleted = new bool[size];
        // [빈칸 ⑥] count를 0으로 돌리고, 살아 있는 칸만 Add로 다시 넣는다
        throw new NotImplementedException("빈칸 ⑥");
    }

    private int FindIndex(TKey key)
    {
        if (key == null)
            throw new ArgumentNullException(nameof(key));

        // [빈칸 ②] 완전히 빈 칸이면 -1, 살아 있는 같은 키면 그 인덱스. 최대 size번 탐사
        throw new NotImplementedException("빈칸 ②");
    }

    // 중복 키는 예외
    public void Add(TKey key, TValue value)
    {
        if (key == null)
            throw new ArgumentNullException(nameof(key));

        // tombstone 뒤에 같은 키가 남아 있을 수 있으므로, 빈 칸을 찾기 전에 먼저 탐색한다.
        // [빈칸 ③] FindIndex로 같은 키가 있으면 "키 중복" 예외
        throw new NotImplementedException("빈칸 ③");

        if ((double)count / size > LoadFactor)
        {
            Resize();
        }

        // [빈칸 ④] attempt 0부터 탐사해 빈 칸이나 tombstone에 넣는다. size번 넘게 못 찾으면 Resize 후 처음부터
        throw new NotImplementedException("빈칸 ④");
    }

    public void Add(KeyValuePair<TKey, TValue> item)
    {
        Add(item.Key, item.Value);
    }

    public void Clear()
    {
        Array.Clear(table, 0, size);
        Array.Clear(occupied, 0, size);
        Array.Clear(deleted, 0, size);
        count = 0;
    }

    public bool Contains(KeyValuePair<TKey, TValue> item)
    {
        int index = FindIndex(item.Key);
        if (index != -1)
        {
            return table[index].Value.Equals(item.Value);
        }

        return false;
    }

    public bool ContainsKey(TKey key)
    {
        return FindIndex(key) != -1;
    }

    public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
    {
        int index = arrayIndex;
        foreach (var kvp in this)
        {
            array[index++] = kvp;
        }
    }

    public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
    {
        for (int i = 0; i < size; ++i)
        {
            if (occupied[i] && !deleted[i])
            {
                yield return table[i];
            }
        }
    }

    public bool Remove(TKey key)
    {
        // [빈칸 ⑤] 찾은 칸을 deleted로 표시하고 count를 줄인다
        throw new NotImplementedException("빈칸 ⑤");
    }

    public bool Remove(KeyValuePair<TKey, TValue> item)
    {
        // [빈칸 ⑤] Remove(key)와 같되 값까지 같아야 지운다
        throw new NotImplementedException("빈칸 ⑤");
    }

    public bool TryGetValue(TKey key, out TValue value)
    {
        int index = FindIndex(key);

        if (index != -1)
        {
            value = table[index].Value;
            return true;
        }

        value = default;
        return false;
    }

    public IEnumerable<BucketView> GetBucketViews()
    {
        for (int i = 0; i < size; ++i)
        {
            if (occupied[i] && !deleted[i])
            {
                yield return new BucketView(i, BucketState.Occupied,
                    Convert.ToString(table[i].Key), Convert.ToString(table[i].Value), null);
            }
            // [빈칸 ⑧] deleted 칸은 Deleted, 나머지는 Empty
            throw new NotImplementedException("빈칸 ⑧");
        }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}