using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dice : MonoBehaviour
{
    [Header("Put number of vertices in 1 wall")]
    [SerializeField] private int verticesInWall;

    [SerializeField] private Transform _Dice;
    [SerializeField] private GameObject WallPrefab;
    [SerializeField] private Scoreboard scoreboard;

    private Vector3 startingPosition;
    private Vector3 diceCenter;
    private Rigidbody rb;
    private float dirX;
    private float dirY;
    private float dirZ;

    [Header("Unselect to use unique numbers on dice walls")]
    public bool UsingStandardNumbers = true;
    public int[] newUniqueNumbers = new int[12];

    public bool diceMoving;

    void Start()
    {
        startingPosition = transform.position;
        diceMoving = false;
        rb = GetComponent<Rigidbody>();
        diceCenter = _Dice.GetComponent<Renderer>().bounds.center;
        Mesh mesh = _Dice.gameObject.GetComponent<MeshCollider>().sharedMesh;

        /*for loop iterates through all vertives in collider mesh, to find all faces
           calculates position in center of every face, then adds wall prefab in calculated position for every one of them.
           Wall prefab contains Canvas with number, Wall script, and sphere collider.  
         * */
        for (int i = 0; i < mesh.vertices.Length; i+=verticesInWall)
        {            
            Vector3 pos = Vector3.zero;
            for (int j = 0; j < verticesInWall; j++)
            {
                pos.x += mesh.vertices[i + j].x;
                pos.y += mesh.vertices[i + j].y;
                pos.z += mesh.vertices[i + j].z;
            }
            pos = pos / verticesInWall;
    
            GameObject wall = Instantiate(WallPrefab,_Dice);
            wall.transform.localPosition = Vector3.zero + pos;
            wall.transform.LookAt(diceCenter);
            wall.transform.position = Vector3.Lerp(wall.transform.position,(2 * wall.transform.position - diceCenter),0.01f);

            Wall currentWall = wall.GetComponent<Wall>();

            /* If UsingStandardNumbers is true, code sets standard numbers for every wall (1-12)
             * If bool is false, code sets newUniqueNumbers for every wall.
             * You can set unique numbers in Inspector
             * */
            if (UsingStandardNumbers)
                currentWall.WallIndex = i / verticesInWall + 1;
            else
                currentWall.WallIndex = newUniqueNumbers[i / verticesInWall];

            currentWall.SetNumberText();

            //After instantiating wall prefab, code calculates new position for sphere collider , on other side of Dice

            SphereCollider sCollider = wall.GetComponent<SphereCollider>();
            sCollider.center = new Vector3(sCollider.center.x,sCollider.center.y, sCollider.center.z + 2* Vector3.Distance(wall.transform.position,diceCenter));
        }
    }
    private void Update()
    {
        /*Detects if dice movement velocity is close to 0, to display score
         * */
        if (diceMoving)
        {
            if (rb.velocity.magnitude ==0)
            {
                if (Scoreboard.Score != 0)
                {
                    scoreboard.ShowScore();
                    diceMoving = false;
                }
                else
                {
                    //Resets dice position when dice stopped moving but didnt land on any wall
                    _Dice.transform.position = startingPosition;
                    diceMoving = false;
                }

            }
        }
    }
    public void _AutoRoll()
    {
        StartCoroutine(AutoRoll());
    }

    /*IEnumerator with WaitForSecond to prevent adding score in first frame of auto throwing,
      by changing diceMoving to true, when dice is still colliding with table
    */
    IEnumerator AutoRoll()
    {
        if (!diceMoving)
        {
            scoreboard.SetScoreText("?");
            
            dirX = Random.Range(0, 500);
            dirY = Random.Range(0, 500);
            dirZ = Random.Range(0, 500);
            _Dice.position = (new Vector3(0, 2, 0));
            _Dice.rotation = Quaternion.identity;
            rb.AddForce(transform.up * 500);
            rb.AddTorque(dirX, dirY, dirZ);

            yield return new WaitForSeconds(0.2f);

            diceMoving = true;
            Scoreboard.Score = 0;
        }
        yield return null;
    }
}
