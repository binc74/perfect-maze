using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpanningTree
{
    private readonly int h, v;
    // Use to keep track with unseen edge
    private List<int> unseenEdge;

    // Mapping between actual edge to hashed edge. Used for edge to nodes lookup
    private int[] edgeMap;

    // The spanning tree using node representation
    private List<int[]> spanningTree;

    public int start, end;

    public SpanningTree(int h, int v)
    {
        this.h = h;
        this.v = v;

        // Init edgemap and the spanning tree
        InitEdgeMapping();
        InitSpanningTree();

        // Construct the tree
        ConstructTree();
    }

    public int Size()
    {
        return h * v;
    }

    public int[] GetEdges(int node)
    {
        return spanningTree[node];
    }

    private void ConstructTree()
    {
        // Init unseen edge
        unseenEdge = new List<int>();

        for (int i = 0; i < h * (v - 1) + v * (h - 1); ++i)
        {
            unseenEdge.Add(i);
        }

        // Use Kruskal's algorithm to generate maze
        // 1 assign each node to each group (init dset with number of nodes)
        DisjointSet groups = new DisjointSet(h * v);

        // Loop 2 and 3 until we iterate through all edges
        while (unseenEdge.Count > 0)
        {
            // 2 choose any edge on grid
            int r = Random.Range(0, unseenEdge.Count);
            int edgeIndex = unseenEdge[r];
            unseenEdge.RemoveAt(r);

            // 3 If the edge is in different groups, add the edge and merge the groups
            int edge = edgeMap[edgeIndex];
            // Split the two nodes from the hashed value
            int node1 = edge / 1000, node2 = edge % 1000;

            if (!groups.InSameSet(node1, node2))
            {
                // Update spanning tree edges
                int dir1 = GetDirection(node1, node2), dir2 = GetDirection(node2, node1);
                spanningTree[node1][dir1] = 1;
                spanningTree[node2][dir2] = 1;

                // Merge group
                groups.Union(node1, node2);
            }
        }
    } 

    // Need the nodes to be valid, and two nodes to be neighbour either vertically or horizontally
    private int GetEdge(int node1, int node2)
    {
        return edgeMap[GetEdgeHash(node1, node2)];
    }

    // Very naive hash method, need the number of node not exceed 1000
    private int GetEdgeHash(int node1, int node2)
    {
        if (node1 > node2)
        {
            int temp = node1;
            node1 = node2;
            node2 = temp;
        }

        return node1 * 1000 + node2;
    }

    // 0->top, 1->left, 2->bot, 3->right
    private void InitSpanningTree()
    {
        spanningTree = new List<int[]>();

        for (int i = 0; i < h * v; ++i)
        {
            spanningTree.Add(new int[4]);
        }

        // Set -1 to the top edges
        for (int i = 0; i < h; ++i)
        {
            spanningTree[i][0] = -1;
        }

        // Set -1 to the left edges
        for (int i = 0; i < h * v; i += h)
        {
            spanningTree[i][1] = -1;
        }

        // Set -1 to bot edges
        for (int i = h * (v - 1); i < h * v; i++)
        {
            spanningTree[i][2] = -1;
        }

        // Set -1 to right edges
        for (int i = h - 1; i < h * v; i += h)
        {
            spanningTree[i][3] = -1;
        }
    }

    // Get the direction of node2 compare to node1 given the two nodes are adjacent. 
    // 0->top, 1->left, 2->bot, 3->right
    private int GetDirection(int node1, int node2)
    {
        int currRow = node1 / h;

        // Check up node
        if (node2 == node1 - h)
            return 0;
        if (node2 == node1 + h)
            return 2;
        if (node2 == node1 - 1)
            return 1;
        if (node2 == node1 + 1)
            return 3;

        return -1;
    }

    private void InitEdgeMapping()
    {
        edgeMap = new int[v * (h - 1) + h * (v - 1)];
        int currEdge = 0;

        // First go through all vertical edges
        for (int i = 0; i < h; ++i)
        {
            for (int j = 0; j < v - 1; ++j)
            {
                int firstNode = j * h + i, secondNode = (j + 1) * h + i;
                int edge = GetEdgeHash(firstNode, secondNode);

                edgeMap[currEdge] = edge;
                currEdge++;
            }
        }

        // Then go through all horizontal edges
        for (int i = 0; i < v; ++i)
        {
            for (int j = 0; j < h - 1; ++j)
            {
                int firstNode = i * h + j, secondNode = i * h + j + 1;
                int edge = GetEdgeHash(firstNode, secondNode);

                edgeMap[currEdge] = edge;
                currEdge++;
            }
        }
    }

    public void InitStartAndEnd()
    {
        List<int> endNodes = new List<int>();

        // Init endNodes (Find the number of nodes that only has one edge)
        for (int i = 0; i < spanningTree.Count; ++i)
        {
            int[] edge = spanningTree[i];
            int countOne = 0;
            foreach (int j in edge)
            {
                if (j == 1)
                {
                    countOne ++;
                }
            }
            if (countOne == 1)
            {
                endNodes.Add(i);
            }
        }

        // Randomly choose startnode
        int r = Random.Range(0, endNodes.Count);
        start = endNodes[r];
        endNodes.RemoveAt(r);

        // Randomly choose endnode
        r = Random.Range(0, endNodes.Count);
        end = endNodes[r];
        endNodes.RemoveAt(r);
    }
}
