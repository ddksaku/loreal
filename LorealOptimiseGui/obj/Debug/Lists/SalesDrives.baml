   M S B A M L   `   `   `  ����a  ]DevExpress.Xpf.Grid.v11.1, Version=11.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a\Dclr-namespace:DevExpress.Xpf.Grid;assembly=DevExpress.Xpf.Grid.v11.1DevExpress.Xpf.Grid  5      a ]DevExpress.Xpf.Core.v11.1, Version=11.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49atPclr-namespace:DevExpress.Xpf.Editors.Settings;assembly=DevExpress.Xpf.Core.v11.1DevExpress.Xpf.Editors.Settings 5      L HLorealOptimiseGui, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullA%clr-namespace:LorealOptimiseGui.ListsLorealOptimiseGui.Lists 5      M ILorealOptimiseData, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullS<clr-namespace:LorealOptimiseData;assembly=LorealOptimiseDataLorealOptimiseData 5      Q MLorealOptimiseBusiness, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullkJclr-namespace:LorealOptimiseBusiness.Lists;assembly=LorealOptimiseBusinessLorealOptimiseBusiness.Lists 5	      )   #LorealOptimiseGui.Lists.SalesDrives   5      \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R	 NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml 	    5      TdxgDclr-namespace:DevExpress.Xpf.Grid;assembly=DevExpress.Xpf.Grid.v11.1 	    5      `dxePclr-namespace:DevExpress.Xpf.Editors.Settings;assembly=DevExpress.Xpf.Core.v11.1 	    5      7local%clr-namespace:LorealOptimiseGui.Lists 	    5      [LorealOptimiseData<clr-namespace:LorealOptimiseData;assembly=LorealOptimiseData 	    5      \ListsJclr-namespace:LorealOptimiseBusiness.Lists;assembly=LorealOptimiseBusiness 	    5	      .��5
      �   � RowDefinitions  5   
   �� 5      $��30 �6   �� 5      $��* �6    � ColumnDefinitions 5   
   �� 5      $��* �6   6+   �� 5      $��70 �6   6,   .+�5   
   �� -   5        $��lblTitle��5      $��0��6   $��Sales Drives��6-   $��2q�6D   $
��Left=�6O   �� 5      -   5        $��
btnRefresh��5   '   $��0��6
   $��1��6   $��Refresh��69   6b   %   DevExpress.Xpf.Grid.GridControl  5   
   -   5          Name$ grdSalesDrives��5   R   $��1��6   $��2��6'      
DataSource 6;   ��    Columns 5      $   DevExpress.Xpf.Grid.GridColumn  5      f
 bDevExpress.Xpf.Grid.v11.1.Core, Version=11.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a$ 
 DevExpress.Xpf.Grid.ColumnBase   	FieldName$ 
IDDivision��6!      Header)  #DevExpress.Xpf.Core.ObjectConverter$ Division 68      EditSettings 5      :  4DevExpress.Xpf.Editors.Settings.ComboBoxEditSettings  5      -   5          Name$ cboDivisions��5   J   8  2DevExpress.Xpf.Editors.Settings.ButtonEditSettings	   IsTextEditable	 .  <  6DevExpress.Xpf.Editors.Settings.LookUpEditSettingsBase
   DisplayMember$

 Name��6`      ValueMember$ ID��6u   6�   5        5      $
 Name��6!     5      -   5          Name$ colYear��5   !   $
 Year��60    5      6  0DevExpress.Xpf.Editors.Settings.TextEditSettings  5          MaskType$ RegEx=�6/      Mask$ [1-9][0-9]{3}��6@   6	  0DevExpress.Xpf.Editors.Settings.BaseEditSettings! 	  HorizontalContentAlignment$
 Left=�6U   5"        5#      -   5        $ colButtonReport��5#   -      Width$	 120��6!   $ Animations Report��6D      CellTemplate 5$      �� 5%      .R�5&      ��  ��Name$ 	btnReport��6%   -   5        $��Report��5&   8   $��0��6I   $��3,3,3,3q�6n   5'      5)         View 5+      #
   DevExpress.Xpf.Grid.TableView
  5,      -   5         
  	AutoWidth .   
  NewItemRowPosition$	 Top=�5,   2   6K   5.      5/      50      