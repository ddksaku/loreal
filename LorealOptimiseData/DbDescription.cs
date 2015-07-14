using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Data.SqlClient;
using System.Reflection;
using LorealOptimiseShared;
using LorealOptimiseData.Exceptions;
using LorealOptimiseShared.Logging;
using System.Collections;

namespace LorealOptimiseData
{
    /// <summary>
    /// DataContext for whole application
    /// </summary>
    partial class DbDataContext
    {
        private bool loadChildEntities = true;

        private static DbDataContext instance;
        public static DbDataContext GetInstance()
        {
            if (instance == null)
            {
                MakeNewInstance();
            }

            return instance;
        }

        public static void MakeNewInstance()
        {
            MakeNewInstance(null);
        }

        public static void MakeNewInstance(DataLoadOptions loadOptions)
        {
            instance = new DbDataContext();

            if (instance.Connection is SqlConnection)
            {
                (instance.Connection as SqlConnection).InfoMessage += new SqlInfoMessageEventHandler(DbDataContext_InfoMessage);
            }

            if (loadOptions != null)
            {
                instance.LoadOptions = loadOptions;
            }
        }

        public DbDataContext(bool loadChildEntities): base(global::LorealOptimiseData.Properties.Settings.Default.LorealOptimiseConnectionString, mappingSource)
        {
            this.loadChildEntities = loadChildEntities;
            OnCreated();
        }

        public delegate void InfoMessageReceivedEventHandler(object sender, string message, IEnumerable<byte> states);
        public static event InfoMessageReceivedEventHandler InfoMessageReceived;

        static void DbDataContext_InfoMessage(object sender, SqlInfoMessageEventArgs e)
        {
            if (InfoMessageReceived != null)
            {
                IEnumerable<byte> states = e.Errors.Cast<SqlError>().Select(se => se.State);
                InfoMessageReceived(sender, e.Message, states);
            }
        }


        partial void OnCreated()
        {
            this.ExecuteCommand("SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED");

            if (loadChildEntities)
            {
                DataLoadOptions loadOptions = new DataLoadOptions();

                loadOptions.LoadWith<User>(u => u.UserRoles);
                loadOptions.LoadWith<UserRole>(ur => ur.Division);
                loadOptions.LoadWith<UserRole>(ur => ur.Role);

                loadOptions.LoadWith<Animation>(a => a.AnimationType);
                loadOptions.LoadWith<Animation>(a => a.Priority);
                loadOptions.LoadWith<Animation>(a => a.SalesDrive);
                //loadOptions.LoadWith<Animation>(a => a.DistributionChannel);
                //loadOptions.LoadWith<Animation>(a => a.OrderType);
                //loadOptions.LoadWith<Animation>(a => a.AnimationCustomerGroups);

                loadOptions.LoadWith<AnimationProduct>(a => a.Animation);
                loadOptions.LoadWith<AnimationProduct>(a => a.Category);
                loadOptions.LoadWith<AnimationProduct>(a => a.ItemGroup);
                loadOptions.LoadWith<AnimationProduct>(a => a.Product);
                loadOptions.LoadWith<AnimationProduct>(a => a.ItemType);
                loadOptions.LoadWith<AnimationProduct>(a => a.Signature);
                loadOptions.LoadWith<AnimationProduct>(a => a.BrandAxe);
                loadOptions.LoadWith<AnimationProduct>(a => a.Multiple);
                loadOptions.LoadWith<AnimationProduct>(a => a.Multiple1);
                //loadOptions.LoadWith<AnimationProduct>(a => a.AnimationProductDetails);

                loadOptions.LoadWith<AnimationProductDetail>(apd => apd.SalesArea);
                //loadOptions.LoadWith<AnimationProductDetail>(apd => apd.CustomerAllocations);
                //loadOptions.LoadWith<AnimationProductDetail>(apd => apd.CustomerGroupAllocations);

                loadOptions.LoadWith<CustomerAllocation>(ca => ca.Customer);

                loadOptions.LoadWith<CustomerGroupAllocation>(ca => ca.CustomerGroup);

                loadOptions.LoadWith<AnimationCustomerGroup>(acg => acg.CustomerGroup);
                loadOptions.LoadWith<AnimationCustomerGroup>(acg => acg.RetailerType);

                //loadOptions.LoadWith<Sale>(s => s.BrandAxe);

                loadOptions.LoadWith<BrandAxe>(b => b.Signature);

                //loadOptions.LoadWith<Customer>(c => c.SalesArea);
                //loadOptions.LoadWith<Customer>(c => c.CustomerGroup);
                //loadOptions.LoadWith<Customer>(c => c.SalesEmployee);

                //loadOptions.LoadWith<SalesArea>(sa => sa.SalesOrganization);

                loadOptions.LoadWith<CustomerGroup>(cg => cg.SalesArea);

                loadOptions.LoadWith<Product>(p => p.Division);

                this.LoadOptions = loadOptions;

                #if DEBUG
                //Log = new DebuggerWriter();
                #endif
            }

            this.CommandTimeout = Utility.SqlCommandTimeOut;
         }

