using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SyslogLogging;
using Watson.ORM;
using ExpressionTree;
using System.ComponentModel;
using System.Data.Common;
using DatabaseWrapper.Core;
using RestfulBackend.Core;

namespace RestfulBackend.Core.Services
{
    public class TenantService
    {
        #region Public-Members

        #endregion

        #region Private-Members

        private LoggingModule _Logging = null;
        private WatsonORM _ORM = null;

        #endregion

        #region Constructors-and-Factories

        public TenantService(LoggingModule logging, WatsonORM orm)
        {
            _Logging = logging ?? throw new ArgumentNullException(nameof(logging));
            _ORM = orm ?? throw new ArgumentNullException(nameof(orm));
        }

        #endregion

        #region Public-Methods

        public List<TenantMetadata> All()
        {
            Expr expr = new Expr(
                _ORM.GetColumnName<TenantMetadata>(nameof(TenantMetadata.GUID)),
                OperatorEnum.IsNotNull,
                null);

            return _ORM.SelectMany<TenantMetadata>(expr);
        }

        public List<TenantMetadata> Search(Expr expr, int indexStart = 0, int maxResults = 1000, ResultOrder[] resultOrder = null)
        {
            if (expr == null) throw new ArgumentNullException(nameof(expr));

            return _ORM.SelectMany<TenantMetadata>(indexStart, maxResults, expr, resultOrder);
        }

        public TenantMetadata Get(string guid)
        {
            if (String.IsNullOrEmpty(guid)) throw new ArgumentNullException(nameof(guid));

            Expr expr = new Expr(
                _ORM.GetColumnName<TenantMetadata>(nameof(TenantMetadata.GUID)),
                OperatorEnum.Equals,
                guid);

            return _ORM.SelectFirst<TenantMetadata>(expr);
        }

        public TenantMetadata GetFromUrl(string url)
        {
            if (String.IsNullOrEmpty(url)) throw new ArgumentNullException(nameof(url));

            Uri uri = new Uri(url);
            string baseDomain = uri.Host;

            return GetFromBaseDomain(baseDomain);
        }

        public TenantMetadata GetFromBaseDomain(string baseDomain)
        {
            if (String.IsNullOrEmpty(baseDomain)) throw new ArgumentNullException(nameof(baseDomain));

            Expr expr = new Expr(
                _ORM.GetColumnName<TenantMetadata>(nameof(TenantMetadata.BaseDomain)),
                OperatorEnum.Equals,
                baseDomain);

            return _ORM.SelectFirst<TenantMetadata>(expr);
        }


        public bool ExistsByGuid(string guid)
        {
            if (String.IsNullOrEmpty(guid)) throw new ArgumentNullException(nameof(guid));

            Expr expr = new Expr(
                _ORM.GetColumnName<TenantMetadata>(nameof(TenantMetadata.GUID)),
                OperatorEnum.Equals,
                guid);

            return _ORM.Exists<TenantMetadata>(expr);
        }

        public bool ExistsByName(string name)
        {
            if (String.IsNullOrEmpty(name)) throw new ArgumentNullException(nameof(name));

            Expr expr = new Expr(
                _ORM.GetColumnName<TenantMetadata>(nameof(TenantMetadata.Name)),
                OperatorEnum.Equals,
                name);

            return _ORM.Exists<TenantMetadata>(expr);
        }

        public TenantMetadata First(Expr expr)
        {
            if (expr == null) throw new ArgumentNullException(nameof(expr));

            return _ORM.SelectFirst<TenantMetadata>(expr);
        }

        public TenantMetadata Add(TenantMetadata tenant)
        {
            if (tenant == null) throw new ArgumentNullException(nameof(tenant));

            return _ORM.Insert<TenantMetadata>(tenant);
        }

        public TenantMetadata Update(TenantMetadata tenant)
        {
            if (tenant == null) throw new ArgumentNullException(nameof(tenant));

            return _ORM.Update<TenantMetadata>(tenant);
        }

        public void Delete(string guid)
        {
            if (String.IsNullOrEmpty(guid)) throw new ArgumentNullException(nameof(guid));

            Expr expr = new Expr(
                _ORM.GetColumnName<TenantMetadata>(nameof(TenantMetadata.GUID)),
                OperatorEnum.Equals,
                guid);

            _ORM.DeleteMany<TenantMetadata>(expr);
        }

        #endregion

        #region Private-Methods

        #endregion
    }
}
