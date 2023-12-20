using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalGate : MonoBehaviour, IInteractable
{
    public GameObject gate;
    public GameObject player;
    public void TriggerAction()
    {
        if (player.GetComponent<PlayerController>().level == 5)
        {
            gate.SetActive(false);
        }
    }

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }
    }
}
