using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton : MonoBehaviour
{
    public PlayerCombat playerCombat;
    public PlayerMovement playerMovement;
    public HealthUI healthUI;


    //standart ->
    private static Singleton instance;
   public static Singleton GetInstance 
    {
        get
        {
            if(instance == null) 
            {
                instance = GameObject.FindObjectOfType<Singleton>();
            }
            return instance;
        }
    }
}
