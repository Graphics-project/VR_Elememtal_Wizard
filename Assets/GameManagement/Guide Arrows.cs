using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuideArrows : MonoBehaviour
{
    private int currentLevel;
    private Transform[] children;

    void Start()
    {
        Transform[] allTransforms = gameObject.GetComponentsInChildren<Transform>();
        children = new Transform[allTransforms.Length - 1];
        for (int i = 1; i < allTransforms.Length; i++)
        {
            children[i - 1] = allTransforms[i];
        }
    }

    void Update()
    {
        currentLevel = GameManager.instance.level;

        foreach (Transform child in children)
        {
            SetActiveRecursively(child, false);
        }

        if (currentLevel > 0 && currentLevel <= children.Length)
        {
            SetActiveRecursively(children[currentLevel - 1], true);
        }
    }

    void SetActiveRecursively(Transform obj, bool state)
    {
        obj.gameObject.SetActive(state);
        foreach (Transform child in obj)
        {
            SetActiveRecursively(child, state);
        }
    }
}