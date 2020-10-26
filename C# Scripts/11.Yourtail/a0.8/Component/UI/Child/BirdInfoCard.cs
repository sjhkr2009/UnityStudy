using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BirdInfoCard : MonoBehaviour
{
    Customers myBird;

    [SerializeField] Image birdImage;
    [SerializeField] Text birdName;
    [SerializeField] Button openInfoButton;

    private void Start()
    {
        ComponentCheck();
    }

    void ComponentCheck()
    {
        if (birdImage == null)
            birdImage = transform.GetChild(0).GetOrAddComponent<Image>();
        if (birdName == null)
            birdName = transform.GetChild(1).GetOrAddComponent<Text>();
        if (openInfoButton == null)
            openInfoButton = gameObject.GetOrAddComponent<Button>();
    }

    public void SetInfo(Customers customer)
    {
        myBird = customer;
        if (myBird == null) return;

        ComponentCheck();

        birdImage.sprite = myBird.Image;
        birdName.text = myBird.Name;
        openInfoButton.onClick.AddListener(OpenInfoWindow);
    }

    void OpenInfoWindow()
    {
        GameManager.UI.CurrentBirdInfo = myBird;
        GameManager.UI.OpenPopupUI<BirdInfoWindow>();
    }

    private void OnDisable()
    {
        if (openInfoButton == null) openInfoButton.onClick.RemoveAllListeners();
    }
}
