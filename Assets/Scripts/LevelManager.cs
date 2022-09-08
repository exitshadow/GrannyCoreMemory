using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private GameObject model;
    [SerializeField] private Material[] randomMats;
    [SerializeField] private int rows;
    [SerializeField] private int columns;
    [SerializeField] private float xOffset = 1f;
    [SerializeField] private float yOffset = 1f;
    [SerializeField] private float coolDownTime = 2f;

    private List<MemoCard> memoCardsBehaviours = new List<MemoCard>();
    private List<Material> poolMats = new List<Material>();
    [HideInInspector] public List<MemoCard> revealedMemoCards = new List<MemoCard>();
    [HideInInspector] public List<MemoCard> matchedMemoCards = new List<MemoCard>();

    IEnumerator _WaitAndHide(float coolDownTime)
    {
        yield return new WaitForSeconds(coolDownTime);
        for (int i = 0; i < revealedMemoCards.Count; i++)
        {
            revealedMemoCards[i].HidePattern();
        }

        revealedMemoCards.Clear();
    }

    public void RevealCard(MemoCard card)
    {
        // blocking the procedure if self-clicking
        if (revealedMemoCards.Contains(card)
            || matchedMemoCards.Contains(card)
            || revealedMemoCards.Count == 2)
            return;

        Debug.Log($"{card.name} has been revealed");
        // nb: la propriété name est héritée de MonoBehaviour

        card.RevealPattern();

        revealedMemoCards.Add(card);

        if (revealedMemoCards.Count >= 2)
        {
            if (revealedMemoCards[0].hiddenFace == revealedMemoCards[1].hiddenFace)
            {
                for (int i = 0; i < revealedMemoCards.Count; i++)
                {
                    matchedMemoCards.Add(revealedMemoCards[i]);
                }
                revealedMemoCards.Clear();
            }
            else
            {
                StartCoroutine(_WaitAndHide(coolDownTime));
            }
        }
    }

    private void Start()
    {
        if ((rows * columns) %2 != 0)
        {
            Debug.LogError("Total of cards generated are an odd number. Generation cancelled.");
            return;
        }

        if ((rows * columns) != randomMats.Length * 2)
        {
            Debug.LogError("Number of materials available for pooling doesn’t match the quantity of cards generated. Generation cancelled.");
            return;
        }

        for (int i = 0; i < randomMats.Length; i++)
        {
            // add 2 instances of each material so we have a pool of materials
            // that is suitable for a memory game
            poolMats.Add(randomMats[i]);
            poolMats.Add(randomMats[i]);
        }

        for (float x = 0; x < columns * xOffset; x += xOffset)
        {
            for (float y = 0; y < rows * yOffset; y += yOffset)
            {
                Vector3 position = new Vector3 (x, .2f, y);
                Vector3 eulerRotation = new Vector3(0, Random.Range(-10, 10), 0);
                CreateCard(position, eulerRotation);
            }
        }
    }

    private void CreateCard(Vector3 position, Vector3 eulerRotation)
    {
        GameObject cardInstance = Instantiate(model, position, Quaternion.Euler(eulerRotation));
        MemoCard memoCardBehaviour = cardInstance.GetComponent<MemoCard>();

        // bind the relationship between the object and the current manager
        memoCardBehaviour.manager = this;
        memoCardsBehaviours.Add(memoCardBehaviour);

        int index = Random.Range(0, poolMats.Count);
        
        memoCardBehaviour.hiddenFace = poolMats[index];
        poolMats.Remove(poolMats[index]);
    }
}
