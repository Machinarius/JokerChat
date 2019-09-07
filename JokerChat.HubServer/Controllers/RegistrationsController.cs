using JokerChat.Common;
using JokerChat.HubServer.Registrations;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace JokerChat.HubServer.Controllers {
  [EnableCors]
  public class RegistrationsController : Controller {
    private readonly IHttpContextAccessor _contextAccessor;
    private readonly IRegistrationsManager _registrationsManager;

    public RegistrationsController(IHttpContextAccessor contextAccessor, IRegistrationsManager registrationsManager) {
      _contextAccessor = contextAccessor ?? throw new ArgumentNullException(nameof(contextAccessor));
      _registrationsManager = registrationsManager ?? throw new ArgumentNullException(nameof(registrationsManager));
    }

    [HttpPost]
    public async Task AnnounceRegistration([FromBody] RegistrationData registrationData) {
      registrationData.EndpointHostName = _contextAccessor.HttpContext.Connection.RemoteIpAddress.ToString();
      await _registrationsManager.StoreRegistrationAsync(registrationData);
    }
  }
}
