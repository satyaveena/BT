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
using BT.CDMS.API.Controllers.Organization;
using BT.CDMS.Business.Constants;
using System;
using System.Data;
using BT.CDMS.Business.Logger.ELMAHLogger;
using Elmah;

namespace BT.CDMS.UnitTest.Organization
{
    class OrganizationTest
    {
        [SetUp]
        public void Init()
        {
            //Unity Mapping
            UnityHelper.Container.RegisterType(typeof(IOrganizationManager), typeof(OrganizationManager));
            UnityHelper.Container.RegisterType(typeof(IOrganizationDAO), typeof(OrganizationDAO));
            UnityHelper.Container.RegisterType(typeof(ErrorLog), typeof(FakeELMAHMongoLogger));
        }

        #region Test SearchByOrgName
        [Test]
        public void Should_return_SearchByOrgName()
        {
            //Arrange
            string _orgNameFake = "FakeName1";
            IOrganizationDAO _orgFakeDAO = A.Fake<IOrganizationDAO>();
            A.CallTo(() => _orgFakeDAO.SearchByOrgName(_orgNameFake)).Returns(GetFakeOrganizations());
            UnityHelper.Container.RegisterInstance(_orgFakeDAO);


            //Act
            OrganizationController _SUT = new OrganizationController();
            var list = _SUT.SearchByOrgName(_orgNameFake);

            //Assert
            Assert.AreEqual(list.Status, AppServiceStatus.Success);
            Assert.AreEqual(list.Data.Count, 2);
            Assert.AreEqual(list.Data[0].OrganizationId, "{013cacd9-4073-4466-934f-cefdf3f75a67}");
            Assert.AreEqual(list.Data[0].OrganizationName, "Org1");
        }

        [Test]
        public void Should_return_SearchByOrgName_Invalid_OrgName()
        {
            //Arrange
            string _orgNameFake = "FakeName1";
            IOrganizationDAO _orgFakeDAO = A.Fake<IOrganizationDAO>();
            var ds = GetFakeOrganizations();
            ds.Tables.Clear();
            A.CallTo(() => _orgFakeDAO.SearchByOrgName(_orgNameFake)).Returns(ds);
            UnityHelper.Container.RegisterInstance(_orgFakeDAO);


            //Act
            OrganizationController _SUT = new OrganizationController();
            var list = _SUT.SearchByOrgName(_orgNameFake);

            //Assert
            Assert.AreEqual(list.Status, AppServiceStatus.Success);
            Assert.AreEqual(list.Data.Count, 0);
        }

        [Test]
        public void Should_return_SearchByOrgName_Exception()
        {
            //Arrange
            string _orgNameFake = "FakeName1";
            IOrganizationDAO _orgFakeDAO = A.Fake<IOrganizationDAO>();
            //A.CallTo(() => _orgFakeDAO.SearchByOrgName(_orgNameFake)).Throws<Exception>();
            A.CallTo(() => _orgFakeDAO.SearchByOrgName(_orgNameFake)).Throws(new Exception("An Error Occurred"));
            UnityHelper.Container.RegisterInstance(_orgFakeDAO);

            //Act
            OrganizationController _SUT = new OrganizationController();
            //var list = _SUT.SearchByOrgName(_orgNameFake);

            //Assert
            //Assert.AreEqual(list.Status, AppServiceStatus.Fail);
            //Assert.AreNotEqual(list.ErrorMessage.Length, 0);
            Assert.Catch(() => { _SUT.SearchByOrgName(_orgNameFake); }, "An Error Occurred");
        }

        #region MockData
        private DataSet GetFakeOrganizations()
        {
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            #region columns
            dt.Columns.Add(ApplicationConstants.COLUMN_ORG_ID_ALIAS);
            dt.Columns.Add(ApplicationConstants.COLUMN_ORG_NAME_ALIAS);
            #endregion
            dt.Rows.Add(new object[] { "{013cacd9-4073-4466-934f-cefdf3f75a67}", "Org1" });
            dt.Rows.Add(new object[] { "{0ghcbdr9-1243-4556-933d-trhgf3f75a67}", "Org2" });
            ds.Tables.Add(dt);
            return ds;
        }
        #endregion
        #endregion

        #region Test GetLoginIDsByOrgId
        [Test]
        public void Should_return_GetLoginIDsByOrgId()
        {
            //Arrange
            string _orgIdFake = "FakeId1";
            IOrganizationDAO _orgFakeDAO = A.Fake<IOrganizationDAO>();
            A.CallTo(() => _orgFakeDAO.GetLoginIDsByOrgId(_orgIdFake)).Returns(GetFakeUserInfo());
            UnityHelper.Container.RegisterInstance(_orgFakeDAO);


            //Act
            OrganizationController _SUT = new OrganizationController();
            var list = _SUT.GetLoginIDsByOrgId(_orgIdFake);

            //Assert
            Assert.AreEqual(list.Status, AppServiceStatus.Success);
            Assert.AreEqual(list.Data.Count, 2);
            Assert.AreEqual(list.Data[0].UserId, "{00B3C297-ADF2-4ED8-A4D6-0B23F6C103C3}");
            Assert.AreEqual(list.Data[0].UserName, "JTHACKER");
            Assert.AreEqual(list.Data[0].UserAlias, "B Collection Development");
        }

