using System;
using System.Data;
using System.Data.SqlClient;
using Microsoft.SqlServer.Server;


public partial class Triggers
{
    [SqlTrigger(Name = "HistoryLogTrigger", Event = "FOR INSERT, UPDATE, DELETE")]
    public static void HistoryLogTrigger()
    {
            string alteredTableName = String.Empty; //Where we store the Altered Table's Name
            string databaseUserName = String.Empty; //Where we will store the Database Username
            string primaryKeyString = String.Empty; //Will temporarily store the Primary Key Column Names and Values here

            DataRow insertedRow; //DataRow to hold the inserted values
            DataRow modifiedRow; //DataRow to how the deleted/overwritten values
            DataRow auditRow; //Audit DataRow to build our Audit entry with

            DataTable auditTable;
            DataTable insertedTable;
            DataTable deletedTable;
            DataTable primaryKeyTable;

            SqlTriggerContext sqlTriggerContext = SqlContext.TriggerContext; //Trigger Context
            SqlDataAdapter auditDataAdapter;
            SqlDataAdapter sqlDataAdapter;
            SqlDataAdapter primaryKeyTableAdapter;


            using (SqlConnection sqlConnection = new SqlConnection("context connection=true"))
            {
                sqlConnection.Open();


                //Get the inserted values
                sqlDataAdapter = new SqlDataAdapter("SELECT * from INSERTED", sqlConnection);
                insertedTable = new DataTable();
                sqlDataAdapter.Fill(insertedTable);

                //Get the deleted and/or overwritten values
                sqlDataAdapter.SelectCommand.CommandText = "SELECT * from DELETED";
                deletedTable = new DataTable();
                sqlDataAdapter.Fill(deletedTable);

                if (insertedTable.Rows.Count == 0 && deletedTable.Rows.Count == 0)
                {
                    //No value was updated/inserted/deleted
                    return;
                }


                auditDataAdapter = new SqlDataAdapter("SELECT * FROM HistoryLog WHERE 1=0", sqlConnection);

                auditTable = new DataTable();
                auditDataAdapter.FillSchema(auditTable, SchemaType.Source);
                SqlCommandBuilder auditCommandBuilder = new SqlCommandBuilder(auditDataAdapter);//Populates the Insert command for us

                //First try to get table name using 'hardcoded' way as getting table name from sys.dm_tran_locks does not work in all cases.
                alteredTableName = GetTableName(insertedTable);

                //Retrieve the Name of the Table that currently has a lock from the executing command(i.e. the one that caused this trigger to fire)
                //If you get an exception here, try to run "GRANT VIEW SERVER STATE TO [LoginName]"
                //It can occur if you do not run the trigger as db admin 
                if (alteredTableName == String.Empty)
                {
                    try
                    {

                        SqlDataAdapter sqlCommand = new SqlDataAdapter("SELECT object_name(resource_associated_entity_id), request_mode FROM sys.dm_tran_locks WHERE request_session_id = @@spid and resource_type = 'OBJECT' and request_mode != 'Sch-M'", sqlConnection);

                        DataTable lockedTables = new DataTable();
                        sqlCommand.Fill(lockedTables);

                        //Issue: on the period group admin page user inserted more than one calculation period to any period group row. First insert was logged correctly, but the second one had TableName='HistoryLog'
                        //Problem seems to be related to above command, which should get table name of inserting row. But in some cases (more active transactions??), it contains also HistoryLog record.
                        //Maybe there is better way how to find out the name of the table, which causes trigger to fire.
                        foreach (DataRow dr in lockedTables.Rows)
                        {
                            if (dr[0] == null)
                            {
                                continue;
                            }

                            if (dr[0].ToString().ToLower() == "historylog")
                            {
                                continue;
                            }

                            if (dr[0].ToString() != String.Empty)
                            {
                                alteredTableName = dr[0].ToString();
                            }
                        }
                    }
                    catch (SqlException exc)
                    {
                        throw new Exception("Exception while getting objects from sys.dm_tran_lock. Run 'GRANT VIEW SERVER STATE TO [LoginName]' for LoginName, who executes the CLR assembly. Detail: " + exc.ToString(), exc);
                    }
                }

                primaryKeyTableAdapter = new SqlDataAdapter(@"SELECT c.COLUMN_NAME from INFORMATION_SCHEMA.TABLE_CONSTRAINTS pk ,INFORMATION_SCHEMA.KEY_COLUMN_USAGE c where pk.TABLE_NAME = '" + alteredTableName + @"'and CONSTRAINT_TYPE = 'PRIMARY KEY'and c.TABLE_NAME = pk.TABLE_NAME and c.CONSTRAINT_NAME = pk.CONSTRAINT_NAME", sqlConnection);
                primaryKeyTable = new DataTable();
                primaryKeyTableAdapter.Fill(primaryKeyTable);

                if (insertedTable.Rows.Count == 0 && deletedTable.Rows.Count == 0)
                {
                    return;
                }

                if (insertedTable.Rows.Count > 0)
                {
                    primaryKeyString = PrimaryKeyStringBuilder(primaryKeyTable, insertedTable.Rows[0]);//the the Primary Keys and There values as a string
                }
                else
                {
                    primaryKeyString = PrimaryKeyStringBuilder(primaryKeyTable, deletedTable.Rows[0]);//the the Primary Keys and There values as a string
                }

                switch (sqlTriggerContext.TriggerAction)//Switch on the Action occuring on the Table
                {

                    case TriggerAction.Update:

                        if (insertedTable.Rows.Count == 0 || deletedTable.Rows.Count == 0)
                        {
                            break;
                        }

                        insertedRow = insertedTable.Rows[0];//Get the inserted values in row form
                        modifiedRow = deletedTable.Rows[0];//Get the overwritten values in row form

                        foreach (DataColumn column in insertedTable.Columns)//Walk through all possible Table Columns
                        {
                            if (column.ColumnName.ToLower() == "modifiedby" || column.ColumnName.ToLower() == "modifieddate")
                            {
                                continue;
                            }

                            if (!insertedRow[column.Ordinal].Equals(modifiedRow[column.Ordinal]))//If value changed
                            {
                                //Build an Audit Entry
                                auditRow = auditTable.NewRow();

                                FillAuditRow(auditRow, "Update", alteredTableName, insertedRow, modifiedRow, column, primaryKeyString);

                                auditTable.Rows.InsertAt(auditRow, 0);//Insert the entry
                            }
                        }
                        break;

                    case TriggerAction.Insert:
                        #region Insert

                        if (insertedTable.Rows.Count == 0)
                        {
                            break;
                        }

                        insertedRow = insertedTable.Rows[0];


                        foreach (DataColumn column in insertedTable.Columns)//Walk through all possible Table Columns
                        {
                            if (column.ColumnName.ToLower() == "modifiedby" || column.ColumnName.ToLower() == "modifieddate")
                            {
                                continue;
                            }

                            auditRow = auditTable.NewRow();

                            FillAuditRow(auditRow, "Insert", alteredTableName, insertedRow, null, column, primaryKeyString);

                            auditTable.Rows.InsertAt(auditRow, 0);//Insert the Entry
                        }

                        break;
                        #endregion

                    case TriggerAction.Delete:

                        if (deletedTable.Rows.Count == 0)
                        {
                            break;
                        }

                        modifiedRow = deletedTable.Rows[0]; //Get the deleted row 
                        foreach (DataColumn column in deletedTable.Columns) //Walk through all possible Table Columns
                        {
                            if (column.ColumnName.ToLower() == "modifiedby" || column.ColumnName.ToLower() == "modifieddate")
                            {
                                continue;
                            }


                            //Build an Audit Entry
                            auditRow = auditTable.NewRow();
                            FillAuditRow(auditRow, "Delete", alteredTableName, modifiedRow, modifiedRow, column, primaryKeyString);
                            auditTable.Rows.InsertAt(auditRow, 0);//Insert the entry

                        }
                        break;

                    default:

                        break;
                }

                auditDataAdapter.Update(auditTable);//Write all Audit Entries back to AuditTable
                sqlConnection.Close(); //Close the Connection
            }
        }
    

