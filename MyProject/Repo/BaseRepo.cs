using MyProject.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;

namespace MyProject.Repo
{
    public class BaseRepo : IDisposable
    {
        private HREntities db;
        private bool disposed;

        public string Error { get; set; }

        public readonly static DateTime MinDateSQL = new DateTime(1900, 1, 1);
        public readonly static DateTime MaxDateSQL = new DateTime(2100, 1, 1);

        public HREntities Db
        {
            get { return db ?? (db = new HREntities()); }
            protected set { db = value; }
        }

        protected bool SaveDbChanges()
        {
            try
            {
                Db.SaveChanges();
            }
            catch (DbEntityValidationException eve)
            {
                var sb = new StringBuilder();
                foreach (var ve in eve.EntityValidationErrors)
                {
                    sb.AppendLine(
                        string.Format("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                            ve.Entry.Entity.GetType().Name, ve.Entry.State));

                    foreach (var verr in ve.ValidationErrors)
                    {
                        sb.AppendLine(string.Format("- Property: \"{0}\", Error: \"{1}\"", verr.PropertyName,
                            verr.ErrorMessage));
                    }
                }
                Error = sb.ToString();
                return false;
            }
            catch (Exception e)
            {
                Error = e.Message;
                return false;
            }

            return true;
        }

        #region Static Methods

        public static DateTime NormalizeSqlDateTime(DateTime test)
        {
            if (test <= MinDateSQL)
                return MinDateSQL;

            if (test >= MaxDateSQL)
                return MaxDateSQL;

            return test;
        }

        #endregion

        #region IDisposable members

        protected virtual void Dispose(bool disposing)
        {
            if(!this.disposed)
            {
                if(disposing)
                {
                    this.db.Dispose();
                }

            }

            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~BaseRepo()
        {
            Dispose(false);
        }

        #endregion

    }
}
