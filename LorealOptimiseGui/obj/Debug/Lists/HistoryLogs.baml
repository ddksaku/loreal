   M S B A M L   `   `   `  ����a  ]DevExpress.Xpf.Grid.v11.1, Version=11.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a\Dclr-namespace:DevExpress.Xpf.Grid;assembly=DevExpress.Xpf.Grid.v11.1DevExpress.Xpf.Grid  5      a ]DevExpress.Xpf.Core.v11.1, Version=11.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49abGclr-namespace:DevExpress.Xpf.Editors;assembly=DevExpress.Xpf.Core.v11.1DevExpress.Xpf.Editors 5      L HLorealOptimiseGui, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullA%clr-namespace:LorealOptimiseGui.ListsLorealOptimiseGui.Lists 5      M ILorealOptimiseData, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullS<clr-namespace:LorealOptimiseData;assembly=LorealOptimiseDataLorealOptimiseData 5      Q MLorealOptimiseBusiness, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullkJclr-namespace:LorealOptimiseBusiness.Lists;assembly=LorealOptimiseBusinessLorealOptimiseBusiness.Lists 5	      )   #LorealOptimiseGui.Lists.HistoryLogs   5      \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R	 NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml 	    5      TdxgDclr-namespace:DevExpress.Xpf.Grid;assembly=DevExpress.Xpf.Grid.v11.1 	    5      WdxeGclr-namespace:DevExpress.Xpf.Editors;assembly=DevExpress.Xpf.Core.v11.1 	    5      7local%clr-namespace:LorealOptimiseGui.Lists 	    5      [LorealOptimiseData<clr-namespace:LorealOptimiseData;assembly=LorealOptimiseData 	    5      \ListsJclr-namespace:LorealOptimiseBusiness.Lists;assembly=LorealOptimiseBusiness 	    5	      .��5
      �   � RowDefinitions  5   
   �� 5      $��30 �6   �� 5      $��30 �6   �� 5      $��* �6    � ColumnDefinitions 5   
   �� 5      $��* �6   6+   �� 5      $��70 �6   6,   .+�5   
   �� -   5        $��lblTitle��5      $��0��6   $��History Logs��6-   $��2q�6D   $
��Left=�6O   �� 5      -   5        $��
btnRefresh��5   +   $��0��6   $��1��6   $��Refresh��6=   6f   � 5   
   $��1��6   $��2��6    5      �� 5      $
��Auto �6#   62   �� 5      $��* �6#   $��50��6-   6=   �� 5      $
��Auto �6#   62   �� 5      $��* �6#   $��50��6-   6=   �� 5      $
��Auto �6#   62   �� 5      $��* �6#   6/   �� 5      $	��100 �6#   61   .+�5       ��  �� Orientation$ 
Horizontal=�6   $��0��62   .+�5!      �� -   5        $��lblTableName��5!      $��
Table Name��6,   $��Center=�6A   $��Center=�6\   %  DevExpress.Xpf.Editors.TextEdit  5"      -   5        $��txtTableName��5"   +   $	��100��6   $��Center=�6?   $��2q�6Z   6e   5#      �� 5$      $ 
Horizontal=�6   $��2��62   .+�5%      �� -   5        $��lblFrom��5%      $
��From��6'   $��Center=�66   $��Center=�6Q   %  DevExpress.Xpf.Editors.DateEdit  5&      -   5        $��	startDate��5&   +   $	��100��6   $��2q�6<   5'      �� 5(      $ 
Horizontal=�6   $��4��62   .+�5)      �� -   5        $��lblTill��5)      $
��Till��6'   $��Center=�66   $��Center=�6Q     5*      -   5        $��endDate��5*   +   $	��100��6   $��2q�6:   5+      �� 5,      -	   5        $��	btnFilter��5,   A   $��6��6   $��80��6%   $��Filter��60   $��Right=�6R   $��25��6�   6�   5-      %   DevExpress.Xpf.Grid.GridControl  5.   
   -
   5          Name$ grdHistoryLogs��5.   R   $��2��6   $��2��6'      
DataSource 6;   ��    Columns 5/      $   DevExpress.Xpf.Grid.GridColumn  50      f
 bDevExpress.Xpf.Grid.v11.1.Core, Version=11.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a$ 
 DevExpress.Xpf.Grid.ColumnBase   	FieldName$ 	TableName��6!     51      $ 	FieldName��6!     52      $ KeyValue��6!     53      $ TypeOfUpdate��6!     54      $ NewValue��6!     55      $ OldValue��6!     56      $ 
ModifiedBy��6!     57      $ ModifiedDate��6!      View 59      #   DevExpress.Xpf.Grid.TableView  5:         	AutoWidth . 	   NewItemRowPosition$
	 None=�61   & 
  DevExpress.Xpf.Grid.DataViewBase
   AllowEditing
 .  6K   5<      5=      5>      