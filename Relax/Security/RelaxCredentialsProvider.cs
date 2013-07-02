using System;
using System.Net;
using Microsoft.TeamFoundation.Client;

namespace Relax.Security
{
    public class RelaxCredentialsProvider : ICredentialsProvider
    {
        public ICredentials GetCredentials(Uri uri, ICredentials credentials)
        {
            //NetworkCredential cred = credentials.GetCredential(uri, "Basic");
            return new NetworkCredential("", "", ""); //NetworkCredential(cred.UserName, cred.Password, cred.Domain);
        }

        public void NotifyCredentialsAuthenticated(Uri uri)
        {
            throw new ApplicationException("Unable to authenticate");
        }
    }
}