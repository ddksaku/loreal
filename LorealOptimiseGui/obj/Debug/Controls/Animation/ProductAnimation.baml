   M S B A M L   `   `   `  ����a  ]DevExpress.Xpf.Grid.v11.1, Version=11.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a\Dclr-namespace:DevExpress.Xpf.Grid;assembly=DevExpress.Xpf.Grid.v11.1DevExpress.Xpf.Grid  5      a ]DevExpress.Xpf.Core.v11.1, Version=11.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49abGclr-namespace:DevExpress.Xpf.Editors;assembly=DevExpress.Xpf.Core.v11.1DevExpress.Xpf.Editors 5      L HLorealOptimiseGui, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullA%clr-namespace:LorealOptimiseGui.ListsLorealOptimiseGui.Lists 5      G(clr-namespace:LorealOptimiseGui.ControlsLorealOptimiseGui.Controls 5      tPclr-namespace:DevExpress.Xpf.Editors.Settings;assembly=DevExpress.Xpf.Core.v11.1DevExpress.Xpf.Editors.Settings 5	      5clr-namespace:LorealOptimiseGuiLorealOptimiseGui 5
      O Kmscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e0891&clr-namespace:System;assembly=mscorlibSystem 5      1   +LorealOptimiseGui.Controls.ProductAnimation   5   $   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      TdxgDclr-namespace:DevExpress.Xpf.Grid;assembly=DevExpress.Xpf.Grid.v11.1     5      WdxeGclr-namespace:DevExpress.Xpf.Editors;assembly=DevExpress.Xpf.Core.v11.1     5      Gdxgt<http://schemas.devexpress.com/winfx/2008/xaml/grid/themekeys   5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      7local%clr-namespace:LorealOptimiseGui.Lists     5      =controls(clr-namespace:LorealOptimiseGui.Controls     5      eSettingsPclr-namespace:DevExpress.Xpf.Editors.Settings;assembly=DevExpress.Xpf.Core.v11.1     5	      =LorealOptimiseGuiclr-namespace:LorealOptimiseGui     5
      9System&clr-namespace:System;assembly=mscorlib     5        � 	Resources  5         HighlightRowStyle �� 
TargetType(   "DevExpress.Xpf.Grid.GridRowContent �� BasedOn9   3DevExpress.Xpf.Grid.Themes.GridRowThemeKeyExtension�  �DevExpress.Xpf.Utils.Themes.ThemeKeyExtensionInternalBase`1[[DevExpress.Xpf.Grid.Themes.GridRowThemeKeys, DevExpress.Xpf.Grid.v11.1, Version=11.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a]]   ResourceKey �� Triggers ^� 
Conditions �� Binding �� Path �� Value	 �� Property
 �� Value��%�   &	        0�� 5   *   *  $ RowStyle=�+1�� 6
   "  6q    6*   2   5      ^� 5       5      �� 5       6$   �� $! DataContext.IsProductManual�
 Manual6Y   .0�5      �� 	 � ��

 ����63   5      5      5      .��5      � $��Stretch=�6    � RowDefinitions 5   
   �� 5      $��30 �6   �� 5      $��* �6    � ColumnDefinitions 5    
   �� 5!      $��* �6   �� 5"      $��* �6   .+�5$   
   � -   5        $��buttons��5$      .+�5%      ��  �� Orientation$ 
Horizontal=�6   .+�5&      �� -   5        $��btnCreateProduct��5&      $ ��Create a new dummy product��61   $
��Left=�6u   $��2q�6�   6�   �� 5'      -   5        $��btnViewProducts��5'      $��View all products��60   $
��Left=�6j   $��2q�6�   6�   5(      5)      )  #DevExpress.Xpf.Editors.ComboBoxEdit  5*   
   -   5        $��cboView��5*      $	��100��6*   $��5q�66   $��1��6A   $��Right=�6Q   +  %DevExpress.Xpf.Editors.LookUpEditBase   SelectedIndex$ 0��6m      Items. 5+      -  'DevExpress.Xpf.Editors.ComboBoxEditItem  .��6#   All6(     5,      .��6#   Marketing View63     5-      .��6#   Logistics View63     5.      .��6#   	Summary6,   5/      %   DevExpress.Xpf.Grid.GridControl  52   
   -   5        $��
