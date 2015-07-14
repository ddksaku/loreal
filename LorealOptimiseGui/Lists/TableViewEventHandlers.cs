using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Linq.Mapping;
using System.Text;
using System.ComponentModel;
using System.Reflection;
using System.Windows.Controls;
using System.Windows;
using System.Data.SqlClient;
using DevExpress.XtraEditors.DXErrorProvider;
using DevExpress.Xpf.Editors;
using DevExpress.Xpf.Grid;
using DevExpress.Xpf.Editors.Settings;
using LorealOptimiseBusiness.Lists;
using LorealOptimiseData;
using LorealOptimiseShared;
using LorealOptimiseShared.Logging;
using LorealOptimiseData.Exceptions;
using LorealOptimiseBusiness.Exceptions;

namespace LorealOptimiseGui.Lists
{
    public class TableViewEventHandlers<T> where T : IPrimaryKey
    {
        private IModify<T> listManager;
        private TableView tableView;
        private GridControl dataGrid;
        private bool isTabKeyDown = false;
        private object prevFocusedRow = null;
        
        private int newitemListSourceRowIndex = -1;

        private bool validate;

        public TableViewEventHandlers(GridControl gridControl, IModify<T> listManager)
        {
            this.listManager = listManager;
            this.tableView = gridControl.View as TableView;
            this.dataGrid = gridControl;
            validate = true;
        }

        public TableViewEventHandlers(GridControl gridControl, IModify<T> listManager, bool validate)
        {
            this.listManager = listManager;
            this.tableView = gridControl.View as TableView;
            this.dataGrid = gridControl;
            this.validate = validate;
        }

        public void AssignEvents(bool isAvailableOnlyForDivisionAdmin = false)
        {
            if (isAvailableOnlyForDivisionAdmin && LoggedUser.GetInstance().IsInRole(LorealOptimiseData.Enums.RoleEnum.DivisionAdmin) == false)
            {
                tableView.AllowEditing = false;
            }

            tableView.CellValueChanged += new CellValueChangedEventHandler(tableView_CellValueChanged);
            tableView.CellValueChanging += new CellValueChangedEventHandler(tableView_CellValueChanging);
            tableView.RowUpdated += new RowEventHandler(tableView_RowUpdated);
            tableView.PreviewKeyDown += new System.Windows.Input.KeyEventHandler(tableView_PreviewKeyDown);
            tableView.FocusedRowChanged += new FocusedRowChangedEventHandler(tableView_FocusedRowChanged);
            tableView.InitNewRow += new InitNewRowEventHandler(tableView_InitNewRow);
            tableView.ShownEditor += new EditorEventHandler(tableView_ShownEditor);
            tableView.MouseLeave += new System.Windows.Input.MouseEventHandler(tableView_MouseLeave);

            dataGrid.CustomRowFilter += new RowFilterEventHandler(dataGrid_CustomRowFilter);

            // stuff for best fit columns
            tableView.AllowBestFit = true;
            tableView.BestFitArea = BestFitArea.All;
            tableView.BestFitMode = DevExpress.Xpf.Core.BestFitMode.VisibleRows;
            tableView.Loaded += new System.Windows.RoutedEventHandler(tableView_Loaded);
            tableView.SizeChanged += new System.Windows.SizeChangedEventHandler(tableView_SizeChanged);

            if (tableView.AllowEditing == true)
            {
                foreach (PropertyInfo property in typeof(T).GetProperties())
                {
                    object[] attributes = property.GetCustomAttributes(false);
                    if (attributes.Length > 0 && attributes[0].GetType() == typeof(ColumnAttribute))
                    {
                        ColumnAttribute colAttribute = (attributes[0] as ColumnAttribute);
                        if (colAttribute.CanBeNull != true || colAttribute.DbType.ToUpper().Contains("NOT NULL"))
                        {
                            if (dataGrid.Columns.Any(col => col.FieldName == property.Name) == true)
                            {
                                dataGrid.Columns[property.Name].Validate += new GridCellValidationEventHandler(RequiredColumn_Validate);
                            }
                        }
                    }
                }

                tableView.ValidateRow +=new GridRowValidationEventHandler(tableView_ValidateRow);
            }
        }

        void tableView_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (isTabKeyDown && this.prevFocusedRow != null)
            {
                InsertOrUpdate((T)prevFocusedRow);
            }
        }

        void dataGrid_CustomRowFilter(object sender, RowFilterEventArgs e)
        {
            T entity = (T)dataGrid.GetRowByListIndex(e.ListSourceRowIndex);
            if (entity != null && entity.ID == Guid.Empty)
            {
                e.Visible = false;
                e.Handled = true;
                newitemListSourceRowIndex = e.ListSourceRowIndex;
            }
        }

