using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ShipUI : MonoBehaviour
{
    public ShipController ship;
    public TextMeshProUGUI speedText;
    public TextMeshProUGUI TargetTime;
    public RectTransform crosshair;
    public RectTransform prograde;
    public RectTransform targetIndicator;
    public RectTransform circleCrosshair;
    public RectTransform retrograde;
    public TextMeshProUGUI retrogradeText;
    public Image[] images;
    public TextMeshProUGUI[] texts;
    public bool showUI = false;
    private void Start()
    {
        crosshair.gameObject.SetActive(false);
        speedText.gameObject.SetActive(false);
        prograde.gameObject.SetActive(false);
        TargetTime.gameObject.SetActive(false);
        targetIndicator.gameObject.SetActive(false);
        retrogradeText.gameObject.SetActive(false);
        retrograde.gameObject.SetActive(false);
    }
    private void Awake() {
        
    }

    void Update()
    {
        images = GetComponentsInChildren<Image>();
        texts = GetComponentsInChildren<TextMeshProUGUI>();
        if(showUI==false)
        {
            foreach (TextMeshProUGUI text in texts)
                text.gameObject.SetActive(false);
            foreach (Image image in images)
                image.gameObject.SetActive(false);
        }
        else
        {
            if (ship == null)
            {
                return;
            }
            crosshair.gameObject.SetActive(true);
            circleCrosshair.gameObject.SetActive(true);
            if (ship.showMotionVectors)
            {
                speedText.gameObject.SetActive(true);
                prograde.gameObject.SetActive(true);
                retrograde.gameObject.SetActive(true);
                retrogradeText.gameObject.SetActive(true);
                speedText.text = $"{ship.relativeVelocity.magnitude:n0}m/s";
                prograde.position = ship.progradePosition;
                speedText.GetComponent<RectTransform>().position = prograde.position + new Vector3(prograde.rect.size.x, 0, 0);
                retrograde.position = new Vector3(ship.progradePosition.x, ship.progradePosition.y, -ship.progradePosition.z);
                retrogradeText.text = $"{-ship.relativeVelocity.magnitude:n0}m/s";
                retrogradeText.GetComponent<RectTransform>().position = retrograde.position + new Vector3(retrograde.rect.size.x,0,0);
            }
            else
            {
                speedText.gameObject.SetActive(false);
                prograde.gameObject.SetActive(false);
                retrograde.gameObject.SetActive(false);
                retrogradeText.gameObject.SetActive(false);
            }
            if (ship.hasTarget && ship.apparentSize < 1)
            {
                var color = targetIndicator.GetComponent<Image>().color;
                color.a = Mathf.Pow((1 - ship.apparentSize), 2);
                targetIndicator.GetComponent<Image>().color = color;
                targetIndicator.gameObject.SetActive(true);
                TargetTime.gameObject.SetActive(true);
                targetIndicator.position = ship.targetPosition;
                if (-ship.targetDistance / ship.relativeTargetVelocityMagnitude > 0)
                    TargetTime.text = $"T+{-ship.targetDistance / ship.relativeTargetVelocityMagnitude:n0}s";
                else TargetTime.text = $"T{-ship.targetDistance / ship.relativeTargetVelocityMagnitude:n0}s";
                if (ship.relativeTargetVelocityMagnitude > 0)
                    TargetTime.color = Color.green;
                else TargetTime.color = Color.red;
                TargetTime.GetComponent<RectTransform>().position = targetIndicator.position;
                targetIndicator.GetComponent<RectTransform>().sizeDelta = new Vector2(ship.apparentSize * 800f, ship.apparentSize * 800f);
                TargetTime.GetComponent<RectTransform>().anchoredPosition = new Vector2(targetIndicator.anchoredPosition.x + targetIndicator.sizeDelta.x / 2 + 40f, targetIndicator.anchoredPosition.y + targetIndicator.sizeDelta.x / 6);
            }
            else
            {
                targetIndicator.gameObject.SetActive(false);
                TargetTime.gameObject.SetActive(false);
            }
            crosshair.position = ship.crosshairPosition;
            foreach (Image image in images)
            {
                if (image.gameObject.GetComponent<RectTransform>().position.z < 0)
                    image.enabled = false;
                else image.enabled = true;
            }
            foreach (TextMeshProUGUI text in texts)
            {
                if (text.gameObject.GetComponent<RectTransform>().position.z < 0)
                    text.enabled = false;
                else text.enabled = true;
            }
        }
    }
}
