using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using mock.depart.Controllers;
using mock.depart.Data;
using mock.depart.Models;
using mock.depart.Services;
using Moq;
using NuGet.ContentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mock.depart.Controllers.Tests
{
    [TestClass()]
    public class CatsControllerTests
    {



        [TestMethod()]
        public void DeleteCatNull()
        {
            Mock<CatsService> catServiceMock = new Mock<CatsService>();
            Mock<CatsController> catControllerMock = new Mock<CatsController>(catServiceMock.Object) { CallBase = true };

            catServiceMock.Setup(s => s.Get(It.IsAny<int>())).Returns((Cat?)null);
            catControllerMock.Setup(foo => foo.UserId).Returns("1");

            var actionresult = catControllerMock.Object.DeleteCat(0);
            var result = actionresult.Result as NotFoundResult;
            Assert.IsNotNull(result);

        }

        [TestMethod()]
        public void DeleteCatBadOwnerId()
        {
            //si le owner id != de userid
            Mock<CatsService> catServiceMock = new Mock<CatsService>();
            Mock<CatsController> catControllerMock = new Mock<CatsController>(catServiceMock.Object) { CallBase = true };
            Mock<Cat> catMock = new Mock<Cat>();

            catControllerMock.Setup(foo => foo.UserId).Returns("2");
            catMock.Setup(c => c.CatOwner).Returns(new CatOwner { Id = "1"});
            catServiceMock.Setup(s => s.Delete(1)).Throws(new BadRequest("Cat is not yours"));

            var actionresult = catControllerMock.Object.DeleteCat(1);
            var result = actionresult.Result as BadRequestResult;
            Assert.IsNotNull(result);
        }

        [TestMethod()]
        public void DeleteCat()
        {
            //delete bien le chat
            Mock<CatsService> catServiceMock = new Mock<CatsService>();
            Mock<CatsController> catControllerMock = new Mock<CatsController>(catServiceMock.Object) { CallBase = true };

            catControllerMock.Setup(foo => foo.DeleteCat(1)).Returns());
        }

        [TestMethod()]
        public void DeleteTooCuteCat()
        {
            //si le chat est trop cute
        }
    }
}