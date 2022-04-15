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


    public class Control_Data
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
        public Status_Data _status_data;
        [SerializeField]
        
        public Control_Data _control_data;
        
        public InputField brokeruri;
        public InputField username;
        public InputField password;

        private void PublishControl(string device,int tno)
        {
            _control_data = new Control_Data();
            _control_data.device=device;
            _control_data = GetComponent<Manager>().Update_Control_Value(_control_data);
            string msg_config = JsonConvert.SerializeObject(_control_data);
            client.Publish(topics[tno], System.Text.Encoding.UTF8.GetBytes(msg_config), MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, true);
            Debug.Log("publish "+device);

        }

        public void PublishLED(){
            PublishControl("LED",1);
        }

        public void PublishPUMP(){
            PublishControl("PUMP",2);
        }
        public void SetEncrypted(bool isEncrypted)
        {
            this.isEncrypted = isEncrypted;
        }


        protected override void OnConnecting()
        {
            base.OnConnecting();
            //SetUiMessage("Connecting to broker on " + brokerAddress + ":" + brokerPort.ToString() + "...\n");
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

            else
                ProcessMessageControl(msg);
        }

        private void ProcessMessageStatus(string msg)
        {
             _status_data = JsonConvert.DeserializeObject<Status_Data>(msg);
            msg_received_from_topic_status = msg;
            GetComponent<Manager>().Update_Status(_status_data);

        }

        private void ProcessMessageControl(string msg)
        {
            _control_data = JsonConvert.DeserializeObject<Control_Data>(msg);
            msg_received_from_topic_control = msg;
            GetComponent<Manager>().Update_Control(_control_data);
        }

        public override void Connect()
        {

            mqttUserName=username.text;
            mqttPassword=password.text;
            brokerAddress=brokeruri.text;
            mqttUserName="bkiot";
            mqttPassword="12345678";
            brokerAddress="mqttserver.tk";

            base.Connect();
            manager.SwitchLayer();

        }

        private void OnDestroy()
        {
            Disconnect();
        }

    }
}