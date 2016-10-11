using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace DBLibrary
{
    public delegate void Iterate<T>(T anEntity, int aIndex);

    public interface ResultIterator
    {
        void IteratorOver<T>(List<T> aList, Iterate<T> aTitle, Iterate<T> aBody, Iterate<T> aFooter);
        void IteratorOver(DataTable aList, Iterate<DataRow> aTitle, Iterate<DataRow> aBody, Iterate<DataRow> aFooter); 
    }

    public class ResultIteratorImpl : ResultIterator
    {
      
        public void IteratorOver<T>(List<T> aList, Iterate<T> aTitle, Iterate<T> aBody, Iterate<T> aFooter)
        {
            int index = 0;
            foreach (T entity in aList)
            {
                if (index == 0)
                {
                    aTitle(entity, index);
                }

                else if (index == aList.Count() - 1)
                {
                    aFooter(entity, index);
                }

                else
                {
                    aBody(entity, index);
                }
                index++;
            }
        }

        public void IteratorOver(DataTable aList, Iterate<DataRow> aTitle, Iterate<DataRow> aBody, Iterate<DataRow> aFooter)
        {
            int index = 0;
            foreach (DataRow entity in aList.Rows)
            {
                if (index == 0)
                {
                    aTitle(entity, index);
                }

                else if (index == aList.Rows.Count - 1)
                {
                    aFooter(entity, index);
                }

                else
                {
                    aBody(entity, index);
                }
                index++;
            }
        }
    }
}
