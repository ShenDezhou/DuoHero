using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScreenUI : MonoBehaviour
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
		uiRect = transform.FindChild ("ScoreImg1").GetComponent<RectTransform> ();
		uiRect.anchoredPosition = new Vector2(uiRect.anchoredPosition.x, uiRect.anchoredPosition.y-blackBar);
		uiRect = transform.FindChild ("ScoreImg2").GetComponent<RectTransform> ();
		uiRect.anchoredPosition = new Vector2(uiRect.anchoredPosition.x, uiRect.anchoredPosition.y-blackBar);
		uiRect = transform.FindChild ("ScoreImg3").GetComponent<RectTransform> ();
		uiRect.anchoredPosition = new Vector2(uiRect.anchoredPosition.x, uiRect.anchoredPosition.y-blackBar);
		uiRect = transform.FindChild ("ScoreImg4").GetComponent<RectTransform> ();
		uiRect.anchoredPosition = new Vector2(uiRect.anchoredPosition.x, uiRect.anchoredPosition.y-blackBar);
		uiRect = transform.FindChild ("ScoreImg5").GetComponent<RectTransform> ();
		uiRect.anchoredPosition = new Vector2(uiRect.anchoredPosition.x, uiRect.anchoredPosition.y-blackBar);

		uiRect = transform.FindChild ("GameRestart").GetComponent<RectTransform> ();
		uiRect.anchoredPosition = new Vector2(uiRect.anchoredPosition.x, uiRect.anchoredPosition.y+blackBar);
		uiRect = transform.FindChild ("GameReturn").GetComponent<RectTransform> ();
		uiRect.anchoredPosition = new Vector2(uiRect.anchoredPosition.x, uiRect.anchoredPosition.y+blackBar);
	}
}