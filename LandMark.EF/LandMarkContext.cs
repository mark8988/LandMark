using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace LandMark.EF;

public partial class LandMarkContext : DbContext
{
    public LandMarkContext()
    {
    }

    public LandMarkContext(DbContextOptions<LandMarkContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Admin> Admins { get; set; }

    public virtual DbSet<AdminActionLog> AdminActionLogs { get; set; }

    public virtual DbSet<AdminGroup> AdminGroups { get; set; }

    public virtual DbSet<AdminRight> AdminRights { get; set; }

    public virtual DbSet<AdminTree> AdminTrees { get; set; }

    public virtual DbSet<CityList> CityLists { get; set; }

    public virtual DbSet<DistList> DistLists { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=59.125.142.83;Database=LandMark;User Id=sa;Password=au/6fu0 gj4jo4dk ru4;TrustServerCertificate=true;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Admin>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("Admin", tb => tb.HasComment("登入者帳號資訊"));

            entity.Property(e => e.AdminAccount)
                .HasMaxLength(50)
                .HasComment("登入帳號");
            entity.Property(e => e.AdminEmail)
                .HasMaxLength(50)
                .HasComment("使用者EMAIL");
            entity.Property(e => e.AdminGroupId)
                .HasComment("群組ID")
                .HasColumnName("AdminGroupID");
            entity.Property(e => e.AdminId)
                .ValueGeneratedOnAdd()
                .HasComment("帳號ID")
                .HasColumnName("AdminID");
            entity.Property(e => e.AdminLineId)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasComment("LineID")
                .HasColumnName("AdminLineID");
            entity.Property(e => e.AdminName)
                .HasMaxLength(255)
                .HasComment("使用者姓名");
            entity.Property(e => e.AdminPassWord)
                .HasMaxLength(50)
                .HasComment("登入密碼");
            entity.Property(e => e.AdminPhone).HasComment("使用者描述(備註)");
            entity.Property(e => e.CreateTime)
                .HasDefaultValueSql("(getdate())")
                .HasComment("此筆記錄新增時間")
                .HasColumnType("datetime");
            entity.Property(e => e.IsEnable)
                .HasDefaultValue(true)
                .HasComment("使否啟用");
            entity.Property(e => e.UpdateTime)
                .HasDefaultValueSql("(getdate())")
                .HasComment("此筆記錄修改時間")
                .HasColumnType("datetime");
            entity.Property(e => e.UpdateUser).HasComment("修改人");
        });

        modelBuilder.Entity<AdminActionLog>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("AdminActionLog", tb => tb.HasComment("使用者操作紀錄(後台)"));

