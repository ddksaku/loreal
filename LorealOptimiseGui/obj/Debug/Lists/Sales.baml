   M S B A M L   `   `   `  ����a  ]DevExpress.Xpf.Grid.v11.1, Version=11.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a\Dclr-namespace:DevExpress.Xpf.Grid;assembly=DevExpress.Xpf.Grid.v11.1DevExpress.Xpf.Grid  5      a ]DevExpress.Xpf.Core.v11.1, Version=11.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49atPclr-namespace:DevExpress.Xpf.Editors.Settings;assembly=DevExpress.Xpf.Core.v11.1DevExpress.Xpf.Editors.Settings 5      L HLorealOptimiseGui, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullA%clr-namespace:LorealOptimiseGui.ListsLorealOptimiseGui.Lists 5      G(clr-namespace:LorealOptimiseGui.ControlsLorealOptimiseGui.Controls 5      M ILorealOptimiseData, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullS<clr-namespace:LorealOptimiseData;assembly=LorealOptimiseDataLorealOptimiseData 5	      Q MLorealOptimiseBusiness, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullkJclr-namespace:LorealOptimiseBusiness.Lists;assembly=LorealOptimiseBusinessLorealOptimiseBusiness.Lists 5
      bGclr-namespace:DevExpress.Xpf.Editors;assembly=DevExpress.Xpf.Core.v11.1DevExpress.Xpf.Editors 5      \Dclr-namespace:DevExpress.Xpf.Data;assembly=DevExpress.Xpf.Grid.v11.1DevExpress.Xpf.Data  5      #   LorealOptimiseGui.Lists.Sales   5      \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R	 NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml 	    5      TdxgDclr-namespace:DevExpress.Xpf.Grid;assembly=DevExpress.Xpf.Grid.v11.1 	    5      `dxePclr-namespace:DevExpress.Xpf.Editors.Settings;assembly=DevExpress.Xpf.Core.v11.1 	    5      7local%clr-namespace:LorealOptimiseGui.Lists 	    5      =Controls(clr-namespace:LorealOptimiseGui.Controls 	    5      [LorealOptimiseData<clr-namespace:LorealOptimiseData;assembly=LorealOptimiseData 	    5	      \ListsJclr-namespace:LorealOptimiseBusiness.Lists;assembly=LorealOptimiseBusiness 	    5
      [EditorsGclr-namespace:DevExpress.Xpf.Editors;assembly=DevExpress.Xpf.Core.v11.1 	    5      SdxDclr-namespace:DevExpress.Xpf.Data;assembly=DevExpress.Xpf.Grid.v11.1 	    5      .��5      �   � RowDefinitions  5   
   �� 5      $��30 �6   �� 5      $��30 �6   �� 5      $��* �6    � ColumnDefinitions 5   
   �� 5      $��* �6   6+   �� 5      $��70 �6   6,   .+�5   
   �� -   5        $��lblTitle��5      $��0��6   $��Sales��6-   $��2q�6=   $
��Left=�6H   � 5   
   $��1��6   $��2��6     5      �� 5      $��* �6    6-    5      �� 5      $
��Auto �6#   62   �� 5      $��* �6#   $��50��6-   6=   �� 5      $
��Auto �6#   62   �� 5       $��* �6#   $��50��6-   6=   �� 5!      $
��Auto �6#   62   �� 5"      $��* �6#   $��50��6-   6=   �� 5#      $
��Auto �6#   62   �� 5$      $��* �6#   $��50��6-   6=   �� 5%      $
��Auto �6#   62   �� 5&      $��* �6#   6/   �� 5'      $	��100 �6#   61   .+�5*      ��  �� Orientation$ 
Horizontal=�6   $��0��62   .+�5+      �� $��Customer��6   6-   %  DevExpress.Xpf.Editors.TextEdit  5,      -   5        $��txtCustomerFilter��5,   /   $	��140��6#   $��Center=�6H   $��2q�6c   6n   5-      �� 5.      $ 
Horizontal=�6   $��2��62   .+�5/      �� $��	Brand/Axe��6   $��Right=�6,   6J   )  #DevExpress.Xpf.Editors.ComboBoxEdit  50      -   5        $��cboBrandAxeFilter��50   3   $	��120��6'   $��Center=�6L   $��2q�6g   +  %DevExpress.Xpf.Editors.LookUpEditBase   DisplayMember$ FullNameWithSignature��6r      ValueMember$ ID��6�   6�   51      �� 52      $ 
Horizontal=�6   $��4��62   .+�53      �� $��	Signature��6   $��Right=�6,   6J     54      -   5        $��cboSignatureFilter��54   3   $	��120��6'   $��Center=�6M   $��2q�6h   $
 Name��6s   $ ID��6�   6�   55      �� 56      $ 
Horizontal=�6   $��6��62   .+�57      �� $��	Date From��6   $��Right=�6,   6J   %  DevExpress.Xpf.Editors.DateEdit  58      -   5        $��startDateFilter��58   .   $��80��6#   $��2q�6E   59      �� 5:      $ 
Horizontal=�6   $��8��62   .+�5;      �� $��Date To��6   $��Right=�6*   6H     5<      -   5        $��endDateFilter��5<   .   $��80��6#   $��2q�6C   5=      �� 5?      -   5        $��	btnFilter��5?   B   $��10��6   $��80��6&   $��Filter��61   $��Right=�6S   $��25��6�   6�   5@      %   DevExpress.Xpf.Grid.GridControl  5A   
   -   5          Name$ grdSales��5A   R   $��2��6   $��2��6'      
DataSource 6;   ��    Columns 5B      $   DevExpress.Xpf.Grid.GridColumn  5C      f
 bDevExpress.Xpf.Grid.v11.1.Core, Version=11.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a$ 
 DevExpress.Xpf.Grid.ColumnBase   	FieldName$ RetailMultiplier��6!   	   Header)  #DevExpress.Xpf.Core.ObjectConverter$	 Retail Multiplier 6>   
   ReadOnly
 .    Fixed$
 Left=�6i      Width$ 80��6v     5D      $/ )Customer.SalesArea.SalesOrganization.Code��6!      Visible .  $	 Sales Organisation Code 6g   
 . $
 Left=�6�   $	 100��6�     5E      $/ )Customer.SalesArea.SalesOrganization.Name��6!   $	 Sales Organisation Name 6W   
 . $
 Left=�6�   $	 100��6�     5F      $1 +Customer.SalesArea.DistributionChannel.Code��6!    .  $	 Distribution Channel Code 6i   
 . $
 Left=�6�   $	 100��6�     5G      $1 +Customer.SalesArea.DistributionChannel.Name��6!   $	 Distribution Channel Name 6Y   
 . $
 Left=�6�   $	 100��6�     5H      $! Customer.CustomerGroup.Code��6!    .  $	 Customer Hierarchy 3 Code 6Y   
 . $
 Left=�6�   $	 100��6�     5I      $ Customer.IDCustomerGroup��6!   $	 Customer Hierarchy 3 Name 6F   
 . $
 Left=�6y   $	 100��6�      GroupInterval$ DisplayText=�6�      EditSettings 5J      :	  4DevExpress.Xpf.Editors.Settings.ComboBoxEditSettings	  5K      -	   5         	 Name$ cboCustomerGroup��5K   J   8
  2DevExpress.Xpf.Editors.Settings.ButtonEditSettings 
  IsTextEditable .  <  6DevExpress.Xpf.Editors.Settings.LookUpEditSettingsBase   DisplayMember$ FullName��6d      ValueMember$ ID��6}   6�   5M        5N      $ Customer.AccountNumber��6!   $	 Account Number 6D   
 . $
 Left=�6l   $ 80��6y     5O      $ 
