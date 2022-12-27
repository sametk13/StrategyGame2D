using UnityEngine;
using UnityEngine.UI;

public class BuildingManager : MonoSingleton<BuildingManager>
{
    GameObject currentBuilding;

    public void InitiateSpawn(BuildingData buildingData)
    {
        currentBuilding = Instantiate(buildingData.BuildingPrefab);
        SpriteRenderer currentSpriteRenderer = currentBuilding.GetComponent<SpriteRenderer>();
        Color newColor = currentSpriteRenderer.color;
        newColor.a = 150f;
        currentSpriteRenderer.color = newColor;
    }

    private void Update()
    {
        if (currentBuilding != null)
        {
            if (Input.GetMouseButtonDown(1))
            {
                Destroy(currentBuilding);
            }
            else if (Input.GetMouseButtonDown(0))
            {
                // build
            }
            else
            {
                currentBuilding.transform.position = Input.mousePosition;
            }
        }
    }
}
