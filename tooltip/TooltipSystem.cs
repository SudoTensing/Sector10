using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class TooltipSystem : MonoBehaviour
{

    public Vector2 offset = new Vector2();
    public Vector2 currentSize;
    public Vector2 maxSizeX = new Vector2();
    public Vector2 maxSizeY = new Vector2();

    public Vector2 Debug1, Debug2, Debug3 = new Vector2();

    private string name = "";
    private string text = "";

    public GameObject tooltipQuad;

    public GameObject ActionPrefab;
    public GameObject PrefabSpawn;
    public Image tooltipArea;
    public GameObject tooltipControls;
    public GameObject tooltipText;
    

    public string Text { get => text; set => text = value; }
    public string Name { get => name; set => name = value; }

    private int totalLines = 1;

    void Start()
    {
        
    }

    //- this all works smooth, but its too hardcoded and not enough freedom to manipulate from other sources
    //- needs better data handling for variables
    //- what if variables are empty, UI is still displayed (easy)
    //- UTF8 ?
    //- SUDOTENSING


    public void SetTooltip(List<string> text, List<string> actions) {
        var finalText = "";
        foreach(string str in text) {
            finalText += str + Environment.NewLine;
        }
        tooltipText.gameObject.transform.Find("border").Find("text").gameObject.GetComponent<TextMeshProUGUI>().text = finalText;

        // spawn TooltipControls for each Action in list
        int index = 0;
        foreach(string str in actions) {
            index++;
            GameObject spawn = Instantiate(ActionPrefab, PrefabSpawn.transform, false);
            spawn.transform.Find("text").GetComponent<TextMeshProUGUI>().text = index + ". "+str;
        }


        tooltipControls.GetComponent<RectTransform>().sizeDelta = new Vector2(250, 0 + (actions.Count * 30) + 5);

    }
    
    void Update()
    {

        if (tooltipQuad != null) {

            //tooltipPanel. SetActive(true);
            tooltipArea.gameObject.SetActive(true);
            tooltipText.gameObject.SetActive(true);
            tooltipControls.gameObject.SetActive(true);

            Rect rect = GUIRectWithObject(Camera.main, tooltipQuad);
            RectTransform rt = tooltipArea.GetComponent<RectTransform>();
            var currX = Mathf.Clamp(rect.size.x, maxSizeX.x, maxSizeX.y);
            var currY = Mathf.Clamp(rect.size.y, maxSizeY.x, maxSizeY.y);
            rt.sizeDelta = new Vector2(currX,currY);
            var overShotX = 0f;
            var overShotY = 0f;
            if(rect.size.x > maxSizeX.y) { overShotX = +(rect.size.x - maxSizeX.y) / 2f; }
            if (rect.size.y > maxSizeY.y) { overShotY = +(rect.size.y - maxSizeY.y) / 2f; }
            rt.anchoredPosition = rect.position + new Vector2(overShotX, overShotY);




            RectTransform rt2 = tooltipControls.GetComponent<RectTransform>();
            rt2.anchoredPosition = rt.anchoredPosition + new Vector2(currX, (currY - rt2.sizeDelta.y/1)) + offset;

            RectTransform rt3 = tooltipText.GetComponent<RectTransform>();
            rt3.anchoredPosition = rt.anchoredPosition + new Vector2(0, -rt3.sizeDelta.y);

            // DEBUG
            Debug1 = rect.size;
            Debug2 = rt.anchoredPosition;
            currentSize = new Vector2(currX, currY);
        } else {

            foreach (Transform child in PrefabSpawn.transform) {
                GameObject.Destroy(child.gameObject);
            }

            tooltipArea.gameObject.SetActive(false);
            tooltipText.gameObject.SetActive(false);
            tooltipControls.gameObject.SetActive(false);


           
        }
    }



    public static Rect GUIRectWithObject(Camera cam, GameObject ship) {

        var renderer = ship.GetComponent<Renderer>();
        Vector3 cen = renderer.bounds.center;
        Vector3 ext = renderer.bounds.extents;

        Vector2[] extentPoints = new Vector2[8]
        {
         cam.WorldToScreenPoint(new Vector3(cen.x-ext.x, cen.y-ext.y, cen.z-ext.z)),
         cam.WorldToScreenPoint(new Vector3(cen.x+ext.x, cen.y-ext.y, cen.z-ext.z)),
         cam.WorldToScreenPoint(new Vector3(cen.x-ext.x, cen.y-ext.y, cen.z+ext.z)),
         cam.WorldToScreenPoint(new Vector3(cen.x+ext.x, cen.y-ext.y, cen.z+ext.z)),
         cam.WorldToScreenPoint(new Vector3(cen.x-ext.x, cen.y+ext.y, cen.z-ext.z)),
         cam.WorldToScreenPoint(new Vector3(cen.x+ext.x, cen.y+ext.y, cen.z-ext.z)),
         cam.WorldToScreenPoint(new Vector3(cen.x-ext.x, cen.y+ext.y, cen.z+ext.z)),
         cam.WorldToScreenPoint(new Vector3(cen.x+ext.x, cen.y+ext.y, cen.z+ext.z))
        };
        Vector2 min = extentPoints[0];
        Vector2 max = extentPoints[0];
        foreach (Vector2 v in extentPoints) {
            min = new Vector2(Mathf.Min(min.x, v.x), Mathf.Min(min.y, v.y));
            max = new Vector2(Mathf.Max(max.x, v.x), Mathf.Max(max.y, v.y));
        }


        return new Rect(min.x, min.y, max.x - min.x, max.y - min.y);
    }

}
