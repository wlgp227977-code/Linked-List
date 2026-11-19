using UnityEngine;
using System;

public class ChainingTest : MonoBehaviour
{
    private void Start()
    {
        TestChainingHashTable();
    }

    private void TestChainingHashTable()
    {
        Debug.Log("=== Chaining Hash Table Test ===");
        var hashTable = new ChainingHashTable<string, int>();

        hashTable.Add("하나", 1);
        hashTable.Add("둘", 2);
        hashTable.Add("셋", 3);

        foreach (var kvp in hashTable)
        {
            Debug.Log($"[{kvp.Key}, {kvp.Value}]");
        }

        Expect(hashTable.Count, 3, "Count");
        Expect(hashTable["둘"], 2, "값 둘");
        Expect(hashTable.ContainsKey("하나"), true, "탐색 하나");
        Expect(hashTable.Remove("하나"), true, "삭제 하나");
        Expect(hashTable.ContainsKey("하나"), false, "탐색 하나 (삭제 후)");
        Expect(hashTable.Count, 2, "Count (삭제 후)");

        Debug.Log("충돌 · 중복 키 · 리사이즈:");
        var collision = new ChainingHashTable<int, int>();
        collision.Add(1, 10);
        collision.Add(17, 170); // 1과 17은 % 16이 같아 같은 버킷에 들어간다
        Expect(collision[17], 170, "충돌한 키 17의 값");
        Expect(collision.Remove(1), true, "같은 버킷의 키 1 삭제");
        Expect(collision.ContainsKey(17), true, "삭제 후 키 17 탐색");
        ExpectThrows<ArgumentException>(() => collision.Add(17, 0), "중복 키 Add");

        for (int key = 100; key < 120; ++key)
        {
            collision.Add(key, key);
        }
        Expect(collision.Capacity > 16, true, $"리사이즈 발생 (Capacity {collision.Capacity})");
        Expect(collision.Count, 21, "리사이즈 후 Count");
        Expect(collision[17], 170, "리사이즈 후 키 17의 값");

        Debug.Log("인덱서 set:");
        collision[17] = 171;
        Expect(collision[17], 171, "인덱서로 기존 키 값 갱신");
        Expect(collision.Count, 21, "갱신 후 Count");
        collision[49] = 490;
        Expect(collision[49], 490, "인덱서로 새 키 추가");
        Expect(collision.Count, 22, "추가 후 Count");    
    }
    
    private void Expect<T>(T actual, T expected, string message)
    {
        if (Equals(actual, expected))
        {
            Debug.Log($"[성공] {message}");
        }
        else
        {
            Debug.LogError($"[실패] {message} / 예상: {expected}, 실제: {actual}");
        }
    }

    private void ExpectThrows<TException>(System.Action action, string message)
        where TException : Exception
    {
        try
        {
            action();
            Debug.LogError($"[실패] {message} / 예외가 발생하지 않음");
        }
        catch (TException)
        {
            Debug.Log($"[성공] {message}");
        }
        catch (Exception e)
        {
            Debug.LogError($"[실패] {message} / 다른 예외 발생: {e.GetType().Name}");
        }
    }

}
