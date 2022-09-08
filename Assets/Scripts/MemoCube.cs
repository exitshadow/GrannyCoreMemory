using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class MemoCube : MonoBehaviour
{
    [SerializeField] private Material hiddenMat;
    [SerializeField] private Material coverMat;

    private void Start()
    {
        coverMat = GetComponent<Renderer>().material;
    }

    // OnMouseOver et OnMouseExit fonctionne en fait
    // avec un RayCast depuis l’origine de la souris
    // et le collider de l’objet

    private void OnMouseOver()
    {
    }

    private void OnMouseExit()
    {
        GetComponent<Renderer>().material = coverMat;
    }

    private void OnMouseUp()
    {
        RevealPattern();
    }

    private void RevealPattern()
    {
        GetComponent<Renderer>().material = hiddenMat;
    }
}
