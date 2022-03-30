﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BLESpiderApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ButtonsView : ContentPage
    {
        private BtConnector bt;

        private bool connected = false;

        private byte SPEED = 0;
        private sbyte DIR = 0;
        private byte FORWARD = 1;

        public ButtonsView()
        {
            InitializeComponent();

            ConnectionBtn.Clicked += ConnectBtn_Clicked;

            SpeedUpBtn.Clicked += SpeedUpBtn_Clicked;
            SpeedDwBtn.Clicked += SpeedDwBtn_Clicked;
            StopBtn.Clicked += StopBtn_Clicked;
            DirBtn.Clicked += DirBtn_Clicked;
            SwitchViewBtn.Clicked += SwitchViewBtn_Clicked;
            LeftBtn.Clicked += LeftBtn_Clicked;
            RightBtn.Clicked += RightBtn_Clicked;

            ParamsBtn.IsEnabled = false;
        }

        private void SwitchViewBtn_Clicked(object sender, EventArgs e)
        {
            this.Navigation.PushAsync(new ManualView());
            this.Navigation.RemovePage(this);

            SPEED = 0;
            DIR = 0;

            ParamsBtn.Text = $"S: {SPEED} DIR: {DIR} {(FORWARD == 1 ? "FORWARD" : "BACKWARD")}";

            SendCommands();

            if (connected)
            {
                if (bt != null)
                    Disconnect();
            }
        }

        private void DirBtn_Clicked(object sender, EventArgs e)
        {
            SPEED = 0;
            DIR = 0;

            FORWARD = FORWARD == (byte)1 ? (byte)0 : (byte)1;

            ParamsBtn.Text = $"S: {SPEED} DIR: {DIR} {(FORWARD == 1 ? "FORWARD" : "BACKWARD")}";

            SendCommands();
        }

        private void RightBtn_Clicked(object sender, EventArgs e)
        {
            if (DIR <= 70)
                DIR += 10;

            ParamsBtn.Text = $"S: {SPEED} DIR: {DIR} {(FORWARD == 1 ? "FORWARD" : "BACKWARD")}";

            SendCommands();
        }

        private void LeftBtn_Clicked(object sender, EventArgs e)
        {
            if (DIR >= -70)
                DIR -= 10;

            ParamsBtn.Text = $"S: {SPEED} DIR: {DIR} {(FORWARD == 1 ? "FORWARD" : "BACKWARD")}";

            SendCommands();
        }

        private void StopBtn_Clicked(object sender, EventArgs e)
        {
            SPEED = 0;
            DIR = 0;

            ParamsBtn.Text = $"S: {SPEED} DIR: {DIR} {(FORWARD == 1 ? "FORWARD" : "BACKWARD")}";

            SendCommands();
        }

        private void SpeedDwBtn_Clicked(object sender, EventArgs e)
        {
            if (SPEED >= 10)
            {
                if (SPEED <= 40)
                    SPEED = 0;
                else
                    SPEED -= 10;
            }

            ParamsBtn.Text = $"S: {SPEED} DIR: {DIR} {(FORWARD == 1 ? "FORWARD" : "BACKWARD")}";

            SendCommands();
        }

        private void SpeedUpBtn_Clicked(object sender, EventArgs e)
        {
            if (SPEED < 100)
            {
                if (SPEED < 40)
                    SPEED = 40;
                else
                    SPEED += 10;
            }

            ParamsBtn.Text = $"S: {SPEED} DIR: {DIR} {(FORWARD == 1 ? "FORWARD" : "BACKWARD")}";

            SendCommands();
        }

        private void OnConnection()
        {
            ConnectionBtn.Text = "Disconnect!";
            ConnectionBtn.TextColor = Color.Green;
            connected = true;
        }

        private void Disconnect()
        {
            DIR = 0;
            SPEED = 0;
            FORWARD = 0;

            bt.Disconnect();
            ConnectionBtn.Text = "Connect!";
            ConnectionBtn.TextColor = Color.Red;
            connected = false;
        }

        private void ConnectBtn_Clicked(object sender, EventArgs e)
        {
            if (connected)
            {
                if (bt != null)
                    Disconnect();
            }
            else
            {
                if (bt == null)
                    bt = new BtConnector();

                bt.Connect(OnConnection);
            }
        }

        private void SendCommands()
        {
            if (bt != null)
                bt.Send(new byte[] { 0xAA, SPEED, (byte)DIR, FORWARD }, 4);
        }
    }
}