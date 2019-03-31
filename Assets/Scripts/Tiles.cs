using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Use for generating tiles
public class Tiles : MonoBehaviour
{
    private int h, v;
    private SpanningTree spanningTree;
    private Dictionary<string, string> tileMapping;
    private List<GameObject> tiles;

    // Use for random generating tiles
    private List<int> remainPos;

    private GameObject startObj;
    private GameObject endObj;

    public Tiles(int h, int v, SpanningTree spanningTree)
    {
        this.h = h;
        this.v = v;
        this.spanningTree = spanningTree;
        tiles = new List<GameObject>();

        remainPos = new List<int>();

        for (int i = 0; i < h * v; ++i)
        {
            remainPos.Add(i);
        }

        InitTileMapping();
    }

    private void InitTileMapping()
    {
        tileMapping = new Dictionary<string, string>();

        tileMapping["1111"] = "Cross";
        tileMapping["0101"] = "StraightsH";
        tileMapping["1010"] = "StraightsV";
        tileMapping["0001"] = "TerminalR";
        tileMapping["0010"] = "TerminalB";
        tileMapping["0100"] = "TerminalL";
        tileMapping["1000"] = "TerminalT";
        tileMapping["0111"] = "TJB";
        tileMapping["1110"] = "TJL";
        tileMapping["1011"] = "TJR";
        tileMapping["1101"] = "TJT";
        tileMapping["0110"] = "TurnsBL";
        tileMapping["0011"] = "TurnsBR";
        tileMapping["1100"] = "TurnsTL";
        tileMapping["1001"] = "TurnsTR";
    }

    private GameObject GetTile(int[] edges, Vector3 pos)
    {
        string hash = "";
        foreach (int i in edges)
        {
            int temp = i > 0 ? 1 : 0;
            hash += temp;
        }

        //Debug.Log(hash + ": " + tileMapping[hash] + ", " + pos);
        GameObject obj = Instantiate(Resources.Load(tileMapping[hash]), pos,
            Quaternion.identity) as GameObject;

        return obj;
    }

    public void Construct()
    {
        // Randomy choose one index
        int r = Random.Range(0, remainPos.Count);
        int curr = remainPos[r];
        remainPos.RemoveAt(r);

        int col = curr % h, row = curr / h;

        Vector3 position = new Vector3(col, 0.15f, -row);
        tiles.Add(GetTile(spanningTree.GetEdges(curr), position));
    }

    public void Destroy()
    {
        foreach (GameObject o in tiles)
        {
            Destroy(o);
        }
        if (startObj != null) Destroy(startObj);
        if (endObj != null) Destroy(endObj);
    }

    public bool IsEnd()
    {
        return remainPos.Count == 0;
    }

    public void DrawStartAndEnd()
    {
        // Create start point
        int start = spanningTree.start;
        int startCol = start % h, startRow = start / h;

        startObj = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        startObj.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
        startObj.transform.position = new Vector3(startCol + 0.5f, 0.15f, -startRow - 0.5f);
        //Debug.Log("start: " + startRow + " " + startCol + " " + startObj.transform.position);

        // Change the color
        startObj.GetComponent<Renderer>().material.color = Color.green;

        // Create end point
        int end = spanningTree.end;
        int endCol = end % h, endRow = end / h;

        endObj = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        endObj.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
        endObj.transform.position = new Vector3(endCol + 0.5f, 0.15f, -endRow - 0.5f);
        //Debug.Log("end: " + endRow + " " + endCol + " " + endObj.transform.position);

        endObj.GetComponent<Renderer>().material.color = Color.white;
    }
}
