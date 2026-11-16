using System;
using Unity.VisualScripting;
using UnityEngine;

public class AVLTreeTest<TKey, TValue> : AVLTree<TKey, TValue> where TKey : IComparable<TKey>
{
    public TreeNode<TKey, TValue> GetRoot()
    {
        return root;
    }
}
public class AVLTree<TKey, TValue> : BinarySearchTree<TKey, TValue> where TKey : IComparable<TKey>
{
    protected int BalanceFactor(TreeNode<TKey, TValue> node)
    {
        return node == null ? 0 : Height(node.Left) - Height(node.Right);
    }

    protected TreeNode<TKey, TValue> Balance(TreeNode<TKey, TValue> node)
    {
        int balanceFactor = BalanceFactor(node);
        // TODO: LL, LR, RR, RL
        if (balanceFactor >= 2)
        {
            if (BalanceFactor(node.Left) >= 0)
            {
                return RotateRight(node);
            }
            else
            {
                node.Left = RotateLeft(node.Left);
                return RotateRight(node);
            }

        }
        else if (balanceFactor <= -2)
        {
            if (BalanceFactor(node.Right) <= 0)
            {
                return RotateLeft(node);
            }
            else
            {
                node.Right = RotateRight(node.Right);
                return RotateLeft(node);
            }
        }

        return node;
    }

    protected TreeNode<TKey, TValue> RotateLeft(TreeNode<TKey, TValue> node)
    {
        // TODO:   
        var rightChild = node.Right;
        var leftSubtree = rightChild.Left;

        rightChild.Left = node;
        node.Right = leftSubtree;

        UpdateHeight(node);
        UpdateHeight(rightChild);
        return rightChild;
    }
    protected TreeNode<TKey, TValue> RotateRight(TreeNode<TKey, TValue> node)
    {
        // TODO:
        var leftChild = node.Left;
        var rightSubtree = leftChild.Right;     // b를 따로보관

        leftChild.Right = node;     // L의 오른쪽 P
        node.Left = rightSubtree;   // P의 왼쪽 b

        UpdateHeight(node);
        UpdateHeight(leftChild);
        return leftChild;
    }

    protected override TreeNode<TKey, TValue> Add
    (TreeNode<TKey, TValue> node, TKey key, TValue value)
    {
        node = base.Add(node, key, value);
        return Balance(node);
    }
    protected override TreeNode<TKey, TValue> AddOrUpdate
   (TreeNode<TKey, TValue> node, TKey key, TValue value)
    {
        node = base.AddOrUpdate(node, key, value);
        return Balance(node);
    }

    protected override TreeNode<TKey, TValue> Remove(TreeNode<TKey, TValue> node, TKey key)
    {
        node = base.Remove(node, key);
        return Balance(node);
    }

    // 모든 노드의 BF ∈ {-1, 0, +1}인지 검증 (AVL 불변식).
    public bool IsBalanced()
    {
        return IsBalanced(root);
    }

    private bool IsBalanced(TreeNode<TKey, TValue> node)
    {
        if (node == null)
        {
            return true;
        }
        if (Mathf.Abs(BalanceFactor(node)) > 1)
        {
            return false;
        }
        return IsBalanced(node.Left) && IsBalanced(node.Right);
    }
}
