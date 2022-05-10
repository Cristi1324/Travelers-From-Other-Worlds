using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

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
    public TextMeshProUGUI altitudeText;
    public Image[] images;
    public TextMeshProUGUI[] texts;
    public bool showUI = false;
    float targetApparentSize;
    float targetRelativeVelocity;
    float targetDistance;
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
            altitudeText.gameObject.SetActive(true);
            altitudeText.text = $"{(ship.ReferenceBody.position-ship.transform.position).magnitude - ship.ReferenceBody.transform.localScale.x/2}m";
            crosshair.gameObject.SetActive(true);
            circleCrosshair.gameObject.SetActive(true);
            if (ship.showMotionVectors)
            {
                speedText.gameObject.SetActive(true);
                prograde.gameObject.SetActive(true);
                retrograde.gameObject.SetActive(true);
                retrogradeText.gameObject.SetActive(true);
                speedText.text = $"{ship.getRelativeVelocity().magnitude:n0}m/s";
                prograde.position = ship.progradePosition;
                speedText.GetComponent<RectTransform>().position = prograde.position + new Vector3(prograde.rect.size.x, 0, 0);
                retrograde.position = new Vector3(ship.progradePosition.x, ship.progradePosition.y, -ship.progradePosition.z);
                retrogradeText.text = $"{-ship.getRelativeVelocity().magnitude:n0}m/s";
                retrogradeText.GetComponent<RectTransform>().position = retrograde.position + new Vector3(retrograde.rect.size.x,0,0);
            }
            else
            {
                speedText.gameObject.SetActive(false);
                prograde.gameObject.SetActive(false);
                retrograde.gameObject.SetActive(false);
                retrogradeText.gameObject.SetActive(false);
            }
            if (ship.hasTarget)
            {
                targetApparentSize = ship.getTargetApparentSize();
                if(targetApparentSize<1)
                {
                    targetRelativeVelocity = ship.getTargetRelativeVelocity();
                    targetDistance = ship.getTargetDistance();
                    var color = targetIndicator.GetComponent<Image>().color;
                    float targetTime = -targetDistance / targetRelativeVelocity;
                    targetTime = Mathf.Abs(targetTime);
                    TimeSpan timeSpan = TimeSpan.FromSeconds((double)targetTime);
                    color.a = Mathf.Pow((1 - targetApparentSize), 1);
                    targetIndicator.GetComponent<Image>().color = color;
                    targetIndicator.gameObject.SetActive(true);
                    TargetTime.gameObject.SetActive(true);
                    targetIndicator.position = ship.targetPosition;
                    if(targetTime>0)
                        TargetTime.text = $"T+";
                    else TargetTime.text = $"T-";
                    if(timeSpan.Days!=0)
                        TargetTime.text += $"{timeSpan.Days}d";
                    if(timeSpan.Hours!=0)
                        TargetTime.text += $"{timeSpan.Hours}h";
                    if(timeSpan.Minutes!=0)
                        TargetTime.text += $"{timeSpan.Minutes}m";
                    if(timeSpan.Seconds!=0)
                        TargetTime.text += $"{timeSpan.Seconds}s";
                    if (targetRelativeVelocity > 0)
                        TargetTime.color = Color.green;
                    else TargetTime.color = Color.red;
                    TargetTime.GetComponent<RectTransform>().position = targetIndicator.position;
                    targetIndicator.GetComponent<RectTransform>().sizeDelta = new Vector2(Mathf.Max(targetApparentSize * 800f,50f), Mathf.Max(targetApparentSize * 800f,50f));
                    TargetTime.GetComponent<RectTransform>().anchoredPosition = new Vector2(targetIndicator.anchoredPosition.x + targetIndicator.sizeDelta.x / 2 + 40f, targetIndicator.anchoredPosition.y + targetIndicator.sizeDelta.x / 6);
                }
                else{
                    targetIndicator.gameObject.SetActive(false);
                    TargetTime.gameObject.SetActive(false);
                }
            }else{
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
