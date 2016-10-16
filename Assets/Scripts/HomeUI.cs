using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HomeUI : MonoBehaviour
{
	private float pixelPerUnit, pixelWidth, pixelHeight;
	private int blackBar;
	private RectTransform uiRect;

    void Awake()
    {
		pixelPerUnit = GetComponent<CanvasScaler>().referencePixelsPerUnit;
		pixelWidth = GetComponent<CanvasScaler> ().referenceResolution.x;
		pixelHeight = GetComponent<CanvasScaler> ().referenceResolution.y;
    }

	void Start(){
		Camera.main.orthographicSize = (pixelWidth / Screen.width) * Screen.height * (1.0f / pixelPerUnit) * 0.5f;
		blackBar = Mathf.FloorToInt(((pixelWidth / Screen.width) * Screen.height - pixelHeight) * 0.5f);
		uiRect = transform.FindChild ("StartButton").GetComponent<RectTransform> ();
		uiRect.anchoredPosition = new Vector2(uiRect.anchoredPosition.x, uiRect.anchoredPosition.y+blackBar);
		uiRect = transform.FindChild ("TutsButton").GetComponent<RectTransform> ();
		uiRect.anchoredPosition = new Vector2(uiRect.anchoredPosition.x, uiRect.anchoredPosition.y+blackBar);
		uiRect = transform.FindChild ("ReturnButton").GetComponent<RectTransform> ();
		uiRect.anchoredPosition = new Vector2(uiRect.anchoredPosition.x, uiRect.anchoredPosition.y+blackBar);
	}

}