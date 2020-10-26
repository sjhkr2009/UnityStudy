using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager
{
    public T Load<T>(string path) where T : Object
    {
        T loaded = Resources.Load<T>(path);
        if (loaded == null)
            Debug.Log($"로드에 실패했습니다. 주소를 다시 확인해주세요. {path}");
        return loaded;
    }

    public ScriptableData LoadDatabase() => Load<ScriptableData>("Data/GameData");
    //public BirdStories LoadDialogData() => Load<BirdStories>("Data/StoryTexts");

    public Sprite LoadImage(Define.ImageType type, int id)
    {
        ScriptableImages data = SelectData(type);
        int index = id;

        if(id >= data.sprites.Count)
        {
            index = 0;
            Debug.Log("데이터베이스에 없는 이미지 로드를 시도했습니다.");
        }
        return data.sprites[index];
    }
    ScriptableImages SelectData(Define.ImageType imageType)
    {
        switch (imageType)
        {
            case Define.ImageType.Base:
                return Load<ScriptableImages>("Data/BaseMaterials");
            case Define.ImageType.Sub:
                return Load<ScriptableImages>("Data/SubMaterials");
            case Define.ImageType.Cocktail:
                return Load<ScriptableImages>("Data/Cocktail");
            case Define.ImageType.Customer:
                return Load<ScriptableImages>("Data/Customers");
            default:
                return null;
        }
    }
        

    public GameObject Instantiate(string path, Transform parent = null)
    {
        GameObject gameObject = Load<GameObject>($"Prefabs/{path}");
        if (gameObject == null)
        {
            Debug.Log("오브젝트 정보를 불러오는 데 실패했습니다.");
            return null;
        }

        return Object.Instantiate(gameObject, parent);
    }

    public void Destroy(GameObject gameObject, float delay = 0f)
    {
        Object.Destroy(gameObject, delay);
    }

    public void Disable(GameObject gameObject, float delay = 0f)
    {
        DOVirtual.DelayedCall(delay, () => { gameObject.SetActive(false); });
    }
}
