using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using mock.depart.Controllers;
using mock.depart.Models;
using mock.depart.Services;
using Moq;

namespace mock.depart.Controllers.Tests
{
    [TestClass()]
    public class CatsControllerTests
    {
        [TestMethod()]
        public void Delete_ok()
        {
            Mock<CatsService> catsServiceMock = new Mock<CatsService>();
            Mock<CatsController> catsControllerMock = new Mock<CatsController>(catsServiceMock.Object) { CallBase = true };

            CatOwner bob = new CatOwner()
            {
                Id = "1111"
            };
            Cat c = new Cat()
            {
                Id = 1,
                Name = "loulou",
                CatOwner = bob,
                CuteLevel = Cuteness.BarelyOk
            };
            catsServiceMock.Setup(s => s.Get(It.IsAny<int>())).Returns(c);
            catsServiceMock.Setup(s => s.Delete(It.IsAny<int>())).Returns(c);
            catsControllerMock.Setup(c => c.UserId).Returns("1111");

            var actionresult = catsControllerMock.Object.DeleteCat(0);

            var result = actionresult.Result as OkObjectResult;

            Assert.IsNotNull(result);

            Cat? catresult = (Cat?)result!.Value;
            Assert.AreEqual(c.Id, catresult!.Id);
        }

        [TestMethod()]
        public void Delete_CatNotFound()
        {
            Mock<CatsService> catsServiceMock = new Mock<CatsService>();
            Mock<CatsController> catsControllerMock = new Mock<CatsController>(catsServiceMock.Object) { CallBase = true };

            catsServiceMock.Setup(s => s.Get(It.IsAny<int>())).Returns(value: null);
            catsControllerMock.Setup(c => c.UserId).Returns("2");

            var actionresult = catsControllerMock.Object.DeleteCat(0);

            var result = actionresult.Result as NotFoundResult;

            Assert.IsNotNull(result);
        }
        [TestMethod()]
        public void Delete_WrongOwner()
        {
            Mock<CatsService> catsServiceMock = new Mock<CatsService>();
            Mock<CatsController> catsControllerMock = new Mock<CatsController>(catsServiceMock.Object) { CallBase = true };

            CatOwner bob = new CatOwner()
            {
                Id = "1111"
            };
            Cat c = new Cat()
            {
                Id = 1,
                Name = "loulou",
                CatOwner = bob,
                CuteLevel = Cuteness.Amazing
            };
            catsServiceMock.Setup(s => s.Get(It.IsAny<int>())).Returns(c);
            catsControllerMock.Setup(c => c.UserId).Returns("2");

            var actionresult = catsControllerMock.Object.DeleteCat(0);

            var result = actionresult.Result as BadRequestObjectResult;

            Assert.AreEqual("Cat is not yours", result.Value);
            
        }
        [TestMethod()]
        public void Delete_TooCute()
        {
            Mock<CatsService> catsServiceMock = new Mock<CatsService>();
            Mock<CatsController> catsControllerMock = new Mock<CatsController>(catsServiceMock.Object) { CallBase = true };

            CatOwner bob = new CatOwner()
            {
                Id = "1111"
            };
            Cat c = new Cat()
            {
                Id = 1,
                Name = "loulou",
                CatOwner = bob,
                CuteLevel = Cuteness.Amazing
            };
            catsServiceMock.Setup(s => s.Get(It.IsAny<int>())).Returns(c);
            catsControllerMock.Setup(c => c.UserId).Returns("1111");

            var actionresult = catsControllerMock.Object.DeleteCat(0);

            var result = actionresult.Result as BadRequestObjectResult;
            Assert.AreEqual("Cat is too cute", result.Value);
        }
    }
}