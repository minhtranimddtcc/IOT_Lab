using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
namespace ChuongGa
{
    public class Manager : MonoBehaviour
    {
        // Start is called before the first frame update
        [SerializeField]
        private SwitchHandler statusLED;
        [SerializeField]
        private SwitchHandler statusPUMP;

        [SerializeField]
        private Text temperatureText;
        [SerializeField]
        private Text humidityText;

        [SerializeField]
        private CanvasGroup allowSwitchLED;
        [SerializeField]
        private CanvasGroup allowSwitchPUMP;
        [SerializeField]
        private CanvasGroup canvasLayer;
        [SerializeField]
        private CanvasGroup canvasLayer2;
        [SerializeField]
        private Window_Graph graph;
        private Tween tweenFade;
        private List<int> listTemp;

        void Start()
        {
            listTemp = new List<int>();
        }

        // Update is called once per frame
        void Update()
        {

        }
        public void Update_Status(Status_Data statusData)
        {
            temperatureText.text = statusData.temperature.ToString() + " °C";
            humidityText.text = statusData.humidity.ToString() + " %";
            listTemp.Add((int)statusData.humidity);
            if (listTemp.Count > 10)
            {
                listTemp.RemoveAt(0);
            }
            graph.DrawGraph(listTemp);
        }
        public void UpdateControlLED(ControlDataLED controlDataLED)
        {
            allowSwitchLED.interactable = true;
            if (controlDataLED.status == "ON")
                statusLED.state = true;
            else
            {
                statusLED.state = false;
            }
        }
        public void UpdateControlPUMP(ControlDataPUMP controlDataPUMP)
        {
            allowSwitchPUMP.interactable = true;
            if (controlDataPUMP.status == "ON")
                statusPUMP.state = true;
            else
            {
                statusPUMP.state = false;
            }
        }

        public ControlDataLED UpdateControlValueLED(ControlDataLED controlData)
        {
            if (controlData.device == "LED")
            {
                controlData.status = (statusLED.state ? "ON" : "OFF");
                allowSwitchLED.interactable = false;
                return controlData;
            }
            else
            {
                controlData.status = (statusPUMP.state ? "ON" : "OFF");
                allowSwitchPUMP.interactable = false;
                return controlData;
            }
        }
        public ControlDataPUMP UpdateControlValuePUMP(ControlDataPUMP controlData)
        {
            if (controlData.device == "LED")
            {
                controlData.status = (statusLED.state ? "ON" : "OFF");
                allowSwitchLED.interactable = false;
                return controlData;
            }
            else
            {
                controlData.status = (statusPUMP.state ? "ON" : "OFF");
                allowSwitchPUMP.interactable = false;
                return controlData;
            }
        }
        IEnumerator _IESwitchLayer()
        {
            if (canvasLayer.interactable == true)
            {
                FadeOut(canvasLayer, 0.25f);
                yield return new WaitForSeconds(0.5f);
                FadeIn(canvasLayer2, 0.25f);
            }
            else
            {
                FadeOut(canvasLayer2, 0.25f);
                yield return new WaitForSeconds(0.5f);
                FadeIn(canvasLayer, 0.25f);
            }
        }


        public void SwitchLayer()
        {
            StartCoroutine(_IESwitchLayer());
        }


        public void Fade(CanvasGroup canvas, float endValue, float duration, TweenCallback onFinish)
        {
            if (tweenFade != null)
            {
                tweenFade.Kill(false);
            }

            tweenFade = canvas.DOFade(endValue, duration);
            tweenFade.onComplete += onFinish;
        }

        public void FadeIn(CanvasGroup canvas, float duration)
        {
            Fade(canvas, 1f, duration, () =>
            {
                canvas.interactable = true;
                canvas.blocksRaycasts = true;
            });
        }

        public void FadeOut(CanvasGroup canvas, float duration)
        {
            Fade(canvas, 0f, duration, () =>
            {
                canvas.interactable = false;
                canvas.blocksRaycasts = false;
            });
        }
    }
}