        void tableView_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            //Tomas: Performance issue
            //if (bestfitWidth == 0)
            //{
            //    foreach (GridColumn col in dataGrid.Columns)
            //    {
            //        col.MinWidth = tableView.CalcColumnBestFitWidth(col);
            //        bestfitWidth += col.MinWidth;
            //    }
            //}

            //if (tableView.AutoWidth != false && bestfitWidth > tableView.ActualWidth)
            //    tableView.AutoWidth = false;
            //if (tableView.AutoWidth != true && bestfitWidth < tableView.ActualWidth)
            //    tableView.AutoWidth = true;
        }

        void tableView_SizeChanged(object sender, System.Windows.SizeChangedEventArgs e)
        {
            if (e.WidthChanged)
            {
                //if (tableView.AutoWidth != false &&  bestfitWidth > tableView.ActualWidth)
                //    tableView.AutoWidth = false;
                //if (tableView.AutoWidth != true &&  bestfitWidth < tableView.ActualWidth)
                //    tableView.AutoWidth = true;
            }
        }

        void tableView_ShownEditor(object sender, EditorEventArgs e)
        {
            if (typeof(T).GetProperties().Any(p => p.Name == e.Column.FieldName) == true)
            {
                PropertyInfo property = typeof(T).GetProperties().FirstOrDefault(p => p.Name == e.Column.FieldName);
                if (property != null && property.PropertyType == typeof(string))
                {
                    object[] attributes = property.GetCustomAttributes(false);
                    if (attributes.Length > 0 && attributes[0].GetType() == typeof(ColumnAttribute))
                    {
                        ColumnAttribute colAttribute = (attributes[0] as ColumnAttribute);

                        int maxLength = Utility.GetLengthLimit(colAttribute);
                        if (maxLength != int.MaxValue)
                        {
                            (e.Editor as TextEdit).MaskType = MaskType.RegEx;
                            (e.Editor as TextEdit).Mask = ".{0," + maxLength.ToString() + "}";
                        }
                    }
                }
            }
        }

        void tableView_ValidateRow(object sender, GridRowValidationEventArgs e)
        {
            T entity = (T)e.Row;
            if (entity != null)
            {
                string errorMessage;
                string firstInvalidColumn;
                if (validate && !Utility.IsValid(entity, out errorMessage, out firstInvalidColumn))
                {
                    tableView.FocusedRowHandle = e.RowHandle;
                    if (dataGrid.Columns.Any(c => c.FieldName == firstInvalidColumn))
                    {
                        tableView.FocusedColumn = dataGrid.Columns[firstInvalidColumn];
                        e.SetError(string.Format(errorMessage, tableView.FocusedColumn.HeaderCaption));
                    }
                    else
                    {
                        e.SetError(string.Format(errorMessage, firstInvalidColumn));
                    }

                    e.Handled = true;
                }
            }
        }

        void RequiredColumn_Validate(object sender, GridCellValidationEventArgs e)
        {
            if (isEmptyValue(e.Value))
                e.SetError("Field cannot be empty");
        }

        bool isEmptyValue(object value)
        {
            if (value != null && typeof(Guid) == value.GetType() && (Guid)value == Guid.Empty)
            {
                return true;
            }
            else
            {
                if (value == null || value.ToString().Trim().Length == 0)
                    return true;

            }

            return false;
        }

        void tableView_InitNewRow(object sender, InitNewRowEventArgs e)
        {
            if (dataGrid.Columns.Any(col => col.FieldName == "IDDivision") == true)
            {
                try
                {
                    dataGrid.SetCellValue(e.RowHandle, dataGrid.Columns["IDDivision"], LoggedUser.LoggedDivision.ID);
                }
                catch (Exception exc)
                {
                    //MessageBox.Show("An error occured when setting 'IDDivision' value for a new row:" + LorealOptimiseShared.Utility.GetExceptionsMessages(exc));
                    MessageBox.Show(SystemMessagesManager.Instance.GetMessage("TableViewExceptionNewRow", LorealOptimiseShared.Utility.GetExceptionsMessages(exc)));
                }
            }

        }

        void tableView_RowUpdated(object sender, RowEventArgs e)
        {
            if (e.Row != null)
            {
                //we are doing insert
                T entity = (T)e.Row;

                if (e.RowHandle == GridControl.NewItemRowHandle)
                {
                    if (newitemListSourceRowIndex != -1)
                    {
                        entity = (T)dataGrid.GetRowByListIndex(newitemListSourceRowIndex);
                    }
                }

                InsertOrUpdate(entity);
            }
        }

        void tableView_CellValueChanging(object sender, CellValueChangedEventArgs e)
        {
            // when the value is being changed, flag down
            isTabKeyDown = false;

            if (e.Row == null )
            {
                //we are doing insert. we want just modify the value in db during update
                prevFocusedRow = null;

                return;
            }

            if(((T)e.Row).ID == Guid.Empty)
            {
                //we are doing insert. we want just modify the value in db during update
                prevFocusedRow = null;

                if ((e.Row as ICleanEntityRef) != null)
                {
                    (e.Row as ICleanEntityRef).CleanEntityRef(e.Column.FieldName);
                }

                return;
            }
            
            T entity = (T)e.Row;

            //we can not user table_CellValueChanged for some kinds of column, because table_CellValueChanged is not called right after the value is changes, but when the row looses focus
            if (e.Column.EditSettings is CheckEditSettings
                || e.Column.EditSettings is ComboBoxEditSettings)
            {
                if ((entity as ICleanEntityRef) != null)
                {
                    (entity as ICleanEntityRef).CleanEntityRef(e.Column.FieldName);
                }

                // Commit changes => then CellValueChanged event will be raised
                tableView.CommitEditing();
            }
        }

        void tableView_CellValueChanged(object sender, CellValueChangedEventArgs e)
        {
            // if this event was raised by {Tab} key, refuse doing Insert or value was not changed
            if (!isTabKeyDown && (e.Value != e.OldValue))
            {
                if (e.Row != null)
                {
                    T entity = (T)e.Row;
                    if (entity.ID != Guid.Empty)
                    {
                        string errMessag = string.Empty;
                        if (Utility.IsValid(entity, out errMessag) == false)
                        {
                            return;
                        }

                        //we are doing Insert/Update
                        InsertOrUpdate(entity);
                    }
                }
            }
        }

        void tableView_FocusedRowChanged(object sender, FocusedRowChangedEventArgs e)
        {
            if (prevFocusedRow == tableView.FocusedRow)
                return;

            T entity = (T)prevFocusedRow;

            // remember the previous focused row 
            prevFocusedRow = tableView.FocusedRow;

            if (entity != null && isTabKeyDown == true)
            {
                //we are doing insert
                InsertOrUpdate(entity);
            }

            isTabKeyDown = false;

        }

        bool InsertOrUpdate(T entity)
        {
            bool exceptionOccured = true;

            try
            {
                listManager.InsertOrUpdate(entity);
                exceptionOccured = false;
                return true;
            }
            catch (SqlException sqlExc)
            {
                if (sqlExc.Number == 50000 && sqlExc.Class == 16)
                {
                    if (sqlExc.Errors.Count > 0)
                    {
                        MessageBox.Show(sqlExc.Errors[0].Message);
                    }
                    else
                    {
                        MessageBox.Show(sqlExc.Message);
                    }
                }
                else
                    //MessageBox.Show("An error occured when inserting an entity: " + Utility.GetExceptionsMessages(sqlExc));
                    MessageBox.Show(SystemMessagesManager.Instance.GetMessage("TableViewExceptionSql", Utility.GetExceptionsMessages(sqlExc)));
            }
            catch (LorealChangeConflictException exc)
            {
                if (exc.IsConflictOnField)
                {
                    MessageBox.Show(exc.Message);
                }
            }
            catch (LorealValidationException exc)
            {
                MessageBox.Show(exc.Message, "Validation failed");
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message);
            }

            if (exceptionOccured)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        void tableView_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Tab)
            {
                // if {Tab} key is down, flag up
                isTabKeyDown = true;
            }
            else if (e.Key == System.Windows.Input.Key.Enter)
            {
                // if {Enter} key is down, do Insert
                // it is needed for the case of clicking {Enter} on not even changed cell
                if (prevFocusedRow != null)
                {
                    //we are doing insert/update
                    tableView.CommitEditing();
                }
            }
            else if (e.Key == System.Windows.Input.Key.Delete && tableView.AllowEditing == true && tableView.IsEditing == false)
            {
                if(tableView.SelectedRows.Count > 0)
                {
                    // check if the entity can be deleted
                    IDeletionLimit deletionLimit = tableView.SelectedRows[0] as IDeletionLimit;
                    string reasonMsg;
                    string warning = string.Empty;
                    if (deletionLimit != null && deletionLimit.CanBeDeleted(out reasonMsg, out warning) == false)
                    {
                        System.Windows.MessageBox.Show(reasonMsg);
                        return;
                    }

                    if (warning != string.Empty)
                    {
                        if (MessageBox.Show(warning, "Warning", MessageBoxButton.YesNo) == MessageBoxResult.No)
                        {
                            return;
                        }
                    }

                    T entity = (T)tableView.SelectedRows[0];
                    tableView.DeleteRow(tableView.GetSelectedRowHandles()[0]);
                    prevFocusedRow = null;
                    listManager.Delete(entity);
                }

            }
        }
    }
}