IDCustomer��6!   $	 
Store Name 68   $
 Left=�6L   $ 80��6Y    5P      	  5Q      -
   5        $ cboCustomers��5Q   J    .  $
 Name��6`   $ ID��6u   6�   5S        5T      $ BrandAxe.Signature.Code��6!   $	 Signature Code 6E    .  
 . $ 80��6}     5U      $ BrandAxe.Signature.Name��6!   $	 Signature Name 6E   
 . $ 80��6m     5V      $ BrandAxe.Code��6!   $	 Brand/Axe Code 6;   
 . $	 100��6c     5W      $ 
IDBrandAxe��6!   $	 Brand/Axe Name 68   $	 100��6P   $ DisplayText=�6\    5X      	  5Y      -   5        $ cboBrandAxes��5Y   J    .  $ FullNameWithSignature��6`   $ ID��6�   6�   5[        5\      $! Customer.SalesEmployee.Name��6!   $	 RBM Name 6I   
 . $ 80��6k     5]      $ Customer1.AccountNumber��6!   $	 Benchmark Store Code 6E   
 . $	 100��6s     5^      $ IDCustomer_Benchmark��6!   $	 Benchmark Store Name 6B   $	 100��6`    5_      	  5`      -   5        $ cboBenchmarkCustomers��5`   J    .  $
 Name��6i   $ ID��6~   6�   5b        5d      $
 Date��6!   $	 Month 62   $ 70��6A   $ Right=�6L    5e      6  0DevExpress.Xpf.Editors.Settings.DateEditSettings  5f      6  0DevExpress.Xpf.Editors.Settings.TextEditSettings   MaskType$ DateTime=�6/   6  0DevExpress.Xpf.Editors.Settings.BaseEditSettings   DisplayFormat$ MM.yyyy��6C      Mask$ MM.yyyy��6[      MaskUseAsDisplayFormat . 6�   5h        5j      $ ManualValue��6!   $	 Manual 69   $ 70��6I   $ Right=�6T    5k      6  0DevExpress.Xpf.Editors.Settings.SpinEditSettings  5l      $ n0��6/      IsFloatValue .  6B   5n        5o      $ 	EPOSValue��6!   $
	 EPOS 67   
 . $ 60��6U   $ Right=�6`    5p        5q         AllowNullInput . $ n0��6E    .  6X   5s        5t      -   5          Name$ clmCacat��5t   p   $ 
