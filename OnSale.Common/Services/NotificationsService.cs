using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Azure.NotificationHubs;
using Microsoft.Azure.NotificationHubs.Messaging;
using Newtonsoft.Json;
using OnSale.Common.Business;
using OnSale.Common.Models;

namespace OnSale.Common.Services
{
    public class NotificationsService
    {
        NotificationHubClient hub;

        public NotificationsService()
        {
            hub = NotificationHubClient.CreateClientFromConnectionString(
                DispatcherMovConstants.FullAccessConnectionString,
                DispatcherMovConstants.NotificationHubName);
        }

        #region [ Register Device Notification]

        public async Task<string> CreateRegistrationId()
        {
            //TODO Eliminar
            var allRegistrations = hub.GetAllRegistrationsAsync(0).Result;
            string newRegistrationId = await hub.CreateRegistrationIdAsync();
            return newRegistrationId;
        }

        public async Task UpdateRegistrationAsync(int idTerritory, Guid idProcess, DeviceRegistration deviceUpdate)
        {
            //TODO Eliminar
            var allRegistrations = hub.GetAllRegistrationsAsync(0).Result;
            var registerDevice = await hub.GetRegistrationAsync<RegistrationDescription>(deviceUpdate.RegistrationId);

            registerDevice.Tags.Add($"all");
            registerDevice.Tags.Add($"territory:{idTerritory}");
            registerDevice.Tags.Add($"process:{idProcess}");

            try
            {
                await hub.UpdateRegistrationAsync(registerDevice);
                //TODO Eliminar
                allRegistrations = hub.GetAllRegistrationsAsync(0).Result;
            }
            catch (MessagingException ex)
            {
                var message = ex.Message;
                return;
            }
            return;
        }

        public async Task UpdateRegistrationiOSAsync(int idTerritory, Guid idProcess, DeviceRegistration deviceUpdate)
        {
            //TODO Eliminar
            var allRegistrations = hub.GetAllRegistrationsAsync(0).Result;

            //RegistrationDescription registration = new AppleRegistrationDescription(deviceUpdate.PNSHandle);
            var temp = await hub.GetRegistrationsByChannelAsync(deviceUpdate.PNSHandle, 0);

            foreach (var item in temp)
            {
                deviceUpdate.RegistrationId = item.RegistrationId;
            }

            var registerDevice = await hub.GetRegistrationAsync<RegistrationDescription>(deviceUpdate.RegistrationId);
            //registration.RegistrationId = await CreateRegistrationId();
            registerDevice.Tags = new HashSet<string>();
            registerDevice.Tags.Add($"all");
            registerDevice.Tags.Add($"territory:{idTerritory}");
            registerDevice.Tags.Add($"process:{idProcess}");

            try
            {
                await hub.UpdateRegistrationAsync(registerDevice);
                //TODO Eliminar
                allRegistrations = hub.GetAllRegistrationsAsync(0).Result;
            }
            catch (MessagingException ex)
            {
                var message = ex.Message;
                return;
            }
            return;
        }
        #endregion [ Register Device Notification ]

        #region [ Send Notifications ]

        public async Task<bool> SendNotificationiOSAsync(NotificationPushiOS notificationPushiOS, string[] tags)
        {
            //TODO Eliminar
            var allRegistrations = hub.GetAllRegistrationsAsync(0).Result;
            
            NotificationOutcome outcome = null;

            // iOS
            string alert = JsonConvert.SerializeObject(notificationPushiOS);
            outcome = await hub.SendAppleNativeNotificationAsync(alert, tags);

            if (outcome != null)
            {
                if (!((outcome.State == NotificationOutcomeState.Abandoned) ||
                    (outcome.State == NotificationOutcomeState.Unknown)))
                {
                    return true;
                }
            }

            return false;
        }

        #endregion [ Send Notifications ]

        //TODO Eliminar éstos métoso que sirven para eliminar un dispositivo del registro de notificaciones
        #region [ Delete Device Notification ]
        public async Task DeleteRegistrationAsync(string registrationId)
        {
            await hub.DeleteRegistrationAsync(registrationId);
        }

        public async Task DeleteAllRegistrationAsync()
        {
            var allRegistrations = hub.GetAllRegistrationsAsync(0).Result;
            foreach (var item in allRegistrations)
            {
                var id = item.RegistrationId;
                await hub.DeleteRegistrationAsync(id);
            }
        }
        #endregion [ Delete Device Notification ]
    }
}
