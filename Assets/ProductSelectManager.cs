using SKUtils.Feedbacks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ProductSelectManager : MonoSingleton<ProductSelectManager>
{
    [SerializeField] Material defaultMaterial, outlineMaterial;

    private SpriteRenderer prevSelectedObject;

    void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            if (prevSelectedObject != null)
                prevSelectedObject.material = defaultMaterial;

            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue()), Vector2.zero);

            if (hit.collider != null)
            {
                SetProductInfoPanel(hit.transform.gameObject);
                SetSelectedObjectMaterial(hit.transform.gameObject);
                PlaySelectedObjectPunchScale(hit.transform.gameObject);

                Debug.Log("Target object: " + hit.collider.gameObject.transform.name, hit.collider.gameObject);
            }
        }
    }

    private void SetProductInfoPanel(GameObject go)
    {
        Debug.Log("SetProductInfoPanel");
        Building building = go.GetComponentInChildren<Building>();
        List<ProductInfoDatas> productInfoDatas = new List<ProductInfoDatas>();
        productInfoDatas.Add(new ProductInfoDatas(building.buildingData, 1));
        InformationPanelHandler.Instance.SetInformationList(productInfoDatas);
    }

    private void SetSelectedObjectMaterial(GameObject go)
    {
        SpriteRenderer renderer = go.GetComponentInChildren<SpriteRenderer>();
        renderer.material = outlineMaterial;
        prevSelectedObject = renderer;
    }
    private void PlaySelectedObjectPunchScale(GameObject go)
    {
        PunchScaleFeedBack punchScaleFeedBack = go.GetComponentInChildren<PunchScaleFeedBack>();
        punchScaleFeedBack.PunchScale();
    }
}
