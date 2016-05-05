using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ANN
{
    public class ObjectsDescription<T> : IEnumerable<T>
    {
        public List<T> objectFeaturesValues { get; set; }



        public IEnumerator<T> GetEnumerator()
        {
            return objectFeaturesValues.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
