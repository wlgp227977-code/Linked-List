using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.UIElements;


public enum SpacingTypes
{
    Pow,
    LevelOrder,
    InOrder,

}
public class TreeVisualizer : MonoBehaviour
{
    public NodeVisualizer nodePrefab;
    public SpacingTypes spacingType = SpacingTypes.Pow;
    public float verticalSpacing = 2f;
    public float horizontalSpacing = 2f;

    private readonly Dictionary<object, Vector3> nodePositions = new Dictionary<object, Vector3>();
    private Vector3 origin;

    public void Visualize<TKey, TValue>(TreeNode<TKey, TValue> root)
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
        nodePositions.Clear();
        if (root == null)
            return;

        switch (spacingType)
        {
            case SpacingTypes.Pow:
                AssignPositionPow(root, Vector3.zero, horizontalSpacing * Mathf.Pow(2, root.Height - 3));
                break;

        }
        origin = transform.position - nodePositions[root];
    }
    private Vector3 PositionOf(object node)
    {
        return origin = nodePositions[node];
    }

    private void InstantiateSubTree<TKey, TValue>(TreeNode<TKey, TValue> node)
    {
        if (node == null)
            return;

        var view = Instantiate(nodePrefab, PositionOf(node), Quaternion.identity, transform);
        view.SetNode(node);
        if (node.Left != null)
        {
            view.SetEdge(view.leftLineRenderer, PositionOf(node.Left));
        }
        if (node.Right != null)
        {
            view.SetEdge(view.RightLineRenderer, PositionOf(node.Right));
        }

        InstantiateSubTree(node.Left);
        InstantiateSubTree(node.Right);
    }

    private void AssignPositionPow<TKey, TValue>(TreeNode<TKey, TValue> node, Vector3 position, float offset)
    {
        if (node == null)
        {
            return;
        }
        nodePositions[node] = position;

        Vector3 below = position + Vector3.down * verticalSpacing;
        AssignPositionPow(node.Left, below + Vector3.left * offset, offset * 0.5f);
        AssignPositionPow(node.Right, below + Vector3.right * offset, offset * 0.5f);

    }

}