CaCatValue��6!   $	 CaCat 68   
 . $ 60��6W   $ Right=�6b    5u        5v       . $ 0,0.0��6E    .  6[   5x        5z      -   5          Name$ colSalsType��5z   !   $ 	SalesType��66   $ Right=�6L   $	 
Sales Type 6Z      UnboundType] YDevExpress.Data.v11.1, Version=11.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a8  2DevExpress.Utils.Design.UnboundColumnTypeConverter$ String 6n      UnboundExpression$� }Iif(IsNull([ManualValue]) Or ([ManualValue] == 0), Iif(IsNull([EPOSValue]) Or ([EPOSValue] == 0), 'CaCat', 'EPOS') ,'Manual')��6�   
 . $ 60��6%    5{      -   5        $ colRetailSales��5{   !   $ RetailSales��69   $ Right=�6Q   $	 Retail Sales 6_   $ Integer 6u   $� �Iif(IsNull([ManualValue]) Or ([ManualValue] == 0), Iif(IsNull([EPOSValue]) Or ([EPOSValue] == 0), [CaCatValue] * [RetailMultiplier], [EPOSValue]) ,[ManualValue])��6�   
 . $ 60��6Q   5|        5}       . $ n0��6E    .  6X   5         TotalSummary 5�      )   #DevExpress.Xpf.Grid.GridSummaryItem  5�      ) 
 #DevExpress.Xpf.Grid.SummaryItemBase   DisplayFormat$ Total: {0:0,0}��6&       	FieldName$  RetailSales��6E   !   SummaryType$	! Sum=�6]     5�      $  Capacity��6&   $ Count: {0:0,0}��6;   $! Count=�6Z   "   ShowInColumn$/" )Customer.SalesArea.SalesOrganization.Code��6n   #   GroupSummary# 5�        5�      $  RetailSales��6&   $! Count=�6>   $ Count: {0:0,0}��6R     5�      $  RetailSales��6&   $	! Sum=�6>   $ Total: {0:0,0}��6P   $   View$ 5�      #   DevExpress.Xpf.Grid.TableView  5�      -   5        & 
  DevExpress.Xpf.Grid.DataViewBase%   ShowTotalSummary% . &   NewItemRowPosition$	& Top=�5�   8   6�   5�      5�      5�      