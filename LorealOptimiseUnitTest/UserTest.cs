using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using LorealOptimiseData;
using LorealOptimiseBusiness;
using System.Security.Principal;
using LorealOptimiseBusiness.Exceptions;

namespace LorealOptimiseUnitTest
{
    //[TestFixture]
    public class UserTest : TestBase
    {
       // [Test]
        public void Login()
        {
            Db.UserRoles.DeleteAllOnSubmit(Db.UserRoles);
            Db.Roles.DeleteAllOnSubmit(Db.Roles);
            Db.Users.DeleteAllOnSubmit(Db.Users);
            Db.Divisions.DeleteAllOnSubmit(Db.Divisions);
            Db.Roles.DeleteAllOnSubmit(Db.Roles);
            Db.SubmitChanges();

            Division division1 = CreateDivision();
            division1.Deleted = false;

            Division division2 = CreateDivision();
            division2.Deleted = false;

            Division division3 = CreateDivision();
            division3.Deleted = false;

            Division divisionDeleted = CreateDivision();
            divisionDeleted.Deleted = true;

            Db.Divisions.InsertOnSubmit(division1);
            Db.Divisions.InsertOnSubmit(division2);
            Db.Divisions.InsertOnSubmit(division3);
            Db.Divisions.InsertOnSubmit(divisionDeleted);
            Db.SubmitChanges();

            UserManager manager = new UserManager(Db);

            Assert.Throws(typeof(LorealLoginException), () => manager.Login(), "Login was successfull for non existing user");
            
            User userCurrentlyLogged = CreateUser(WindowsIdentity.GetCurrent().Name, true);

            Assert.Throws(typeof(LorealLoginException), () => manager.Login(), "Login was successfull for user with no roles");

            AssignUserToDivisions(userCurrentlyLogged, division1, division2, divisionDeleted);

            int divisionCount = 0;
            User loggedUser = manager.Login(out divisionCount);

            Assert.AreEqual(divisionCount, 2, "User was assigned to 2 non deleted division, but Login function returned {0}", divisionCount);
            Assert.IsNotNull(loggedUser, "Login method returned null, even login should be successfull");
        }

    }
}