        void sqlConnection_InfoMessage(object sender, SqlInfoMessageEventArgs e)
        {
            
        }

        /// <summary>
        /// Sends changes that were made to retrieved objects to the underlying database, and specifies the action to be taken if the submission fails.
        /// Assign ModifiedBy and ModifiedDate properties to entities, which implementes ITrackHistory interface during insert/update
        /// Creates new record in HistoryLog for entities being deleted
        /// </summary>
        /// <param name="failureMode">The action to be taken if the submission fails. Valid arguments are as follows: System.Data.Linq.ConflictMode.FailOnFirstConflictSystem.Data.Linq.ConflictMode.ContinueOnConflict</param>
        public override void SubmitChanges(ConflictMode failureMode)
        {
            ChangeSet changes = null;
            try
            {
                changes = GetChangeSet();
            }
            catch (Exception ex)
            {
                Logger.Log("GetChangeSet error " + ex.Message, LogLevel.Error, LogFilePrefix.Db);
            }

            PreProcessChangeSet(changes); 

            if (Connection is SqlConnection)
            {
                SqlConnection sqlConnection = Connection as SqlConnection;
                sqlConnection.InfoMessage += new SqlInfoMessageEventHandler(sqlConnection_InfoMessage);
            }

            try
            {
                base.SubmitChanges(ConflictMode.ContinueOnConflict);
            }
            catch (ChangeConflictException exc)
            {
                ResolveConflict(exc);
            }
            catch (Exception exc)
            {
                throw exc;
            }

            PostProcessChangeSet(changes);
        }

        private void ResolveConflict(ChangeConflictException exc)
        {
            StringBuilder message = new StringBuilder("The field, which you have modified was recently updated by another user.\n\n");

            bool conflictOnField = false;

            foreach (ObjectChangeConflict occ in ChangeConflicts)
            {
                MetaTable metatable = Mapping.GetTable(occ.Object.GetType());
                message.AppendFormat("Table name: {0}\n\n", metatable.TableName.Replace(".dbo", String.Empty));

                foreach (MemberChangeConflict mcc in occ.MemberConflicts)
                {
                    object currVal = mcc.CurrentValue;
                    object origVal = mcc.OriginalValue;
                    object databaseVal = mcc.DatabaseValue;
                    MemberInfo mi = mcc.Member;
                    message.AppendFormat("Member: {0}\n", mi.Name);
                    message.AppendFormat("Current value: {0} (your new value)\n", currVal);
                    message.AppendFormat("Original value: {0} (value you are changing from)\n", origVal);
                    message.AppendFormat("Database value: {0} (value, that another user changed to)\n\n", databaseVal);

                    if (!Equals(currVal, origVal) && //only if I modified the value
                        !Equals(databaseVal, currVal) && //only if my modification is different from concurent user's modification
                        mi.Name != "ModifiedBy" && mi.Name != "ModifiedDate")
                    {
                        conflictOnField = true;
                    }
                }

                if (conflictOnField)
                {
                    occ.Resolve(RefreshMode.OverwriteCurrentValues);
                }
                else
                {
                    occ.Resolve(RefreshMode.KeepChanges);
                }
            }

            SubmitChanges();

            throw new LorealChangeConflictException(message.ToString(), exc, conflictOnField);
        }

