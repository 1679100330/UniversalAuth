using NHibernate;
using NHibernate.Cfg;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Web;

namespace UniversalAuth.Utils
{
    public class DbUtils
    {
        private static ISessionFactory sessionFactory = null;
        private static string connStr = null;

        static DbUtils(){
            Configuration config = new Configuration().Configure();
            connStr = config.GetProperty("connection.connection_string");
            sessionFactory = config.BuildSessionFactory();
        }

        public static string GetConnStr()
        {
            return connStr;
        }

        public static ISession GetSession()
        {
            return sessionFactory.OpenSession();
        }

        public static void Save<Model>(Model model) {
            using (ISession session = sessionFactory.OpenSession())
            {                
                using (ITransaction tx = session.BeginTransaction())
                {                                        
                    session.Save(model);
                    tx.Commit();
                }
            }
        }

        public static Model Get<Model>(object id)
        {
            using (ISession session = sessionFactory.OpenSession())
            {
                return session.Get<Model>(id);
            }
        }

        public static void Delete<Model>(object id)
        {
            using (ISession session = sessionFactory.OpenSession())
            {
                using (ITransaction tx = session.BeginTransaction())
                {
                    session.Delete(id);
                    tx.Commit();
                }
            }
        }

        public static void Update<Model>(Model model)
        {
            using (ISession session = sessionFactory.OpenSession())
            {
                using (ITransaction tx = session.BeginTransaction())
                {
                    session.Update(model);
                    tx.Commit();
                }
            }
        }

        public int ExecuteNonQuery(string cmdText, CommandType type, params DbParameter[] paras)
        {
            using (OracleConnection conn = new OracleConnection(connStr))
            {
                using (OracleCommand cmd = new OracleCommand(cmdText, conn))
                {
                    if (paras != null)
                    {
                        cmd.Parameters.AddRange(paras);
                    }
                    cmd.CommandType = type;
                    conn.Open();
                    return cmd.ExecuteNonQuery();
                }
            }
        }
        /// <summary>
        /// string cmdText = "SELECT COUNT(*) from SFIS1.DAILY_REPORT_USER WHERE USERNAME=:USERNAME and PASSWORD = :PASSWORD";
        ///    int count = Convert.ToInt32(helper.ExecuteScalar(cmdText, CommandType.Text
        ///        , new OracleParameter("USERNAME", user.username)
        ///        , new OracleParameter("PASSWORD", user.password)));
        /// </summary>
        /// <param name="cmdText"></param>
        /// <param name="type"></param>
        /// <param name="paras"></param>
        /// <returns></returns>
        public object ExecuteScalar(string cmdText, CommandType type, params DbParameter[] paras)
        {
            using (OracleConnection conn = new OracleConnection(connStr))
            {
                using (OracleCommand cmd = new OracleCommand(cmdText, conn))
                {
                    if (paras != null)
                    {
                        cmd.Parameters.AddRange(paras);
                    }
                    cmd.CommandType = type;
                    conn.Open();
                    return cmd.ExecuteScalar();
                }
            }
        }

        public DbDataReader ExecuteReader(string cmdText, CommandType type, params DbParameter[] paras)
        {

            OracleConnection conn = new OracleConnection(connStr);
            using (OracleCommand cmd = new OracleCommand(cmdText, conn))
            {
                if (paras != null)
                {
                    cmd.Parameters.AddRange(paras);
                }
                cmd.CommandType = type;
                conn.Open();
                try
                {
                    return cmd.ExecuteReader(CommandBehavior.CloseConnection);
                }
                catch (Exception ex)
                {
                    conn.Dispose();
                    throw ex;
                }
            }
        }

        public DataSet GetDataSet(string cmdText, CommandType type, params DbParameter[] paras)
        {
            DataSet ds = new DataSet();
            using (OracleConnection conn = new OracleConnection(connStr))
            {
                using (OracleDataAdapter oda = new OracleDataAdapter(cmdText, conn))
                {
                    if (paras != null)
                    {
                        oda.SelectCommand.Parameters.AddRange(paras);
                    }
                    oda.SelectCommand.CommandType = type;
                    oda.Fill(ds);
                }
                return ds;
            }
        }

        public int ExecuteNonQuery(string cmdText, params DbParameter[] paras)
        {
            return ExecuteNonQuery(cmdText, CommandType.Text, paras);
        }

        public object ExecuteScalar(string cmdText, params DbParameter[] paras)
        {
            return ExecuteScalar(cmdText, CommandType.Text, paras);
        }

        public DbDataReader ExecuteReader(string cmdText, params DbParameter[] paras)
        {
            return ExecuteReader(cmdText, CommandType.Text, paras);
        }

        public DataSet GetDataSet(string cmdText, params DbParameter[] paras)
        {
            return GetDataSet(cmdText, CommandType.Text, paras);
        }

    }
}