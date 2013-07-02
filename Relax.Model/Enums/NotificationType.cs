namespace Relax.Model.Enums
{
    /// <summary>
    /// 
    /// </summary>
    //If these types change they also need to be changed in the client app and the relax service, so only change.s
    public enum NotificationType
    {
        //Should be used when sending secure information down to the client
        Secure = 1,
        //Should be used when sending down a notification that should be displayed in the notification bar
        Notify = 2,
        //Should be used when sending down a message the should be display as an Alert/DialogS
        Alert = 3,
        //Should be used when the message is to large to be sent down to the client via the 1024 KB payload, this will let the client know that it needs to fetch the message from the server 
        Fetch = 4
    }
}