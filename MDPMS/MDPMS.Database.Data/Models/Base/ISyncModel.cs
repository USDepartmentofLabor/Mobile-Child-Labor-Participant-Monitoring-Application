using System;

namespace MDPMS.Database.Data.Models.Base
{        
    public interface ISyncable<T> :
        IJsonToObjectConvertable<T>,
        IObjectToJsonConvertible<T>,
        IUpdateable<T>
    {
        int? GetExternalId();
        void SetExternalId(int? id);

        DateTime? GetLastUpdatedAt();
        void SetLastUpdatedAt(DateTime? dateTime);

        int? GetInternalId();

        void SetMdpmsdbContext(MDPMS.Database.Data.Database.MDPMSDatabaseContext context);
    }

    public interface ISyncableWithChildren<T> : ISyncable<T>
    {
        void SetParentIdsInChildObjects();
    }

    public interface ISyncableAsChild<T> :
        ISyncable<T>,
        IJsonToObjectConvertableAsChild<T>
    {
        int? GetExternalParentId();
        void SetExternalParentId(int? id);

        int? GetInternalParentId();
        void SetInternalParentId(int? id);
    }

    public interface IJsonToObjectConvertable<T>
    {
        T GetObjectFromJson(dynamic json);
    }

    public interface IJsonToObjectConvertableAsChild<T>
    {
        Tuple<int, T> GetObjectFromJsonWithParentId(dynamic json, string parentIdPropertyName);
    }

    public interface IObjectToJsonConvertible<T>
    {
        string GetJsonFromObject();
    }

    public interface IUpdateable<T>
    {
        void UpdateObject(T updateFrom);
        bool GetObjectNeedsUpate(T checkUpdateFrom);
        string GenerateUpdateJsonFromObject(T updateFrom);
    }
}
