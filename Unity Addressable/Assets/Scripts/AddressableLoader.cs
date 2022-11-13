using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class AddressableLoader : MonoBehaviour  {
    /* 어드레서블 로드 플로우
     * 참고) 어드레서블은 에셋번들 기반의 시스템으로, 에셋번들 내용이 업데이트되어도 internal id가 같아서 새 에셋번들을 런타임에 받아 사용하지 못 하는 현상을 개선한 것.
     *       (string 키 : 에셋번들) 형태로 저장한 후, 키값으로 에셋을 불러와 ResourceLocation으로 변환, 동적으로 internal id를 할당한다.
     *
     * 1. InitializeAsync() 호출 시 catalog.json 오픈 -> 에셋번들들의 정보와 어드레서블 키 값 등을 읽음
     * 2. ResourceLocator를 통해 string 키값으로 해당 에셋을 IResourceLocation 형태로 변환시킨다
     *      ㄴ ResourceLocation에 해당 에셋을 로드하기 위한 정보(Dependency/Key/Provider 등)가 들어있기 때문
     *      ㄴ 어드레서블 빌드 시 의존성이 있는 에셋들도 모두 에셋번들에 포함된다. 여러 번들에 의존성이 있는 요소가 중복 빌드되는걸 방지하려면 Addressable - Analysis에서 확인할 것.
     * 3. 해당 에셋 로드 시 변환된 ResourceLocation에 접근, 적절한 ResourceProvider를 사용해 실제 에셋으로 로드한다.
     *      ㄴ Provider는 해당 에셋번들 그룹에 정의되어 있다.
     */
    
    
    private void Start() {
        LoadAsync();
    }
    
    // Window - Asset Management - Addressables - Event Viewer 에서 레퍼런스 카운트를 확인할 수 있다
    // 단, AddressableAssetSettings의 옵션에서 Diagnostics - Send Profiler Events 가 체크되어 있어야 프로파일링 가능
    async Task LoadAsync() {
        // 모든 비동기함수는 AsyncOperationHandle을 반환한다.
        // 코루틴일 경우 함수 자체를 yield return 하거나, Addressables.InitializeAsync().Completed에 콜백을 넣을 수 있다.
        await Addressables.InitializeAsync().Task;

        var loadHandle = Addressables.LoadAssetAsync<GameObject>("NamePopup");
        await loadHandle.Task;
        
        // 레퍼런스 카운트의 증가/감소를 보기 위해 딜레이를 둔다. 원래는 대기할 필요 없음.
        await Task.Delay(1000);
        
        GameObject createdObj = null;
        var instHandle = Addressables.InstantiateAsync("NamePopup");
        // 완료 시 콜백으로 반환값을 createdObj에 할당
        instHandle.Completed += ret => createdObj = ret.Result;

        await instHandle.Task;

        for (int i = 0; i < 3; i++) {
            Debug.Log($"Wait Destroy... {3 - i}sec");
            await Task.Delay(1000);
        }
        
        // 릴리즈는 대기할 필요 없음
        Addressables.ReleaseInstance(createdObj);
        
        // 레퍼런스 카운트의 증가/감소를 보기 위해 딜레이를 둔다. 원래는 대기할 필요 없음.
        await Task.Delay(1000);
        Addressables.Release(loadHandle); // Load시 받은 핸들을 보관했다가 언로드 때 사용할 수 있다.
    }
    
    /* 어드레서블 다운로드
     *
     * 어드레서블 초기화 이후 진행한다.
     * 
     * 1. Catalog 다운로드: Remote Catalog와 Local Catalog의 해시값이 다르면 카탈로그 다운로드가 필요하다고 판정 (해시는 어드레서블 내의 에셋이 바뀔때마다 변경된다)
     *      ㄴ CheckForCatalogUpdates: 카탈로그 업데이트 여부를 확인한다.
     *      ㄴ UpdateCatalogs: 카탈로그를 다운로드해서 로컬에 캐싱한다.
     *
     * 2. Size 다운로드: 새로 받을 번들의 크기를 확인한다. 이후 크기에 따라 유저에게 동의를 구할 수 있다. 다운받을게 없다면 사이즈는 0이 된다.
     *      ㄴ DownloadSizeAsync: 라벨을 입력하여 어드레서블 안에서 해당 라벨의 에셋을 하나라도 포함하는 에셋번들 중 다운로드가 필요한 요소들의 크기를 반환한다.
     *
     * 3. Bundle 다운로드: 에셋번들을 다운로드한다.
     *      ㄴ DownloadDependenciesAsync: 라벨을 입력하여 해당 라벨을 포함하는 에셋번들에 대해 다운로드를 진행한다. 
     */
    async Task DownLoadAsync() {
        
    }
}
