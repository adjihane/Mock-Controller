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
            //dans un test ou le controller va chercher un user il faut mocker le controller
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
            Cat catMock = new Cat();
            catMock.CatOwner = new CatOwner();
            catMock.CatOwner.Id = "1";
            catMock.Id = 1;

            //mock UserId
            catControllerMock.Setup(foo => foo.UserId).Returns("2");


            //catMock.Setup(c => c.CatOwner).Returns(new CatOwner { Id = "1"});


            catServiceMock.Setup(s => s.Get(1)).Returns(catMock);

            // catServiceMock.Setup(s => s.Delete(1)).Throws(new BadRequestResult());

            var actionresult = catControllerMock.Object.DeleteCat(1);
            var result = actionresult.Result as BadRequestObjectResult;
            Assert.IsNotNull(result);
        }

        [TestMethod()]
        public void DeleteCat()
        {
            //delete bien le chat
            Mock<CatsService> catServiceMock = new Mock<CatsService>();
            Mock<CatsController> catControllerMock = new Mock<CatsController>(catServiceMock.Object) { CallBase = true };
            //CatsController catsController = new CatsController(catServiceMock.Object);

            Cat cat = new Cat() { CuteLevel = Cuteness.BarelyOk, CatOwner = new CatOwner() { Id = "1" } };

            catServiceMock.Setup(s => s.Get(cat.Id)).Returns(cat);
            catControllerMock.Setup(foo => foo.UserId).Returns("1");

            var actionresult = catControllerMock.Object.DeleteCat(cat.Id);
            var result = actionresult.Result as OkObjectResult;
            Assert.IsNotNull(result);
        }

        [TestMethod()]
        public void DeleteTooCuteCat()
        {
            //si le chat est trop cute
            Mock<CatsService> catServiceMock = new Mock<CatsService>();
            Mock<CatsController> catControllerMock = new Mock<CatsController>(catServiceMock.Object) { CallBase = true };
            Cat catTooCute = new Cat() { CuteLevel = Cuteness.Amazing, CatOwner = new CatOwner() { Id = "1" } };

            catControllerMock.Setup(t => t.UserId).Returns("1");
            catServiceMock.Setup(t => t.Get(catTooCute.Id)).Returns(catTooCute);

            var actionresult = catControllerMock.Object.DeleteCat(catTooCute.Id);
            var result = actionresult.Result as BadRequestObjectResult;
            Assert.IsNotNull(result);

        }
    }
}