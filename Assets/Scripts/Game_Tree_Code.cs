using UnityEngine;
using System.Collections;

public class Game_Tree_Code : MonoBehaviour
{
    void ShowTitle()
    {
        transform.parent.GetChild(1).gameObject.SetActive(true);
    }
}