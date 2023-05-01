using System;
using Watson.ORM.Core;

namespace RestfulBackend.Core
{
    [Table("tenants")]
    public class TenantMetadata
    {
        #region Public-Members

        [Column("id", true, DataTypes.Int, false)]
        public int Id { get; set; } = 0;

        [Column("guid", false, DataTypes.Nvarchar, 64, false)]
        public string GUID { get; set; } = Guid.NewGuid().ToString();

        [Column("name", false, DataTypes.Nvarchar, 64, false)]
        public string Name { get; set; } = string.Empty;

        [Column("region", false, DataTypes.Nvarchar, 32, false)]
        public string Region { get; set; } = "us-west-1";

        [Column("basedomain", false, DataTypes.Nvarchar, 64, false)]
        public string BaseDomain { get; set; } = string.Empty;

        [Column("active", false, DataTypes.Boolean, false)]
        public bool Active { get; set; } = true;

        [Column("createdutc", false, DataTypes.DateTime, false)]
        public DateTime CreatedUtc = DateTime.UtcNow;

        #endregion

        #region Private-Members

        #endregion

        #region Constructors-and-Factories

        public TenantMetadata()
        {

        }

        #endregion

        #region Public-Methods

        #endregion

        #region Private-Methods

        #endregion
    }
}