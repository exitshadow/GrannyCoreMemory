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

    private List<MemoCard> memoCardsBehaviours = new List<MemoCard>();
    private List<MemoCard> revealedMemoCards = new List<MemoCard>();
    private List<Material> poolMats = new List<Material>();

    public void RevealCard(MemoCard card)
    {
        Debug.Log($"{card.name} has been revealed");
        // nb: la propriété name est héritée de MonoBehaviour

        card.RevealPattern();
        
        // blocking the procedure if self-clicking
        if (revealedMemoCards.Contains(card)) return;

        revealedMemoCards.Add(card);

        if (revealedMemoCards.Count >= 2)
        {
            if (revealedMemoCards[0].hiddenFace == revealedMemoCards[1].hiddenFace)
            {
                Debug.Log("Matching patterns!");
            } else Debug.Log("No matching patterns");

            revealedMemoCards.Clear();
        }
    }

    private void Start()
    {
        if ((rows * columns) %2 != 0)
        {
            Debug.LogError("Total of cards generated are an odd number. Generation cancelled.");
            return;
        }

        if ((rows * columns) > randomMats.Length * 2)
        {
            Debug.LogError("Number of materials available for pooling are insufficient for the quantity of cards generated. Generation cancelled.");
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
                Vector3 position = new Vector3 (x, .1f, y);
                CreateCard(position);
            }
        }
    }

    private void CreateCard(Vector3 position)
    {
        GameObject cardInstance = Instantiate(model, position, Quaternion.identity);
        MemoCard memoCardBehaviour = cardInstance.GetComponent<MemoCard>();

        // bind the relationship between the object and the current manager
        memoCardBehaviour.manager = this;
        memoCardsBehaviours.Add(memoCardBehaviour);

        int index = Random.Range(0, poolMats.Count);
        
        memoCardBehaviour.hiddenFace = poolMats[index];
        poolMats.Remove(poolMats[index]);
    }
}
