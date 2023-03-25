using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pointerExit : MonoBehaviour
{
    public Transform player;
    public GameObject pointer;
    Camera camera;

    private void Start()
    {
        pointer.SetActive(false);
        camera = Camera.main;
        AllIvents.pointerExitShow.AddListener(Ivent);
    }
    
    public void Ivent()
    {
        StartCoroutine(Show());
    }

    IEnumerator Show()
    {
        pointer.SetActive(true);
        yield return new WaitForSeconds(4f);
        pointer.SetActive(false);
    }

    private void Update()
    {
        Vector3 fromPlayerToExit = transform.position - player.position;
        Ray ray = new Ray(player.position, fromPlayerToExit);
        Debug.DrawRay(player.position, fromPlayerToExit);

        Plane[] planes = GeometryUtility.CalculateFrustumPlanes(camera);

        float minDistance = Mathf.Infinity;

        // Нужны первые 4 плана
        for (int i = 0; i < 4; i++) 
        {
            if(planes[i].Raycast(ray, out float distance))
            {
                if(distance < minDistance)
                {
                    minDistance = distance;
                }
            }
        }

        minDistance = Mathf.Clamp(minDistance, 0, fromPlayerToExit.magnitude);
         
        Vector3 worldPosition = ray.GetPoint(minDistance);
        pointer.transform.position = worldPosition;
    }
}