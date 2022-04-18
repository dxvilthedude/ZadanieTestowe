using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Wall : MonoBehaviour
{
    private int wallIndex;
    public int WallIndex
    {
        get { return wallIndex; }
        set 
        {
            wallIndex = value;
            SetNewNumber();
        }

    }

    [SerializeField] private Text numberText;
    private Collider wallCollider;
    private Dice Dice;

    private void Start()
    {
        Dice = FindObjectOfType<Dice>();
        wallCollider = GetComponent<SphereCollider>();
        wallCollider.isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        Scoreboard.Score = wallIndex;
    }
    private void OnTriggerExit(Collider other)
    {
        Scoreboard.Score = 0;
    }
    public void SetNumberText()
    {
        numberText.text = wallIndex.ToString();
    }
    private void SetNewNumber()
    {
        numberText.text = WallIndex.ToString();
    }
    

}
