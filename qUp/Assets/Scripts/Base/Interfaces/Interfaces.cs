using System;

namespace Base.Interfaces {
    public interface IManager { }

    public interface IData { }

    public interface IDataProvider {
        Type GetDataType();

        object GetData();
    }
}
