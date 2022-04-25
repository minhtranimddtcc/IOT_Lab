using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;
using M2MqttUnity;
using Newtonsoft.Json.Linq;
using System.Linq;
using Newtonsoft.Json;

namespace ChuongGa
{
    public class Status_Data
    {
        public float temperature { get; set; }
        public float humidity { get; set; }
    }


    public class ControlDataLED
    {
        public string device { get; set; }
        public string status { get; set; }

    }
    public class ControlDataPUMP
    {
        public string device { get; set; }
        public string status { get; set; }

    }

    public class ChuongGaMqtt : M2MqttUnityClient
    {
        public List<string> topics = new List<string>();


        public string msg_received_from_topic_status = "";
        public string msg_received_from_topic_control = "";


        public Manager manager;
        private List<string> eventMessages = new List<string>();
        [SerializeField]
        public Status_Data statusData;
        [SerializeField]

        public InputField brokeruri;
        public InputField username;
        public InputField password;

        public void PublishLED()
        {

            ControlDataLED controlData = new ControlDataLED();
            controlData.device = "LED";
            controlData = GetComponent<Manager>().UpdateControlValueLED(controlData);
            string msg_config = JsonConvert.SerializeObject(controlData);
            client.Publish(topics[1], System.Text.Encoding.UTF8.GetBytes(msg_config), MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, true);
            Debug.Log("publish LED");
        }

        public void PublishPUMP()
        {
            ControlDataPUMP controlData = new ControlDataPUMP();
            controlData.device = "PUMP";
            controlData = GetComponent<Manager>().UpdateControlValuePUMP(controlData);
            string msg_config = JsonConvert.SerializeObject(controlData);
            client.Publish(topics[2], System.Text.Encoding.UTF8.GetBytes(msg_config), MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, true);
            Debug.Log("publish PUMP");
        }
        public void SetEncrypted(bool isEncrypted)
        {
            this.isEncrypted = isEncrypted;
        }


        protected override void OnConnecting()
        {
            base.OnConnecting();

        }

        protected override void OnConnected()
        {
            base.OnConnected();

            SubscribeTopics();
        }

        protected override void SubscribeTopics()
        {

            foreach (string topic in topics)
            {
                if (topic != "")
                {
                    client.Subscribe(new string[] { topic }, new byte[] { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE });

                }
            }
        }

        protected override void UnsubscribeTopics()
        {
            foreach (string topic in topics)
            {
                if (topic != "")
                {
                    client.Unsubscribe(new string[] { topic });
                }
            }

        }

        protected override void OnConnectionFailed(string errorMessage)
        {
            Debug.Log("CONNECTION FAILED! " + errorMessage);
        }

        protected override void OnDisconnected()
        {
            Debug.Log("Disconnected.");
        }

        protected override void OnConnectionLost()
        {
            Debug.Log("CONNECTION LOST!");
        }



        protected override void Start()
        {

            base.Start();
        }

        protected override void DecodeMessage(string topic, byte[] message)
        {
            string msg = System.Text.Encoding.UTF8.GetString(message);
            Debug.Log("Received: " + msg);
            if (topic == topics[0])
                ProcessMessageStatus(msg);

            else if (topic == topics[1])
                ProcessMessageControlLED(msg);
            else
                ProcessMessageControlPUMP(msg);
        }

        private void ProcessMessageStatus(string msg)
        {
            statusData = JsonConvert.DeserializeObject<Status_Data>(msg);
            msg_received_from_topic_status = msg;
            GetComponent<Manager>().Update_Status(statusData);

        }

        private void ProcessMessageControlLED(string msg)
        {
            ControlDataLED controlData = JsonConvert.DeserializeObject<ControlDataLED>(msg);
            msg_received_from_topic_control = msg;
            GetComponent<Manager>().UpdateControlLED(controlData);
        }
        private void ProcessMessageControlPUMP(string msg)
        {
            ControlDataPUMP controlData = JsonConvert.DeserializeObject<ControlDataPUMP>(msg);
            msg_received_from_topic_control = msg;
            GetComponent<Manager>().UpdateControlPUMP(controlData);
        }

        public override void Connect()
        {

            mqttUserName = username.text;
            mqttPassword = password.text;
            brokerAddress = brokeruri.text;
            // mqttUserName = "bkiot";
            // mqttPassword = "12345678";
            // brokerAddress = "mqttserver.tk";
            base.Connect();
            manager.SwitchLayer();
        }

        private void OnDestroy()
        {
            Disconnect();
        }

    }
}