        /// <summary>
        /// Runs before SubmitChanges is called. 
        /// Assignes ModifiedBy and ModifiedDate properties for tables, that implementes ITrackChanges interface
        /// </summary>
        /// <param name="changes"></param>
        protected void PreProcessChangeSet(ChangeSet changes)
        {
            IList<Object> inserted = changes.Inserts;
            IList<Object> updated = changes.Updates;

            if (inserted != null)
            {
                foreach (var insertedRecord in inserted)
                {
                    ITrackChanges trackChanges = insertedRecord as ITrackChanges;
                    if (trackChanges != null)
                    {
                        trackChanges.ModifiedBy = LoggedUser.IsLogged
                                                      ? LoggedUser.GetInstance().LoginName
                                                      : String.Empty;
                        trackChanges.ModifiedDate = DateTime.Now;
                    }

                    IPrimaryKey key = insertedRecord as IPrimaryKey;
                    if (key == null)
                    {
                        // throw new ArgumentException("All entities must implement IPrimaryKey interface");
                    }
                    else
                    {
                        key.ID = Guid.NewGuid();
                    }
                }
            }

            if (updated != null)
            {
                foreach (var updatedRecord in updated)
                {
                    ITrackChanges dataObjectEntity = updatedRecord as ITrackChanges;
                    if (dataObjectEntity != null)
                    {
                        dataObjectEntity.ModifiedBy = LoggedUser.IsLogged
                                                      ? LoggedUser.GetInstance().LoginName
                                                      : String.Empty;
                        dataObjectEntity.ModifiedDate = DateTime.Now;
                    }
                }

            }
        }

        /// <summary>
        /// Runs after SubmitChanges is called. 
        /// Inserts new record into HistoryLog table if some record where deleted
        /// </summary>
        /// <param name="changes"></param>
        protected void PostProcessChangeSet(ChangeSet changes)
        {
            IList<Object> deleted = changes.Deletes;

            try
            {
                foreach (object deletedRecord in deleted)
                {
                    ITrackChanges dataObjectEntity = deletedRecord as ITrackChanges;
                    if (dataObjectEntity == null)
                    {
                        continue;
                    }

                    MetaTable meta = Mapping.GetTable(deletedRecord.GetType());

                    HistoryLog historyLog = new HistoryLog();
                    historyLog.TableName = meta.TableName.Replace("dbo.", String.Empty);
                    historyLog.TypeOfUpdate = "Delete";

                    historyLog.KeyValue = dataObjectEntity.ID;

                    historyLog.ModifiedBy = LoggedUser.IsLogged ? LoggedUser.GetInstance().LoginName : "Nobody logged";
                    historyLog.ModifiedDate = DateTime.Now;

                    DbDataContext context = new DbDataContext();

                    context.HistoryLogs.InsertOnSubmit(historyLog);
                    context.SubmitChanges();

                }
            }
            catch (SqlException e)
            {
                Logger.Log("Cannot add new record to the HistoryLog:" + e.Message, LogLevel.Error, LogFilePrefix.Db);
            }
        }

        [global::System.Data.Linq.Mapping.FunctionAttribute(Name = "dbo.uf_allocate_animationID")]
        public Guid? uf_allocate_animationID([global::System.Data.Linq.Mapping.ParameterAttribute(DbType = "UniqueIdentifier")] System.Nullable<System.Guid> animationId, [global::System.Data.Linq.Mapping.ParameterAttribute(DbType = "Bit")] System.Nullable<bool> loggingEnabled)
        {
            IEnumerable<Guid> resultCollection = this.ExecuteQuery<Guid>("exec uf_allocate_animationID {0}, {1}", animationId, loggingEnabled);

            object result = resultCollection.FirstOrDefault();

            if (result == null)
            {
                return null;
            }

            Guid value = Guid.Empty;

            try
            {
                value = new Guid(result.ToString());

                return value;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
