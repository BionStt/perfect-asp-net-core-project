using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using DAL.Data.Context;
using DAL.Entities.Identity;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using UnitTests.WebUI;
using WebUI;
using WebUI.Controllers;
using WebUI.Models.AccountViewModels;

namespace UnitTests
{
    [TestClass]
    public class AuthTests : BaseWebHostTests
    {
        private HttpClient _client;
        private AccountController _controller;

        [TestInitialize]
        public void Init()
        {
            _client = Server.CreateClient();
            var signInManager = Services.GetService<SignInManager<User>>();
            var logger = Services.GetService<ILogger<AccountController>>();
            var userManager = Services.GetService<UserManager<User>>();
            _controller = new AccountController(userManager, signInManager, logger);
        }

        [TestMethod]
        public async Task GetTokenTest()
        {
            //Arrange
            var userName = "username";
            var password = "password";
            var userManager = (UserManager<User>)Services.GetService(typeof(UserManager<User>));
            await userManager.CreateAsync(new User { UserName = userName, Email = $"{userName}@email.com" }, password);
            //Act
            var result = _controller.Login(new LoginViewModel { UserName = userName, Password = password }).Result as OkObjectResult;
            //Assert
            Assert.IsNotNull((result.Value as JWTAuthorizationResponse));
            Assert.AreEqual((result.Value as JWTAuthorizationResponse).UserName, userName);
        }

        [TestMethod]
        public async Task RegisterTest()
        {
            //Arrange
            var userName = "username";
            var password = "password";
            var model = new RegisterViewModel { UserName = userName, Password = password, ConfirmPassword = password };
            //Act
            var result = await _controller.Register(model);
            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            var okResult = result as OkObjectResult;
            var tokenResponse = okResult.Value as JWTAuthorizationResponse;
            Assert.IsNotNull(tokenResponse);
            Assert.AreSame(model.UserName, tokenResponse.UserName);

        }

        [TestMethod]
        public async Task RegisterErrorHandlingTest()
        {
            //Arrange
            var userName = "username";
            var password = "password";
            var model = new RegisterViewModel { UserName = userName, Password = password, ConfirmPassword = password };
            //Act
            _controller.ModelState.AddModelError("key", "error");
            var result = await _controller.Register(model);
            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
        }



        [TestCleanup]
        public void CleanupTest()
        {
            Services.GetService<ApplicationContext>().Database.EnsureDeleted();
        }
    }
}
