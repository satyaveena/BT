using System.Collections.Generic;
using BT.CDMS.API.Controllers;
using BT.CDMS.Business.Helpers;
using BT.CDMS.Business.DataAccess.Interface;
using BT.CDMS.Business.DataAccess;
using BT.CDMS.Business.Manager.Interface;
using BT.CDMS.Business.Manager;
using FakeItEasy;
using NUnit.Framework;
using Unity;
using BT.CDMS.API.Controllers.Grid;
using BT.CDMS.Business.Constants;
using System;
using System.Data;
using BT.CDMS.Business.Logger.ELMAHLogger;
using Elmah;
using BT.CDMS.Business.Models;

namespace BT.TS360.UnitTest.Grid
{
    class GridTest
    {
        [SetUp]
        public void Init()
        {
            //Unity Mapping
            UnityHelper.Container.RegisterType(typeof(IGridManager), typeof(GridManager));
            UnityHelper.Container.RegisterType(typeof(IGridDAO), typeof(GridDAO));
            UnityHelper.Container.RegisterType(typeof(ErrorLog), typeof(FakeELMAHMongoLogger));
        }

        #region Test GetGridTemplatesByOrgId
        [Test]
        public void Should_return_GetGridTemplatesByOrgId()
        {
            //Arrange
            string _orgIdFake = "FakeId1";
            IGridDAO _gridFakeDAO = A.Fake<IGridDAO>();
            A.CallTo(() => _gridFakeDAO.GetGridTemplatesByOrgId(_orgIdFake)).Returns(GetFakeGridTemplates());
            UnityHelper.Container.RegisterInstance(_gridFakeDAO);


            //Act
            GridController _SUT = new GridController();
            var list = _SUT.GetGridTemplatesByOrgId(_orgIdFake);

            //Assert
            Assert.AreEqual(list.Status, AppServiceStatus.Success);
            Assert.AreEqual(list.Data.Count, 2);
            Assert.AreEqual(list.Data[0].GridTemplateId, "{ADD63AAA-F6B9-4D6A-83B2-F17262D67F50}");
            Assert.AreEqual(list.Data[0].GridTemplateName, "Grid Template A");
            Assert.AreEqual(list.Data[0].Description, "Grid 1");
        }

        [Test]
        public void Should_return_GetGridTemplatesByOrgId_Invalid_OrgId()
        {
            //Arrange
            string _orgIdFake = "FakeId1";
            IGridDAO _gridFakeDAO = A.Fake<IGridDAO>();
            var ds = GetFakeGridTemplates();
            ds.Tables.Clear();
            A.CallTo(() => _gridFakeDAO.GetGridTemplatesByOrgId(_orgIdFake)).Returns(ds);
            UnityHelper.Container.RegisterInstance(_gridFakeDAO);


            //Act
            GridController _SUT = new GridController();
            var list = _SUT.GetGridTemplatesByOrgId(_orgIdFake);

            //Assert
            Assert.AreEqual(list.Status, AppServiceStatus.Success);
            Assert.AreEqual(list.Data.Count, 0);
        }

        [Test]
        public void Should_return_GetGridTemplatesByOrgId_Exception()
        {
            //Arrange
            string _orgIdFake = "FakeId1";
            IGridDAO _gridFakeDAO = A.Fake<IGridDAO>();
            A.CallTo(() => _gridFakeDAO.GetGridTemplatesByOrgId(_orgIdFake)).Throws(new Exception("An Error Occurred"));
            UnityHelper.Container.RegisterInstance(_gridFakeDAO);

            //Act
            GridController _SUT = new GridController();

            //Assert
            Assert.Catch(() => { _SUT.GetGridTemplatesByOrgId(_orgIdFake); }, "An Error Occurred");
        }

