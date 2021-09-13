using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 이미지를 뽑아내고 싶은 타워 tower가 있다면
// MakeTowerImage(tower); 를 호출하면
// 해당 타워를 진실의 방에서 복제하여 카메라로 찍고 그 영상을 Texture로 반환해줍니다.
public class TowerPreviewImage : MonoBehaviour
{
    // 프리팹, 또는 씬에 존재하던 타워의 리스트
    [ReadOnly] [SerializeField] List<GameObject> imageModels = null;
    // 생성된 임시 오브젝트들은 리스트로 관리
    [SerializeField]
    List<GameObject> modelInstance = null;
    [SerializeField]
    List<RenderTexture> renderTextures = null;
    List<int> imageUseCount = null;
    [SerializeField] Vector3 defaultModelPosition;
    [SerializeField] Vector3 CameraPosOffset;
    [SerializeField] Vector3 CameraRotate;

    [SerializeField] float Cam_OrthographicSize = 2.0f;
    // Start is called before the first frame update
    void Start()
    {
        // singleton
        imageModels = new List<GameObject>();
        modelInstance = new List<GameObject>();
        renderTextures = new List<RenderTexture>();
        imageUseCount = new List<int>();

    }

    // Update is called once per frame
    void Update()
    {

    }

    public RenderTexture GetTowerImage(GameObject model)
    {
        int index = imageModels.FindIndex(existed => existed.Equals(model));
        if (index != -1) // 기존에 존재할 경우
        {
            imageUseCount[index]++;
            return renderTextures[index];
        }
        else // 기존에 존재 안할 경우
        {
            return GetNewTowerImage(model);
        }
    }

    RenderTexture GetNewTowerImage(GameObject model)
    {
        RenderTexture imageTexture = MakeRuntimeTexture();
        GameObject toweInstance = SpawnModel(model);
        SetCamera(toweInstance, imageTexture);
        SortInstancePositions();
        imageModels.Add(model); // 프리팹
        modelInstance.Add(toweInstance); // 씬에 Instantiate 된 것
        renderTextures.Add(imageTexture); // 같은 모델을 공유할 때 기존 텍스쳐 재활용
        imageUseCount.Add(1); // modelInstance를 정리할 지 판단 기준

        return imageTexture;
    }

    RenderTexture MakeRuntimeTexture()
    {
        RenderTexture texture = new RenderTexture(512, 512, 16, RenderTextureFormat.ARGB32);
        return texture;
    }

    GameObject SpawnModel(GameObject model)
    {
        Vector3 spawnPosition = defaultModelPosition + new Vector3(imageModels.Count * Cam_OrthographicSize * 2, 0, 0);
        GameObject newModel = Instantiate(model, spawnPosition, Quaternion.identity);

        return newModel;
    }

    void SetCamera(GameObject model, RenderTexture imageTexture)
    {
        GameObject cameraObject = new GameObject("TowerCam");
        cameraObject.AddComponent<Camera>();

        // 위치 선정
        cameraObject.transform.parent = model.transform;
        cameraObject.transform.localPosition = CameraPosOffset; // 임의로 지정한 값. 자세한 값은 측정 후 재입력 필요
        cameraObject.transform.rotation = Quaternion.Euler(CameraRotate.x, CameraRotate.y, CameraRotate.z);

        Camera cameraComponent = cameraObject.GetComponent<Camera>();
        cameraComponent.clearFlags = CameraClearFlags.SolidColor;
        cameraComponent.orthographic = true;
        cameraComponent.orthographicSize = Cam_OrthographicSize;
        cameraComponent.farClipPlane = 4.0f;
        cameraComponent.targetTexture = imageTexture;
    }

    void SortInstancePositions()
    {
        for (int i = 0; i < modelInstance.Count; i++)
        {
            modelInstance[i].transform.position = defaultModelPosition + new Vector3(i * Cam_OrthographicSize * 2, 0, 0);
        }
    }

    public void CloseImage(GameObject towerModel)
    {
        int index = imageModels.FindIndex(existed => existed.Equals(towerModel));
        if (index != -1)
        {
            imageUseCount[index]--;
            if (imageUseCount[index] <= 0)
            {
                DeleteTowerImage(index);
            }
        }
    }

    public void DeleteTowerImage(int index)
    {
        imageModels.RemoveAt(index);
        Destroy(modelInstance[index]);
        modelInstance.RemoveAt(index);
        Destroy(renderTextures[index]);
        renderTextures.RemoveAt(index);
        imageUseCount.RemoveAt(index);
        SortInstancePositions();
    }

}
