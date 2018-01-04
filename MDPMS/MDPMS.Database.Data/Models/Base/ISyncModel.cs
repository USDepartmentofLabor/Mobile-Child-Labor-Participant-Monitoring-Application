using System;

namespace MDPMS.Database.Data.Models.Base
{        
    interface ISyncable<T> :
        IJsonToObjectConvertable<T>,
        IObjectToJsonConvertible<T>,
        IUpdateable<T>
    {
    }

    interface ISyncableAsChild<T> :
        ISyncable<T>,
        IJsonToObjectConvertableAsChild<T>
    {        
    }

    interface IJsonToObjectConvertable<T>
    {
        T GetObjectFromJson(dynamic json);
    }

    interface IJsonToObjectConvertableAsChild<T>
    {
        Tuple<int, T> GetObjectFromJsonWithParentId(dynamic json, string parentIdPropertyName);
    }

    interface IObjectToJsonConvertible<T>
    {
        string GetJsonFromObject();
    }

    interface IUpdateable<T>
    {
        void UpdateObject(T updateFrom);
        bool GetObjectNeedsUpate(T checkUpdateFrom);
        string GenerateUpdateJsonFromObject(T updateFrom);
    }
}