        #region MockData
        private DataSet GetFakeGridTemplates()
        {
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            #region columns
            dt.Columns.Add(ApplicationConstants.COLUMN_GRID_TEMPLATE_ID);
            dt.Columns.Add(ApplicationConstants.COLUMN_GRID_TEMPLATE_NAME);
            dt.Columns.Add(ApplicationConstants.COLUMN_GRID_TEMPLATE_DESCRIPTION);

            #endregion
            dt.Rows.Add(new object[] { "{ADD63AAA-F6B9-4D6A-83B2-F17262D67F50}", "Grid Template A", "Grid 1" });
            dt.Rows.Add(new object[] { "{B14F81FE-760F-4E0C-98A8-39754CB02574}", "Grid Template B", "Grid 2" });
            ds.Tables.Add(dt);
            return ds;
        }
        #endregion
        #endregion

        #region Test CheckIfTemplateIsAccessibleToListOfUsers
        [Test]
        public void Should_return_CheckIfTemplateIsAccessibleToListOfUsers()
        {
            //Arrange
            var request = new CheckGridTemplateAccessRequest
            {
                GridTemplateId = "FakeGridTemplateId1",
                UserIdsList = new List<string> { "FakeUserId1", "FakeUserId2" }
            };
            IGridDAO _gridFakeDAO = A.Fake<IGridDAO>();
            A.CallTo(() => _gridFakeDAO.CheckIfTemplateIsAccessibleToListOfUsers(request)).Returns(GetFakeUserAccess());
            UnityHelper.Container.RegisterInstance(_gridFakeDAO);


            //Act
            GridController _SUT = new GridController();
            var list = _SUT.CheckIfTemplateIsAccessibleToListOfUsers(request);

            //Assert
            Assert.AreEqual(list.Status, AppServiceStatus.Success);
            Assert.AreEqual(list.Data.Count, 2);
            Assert.AreEqual(list.Data[0].UserId, "FakeUserId1");
            Assert.AreEqual(list.Data[0].IsAccess, true);
            Assert.AreEqual(list.Data[1].UserId, "FakeUserId2");
            Assert.AreEqual(list.Data[1].IsAccess, false);
        }

        [Test]
        public void Should_return_CheckIfTemplateIsAccessibleToListOfUsers_Invalid_GridTemplateId()
        {
            //Arrange
            var request = new CheckGridTemplateAccessRequest
            {
                GridTemplateId = "FakeGridTemplateId1",
                UserIdsList = new List<string> { "FakeUserId1", "FakeUserId2" }
            };
            IGridDAO _gridFakeDAO = A.Fake<IGridDAO>();
            var ds = GetFakeUserAccess();
            ds.Tables.Clear();
            A.CallTo(() => _gridFakeDAO.CheckIfTemplateIsAccessibleToListOfUsers(request)).Returns(ds);
            UnityHelper.Container.RegisterInstance(_gridFakeDAO);

            //Act
            GridController _SUT = new GridController();
            var list = _SUT.CheckIfTemplateIsAccessibleToListOfUsers(request);

            //Assert
            Assert.AreEqual(list.Status, AppServiceStatus.Success);
            Assert.AreEqual(list.Data.Count, 0);
        }

        [Test]
        public void Should_return_CheckIfTemplateIsAccessibleToListOfUsers_Exception()
        {
            //Arrange
            var request = new CheckGridTemplateAccessRequest
            {
                GridTemplateId = "FakeGridTemplateId1",
                UserIdsList = new List<string> { "FakeUserId1", "FakeUserId2" }
            };
            IGridDAO _gridFakeDAO = A.Fake<IGridDAO>();
            A.CallTo(() => _gridFakeDAO.CheckIfTemplateIsAccessibleToListOfUsers(request)).Throws(new Exception("An Error Occurred"));
            UnityHelper.Container.RegisterInstance(_gridFakeDAO);

            //Act
            GridController _SUT = new GridController();

            //Assert
            Assert.Catch(() => { _SUT.CheckIfTemplateIsAccessibleToListOfUsers(request); }, "An Error Occurred");
        }

        #region MockData
        private DataSet GetFakeUserAccess()
        {
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            #region columns
            dt.Columns.Add(ApplicationConstants.COLUMN_USER_ID_ALIAS);
            dt.Columns.Add("IsAccess");

            #endregion
            dt.Rows.Add(new object[] { "FakeUserId1", 1 });
            dt.Rows.Add(new object[] { "FakeUserId2", 0 });
            ds.Tables.Add(dt);
            return ds;
        }
        #endregion
        #endregion
    }
}
