using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using DBLibrary.QueryEngine.Query;

namespace DBLibrary.Session
{

    public interface Transaction : IDisposable 
    {
        void Commit();
        void RollBack();
    }

    interface TransactionDetail : Transaction
    {
        SqlTransaction SqlTransaction { get; }
    }

    class Transaction_Null : TransactionDetail
    {
        private static Transaction_Null trans = new Transaction_Null();
        public static Transaction_Null Current { get { return trans; } }

        private Transaction_Null()
        {

        }
        public void Commit()
        {
            return;
        }

        public void RollBack()
        {
            return;
        }

        public void Dispose()
        {
            return;
        }

        public SqlTransaction SqlTransaction
        {
            get { return null; }
        }
    }

    class TransactionImpl : TransactionDetail
    {
        public SqlTransaction SqlTransaction { private set; get; }

        public TransactionImpl(SqlConnection aConnection)
        {
            SqlTransaction =  aConnection.BeginTransaction();           
        }


        public void Commit()
        {
            SqlTransaction.Commit();
        }

        public void RollBack()
        {
            SqlTransaction.Rollback();
        }

        public void Dispose()
        {
            SqlTransaction.Dispose();
        }

    }
}
