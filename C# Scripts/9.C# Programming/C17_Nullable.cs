using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class C17_Nullable : MonoBehaviour
{
    // 17. 널러블 (Nullable)

    // 클래스를 반환하는 함수의 경우, 아래와 같이 못 찾았으면 null을 반환하도록 하는 게 일반적이다.
    C17_Player FindPlayer(int code)
    {
        if (code == 1) return new C17_Player();
        return null;
    }


    // 하지만 int 등은 null을 대입할 수 없어서, 예측하지 못 한 결과가 나와도 확인하기 어렵다.
    // 이 때 nullable을 사용한다.

    [Button]
    void Test01()
    {
        // 형식 뒤에 ?를 붙여서 선언할 수 있다.
        int? num = 5;

        int num2 = (int)num; //널러블과 널러블이 아닌 것은 다른 형식이라 암시적으로 int로 변환할수는 없고, 캐스트가 필요하다.
        num2 = num.Value; //널러블.Value로 해당 널러블의 값을 가져올 수 있다. 즉 Value를 통해 int?를 int로 변경할 수 있다.

        num = null; //null을 대입할 수 있음을 알 수 있다. 이 상태에서 다른 곳에 대입하려 하면 에러가 뜰 것이다.

        if (num != null) num2 = num.Value; //클래스처럼 null이 아닌 경우에만 대입하도록 할 수 있다.
        if (!num.HasValue) Debug.Log("num이 Null 입니다."); //이는 .HasValue로 체크할 수 있다.

        //단, null인지 아닌지 if문으로 매번 체크하는 것은 번거로우므로, 주로 아래와 같은 형태로 사용한다.

        int num3 = num ?? 1; //널러블을 대입하고, 널러블이 null이면 뒤의 값을 대입한다. 캐스트 없이 대입 가능하다.
        Debug.Log(num3);
    }

    [Button]
    void Test02()
    {
        C17_Player player = null;
        int playerId;

        //일반적으로는 아래와 같이 사용하지만...
        if (player != null) playerId = player.Id;

        //클래스의 null 여부도 ?를 붙여서 체크할 수 있다. player가 null이면 에러를 띄우지 않고 그냥 playerId2에 null을 대입한다.
        int? playerId2 = player?.Id;
        Debug.Log($"플레이어 ID가 있습니까? - {playerId2.HasValue}");
    }

}

class C17_Player
{
    public int Id { get; set; }
}