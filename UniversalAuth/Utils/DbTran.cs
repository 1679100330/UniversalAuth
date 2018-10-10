using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Web;

/* 例子：
    using (TransactionHelper tran = new TransactionHelper(OracleHelper.GetConnection())) 
    {
        tran.BeginTransaction();
        try
        {
            tran.ExecuteNonQuery("", CommandType.Text);
            tran.ExecuteNonQuery("", CommandType.Text);
            tran.Commit();
        }
        catch (Exception ex)
        {
            tran.Rollback();
            throw ex;
        }                
    }
 */
namespace UniversalAuth.Utils
{
    public class DbTran
    {
        private OracleConnection conn;
        private OracleCommand cmd;
        private OracleTransaction tran;

        public DbTran(string connStr)
        {
            this.conn = new OracleConnection(connStr);
            conn.Open();
        }

        public void BeginTransaction()
        {
            tran = conn.BeginTransaction();
        }

        public int ExecuteNonQuery(string cmdText, CommandType type, params DbParameter[] paras)
        {
            if (tran == null)
            {
                throw new Exception("事务没有开启！");
            }
            cmd = new OracleCommand(cmdText, conn);
            if (paras != null)
            {
                cmd.Parameters.AddRange(paras);
            }
            cmd.CommandType = type;
            cmd.Transaction = tran;
            return cmd.ExecuteNonQuery();
        }

        public int ExecuteNonQuery(string cmdText, params DbParameter[] paras)
        {
            return ExecuteNonQuery(cmdText, CommandType.Text, paras);
        }

        public void Commit()
        {
            tran.Commit();
            if (tran != null)
            {
                tran.Dispose();
                tran = null;
            }
        }

        public void Rollback()
        {
            tran.Rollback();
            if (tran != null)
            {
                tran.Dispose();
                tran = null;
            }
        }

        public void Dispose()
        {
            if (tran != null)
            {
                tran.Dispose();
                tran = null;
            }
            if (cmd != null)
            {
                cmd.Dispose();
                cmd = null;
            }
            if (conn != null)
            {
                conn.Dispose();
                conn = null;
            }
        }
    }
}