        [Test]
        public void Should_return_GetLoginIDsByOrgId_Invalid_OrgId()
        {
            //Arrange
            string _orgIdFake = "FakeId1";
            IOrganizationDAO _orgFakeDAO = A.Fake<IOrganizationDAO>();
            var ds = GetFakeUserInfo();
            ds.Tables.Clear();
            A.CallTo(() => _orgFakeDAO.GetLoginIDsByOrgId(_orgIdFake)).Returns(ds);
            UnityHelper.Container.RegisterInstance(_orgFakeDAO);


            //Act
            OrganizationController _SUT = new OrganizationController();
            var list = _SUT.GetLoginIDsByOrgId(_orgIdFake);

            //Assert
            Assert.AreEqual(list.Status, AppServiceStatus.Success);
            Assert.AreEqual(list.Data.Count, 0);
        }

        [Test]
        public void Should_return_GetLoginIDsByOrgId_Exception()
        {
            //Arrange
            string _orgIdFake = "FakeId1";
            IOrganizationDAO _orgFakeDAO = A.Fake<IOrganizationDAO>();
            A.CallTo(() => _orgFakeDAO.GetLoginIDsByOrgId(_orgIdFake)).Throws(new Exception("An Error Occurred"));
            UnityHelper.Container.RegisterInstance(_orgFakeDAO);

            //Act
            OrganizationController _SUT = new OrganizationController();

            //Assert
            Assert.Catch(() => { _SUT.GetLoginIDsByOrgId(_orgIdFake); }, "An Error Occurred");
        }

        #region MockData
        private DataSet GetFakeUserInfo()
        {
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            #region columns
            dt.Columns.Add(ApplicationConstants.COLUMN_USER_ID);
            dt.Columns.Add(ApplicationConstants.COLUMN_USER_NAME);
            dt.Columns.Add(ApplicationConstants.COLUMN_USER_ALIAS);

            #endregion
            dt.Rows.Add(new object[] { "{00B3C297-ADF2-4ED8-A4D6-0B23F6C103C3}", "JTHACKER", "B Collection Development" });
            dt.Rows.Add(new object[] { "{005E7638-E097-4B26-9E9F-A4E6529BF15D}", "CARMICHY@CLAYTONPL.ORG", "YVONNE CARMICHAEL" });
            ds.Tables.Add(dt);
            return ds;
        }
        #endregion
        #endregion

        #region Test SearchByLoginId
        [Test]
        public void Should_return_SearchByLoginId()
        {
            //Arrange
            string _loginIdFake = "FakeLoginId1";
            IOrganizationDAO _orgFakeDAO = A.Fake<IOrganizationDAO>();
            A.CallTo(() => _orgFakeDAO.SearchByLoginId(_loginIdFake)).Returns(GetFakeUserID());
            UnityHelper.Container.RegisterInstance(_orgFakeDAO);


            //Act
            OrganizationController _SUT = new OrganizationController();
            var list = _SUT.SearchByLoginId(_loginIdFake);

            //Assert
            Assert.AreEqual(list.Status, AppServiceStatus.Success);
            Assert.AreEqual(list.Data, true);
        }

        [Test]
        public void Should_return_SearchByLoginId_Invalid_UserName()
        {
            //Arrange
            string _loginIdFake = "FakeLoginId1";
            IOrganizationDAO _orgFakeDAO = A.Fake<IOrganizationDAO>();
            A.CallTo(() => _orgFakeDAO.SearchByLoginId(_loginIdFake)).Returns(string.Empty);
            UnityHelper.Container.RegisterInstance(_orgFakeDAO);


            //Act
            OrganizationController _SUT = new OrganizationController();
            var list = _SUT.SearchByLoginId(_loginIdFake);

            //Assert
            Assert.AreEqual(list.Status, AppServiceStatus.Success);
            Assert.AreEqual(list.Data, false);
        }

        [Test]
        public void Should_return_SearchByLoginId_Exception()
        {
            //Arrange
            string _loginIdFake = "FakeLoginId1";
            IOrganizationDAO _orgFakeDAO = A.Fake<IOrganizationDAO>();
            A.CallTo(() => _orgFakeDAO.SearchByLoginId(_loginIdFake)).Throws(new Exception("An Error Occurred"));
            UnityHelper.Container.RegisterInstance(_orgFakeDAO);

            //Act
            OrganizationController _SUT = new OrganizationController();

            //Assert
            Assert.Catch(() => { _SUT.SearchByLoginId(_loginIdFake); }, "An Error Occurred");
        }

        #region MockData
        private string GetFakeUserID()
        {
            var userID = "{00B3C297-ADF2-4ED8-A4D6-0B23F6C103C3}";
            return userID;
        }
        #endregion
        #endregion
    }
}