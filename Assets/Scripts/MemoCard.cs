using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class MemoCard : MonoBehaviour
{
    [SerializeField] public Material hiddenFace;
    [SerializeField] private Material frontFace;

    [HideInInspector] public Material[] mats;
    private Renderer rend;

    [HideInInspector] public LevelManager manager;

    private void Start()
    {
        rend = GetComponent<Renderer>();
        mats = GetComponent<Renderer>().materials;
        
    }

    private void OnMouseDown()
    {
        Debug.Log("Enter OnMouseDown");
        // on se passe au manager
        manager.RevealCard(this);
    }

    private void OnMouseUp()
    {
        Debug.Log("Enter OnMouseUp");
        //HidePattern();
    }

    public void RevealPattern()
    {
        mats[1] = hiddenFace;
        rend.materials = mats;

    }

    public void HidePattern()
    {
        mats[1] = frontFace;
        rend.materials = mats;
    }
}
