using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExpressionTree;
using SyslogLogging;
using Watson.ORM;

namespace RestfulBackend.Core.Services
{
    public class UserMasterService
    {
        #region Public-Members

        #endregion

        #region Private-Members

        private LoggingModule _Logging = null;
        private WatsonORM _ORM = null;

        #endregion

        #region Constructors-and-Factories

        public UserMasterService(LoggingModule logging, WatsonORM orm)
        {
            _Logging = logging ?? throw new ArgumentNullException(nameof(logging));
            _ORM = orm ?? throw new ArgumentNullException(nameof(orm));
        }

        #endregion

        #region Public-Methods

        public List<UserMaster> All()
        {
            Expr expr = new Expr(
                _ORM.GetColumnName<UserMaster>(nameof(UserMaster.Id)),
                OperatorEnum.GreaterThan,
                0
                );

            return _ORM.SelectMany<UserMaster>(expr);
        }

        public UserMaster GetByEmail(string email)
        {
            if (String.IsNullOrEmpty(email)) throw new ArgumentNullException(nameof(email));

            Expr expr = new Expr(
                _ORM.GetColumnName<UserMaster>(nameof(UserMaster.Email)),
                OperatorEnum.Equals,
                email
                );

            return _ORM.SelectFirst<UserMaster>(expr);
        }

        public bool ExistsByEmail(string email)
        {
            if (String.IsNullOrEmpty(email)) throw new ArgumentNullException(nameof(email));

            Expr expr = new Expr(
                _ORM.GetColumnName<UserMaster>(nameof(UserMaster.Email)),
                OperatorEnum.Equals,
                email
                );

            return _ORM.Exists<UserMaster>(expr);
        }

        public UserMaster GetByGuid(string guid)
        {
            if (String.IsNullOrEmpty(guid)) throw new ArgumentNullException(nameof(guid));

            guid = guid.ToUpper();

            Expr expr = new Expr(
                _ORM.GetColumnName<UserMaster>(nameof(UserMaster.GUID)),
                OperatorEnum.Equals,
                guid
                );

            return _ORM.SelectFirst<UserMaster>(expr);
        }

        public bool ExistsByGuid(string guid)
        {
            if (String.IsNullOrEmpty(guid)) throw new ArgumentNullException(nameof(guid));

            guid = guid.ToUpper();

            Expr expr = new Expr(
                _ORM.GetColumnName<UserMaster>(nameof(UserMaster.GUID)),
                OperatorEnum.Equals,
                guid
                );

            return _ORM.Exists<UserMaster>(expr);
        }

        public List<UserMaster> Search(Expr expr, int startIndex, int maxResults)
        {
            if (expr == null) throw new ArgumentNullException(nameof(expr));
            if (startIndex < 0) throw new ArgumentOutOfRangeException(nameof(startIndex));
            if (maxResults > 1000) throw new ArgumentOutOfRangeException(nameof(maxResults));

            return _ORM.SelectMany<UserMaster>(startIndex, maxResults, expr);
        }

        public UserMaster Add(UserMaster user)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));

            user.GUID = user.GUID.ToUpper();
            user.TenantGUID = user.TenantGUID.ToUpper();

            if (ExistsByGuid(user.GUID)) throw new ArgumentException("User with GUID '" + user.GUID + "' already exists.");
            if (ExistsByEmail(user.Email)) throw new ArgumentException("User with email '" + user.Email + "' already exists.");

            return _ORM.Insert<UserMaster>(user);
        }

        public UserMaster Update(UserMaster user)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));

            user.GUID = user.GUID.ToUpper();
            user.TenantGUID = user.TenantGUID.ToUpper();

            return _ORM.Update<UserMaster>(user);
        }

        public void DeleteByGuid(string guid)
        {
            if (String.IsNullOrEmpty(guid)) throw new ArgumentNullException(nameof(guid));

            guid = guid.ToUpper();

            Expr expr = new Expr(
                _ORM.GetColumnName<UserMaster>(nameof(UserMaster.GUID)),
                OperatorEnum.Equals,
                guid
                );

            _ORM.DeleteMany<UserMaster>(expr);
        }

        public void DeleteByEmail(string email)
        {
            if (String.IsNullOrEmpty(email)) throw new ArgumentNullException(nameof(email));

            Expr expr = new Expr(
                _ORM.GetColumnName<UserMaster>(nameof(UserMaster.Email)),
                OperatorEnum.Equals,
                email
                );

            _ORM.DeleteMany<UserMaster>(expr);
        }

        public void DeleteByTenantGuid(string tenantGuid)
        {
            if (String.IsNullOrEmpty(tenantGuid)) throw new ArgumentNullException(nameof(tenantGuid));

            tenantGuid = tenantGuid.ToUpper();

            Expr expr = new Expr(
                _ORM.GetColumnName<UserMaster>(nameof(UserMaster.TenantGUID)),
                OperatorEnum.Equals,
                tenantGuid
                );

            _ORM.DeleteMany<UserMaster>(expr);
        }

        #endregion

        #region Private-Methods

        #endregion
    }
}
