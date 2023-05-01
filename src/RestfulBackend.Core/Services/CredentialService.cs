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
    public class CredentialService
    {
        #region Public-Members

        #endregion

        #region Private-Members

        private LoggingModule _Logging = null;
        private WatsonORM _ORM = null;

        #endregion

        #region Constructors-and-Factories

        public CredentialService(LoggingModule logging, WatsonORM orm)
        {
            _Logging = logging ?? throw new ArgumentNullException(nameof(logging));
            _ORM = orm ?? throw new ArgumentNullException(nameof(orm));
        }

        #endregion

        #region Public-Methods

        public List<Credential> All()
        {
            Expr expr = new Expr(
                _ORM.GetColumnName<Credential>(nameof(Credential.GUID)),
                OperatorEnum.IsNotNull,
                null);

            return _ORM.SelectMany<Credential>(expr);
        }

        public List<Credential> Search(Expr expr, int indexStart = 0, int maxResults = 1000, ResultOrder[] resultOrder = null)
        {
            if (expr == null) throw new ArgumentNullException(nameof(expr));

            return _ORM.SelectMany<Credential>(indexStart, maxResults, expr, resultOrder);
        }

        public Credential Get(string guid)
        {
            if (String.IsNullOrEmpty(guid)) throw new ArgumentNullException(nameof(guid));

            Expr expr = new Expr(
                _ORM.GetColumnName<Credential>(nameof(Credential.GUID)),
                OperatorEnum.Equals,
                guid);

            return _ORM.SelectFirst<Credential>(expr);
        }

        public Credential GetByAccessKey(string tenantGuid, string accessKey)
        {
            if (String.IsNullOrEmpty(tenantGuid)) throw new ArgumentNullException(nameof(tenantGuid));
            if (String.IsNullOrEmpty(accessKey)) throw new ArgumentNullException(nameof(accessKey));

            Expr expr = new Expr(
                new Expr(
                    _ORM.GetColumnName<Credential>(nameof(Credential.AccessKey)),
                    OperatorEnum.Equals,
                    accessKey),
                OperatorEnum.And,
                new Expr(
                    _ORM.GetColumnName<Credential>(nameof(Credential.TenantGUID)),
                    OperatorEnum.Equals,
                    tenantGuid)
                );

            return _ORM.SelectFirst<Credential>(expr);
        }

        public bool ExistsByGuid(string guid)
        {
            if (String.IsNullOrEmpty(guid)) throw new ArgumentNullException(nameof(guid));

            Expr expr = new Expr(
                _ORM.GetColumnName<Credential>(nameof(Credential.GUID)),
                OperatorEnum.Equals,
                guid);

            return _ORM.Exists<Credential>(expr);
        }

        public bool ExistsByAccessKey(string tenantGuid, string accessKey)
        {
            if (String.IsNullOrEmpty(tenantGuid)) throw new ArgumentNullException(nameof(tenantGuid));
            if (String.IsNullOrEmpty(accessKey)) throw new ArgumentNullException(nameof(accessKey));

            Expr expr = new Expr(
                new Expr(
                    _ORM.GetColumnName<Credential>(nameof(Credential.AccessKey)),
                    OperatorEnum.Equals,
                    accessKey),
                OperatorEnum.And,
                new Expr(
                    _ORM.GetColumnName<Credential>(nameof(Credential.TenantGUID)),
                    OperatorEnum.Equals,
                    tenantGuid)
                );

            return _ORM.Exists<Credential>(expr);
        }

        public Credential First(Expr expr)
        {
            if (expr == null) throw new ArgumentNullException(nameof(expr));

            return _ORM.SelectFirst<Credential>(expr);
        }

        public Credential Add(Credential cred)
        {
            if (cred == null) throw new ArgumentNullException(nameof(cred));

            return _ORM.Insert<Credential>(cred);
        }

        public Credential Update(Credential cred)
        {
            if (cred == null) throw new ArgumentNullException(nameof(cred));

            return _ORM.Update<Credential>(cred);
        }

        public void Delete(string guid)
        {
            if (String.IsNullOrEmpty(guid)) throw new ArgumentNullException(nameof(guid));

            Expr expr = new Expr(
                _ORM.GetColumnName<Credential>(nameof(Credential.GUID)),
                OperatorEnum.Equals,
                guid);

            _ORM.DeleteMany<Credential>(expr);
        }

        #endregion

        #region Private-Methods

        #endregion
    }
}
