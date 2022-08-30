// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.Threading.Tasks;
using Duende.IdentityServer.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace IdentityServer.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class LogoutModel : PageModel
    {
        private readonly IIdentityServerInteractionService _interactionService;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly ILogger<LogoutModel> _logger;

        public LogoutModel(IIdentityServerInteractionService interactionService,SignInManager<IdentityUser> signInManager, ILogger<LogoutModel> logger)
        {
             _interactionService = interactionService;
            _signInManager = signInManager;
            _logger = logger;
        }
        public bool LoggedOut { get; set; }
        public string PostLogoutRedirectUri { get; set; }
        public string SignOutIframeUrl { get; set; }
        public async Task<IActionResult> OnPost(string logoutId)
        {
            LoggedOut = true;
            await _signInManager.SignOutAsync();
            _logger.LogInformation("User logged out.");
            var request = await _interactionService.GetLogoutContextAsync(logoutId);
            if (request != null)
            {
                PostLogoutRedirectUri = request.PostLogoutRedirectUri;
                SignOutIframeUrl = request.SignOutIFrameUrl;
            }
            return Page();
        }
    }
}
