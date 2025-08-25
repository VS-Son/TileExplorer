public interface DataBase
{
  int itemId { get; }
  ShopTypeItem itemType { get; }
  
}
public interface IShopCellView
{
  public void SetData(DataBase data, int index);
}