    private static void FillAuditRow(DataRow auditRow, string typeofUpdate, string tableName, DataRow newValues, DataRow oldValues, DataColumn columnToLog, string primaryKey)
    {
        string modifiedBy = GetModifiedByValue(newValues);

        auditRow["ID"] = Guid.NewGuid();
        auditRow["TypeOfUpdate"] = typeofUpdate;
        auditRow["TableName"] = tableName.Length > 50 ? tableName.Substring(0, 50) : tableName;
        auditRow["KeyValue"] = primaryKey;
        auditRow["ModifiedDate"] = DateTime.Now;
        auditRow["ModifiedBy"] = modifiedBy.Length > 50 ? modifiedBy.Substring(0,50) : modifiedBy;

        auditRow["FieldName"] = columnToLog.ColumnName;
        if (newValues[columnToLog.Ordinal].ToString() != string.Empty && newValues[columnToLog.Ordinal] != DBNull.Value)
        {
            auditRow["NewValue"] = newValues[columnToLog.Ordinal].ToString();
        }

        if (oldValues != null)
        {
            if (oldValues[columnToLog.Ordinal].ToString() != string.Empty && oldValues[columnToLog.Ordinal] != DBNull.Value)
            {
                auditRow["OldValue"] = oldValues[columnToLog.Ordinal].ToString();
            }
        }
    }

