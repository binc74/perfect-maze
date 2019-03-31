using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Use disjoint set to help me easily merge groups
public class DisjointSet
{
    private class Node
    {
        public Node parent;
        public int size;
        public int height;

        public Node()
        {
            parent = this;
            size = 1;
            height = 0;
        }
    }

    private List<Node> nodes;

    public DisjointSet(int nodeCount)
    {
        nodes = new List<Node>();
        for (int i = 0; i < nodeCount; ++i)
        {
            nodes.Add(new Node());
        }
    }

    // Return the parent of the set
    private Node FindSet(int node)
    {
        Node n = nodes[node];

        while (n.parent != n)
            n = n.parent;

        return n;
    }

    // Return true if two nodes are in same set
    public bool InSameSet(int node1, int node2)
    {
        return FindSet(node1) == FindSet(node2);
    }

    // Merge two sets according to tree height in order to boost performance
    public void Union(int node1, int node2)
    {
        Node p1 = FindSet(node1), p2 = FindSet(node2);
        if (p1 == p2)
            return;

        if (p1.height < p2.height)
        {
            p1.parent = p2;
            p2.size += p1.size;
        }
        else if (p1.height > p2.height)
        {
            p2.parent = p1;
            p1.size += p2.size;
        }
        else
        {
            p1.parent = p2;
            p2.height++;
            p2.size += p1.size;
        }
    }
}