            entity.Property(e => e.Action)
                .HasMaxLength(100)
                .HasComment("行為");
            entity.Property(e => e.ActionTime)
                .HasComment("行為時間")
                .HasColumnType("datetime");
            entity.Property(e => e.AdminId)
                .HasComment("使用者Id")
                .HasColumnName("AdminID");
            entity.Property(e => e.Params).HasComment("參數");
            entity.Property(e => e.SerialNo)
                .ValueGeneratedOnAdd()
                .HasComment("流水號");
        });

        modelBuilder.Entity<AdminGroup>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("AdminGroup", tb => tb.HasComment("使用者群組管理"));

            entity.Property(e => e.AdminGroupId)
                .ValueGeneratedOnAdd()
                .HasComment("資料流水號")
                .HasColumnName("AdminGroupID");
            entity.Property(e => e.CreateTime)
                .HasComment("此筆記錄新增時間")
                .HasColumnType("datetime");
            entity.Property(e => e.GroupName)
                .HasMaxLength(50)
                .HasComment("群組名稱");
            entity.Property(e => e.IsHeadquarter).HasComment("是否為總部(0否/1是)");
            entity.Property(e => e.IsStoreManage).HasComment("是否為門店(0否/1是)");
            entity.Property(e => e.IsSupervisor).HasComment("是否為督導(0否/1是)");
            entity.Property(e => e.LoginIp)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasComment("登入IP")
                .HasColumnName("LoginIP");
            entity.Property(e => e.LoginTimeEnd).HasComment("允許登入時間(迄)");
            entity.Property(e => e.LoginTimeStart).HasComment("允許登入時間(起)");
            entity.Property(e => e.OrganizeManageId)
                .HasComment("組織管理ID")
                .HasColumnName("OrganizeManageID");
            entity.Property(e => e.UpdateTime)
                .HasComment("此筆記錄修改時間")
                .HasColumnType("datetime");
            entity.Property(e => e.UpdateUser).HasComment("後台登入者帳號");
        });

        modelBuilder.Entity<AdminRight>(entity =>
        {
            entity.HasKey(e => e.AdminRightsId).HasName("PK__AdminRig__CE808C5E0247E2F9");

            entity.ToTable(tb => tb.HasComment("使用者權限管理"));

            entity.Property(e => e.AdminRightsId)
                .HasComment("流水號")
                .HasColumnName("Admin_Rights_ID");
            entity.Property(e => e.AllowDelete)
                .HasComment("是否可執行刪除")
                .HasColumnName("Allow_Delete");
            entity.Property(e => e.AllowInsert)
                .HasComment("是否可執行新增")
                .HasColumnName("Allow_Insert");
            entity.Property(e => e.AllowShowAllData)
                .HasComment("是否顯示查詢的全部資訊")
                .HasColumnName("Allow_ShowAllData");
            entity.Property(e => e.AllowUpdate)
                .HasComment("是否可執行修改")
                .HasColumnName("Allow_Update");
            entity.Property(e => e.GroupId)
                .HasComment("群組ID")
                .HasColumnName("Group_ID");
            entity.Property(e => e.ReceiveDido).HasComment("接收通知");
            entity.Property(e => e.SendDido).HasComment("寄送通知");
            entity.Property(e => e.ShowInMenu).HasComment("是否顯示左側導航列");
            entity.Property(e => e.TreeId)
                .HasComment("TreeID")
                .HasColumnName("Tree_Id");
            entity.Property(e => e.TreeOrder)
                .HasComment("顯示排序")
                .HasColumnName("Tree_order");
        });

        modelBuilder.Entity<AdminTree>(entity =>
        {
            entity.HasKey(e => e.TreeId).HasName("PK__AdminTre__538C16676B91C0FF");

            entity.ToTable("AdminTree", tb => tb.HasComment("網頁功能連結管理"));

            entity.Property(e => e.TreeId)
                .HasComment("功能(連結)Id")
                .HasColumnName("Tree_Id");
            entity.Property(e => e.DidoSenderId).HasColumnName("DidoSender_ID");
            entity.Property(e => e.Icon)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.TreeParam)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("Tree_Param");
            entity.Property(e => e.TreeRoot)
                .HasComment("功能(連結)Root")
                .HasColumnName("Tree_Root");
            entity.Property(e => e.TreeTitle)
                .HasMaxLength(50)
                .HasComment("功能(連結)名稱")
                .HasColumnName("Tree_Title");
            entity.Property(e => e.TreeUrl)
                .HasMaxLength(50)
                .HasComment("功能(連結)Url")
                .HasColumnName("Tree_Url");
        });

        modelBuilder.Entity<CityList>(entity =>
        {
            entity.HasKey(e => e.CityNo).HasName("PK__City_Lis__DE9D0AA306ED4A02");

            entity.ToTable("CityList", tb => tb.HasComment("台灣縣市"));

            entity.Property(e => e.CityNo)
                .HasComment("城市代碼")
                .HasColumnName("City_No");
            entity.Property(e => e.CityEngName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasComment("城市名稱(英文)")
                .HasColumnName("City_EngName");
            entity.Property(e => e.CityName)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasComment("城市名稱")
                .HasColumnName("City_Name");
            entity.Property(e => e.InsertTime)
                .HasDefaultValueSql("(getdate())")
                .HasComment("此筆記錄新增時間")
                .HasColumnType("datetime");
            entity.Property(e => e.Remark).HasComment("顯示在前台0/不顯示1");
            entity.Property(e => e.RetireTime)
                .HasComment("此筆記錄汰舊時間")
                .HasColumnType("datetime");
            entity.Property(e => e.UpdateTime)
                .HasDefaultValueSql("(getdate())")
                .HasComment("此筆記錄修改時間")
                .HasColumnType("datetime");
            entity.Property(e => e.UpdateUser)
                .HasComment("後台登入人員帳號")
                .HasColumnName("Update_User");
        });

        modelBuilder.Entity<DistList>(entity =>
        {
            entity.HasKey(e => e.DistNo).HasName("PK__Dist_Lis__559D19B287F1E19F");

            entity.ToTable("DistList", tb => tb.HasComment("鄉鎮市區"));

            entity.Property(e => e.DistNo)
                .HasComment("城市區域代碼")
                .HasColumnName("Dist_No");
            entity.Property(e => e.CityNo)
                .HasComment("城市代碼")
                .HasColumnName("City_No");
            entity.Property(e => e.DistEngName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasComment("城市區域名稱(英文)")
                .HasColumnName("Dist_EngName");
            entity.Property(e => e.DistName)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasComment("城市區域名稱")
                .HasColumnName("Dist_Name");
            entity.Property(e => e.InsertTime)
                .HasDefaultValueSql("(getdate())")
                .HasComment("此筆記錄新增時間")
                .HasColumnType("datetime");
            entity.Property(e => e.PostalCode)
                .HasComment("郵遞區號")
                .HasColumnName("Postal_Code");
            entity.Property(e => e.Remark).HasComment("修改原因");
            entity.Property(e => e.RetireTime)
                .HasComment("此筆記錄汰舊時間")
                .HasColumnType("datetime");
            entity.Property(e => e.UpdateTime)
                .HasDefaultValueSql("(getdate())")
                .HasComment("此筆記錄修改時間")
                .HasColumnType("datetime");
            entity.Property(e => e.UpdateUser)
                .HasComment("後台登入人員帳號")
                .HasColumnName("Update_User");

            entity.HasOne(d => d.CityNoNavigation).WithMany(p => p.DistLists)
                .HasForeignKey(d => d.CityNo)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Dist_List__City___4E88ABD4");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
