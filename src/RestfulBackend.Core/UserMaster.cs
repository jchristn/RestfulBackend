using System;
using System.Text.Json.Serialization;
using Watson.ORM.Core;

namespace RestfulBackend
{
    [Table("users")]
    public class UserMaster
    {
        #region Public-Members

        [Column("id", true, DataTypes.Int, false)]
        public int Id { get; set; } = 0;

        [Column("guid", false, DataTypes.Nvarchar, 64, false)]
        public string GUID { get; set; } = Guid.NewGuid().ToString();

        [Column("tenantguid", false, DataTypes.Nvarchar, 64, false)]
        public string TenantGUID { get; set; } = Guid.NewGuid().ToString();

        [Column("firstname", false, DataTypes.Nvarchar, 64, false)]
        public string FirstName { get; set; } = string.Empty;

        [Column("lastname", false, DataTypes.Nvarchar, 64, false)]
        public string LastName { get; set; } = string.Empty;

        [Column("notes", false, DataTypes.Nvarchar, 64, true)]
        public string Notes { get; set; } = string.Empty;

        [Column("email", false, DataTypes.Nvarchar, 64, false)]
        public string Email { get; set; } = string.Empty;

        [Column("passwordsha256", false, DataTypes.Nvarchar, 64, false)]
        public string PasswordSha256 { get; set; } = string.Empty;

        [Column("active", false, DataTypes.Boolean, false)]
        public bool Active { get; set; } = true;

        [Column("createdutc", false, DataTypes.DateTime, false)]
        public DateTime CreatedUtc = DateTime.UtcNow;

        #endregion

        #region Private-Members

        #endregion

        #region Constructors-and-Factories

        public UserMaster()
        {

        }

        #endregion

        #region Public-Methods

        #endregion

        #region Private-Methods

        #endregion
    }
}