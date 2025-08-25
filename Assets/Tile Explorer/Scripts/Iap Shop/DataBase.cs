using System;
public enum ShopTypeItem
{
  Bundle,
  RemoveAds,
  Coin , 
}
public interface IDataBase
{
  string Id { get; }
  bool IsPurchase { get; }
  ShopTypeItem itemType { get; }
  
}
public interface IShopCellView
{
  Action<IShopCellView> onClick { get; set; }
  public void SetData(IDataBase data, int index);
  public void OnClick();
  public void OnPurchase(IDataBase data);
}

