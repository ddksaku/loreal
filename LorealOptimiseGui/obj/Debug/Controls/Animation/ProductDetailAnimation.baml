   M S B A M L   `   `   `  ����L  HLorealOptimiseGui, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullA%clr-namespace:LorealOptimiseGui.ListsLorealOptimiseGui.Lists  5      5clr-namespace:LorealOptimiseGuiLorealOptimiseGui  5      a ]DevExpress.Xpf.Grid.v11.1, Version=11.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a\Dclr-namespace:DevExpress.Xpf.Grid;assembly=DevExpress.Xpf.Grid.v11.1DevExpress.Xpf.Grid 5      a ]DevExpress.Xpf.Core.v11.1, Version=11.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49atPclr-namespace:DevExpress.Xpf.Editors.Settings;assembly=DevExpress.Xpf.Core.v11.1DevExpress.Xpf.Editors.Settings 5      Q MLorealOptimiseBusiness, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null_Dclr-namespace:LorealOptimiseBusiness;assembly=LorealOptimiseBusinessLorealOptimiseBusiness 5      7    1LorealOptimiseGui.Controls.ProductDetailAnimation   5   $   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      7local%clr-namespace:LorealOptimiseGui.Lists     5      =LorealOptimiseGuiclr-namespace:LorealOptimiseGui     5      TdxgDclr-namespace:DevExpress.Xpf.Grid;assembly=DevExpress.Xpf.Grid.v11.1     5      eSettingsPclr-namespace:DevExpress.Xpf.Editors.Settings;assembly=DevExpress.Xpf.Core.v11.1     5      gLorealOptimiseBusinessDclr-namespace:LorealOptimiseBusiness;assembly=LorealOptimiseBusiness     5      .��5	      � .+�5
   
   %  DevExpress.Xpf.Grid.GridControl  -   5        $��grdProductDetails��5
          AutoExpandAllGroups  . $��Stretch=�6�   $��Stretch=�6�      
DataSource 63   �� *$"Animation.ObservableProductDetails+   View 5      #  DevExpress.Xpf.Grid.TableView  5         CellNormalStylef	 bDevExpress.Xpf.Grid.v11.1.Core, Version=11.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a& 	  DevExpress.Xpf.Grid.DataViewBase   	CellStyle# [  6Z      
FocusedRow 6    �� *SelectedProductDetail+ �� Mode$ TwoWay=�6Z      Columns 5      $  DevExpress.Xpf.Grid.GridColumn  5      $ 	 DevExpress.Xpf.Grid.ColumnBase   VisibleIndex$ 1��6!      	FieldName$ ID��62   	   Header)  #DevExpress.Xpf.Core.ObjectConverter$	 ID 6A   
   Fixed$

 Left=�6M      Visible .  5        5      -   5          Name$ clmItemGroup��5      $ 1��6!   $% AnimationProduct.ItemGroup.Name��62   $	 
Item Group 6^   $

 Left=�6r      Width$ 90��6�      ReadOnly . 5        5      -   5        $ clmMaterialCode��5   s   $ 2��6!   $# AnimationProduct.MaterialCode��62   $	 Material Code 6\   $

 Left=�6�   $	 130��6�    . 5        5      -   5        $ clmProductDescription��5   p   $ 3��6!   $" AnimationProduct.Description��62   $	 Description 6[   $	 200��6�   $

 Left=�6�    .    BestFitArea$
 Rows=�6�      BestFitMaxRowCount$ 25��6�      BestFitMode$ Smart=�6�   5        5      -   5        $ clmSortOrder��5   m   $ 4��6!   $  AnimationProduct.SortOrder��62   $	 
Sort Order 6Y   $

 Left=�6�   $ 70��6�    . 5        5      -   5        $ clmSalesArea��5   J   $ IDSalesArea��6!   $	 Country 69   $ 90��6^      EditSettings 5       :  4DevExpress.Xpf.Editors.Settings.ComboBoxEditSettings  5!      -   5          Name$ cboSalesArea��5!   O   8  2DevExpress.Xpf.Editors.Settings.ButtonEditSettings   IsTextEditable .  <	  6DevExpress.Xpf.Editors.Settings.LookUpEditSettingsBase 	  DisplayMember$
 Name��6e    	  ValueMember$ ID��6z   6�   5#        5$      -   5        $ clmRRP��5$   !   $	 RRP��6/   $		 RRP 6?   $ 70��6L    5%      6
  0DevExpress.Xpf.Editors.Settings.SpinEditSettings
  5&       
  MinValue$ 0X�64   6  0DevExpress.Xpf.Editors.Settings.BaseEditSettings   DisplayFormat$ N2��6A   6  0DevExpress.Xpf.Editors.Settings.TextEditSettings   Mask$ N2��6T      MaskType$ Numeric=�6^   6s   5(        5)      $ 	ListPrice��6!   $	 
List Price 67    . $ 90��6[    5*      
  5+      $ 0X�64   $ N2��6A   $ N2��6T   $ Numeric=�6^   6s   5-        5.      $" AnimationProduct.Product.CIV��6!   $	 Cost Price (CIV) 6J    . $ 90��6t    5/      
  50      $ 0X�64   $ N2��6A   $ N2��6T   $ Numeric=�6^   6s   52        53      $ 	CostValue��6!   $	 Cost Value (CIV) 67    . $ 90��6a    54      
  55      $ 0X�64   $ N0��6A   $ N0��6T   $ Numeric=�6^   6s   57        58      $	 Maximum Capacity 6!   $ MaximumCapacity��6;    . $	 100��6g    59      
  5:      $ 0X�64    
  IsFloatValue .  6X   5<        5=      -	   5        $ clmBDCQuantity��5=   !   $ BDCQuantity��67   $	 BDC Quantity 6O   $ 90��6e    5>      
  5?      $ 0X�64    .  6X   5A        5B      -
   5        $ clmForecastProcQuantity��5B   !   $ ForecastProcQuantity��6@   $#	 Forecast Procurement Quantity 6a   $	 100��6�    5C      
  5D      $ 0X�64    .  6X   5F        5G      -   5        $ clmAllocationQuantity��5G   !   $ AllocationQuantity��6>   $	 Allocation Quantity 6]   $ 90��6z    5H      
  5I      $ 0X�64    .  6X   5K        5L      -   5        $ clmMarketingComments��5L   !   $ MarketingComments��6=   $	 Marketing Comments 6[   $	 120��6w    .   5M      6  0DevExpress.Xpf.Editors.Settings.MemoEditSettings  5N      66   5P        5Q      $ AllocationRemainder��6!   $	 Allocation Remainder 6A   $ 90��6_    .  5R      
  5S       .  6K   5U        5V      -   5        $ colAllocate��5V   .   $		 Fix 6!   $ 80��6A      CellTemplate 5W      �� 5X      .R�5Y      ��  ��Name$ btnOpenAllocation��63   -   5        $	��Fix��5Y   %   6p   5Z      5\      5_      5`      5a      