grdProduct��52      $��1��6,   $��2��69   $��Stretch=�6�   $��Stretch=�6�      
DataSource 6M   �� *Animation.ObservableProducts+ �� Mode$ TwoWay=�  53      ��%    55         Columns 56      $   DevExpress.Xpf.Grid.GridColumn  57      -   5        f	 bDevExpress.Xpf.Grid.v11.1.Core, Version=11.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a$	 	 DevExpress.Xpf.Grid.ColumnBase 	 Name$ clmItemGroup��57   k    	  VisibleIndex$ 1��6!    	  	FieldName$ IDItemGroup��62    	  Header)
  #DevExpress.Xpf.Core.ObjectConverter$ 
Item Group
 6J    	  Fixed$
 Left=�6^    	  Width$ 80��6    	  EditSettings 58      :  4DevExpress.Xpf.Editors.Settings.ComboBoxEditSettings  59      -   5          Name$ cboItemGroups��59   �   <  6DevExpress.Xpf.Editors.Settings.LookUpEditSettingsBase   AutoComplete . 8  2DevExpress.Xpf.Editors.Settings.ButtonEditSettings   IsTextEditable .    DisplayMember$
 Name��6b      ValueMember$ ID��6w   6�   5;        5=      -   5        $ clmMaterialCode��5=   b   $ 2��6!   $ MaterialCode��62   $ Material Code
 6K   $
 Left=�6y   $	 100��6�    5>        5?      -	   5        $ cboMaterialCodes��5?   b    .  . 6~   5A        5C      -
   5        $ clmProductDescription��5C   _   $ 3��6!   $ Description��62   $ Description
 6J   $	 100��6|   $
 Left=�6�    	  BestFitMode$ Smart=�6�     	  BestFitArea$
  Rows=�6�   ! 	  BestFitMaxRowCount$! 25��6�    5D        5E      -   5        $ cboDescriptions��5E   b    .  . 6}   5G        5H      -   5        $ clmSortOrder��5H   \   $ 4��6!   $ 	SortOrder��62   $ 
Sort Order
 6H   $
 Left=�6p   $ 70��6}   5I        5J      -   5        $ clmInternationCode��5J   c   $ Product.InternationalCode��6!   $ International Code
 6G   " 	  ReadOnly" . $	 100��6�   5K        5L      -   5        $ clmBDC��5L   ;   $ BDCBookNumber��6!   $	 BDC
 6I   $ 55��6V   5M        5N      -   5        $ clmEan��5N   V   $ Product.EAN��6!   $	 EAN
 69   " . $	 100��6d   5O        5P      -   5        $ 	clmStatus��5P   \   $ Product.Status��6!   $ Status
 6<   " . $ 80��6m   5Q        5R      -   5        $ 	clmSource��5R   h   $ IsProductManual��6!   $ 80��6=   $ Source
 6H   " . # 	  UnboundType]
 YDevExpress.Data.v11.1, Version=11.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a8 
 2DevExpress.Utils.Design.UnboundColumnTypeConverter$# String 6y   $ 	  UnboundExpression$*$ $Iif([Product.Manual],'Manual','SAP')��6�   5S        5T      -   5        $ clmForOrdering��5T   `   $ ForOrdering��6!   $ For Ordering?
 69   " . $ 80��6v   5U        5V      -   5        $ clmItemType��5V   W   $ 
IDItemType��6!   $ Allocate by Item Type
 68    5W        5X      -   5        $ cboItemTypes��5X   L    . $
 Name��6b   $ ID��6w   5Z        5[      -   5        $ clmSignature��5[   9   $ IDSignature��6!   $ Allocate by Signature
 6M    5\        5]      -   5        $ cboSignature��5]   b    .  . $
 Name��6x   $ ID��6�   5_        5a      -   5        $ clmBrandAxe��5a   8   $ 
