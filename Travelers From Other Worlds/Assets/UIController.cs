using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public ShipController ship;
    public TextMeshProUGUI speedText;
    public TextMeshProUGUI TargetTime;
    public RectTransform crosshair;
    public RectTransform prograde;
    public RectTransform targetIndicator;
    public Image[] images;
    public TextMeshProUGUI[] texts;
    private void Start()
    {
        crosshair.gameObject.SetActive(false);
        speedText.gameObject.SetActive(false);
        prograde.gameObject.SetActive(false);
        TargetTime.gameObject.SetActive(false);
        targetIndicator.gameObject.SetActive(false);
        
    }
    private void Awake() {
        
    }

    void LateUpdate()
    {
        images = FindObjectsOfType<Image>();
        texts = FindObjectsOfType<TextMeshProUGUI>();
        if (ship == null)
        {
            Debug.LogError("Jet controller is missing");
            return;
        }
        if(ship.cam.enabled)
        {
            crosshair.gameObject.SetActive(true);
            if(ship.showMotionVectors)
            {
                speedText.gameObject.SetActive(true);
                prograde.gameObject.SetActive(true);
                speedText.text = $"{ship.relativeVelocity.magnitude:n0}m/s";
                prograde.position = ship.progradePosition;
                speedText.GetComponent<RectTransform>().position = prograde.position + new Vector3(100,0,0);
            }else{
                speedText.gameObject.SetActive(false);
                prograde.gameObject.SetActive(false);
            }
            if(ship.hasTarget&&ship.apparentSize<1)
            {
                var color = targetIndicator.GetComponent<Image>().color;
                color.a=Mathf.Pow((1-ship.apparentSize),2);
                targetIndicator.GetComponent<Image>().color=color;
                targetIndicator.gameObject.SetActive(true);
                TargetTime.gameObject.SetActive(true);
                targetIndicator.position=ship.targetPosition;
                TargetTime.text = $"T{-ship.targetDistance/ship.relativeTargetVelocityMagnitude:n0}s";
                if(ship.relativeTargetVelocityMagnitude>0)
                    TargetTime.color = Color.green;
                else TargetTime.color = Color.red;
                TargetTime.GetComponent<RectTransform>().position = targetIndicator.position + new Vector3(targetIndicator.sizeDelta.x/3,targetIndicator.sizeDelta.x/8,0);
                targetIndicator.GetComponent<RectTransform>().sizeDelta = new Vector2(ship.apparentSize*800f+20f,ship.apparentSize*800f+20f);
            }else
            {
                targetIndicator.gameObject.SetActive(false);
                TargetTime.gameObject.SetActive(false);
            }
            crosshair.position = ship.crosshairPosition;
        }else{
            crosshair.gameObject.SetActive(false);
            speedText.gameObject.SetActive(false);
            prograde.gameObject.SetActive(false);
        }
        foreach(Image image in images)
        {
            if(image.gameObject.GetComponent<RectTransform>().position.z<0)
                image.enabled=false;
            else image.enabled = true;
        }
        foreach(TextMeshProUGUI text in texts)
        {
            if(text.gameObject.GetComponent<RectTransform>().position.z<0)
                text.enabled=false;
            else text.enabled = true;
        }
    }
}
