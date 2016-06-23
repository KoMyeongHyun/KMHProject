using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BeShot : MonoBehaviour
{
    private RawImage imgBeShot;

    private float alpha;
    //private float alphaVariation;

    [SerializeField]
    private float maxAlpha;
    [SerializeField]
    private float fadeInTime;
    [SerializeField]
    private float fadeOutTime;

	// Use this for initialization
	void Start ()
    {
        imgBeShot = this.GetComponent<RawImage>();
        alpha = 0.0f;

        imgBeShot.color = new Color(1.0f, 1.0f, 1.0f, alpha);

        StartCoroutine(FadeImage());
    }
	
    //맞을 때마다 해당 프리팹 생성하고 투명도 0되면 destory
    IEnumerator FadeImage()
    {
        while(alpha < maxAlpha)
        {
            alpha += maxAlpha * Time.deltaTime / fadeInTime;
            imgBeShot.color = new Color(1.0f, 1.0f, 1.0f, alpha);
            yield return null;
        }

        alpha = maxAlpha;
        imgBeShot.color = new Color(1.0f, 1.0f, 1.0f, alpha);

        while(alpha > 0.0f)
        {
            alpha -= maxAlpha * Time.deltaTime / fadeOutTime;
            imgBeShot.color = new Color(1.0f, 1.0f, 1.0f, alpha);
            yield return null;
        }

        Destroy(gameObject);
    }
}
