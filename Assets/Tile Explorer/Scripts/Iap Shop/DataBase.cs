using System;
public enum ShopTypeItem
{
  Bundle,
  RemoveAds,
  Coin , 
}
public enum PurchaseType
{
  Consumable,   
  NonConsumable 
}
public enum TypeTileTheme
{
  Fruits,
  Element,
  Candy
}
public interface IDataBase
{
  string Id { get; }
  bool IsPurchase { get; set; }
  ShopTypeItem itemType { get; }
  PurchaseType purchaseType { get; }
}

public interface ITileThemeDataBase
{
  
}
public interface IShopCellView
{
  Action<IShopCellView> onClick { get; set; }
  public void SetData(IDataBase data, int index);
  public void OnClick();
  public void OnPurchase(IDataBase data);
}

