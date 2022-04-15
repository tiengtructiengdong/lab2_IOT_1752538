using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Runtime.InteropServices;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;

namespace M2MqttUnity {

    public class Login : M2MqttUnityClient
    {
        public bool autoTest = true;
        public InputField uri;
        public InputField username;
        public InputField password;
        public Text errText;
        public Text humidity;
        public Text temperature;

        public Button loginButton;

        

        // Start is called before the first frame update
        protected override void Start() {
            base.Start();
        }

        // Update is called once per frame
        private void UpdateLoginUI() {
            this.brokerAddress = uri.text.ToString();
            this.mqttUserName = username.text.ToString();
            this.mqttPassword = password.text.ToString();
        }

        protected override void Update()
        {
            if (uri && username && password)   
                UpdateLoginUI();
            else
                UpdateMain();
        }

        private void UpdateMain() {
            base.Update();
        }
        
        public void ToggleLED(bool val) {
            client.Publish("v1/devices/me/attributes", System.Text.Encoding.UTF8.GetBytes("{\"isFanOn\":true}"), MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, false);
        }
        public void TogglePump(bool val) {
            client.Publish("v1/devices/me/attributes", System.Text.Encoding.UTF8.GetBytes("{\"isLEDOn\":true}"), MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, false);
        }

        protected override void OnConnectionFailed(string errorMessage) {
            errText.text = errorMessage;
        }

        public void SetUiMessage(string msg)
        {
            Debug.Log(msg);
        }
        protected override void SubscribeTopics()
        {
            client.Subscribe(new string[] { "v1/devices/me/rpc/request/+" }, new byte[] { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE });
            Debug.Log("Sub");
        }

        protected override void UnsubscribeTopics()
        {
            client.Unsubscribe(new string[] { "v1/devices/me/rpc/request/+" });
        }
        protected override void DecodeMessage(string topic, byte[] message)
        {
            base.DecodeMessage(topic, message);
            Debug.Log("Received: ");
            string msg = System.Text.Encoding.UTF8.GetString(message);
            Debug.Log("Received message: " + msg.ToString());
            
            temperature.text = "454C";
            humidity.text = "454C";
        }

        protected override void OnConnecting()
        {
            base.OnConnecting();
            SetUiMessage("Connecting to broker on " + brokerAddress + ":" + brokerPort.ToString() + "...\n");
        }

        protected override void OnConnected()
        {
            base.OnConnected();
            errText.text = "";
            SetUiMessage("Connected to broker on " + brokerAddress + "\n");
            SceneManager.LoadScene(1);
        }
    }
}

// hBqfKlgjc1xV8ek2EeUI