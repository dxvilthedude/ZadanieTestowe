using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragDice : MonoBehaviour
{
    [SerializeField] private Dice dice;
    [SerializeField] private Scoreboard scoreboard;
    public float ThrowSmoothTime = 0.1f;
    private Rigidbody rb;
    private Vector3 startingPosition;
    private Vector3 mOffset;
    private float mZCoord;

    
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    private void OnMouseDown()
    {
        if (dice.diceMoving)
            return;
        scoreboard.SetScoreText("?");
        mZCoord = Camera.main.WorldToScreenPoint(gameObject.transform.position).z;
        mOffset = gameObject.transform.position - GetMouseWorldPos();

    }
    private void OnMouseUp()
    {
        if (dice.diceMoving)
            return;

        dice.diceMoving = true;
        rb.useGravity = true;

        Vector3 throwVector = gameObject.transform.position - startingPosition;
        float speed = throwVector.magnitude / Time.deltaTime;
        Vector3 throwVelocity = speed * ThrowSmoothTime * throwVector.normalized;
        rb.velocity = throwVelocity;
        ThrowRotation();
    }
    private void OnMouseDrag()
    {
        if (dice.diceMoving)
            return;

        /* Calculates position of mouse with switched z and y axis
         * 
         */

        startingPosition = gameObject.transform.position;
        Vector3 newPos = GetMouseWorldPos();
        newPos.z = Mathf.Clamp(GetMouseWorldPos().y,-9,9);
        newPos.x = Mathf.Clamp(newPos.x, -10, 8);
        newPos.y = 3f;
        transform.position = newPos + mOffset; 
    }
    private Vector3 GetMouseWorldPos()
    {
        Vector3 mousePoint = Input.mousePosition;
        mousePoint.z = mZCoord;
        return Camera.main.ScreenToWorldPoint(mousePoint);

    }

    //Adds small random rotation while throwing dice
    private void ThrowRotation()
    {
        float dirX = Random.Range(0, 100);
        float dirY = Random.Range(0, 100);
        float dirZ = Random.Range(0, 100);
        rb.AddTorque(dirX, dirY, dirZ);
    }

}
