   M S B A M L   `   `   `  ����a  ]DevExpress.Xpf.Grid.v11.1, Version=11.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a\Dclr-namespace:DevExpress.Xpf.Grid;assembly=DevExpress.Xpf.Grid.v11.1DevExpress.Xpf.Grid  5      a ]DevExpress.Xpf.Core.v11.1, Version=11.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49atPclr-namespace:DevExpress.Xpf.Editors.Settings;assembly=DevExpress.Xpf.Core.v11.1DevExpress.Xpf.Editors.Settings 5      L HLorealOptimiseGui, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullA%clr-namespace:LorealOptimiseGui.ListsLorealOptimiseGui.Lists 5      M ILorealOptimiseData, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullS<clr-namespace:LorealOptimiseData;assembly=LorealOptimiseDataLorealOptimiseData 5	      Q MLorealOptimiseBusiness, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullkJclr-namespace:LorealOptimiseBusiness.Lists;assembly=LorealOptimiseBusinessLorealOptimiseBusiness.Lists 5
      '   !LorealOptimiseGui.Lists.BrandAxes   5      \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R	 NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml 	    5      TdxgDclr-namespace:DevExpress.Xpf.Grid;assembly=DevExpress.Xpf.Grid.v11.1 	    5      f
 bDevExpress.Xpf.Grid.v11.1.Core, Version=11.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a?grid2http://schemas.devexpress.com/winfx/2008/xaml/grid   
 5      \dxePclr-namespace:DevExpress.Xpf.Editors.Settings;assembly=DevExpress.Xpf.Core.v11.1   
 5      3local%clr-namespace:LorealOptimiseGui.Lists   
 5      WLorealOptimiseData<clr-namespace:LorealOptimiseData;assembly=LorealOptimiseData   
 5	      XListsJclr-namespace:LorealOptimiseBusiness.Lists;assembly=LorealOptimiseBusiness   
 5
        � 	Resources  5         CellCustomStyle  CellNormalStyle �� BasedOn �� 
TargetType. 
 (DevExpress.Xpf.Grid.CellContentPresenter �� Triggers �� Binding �� Path �� Value �� Property �� Value��%�   &	        7  �� 5   
   8   "  6S    5      �� 5       6   �� $ DataContext.Manual�	 False6J   .a�5      ��  � ��
 �����63   5      5      5      .��5      � 	 � RowDefinitions	 5   
   �� 5      $��30 �6   �� 5      $��* �6   
 � ColumnDefinitions
 5   
   �� 5      $��* �6   6+   �� 5      $��70 �6   6,   .+�5   
   �� -   5        $��lblTitle��5      $��0��6    $��2q�6-   $��
Brand/Axes��68   $
��Left=�6M   6j   �� 5       -   5        $��
btnRefresh��5    '   $��0��6
   $��1��6   $��Refresh��69   6b   %   DevExpress.Xpf.Grid.GridControl  5!   
   -   5          Name$ grdBrandAxes��5!   R   $��1��6   $��2��6'      
DataSource 6;   ��    Columns 5"      $   DevExpress.Xpf.Grid.GridColumn  5#      $ 
 DevExpress.Xpf.Grid.ColumnBase   	FieldName$ IDSignature��6!      Header)  #DevExpress.Xpf.Core.ObjectConverter$ 	Signature 69      EditSettings 5$      :  4DevExpress.Xpf.Editors.Settings.ComboBoxEditSettings  5%      -   5          Name$ cboSignatures��5%   J   8  2DevExpress.Xpf.Editors.Settings.ButtonEditSettings   IsTextEditable .  <  6DevExpress.Xpf.Editors.Settings.LookUpEditSettingsBase   DisplayMember$
 Name��6a      ValueMember$ ID��6v   6�   5'        5)      $
 Name��6!   $ 	Brand/Axe 62     5*      -   5        $
 Code��5*   !   62     5+      $ Brand��6!      	CellStyle# [  63    5,      7	  1DevExpress.Xpf.Editors.Settings.CheckEditSettings	  5-      62   5/        50      $ Manual��6!      ReadOnly . 64      View 52      #
   DevExpress.Xpf.Grid.TableView
  53      -   5         
  	AutoWidth .  
  NewItemRowPosition$	 Top=�53   1   6J   55      56      57      