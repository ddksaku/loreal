using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using LorealOptimiseData;

namespace LorealOptimiseUnitTest
{
  //  [TestFixture]
    public class GeneralTest
    {
        //[Test]
        public void HistoryLogNewRecord()
        {
            DbDataContext db = new DbDataContext();
            db.HistoryLogs.DeleteAllOnSubmit(db.HistoryLogs);

            const string roleName = "TestRole";
            const int roleColumnsCount = 2;

            Role role = new Role();
            role.Name = roleName;
            db.Roles.InsertOnSubmit(role);
            db.SubmitChanges();

            
            List<HistoryLog> logs = db.HistoryLogs.Where(h => h.KeyValue == role.ID).ToList();

            Assert.Greater(logs.Count, 0, "No record was created in history log table.");
            Assert.AreEqual(logs.Count, roleColumnsCount, string.Format("Number of new records in history log table should be {0}, but is {1}.", roleColumnsCount, logs.Count));

            HistoryLog hlId = logs.Where(h=>h.FieldName == "ID").FirstOrDefault();
            Assert.IsNotNull(hlId, "No new record in history table with FieldName = 'ID' was found.");
            Assert.AreEqual(hlId.TableName, "Role");
            Assert.AreEqual(hlId.FieldName, "ID");
            Assert.AreEqual(hlId.NewValue.ToString(), role.ID.ToString());
            Assert.AreEqual(hlId.TypeOfUpdate, "Insert");
            Assert.AreEqual(hlId.OldValue, null);

            HistoryLog hlName = logs.Where(h => h.FieldName == "Name").FirstOrDefault();
            Assert.IsNotNull(hlName, "No new record in history table with FieldName = 'Name' was found.");
            Assert.AreEqual(hlName.TableName, "Role");
            Assert.AreEqual(hlName.FieldName, "Name");
            Assert.AreEqual(hlName.NewValue, roleName);
            Assert.AreEqual(hlName.TypeOfUpdate, "Insert");
            Assert.AreEqual(hlName.OldValue, null);
            Assert.IsNotNull(hlName.ModifiedDate);

            //db.HistoryLogs.DeleteAllOnSubmit(db.HistoryLogs);
            //db.Roles.DeleteOnSubmit(role);
            //db.SubmitChanges();
        }

       // [Test]
        public void HistoryLogUpdate()
        {
            DbDataContext db = new DbDataContext();
            db.HistoryLogs.DeleteAllOnSubmit(db.HistoryLogs);

            const string roleName = "TestRole";
            const string roleNameUpdated = "NewRoleName";

            Role role = new Role();
            role.Name = roleName;
            db.Roles.InsertOnSubmit(role);
            db.SubmitChanges();

            db.Dispose();
            db = new DbDataContext();
            role = db.Roles.Where(r => r.ID == role.ID).First();
            role.Name = roleNameUpdated;
            db.SubmitChanges();

            List<HistoryLog> logs = db.HistoryLogs.Where(h => h.KeyValue == role.ID && h.TypeOfUpdate == "Update").ToList();
            HistoryLog hlName = logs.Where(h => h.FieldName == "Name").FirstOrDefault();
            Assert.IsNotNull(hlName, "No new record in history table with FieldName = 'Name' was found.");
            Assert.AreEqual(hlName.TableName, "Role");
            Assert.AreEqual(hlName.FieldName, "Name");
            Assert.AreEqual(hlName.NewValue, roleNameUpdated);
            Assert.AreEqual(hlName.TypeOfUpdate, "Update");
            Assert.AreEqual(hlName.OldValue, roleName);
            Assert.IsNotNull(hlName.ModifiedDate);

            //db.HistoryLogs.DeleteAllOnSubmit(db.HistoryLogs);
            //db.Roles.DeleteOnSubmit(role);
            //db.SubmitChanges();
        }
    }
}