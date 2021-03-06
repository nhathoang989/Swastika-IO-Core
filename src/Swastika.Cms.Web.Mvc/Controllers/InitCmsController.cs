﻿// Licensed to the Swastika I/O Foundation under one or more agreements.
// The Swastika I/O Foundation licenses this file to you under the GNU General Public License v3.0.
// See the LICENSE file in the project root for more information.

using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Swastika.Cms.Lib;
using Swastika.Cms.Lib.Models.Cms;
using Swastika.Cms.Lib.Services;
using Swastika.Cms.Mvc.Controllers;
using Swastika.Identity.Models;
using System;
using System.Linq;
using static Swastika.Common.Utility.Enums;

namespace Swastika.Cms.Web.Mvc.Controllers
{
    public class InitCmsController : BaseController<InitCmsController>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public InitCmsController(IHostingEnvironment env,
             UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager
            ) : base(env)
        {
            this._userManager = userManager;
            this._roleManager = roleManager;
        }

        [HttpGet]
        [Route("")]
        [Route("{culture}")]
        public async System.Threading.Tasks.Task<IActionResult> Index()
        {
            if (string.IsNullOrEmpty(GlobalConfigurationService.Instance.GetConfigConnectionKey()))
            {
                return RedirectToAction("Init", "Portal", new { culture = ROUTE_DEFAULT_CULTURE });
            }
            else
            {
                var superAdmin = await _userManager.GetUsersInRoleAsync("SuperAdmin");
                if (superAdmin.Count == 0)
                {
                    return Redirect($"/{ROUTE_DEFAULT_CULTURE}/Portal/Auth/RegisterSuperAdmin");
                }
                else
                {
                    GlobalConfigurationService.Instance.IsInit = true;
                    GlobalConfigurationService.Instance.Refresh();
                    return RedirectToAction("", "Home", new { culture = CurrentLanguage });
                }
            }
        }
    }
}