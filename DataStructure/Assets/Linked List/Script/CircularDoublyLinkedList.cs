using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System;

public class CircularDoublyLinkedListNode<T>
{
    public T Value { get; set; }
    public CircularDoublyLinkedList<T> List { get; set; }

    // 리스트 내부 전용 링크 = 마지막 노드의 next가 처음 노드인 원형 연결
    public CircularDoublyLinkedListNode<T> next;
    public CircularDoublyLinkedListNode<T> prev;

    // 사용자가 보는 링크 = 이음새(마지막 → 처음)에서 null을 돌려줘 원형을 숨긴다
    public CircularDoublyLinkedListNode<T> Next => next == null || next == List.First ? null : next;
    public CircularDoublyLinkedListNode<T> Previous => prev == null || this == List.First ? null : prev;

    public CircularDoublyLinkedListNode(T value)
    {
        Value = value;
    }
}


public class CircularDoublyLinkedList<T> : IEnumerable<T>
{
    private CircularDoublyLinkedListNode<T> head;
    public int Count { get; private set; }

    public CircularDoublyLinkedListNode<T> First => head;
    public CircularDoublyLinkedListNode<T> Last => head == null ? null : head.prev;

    public IEnumerator<T> GetEnumerator()
    {
        if (head == null) yield break;

        var current = head;
        do
        {
            yield return current.Value;
            current = current.next;
        } while (current != head);
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    // target 바로 앞에 newNode를 끼운다
    private void InsertBefore(CircularDoublyLinkedListNode<T> target,
        CircularDoublyLinkedListNode<T> newNode)
    {
        ValidateNode(target);

        var targetPrev = target.prev;
        targetPrev.next = newNode;
        target.prev = newNode;
        newNode.prev = targetPrev;
        newNode.next = target;

        newNode.List = this;
        Count++;
    }

    public CircularDoublyLinkedListNode<T> AddFirst(T value)
    {
        var node = AddLast(value);
        head = node;
        return node;
    }

    public CircularDoublyLinkedListNode<T> AddLast(T value)
    {
        var node = new CircularDoublyLinkedListNode<T>(value);

        if (head == null)
        {
            head = node;
            node.prev = node;
            node.next = node;
            node.List = this;
            Count++;
        }
        else
        {
            InsertBefore(head, node);
        }

        return node;
    }

    public CircularDoublyLinkedListNode<T> AddAfter(CircularDoublyLinkedListNode<T> node, T value)
    {
        ValidateNode(node);
        var newNode = new CircularDoublyLinkedListNode<T>(value);

        InsertBefore(node.next, newNode);

        return newNode;
    }

    public CircularDoublyLinkedListNode<T> AddBefore(CircularDoublyLinkedListNode<T> node, T value)
    {
        ValidateNode(node);
        var newNode = new CircularDoublyLinkedListNode<T>(value);

        InsertBefore(node, newNode);
        if (node == head)
        {
            head = newNode;
        }

        return newNode;
    }

    public void RemoveFirst()
    {
        if (head == null)
        {
            throw new InvalidOperationException("리스트가 비어있습니다.");
        }
        Remove(head);
    }

    public void RemoveLast()
    {
        if (head == null)
        {
            throw new InvalidOperationException("리스트가 비어있습니다.");
        }
        Remove(head.prev);
    }

    public bool Remove(T value)
    {
        var node = Find(value);
        if (node == null)
        {
            return false;
        }
        Remove(node);
        return true;
    }

    public void Remove(CircularDoublyLinkedListNode<T> node)
    {
        ValidateNode(node);

        if (Count == 1)
        {
            head = null;
        }
        else
        {
            node.prev.next = node.next;
            node.next.prev = node.prev;

            if (node == head)
            {
                head = node.next;
            }
        }

        node.next = null;
        node.prev = null;
        node.List = null;
        Count--;
    }

    public CircularDoublyLinkedListNode<T> Find(T value)
    {
        if (head == null) return null;

        var comparer = EqualityComparer<T>.Default;
        var current = head;
        do
        {
            if (comparer.Equals(current.Value, value))
            {
                return current;
            }
            current = current.next;
        } while (current != head);

        return null;
    }

    public bool Contains(T value)
    {
        return Find(value) != null;
    }

    public IEnumerable<T> Backward()
    {
        if (head == null) yield break;

        var current = head.prev;
        do
        {
            yield return current.Value;
            current = current.prev;
        } while (current != head.prev);
    }

    public void Clear()
    {
        if (head == null) return;

        var current = head;
        do
        {
            var next = current.next;
            current.next = null;
            current.prev = null;
            current.List = null;
            current = next;
        } while (current != head);

        head = null;
        Count = 0;
    }

    private void ValidateNode(CircularDoublyLinkedListNode<T> node)
    {
        if (node == null)
        {
            throw new ArgumentNullException(nameof(node));
        }
        if (node.List != this)
        {
            throw new InvalidOperationException("이 리스트에 속한 노드가 아닙니다.");
        }
    }
}