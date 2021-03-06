﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AutoMova.UI
{
    public class TrayIcon
    {
        public event EventHandler<EventArgs> DoubleClick;
        public event EventHandler<EventArgs> ExitClick;
        public event EventHandler<EventArgs> SettingsClick;
        public event EventHandler<EventArgs> TogglePowerClick;

        public NotifyIcon trayIcon;
        private ContextMenu trayMenu;
        private MenuItem togglePowerItem;
        private bool wasShownBeforeTooltip;

        public TrayIcon (bool? visible = true)
	    {
            trayMenu = new ContextMenu();
            togglePowerItem = new MenuItem("", OnPowerClick);
            trayMenu.MenuItems.Add(togglePowerItem);
            trayMenu.MenuItems.Add("Settings", OnSettingsClick);
            trayMenu.MenuItems.Add("-");
            trayMenu.MenuItems.Add("Exit", OnExitClick);

            trayIcon = new NotifyIcon();
            trayIcon.Text = "AutoMova";
            if (Environment.OSVersion.Version < new Version(6,2))
            {
                trayIcon.Icon = Properties.Resources.icon7;
            }
            else
            {
                trayIcon.Icon = Properties.Resources.icon10;
            }            
            trayIcon.BalloonTipClosed += trayIcon_BalloonTipClosed;

            trayIcon.MouseDoubleClick += trayIcon_Click;
            trayIcon.ContextMenu = trayMenu;
            trayIcon.Visible = visible == true;
	    }


        public void SetRunning(bool isRunning)
        {
            togglePowerItem.Text = isRunning ? "Turn off" : "Turn on";
        }
        public void Show()
        {
            trayIcon.Visible = true;
        }
        public void Hide()
        {
            trayIcon.Visible = false;
        }

        public void ShowErrorTooltip(string p, ToolTipIcon icon)
        {
            wasShownBeforeTooltip = trayIcon.Visible;
            Show();
            trayIcon.ShowBalloonTip(2000, "AutoMova error", p, icon);
        }

        public void ShowInfoTooltip(string p, ToolTipIcon icon)
        {
            wasShownBeforeTooltip = trayIcon.Visible;
            Show();
            trayIcon.ShowBalloonTip(2000, "AutoMova", p, icon);
        }



        private void OnExitClick(object sender, EventArgs e)
        {
            if (ExitClick != null)
            {
                ExitClick(this, null);
            }
        }

        private void OnSettingsClick(object sender, EventArgs e)
        {
            if (SettingsClick != null)
            {
                SettingsClick(this, null);
            }
        }

        private void OnPowerClick(object sender, EventArgs e)
        {
            if (TogglePowerClick != null)
            {
                TogglePowerClick(this, null);
            }
        }

        void trayIcon_Click(object sender, MouseEventArgs e)
        {
            if (DoubleClick != null)
            {
                DoubleClick(this, null);
            }
        }

        void trayIcon_BalloonTipClosed(object sender, EventArgs e)
        {
            trayIcon.Visible = wasShownBeforeTooltip;
        }
    }
}
