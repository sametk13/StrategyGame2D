using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Barrack : Building
{
    public OverlayTile destinationTile { get; private set; }
    public OverlayTile spawnTile { get; private set; }

    [SerializeField] private Transform _destinationTransform;
    [SerializeField] private Transform _spawnPointTransform;

    private SpriteRenderer _destinationSpriteRenderer;

    private void Start()
    {
        _destinationSpriteRenderer = _destinationTransform.GetComponentInChildren<SpriteRenderer>();
        HideDestinationSprite();
    }

    private void Update()
    {
        // Handling spawn point
        if (Mouse.current.rightButton.wasPressedThisFrame && isSelected)
        {
            RaycastHit2D? hit = Raycaster.GetMouseRaycastHit();
            if (hit == null) return;

            OverlayTile tile = hit.Value.collider.gameObject.GetComponent<OverlayTile>();
            if (tile == null) return;

            _destinationTransform.position = tile.transform.position;
            destinationTile = tile;
        }
    }

    public override void build()
    {
        base.build();

        Vector2Int tileToCheck = new Vector2Int((int)_spawnPointTransform.position.x, (int)_spawnPointTransform.position.y);
        spawnTile = GridMapManager.Instance.GetNearestOnTile(tileToCheck);

        _spawnPointTransform.position = spawnTile.transform.position;
        _destinationTransform.position = spawnTile.transform.position;

        destinationTile = spawnTile;
    }

    public override void Selected() //Selecting Building
    {
        base.Selected();

        List<ProductInfoDatas> productInfoDatas = new List<ProductInfoDatas>();
        productInfoDatas.Add(new ProductInfoDatas(buildingData, 1));
        //Handling Panels
        InformationManager.Instance.SetInformationList(productInfoDatas);

        List<ProductData> productDatas = new List<ProductData>();

        for (int i = 0; i < buildingData.unitDatas.Count; i++)
        {
            productDatas.Add(buildingData.unitDatas[i]);
        }

        ProductionMenuManager.Instance.SetProductCardList(productDatas,this);

        ShowDestinationSprite();
    }

    public override void UnSelected()
    {
        base.UnSelected();

        HideDestinationSprite();
    }

    private void ShowDestinationSprite()
    {
        _destinationSpriteRenderer.enabled = true;
    }

    private void HideDestinationSprite()
    {
        _destinationSpriteRenderer.enabled = false;
    }
}