IDBrandAxe��6!   $ Allocate by Brand/Axe
 6K    5b        5c      -   5        $ cboBrandAxes��5c   b    .  . $ FullName��6x   $ ID��6�   % 	  DisplayTemplate% 5e      �� 5f      .R�5g      �� $��3q�6K   $��NoWrap=�6V   ��6(   �� *Data.BrandAxeName+6V   5h      5j        5k      -   5        $ clmCategory��5k   8   $ 
IDCategory��6!   $  Allocate by Store Category
 6K    5l        5m      -   5        $ cboCategories��5m   b    .  . $
 Name��6y   $ ID��6�   5o        5p      -   5        $ clmOnCas��5p   3   $ OnCAS��6!   $ 80��6C   $ On CAS?
 6N    5q      7  1DevExpress.Xpf.Editors.Settings.CheckEditSettings  5r      67   5t        5u      -   5        $ clmNormalMultiple��5u   !   $ IDMultipleNormal��6:   $ 80��6W   $ Normal Multiple
 6b   & 	  ColumnFilterMode$& DisplayText=�6{    5v        5w      -   5        $ cboNormalMultiple��5w   b    .  . $ ValueString��6}   $ ID��6�   % 5y      �� 5z      .R�5{      �� $��3q�6M   $��NoWrap=�6X   ��6(   �� *Data.NormalMultiple+6X   5|      5~        5      -   5        $ clmWarehouseMultiple��5   !   $ IDMultipleWarehouse��6=   $ 90��6]   $ Warehouse Multiple
 6h   $& DisplayText=�6�    5�        5�      -   5        $ cboWarehouseMultiple��5�   b    .  . $ ValueString��6�   $ ID��6�   % 5�      �� 5�      .R�5�      �� $��3q�6P   $��NoWrap=�6[   ��6(   �� *Data.WarehouseMultiple+6[   5�      5�        5�      -    5        $ clmTotalCapacity��5�   !   $ TotalCapacity��69   " . $ 90��6c   $ Total Capacity
 6n    5�      6  0DevExpress.Xpf.Editors.Settings.SpinEditSettings  5�      '   MinValue$' 0X�64   (   IsFloatValue( .  6X   5�        5�      -!   5        $ clmTotalBDCQuantity��5�   !   $ TotalBDCQuantity��6<   " . $ 90��6i   $ Total BDC Quantity
 6t   5�        5�      -"   5        $ clmTotalForecast��5�   !   $ TotalForecastQuantity��69   " . $	 100��6k   $) #Total Forecast Procurement Quantity
 6w   5�        5�      -#   5        $ clmTotalAllocation��5�   !   $ TotalAllocationQuantity��6;   " . $	 110��6o   $) #Total Allocation Quantity Available
 6{   5�        5�      -$   5        $ clmAllocationQuantityUK��5�   !   " . $ AllocationQuantityUK��6P   $ 90��6q   $ Allocation Quantity UK
 6|   5�        5�      -%   5        $ clmAllocationQuantityROI��5�   !   " . $ AllocationQuantityROI��6Q   $ 90��6s   $ Allocation Quantity ROI
 6~   5�        5�      -&   5        $ clmTotalCostValue��5�   !   $ TotalCostValue��6:   " . $ 80��6e   $ Total Cost Value
 6p    5�      6  0DevExpress.Xpf.Editors.Settings.TextEditSettings  5�      6  0DevExpress.Xpf.Editors.Settings.BaseEditSettings)   DisplayFormat$) N0��64   5�        5�      -'   5        $ 	clmUKList��5�   !   " . $ UKListPrice��6B   $ 55��6Z   $ UK List
 6e    5�        5�      $) N2��64   *   Mask$* N2��6G   +   MaskType$+ Numeric=�6Q   5�        5�      -(   5        $ 
clmROIList��5�   !   " . $ ROIListPrice��6C   $ 60��6\   $ ROI List
 6g    5�        5�      $) N2��64   5�        5�      -)   5        $ clmUKRrp��5�   !   " . $ UKRrp��6A   $ 60��6S   $ UK RRP
 6^    5�        5�      $) N2��64   5�        5�      -*   5        $ 	clmROIRrp��5�   !   " . $ ROIRrp��6B   $ 60��6U   $ ROI RRP
 6`    5�        5�      $) N2��64   5�        5�      -+   5        $ clmMarketingComments��5�   !   $ MarketingComments��6=   $	 100��6[   $ Marketing Comments
 6g    5�      6  0DevExpress.Xpf.Editors.Settings.MemoEditSettings  5�      ,   ShowIcon, .   -   ShowTooltipForTrimmedText- . 6h   5�        5�      -,   5        $ clmProcurementType��5�   U   $ Product.ProcurementType��6!   " . $ Procurement Type
 6o   $ 95��6�   5�        5�      --   5        $ clmMadMonth��5�   E   $ ConfirmedMadMonthString��6!   $ Confirmed MAD Month
 6X   $ 95��6u    5�        5�      -.   5        $ cboMadMonth��5�   b    .  . 6y   5�        5�      -/   5        $ clmStockRisk��5�   7   $ 	StockRisk��6!   $ 
Stock Risk
 6K   $ 70��6_    5�        5�      67   5�        5�      -0   5        $ clmDeliveryRisk��5�   :   $ DeliveryRisk��6!   $ Delivery Risk
 6Q   $ 70��6h    5�        5�      67   5�        5�      -1   5        $ clmActiveAnimations��5�   !   $ ActiveAnimations��6<   " . $ Active Animations
 6i   $ 85��6�    5�        5�      ( .  6K   5�        5�      -2   5        $ clmDuplicateProduct��5�   !   " . $ DuplicateProduct��6L   $ Duplicate Product
 6i   $ 80��6�   5�        5�      -3   5        $ clmTotalAnimationQuantity��5�   !   " . $ TotalAnimationQuantity��6R   $ Total Animation Quantity
 6u   $ 90��6�   5�        5�      -4   5        $ clmInTransit��5�   !   " . $ Product.InTransit��6E   $ 
In Transit
 6c   $ 70��6w   5�        5�      -5   5        $ clmConfirmedQuantity��5�   !   " . $ Product.ConfirmedQuantity��6M   $ Confirmed Quantity
 6s   $ 85��6�   5�        5�      -6   5        $ clmReliquat��5�   !   " . $ Product.Reliquat��6D   $ Reliquat
 6a   $ 70��6s   5�        5�      -7   5        $ 	clmMonth1��5�   !   " . $ Product.Month��6B   $ Current Month
 6\   $ 80��6s   5�        5�      -8   5        $ 	clmMonth2��5�   !   " . $ Product.Month1��6B   $ Month 1
 6]   $ 80��6n   5�        5�      -9   5        $ 	clmMonth3��5�   !   " . $ Product.Month2��6B   $ Month 2
 6]   $ 80��6n   5�        5�      -:   5        $ 	clmMonth4��5�   !   " . $ Product.Month3��6B   $ Month 3
 6]   $ 80��6n   5�        5�      -;   5        $ 	clmMonth5��5�   !   " . $ Product.Month4��6B   $ Month 4
 6]   $ 80��6n   5�        5�      -<   5        $ 	clmMonth6��5�   !   " . $ Product.Month5��6B   $ Month 5
 6]   $ 80��6n   5�        5�      -=   5        $ 	clmMonth7��5�   !   " . $ Product.Month6��6B   $ Month 6
 6]   $ 80��6n   5�        5�      ->   5        $ 	clmMonth8��5�   !   " . $ Product.Month7��6B   $ Month 7
 6]   $ 80��6n   5�        5�      -?   5        $ 	clmMonth9��5�   !   " . $ Product.Month8��6B   $ Month 8
 6]   $ 80��6n   5�        5�      -@   5        $ 
clmMonth10��5�   !   " . $ Product.Month9��6C   $ Month 9
 6^   $ 80��6o   5�        5�      -A   5        $ 
clmMonth11��5�   !   " . $ Product.Month10��6C   $ Month 10
 6_   $ 80��6q   5�        5�      -B   5        $ 
clmMonth12��5�   !   " . $ Product.Month11��6C   $ Month 11
 6_   $ 80��6q   5        5     -C   5        $ clmRecievedToDate��5  !   " . $ ReceivedToDate��6J   $ Received to Date
 6e   $ 80��6   5       5     -D   5        $ clmStock��5  !   " . $ Product.Stock��6A   $ Stock
 6[   $ 60��6j   5       5     -E   5        $ clmPipe��5  !   " . $ Product.Pipe��6@   $
 Pipe
 6Y   $ 60��6g   5	       5
     -F   5        $ clmStockLessPipe��5
  !   " . $ Product.StockLessPipe��6I   $ Stock Less Pipe
 6k   $ 80��6�   5       5     -G   5        $ clmLogisticsComments��5  ?   $ LogisticsComments��6!   $ Logistics Comments
 6[    5       5     , .  - . 6h   5       5     -H   5        $ clmAllocationQuantity��5  !   $ AllocationQuantity��6>   " . $ Allocation Quantity
 6m   $ 80��6�   5     .   View. 5     #   DevExpress.Xpf.Grid.TableView  5     -I   5        /   RowStyle#/ [  5      0   NewItemRowPosition$	0 Top=�6�   & 	  DevExpress.Xpf.Grid.DataViewBase1   
FocusedRow1 6N   �� *SelectedAnimationProduct+$ TwoWay=�6l  5     5     5     