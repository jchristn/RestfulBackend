using System;
using Watson.ORM.Core;

namespace RestfulBackend.Core
{
    [Table("credentials")]
    public class Credential
    {
        #region Public-Members

        [Column("id", true, DataTypes.Int, false)]
        public int Id { get; set; } = 0;

        [Column("guid", false, DataTypes.Nvarchar, 64, false)]
        public string GUID { get; set; } = Guid.NewGuid().ToString();

        [Column("tenantguid", false, DataTypes.Nvarchar, 64, false)]
        public string TenantGUID { get; set; } = Guid.NewGuid().ToString();

        [Column("userguid", false, DataTypes.Nvarchar, 64, false)]
        public string UserGUID { get; set; } = Guid.NewGuid().ToString();

        [Column("accesskey", false, DataTypes.Nvarchar, 256, false)]
        public string AccessKey { get; set; } = string.Empty;

        [Column("secretkey", false, DataTypes.Nvarchar, 256, false)]
        public string SecretKey { get; set; } = string.Empty;

        [Column("active", false, DataTypes.Boolean, false)]
        public bool Active { get; set; } = true;

        [Column("createdutc", false, DataTypes.DateTime, false)]
        public DateTime CreatedUtc = DateTime.UtcNow;

        #endregion

        #region Private-Members

        #endregion

        #region Constructors-and-Factories

        public Credential()
        {

        }

        #endregion

        #region Public-Methods

        #endregion

        #region Private-Methods

        #endregion
    }
}