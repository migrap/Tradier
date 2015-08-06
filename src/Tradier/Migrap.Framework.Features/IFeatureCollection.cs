using System;
using System.Collections.Generic;

namespace Migrap.Framework.Features {
    internal interface IFeatureCollection : IDictionary<Type, object>, IDisposable {
        int Revision { get; }
    }
}
