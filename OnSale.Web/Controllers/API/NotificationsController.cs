using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OnSale.Common.Models;
using OnSale.Common.Services;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace OnSale.Web.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]

    public class NotificationsController : ControllerBase
    {
        [HttpGet]
        [Route("SendNotificationiOS")]
        public async Task<IActionResult> SendNotificationiOS(string tag1, string tag2)
        {
            string[] tags = new string[2];
            tags[0] = tag1;
            tags[1] = tag2;
            NotificationPushiOS notificationPushiOS = new NotificationPushiOS
            {
                Aps = new Aps
                {
                    Alert = "Título de la notificación\nOtro renglon de la notificación",
                    Parameters = new Parameters
                    {
                        Title = "Título de la notificación",
                        Body = $"El cuerpo de la notificación {tag1}",
                        Tag = "Encuesta",
                        Id = "04F238E2-50B2-4C4D-96EE-9B11F39A91B1",
                    }
                }
            };

            NotificationsService sendNotifications = new NotificationsService();
            await sendNotifications.SendNotificationiOSAsync(notificationPushiOS, tags);
            return Ok();
        }

        [HttpGet]
        [Route("DeleteDevicesNotifications")]
        public async Task<IActionResult> DeleteDevicesNotifications()
        {
            NotificationsService sendNotifications = new NotificationsService();
            await sendNotifications.DeleteAllRegistrationAsync();
            return Ok();
        }

        [HttpGet]
        [Route("ChangeTags")]
        public async Task<IActionResult> Changetags(string handle)
        {
            var idTerritorio = 4;
            var idProceso = Guid.NewGuid();
            DeviceRegistration deviceRegistration = new DeviceRegistration
            {
                PNSHandle = handle
            };

            NotificationsService sendNotifications = new NotificationsService();          
            await sendNotifications.UpdateRegistrationiOSAsync(idTerritorio, idProceso, deviceRegistration);
            return Ok();
        }


    }
}
