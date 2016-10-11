using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ninject;

namespace Loader
{
    public interface InjectFactory
    {
        IKernel Kernel { get; }
        T GetInstance<T>();
    }

    public abstract class AbstractInjectFactory
    {
        public IKernel Kernel { get; private set; }

        public AbstractInjectFactory(IKernel aKernel)
        {
            Kernel = aKernel;
        }

        
        public T GetInstance<T>()
        {
            return Kernel.Get<T>();
        }
    }

    public class BaseFactory : AbstractInjectFactory
    {
        public BaseFactory(IKernel aKernel)
            : base(aKernel)
        {
        }

        public static BaseFactory Instance { get; set; }
        
    }

}
