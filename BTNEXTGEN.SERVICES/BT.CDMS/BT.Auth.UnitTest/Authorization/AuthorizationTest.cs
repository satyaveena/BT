using BT.Auth.Business.Helpers;
using FakeItEasy;
using NUnit.Framework;
using Unity;
using BT.Auth.Business.Constants;
using System;
using System.Data;
using BT.Auth.Business.Logger.ELMAHLogger;
using Elmah;

namespace BT.Auth.UnitTest.Authentication
{
    class AuthorizationTest
    {
        [SetUp]
        public void Init()
        {
            //Unity Mapping
            //UnityHelper.Container.RegisterType(typeof(IXXXXManager), typeof(XXXXManager));
            //UnityHelper.Container.RegisterType(typeof(IXXXXDAO), typeof(XXXXDAO));
            UnityHelper.Container.RegisterType(typeof(ErrorLog), typeof(FakeELMAHMongoLogger));
        }

        [Test]
        public void Should_return_XXXX()
        {
            //Arrange
            


            //Act
            

            //Assert
            
        }

         

        #region MockData
         
       
        #endregion

    }
}
