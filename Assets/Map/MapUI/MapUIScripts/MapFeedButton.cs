﻿using UnityEngine;
using UnityEngine.EventSystems;

public class MapFeedButton : MonoBehaviour, IPointerClickHandler {
	public void OnPointerClick(PointerEventData eventData) {
		Debug.Log("Click Feed Button");
	}
}
