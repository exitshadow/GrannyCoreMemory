using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Renderer))]
[RequireComponent(typeof(Animator))]
public class MemoCard : MonoBehaviour
{
    [SerializeField] public Material hiddenFace;
    [SerializeField] private Material frontFace;

    [HideInInspector] public Material[] mats;
    private Renderer rend;
    private Animator animator;

    [HideInInspector] public LevelManager manager;

    private void Start()
    {
        animator = GetComponent<Animator>();
        rend = GetComponent<Renderer>();
        mats = GetComponent<Renderer>().materials;
        
    }

    private void OnMouseDown()
    {
        Debug.Log("Enter OnMouseDown");
        // on se passe au manager
        animator.SetBool("isMouseOver", false);
        animator.SetTrigger("Flip");
        manager.RevealCard(this);
    }

    private void OnMouseUp()
    {
        Debug.Log("Enter OnMouseUp");
        //HidePattern();
    }

    void OnMouseOver()
    {
        if (!manager.matchedMemoCards.Contains(this))
        {
            animator.SetBool("isMouseOver", true);
        }
    }

    void OnMouseExit()
    {
        animator.SetBool("isMouseOver", false);
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
