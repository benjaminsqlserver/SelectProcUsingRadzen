using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SelectStoredProcedureRadzen.Models.ConData
{
  [Table("SelectAllMarkets", Schema = "dbo")]
  public partial class SelectAllMarket
  {
    public int MarketListID
    {
      get;
      set;
    }
    public string MarketName
    {
      get;
      set;
    }
    public string MarketLocation
    {
      get;
      set;
    }
    public decimal? MarketSizeInHectares
    {
      get;
      set;
    }
  }
}
