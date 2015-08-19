using GalaSoft.MvvmLight.Ioc;
using System;
using System.Collections.Generic;
using UTHPortal.Models;

namespace UTHPortal.Common
{
    public class PushService : IPushService
    {
        IStorageService storageService;

        public PushService()
        {
            storageService = SimpleIoc.Default.GetInstance<IStorageService>();
        }

        public void RegisterUrl(string url)
        {
            var activePushEvents = (List<PushEvent>)storageService.GetSettingsEntry("ActivePushEvents");
            var regEventName = getEventName(url);

            if ( !activePushEvents.Exists( activeEvent => activeEvent.Name == regEventName)) {
                // TODO: Make necessary connections to server in order
                //       ensure that the events is registered to client

                activePushEvents.Add(new PushEvent(url, regEventName));
                storageService.SetSettingsEntry("ActivePushEvents", activePushEvents);
            }
        }

        public void UnregisterUrl(string url)
        {
            var activePushEvents = (List<PushEvent>)storageService.GetSettingsEntry("ActivePushEvents");
            var regEventName = getEventName(url);

            if ( activePushEvents.Exists( activeEvent => activeEvent.Name == regEventName)) {
                // TODO: Make necessary connections to server in order
                //       ensure that the events is registered to client

                activePushEvents.Remove(new PushEvent(url, regEventName));
                storageService.SetSettingsEntry("ActivePushEvents", activePushEvents);
            }
        }

        private string getEventName(string url)
        {
            return url.Substring(RestAPI.baseUrl.Length).Replace('/', '.');
        }
    }
}