    private static string GetModifiedByValue(DataRow newValues)
    {
        string modifiedByField = "DirectAccess";

        //Get infromation about ChangedBy
        foreach (DataColumn tablecolumn in newValues.Table.Columns)
        {
            if (tablecolumn.ColumnName.ToLower() == "modifiedby")
            {
                if (newValues[tablecolumn.Ordinal] == null || newValues[tablecolumn] == DBNull.Value)
                {
                    return modifiedByField;
                }

                modifiedByField = newValues[tablecolumn.Ordinal].ToString();

                return modifiedByField;
            }
        }

        return modifiedByField;
    }

    public static string PrimaryKeyStringBuilder(DataTable primaryKeysTable, DataRow valuesDataRow)
    {
        if (valuesDataRow == null)
        {
            return String.Empty;
        }

        string correctName = String.Empty;

        foreach (DataRow keyColumn in primaryKeysTable.Rows)
            correctName = valuesDataRow[keyColumn[0].ToString()].ToString();

        if (String.IsNullOrEmpty(correctName) == false)
        {
            return correctName;
        }

        //in some cases (user running the trigger does not have rights to access master views), primaryKeysTable can be null. We try to look for ID column and expect it contains ID value
        if (valuesDataRow.Table.Columns.Contains("ID") && valuesDataRow["ID"] != null)
        {
            return valuesDataRow["ID"].ToString();
        }

        return String.Empty;
    }

    /// <summary>
    /// We try to get table name using this "hardcoded"  way. This is probably the only 100% working way how to get table name from trigger. Previsou way did not work in all ocassions (when more than one table was locked - cascade update, ...)
    /// </summary>
    /// <param name="insertedTable"></param>
    /// <returns></returns>
    private static string GetTableName(DataTable insertedTable)
    {
        if (insertedTable == null || insertedTable.Columns.Count == 0)
        {
            return String.Empty;
        }

        foreach (DataColumn dc in insertedTable.Columns)
        {
            if (dc.ColumnName == "DefaultCustomerReference")
            {
                return "Animation";
            }
            else if (dc.ColumnName == "BDCBookNumber")
            {
                return "AnimationProduct";
            }
            else if (dc.ColumnName == "BDCQuantity")
            {
                return "AnimationProductDetail";
            }
            else if (dc.ColumnName == "IDRetailerType")
            {
                return "AnimationCustomerGroup";
            }
            else if (dc.ColumnName == "CachedCapacity")
            {
                return "CustomerAllocation";
            }
            else if (dc.ColumnName == "ManualFixedAllocation")
            {
                return "CustomerGroupAllocation";
            }
            else if (dc.ColumnName == "Capacity")
            {
                return "Capacity";
            }
        }

        return String.Empty;
    }

}
