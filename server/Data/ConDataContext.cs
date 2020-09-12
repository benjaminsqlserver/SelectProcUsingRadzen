using System.Reflection;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Configuration;

using SelectStoredProcedureRadzen.Models.ConData;

namespace SelectStoredProcedureRadzen.Data
{
  public partial class ConDataContext : Microsoft.EntityFrameworkCore.DbContext
  {
    public ConDataContext(DbContextOptions<ConDataContext> options):base(options)
    {
    }

    public ConDataContext()
    {
    }

    partial void OnModelBuilding(ModelBuilder builder);

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<SelectStoredProcedureRadzen.Models.ConData.SelectAllMarket>().HasNoKey();


        this.OnModelBuilding(builder);
    }


    public DbSet<SelectStoredProcedureRadzen.Models.ConData.Market> Markets
    {
      get;
      set;
    }

    public DbSet<SelectStoredProcedureRadzen.Models.ConData.SelectAllMarket> SelectAllMarkets
    {
      get;
      set;
    }
  }
}
