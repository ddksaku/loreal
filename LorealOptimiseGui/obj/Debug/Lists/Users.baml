   M S B A M L   `   `   `  ����a  ]DevExpress.Xpf.Grid.v11.1, Version=11.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a\Dclr-namespace:DevExpress.Xpf.Grid;assembly=DevExpress.Xpf.Grid.v11.1DevExpress.Xpf.Grid  5      a ]DevExpress.Xpf.Core.v11.1, Version=11.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49atPclr-namespace:DevExpress.Xpf.Editors.Settings;assembly=DevExpress.Xpf.Core.v11.1DevExpress.Xpf.Editors.Settings 5      L HLorealOptimiseGui, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullA%clr-namespace:LorealOptimiseGui.ListsLorealOptimiseGui.Lists 5      G(clr-namespace:LorealOptimiseGui.ControlsLorealOptimiseGui.Controls 5      M ILorealOptimiseData, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullS<clr-namespace:LorealOptimiseData;assembly=LorealOptimiseDataLorealOptimiseData 5      Q MLorealOptimiseBusiness, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullkJclr-namespace:LorealOptimiseBusiness.Lists;assembly=LorealOptimiseBusinessLorealOptimiseBusiness.Lists 5	      5clr-namespace:LorealOptimiseGuiLorealOptimiseGui 5
      #   LorealOptimiseGui.Lists.Users   5   (   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R	 NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml 	    5      TdxgDclr-namespace:DevExpress.Xpf.Grid;assembly=DevExpress.Xpf.Grid.v11.1 	    5      `dxePclr-namespace:DevExpress.Xpf.Editors.Settings;assembly=DevExpress.Xpf.Core.v11.1 	    5      7local%clr-namespace:LorealOptimiseGui.Lists 	    5      =controls(clr-namespace:LorealOptimiseGui.Controls 	    5      [LorealOptimiseData<clr-namespace:LorealOptimiseData;assembly=LorealOptimiseData 	    5      \ListsJclr-namespace:LorealOptimiseBusiness.Lists;assembly=LorealOptimiseBusiness 	    5	      =LorealOptimiseGuiclr-namespace:LorealOptimiseGui 	    5
      .��5      �   � RowDefinitions  5   
   �� 5      $��30 �6   �� 5      $��30 �6   �� 5      $��* �6    � ColumnDefinitions 5   
   �� 5      $	��150 �6   �� 5      $	��150 �6   �� 5      $
��.80* �6   �� 5      $��* �6   .+�5   
   �� -   5        $��lblTitle��5   $   $��3��6   $��User and Role Maintenance��64   $��2q�6X   $
��Left=�6c   6�   �� 5   
   -   5        $��lblChooseUser��5      $��1��6%   $
��Left=�62   $��Center=�6T   $��14(�6v   .��6�   Enter user name:6�   �� 5   
   -   5        $��txtUser��5      $��1��6f   $��1��6v   $��Center=�6�   ��6!   ��  �� Path$ 	LoginName� �� UpdateSourceTrigger$ PropertyChanged=�.��6�   5      �� 5   
   -   5        $��
btnAddUser��5   .   $��2��6   $��1��6!   $	��120��6b   $
��Left=�6n   $��2q�6�   ��6@   �� *InsertCommand+.��6�   
Add User6�   �� 5   
   -   5        $��lblAssignRoles��5   -   $��3��6   $��1��6    $��Center=�6C   $��Center=�6e   $��14(�6�   .��6�   !Assign Roles (to selected user)6�   %   DevExpress.Xpf.Grid.GridControl  5   
   -   5        $��grdUsers��5   ;   $��3��6   $��2��6.      
DataSource 6K   �� *Users+   Columns 5      $   DevExpress.Xpf.Grid.GridColumn  5      f
 bDevExpress.Xpf.Grid.v11.1.Core, Version=11.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a$ 
 DevExpress.Xpf.Grid.ColumnBase   	FieldName$ 	LoginName��6!      Header)  #DevExpress.Xpf.Core.ObjectConverter$ 	User Name 67      AllowEditing] YDevExpress.Data.v11.1, Version=11.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a5  /DevExpress.Utils.Design.DefaultBooleanConverter$ False 6J     5       $
 Name��6!   $ False 62     5!      $ EmailAddress��6!   $ False 6:   	   View	 5#      #   DevExpress.Xpf.Grid.TableView  5$      -   5        
   	AutoWidth
 .    ShowAutoFilterRow .    ShowIndicator . & 
  DevExpress.Xpf.Grid.DataViewBase   
FocusedRow 5$   _   �� *SelectedUser+ �� Mode$ TwoWay=�6�   5&        5(   
   -   5        $��grdRoles��5(   7   $��4��6   $��2��6*    5)        5*      $ IDRole��6!   $
 Role 64   $
 True 6B      ReadOnly .     SortMode$ DisplayText=�6g      EditSettings 5+      :  4DevExpress.Xpf.Editors.Settings.ComboBoxEditSettings  5,      -	   5          Name$ cboRoles��5,   J   8	  2DevExpress.Xpf.Editors.Settings.ButtonEditSettings 	  IsTextEditable .  <
  6DevExpress.Xpf.Editors.Settings.LookUpEditSettingsBase 
  DisplayMember$
 Name��6\    
  ValueMember$ ID��6q   6�      DisplayTemplate 5.      �� 5/      .R�50      �� ��6(   �� *Data.RoleName+51      53        54      $ 
IDDivision��6!   $ Division 68   $
 True 6J    .  $ DisplayText=�6o    55        56      -
   5        $ cboDivisions��56   J    .  $
 Name��6`   $ ID��6u    	  AllowDefaultButton . 57       59      �� 5:      .R�5;      �� ��6(   �� *Data.DivisionName+5<      5>      	 5@        5A      -   5        
 .    NewItemRowPosition$	 Top=�5A   1   6�   5C      5D      5E      