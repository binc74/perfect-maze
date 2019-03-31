using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeController : MonoBehaviour
{
    // number of columns
    public int HorizontalNodeCount;
    // number of rows
    public int VerticalNodeCount;

    private GameObject mainCam;
    private GameObject ground;
    // The spanning tree using node representation
    private SpanningTree spanningTree;

    // The object that stores all the tiles
    private Tiles tiles;

    // Use for solving maze
    private MazeSolver mazeSolver;

    public float dt;

    private float totalTime;
    private float deltaTime;

    private bool needSolve;

    // Start is called before the first frame update
    void Start()
    {
        // Construct the spanning tree
        spanningTree = new SpanningTree(HorizontalNodeCount, VerticalNodeCount);
        tiles = new Tiles(HorizontalNodeCount, VerticalNodeCount, spanningTree);        

        // Init the start point and end point
        spanningTree.InitStartAndEnd();
        tiles.DrawStartAndEnd();

        // Initialize the position of camera
        mainCam = GameObject.Find("Main Camera");
        mainCam.transform.position = new Vector3(HorizontalNodeCount / 2, 10, -VerticalNodeCount / 2);

        mazeSolver = new MazeSolver(HorizontalNodeCount, VerticalNodeCount, spanningTree);

        // Initialize ground
        ground = GameObject.CreatePrimitive(PrimitiveType.Cube);
        ground.transform.position = new Vector3(((float)HorizontalNodeCount)/2, 0, -((float)VerticalNodeCount)/2);
        ground.transform.localScale = new Vector3(HorizontalNodeCount, 0f, VerticalNodeCount);
        ground.GetComponent<Renderer>().material.color = Color.black;

        deltaTime = dt / 1000;
        needSolve = false;
        // Print the spanning tree for testing
        //for (int i = 0; i < spanningTree.Size(); ++i)
        //    Debug.Log(i + ": " + string.Join(",", spanningTree.GetEdges(i)));
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 currPos = mainCam.transform.position;
        totalTime += Time.deltaTime;

        // Reconstruct the maze
        if (Input.GetKeyDown(KeyCode.R))
        {
            tiles.Destroy();

            spanningTree = new SpanningTree(HorizontalNodeCount, VerticalNodeCount);
            spanningTree.InitStartAndEnd();
            tiles = new Tiles(HorizontalNodeCount, VerticalNodeCount, spanningTree);
            tiles.DrawStartAndEnd();
            mazeSolver.ClearPath();
            mazeSolver = new MazeSolver(HorizontalNodeCount, VerticalNodeCount, spanningTree);         

            totalTime = 0;
            needSolve = false;
        }
        if (Input.GetKey(KeyCode.W))
        {
            currPos.z += 0.1f;
        }
        if (Input.GetKey(KeyCode.S))
        {
            currPos.z -= 0.1f;
        }
        if (Input.GetKey(KeyCode.A))
        {
            currPos.x -= 0.1f;
        }
        if (Input.GetKey(KeyCode.D))
        {
            currPos.x += 0.1f;
        }
        if (Input.GetKey(KeyCode.Q))
        {
            currPos.y += 0.1f;
        }
        if (Input.GetKey(KeyCode.E))
        {
            currPos.y -= 0.1f;
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            mazeSolver.Solve();
        }

        if (totalTime > deltaTime)
        {
            if (!tiles.IsEnd()) tiles.Construct();

            totalTime -= deltaTime;
        }

        mainCam.transform.position = currPos;
    }
}
