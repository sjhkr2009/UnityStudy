using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Bar : UIBase_Scene
{
    enum CustomerType
    {
        Customer1,
        Customer2,
        Customer3
    }
    
    public override void Init()
    {
        base.Init();

        Bind<Image>(typeof(CustomerType));

        GetImage((int)CustomerType.Customer1).gameObject.SetActive(false);
        GetImage((int)CustomerType.Customer2).gameObject.SetActive(false);
        GetImage((int)CustomerType.Customer3).gameObject.SetActive(false);

        //GameManager.Instance.OnSetCustomer -= SetCustomer;
        //GameManager.Instance.OnSetCustomer += SetCustomer;
    }

    private void Start()
    {
        Init();
    }

    void SetCustomer(Customers value)
    {
        GetImage((int)CustomerType.Customer3).gameObject.SetActive(false);
        GetImage((int)CustomerType.Customer3).gameObject.SetActive(true);
        GetImage((int)CustomerType.Customer3).sprite = value.Image;
    }
}
