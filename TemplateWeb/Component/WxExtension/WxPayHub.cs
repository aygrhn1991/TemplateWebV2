using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TemplateWeb.Component.WxExtension
{
    public class WxPayHub:Hub
    {
        public void Send(string name, string message)
        {
            // Call the broadcastMessage method to update clients.
        }
    }
}