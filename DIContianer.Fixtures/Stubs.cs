using DIContainers;
using System;
using System.Collections.Generic;
using System.Text;

namespace DIContianer.Fixtures
{
    public interface IX { }

    public interface IY { }

    public class X : IX
    {
        
    }

    public class XWithProperty : IX
    {
        [Dependency]
        public IY Y { get; set; }
    }

    public class XWithMultipleProperties : IX
    {
        [Dependency]
        public IY Y1 { get; set; }

        public IY Y2 { get; set; }
    }

    public class Y : IY { }

    public class XAlt : IX
    {
        public XAlt(X x)
        {
            Inner = x;
        }

        public X Inner { get; }
    }
}
