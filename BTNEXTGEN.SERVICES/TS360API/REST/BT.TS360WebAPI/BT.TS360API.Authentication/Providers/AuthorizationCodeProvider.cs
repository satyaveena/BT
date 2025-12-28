using BT.TS360API.Authentication.DataAccess;
using Microsoft.Owin.Security.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace BT.TS360API.Authentication.Providers
{
    public class AuthorizationCodeProvider : AuthenticationTokenProvider
    {
        public override void Create(AuthenticationTokenCreateContext context)
        {
            context.Ticket.Properties.IssuedUtc = DateTime.UtcNow;
            context.SetToken(context.SerializeTicket());

            // save auth code to MongoDB
            AuthDAOManager.AddAuthCodeInfo(context);
        }

        public override void Receive(AuthenticationTokenReceiveContext context)
        {
            context.DeserializeTicket(context.Token);
        }

        //public override async Task CreateAsync(AuthenticationTokenCreateContext context)
        //{
        //    context.SetToken(context.SerializeTicket());
        //}

        //public override async Task ReceiveAsync(AuthenticationTokenReceiveContext context)
        //{
        //    context.DeserializeTicket(context.Token);
        //}
    }
}