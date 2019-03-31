using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Use for solving the maze. Just for fun.
public class MazeSolver : MonoBehaviour
{
    private int h, v;
    private SpanningTree spanningTree;
    private List<GameObject> lines;
    private List<HashSet<int>> graph;
    private HashSet<int> seen;
    private Stack<int> path;

    // For convinence
    private float endX, endZ;
    private int start, end, curr;
    private bool hasSolved;


    public MazeSolver(int h, int v, SpanningTree spanningTree)
    {
        this.h = h;
        this.v = v;
        this.spanningTree = spanningTree;
        start = spanningTree.start;
        end = spanningTree.end;
        curr = start;
        hasSolved = false;

        int endRow = end % h, endCol = end / h;
        endX = endRow + 0.5f;
        endZ = -endCol - 0.5f;    

        ConstructGraph();
    }

    public bool HasSolved()
    {
        return hasSolved;
    }

    private void ConstructGraph()
    {
        graph = new List<HashSet<int>>();

        // Init graph
        for (int i = 0; i < h * v; ++i)
        {
            graph.Add(new HashSet<int>());
        }

        // Add edges to graph
        for (int i = 0; i < h * v; ++i)
        {
            int[] edges = spanningTree.GetEdges(i);
            int[] neighbours = GetNeighbours(i);

            //Debug.Log(i +   ": " + string.Join(", ", neighbours));

            for (int j = 0; j < edges.Length; ++j)
            {
                if (edges[j] > 0)
                {
                    graph[i].Add(neighbours[j]);
                }
            }
        }
    }

    // Return [up, left, bot, right], negative number represents no node in that direction
    private int[] GetNeighbours(int node)
    {
        int currRow = node / h;
        int up = node - h;

        // If bot node exceeds the # of total nodes, it does not exist
        int bot = node + h;
        if (bot >= h * v)
        {
            bot = -1;
        }

        // If left node is not at same row, it does not exist
        int left = node - 1;
        if (left / h != currRow)
        {
            left = -1;
        }

        int right = node + 1;
        if (right / h != currRow)
        {
            right = -1;
        }

        return new int[] { up, left, bot, right };
    }

    private bool Dfs(int node)
    {
        if (node == end)
        {
            path.Push(node);
            return true;
        }

        foreach (int child in graph[node])
        {
            if (!seen.Contains(child))
            {
                seen.Add(child);
                if (Dfs(child))
                {
                    path.Push(child);
                    return true;
                }
            }
        }

        return false;
    }

    // Using naive dfs to solve this
    public void Solve()
    {
        path = new Stack<int>();
        seen = new HashSet<int>();
        seen.Add(start);
        Dfs(start);
        hasSolved = true;
        path.Push(start);
        DrawPath();
    }

    private Vector3 GetPosition(int node)
    {
        int nodeCol = node % h, nodeRow = node / h;

        return new Vector3(nodeCol + 0.5f, 0.15f, -nodeRow - 0.5f);
    }

    public void DrawPath()
    {
        int currVertex = 0;
        lines = new List<GameObject>();
        Debug.Log("KK");
        Vector3 posS = GetPosition(path.Pop()) + new Vector3(0, 0.15f, 0);
        while (path.Count != 0)
        {
            int s = path.Pop();
            Vector3 posE = GetPosition(s) + new Vector3(0, 0.15f, 0);
            GameObject line = new GameObject();
            lines.Add(line);
            LineRenderer lineRenderer = line.AddComponent<LineRenderer>();
            lineRenderer.SetWidth(0.1f, 0.1f);
            lineRenderer.SetPosition(0, posS);
            lineRenderer.SetPosition(1, posE);
            currVertex++;
            posS = posE;
        }

    }

    public void ClearPath()
    {
        foreach(GameObject l in lines)
        {
            LineRenderer temp = l.GetComponent<LineRenderer>();
            temp.positionCount = 0;

            Destroy(temp);
            Destroy(l);
        }
    }
}
