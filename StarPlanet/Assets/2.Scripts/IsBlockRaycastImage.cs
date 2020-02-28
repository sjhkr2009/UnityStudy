using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class IsBlockRaycastImage : MonoBehaviour
{
    [SerializeField] bool isBlockRaycast = true;
    Image image;
    void Awake()
    {
        image = GetComponent<Image>();
        //if(isBlockRaycast) image.blocksRaycasts = true;
    }
}
