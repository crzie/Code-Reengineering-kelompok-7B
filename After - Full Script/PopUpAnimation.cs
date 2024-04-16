using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PopUpAnimation : MonoBehaviour
{
    [SerializeField] private AnimationCurve opacityCurve;
    [SerializeField] private AnimationCurve sizeCurve;
    [SerializeField] private AnimationCurve heightCurve;

    private TextMeshProUGUI tmp;
    private Vector3 oriScale;
    private Vector3 oriPosition;
    private float time = 0;

    void Start()
    {
        tmp = transform.GetComponentInChildren<TextMeshProUGUI>();
        oriScale = transform.localScale;
        oriPosition = transform.position;

    }

    // Update is called once per frame
    void Update()
    {
        tmp.color = new Color(tmp.color.r, tmp.color.g, tmp.color.b, opacityCurve.Evaluate(time));
        transform.localScale = oriScale * sizeCurve.Evaluate(time);
        transform.position = oriPosition + new Vector3(0, heightCurve.Evaluate(time), 0);

        transform.forward = Camera.main.transform.forward;

        time += Time.deltaTime;
    }
}
