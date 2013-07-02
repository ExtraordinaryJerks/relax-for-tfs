using System;
using Relax.Model.Enums;
using Relax.Model.Interfaces;

namespace Relax.Model
{
    public class RelaxNotificationMessagePayload
    {
        public NotificationType NotificationType { get; set; }
        public String Title { get; set; }
        public String Message { get; set; }
    }
}