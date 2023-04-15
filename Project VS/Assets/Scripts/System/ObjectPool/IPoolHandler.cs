/** 오브젝트 풀링 대상인 오브젝트에 IPoolHandler 컴포넌트가 있을 경우, 추가적인 초기화와 릴리즈 작업을 수행합니다. */
public interface IPoolHandler {
    void OnInitialize();
    void OnRelease();
}
