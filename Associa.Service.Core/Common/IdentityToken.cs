using Associa.Service.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Linq;

namespace Associa.Service.Core.Common
{
   public class IdentityToken: IIdentityToken
   {
       private readonly string _idToken;
       private readonly JwtSecurityToken _tokenS;
       private Guid _userId;
       private string _role;
       public string AuthorizationHeader { get; }

       public IdentityToken(AuthenticationHeaderValue authorizationHeader)
       {
           if (authorizationHeader == null)
               throw new Exception("Error: Please provide valid authorization header.");

           _idToken = authorizationHeader.Parameter;
           AuthorizationHeader = authorizationHeader.Parameter;
           try
           {
               var handler = new JwtSecurityTokenHandler();
               _tokenS = handler.ReadToken(_idToken) as JwtSecurityToken;
           }
           catch (Exception ex)
           {
               throw new Exception("Error processing person id from Authorization header.", ex);
           }
       }

       public Guid UserId
       {
           get
           {
               if (_idToken != null && _tokenS != null)
               {
                   var accessId = _tokenS.Claims.First(claim => claim.Type == "custom:userid").Value;
                   _userId = new Guid(accessId);
               }
               return _userId;
           }
       }
       public string Role
       {
           get
           {
               if (_idToken != null && _tokenS != null)
               {
                   var accessRole = _tokenS.Claims.First(claim => claim.Type == "custom:userrole").Value;
                   _role = accessRole;
               }
               return _role;
           }
       }
    }
}
