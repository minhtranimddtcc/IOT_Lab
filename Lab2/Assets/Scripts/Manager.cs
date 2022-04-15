using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
namespace ChuongGa{
    public class Manager : MonoBehaviour
    {
        // Start is called before the first frame update
            [SerializeField]
            private SwitchHandler status_LED;
            [SerializeField]
            private SwitchHandler status_PUMP;
            
            [SerializeField]
            private Text tempText;
            [SerializeField]
            private Text humText;

            [SerializeField]
            private CanvasGroup allow_sw;
            [SerializeField]
            private CanvasGroup allow_sw2;
                    [SerializeField]
            private CanvasGroup _canvasLayer1;
                    [SerializeField]
            private CanvasGroup _canvasLayer2;
            [SerializeField]
            private Window_Graph graph;
                    private Tween twenFade;
                    private List<int> lTemp;

        void Start()
        {
            lTemp=new List<int>();
        }

        // Update is called once per frame
        void Update()
        {
            
        }
        public void Update_Status(Status_Data _status_data)
            {
                tempText.text=_status_data.temperature.ToString()+" °C";
                humText.text=_status_data.humidity.ToString()+" %";
                lTemp.Add((int)_status_data.temperature); 
                if (lTemp.Count>10) {
                    lTemp.RemoveAt(0);
                }
                graph.DrawGraph(lTemp);
            }
        public void Update_Control(Control_Data _control_data)
            
            {
                if (_control_data.device=="LED")
                {
                    allow_sw.interactable = true;
                    if (_control_data.status == "ON")
                        status_LED.switchState = true;
                    else{

                        status_LED.switchState = false;
                    }
                }
                else{
                    allow_sw2.interactable = true;
                    if (_control_data.status == "ON")
                        status_PUMP.switchState = true;
                    else{

                        status_PUMP.switchState = false;
                    }
                }

            }
        public Control_Data Update_Control_Value(Control_Data _control)
            {
                if (_control.device=="LED"){
                    _control.status = (status_LED.switchState ? "ON" : "OFF");
                    allow_sw.interactable = false;
                    return _control;
                }
                else{
                    _control.status = (status_PUMP.switchState ? "ON" : "OFF");
                    allow_sw2.interactable = false;
                    return _control;
                }
            }
        

        IEnumerator _IESwitchLayer()
        {
            if (_canvasLayer1.interactable == true)
            {
                FadeOut(_canvasLayer1, 0.25f);
                yield return new WaitForSeconds(0.5f);
                FadeIn(_canvasLayer2, 0.25f);
            }
            else
            {
                FadeOut(_canvasLayer2, 0.25f);
                yield return new WaitForSeconds(0.5f);
                FadeIn(_canvasLayer1, 0.25f);
            }
        }


        public void SwitchLayer()
        {
            StartCoroutine(_IESwitchLayer());
        }


        public void Fade(CanvasGroup _canvas, float endValue, float duration, TweenCallback onFinish)
        {
            if (twenFade != null)
            {
                twenFade.Kill(false);
            }

            twenFade = _canvas.DOFade(endValue, duration);
            twenFade.onComplete += onFinish;
        }

        public void FadeIn(CanvasGroup _canvas, float duration)
        {
            Fade(_canvas, 1f, duration, () =>
            {
                _canvas.interactable = true;
                _canvas.blocksRaycasts = true;
            });
        }

        public void FadeOut(CanvasGroup _canvas, float duration)
        {
            Fade(_canvas, 0f, duration, () =>
            {
                _canvas.interactable = false;
                _canvas.blocksRaycasts = false;
            });
        }
    }
}